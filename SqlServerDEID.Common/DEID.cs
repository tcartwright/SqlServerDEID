using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.PowerShell;
using SqlServerDEID.Common.Globals.Extensions;
using SqlServerDEID.Common.Globals.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;

namespace SqlServerDEID.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DEID : IDEID
    {
        private StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;
        private InitialSessionState _initialSessionState = null;
        private Database _database;
        private CancellationTokenSource _cancellationTokenSource = null;
        private static Action<string, MessageImportance> _stdout = null;
        private static Action<string> _stderror = null;

        public int TablesThreadCount { get; }
        public int UpdateBatchSize { get; }
        public int ProcessRowCount { get; }
        public bool OutputPowershell { get; }

        public DEID(int tablesThreadCount, int updateBatchSize, int processRowCount, bool outputPowershell, Action<string, MessageImportance> stdout = null, Action<string> stderror = null)
        {
            // limit the thread count to 1 to processor (core) count
            TablesThreadCount = Math.Max(1, Math.Min(Environment.ProcessorCount, tablesThreadCount));
            // limit the row counts to keep memory usage low
            ProcessRowCount = Math.Max(500, Math.Min(500000, processRowCount));
            UpdateBatchSize = Math.Max(0, Math.Min(1000, updateBatchSize));
            OutputPowershell = outputPowershell;

            _stderror = stderror;
            _stdout = stdout;

            Thread.GetDomain().UnhandledException += (sender, eventArgs) => Exiting(sender, (Exception)eventArgs.ExceptionObject);
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) => Exiting(sender, null);
            AppDomain.CurrentDomain.DomainUnload += (sender, eventArgs) => Exiting(sender, null);

            if (_stdout == null)
            {
                _stdout = (msg, importance) =>
                {
                    if (importance == MessageImportance.High)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(msg);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(msg);
                    }
                };
            }
            if (_stderror == null)
            {
                _stderror = (msg) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(msg);
                    Console.ResetColor();
                };
            }
        }

        private void Exiting(object sender, Exception exception)
        {
            if (_cancellationTokenSource != null)
            {
                Console.WriteLine("DEID process is being shut down. Please wait for graceful exit.");
                if (this.Cancel())
                {
                    Console.WriteLine();
                    Console.WriteLine("Cancellation request complete.");
                }
                else
                {
                    Console.WriteLine("DEID transform process failed to shutdown properly.");
                }
            }
        }

        /// <summary>
        /// Runs the transform.
        /// </summary>
        /// <param name="filePath">The tranform file containing the transform xml.</param>
        /// <exception cref="System.ArgumentException">'{nameof(tranformFile)}' cannot be null or whitespace. - tranformFile</exception>
        public void RunTransform(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) { throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath)); }

            RunTransform(Database.LoadFile(filePath));
        }

        /// <summary>
        /// Runs the transform.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <exception cref="System.IO.FileFormatException">The file supplied must either be of JSON or XML format</exception>
        public void RunTransform(string data, FileType fileType)
        {
            if (string.IsNullOrWhiteSpace(data)) { throw new ArgumentException($"'{nameof(data)}' cannot be null or whitespace.", nameof(data)); }

            RunTransform(Database.LoadData(data, fileType));
        }


        /// <summary>
        /// Runs the transform.
        /// </summary>
        /// <param name="transformXml">The transform XML.</param>
        /// <exception cref="System.ArgumentException">'{nameof(transformXml)}' cannot be null or whitespace. - transformXml</exception>
        public void RunTransform(Database database)
        {
            _database = database;
            _cancellationTokenSource = new CancellationTokenSource();

            var results = new List<ValidationResult>();
            var context = new ValidationContext(database, null, null);
            if (!Validator.TryValidateObject(database, context, results, true))
            {
                var message = string.Join("\r\n", results.Select(x => $" - {x.ErrorMessage}"));
                WriteError(message);
                return;
            }

            var scriptingImports = database.ScriptingImports
                .Select(si => si.NameSpace)
                .ToList();

            // ensure these imports are always added by default
            scriptingImports.Add("Bogus");
            scriptingImports.Add("Bogus.DataSets");
            scriptingImports.Add("System");
            scriptingImports.Add("System.Text");
            scriptingImports.Add("System.Text.RegularExpressions");

            var imports = scriptingImports
                .Distinct()
                .OrderBy(si => si)
                .ToArray();

            var cancellationToken = _cancellationTokenSource.Token;

            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = TablesThreadCount,
                CancellationToken = cancellationToken
            };


            try
            {
                Parallel.ForEach(database.Tables, parallelOptions, databaseTable =>
                {
                    // cant use the transaction scope with runspace. https://stackoverflow.com/questions/29804554/calling-runspace-open-inside-a-transactionscope-changes-transaction-current-an
                    //var transactionOptions = new TransactionOptions()
                    //{
                    //    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                    //};
                    //using (var trans = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                    using (var connection = database.GetConnection())
                    {
                        try
                        {
                            connection.Open();
                            database.GetMetaData(connection);

                            var sw = Stopwatch.StartNew();
                            WriteLine("[{0}]Starting [{1}].{2}", Thread.CurrentThread.ManagedThreadId, database.ServerName.CleanName().ToUpper(), databaseTable.Name.ToUpper());

                            if (databaseTable.DisableConstraints)
                            {
                                WriteLine("DISABLING ALL CONSTRAINTS FOR TABLE {0}", databaseTable.Name);
                                connection.RunScript($"ALTER TABLE {databaseTable.Name} NOCHECK CONSTRAINT ALL", (Exception ex) => WriteError($"EXCEPTION DISABLING CONSTRAINTS: {ex.Message}"));
                            }
                            if (databaseTable.DisableTriggers)
                            {
                                WriteLine("DISABLING ALL TRIGGERS FOR TABLE {0}", databaseTable.Name);
                                connection.RunScript($"ALTER TABLE {databaseTable.Name} DISABLE TRIGGER ALL", (Exception ex) => WriteError($"EXCEPTION DISABLING TRIGGERS: {ex.Message}"));
                            }

                            connection.RunScriptFile(database.TransformFilePath, database.PreScript, database.ScriptTimeout);
                            UpdateTable(connection, databaseTable, imports, cancellationToken, database.Locale);
                            connection.RunScriptFile(database.TransformFilePath, database.PostScript, database.ScriptTimeout);

                            sw.Stop();
                            WriteLine("[{0}]Finished [{1}].{2} in {3}", Thread.CurrentThread.ManagedThreadId, database.ServerName.CleanName().ToUpper(), databaseTable.Name.ToUpper(), sw.Elapsed.ToStringFormat());
                        }
                        catch (Exception)
                        {
                            _cancellationTokenSource.Cancel();
                            throw;
                        }
                        finally
                        {
                            if (connection.State != ConnectionState.Closed)
                            {
                                if (databaseTable.DisableConstraints)
                                {
                                    WriteLine("ENABLING ALL CONSTRAINTS FOR TABLE {0}", databaseTable.Name);
                                    connection.RunScript($"ALTER TABLE {databaseTable.Name} WITH CHECK CHECK CONSTRAINT ALL", (Exception ex) => WriteError($"EXCEPTION ENABLING CONSTRAINTS: {ex.Message}"), 600);
                                }
                                if (databaseTable.DisableTriggers)
                                {
                                    WriteLine("ENABLING ALL TRIGGERS FOR TABLE {0}", databaseTable.Name);
                                    connection.RunScript($"ALTER TABLE {databaseTable.Name} ENABLE TRIGGER ALL", (Exception ex) => WriteError($"EXCEPTION ENABLING TRIGGERS: {ex.Message}"));
                                }
                            }
                        }
                    }
                });
            }
            finally
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Cancels the transform operation if one is running.
        /// </summary>
        public bool Cancel()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel(false);
                var stopwatch = Stopwatch.StartNew();

                while (_cancellationTokenSource != null)
                {
                    Thread.Sleep(1000);
                    if (stopwatch.Elapsed.TotalMinutes >= 3) { return false; }
                }
            }
            return true;
        }

        private void UpdateTable(SqlConnection connection, DatabaseTable databaseTable, string[] imports, CancellationToken cancellationToken, string locale = "en")
        {
            if (cancellationToken.IsCancellationRequested) { return; }

            var offset = 0;
            var tableRowCount = 0;

            //create the scriptglobals to be used by both the C# and PS scripts
            var scriptGlobals = new ScriptGlobals(locale);

            // the scriptoptions for the C# scriptlets
            var scriptOptions = ScriptOptions.Default
                .WithImports(imports)
                .WithReferences(
                    typeof(Bogus.Faker).Assembly,
                    typeof(ScriptGlobals).Assembly
                );

            //creating and re-using the runspace pool for a lot of PS script(s) for performance 
            using (var runspacePool = RunspaceFactory.CreateRunspacePool(CreateISS()))
            {
                runspacePool.Open();

                try
                {
                    connection.RunScriptFile(_database.TransformFilePath, databaseTable.PreScript, databaseTable.ScriptTimeout);
                    do
                    {
                        if (cancellationToken.IsCancellationRequested) { return; }

                        using (var dataAdapter = new SqlDataAdapter(databaseTable.GetSelectStatement(), connection))
                        {
                            dataAdapter.RowUpdating += DataAdapter_RowUpdating;
                            dataAdapter.SelectCommand.Parameters.AddRange(databaseTable.GetSelectParameters(offset, ProcessRowCount));
                            dataAdapter.SelectCommand.UpdatedRowSource = UpdateRowSource.None;

                            dataAdapter.UpdateCommand = new SqlCommand(databaseTable.GetUpdateStatement(), connection);
                            dataAdapter.UpdateCommand.Parameters.AddRange(databaseTable.GetUpdateParameters());
                            dataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                            var table = new DataTable();
                            dataAdapter.Fill(table);

                            if ((tableRowCount = table.Rows.Count) == 0) { break; }

                            WriteLine("[{0}]Processing table {1}, Rows {2}-{3}, RowCount {4}", Thread.CurrentThread.ManagedThreadId, databaseTable.Name.ToUpper(), offset, offset + ProcessRowCount, tableRowCount);

                            //RowValue expressions must be processed AFTER the other expressions as they use the results of the initial bogus values.
                            //we will leave it to the user to order them correctly
                            foreach (var column in databaseTable.Columns.Where(c => c.Transforms != null && !(c.IsPk || c.IsComputed || c.IsIdentity)))
                            {
                                if (cancellationToken.IsCancellationRequested) { return; }

                                WriteLine("[{0}]Processing column {1}.{2}", Thread.CurrentThread.ManagedThreadId, databaseTable.Name.ToUpper(), column.Name.ToUpper());

                                // convert the column to a very simple POCO so we can pass it into the PS, and C# script
                                scriptGlobals.DatabaseName = connection.Database;
                                scriptGlobals.TableName = databaseTable.Name;
                                scriptGlobals.Column = column.ToColumnInfo();

                                foreach (var columnTransform in column.Transforms.Where(t => !string.IsNullOrWhiteSpace(t.Transform)))
                                {
                                    if (cancellationToken.IsCancellationRequested) { return; }

                                    DataView view = table.AsDataView();
                                    if (!string.IsNullOrEmpty(columnTransform.WhereClause))
                                    {
                                        view.RowFilter = columnTransform.WhereClause;
                                    }

                                    //Getting this when using Parallel: System.InvalidOperationException: 'DataTable internal index is corrupted: '5'.'
                                    // turns out that the DataTable / DataView is not thread safe
                                    //Parallel.ForEach(view.Cast<DataRowView>(), parallelOptions, (DataRowView row) =>

                                    foreach (DataRowView row in view)
                                    {
                                        if (cancellationToken.IsCancellationRequested) { return; }
                                        row.Row.BeginEdit();

                                        // convert the rowvalues into a dynamic object
                                        scriptGlobals.RowValues = row.ToExpandoObject();
                                        if (columnTransform.TransformType == TransformType.expression)
                                        {
                                            var script = columnTransform.Script as Script<object>;
                                            if (script == null)
                                            {
                                                //create the script once for each column for the sake of speed and cache it for the other rows
                                                script = CSharpScript.Create(columnTransform.Transform, options: scriptOptions, globalsType: scriptGlobals.GetType());
                                                columnTransform.Script = script;
                                            }
                                            
                                            var result = script.RunAsync(scriptGlobals);
                                            row[scriptGlobals.Column.Name] = result.Result.ReturnValue;
                                        }
                                        else if (columnTransform.TransformType == TransformType.powershell)
                                        {
                                            var result = RunPowerShell(runspacePool, columnTransform, scriptGlobals, imports);
                                            row[scriptGlobals.Column.Name] = result;
                                        }

                                        row.Row.EndEdit();
                                        // can only call setmodified once, otherwise an exception occurs
                                        if (row.Row.RowState == DataRowState.Unchanged)
                                        {
                                            row.Row.SetModified();
                                        }
                                    }
                                }
                            }

                            offset += ProcessRowCount;
                            if (cancellationToken.IsCancellationRequested) { return; }
                            WriteLine("[{0}]Updating table {1}, Rows {2}-{3}, RowCount {4}", Thread.CurrentThread.ManagedThreadId, databaseTable.Name.ToUpper(), offset, offset + ProcessRowCount, tableRowCount);
                            dataAdapter.UpdateBatchSize = UpdateBatchSize;
                            dataAdapter.Update(table);
                            WriteLine("[{0}]Update table complete {1}, Rows {2}-{3}, RowCount {4}", Thread.CurrentThread.ManagedThreadId, databaseTable.Name.ToUpper(), offset, offset + ProcessRowCount, tableRowCount);

                            //if we did not get a full processrow count, then we are at the end of the run
                            if (tableRowCount < ProcessRowCount) { break; }
                        }
                    } while (tableRowCount > 0);
                    connection.RunScriptFile(_database.TransformFilePath, databaseTable.PostScript, databaseTable.ScriptTimeout);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception proccessing table: {databaseTable.Name}", ex);
                }
            }

            return;
        }

        private void DataAdapter_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
        {
            //if cancelled out, lets bail out on our updates asap
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
        }

        private object RunPowerShell(RunspacePool runspacePool, DatabaseTableColumnTransform columnTransform, ScriptGlobals scriptGlobals, string[] imports)
        {
            var path = columnTransform.Transform.GetPath(true, _database.TransformFilePath);

            using (PowerShell ps = PowerShell.Create())
            {
                ps.RunspacePool = runspacePool;

                ps.AddCommand(path)
                    .AddParameter("Faker", scriptGlobals.Faker)
                    .AddParameter("Column", scriptGlobals.Column)
                    .AddParameter("RowValues", scriptGlobals.RowValues)
                    .AddParameter("Male", scriptGlobals.Male)
                    .AddParameter("Female", scriptGlobals.Female);

                var streams = ps.Streams;
                if (OutputPowershell)
                {
                    streams.Debug.DataAdded += Debug_DataAdded;
                    streams.Error.DataAdded += Error_DataAdded;
                    streams.Information.DataAdded += Information_DataAdded;
                    streams.Progress.DataAdded += Progress_DataAdded;
                    streams.Verbose.DataAdded += Verbose_DataAdded;
                    streams.Warning.DataAdded += Warning_DataAdded;
                }
                try
                {
                    var psOut = ps.Invoke();

                    if (ps.HadErrors)
                    {
                        var errors = ps.Streams.Error.ReadAll();
                        foreach (var error in errors)
                        {
                            throw error.Exception;
                        }
                    }

                    return Convert.ToString(psOut.LastOrDefault());
                }
                finally
                {
                    if (OutputPowershell)
                    {
                        streams.Debug.DataAdded -= Debug_DataAdded;
                        streams.Error.DataAdded -= Error_DataAdded;
                        streams.Information.DataAdded -= Information_DataAdded;
                        streams.Progress.DataAdded -= Progress_DataAdded;
                        streams.Verbose.DataAdded -= Verbose_DataAdded;
                    }
                }
            }
        }

        private static void Warning_DataAdded(object sender, DataAddedEventArgs e)
        {
            var records = (PSDataCollection<WarningRecord>)sender;
            WriteLine("PS-WARNING>{0}", records[e.Index]);
        }

        private static void Debug_DataAdded(object sender, DataAddedEventArgs e)
        {
            var records = (PSDataCollection<DebugRecord>)sender;
            WriteLine("PS>{0}", records[e.Index]);
        }

        private static void Progress_DataAdded(object sender, DataAddedEventArgs e)
        {
            var records = (PSDataCollection<ProgressRecord>)sender;
            WriteLine("PS>{0}", records[e.Index]);
        }

        private static void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var records = (PSDataCollection<ErrorRecord>)sender;
            WriteLine("PS-ERROR>{0}", records[e.Index]);
        }

        private static void Information_DataAdded(object sender, DataAddedEventArgs e)
        {
            var records = (PSDataCollection<InformationRecord>)sender;
            WriteLine("PS>{0}", records[e.Index]);
        }

        private static void Verbose_DataAdded(object sender, DataAddedEventArgs e)
        {
            var records = (PSDataCollection<VerboseRecord>)sender;
            WriteLine("PS-VERBOSE>{0}", records[e.Index]);
        }

        private InitialSessionState CreateISS()
        {
            if (_initialSessionState == null)
            {
                _initialSessionState = InitialSessionState.CreateDefault();
                _initialSessionState.ApartmentState = ApartmentState.STA;
                _initialSessionState.ExecutionPolicy = ExecutionPolicy.Bypass;
                _initialSessionState.Assemblies.Add(new SessionStateAssemblyEntry(typeof(Bogus.Faker).Assembly.ToString()));
                _initialSessionState.Assemblies.Add(new SessionStateAssemblyEntry(typeof(ScriptGlobals).Assembly.ToString()));
            }
            return _initialSessionState;
        }

        private static void WriteLine(string message, params object[] args)
        {
            var msg = $"[{DateTime.Now:yyyy-MM-ddTHH\\:mm\\:ss.fffzzz}]{string.Format(message, args)}";
            _stdout(msg, MessageImportance.Normal);
        }
        private static void WriteLine(string message, MessageImportance importance = MessageImportance.Normal, params object[] args)
        {
            var msg = $"[{DateTime.Now:yyyy-MM-ddTHH\\:mm\\:ss.fffzzz}]{string.Format(message, args)}";
            _stdout(msg, importance);
        }
        private static void WriteError(string message)
        {
            _stderror($"[{DateTime.Now:yyyy-MM-ddTHH\\:mm\\:ss.fffzzz}]{message}");
        }
    }
}
