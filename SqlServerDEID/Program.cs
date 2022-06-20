using CommandLine;
using SqlServerDEID.Common;
using SqlServerDEID.Common.Globals.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace SqlServerDEID
{
    internal class Program
    {
        private static DEID _deid = null;
        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
        #endregion

        static int Main(string[] args)
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            DeidCmdLineOptions deidCmdLineOptions = null;
            CredentialCmdLineOptions credentialCmdLineOptions = null;

            var parserResult = Parser.Default
                .ParseArguments<DeidCmdLineOptions, CredentialCmdLineOptions>(args)
                .WithParsed<DeidCmdLineOptions>(options => deidCmdLineOptions = options)
                .WithParsed<CredentialCmdLineOptions>(options => credentialCmdLineOptions = options);

            var notParsed = parserResult as NotParsed<object>;
            if (notParsed != null && notParsed.Errors.Any())
            {
                return -1;
            }

            if (credentialCmdLineOptions != null)
            {
                try
                {
                    if (credentialCmdLineOptions.SaveCredential)
                    {
                        DEID.WriteCredential(credentialCmdLineOptions.ApplicationName, credentialCmdLineOptions.UserName, credentialCmdLineOptions.Password);
                        Console.WriteLine($"Credential '{credentialCmdLineOptions.ApplicationName}' written to the credential manager.");
                    }
                    else if (credentialCmdLineOptions.RemoveCredential)
                    {
                        DEID.RemoveCredential(credentialCmdLineOptions.ApplicationName);
                        Console.WriteLine($"Credential '{credentialCmdLineOptions.ApplicationName}' removed from the credential manager.");
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                    return -5;
                }
            }

            try
            {
                var stopwatch = Stopwatch.StartNew();
                _deid = new DEID(deidCmdLineOptions.TablesThreadCount, deidCmdLineOptions.UdateBatchSize, deidCmdLineOptions.ProcessRowCount, deidCmdLineOptions.OutputPowershell);
                _deid.RunTransform(deidCmdLineOptions.File);
                stopwatch.Stop();
                Console.WriteLine("Done in {0}", stopwatch.Elapsed.ToStringFormat());
                return 0;
            }
            catch (OperationCanceledException ocex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ocex.Message);
                Console.ResetColor();
                return -2;
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                if (ex.GetRootInnerException() is OperationCanceledException ocex)
                {
                    message = ocex.Message;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
                return -3;
            }
        }

        private static bool Handler(CtrlType sig)
        {
            if (_deid != null)
            {
                Console.WriteLine("DEID cancellation requested, shutting down.");
                if (_deid.Cancel())
                {
                    Console.WriteLine();
                    Console.WriteLine("DEID cancellation request complete.");
                    return true;
                }
                else
                {
                    Console.WriteLine("DEID transform process failed to shutdown properly.");
                    return false;
                }
            }
            return true;
        }
    }
}
