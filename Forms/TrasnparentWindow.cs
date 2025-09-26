using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MousePosition
{
    public partial class TrasnparentWindow : Form
    {

        const int WS_EX_LAYERED = 0x80000;
        const int WS_EX_NOACTIVATE = 0x08000000;
        const int GWL_EXSTYLE = -20;
        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")] 
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")] 
        static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        public TrasnparentWindow(Screen target)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = target.Bounds;
            this.BackColor = Color.White;
            this.TopMost = true;
            this.ShowInTaskbar = true;
            this.ContextMenuStrip = null;

            this.Load += (s, e) =>
            {
                int exStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
                exStyle |= WS_EX_LAYERED;
                exStyle |= WS_EX_NOACTIVATE;
                SetWindowLong(this.Handle, GWL_EXSTYLE, exStyle);

                // Set the window to be layered with alpha blending and low opacity (1 out of 255)
                SetLayeredWindowAttributes(this.Handle, 0, 1, 0x2);
            };
        }
    }
}
