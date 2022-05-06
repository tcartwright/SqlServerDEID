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

            CmdLineOptions cmdLineOptions = null;

            var parserResult = Parser.Default
                .ParseArguments<CmdLineOptions>(args)
                .WithParsed(options => cmdLineOptions = options);

            var notParsed = parserResult as NotParsed<CmdLineOptions>;
            if (notParsed != null && notParsed.Errors.Any())
            {
                return -1;
            }

            try
            {
                var stopwatch = Stopwatch.StartNew();
                _deid = new DEID(cmdLineOptions.TablesThreadCount, cmdLineOptions.UdateBatchSize, cmdLineOptions.ProcessRowCount, cmdLineOptions.OutputPowershell);
                _deid.RunTransform(cmdLineOptions.File);
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
