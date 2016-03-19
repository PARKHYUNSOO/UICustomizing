using Aveva.ApplicationFramework;
using Aveva.Marine.Drafting;
using Aveva.Marine.UI;
using Aveva.Pdms.Database;
using Aveva.Pdms.Graphics;
using Aveva.Pdms.Presentation.ExplorerControl;
using Aveva.Pdms.Shared;
using Aveva.PDMS.Database.Filters;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;
using PHS.Utilities.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Presentation = Aveva.ApplicationFramework.Presentation;
namespace PHS.Utilities
{
    public partial class ElementRenameForm : XtraForm
    {
        ExplorerCtrl mDesExpCtrl = null;
        DbElementType[] dbtypes = null;
        DbElement root = null;
        DbElementType site_type = DbElementTypeInstance.SITE;
        DbElementType zone_type = DbElementTypeInstance.ZONE;
        DbElementType pipe_type = DbElementTypeInstance.PIPE;
        DbElementType bran_type = DbElementTypeInstance.BRANCH;
        DbElementType pspool_type = DbElementTypeInstance.PSPOOL;
        DbElementType stru_type = DbElementTypeInstance.STRUCTURE;
        DbElementType frmw_type = DbElementTypeInstance.FRMWORK;
        DbElementType equi_type = DbElementTypeInstance.EQUIPMENT;
        DbElementType block_type = DbElementTypeInstance.BLOCK;
        DbElementType sele_type = DbElementTypeInstance.SELEC;
        DbElementType spco_type = DbElementTypeInstance.SPCOMPONENT;
        DbElementType category_type = DbElementTypeInstance.CATEGORY;
        DbElementType dept_type = DbElementTypeInstance.DEPT;
        DbElementType regi_type = DbElementTypeInstance.REGISTRY;
        DbElementType drwg_type = DbElementTypeInstance.DRWG;
        DbElementType liby_type = DbElementTypeInstance.LIBY;
        DbElementType shee_type = DbElementTypeInstance.SHEET;
      


       

        public ElementRenameForm()
        {
            InitializeComponent();
            
            if (ServiceManager.Instance.ApplicationName == "Outfitting")
            {
                mDesExpCtrl = Presentation.WindowManager.Instance.Windows["DesignExplorer"].Control as ExplorerCtrl;                                
                dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE, DbElementTypeInstance.BRANCH, DbElementTypeInstance.PSPOOL, DbElementTypeInstance.STRUCTURE, DbElementTypeInstance.FRMWORK, DbElementTypeInstance.EQUIPMENT,DbElementTypeInstance.BLOCK };
                root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);
                
                TreeNode sitenode=tree_type.Nodes.Add("SITE");
                TreeNode zonenode=sitenode.Nodes.Add("ZONE");
                TreeNode pipenode = zonenode.Nodes.Add("PIPE");
                TreeNode brannode = pipenode.Nodes.Add("BRAN");
                TreeNode pspoolnode = pipenode.Nodes.Add("PSPOOL");
                TreeNode strunode = zonenode.Nodes.Add("STRU");
                TreeNode frmwnode = strunode.Nodes.Add("FRMW");
                TreeNode equinode = zonenode.Nodes.Add("EQUI");
                TreeNode blocknode = zonenode.Nodes.Add("BLOCK");
                
                
                sitenode.Tag = DbElementTypeInstance.SITE;
                zonenode.Tag = DbElementTypeInstance.ZONE;
                pipenode.Tag = DbElementTypeInstance.PIPE;
                brannode.Tag = DbElementTypeInstance.BRANCH;
                pspoolnode.Tag = DbElementTypeInstance.PSPOOL;
                strunode.Tag = DbElementTypeInstance.STRUCTURE;
                frmwnode.Tag = DbElementTypeInstance.FRMWORK;
                equinode.Tag = DbElementTypeInstance.EQUIPMENT;
                blocknode.Tag = DbElementTypeInstance.BLOCK;

