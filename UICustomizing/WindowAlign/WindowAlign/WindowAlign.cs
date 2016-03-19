using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aveva.PDMS.PMLNet;
using Aveva.ApplicationFramework;
using System.Windows.Forms;
using Aveva.ApplicationFramework.Presentation;
using Aveva.Pdms.Viewer3D.Addin.Windows;
using Aveva.Pdms.Viewer3D.Addin;
namespace WindowAlign
{
    [PMLNetCallable()]
    public class WindowAlign
    {
        [PMLNetCallable()]
        public WindowAlign()
        {
        }
        [PMLNetCallable()]
        public void Assign(WindowAlign that)
        {
        }

        public Control mDesignCanvas;
        [PMLNetCallable]
        public void AlignHorizontal()
        {
            //System.Diagnostics.Debugger.Launch();
            var windowState = WindowManager.Instance.MainForm.WindowState;
            //WindowManager.Instance.MainForm.WindowState = FormWindowState.Minimized;
            WindowManager.Instance.MainForm.LayoutMdi(MdiLayout.TileHorizontal);
            mDesignCanvas = GetCanvasControl("!!GPH3DDESIGN1");
            //WindowsManager.Instance.Active.Window.Enabled = false;
            
            Console.WriteLine("1");
            //restore the old size...
            //WindowManager.Instance.MainForm.WindowState = windowState;
        }
        /// <summary>
        /// Just for test..ㅋㅋ
        /// </summary>
        /// 
        private Control GetCanvasControl(string key)
        {
            return
                WindowManager.Instance.Windows.OfType<MdiWindow>()
                    .Where(result => result.Key == key)
                    .SelectMany(
                        result =>
                            result.Control.Controls.OfType<Panel>()
                                .Select(control => control as Control)
                                .Select(control1 => control1.Controls["UI_DruidCanvas"]))
                    .FirstOrDefault();
        }
        [PMLNetCallable]
        public void ForceGC()
        {
            var windowState = WindowManager.Instance.MainForm.WindowState;
            WindowManager.Instance.MainForm.WindowState = FormWindowState.Minimized;

            //가비지 collecting의 경우 Applicaton의 performance를 저하 시킬 수 있습니다.
            //GC를 할경우 순간 AVEVA Marine에 작업memory가 올라 가는 것을 볼 수있습니다.
            //아무래도 이 방법은 좋아 보이질 않네요.
            //GC없이 Minimize하는 것이 좋을 것 같습니다.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            WindowManager.Instance.MainForm.WindowState = windowState;
        }

        [PMLNetCallable]
        public void ModelEditorOnOff()
        {
            Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("!!ModelEditorOnOff()").RunInPdms();
        }

    }
}
