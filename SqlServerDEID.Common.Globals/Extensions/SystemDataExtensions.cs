using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace SqlServerDEID.Common.Globals.Extensions
{
    public static partial class Extensions
    {

        /// <summary>
        /// https://csharp-extension.com/en/method/1002764/datarow-toexpandoobject
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static dynamic ToExpandoObject(this DataRow row)
        {
            if (row is null) { throw new ArgumentNullException(nameof(row)); }

            dynamic entity = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)entity;

            foreach (DataColumn column in row.Table.Columns)
            {
                expandoDict.Add(column.ColumnName, row[column.ColumnName]);
            }

            return expandoDict;
        }
        public static dynamic ToExpandoObject(this DataRowView rowView)
        {
            if (rowView is null) { throw new ArgumentNullException(nameof(rowView)); }

            dynamic entity = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)entity;

            foreach (DataColumn column in rowView.DataView.Table.Columns)
            {
                expandoDict.Add(column.ColumnName, rowView[column.ColumnName]);
            }

            return expandoDict;
        }
    }
}
