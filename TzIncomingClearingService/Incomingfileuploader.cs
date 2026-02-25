using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using System.Timers;
using System.IO;
using System.Data.Common;
using System.Configuration;
using BrClearing.Common.TZ;
using System.Windows.Forms;
using BrClearing.Common;
using Serilog.Formatting.Json;

namespace TzIncomingClearingService
{
    public partial class Incomingfileuploader : ServiceBase
    {
        private System.Threading.Timer timer;
        SemaphoreSlim uploadSemaphore = new SemaphoreSlim(5, 5);
        private readonly string emailRecipient = "brenda.nyaswa@craftsilicon.com";
        private readonly string emailSubjectPrefix = "Service Error: ";
        private readonly string maxRetries = ConfigurationManager.AppSettings["maxRetries"];
        private readonly static string ArchiveFilePath = ConfigurationManager.AppSettings["Archive"];
        private protected string configFilePath = ConfigurationManager.AppSettings["configFilePath"];
        private readonly static string IncomingFilePath = ConfigurationManager.AppSettings["IncomingFiles"];
        private readonly static string BRCountryCode = ConfigurationManager.AppSettings["CountryCode"];
        private readonly static string errorLog = ConfigurationManager.AppSettings["ClearingErrorLogFilePath"];
        private readonly static string HQBranchID = ConfigurationManager.AppSettings["HeadOfficeBranchID"];

        public Incomingfileuploader()
        {
            InitializeComponent();
            Modscan.OurBranchID = HQBranchID;
            try
            {
                Log.Logger = new LoggerConfiguration()
               .WriteTo.File(new JsonFormatter(), Path.Combine(errorLog, "Information/log.txt"),
               restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
               rollingInterval: RollingInterval.Day)
               .WriteTo.File(new JsonFormatter(), Path.Combine(errorLog, "Errors/log.txt"),
               restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
               rollingInterval: RollingInterval.Day)
               .CreateLogger();
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configFilePath
                };
                OnStart(null);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing service");
            }
        }

        protected override void OnStart(string[] args)
        {
#pragma warning disable
            timer = new System.Threading.Timer(state => ExecutePeriodicTaskAsync(DoWorkAsync), null, TimeSpan.Zero, TimeSpan.FromMinutes(0.5));
#pragma warning restore
        }

        protected override void OnStop()
        {
            timer.Dispose();
        }

        private async Task ExecutePeriodicTaskAsync(Func<Task> task)
        {
            try
            {
                await task.Invoke();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error executing periodic task");
            }
        }

        private async Task DoWorkAsync()
        {
            await ExecuteSynchronizedAsync(async () =>
            {
                try
                {
                    switch (BRCountryCode.ToString().ToUpper())
                    {
                        case "TZ":
                            await PerformTzAsyncTask();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in DoWorkAsync");
                }
            });
        }

        private async Task ExecuteSynchronizedAsync(Func<Task> asyncAction)
        {
            if (await uploadSemaphore.WaitAsync(0))
            {
                try
                {
                    await asyncAction.Invoke();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in ExecuteSynchronizedAsync");
                }
                finally
                {
                    uploadSemaphore.Release();
                }
            }
        }

        private async Task PerformTzAsyncTask()
        {
            // Log current file state for debugging
            LogCurrentFileState();

            // Ensure directories exist
            if (!Directory.Exists(Path.Combine(IncomingFilePath, "Temp")))
            {
                Directory.CreateDirectory(Path.Combine(IncomingFilePath, "Temp"));
            }

            string workingFolder = Path.Combine(IncomingFilePath, "File");
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder);
            }

            // Check if there are files in Temp folder
            string tempFolder = Path.Combine(IncomingFilePath, "Temp");
            string[] tempFiles = Directory.GetFiles(tempFolder);

            if (tempFiles.Length == 0)
            {
                Log.Information("No files found in Temp folder.");
                return;
            }

            Log.Information($"Found {tempFiles.Length} files in Temp folder. Starting move process...");
            // Create backup folder with timestamp
            string backupRoot = Path.Combine(IncomingFilePath, "Backup");
            string backupFolder = Path.Combine(
                backupRoot,
                DateTime.Now.ToString("yyyyMMdd_HHmmss")
            );

            Directory.CreateDirectory(backupFolder);


            Log.Information($"Backing up {tempFiles.Length} files from Temp to {backupFolder}");

            try
            {
                foreach (string file in tempFiles)
                {
                    string destFile = Path.Combine(backupFolder, Path.GetFileName(file));
                    File.Copy(file, destFile, overwrite: true);
                    Log.Information($"Backed up file: {Path.GetFileName(file)}");
                }

                Log.Information("Backup completed successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Backup failed. Aborting processing to avoid data loss.");
                return; 
            }


