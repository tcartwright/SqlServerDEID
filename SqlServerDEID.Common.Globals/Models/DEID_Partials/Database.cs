using Meziantou.Framework.Win32;
using SqlServerDEID.Common.Globals.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SqlServerDEID.Common.Globals.Models
{
    public partial class Database : IValidatableObject
    {
        private static StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;
        private bool _isMetaDataRetrieved = false;
        public void GetMetaData(SqlConnection connection)
        {
            if (_isMetaDataRetrieved) { return; }

            foreach (var table in this.Tables)
            {
                table.GetMetaData(connection);
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

            if (_stringComparer.Equals(extension, ".xml"))
            {
                return LoadData(fileText, FileType.xml);
            }
            else if (_stringComparer.Equals(extension, ".json"))
            {
                return LoadData(fileText, FileType.json);
            }
            else
            {
                throw new FileFormatException("The file supplied must either be of JSON or XML format");
            }
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

        public DataSet ToDataSet()
        {
            var dataset = new DataSet();

            var tablesTable = new DataTable("Tables");
            tablesTable.Columns.Add("TableName", typeof(string));
            tablesTable.Columns.Add("DisableTriggers", typeof(bool));
            tablesTable.Columns.Add("DisableConstraints", typeof(bool));
            tablesTable.Columns.Add("ScriptTimeout", typeof(int));
            tablesTable.Columns.Add("ColumnsCount", typeof(int));

            var columnsTable = new DataTable("Columns");
            columnsTable.Columns.Add("TableName", typeof(string));
            columnsTable.Columns.Add("ColumnName", typeof(string));
            columnsTable.Columns.Add("Datatype", typeof(string));
            columnsTable.Columns.Add("IsPK", typeof(bool));
            columnsTable.Columns.Add("IsIdentity", typeof(bool));
            columnsTable.Columns.Add("IsComputed", typeof(bool));
            columnsTable.Columns.Add("TransformsCount", typeof(int));

            var transformsTable = new DataTable("Transforms");
            transformsTable.Columns.Add("TableName", typeof(string));
            transformsTable.Columns.Add("ColumnName", typeof(string));
            transformsTable.Columns.Add("Transform", typeof(string));
            transformsTable.Columns.Add("Type", typeof(int));
            transformsTable.Columns.Add("WhereClause", typeof(string));

            dataset.Tables.Add(tablesTable);
            dataset.Tables.Add(columnsTable);
            dataset.Tables.Add(transformsTable);

            dataset.Relations.Add(new DataRelation("Columns", tablesTable.Columns["TableName"], columnsTable.Columns["TableName"], true));
            dataset.Relations.Add(new DataRelation("Transforms", 
                new [] { 
                    columnsTable.Columns["TableName"], 
                    columnsTable.Columns["ColumnName"] 
                }, new [] { 
                    transformsTable.Columns["TableName"], 
                    transformsTable.Columns["ColumnName"] 
                }, true));

            foreach (var table in this.Tables)
            {
                var tableName = table.Name.CleanName();
                tablesTable.Rows.Add(tableName, table.DisableTriggers, table.DisableConstraints, table.ScriptTimeout, table.Columns.Count);

                foreach (var column in table.Columns)
                {
                    var columnName = column.Name.CleanName();
                    columnsTable.Rows.Add(tableName, columnName, column.SqlDbType.ToString(), column.IsPk, column.IsIdentity, column.IsComputed, column.Transforms.Count);

                    foreach (var transform in column.Transforms)
                    {
                        transformsTable.Rows.Add(tableName, columnName, transform.Transform, (int)transform.TransformType, transform.WhereClause);
                    }
                }
            }

            return dataset;
        }
    }

}
