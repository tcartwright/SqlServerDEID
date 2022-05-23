using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace SqlServerDEID.Editor.Controls
{
    public class DataGridComboBoxColumn : DataGridTextBoxColumn
    {
        private DataGridComboBox _comboBox;
        private CurrencyManager _source;
        private int _rowNumber;
        private bool _editing;

        public DataGridComboBoxColumn(IEnumerable dataSource, string valueMember, string displayMember, ComboBoxStyle comboBoxStyle = ComboBoxStyle.DropDownList)
        {
            _source = null;
            _editing = false;

            _comboBox = new DataGridComboBox
            {
                DropDownStyle = comboBoxStyle,
                Visible = false,
                DataSource = dataSource,
                DisplayMember = displayMember,
                ValueMember = valueMember
            };

            _comboBox.Leave += new EventHandler(_comboBox_Leave);
            _comboBox.SelectionChangeCommitted += new EventHandler(_comboBox_SelectionChangeCommitted);
        }

        public DataGridComboBox ComboBox
        {
            get
            {
                return _comboBox;
            }
        }

        private void _comboBox_Leave(object sender, EventArgs e)
        {
            if (_editing)
            {
                _editing = false;
                SetColumnValueAtRow(_source, _rowNumber, _comboBox.Text);
                Invalidate();
            }
            _comboBox.Visible = false;
            DataGridTableStyle.DataGrid.Scroll += new EventHandler(DataGrid_Scroll);
        }
         
        private void DataGrid_Scroll(object sender, EventArgs e)
        {
            if (_comboBox.Visible)
                _comboBox.Visible = false;
        }

        private void _comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _editing = true;
            base.ColumnStartedEditing((Control)sender);
        }

        protected override int GetMinimumHeight()
        {
            return _comboBox.PreferredHeight;
        }

        protected override void SetDataGridInColumn(DataGrid value)
        {
            base.SetDataGridInColumn(value);
            _comboBox.Parent = (Control)value;
        }

        protected override void ConcedeFocus()
        {
            base.ConcedeFocus();
            _comboBox.Visible = false;
        }

        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
        {
            base.Edit(source, rowNum, bounds, readOnly, instantText, cellIsVisible);
            this.TextBox.Visible = false;

            _rowNumber = rowNum;
            _source = source;

            _comboBox.Bounds = bounds;
            _comboBox.RightToLeft = this.DataGridTableStyle.DataGrid.RightToLeft;
            if (cellIsVisible && !readOnly)
            {
                _comboBox.Visible = true;
                _comboBox.BringToFront();
                _comboBox.Focus();
            }

            _comboBox.SelectedIndex = _comboBox.FindStringExact(this.TextBox.Text);

            DataGridTableStyle.DataGrid.Scroll += new EventHandler(DataGrid_Scroll);
        }

        protected override bool Commit(CurrencyManager dataSource, int rowNum)
        {
            if (_editing)
            {
                _editing = false;
                SetColumnValueAtRow(dataSource, rowNum, _comboBox.Text);
            }

            return true;
        }

        protected override void SetColumnValueAtRow(CurrencyManager source, int rowNum, object value)
        {
            base.SetColumnValueAtRow(source, rowNum, _comboBox.FindValueMember(value));
        }

        protected override object GetColumnValueAtRow(CurrencyManager source, int rowNum)
        {
            object val = base.GetColumnValueAtRow(source, rowNum);
            return _comboBox.FindDisplayMember(val);
        }

    }
}
