using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PHS.Utilities
{
    class Configure_Menu
    {
        
        static public void LoadDll(String fName, String className,object [] construct_Args, String methodName)
        {
            try
            {
                if (File.Exists(fName))
                {
                    Assembly assembly = LoadFile(fName);
                    String assemblyName = Path.GetFileNameWithoutExtension(fName.Substring(fName.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                    System.Type type = assembly.GetType(className);
                    object target = Activator.CreateInstance(type, construct_Args);
                    type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, target, new object[0]);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error to load dll", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        static public Assembly LoadFile(string filename)
        {

            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }



        }
        public void RunCmd(object sender, EventArgs e)
        {

            ButtonTool kb = sender as ButtonTool;
            String fName = kb.Caption;
            String className = kb.Caption;
            //
            //LoadDll(@"C:\AVEVA\Marine\OH12.1.SP4\PHS.Utilities.dll", "PHS.Utilities.Specgenerator", "run");//
            Specgenerator spec_gen = new Specgenerator();
            spec_gen.run();
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p |실행했다옹|").RunInPdms();

        }


        CommandBarManager sCommandBarManager = null;
        public Configure_Menu()
        {


            
        }
        public Configure_Menu(CommandBarManager sCommandBarManager)
        {
            this.sCommandBarManager = sCommandBarManager;            
        }
        public void run()
        {
            
            if (ServiceManager.Instance.ApplicationName != "Paragon")
            {
                //windowManager.WindowLayoutLoaded += windowManager_WindowLayoutLoaded;
            }
            else
            {

                //create toolbar
                CommandBar myToolBar = sCommandBarManager.CommandBars.AddCommandBar("mytoolbar");
                myToolBar.Caption = "박현수 파라곤 유틸리티";


                //CREATE BUTTON
                ButtonTool bt = sCommandBarManager.RootTools.AddButtonTool("specgenerator", "specgenerator", Image.FromFile(@"Q:\env\Icons\AngleDetail.jpg"));
                bt.Tooltip = "보바수바";
                bt.ToolClick += new System.EventHandler(this.RunCmd);

                //CREATE BUTTON
                ButtonTool bt2 = sCommandBarManager.RootTools.AddButtonTool("park2", "33", Image.FromFile(@"Q:\env\Icons\banmok.jpg"));
                bt2.Tooltip = "보바수바2";
                bt2.ToolClick += new System.EventHandler(this.RunCmd);


                //CREATE COMBOBOX
                ComboBoxTool cb = sCommandBarManager.RootTools.AddComboBoxTool("park3", "44", null);

                //ADD VALUE LIST 
                cb.ValueList.Add("11");
                cb.ValueList.Add("12");
                cb.ValueList.Add("13");

                //ADD TOOLBAR ITEM 
                myToolBar.Tools.AddTool("specgenerator");
                myToolBar.Tools.AddTool("park2");
                myToolBar.Tools.AddTool("park3");
                //sCommandBarManager.RootTools.AddButtonTool()




                //myToolBar.Tools.AddTool("nbCommand2");
                //myToolBar.Tools.AddTool("nbCommand3");
                //myToolBar.Tools.AddTool("nbCommand4");

                //for (int i = 0; i < mDlls.Count; i++)
                //{


                //    System.Drawing.Image icon  = null;              
                //    ButtonTool bt = sCommandBarManager.RootTools.AddButtonTool("babo1", "babo11-1", icon);
                //    bt.Tooltip = "baboya";
                //    bt.ToolClick += new System.EventHandler(this.RunCmd);
                //    myToolBar.Tools.AddTool(fName);

                //}

            }
        }
    }
}
