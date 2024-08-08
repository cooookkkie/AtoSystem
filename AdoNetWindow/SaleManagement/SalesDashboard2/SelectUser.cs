using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using static AdoNetWindow.SaleManagement.SalesDashboard2.salesDashboard2;

namespace AdoNetWindow.SaleManagement.SalesDashboard2
{
    public partial class SelectUser : Form
    {
        IUsersRepository usersRepository = new UsersRepository();
        salesDashboard2 sd2 = null;
        List<SelectUserModel> list = new List<SelectUserModel>();
        public SelectUser(salesDashboard2 sd2, List<SelectUserModel> list)
        {
            InitializeComponent();
            this.sd2 = sd2;
            this.list = list;
        }
        private void SelectUser_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int n = dgvSelectManager.Rows.Add();
                dgvSelectManager.Rows[n].Cells["select_user_id"].Value = list[i].user_id;
                dgvSelectManager.Rows[n].Cells["select_department"].Value = list[i].department;
                dgvSelectManager.Rows[n].Cells["select_team"].Value = list[i].team;
                dgvSelectManager.Rows[n].Cells["select_user_name"].Value = list[i].user_name;
                dgvSelectManager.Rows[n].Cells["select_grade"].Value = list[i].grade;
            }
            GetData();
        }

        #region Key event
        private void SelectUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.M:
                        txtDepartment.Focus();
                        break;
                    case Keys.N:
                        txtDepartment.Text = string.Empty;
                        txtTeam.Text = string.Empty;
                        txtGrade.Text = string.Empty;
                        txtManager.Text = string.Empty;

                        txtDepartment.Focus();
                        break;
                }
            }
            else 
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                }
            }
        }

        private void txtDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearching.PerformClick();
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            list = new List<SelectUserModel>();
            if (dgvSelectManager.Rows.Count > 0)
            {
                for (int i = 0; i < dgvSelectManager.Rows.Count; i++)
                {
                    SelectUserModel model = new SelectUserModel();
                    model.user_id = dgvSelectManager.Rows[i].Cells["select_user_id"].Value.ToString();
                    model.department = dgvSelectManager.Rows[i].Cells["select_department"].Value.ToString();
                    model.team = dgvSelectManager.Rows[i].Cells["select_team"].Value.ToString();
                    model.user_name = dgvSelectManager.Rows[i].Cells["select_user_name"].Value.ToString();
                    model.grade = dgvSelectManager.Rows[i].Cells["select_grade"].Value.ToString();

                    list.Add(model);
                }
            }
            this.Dispose();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            AddUsers();
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void panel11_MouseClick(object sender, MouseEventArgs e)
        {
            AddUsers();
        }
        #endregion

        #region Method
        public List<SelectUserModel> GetUsers()
        {
            this.ShowDialog();

            return list;
        }
        private void AddUsers()
        {
            for (   int i = 0; i < dgvManager.Rows.Count; i++)
            {
                if (dgvManager.Rows[i].Selected)
                {
                    bool isExist = false;
                    for (int j = 0; j < dgvSelectManager.Rows.Count; j++)
                    {
                        if (dgvSelectManager.Rows[j].Cells["select_user_id"].Value == dgvManager.Rows[i].Cells["user_id"].Value)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        int n = dgvSelectManager.Rows.Add();
                        dgvSelectManager.Rows[n].Cells["select_user_id"].Value = dgvManager.Rows[i].Cells["user_id"].Value;
                        dgvSelectManager.Rows[n].Cells["select_department"].Value = dgvManager.Rows[i].Cells["department"].Value;
                        dgvSelectManager.Rows[n].Cells["select_team"].Value = dgvManager.Rows[i].Cells["team"].Value;
                        dgvSelectManager.Rows[n].Cells["select_grade"].Value = dgvManager.Rows[i].Cells["grade"].Value;
                        dgvSelectManager.Rows[n].Cells["select_user_name"].Value = dgvManager.Rows[i].Cells["user_name"].Value;

                    }
                }
            }
        }
        private void GetData()
        { 
            dgvManager.Rows.Clear();
            DataTable userDt = usersRepository.GetUsers("", txtDepartment.Text, txtTeam.Text, txtManager.Text, "승인", txtGrade.Text);
            if (userDt.Rows.Count > 0)
            { 
                for(int i = 0; i < userDt.Rows.Count; i++) 
                {
                    int n = dgvManager.Rows.Add();
                    dgvManager.Rows[n].Cells["user_id"].Value = userDt.Rows[i]["user_id"].ToString();
                    dgvManager.Rows[n].Cells["department"].Value = userDt.Rows[i]["department"].ToString();
                    dgvManager.Rows[n].Cells["team"].Value = userDt.Rows[i]["team"].ToString();
                    dgvManager.Rows[n].Cells["grade"].Value = userDt.Rows[i]["grade"].ToString();
                    dgvManager.Rows[n].Cells["user_name"].Value = userDt.Rows[i]["user_name"].ToString();
                }
            }
            dgvManager.Focus();
        }


        #endregion

        #region Datagridview event
        private void dgvManager_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 0) 
            {
                bool isExist = false;
                for (int j = 0; j < dgvSelectManager.Rows.Count; j++)
                {
                    if (dgvSelectManager.Rows[j].Cells["select_user_id"].Value == dgvManager.Rows[e.RowIndex].Cells["user_id"].Value)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    int n = dgvSelectManager.Rows.Add();
                    dgvSelectManager.Rows[n].Cells["select_user_id"].Value = dgvManager.Rows[e.RowIndex].Cells["user_id"].Value;
                    dgvSelectManager.Rows[n].Cells["select_department"].Value = dgvManager.Rows[e.RowIndex].Cells["department"].Value;
                    dgvSelectManager.Rows[n].Cells["select_team"].Value = dgvManager.Rows[e.RowIndex].Cells["team"].Value;
                    dgvSelectManager.Rows[n].Cells["select_grade"].Value = dgvManager.Rows[e.RowIndex].Cells["grade"].Value;
                    dgvSelectManager.Rows[n].Cells["select_user_name"].Value = dgvManager.Rows[e.RowIndex].Cells["user_name"].Value;

                }
            }
        }
        #endregion
    }
}
