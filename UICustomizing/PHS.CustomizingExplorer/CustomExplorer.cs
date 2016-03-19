using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Windows.Forms;
using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;
using Aveva.Pdms.Database;
using Aveva.Pdms.Explorer;
using Aveva.Pdms.Graphics;
using Aveva.Pdms.Presentation.ExplorerControl;
using Aveva.Pdms.Shared;
using Infragistics.Win.UltraWinTree;

namespace PHS.CustomizingExplorer
{
    public interface IFinish
    {
        void Finish();
    }

    public interface ICustomExplorer
    {
        void ClearSelection();
        void Start();
        IEnumerable<DbElement> GetSelectedElements();
        void AddTools(string key, string name, bool isfirst, Bitmap image, EventHandler onClickEvent);

    }

    enum SelectMode
    {
        Single,Mulity
    }

    public class CustomExplorer:IFinish, ICustomExplorer
    {

        private static CustomExplorer intance = new CustomExplorer();
        public static CustomExplorer Instance { get { return intance; } }

        private ExplorerCtrl mDesExpCtrl;
        private UltraTree mUltraTree;
        private List<UltraTreeNode> mSelectedElements;

        private IExplorerTreeConfig mExplorerTreeConfig;
        private SelectMode mSelect;

        private List<ITool> mDefaultAddCExpMenus; 


        //rev3 shift key selection..
        private ExplorerTreeNode mFirstSelected;
        private List<UltraTreeNode> mExcludedNodes; 

        private CustomExplorer()
        {
            SetExplorer();
            if(mDesExpCtrl == null)
                throw new InstanceNotFoundException("Can't find Design Explorer!!");

            mSelectedElements = new List<UltraTreeNode>();

            mUltraTree = mDesExpCtrl.Controls.OfType<UltraTree>().First();


            try
            {
                IEnumerable<UltraTreeNode> readonlySites = mUltraTree.Nodes[0].Nodes.Cast<UltraTreeNode>().
                       Where(node => ((DbElement)node.Tag).GetBool(DbAttributeInstance.DBWRIT) == false);
                foreach (UltraTreeNode node in readonlySites)
                {

                    node.Override.NodeAppearance.Image = Properties.Resources.Readonly;
                }
            }
            catch (Exception ee) { }

            
            //mUltraTree.AfterNodeUpdate += mUltraTree_AfterNodeUpdate;
            mUltraTree.BeforeExpand += mUltraTree_BeforeExpand;
            mUltraTree.InitializeDataNode += mUltraTree_InitializeDataNode;
            
            //mUltraTree.BeforeNodeUpdate += mUltraTree_BeforeNodeUpdate;
            //mUltraTree.FontChanged += mUltraTree_FontChanged;
            //mUltraTree.AfterDataNodesCollectionPopulated += mUltraTree_AfterDataNodesCollectionPopulated;
            //mUltraTree.BeforeActivate += mUltraTree_BeforeActivate;
            //mUltraTree.Update();
            //mUltraTree.Nodes.SubObjectPropChanged += Nodes_SubObjectPropChanged;

        }

        public void settingdata()
        {
            Console.WriteLine("11");
        }

        void mUltraTree_InitializeDataNode(object sender, InitializeDataNodeEventArgs e)
        {
            Console.WriteLine("22");
            
        }

        

        void Nodes_SubObjectPropChanged(Infragistics.Shared.PropChangeInfo propChange)
        {

            Console.WriteLine("33");
        }

        void mUltraTree_BeforeActivate(object sender, CancelableNodeEventArgs e)
        {
            Console.WriteLine("33");
        }

        void mUltraTree_AfterNodeUpdate(object sender, NodeEventArgs e)
        {
            Console.WriteLine("33");
        }

        void mUltraTree_AfterDataNodesCollectionPopulated(object sender, AfterDataNodesCollectionPopulatedEventArgs e)
        {
            Console.WriteLine("22");
        }


