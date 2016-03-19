using Aveva.Pdms.Database;
using Aveva.PDMS.Database.Filters;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;
using SourceGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PHS.Utilities.ModelInsertPGM
{
    public partial class ModelInsertVerify : XtraForm
    {
        public ModelInsertVerify()
        {
            InitializeComponent();
        }
        DbElement[] pipe_collection;
        DbElement[] stru_collection;
        DbElement[] equip_collection;
        DbElement[] hull_collection;
        DbElement[] rso_collection;
        ModelInsert ownerform;
        public ModelInsertVerify(ModelInsert ownerform, DbElement[] pipe_collection, DbElement[] stru_collection, DbElement[] equip_collection, DbElement[] hull_collection, DbElement[] rso_collection)
        {
            InitializeComponent();
            this.ownerform = ownerform;

            this.pipe_collection = pipe_collection;
            this.stru_collection = stru_collection;
            this.equip_collection = equip_collection;
            this.hull_collection = hull_collection;
            this.rso_collection = rso_collection;
        }

        private void ModelInsertVerify_Load(object sender, EventArgs e)
        {
            try
            {
                OfficeSkins.Register();
                BonusSkins.Register();
                SkinManager.EnableFormSkins();
                defaultLookAndFeel1.LookAndFeel.SkinName = "McSkin";
                grid1.BorderStyle = BorderStyle.FixedSingle;
                grid1.ColumnsCount = 4;
                grid1.FixedRows = 1;
                grid1.Rows.Insert(0);
                grid1[0, 0] = new SourceGrid.Cells.ColumnHeader("체크");
                grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("Type");
                grid1[0, 2] = new SourceGrid.Cells.ColumnHeader("Model Name");
                grid1[0, 3] = new SourceGrid.Cells.ColumnHeader("Zone Name");
                grid1.Columns[0].Width = 50;
                grid1.Columns[1].Width = 70;
                grid1.Columns[2].Width = 250;
                grid1.Columns[3].Width = 250;
                int rowcnt = 0;

                foreach (DbElement element in pipe_collection)
                {
                    if (element.IsNull == true)
                    {
                        continue;
                    }

                    grid1.Rows.Insert(rowcnt + 1);
                    grid1[rowcnt + 1, 0] = new SourceGrid.Cells.CheckBox(null, false);
                    grid1[rowcnt + 1, 1] = new SourceGrid.Cells.Cell(element.GetAsString(DbAttributeInstance.TYPE), typeof(string));
                    grid1[rowcnt + 1, 2] = new SourceGrid.Cells.Cell(element, typeof(DbElement));
                    grid1[rowcnt + 1, 3] = new SourceGrid.Cells.Cell(element.GetElement(DbAttributeInstance.OWNER).GetAsString(DbAttributeInstance.NAMN), typeof(DbElement));
                    //grid1[rowcnt, 2] = new SourceGrid.Cells.Cell(element.GetAsString(DbAttributeInstance.TYPE), typeof(string));
                    rowcnt++;
                }

                foreach (DbElement element in stru_collection)
                {
                    if (element.IsNull == true)
                    {
                        continue;
                    }

                    grid1.Rows.Insert(rowcnt + 1);
                    grid1[rowcnt + 1, 0] = new SourceGrid.Cells.CheckBox(null, false);
                    grid1[rowcnt + 1, 1] = new SourceGrid.Cells.Cell(element.GetAsString(DbAttributeInstance.TYPE), typeof(string));
                    grid1[rowcnt + 1, 2] = new SourceGrid.Cells.Cell(element, typeof(DbElement));
                    grid1[rowcnt + 1, 3] = new SourceGrid.Cells.Cell(element.GetElement(DbAttributeInstance.OWNER).GetAsString(DbAttributeInstance.NAMN), typeof(DbElement));
                    rowcnt++;
                }
                foreach (DbElement element in equip_collection)
                {
                    if (element.IsNull == true)
                    {
                        continue;
                    }

                    grid1.Rows.Insert(rowcnt + 1);
                    grid1[rowcnt + 1, 0] = new SourceGrid.Cells.CheckBox(null, false);
                    grid1[rowcnt + 1, 1] = new SourceGrid.Cells.Cell(element.GetAsString(DbAttributeInstance.TYPE), typeof(string));
                    grid1[rowcnt + 1, 2] = new SourceGrid.Cells.Cell(element, typeof(DbElement));
                    grid1[rowcnt + 1, 3] = new SourceGrid.Cells.Cell(element.GetElement(DbAttributeInstance.OWNER).GetAsString(DbAttributeInstance.NAMN), typeof(DbElement));
                    rowcnt++;
                }
                foreach (DbElement element in hull_collection)
                {
                    if (element.IsNull == true)
                    {
                        continue;
                    }

                    grid1.Rows.Insert(rowcnt + 1);
                    grid1[rowcnt + 1, 0] = new SourceGrid.Cells.CheckBox(null, false);
                    grid1[rowcnt + 1, 1] = new SourceGrid.Cells.Cell(element.GetAsString(DbAttributeInstance.TYPE), typeof(string));
                    grid1[rowcnt + 1, 2] = new SourceGrid.Cells.Cell(element, typeof(DbElement));
                    grid1[rowcnt + 1, 3] = new SourceGrid.Cells.Cell(element.GetElement(DbAttributeInstance.OWNER).GetAsString(DbAttributeInstance.NAMN), typeof(DbElement));

                    rowcnt++;
                }
                foreach (DbElement element in rso_collection)
                {
                    if (element.IsNull == true)
                    {
                        continue;
                    }

                    grid1.Rows.Insert(rowcnt + 1);
                    grid1[rowcnt + 1, 0] = new SourceGrid.Cells.CheckBox(null, false);
                    grid1[rowcnt + 1, 1] = new SourceGrid.Cells.Cell(element.GetAsString(DbAttributeInstance.TYPE), typeof(string));
                    grid1[rowcnt + 1, 2] = new SourceGrid.Cells.Cell(element, typeof(DbElement));
                    grid1[rowcnt + 1, 3] = new SourceGrid.Cells.Cell(element.GetElement(DbAttributeInstance.OWNER).GetAsString(DbAttributeInstance.NAMN), typeof(DbElement));

                    rowcnt++;
                }
                lblcnt.Text = "검색건수 : " + rowcnt.ToString();
            }catch(Exception ee)
            {
                Console.WriteLine("올류");
            }
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            var selectrow=grid1.Rows.Cast<SourceGrid.GridRow>().Where(x => x.Grid.GetCell(x.Index, 0).ToString() == "True");
            var selectrow2 = grid1.Rows.Cast<SourceGrid.GridRow>().Where(x => x.Grid.GetCell(x.Index, 0).ToString() == "True").Select(r=>(DbElement)(r.Grid.GetCell(r.Index,1)));
            var selectrow3 = selectrow2 = grid1.Rows.Cast<SourceGrid.GridRow>().Where(x => x.Grid.GetCell(x.Index, 0).ToString() == "True").Select(r => (DbElement)((SourceGrid.Cells.Cell)(r.Grid.GetCell(r.Index, 2))).Value);
            //var selectrow3 = grid1.Rows.Cast<SourceGrid.GridRow>().Where(x => x.Grid.GetCell(x.Index, 0).ToString() == "True");

            ownerform.VerifyModelList.AddRange(selectrow3);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_includeall_Click(object sender, EventArgs e)
        {
            foreach(GridRow row in grid1.Rows)
            {
                if (row.Index == 0)
                    continue;
                grid1[row.Index, 0].Value = true;
            }
        }

        private void btn_excludeall_Click(object sender, EventArgs e)
        {
            foreach (GridRow row in grid1.Rows)
            {
                if (row.Index == 0)
                    continue;
                grid1[row.Index, 0].Value = false;
            }
        }
    }
}
