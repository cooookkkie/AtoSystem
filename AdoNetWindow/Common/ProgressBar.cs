using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Common
{
    public partial class ProgressBar : Form
    {
        private Timer timer;
        private int timerCount = 0;
        public ProgressBar()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
        }

        public void SetStyle(int styleType)
        {
            switch (styleType)
            {
                case 3:
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    progressBar1.Enabled = true;
                    break;
            }
        }
        public void Close()
        {
            this.Dispose();
        }
        public void Start()
        {
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs a)
        { 
            //progressBar1.PerformStep();

            // 타이머 중지 조건
            if (++timerCount == 10)
            {
                timer.Stop();
                progressBar1.Enabled = false;
            }
        }
    }
}
