using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;

using Aveva.Pdms.Explorer;
using PHS.ExplorerCommands;

namespace PHS.CustomizingExplorer
{
    class CustomExplorerAddin:IAddin
    {
        private CustomExplorer mCustExp;
        internal static IExplorerTreeConfig DesigExpTreeConfig ;
        internal static List<ExplorerContextMenu> SingleSelectionMenus;
        internal static List<ExplorerContextMenu> MultiSelectionMenus; 

        private ServiceManager mServiceManager;

        public void Start(ServiceManager serviceManager)
        {
            Console.WriteLine("Start Multi Selecting ExplorerAddin");
            mServiceManager = serviceManager;
            var windowManager = mServiceManager.GetService(typeof (WindowManager)) as WindowManager;
            windowManager.WindowLayoutLoaded += windowManager_WindowLayoutLoaded;
            
        }


        void windowManager_WindowLayoutLoaded(object sender, EventArgs e)
        {

            var explorerService = mServiceManager.GetService(typeof(ExplorerService)) as ExplorerService;
            var sCmdBarManager = mServiceManager.GetService(typeof(CommandBarManager)) as CommandBarManager;

            
            SingleSelectionMenus = new List<ExplorerContextMenu>();
            MultiSelectionMenus = new List<ExplorerContextMenu>();
            
            if (explorerService != null)
            {
                if (ServiceManager.Instance.ApplicationName.ToUpper() == "OUTFITTING")
                    DesigExpTreeConfig = explorerService.GetExplorerConfig("Design");
                else if (ServiceManager.Instance.ApplicationName.ToUpper() == "PARAGON")
                    DesigExpTreeConfig = explorerService.GetExplorerConfig("Catalogue");
                else if (ServiceManager.Instance.ApplicationName.ToUpper() == "MARINEDRAFTING")
                    DesigExpTreeConfig = explorerService.GetExplorerConfig("Design");



                //DesigExpTreeConfig = explorerService.GetExplorerConfig("Design");
                //explorerService.GetExplorerConfig("Catalogue");
                //explorerService.GetExplorerConfig("Draft");
            }
            
            mCustExp = CustomExplorer.Instance;

            
            var stbtnMSelect = sCmdBarManager.RootTools.AddStateButtonTool("PHS.DoMulitySelect", "Multi Selection Mode", null);


            foreach (ITool tool in DesigExpTreeConfig.ContextMenu.Tools)
            {
                SingleSelectionMenus.Add(new ExplorerContextMenu(tool));
            }

            DesigExpTreeConfig.ContextMenu.Tools.InsertTool("PHS.DoMulitySelect", 0).IsFirstInGroup = true;

            stbtnMSelect.Checked = false;
            //mCustExp.Start();

            stbtnMSelect.ToolClick += stbtnMSelect_ToolClick;


            //C:\AVEVA\Marine\OH12.1.SP4\DesignExplorer.uic 파일이 만들어지고 해당 메뉴를 Design Explorer의 Popup Menu에 추가한다.
            //add multiy..
            if (ServiceManager.Instance.ApplicationName.ToUpper() == "MARINEDRAFTING")
            {
                var uicFile = ExplorerCommandUic.CreateUicFile("Design");
                foreach (var tool in ExplorerCommands.ExplorerCommands.CreateButtons(uicFile, DesigExpTreeConfig.ContextMenu, "MTool"))
                {
                    SingleSelectionMenus.Add(new ExplorerContextMenu(tool));
                }
            }

            mCustExp.settingdata();

        }

        void stbtnMSelect_ToolClick(object sender, EventArgs e)
        {
            //rev2 
            //mulity selection 모드 변경시 tool을 multy  또는 single mode로 
            //변경하고 다시 뛰운다...
            var buttonTool = sender as StateButtonTool;
            try
            {
                if (buttonTool.Checked)
                {

                    mCustExp.Start();
                    

                }
                else
                {
                    mCustExp.Finish();

                }
            }
            catch (Exception ee)
            {
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p |error to multiselect|").RunInPdms();
            }
            //rev2 
            //mulity selection 모드 변경시 tool을 multy  또는 single mode로 
            //변경하고 다시 뛰운다...
            DesigExpTreeConfig.ContextMenu.ShowPopup();
        }

        public void Stop()
        {
            mCustExp.Finish();
        }

        public string Name { get { return "PHS.CustomizingExplorer"; } }
        public string Description { get { return "Design Explorere Multi Selecting Addin"; } } 
    }
}
