using System;
using System.Windows.Forms;

namespace BRNETUploadDownload
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new BRNetUploadDownLoadUtility());
            //Application.Run(new BRNetUploadDownLoadUtilityMFI());

            Application.Run(new frmLogin());


        }
    }
}