using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerDEID.Editor
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadFile(string fileName)
        {
            //var database = Database.LoadFile(fileName);

            //using (var connection = database.GetConnection())
            //{
            //    connection.Open();
            //    database.GetMetaData(connection, true);
            //}

            //bindingSource1.DataSource = database.Tables;
            //gridControl1.PopulateValues(GridRangeInfo.Cells(1, 1, this.gridControl1.RowCount, this.gridControl1.ColCount), database.Tables);
            //sfDataGrid1.DataSource = database.Tables;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadFile(@"..\..\..\..\SqlServerDEID\TestFiles\tclab_savemoney.xml");
        }
    }
}
