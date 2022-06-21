using CommandLine;
using SqlServerDEID.Common;
using SqlServerDEID.Common.Globals;
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

        enum RETURN_VALUE: int
        {
            OK = 0,  
            CMD_LINE_NOT_PARSED = -1,
            CREDENTIAL_FAILURE = -2,
            OPERATION_CANCELLED = -3,
            DEID_EXCEPTION = -4
        }

        static int Main(string[] args)
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            var result = RETURN_VALUE.OK;
            var parserResult = Parser.Default
                .ParseArguments<DeidCmdLineOptions, CredentialCmdLineOptions>(args)
                .WithParsed<DeidCmdLineOptions>(options => result = RunDEID(options))
                .WithParsed<CredentialCmdLineOptions>(options => result = RunCredentialsOp(options))
                .WithNotParsed(errors => result = RETURN_VALUE.CMD_LINE_NOT_PARSED);

            return Convert.ToInt32(result);
        }

        private static RETURN_VALUE RunCredentialsOp(CredentialCmdLineOptions credentialCmdLineOptions)
        {
            try
            {
                if (credentialCmdLineOptions.SaveCredential)
                {
                    Credentials.WriteCredential(credentialCmdLineOptions.ApplicationName, credentialCmdLineOptions.UserName, credentialCmdLineOptions.Password);
                    Console.WriteLine($"Credential '{credentialCmdLineOptions.ApplicationName}' written to the credential manager.");
                }
                else if (credentialCmdLineOptions.RemoveCredential)
                {
                    Credentials.RemoveCredential(credentialCmdLineOptions.ApplicationName);
                    Console.WriteLine($"Credential '{credentialCmdLineOptions.ApplicationName}' removed from the credential manager.");
                }
                return RETURN_VALUE.OK;
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
                return RETURN_VALUE.CREDENTIAL_FAILURE;
            }
        }

        private static RETURN_VALUE RunDEID(DeidCmdLineOptions options)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                _deid = new DEID(options.TablesThreadCount, options.UdateBatchSize, options.ProcessRowCount, options.OutputPowershell);
                _deid.RunTransform(options.File);
                stopwatch.Stop();
                Console.WriteLine("Done in {0}", stopwatch.Elapsed.ToStringFormat());
                return RETURN_VALUE.OK;
            }
            catch (OperationCanceledException ocex)
            {
                WriteError(ocex.Message);
                return RETURN_VALUE.OPERATION_CANCELLED;
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                if (ex.GetRootInnerException() is OperationCanceledException ocex)
                {
                    message = ocex.Message;
                }
                WriteError(message);
                return RETURN_VALUE.DEID_EXCEPTION;
            }
        }

        private static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
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
