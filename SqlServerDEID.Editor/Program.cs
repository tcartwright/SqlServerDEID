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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjM1ODU5QDMyMzAyZTMxMmUzMGV6V3JCdjQ2SUVHMVlTckwrU3dvdG5oRFU1T1E1YmxPNW9mWkZnM2xGeWs9");

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
