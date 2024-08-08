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
    public partial class GetDateToDate : Form
    {
        private string stt_date = "", end_date = "";
        public GetDateToDate()
        {
            InitializeComponent();
        }

        private void GetDateToDate_Load(object sender, EventArgs e)
        {

        }

        public void GetTerm(out string sttdate, out string enddate)
        {
            this.ShowDialog();

            DateTime sdate, edate;
            if (!DateTime.TryParse(stt_date, out sdate) || !DateTime.TryParse(end_date, out edate))
            {
                sttdate = DateTime.Now.ToString("yyyy-MM-dd");
                enddate = "";
            }
            else
            {
                sttdate = sdate.ToString("yyyy-MM-dd");
                enddate = edate.ToString("yyyy-MM-dd");
            }
        }

        private void GetDateToDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnSelect.PerformClick();
                        break;
                    case Keys.X:
                        txtSttdate.Text = String.Empty;
                        txtEnddate.Text = String.Empty;
                        this.Dispose();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        txtSttdate.Text = String.Empty;
                        txtEnddate.Text = String.Empty;
                        this.Dispose();
                        break;
                }
            }
        }

        private void btnSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DateTime sdate, edate;
            if (!DateTime.TryParse(txtSttdate.Text, out sdate) || !DateTime.TryParse(txtEnddate.Text, out edate))
            {
                MessageBox.Show(this, "선택한 기간의 값이 날짜형식이 아닙니다.");
                this.Activate();
            }
            else
            {
                stt_date = sdate.ToString("yyyy-MM-dd");
                end_date = edate.ToString("yyyy-MM-dd");
                this.Dispose();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            txtSttdate.Text = String.Empty;
            txtEnddate.Text = String.Empty;
            this.Dispose();
        }
    }
}
