using Aveva.Pdms.Database;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Ribbon;
using PHS.Utilities.ModelInsertPGM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using UICustomizing.Canvas_Addin;

namespace PHS.Utilities
{
    class StartForm
    {
        public StartForm()
        {

        }
        string formname = "projectchange";
        string username = "";
        System.Drawing.Point pos = new System.Drawing.Point(0, 0);
        Form owner = null;
        //private BarManager barManager1;
        //static RadialMenu radialmenu = new RadialMenu();
       
        public StartForm(string formname)
        {
            this.formname = formname;
        }

        public StartForm(string formname,System.Drawing.Point pos)
        {
            
            this.formname = formname;
            this.pos = pos;
        }
        public StartForm(string formname, Form owner)
        {

            this.formname = formname;
            this.owner = owner;
        }
        public StartForm(string formname, string username)
        {

            this.formname = formname;
            this.username = username;
        }
        public void Showform()
        {


            

            if (formname == "projectchange")
            {
                ProjectChange pc = new ProjectChange(this.username);
                if (pc.run() == true)
                {
                    pc.Show();
                    pc.TopMost = true;
                    pc.BringToFront();
                }
            }
            else if(formname == "modelinsert")
            {
                ModelInsert mi = new ModelInsert();
                mi.TopMost = true;
                mi.Show();
            }
            //else if (formname == "helpform")
            //{
            //    HelpForm hf = new HelpForm();
            //    hf.Location = pos;
                
            //    hf.Show();
            //    hf.TopMost = true;
            //}
            else if(formname=="specgenform")
            {
                SpecGen_Form specform = new SpecGen_Form();
                specform.Show();
                specform.TopMost = true;
            }
            else if (formname == "ElementRenameForm")
            {
                ElementRenameForm renameform = new ElementRenameForm();
                renameform.Show();
                renameform.TopMost = true;
            }

            else if(formname=="RadialMenu")
            {
                //try
                //{

                //    this.barManager1 = new DevExpress.XtraBars.BarManager();
                //    this.barManager1.Form = owner;


                //    radialmenu.ClearLinks();
                //    radialmenu.Manager = barManager1;
                //    radialmenu.InnerRadius = 90;
                //    radialmenu.MenuRadius = 150;
                //    RadialMenuItem menuitem = new RadialMenuItem();
                //    //barManager1.Images= menuitem.CreateImageCollection();
                //    DummyForm dummy = new DummyForm();


                //    barManager1.Images = dummy.GetImageCollection();
                //    BarItem[] baritems = menuitem.CreateBarItems(ref barManager1);
                //    radialmenu.AddItems(baritems);
                //    //menu.BackColor = Color.Transparent;
                //    //menu.BorderColor = Color.Transparent;
                //    radialmenu.Glyph = global::PHS.Utilities.Properties.Resources.box;
                //    radialmenu.ShowPopup(Cursor.Position, true);

                //    //menu.Expand();
                //}
                //catch (Exception ee)
                //{

                //}
            }

            
        }
    }
}
