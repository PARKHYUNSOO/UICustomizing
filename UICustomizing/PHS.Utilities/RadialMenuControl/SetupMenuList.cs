using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PHS.Utilities.RadialMenuControl
{
    public partial class SetupMenuList : XtraForm
    {
        public SetupMenuList()
        {
            InitializeComponent();
        }

        private void SetupMenuList_Load(object sender, EventArgs e)
        {
            OfficeSkins.Register();
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            defaultLookAndFeel1.LookAndFeel.SkinName = "McSkin";
        }
    }
}
