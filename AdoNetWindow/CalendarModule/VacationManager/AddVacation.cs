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
    public partial class AddVacation : Form
    {
        IAnnualRepository annualRepository = new AnnualRepository();
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um;
        public AddVacation(UsersModel um)
        {
            InitializeComponent();
            this.um = um;

            Refresh();
        }
        public AddVacation(UsersModel um, int annual_id)
        {
            InitializeComponent();
            this.um = um;

            lbAnnualId.Text = annual_id.ToString();
            /*DataTable annualDt = annualRepository.GetAnnualAsOne(annual_id);
            if (annualDt.Rows.Count > 0)
            {
                cbDivision.Text = annualDt.Rows[0]["division"].ToString();
                txtUserName.Text = annualDt.Rows[0]["user_name"].ToString();
                txtDepartment.Text = annualDt.Rows[0]["department"].ToString();
                txtGrade.Text = annualDt.Rows[0]["grade"].ToString();

                DateTime createtime;
                if(DateTime.TryParse(annualDt.Rows[0]["createtime"].ToString(), out createtime))
                    txtCreatetime.Text = createtime.ToString("yyyy-MM-dd");

                DateTime sttdate;
                if (DateTime.TryParse(annualDt.Rows[0]["sttdate"].ToString(), out sttdate))
                    txtSttdate.Text = sttdate.ToString("yyyy-MM-dd");

                DateTime enddate;
                if (DateTime.TryParse(annualDt.Rows[0]["enddate"].ToString(), out enddate))
                    txtEnddate.Text = enddate.ToString("yyyy-MM-dd");

                txtUsedDays.Text = annualDt.Rows[0]["used_days"].ToString();
                txtRemark.Text = annualDt.Rows[0]["remark"].ToString();
                cbStatus.Text = annualDt.Rows[0]["status"].ToString();

                bool approval_manager;
                if (!bool.TryParse(annualDt.Rows[0]["approval_manager"].ToString(), out approval_manager))
                    approval_manager = false;
                cbApprovalManager.Checked = approval_manager;

                if (approval_manager)
                    btnApprovalManager.BackgroundImage = Properties.Resources.Check_icon;
                else
                    btnApprovalManager.BackgroundImage = null;

                bool approval_deputy_general_manager;
                if (!bool.TryParse(annualDt.Rows[0]["approval_deputy_general_manager"].ToString(), out approval_deputy_general_manager))
                    approval_deputy_general_manager = false;
                cbApprovalDeputyGeneralManager.Checked = approval_deputy_general_manager;

                if (approval_deputy_general_manager)
                    btnApprovalDeputyGeneralManager.BackgroundImage = Properties.Resources.Check_icon;
                else
                    btnApprovalDeputyGeneralManager.BackgroundImage = null;


                bool approval_general_manager;
                if (!bool.TryParse(annualDt.Rows[0]["approval_general_manager"].ToString(), out approval_general_manager))
                    approval_general_manager = false;
                cbApprovalGeneralManager.Checked = approval_general_manager;

                if (approval_general_manager)
                    btnApprovalGeneralManager.BackgroundImage = Properties.Resources.Check_icon;
                else
                    btnApprovalGeneralManager.BackgroundImage = null;
            }
            else
                MessageBox.Show(this,"저장내역을 찾을 수 없습니다.");*/
        }
        private void AddVacation_Load(object sender, EventArgs e)
        {
            if (um.auth_level >= 95)
            {
                /*cbApprovalManager.Visible = true;
                cbApprovalDeputyGeneralManager.Visible = true;
                cbApprovalGeneralManager.Visible = true;*/
                lbStatus.Visible = true;
                cbStatus.Visible = true;
            }
        }

        #region Method
        private void Refresh()
        {
            //초기화
            txtUserName.Text = um.user_name;
            lbAnnualId.Text = commonRepository.GetNextId("t_annual", "id").ToString();

            string department;
            if (um.department.Contains("영업"))
                department = "영업부";
            else
                department = um.department;

            txtDepartment.Text = department;
            txtGrade.Text = um.grade;

            txtCreatetime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtSttdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            txtRemark.Text = String.Empty;
            txtAgent.Text = String.Empty;

            cbApprovalManager.Checked = false;
            btnApprovalManager.BackgroundImage = null;
            cbApprovalGeneralManager.Checked = false;
            btnApprovalGeneralManager.BackgroundImage = null;
            cbApprovalDeputyGeneralManager.Checked = false;
            btnApprovalDeputyGeneralManager.BackgroundImage = null;
        }
        #endregion

        #region 캘린더 날짜 선택
        private void txtSttdate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                TextBox tb = (TextBox)sender;
                tb.Text = sdate;
            }
        }
        #endregion

        #region Text, index Changed
        private void cbDivision_TextChanged(object sender, EventArgs e)
        {
            lbContents.Text = $"위와 같이 {cbDivision.Text}를 사용하고 합니다.";
            DateTime sttdate, enddate;
            if (DateTime.TryParse(txtSttdate.Text, out sttdate) && DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                if (cbDivision.Text == "반차")
                    txtUsedDays.Text = "0.5";
                else
                {
                    TimeSpan ts = enddate - sttdate;
                    int useDays = ts.Days + 1;
                    txtUsedDays.Text = useDays.ToString("#,##0");
                }
            }
        }

        private void txtSttdate_TextChanged(object sender, EventArgs e)
        {
            DateTime sttdate, enddate;
            if (DateTime.TryParse(txtSttdate.Text, out sttdate) && DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                if (cbDivision.Text == "반차")
                    txtUsedDays.Text = "0.5";
                else
                { 
                    TimeSpan ts = enddate - sttdate;
                    int useDays = ts.Days + 1;
                    txtUsedDays.Text = useDays.ToString("#,##0");
                }
            }
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //유효성검사
            int annual_id;
            if (!int.TryParse(lbAnnualId.Text, out annual_id))
            {
                MessageBox.Show(this, "저장된 내역을 찾을 수 없습니다.");
                return;
            }
            /*DataTable annualDt = annualRepository.GetAnnualAsOne(annual_id);
            if (annualDt.Rows.Count == 0)
            {
                MessageBox.Show(this,"저장된 내역을 찾을 수 없습니다.");
                return;
            }
            //MSG
            if (MessageBox.Show(this, "삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;*/
            //Execute
            /*StringBuilder sql = annualRepository.DeleteAnnual(annual_id);
            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);

            if (commonRepository.UpdateTran(sqlList) == -1)
                MessageBox.Show(this,"삭제중 에러가 발생하였습니다.");
            else
            {
                MessageBox.Show(this,"삭제완료");
                Refresh();
            }*/
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            //유효성검사
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                MessageBox.Show(this, "성명을 입력해주세요.");
                return;
            }
            if (string.IsNullOrEmpty(txtDepartment.Text.Trim()))
            {
                MessageBox.Show(this, "부서를 입력해주세요.");
                return;
            }
            if (string.IsNullOrEmpty(cbDivision.Text.Trim()))
            {
                MessageBox.Show(this, "연차구분을 입력해주세요.");
                return;
            }
            DateTime createtime;
            if (!DateTime.TryParse(txtCreatetime.Text, out createtime))
            {
                MessageBox.Show(this, "작성일 값이 날짜 형식이 아닙니다.");
                return;
            }
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate) || !DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "사용기간 값이 날짜 형식이 아닙니다.");
                return;
            }

            //MSG
            if (MessageBox.Show(this, "저장하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            int annual_id;
            if (!int.TryParse(lbAnnualId.Text, out annual_id))
                annual_id = commonRepository.GetNextId("t_annual", "id");

            float used_days;
            if (!float.TryParse(txtUsedDays.Text, out used_days))
                used_days = 0;

            /*AnnualModel model = new AnnualModel();
            model.id = annual_id;
            model.division = cbDivision.Text;
            model.user_id = um.user_id;
            model.user_name = txtUserName.Text;
            model.department = txtDepartment.Text;
            model.grade = txtGrade.Text;
            model.createtime = createtime.ToString("yyyy-MM-dd");
            model.approval_manager = cbApprovalManager.Checked;
            model.approval_deputy_general_manager = cbApprovalDeputyGeneralManager.Checked;
            model.approval_general_manager = cbApprovalGeneralManager.Checked;

            model.remark = txtRemark.Text;
            model.sttdate = sttdate.ToString("yyyy-MM-dd");
            model.enddate = enddate.ToString("yyyy-MM-dd");
            model.used_days = used_days;
            model.agent = txtUsedDays.Text;

            model.edit_user = um.user_name;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.status = cbStatus.Text;

            
            StringBuilder sql;
            DataTable annualDt = annualRepository.GetAnnualAsOne(model.id);
            if (annualDt.Rows.Count == 0)
                sql = annualRepository.InsertAnnual(model);
            else
                sql = annualRepository.UpdateAnnual(model);

            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);

            if (commonRepository.UpdateTran(sqlList) == -1)
                MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
            else
                MessageBox.Show(this,"등록완료");*/
        }

        #endregion

        #region 결재버튼
        private void btnApprovalManager_Click(object sender, EventArgs e)
        {
            if (um.auth_level >= 95)
                cbApprovalManager.Checked = !cbApprovalManager.Checked;
            else
                MessageBox.Show(this, "수정권한이 없습니다.");
        }

        private void btnApprovalDeputyGeneralManager_Click(object sender, EventArgs e)
        {
            if (um.auth_level >= 95)
                cbApprovalDeputyGeneralManager.Checked = !cbApprovalDeputyGeneralManager.Checked;
            else
                MessageBox.Show(this, "수정권한이 없습니다.");
        }

        private void btnApprovalGeneralManager_Click(object sender, EventArgs e)
        {
            if (um.auth_level >= 95)
                cbApprovalGeneralManager.Checked = !cbApprovalGeneralManager.Checked;
            else
                MessageBox.Show(this, "수정권한이 없습니다.");
        }

        private void cbApprovalManager_CheckedChanged(object sender, EventArgs e)
        {
            if (cbApprovalManager.Checked)
                btnApprovalManager.BackgroundImage = Properties.Resources.Check_icon;
            else
                btnApprovalManager.BackgroundImage = null;
        }

        private void cbApprovalDeputyGeneralManager_CheckedChanged(object sender, EventArgs e)
        {
            if (cbApprovalDeputyGeneralManager.Checked)
                btnApprovalDeputyGeneralManager.BackgroundImage = Properties.Resources.Check_icon;
            else
                btnApprovalDeputyGeneralManager.BackgroundImage = null;
        }

        private void cbApprovalGeneralManager_CheckedChanged(object sender, EventArgs e)
        {
            if (cbApprovalGeneralManager.Checked)
                btnApprovalGeneralManager.BackgroundImage = Properties.Resources.Check_icon;
            else
                btnApprovalGeneralManager.BackgroundImage = null;
        }

        #endregion

        #region Key event
        private void AddVacation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnRegister.PerformClick();
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
