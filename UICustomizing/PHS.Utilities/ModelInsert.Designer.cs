namespace PHS.Utilities
{
    partial class ModelInsert
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
            this.txt_pipe_name = new System.Windows.Forms.TextBox();
            this.txt_pipe_module = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.checkPipe = new System.Windows.Forms.CheckBox();
            this.checkStru = new System.Windows.Forms.CheckBox();
            this.checkEquip = new System.Windows.Forms.CheckBox();
            this.txt_stru_name = new System.Windows.Forms.TextBox();
            this.txt_stru_module = new System.Windows.Forms.TextBox();
            this.txt_equip_name = new System.Windows.Forms.TextBox();
            this.txt_equip_module = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnModelInsert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_pipe_name
            // 
            this.txt_pipe_name.Location = new System.Drawing.Point(118, 80);
            this.txt_pipe_name.Name = "txt_pipe_name";
            this.txt_pipe_name.Size = new System.Drawing.Size(122, 21);
            this.txt_pipe_name.TabIndex = 0;
            // 
            // txt_pipe_module
            // 
            this.txt_pipe_module.Location = new System.Drawing.Point(246, 80);
            this.txt_pipe_module.Name = "txt_pipe_module";
            this.txt_pipe_module.Size = new System.Drawing.Size(137, 21);
            this.txt_pipe_module.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(191, 286);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(101, 51);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "모델검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // checkPipe
            // 
            this.checkPipe.AutoSize = true;
            this.checkPipe.Location = new System.Drawing.Point(12, 85);
            this.checkPipe.Name = "checkPipe";
            this.checkPipe.Size = new System.Drawing.Size(49, 16);
            this.checkPipe.TabIndex = 3;
            this.checkPipe.Text = "Pipe";
            this.checkPipe.UseVisualStyleBackColor = true;
            // 
            // checkStru
            // 
            this.checkStru.AutoSize = true;
            this.checkStru.Location = new System.Drawing.Point(12, 121);
            this.checkStru.Name = "checkStru";
            this.checkStru.Size = new System.Drawing.Size(74, 16);
            this.checkStru.TabIndex = 4;
            this.checkStru.Text = "Structure";
            this.checkStru.UseVisualStyleBackColor = true;
            // 
            // checkEquip
            // 
            this.checkEquip.AutoSize = true;
            this.checkEquip.Location = new System.Drawing.Point(12, 159);
            this.checkEquip.Name = "checkEquip";
            this.checkEquip.Size = new System.Drawing.Size(84, 16);
            this.checkEquip.TabIndex = 5;
            this.checkEquip.Text = "Equipment";
            this.checkEquip.UseVisualStyleBackColor = true;
            // 
            // txt_stru_name
            // 
            this.txt_stru_name.Location = new System.Drawing.Point(118, 116);
            this.txt_stru_name.Name = "txt_stru_name";
            this.txt_stru_name.Size = new System.Drawing.Size(122, 21);
            this.txt_stru_name.TabIndex = 0;
            // 
            // txt_stru_module
            // 
            this.txt_stru_module.Location = new System.Drawing.Point(246, 116);
            this.txt_stru_module.Name = "txt_stru_module";
            this.txt_stru_module.Size = new System.Drawing.Size(137, 21);
            this.txt_stru_module.TabIndex = 1;
            // 
            // txt_equip_name
            // 
            this.txt_equip_name.Location = new System.Drawing.Point(118, 154);
            this.txt_equip_name.Name = "txt_equip_name";
            this.txt_equip_name.Size = new System.Drawing.Size(122, 21);
            this.txt_equip_name.TabIndex = 0;
            // 
            // txt_equip_module
            // 
            this.txt_equip_module.Location = new System.Drawing.Point(246, 154);
            this.txt_equip_module.Name = "txt_equip_module";
            this.txt_equip_module.Size = new System.Drawing.Size(137, 21);
            this.txt_equip_module.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Model Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Model Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(273, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Module";
            // 
            // btnModelInsert
            // 
            this.btnModelInsert.Location = new System.Drawing.Point(298, 286);
            this.btnModelInsert.Name = "btnModelInsert";
            this.btnModelInsert.Size = new System.Drawing.Size(94, 51);
            this.btnModelInsert.TabIndex = 7;
            this.btnModelInsert.Text = "모델 넣기";
            this.btnModelInsert.UseVisualStyleBackColor = true;
            this.btnModelInsert.Click += new System.EventHandler(this.btnModelInsert_Click);
            // 
            // ModelInsert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 393);
            this.Controls.Add(this.btnModelInsert);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkEquip);
            this.Controls.Add(this.checkStru);
            this.Controls.Add(this.checkPipe);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txt_equip_module);
            this.Controls.Add(this.txt_equip_name);
            this.Controls.Add(this.txt_stru_module);
            this.Controls.Add(this.txt_stru_name);
            this.Controls.Add(this.txt_pipe_module);
            this.Controls.Add(this.txt_pipe_name);
            this.Name = "ModelInsert";
            this.Text = "ModelInsert";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_pipe_name;
        private System.Windows.Forms.TextBox txt_pipe_module;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox checkPipe;
        private System.Windows.Forms.CheckBox checkStru;
        private System.Windows.Forms.CheckBox checkEquip;
        private System.Windows.Forms.TextBox txt_stru_name;
        private System.Windows.Forms.TextBox txt_stru_module;
        private System.Windows.Forms.TextBox txt_equip_name;
        private System.Windows.Forms.TextBox txt_equip_module;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnModelInsert;
    }
}