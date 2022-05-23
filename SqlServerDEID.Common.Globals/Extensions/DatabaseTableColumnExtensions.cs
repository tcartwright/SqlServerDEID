using Bogus;
using SqlServerDEID.Common.Globals.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
    {
        public static CustomExpandoObject GetRowValues(this List<DatabaseTableColumn> columns, string locale = "en")
        {
            var val = new Dictionary<string, Object>(StringComparer.OrdinalIgnoreCase);
            var faker = new Faker(locale);

            foreach (var column in columns)
            {
                switch (column.SqlDbType)
                {
                    case System.Data.SqlDbType.TinyInt:
                        val.Add(column.CleanName, faker.Random.Byte());
                        break;
                    case System.Data.SqlDbType.SmallInt:
                        val.Add(column.CleanName, faker.Random.Int(Int16.MinValue, Int16.MaxValue));
                        break;
                    case System.Data.SqlDbType.Int:
                        val.Add(column.CleanName, faker.Random.Int());
                        break;
                    case System.Data.SqlDbType.BigInt:
                        val.Add(column.CleanName, faker.Random.Long());
                        break;
                    case System.Data.SqlDbType.Binary:
                    case System.Data.SqlDbType.Image:
                    case System.Data.SqlDbType.VarBinary:
                        val.Add(column.CleanName, faker.Random.Bytes(Math.Min(100, column.MaxLength)));
                        break;
                    case System.Data.SqlDbType.Bit:
                        val.Add(column.CleanName, faker.Random.Bool());
                        break;
                    case System.Data.SqlDbType.Char:
                    case System.Data.SqlDbType.NChar:
                    case System.Data.SqlDbType.NVarChar:
                    case System.Data.SqlDbType.VarChar:
                        var strVal = $"{column.CleanName} VALUE";
                        var maxLen = column.MaxLength == -1 ? strVal.Length : Math.Min(strVal.Length, column.MaxLength);
                        val.Add(column.CleanName, strVal.Substring(0, maxLen));
                        break;
                    case System.Data.SqlDbType.Time:
                        val.Add(column.CleanName, DateTime.Parse(faker.Date.Past(1).ToString("hh:mm:ss")));
                        break;
                    case System.Data.SqlDbType.Date:
                        val.Add(column.CleanName, DateTime.Parse(faker.Date.Past(1).ToString("yyyy-MM-dd ")));
                        break;
                    case System.Data.SqlDbType.DateTime:
                    case System.Data.SqlDbType.DateTime2:
                        val.Add(column.CleanName, faker.Date.Past(1));
                        break;
                    case System.Data.SqlDbType.SmallDateTime:
                        val.Add(column.CleanName, DateTime.Parse(faker.Date.Past(1).ToString("yyyy-MM-dd hh:mm:ss")));
                        break;
                    case System.Data.SqlDbType.DateTimeOffset:
                        val.Add(column.CleanName, faker.Date.PastOffset(1));
                        break;
                    case System.Data.SqlDbType.Decimal:
                    case System.Data.SqlDbType.Float:
                    case System.Data.SqlDbType.Money:
                    case System.Data.SqlDbType.SmallMoney:
                    case System.Data.SqlDbType.Real:
                        val.Add(column.CleanName, faker.Random.Decimal());
                        break;
                    case System.Data.SqlDbType.UniqueIdentifier:
                        val.Add(column.CleanName, faker.Random.Guid());
                        break;
                    case System.Data.SqlDbType.Xml:
                        val.Add(column.CleanName, "<fake></fake>");
                        break;
                    default:
                        val.Add(column.CleanName, $"invalid data type: ({column.DataType})");
                        break;
                }

            }

            return new CustomExpandoObject(val);
        }
    }
}
