using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace AdoNetWindow.Common
{
    public partial class ProcessBar : Form
    {
        int maxIndex;
        int currentIndex;
        public ProcessBar(int maxIndex)
        {
            InitializeComponent();

            lbCurrent.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            lbTotal.BackColor = Color.Transparent;

            lbCurrent.Width = 0;

            this.maxIndex = maxIndex;
            lbTotal.Text = maxIndex.ToString();
            lbTotal.Update();
            currentIndex = 1;
        }

        private Timer aTimer;
        private readonly double cycleTime = 1500; //1.5초
        public ProcessBar()
        {
            InitializeComponent();
            lbCurrent.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            lbTotal.BackColor = Color.Transparent;

            lbTotal.Visible = false;
            lbCurrent.Text = "Loding...";

            maxIndex = 100;
            currentIndex = 0;

            this.Update();
            //타이머
            SetTimer();
            aTimer.Start();
        }
        #region 로딩 루프
        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(cycleTime);

            // 이벤트 핸들러 연결
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Timer에서 Elapsed 이벤트를 반복해서 발생
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            currentIndex++;
            int width = (int)((lbTotalRate.Width / maxIndex) * currentIndex);
            if (width > 300)
            {
                maxIndex = 10;
                currentIndex = 0;
            }
                
            lbCurrentRate.Width = width;
            this.Update();
        }
        #endregion

        public void AddProcessing()
        {
            currentIndex++;
            lbCurrent.Text = currentIndex.ToString();
            lbCurrent.Update();
            lbCurrentRate.Width = lbTotalRate.Width / maxIndex * currentIndex;
            this.Update();
            if (currentIndex == maxIndex)
                this.Dispose();
        }
        public void Close()
        {
            this.Dispose();
        }
    }
}
