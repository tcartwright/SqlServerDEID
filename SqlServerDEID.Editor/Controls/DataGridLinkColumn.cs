using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerDEID.Editor.Controls
{
    public class DataGridLinkColumn : System.Windows.Forms.DataGridTextBoxColumn
    {
        private DataGrid _dataGrid;
        private readonly string _linkText;
        private int _currentRow = -1;

        public delegate void LinkColumnClickedEventHandler(object sender, DataGridCellButtonClickEventArgs e);

        public event LinkColumnClickedEventHandler LinkColumnClicked;

        public DataGridLinkColumn(DataGrid dataGrid, string linkText)
        {
            this._dataGrid = dataGrid;
            this._linkText = linkText;
            this._dataGrid.MouseMove += new MouseEventHandler(dataGrid_MouseMove);

            this._dataGrid.MouseUp += new MouseEventHandler(dataGrid_MouseUp);
        }

        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Brush backBrush, System.Drawing.Brush foreBrush, bool alignToRight)
        {
            g.FillRectangle(backBrush, bounds);

            //string s = ((DataRowView)source.Current).DataView[rowNum].Row[this.MappingName].ToString();

            Font f = this.DataGridTableStyle.DataGrid.Font;

            Font font = new Font(f.FontFamily.Name, f.Size, (_currentRow == rowNum ? FontStyle.Underline : FontStyle.Regular));

            g.DrawString(_linkText, font, Brushes.Blue, new RectangleF(new PointF(bounds.X, bounds.Y), new SizeF(bounds.Width, bounds.Height)));
        }


        protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
        {
            return;
        }

        private void dataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dataGrid != null)
            {
                _currentRow = -1;

                DataGrid.HitTestInfo hti = _dataGrid.HitTest(e.X, e.Y);

                if (hti.Type == DataGrid.HitTestType.Cell && this.MappingName == this.DataGridTableStyle.GridColumnStyles[hti.Column].MappingName) _currentRow = hti.Row;

                this.Invalidate();
            }
        }

        private void dataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (_dataGrid != null)
            {
                DataGrid.HitTestInfo hti = _dataGrid.HitTest(e.X, e.Y);

                if (hti.Type == DataGrid.HitTestType.Cell && this.MappingName == this.DataGridTableStyle.GridColumnStyles[hti.Column].MappingName)
                {
                    //CurrencyManager cm = (CurrencyManager)_dataGrid.BindingContext[_dataGrid.DataSource, _dataGrid.DataMember];

                    //LinkColumnClicked(((DataRowView)cm.Current).DataView[hti.Row].Row[this.MappingName].ToString());

                    if (LinkColumnClicked != null)
                    {
                        LinkColumnClicked(_dataGrid, new DataGridCellButtonClickEventArgs(hti.Row, hti.Column));
                    }
                }
            }
        }
    }
}
