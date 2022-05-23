using System;
using System.Drawing;
using System.Windows.Forms;

namespace SqlServerDEID.Editor.Controls
{
    public delegate void DataGridCellButtonClickEventHandler(object sender, DataGridCellButtonClickEventArgs e);

    public class DataGridButtonColumn : DataGridTextBoxColumn
    {
        private DataGridButton _button;
        private int _rowNum;
        private CurrencyManager _source;
        private DataGrid _dataGrid;

        public event DataGridCellButtonClickEventHandler CellButtonClicked;

        public DataGridButtonColumn(string buttonText)
        {
            _button = new DataGridButton()
            {
                Text = buttonText
            };
            
            _button.Click += _button_Click;
        }

        private void _button_Click(object sender, EventArgs e)
        {
            var hti = _dataGrid.HitTest(new Point(_button.Location.X, _button.Location.Y));

            if (hti.Column > -1 && hti.Row > -1 && CellButtonClicked != null)
            {
                CellButtonClicked(_dataGrid, new DataGridCellButtonClickEventArgs(hti.Row, hti.Column));
            }
        }

        public DataGridButton Button { get { return _button; } }

        protected override int GetMinimumHeight()
        {
            return _button.Size.Height;
        }

        protected override Size GetPreferredSize(Graphics g, object value)
        {
            return _button.Size;
        }

        protected override void SetDataGridInColumn(DataGrid value)
        {
            base.SetDataGridInColumn(value);
            _button.Parent = (Control)value;
            _dataGrid = value;
        }

        protected override void ConcedeFocus()
        {
            base.ConcedeFocus();
            _button.Visible = false;
        }

        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
        {
            base.Edit(source, rowNum, bounds, readOnly, instantText, cellIsVisible);
            this.TextBox.Visible = false;
            _rowNum = rowNum;
            _source = source;

            _button.Bounds = bounds;

            if (cellIsVisible && !readOnly)
            {
                _button.Visible = true;
                _button.BringToFront();
                _button.Focus();
            }

            DataGridTableStyle.DataGrid.Scroll += new EventHandler(DataGrid_Scroll);
        }

        private void DataGrid_Scroll(object sender, EventArgs e)
        {
            if (_button.Visible) { _button.Visible = false; }
        }

        protected override bool Commit(CurrencyManager dataSource, int rowNum)
        {
            //return base.Commit(dataSource, rowNum);
            return true;
        }
        protected override void SetColumnValueAtRow(CurrencyManager source, int rowNum, object value)
        {
            base.SetColumnValueAtRow(source, rowNum, value);
        }
        protected override object GetColumnValueAtRow(CurrencyManager source, int rowNum)
        {
            return base.GetColumnValueAtRow(source, rowNum);
        }
    }

    public class DataGridButton : Button
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
    }

}
