using AdoNetWindow.Model;
using Repositories;
using Repositories.Calender;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.CalendarModule.VacationManager
{
    public partial class VacationAgreementManager : Form
    {
        IUsersRepository usersRepository = new UsersRepository();
        IAnnualRepository annualRepository = new AnnualRepository();
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um;
        public VacationAgreementManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void VacationAgreementManager_Load(object sender, EventArgs e)
        {
            DateTime pre_sttdate = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
            DateTime pre_enddate = new DateTime(DateTime.Now.AddYears(-1).Year, 12, 31);

            DateTime user_in_date;
            if (DateTime.TryParse(um.user_in_date, out user_in_date))
            {
                if (pre_sttdate.Year == user_in_date.Year && pre_sttdate < user_in_date)
                    pre_sttdate = user_in_date;
            }

            lbAccruedDate.Text = pre_sttdate.ToString("yyyy.MM.dd.") + " ~ " + pre_enddate.ToString("yyyy.MM.dd.");

            DateTime sttdate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime enddate = new DateTime(DateTime.Now.Year, 12, 31);

            lbUsedDate.Text = sttdate.ToString("yyyy.MM.dd.") + " ~ " + enddate.ToString("yyyy.MM.dd.");

            GetAccuredVacation();
        }

        #region Method
        private void GetAccuredVacation()
        {
            int year = DateTime.Now.Year;
            DataTable userDt = usersRepository.GetUsersVacation(DateTime.Now.Year, um.user_id);

            if (userDt.Rows.Count > 0)
            {
                double accrued_annual;
                if (!double.TryParse(userDt.Rows[0]["vacation"].ToString(), out accrued_annual))
                    accrued_annual = 0;
                double used_days;
                if (!double.TryParse(userDt.Rows[0]["used_days"].ToString(), out used_days))
                    used_days = 0;
                DateTime user_in_date;
                if (!DateTime.TryParse(userDt.Rows[0]["user_in_date"].ToString(), out user_in_date))
                    user_in_date = new DateTime(1900 ,1, 1);

                //등록값이 없으면 계산
                if (accrued_annual == 0)
                {
                    TimeSpan ts = new DateTime(year - 1, 12, 31) - user_in_date;
                    int workDays = ts.Days + 1;
                    if (user_in_date.Year == year && workDays / 365 < 1)
                    {
                        while (DateTime.Now >= user_in_date)
                        {
                            accrued_annual++;
                            user_in_date = user_in_date.AddMonths(1);
                        }
                    }
                    else if (user_in_date.Year < year && workDays / 365 < 1)
                        accrued_annual = 15;
                    else
                        accrued_annual = Math.Round((double)(int)((double)workDays / 365) / 2, 0, MidpointRounding.AwayFromZero) + 14;
                }
                lbAccuredVacation.Text = accrued_annual.ToString();
                lbNoticeDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                lbUsedVacation.Text = (Math.Round(used_days * 10) / 10).ToString();
                lbLeavedVacation.Text = (Math.Round((accrued_annual - used_days) * 10) / 10).ToString();
                lbUntilDate.Text = (new DateTime(DateTime.Now.Year, 12, 31)).ToString("yyyy년 MM월 dd일까지");
            }
            else
                MessageBox.Show(this,"유저정보를 찾을수 없습니다.");
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e) 
        {
            if (cbAgreement.Checked)
            {
                VacationAgreementModel model = new VacationAgreementModel();
                model.user_id = um.user_id;
                model.year = DateTime.Now.Year;
                model.month = DateTime.Now.Month;
                model.is_agreement = true;
                model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.edit_user = um.user_name;

                StringBuilder sql = annualRepository.InsertAgreementAnnual(model);
                List<StringBuilder> sqlList = new List<StringBuilder>();
                sqlList.Add(sql);

                if (commonRepository.UpdateTran(sqlList) == -1)
                    MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
            }
            this.Dispose();
        }
        #endregion

        #region Key event
        private void VacationAgreementManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        cbAgreement.Checked = !cbAgreement.Checked;
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
        #endregion
    }
}
