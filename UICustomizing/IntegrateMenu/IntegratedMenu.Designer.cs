using Aveva.PDMS.PMLNet;
namespace HMD.AM.IntegratedMenu
{
    [PMLNetCallable()]
    partial class IntegratedMenu
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IntegratedMenu));
            this.button1 = new System.Windows.Forms.Button();
            this.treeViewAdv1 = new Aga.Controls.Tree.TreeViewAdv();
            this.nodeTextBox1 = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox2 = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeIcon1 = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.qExplorerBar1 = new Qios.DevSuite.Components.QExplorerBar();
            this.qExplorerItem1 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem4 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem5 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem6 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem2 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem8 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem10 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem9 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem11 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem12 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem13 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem3 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.qExplorerItem7 = new Qios.DevSuite.Components.QExplorerItem(this.components);
            this.treeViewAdv2 = new Aga.Controls.Tree.TreeViewAdv();
            this.treeViewAdv3 = new Aga.Controls.Tree.TreeViewAdv();
            this.treeViewAdv4 = new Aga.Controls.Tree.TreeViewAdv();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.qRibbonItem2 = new Qios.DevSuite.Components.Ribbon.QRibbonItem();
            this.qCompositeMenuItem2 = new Qios.DevSuite.Components.QCompositeMenuItem();
            this.qCompositeMenuItem1 = new Qios.DevSuite.Components.QCompositeMenuItem();
            this.qRibbonItem1 = new Qios.DevSuite.Components.Ribbon.QRibbonItem();
            this.qRibbonPanel1 = new Qios.DevSuite.Components.Ribbon.QRibbonPanel();
            this.qRibbonPage1 = new Qios.DevSuite.Components.Ribbon.QRibbonPage();
            this.qRibbon1 = new Qios.DevSuite.Components.Ribbon.QRibbon();
            this.qRibbonPage2 = new Qios.DevSuite.Components.Ribbon.QRibbonPage();
            this.qRibbonPanel2 = new Qios.DevSuite.Components.Ribbon.QRibbonPanel();
            this.qRibbonPage3 = new Qios.DevSuite.Components.Ribbon.QRibbonPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.qRibbonPage4 = new Qios.DevSuite.Components.Ribbon.QRibbonPage();
            this.qRibbonPage5 = new Qios.DevSuite.Components.Ribbon.QRibbonPage();
            this.qCompositeMenuItem3 = new Qios.DevSuite.Components.QCompositeMenuItem();
            this.statusStrip1.SuspendLayout();
            this.qExplorerBar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbon1)).BeginInit();
            this.qRibbon1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage5)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // treeViewAdv1
            // 
            this.treeViewAdv1.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdv1.DefaultToolTipProvider = null;
            this.treeViewAdv1.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv1.LineColor = System.Drawing.SystemColors.ControlDark;
            resources.ApplyResources(this.treeViewAdv1, "treeViewAdv1");
            this.treeViewAdv1.Model = null;
            this.treeViewAdv1.Name = "treeViewAdv1";
            this.treeViewAdv1.NodeControls.Add(this.nodeIcon1);
            this.treeViewAdv1.NodeControls.Add(this.nodeTextBox1);
            this.treeViewAdv1.NodeControls.Add(this.nodeTextBox2);
            this.treeViewAdv1.SelectedNode = null;
            this.treeViewAdv1.ShowNodeToolTips = true;
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.DataPropertyName = "Text";
            this.nodeTextBox1.IncrementalSearchEnabled = true;
            this.nodeTextBox1.LeftMargin = 3;
            this.nodeTextBox1.ParentColumn = null;
            // 
            // nodeTextBox2
            // 
            this.nodeTextBox2.DataPropertyName = "Tag";
            this.nodeTextBox2.IncrementalSearchEnabled = true;
            this.nodeTextBox2.LeftMargin = 3;
            this.nodeTextBox2.ParentColumn = null;
            // 
            // nodeIcon1
            // 
            this.nodeIcon1.DataPropertyName = "Image";
            this.nodeIcon1.LeftMargin = 1;
            this.nodeIcon1.ParentColumn = null;
            this.nodeIcon1.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // qExplorerBar1
            // 
            this.qExplorerBar1.Appearance.BackgroundStyle = Qios.DevSuite.Components.QColorStyle.Solid;
            this.qExplorerBar1.Appearance.ShowBorders = false;
            this.qExplorerBar1.ColorScheme.ExplorerBarBackground1.SetColor("VistaBlack", System.Drawing.Color.RoyalBlue, false);
            this.qExplorerBar1.ColorScheme.ExplorerBarBackground2.SetColor("VistaBlack", System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128))))), false);
            this.qExplorerBar1.ColorScheme.ExplorerBarGroupItemBackground1.SetColor("VistaBlack", System.Drawing.Color.White, false);
            this.qExplorerBar1.ColorScheme.ExplorerBarGroupItemBackground2.SetColor("VistaBlack", System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))), false);
            this.qExplorerBar1.Controls.Add(this.button1);
            this.qExplorerBar1.CreateNew = true;
            this.qExplorerBar1.ExplorerItems.AddRange(new Qios.DevSuite.Components.QMenuItem[] {
            this.qExplorerItem1,
            this.qExplorerItem2,
            this.qExplorerItem3,
            this.qExplorerItem7});
            resources.ApplyResources(this.qExplorerBar1, "qExplorerBar1");
            this.qExplorerBar1.Name = "qExplorerBar1";
            this.qExplorerBar1.PersistGuid = new System.Guid("f439be93-721b-49ad-830d-a197f4eb1aa0");
            this.qExplorerBar1.CustomizeMenuShowed += new System.EventHandler(this.qExplorerBar1_CustomizeMenuShowed);
            // 
            // qExplorerItem1
            // 
            this.qExplorerItem1.Expanded = true;
            this.qExplorerItem1.Icon = ((System.Drawing.Icon)(resources.GetObject("qExplorerItem1.Icon")));
            this.qExplorerItem1.ItemName = "냠냠";
            this.qExplorerItem1.MenuItems.AddRange(new Qios.DevSuite.Components.QMenuItem[] {
            this.qExplorerItem4,
            this.qExplorerItem5,
            this.qExplorerItem6});
            resources.ApplyResources(this.qExplorerItem1, "qExplorerItem1");
            // 
            // qExplorerItem4
            // 
            resources.ApplyResources(this.qExplorerItem4, "qExplorerItem4");
            // 
            // qExplorerItem5
            // 
            resources.ApplyResources(this.qExplorerItem5, "qExplorerItem5");
            // 
            // qExplorerItem6
            // 
            resources.ApplyResources(this.qExplorerItem6, "qExplorerItem6");
            // 
            // qExplorerItem2
            // 
            this.qExplorerItem2.MenuItems.AddRange(new Qios.DevSuite.Components.QMenuItem[] {
            this.qExplorerItem8,
            this.qExplorerItem10,
            this.qExplorerItem9});
            resources.ApplyResources(this.qExplorerItem2, "qExplorerItem2");
            // 
            // qExplorerItem8
            // 
            resources.ApplyResources(this.qExplorerItem8, "qExplorerItem8");
            // 
            // qExplorerItem10
            // 
            resources.ApplyResources(this.qExplorerItem10, "qExplorerItem10");
            // 
            // qExplorerItem9
            // 
            this.qExplorerItem9.MenuItems.AddRange(new Qios.DevSuite.Components.QMenuItem[] {
            this.qExplorerItem11,
            this.qExplorerItem12,
            this.qExplorerItem13});
            resources.ApplyResources(this.qExplorerItem9, "qExplorerItem9");
            // 
            // qExplorerItem11
            // 
            resources.ApplyResources(this.qExplorerItem11, "qExplorerItem11");
            // 
            // qExplorerItem12
            // 
            resources.ApplyResources(this.qExplorerItem12, "qExplorerItem12");
            // 
            // qExplorerItem13
            // 
            resources.ApplyResources(this.qExplorerItem13, "qExplorerItem13");
            // 
            // treeViewAdv2
            // 
            this.treeViewAdv2.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdv2.DefaultToolTipProvider = null;
            this.treeViewAdv2.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv2.LineColor = System.Drawing.SystemColors.ControlDark;
            resources.ApplyResources(this.treeViewAdv2, "treeViewAdv2");
            this.treeViewAdv2.Model = null;
            this.treeViewAdv2.Name = "treeViewAdv2";
            this.treeViewAdv2.SelectedNode = null;
            this.treeViewAdv2.ShowNodeToolTips = true;
            // 
            // treeViewAdv3
            // 
            this.treeViewAdv3.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdv3.DefaultToolTipProvider = null;
            this.treeViewAdv3.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv3.LineColor = System.Drawing.SystemColors.ControlDark;
            resources.ApplyResources(this.treeViewAdv3, "treeViewAdv3");
            this.treeViewAdv3.Model = null;
            this.treeViewAdv3.Name = "treeViewAdv3";
            this.treeViewAdv3.SelectedNode = null;
            this.treeViewAdv3.ShowNodeToolTips = true;
            // 
            // treeViewAdv4
            // 
            this.treeViewAdv4.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdv4.DefaultToolTipProvider = null;
            this.treeViewAdv4.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv4.LineColor = System.Drawing.SystemColors.ControlDark;
            resources.ApplyResources(this.treeViewAdv4, "treeViewAdv4");
            this.treeViewAdv4.Model = null;
            this.treeViewAdv4.Name = "treeViewAdv4";
            this.treeViewAdv4.SelectedNode = null;
            this.treeViewAdv4.ShowNodeToolTips = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "2012_전산개발의뢰.ico");
            this.imageList1.Images.SetKeyName(1, "32512.bmp");
            this.imageList1.Images.SetKeyName(2, "20131207_092259_editImage.Jpg");
            this.imageList1.Images.SetKeyName(3, "20140404_135011_editImage.Jpg");
            this.imageList1.Images.SetKeyName(4, "1371443785_Share.png");
            this.imageList1.Images.SetKeyName(5, "1372825180_22930.ico");
            this.imageList1.Images.SetKeyName(6, "1372825222_18121.ico");
            this.imageList1.Images.SetKeyName(7, "1372825228_68010.ico");
            this.imageList1.Images.SetKeyName(8, "1372825236_25265.ico");
            this.imageList1.Images.SetKeyName(9, "1372825373_play.png");
            this.imageList1.Images.SetKeyName(10, "1372825379_media-playback-start.png");
            this.imageList1.Images.SetKeyName(11, "1374764821_stock_save.png");
            // 
            // qRibbonItem2
            // 
            resources.ApplyResources(this.qRibbonItem2, "qRibbonItem2");
            // 
            // qRibbonItem1
            // 
            this.qRibbonItem1.ChildItems.Add(this.qCompositeMenuItem1);
            this.qRibbonItem1.ChildItems.Add(this.qCompositeMenuItem2);
            this.qRibbonItem1.Configuration.Direction = Qios.DevSuite.Components.QPartDirection.Vertical;
            this.qRibbonItem1.Configuration.IconConfiguration.IconSize = new System.Drawing.Size(32, 32);
            this.qRibbonItem1.ControlSize = new System.Drawing.Size(30, 30);
            this.qRibbonItem1.Icon = ((System.Drawing.Icon)(resources.GetObject("qRibbonItem1.Icon")));
            resources.ApplyResources(this.qRibbonItem1, "qRibbonItem1");
            // 
            // qRibbonPanel1
            // 
            this.qRibbonPanel1.Items.Add(this.qRibbonItem1);
            this.qRibbonPanel1.Items.Add(this.qRibbonItem2);
            resources.ApplyResources(this.qRibbonPanel1, "qRibbonPanel1");
            // 
            // qRibbonPage1
            // 
            this.qRibbonPage1.ButtonOrder = 0;
            this.qRibbonPage1.ColorScheme.RibbonPageBackground1.SetColor("VistaBlack", System.Drawing.Color.White, false);
            this.qRibbonPage1.ColorScheme.RibbonPageBackground2.SetColor("VistaBlack", System.Drawing.Color.LightGray, false);
            this.qRibbonPage1.Items.Add(this.qRibbonPanel1);
            resources.ApplyResources(this.qRibbonPage1, "qRibbonPage1");
            this.qRibbonPage1.Name = "qRibbonPage1";
            this.qRibbonPage1.PersistGuid = new System.Guid("a3a56bfb-7e74-494e-892b-814d8c0249a3");
            // 
            // qRibbon1
            // 
            this.qRibbon1.ActiveTabPage = this.qRibbonPage1;
            this.qRibbon1.ColorScheme.RibbonBackground1.SetColor("VistaBlack", System.Drawing.Color.Navy, false);
            this.qRibbon1.ColorScheme.RibbonBackground2.SetColor("VistaBlack", System.Drawing.Color.White, false);
            this.qRibbon1.ColorScheme.RibbonPageBackground1.SetColor("VistaBlack", System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))), false);
            this.qRibbon1.ColorScheme.RibbonPanelText.SetColor("VistaBlack", System.Drawing.Color.Black, false);
            this.qRibbon1.ColorScheme.RibbonPanelTextActive.SetColor("VistaBlack", System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128))))), false);
            this.qRibbon1.Controls.Add(this.qRibbonPage1);
            this.qRibbon1.Controls.Add(this.qRibbonPage2);
            this.qRibbon1.Controls.Add(this.qRibbonPage3);
            this.qRibbon1.Controls.Add(this.qRibbonPage4);
            this.qRibbon1.Controls.Add(this.qRibbonPage5);
            this.qRibbon1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.qRibbon1, "qRibbon1");
            this.qRibbon1.Name = "qRibbon1";
            this.qRibbon1.PersistGuid = new System.Guid("156cb1ef-5fc5-4d87-a25a-d624250bc782");
            // 
            // qRibbonPage2
            // 
            this.qRibbonPage2.ButtonOrder = 1;
            this.qRibbonPage2.ColorScheme.RibbonPageBackground2.SetColor("VistaBlack", System.Drawing.Color.White, false);
            this.qRibbonPage2.Items.Add(this.qRibbonPanel2);
            resources.ApplyResources(this.qRibbonPage2, "qRibbonPage2");
            this.qRibbonPage2.Name = "qRibbonPage2";
            this.qRibbonPage2.PersistGuid = new System.Guid("f86af0d2-bce9-42e5-a9f1-f8cb33ad1906");
            // 
            // qRibbonPanel2
            // 
            resources.ApplyResources(this.qRibbonPanel2, "qRibbonPanel2");
            // 
            // qRibbonPage3
            // 
            this.qRibbonPage3.ButtonOrder = 2;
            resources.ApplyResources(this.qRibbonPage3, "qRibbonPage3");
            this.qRibbonPage3.Name = "qRibbonPage3";
            this.qRibbonPage3.PersistGuid = new System.Guid("cc2d6e3c-0d90-4033-b8f1-6ad03aa0e791");
            // 
            // treeView1
            // 
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.Name = "treeView1";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes1"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes2"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes3"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes4"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes5"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView1.Nodes6")))});
            // 
            // qRibbonPage4
            // 
            this.qRibbonPage4.ButtonOrder = 3;
            resources.ApplyResources(this.qRibbonPage4, "qRibbonPage4");
            this.qRibbonPage4.Name = "qRibbonPage4";
            this.qRibbonPage4.PersistGuid = new System.Guid("92d0d506-5aab-4bad-9ba8-54c360f4ee0e");
            // 
            // qRibbonPage5
            // 
            this.qRibbonPage5.ButtonOrder = 4;
            this.qRibbonPage5.Items.Add(this.qCompositeMenuItem3);
            resources.ApplyResources(this.qRibbonPage5, "qRibbonPage5");
            this.qRibbonPage5.Name = "qRibbonPage5";
            this.qRibbonPage5.PersistGuid = new System.Guid("8e4b49a2-23ef-41a4-b9cd-57927739752e");
            // 
            // qCompositeMenuItem3
            // 
            resources.ApplyResources(this.qCompositeMenuItem3, "qCompositeMenuItem3");
            // 
            // IntegratedMenu
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.qRibbon1);
            this.Controls.Add(this.qExplorerBar1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.treeViewAdv4);
            this.Controls.Add(this.treeViewAdv3);
            this.Controls.Add(this.treeViewAdv2);
            this.Controls.Add(this.treeViewAdv1);
            this.Name = "IntegratedMenu";
            this.Load += new System.EventHandler(this.IntegratedMenu_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.qExplorerBar1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbon1)).EndInit();
            this.qRibbon1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qRibbonPage5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private Aga.Controls.Tree.TreeViewAdv treeViewAdv1;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Qios.DevSuite.Components.QExplorerBar qExplorerBar1;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem1;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem4;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem5;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem6;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem2;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem8;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem10;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem9;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem11;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem12;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem13;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem3;
        private Qios.DevSuite.Components.QExplorerItem qExplorerItem7;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon1;
        private Aga.Controls.Tree.TreeViewAdv treeViewAdv2;
        private Aga.Controls.Tree.TreeViewAdv treeViewAdv3;
        private Aga.Controls.Tree.TreeViewAdv treeViewAdv4;
        private System.Windows.Forms.ImageList imageList1;
        private Qios.DevSuite.Components.Ribbon.QRibbonItem qRibbonItem2;
        private Qios.DevSuite.Components.QCompositeMenuItem qCompositeMenuItem2;
        private Qios.DevSuite.Components.QCompositeMenuItem qCompositeMenuItem1;
        private Qios.DevSuite.Components.Ribbon.QRibbonItem qRibbonItem1;
        private Qios.DevSuite.Components.Ribbon.QRibbonPanel qRibbonPanel1;
        private Qios.DevSuite.Components.Ribbon.QRibbonPage qRibbonPage1;
        private Qios.DevSuite.Components.Ribbon.QRibbon qRibbon1;
        private Qios.DevSuite.Components.Ribbon.QRibbonPage qRibbonPage2;
        private Qios.DevSuite.Components.Ribbon.QRibbonPanel qRibbonPanel2;
        private Qios.DevSuite.Components.Ribbon.QRibbonPage qRibbonPage3;
        private System.Windows.Forms.TreeView treeView1;
        private Qios.DevSuite.Components.Ribbon.QRibbonPage qRibbonPage4;
        private Qios.DevSuite.Components.Ribbon.QRibbonPage qRibbonPage5;
        private Qios.DevSuite.Components.QCompositeMenuItem qCompositeMenuItem3;
    }
}

