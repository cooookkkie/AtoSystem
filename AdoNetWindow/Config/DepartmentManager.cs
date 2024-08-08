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
    public partial class DepartmentManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IDepartmentRepository departmentRepository = new DepartmentRepository();
        bool isNew;
        AdminConfigManager acm;
        public DepartmentManager(AdminConfigManager acManager, int id, string name, int auth_lebel)
        {
            InitializeComponent();

            lbId.Text = id.ToString(); 
            txtName.Text = name.ToString();
            txtAuthLevel.Text = auth_lebel.ToString();
            isNew = false;
            acm = acManager;
        }
        public DepartmentManager(AdminConfigManager acManager)
        {
            InitializeComponent();
            isNew = true;
            acm = acManager;
        }
        private void DepartmentManager_Load(object sender, EventArgs e)
        {
            if (isNew)
                btnDelete.Visible = false;
        }

        #region Button
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DepartmentModel model = new DepartmentModel();
            

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show(this, "부서명을 입력해주세요.");
                this.Activate();
                return;
            }
            model.name = txtName.Text;
            int auth_level;
            if (!int.TryParse(txtAuthLevel.Text, out auth_level))
            {
                MessageBox.Show(this, "권한은 숫자 형식으로만 입력이 가능합니다.");
                this.Activate();
                return;
            }
            else if (auth_level > 99 || auth_level < 1)
            {
                MessageBox.Show(this, "권한은 1부터 99이하만 입력가능합니다.");
                this.Activate();
                return;
            }
            model.auth_level = auth_level;
            //sql 
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = departmentRepository.DeleteDepartment(model);
            sqlList.Add(sql);

            if (isNew)
            {
                model.id = commonRepository.GetNextId("t_department", "id");
                sql = departmentRepository.InsertDepartment(model);
            }
            else
            {
                model.id = Convert.ToInt32(lbId.Text);
                sql = departmentRepository.UpdateDepartment(model);
            }
            sqlList.Add(sql);
            //Excute
            int result = commonRepository.UpdateTran(sqlList);
            if (result == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                acm.GetDepartment();
                this.Dispose();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (isNew)
                return;

            if (MessageBox.Show(this, "부서를 삭제하시겠습니가?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DepartmentModel model = new DepartmentModel();
                model.id = Convert.ToInt32(lbId.Text);
                //sql 
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = departmentRepository.DeleteDepartment(model);
                sqlList.Add(sql);
                //Excute
                int result = commonRepository.UpdateTran(sqlList);
                if (result == -1)
                {
                    MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    acm.GetDepartment();
                    this.Dispose();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        #region Key event
        private void DepartmentManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnUpdate.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }

        private void txtAuthLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)45 || e.KeyChar == (Char)46 || e.KeyChar == (Char)47))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
            //이전값이 0일경우 삭제 후 입력
            Control con = (Control)sender;
            if (con.Text == "0")
            {
                con.Text = "";
            }
        }
        #endregion
    }
}
