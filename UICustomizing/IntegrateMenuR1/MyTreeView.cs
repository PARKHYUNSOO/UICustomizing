﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HMD.AM.IntegratedMenu
{
    class MyTreeView : TreeView
    {

        private Image mImage;
        public Image Image
        {
            get { return mImage; }
            set { mImage = value; Invalidate(); }
        }
        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            if (mImage != null) Invalidate();
            base.OnAfterCollapse(e);
        }
        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            if (mImage != null) Invalidate();
            base.OnAfterExpand(e);
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x14 && mImage != null)
            {
                using (var gr = Graphics.FromHdc(m.WParam))
                {
                    gr.DrawImage(mImage, Point.Empty);
                }
            }
        }
    }
}
