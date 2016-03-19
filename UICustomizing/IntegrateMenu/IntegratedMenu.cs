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
namespace HMD.AM.IntegratedMenu
{
    [PMLNetCallable()]
    public partial class IntegratedMenu : UserControl
    {

        TreeModel treemodel = new TreeModel();
        [PMLNetCallable()]
        public event PMLNetDelegate.PMLNetEventHandler PMLNetExampleEvent;

        [PMLNetCallable()]
        public IntegratedMenu()
        {
            InitializeComponent();
        }
        [PMLNetCallable()]
        private void IntegratedMenu_Load(object sender, EventArgs e)
        {
            qExplorerItem1.Expanded = true;
            
            
            qExplorerItem1.Expanded = false;
            //_tree.Model = treemodel;

            ArrayList ar = new ArrayList();
            ar.Add("1!");
            ar.Add("12");
            ar.Add("13");
            Node node1 = new Node("바보");
            node1.Text = "바보";
            node1.Image = imageList1.Images[0];


            node1.Tag = ar;
            
            Node node2 = new Node("청멍");
            node2.Text = "바보";
            node1.Image = imageList1.Images[1];

            treemodel.Nodes.Add(node1);
            //treemodel.Nodes.Add(node1);
            treemodel.Nodes.Add(new Node("바보3"));
            treemodel.Nodes.Add(new Node("바보4"));
            //treemodel.Nodes[0].Nodes.Add(node1);
            treemodel.Nodes[0].Nodes.Add(node2);
            treeViewAdv1.Model = treemodel;


            Command cmd = Command.CreateCommand("");
            cmd.CommandString = "$p parkmaker";
            cmd.RunInPdms();
            
        }

        [PMLNetCallable()]
        public void Assign(IntegratedMenu that)
        {
        }
        [PMLNetCallable()]
        private void button1_Click(object sender, EventArgs e)
        {

            Command cmd = Command.CreateCommand("");
            cmd.CommandString = "!!autodrawingstruct()";
            cmd.RunInPdms();

            

        }
        [PMLNetCallable()]
        public void xxx()
        {
        }

        private void qExplorerBar1_CustomizeMenuShowed(object sender, EventArgs e)
        {
            MessageBox.Show("!1");
        }


    }
}
