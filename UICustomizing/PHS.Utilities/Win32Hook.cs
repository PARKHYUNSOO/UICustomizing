using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace HookedAp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public ulong time;
        public Point pt;
    }

    public enum HookId
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK,
        WH_KEYBOARD,
        WH_GETMESSAGE,
        WH_CALLWNDPROC,
        WH_CBT,
        WH_SYSMSGFILTER,
        WH_MOUSE,
        WH_HARDWARE,
        WH_DEBUG,
        WH_SHELL,
        WH_FOREGROUNDIDLE,
        WH_CALLWNDPROCRET,
        WH_KEYBOARD_LL,
        WH_MOUSE_LL
    }

    public enum QueueStatus
    {
        PM_NOREMOVE = 0,
        PM_REMOVE = 1
    }

    class HookEventArgs : EventArgs
    {
        private MSG msg;
        private int code;
        private int wparam;

        public MSG Message 
        {   get { return msg; }
            set { msg = value; }
        }

        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public int wParam
        {
            get { return wparam; }
            set { wparam = value; }
        }
    }

    class Win32Hook
    {
        public bool InstallHook()
        {
            Win32Hook.HProc hookProc = new Win32Hook.HProc(HookProcedure);
            m_hHook = Win32Hook.SetWindowsHookEx(HookId.WH_GETMESSAGE, hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());

            return m_hHook != IntPtr.Zero;
        }

        public bool UninstallHook()
        {
            return UnhookWindowsHookEx(m_hHook) != 0;
        }

        private int HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(m_hHook, nCode, wParam, lParam);
            MSG msg = (MSG)Marshal.PtrToStructure(lParam, typeof(MSG));

            HookEventArgs hea = new HookEventArgs();
            hea.Code = nCode;
            hea.Message = msg;
            hea.wParam = (int)wParam;

            if (HookInvoked != null)
            {
                HookInvoked(this, hea);
            }

            return CallNextHookEx(m_hHook, nCode, wParam, lParam); ;
        }

        public delegate void HookEventHandler(object sender, HookEventArgs hea);
        public event HookEventHandler HookInvoked;


        #region P/Invoke

        private delegate int HProc(int nCode, IntPtr wParam, IntPtr lParam);
        private event HProc HookProc;

        [DllImport("user32.dll")]
        private static  extern IntPtr SetWindowsHookEx(HookId hookId, HProc hookProc, IntPtr hInstance, Int32 threadId);

        [DllImport("user32.dll")]
        private static extern Int32 UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll")]
        private static extern Int32 CallNextHookEx(IntPtr hHook, Int32 nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr m_hHook = IntPtr.Zero;

        #endregion
    }
}
