using SqlServerDEID.Common;
using SqlServerDEID.Common.Globals.Extensions;
using SqlServerDEID.Common.Globals.Models;
using SqlServerDEID.Editor.Properties;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace SqlServerDEID.Editor
{
    public partial class frmTransformTest : Form
    {
        private Database _database;
        private string _query;
        private readonly SqlConnection _connection;
        private readonly string _tableName;
        private frmTransformTest()
        {
            InitializeComponent();
        }

        public frmTransformTest(Database database) : this()
        {
            _database = database;
            txtQuery.Text = _database.Tables.First().GetSelectStatement(padding: "", columnsSeperator: "");
            // we have to use a global table here, as we are spanning connections :|
            _tableName = $"##TransformTest_{Guid.NewGuid():N}";
            _connection = database.GetConnection();
            _connection.Open();
        }

        private void frmTransformTest_Load(object sender, EventArgs e)
        {

        }

        private void btnRunQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtQuery.Text = _database.Tables.First().GetSelectStatement(txtWhereClause.Text, "", "");

                var query = Resources.GetTransformData.Replace("{{QUERY}}", txtQuery.Text).Replace("{{TEMP_TABLE}}", _tableName);
                var tbl = _connection.ExecuteDataTable(query, new[]
                    {
                    new SqlParameter("@offset", SqlDbType.Int) { Value = 0 },
                    new SqlParameter("@rows", SqlDbType.Int) { Value = 500 }
                });
                gridRawData.DataSource = tbl;

                btnRunTransform.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;

                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRunTransform_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                var db = _database.CloneObject<Database>();
                db.Tables.First().Name = _tableName;

                var deid = new DEID(1, 100, 100, false);
                deid.RunTransform(db);

                var tbl = _connection.ExecuteDataTable($"SELECT * FROM [{_tableName}] AS [tt]");
                gridTransformedData.DataSource = tbl;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;

                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void frmTransformTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connection != null)
            {
                _connection.ExecuteNonQuery($"IF OBJECT_ID('tempdb..{_tableName}') IS NOT NULL DROP TABLE {_tableName}; ");
                _connection.Dispose();
            }
        }
    }
}
