using SqlServerDEID.Common.Globals.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerDEID.Editor
{
    public partial class frmMain : Form
    {
        private DataGridTableStyle _columnsStyle;
        private DataGridTableStyle _transformsStyle;
        private readonly StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var database = Database.LoadFile(@"C:\Users\tim.cartwright.VGNET\source\repos\SqlServerDEID\SqlServerDEID\TestFiles\Transform.xml");

            var ds = database.ToDataSet();

            dataGrid1.DataSource = ds.Tables["Tables"];
            dataGrid1.ReadOnly = false;

            ds.Tables["Tables"].RowDeleted += FrmMain_RowDeleted;
            ds.Tables["Tables"].RowChanged += FrmMain_RowChanged;

            ds.Tables["Transforms"].RowDeleted += FrmMain_RowDeleted;
            ds.Tables["Transforms"].RowChanged += FrmMain_RowChanged;

            // TABLE STYLES
            var style = NewStyle("Tables");
            var colStyle = style.GridColumnStyles["TableName"];
            colStyle.HeaderText = "Table Name";
            colStyle.ReadOnly = false;

            colStyle = style.GridColumnStyles["DisableTriggers"];
            colStyle.HeaderText = "Disable Triggers";

            colStyle = style.GridColumnStyles["DisableConstraints"];
            colStyle.HeaderText = "Disable Constraints";

            colStyle = style.GridColumnStyles["ScriptTimeout"];
            colStyle.HeaderText = "Script Timeout";

            colStyle = style.GridColumnStyles["ColumnsCount"];
            colStyle.HeaderText = "Columns Count";
            colStyle.ReadOnly = true;
            colStyle.Width = 100;

        }

        private void FrmMain_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            Console.WriteLine(e);
        }

        private void FrmMain_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            Console.WriteLine(e);
        }

        private DataGridTableStyle NewStyle(string mappingName)
        {
            var style = new DataGridTableStyle();
            style.MappingName = mappingName;
            dataGrid1.TableStyles.Add(style);
            return style;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(dataGrid1.DataMember);
        }

        private void dataGrid1_Navigate(object sender, NavigateEventArgs ne)
        {
            dataGrid1.ReadOnly = _stringComparer.Equals(dataGrid1.DataMember, "Columns");

            if (ne.Forward)
            {
                if (_stringComparer.Equals(dataGrid1.DataMember, "Columns") && _columnsStyle is null)
                {
                    // COLUMN STYLES
                    _columnsStyle = NewStyle("Columns");
                    var colStyle = _columnsStyle.GridColumnStyles["TableName"];
                    colStyle.Width = 0;

                    colStyle = _columnsStyle.GridColumnStyles["ColumnName"];
                    colStyle.HeaderText = "Column Name";
                    colStyle.ReadOnly = true;
                    colStyle.Width = 100;

                    colStyle = _columnsStyle.GridColumnStyles["Datatype"];
                    colStyle.HeaderText = "Data type";
                    colStyle.ReadOnly = true;

                    colStyle = _columnsStyle.GridColumnStyles["Datatype"];
                    colStyle.HeaderText = "Data type";
                    colStyle.ReadOnly = true;

                    colStyle = _columnsStyle.GridColumnStyles["IsPK"];
                    colStyle.HeaderText = "Is PK?";
                    colStyle.ReadOnly = true;

                    colStyle = _columnsStyle.GridColumnStyles["IsIdentity"];
                    colStyle.HeaderText = "Is Identity?";
                    colStyle.ReadOnly = true;

                    colStyle = _columnsStyle.GridColumnStyles["IsComputed"];
                    colStyle.HeaderText = "Is Comnputed?";
                    colStyle.ReadOnly = true;

                    colStyle = _columnsStyle.GridColumnStyles["TransformsCount"];
                    colStyle.HeaderText = "Transforms Count";
                    colStyle.ReadOnly = true;
                    colStyle.Width = 100;
                }
                else if (_stringComparer.Equals(dataGrid1.DataMember, "Columns.Transforms") && _transformsStyle is null)
                {
                    // TRANSFORM STYLES
                    _transformsStyle = NewStyle("Transforms");
                    var colStyle = _transformsStyle.GridColumnStyles["TableName"];
                    colStyle.Width = 0;

                    colStyle = _transformsStyle.GridColumnStyles["ColumnName"];
                    colStyle.Width = 0;

                    colStyle = _transformsStyle.GridColumnStyles["Transform"];
                    colStyle.Width = 320;

                    var values = Enum.GetValues(typeof(TransformType))
                        .Cast<int>()
                        .Select(e => Tuple.Create(e, Enum.GetName(typeof(TransformType), e)))
                        .ToList();

                    if (_transformsStyle.GridColumnStyles.Contains("Type"))
                    {
                        _transformsStyle.GridColumnStyles.Remove(_transformsStyle.GridColumnStyles["Type"]);
                    }

                    var dgCbo = new Controls.DataGridComboBoxColumn(values, "Item1", "Item2")
                    {
                        HeaderText = "Type",
                        MappingName = "Type"
                    };
                    _transformsStyle.GridColumnStyles.Add(dgCbo);

                    colStyle = _transformsStyle.GridColumnStyles["WhereClause"];
                    colStyle.HeaderText = "Where Clause";
                    colStyle.Width = 320;
                }
            }
        }
    }
}
