using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aveva.Pdms.Database;
using Aveva.ApplicationFramework.Presentation;
namespace PHS.Utilities
{

    //AM모듈을 다시 띄우지 않고 프로젝트,MDB,User를 바꿀수 있는 기능.
    public partial class ProjectChange : Form
    {
        public ProjectChange()
        {

            InitializeComponent();
            

        }
        public ProjectChange(string username)
        {
            this.username = username;
            if (username == "" || username == "Null Element")
            {
                username = System.Environment.UserName;
            }
            InitializeComponent();
            txtID.Text = username;
            txtPW.Text = "HMD" + username.Substring(1,6);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        string orgmdbname = "";
        string project = "";
        string username = "";
        public bool run()
        {
            

            if (DialogResult.Yes == MessageBox.Show("저장하시겠습니까??", "MDB변경", MessageBoxButtons.YesNo))
            {
                MDB.CurrentMDB.SaveWork("MDB변환");
            }
            else
            {
                
                //MDB.CurrentMDB.ReleaseAll();
                return false;
            }
            
            
            Project curproject = Project.CurrentProject;

            string projectname = curproject.Name;
            orgmdbname = MDB.CurrentMDB.Name;
            //curproject.Open("408506", "SYSTEM", "XXXXXX");
            //MDB mdb = Project.OpenMDB(MDBSetup.CreateMDBSetup("/MIGRATION"));

            treeProject.Nodes.Clear();

            var projectlist = Aveva.Pdms.PdmsApplication.ProjectList();
            foreach (string proj in projectlist.Keys.OfType<string>().OrderBy(x=>x))
            {
                if ((String)projectlist[proj] == projectname)
                    project = proj;
                treeProject.Nodes.Add(proj, proj + "(" + projectlist[proj].ToString() + ")");
                treeProject.Nodes[proj].Text += string.Empty;
                treeProject.Nodes[proj].ImageIndex = 0;
                treeProject.Nodes[proj].ForeColor = Color.Green;
                treeProject.Nodes[proj].NodeFont = new Font("굴림", 11.0f, FontStyle.Bold);
                treeProject.Nodes[proj].Text += string.Empty;
                string projid = (string)projectlist[proj];
                //Project.CurrentProject.GetMdbData();
                
                //연결하는 순간 Design Explorer의 Element가 초기화된다.
                string[] mdblist = Aveva.Pdms.Database.DatabaseService.ProjectMdbData(projid);


                string[] userlist = DatabaseService.ProjectUserData(projid);

                foreach (string mdb in mdblist)
                {

                    treeProject.Nodes[proj].Nodes.Add(mdb, mdb);
                    treeProject.Nodes[proj].Nodes[mdb].ToolTipText = mdb;
                    treeProject.Nodes[proj].Nodes[mdb].NodeFont = new Font("굴림", 9.0f);
                    treeProject.Nodes[proj].Nodes[mdb].ImageIndex = 1;
                }
            }
            //if (orgmdbname != "Null Element")
            //{
            //    curproject.Open(project, "SYSTEM", "XXXXXX");

            //    Project.OpenMDB(MDBSetup.CreateMDBSetup("/"+orgmdbname));
            //}
            return true;
        }

        private void treeProject_DoubleClick(object sender, EventArgs e)
        {
            if (treeProject.SelectedNode.Level == 1)            
            {
                Project curproject = Project.CurrentProject;
                if (DialogResult.Yes == MessageBox.Show("선택한 프로젝트의 MDB로 전환하시겠습니까??", "Project전환기능", MessageBoxButtons.YesNo))
                {

                    splashScreenManager1.ShowWaitForm();
                    try
                    {
                        project = treeProject.SelectedNode.Parent.Name;
                        string mdbname = treeProject.SelectedNode.Name.Split(' ')[0];

                        curproject.Open(project, txtID.Text, txtPW.Text);
                        if (curproject.IsLoggedIn())
                        {
                            
                            MDB mdb = Project.OpenMDB(MDBSetup.CreateMDBSetup(mdbname));
                        }
                        else
                        {
                            MessageBox.Show("해당계정으로 로그인할수 없습니다.");
                        }
                        
                        //Title Bar 변경.
                        WindowManager.Instance.MainForm.Text = string.Format("[Project-{0}, MDB-{1}]AM Outfitting ",project,mdbname);
                        splashScreenManager1.CloseWaitForm();
                        MessageBox.Show("프로젝트/MDB변경이 완료되었습니다.");
                    }catch(Exception ee)
                    {
                        splashScreenManager1.CloseWaitForm();
                        MessageBox.Show("해당 프로젝트의 MDB설정 오류로 인해서 MDB를 변경할수없습니다.");
                    }
                    //curproject.Open("2531VR", "SYSTEM", "XXXXXX");
                    //MDB mdb = Project.OpenMDB(MDBSetup.CreateMDBSetup("/HULL"));
                    
                }
                else
                {
                    
                    
                }
            }
        }
    }
}
