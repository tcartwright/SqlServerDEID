using Meziantou.Framework.Win32;
using SqlServerDEID.Common.Globals.Extensions;
using SqlServerDEID.Common.Globals.Models;
using SqlServerDEID.Editor.Controls;
using SqlServerDEID.Editor.Properties;
using Syncfusion.Windows.Forms;
using Syncfusion.WinForms.Core.Utils;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SqlServerDEID.Editor
{
    public partial class frmMain : Form
    {
        private Database _database;
        private readonly StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;
        private readonly SfToolTip _tooltip = new SfToolTip() { AutoPopDelay = 5000 };

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            BindCredentials();
            SetupGrid();
            BindNewDatabase();
            portNumber.Value = GetDefaultSqlPort();
            this.Text = "SQL Server DEID Editor";
        }

        private void BindCredentials()
        {
            var credentials = CredentialManager.EnumerateCredentials()
                .OrderBy(cr => cr.ApplicationName)
                .Where(cr => cr.CredentialType == CredentialType.Generic && !Regex.IsMatch(cr.ApplicationName, "git.*|microsoft.*|onedrive.*|xbox.*", RegexOptions.IgnoreCase))
                .Select(cr => new KeyValuePair<string, string>(cr.ApplicationName, cr.ApplicationName.ToLower()))
                .ToList();
            ddlCredentials.DisplayMember = "Key";
            ddlCredentials.ValueMember = "Value";
            credentials.Insert(0, new KeyValuePair<string, string>("Trusted Connection", ""));

            ddlCredentials.DataSource = credentials;
            ddlCredentials.SelectedIndex = 0;
        }

        private void SetupGrid()
        {
            var transformTypeValues = Enum.GetValues(typeof(TransformType))
                .Cast<TransformType>()
                .Select(tt => new KeyValuePair<string, TransformType>(ProperCase(Enum.GetName(typeof(TransformType), tt)), tt))
                .ToList();

            var integerFormatInfo = new NumberFormatInfo() { NumberDecimalDigits = 0, NumberGroupSizes = new int[] { } };

            //tablesGrid.ResetTableControl();
            tablesGrid.EditMode = EditMode.SingleClick;
            tablesGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            tablesGrid.AutoGenerateColumns = false;
            tablesGrid.AllowDeleting = true;
            tablesGrid.AddNewRowPosition = RowPosition.Bottom;
            tablesGrid.CellButtonClick += TablesGrid_CellButtonClick;
            tablesGrid.RecordDeleting += TablesGrid_RecordDeleting;
            tablesGrid.CurrentCellBeginEdit += TablesGrid_CurrentCellBeginEdit;
            tablesGrid.CellComboBoxSelectionChanged += TablesGrid_CellComboBoxSelectionChanged;
            tablesGrid.ToolTipOpening += Grid_ToolTipOpening;

            tablesGrid.Columns.Add(new GridComboBoxColumn() { MappingName = "Name", HeaderText = "Table Name", ValueMember = "TableName", DisplayMember = "TableName", Width = 300, ShowToolTip = true }); // new GridTextColumn() { MappingName = "Name", HeaderText = "Table Name", AllowEditing = false });
            tablesGrid.Columns.Add(new GridNumericColumn() { MappingName = "Columns.Count", HeaderText = "Columns Count", NumberFormatInfo = integerFormatInfo, AllowEditing = false });
            tablesGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "DisableTriggers", HeaderText = "Disable Triggers" });
            tablesGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "DisableConstraints", HeaderText = "Disable Constraints" });
            tablesGrid.Columns.Add(new GridTextColumn() { MappingName = "PreScript", HeaderText = "Pre Script", ShowHeaderToolTip = true });
            tablesGrid.Columns.Add(new GridTextColumn() { MappingName = "PostScript", HeaderText = "Post Script", ShowHeaderToolTip = true });
            tablesGrid.Columns.Add(new GridNumericColumn() { MappingName = "ScriptTimeout", HeaderText = "Script Timeout", NumberFormatInfo = integerFormatInfo, ShowHeaderToolTip = true });
            tablesGrid.Columns.Add(new GridButtonColumn() { HeaderText = "Test Transform", DefaultButtonText = "Test Transform", AllowDefaultButtonText = true, MappingName = "Name" });

            //columns
            var columnsGrid = new SfDataGrid
            {
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = AutoSizeColumnsMode.Fill,
                EditMode = EditMode.SingleClick,
                AllowDeleting = false,
                AllowSorting = false
            };
            columnsGrid.ToolTipOpening += Grid_ToolTipOpening;
            columnsGrid.DetailsViewExpanding += ColumnsGrid_DetailsViewExpanding;

            columnsGrid.Columns.Add(new GridTextColumn() { MappingName = "Name", HeaderText = "Column Name", AllowEditing = false, Width = 150 });
            columnsGrid.Columns.Add(new GridNumericColumn() { MappingName = "Transforms.Count", HeaderText = "Transforms Count", NumberFormatInfo = integerFormatInfo, AllowEditing = false });
            columnsGrid.Columns.Add(new GridTextColumn() { MappingName = "DataType", HeaderText = "Data Type", AllowEditing = false });
            columnsGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsSelected", HeaderText = "Is Selected", ShowHeaderToolTip = true });
            columnsGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsPk", HeaderText = "Is Pk", AllowEditing = false });
            columnsGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsIdentity", HeaderText = "Is Identity", AllowEditing = false });
            columnsGrid.Columns.Add(new GridCheckBoxColumn() { MappingName = "IsComputed", HeaderText = "Is Computed", AllowEditing = false });
            // this column will not update. for w/e reason

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
            transformsGrid.ToolTipOpening += Grid_ToolTipOpening;

            transformsGrid.Columns.Add(new GridTextColumn() { MappingName = "Transform", HeaderText = "Transform", Width = 300, ShowHeaderToolTip = true });
            transformsGrid.Columns.Add(new GridTextColumn() { MappingName = "WhereClause", HeaderText = "Where Clause", Width = 300, ShowHeaderToolTip = true });
            transformsGrid.Columns.Add(new GridComboBoxColumn() { MappingName = "TransformType", HeaderText = "TransformType", DisplayMember = "Key", ValueMember = "Value", DataSource = transformTypeValues, ShowHeaderToolTip = true });
            transformsGrid.Columns.Add(new GridButtonColumn() { MappingName = "Transform", DefaultButtonText = "Edit Transform", HeaderText = "Edit", AllowDefaultButtonText = true });
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

                            if (!table.Columns.Any(c => c.IsPk))
                            {
                                MessageBox.Show(this, "Transforms cannot be created for tables that do not have primary keys. Either add a primary key for this table or select a different table.", "No PK", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                if (tablesGrid.View.IsAddingNew && tablesGrid.IsAddNewRowIndex(tablesGrid.CurrentCell.RowIndex))
                                {
                                    _database.Tables.Remove(table);
                                    tablesGrid.View.CancelNew();
                                }
                                else if (this.tablesGrid.View.CanCancelEdit)
                                {
                                    tablesGrid.View.CancelEdit();
                                }
                                tablesGrid.View.Refresh();
                                return;
                            }
                            if (tablesGrid.View.IsAddingNew && tablesGrid.IsAddNewRowIndex(tablesGrid.CurrentCell.RowIndex))
                            {
                                tablesGrid.View.CommitNew();
                            }
                        }
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
                if (table.HasTransforms())
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
                && table.HasTransforms()
                && MessageBox.Show(this, "This table has transforms, do you still with to delete it?", "Delete Table", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void TablesGrid_CellButtonClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellButtonClickEventArgs e)
        {
            var table = ((DataRowBase)e.Record).RowData as DatabaseTable;

            if (!table.HasTransforms())
            {
                MessageBox.Show(this, "This table does not have any transforms. You cannot test it.", "Test Transforms", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var database = _database.CloneObject<Database>();
            database.Tables.Clear();
            database.Tables.Add(table);
            database.RemoveNullTransforms();


            var tranformsTestForm = new frmTransformTest(database);
            if (tranformsTestForm.ShowDialog(this) == DialogResult.OK)
            {
                //TODO: is there anything to do here???
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

        private void Grid_ToolTipOpening(object sender, Syncfusion.WinForms.DataGrid.Events.ToolTipOpeningEventArgs e)
        {
            var position = Cursor.Position;
            var popupDelay = 3000;
            e.ToolTipInfo.Items[0].Text = "";

            switch (e.Column.MappingName.ToLower())
            {
                case "isselected":
                    _tooltip.Show("This column determines whether or not the column will appear in the transform document, and also be available in the RowValues object.", position, popupDelay);
                    break;
                case "prescript":
                    _tooltip.Show("This sql script will be run before the table transform is run. \r\nA fully qualified or relative path can be used. \r\n\r\nAll relative paths will be relative to the transform configuration file.", position, popupDelay);
                    break;
                case "postscript":
                    _tooltip.Show("This sql script will be run after the table transform is run. \r\nA fully qualified or relative path can be used. \r\n\r\nAll relative paths will be relative to the transform configuration file.", position, popupDelay);
                    break;
                case "scripttimeout":
                    _tooltip.Show("Gets the wait time (in seconds) before terminating the attempt to execute the PreScript or PostScript and generating an error.", position, popupDelay);
                    break;
                case "transform":
                    _tooltip.Show("The transform can either be a C# expression or a path to a powershell file. If pointing to a powershell file path, then make the path relative to where the transform file will be saved, or use an absolute path.", position, popupDelay);
                    break;
                case "whereclause":
                    _tooltip.Show("The where clause is a standard SQL where clause without the WHERE statement. If there are multiple transforms for a single column then the WHERE clause is required for each one.", position, popupDelay);
                    break;
                case "transformtype":
                    _tooltip.Show("Determines whether or not a C# expression will be used, or a powershell file.", position, popupDelay);
                    break;
                default:
                    break;
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
            try
            {
                ResetData();

                _database = Database.LoadFile(fileName);

                LoadData();

                DisableEnableControls(false);
            }
            catch (Exception ex)
            {
                this.MessageException(ex, "Exception trying to load file:");
            }
        }

        private void DisableEnableControls(bool enabled = false)
        {
            txtServerName.Enabled =
                txtDatabaseName.Enabled =
                btnConnect.Enabled = enabled;

            btnEditScriptImports.Enabled = 
                btnRefreshTables.Enabled = !enabled;
        }

        private void ResetData()
        {
            ////tablesGrid.DataBindings.Clear();
            //tablesGrid.DataSource = null;
            //bindingSourceFormMain.DataSource = typeof(Database); //cant use null for this one... for w/e reason
            ddlCredentials.SelectedIndex = 0;
        }

        private bool LoadData()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DataTable tables;
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    _database.GetMetaData(connection, true);
                    tables = connection.ExecuteDataTable(Resources.GetTableNames.Replace("{{DB_NAME}}", _database.DatabaseName.CleanName()));
                }
                var tablesCombo = (GridComboBoxColumn)tablesGrid.Columns["Name"];
                tablesCombo.DataSource = tables;
                tablesCombo.MappingName = "Name";
                tablesCombo.ValueMember = "TableName";
                tablesCombo.DisplayMember = "TableName";

                tablesGrid.DataSource = _database.Tables;
                bindingSourceFormMain.DataSource = _database;

                ValidateForm();
                return true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageException(ex, "Problem loading data.");
                return false;
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
            try
            {
                Cursor.Current = Cursors.WaitCursor;
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
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageException(ex, "Exception loading file.");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void MessageException(Exception ex, string message, string title = "Exception")
        {
            var msg = $"{message} Exception:\r\n\r\n{ex.Message}";
            MessageBox.Show(this, msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BindNewDatabase();
        }

        private int GetDefaultSqlPort()
        {
            var port = 1433;
            int.TryParse(ConfigurationManager.AppSettings["DefaultPort"], out port);
            return port;
        }

        private void BindNewDatabase()
        {
            _database = new Database();
            _database.Port = GetDefaultSqlPort();
            ResetData();
            tablesGrid.DataSource = _database.Tables;
            bindingSourceFormMain.DataSource = _database;
            DisableEnableControls(true);
            txtServerName.Focus();
            ddlCredentials.SelectedIndex = 0;
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
                MessageException(ex, "Exception saving file.");
            }
        }

        private void ValidateForSave()
        {
            if (!_database.Tables.Any(t => t.HasTransforms()))
            {
                MessageBox.Show(this, "The current database does not have any transforms and cannot be saved.", "No Transforms", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (LoadData())
                {
                    DisableEnableControls(false);
                    MessageBox.Show(this, "Connected", "Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
            return ValidateServerName()
                && ValidateDatabaseName();
        }

        private void btnEditScriptImports_Click(object sender, EventArgs e)
        {
            // https://localcoder.org/is-there-any-way-to-use-a-collectioneditor-outside-of-the-property-grid
            PropertyDescriptor pd = TypeDescriptor.GetProperties(_database)["ScriptingImports"];
            UITypeEditor editor = (UITypeEditor)pd.GetEditor(typeof(UITypeEditor));
            RuntimeServiceProvider serviceProvider = new RuntimeServiceProvider();
            editor.EditValue(serviceProvider, serviceProvider, _database.ScriptingImports);
        }

        private void txtLocale_DoubleClick(object sender, EventArgs e)
        {
            Process.Start("https://github.com/bchavez/Bogus#locales");
        }

        private void label8_DoubleClick(object sender, EventArgs e)
        {
            Process.Start("https://github.com/bchavez/Bogus#api-extension-methods");
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refreshCredentialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindCredentials();
        }

        private void btnRefreshTables_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
