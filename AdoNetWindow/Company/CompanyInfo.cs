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

namespace AdoNetWindow.Company
{
    public partial class CompanyInfo : Form
    {
        ICompanyRepository companyRepository = new CompanyRepository();
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um;
        int id;
        //등록
        public CompanyInfo(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            id = commonRepository.GetNextId("t_company", "id");
            txtAtoManager.Text = um.user_name;
        }
        //수정
        public CompanyInfo(UsersModel uModel, DataTable companyDt)
        {
            InitializeComponent();
            um = uModel;
            id = Convert.ToInt32(companyDt.Rows[0]["id"].ToString());
            cbDivision.Text = companyDt.Rows[0]["division"].ToString();
            txtRegistrationNumber.Text = companyDt.Rows[0]["registration_number"].ToString();
            txtGroup.Text = companyDt.Rows[0]["group_name"].ToString();
            txtName.Text = companyDt.Rows[0]["name"].ToString();
            txtOrigin.Text = companyDt.Rows[0]["origin"].ToString();
            txtAddress.Text = companyDt.Rows[0]["address"].ToString();
            txtCeo.Text = companyDt.Rows[0]["ceo"].ToString();

            txtTel.Text = companyDt.Rows[0]["tel"].ToString();
            txtFax.Text = companyDt.Rows[0]["fax"].ToString();
            txtPhone.Text = companyDt.Rows[0]["phone"].ToString();
            txtCompanyManager.Text = companyDt.Rows[0]["company_manager"].ToString();
            txtCompanyManagerPosition.Text = companyDt.Rows[0]["company_manager_position"].ToString();
            txtEmail.Text = companyDt.Rows[0]["email"].ToString();
            txtSns1.Text = companyDt.Rows[0]["sns1"].ToString();
            txtSns2.Text = companyDt.Rows[0]["sns2"].ToString();
            txtSns3.Text = companyDt.Rows[0]["sns3"].ToString();
            txtWeb.Text = companyDt.Rows[0]["web"].ToString();
            txtRemark.Text = companyDt.Rows[0]["remark"].ToString();
            txtAtoManager.Text = companyDt.Rows[0]["ato_manager"].ToString();
            txtSeaoverCode.Text = companyDt.Rows[0]["seaover_company_code"].ToString();


            bool isManagement1;
            if (!bool.TryParse(companyDt.Rows[0]["isManagement1"].ToString(), out isManagement1))
                isManagement1 = false;

            cbManagement.Checked = isManagement1;

            bool isManagement2;
            if (!bool.TryParse(companyDt.Rows[0]["isManagement2"].ToString(), out isManagement2))
                isManagement2 = false;

            cbIsManagement2.Checked = isManagement1;

            bool isManagement3;
            if (!bool.TryParse(companyDt.Rows[0]["isManagement1"].ToString(), out isManagement1))
                isManagement1 = false;

            cbIsManagement3.Checked = isManagement1;

            bool isManagement4;
            if (!bool.TryParse(companyDt.Rows[0]["isManagement4"].ToString(), out isManagement4))
                isManagement4 = false;

            cbIsManagement4.Checked = isManagement1;

            bool isHide;
            if (!bool.TryParse(companyDt.Rows[0]["isHide"].ToString(), out isHide))
                isHide = false;
            cbIsHide.Checked = isHide;


        }
        #region Method
        private void InsertCompany()
        {
            if (string.IsNullOrEmpty(cbDivision.Text))
            {
                MessageBox.Show(this, "구분을 입력해주시기 바랍니다.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(this, "상호를 입력해주시기 바랍니다.");
                this.Activate();
                return;
            }

            string mode = "";
            DataTable companyDt = companyRepository.GetCompany("", "", "", false, "", "", "", "", "", "", id.ToString());
            if (companyDt.Rows.Count == 0)
                mode = "등록";
            else
                mode = "수정";

            if (MessageBox.Show(this,mode + "하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = new StringBuilder();
                CompanyModel cm = new CompanyModel();
                cm.division = cbDivision.Text;
                cm.registration_number = txtRegistrationNumber.Text;
                cm.group_name = txtGroup.Text;
                cm.name = txtName.Text;
                cm.origin = txtOrigin.Text;
                cm.address = txtAddress.Text;
                cm.ceo = txtCeo.Text;
                cm.tel = txtTel.Text;
                cm.fax = txtFax.Text;
                cm.phone = txtPhone.Text;
                cm.company_manager = txtCompanyManager.Text;
                cm.company_manager_position = txtCompanyManagerPosition.Text;
                cm.email = txtEmail.Text;
                cm.sns1 = txtSns1.Text;
                cm.sns2 = txtSns2.Text;
                cm.sns3 = txtSns3.Text;
                cm.web = txtWeb.Text;
                cm.remark = txtRemark.Text;
                cm.ato_manager = txtAtoManager.Text;
                cm.isManagement1 = cbManagement.Checked;
                cm.isManagement2 = cbIsManagement2.Checked;
                cm.isManagement3 = cbIsManagement3.Checked;
                cm.isManagement4 = cbIsManagement4.Checked;
                cm.isHide = cbIsHide.Checked;
                cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                cm.edit_user = um.user_name;
                cm.seaover_company_code = txtSeaoverCode.Text;

                if (mode == "등록")
                {
                    cm.id = commonRepository.GetNextId("t_company", "id");
                    cm.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                    //Insert
                    sql = companyRepository.InsertCompany(cm);
                    sqlList.Add(sql);
                }
                else
                {
                    cm.id = id;
                    DateTime createtime;
                    if (DateTime.TryParse(companyDt.Rows[0]["createtime"].ToString(), out createtime))
                        cm.createtime = createtime.ToString("yyyy-MM-dd");
                    //Update
                    sql = companyRepository.UpdateCompany(cm);
                    sqlList.Add(sql);
                }
                
                //Execute
                int results = commonRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    if (mode == "수정")
                        this.Dispose();
                    else
                        Refresh();
                }
           }
        }
        private void DeleteCompany()
        {
            if (MessageBox.Show(this, "삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = new StringBuilder();
                //거래처 정보
                sql = companyRepository.DeleteCompany(id);
                sqlList.Add(sql);
                //Execute
                int results = commonRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                    this.Dispose();
            }
        }
        private void Refresh()
        {
            txtGroup.Text = "";
            txtName.Text = "";
            txtOrigin.Text = "";
            txtRegistrationNumber.Text = "";
            txtAddress.Text = "";
            txtCeo.Text = "";
            txtTel.Text = "";
            txtFax.Text = "";
            txtPhone.Text = "";
            txtCompanyManager.Text = "";
            txtCompanyManagerPosition.Text = "";
            txtEmail.Text = "";
            txtSns1.Text = "";
            txtSns2.Text = "";
            txtSns3.Text = "";
            txtWeb.Text = "";
            txtRemark.Text = "";
            txtSeaoverCode.Text = "";
            txtAtoManager.Text = um.user_name;
            cbManagement.Checked = false;
            cbIsManagement2.Checked = false;
            cbIsManagement3.Checked = false;
            cbIsManagement4.Checked = false;
            cbIsHide.Checked = false;
            id = commonRepository.GetNextId("t_company", "id");
        }
        #endregion

        #region Button Click
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteCompany();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertCompany();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnManagment_Click(object sender, EventArgs e)
        {
            cbManagement.Checked = !cbManagement.Checked;
        }

        private void cbIsManagement_CheckedChanged(object sender, EventArgs e)
        {
            if (cbManagement.Checked)
                btnManagment.BackgroundImage = Properties.Resources.Star_icon1;
            else
                btnManagment.BackgroundImage = Properties.Resources.Star_empty_icon;
        }
        private void btnManagment2_Click(object sender, EventArgs e)
        {
            cbIsManagement2.Checked = !cbIsManagement2.Checked;
        }

        private void btnManagment3_Click(object sender, EventArgs e)
        {
            cbIsManagement3.Checked = !cbIsManagement3.Checked;
        }

        private void btnManagment4_Click(object sender, EventArgs e)
        {
            cbIsManagement4.Checked = !cbIsManagement4.Checked;
        }

        private void cbIsManagement2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement2.Checked)
                btnManagment2.BackgroundImage = Properties.Resources.Star_icon2;
            else
                btnManagment2.BackgroundImage = Properties.Resources.Star_empty_icon;
        }

        private void cbIsManagement3_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement3.Checked)
                btnManagment3.BackgroundImage = Properties.Resources.Star_icon3;
            else
                btnManagment3.BackgroundImage = Properties.Resources.Star_empty_icon;
        }

        private void cbIsManagement4_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsManagement4.Checked)
                btnManagment4.BackgroundImage = Properties.Resources.Star_icon4;
            else
                btnManagment4.BackgroundImage = Properties.Resources.Star_empty_icon;
        }
        #endregion

        #region Key Event
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            {
                switch (keyData)
                {
                    case Keys.Left:
                        return base.ProcessCmdKey(ref msg, keyData);
                    case Keys.Right:
                        return base.ProcessCmdKey(ref msg, keyData);
                    case Keys.Up:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Down:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        private void CompanyInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        {
                            InsertCompany();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
        }


        #endregion

        private void CompanyInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
