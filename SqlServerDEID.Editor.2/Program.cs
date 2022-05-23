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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjM1ODU5QDMyMzAyZTMxMmUzMGV6V3JCdjQ2SUVHMVlTckwrU3dvdG5oRFU1T1E1YmxPNW9mWkZnM2xGeWs9");
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MDAxQDMyMzAyZTMxMmUzMGV6V3JCdjQ2SUVHMVlTckwrU3dvdG5oRFU1T1E1YmxPNW9mWkZnM2xGeWs9");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
