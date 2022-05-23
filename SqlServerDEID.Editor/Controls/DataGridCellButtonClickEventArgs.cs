using System;

namespace SqlServerDEID.Editor.Controls
{
    public class DataGridCellButtonClickEventArgs : EventArgs
	{
		private int _row;
		private int _column;

		public DataGridCellButtonClickEventArgs(int row, int column)
		{
			_row = row;
			_column = column;
		}

		public int Row
		{
			get { return _row; }
		}

		public int Column
		{
			get { return _column; }
		}
	}

}
