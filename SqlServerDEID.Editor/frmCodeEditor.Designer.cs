namespace SqlServerDEID.Editor
{
    partial class frmCodeEditor
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
            this.codeEditor1 = new CDS.CSharpScripting.CodeEditor();
            this.outputPanel1 = new CDS.CSharpScripting.OutputPanel();
            this.btnTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // codeEditor1
            // 
            this.codeEditor1.CDSScript = "// C# code editor";
            this.codeEditor1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codeEditor1.Location = new System.Drawing.Point(12, 22);
            this.codeEditor1.Name = "codeEditor1";
            this.codeEditor1.Size = new System.Drawing.Size(776, 224);
            this.codeEditor1.TabIndex = 0;
            // 
            // outputPanel1
            // 
            this.outputPanel1.Location = new System.Drawing.Point(12, 252);
            this.outputPanel1.Name = "outputPanel1";
            this.outputPanel1.Size = new System.Drawing.Size(776, 177);
            this.outputPanel1.TabIndex = 1;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(346, 436);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "Test Code";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // frmCodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 476);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.outputPanel1);
            this.Controls.Add(this.codeEditor1);
            this.Name = "frmCodeEditor";
            this.Text = "Code Editor";
            this.Load += new System.EventHandler(this.frmCodeEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CDS.CSharpScripting.CodeEditor codeEditor1;
        private CDS.CSharpScripting.OutputPanel outputPanel1;
        private System.Windows.Forms.Button btnTest;
    }
}

