using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.Windows.Forms;

namespace BRCATDS
{
    //BRCATDS = BRClrAlladinTokenDigiSign
    public interface BRCI
    {
        //BRCI = IBRClearingEnc
        //BRCDS = BRClearingDSign
        string BRCDS(string v, string w, string x, Int16 y, string t, string u, out string z, string c = "UG", string b = "", string j = "", string e = "", bool k = false);
    }
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class BRCS : BRCI
    {

        //BRCS = BRClrDigiSign
        public string BRCDS(string v, string w, string x, Int16 y, string t, string u, out string z, string c = "", string b = "", string j = "", string e = "", bool k = false)
        {
            //v = SourceFileName, w = DestinationFileName, x = PriKey, y = action,t = TokenUsername,u = TokenPassword, out z = Message

            return BREnc(v, w, x, y, t, u, out z, c, b, j, e, k);
        }
        static private string _cUser = String.Empty;
        static private string _HashString = string.Empty;
        private string BREnc(string v, string w, string x, Int16 y, string t, string u, out string z, string c, string b, string j, string e, bool k)
        {
            z = string.Empty;
            try
            {
                if (BRRSACryptography.CryptographyHelper.Decrypt(x) != "h / KNJ1uE5CmUcQb4xbsfoW9ZPzk =")
                {
                    z = "Done";
                }
                else
                {
                    z = "";
                    if (y == 71)
                    {
                        //MessageBox.Show("Step 5.3.1");
                        z = EncodeFiles(BRRSACryptography.CryptographyHelper.Decrypt(v), BRRSACryptography.CryptographyHelper.Decrypt(w), t, u, c, b, j, e, k);
                        //MessageBox.Show("Step 5.3.2");
                    }
                    else if (y == 32)
                    {
                        DecodeFiles(BRRSACryptography.CryptographyHelper.Decrypt(v), BRRSACryptography.CryptographyHelper.Decrypt(w), t, c, k);
                        z = "Done";
                    }
                    else
                    {
                        z = "Done";
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return z;
        }

        private void LogError(Exception ex)
        {
            string ErrorLogPath = "C:\\BRExceptionLog\\ClearingErrorLogs\\";
            int errline = GetLineNumber(ex);
            StackTrace stackTrace = new StackTrace();
            string MethodName = stackTrace.GetFrame(1).GetMethod().Name;
            if (Convert.ToString(ErrorLogPath) != "")
            {
                if (!Directory.Exists(ErrorLogPath))
                {
                    Directory.CreateDirectory(ErrorLogPath);
                }
                string AppendErrorMessage =
                Environment.NewLine
              + Environment.NewLine + "=============================================================================================="
                    //+ Environment.NewLine + "User Logged in :" + User
              + Environment.NewLine + "Error Message :" + ex.Message
                    //+ Environment.NewLine + "Stack Trace : " + ex.StackTrace
              + Environment.NewLine + "Target Site : " + ex.TargetSite
              + Environment.NewLine + "Line Number : " + errline
              + Environment.NewLine + "Error Method : " + MethodName
              + Environment.NewLine + "Date : " + DateTime.Now
              + Environment.NewLine + "===============================================================================================";
                System.IO.File.AppendAllText(ErrorLogPath + "BRDigiSign" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".Errlog", AppendErrorMessage);
            }
        }
        private int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            try
            {
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    if (int.TryParse(lineNumberText, out lineNumber))
                    {
                    }
                }
            }
            catch (Exception x)
            {
                LogError(x);
            }
            return lineNumber;
        }
        private static string EncodeFiles(string FilenameToSign, string SignedFile, string UserTokenName, string TokenPass, string CountryID, string strBatchPath = "", string jksFilePath = "", string strJavaExeInstallation = "", bool k = false)
        {
            string msgCallBack = "";
            string fileEncrypted = "";
            try
            {
                string z = "";
                //MessageBox.Show("Step 5.3.1.1");
                var signature = new BRCSTDS(UserTokenName, true, out z, CountryID, k);
                //MessageBox.Show("Step 5.3.1.2");
                if (CountryID != "TZ")
                {
                    try
                    {
                        UserTokenName = BRRSACryptography.CryptographyHelper.Decrypt(UserTokenName);
                    }
                    catch
                    {
                        //MessageBox.Show("Chapa - Step 5.3.1.3");
                    }
                }


                if (signature._cert == null && k == true)
                {
                    z = BRRSACryptography.CryptographyHelper.Decrypt(z);
                    if (z.Substring(0, 3) == "INV")
                    {
                        msgCallBack = "Invalid token username: " + UserTokenName + ". The connected token is for: " + z.ToString().Substring(3, z.Length - 3);
                    }
                    else
                    {
                        msgCallBack = "Missing token or invalid token name and password for:" + UserTokenName;
                    }
                    return msgCallBack;
                }


                try
                {
                    //MessageBox.Show("FilenameToSign " + FilenameToSign);

                    string[] fileEntries = Directory.GetFiles(FilenameToSign);
                    if (fileEntries.Length == 0)
                    {
                        msgCallBack = "There are no files to generate in this path: " + FilenameToSign;
                    }
                    foreach (string fileName in fileEntries)
                    {

                        if (CountryID == "TZ" && k == false)
                        {
                            //MessageBox.Show("Step 5.3.1.7 - BRQVL");
                            signature.BRQVL(fileName, SignedFile, strBatchPath, TokenPass.Trim(), jksFilePath, strJavaExeInstallation);
                        }
                        if (CountryID == "TZ" && k == true)
                        {
                            fileEncrypted = SignedFile + "\\" + Path.GetFileName(fileName);
                            byte[] b = signature.BRQVL(File.ReadAllBytes(fileName), TokenPass.Trim(), CountryID);
                            File.WriteAllBytes(fileEncrypted, b);
                        }
                        else if (CountryID == "UG" || CountryID == "ET")
                        {
                            fileEncrypted = SignedFile + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".chk";
                            byte[] b = signature.BRQVL(File.ReadAllBytes(fileName), TokenPass.Trim(), CountryID);
                            File.WriteAllBytes(fileEncrypted, b);
                        }
                        msgCallBack = "success";
                    }
                }
                catch (Exception ex)
                {
                    msgCallBack = ex.Message;
                    //MessageBox.Show("Step 5.3.1.7 + Chapa " + msgCallBack); 
                }


            }
            catch (Exception ex)
            {
                msgCallBack = "Missing token or invalid token name and password for:" + BRRSACryptography.CryptographyHelper.Decrypt(UserTokenName);
            }
            return msgCallBack;

        }

        private byte[] ReadSigned(byte[] dataBytes)
        {
            try
            {
                CmsSignedData signed = new CmsSignedData(dataBytes);
                IX509Store st = signed.GetCertificates("Collection/BC");
                SignerInformationStore infoStore = signed.GetSignerInfos();
                if (infoStore == null)
                    return null;
                ICollection c = infoStore.GetSigners();
                IEnumerator it = c.GetEnumerator();
                it.MoveNext();
                SignerInformation signer = (SignerInformation)it.Current;
                ICollection coll = st.GetMatches(signer.SignerID);
                it = coll.GetEnumerator();
                it.MoveNext();
                Org.BouncyCastle.X509.X509Certificate cert = (Org.BouncyCastle.X509.X509Certificate)it.Current;
                if (cert == null)
                    return null;
                if (signer.Verify(cert))
                    return (byte[])signed.SignedContent.GetContent();
                return dataBytes;
            }
            catch
            {
                return dataBytes;
            }
        }


        private void DecodeFiles(string SignedFile, string unSignedFile, string tokenName, string CountryID, bool k)
        {
            try
            {
                RemoveSign(SignedFile, tokenName, unSignedFile, CountryID, k);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RemoveSign(string SignedFile, string cert, string unSignedFile, string CountryID, bool TokenBased)
        {
            string z = "";
            var signature = new BRCSTDS(cert, false, out z, CountryID, TokenBased);
            byte[] b = ReadSigned(File.ReadAllBytes(SignedFile));
            File.WriteAllBytes(unSignedFile, b);
        }
    }
    public class BRCSTDS
    {
        //BRCSTDS = BRClrTokenDigiSign
        private string _cUser = string.Empty;
        private X509Certificate2 mycert;
        public Org.BouncyCastle.X509.X509Certificate _cert;
        private AsymmetricCipherKeyPair _key;
        private string TokenUserName;
        public BRCSTDS(string pp, bool tY, out string z, string c, bool k)
        {
            _cUser = pp;
            z = "";
            //MessageBox.Show("Step 5.3.1.1.1");
            if (c == "TZ" && k == false)
            {
                //GetCertificate();
                z = "";
            }
            else if (c == "TZ" && k == true)
            {
                GetCertificate(tY, c);
                z = BRRSACryptography.CryptographyHelper.Encrypt(TokenUserName);
            }
            else if (c == "UG")
            {
                GetCertificate(tY, c);
                z = BRRSACryptography.CryptographyHelper.Encrypt(TokenUserName);
            }
            else if (c== "ET")
            {
                GetCertificate(c);
                z = BRRSACryptography.CryptographyHelper.Encrypt(TokenUserName);
            }
            //MessageBox.Show("Step 5.3.1.1.2");
            //MessageBox.Show("Step 5.3.1.1.3");
        }
        private void GetCertificate()
        {
            X509Store st = null;
            _cUser = BRRSACryptography.CryptographyHelper.Decrypt(_cUser);
            try
            {
                st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                st.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = st.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, _cUser, false);

                if ((col.Count > 0))
                {
                    //MessageBox.Show("GetCertificate: Imeingia hapa");
                    X509Certificate2 thisCert = col[0];
                    string tname = thisCert.GetNameInfo(X509NameType.SimpleName, false).ToString();
                    TokenUserName = tname;
                    _cert = DotNetUtilities.FromX509Certificate(thisCert);
                    _key = GenerateKeys(thisCert.PrivateKey.KeySize);
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Step 5.3.1.1.1.i" + " : " + ex.Message);
            }
            finally
            {
                st.Close();
            }

        }
        private static void SignFile(string sFile, string dFile, string strBatchPath, string TokenPassword, string jksFilePath, string strJavaExeInstallation)
        {
            try
            {
                //MessageBox.Show("sFile  :"  + sFile);
                //MessageBox.Show("dFile  :" + dFile);
                //MessageBox.Show("strBatchPath  :" + strBatchPath);
                //MessageBox.Show("TokenPassword  :" + TokenPassword);
                //MessageBox.Show("jksFilePath  :" + jksFilePath);
                //MessageBox.Show("strJavaExeInstallation  :" + strJavaExeInstallation);

                jksFilePath = BRRSACryptography.CryptographyHelper.Decrypt(jksFilePath);
                strBatchPath = BRRSACryptography.CryptographyHelper.Decrypt(strBatchPath);
                strJavaExeInstallation = BRRSACryptography.CryptographyHelper.Decrypt(strJavaExeInstallation);
                //MessageBox.Show("TokenPassword 1 " + TokenPassword);
                TokenPassword = BRRSACryptography.CryptographyHelper.Decrypt(TokenPassword);
                //MessageBox.Show("sFile " + sFile);
                //MessageBox.Show("dFile " + dFile);
                //MessageBox.Show("strBatchPath " + strBatchPath);
                //MessageBox.Show("TokenPassword 2" + TokenPassword);
                //MessageBox.Show("jksFilePath " + jksFilePath);
                //MessageBox.Show("strJavaExeInstallation " + strJavaExeInstallation);
                sFile = sFile.Replace(@"\", "/");
                if (Path.GetExtension(strBatchPath) == ".bat")
                {
                    //MessageBox.Show(".bat ");
                    strBatchPath = Path.GetDirectoryName(strBatchPath);
                    strBatchPath = strBatchPath + @"\";
                }
                strBatchPath = strBatchPath + "Execute.bat";
                string Dskkey = jksFilePath.Substring(jksFilePath.ToString().LastIndexOf(@"\") + 1, jksFilePath.ToString().LastIndexOf(".") - jksFilePath.ToString().LastIndexOf(@"\") - 1);
                string strCmd = "\"" + strJavaExeInstallation.Trim() + "\" -cp .;com.springsource.org.bouncycastle.jce-1.46.0.jar;com.springsource.org.bouncycastle.mail-1.46.0.jar SignatureClient DSkeyFile=" + jksFilePath.Trim().Replace(@"\", "/") + " fileName=" + sFile + " function=sign mode=CMS keyAlias=" + Dskkey + " certificateAlias=" + Dskkey + " keyPass=" + TokenPassword + "";
                //MessageBox.Show("Dskkey " + Dskkey);

                //MessageBox.Show("strCmd " + strCmd);
                FileStream myFileStream = null;
                StreamWriter myEJContentStreamWriter = null;
                try
                {
                    myEJContentStreamWriter = new StreamWriter(strBatchPath, true);
                    myEJContentStreamWriter.WriteLine(strCmd);
                }
                finally
                {
                    if (!(myEJContentStreamWriter == null))
                        myEJContentStreamWriter.Close();
                }
                ExecuteCommand(strBatchPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registerd, check error log");
                ErrorLog("output>>" + (String.IsNullOrEmpty(ex.Message) ? "(none)" : ex.Message), "- SignFile-TZ Files");
            }
        }
        private static void ExecuteCommand(string strBatchPath)
        {
            try
            {
                int ExitCode;
                ProcessStartInfo ProcessInfo;
                Process process__1 = null;
                string output = "";
                string error = "";
                string strWorkingDir = Path.GetDirectoryName(strBatchPath);
                try
                {
                    ProcessInfo = new ProcessStartInfo(strBatchPath);
                    ProcessInfo.CreateNoWindow = true;
                    ProcessInfo.UseShellExecute = false;
                    ProcessInfo.WorkingDirectory = strWorkingDir;

                    ProcessInfo.RedirectStandardError = true;
                    ProcessInfo.RedirectStandardOutput = true;

                    process__1 = Process.Start(ProcessInfo);
                    process__1.WaitForExit();

                    output = process__1.StandardOutput.ReadToEnd();
                    error = process__1.StandardError.ReadToEnd();
                    if (error != "")
                    {
                        //MessageBox.Show("E2: " + output);
                        //MessageBox.Show("E2: " + error);
                        ErrorLog("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output), "ExecuteCommand-TZ Files");
                        ErrorLog("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error), "ExecuteCommand-TZ Files");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("E2: " + ex.Message);
                    MessageBox.Show("Error registerd, check error log");
                    ErrorLog("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output), "ExecuteCommand-TZ Files");
                    ErrorLog("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error), "ExecuteCommand-TZ Files");
                }
                ExitCode = process__1.ExitCode;
                process__1.Close();
                //MessageBox.Show("About to delete strBatchPath " + strBatchPath);
                File.Delete(strBatchPath);
                //MessageBox.Show("deleted strBatchPath " + strBatchPath);
                strBatchPath = "";
            }
            catch (Exception ex)
            {
                //MessageBox.Show("E3: " + ex.Message);
                MessageBox.Show("Error registerd, check error log");
                ErrorLog(ex.Message, "- Out ExecuteCommand-TZ Files");
            }
        }
        private static void ErrorLog(string strMsg, string strMethodorFunctionName)
        {
            string strErrorLogPath = ConfigurationManager.AppSettings["ClearingErrorLogFilePath"];
            if (!Directory.Exists(strErrorLogPath + @"\ClearingErrorLog\"))
                Directory.CreateDirectory(strErrorLogPath + @"\ClearingErrorLog\");
            string AppendErrorMessage = "Error Message: " + strMethodorFunctionName + " :" + strMsg.ToString() + Environment.NewLine + "Date" + ":" + DateTime.Now + Environment.NewLine + "--------------------------" + Environment.NewLine;
            System.IO.File.AppendAllText(strErrorLogPath + @"\ClearingErrorLog\" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".txt", AppendErrorMessage);
        }
        private void GetCertificate(string CountryID)
        {
            _cUser = BRRSACryptography.CryptographyHelper.Decrypt(_cUser);
            X509Store st = null;
            try
            {
                st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                st.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = st.Certificates.Find(X509FindType.FindBySubjectName, _cUser, true);
                if (col.Count > 0)
                {
                    X509Certificate2 thisCert = col[0];
                    _cert = DotNetUtilities.FromX509Certificate(thisCert);
                    _key = GenerateKeys(thisCert.PrivateKey.KeySize);
                }
            }
            catch { }
            finally
            {
                st.Close();
            }
        }
        private void GetCertificate(bool encrypdecry, string CountryID)
        {
            // MessageBox.Show("CUser :" + _cUser);
            _cUser = BRRSACryptography.CryptographyHelper.Decrypt(_cUser);
            X509Store store = null;
            try
            {
                //MessageBox.Show("Step 5.3.1.1.1.a");
                string TokenCertificateName = _cUser;
                string NonTokenCertificateName = "SelfSigned";
                string certLocation = "Token";
                store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.OpenExistingOnly);
                X509Certificate2Collection certificates_ = store.Certificates;
                X509Certificate2 certificate_ = new X509Certificate2();
                X509Certificate2 useCertificate_ = new X509Certificate2();
                if (certLocation == "Token")
                {
                    //MessageBox.Show("Step 5.3.1.1.1.b");
                    for (int i = 0; i < certificates_.Count; i++)
                    {
                        certificate_ = certificates_[i];
                        string subj = certificate_.Subject;
                        List<X509KeyUsageExtension> extensions = certificate_.Extensions.OfType<X509KeyUsageExtension>().ToList();
                        string tname = certificate_.GetNameInfo(X509NameType.SimpleName, false).ToString();
                        TokenUserName = tname;

                        //MessageBox.Show("TKN :" + tname + " - TCN :" + TokenCertificateName);//Kamunya

                        if (tname == TokenCertificateName)
                        {
                            for (int j = 0; j < extensions.Count; j++)
                            {
                                if ((extensions[j].KeyUsages & X509KeyUsageFlags.DigitalSignature) == X509KeyUsageFlags.DigitalSignature)
                                {
                                    useCertificate_ = certificate_;
                                    j = extensions.Count + 1;
                                    mycert = certificate_;
                                    X509Certificate2 thisCert = certificate_;
                                    _cert = DotNetUtilities.FromX509Certificate(thisCert);
                                    _key = encrypdecry == true ? GenerateKeys(thisCert.PrivateKey.KeySize) : GenerateKeys(thisCert.PublicKey.Key.KeySize);
                                }
                            }
                            return;
                        }
                        //else
                        //{
                        //    TokenUserName = "INV "+ tname;
                        //}
                    }
                }
                else
                {
                    for (int i = 0; i < certificates_.Count; i++)
                    {
                        certificate_ = certificates_[i];
                        string subj = certificate_.Subject;
                        List<X509KeyUsageExtension> extensions = certificate_.Extensions.OfType<X509KeyUsageExtension>().ToList();
                        if (certificate_.GetNameInfo(X509NameType.SimpleName, false).ToString() == NonTokenCertificateName)
                            useCertificate_ = certificate_;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Step 5.3.1.1.1.i" + " : " + ex.Message);
            }
            finally
            {
                store.Close();
            }
        }
        private AsymmetricCipherKeyPair GenerateKeys(int keySizeInBits)
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), keySizeInBits));
            AsymmetricCipherKeyPair keys = r.GenerateKeyPair();
            return keys;
        }
        //BRQVL = Token_Signature--UG
        public byte[] BRQVL(byte[] xp, string ds, string CountryID)
        {
            //(xp = dataBytes, ds = tokenpass)
            return LogonToken(mycert, ds, xp, CountryID);
        }
        //BRQVL = Token_Signature--TZ
        public void BRQVL(string sFile, string dFile, string strBatchPath, string TokenPassword, string jksFilePath, string strJavaExeInstallation)
        {
            SignFile(sFile, dFile, strBatchPath, TokenPassword, jksFilePath, strJavaExeInstallation);
        }



        private byte[] LogonToken(X509Certificate2 Cert, string password, byte[] dataBytes, string CountryID)
        {
            //MessageBox.Show("Step 5.3.1.5.i");
            try
            {
                password = BRRSACryptography.CryptographyHelper.Decrypt(password);
            }
            catch (Exception ex)
            {

            }
            //MessageBox.Show("Step 5.3.1.5.ii - " + password);
            byte[] str2 = null;
            RSACryptoServiceProvider privatekey = new RSACryptoServiceProvider();



            //MessageBox.Show("Step 5.3.1.5.iii");
            try
            {
                //MessageBox.Show("Cert :-" + Cert.NotAfter.ToString());
                if (!Cert.HasPrivateKey)
                {
                    //MessageBox.Show("Step 5.3.1.5.iv");
                    throw new CryptographicException("The Private key is not accessible !");
                }
                try
                {
                    privatekey = Cert.PrivateKey as RSACryptoServiceProvider;
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }

                //MessageBox.Show("Step 5.3.1.5.v");
                ;

                CspParameters parameters = new CspParameters(privatekey.CspKeyContainerInfo.ProviderType, privatekey.CspKeyContainerInfo.ProviderName)
                {

                    KeyNumber = 1,
                    Flags = CspProviderFlags.UseDefaultKeyContainer,
                    ProviderName = "Microsoft Strong Cryptographic Provider"

                };

                try
                {
                    //MessageBox.Show("Step 5.3.1.5.vi");
                    CryptoKeyAccessRule rule = new CryptoKeyAccessRule(_cUser, CryptoKeyRights.FullControl, AccessControlType.Allow);
                    parameters.CryptoKeySecurity = new CryptoKeySecurity();
                    parameters.CryptoKeySecurity.SetAccessRule(rule);

                    if (password.Length > 0)
                    {
                        SecureString str = new SecureString();
                        foreach (char ch in password)
                        {
                            str.AppendChar(ch);
                        }
                        parameters.KeyPassword = str;
                        //MessageBox.Show("Step 5.3.1.5.vii");
                    }

                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
                try
                {
                    //MessageBox.Show("Step 5.3.1.5.viii");
                    privatekey = new RSACryptoServiceProvider(parameters);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
                //MessageBox.Show("Step 5.3.1.5.ix");
                try
                {
                    //str2 = SignMessage(Cert, dataBytes);
                    str2 = SignMessageFile(_cert, dataBytes);
                    //MessageBox.Show("Step 5.3.1.5.x");
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Step 5.3.1.5.xi" + " : " + ex.Message);
                    throw new Exception(ex.Message + "Token belongs to: " + TokenUserName);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Step 5.3.1.5.vii.Chapa : " + ex.Message);

            }
            finally
            {
                privatekey.Clear();
            }
            //MessageBox.Show("Step 5.3.1.5.xii");
            return str2;
        }
        private void LogError(Exception ex)
        {
            string ErrorLogPath = "C:\\BRExceptionLog\\ClearingErrorLogs\\";
            int errline = GetLineNumber(ex);
            StackTrace stackTrace = new StackTrace();
            string MethodName = stackTrace.GetFrame(1).GetMethod().Name;
            if (Convert.ToString(ErrorLogPath) != "")
            {
                if (!Directory.Exists(ErrorLogPath))
                {
                    Directory.CreateDirectory(ErrorLogPath);
                }
                string AppendErrorMessage =
                Environment.NewLine
              + Environment.NewLine + "=============================================================================================="
                    //+ Environment.NewLine + "User Logged in :" + User
              + Environment.NewLine + "Error Message :" + ex.Message
                    //+ Environment.NewLine + "Stack Trace : " + ex.StackTrace
              + Environment.NewLine + "Target Site : " + ex.TargetSite
              + Environment.NewLine + "Line Number : " + errline
              + Environment.NewLine + "Error Method : LN-" + MethodName
              + Environment.NewLine + "Date : " + DateTime.Now
              + Environment.NewLine + "===============================================================================================";
                System.IO.File.AppendAllText(ErrorLogPath + "BRDigiSign" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + ".Errlog", AppendErrorMessage);
            }
        }
        private int GetLineNumber(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            try
            {
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    if (int.TryParse(lineNumberText, out lineNumber))
                    {
                    }
                }
            }
            catch (Exception x)
            {
                LogError(x);
            }
            return lineNumber;
        }
        private byte[] SignMessage(X509Certificate2 cert, byte[] dataBytes)
        {
            try
            {
                byte[] content = dataBytes;
                ContentInfo contentinfo = new ContentInfo(content);
                SignedCms cms = new SignedCms(contentinfo);
                CmsSigner signer = new CmsSigner(cert)
                {
                    IncludeOption = X509IncludeOption.EndCertOnly
                };
                cms.ComputeSignature(signer, false);
                return cms.Encode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Step 5.3.1.5.ttttttttt- Chapa " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public byte[] SignMessageFile(Org.BouncyCastle.X509.X509Certificate cert, byte[] dataBytes)
        {
            IList certList = new ArrayList();
            IList crlList = new ArrayList();
            CmsProcessable msg = new CmsProcessableByteArray(dataBytes);
            certList.Add(cert);
            IX509Store x509Certs = X509StoreFactory.Create("Certificate/Collection", new X509CollectionStoreParameters(certList));
            CmsSignedDataGenerator gen = new CmsSignedDataGenerator();
            gen.AddSigner(_key.Private, _cert, CmsSignedDataGenerator.DigestSha1);
            gen.AddCertificates(x509Certs);
            CmsSignedData signedData = gen.Generate(msg, true);
            return signedData.GetEncoded();
        }
        static private byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

    }
}