            try
            {
                // Move files from Temp to File folder
                await MoveFiles(tempFolder, workingFolder);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error moving files from Temp to File folder");
                return; // Don't proceed if move failed
            }

            // Check if files were successfully moved
            string[] filesInWorkingFolder = Directory.GetFiles(workingFolder);
            if (filesInWorkingFolder.Length == 0)
            {
                Log.Information("No files found in working folder after move.");
                return;
            }

            Log.Information($"Found {filesInWorkingFolder.Length} files in working folder for processing.");

            // Prepare for file processing
            List<Task> uploadTasks = new List<Task>();
            ProgressBar prgAll = new ProgressBar();
            Label lblmessage = new Label();
            string TokenName = "";
            string TokenPassword = "";
            Modscan.CountryCode = BRCountryCode;
            Modscan.OurBankID = "51";
            Modscan.cFromDate = DateTime.Now.Date.ToString();
            Modscan.cToDate = DateTime.Now.Date.ToString();

            DirectoryInfo d = new DirectoryInfo(workingFolder);
            FileInfo[] FLItems = d.GetFiles();

            TimeSpan baseDelay = TimeSpan.FromSeconds(1);
            TimeSpan maxDelay = TimeSpan.FromSeconds(30);
            List<string> Flist = new List<string>();
            string FName = "";
            string FExt = "";

            foreach (FileInfo file in FLItems)
            {
                // Verify file still exists before processing
                if (!File.Exists(file.FullName))
                {
                    Log.Warning($"File {file.Name} no longer exists. Skipping...");
                    continue;
                }

                FExt = "";
                string sExt = Path.GetExtension(file.Name).ToUpper();

                if (sExt.Contains("ZIP"))
                {
                    FExt = ".ZIP";
                }
                else if (sExt.Contains("T"))
                {
                    FExt = ".T";
                }
                else if (sExt.Contains("U"))
                {
                    FExt = ".U";
                }
                else if (sExt.Contains("I"))
                {
                    FExt = ".I";
                }
                else if (sExt.Contains("Q"))
                {
                    FExt = ".Q";
                }
                else if (sExt.Contains("S"))
                {
                    FExt = ".S";
                }
                else if (sExt.Contains("Y"))
                {
                    FExt = ".Y";
                }
                else if (sExt.Contains("W"))
                {
                    FExt = ".W";
                }
                else if (sExt.Contains("D"))
                {
                    FExt = ".D";
                }
                else
                {
                    FExt = sExt;
                }

                switch (FExt.ToUpper())
                {
                    case ".ZIP":
                    case ".T":
                    case ".Y":
                    case ".U":
                    case ".W":
                    case ".D":
                    case ".Q":
                    case ".V":
                    case ".S":
                    case ".N":
                    case ".TXT":
                    case ".RC":
                    case ".R":
                        FName = file.FullName;
                        Flist.Add(FName);
                        Log.Information($"Added file to processing list: {file.Name}");
                        break;
                    default:
                        Log.Information($"Skipping file with unsupported extension: {file.Name}");
                        break;
                }
            }

            if (Flist.Count == 0)
            {
                Log.Information("No valid files found for processing.");
                return;
            }

            // Configure upload settings
            Modscan.strJavaExeInstallation = ConfigurationManager.AppSettings["strJavaExeInstallation"].Trim();
            Modscan.strDSkeyFile = ConfigurationManager.AppSettings["strDSkeyFile"].Trim();
            Modscan.strBatchPath = ConfigurationManager.AppSettings["strBatchPath"].Trim();
            Modscan.keyPass = ConfigurationManager.AppSettings["keypass"].Trim();

            bool success = false;
            try
            {
                Log.Information($"Uploading {Flist.Count} files...");
                success = await ExecuteWithExponentialBackoff(() =>
                    Inwards.ImportTZ(TokenName, TokenPassword, Flist, ref lblmessage, ref prgAll, ref prgAll, FileType.Cheques, TokenPassword, TokenName, TokenPassword),
                    Convert.ToInt32(maxRetries), baseDelay, maxDelay);

                Log.Information($"Upload completed. Success: {success}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during file upload");
            }

