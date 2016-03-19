using Aveva.Marine.Utility;
using Aveva.Pdms.Utilities.CommandLine;
using DevExpress.Utils;
using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PHS.Utilities.RadialMenuControl
{
    class RadialMenuItem
    {
        public RadialMenuItem()
        {



        }


        //Radial Menu에 등록될 항목들을 설정하는 부분.
        public BarItem[] CreateBarItems(ref BarManager barManager1)
        {
                       
            // Create bar items to display in Radial Menu
            
            
            BarItem bar_main1 = new BarButtonItem(barManager1, "Comp생성", 0);
            BarItem bar_main2 = new BarButtonItem(barManager1, "Bran", 1);
            BarItem bar_main3 = new BarButtonItem(barManager1, "보조선", 2);
            BarItem bar_main4 = new BarButtonItem(barManager1, "Split", 3);


            // Sub-menu with 3 check buttons
            BarSubItem bar_main5 = new BarSubItem(barManager1, "Util", 4);
                BarItem bar_util1 = new BarButtonItem(barManager1, "Head>1st")
                {
                
                    ImageIndex = 8,                
                };
                BarButtonItem bar_util2 = new BarButtonItem(barManager1,"Tail>Last")
                {
                
                    ImageIndex = 9,
                    //Caption = "Italic"
                };
                BarButtonItem bar_util3 = new BarButtonItem(barManager1, "Tail자동세팅")
                {
                    ImageIndex = 10,                 
                };
                BarItem bar_util4 = new BarButtonItem(barManager1, "엘보<>벤드")
                {
                
                    ImageIndex = 11,                
                };
                BarButtonItem bar_util5 = new BarButtonItem(barManager1,"Flow")
                {
                
                    ImageIndex = 12,
                    //Caption = "Italic"
                };
                BarButtonItem bar_util6 = new BarButtonItem(barManager1, "길이/볼륨")
                {
                    ImageIndex = 13,
                    //Caption = "Underline",

                };
                BarButtonItem bar_util7 = new BarButtonItem(barManager1,"PipeAlign")
                {
                
                    ImageIndex = 14,
                    //Caption = "Italic"
                };
                BarButtonItem bar_util8 = new BarButtonItem(barManager1, "Reorderbasic")
                {
                    ImageIndex = 15,
                    //Caption = "Underline",

                };
            BarItem[] subMenuItems = new BarItem[] {bar_util1,bar_util2,bar_util3,bar_util4,bar_util5,bar_util6,bar_util7,bar_util8 };
            bar_main5.AddItems(subMenuItems);

            BarSubItem bar_main6 = new BarSubItem(barManager1, "체크", 5);
                BarButtonItem barbtnchecksounding = new BarButtonItem(barManager1, "Sounding")
                {

                    ImageIndex = 16,
                };
                BarButtonItem barbtncheckconn = new BarButtonItem(barManager1, "Conn.")
                {

                    ImageIndex = 17,
                    //Caption = "Italic"
                };
                BarButtonItem barbtncheckelec = new BarButtonItem(barManager1, "전장품상부")
                {
                    ImageIndex = 18,
                    
                    //Caption = "Underline",

                };
                BarButtonItem barbtncheckstd_design = new BarButtonItem(barManager1, "표준체크")
                {
                    ImageIndex = 19,                   
                };
                BarButtonItem barbtncheck5 = new BarButtonItem(barManager1, "도장관체크")
                {
                    ImageIndex = 20,
                };

                bar_main6.AddItems(new BarItem[] { barbtnchecksounding, barbtncheckconn, barbtncheckelec, barbtncheckstd_design, barbtncheck5 });

            BarItem bar_main7 = new BarButtonItem(barManager1, "생산정보",6);
            BarItem bar_main8 = new BarButtonItem(barManager1, "3D박스", 7);

            bar_main1.ItemClick += barItem_ItemClick;
            bar_main2.ItemClick += barItem_ItemClick;
            bar_main3.ItemClick += barItem_ItemClick;
            bar_main4.ItemClick += barItem_ItemClick;
            bar_main5.ItemClick += barItem_ItemClick;
            bar_main6.ItemClick += barItem_ItemClick;
            bar_main7.ItemClick += barItem_ItemClick;
            bar_main8.ItemClick += barItem_ItemClick;

            bar_util1.ItemClick += barItem_ItemClick;
            bar_util2.ItemClick += barItem_ItemClick;
            bar_util3.ItemClick += barItem_ItemClick;
            bar_util4.ItemClick += barItem_ItemClick;
            bar_util5.ItemClick += barItem_ItemClick;
            bar_util6.ItemClick += barItem_ItemClick;
            bar_util7.ItemClick += barItem_ItemClick;
            bar_util8.ItemClick += barItem_ItemClick;

            barbtnchecksounding.ItemClick += barItem_ItemClick;
            barbtncheckconn.ItemClick += barItem_ItemClick;
            barbtncheckelec.ItemClick += barItem_ItemClick;
            barbtncheckstd_design.ItemClick += barItem_ItemClick;
            barbtncheck5.ItemClick += barItem_ItemClick;

            return new BarItem[] {bar_main1, bar_main2, bar_main3, bar_main4, 
                bar_main5, bar_main6, bar_main7, bar_main8};
        }

        void barItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(e.Item.Caption=="Comp생성")
                Command.CreateCommand("show !!testcomponentcreation").RunInPdms();
            else if (e.Item.Caption == "Bran")
                Command.CreateCommand("show !!createbranch").RunInPdms();
            else if (e.Item.Caption == "보조선")
                Command.CreateCommand("show !!edglines").RunInPdms();
            else if (e.Item.Caption == "Split")
                Command.CreateCommand("show !!testpipeSplitting").RunInPdms();

            else if (e.Item.Caption == "Head>1st")
                Command.CreateCommand("!!connection(1)").RunInPdms();

            else if (e.Item.Caption == "Tail>Last")
                Command.CreateCommand("!!connection(2)").RunInPdms();
            
            else if (e.Item.Caption == "Flow")
                Command.CreateCommand("!!showpipeflow2()").RunInPdms();
            
            else if (e.Item.Caption == "Sounding")
                Command.CreateCommand("show !!soundingpipe").RunInPdms();

            else if (e.Item.Caption == "Conn.")
                Command.CreateCommand(" show !!modelconnectioncheck").RunInPdms();

            else if (e.Item.Caption == "전장품상부")
                Command.CreateCommand(" show !!elecflangechk").RunInPdms();
            
            else if (e.Item.Caption == "표준체크")
                Command.CreateCommand("show !!PipeCheck").RunInPdms();

            else if (e.Item.Caption == "각도변경")
                Command.CreateCommand("!!angle()").RunInPdms();

            else if (e.Item.Caption == "엘보<>벤드")                
                Command.CreateCommand("!!sametype()").RunInPdms();

            else if (e.Item.Caption == "생산정보")
                Command.CreateCommand("show !!productioninfo").RunInPdms();
            else if (e.Item.Caption == "3D박스")
                Command.CreateCommand("show !!boxcontrol").RunInPdms();
            else if (e.Item.Caption == "Reorderbasic")
                Command.CreateCommand("show !!reorderbasic").RunInPdms();
            else if (e.Item.Caption == "PipeAlign")
                Command.CreateCommand("show !!opafputilfrm").RunInPdms();
            else if (e.Item.Caption == "길이/볼륨")
                Command.CreateCommand("show !!PipeLengthAndVolume").RunInPdms();
            else if (e.Item.Caption == "도장관체크")
                Command.CreateCommand("show !!spoolpaintlengthchk").RunInPdms();
              
        }

    }
}
