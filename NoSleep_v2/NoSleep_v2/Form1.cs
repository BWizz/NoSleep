using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Runtime.InteropServices;

namespace NoSleep_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            CreateTimer();
            StartTimer();
            radioButton1.Checked = true;
            textBox1.Text = DateTime.Now.ToString("h:mm:ss tt");
            textBox1.SelectionLength = 0;
            textBox1.SelectionStart = this.textBox1.TextLength;
        }

        private static System.Windows.Forms.Timer aTimer;
        private static bool isClicked = true;
        [DllImport("kernel32.dll")]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
  


        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINIOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001,
        }

        private void Mouse_Click_Fun(object sender, EventArgs e)
        {
            if (isClicked == false)
            {
                isClicked = true;
                radioButton1.Checked = true;
                StartTimer();
                UpdateText(DateTime.Now.ToString("h:mm:ss tt"));
            }
            else
            {
                isClicked = false;
                radioButton1.Checked = false;
                StopTimer();
                UpdateText("NoSleep Disabled!");
            }
        }

 
        private void OnTimedEvent(Object source, EventArgs e)
        {
            //Reset OS Sleep Timers
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINIOUS);

            //Update Text UI Text Box
            UpdateText(DateTime.Now.ToString("h:mm:ss tt"));

        }

        private void UpdateText(string txt)
        {
            textBox1.Text = txt;
            textBox1.SelectionLength = 0;
            textBox1.SelectionStart = this.textBox1.TextLength;
        }

        private void CreateTimer()
        {
            // Create a timer with a 60 second interval.
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Interval = 60000;
            aTimer.Enabled = false;
            // Hook up the Elapsed event for the timer. 
            aTimer.Tick += new EventHandler(OnTimedEvent);
        }
        private static void StartTimer()
        {
            aTimer.Enabled = true;
        }
        private static void StopTimer()
        {
            aTimer.Enabled = false;
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            bool MousePointerNotOnTaskBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);
            if (FormWindowState.Minimized == this.WindowState && MousePointerNotOnTaskBar)
            {

                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipText = "NoSleep minimized to system tray";
                notifyIcon1.Icon = new Icon("appicon.ico");
                notifyIcon1.ShowBalloonTip(500);
                this.ShowInTaskbar = false;
            }

        }
        private void notifyIcon1_clicked(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void About_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.Show();
        }
    }
}
