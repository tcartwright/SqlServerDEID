using SqlServerDEID.Common;
using SqlServerDEID.Common.Globals.Extensions;
using SqlServerDEID.Common.Globals.Models;
using SqlServerDEID.Editor.Properties;
using Syncfusion.WinForms.DataGrid;
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
            txtQuery.Text = _database.Tables.First().GetSelectStatement(padding: " ", columnsSeperator: "");
            // we have to use a global table here, as we are spanning connections :|
            _tableName = $"##TransformTest_{Guid.NewGuid():N}";
            this.Text = $"Transform Test ({database.Tables.First().Name} - {_tableName})";
            _connection = database.GetConnection();
            _connection.Open();
            gridRawData.TableControl.Scroll += gridRawData_Scroll;
            gridTransformedData.TableControl.Scroll += gridTransformedData_Scroll;
        }

        private void gridTransformedData_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollGrid(e, gridRawData, gridTransformedData);
        }

        private void gridRawData_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollGrid(e, gridTransformedData, gridRawData);
        }

        private void ScrollGrid(ScrollEventArgs scrollArgs, SfDataGrid leftGrid, SfDataGrid rightGrid)
        {
            if (scrollArgs.OldValue == -1) { return; }
            if (scrollArgs.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                leftGrid.TableControl.HorizontalScroll.Value = scrollArgs.NewValue;
                leftGrid.TableControl.VerticalScroll.Value = rightGrid.TableControl.VerticalScroll.Value;
            }
            else
            {
                leftGrid.TableControl.HorizontalScroll.Value = rightGrid.TableControl.HorizontalScroll.Value;
                leftGrid.TableControl.VerticalScroll.Value = scrollArgs.NewValue;
            }
            leftGrid.TableControl.Refresh();
            rightGrid.TableControl.Refresh();
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
                var topRows = 100;

                var query = Resources.GetTransformData
                    .Replace("{{QUERY}}", txtQuery.Text)
                    .Replace("{{TEMP_TABLE}}", _tableName);
                var tbl = _connection.ExecuteDataTable(query, new[]
                    {
                    new SqlParameter("@offset", SqlDbType.Int) { Value = 0 },
                    new SqlParameter("@rows", SqlDbType.Int) { Value = topRows }
                });
                gridRawData.DataSource = tbl;
                btnRunTransform.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageException(ex, $"Exception running query to generate temp table: {_tableName} from {_database.Tables.First().Name}.");
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

                // sync up the columns width
                for (int i = 0; i <= gridRawData.Columns.Count - 1; i++)
                {
                    gridTransformedData.Columns[i].Width = gridRawData.Columns[i].ActualWidth;
                }
                // now reset the scroll bars
                gridTransformedData.TableControl.HorizontalScroll.Value = 0;
                gridTransformedData.TableControl.VerticalScroll.Value = 0;
                gridTransformedData.TableControl.Refresh();
                gridRawData.TableControl.HorizontalScroll.Value = 0;
                gridRawData.TableControl.VerticalScroll.Value = 0;
                gridRawData.TableControl.Refresh();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageException(ex, $"Exception running transform.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void frmTransformTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_connection != null)
                {
                    _connection.ExecuteNonQuery($"IF OBJECT_ID('tempdb..{_tableName}') IS NOT NULL DROP TABLE {_tableName}; ");
                }
            }
            catch (Exception ex)
            {
                MessageException(ex, $"Unable to drop the temp table: {_tableName}. Please drop it manually.");
            }
            finally
            {
                if (_connection != null) { _connection.Dispose(); }
            }
        }

        private void MessageException(Exception ex, string message, string title = "Exception")
        {
            var msg = $"{message} Exception:\r\n\r\n{ex.GetBaseException().Message}";
            MessageBox.Show(this, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
