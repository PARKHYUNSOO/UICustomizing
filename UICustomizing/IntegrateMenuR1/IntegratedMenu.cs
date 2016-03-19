using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aveva.PDMS.PMLNet;
using Aveva.Pdms.Utilities.CommandLine;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using Aga.Controls;
using System.Collections;
using System.Threading;
namespace HMD.AM.IntegratedMenu
{
    [PMLNetCallable()]
    public partial class IntegratedMenu : Form
    {

        TreeModel treemodel = new TreeModel();
       // [PMLNetCallable()]
       //public event PMLNetDelegate.PMLNetEventHandler PMLNetExampleEvent;

        [PMLNetCallable()]
        public IntegratedMenu()
        {
            InitializeComponent();
        }
        [PMLNetCallable()]
        private void IntegratedMenu_Load(object sender, EventArgs e)
        {
            

            //ex1_dwgutility.Expanded = true;
            //ex1_generalutility.Expanded = true;
            //ex1_modelutil.Expanded = true;
            
            
            ////_tree.Model = treemodel;

            //ArrayList ar = new ArrayList();
            //ar.Add("1!");
            //ar.Add("12");
            //ar.Add("13");
            //Node node1 = new Node("바보");
            //node1.Text = "바보";
            //node1.Image = imageList1.Images[0];


            //node1.Tag = ar;
            
            //Node node2 = new Node("청멍");
            //node2.Text = "바보";
            //node1.Image = imageList1.Images[1];

            //treemodel.Nodes.Add(node1);
            ////treemodel.Nodes.Add(node1);
            //treemodel.Nodes.Add(new Node("바보3"));
            //treemodel.Nodes.Add(new Node("바보4"));
            ////treemodel.Nodes[0].Nodes.Add(node1);
            //treemodel.Nodes[0].Nodes.Add(node2);
            ////.Model = treemodel;


            ////Command cmd = Command.CreateCommand("");
            ////cmd.CommandString = "$p parkmaker";
            ////cmd.RunInPdms();
            
        }

        [PMLNetCallable()]
        public void Assign(IntegratedMenu that)
        {
        }
        [PMLNetCallable()]
        private void button1_Click(object sender, EventArgs e)
        {

            //Command cmd = Command.CreateCommand("");
            //cmd.CommandString = "!!autodrawingstruct()";
            //cmd.RunInPdms();
            

        }



        [PMLNetCallable()]
        public void xxx()
        {
        }

        private void qExplorerBar1_CustomizeMenuShowed(object sender, EventArgs e)
        {
            MessageBox.Show("!1");
        }

        private void qRibbonPage3_Activated(object sender, EventArgs e)
        {

        }

        private void qRibbonPanel10_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {

        }

        private void qRibbon1_HelpButtonActivated(object sender, EventArgs e)
        {
            MessageBox.Show("222");
        }

        private void qRibbonItem5_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {

        }

        private void qRibbonItem17_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            Command cmd = Command.CreateCommand("");
            cmd.CommandString = "show !!reorderbasic";
            cmd.RunInPdms();
        }

        private void qRibbonItem7_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p |실행했다옹 예은아 힘내자|").RunInPdms();
        }

        private void qRibbonItem3_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            Command cmd = Command.CreateCommand("");
            cmd.CommandString = "show !!reorderbasic";
            cmd.RunInPdms();
        }


    }
}
