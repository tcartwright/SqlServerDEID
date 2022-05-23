using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SqlServerDEID.Editor.Controls
{
    public class DataGridComboBox : System.Windows.Forms.ComboBox
    {
        private const int WM_KEYUP = 0x101;
        private StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;

        protected override void WndProc(ref System.Windows.Forms.Message theMessage)
        {
            if (theMessage.Msg == WM_KEYUP)
            {
                return;
            }
            base.WndProc(ref theMessage);
        }

        public object FindValueMember(object value)
        {
            return FindTheValue(value, false);
        }

        public object FindDisplayMember(object value)
        {
            return FindTheValue(value, true);
        }

        private object FindTheValue(object value, bool testWithValue)
        {
            PropertyInfo valueMemberPI = null;
            PropertyInfo displayMemberPI = null;

            var dv = (IEnumerable)DataSource;
            var enumerator = dv.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (valueMemberPI == null) { valueMemberPI = item.GetType().GetProperty(this.ValueMember); }
                if (displayMemberPI == null) { displayMemberPI = item.GetType().GetProperty(this.DisplayMember); }

                var val = testWithValue ? valueMemberPI.GetValue(item, null) : displayMemberPI.GetValue(item, null);
                if (value.Equals(val) || _stringComparer.Equals(value, val))
                {
                    return !testWithValue ? valueMemberPI.GetValue(item, null) : displayMemberPI.GetValue(item, null);
                }
            }
            if (valueMemberPI != null)
            {
                return !testWithValue ? GetDefault(valueMemberPI.GetType()) : GetDefault(displayMemberPI.GetType());
            }
            else
            {
                return DBNull.Value;
            }
        }

        public object GetDefault(Type type)
        {
            return this.GetType()
                .GetMethod("GetDefaultGeneric")
                .MakeGenericMethod(type)
                .Invoke(this, null);
        }

        public T GetDefaultGeneric<T>()
        {
            return default(T);
        }
    }
}