        void mUltraTree_FontChanged(object sender, EventArgs e)
        {
            Console.WriteLine("11");
        }




        public IEnumerable<DbElement> GetCurrentSelectedElements()
        {
            return mSelectedElements.Cast<ExplorerTreeNode>().Select(x => x.Element).AsEnumerable();
        } 

        public void Start()
        {
            //get treeview...
            mSelect = SelectMode.Mulity;
            

            //rev3
            mExcludedNodes = new List<UltraTreeNode>();

            //rev2
            mUltraTree.AllowDrop = true;
            mUltraTree.MouseClick += mUltraTree_MouseClick;
            
            mUltraTree.BeforeSelect += mUltraTree_BeforeSelect;
            mUltraTree.DragDrop += mUltraTree_DragDrop;
           
            //add context menu...
       /*     var explorerService = ServiceManager.Instance.GetService(typeof(ExplorerService)) as ExplorerService;
            if (explorerService != null)
            {
                mExplorerTreeConfig = explorerService.GetExplorerConfig("Design");
                CreateToos();
                AddToolsOnDesignExplorer(mExplorerTreeConfig);
            }*/


            //add context menu...
            mExplorerTreeConfig = CustomExplorerAddin.DesigExpTreeConfig;
           
          
            CreateToos();
            AddToolsOnDesignExplorer(mExplorerTreeConfig);

            //remove single selection menu....
            SetMultiSelectMenu(mExplorerTreeConfig.ContextMenu);


        }


        private UltraTreeNode prev_node = null;
        void mUltraTree_BeforeSelect(object sender, BeforeSelectEventArgs e)
        {
            try
            {
                prev_node = mUltraTree.SelectedNodes[0];

                Console.WriteLine("11");
            }catch(Exception ee)
            { }
            
        }

        void mUltraTree_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("1");
        }

        void mUltraTree_BeforeNodeUpdate(object sender, CancelableNodeEventArgs e)
        {
            Console.WriteLine("1");
            //e.TreeNode;
        }

        void mUltraTree_BeforeExpand(object sender, CancelableNodeEventArgs e)
        {

            if(e.TreeNode.Level==0)
            {
                IEnumerable<UltraTreeNode> readonlySites = mUltraTree.Nodes[0].Nodes.Cast<UltraTreeNode>().
                       Where(node => ((DbElement)node.Tag).GetBool(DbAttributeInstance.DBWRIT) == false);
                foreach (UltraTreeNode node in readonlySites)
                {
                    e.TreeNode.Override.NodeAppearance.BackColor = Color.LightGray;
                    node.Override.NodeAppearance.Image = Properties.Resources.Readonly;
                }


                //DbElement site = (DbElement)e.TreeNode.Tag;
                //bool writable= site.GetBool(DbAttributeInstance.DBWRIT);
                //if(!writable)
                //    e.TreeNode.Override.NodeAppearance.BackColor = Color.LightGray;
            }

            //If Exists, Pipe Spool it would be coloured yellow
            //node를 Traversing해서 검색을 해가니 2레벨 아래에 녀석들이 제대로 안나온다. 
            //node의 tag가 Dbelement를 담고 있어서 그걸통해서 조건문을 만들어서 원하는 결과를 얻어냈다. 아래를 참조하자.
            if (e.TreeNode.Text.StartsWith("ZONE"))
            {
                IEnumerable<UltraTreeNode> pipenodes = e.TreeNode.Nodes.Cast<UltraTreeNode>().
                    Where(node => node.Text.StartsWith("PIPE") && ((DbElement)node.Tag).Members().Cast<DbElement>().
                    Where(part => part.GetElementType() == DbElementTypeInstance.PSLIST && part.Members().Count() > 0).Count() > 0);
                IEnumerable<UltraTreeNode> noteRedaynodes = e.TreeNode.Nodes.Cast<UltraTreeNode>().
                        Where(node => node.Text.StartsWith("PIPE") && ((DbElement)node.Tag).Members().Cast<DbElement>().
                        Where(part => part.GetElementType() == DbElementTypeInstance.PSLIST).Count() == 0);
                Console.WriteLine("11");
                foreach (UltraTreeNode pipe in pipenodes)
                {
                    pipe.Override.NodeAppearance.BackColor = Color.GreenYellow;
                    //pipe.Override.NodeAppearance.BackColor2 = Color.RosyBrown;
                    
                    //pipe.Override.NodeAppearance.CreateFont(new Font("굴림체",15.0f));
                    
                }
                foreach (UltraTreeNode pipe in noteRedaynodes)
                {
                    pipe.Override.NodeAppearance.BackColor = Color.Bisque;
                    //pipe.Override.NodeAppearance.BackColor2 = Color.RosyBrown;

                    //pipe.Override.NodeAppearance.CreateFont(new Font("굴림체",15.0f));

                } 
              
                        
                
            }

           
        }



