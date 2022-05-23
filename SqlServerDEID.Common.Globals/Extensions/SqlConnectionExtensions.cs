using Meziantou.Framework.Win32;
using SqlServerDEID.Common.Globals.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
    {
        public static SqlConnection GetConnection(this Database db)
        {
            if (db is null) { throw new System.ArgumentNullException(nameof(db)); }

            return GetConnection(db.ServerName, db.DatabaseName, db.Port, db.CredentialsName);
        }

        public static SqlConnection GetConnection(string serverName, string databaseName, int port = 0, string credentialsName = null)
        {
            if (string.IsNullOrWhiteSpace(serverName)) { throw new System.ArgumentException($"'{nameof(serverName)}' cannot be null or whitespace.", nameof(serverName)); }
            if (string.IsNullOrWhiteSpace(databaseName)) { throw new System.ArgumentException($"'{nameof(databaseName)}' cannot be null or whitespace.", nameof(databaseName)); }

            SqlCredential credentials = null;

            var connectionString = $"Persist Security Info=False;Initial Catalog={databaseName};MultipleActiveResultSets=True;Application Name=SQLServerDEID;";
            if (port == 0)
            {
                connectionString += $"Data Source={serverName};";
            }
            else
            {
                connectionString += $"Data Source={serverName},{port};";
            }
            if (string.IsNullOrWhiteSpace(credentialsName))
            {
                connectionString += "Integrated Security=SSPI;";
            }
            else
            {
                var cred = CredentialManager.ReadCredential(applicationName: credentialsName);
                if (cred == null)
                {
                    throw new CredentialNotFoundException($"Credentials could not be found by the name: {credentialsName} in the Credential Manager");
                }
                // https://github.com/dotnet/platform-compat/blob/master/docs/DE0001.md 
                // until they come up with a better way.... this is the only way for now.
                var securePwd = new SecureString();
                foreach (var c in cred.Password) { securePwd.AppendChar(c); }
                securePwd.MakeReadOnly();
                credentials = new SqlCredential(cred.UserName, securePwd);
            }

            return new SqlConnection(connectionString, credentials);
        }

        public static void RunScriptFile(this SqlConnection connection, string baseTransformPath, string fileName, int timeout = 30)
        {
            if (connection is null) { throw new System.ArgumentNullException(nameof(connection)); }

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                fileName = fileName.GetPath(true, baseTransformPath);
                var contents = File.ReadAllText(fileName);
                var scripts = Regex.Split(contents, @"^GO\s*$", RegexOptions.IgnoreCase);

                foreach (var script in scripts)
                {
                    connection.RunScript(script, timeout);
                }
            }
        }

        public static void RunScript(this SqlConnection connection, string commandText, int timeout = 30)
        {
            if (connection is null) { throw new System.ArgumentNullException(nameof(connection)); }

            if (!string.IsNullOrWhiteSpace(commandText))
            {
                connection.ExecuteNonQuery((command) =>
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = timeout;
                });
            }
        }
        public static void RunScript(this SqlConnection connection, string commandText, Action<Exception> exceptionHandler, int timeout = 30)
        {
            if (connection is null) { throw new System.ArgumentNullException(nameof(connection)); }

            try
            {
                connection.RunScript(commandText, timeout);
            }
            catch (Exception ex)
            {
                exceptionHandler(ex);
                return;
            }
        }
    }
}

