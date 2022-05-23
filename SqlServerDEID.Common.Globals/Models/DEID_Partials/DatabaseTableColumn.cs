using SqlServerDEID.Common.Globals.Extensions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SqlServerDEID.Common.Globals.Models
{
    public partial class DatabaseTableColumn
    {
        private string _cleanName = String.Empty;

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

        internal void SetMetaData(DataRow row)
        {
            SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), Convert.ToString(row["type_name"]), true);
            if (!row.IsNull("max_length")) { MaxLength = Convert.ToInt32(row["max_length"]); }
            if (!row.IsNull("precision")) { Precision = Convert.ToByte(row["precision"]); }
            if (!row.IsNull("scale")) { Scale = Convert.ToByte(row["scale"]); }
            IsIdentity = Convert.ToBoolean(row["is_identity"]);
            IsComputed = Convert.ToBoolean(row["is_computed"]);
            IsPk = Convert.ToBoolean(row["is_pk"]);
            if (!row.IsNull("pk_ordinal")) { PkOrdinal = Convert.ToInt32(row["pk_ordinal"]); }
        }

        public ColumnInfo ToColumnInfo()
        {
            return new ColumnInfo()
            {
                Name = this.CleanName,
                SqlType = this.SqlDbType.ToString().ToUpper(),
                DataType = this.DataType,
                MaxLength = this.MaxLength,
                Scale = this.Scale,
                Precision = this.Precision
            };
        }

        public SqlParameter GetSqlParameter()
        {
            var parameter = new SqlParameter()
            {
                ParameterName = $"@{this.CleanName}",
                SqlDbType = this.SqlDbType,
                Direction = ParameterDirection.Input,
                SourceColumn = this.CleanName
            };

            switch (this.SqlDbType)
            {
                case SqlDbType.VarBinary:
                case SqlDbType.Binary:
                    parameter.Size = this.MaxLength;
                    break;
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                    parameter.Size = this.MaxLength;
                    break;
                case SqlDbType.Decimal:
                    parameter.Scale = this.Scale;
                    parameter.Precision = this.Precision;
                    break;
                case SqlDbType.Float:
                    parameter.Precision = this.Precision;
                    break;
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                case SqlDbType.Time:
                    parameter.Scale = this.Scale;
                    break;
            }

            return parameter;
        }

        public override string ToString()
        {
            return $"{this.Name} {this.SqlDbType}";
        }

        [XmlIgnore]
        [JsonIgnore]
        public bool IsSelected { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string DataType
        {
            get
            {
                switch (this.SqlDbType)
                {
                    case SqlDbType.Char:
                    case SqlDbType.Binary:
                    case SqlDbType.NChar:
                    case SqlDbType.NVarChar:
                    case SqlDbType.VarBinary:
                    case SqlDbType.VarChar:
                        return $"{this.SqlDbType} ({this.MaxLength})";
                    case SqlDbType.Decimal:
                        return $"{this.SqlDbType} ({this.Precision}, {this.Scale})";
                    case SqlDbType.Time:
                    case SqlDbType.DateTime2:
                    case SqlDbType.DateTimeOffset:
                        return $"{this.SqlDbType} ({this.Scale})";
                    default:
                        return $"{this.SqlDbType}";
                }
                return "";
            }
        }
    }
}
