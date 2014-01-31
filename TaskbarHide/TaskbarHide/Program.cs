using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TaskbarHide
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, IntPtr lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;

        [StructLayout(LayoutKind.Sequential)]
        struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hwnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public Rectangle rc;
            public int lParam;
        };

        private const int ABM_SETSTATE = 10;
        private const int ABS_AUTOHIDE = 1;
        private const int ABS_ALWAYSONTOP = 2;

        [DllImport("shell32.dll")]
        static extern int SHAppBarMessage(int msg, ref APPBARDATA pbd);
        static void Main(string[] args)
        {
            hide();
            //show();
        }
        static void hide(){
            // 「タスクバーを自動的に隠す」
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.lParam = ABS_AUTOHIDE;
            SHAppBarMessage(ABM_SETSTATE, ref abd);

            System.Threading.Thread.Sleep(100);

            // タスクバーを非表示
            ShowWindow(FindWindow("Shell_TrayWnd", IntPtr.Zero), SW_HIDE);
        
        }
        static void show()
        {
            // タスクバーを常に表示
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.lParam = ABS_ALWAYSONTOP;
            SHAppBarMessage(ABM_SETSTATE, ref abd);

            // タスクバーを表示
            ShowWindow(FindWindow("Shell_TrayWnd", IntPtr.Zero), SW_NORMAL);

        }
    }
}
