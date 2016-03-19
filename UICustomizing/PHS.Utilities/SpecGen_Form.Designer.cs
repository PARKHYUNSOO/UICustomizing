namespace PHS.Utilities
{
    partial class SpecGen_Form
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
            this.cb_dept = new System.Windows.Forms.ComboBox();
            this.txtshipno = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_createspec = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_emptysele_cnt = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_change_vv_type = new System.Windows.Forms.Button();
            this.txt_specname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::PHS.Utilities.Utility.GeneralWaitForm), true, true);
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtconn = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cb_dept
            // 
            this.cb_dept.FormattingEnabled = true;
            this.cb_dept.Items.AddRange(new object[] {
            "ACCOM",
            "MACH",
            "PIPE"});
            this.cb_dept.Location = new System.Drawing.Point(31, 75);
            this.cb_dept.Name = "cb_dept";
            this.cb_dept.Size = new System.Drawing.Size(121, 20);
            this.cb_dept.TabIndex = 0;
            this.cb_dept.Text = "PIPE";
            // 
            // txtshipno
            // 
            this.txtshipno.Location = new System.Drawing.Point(171, 75);
            this.txtshipno.Name = "txtshipno";
            this.txtshipno.Size = new System.Drawing.Size(100, 21);
            this.txtshipno.TabIndex = 1;
            this.txtshipno.Text = "2435";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "부서";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(188, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "호선";
            // 
            // btn_createspec
            // 
            this.btn_createspec.Location = new System.Drawing.Point(295, 27);
            this.btn_createspec.Name = "btn_createspec";
            this.btn_createspec.Size = new System.Drawing.Size(73, 73);
            this.btn_createspec.TabIndex = 4;
            this.btn_createspec.Text = "스펙생성1";
            this.btn_createspec.UseVisualStyleBackColor = true;
            this.btn_createspec.Click += new System.EventHandler(this.btn_createspec_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(195, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 36);
            this.button1.TabIndex = 5;
            this.button1.Text = "빈 셀레지우기(전체에서 검색)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_emptysele_cnt
            // 
            this.lbl_emptysele_cnt.AutoSize = true;
            this.lbl_emptysele_cnt.Location = new System.Drawing.Point(83, 266);
            this.lbl_emptysele_cnt.Name = "lbl_emptysele_cnt";
            this.lbl_emptysele_cnt.Size = new System.Drawing.Size(106, 12);
            this.lbl_emptysele_cnt.TabIndex = 6;
            this.lbl_emptysele_cnt.Text = "lbl_emptysele_cnt";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(321, 148);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 36);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_change_vv_type
            // 
            this.btn_change_vv_type.Location = new System.Drawing.Point(298, 254);
            this.btn_change_vv_type.Name = "btn_change_vv_type";
            this.btn_change_vv_type.Size = new System.Drawing.Size(121, 37);
            this.btn_change_vv_type.TabIndex = 8;
            this.btn_change_vv_type.Text = "밸브타입 바꾸기\r\n안쓰는밸브 삭제";
            this.btn_change_vv_type.UseVisualStyleBackColor = true;
            this.btn_change_vv_type.Click += new System.EventHandler(this.btn_change_vv_type_Click);
            // 
            // txt_specname
            // 
            this.txt_specname.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_specname.Location = new System.Drawing.Point(171, 112);
            this.txt_specname.Name = "txt_specname";
            this.txt_specname.Size = new System.Drawing.Size(124, 21);
            this.txt_specname.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "생성할 시스템\r\n(비워져있으면 전체생성)";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(321, 210);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 38);
            this.button3.TabIndex = 11;
            this.button3.Text = "valve tanswer원래대로";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(28, 158);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(124, 42);
            this.button4.TabIndex = 12;
            this.button4.Text = "사이즈 순 정렬(CE)";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(28, 206);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(124, 57);
            this.button5.TabIndex = 13;
            this.button5.Text = "Elbo각도제한해제(min:0.1 max:90.1) CE아래적용";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(422, 108);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(99, 91);
            this.button6.TabIndex = 14;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(165, 156);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(96, 42);
            this.button7.TabIndex = 15;
            this.button7.Text = "WELD SHOP정보 FALSE로";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(443, 218);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(124, 48);
            this.button8.TabIndex = 16;
            this.button8.Text = "savetracking";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(442, 42);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(78, 33);
            this.button9.TabIndex = 17;
            this.button9.Text = "button9";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(189, 312);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(100, 63);
            this.button10.TabIndex = 18;
            this.button10.Text = "ConnCode확인";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 337);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "찾을 Connection Code";
            // 
            // txtconn
            // 
            this.txtconn.Location = new System.Drawing.Point(31, 352);
            this.txtconn.Name = "txtconn";
            this.txtconn.Size = new System.Drawing.Size(100, 21);
            this.txtconn.TabIndex = 20;
            // 
            // SpecGen_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 414);
            this.Controls.Add(this.txtconn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_specname);
            this.Controls.Add(this.btn_change_vv_type);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lbl_emptysele_cnt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_createspec);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtshipno);
            this.Controls.Add(this.cb_dept);
            this.Name = "SpecGen_Form";
            this.Text = "SpecGen_Form";
            this.Load += new System.EventHandler(this.SpecGen_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_dept;
        private System.Windows.Forms.TextBox txtshipno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_createspec;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbl_emptysele_cnt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_change_vv_type;
        private System.Windows.Forms.TextBox txt_specname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtconn;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
    }
}