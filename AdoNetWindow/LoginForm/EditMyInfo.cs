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

namespace AdoNetWindow.Config
{
    public partial class EditMyInfo : Form
    {
        CalendarModule.calendar cal;
        IUsersRepository usersRepository = new UsersRepository();
        IDepartmentRepository departmentRepository = new DepartmentRepository();
        UsersModel um;
        public EditMyInfo(UsersModel model)
        {
            InitializeComponent();
            um = model;
        }
        public EditMyInfo(UsersModel model, CalendarModule.calendar calendar)
        {
            InitializeComponent();
            um = model;
            cal = calendar;   
        }
        public UsersModel UpdateSeaoverId()
        {
            this.ShowDialog();
            um = usersRepository.GetUserInfo(um.user_id);
            return um;
        }

        private void EditMyInfo_Load(object sender, EventArgs e)
        {
            txtName.Text = um.user_name;
            txtRank.Text = um.grade;
            txtTel.Text = um.tel;
            txtSeaoverId.Text = um.seaover_id;
            DateTime user_in_date;
            if(DateTime.TryParse(um.user_in_date, out user_in_date))
                txtInDate.Text = user_in_date.ToString("yyyy-MM-dd");
            cbWorkplace.Text = um.workplace;
            DataTable departmentDt = departmentRepository.GetDepartment("", "");
            for (int i = 0; i < departmentDt.Rows.Count; i++)
            {
                cbDepartment.Items.Add(departmentDt.Rows[i]["name"].ToString());
            }

            if (um.auth_level < 99)
            {
                cbDepartment.Items.Remove("관리부");
                cbDepartment.Items.Remove("전산부");
            }

            cbDepartment.Text = um.department;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtInspection())
            { 
                UsersModel model = new UsersModel();
                model.user_id = um.user_id;
                model.user_password = txtPassword.Text;
                model.user_name = txtName.Text;
                model.grade = txtRank.Text;
                model.tel = txtTel.Text;
                model.seaover_id = txtSeaoverId.Text;
                model.user_status = "승인";
                model.user_in_date = Convert.ToDateTime(txtInDate.Text).ToString("yyyy-MM-dd");
                model.workplace = cbWorkplace.Text;
                model.excel_password = txtExcelPassword.Text;
                DataTable departmentDt = departmentRepository.GetDepartment(cbDepartment.Text);
                if (departmentDt.Rows.Count == 0)
                {
                    MessageBox.Show(this, "부서정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }
                else
                {
                    model.department = departmentDt.Rows[0]["name"].ToString();
                    model.auth_level = Convert.ToInt32(departmentDt.Rows[0]["auth_level"].ToString());
                }

                UsersModel mm = usersRepository.GetByUser(model.user_id, model.user_password);
                if (mm == null)
                {
                    MessageBox.Show(this, "입력한 비밀번호가 틀렸습니다.");
                    this.Activate();
                }
                else
                {
                    int results = usersRepository.UpdateMyInfo(model);
                    if (results == -1)
                    {
                        MessageBox.Show(this, "수정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else 
                    {
                        MessageBox.Show(this, "수정완료");
                        this.Activate();
                        cal.GetUserInfo();
                        this.Dispose();
                    }
                }
            }
        }

        private bool txtInspection()
        {
            bool isPass = true;
            if (txtPassword.Text == "")
            {
                MessageBox.Show(this, "현재 비밀번호를 입력해주세요.");
                this.Activate();
                isPass = false;
            }
            else if (txtName.Text == "")
            {
                MessageBox.Show(this, "이름을 입력해주세요.");
                this.Activate();
                isPass = false;
            }
            else if (txtSeaoverId.Text == "")
            {
                MessageBox.Show(this, "SEAOVER 사번을 입력해주세요.");
                this.Activate();
                isPass = false;
            }
            else if (!DateTime.TryParse(txtInDate.Text, out DateTime dt))
            {
                MessageBox.Show(this, "입사일을 확인해주세요.");
                this.Activate();
                isPass = false;
            }
            else if (string.IsNullOrEmpty(txtExcelPassword.Text))
            {
                MessageBox.Show(this, "Excel 다운로드 비밀번호를 입력해주세요.");
                this.Activate();
                isPass = false;
            }

            return isPass;
        }

        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtInDate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
    }
}
