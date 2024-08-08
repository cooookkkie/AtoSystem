using AdoNetWindow.Common.MultiDatesCalendar;
using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement.AddSaleCompany;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    public partial class AlarmSettingManager : Form
    {
        UsersModel um;
        SalesManager sm;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public AlarmSettingManager(UsersModel um, SalesManager sm)
        {
            InitializeComponent();
            this.sm = sm;
            this.um = um;
        }
        private void AlarmSettingManager_Load(object sender, EventArgs e)
        {
            for (int i = 2016; i <= DateTime.Now.Year + 2; i++)
                cbYear.Items.Add(i.ToString());

            cbYear.Text = DateTime.Now.Year.ToString();
            cbMonth.Text = DateTime.Now.Month.ToString();

            int year;
            if (!int.TryParse(cbYear.Text, out year))
                year = DateTime.Now.Year;

            int month;
            if (!int.TryParse(cbMonth.Text, out month))
                month = DateTime.Now.Month;
            GetCalendar(year, month);
        }

        #region Button, Checkbox
        private void cbAlarmMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbAlarmMonth.Text.Trim()))
            {

                foreach (DataGridViewRow row in dgvAlarm.Rows)
                {
                    if (row.Cells["alarm_category"].Value.ToString().Equals("월알람") && row.Cells["alarm_date"].Value.ToString().Equals(cbAlarmMonth.Text.Trim()))
                    {
                        messageBox.Show(this, "이미 선택된 날짜입니다.");
                        return;
                    }
                }

                int n = dgvAlarm.Rows.Add();
                DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                cCell.Items.Add("발주");
                cCell.Items.Add("결제");
                cCell.Items.Add("신규");
                cCell.Items.Add("기타");
                dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                dgvAlarm.Rows[n].Cells["alarm_division"].Value = cbAlarmDivision.Text;
                dgvAlarm.Rows[n].Cells["alarm_category"].Value = "월알람";
                dgvAlarm.Rows[n].Cells["alarm_date"].Value = cbAlarmMonth.Text.Trim();
                dgvAlarm.Rows[n].Cells["edit_user"].Value = um.user_name;
                dgvAlarm.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {        
            if (sm != null)
            { 
                sm.SettingAlarm(0, "", dgvAlarm);
                this.Dispose();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvAlarm.Rows.Clear();
            cbMonday.Checked = false;
            cbTuseday.Checked = false;
            cbWednesday.Checked = false;
            cbThursday.Checked = false;
            cbFriday.Checked = false;
        }

        private void btnAlarmRefresh_Click(object sender, EventArgs e)
        {
            MultiDatesCalendar cd = new MultiDatesCalendar();
            cd.Owner = this;
            string[] selectDates = cd.GetSelectDate();
            if (selectDates != null)
            {
                foreach (string date in selectDates)
                {
                    if (DateTime.TryParse(date, out DateTime selectDt))
                    {
                        bool isExist = false;
                        foreach (DataGridViewRow row in dgvAlarm.Rows)
                        {
                            if (row.Cells["alarm_date"].Value != null && DateTime.TryParse(row.Cells["alarm_date"].Value.ToString(), out DateTime alarm_date) && selectDt == alarm_date)
                            {
                                isExist = true;
                                break;
                            }
                        }

                        //등록되지 않았을 경우만
                        if (!isExist)
                        {
                            int n = dgvAlarm.Rows.Add();
                            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("발주");
                            cCell.Items.Add("결제");
                            cCell.Items.Add("신규");
                            cCell.Items.Add("기타");
                            dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                            dgvAlarm.Rows[n].Cells["alarm_division"].Value = cbAlarmDivision.Text;
                            dgvAlarm.Rows[n].Cells["alarm_date"].Value = selectDt.ToString("yyyy-MM-dd");
                            dgvAlarm.Rows[n].Cells["edit_user"].Value = um.user_name;
                            dgvAlarm.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
            }
        }
        #endregion

        #region Key event
        private void AlarmSettingManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { }
            else if (e.Modifiers == Keys.Control)
            { }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                    case Keys.Enter:
                        btnRegister.PerformClick();
                        break;
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Method
        private bool ExistAlarm(string category, string date)
        {
            foreach (DataGridViewRow row in dgvAlarm.Rows)
            {
                if (row.Cells["alarm_category"].Value.ToString() == category
                    && row.Cells["alarm_date"].Value.ToString() == date)
                    return true;
            }
            return false;
        }
        #endregion


        #region Add Alarm

        public void AddAlarmEvent(DateTime dt)
        {
            if (!ExistAlarm("특정일자", dt.ToString("yyyy-MM-dd")))
            {
                int n = dgvAlarm.Rows.Add();
                DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                cCell.Items.Add("발주");
                cCell.Items.Add("결제");
                cCell.Items.Add("신규");
                cCell.Items.Add("기타");
                dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                dgvAlarm.Rows[n].Cells["alarm_division"].Value = cbAlarmDivision.Text;
                dgvAlarm.Rows[n].Cells["alarm_category"].Value = "특정일자";
                dgvAlarm.Rows[n].Cells["alarm_date"].Value = dt.ToString("yyyy-MM-dd");
                dgvAlarm.Rows[n].Cells["edit_user"].Value = um.user_name;
                dgvAlarm.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        private void cbMonday_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            string week = "";
            switch (cb.Name)
            {
                case "cbMonday":
                    week = "월";
                    break;
                case "cbTuseday":
                    week = "화";
                    break;
                case "cbWednesday":
                    week = "수";
                    break;
                case "cbThursday":
                    week = "목";
                    break;
                case "cbFriday":
                    week = "금";
                    break;
            }

            //주알람 추가
            if (cb.Checked)
            {
                if (!string.IsNullOrEmpty(week) && !ExistAlarm("주알람", week))
                {
                    int n = dgvAlarm.Rows.Add();
                    DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("발주");
                    cCell.Items.Add("결제");
                    cCell.Items.Add("신규");
                    cCell.Items.Add("기타");
                    dgvAlarm.Rows[n].Cells["alarm_division"] = cCell;
                    dgvAlarm.Rows[n].Cells["alarm_division"].Value = cbAlarmDivision.Text;
                    dgvAlarm.Rows[n].Cells["alarm_category"].Value = "주알람";
                    dgvAlarm.Rows[n].Cells["alarm_date"].Value = week;
                    dgvAlarm.Rows[n].Cells["edit_user"].Value = um.user_name;
                    dgvAlarm.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            //주알람 해제
            else
            {
                foreach (DataGridViewRow row in dgvAlarm.Rows)
                {
                    if (row.Cells["alarm_category"].Value.ToString().Equals("주알람") && row.Cells["alarm_date"].Value.ToString().Equals(week))
                    {
                        dgvAlarm.Rows.Remove(row);
                        return;
                    }
                }
            }
        }
        private void btnPreMonth_Click(object sender, EventArgs e)
        {
            int year;
            if (!int.TryParse(cbYear.Text, out year))
                year = DateTime.Now.Year;

            int month;
            if (!int.TryParse(cbMonth.Text, out month))
                month = DateTime.Now.Month;

            DateTime standardDt = new DateTime(year, month, 1).AddMonths(-1);

            cbYear.Text = standardDt.Year.ToString();
            cbMonth.Text = standardDt.Month.ToString();

            GetCalendar(standardDt.Year, standardDt.Month);
        }

        private void btnAfterMonth_Click(object sender, EventArgs e)
        {
            int year;
            if (!int.TryParse(cbYear.Text, out year))
                year = DateTime.Now.Year;

            int month;
            if (!int.TryParse(cbMonth.Text, out month))
                month = DateTime.Now.Month;

            DateTime standardDt = new DateTime(year, month, 1).AddMonths(1);

            cbYear.Text = standardDt.Year.ToString();
            cbMonth.Text = standardDt.Month.ToString();

            GetCalendar(standardDt.Year, standardDt.Month);
        }
        Libs.Tools.Common common = new Libs.Tools.Common();
        private void GetCalendar(int year, int month)
        {
            //Lets get the first day of the month
            DateTime startofthemonth = new DateTime(year, month, 1);

            //get the count of days of the month
            int days = DateTime.DaysInMonth(year, month);

            //convert the startofthemonth to integer
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            //Holiday List
            int[] no;
            string[] name;
            common.getRedDay(year, month, out no, out name);

            DateTime sttdate = new DateTime(year, month, 1, 0, 0, 0);
            DateTime sttdate2 = new DateTime(year, month, 1, 0, 0, 0).AddDays(-7);
            DateTime enddate = new DateTime(year, month, 1, 0, 0, 0).AddMonths(1).AddDays(-1);
            int dayofnextmonth = 0;
            //마지막주의 금요일까지
            while (!(enddate.DayOfWeek == DayOfWeek.Saturday || enddate.DayOfWeek == DayOfWeek.Sunday))
            {
                enddate = enddate.AddDays(1);
                dayofnextmonth++;      //다음달 오버되는 일짜
            }
            pnDays.Controls.Clear();
            //==========================================================================
            //first lets create a blank usercontrol
            for (int i = 1; i < dayoftheweek; i++)
            {
                UnitEmptyDays ucblack = new UnitEmptyDays();
                pnDays.Controls.Add(ucblack);
            }
            //==========================================================================
            //now lets create usercontrol for days
            for (int i = 1; i <= days; i++)
            {
                DateTime dt = new DateTime(year, month, i);
                UnitDays ucdays = new UnitDays(this, dt);
                pnDays.Controls.Add(ucdays);
            }
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            AddAlarmEvent(DateTime.Now);
        }

        private void btnTomorrow_Click(object sender, EventArgs e)
        {
            AddAlarmEvent(DateTime.Now.AddDays(1));
        }

        #endregion

    }
}
