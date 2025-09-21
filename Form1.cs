using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace MousePosition
{
    public partial class Form1 : Form
    {

        private IKeyboardMouseEvents m_GlobalHook;
        private TrasnparentWindow overlay;
        private bool isActivate = false;
        private NotifyIcon trayIcon;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Posição do Rato";

            label1.Text = "";
            label2.Text = "";

            timer1.Interval = 1;
            timer1.Tick += Timer1_Tick;
            timer1.Start();

            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.MouseClick += GlobalHook_MouseClick;
            m_GlobalHook.KeyDown += GlobalHook_KeyDown;

            this.ContextMenu = null;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.Hide();

            trayIcon = new NotifyIcon();
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;
            trayIcon.Text = "Press F10 to activate";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            trayIcon.ContextMenuStrip = contextMenu;
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (isActivate)
            {
                Point pos = Cursor.Position;
                label1.Text = $"X: {pos.X} | Y: {pos.Y}";
                using (Bitmap bmp = new Bitmap(1, 1))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(pos, new Point(0, 0), new Size(1, 1));
                    }
                    Color color = bmp.GetPixel(0, 0);
                    label2.Text = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                    pictureBox1.BackColor = color;
                    this.Location = new Point(pos.X + 10, pos.Y);
                }
            }
        }


        private void GlobalHook_MouseClick(object sender, MouseEventArgs e)
        {
            if (isActivate)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (label1.Text != "")
                    {
                        Console.WriteLine("Left Click Detected.");

                        Clipboard.SetDataObject(label1.Text);
                        overlay?.Close();
                        this.Hide();
                        isActivate = false;
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if (label2.Text != "")
                    {
                        Console.WriteLine("Right Click Detected.");
                        Clipboard.SetDataObject(label2.Text);
                        overlay?.Close();
                        this.Hide();
                        isActivate = false;
                    }
                }
            }
        }

        private void GlobalHook_KeyDown(Object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F10)
            {
                if(isActivate == false)
                {
                    overlay = new TrasnparentWindow(this);
                    overlay.Show();
                    this.Show();
                    this.TopMost = true;
                    isActivate = true;
                }
            }
            if(e.KeyCode == Keys.Escape)
            {
                if(isActivate == true)
                {
                    overlay?.Close();
                    this.Hide();
                    isActivate = false;
                }
            }
                
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if(m_GlobalHook != null)
            {
                m_GlobalHook.MouseClick -= GlobalHook_MouseClick;
                m_GlobalHook.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}
