﻿namespace SqlServerDEID.Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransformTest));
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
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransformedData)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtQuery, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtWhereClause, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnRunQuery, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnRunTransform, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.gridTransformedData, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridRawData, 1, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(810, 647);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 18);
            this.label2.TabIndex = 22;
            this.label2.Text = "Where Clause";
            // 
            // txtQuery
            // 
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.Location = new System.Drawing.Point(128, 63);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ReadOnly = true;
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(679, 109);
            this.txtQuery.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(74, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 20;
            this.label1.Text = "Query";
            // 
            // txtWhereClause
            // 
            this.txtWhereClause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWhereClause.Location = new System.Drawing.Point(128, 178);
            this.txtWhereClause.Name = "txtWhereClause";
            this.txtWhereClause.Size = new System.Drawing.Size(679, 20);
            this.txtWhereClause.TabIndex = 21;
            // 
            // btnRunQuery
            // 
            this.btnRunQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRunQuery.Location = new System.Drawing.Point(419, 208);
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
            this.gridRawData.AllowResizingColumns = true;
            this.gridRawData.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            this.gridRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRawData.Location = new System.Drawing.Point(128, 238);
            this.gridRawData.Name = "gridRawData";
            this.gridRawData.Size = new System.Drawing.Size(679, 175);
            this.gridRawData.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 326);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 18);
            this.label3.TabIndex = 25;
            this.label3.Text = "Raw Table Data";
            // 
            // btnRunTransform
            // 
            this.btnRunTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRunTransform.Enabled = false;
            this.btnRunTransform.Location = new System.Drawing.Point(419, 438);
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
            this.gridTransformedData.AllowResizingColumns = true;
            this.gridTransformedData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTransformedData.Location = new System.Drawing.Point(128, 468);
            this.gridTransformedData.Name = "gridTransformedData";
            this.gridTransformedData.Size = new System.Drawing.Size(679, 175);
            this.gridTransformedData.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 525);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 36);
            this.label4.TabIndex = 28;
            this.label4.Text = "Transformed Data";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(128, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(679, 60);
            this.label5.TabIndex = 29;
            this.label5.Text = resources.GetString("label5.Text");
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmTransformTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 651);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
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
        private System.Windows.Forms.Label label5;
    }
}