using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerDEID.Editor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzQ4NTY4QDMyMzAyZTMzMmUzME9WN21wOStJbGh2S2VEZ05qMlJnTVh5VFBBbFVGWEh2V2dDellkMmtMMjg9");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            MessageBox.Show($"Unhandled exception occurred:\r\n\r\n {ex}", "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
