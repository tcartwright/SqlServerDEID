using Meziantou.Framework.Win32;
using SqlServerDEID.Common.Globals.Extensions;
using SqlServerDEID.Common.Globals.Models;
using SqlServerDEID.Editor.Controls;
using SqlServerDEID.Editor.Properties;
using Syncfusion.WinForms.Core.Utils;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlServerDEID.Editor
{
    public partial class frmMain : Form
    {
        private Database _database;
        private GridComboBoxColumn _tablesColumn;
        private readonly StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var credentials = CredentialManager.EnumerateCredentials()
                .OrderBy(cr => cr.ApplicationName)
                .Where(cr => cr.CredentialType == CredentialType.Generic && !Regex.IsMatch(cr.ApplicationName, "git.*|microsoft.*|onedrive.*", RegexOptions.IgnoreCase))
                .Select(cr => Tuple.Create(cr.ApplicationName, cr.ApplicationName))
                .ToList();
            ddlCredentials.DisplayMember = "Item1";
            ddlCredentials.ValueMember = "Item2";
            credentials.Insert(0, new Tuple<string, string>("Trusted Connection", ""));

            ddlCredentials.DataSource = credentials;
            ddlCredentials.SelectedText = "";
            SetupGrid();
            SetNewDatabase();
        }

        private void SetupGrid()
        {
            var transformTypeValues = Enum.GetValues(typeof(TransformType))
                .Cast<TransformType>()
                .Select(tt => Tuple.Create(tt, ProperCase(Enum.GetName(typeof(TransformType), tt))))
                .ToList();

            var integerFormatInfo = new NumberFormatInfo() { NumberDecimalDigits = 0, NumberGroupSizes = new int[] { } };

            tablesGrid.EditMode = EditMode.SingleClick;
            tablesGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            tablesGrid.AutoGenerateColumns = false;
            tablesGrid.AllowDeleting = true;
            tablesGrid.AddNewRowPosition = RowPosition.Bottom;
            tablesGrid.CellButtonClick += TablesGrid_CellButtonClick;
            tablesGrid.RecordDeleting += TablesGrid_RecordDeleting;
            tablesGrid.CurrentCellBeginEdit += TablesGrid_CurrentCellBeginEdit;
            tablesGrid.CellComboBoxSelectionChanged += TablesGrid_CellComboBoxSelectionChanged;

            _tablesColumn = new GridComboBoxColumn() { MappingName = "Name", HeaderText = "Table Name", ValueMember = "TableName", DisplayMember = "TableName", Width = 300 };
            tablesGrid.Columns.Add(_tablesColumn); // new GridTextColumn() { MappingName = "Name", HeaderText = "Table Name", AllowEditing = false });
            tablesGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "DisableTriggers", HeaderText = "Disable Triggers" });
            tablesGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "DisableConstraints", HeaderText = "Disable Constraints" });
            tablesGrid.Columns.Add(new GridTextColumn() { MappingName = "PreScript", HeaderText = "Pre Script" });
            tablesGrid.Columns.Add(new GridTextColumn() { MappingName = "PostScript", HeaderText = "Post Script" });
            tablesGrid.Columns.Add(new GridNumericColumn() { MappingName = "ScriptTimeout", HeaderText = "Script Timeout", NumberFormatInfo = integerFormatInfo });
            tablesGrid.Columns.Add(new GridButtonColumn() { HeaderText = "Test Transform", DefaultButtonText = "Test Transform", AllowDefaultButtonText = true, MappingName = "Name" });

            //columns
            var columnsGrid = new SfDataGrid
            {
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = AutoSizeColumnsMode.Fill,
                EditMode = EditMode.SingleClick,
                AllowDeleting = false
            };
            columnsGrid.ToolTipOpening += ColumnsGrid_ToolTipOpening;
            columnsGrid.DetailsViewExpanding += ColumnsGrid_DetailsViewExpanding;

            columnsGrid.Columns.Add(new GridTextColumn() { MappingName = "Name", HeaderText = "Column Name", AllowEditing = false, Width = 150 });
            columnsGrid.Columns.Add(new GridTextColumn() { MappingName = "DataType", HeaderText = "Data Type", AllowEditing = false });
            columnsGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsSelected", HeaderText = "Is Selected", ShowHeaderToolTip = true });
            columnsGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsPk", HeaderText = "Is Pk", AllowEditing = false });
            columnsGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsIdentity", HeaderText = "Is Identity", AllowEditing = false });
            columnsGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsComputed", HeaderText = "Is Computed", AllowEditing = false });

            tablesGrid.DetailsViewDefinitions.Add(new GridViewDefinition
            {
                RelationalColumn = "Columns",
                DataGrid = columnsGrid
            });

            // tranforms
            var transformsGrid = new SfDataGrid
            {
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = AutoSizeColumnsMode.Fill,
                AllowResizingColumns = true,
                EditMode = EditMode.SingleClick,
                AddNewRowPosition = RowPosition.Bottom,
                AllowDeleting = true
            };
            transformsGrid.CellButtonClick += TransformsChildGrid_CellButtonClick;
            transformsGrid.CellComboBoxSelectionChanged += TransformsGrid_CellComboBoxSelectionChanged;

            transformsGrid.Columns.Add(new GridTextColumn() { MappingName = "Transform", HeaderText = "Transform", Width = 300 });
            transformsGrid.Columns.Add(new GridTextColumn() { MappingName = "WhereClause", HeaderText = "Where Clause", Width = 300 });
            transformsGrid.Columns.Add(new GridComboBoxColumn() { MappingName = "TransformType", HeaderText = "TransformType", ValueMember = "Item1", DisplayMember = "Item2", DataSource = transformTypeValues });
            transformsGrid.Columns.Add(new GridButtonColumn() { MappingName = "Transform", DefaultButtonText = "Edit", HeaderText = "Edit", AllowDefaultButtonText = true });
            columnsGrid.DetailsViewDefinitions.Add(new GridViewDefinition
            {
                RelationalColumn = "Transforms",
                DataGrid = transformsGrid
            });
        }

        private void TablesGrid_CellComboBoxSelectionChanged(object sender, CellComboBoxSelectionChangedEventArgs e)
        {
            if (_stringComparer.Equals(e.GridColumn.MappingName, "Name"))
            {
                var table = e.Record as DatabaseTable;
                var selection = Convert.ToString(((DataRowView)e.SelectedItem).Row.ItemArray.FirstOrDefault());

                if (String.IsNullOrWhiteSpace(table.Name) || !_stringComparer.Equals(table.Name, selection))
                {
                    try
                    {
                        table.Name = selection;
                        using (var connection = _database.GetConnection())
                        {
                            connection.Open();
                            table.Reset();
                            table.GetMetaData(connection, true);
                        }
                        // cant seem to get the +/- to show up when the row is added
                        //tablesGrid.Invalidate(true);
                        //tablesGrid.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageException(ex, "Problem loading data.");
                    }
                }
            }
        }

        private void TablesGrid_CurrentCellBeginEdit(object sender, CurrentCellBeginEditEventArgs e)
        {
            if (_stringComparer.Equals(e.DataColumn.GridColumn.MappingName, "Name"))
            {
                var table = e.DataRow.RowData as DatabaseTable;
                if (table.Columns.Any(c => c.Transforms.Any()))
                {
                    e.Cancel = true;
                    MessageBox.Show(this, "The table cannot be changed once it has column transforms assigned.", "Edit Table", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TablesGrid_RecordDeleting(object sender, Syncfusion.WinForms.DataGrid.Events.RecordDeletingEventArgs e)
        {
            var table = tablesGrid.CurrentItem as DatabaseTable;
            if (table != null
                && table.Columns.Any(c => c.Transforms.Any())
                && MessageBox.Show(this, "This table has transforms, do you still with to delete it?", "Delete Table", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void TablesGrid_CellButtonClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellButtonClickEventArgs e)
        {
            var table = ((DataRowBase)e.Record).RowData as DatabaseTable;
            var tranformsTestForm = new frmTransformTest();
            if (tranformsTestForm.ShowDialog(this) == DialogResult.OK)
            {
                //TODO: ???
            }
        }

        private void ColumnsGrid_DetailsViewExpanding(object sender, Syncfusion.WinForms.DataGrid.Events.DetailsViewExpandingEventArgs e)
        {
            //var grid = e.OriginalSender as SfDataGrid;
            var column = e.Record as DatabaseTableColumn;
            if (column.IsPk || column.IsComputed || column.IsIdentity)
            {
                MessageBox.Show(this, "Transforms cannot be added to a Primary Key, Identity, or Computed column.", "Transforms", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        private void ColumnsGrid_ToolTipOpening(object sender, Syncfusion.WinForms.DataGrid.Events.ToolTipOpeningEventArgs e)
        {
            if (_stringComparer.Equals("IsSelected", e.Column.MappingName))
            {
                e.ToolTipInfo.Items[0].Text = "This column determines whether or not the column will be in the transform document, and also be available in the RowValues object.";
            }
        }

        private void TransformsGrid_CellComboBoxSelectionChanged(object sender, CellComboBoxSelectionChangedEventArgs e)
        {
            var transform = e.Record as DatabaseTableColumnTransform;
            if (!string.IsNullOrWhiteSpace(transform.Transform))
            {
                MessageBox.Show(this, "Changing the transform type clears the transform.", "Transform Type Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            transform.Transform = null;
        }

        private void TransformsChildGrid_CellButtonClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellButtonClickEventArgs e)
        {
            var row = e.Record as DataRowBase;
            var data = row.RowData as DatabaseTableColumnTransform;

            if (data.TransformType == TransformType.expression)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    var grid = ReflectionHelper.GetProperty(row.GetType(), "DataGrid").GetValue(row, null) as SfDataGrid;
                    var columnsGrid = grid.NotifyListener.GetParentDataGrid();
                    var tablesGrid = columnsGrid.NotifyListener.GetParentDataGrid();

                    var columns = columnsGrid.DataSource as IList<DatabaseTableColumn>;
                    var selectedColumns = columns.Where(c => c.IsSelected).ToList();

                    dynamic rowValues = selectedColumns.GetRowValues(_database.Locale);

                    var scriptGlobals = new ScriptGlobals(_database.Locale)
                    {
                        RowValues = rowValues,
                        DatabaseName = _database.DatabaseName,
                        TableName = (tablesGrid.CurrentItem as DatabaseTable)?.Name,
                        Column = (columnsGrid.CurrentItem as DatabaseTableColumn)?.ToColumnInfo(),
                    };

                    var editorForm = new frmCodeEditor(scriptGlobals, data.Transform);
                    if (editorForm.ShowDialog(this) == DialogResult.OK)
                    {
                        data.Transform = editorForm.Transform;
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show(this, "Only transforms of type expression can be edited.", "Edit Transform", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadFile(string fileName)
        {
            _database = Database.LoadFile(fileName);

            LoadData();

            DisableEnableControls(false);
        }

        private void DisableEnableControls(bool enabled = false)
        {
            txtServerName.Enabled =
                txtDatabaseName.Enabled =
                btnConnect.Enabled = enabled;
        }

        private void ResetData()
        {
            _tablesColumn.DataSource = null;
            tablesGrid.DataSource = null;
            bindingSource1.DataSource = typeof(Database);
            ddlCredentials.SelectedText = "";
        }

        private void LoadData()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetData();
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    _database.GetMetaData(connection, true);
                    _tablesColumn.DataSource = connection.ExecuteDataTable(Resources.GetTableNames.Replace("{{DB_NAME}}", _database.DatabaseName.CleanName()));
                }

                tablesGrid.DataSource = _database.Tables;
                bindingSource1.DataSource = _database;
                ValidateForm();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageException(ex, "Problem loading data.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private string ProperCase(string value)
        {
            return $"{Char.ToUpper(value[0])}{value.Substring(1)}";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Transform Files (*.xml, *.json)|*.xml; *.json|All files (*.*)|*.*";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadFile(dialog.FileName);
                }
            }
        }

        private void MessageException(Exception ex, string message, string title = "Exception")
        {
            var msg = $"{message} Exception:\r\n\r\n{ex.Message}";
            MessageBox.Show(this, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetNewDatabase();
        }

        private void SetNewDatabase()
        {
            _database = new Database();
            ResetData();
            tablesGrid.DataSource = _database.Tables;
            bindingSource1.DataSource = _database;
            DisableEnableControls(true);
            txtServerName.Focus();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_database != null)
            {
                ValidateForSave();

                if (!string.IsNullOrWhiteSpace(_database.TransformFilePath))
                {
                    SaveFile();
                }
                else
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_database != null)
            {
                ValidateForSave();

                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "Transform Files (*.xml, *.json)|*.xml; *.json|All files (*.*)|*.*";
                    dialog.RestoreDirectory = true;
                    dialog.FileName = _database.TransformFilePath;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _database.TransformFilePath = dialog.FileName;
                        SaveFile();
                    }
                }
            }
        }

        private void SaveFile()
        {
            var db = _database.CloneObject<Database>();
            db.RemoveNullTransforms();

            try
            {
                var ext = Path.GetExtension(db.TransformFilePath).ToLower();

                if (_stringComparer.Equals(ext, ".xml"))
                {
                    File.WriteAllText(db.TransformFilePath, db.ToXml());
                }
                else if (_stringComparer.Equals(ext, ".json"))
                {
                    File.WriteAllText(db.TransformFilePath, db.ToJson());
                }
            }
            catch (Exception ex)
            {
                MessageException(ex, "Exception saving file");
            }
        }

        private void ValidateForSave()
        {
            if (!_database.Tables.Any(t => t.Columns.Any(c => c.Transforms.Any())))
            {
                MessageBox.Show(this, "The current database does not have any transforms and cannot be saved.", "No Transforms", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                LoadData();
            }
        }

        private void txtServerName_Validating(object sender, CancelEventArgs e)
        {
            ValidateServerName();
        }

        private void txtDatabaseName_Validating(object sender, CancelEventArgs e)
        {
            ValidateDatabaseName();
        }

        private bool ValidateServerName()
        {
            if (string.IsNullOrWhiteSpace(txtServerName.Text))
            {
                errorProvider1.SetError(txtServerName, "ServerName is required");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtServerName, "");
                return true;
            }
        }
        private bool ValidateDatabaseName()
        {
            if (string.IsNullOrWhiteSpace(txtDatabaseName.Text))
            {
                errorProvider1.SetError(txtDatabaseName, "Database name is required");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtDatabaseName, "");
                return true;
            }
        }

        private bool ValidateForm()
        {
            return ValidateServerName() && ValidateDatabaseName();
        }
    }
}