        public void Finish()
        {
            
            //rev3 note temp remark...
            try
            {
                mSelect = SelectMode.Single;
               
                mUltraTree.MouseClick -= mUltraTree_MouseClick;

                //rev2 multi selection시에 clear  되지 않도록 변경.
                ClearSelection();

                RemoveToolsOnDesignExplorer(mExplorerTreeConfig);
                SetSignSelectMenu(mExplorerTreeConfig.ContextMenu);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void SetMultiSelectMenu(ITools treeContextMenu)
        {
            foreach (var contextMenu in CustomExplorerAddin.SingleSelectionMenus)
            {
                if (treeContextMenu.Tools.Contains(contextMenu.Key))
                     treeContextMenu.Tools.Remove(contextMenu.Key);
            }


            var count = treeContextMenu.Tools.Count;
            foreach (var contextMenu in CustomExplorerAddin.MultiSelectionMenus)
            {
                if (treeContextMenu.Tools.Contains(contextMenu.Key))
                    continue;
                treeContextMenu.Tools.InsertTool(contextMenu.Key,count++).IsFirstInGroup = contextMenu.IsFirstGroup;
            }
        }

        private static void SetSignSelectMenu(ITools treeContextMenu)
        {
            foreach (var contextMenu in CustomExplorerAddin.MultiSelectionMenus)
            {
                if (treeContextMenu.Tools.Contains(contextMenu.Key))
                    treeContextMenu.Tools.Remove(contextMenu.Key);
            }


            foreach (var contextMenu in CustomExplorerAddin.SingleSelectionMenus)
            {
                /*if (treeContextMenu.Tools.Contains(contextMenu.Key))
                    continue;*/
                if (treeContextMenu.Tools.Contains(contextMenu.Key))
                    treeContextMenu.Tools.Remove(contextMenu.Key);

                treeContextMenu.Tools.AddTool(contextMenu.Key).IsFirstInGroup = contextMenu.IsFirstGroup;
            }


        }

        
        void mUltraTree_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {

                //right button을 누려면 context menu실행....
                if(e.Button == MouseButtons.Right)
                    return;

                //mouse left를 누르고 Control Key를 누르지 않을 경우
                //모든 selction을 없앤다...
                if (e.Button == MouseButtons.Left && (Control.ModifierKeys != Keys.Control && Control.ModifierKeys != Keys.Shift))
                {
                    ClearSelection();
                    mFirstSelected = null;
                    return;
                }

                //rev2 multi
                if(mSelect == SelectMode.Single) 
                    return;


              
                if (e.Button != MouseButtons.Left || (Control.ModifierKeys != Keys.Control && Control.ModifierKeys != Keys.Shift))
                {
                    
                    if(mSelectedElements != null && mSelectedElements.Count > 0)
                    {
                        mSelectedElements.ForEach(RestoreBackColor);
                       
                    }

                    mFirstSelected = null;
                    return;
                }

                //get TreeNode from clikc location
                var expTreeNode = mUltraTree.GetNodeFromPoint(e.Location) as ExplorerTreeNode;
                if(expTreeNode == null)
                    return;

                if (Control.ModifierKeys == Keys.Control)
                {
                    if (!mSelectedElements.Contains(prev_node))
                    {
                        SetBackColor(prev_node);
                        mSelectedElements.Add(prev_node);
                    }
                    //선택된 것이 다시 선택 된다면 삭제하고 backcolor는 원상태로 복귀...
                    if (mSelectedElements.Contains(expTreeNode))
                    {
                        RestoreBackColor(expTreeNode);
                        mSelectedElements.Remove(expTreeNode);
                    }
                    else
                    {
                        //change the back color...
                        SetBackColor(expTreeNode);
                        mSelectedElements.Add(expTreeNode);
                    }

                    mFirstSelected = null;
                }

                else if(Control.ModifierKeys == Keys.Shift)
                {
                  
                    ClearSelection();
                    

                    if (mFirstSelected == null)
                    {
                        mFirstSelected = expTreeNode;
                    }

                    else
                    {
                        try
                        {
                            var sEndSelected = mUltraTree.GetNodeFromPoint(e.Location) as ExplorerTreeNode;

                            //모든 범위가 정해 질 경우...
                            if (sEndSelected != null)
                            {

                                mSelectedElements.Clear();

                                //같은 level을 가진 elements을 선택 할 경우....
                                if (mFirstSelected.Parent == sEndSelected.Parent)
                                {
                                    var parent = mFirstSelected.Parent;

                              
                                    //첫번째 선택한 node가 위에 있을 경우..
                                    if (mFirstSelected.Index < sEndSelected.Index)
                                    {
                                        for (int i = mFirstSelected.Index; i <= sEndSelected.Index; i++)
                                        {
                                            var ultraTreeNode = parent.Nodes[i];
                                            if (!ultraTreeNode.Visible)
                                                continue;

                                            mSelectedElements.Add(ultraTreeNode);
                                        }

                                        //Only visiable...의 색깔을 넣기 위해....
                                        UltraTreeNode nextNd = mFirstSelected;
                                        while (nextNd != sEndSelected)
                                        {
                                            nextNd = nextNd.NextVisibleNode;
                                            if (nextNd.Level <= mFirstSelected.Level) continue;

                                            mSelectedElements.Add(nextNd);
                                            mExcludedNodes.Add(nextNd);
                                        }
                                    }

                                        //같을 경우.
                                    else if(mFirstSelected.Index == sEndSelected.Index)
                                    {
                                        mSelectedElements.Add(mFirstSelected);
                                    }

                                        //첫번째 선택한 node가 밑에 있을 경우....
                                    else
                                    {
                                        for (int i = sEndSelected.Index; i <= mFirstSelected.Index; i++)
                                        {
                                            var ultraTreeNode = parent.Nodes[i];
                                            if(!ultraTreeNode.Visible)
                                                continue;

                                            mSelectedElements.Add(ultraTreeNode);
                                        
                                        }

                                  
                                        //only visiable...
                                        UltraTreeNode nextNd = mFirstSelected;
                                        while (nextNd != sEndSelected)
                                        {
                                            nextNd = nextNd.PrevVisibleNode;
                                            if (nextNd.Level <= sEndSelected.Level) continue;

                                            mSelectedElements.Add(nextNd);
                                            mExcludedNodes.Add(nextNd);
                                        }
                                    }

                                }//end 같은 parents를 가질 경우...


                                    //parenet가 서로 다를경우...
                                else
                                {
                              
                                    int ownerLevel = Math.Min(mFirstSelected.Level, sEndSelected.Level);

                                    UltraTreeNode sStParent = mFirstSelected;
                                    UltraTreeNode sEdParent = sEndSelected;


                                    //start parent를 찾는다....
                                    while (sStParent.Level > ownerLevel)
                                    {
                                        sStParent = sStParent.Parent;
                                    }

                                    //end parent를 찾는다...
                                    while (sEdParent.Level > ownerLevel)
                                    {
                                        sEdParent = sEdParent.Parent;
                                    }

                                    while (sStParent.Parent != sEdParent.Parent)
                                    {
                                        sStParent = sStParent.Parent;
                                        sEdParent = sEdParent.Parent;
                                    }


                              
                                
                                    //select owner...
                              

                                    if (mFirstSelected.Level > sStParent.Level)
                                    {
                                        UltraTreeNode curNd = mFirstSelected;
                                        while (curNd.Level != sStParent.Level)
                                        {
                                            curNd = curNd.PrevVisibleNode;
                                            if (curNd.Level == sStParent.Level) break;
                                            mSelectedElements.Add(curNd);
                                        }

                                        GetExcludeNodes(mFirstSelected, sStParent.Level);
                                    }

                                    if (sEndSelected.Level > sEdParent.Level)
                                    {
                                        UltraTreeNode curNd = sEndSelected;
                                        while (true)
                                        {
                                            curNd = curNd.PrevVisibleNode;
                                            if (curNd.Level == sEdParent.Level) break;

                                            mSelectedElements.Add(curNd);
                                        }

                                        GetExcludeNodes(sEndSelected, sEdParent.Level);
                                    }


                                    //첫번째 선택한 node가 위에 있을 경우..
                                    if (sStParent.Index < sEdParent.Index)
                                    {
                                        for (int i = sStParent.Index; i <= sEdParent.Index; i++)
                                        {
                                            var ultraTreeNode = sStParent.Parent.Nodes[i];
                                            if (!ultraTreeNode.Visible)
                                                continue;

                                            mSelectedElements.Add(ultraTreeNode);
                                        }

                                        //Only visiable...의 색깔을 넣기 위해....
                                        UltraTreeNode nextNd = mFirstSelected;
                                        while (nextNd != sEndSelected)
                                        {
                                            nextNd = nextNd.NextVisibleNode;
                                            if (nextNd.Level <= mFirstSelected.Level) continue;

                                            if(mSelectedElements.Contains(nextNd))
                                                continue;

                                            mSelectedElements.Add(nextNd);
                                            mExcludedNodes.Add(nextNd);
                                        }


                                    }

                                        //같을 경우.
                                    else if (sStParent.Index == sEdParent.Index)
                                    {
                                        mSelectedElements.Add(mFirstSelected);
                                    }

                                        //첫번째 선택한 node가 밑에 있을 경우....
                                    else
                                    {
                                        for (int i = sEdParent.Index; i <= sStParent.Index; i++)
                                        {
                                            var ultraTreeNode = sStParent.Parent.Nodes[i];
                                            if (!ultraTreeNode.Visible)
                                                continue;

                                            mSelectedElements.Add(ultraTreeNode);

                                        }


                                        //only visiable...
                                        UltraTreeNode nextNd = mFirstSelected;
                                        while (nextNd != sEndSelected)
                                        {
                                            nextNd = nextNd.PrevVisibleNode;
                                            if (nextNd.Level <= sEndSelected.Level) continue;

                                            if (mSelectedElements.Contains(nextNd))
                                                continue;

                                            mSelectedElements.Add(nextNd);
                                            mExcludedNodes.Add(nextNd);
                                        }
                                    }

                                    if(!mSelectedElements.Contains(mFirstSelected))
                                        mSelectedElements.Add(mFirstSelected);

                                    if(!mSelectedElements.Contains(sEndSelected))
                                        mSelectedElements.Add(sEndSelected);

                                }//end parent가 다를 경우.. selec parent level elements....

                            }

                            mFirstSelected = null;
                            UpdateColor();
                      
                            //exclude redundant parent....
                            foreach (var node in mExcludedNodes.Where(node => mSelectedElements.Contains(node)))
                            {
                                mSelectedElements.Remove(node);
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            ClearSelection();
                            
                        }

                    }
                }

                Selection.CurrentSelection.Members =
                    mSelectedElements.Cast<ExplorerTreeNode>().Select(x => x.Element).ToArray();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                if (mSelectedElements != null)
                {
                    mSelectedElements.ForEach(RestoreBackColor);
                    mSelectedElements.Clear();
                }
            }
        }


        private void GetExcludeNodes(UltraTreeNode node , int excludeLevel)
        {
            var partNd = node.Parent;
            while (partNd.Level >= excludeLevel)
            {
                mExcludedNodes.Add(partNd);
                partNd = partNd.Parent;
            }
        }


        private  void UpdateColor()
        {
            mUltraTree.BeginUpdate();
            SetBackColor(mSelectedElements);
            mUltraTree.EndUpdate();
        }

        public void ClearSelection()
        {
            if (mSelectedElements == null || mSelectedElements.Count <= 0) return;

            mUltraTree.BeginUpdate();

            //변경된 모든 explrere node을 원상태로 돌린다.
            mSelectedElements.ForEach(RestoreBackColor); 
            mSelectedElements.Clear();
            Selection.CurrentSelection.Members = new[]{CurrentElement.Element};

            //shift key을 눌러 선택할 경우 중복 elements가 발생 할 수 있으므로 삭제한다..
            if (mExcludedNodes != null && mExcludedNodes.Any())
            {
                RestoreBackColor(mExcludedNodes);
                mExcludedNodes.Clear();
            }

            mUltraTree.EndUpdate();
        }

        public IEnumerable<DbElement> GetSelectedElements()
        {
            var elements = mSelectedElements.Cast<ExplorerTreeNode>().Select(x => x.Element);
            return elements.Count() != 0 ? elements : new[] {CurrentElement.Element};
        }

        private static void RestoreBackColor(UltraTreeNode node)
        {
            node.Override.NodeAppearance.BackColor = new Color();
        }


        private static void RestoreBackColor(IEnumerable<UltraTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.Override.NodeAppearance.BackColor = new Color();
            }
         
        }
        private static void SetBackColor(UltraTreeNode node)
        {
            node.Override.NodeAppearance.BackColor = SystemColors.Highlight;
        }

        private static void SetBackColor(IEnumerable<UltraTreeNode> nodes)
        {

            foreach (UltraTreeNode node in nodes)
            {
                node.Override.NodeAppearance.BackColor = SystemColors.Highlight;
            }
            
        }


        private void SetExplorer()
        {
            if(ServiceManager.Instance.ApplicationName.ToUpper()=="OUTFITTING")
                mDesExpCtrl = WindowManager.Instance.Windows["DesignExplorer"].Control as ExplorerCtrl;
            else if(ServiceManager.Instance.ApplicationName.ToUpper()=="PARAGON")
                mDesExpCtrl = WindowManager.Instance.Windows["CatalogueExplorer"].Control as ExplorerCtrl;
            else if (ServiceManager.Instance.ApplicationName.ToUpper()=="MARINEDRAFTING")
                mDesExpCtrl = WindowManager.Instance.Windows["DesignExplorer"].Control as ExplorerCtrl;
        }
        

        //rev1 add insert 2dView...
        private void AddToInsertModel()
        {
            var currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            if (mSelectedElements.Count != 0)
                Selection.CurrentSelection.Members =
                    mSelectedElements.Cast<ExplorerTreeNode>().Select(x => x.Element).ToArray();

            var command = CommandManager.Instance.Commands["AVEVA.Marine.UI.Menu.GeneralAddToInsertModel"];           

            command.Execute();

            ClearSelection();
            Cursor.Current = currentCursor;
        }


        private void AddIn3DView()
        {
            var drawListManager = DrawListManager.Instance;

            var currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            drawListManager.BeginUpdate();
            var currentDrawList = drawListManager.CurrentDrawList;

            if(mSelectedElements.Count ==0)
                currentDrawList.Add(CurrentElement.Element);
            else
            {
                currentDrawList.Add(mSelectedElements.Cast<ExplorerTreeNode>().Select(x => x.Element).ToArray());
            }
          
            drawListManager.EndUpdate();
            currentDrawList.VisibleAll();


            ClearSelection();
            Cursor.Current = currentCursor;
        }

        private void DeleteElements()
        {
            var dialogResult = MessageBox.Show("Are you sure delete?", "Delete", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question);
            if(dialogResult != DialogResult.Yes)
            {
                mSelectedElements.Clear();
                return;
            }
              

            if(mSelectedElements.Count == 0)
            {
                var dbElement = CurrentElement.Element;
                if (dbElement.IsDeleteable)
                    dbElement.Delete();
            }

            else
            {
                foreach (ExplorerTreeNode treeNode in mSelectedElements)
                {
                    
                    var dbElement = treeNode.Element;

                    if (dbElement.IsDeleteable)
                    {
                        //treeNode.Remove();
                        dbElement.Delete();
                    }
                    
                }
            }

            mUltraTree.TopNode.Selected = true;
            
            mSelectedElements.Clear();
            //mUltraTree = mDesExpCtrl.Controls.OfType<UltraTree>().First();
            //mUltraTree.Update();
            
            //Aveva.Pdms.Utilities.CommandLine.Command.FormRefresh();
        }



        private void CopyElements()
        {

            MessageBox.Show("Please let me know, how to copy");
/*            var dialogResult = MessageBox.Show("Please Incidate Target","Copy",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if(dialogResult != DialogResult.Yes)
                return;

            var target = CurrentElement.Element;

            foreach (ExplorerTreeNode element in mSelectedElements)
            {
               ExplorerCtrl.DoCopyMove(element.Element,target,ExplorerCtrl.CurrentDropPosition,EditMode.Copy);
            }


            ExplorerCtrl.CurrentDropPosition = DropPositionEnum.None;
            mSelectedElements.Clear();
            Pdms.Utilities.CommandLine.Command.FormRefresh();*/
        }
         



        private void CreateToos()
        {
            mDefaultAddCExpMenus = new List<ITool>();
            var rootTools = CommandBarManager.Instance.RootTools;

            if (rootTools.Contains("PHS.CstExpAddElements"))
                rootTools.Remove("PHS.CstExpAddElements");
            if (rootTools.Contains("PHS.CstExpDeleteElements"))
                rootTools.Remove("PHS.CstExpDeleteElements");

            //3D View에 추가버튼
            var addButtonTool = rootTools.AddButtonTool("PHS.CstExpAddElements", "Add in 3D View", Properties.Resources.Add);          
            addButtonTool.ToolClick += (sender, e) => AddIn3DView();
            mDefaultAddCExpMenus.Add(addButtonTool);

            //삭제버튼
            var DeleteButtonTool = rootTools.AddButtonTool("PHS.CstExpDeleteElements", "선택된 Element삭제", Properties.Resources.Delete);
            DeleteButtonTool.ToolClick += (sender, e) => DeleteElements();
            mDefaultAddCExpMenus.Add(DeleteButtonTool);

            switch (ServiceManager.Instance.ApplicationName.ToUpperInvariant())
            {
                case "HULLDESIGN":
                case "MARINEDRAFTING":
                    if (rootTools.Contains("PHS.CstExpAdd2DElements"))
                        rootTools.Remove("PHS.CstExpAdd2DElements");

                    var add2DviewButtonTool = rootTools.AddButtonTool("PHS.CstExpAdd2DElements",
                                                                      "Add to Insert Model..", Properties.Resources.Add);
                    add2DviewButtonTool.ToolClick += (sender, e) => AddToInsertModel();
                    mDefaultAddCExpMenus.Add(add2DviewButtonTool); 
                    break;
 
                default:

                    break;
            }



/*            
            if(rootTools.Contains("Aveva.Jin.CstExpDeleteElements"))
                rootTools.Remove("Aveva.Jin.CstExpDeleteElements");

            var delBtnTool = rootTools.AddButtonTool("Aveva.Jin.CstExpDeleteElements", "Delete Elements", Properties.Resources.Delete);
            delBtnTool.ToolClick += (sender, e) => DeleteElements();

            mDefaultAddCExpMenus.Add(delBtnTool);*/


       /*     
            if (rootTools.Contains("Aveva.Jin.CstExpCopyElements"))
                rootTools.Remove("Aveva.Jin.CstExpCopyElements");

            var copyBtnTool = rootTools.AddButtonTool("Aveva.Jin.CstExpCopyElements", "Copy Elements", null);
            copyBtnTool.ToolClick += (sender, e) => CopyElements();*/
        }

        public void AddTools(string key, string name, bool isfirst,Bitmap image,  EventHandler onClickEvent)
        {
            var rootTools = CommandBarManager.Instance.RootTools;
            if (rootTools.Contains(key))
                rootTools.Remove(key);

            var addButtonTool = rootTools.AddButtonTool(key, name, image);
            addButtonTool.ToolClick += onClickEvent;

            //
            AddToolsOnExpolrer(key, isfirst, "Design");
        }


        private void AddToolsOnExpolrer(string key, bool isfrist , string expKey)
        {
            var explorerService = ServiceManager.Instance.GetService(typeof(ExplorerService)) as ExplorerService;
            if (explorerService != null)
            {
                var desExpConfig = explorerService.GetExplorerConfig(expKey);
                var toolsCollection = desExpConfig.ContextMenu.Tools;

                if (toolsCollection.Contains(key))
                    toolsCollection.Remove(key);

                var addTool = toolsCollection.AddTool(key);
                addTool.IsFirstInGroup = isfrist;
            }
        }


        private  void RemoveToolsOnDesignExplorer(IExplorerTreeConfig config)
        {
            var toolsCollection = config.ContextMenu.Tools;

            foreach (var key in mDefaultAddCExpMenus.Select(x => x.Key))
            {
                if (toolsCollection.Contains(key))
                    toolsCollection.Remove(key);
            }
        }

        private  void AddToolsOnDesignExplorer(IExplorerTreeConfig config)
        {
            var toolsCollection = config.ContextMenu.Tools;

            for (int i = 0; i < mDefaultAddCExpMenus.Count; i++)
            {
                var tool = mDefaultAddCExpMenus[i];
                if (toolsCollection.Contains(tool.Key))
                    toolsCollection.Remove(tool.Key);

                toolsCollection.AddTool(tool.Key).IsFirstInGroup = i == 0;
            }


/*
            foreach (var tool in mDefaultAddCExpMenus)
            {
                try
                {
                    if (toolsCollection.Contains(tool.Key))
                        toolsCollection.Remove(tool.Key);

                    toolsCollection.AddTool(tool.Key).IsFirstInGroup = tool.IsFirstInGroup;
                }
                catch (Exception e)
                {
                    continue;
                }
            }*/

  /*          if(toolsCollection.Contains("Aveva.Jin.CstExpAddElements"))
                toolsCollection.Remove("Aveva.Jin.CstExpAddElements");

            var addTool = toolsCollection.AddTool("Aveva.Jin.CstExpAddElements");
            addTool.IsFirstInGroup = true;

            if (toolsCollection.Contains("Aveva.Jin.CstExpAdd2DElements"))
                toolsCollection.Remove("Aveva.Jin.CstExpAdd2DElements");
            toolsCollection.AddTool("Aveva.Jin.CstExpAdd2DElements");
           

            if(toolsCollection.Contains("Aveva.Jin.CstExpDeleteElements"))
                toolsCollection.Remove("Aveva.Jin.CstExpDeleteElements");

            var delTool = toolsCollection.AddTool("Aveva.Jin.CstExpDeleteElements");
            delTool.IsFirstInGroup = true;


            if (toolsCollection.Contains("Aveva.Jin.CstExpCopyElements"))
                toolsCollection.Remove("Aveva.Jin.CstExpCopyElements");

            toolsCollection.AddTool("Aveva.Jin.CstExpCopyElements");*/
        }
    } 
}
