namespace PHS.Utilities
{
    partial class ElementRenameForm
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
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::PHS.Utilities.Utility.GeneralWaitForm), true, true);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabfind = new System.Windows.Forms.TabPage();
            this.btn_search = new System.Windows.Forms.Button();
            this.lbl_highlightoff = new System.Windows.Forms.LinkLabel();
            this.lbl_result = new System.Windows.Forms.Label();
            this.btn_next = new System.Windows.Forms.Button();
            this.btn_pre = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.tabreplace = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtsearch_replace = new System.Windows.Forms.TextBox();
            this.btnrename = new System.Windows.Forms.Button();
            this.txtreplace = new System.Windows.Forms.TextBox();
            this.tree_type = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.radio_entire = new System.Windows.Forms.RadioButton();
            this.radio_ce = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radio_3dview = new System.Windows.Forms.RadioButton();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.listView_searchresult = new System.Windows.Forms.ListView();
            this.col_idx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_elementname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_islock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_marptype = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabfind.SuspendLayout();
            this.tabreplace.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabfind);
            this.tabControl1.Controls.Add(this.tabreplace);
            this.tabControl1.Location = new System.Drawing.Point(8, 71);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(350, 192);
            this.tabControl1.TabIndex = 24;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabfind
            // 
            this.tabfind.Controls.Add(this.btn_search);
            this.tabfind.Controls.Add(this.lbl_highlightoff);
            this.tabfind.Controls.Add(this.lbl_result);
            this.tabfind.Controls.Add(this.btn_next);
            this.tabfind.Controls.Add(this.btn_pre);
            this.tabfind.Controls.Add(this.label1);
            this.tabfind.Controls.Add(this.txt_search);
            this.tabfind.Location = new System.Drawing.Point(4, 23);
            this.tabfind.Name = "tabfind";
            this.tabfind.Padding = new System.Windows.Forms.Padding(3);
            this.tabfind.Size = new System.Drawing.Size(342, 165);
            this.tabfind.TabIndex = 0;
            this.tabfind.Text = "찾기";
            this.tabfind.UseVisualStyleBackColor = true;
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(222, 7);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(90, 32);
            this.btn_search.TabIndex = 27;
            this.btn_search.Text = "검색";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // lbl_highlightoff
            // 
            this.lbl_highlightoff.AutoSize = true;
            this.lbl_highlightoff.Location = new System.Drawing.Point(6, 143);
            this.lbl_highlightoff.Name = "lbl_highlightoff";
            this.lbl_highlightoff.Size = new System.Drawing.Size(95, 14);
            this.lbl_highlightoff.TabIndex = 26;
            this.lbl_highlightoff.TabStop = true;
            this.lbl_highlightoff.Text = "하이라이트 끄기";
            this.lbl_highlightoff.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbl_highlightoff_LinkClicked);
            // 
            // lbl_result
            // 
            this.lbl_result.AutoSize = true;
            this.lbl_result.Location = new System.Drawing.Point(6, 50);
            this.lbl_result.Name = "lbl_result";
            this.lbl_result.Size = new System.Drawing.Size(63, 14);
            this.lbl_result.TabIndex = 25;
            this.lbl_result.Text = "검색결과 :";
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(162, 79);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(75, 51);
            this.btn_next.TabIndex = 24;
            this.btn_next.Text = "다음찾기";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_pre
            // 
            this.btn_pre.Location = new System.Drawing.Point(81, 79);
            this.btn_pre.Name = "btn_pre";
            this.btn_pre.Size = new System.Drawing.Size(75, 51);
            this.btn_pre.TabIndex = 24;
            this.btn_pre.Text = "이전찾기";
            this.btn_pre.UseVisualStyleBackColor = true;
            this.btn_pre.Click += new System.EventHandler(this.btn_pre_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 14);
            this.label1.TabIndex = 23;
            this.label1.Text = "대상 문자열";
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(81, 17);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(116, 22);
            this.txt_search.TabIndex = 22;
            this.txt_search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_search_KeyDown);
            // 
            // tabreplace
            // 
            this.tabreplace.Controls.Add(this.groupBox2);
            this.tabreplace.Location = new System.Drawing.Point(4, 23);
            this.tabreplace.Name = "tabreplace";
            this.tabreplace.Padding = new System.Windows.Forms.Padding(3);
            this.tabreplace.Size = new System.Drawing.Size(335, 165);
            this.tabreplace.TabIndex = 1;
            this.tabreplace.Text = "바꾸기";
            this.tabreplace.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtsearch_replace);
            this.groupBox2.Controls.Add(this.btnrename);
            this.groupBox2.Controls.Add(this.txtreplace);
            this.groupBox2.Location = new System.Drawing.Point(6, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 90);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Element 이름 변경";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 14);
            this.label5.TabIndex = 23;
            this.label5.Text = "바꿀 문자열";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 14);
            this.label4.TabIndex = 22;
            this.label4.Text = "대상 문자열";
            // 
            // txtsearch_replace
            // 
            this.txtsearch_replace.Location = new System.Drawing.Point(85, 23);
            this.txtsearch_replace.Name = "txtsearch_replace";
            this.txtsearch_replace.Size = new System.Drawing.Size(116, 22);
            this.txtsearch_replace.TabIndex = 21;
            // 
            // btnrename
            // 
            this.btnrename.Location = new System.Drawing.Point(212, 17);
            this.btnrename.Name = "btnrename";
            this.btnrename.Size = new System.Drawing.Size(81, 58);
            this.btnrename.TabIndex = 20;
            this.btnrename.Text = "이름바꾸기";
            this.btnrename.UseVisualStyleBackColor = true;
            this.btnrename.Click += new System.EventHandler(this.btnrename_Click);
            // 
            // txtreplace
            // 
            this.txtreplace.Location = new System.Drawing.Point(85, 51);
            this.txtreplace.Name = "txtreplace";
            this.txtreplace.Size = new System.Drawing.Size(116, 22);
            this.txtreplace.TabIndex = 21;
            // 
            // tree_type
            // 
            this.tree_type.CheckBoxes = true;
            this.tree_type.Location = new System.Drawing.Point(364, 81);
            this.tree_type.Name = "tree_type";
            this.tree_type.Size = new System.Drawing.Size(135, 182);
            this.tree_type.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 14);
            this.label2.TabIndex = 26;
            this.label2.Text = "변경 타입";
            // 
            // radio_entire
            // 
            this.radio_entire.AutoSize = true;
            this.radio_entire.Location = new System.Drawing.Point(6, 22);
            this.radio_entire.Name = "radio_entire";
            this.radio_entire.Size = new System.Drawing.Size(119, 18);
            this.radio_entire.TabIndex = 27;
            this.radio_entire.Text = "전체(해당MDB내)";
            this.radio_entire.UseVisualStyleBackColor = true;
            this.radio_entire.CheckedChanged += new System.EventHandler(this.Mode_Change);
            // 
            // radio_ce
            // 
            this.radio_ce.AutoSize = true;
            this.radio_ce.Checked = true;
            this.radio_ce.Location = new System.Drawing.Point(156, 22);
            this.radio_ce.Name = "radio_ce";
            this.radio_ce.Size = new System.Drawing.Size(39, 18);
            this.radio_ce.TabIndex = 28;
            this.radio_ce.TabStop = true;
            this.radio_ce.Text = "CE";
            this.radio_ce.UseVisualStyleBackColor = true;
            this.radio_ce.CheckedChanged += new System.EventHandler(this.Mode_Change);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radio_3dview);
            this.groupBox1.Controls.Add(this.radio_entire);
            this.groupBox1.Controls.Add(this.radio_ce);
            this.groupBox1.Location = new System.Drawing.Point(12, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 48);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "검색 범위";
            // 
            // radio_3dview
            // 
            this.radio_3dview.AutoSize = true;
            this.radio_3dview.Location = new System.Drawing.Point(222, 22);
            this.radio_3dview.Name = "radio_3dview";
            this.radio_3dview.Size = new System.Drawing.Size(71, 18);
            this.radio_3dview.TabIndex = 30;
            this.radio_3dview.Text = "3D View";
            this.radio_3dview.UseVisualStyleBackColor = true;
            this.radio_3dview.CheckedChanged += new System.EventHandler(this.Mode_Change);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 270);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 14);
            this.label3.TabIndex = 31;
            this.label3.Text = "검색결과 : 0";
            // 
            // listView_searchresult
            // 
            this.listView_searchresult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_searchresult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_idx,
            this.col_elementname,
            this.col_type,
            this.col_islock,
            this.col_marptype});
            this.listView_searchresult.FullRowSelect = true;
            this.listView_searchresult.GridLines = true;
            this.listView_searchresult.Location = new System.Drawing.Point(8, 287);
            this.listView_searchresult.Name = "listView_searchresult";
            this.listView_searchresult.Size = new System.Drawing.Size(491, 200);
            this.listView_searchresult.TabIndex = 32;
            this.listView_searchresult.UseCompatibleStateImageBehavior = false;
            this.listView_searchresult.View = System.Windows.Forms.View.Details;
            this.listView_searchresult.SelectedIndexChanged += new System.EventHandler(this.listView_searchresult_SelectedIndexChanged);
            // 
            // col_idx
            // 
            this.col_idx.Text = "순번";
            this.col_idx.Width = 40;
            // 
            // col_elementname
            // 
            this.col_elementname.Text = "Element Name";
            this.col_elementname.Width = 250;
            // 
            // col_type
            // 
            this.col_type.Text = "타입";
            // 
            // col_islock
            // 
            this.col_islock.Text = "Lock여부";
            this.col_islock.Width = 65;
            // 
            // col_marptype
            // 
            this.col_marptype.Text = "도면타입";
            // 
            // ElementRenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 492);
            this.Controls.Add(this.listView_searchresult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tree_type);
            this.Name = "ElementRenameForm";
            this.Text = "Element Control Utility";
            this.Load += new System.EventHandler(this.ElementRenameForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabfind.ResumeLayout(false);
            this.tabfind.PerformLayout();
            this.tabreplace.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabfind;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.Button btn_pre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.TabPage tabreplace;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtsearch_replace;
        private System.Windows.Forms.Button btnrename;
        private System.Windows.Forms.TextBox txtreplace;
        private System.Windows.Forms.Label lbl_result;
        private System.Windows.Forms.TreeView tree_type;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radio_entire;
        private System.Windows.Forms.RadioButton radio_ce;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radio_3dview;
        private System.Windows.Forms.LinkLabel lbl_highlightoff;
        private System.Windows.Forms.Button btn_search;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listView_searchresult;
        private System.Windows.Forms.ColumnHeader col_idx;
        private System.Windows.Forms.ColumnHeader col_elementname;
        private System.Windows.Forms.ColumnHeader col_islock;
        private System.Windows.Forms.ColumnHeader col_type;
        private System.Windows.Forms.ColumnHeader col_marptype;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;

    }
}