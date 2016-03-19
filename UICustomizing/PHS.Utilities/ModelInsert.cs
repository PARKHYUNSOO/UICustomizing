using Aveva.Pdms.Database;
using Aveva.Pdms.Graphics;
using Aveva.PDMS.Database.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PHS.Utilities
{
    public partial class ModelInsert : Form
    {
        public ModelInsert()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnModelInsert_Click(object sender, EventArgs e)
        {
            //Drawlist 정의하고 추가하는 부분
            var drawListManager = DrawListManager.Instance;

            var currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            drawListManager.BeginUpdate();
            var currentDrawList = drawListManager.CurrentDrawList;

            DbElement Outfit_Elements = MDB.CurrentMDB.GetFirstWorld(Aveva.Pdms.Database.DbType.Design);

            DbElementType[] dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE };
            if (checkPipe.Checked)
            {
                dbtypes = new DbElementType[] { DbElementTypeInstance.PIPE };
                DBElementCollection pipe_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_pipe_name.Text,txt_pipe_module.Text);
                currentDrawList.Add(pipe_collection.Cast<DbElement>().ToArray());
            }
            if (checkStru.Checked)
            {
                dbtypes = new DbElementType[] { DbElementTypeInstance.STRUCTURE };
                DBElementCollection stru_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_stru_name.Text,txt_stru_module.Text);
                currentDrawList.Add(stru_collection.Cast<DbElement>().ToArray());
            }
            if (checkEquip.Checked)
            {
                dbtypes = new DbElementType[] { DbElementTypeInstance.EQUIPMENT };
                DBElementCollection equip_collection = Get_Element_Collection(Outfit_Elements, dbtypes, txt_equip_name.Text,txt_equip_module.Text);
                currentDrawList.Add(equip_collection.Cast<DbElement>().ToArray());
            }





            drawListManager.EndUpdate();
            currentDrawList.VisibleAll();



            Cursor.Current = currentCursor;

        }

        private DBElementCollection Get_Element_Collection(DbElement Outfit_Elements,DbElementType [] dbtypes,string search_txt,string modulename)
        {
            DBElementCollection result_collection = null;
            try
            {
                
                TypeFilter filter = new TypeFilter(dbtypes);
                AndFilter finalfilter = new AndFilter();

                AttributeStringFilter filter2 = null;
                AttributeRefFilter filter3 = null;

                if (search_txt.StartsWith("*") && search_txt.EndsWith("*"))
                {
                    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.EndsWith, search_txt.Replace("*", ""));
                }
                else if (search_txt.EndsWith("*") && !search_txt.StartsWith("*"))
                {
                    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.StartsWith, search_txt.Replace("*", ""));
                }
                else if (search_txt.StartsWith("*") && search_txt.EndsWith("*"))
                {
                    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.Contains, search_txt.Replace("*", ""));
                }
                else
                {
                    filter2 = new AttributeStringFilter(DbAttribute.GetDbAttribute("NamN"), FilterOperator.Equals, search_txt.Replace("*", ""));
                }
                if(modulename.Trim()!="")
                {
                    filter3=new AttributeRefFilter(DbAttributeInstance.OWNER, FilterOperator.Equals, DbElement.GetElement("/"+modulename));
                    finalfilter.Add(filter3);    
                }

                finalfilter.Add(filter);
                finalfilter.Add(filter2);


                result_collection = new DBElementCollection(Outfit_Elements, finalfilter);

            }

            catch (Exception ee)
            {
                Console.WriteLine("오류");
            }

            return result_collection;
        }
    }
}
