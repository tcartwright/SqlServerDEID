using Meziantou.Framework.Win32;
using SqlServerDEID.Common.Globals.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    public partial class Database : IValidatableObject
    {
        private static StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;
        private bool _isMetaDataRetrieved = false;
        public void GetMetaData(SqlConnection connection, bool addAllColumns = false)
        {
            if (_isMetaDataRetrieved) { return; }

            foreach (var table in this.Tables)
            {
                table.GetMetaData(connection, addAllColumns);
            }

            if (this.Tables.Any(t => !t.Columns.Any(c => c.IsPk)))
            {
                throw new ApplicationException("One or more tables does not have a primary key. The DEID system requires all tables to have a primary key.");
            }
            _isMetaDataRetrieved = true;
        }


        public static Database LoadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) { throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath)); }

            var path = filePath.GetPath(true);
            var fileText = File.ReadAllText(path);
            var extension = Path.GetExtension(path);
            Database database = null;

            if (_stringComparer.Equals(extension, ".xml"))
            {
                database = LoadData(fileText, FileType.xml);
            }
            else if (_stringComparer.Equals(extension, ".json"))
            {
                database= LoadData(fileText, FileType.json);
            }
            else
            {
                throw new FileFormatException("The file supplied must either be of JSON or XML format");
            }
            database.TransformFilePath = path;
            return database;
        }

        public static Database LoadData(string fileData, FileType fileType)
        {
            Database database;
            switch (fileType)
            {
                case FileType.xml:
                    database = fileData.FromXml<Database>();
                    break;
                case FileType.json:
                    database = fileData.FromJson<Database>();
                    break;
                default:
                    throw new FileFormatException("The file supplied must either be of JSON or XML format");
            }
            return database;
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(CredentialsName))
            {
                var credentials = CredentialManager.ReadCredential(applicationName: this.CredentialsName);
                if (credentials == null)
                {
                    yield return new ValidationResult($"- The database has a credentials specified, but a generic credential could not be found in the Credential Manager with the name {CredentialsName}. ");
                }
            }

            foreach (var table in this.Tables)
            {
                var columns = table.Columns.Where(c => c.Transforms != null && c.Transforms.Count() > 1 && c.Transforms.Any(x => string.IsNullOrWhiteSpace(x.WhereClause)));
                foreach (var column in columns)
                {
                    yield return new ValidationResult($"- Column {table.Name}.[{column.CleanName}] has multiple transforms, but one or more of them has an empty where clause. ");
                }
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public string TransformFilePath { get; set; }


        public override string ToString()
        {
            return $"{this.ServerName} - {this.DatabaseName}";
        }
    }

}