            if (!success)
            {
                Log.Error("Failed uploading files");
                // Optional: Move failed files to error folder
                await MoveFailedFiles(Flist, workingFolder);
            }
            else
            {
                try
                {
                    await DeleteFilesInFolder(workingFolder, Flist);
                    Log.Information($"Successfully deleted {Flist.Count} processed files");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error deleting processed files");
                }
            }
        }

        private async Task MoveFiles(string sourceFolder, string destinationFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Log.Warning($"Source folder does not exist: {sourceFolder}");
                return;
            }

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            string[] files = Directory.GetFiles(sourceFolder);

            Log.Information($"Found {files.Length} files to move from {sourceFolder}");

            foreach (var filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                string destinationPath = Path.Combine(destinationFolder, fileName);

                try
                {
                    // Check if source file exists
                    if (!File.Exists(filePath))
                    {
                        Log.Warning($"File no longer exists: {filePath}. Skipping...");
                        continue;
                    }

                    // Check file size to ensure it's not empty
                    FileInfo sourceInfo = new FileInfo(filePath);
                    if (sourceInfo.Length == 0)
                    {
                        Log.Warning($"File is empty: {fileName}. Skipping...");
                        continue;
                    }

                    // Check if destination file already exists
                    if (File.Exists(destinationPath))
                    {
                        // Generate unique filename to avoid conflicts
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string uniqueName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{Path.GetExtension(fileName)}";
                        destinationPath = Path.Combine(destinationFolder, uniqueName);
                        Log.Information($"Destination file exists. Using unique name: {uniqueName}");
                    }

                    // Use File.Copy then File.Delete for better reliability
                    File.Copy(filePath, destinationPath, overwrite: false);

                    // Verify copy succeeded before deleting source
                    if (File.Exists(destinationPath))
                    {
                        FileInfo destInfo = new FileInfo(destinationPath);
                        if (destInfo.Length == sourceInfo.Length)
                        {
                            File.Delete(filePath);
                            Log.Information($"Successfully moved {fileName} to {destinationFolder}");
                        }
                        else
                        {
                            Log.Error($"Copy verification failed for {fileName}. File sizes don't match.");
                            if (File.Exists(destinationPath))
                                File.Delete(destinationPath);
                        }
                    }
                    else
                    {
                        Log.Error($"Copy failed for {fileName}. Destination file not created.");
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Log.Warning(ex, $"File {fileName} was not found during move operation");
                    continue;
                }
                catch (IOException ioEx) when (ioEx.HResult == -2147024894 || ioEx is FileNotFoundException)
                {
                    Log.Warning(ioEx, $"File {fileName} not accessible. May be in use or already moved.");
                    continue;
                }
                catch (UnauthorizedAccessException authEx)
                {
                    Log.Error(authEx, $"Permission denied for file {fileName}");
                    continue;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Error moving file {fileName}: {ex.Message}");
                }
            }
        }

        private async Task DeleteFilesInFolder(string folderPath, List<string> filesToDelete)
        {
            if (!Directory.Exists(folderPath))
            {
                Log.Warning($"Folder does not exist: {folderPath}");
                return;
            }

            int deletedCount = 0;
            int failedCount = 0;

            foreach (string fullPath in filesToDelete)
            {
                try
                {
                    // Get just the filename from the full path
                    string fileName = Path.GetFileName(fullPath);
                    string filePath = Path.Combine(folderPath, fileName);

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        deletedCount++;
                        Log.Information($"Deleted file: {fileName}");
                    }
                    else
                    {
                        // Try to delete using the full path directly
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                            deletedCount++;
                            Log.Information($"Deleted file using full path: {fullPath}");
                        }
                        else
                        {
                            Log.Warning($"File not found for deletion: {fileName}");
                            failedCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Error deleting file '{fullPath}'");
                    failedCount++;
                }
            }

            Log.Information($"Deletion completed. Success: {deletedCount}, Failed: {failedCount}");
        }

        private async Task<bool> ExecuteWithExponentialBackoff(Action operation, int maxRetries, TimeSpan baseDelay, TimeSpan maxDelay)
        {
            int retryCount = 0;
            while (retryCount < maxRetries)
            {
                try
                {
                    operation();
                    return true;
                }
                catch (DbException dbException) when (IsDeadlockException(dbException))
                {
                    Log.Error(dbException, "Deadlock detected. Retrying...");
                    double exponentialFactor = Math.Pow(2, retryCount);
                    TimeSpan delay = TimeSpan.FromTicks((long)(baseDelay.Ticks * exponentialFactor));
                    delay = TimeSpan.FromTicks(Math.Min(delay.Ticks, maxDelay.Ticks));
                    Log.Information($"Retrying in {delay.TotalSeconds} seconds...");
                    await Task.Delay(delay);
                    retryCount++;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Error in operation. Retry {retryCount + 1} of {maxRetries}");
                    double exponentialFactor = Math.Pow(2, retryCount);
                    TimeSpan delay = TimeSpan.FromTicks((long)(baseDelay.Ticks * exponentialFactor));
                    delay = TimeSpan.FromTicks(Math.Min(delay.Ticks, maxDelay.Ticks));
                    Log.Information($"Retrying in {delay.TotalSeconds} seconds...");
                    await Task.Delay(delay);
                    retryCount++;
                }
            }

            Log.Error($"Operation failed after {maxRetries} retries");
            return false;
        }

        private bool IsDeadlockException(DbException exception)
        {
            return exception.Message.Contains("deadlock");
        }

        private async Task LogErrorAsync(string message, Exception exception = null)
        {
            try
            {
                string errorMessage = message;
                if (exception != null)
                {
                    errorMessage = $"{message}: {exception.Message}\nStackTrace: {exception.StackTrace}";
                }

                Log.Error(errorMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to log the error");
            }
        }

        private async Task SendEmailNotification(string message)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("brclearingerrnotifier@gmail.com", "Craft@1234!");
                    client.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("brclearingerrnotifier@gmail.com");
                    mailMessage.To.Add(emailRecipient);
                    mailMessage.Subject = $"{emailSubjectPrefix}{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    mailMessage.Body = message;

                    await client.SendMailAsync(mailMessage);
                    Log.Information("Email notification sent successfully");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error sending email notification");
            }
        }

        private async Task MoveFailedFiles(List<string> failedFiles, string sourceFolder)
        {
            string errorFolder = Path.Combine(IncomingFilePath, "Errors");
            if (!Directory.Exists(errorFolder))
            {
                Directory.CreateDirectory(errorFolder);
            }

            foreach (string filePath in failedFiles)
            {
                try
                {
                    string fileName = Path.GetFileName(filePath);
                    string errorFilePath = Path.Combine(errorFolder, fileName);

                    if (File.Exists(filePath))
                    {
                        // Move to error folder with timestamp
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string uniqueName = $"{Path.GetFileNameWithoutExtension(fileName)}_FAILED_{timestamp}{Path.GetExtension(fileName)}";
                        errorFilePath = Path.Combine(errorFolder, uniqueName);

                        File.Move(filePath, errorFilePath);
                        Log.Information($"Moved failed file to error folder: {uniqueName}");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Error moving failed file: {filePath}");
                }
            }
        }

        private void LogCurrentFileState()
        {
            try
            {
                string tempDir = Path.Combine(IncomingFilePath, "Temp");
                string fileDir = Path.Combine(IncomingFilePath, "File");
                string archiveDir = ArchiveFilePath;

                Log.Information("=== Current File State ===");
                Log.Information($"Temp directory: {tempDir} (Exists: {Directory.Exists(tempDir)})");
                Log.Information($"File directory: {fileDir} (Exists: {Directory.Exists(fileDir)})");
                Log.Information($"Archive directory: {archiveDir} (Exists: {Directory.Exists(archiveDir)})");

                if (Directory.Exists(tempDir))
                {
                    var tempFiles = Directory.GetFiles(tempDir);
                    Log.Information($"Files in Temp folder: {tempFiles.Length}");
                    foreach (var file in tempFiles)
                    {
                        try
                        {
                            var info = new FileInfo(file);
                            Log.Information($"  - {Path.GetFileName(file)} (Size: {info.Length}, Modified: {info.LastWriteTime:yyyy-MM-dd HH:mm:ss})");
                        }
                        catch
                        {
                            Log.Information($"  - {Path.GetFileName(file)} (Unable to get file info)");
                        }
                    }
                }

                if (Directory.Exists(fileDir))
                {
                    var fileDirFiles = Directory.GetFiles(fileDir);
                    Log.Information($"Files in File folder: {fileDirFiles.Length}");
                    foreach (var file in fileDirFiles.Take(5)) // Limit to first 5
                    {
                        try
                        {
                            var info = new FileInfo(file);
                            Log.Information($"  - {Path.GetFileName(file)} (Size: {info.Length}, Modified: {info.LastWriteTime:yyyy-MM-dd HH:mm:ss})");
                        }
                        catch
                        {
                            Log.Information($"  - {Path.GetFileName(file)} (Unable to get file info)");
                        }
                    }
                    if (fileDirFiles.Length > 5)
                    {
                        Log.Information($"  ... and {fileDirFiles.Length - 5} more files");
                    }
                }

                Log.Information("=== End File State ===");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error logging file state");
            }
        }

        // Overloaded method for string array (kept for compatibility)
        private async Task DeleteFilesInFolder(string folderPath, string[] filesToDelete)
        {
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            foreach (string fileName in filesToDelete)
            {
                try
                {
                    string filePath = Path.Combine(folderPath, fileName);

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Error deleting file '{fileName}': {ex.Message}");
                }
            }
        }
    }
}