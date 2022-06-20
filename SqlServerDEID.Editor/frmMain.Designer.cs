namespace SqlServerDEID.Editor
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshCredentialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tablesGrid = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.scriptTimeout = new System.Windows.Forms.NumericUpDown();
            this.bindingSourceFormMain = new System.Windows.Forms.BindingSource(this.components);
            this.btnEditScriptImports = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPostScript = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPreScript = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ddlCredentials = new System.Windows.Forms.ComboBox();
            this.portNumber = new System.Windows.Forms.NumericUpDown();
            this.txtLocale = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablesGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scriptTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFormMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portNumber)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1051, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.refreshCredentialsToolStripMenuItem,
            this.refreshTablesToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem1.Text = "&New";
            this.newToolStripMenuItem1.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // refreshCredentialsToolStripMenuItem
            // 
            this.refreshCredentialsToolStripMenuItem.Name = "refreshCredentialsToolStripMenuItem";
            this.refreshCredentialsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.refreshCredentialsToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.refreshCredentialsToolStripMenuItem.Text = "Refresh &Credentials";
            this.refreshCredentialsToolStripMenuItem.Click += new System.EventHandler(this.refreshCredentialsToolStripMenuItem_Click);
            // 
            // refreshTablesToolStripMenuItem
            // 
            this.refreshTablesToolStripMenuItem.Name = "refreshTablesToolStripMenuItem";
            this.refreshTablesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.refreshTablesToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.refreshTablesToolStripMenuItem.Text = "Refresh &Tables";
            this.refreshTablesToolStripMenuItem.Click += new System.EventHandler(this.refreshTablesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            // 
            // saveFileToolStripMenuItem
            // 
            this.saveFileToolStripMenuItem.Name = "saveFileToolStripMenuItem";
            this.saveFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveFileToolStripMenuItem.Text = "Save File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // tablesGrid
            // 
            this.tablesGrid.AccessibleName = "Table";
            this.tablesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablesGrid.Location = new System.Drawing.Point(12, 173);
            this.tablesGrid.Name = "tablesGrid";
            this.tablesGrid.Size = new System.Drawing.Size(1027, 500);
            this.tablesGrid.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 140);
            this.panel1.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.scriptTimeout);
            this.groupBox1.Controls.Add(this.btnEditScriptImports);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtPostScript);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtPreScript);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ddlCredentials);
            this.groupBox1.Controls.Add(this.portNumber);
            this.groupBox1.Controls.Add(this.txtLocale);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(330, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(694, 131);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(347, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Script Timeout";
            // 
            // scriptTimeout
            // 
            this.scriptTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSourceFormMain, "ScriptTimeout", true));
            this.scriptTimeout.Location = new System.Drawing.Point(425, 76);
            this.scriptTimeout.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.scriptTimeout.Name = "scriptTimeout";
            this.scriptTimeout.Size = new System.Drawing.Size(242, 20);
            this.scriptTimeout.TabIndex = 23;
            this.toolTip1.SetToolTip(this.scriptTimeout, "Gets the wait time (in seconds) before terminating the attempt to execute the Pre" +
        "Script or PostScript and generating an error.");
            // 
            // bindingSourceFormMain
            // 
            this.bindingSourceFormMain.DataSource = typeof(SqlServerDEID.Common.Globals.Models.Database);
            // 
            // btnEditScriptImports
            // 
            this.btnEditScriptImports.Enabled = false;
            this.btnEditScriptImports.Location = new System.Drawing.Point(425, 101);
            this.btnEditScriptImports.Name = "btnEditScriptImports";
            this.btnEditScriptImports.Size = new System.Drawing.Size(115, 20);
            this.btnEditScriptImports.TabIndex = 22;
            this.btnEditScriptImports.Text = "Edit";
            this.btnEditScriptImports.UseVisualStyleBackColor = true;
            this.btnEditScriptImports.Click += new System.EventHandler(this.btnEditScriptImports_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(351, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Script Imports";
            this.toolTip1.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
            this.label8.DoubleClick += new System.EventHandler(this.label8_DoubleClick);
            // 
            // txtPostScript
            // 
            this.txtPostScript.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFormMain, "PostScript", true));
            this.txtPostScript.Location = new System.Drawing.Point(425, 51);
            this.txtPostScript.Name = "txtPostScript";
            this.txtPostScript.Size = new System.Drawing.Size(242, 20);
            this.txtPostScript.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtPostScript, "This sql script will be run after all table transforms are run. \r\nA fully qualifi" +
        "ed or relative path can be used. \r\n\r\nAll relative paths will be relative to the " +
        "transform configuration file.");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(364, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Post Script";
            // 
            // txtPreScript
            // 
            this.txtPreScript.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFormMain, "PreScript", true));
            this.txtPreScript.Location = new System.Drawing.Point(425, 26);
            this.txtPreScript.Name = "txtPreScript";
            this.txtPreScript.Size = new System.Drawing.Size(242, 20);
            this.txtPreScript.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtPreScript, "This sql script will be run before all table transforms are run. \r\nA fully qualif" +
        "ied or relative path can be used. \r\n\r\nAll relative paths will be relative to the" +
        " transform configuration file. ");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(369, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Pre Script";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Credentials";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(66, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Port";
            // 
            // ddlCredentials
            // 
            this.ddlCredentials.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceFormMain, "CredentialsName", true));
            this.ddlCredentials.FormattingEnabled = true;
            this.ddlCredentials.Location = new System.Drawing.Point(92, 26);
            this.ddlCredentials.Name = "ddlCredentials";
            this.ddlCredentials.Size = new System.Drawing.Size(242, 21);
            this.ddlCredentials.TabIndex = 0;
            this.toolTip1.SetToolTip(this.ddlCredentials, "A generic credential must be created in the Credential Manager to be available in" +
        " this dropdown.\r\n Use \"Trusted Connection\" to connect as the currently logged on" +
        " user.");
            // 
            // portNumber
            // 
            this.portNumber.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSourceFormMain, "Port", true));
            this.portNumber.Location = new System.Drawing.Point(93, 51);
            this.portNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portNumber.Name = "portNumber";
            this.portNumber.Size = new System.Drawing.Size(242, 20);
            this.portNumber.TabIndex = 1;
            this.toolTip1.SetToolTip(this.portNumber, "The port to connect to SQL Server on. 0 and 1433 are synonymous.");
            // 
            // txtLocale
            // 
            this.txtLocale.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFormMain, "Locale", true));
            this.txtLocale.Location = new System.Drawing.Point(93, 76);
            this.txtLocale.Name = "txtLocale";
            this.txtLocale.Size = new System.Drawing.Size(242, 20);
            this.txtLocale.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtLocale, "More information about locales and available locales can be found here: https://g" +
        "ithub.com/bchavez/Bogus#locales. \r\n\r\nDouble click to open the url.");
            this.txtLocale.DoubleClick += new System.EventHandler(this.txtLocale_DoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Locale";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDatabaseName);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.txtServerName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 131);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Required";
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFormMain, "DatabaseName", true));
            this.errorProvider1.SetError(this.txtDatabaseName, "Database name is required");
            this.txtDatabaseName.Location = new System.Drawing.Point(95, 51);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(200, 20);
            this.txtDatabaseName.TabIndex = 1;
            this.txtDatabaseName.Validating += new System.ComponentModel.CancelEventHandler(this.txtDatabaseName_Validating);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Database Name";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(95, 75);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(95, 20);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtServerName
            // 
            this.txtServerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFormMain, "ServerName", true));
            this.errorProvider1.SetError(this.txtServerName, "ServerName is required");
            this.txtServerName.Location = new System.Drawing.Point(95, 26);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(200, 20);
            this.txtServerName.TabIndex = 0;
            this.txtServerName.Validating += new System.ComponentModel.CancelEventHandler(this.txtServerName_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server Name";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 30000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 685);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tablesGrid);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablesGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scriptTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFormMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portNumber)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private Syncfusion.WinForms.DataGrid.SfDataGrid tablesGrid;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource bindingSourceFormMain;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddlCredentials;
        private System.Windows.Forms.NumericUpDown portNumber;
        private System.Windows.Forms.TextBox txtLocale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPostScript;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPreScript;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnEditScriptImports;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown scriptTimeout;
        private System.Windows.Forms.ToolStripMenuItem refreshCredentialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshTablesToolStripMenuItem;
    }
}