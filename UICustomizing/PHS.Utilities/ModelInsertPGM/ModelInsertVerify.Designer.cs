namespace PHS.Utilities.ModelInsertPGM
{
    partial class ModelInsertVerify
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
            this.grid1 = new SourceGrid.Grid();
            this.lblcnt = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btn_includeall = new System.Windows.Forms.Button();
            this.btn_excludeall = new System.Windows.Forms.Button();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.EnableSort = true;
            this.grid1.Location = new System.Drawing.Point(3, 37);
            this.grid1.Name = "grid1";
            this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid1.Size = new System.Drawing.Size(793, 497);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = true;
            this.grid1.ToolTipText = "";
            // 
            // lblcnt
            // 
            this.lblcnt.AutoSize = true;
            this.lblcnt.Location = new System.Drawing.Point(12, 9);
            this.lblcnt.Name = "lblcnt";
            this.lblcnt.Size = new System.Drawing.Size(59, 12);
            this.lblcnt.TabIndex = 1;
            this.lblcnt.Text = "총건수 : 0";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(546, 555);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 39);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(672, 555);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(124, 39);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btn_includeall
            // 
            this.btn_includeall.Location = new System.Drawing.Point(25, 558);
            this.btn_includeall.Name = "btn_includeall";
            this.btn_includeall.Size = new System.Drawing.Size(76, 35);
            this.btn_includeall.TabIndex = 4;
            this.btn_includeall.Text = "&IncludeAll";
            this.btn_includeall.UseVisualStyleBackColor = true;
            this.btn_includeall.Click += new System.EventHandler(this.btn_includeall_Click);
            // 
            // btn_excludeall
            // 
            this.btn_excludeall.Location = new System.Drawing.Point(118, 557);
            this.btn_excludeall.Name = "btn_excludeall";
            this.btn_excludeall.Size = new System.Drawing.Size(88, 35);
            this.btn_excludeall.TabIndex = 4;
            this.btn_excludeall.Text = "&Exculde All";
            this.btn_excludeall.UseVisualStyleBackColor = true;
            this.btn_excludeall.Click += new System.EventHandler(this.btn_excludeall_Click);
            // 
            // ModelInsertVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 625);
            this.Controls.Add(this.btn_excludeall);
            this.Controls.Add(this.btn_includeall);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblcnt);
            this.Controls.Add(this.grid1);
            this.Name = "ModelInsertVerify";
            this.Text = "검색된 Model 확인";
            this.Load += new System.EventHandler(this.ModelInsertVerify_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SourceGrid.Grid grid1;
        private System.Windows.Forms.Label lblcnt;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btn_includeall;
        private System.Windows.Forms.Button btn_excludeall;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
    }
}