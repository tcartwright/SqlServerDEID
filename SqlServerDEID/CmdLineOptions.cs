using CommandLine;

namespace SqlServerDEID
{
    public class CmdLineOptions
    {
        [Option('f', "file", Required = true, HelpText = "The transform xml or json file to run.")]
        public string File { get; set; }

        [Option('r', "RowCount", 
            Required = false, 
            HelpText = "The max number of rows to process for each table at one time.", 
            Default = 5000)]
        public int ProcessRowCount { get; set; }

        [Option('t', "TablesThreadCount", 
            Required = false, 
            HelpText = "The max number of threads to use when processing tables.", 
            Default = 4)]
        public int TablesThreadCount { get; set; }

        [Option('b', "UdateBatchSize",
            Required = false,
            HelpText = "A value that enables or disables batch processing support, and specifies the number of commands that can be executed in a batch.\r\n\t\t0: There is no limit on the batch size.\r\n\t\t1: Disables batch updating.\r\n\t\t>1: Changes are sent using batches of 'UpdateBatchSize' operations at a time.",
            Default = 250)]
        public int UdateBatchSize { get; set; }

        [Option('o', "OutputPowershell",
            Required = false,
            HelpText = "If enabled the output from powershell scripts will be written out to the console.")]
        public bool OutputPowershell { get; set; }
    }
}
