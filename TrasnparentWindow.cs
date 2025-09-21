using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MousePosition
{
    public partial class TrasnparentWindow : Form
    {
        private Form1 _mainWindow;
        public TrasnparentWindow(Form1 mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Opacity = 0.01;
            this.ShowInTaskbar = true;
            this.ContextMenuStrip = null;

            this.MouseClick += Control_MouseClick;

        }
        private void Control_MouseClick(Object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                Console.WriteLine("h");
            }
            if(e.Button == MouseButtons.Left)
            {
                Console.WriteLine("l");
            }
        }
    }
}
