using CommandLine;

namespace SqlServerDEID
{
 
    [Verb("deid", HelpText = "Runs DEID operations against a database using a transform file.")]
    public class DeidCmdLineOptions
    {
        [Option('f', "file", 
            Required = true, 
            HelpText = "The transform xml or json file to run.")]
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
            HelpText = "A value that enables or disables batch processing support, and specifies the number of update commands that can be executed in a batch.\r\n\t\t0: There is no limit on the batch size.\r\n\t\t1: Disables batch updating.\r\n\t\t>1: Changes are sent using batches of 'UpdateBatchSize' operations at a time.",
            Default = 500)]
        public int UdateBatchSize { get; set; }
        [Option('o', "OutputPowershell",
            Required = false,
            HelpText = "If enabled the output from powershell scripts will be written out to the console.")]
        public bool OutputPowershell { get; set; }
    }


    [Verb("credential", HelpText = "Performs credential operations against the Credential Manager.")]
    public class CredentialCmdLineOptions
    {
        [Option('r', "RemoveCredential",
            Required = true,
            HelpText = "Removes a credential from the credential manager.",
            SetName = "RemoveCredential")]
        public bool RemoveCredential { get; set; }
        [Option('a', "SaveCredential",
            Required = true,
            HelpText = "Specifies that a credential is to be added or updated in the credential manager.", 
            SetName = "SaveCredential")]
        public bool SaveCredential { get; set; }
        [Option('n', "ApplicationName",
            Required = true,
            HelpText = "The application name for the credential.")]
        public string ApplicationName { get; set; }
        [Option('u', "UserName",
            Required = true,
            HelpText = "The user name for the credential.",
            SetName = "SaveCredential")]
        public string UserName { get; set; }
        [Option('p', "Password",
            Required = true,
            HelpText = "The password for the credential.",
            SetName = "SaveCredential")]
        public string Password { get; set; }
    }
}
