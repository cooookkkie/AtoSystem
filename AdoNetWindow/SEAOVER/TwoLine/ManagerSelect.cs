using AdoNetWindow.Model;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.TwoLine
{
    public partial class ManagerSelect : Form
    {
        IUsersRepository usersRepository = new UsersRepository();
        UsersModel um;
        string remark = null;
        public ManagerSelect(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            GetData();
        }

        #region Method
        public string GetRemark()
        {
            GetData();
            this.ShowDialog();

            return remark;

        }
       
        private void GetData()
        {
            dgvManager.Rows.Clear();
            List<UsersModel> list = usersRepository.GetUsersList(txtDepartment.Text, txtUsername.Text, txtGrade.Text, txtTel.Text);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    int n = dgvManager.Rows.Add();
                    dgvManager.Rows[n].Cells["department"].Value = list[i].department;
                    dgvManager.Rows[n].Cells["user_name"].Value = list[i].user_name;
                    dgvManager.Rows[n].Cells["grade"].Value = list[i].grade;
                    dgvManager.Rows[n].Cells["tel"].Value = list[i].tel;
                }
            }
        }
        #endregion

        #region Datagridview event
        private void dgvManager_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvManager.ClearSelection();
            foreach (DataGridViewRow row in dgvManager.Rows)
            {
                row.Cells[0].Value = false;
            }

            dgvManager.Rows[e.RowIndex].Cells[0].Value = true;

            if (!string.IsNullOrEmpty(txtRemark.Text.Trim()))
            {
                txtRemark.Text += ", ";
            }
            txtRemark.Text += dgvManager.Rows[e.RowIndex].Cells["user_name"].Value + " "
                            + dgvManager.Rows[e.RowIndex].Cells["grade"].Value;

            string tel = Libs.Tools.Common.autoHypehen(dgvManager.Rows[e.RowIndex].Cells["tel"].Value.ToString());

            if (!string.IsNullOrEmpty(tel.Trim()))
            {
                txtRemark.Text += "(" + tel + ")";
            }
        }
        #endregion

        #region Button
        private void btnSelect_Click(object sender, EventArgs e)
        {
            remark = txtRemark.Text;
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            remark = null;
            this.Dispose();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string managerTxt;
            if (um.grade == null)
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }
            else if (string.IsNullOrEmpty(um.grade))
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }
            else
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " " + um.grade.ToString() + " (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }
            txtRemark.Text = managerTxt + " / " + um.form_remark;
            txtUsername.Text = string.Empty;
            txtGrade.Text = string.Empty;
            txtTel.Text = string.Empty;
            btnSelect.PerformClick();
        }
        #endregion

        #region Key event
        private void ManagerSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.A:
                        btnSelect.PerformClick();
                        break;
                    case Keys.M:
                        txtUsername.Focus();
                        break;
                    case Keys.N:
                        txtUsername.Text = String.Empty;
                        txtGrade.Text = String.Empty;
                        txtTel.Text = String.Empty;
                        txtUsername.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                }
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }
        #endregion
    }
}