                sitenode.Checked = false ;
                zonenode.Checked = false;
                pipenode.Checked = true;
                brannode.Checked = true;
                pspoolnode.Checked = true;
                strunode.Checked = true;
                frmwnode.Checked = true;
                equinode.Checked = true;
                blocknode.Checked = true;
                


            }

            else if (ServiceManager.Instance.ApplicationName == "Paragon")
            {
                mDesExpCtrl = Presentation.WindowManager.Instance.Windows["CatalogueExplorer"].Control as ExplorerCtrl;
                dbtypes = new DbElementType[] { DbElementTypeInstance.SELEC, DbElementTypeInstance.SPCOMPONENT, DbElementTypeInstance.CATEGORY};
                root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Catalogue);

                TreeNode specnode = tree_type.Nodes.Add("SPEC");
                TreeNode selenode = tree_type.Nodes.Add("SELE");
                TreeNode spconode = tree_type.Nodes.Add("SPCO");
                TreeNode catenode = tree_type.Nodes.Add("CATE");

                specnode.Tag = DbElementTypeInstance.SPECIFICATION;
                selenode.Tag = DbElementTypeInstance.SELEC;
                spconode.Tag = DbElementTypeInstance.SPCOMPONENT;
                catenode.Tag = DbElementTypeInstance.CATEGORY;

