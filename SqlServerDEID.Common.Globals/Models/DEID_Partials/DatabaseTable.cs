﻿using SqlServerDEID.Common.Globals.Extensions;
using SqlServerDEID.Common.Globals.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    public partial class DatabaseTable
    {
        private const string tab = " ";
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IList<string> _columnNames;
        private IList<string> _updateColumnNames;
        private IList<string> _primaryKeyColumnNames;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private string _cleanName = String.Empty;
        private readonly StringComparer _stringComparer = StringComparer.InvariantCultureIgnoreCase;

        [XmlIgnore]
        [JsonIgnore]
        public string CleanName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_cleanName))
                {
                    _cleanName = this.Name.CleanName();
                }
                return _cleanName;
            }
        }

        public void Reset()
        {
            var reset = new DatabaseTable();
            this.Columns.Clear();
            this.DisableTriggers = reset.DisableTriggers;
            this.DisableConstraints = reset.DisableConstraints;
            this.ScriptTimeout = reset.ScriptTimeout;
        }

        public void GetMetaData(SqlConnection connection, bool addAllColumns = false)
        {
            var table = connection.ExecuteDataTable(
                Resources.GetTableMetaData,
                new[] {
                    new SqlParameter("@table_name", SqlDbType.NVarChar, 256) { Value = this.Name }
                },
                CommandType.Text);

            // remove any columns that do not exist in the database to cleanup
            for (int i = this.Columns.Count - 1; i >= 0; i--)
            {
                var col = this.Columns[i];
                if (table.Select($"column_name = '{col.CleanName}' OR column_name = '{col.Name}'").Length == 0)
                {
                    this.Columns.RemoveAt(i);
                }
            }

            foreach (DataRow row in table.Rows)
            {
                var column = this.Columns.FirstOrDefault(c => _stringComparer.Equals(c.CleanName, row["column_name"]));

                if (column != null)
                {
                    column.IsSelected = true;
                }
                else if (Convert.ToBoolean(row["is_pk"]) || addAllColumns)
                {
                    column = new DatabaseTableColumn
                    {
                        Name = Convert.ToString(row["column_name"])
                    };
                    this.Columns.Add(column);
                }

                if (column != null)
                {
                    column.SetMetaData(row);
                }
            }
        }

        private void GetColumnNames()
        {
            if (_columnNames == null)
            {
                _columnNames = this.Columns
                    .Where(c => !c.IsPk)
                    .Select(c => c.CleanName)
                    .ToList();
            }
            if (_updateColumnNames == null)
            {
                _updateColumnNames = this.Columns
                    .Where(c => !c.IsPk && !c.IsIdentity && !c.IsComputed && c.Transforms.Any(t => !string.IsNullOrWhiteSpace(t.Transform)))
                    .Select(c => c.CleanName)
                    .ToList();
            }
            if (_primaryKeyColumnNames == null)
            {
                _primaryKeyColumnNames = this.Columns
                    .Where(c => c.IsPk)
                    .OrderBy(c => c.PkOrdinal)
                    .Select(c => c.CleanName)
                    .ToList();
            }

        }

        public string GetSelectStatement(string whereClause = null, string padding = null, string columnsSeperator = "\r\n")
        {
            if (string.IsNullOrWhiteSpace(padding))
            {
                padding = tab;
            }
            GetColumnNames();
            var sb = new StringBuilder();
            // append the two lists together, so the primary key columns show up at the front of the query.
            var columns = _primaryKeyColumnNames.Concat(_columnNames).ToList();
            sb.AppendLine($"SELECT {string.Join($",{columnsSeperator}{padding}", columns.Select(c => $"[{c}]"))}");
            sb.AppendLine($"FROM {this.Name}");
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                if (!whereClause.Trim().ToLower().StartsWith("where"))
                {
                    whereClause = "WHERE " + whereClause.Trim();
                }
                sb.AppendLine(whereClause);
            }
            sb.AppendLine($"ORDER BY {string.Join($",{columnsSeperator}{padding}", _primaryKeyColumnNames.Select(c => $"[{c}]"))}");
            sb.AppendLine($"OFFSET @offset ROWS");
            sb.AppendLine($"FETCH NEXT @rows ROWS ONLY");

            return sb.ToString();
        }

        public string GetUpdateStatement()
        {
            GetColumnNames();
            var sb = new StringBuilder();
            sb.AppendLine($"UPDATE {this.Name} SET");
            sb.AppendLine($"{tab}{string.Join($",\r\n{tab}", _updateColumnNames.Select(c => $"[{c}] = @{c}"))}");
            sb.AppendLine("WHERE");
            sb.AppendLine($"{tab}{string.Join($",\r\n{tab}AND ", _primaryKeyColumnNames.Select(c => $"[{c}] = @{c}"))}");

            return sb.ToString();
        }

        public SqlParameter[] GetSelectParameters(int offset, int rows)
        {
            return new[] {
                new SqlParameter(){ ParameterName = "@offset", SqlDbType = SqlDbType.Int, Value = offset},
                new SqlParameter(){ ParameterName = "@rows", SqlDbType = SqlDbType.Int, Value = rows}
            };
        }

        public SqlParameter[] GetUpdateParameters()
        {
            var parameters = this.Columns
                .Where(c => c.IsPk || c.Transforms.Any(t => !string.IsNullOrWhiteSpace(t.Transform)))
                .Select(c => c.GetSqlParameter());
            return parameters.ToArray();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public bool HasTransforms()
        {
            return this.Columns.Any(c => c.Transforms.Any(t => !string.IsNullOrWhiteSpace(t.Transform)));
        }
    }

}
