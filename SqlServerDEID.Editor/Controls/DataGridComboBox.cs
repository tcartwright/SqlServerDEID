using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace SqlServerDEID.Editor.Controls
{
    public class DataGridComboBox : System.Windows.Forms.ComboBox
    {
        private const int WM_KEYUP = 0x101;
        private PropertyInfo _valueMemberPI = null;
        private PropertyInfo _displayMemberPI = null;

        protected override void WndProc(ref System.Windows.Forms.Message theMessage)
        {
            if (theMessage.Msg == WM_KEYUP)
            {
                return;
            }
            base.WndProc(ref theMessage);
        }

        public object FindValueMember(object display)
        {
            var dv = (IEnumerable)DataSource;
            var enumerator = dv.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (_valueMemberPI == null) { _valueMemberPI = item.GetType().GetProperty(this.ValueMember); }
                if (_displayMemberPI == null) { _displayMemberPI = item.GetType().GetProperty(this.DisplayMember); }

                var val = _displayMemberPI.GetValue(item, null);
                if (display.Equals(val))
                {
                    return _valueMemberPI.GetValue(item, null);
                }
            }
            return DBNull.Value;
        }

        public object FindDisplayMember(object value)
        {
            var dv = (IEnumerable)DataSource;
            var enumerator = dv.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (_valueMemberPI == null) { _valueMemberPI = item.GetType().GetProperty(this.ValueMember); }
                if (_displayMemberPI == null) { _displayMemberPI = item.GetType().GetProperty(this.DisplayMember); }

                var val = _valueMemberPI.GetValue(item, null);
                if (value.Equals(val))
                {
                    return _displayMemberPI.GetValue(item, null);
                }
            }
            return DBNull.Value;
        }
    }
}
