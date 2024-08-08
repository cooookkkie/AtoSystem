using AdoNetWindow.CalendarModule;
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
    public partial class AdminConfigManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IUsersRepository usersRepository = new UsersRepository();
        IDepartmentRepository departmentRepository = new DepartmentRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        calendar cal;
        public AdminConfigManager(UsersModel um, calendar cal)
        {
            InitializeComponent();
            this.um = um;
            this.cal = cal;
        }

        private void AdminConfigManager_Load(object sender, EventArgs e)
        {
            GetDepartment();
            GetUsers();
        }

        #region Method
        private void userUpdate(DataGridViewRow row)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "설정", "관리자설정", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            UsersModel model = new UsersModel();
            model.user_id = row.Cells["user_id"].Value.ToString();
            model.user_name = row.Cells["user_name"].Value.ToString();
            model.grade = row.Cells["grade"].Value.ToString();
            model.tel = row.Cells["tel"].Value.ToString();
            model.seaover_id = row.Cells["seaover_id"].Value.ToString();
            model.user_status = row.Cells["status"].Value.ToString();
            model.department = row.Cells["department"].Value.ToString();
            model.team = row.Cells["team"].Value.ToString();
            model.workplace = row.Cells["workplace"].Value.ToString();
            if(row.Cells["excel_password"].Value != null)
                model.excel_password = row.Cells["excel_password"].Value.ToString();

            DateTime user_in_date;
            if (row.Cells["user_in_date"].Value != null && DateTime.TryParse(row.Cells["user_in_date"].Value.ToString(), out user_in_date))
                model.user_in_date = user_in_date.ToString("yyyy-MM-dd");
            DateTime user_out_date;
            if (row.Cells["user_out_date"].Value != null && DateTime.TryParse(row.Cells["user_out_date"].Value.ToString(), out user_out_date))
                model.user_out_date = user_out_date.ToString("yyyy-MM-dd");

            DataTable dt = commonRepository.SelectAsOne("t_department", "auth_level", "name", "'" + model.department + "'");
            if(dt.Rows.Count > 0)
                model.auth_level = Convert.ToInt32(dt.Rows[0]["auth_level"].ToString());
            else
                model.auth_level = 1;

            int results = usersRepository.UpdateMyInfo2(model);
            if (results == -1)
            {
                messageBox.Show(this, "수정중 에러가 발생하였습니다.");
                this.Activate();
            }
        }
        public void GetUsers()
        {
            dgvUsers.Rows.Clear();
            DataTable usersDt = usersRepository.GetUsers(txtWorkplace.Text, txtUserDepartment.Text, txtTeam.Text, txtUserName.Text, cbStatus.Text, txtGrade.Text);
            if (usersDt.Rows.Count > 0)
            {
                for (int i = 0; i < usersDt.Rows.Count; i++)
                {
                    int n = dgvUsers.Rows.Add();
                    DataGridViewRow row = dgvUsers.Rows[n];

                    row.Cells["user_id"].Value = usersDt.Rows[i]["user_id"].ToString();
                    row.Cells["workplace"].Value = usersDt.Rows[i]["workplace"].ToString();
                    row.Cells["department"].Value = usersDt.Rows[i]["department"].ToString();
                    row.Cells["team"].Value = usersDt.Rows[i]["team"].ToString();
                    row.Cells["user_name"].Value = usersDt.Rows[i]["user_name"].ToString();
                    DateTime user_in_date;
                    if(DateTime.TryParse(usersDt.Rows[i]["user_in_date"].ToString(), out user_in_date))
                        row.Cells["user_in_date"].Value = user_in_date.ToString("yyyy-MM-dd");
                    DateTime user_out_date;
                    if (DateTime.TryParse(usersDt.Rows[i]["user_out_date"].ToString(), out user_out_date))
                        row.Cells["user_out_date"].Value = user_out_date.ToString("yyyy-MM-dd");

                    row.Cells["grade"].Value = usersDt.Rows[i]["grade"].ToString();
                    row.Cells["tel"].Value = usersDt.Rows[i]["tel"].ToString();
                    row.Cells["seaover_id"].Value = usersDt.Rows[i]["seaover_id"].ToString();


                    DataGridViewComboBoxCell sCell = new DataGridViewComboBoxCell();
                    sCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    sCell.Items.Add("승인");
                    sCell.Items.Add("대기");
                    sCell.Items.Add("퇴사");
                    sCell.Items.Add("삭제");
                    row.Cells["status"] = sCell;
                    row.Cells["status"].Value = usersDt.Rows[i]["user_status"].ToString();
                }
            }
        }

        public void GetDepartment()
        {
            dgvDepartment.Rows.Clear();
            DataTable departmentDt = departmentRepository.GetDepartment(txtDepartment.Text, txtAuthLevel.Text);
            if (departmentDt.Rows.Count > 0)
            {
                for (int i = 0; i < departmentDt.Rows.Count; i++)
                {
                    int n = dgvDepartment.Rows.Add();
                    DataGridViewRow row = dgvDepartment.Rows[n];

                    row.Cells["department_name"].Value = departmentDt.Rows[i]["name"].ToString();
                    row.Cells["auth_level"].Value = departmentDt.Rows[i]["auth_level"].ToString();
                    row.Cells["department_id"].Value = departmentDt.Rows[i]["id"].ToString();
                }
            }
        }
        #endregion

        #region Datagridview event
        private void dgvDepartment_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvDepartment.Columns[e.ColumnIndex].Name == "btnUpdate1")
                {
                    int id = Convert.ToInt32(dgvDepartment.Rows[e.RowIndex].Cells["department_id"].Value.ToString());
                    string name = dgvDepartment.Rows[e.RowIndex].Cells["department_name"].Value.ToString();
                    int auth_level = Convert.ToInt32(dgvDepartment.Rows[e.RowIndex].Cells["auth_level"].Value.ToString());
                    DepartmentManager dm = new DepartmentManager(this, id, name, auth_level);
                    dm.ShowDialog();
                }
            }
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
            var datagridview = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox 
            if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
            {
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
        }

        private void dgvUsers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgvUsers.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvUsers.Columns[e.ColumnIndex].Name == "btnUpdate2")
                {
                    userUpdate(dgvUsers.Rows[e.RowIndex]);
                }
            }
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "설정", "관리자설정", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvUsers.Rows.Count > 0)
            {
                if (messageBox.Show(this, "일괄수정 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dgvUsers.Rows)
                        userUpdate(row);
                }
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            GetUsers();
        }
        private void btnDepartmentAdd_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "설정", "관리자설정", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            DepartmentModel model = new DepartmentModel();
            if (string.IsNullOrEmpty(txtDepartment.Text.Trim()))
            {
                messageBox.Show(this, "부서명을 입력해주세요.");
                this.Activate();
                return;
            }
            model.name = txtDepartment.Text;
            int auth_level;
            if (!int.TryParse(txtAuthLevel.Text, out auth_level))
            {
                messageBox.Show(this, "권한은 숫자 형식으로만 입력이 가능합니다.");
                this.Activate();
                return;
            }
            else if (auth_level > 99 || auth_level < 1)
            {
                messageBox.Show(this, "권한은 1부터 99이하만 입력가능합니다.");
                this.Activate();
                return;
            }
            model.auth_level = auth_level;
            //sql 
            List<StringBuilder> sqlList = new List<StringBuilder>();

            model.id = commonRepository.GetNextId("t_department", "id");
            StringBuilder sql = departmentRepository.InsertDepartment(model);
            
            sqlList.Add(sql);
            //Excute
            int result = commonRepository.UpdateTran(sqlList);
            if (result == -1)
            {
                messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                txtDepartment.Text = String.Empty;
                txtAuthLevel.Text = String.Empty;
                GetDepartment();
            }
                
        }
        #endregion

        #region key event
        private void txtUserDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetUsers();
            }
        }
        private void AdminConfigManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnUpdate.PerformClick();
                        break;
                    case Keys.M:
                        txtUserName.Focus();
                        break;
                    case Keys.N:
                        txtWorkplace.Text = string.Empty;
                        txtDepartment.Text = string.Empty;
                        txtUserName.Text = string.Empty;
                        txtTeam.Text = string.Empty;
                        txtGrade.Text = string.Empty;
                        txtUserName.Focus();
                        break;
                    case Keys.Q:
                        btnSelect.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }

        #endregion

        private void btnAuthorityManager_Click(object sender, EventArgs e)
        {
            AuthorityManager.AuthorityManager am = new AuthorityManager.AuthorityManager(um);
            am.Owner = this;
            am.Show();
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
