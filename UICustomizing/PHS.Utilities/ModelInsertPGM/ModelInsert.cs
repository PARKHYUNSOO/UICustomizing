using Aveva.ApplicationFramework;
using Aveva.Marine.Utility;
using Aveva.Pdms.Clasher;
using Aveva.Pdms.Database;
using Aveva.Pdms.Geometry;
using Aveva.Pdms.Graphics;
using Aveva.Pdms.PipeFabrication.FabricationCheck;
using Aveva.Pdms.Piping.Fabrication;
using Aveva.Pdms.Presentation.ExplorerControl;
using Aveva.Pdms.Shared;
using Aveva.Pdms.Utilities.CommandLine;
using Aveva.PDMS.Database.Filters;
using HookedAp;
using PHS.Utilities.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Presentation = Aveva.ApplicationFramework.Presentation;
using CLASHER = Aveva.Pdms.Clasher.Clasher;
using Aveva.Pdms.Clasher.Implementation;
using Aveva.Marine.DataExtraction;
using Aveva.Marine.Drafting;
using Aveva.Marine.UI;
using Aveva.Marine.Geometry;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.UserSkins;
using DevExpress.Skins;
using PHS.Utilities.RadialMenuControl;
using Aveva.Pdms.Presentation.PDMSCommands;

namespace PHS.Utilities.ModelInsertPGM
{
    public partial class ModelInsert : XtraForm
    {
        public ModelInsert()
        {
            InitializeComponent();
            this.KeyPreview = true;
            
        }


        //DrawListManager drawListManager = DrawListManager.Instance;
        private void btnModelInsert_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            splashScreenManager1.ShowWaitForm();
            
            InserModel();
            splashScreenManager1.CloseWaitForm();
            //progressPanel1.Visible = true;
            //backgroundWorker1.RunWorkerAsync("InsertModel");
            //backgroundWorker1.RunWorkerCompleted+=backgroundWorker1_RunWorkerCompleted;

            //this.Visible = true;

        }

        private void InserModel()
        {
            if (ServiceManager.Instance.ApplicationName.ToUpper() == "OUTFITTING")
            {
                DrawListManager.Instance.BeginUpdate();
                DrawListManager.Instance.CurrentDrawList.Add(TotalModelList.ToArray());
                DrawListManager.Instance.EndUpdate();
                DrawListManager.Instance.CurrentDrawList.VisibleAll();
            }            
            else if (ServiceManager.Instance.ApplicationName.ToUpper() == "MARINEDRAFTING")
            {
                var currentCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                if (TotalModelList.Count != 0)
                {
                    Aveva.Pdms.Shared.Selection.CurrentSelection.Members =
                        TotalModelList.Cast<ExplorerTreeNode>().Select(x => x.Element).ToArray();
                }

                var command =  Aveva.ApplicationFramework.Presentation.CommandManager.Instance.Commands["AVEVA.Marine.UI.Menu.GeneralAddToInsertModel"];

                command.Execute();

                
                Cursor.Current = currentCursor;
            }
            





            
            
            //drawListManager.BeginUpdate();
            //var currentDrawList = drawListManager.CurrentDrawList;
            //drawListManager.CurrentDrawList.Add(TotalModelList.ToArray());

            //drawListManager.EndUpdate();
            //currentDrawList.VisibleAll();
        }

        DbElement [] pipe_collection = new DbElement[] { };
        DbElement[] stru_collection = new DbElement[] { };
        DbElement[] equip_collection = new DbElement[] { };
        DbElement[] hull_collection = new DbElement[] { };
        DbElement[] rso_collection = new DbElement[] { };

        private void SearchModel()
        {
            pipe_collection = new DbElement[] { };
            stru_collection = new DbElement[] { };
            equip_collection = new DbElement[] { };
            hull_collection = new DbElement[] { };
            rso_collection = new DbElement[] { };
            DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);