                specnode.Checked = true;
                selenode.Checked = true;
                spconode.Checked = true;
                catenode.Checked = true;

            }
            else if (ServiceManager.Instance.ApplicationName == "MarineDrafting")
            {
                mDesExpCtrl = Presentation.WindowManager.Instance.Windows["DraftExplorer"].Control as ExplorerCtrl;
                dbtypes = new DbElementType[] { DbElementTypeInstance.DEPT, DbElementTypeInstance.REGISTRY, DbElementTypeInstance.DRWG, DbElementTypeInstance.LIBY, DbElementTypeInstance.SHEET };
                root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Draft);

                TreeNode deptnode = tree_type.Nodes.Add("DEPT");
                TreeNode reginode = tree_type.Nodes.Add("REGISTRY");
                TreeNode drwgnode = tree_type.Nodes.Add("DRWG");
                TreeNode libynode = tree_type.Nodes.Add("LIBY");
                TreeNode sheenode = tree_type.Nodes.Add("SHEET");

                deptnode.Tag = DbElementTypeInstance.DEPT;
                reginode.Tag = DbElementTypeInstance.REGISTRY;
                drwgnode.Tag = DbElementTypeInstance.DRWG;
                libynode.Tag = DbElementTypeInstance.LIBY;
                sheenode.Tag = DbElementTypeInstance.SHEET;

                deptnode.Checked = true;
                reginode.Checked = true;
                drwgnode.Checked = true;
                libynode.Checked = true;
                sheenode.Checked = true;
            }
            tree_type.ExpandAll();
        }

        private void btnrename_Click(object sender, EventArgs e)
        {
            bool replace_mode=true;
            if (txtsearch_replace.Text.Trim() == "")
            {
                MessageBox.Show("찾을 문자열이 입력되지 않았습니다. 입력후 명령을 수행하세요");
                return;
            }
            else if (txtreplace.Text.Trim() == "")
            {
                MessageBox.Show("바꿀 문자열이 입력되지 않았습니다. 입력후 명령을 수행하세요");
                return;
            }
            Search_Element(txtsearch_replace.Text, replace_mode);
            
            
        }
        DBElementCollection result = null;
        DbElement[] FindElements = null;
        int findindex = 0;
        private void Search_Element(string findstr,bool replace_mode)
        {
            findindex = 0;
            string searchname = "*" + findstr + "*";
            string replacestr = txtreplace.Text;

            //트리에서 Element Type을 선택할수 있게 함.
            List<DbElementType> dbtype_list = new List<DbElementType>();
            foreach (TreeNode firstnode in tree_type.Nodes)
            {
                if (firstnode.Checked)                
                    dbtype_list.Add((DbElementType)firstnode.Tag);
                
                foreach (TreeNode secondnode in firstnode.Nodes)
                {
                    if (secondnode.Checked)
                        dbtype_list.Add((DbElementType)secondnode.Tag);                    
                    foreach (TreeNode thirdnode in secondnode.Nodes)
                    {
                        if (thirdnode.Checked)
                            dbtype_list.Add((DbElementType)thirdnode.Tag);                    
                        foreach (TreeNode forthnode in thirdnode.Nodes)
                        {
                            if (forthnode.Checked)
                                dbtype_list.Add((DbElementType)forthnode.Tag);                   
                            
                        } 
                    } 
                } 
            }            
            dbtypes = dbtype_list.ToArray();
            
            if(radio_3dview.Checked)
            {
                DrawList drawlist = DrawListManager.Instance.CurrentDrawList;
                DrawListMember[] drawlistmems = drawlist.Members();
                DrawListMember [] FindDrawElements = drawlistmems.Cast<DrawListMember>()
                    .Where(item => item.DbElement.GetAsString(DbAttributeInstance.NAMN).Contains(txt_search.Text) && dbtypes.Contains(item.DbElement.GetElementType()))
                   .ToArray();


                FindElements = FindDrawElements.Select(x => x.DbElement).ToArray();
                
                //Highlight시 색은 General Colours에서 Visible의 색을 따라감.
                drawlist.Highlight(FindElements);

                foreach (DbElement item in FindElements)
                {
                    string type = item.GetAsString(DbAttributeInstance.TYPE);
                    string pos = "";
                    if (type == "BRAN")
                        pos = item.GetAsString(DbAttributeInstance.HPOS);
                    else
                        pos = item.GetAsString(DbAttributeInstance.POS);

                    string name = item.GetAsString(DbAttributeInstance.NAMN);                    
                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("AID TEXT number 9898   |{0}| at {1}", name, pos)).RunInPdms();
                }
                

                
            }
            else 
            {
                //Root가 CE일때
                if (radio_ce.Checked)
                    root = CurrentElement.Element;
                else if (radio_entire.Checked)
                {
                    if (ServiceManager.Instance.ApplicationName == "Outfitting")
                        root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);
                    if (ServiceManager.Instance.ApplicationName == "Paragon")
                        root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Catalogue);
                    if (ServiceManager.Instance.ApplicationName == "MarineDrafting")
                        root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Draft);
                }
                TypeFilter typefilter = new TypeFilter(dbtypes);
                AndFilter finalfilter = new AndFilter();
                
                AttributeLikeFilter xfilter = new AttributeLikeFilter(DbAttributeInstance.NAMN, searchname);

                finalfilter.Add(typefilter);
                finalfilter.Add(xfilter);
                result = new DBElementCollection(root, finalfilter);
                FindElements = result.Cast<DbElement>().ToArray();
            }


            //Item 들이 가지고 속해있는 DB리스트생성
            List<Db> dblist = new List<Db>();
            foreach (DbElement item in FindElements)
            {
                if (!dblist.Contains(item.Db))
                {
                    dblist.Add(item.Db);
                }
            }
            //Partial Getwork수행
            MDB.CurrentMDB.GetWork(dblist.ToArray());




            int replace_count=0;
            if (replace_mode == true)
            {

                foreach (DbElement item in FindElements)
                {
                    try
                    {
                        //listView_searchresult.Items.Add(new ListViewItem(new string[]{"1","2","3","4"}));
                        
                        string element_name = item.GetAsString(DbAttributeInstance.NAME);
                        
                        string name = element_name.Replace(findstr, replacestr);
                        DbElement exsititem= DbElement.GetElement(name);
                        if (!exsititem.IsNull)
                        {
                            MessageBox.Show(string.Format("바꿀려는 이름 {0}은 존재하므로 바꿀수 없습니다.",name));
                            return;
                            continue;
                        }
                        
                        item.SetAttribute(DbAttributeInstance.NAME, name);
                        replace_count++;
                        mDesExpCtrl.RefreshNodes(item);
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(item.ToString());
                        Console.WriteLine("오류발생");
                    }
                }
                //Tree View 업데이트 부분 아래 구문이 없으면 화면이 업데이트 안됨.
                try
                {
                    var windows = Aveva.ApplicationFramework.Presentation.WindowManager.Instance.Windows.OfType<Presentation.DockedWindow>().Where(x => x.Key == "DesignExplorer").ToArray();
                    if (windows.Count() > 0)
                    {
                        
                        MessageBox.Show(replace_count+"개 항목의 이름 바꾸기 완료!");

                    }
                }
                catch (Exception ee)
                {

                }
            }
            else
            {

                if (!radio_3dview.Checked)
                {
                    if (FindElements.Count() != 0)
                    {
                        string refno = FindElements[0].GetAsString(DbAttributeInstance.REF);
                        Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(refno).RunInPdms();
                    }
                }
                int idx = 1;
                //listView_searchresult.ColumnClick -= listView_searchresult_ColumnClick;
                listView_searchresult.Items.Clear();
                foreach (DbElement item in FindElements)
                {
                    string elementname = item.GetAsString(DbAttributeInstance.FLNN);
                    string elementtype = item.GetAsString(DbAttributeInstance.TYPE);
                    string marptype="0";
                    if (elementtype == "SHEE")
                        marptype = item.GetAsString(DbAttributeInstance.MARPTY);
                    bool lclm = item.GetBool(DbAttributeInstance.LCLM);
                    string userclaim = item.GetAsString(DbAttributeInstance.USERC);
                    string islock = "X";
                    if (lclm==true)                    
                        islock = "X";                    
                    else
                    {
                        if (userclaim == "unset")
                            islock = "X";
                        else
                            islock = "O";
                    }
                    
                    listView_searchresult.Items.Add(new ListViewItem(new string[] { idx.ToString(), elementname, elementtype,islock,marptype }));

                    idx++;
                }
                //listView_searchresult.ColumnClick += listView_searchresult_ColumnClick;
                lbl_result.Text = "검색결과 : " + FindElements.Count().ToString() + " 건";
                

            }
            
        }

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(e.KeyData== Keys.Enter)
            {
                if (txt_search.Text.Trim()=="")
                {
                    MessageBox.Show("입력된 문자열이 없습니다. 입력후 검색해주세요");
                    return;

                }
                Search_Element(txt_search.Text,false);
            }
        }

        private void btn_pre_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (FindElements == null)
                {
                    MessageBox.Show("검색된 결과가 없습니다.");
                    return;
                }
                if (findindex == 0)
                {
                    MessageBox.Show("맨 처음입니다.");

                    return;
                }
                string refno = FindElements[findindex - 1].GetAsString(DbAttributeInstance.REF);
                if (!radio_3dview.Checked)
                {

                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(refno).RunInPdms();
                }
                else
                {

                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("AUTO " + refno).RunInPdms();
                }





                findindex--;
            }catch(Exception ee)
            {
                Console.WriteLine("11");
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            try
            {
                if (FindElements == null)
                {
                    MessageBox.Show("검색된 결과가 없습니다.");
                    return;
                }
                if (FindElements.Count() == 0)
                {
                    MessageBox.Show("검색된 결과가 없습니다.");
                    return;
                }
                if (findindex == FindElements.Count() - 1)
                {
                    MessageBox.Show("맨 마지막입니다.");
                    return;
                }

                string refno = FindElements[findindex + 1].GetAsString(DbAttributeInstance.REF);
                
                if (!radio_3dview.Checked)
                {

                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(refno).RunInPdms();
                }
                else
                {
                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("AUTO " + refno).RunInPdms();

                }


                findindex++;
            }catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        private void Mode_Change(object sender, EventArgs e)
        {
            if (radio_3dview.Checked)
            {
                tree_type.Nodes.Clear();

                TreeNode pipenode = tree_type.Nodes.Add("PIPE");
                TreeNode brannode = pipenode.Nodes.Add("BRAN");
                TreeNode strunode = tree_type.Nodes.Add("STRU");
                TreeNode equinode = tree_type.Nodes.Add("EQUI");
                



                pipenode.Tag = DbElementTypeInstance.PIPE;
                brannode.Tag = DbElementTypeInstance.BRANCH;
                strunode.Tag = DbElementTypeInstance.STRUCTURE;
                equinode.Tag = DbElementTypeInstance.EQUIPMENT;
                


                pipenode.Checked = true;
                brannode.Checked = true;
                strunode.Checked = true;
                equinode.Checked = true;
                

            }
            else
            {
                tree_type.Nodes.Clear();
                if (ServiceManager.Instance.ApplicationName == "Outfitting")
                {
                    mDesExpCtrl = Presentation.WindowManager.Instance.Windows["DesignExplorer"].Control as ExplorerCtrl;
                    dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE, DbElementTypeInstance.BRANCH, DbElementTypeInstance.PSPOOL, DbElementTypeInstance.STRUCTURE, DbElementTypeInstance.FRMWORK, DbElementTypeInstance.EQUIPMENT, DbElementTypeInstance.BLOCK };
                    root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);

                    TreeNode sitenode = tree_type.Nodes.Add("SITE");
                    TreeNode zonenode = sitenode.Nodes.Add("ZONE");
                    TreeNode pipenode = zonenode.Nodes.Add("PIPE");
                    TreeNode brannode = pipenode.Nodes.Add("BRAN");
                    TreeNode pspoolnode = pipenode.Nodes.Add("PSPOOL");
                    TreeNode strunode = zonenode.Nodes.Add("STRU");
                    TreeNode frmwnode = strunode.Nodes.Add("FRMW");
                    TreeNode equinode = zonenode.Nodes.Add("EQUI");
                    TreeNode blocknode = zonenode.Nodes.Add("BLOCK");


                    sitenode.Tag = DbElementTypeInstance.SITE;
                    zonenode.Tag = DbElementTypeInstance.ZONE;
                    pipenode.Tag = DbElementTypeInstance.PIPE;
                    brannode.Tag = DbElementTypeInstance.BRANCH;
                    pspoolnode.Tag = DbElementTypeInstance.PSPOOL;
                    strunode.Tag = DbElementTypeInstance.STRUCTURE;
                    frmwnode.Tag = DbElementTypeInstance.FRMWORK;
                    equinode.Tag = DbElementTypeInstance.EQUIPMENT;
                    blocknode.Tag = DbElementTypeInstance.BLOCK;

                    sitenode.Checked = false;
                    zonenode.Checked = false;
                    pipenode.Checked = true;
                    brannode.Checked = true;
                    pspoolnode.Checked = true;
                    strunode.Checked = true;
                    frmwnode.Checked = true;
                    equinode.Checked = true;
                    blocknode.Checked = true;



                }

                else if (ServiceManager.Instance.ApplicationName == "Paragon")
                {
                    mDesExpCtrl = Presentation.WindowManager.Instance.Windows["CatalogueExplorer"].Control as ExplorerCtrl;
                    dbtypes = new DbElementType[] { DbElementTypeInstance.SELEC, DbElementTypeInstance.SPCOMPONENT, DbElementTypeInstance.CATEGORY };
                    root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Catalogue);

                    TreeNode specnode = tree_type.Nodes.Add("SPEC");
                    TreeNode selenode = tree_type.Nodes.Add("SELE");
                    TreeNode spconode = tree_type.Nodes.Add("SPCO");
                    TreeNode catenode = tree_type.Nodes.Add("CATE");

                    specnode.Tag = DbElementTypeInstance.SPECIFICATION;
                    selenode.Tag = DbElementTypeInstance.SELEC;
                    spconode.Tag = DbElementTypeInstance.SPCOMPONENT;
                    catenode.Tag = DbElementTypeInstance.CATEGORY;

                    specnode.Checked = true;
                    selenode.Checked = true;
                    spconode.Checked = true;
                    catenode.Checked = true;

                }
                else if (ServiceManager.Instance.ApplicationName == "MarineDrafting")
                {
                    mDesExpCtrl = Presentation.WindowManager.Instance.Windows["DraftExplorer"].Control as ExplorerCtrl;
                    dbtypes = new DbElementType[] { DbElementTypeInstance.DEPT, DbElementTypeInstance.REGISTRY, DbElementTypeInstance.DRWG, DbElementTypeInstance.LIBY, DbElementTypeInstance.SHEET };
                    root = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Draft);

                    TreeNode deptnode = tree_type.Nodes.Add("DEPT");
                    TreeNode reginode = tree_type.Nodes.Add("REGISTRY");
                    TreeNode drwgnode = tree_type.Nodes.Add("DRWG");
                    TreeNode libynode = tree_type.Nodes.Add("LIBY");
                    TreeNode sheenode = tree_type.Nodes.Add("SHEET");

                    deptnode.Tag = DbElementTypeInstance.DEPT;
                    reginode.Tag = DbElementTypeInstance.REGISTRY;
                    drwgnode.Tag = DbElementTypeInstance.DRWG;
                    libynode.Tag = DbElementTypeInstance.LIBY;
                    sheenode.Tag = DbElementTypeInstance.SHEET;

                    deptnode.Checked = true;
                    reginode.Checked = true;
                    drwgnode.Checked = true;
                    libynode.Checked = true;
                    sheenode.Checked = true;
                }
            }

            tree_type.ExpandAll();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex==0)
            {
                radio_3dview.Visible = true;
            }
            else
            {
                radio_3dview.Visible = false;
                if(radio_3dview.Checked)
                {
                    radio_entire.Checked = true;
                }
            }
        }

        private void lbl_highlightoff_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("unenhance all aid clear text 9898").RunInPdms();
        }

        private void ElementRenameForm_Load(object sender, EventArgs e)
        {
            OfficeSkins.Register();
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            defaultLookAndFeel1.LookAndFeel.SkinName = "McSkin";
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            if (txt_search.Text.Trim() == "")
            {
                MessageBox.Show("입력된 문자열이 없습니다. 입력후 검색해주세요");
                return;

            }
            Search_Element(txt_search.Text, false);

            splashScreenManager1.CloseWaitForm();
        }

        private void listView_searchresult_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                splashScreenManager1.ShowWaitForm();
                
                string dwgname = listView_searchresult.SelectedItems[0].SubItems[1].Text;
                string marptype = listView_searchresult.SelectedItems[0].SubItems[4].Text;
                if (marptype=="0")
                {
                    return;
                }
                MarDrafting kcs_draft = new MarDrafting();
                MarUi kcs_ui = new MarUi();

                if (kcs_draft.DwgCurrent())
                {
                    kcs_draft.DwgClose();
                    kcs_draft.DwgOpen(dwgname,Convert.ToDouble(marptype),0);
                }
                else
                {
                    kcs_draft.DwgOpen(dwgname, Convert.ToDouble(marptype), 0);
                }
                kcs_ui.AppWindowRefresh();
                
            }catch(Exception ee)
            {

            }
            finally
            {
                splashScreenManager1.CloseWaitForm();
            }
        }

        private void listView_searchresult_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //정렬을 위하여 사용 됨.
            if (this.listView_searchresult.Sorting == SortOrder.Ascending || listView_searchresult.Sorting == SortOrder.None)
            {
                this.listView_searchresult.ListViewItemSorter = new ListViewItemComparer(e.Column, "desc");
                listView_searchresult.Sorting = SortOrder.Descending;
            }
            else
            {
                this.listView_searchresult.ListViewItemSorter = new ListViewItemComparer(e.Column, "asc");
                listView_searchresult.Sorting = SortOrder.Ascending;
            }

            listView_searchresult.Sort();
        }
    }
}
