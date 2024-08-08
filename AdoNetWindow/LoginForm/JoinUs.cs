using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Login
{
    public partial class JoinUs : Form
    {
        IDepartmentRepository departmentRepository = new DepartmentRepository();
        IUsersRepository usersRepository = new UsersRepository();
        public JoinUs()
        {
            InitializeComponent();
        }

        private void JoinUs_Load(object sender, EventArgs e)
        {
            DataTable departmentDt = departmentRepository.GetDepartment("", "");
            for (int i = 0; i < departmentDt.Rows.Count; i++)
            {
                cbDepartment.Items.Add(departmentDt.Rows[i]["name"].ToString());
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                UsersModel um = new UsersModel();
                um = usersRepository.GetUserInfo(txtId.Text);
                if (um != null)
                {
                    MessageBox.Show(this, "중복된 ID입니다.");
                    this.Activate();
                }
                else
                {
                    CreateAccount();
                    //시작프로그램 등록
                    Libs.Tools.Common common = new Libs.Tools.Common();
                    common.AddStartupProgram("AtoSystem", Application.ExecutablePath);
                }
            }
        }
        private bool Validation()
        {
            if (txtId.Text == "")
            {
                MessageBox.Show(this, "ID를 입력해주세요.");
                this.Activate();
                return false;
            }
            else if (txtPassword.Text == "")
            {
                MessageBox.Show(this, "비밀번호를 입력해주세요.");
                this.Activate();
                return false;
            }
            else if (txtPasswordComfirm.Text == "")
            {
                MessageBox.Show(this, "비밀번호 확인을 입력해주세요.");
                this.Activate();
                return false;
            }
            else if (txtTel.Text == "")
            {
                MessageBox.Show(this, "연락처를 입력해주세요.(품목서에 기재될 전화번호입니다.)");
                this.Activate();
                return false;
            }
            else if (txtPassword.Text != txtPasswordComfirm.Text)
            {
                MessageBox.Show(this, "비밀번호가 같지 않습니다.");
                this.Activate();
                return false;
            }
            else if (txtName.Text == "")
            {
                MessageBox.Show(this, "이름을 입력해주세요.");
                this.Activate();
                return false;
            }
            else if (string.IsNullOrEmpty(cbWorkplace.Text))
            {
                MessageBox.Show(this, "사업장을 선택해주세요.");
                this.Activate();
                return false;
            }
            else if (string.IsNullOrEmpty(cbDepartment.Text))
            {
                MessageBox.Show(this, "부서를 선택해주세요.");
                this.Activate();
                return false;
            }
            else if (txtSeaoverId.Text == "")
            {
                MessageBox.Show(this, "SEAOVER 사번을 입력해주세요.");
                this.Activate();
                return false;
            }
            else if (!DateTime.TryParse(txtInDate.Text, out DateTime dt))
            {
                MessageBox.Show(this, "입사일을 확인해주세요.");
                this.Activate();
                return false;
            }
            else if (string.IsNullOrEmpty(txtExcelPassword.Text))
            {
                MessageBox.Show(this, "Excel 다운로드 비밀번호를 확인해주세요.");
                this.Activate();
                return false;
            }
            else if (txtExcelPassword.Text == txtPassword.Text)
            {
                MessageBox.Show(this, "Excel 다운로드 비밀번호는 계정 비밀번호와 다르게 입력해주세요.");
                this.Activate();
                return false;
            }

            return true;
        }

        private void CreateAccount()
        {
            UsersModel um = new UsersModel();
            um.user_id = txtId.Text;
            um.user_password = txtPassword.Text;
            um.user_name = txtName.Text;
            um.tel = txtTel.Text;
            um.grade = txtRank.Text;
            um.form_remark = "*단가 조정 필요하신 부분은 문의 부탁드리겠습니다*";
            um.seaover_id = txtSeaoverId.Text;
            um.user_in_date = Convert.ToDateTime(txtInDate.Text).ToString("yyyy-MM-dd");
            um.workplace = cbWorkplace.Text;
            um.excel_password = txtExcelPassword.Text;

            DataTable departmentDt = departmentRepository.GetDepartment(cbDepartment.Text);
            if (departmentDt.Rows.Count == 0)
            {
                MessageBox.Show(this, "부서정보를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                um.department = departmentDt.Rows[0]["name"].ToString();
                um.auth_level = Convert.ToInt32(departmentDt.Rows[0]["auth_level"].ToString());
            }

            int results = usersRepository.InsertUser(um);
            if (results == -1)
            {
                MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                this.Activate();
                return;
            }
            else
            {
                MessageBox.Show(this, "생성완료. 로그인해주세요.");
                this.Activate();
                this.Dispose();   
            }
        }
      

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtInDate.Text = sdate;
            }
        }
    }
}
