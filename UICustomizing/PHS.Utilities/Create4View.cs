using Aveva.ApplicationFramework.Presentation;
using Aveva.Pdms.Presentation.PDMSCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PHS.Utilities
{
    public class Create4View
    {

        public Create4View()
        {
            
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11; 
        public void run()
        {
            try
            {
                SendMessage(WindowManager.Instance.MainForm.Handle, WM_SETREDRAW, false, 0);
                PMLNetCommandManager cmdmanager = new PMLNetCommandManager();
                
                int formcount = WindowManager.Instance.Windows.OfType<MdiWindow>().Count();

                if (formcount < 4)
                {

                    for (int i = 0; i < 4 - formcount; i++)
                    {
                        //뷰추가
                        string command = "!!gphViews.addDesign()";
                        Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(command).RunInPdms();

                                            }



                }
                else if (formcount > 4)
                {
                    for (int i = formcount - 1; i > 3; i--)
                    {
                        WindowManager.Instance.MainForm.MdiChildren[i].Close();

                    }
                }

                List<MdiWindow> windowlist = WindowManager.Instance.Windows.OfType<MdiWindow>().ToList();

                ////Section
                windowlist[0].Form.Top = 40;
                windowlist[0].Form.Left = 40;
              //  windowlist[0].Title = "PLAN (LOOK -Z)";
                windowlist[1].Form.Top = 40;
                windowlist[1].Form.Left = 20;
//                windowlist[1].Title = "ISO3";
                windowlist[2].Form.Top = 40;
                windowlist[2].Form.Left = 30;
  //              windowlist[2].Title = "ELEV (LOOK Y)";
                windowlist[3].Form.Top = 40;
                windowlist[3].Form.Left = 10;
    //            windowlist[3].Title = "SECTION (LOOK X)";
                

                
                


                windowlist[0].Form.BringToFront();
                windowlist[1].Form.BringToFront();
                windowlist[2].Form.BringToFront();
                windowlist[3].Form.BringToFront();

                
                
                //PLAN
                string cmdstr = String.Format(@"!dir = object array()");
                string cmdstr2= String.Format(@"!dir.append(0) !dir.append(0) !dir.append(-1) ");
                string cmdstr3=String.Format(@" !!gph3ddesign4.view.direction= !dir !!gph3ddesign4.view.bearing = 0");
                
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr).RunInPdms();
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr2).RunInPdms();
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr3).RunInPdms();

                // ISO
                cmdstr = String.Format(@"!dir = object array()");
                cmdstr2 = String.Format(@"!dir.append( 0.577350273783069) !dir.append( 0.577350273783069) !dir.append( -0.57735026000274) ");
                cmdstr3 = String.Format(@" !!gph3ddesign2.view.direction= !dir ");
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr).RunInPdms();
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr2).RunInPdms();
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr3).RunInPdms();

                // ELEV
                cmdstr = String.Format(@"!dir = object array()");
                cmdstr2 = String.Format(@"!dir.append(0) !dir.append(1) !dir.append(0) ");
                cmdstr3 = String.Format(@" !!gph3ddesign3.view.direction= !dir ");
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr).RunInPdms();
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr2).RunInPdms();
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr3).RunInPdms();

                // SECT
                cmdstr = String.Format(@"!dir = object array()");
                cmdstr2 = String.Format(@"!dir.append(1) !dir.append(0) !dir.append(0) ");
                cmdstr3 = String.Format(@" !!gph3ddesign1.view.direction= !dir ");
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr).RunInPdms();
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr2).RunInPdms();
                Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(cmdstr3).RunInPdms();

                SendMessage(WindowManager.Instance.MainForm.Handle, WM_SETREDRAW, true, 0);
                WindowManager.Instance.MainForm.LayoutMdi(MdiLayout.TileHorizontal);
                for (int i = 0; i < 4; i++)
                {

                    windowlist[i].Form.Focus();
                    string cmd = "AVEVA.View.WalkTo.DrawList";

                    cmdmanager.ExecuteCommand(cmd);

                }
                //WindowManager.Instance.MainForm.Update();
                //모든뷰 Autoscaling


                //Form[] mdiforms = WindowManager.Instance.MainForm.MdiChildren;
                //foreach (MdiWindow form in windowlist)
                //{

                //    SendMessage(form.Form.Handle, WM_SETREDRAW, true, 0);

                //}


                //Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("!!walktodrawlistAllview()").RunInPdms();
                ////4개의 View에 WalkToDrawlist실행


            }
            catch (Exception ee)
            {

            }
        }
    }
}
