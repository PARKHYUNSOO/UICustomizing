using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UICustomizing.Canvas_Addin
{
    public partial class DummyForm : Form
    {
        public DummyForm()
        {
            InitializeComponent();
        }

        private void DummyForm_Load(object sender, EventArgs e)
        {

        }
        public ImageCollection GetImageCollection()
        {
            return imageCollection1;
        }
    }
}
