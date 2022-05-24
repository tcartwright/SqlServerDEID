namespace SqlServerDEID.Editor
{
    partial class frmTransformTest
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWhereClause = new System.Windows.Forms.TextBox();
            this.btnRunQuery = new System.Windows.Forms.Button();
            this.gridRawData = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRunTransform = new System.Windows.Forms.Button();
            this.gridTransformedData = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransformedData)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.80109F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.19891F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtQuery, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtWhereClause, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRunQuery, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.gridRawData, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnRunTransform, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.gridTransformedData, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(810, 618);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 18);
            this.label2.TabIndex = 22;
            this.label2.Text = "Where Clause";
            // 
            // txtQuery
            // 
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.Location = new System.Drawing.Point(155, 3);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ReadOnly = true;
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(652, 169);
            this.txtQuery.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(101, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 20;
            this.label1.Text = "Query";
            // 
            // txtWhereClause
            // 
            this.txtWhereClause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWhereClause.Location = new System.Drawing.Point(155, 178);
            this.txtWhereClause.Name = "txtWhereClause";
            this.txtWhereClause.Size = new System.Drawing.Size(652, 20);
            this.txtWhereClause.TabIndex = 21;
            // 
            // btnRunQuery
            // 
            this.btnRunQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRunQuery.Location = new System.Drawing.Point(432, 208);
            this.btnRunQuery.Name = "btnRunQuery";
            this.btnRunQuery.Size = new System.Drawing.Size(97, 24);
            this.btnRunQuery.TabIndex = 23;
            this.btnRunQuery.Text = "Run Query";
            this.btnRunQuery.UseVisualStyleBackColor = true;
            this.btnRunQuery.Click += new System.EventHandler(this.btnRunQuery_Click);
            // 
            // gridRawData
            // 
            this.gridRawData.AccessibleName = "Table";
            this.gridRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRawData.Location = new System.Drawing.Point(155, 238);
            this.gridRawData.Name = "gridRawData";
            this.gridRawData.Size = new System.Drawing.Size(652, 169);
            this.gridRawData.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(36, 313);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 18);
            this.label3.TabIndex = 25;
            this.label3.Text = "Raw Table Data";
            // 
            // btnRunTransform
            // 
            this.btnRunTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRunTransform.Enabled = false;
            this.btnRunTransform.Location = new System.Drawing.Point(432, 413);
            this.btnRunTransform.Name = "btnRunTransform";
            this.btnRunTransform.Size = new System.Drawing.Size(97, 24);
            this.btnRunTransform.TabIndex = 26;
            this.btnRunTransform.Text = "Run Transform";
            this.btnRunTransform.UseVisualStyleBackColor = true;
            this.btnRunTransform.Click += new System.EventHandler(this.btnRunTransform_Click);
            // 
            // gridTransformedData
            // 
            this.gridTransformedData.AccessibleName = "Table";
            this.gridTransformedData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTransformedData.Location = new System.Drawing.Point(155, 443);
            this.gridTransformedData.Name = "gridTransformedData";
            this.gridTransformedData.Size = new System.Drawing.Size(652, 172);
            this.gridTransformedData.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 511);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 36);
            this.label4.TabIndex = 28;
            this.label4.Text = "Transformed Table Data";
            // 
            // frmTransformTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 618);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransformTest";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transform Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTransformTest_FormClosing);
            this.Load += new System.EventHandler(this.frmTransformTest_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRawData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransformedData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWhereClause;
        private System.Windows.Forms.Button btnRunQuery;
        private Syncfusion.WinForms.DataGrid.SfDataGrid gridRawData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRunTransform;
        private Syncfusion.WinForms.DataGrid.SfDataGrid gridTransformedData;
        private System.Windows.Forms.Label label4;
    }
}