            DbElementType[] dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE };
            if (checkPipe.Checked)
            {
                dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE };
                pipe_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_pipe_name.Text, txt_pipe_module.Text);
                
            }
            if (checkStru.Checked)
            {
                dbtypes = new DbElementType[] { DbElementTypeInstance.STRUCTURE };
                stru_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_stru_name.Text, txt_stru_module.Text);
                
            }
            if (checkEquip.Checked)
            {
                dbtypes = new DbElementType[] { DbElementTypeInstance.EQUIPMENT };
                equip_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_equip_name.Text, txt_equip_module.Text);
                //currentDrawList.Add(equip_collection.Cast<DbElement>().ToArray());
            }
            if (check_hull.Checked)
            {
                dbtypes = new DbElementType[] { DbElementTypeInstance.CPANEL,DbElementTypeInstance.HPANEL };
                hull_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_hull_name.Text, txt_hull_module.Text);
                //currentDrawList.Add(equip_collection.Cast<DbElement>().ToArray());
            }
            if (checkrso.Checked)
            {
                dbtypes = new DbElementType[] { DbElementTypeInstance.HRSO };
                rso_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_rso_name.Text, "");
            }
        }

        private DbElement[] Get_Element_Collection(DbElement Outfit_Elements,DbElementType [] dbtypes,string search_txt,string modulename)
        {
            DBElementCollection result_collection = null;
            DbElement [] result2_collection = null;
            try
            {
                
                TypeFilter filter = new TypeFilter(dbtypes);
                AndFilter finalfilter = new AndFilter();

                AttributeStringFilter filter2 = null;
                AttributeRefFilter filter3 = null;


                search_txt = search_txt.Trim();
                //model name
                if (search_txt!= "")
                {
                    if (search_txt.Substring(0, 1) != "*")
                        search_txt = "^" + search_txt;
                    if(search_txt.Substring(search_txt.Length-1,1)!="*")
                        search_txt = search_txt+"$";
                    search_txt = search_txt.Replace(".", "\\.");
                    search_txt = search_txt.Replace("*", ".*");
                    search_txt = search_txt.Replace("?", ".");
                    //AttributeLikeFilter xfilter = new AttributeLikeFilter(DbAttributeInstance.NAMN, search_txt.Trim());
                    AttributeStringFilter xfilter = new AttributeStringFilter(DbAttributeInstance.NAMN, FilterOperator.MatchesRegularExpression, search_txt);
                    
                    
                    finalfilter.Add(xfilter);
                    //BelowFilter bfilter = new BelowFilter(xfilter);
                    
                    //if (search_txt.StartsWith("*") && search_txt.EndsWith("*"))
                    //{
                    //    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.EndsWith, search_txt.Replace("*", ""));

                    //}
                    //else if (search_txt.EndsWith("*") && !search_txt.StartsWith("*"))
                    //{
                    //    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.StartsWith, search_txt.Replace("*", ""));
                    //}
                    //else if (search_txt.StartsWith("*") && search_txt.EndsWith("*"))
                    //{
                    //    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.Contains, search_txt.Replace("*", ""));
                    //}
                    ////else if ( search_txt.EndsWith("?"))
                    ////{
                    ////    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.MatchesRegularExpression, search_txt);
                    ////}

                    //else
                    //{
                    //    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.Equals, search_txt.Replace("*", ""));
                    //}
                    //finalfilter.Add(filter2);
                }

                //module name
                //if(modulename.Trim()!="")
                //{


                //    filter3=new AttributeRefFilter(DbAttributeInstance.OWNER, FilterOperator.Equals, DbElement.GetElement("/"+modulename));
                //    finalfilter.Add(filter3);    
                //}
                Stopwatch st = new Stopwatch();
                st.Start();
                
                st.Stop();
                Console.WriteLine("11/"+st.ElapsedMilliseconds.ToString());
                
                st.Start();
                if (modulename.Trim() != "")
                {
                    modulename=modulename.Trim();
                    if (modulename.Substring(0, 1) != "*")
                        modulename = "^" + modulename;
                    if (modulename.Substring(modulename.Length - 1, 1) != "*")
                        modulename = modulename + "$";
                    modulename = modulename.Replace(".", "\\.");
                    modulename = modulename.Replace("*", ".*");
                    modulename = modulename.Replace("?", ".");
                    //AttributeLikeFilter modulefilter = new AttributeLikeFilter(DbAttributeInstance.NAMN, modulename.Trim());
                    AttributeStringFilter modulefilter = new AttributeStringFilter(DbAttributeInstance.NAMN, FilterOperator.MatchesRegularExpression, modulename);
                    BelowFilter filter4 = new BelowFilter(modulefilter);
                    

                    finalfilter.Add(filter4);
                    
                    //string Convert_Searchtxt = getconvert_Regexp(modulename);
                    //result2_collection = result_collection.Cast<DbElement>().Where(x => Regex.IsMatch(x.Owner.GetAsString(DbAttributeInstance.NAMN), Convert_Searchtxt, RegexOptions.IgnoreCase) == true).ToArray();
                }
                finalfilter.Add(filter);
                result_collection = new DBElementCollection(Outfit_Elements, finalfilter);
                result2_collection = result_collection.Cast<DbElement>().ToArray();
                st.Stop();
                Console.WriteLine("22 /" + st.ElapsedMilliseconds.ToString());
                //Regex.IsMatch
            }

            catch (Exception ee)
            {
                result2_collection = new DbElement[] { };

                Console.WriteLine("오류");
            }

            return result2_collection;
        }
        string getconvert_Regexp(string searchtxt)
        {
            string temp = searchtxt;

            if (temp.Contains("("))
                temp=temp.Replace("(", @"\(");
            if (temp.Contains(")"))
                temp=temp.Replace(")", @"\)");
            if (temp.Substring(0, 1) != "?" && temp.Substring(0, 1) != "*")
            {
                temp = @"^" + temp;
            }
            if (temp.Substring(temp.Length - 1, 1) != "?" && temp.Substring(temp.Length - 1, 1) != "*")
            {
                temp = temp + @"$";
            }
            if (temp.Contains("."))
            {
                temp = temp.Replace(".", @"\.");
            }
            if (temp.Substring(0, 1) == "?")
            {
                temp = @"^." + temp.Substring(1);

            }
            if (temp.Substring(temp.Length - 1, 1) == "?")
            {
                temp = temp.Substring(0, temp.Length - 1) + @".$";
            }
            if (temp.Contains("?"))
            {
                temp = temp.Replace("?", ".");
            }
            if (temp.Contains("*"))
            {
                temp = temp.Replace("*", @"\S*");
            }
            return temp;
        }


        List<DbElement> TotalModelList = new List<DbElement>();
        public List<DbElement> VerifyModelList = new List<DbElement>(); 
        public void xxX()
        {

        }
        private void btnVerify_Click(object sender, EventArgs e)
        {

            //Drawlist 정의하고 추가하는 부분
            VerifyModelList.Clear();
            try
            {
                splashScreenManager1.ShowWaitForm();
                SearchModel();
                splashScreenManager1.CloseWaitForm();
                ModelInsertVerify verifyform = new ModelInsertVerify(this, pipe_collection, stru_collection, equip_collection, hull_collection, rso_collection);
                verifyform.TopMost = true;
                verifyform.BringToFront();
                if (verifyform.ShowDialog() == DialogResult.OK)
                {

                }
                else
                {

                }
                TotalModelList.AddRange(VerifyModelList.Cast<DbElement>().Where(x => !TotalModelList.Contains(x) && x.IsNull == false).ToArray());
                int modelcnt = TotalModelList.Count();
                lblcnt.Text = "모델 수량 : " + modelcnt.ToString();
            }
            catch (Exception ee)
            {

            }

            
            
        }

        


        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Drawlist 정의하고 추가하는 부분
            splashScreenManager1.ShowWaitForm();

            var currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            SearchModel();
            
            TotalModelList.AddRange(pipe_collection.Cast<DbElement>().Where(x=> !TotalModelList.Contains(x) && x.IsNull == false).ToArray());
            TotalModelList.AddRange(stru_collection.Cast<DbElement>().Where(x => !TotalModelList.Contains(x) && x.IsNull==false).ToArray());
            TotalModelList.AddRange(equip_collection.Cast<DbElement>().Where(x => !TotalModelList.Contains(x) && x.IsNull == false).ToArray());
            TotalModelList.AddRange(hull_collection.Cast<DbElement>().Where(x => !TotalModelList.Contains(x) && x.IsNull == false).ToArray());
            TotalModelList.AddRange(rso_collection.Cast<DbElement>().Where(x => !TotalModelList.Contains(x) && x.IsNull == false).ToArray());
            int modelcnt = TotalModelList.Count();


            lblcnt.Text = "모델 수량 : " + modelcnt.ToString();            

            Cursor.Current = currentCursor;
            splashScreenManager1.CloseWaitForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblcnt.Text = "모델 수량 : 0";
            TotalModelList.Clear();
        }
        public static Control FindFocusedControl(Control control)
        {
            var container = control as ContainerControl;
            return (null != container
                ? FindFocusedControl(container.ActiveControl)
                : control);
        }
        private void ModelInsert_KeyDown(object sender, KeyEventArgs e)
        {
            var focused = FindFocusedControl(this);
            if (focused.GetType().Name != "TextBox")
            {
                //   vConsole.WriteLine(focused);
                Console.WriteLine(e.KeyData.ToString());
                //if (e.KeyData==Keys.I)
                //{
                //    checkPipe.Checked = !checkPipe.Checked;
                //    checkPipe.Focus();
                //}
                //else if (e.KeyData == Keys.H)
                //{
                //    check_hull.Checked = !check_hull.Checked;
                //    check_hull.Focus();
                //}
                //else if (e.KeyData == Keys.R)
                //{
                //    checkStru.Checked = !checkStru.Checked;
                //    checkStru.Focus();
                //}
                //else if (e.KeyData == Keys.Q)
                //{
                //    checkEquip.Checked = !checkEquip.Checked;
                //    checkEquip.Focus();
                //}
                if (e.KeyData == Keys.C)
                {
                    btnClear_Click(null, null);
                }
                else if (e.KeyData == Keys.A)
                {
                    btnSearch_Click(null, null);
                }
                else if (e.KeyData == Keys.Y)
                {
                    btnVerify_Click(null, null);
                }
                else if (e.KeyData == Keys.Enter)
                {
                    btnModelInsert_Click(null, null);
                }
                
            }
            if (e.KeyData == Keys.Escape)                
                this.Close();



        }
        //private Win32Hook m_oHookManager = new Win32Hook();
        private void ModelInsert_Load(object sender, EventArgs e)
        {
            

            OfficeSkins.Register();
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            defaultLookAndFeel1.LookAndFeel.SkinName = "McSkin";
            //DbPostElementChange.AddPostAttributeChange(DbAttributeInstance.LSTU, new DbPostElementChange.PostAttributeChangeDelegate(Setting_FlangeOffset));

            Selection.SelectionChanged -= Selection_SelectionChanged;
            
            DrawListManager.Changed +=DrawListManager_Changed;

            SendKeys.Send("ALT");
            
           
        }

        void DrawListManager_Changed(object sender, DrawListEventArgs e)
        {
            
            
            //Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p |drawlist cheanged|").RunInPdms();
        }

        void Selection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Selection.CurrentSelection.
            
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p |selection change|").RunInPdms();
            

        }


        private void ModelInsert_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {

            //PipeSpoolManager.GeneratePipeSpoolsForPipe(CurrentElement.Element);
            
            
            //PipePieceManager.GeneratePipePieces(CurrentElement.Element);


            //DbElement[] elements = drawListManager.CurrentDrawList.GetWithinVolume(DbElement.GetElement("ABOX 2 of AREADEF /abce"));
            Console.WriteLine("1");
            
           
        }

        private void button2_Click(object sender, EventArgs e)
        {

            
               
            //PipePieceManager.RemovePipePieces(CurrentElement.Element);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("!!a = |parkmaker|").RunInPdms();
            Command cmd1 = Command.CreateCommand("!!ALL1 = !!USERINFO[2]");
            cmd1.RunInPdms();
            Command test = Command.CreateCommand("Q VAR !!ALL1");
            test.RunInPdms();
            string name = cmd1.GetPMLVariableString("ALL1");

            Console.WriteLine(name);


            //drawListManager.getDrawList(0);
            //drawListManager.CurrentDrawList.Add(CurrentElement.Element);
            ////DrawListMember drawmember= drawListManager.CurrentDrawList.GetDrawListMember(CurrentElement.Element);

            //var colour=Aveva.Pdms.Graphics.Colours.FindPdmsColourByIndex(20);
            //drawListManager.CurrentDrawList.SetColour(drawListManager.CurrentDrawList.Members(), 16);
            //drawListManager.getViewDrawList(1).ClearGraphicalSelection();

        }
        
        private void button4_Click(object sender, EventArgs e)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //PHS_lib xx = new PHS_lib();
            //xx.selectByIndicate(1);
            //xx.Configure_Model_Information();
            //xx.modeldraw();
            //Console.WriteLine("1");


            //PHS_lib xx = new PHS_lib();
            //xx.selectByIndicate(0);
            //xx.Configure_Model_Information();

            //DbElement de = DbElement.GetElement("/AAA");
            //de.Create(0, DbElementTypeInstance.FRMWORK);
            //de.SetAttribute(DbAttributeInstance.NAME, "111");


            //Command.CreateCommand(" new sctn /xxxstruct").RunInPdms();

            #region 볼륨내에 있는 애들 가져오기

            //DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);
            ////DbElement Outfit_Elements = DbElement.GetElement("/GG_TEST");
            ////DbElementType[] dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE, DbElementTypeInstance.BRANCH,DbElementTypeInstance.PSPOOL };
            //DbElementType[] dbtypes = new DbElementType[] { DbElementTypeInstance.BRANCH };
            //DBElementCollection result_collection = null;
            //TypeFilter filter = new TypeFilter(dbtypes);
            
            //AndFilter finalfilter = new AndFilter();
            //////AttributeStringFilter filter2 = new AttributeStringFilter(DbAttributeInstance.NAME, FilterOperator.Contains, "park");
            //////AttributeLikeFilter filter2 = new AttributeLikeFilter(DbAttributeInstance.NAME, "park");
            //////AttributeStringFilter filter2 = null;
            //////AttributeRefFilter filter3 = null;


            //InVolumeFilter volfilter = new InVolumeFilter(CurrentElement.Element, dbtypes, false);


            ////finalfilter.Add(filter);
            ////// finalfilter.Add(filter2);
            //finalfilter.Add(volfilter);
            //result_collection = new DBElementCollection(Outfit_Elements, finalfilter);
            //var abc=result_collection.OfType<DbElement>().ToArray();
            #endregion
            //Console.WriteLine("1");
            //Db[] dbChk = MDB.CurrentMDB.GetDBArray(Aveva.Pdms.Database.DbType.Design);
            //foreach (Db db in dbChk)
            //{
            //    SpatialMapStatus stat = SpatialMap.Instance.CheckSpatialMap(db);
            //}

            //sw.Reset();
            //sw.Start();
            // Build spatial maps for MDB
            //try
            //{
            //    SpatialMap.Instance.BuildSpatialMap();
                
            //}
            //catch (Exception ex)
            //{

            //}
            //sw.Stop();
            
            //SpatialMap.Instance.BuildSpatialMap();
            
            //MessageBox.Show("걸린시간1:" + sw.ElapsedMilliseconds.ToString());
            //sw.Reset();
           // sw.Start();
            //

            //ClashOptions opt = ClashOptions.Create();
            //opt.IncludeTouches = true;
            //opt.Clearance = 10.0;

            //ObstructionList obst = ObstructionList.Create();
            //ClashSet cs = ClashSet.Create();
            
            //bool bResult = CLASHER.Instance.CheckAll(opt, obst, cs);
            //if (bResult)
            //{
            //    // inspect clashes
            //    Clash[] aClashes = cs.Clashes;
            //    for (int i = 0; i < aClashes.Length; i++)
            //    {
            //        DbElement first = aClashes[i].First;
            //        DbElement second = aClashes[i].Second;
            //        ClashType ctp = aClashes[i].Type;
            //        Position pos = aClashes[i].ClashPosition;
            //    }
            //}


            //ClashOptions opt = ClashOptions.Create();
            //opt.IncludeTouches = true;
            //opt.Clearance = 10.0;

            //ObstructionList obst = ObstructionList.Create();
            //ClashSet cs = ClashSet.Create();

            //bool bResult = CLASHER.Instance.CheckAll(opt, obst, cs);
            //if (bResult)
            //{
            //    // inspect clashes
            //    Clash[] aClashes = cs.Clashes;
            //    for (int i = 0; i < aClashes.Length; i++)
            //    {
            //        DbElement first = aClashes[i].First;
            //        DbElement second = aClashes[i].Second;
            //        ClashType ctp = aClashes[i].Type;
            //        Position pos = aClashes[i].ClashPosition;
            //    }
            //}

           

            gogo();


           
           
            //ClashOptions opt = ClashOptions.Create();
            //opt.IncludeTouches = true;
            //opt.Clearance = 10.0;

            //ObstructionList obst = ObstructionList.Create();
            //ClashSet cs = ClashSet.Create();

            //bool bResult = CLASHER.Instance.CheckAll(opt, obst, cs);
            //if (bResult)
            //{
            //    // inspect clashes
            //    Clash[] aClashes = cs.Clashes;
            //    for (int i = 0; i < aClashes.Length; i++)
            //    {
            //        DbElement first = aClashes[i].First;
            //        DbElement second = aClashes[i].Second;
            //        ClashType ctp = aClashes[i].Type;
            //        Position pos = aClashes[i].ClashPosition;
            //    }
            //}

            //sw.Stop();

            //MessageBox.Show("걸린시간:" + sw.ElapsedMilliseconds.ToString());

            //Command.CreateCommand(string.Format("$p |{0}|", result_collection.Cast<DbElement>().Count().ToString())).RunInPdms();
        }
        static ClashOptions aa;
        static public  void gogo()
        {

            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("DESCLASH").RunInPdms();
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("OVERRIDE ON").RunInPdms();
        //// Element Type 정의
            DbElementType[] dbtypes = new DbElementType[] { DbElementTypeInstance.AREADEF, DbElementTypeInstance.BRANCH, DbElementTypeInstance.PSPOOL, DbElementTypeInstance.PIPE, DbElementTypeInstance.SCTN, DbElementTypeInstance.FLANGE, DbElementTypeInstance.TUBING, DbElementTypeInstance.STRUCTURE,DbElementTypeInstance.PANEL};
            DbElementType[] ignoretypes = new DbElementType[] { };
            ////
            // Check Option

            
            ClasherImpl.Init();
            
            ClashOptions opt = ClashOptions.Create();
            

            ClashOptionsImpl.Init();
            ClashSetImpl.Init();


            //opt = ClashOptionsImpl.Create();

            //ClashOptions opt = ClashOptions.Create();
            
            opt.Clearance = 10.0;
            opt.HullDesign = true;
            opt.HullProduction = true;
            opt.IgnoreConnectionsWithSpecifications = true;
            //opt.LMIDisplay = LMIDisplayType.CELLS;
            //opt.LMIDisplayExpandCells = 10.0;
            //opt.LMITestExact = true;
            opt.Midpoint = false;
            opt.NoCheck(ignoretypes);
            opt.NoCheckWithin(ignoretypes);
            

            opt.IncludeTouches = true;
            opt.IncludeConnections = false;
            
            opt.TouchGap = 2.0;
            opt.TouchOverlap = 1.0;
            opt.BranchCheckType = BranchCheck.BCHECK;
            
            opt.Within(dbtypes);
            
            ObstructionList obst = ObstructionList.Create();
            //obst.AllObstructions = true;
           // obst.AddObstructions(new DbElement[] { DbElement.GetElement("GPSET 1 of /CLASH.CHECK.GPWL") });

            obst.AddObstructions(new DbElement[] { DbElement.GetElement("/CLASH.CHECK.GPWL")});
            //obst.AllObstructions = true;
            ClashSet cs = ClashSet.Create();

            
            
            //bool bResult = Clasher.Instance.CheckAddAll(opt, obst, cs);
            //bool bResult = CLASHER.Instance.BoxCheckAll(opt, obst, cs);
            //Clasher.Equals

            bool bResult = Clasher.Instance.Check(new DbElement[] { DbElement.GetElement("/CLASH.CHECK.GPWL") }, opt, obst, cs);
            if (bResult)
            {
                // inspect clashes
                Clash[] aClashes = cs.Clashes;
                for (int i = 0; i < aClashes.Length; i++)
                {
                    DbElement first = aClashes[i].First;
                    DbElement second = aClashes[i].Second;
                    ClashType ctp = aClashes[i].Type;
                    Position pos = aClashes[i].ClashPosition;
                }

                //Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(string.Format("$p 갤수: {0}", aClashes.Length.ToString())).RunInPdms();
            }
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("EXIT").RunInPdms();


        }
        private void xx()
        {
            Position p1 = Position.Create(5000.0, 0.0, 15000.0);
            Position p2 = Position.Create(15000.0, 9000.0, 30000.0);
            LimitsBox limitsBox = LimitsBox.Create(p1, p2);
            InVolumeFilter abv = new InVolumeFilter("/PAR321", false);
            DbElement[] eles = Spatial.Instance.ElementsInBox(limitsBox, false);
            Command.CreateCommand("$P |계산완료|").RunInPdms();
            Console.WriteLine("222");
            DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);
            DBElementCollection abc = new DBElementCollection(Outfit_Elements);
            abc.Cast<DbElement>().Count();
            Command.CreateCommand("$P |계산완료2|").RunInPdms();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                Command.CreateCommand("loadform !!PFFABRICATIONCHECK").RunInPdms();
                PipeFabricationControl bb = new PipeFabricationControl();
                
                PFFabricationMachineManager fabManager = new Aveva.Pdms.PipeFabrication.FabricationCheck.PFFabricationMachineManager();
                PFDBChangeManager.GetInstance().SuspendChanges();
                bool disableSpooltype = PFDefaults.Instance.DisableSpoolType;

                //PFFabricationStatus.ValidForFabrication
                fabManager.Reset();
                DbElementType curElementType=CurrentElement.Element.GetElementType();
                DbElementType ownerElementType=CurrentElement.Element.Owner.GetElementType();
                PFPipe [] pipes=new PFPipe[] {};
                
                if(curElementType==DbElementTypeInstance.PIPE)
                    pipes=new PFPipe[]{new PFPipe(CurrentElement.Element)};
                else if(ownerElementType==DbElementTypeInstance.BRANCH)
                    pipes=new PFPipe[]{new PFPipe(CurrentElement.Element.Owner.Owner)};
                else if(curElementType==DbElementTypeInstance.BRANCH)
                    pipes=new PFPipe[]{new PFPipe(CurrentElement.Element.Owner)};
                else if(curElementType==DbElementTypeInstance.ZONE)
                {
                    List<PFPipe> pipeslist=new List<PFPipe>();
                    var pipecollection=CurrentElement.Element.Members().OfType<DbElement>().Where(x=>x.GetElementType()==DbElementTypeInstance.PIPE);
                    foreach (DbElement item in pipecollection)
                    {
                        pipeslist.Add(new PFPipe(item));
                    }
                    pipes=pipeslist.ToArray();
                }

                foreach (PFPipe pipe in pipes)
                {


                    lock (pipe)
                    {

                        try
                        {
                            pipe.DeleteGabageElements();
                            foreach (PFPipeSpool spool in pipe.PipeSpools)
                            {
                                foreach (PFPipePiece piece in spool.PipePieces)
                                {
                                    fabManager.ValidatePipePiece(piece, false, false);
                                }
                                if (!disableSpooltype)
                                {
                                    spool.UpdateSpoolType();
                                }
                            }
                            fabManager.CreateSpools(pipe, false);
                            //if(pipe.FabricationStatus==PFFabricationStatus.ValidForFabrication)
                            //{
                            //    fabManager.CreateSpools(pipe, false);
                            //}

                        }
                        catch (Exception ex)
                        {
                            PFUtils.ShowError(ex);
                        }

                    }
                }
                PFDBChangeManager.GetInstance().ResumeChanges();



                splashScreenManager1.CloseWaitForm();
                //aa.CreateSpools(pipe, true);
               
                //CurrentElement.Element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement("/Fabrication_Machines"));
                //CurrentElement.Element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement("/Fabrication_Machines"));

            }catch(Exception ee)
            {

            }
            //string command = "!!ModelEditorOnOff()";
            //Command cmd = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(command);
            //cmd.CommandString = command;
            //cmd.Run();
            //ThreadStart ts = new ThreadStart(xx);
            //Thread t = new Thread(ts);
            //t.Start();
            //MessageBox.Show("11");
            //Console.WriteLine("11");

            //Position p1 = Position.Create(0.0, 0.0, 0.0);
            //Position p2 = Position.Create(5000.0, 5000.0, 5000.0);
            //LimitsBox limitsBox = LimitsBox.Create(p1, p2);

            //DbElement[] eles = Spatial.Instance.ElementsInBox(limitsBox, false);

            //#region Partial Getwork
            //try
            //{
            //    Db[] xx = new Db[] { MDB.CurrentMDB.GetDBArray()[2] };
            //    MDB.CurrentMDB.GetWork(xx);
            //    CurrentElement.Element.Release();
            //}

            //catch (Exception ee)
            //{

            //}
            //#endregion 

        }



        DataTable dt = new DataTable();
        DataTable offsettable = new DataTable();
        DataTable flangetype_dt = new DataTable();
        DataTable pipingtype_dt = new DataTable();
        DataTable amflangeoffset_dt = new DataTable();
        DataTable pipingtype_special_dt = new DataTable();
        private void button6_Click(object sender, EventArgs e)
        {
            Console.WriteLine("11");
            //DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Draft);

            //DbElementType[] dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE };
            //if (checkPipe.Checked)
            //{
            //    dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE };
            //    pipe_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_pipe_name.Text, txt_pipe_module.Text);

            //}
            OdbcConnection conn = new OdbcConnection("dsn=mis-1;uid=tbdb01;pwd=tbdb01");


            
            string query = "select * from  am_flange_offset";
            OdbcCommand cmd = new OdbcCommand(query, conn);
            OdbcDataAdapter da = new OdbcDataAdapter(cmd);
            da.Fill(amflangeoffset_dt);

            query = "select * from  am_flange_type";
            cmd = new OdbcCommand(query, conn);
            da = new OdbcDataAdapter(cmd);
            da.Fill(flangetype_dt);

            query = "select * from  am_piping_type";
            cmd = new OdbcCommand(query, conn);
            da = new OdbcDataAdapter(cmd);
            da.Fill(pipingtype_dt);

            query = "select * from  am_piping_special_type";
            cmd = new OdbcCommand(query, conn);
            da = new OdbcDataAdapter(cmd);
            da.Fill(pipingtype_special_dt);

            
            DbPostElementChange.AddPostAttributeChange(DbAttributeInstance.LSTU, new DbPostElementChange.PostAttributeChangeDelegate(Setting_FlangeOffset));
            DbPostElementChange.AddPostAttributeChange(DbAttributeInstance.ORI, new DbPostElementChange.PostAttributeChangeDelegate(flow_determination));
            DbPostElementChange.AddPostAttributeChange(DbAttributeInstance.POS, new DbPostElementChange.PostAttributeChangeDelegate(flow_determination));
            DbPostElementChange.AddPostAttributeChange(DbAttributeInstance.SPRE, new DbPostElementChange.PostAttributeChangeDelegate(xxx));
            DbPostElementChange.AddPostAttributeChange(DbAttributeInstance.ANGL, new DbPostElementChange.PostAttributeChangeDelegate(xxx));
            DbPostElementChange.AddPostCreateElement(DbElementTypeInstance.COUPLING, new DbPostElementChange.PostCreateDelegate(Element_Creat_Event));
            
            
            
            //DbPostElementChange.AddPostCreateElement(DbElementTypeInstance.ZONE, new DbPostElementChange.PostCreateDelegate(Zone_Created));


        }

        private void xxx(DbElement ele, DbAttribute att)
        {
            if (ele.GetAsString(DbAttributeInstance.TYPE) == "ELBO" && att.ToString() == "ANGL")
            {
                try
                {
                    DbElement spref = ele.GetElement(DbAttributeInstance.SPRE);
                    DbElement lstube = ele.GetElement(DbAttributeInstance.LSTU);
                    double angle = ele.GetDouble(DbAttributeInstance.ANGL);
                    if (spref.IsNull == false && angle != 0.0)
                    {

                        double bore = ele.GetDouble(DbAttributeInstance.ABOR);
                        string pspec = ele.Owner.GetAsString(DbAttributeInstance.PSPE);
                        string stype = "4S";
                        if (angle > 45.1)
                            if (bore > 20)
                                stype = "9S";
                            else
                                stype = "9L";
                        else
                            if (bore > 20)
                                stype = "4S";
                            else
                                stype = "4L";
                        var spco = DbElement.GetElement(pspec).Members(DbElementTypeInstance.SELEC).Where(x => x.GetAsString(DbAttributeInstance.TANS) == "ELBO").ToArray()[0].
                        Members().Where(x => x.GetDouble(DbAttributeInstance.ANSW) == bore).ToArray()[0].
                        Members().Where(x => x.GetAsString(DbAttributeInstance.TANS) == stype).ToArray()[0].Members().ToArray()[0].Members()[0];
                        ele.SetAttribute(DbAttributeInstance.SPRE, spco);
                        //Command.CreateCommand(string.Format("SELECT WITH SPEC {0} type elbo stype |{1}| ANGLE {2}", pspec, stype, angle.ToString())).RunInPdms();
                    }
                }
                catch (Exception ee)
                {

                }
            }
            
           
        }

        private void Element_Creat_Event(DbElement ele)
        {
            try
            {
                if (!ele.Previous.IsNull)
                {
                    //Command.CreateCommand("$p |얍2|").RunInPdms();
                    string pp_arrive = ele.GetAsString(DbAttributeInstance.ARRI);
                    string pp_leave = ele.GetAsString(DbAttributeInstance.LEAV);

                    if (pp_arrive == "1" && pp_leave == "2")
                    {
                        ele.SetAttribute(DbAttributeInstance.CSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.LSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.ASFBR, false);

                    }
                    else
                    {
                        ele.SetAttribute(DbAttributeInstance.CSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.LSFBR, false);
                        ele.SetAttribute(DbAttributeInstance.ASFBR, true);
                    }
                }
                else
                {
                    ele.SetAttribute(DbAttributeInstance.CSFBR, false);
                    ele.SetAttribute(DbAttributeInstance.LSFBR, false);
                    ele.SetAttribute(DbAttributeInstance.ASFBR, false);
                }
            }catch(Exception ee)
            {
                Console.WriteLine("1");
            }
        }

        private void flow_determination(DbElement ele, DbAttribute att)
        {
            try
            {
                if (ele.GetAsString(DbAttributeInstance.TYPE) == "COUP")
                {
                    if (!ele.Previous.IsNull && ele.Previous.GetAsString(DbAttributeInstance.TYPE)=="TUBI" )
                    {
                        //Command.CreateCommand("$p |얍2|").RunInPdms();
                        string pp_arrive = ele.GetAsString(DbAttributeInstance.ARRI);
                        string pp_leave = ele.GetAsString(DbAttributeInstance.LEAV);

                        if (pp_arrive == "1" && pp_leave == "2")
                        {
                            ele.SetAttribute(DbAttributeInstance.CSFBR, true);
                            ele.SetAttribute(DbAttributeInstance.LSFBR, true);
                            ele.SetAttribute(DbAttributeInstance.ASFBR, false);

                        }
                        else
                        {
                            ele.SetAttribute(DbAttributeInstance.CSFBR, true);
                            ele.SetAttribute(DbAttributeInstance.LSFBR, false);
                            ele.SetAttribute(DbAttributeInstance.ASFBR, true);
                        }
                    }
                    else
                    {
                        ele.SetAttribute(DbAttributeInstance.CSFBR, false);
                        ele.SetAttribute(DbAttributeInstance.LSFBR, false);
                        ele.SetAttribute(DbAttributeInstance.ASFBR, false);
                    }

                }
                if(ele.GetAsString(DbAttributeInstance.TYPE)=="ELBO" && att.ToString()=="POS")
                {
                    try
                    {
                        DbElement spref = ele.GetElement(DbAttributeInstance.SPRE);
                        DbElement lstube = ele.GetElement(DbAttributeInstance.LSTU);
                        double angle = ele.GetDouble(DbAttributeInstance.ANGL);
                        if (spref.IsNull == false && angle!=0.0)
                        {
                            
                            double bore = ele.GetDouble(DbAttributeInstance.ABOR);
                            string pspec = ele.Owner.GetAsString(DbAttributeInstance.PSPE);
                            string stype = "4S";
                            if (angle > 45.1 )
                                if ( bore>20)
                                    stype = "9S";
                                else
                                    stype = "9L";
                            else
                                if (bore > 20)
                                    stype = "4S";
                                else
                                    stype = "4L";
                            DbElement spco = DbElement.GetElement(pspec).Members(DbElementTypeInstance.SELEC).Where(x => x.GetAsString(DbAttributeInstance.TANS) == "ELBO").ToArray()[0].
                            Members().Where(x => x.GetDouble(DbAttributeInstance.ANSW) == bore).ToArray()[0].
                            Members().Where(x => x.GetAsString(DbAttributeInstance.TANS) == stype).ToArray()[0].Members().ToArray()[0].Members()[0];
                            if (spco.IsNull != false)
                                ele.SetAttribute(DbAttributeInstance.SPRE, spco);
                            else
                                Command.CreateCommand("$P 해당스펙에 SYPE:" + stype + " 이 없음");
                            //Command.CreateCommand(string.Format("SELECT WITH SPEC {0} type elbo stype |{1}| ANGLE {2}", pspec, stype, angle.ToString())).RunInPdms();
                        }
                    }
                    catch (Exception ee)
                    {

                    }
                }
            }catch(Exception EE)
            {
                Command.CreateCommand("$p |오류|").RunInPdms();
            }
        }
        private void Zone_Created(DbElement ele)
        {

            ele.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement("/Fabrication_Machines"));
        }

        private void Setting_FlangeOffset(DbElement ele, DbAttribute att)
        {
            try
            {
                //string wallt = ele.GetDouble(DbAttributeInstance.LWALLT).ToString();
                //DbDouble[] desp = ele.GetDbDoubleArray(DbAttributeInstance.DESP);
                //string flangegroup=ele.GetAsString(Dbat)
                if (ele.GetAsString(DbAttributeInstance.TYPE) == "FLAN")
                {
                    DbDouble[] desp = new DbDouble[1];
                    DbElement lstube_catref = DbElement.GetElement(ele.GetAsString(DbAttributeInstance.LSTU)).GetElement(DbAttributeInstance.CATR);
                    DbElement flange_catref = DbElement.GetElement(ele.GetAsString(DbAttributeInstance.SPRE)).GetElement(DbAttributeInstance.CATR);
                    string lstube_desc = DbElement.GetElement(ele.GetAsString(DbAttributeInstance.LSTU)).GetElement(DbAttributeInstance.CATR).GetAsString(DbAttributeInstance.DESC);
                    double wallthick = ele.GetElement(DbAttributeInstance.LTWREF).GetDouble(DbAttributeInstance.WTHICK);
                    double outdiameter = ele.GetDouble(DbAttributeInstance.AOD);
                    double nominal_dia = ele.GetDouble(DbAttributeInstance.MAXB);
                    string stype = ele.GetAsString(DbAttributeInstance.STYP);
                    DataRow[] material_types = pipingtype_dt.Rows.Cast<DataRow>().Where(row => row["COMP_GROUP"].ToString() == lstube_catref.ToString().Split('-')[0] && row["MATL"] != null).ToArray();
                    DataRow[] flange_types = flangetype_dt.Rows.Cast<DataRow>().Where(row => row["COMP_GROUP"].ToString() == flange_catref.ToString().Split('-')[0]).ToArray();
                    DataRow[] datarow = null;
                    if (material_types.Count() > 0 && flange_types.Count() > 0)
                    {
                        string flangetype = flange_types[0]["TYPE"].ToString();
                        string press = flange_types[0]["PRESSURE"].ToString();
                        string matl = material_types[0]["MATL"].ToString();

                        if (flangetype == "SQ" || press == "20K" || press == "30K" || press == "40K")
                        {
                            datarow = amflangeoffset_dt.Rows.Cast<DataRow>().
                                Where(row => row["FLANGE_TYPE"].ToString() == flangetype &&
                                        row["PRESS"].ToString() == press && row["BORE"].ToString() == Convert.ToInt32(nominal_dia).ToString()).ToArray();
                        }
                        else if (flangetype == "F-TYPE")
                        {
                            datarow = amflangeoffset_dt.Rows.Cast<DataRow>().
                                Where(row => row["FLANGE_TYPE"].ToString() == flangetype && row["BORE"].ToString() == Convert.ToInt32(nominal_dia).ToString()).ToArray();
                        }
                        else
                        {
                            datarow = amflangeoffset_dt.Rows.Cast<DataRow>().
                                Where(row => row["MATERIAL"].ToString() == matl && row["FLANGE_TYPE"].ToString() == flangetype &&
                                        row["PRESS"].ToString() == press && row["BORE"].ToString() == Convert.ToInt32(nominal_dia).ToString()).ToArray();

                        }
                        

                        if (datarow.Count() == 0)
                            desp[0] = DbDouble.Create(0.0d);
                        else
                        {
                            double flangeoffset=Convert.ToDouble(datarow[0]["FLANGE_OFFSET"].ToString());
                            desp[0] = DbDouble.Create(flangeoffset);
                        }
                        ele.SetAttribute(DbAttributeInstance.DESP, desp);

                    }
                    else//일반자재에 없다면
                    {
                        material_types = pipingtype_special_dt.Rows.Cast<DataRow>().Where(row => row["COMP_GROUP"].ToString() == lstube_catref.ToString().Split('-')[0]).ToArray();

                        //string lstube_despara=lstube_catref.GetAsString(DbAttributeInstance.PARA);
                        //string[] lstube_despara_arr = lstube_despara.Split(' ');
                        
                        if (material_types.Count() > 0)
                        {
                            int flangeoffset=0;
                            if(material_types[0]["FLANGE_OFFSET"].ToString()=="5T")
                            {
                                if(wallthick<=5)
                                    flangeoffset=5;
                                else
                                    flangeoffset= ((int)(wallthick+0.5d));
                            }
                            else if(material_types[0]["FLANGE_OFFSET"].ToString()=="7T")
                            {
                                if (wallthick <= 7)
                                    flangeoffset = 7;
                                else
                                    flangeoffset = ((int)(wallthick + 0.5d));
                            }
                            else if(material_types[0]["FLANGE_OFFSET"].ToString()=="10T")
                            {
                                if (wallthick <= 10)
                                    flangeoffset = 10;
                                else
                                    flangeoffset = ((int)(wallthick + 0.5d));
                            }
                                                        
                            desp[0] = DbDouble.Create(Convert.ToDouble(flangeoffset));
                            ele.SetAttribute(DbAttributeInstance.DESP, desp);
                        }

                        
                    }
                    
                }
                else if (ele.GetAsString(DbAttributeInstance.TYPE) == "COUP")
                {
                    //Command.CreateCommand("$p |얍2|").RunInPdms();
                    string pp_arrive = ele.GetAsString(DbAttributeInstance.ARRI);
                    string pp_leave = ele.GetAsString(DbAttributeInstance.LEAV);
                    DbElement tube = ele.Previous;
                    if (pp_arrive == "1" && pp_leave == "2")
                    {
                        ele.SetAttribute(DbAttributeInstance.CSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.LSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.ASFBR, false);

                    }
                    else
                    {
                        ele.SetAttribute(DbAttributeInstance.CSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.LSFBR, false);
                        ele.SetAttribute(DbAttributeInstance.ASFBR, true);
                    }

                }

            }
            catch (Exception ee)
            {
                //desp[0] = DbDouble.Create(6.0);
                //ele.SetAttribute(DbAttributeInstance.DESP, desp);
            }
            // string wallt2 = ele.GetDouble(DbAttributeInstance.ATWALL).ToString();
            //DbElement FlangeCate = ele.GetElement(DbAttributeInstance.CATR).Owner; //CATREF

        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;
        

        private void button7_Click(object sender, EventArgs e)
        {
            //Command.CreateCommand("!!ModelEditorOnoff()").Run();
            MarUtil dd = new MarUtil();
            dd.TBEnvironmentSet("SB_PDB_PADD", "A_GENERAL.DEPT;A_GENERAL.REGI");


            Aveva.Marine.UI.PlanarHullToolsDefaultParametersReInit tt = new Aveva.Marine.UI.PlanarHullToolsDefaultParametersReInit();
            
            tt.Execute();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                PipeFabricationControl cc = new PipeFabricationControl();
                
                PFFabricationMachineManager aa = new Aveva.Pdms.PipeFabrication.FabricationCheck.PFFabricationMachineManager();

                if(CurrentElement.Element.GetInteger(DbAttributeInstance.DDEP)<3)
                {
                    DbElement baseelemnt = CurrentElement.Element;
                    AndFilter filter = new AndFilter();
                    TypeFilter typefilter = new TypeFilter(DbElementTypeInstance.PIPE);
                    
                    filter.Add(typefilter);                    
                    DBElementCollection result_collection = new DBElementCollection(baseelemnt, filter);

                    DbElement[] pipelist = result_collection.Cast<DbElement>().ToArray();
                    foreach (DbElement element in pipelist)
                    {
                        PFPipe pipe = new PFPipe(element);
                        pipe.DeletePipeSpools();
                    }
                }
                else
                {
                    PFPipe pipe = new PFPipe(CurrentElement.Element);
                    pipe.DeletePipeSpools();
                }

                

                //CurrentElement.Element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement("/Fabrication_Machines"));
                //CurrentElement.Element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement("/Fabrication_Machines"));

            }
            catch (Exception ee)
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Selection select = Selection.CurrentSelection;
            DbElement [] select_members= select.Members;
            foreach (DbElement ele in select_members)
            {
                if (ele.GetAsString(DbAttributeInstance.TYPE) == "COUP")
                {
                    //Command.CreateCommand("$p |얍2|").RunInPdms();
                    string pp_arrive = ele.GetAsString(DbAttributeInstance.ARRI);
                    string pp_leave = ele.GetAsString(DbAttributeInstance.LEAV);
                    if (pp_arrive == "1" && pp_leave == "2")
                    {
                        ele.SetAttribute(DbAttributeInstance.CSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.LSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.ASFBR, false);

                    }
                    else
                    {
                        ele.SetAttribute(DbAttributeInstance.CSFBR, true);
                        ele.SetAttribute(DbAttributeInstance.LSFBR, false);
                        ele.SetAttribute(DbAttributeInstance.ASFBR, true);
                    }

                }
                
            }
            

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                DbElement ele = CurrentElement.Element;
                //BRAN 아래 애들은 CLAIM정보를 가지고 있지 않다 그래서 BRAN으로 옮겨서 CLAIM여부를 체크하기위해서 ELEMENT 를 한단꼐 위로 올린다.
                if (ele.Owner.GetAsString(DbAttributeInstance.TYPE) == "BRAN") 
                {
                    ele = ele.Owner;
                }
                try
                {
                    ele.SetAttribute(DbAttributeInstance.LOCK, true);
                    
                    ele.SetAttribute(DbAttributeInstance.LOCK, false);
                    var pdmscmd = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("unclaim ce");
                    pdmscmd.Run();
                    MessageBox.Show("현재 선택된 Element는 Claim이 걸려있지 않습니다.");
                }
                catch
                {

                    string dbname = ele.GetAsString(DbAttributeInstance.DBNA);                   
                    

                    Db[] currentdbs = MDB.CurrentMDB.GetDBArray(Aveva.Pdms.Database.DbType.Design).OfType<Db>().Where(db => db.Name == dbname).ToArray();
                    MDB.CurrentMDB.GetWork(currentdbs);

                    string claimid = ele.GetAsString(DbAttributeInstance.CLMID);    //claim걸려있는 컴퓨터 이름
                    string claimnum = ele.GetAsString(DbAttributeInstance.CLMNUM);  //claim걸려있는 session num
                    string userclaim = ele.GetAsString(DbAttributeInstance.USERC);  //claim걸려있는 유저id
                    string username = Project.CurrentProject.UserName;
                    int dbno = ele.GetInteger(DbAttributeInstance.DBNU);

                    var pdmscmd = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("q clmnum");
                    pdmscmd.Run();

                    if(username!= userclaim && username!="SYSTEM")
                    {
                        MessageBox.Show("현재 Element는 " + userclaim + "의 사용자가 점유하고 있습니다. 해당 유저에게 요청해서 해제하십시오.","클레임 해제 실패"); ;
                        return;
                        
                    }


                    string tempFileName = Path.GetTempFileName();
                    

                    using (var sw = new StreamWriter(tempFileName, false))
                    {
                        sw.WriteLine(string.Format("EXPUNGE db {0} USER {1} ", dbname, claimnum));
                        sw.WriteLine("FINISH");
                    }

                    string proj = Project.CurrentProject.Name;
                    string user = "PROJPROGRAM";
                    string pass = "HMDPROGRAM";

                    //string cmd = string.Format("syscom |cmd /c start /min cmd.exe /c adm -PROJ={0} -USER={1} -PASS={2} -MDB= -tty -CONSOLE -macro=$m/{3}|  $p |우하|", proj, user, pass, tempFileName);
                    string cmd = string.Format("Q CLMNUM sysCOM |CMD /C START /min cmd.exe /C  adm -PROJ={0} -USER={1} -PASS={2} -MDB= -tty -macro=$m/{3}|  ", proj, user, pass, tempFileName);
                    pdmscmd = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmd);
                    pdmscmd.Run();
                    

                    MDB.CurrentMDB.GetWork(currentdbs);

                    

                    MessageBox.Show("클레임이 해제되었습니다. 작업을 진행하세요");

                }
                
            }catch(Exception ce)
            {
                Console.WriteLine(ce.Message);
            }

        }
        
        private void button11_Click(object sender, EventArgs e)
        {
            PMLNetCommand vv = new PMLNetCommand();
            
            //Aveva.ApplicationFramework.Presentation.CommandManager.Instance.Commands
            
            PMLNetCommandManager cmd=new PMLNetCommandManager();
            cmd.ExecuteCommand("AVEVA.StatusController.DisplayStatusEditorCommand");
            
            
            return;
            try
            {
                MarDrafting kcsdraft = new MarDrafting();
                print_current_dwg();


                CreatePostscript2PDF();

                ConvertPCL2PDF();

                //kcsdraft.DwgTIFExport(@"c:\bbb.tif", 2970, 2100);


                return;
                ////
                //RadialMenuControl.SetupMenuList aa = new RadialMenuControl.SetupMenuList();
                //aa.Show();
                
                //return;
                

                List<string> dwglist = new List<string>();
                dwglist.Add("24353RD4TH_COM-001");
                //dwglist.Add("9435T_N31PIT221_B007");
                foreach (string dwgname in dwglist)
                {
                    if (kcsdraft.DwgCurrent())
                        kcsdraft.DwgClose();
                    kcsdraft.DwgOpen(dwgname);
                    print_current_dwg();
                    
                    kcsdraft.DwgClose();
                    
                }

                







                return;
                MarDrafting kcs_draft = new MarDrafting();
                MarUi kcs_ui = new MarUi();
                MarPointPlanar p1 = new MarPointPlanar();
                MarStatPointPlanarReq status = new MarStatPointPlanarReq();
                status.DefMode = "ModeCursor";

                MarCursorType CurType = new MarCursorType();
                CurType.SetCrossHair();
                status.Cursor = CurType;

                int result = kcs_ui.PointPlanarReq("Indicate GEomery", p1, status);

                MarElementHandle handle = kcs_draft.ComponentIdentify(p1);
                MarModel model = kcs_draft.ModelPropertiesGet(handle);

                string name = kcs_draft.ElementDbrefGet(handle);
                bool xx = kcs_draft.ElementIsImpliedModelPart(handle);

                //MarDex dex = new MarDex();
                ////dex.Extract("DRAWING('SAB-TF61-KJT-005@SB_PDB033_PADD').VIEW(*).TEXT(1:1000).VALUE");
                //dex.Extract("DRAWING('@SB_PDB033'*).NAME");
                //while (dex.NextResult() == 3)
                //{
                //    string aa = dex.GetString();
                //    Command.CreateCommand("$p " + aa).RunInPdms();
                //}

                Console.WriteLine("1");
                //MarDrafting kcsdraft = new MarDrafting();
                //MarUtil kcsutil = new MarUtil();
                //MarUi kcs_ui = new MarUi();
                //if (kcsdraft.DwgCurrent())
                //    kcsdraft.DwgClose();

                //kcsdraft.DwgOpen("SAB-TF11CI27B-BA101-2", 1033);
                //kcs_ui.AppWindowRefresh();


            }catch(Exception ee)
            {

            }
        }

        private void ConvertPCL2PDF()
        {
            Process process = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = @"M:\hmd\hmdpgm\ghostscript\ghostpcl";
            info.FileName = "gpcl6win32-9.18";
            info.Arguments = @"-dNOPAUSE -sDEVICE=pdfwrite -sOutputFile=c:\temp\park.pdf c:\temp\aa.prn";
            info.CreateNoWindow = true;
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo = info;

            process.Start();
        }

        private static void CreatePostscript2PDF()
        {
            Process process = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = @"M:\hmd\hmdpgm\ghostscript\gs\bin";
            info.UseShellExecute = true;
            info.FileName = "gswin32c";
            info.Arguments = @"-dNOPAUSE -dBATCH -sDEVICE=pdfwrite -sOutputFile=c:\temp\park3.pdf c:\temp\aa.prn";
            info.CreateNoWindow = true;
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo = info;

            process.Start();
        }

 

        private void print_current_dwg()
        {
            MarPrintOptions marPrintOpt = new MarPrintOptions();
            //KONICA MINOLTA 423SeriesPCL
            //marPrintOpt.PrinterName = "KONICA MINOLTA 423SeriesPCL";
            marPrintOpt.PrinterName = "NPS PDF Converter";

            //도면스케일설정 0  - use default value, 직접 입력.
            marPrintOpt.Scale = 1;
            //autoscale 0 - use default value, 1 - scale to fit, 2 - no scale to fit
            marPrintOpt.ScaleToFit = 1;
            
            
            //종이 중심에 위치 0 - use default value, 1 - center on page, 2 - do not center on page
            marPrintOpt.CenterOnPage = 1;


            //출력갯수
            marPrintOpt.NoOfCopies = 1;
             
            //EFFprintArea를 4번인 capture area로 설정시에 적용되는 인쇄범위
            //marPrintOpt.Point1 = new MarPointPlanar(0.0, 0.0);
            //marPrintOpt.Point2 = new MarPointPlanar(1000.0, 1000.0);

            //인쇄유효영역설정 0 - Currently selected,  1 - Drawing Form,   2 - Drawing Extensions,    3 - Current Window,    4 - Capture Area
            marPrintOpt.EffPrintArea =1;


            //Console.WriteLine("-aaa-");
            //아래의 종이 사이즈는 0.1mm단위이다.
            //marPrintOpt.PaperWidth = 2100;
            //marPrintOpt.PaperLength = 2970;
            marPrintOpt.PaperName = "A4";

            //종이방향 1-Default, 2-Portrait, 3-Landscape
            marPrintOpt.Orientation = 0;
            
            //종이방향 자동설정 0 - use default value, 1 - auto orient, 2 - no auto orient, 3 - rotate 90, 4 - rotate 180, 5 - rotate 270
            marPrintOpt.AutoOrient = 1;

            //색깔 0-default, 1-흑백,2-흑백2(이게 잘나옴), 3-칼라(dim안나옴),4-칼라2(이게 잘나옴)
            marPrintOpt.ColourMode = 1;
            //파일로 내보낼때--> .ps파일로 떨어짐
            //ps 파일 생성
            marPrintOpt.PrintToFile = 1;  //1-active, 0-inactive
            marPrintOpt.FileName = @"c:\temp\aa.prn";

            MarDrafting marDrafting = new MarDrafting();
            marDrafting.DwgPrint(marPrintOpt);
            //MessageBox.Show("완료");
        }

        private void button12_Click(object sender, EventArgs e)
        {

            DbElement baseelement = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);
            //AttributeLikeFilter modulefilter = new AttributeLikeFilter(DbAttributeInstance.NAMN, "TC11SI72B.PIPE");

            string searchtxt = txtreplace.Text;
            searchtxt = searchtxt.Replace(".", "\\.");
            searchtxt = searchtxt.Replace("*", ".*");
            searchtxt = searchtxt.Replace("?", ".");

            // 정규식시작 : ^ , 정규식끝 : $  , 한글자 대체 : . , 여러글자대체 : .* 를 쓸때에는 ^, $ 사용하면 안됨.
            AndFilter filter = new AndFilter();
            TypeFilter typefilter = new TypeFilter(DbElementTypeInstance.HPANEL);
            AttributeStringFilter stringfilter = new AttributeStringFilter(DbAttributeInstance.NAMN, FilterOperator.MatchesRegularExpression, searchtxt);
            BelowFilter filter4 = new BelowFilter(stringfilter);
            filter.Add(typefilter);
            filter.Add(filter4);
            DBElementCollection result_collection = new DBElementCollection(baseelement, filter);
            
            DbElement [] list=result_collection.Cast<DbElement>().ToArray();
            Console.WriteLine("1");
            
            
            //DbElement [] sortedmember= baseelement.Members().OrderBy(x => x.GetDouble(DbAttributeInstance.ANSW)).ToArray();

            //for (int i = 0; i < sortedmember.Length; i++)
            //{
            //    sortedmember[i].InsertAfterLast(baseelement);
            //}

        }

        private void btnModelSkip_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();

            DrawListManager.Instance.BeginUpdate();
            DrawListManager.Instance.CurrentDrawList.Remove(TotalModelList.ToArray());
            DrawListManager.Instance.EndUpdate();
            DrawListManager.Instance.CurrentDrawList.VisibleAll();


            splashScreenManager1.CloseWaitForm();
            //drawListManager.BeginUpdate();
            //var currentDrawList = drawListManager.CurrentDrawList;
            //drawListManager.CurrentDrawList.Remove(TotalModelList.ToArray());
            
            //drawListManager.EndUpdate();
            //currentDrawList.VisibleAll();
        }
        private BarManager barManager1;
        ImageCollection imageCollection1;
        private void button13_Click(object sender, EventArgs e)
        {
            
            

            MarDrafting kcs_draft = new MarDrafting();
            MarUi kcs_ui = new MarUi();
            MarUtil kcs_util = new MarUtil();
            
            //if (kcs_draft.DwgCurrent())
            //    kcs_draft.DwgClose();

            //kcs_draft.DwgDxfImport(@"C:\isoout\plot001.dxf", "parkmaker1111");
            //kcs_ui.AppWindowRefresh();

            
            
            //defaultLookAndFeel1.LookAndFeel.SkinName = "McSkin";
            ////GC.Collect();
            ////return;
            //try
            //{
            //    this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            //    this.barManager1 = new DevExpress.XtraBars.BarManager();
            //    this.barManager1.Form = this;
                
                
            //    RadialMenu menu = new RadialMenu();
                
            //    menu.Manager = barManager1;
            //    menu.AddItems(CreateBarItems());
            //    //menu.BackColor = Color.Transparent;
            //    //menu.BorderColor = Color.Transparent;
            //    menu.Glyph = global::PHS.Utilities.Properties.Resources.box;
            //    menu.ShowPopup(MousePosition);
            //    menu.Expand();
            //}catch(Exception ee)
            //{

            //}
        }
        
                    
        BarItem[] CreateBarItems()        
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelInsert));
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "copy16x16.png");
            this.imageCollection1.Images.SetKeyName(1, "cut16x16.png");
            this.imageCollection1.Images.SetKeyName(2, "delete16x16.png");
            this.imageCollection1.Images.SetKeyName(3, "paste16x16.png");
            this.imageCollection1.Images.SetKeyName(4, "bold16x16.png");
            this.imageCollection1.Images.SetKeyName(5, "italic16x16.png");
            this.imageCollection1.Images.SetKeyName(6, "underLine16x16.png");
            this.imageCollection1.Images.SetKeyName(7, "find16x16.png");
            this.imageCollection1.Images.SetKeyName(8, "undo16x16.png");
            this.imageCollection1.Images.SetKeyName(9, "redo16x16.png");
            this.imageCollection1.Images.SetKeyName(10, "repeat16x16.png");
            this.imageCollection1.Images.SetKeyName(11, "replace16x16.png");
            // Create bar items to display in Radial Menu
            barManager1.Images = imageCollection1;

            BarItem barButtonItem0 = new BarButtonItem(barManager1, "Copy", 0);
            
            BarItem barButtonItem1 = new BarButtonItem(barManager1, "Cut", 1);
            
            BarItem barButtonItem2 = new BarButtonItem(barManager1, "Delete", 2);
            BarItem barButtonItem3 = new BarButtonItem(barManager1, "Paste", 3);

            // Sub-menu with 3 check buttons
            BarSubItem subMenu = new BarSubItem(barManager1, "Format",10);
            BarCheckItem barCheckItem4 = new BarCheckItem(barManager1, false)
            {
                ImageIndex = 4,
                Caption = "Bold"
            };
            
            BarCheckItem barCheckItem5 = new BarCheckItem(barManager1, true)
            {
                ImageIndex = 5,
                Caption = "Italic"
            };
            BarCheckItem barCheckItem6 = new BarCheckItem(barManager1, false)
            {
                ImageIndex = 6,
                Caption = "Underline",
                
            };
            BarItem[] subMenuItems = new BarItem[] { barCheckItem4, barCheckItem5, barCheckItem6 };
            subMenu.AddItems(subMenuItems);

            BarItem barButtonItem7 = new BarButtonItem(barManager1, "Find", 7);
            BarItem barButtonItem8 = new BarButtonItem(barManager1, "Undo", 8);
            BarItem barButtonItem9 = new BarButtonItem(barManager1, "Redo", 9);

            return new BarItem[] {barButtonItem0, barButtonItem1, barButtonItem2, barButtonItem3, 
                subMenu, barButtonItem7, barButtonItem8, barButtonItem9};
        }

        private void btnrename_Click(object sender, EventArgs e)
        {

        }

        private void tileItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            SetupMenuList a = new SetupMenuList();
            a.Show();
        }

        private void tileItem2_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                PipeFabricationControl cc = new PipeFabricationControl();

                PFFabricationMachineManager aa = new Aveva.Pdms.PipeFabrication.FabricationCheck.PFFabricationMachineManager();

                if (CurrentElement.Element.GetInteger(DbAttributeInstance.DDEP) < 3)
                {
                    DbElement baseelemnt = CurrentElement.Element;
                    AndFilter filter = new AndFilter();
                    TypeFilter typefilter = new TypeFilter(DbElementTypeInstance.PIPE);

                    filter.Add(typefilter);
                    DBElementCollection result_collection = new DBElementCollection(baseelemnt, filter);

                    DbElement[] pipelist = result_collection.Cast<DbElement>().ToArray();
                    foreach (DbElement element in pipelist)
                    {
                        PFPipe pipe = new PFPipe(element);
                        pipe.DeletePipeSpools();
                    }
                }
                else
                {
                    PFPipe pipe = new PFPipe(CurrentElement.Element);
                    pipe.DeletePipeSpools();
                }



                //CurrentElement.Element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement("/Fabrication_Machines"));
                //CurrentElement.Element.SetAttribute(DbAttributeInstance.BENDMA, DbElement.GetElement("/Fabrication_Machines"));

            }
            catch (Exception ee)
            {

            }
        }






    }
}
