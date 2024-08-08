using AdoNetWindow.Model;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AdoNetWindow.SaleManagement.SalesDashboard2.salesDashboard2;

namespace AdoNetWindow.SaleManagement.SalesDashboard2
{
    public partial class SelectCompany : Form
    {
        ISeaoverCompanyRepository seaoverCompanyRepository = new SeaoverCompanyRepository();
        salesDashboard2 sd2 = null;
        List<SelectCompanyModel> list = new List<SelectCompanyModel>();
        UsersModel um;
        public SelectCompany(UsersModel um, salesDashboard2 sd2, List<SelectCompanyModel> list)
        {
            InitializeComponent();
            this.um = um;
            this.list = list;
            this.sd2 = sd2;
        }
        private void SelectCompany_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int n = dgvSelectCompany.Rows.Add();
                dgvSelectCompany.Rows[n].Cells["select_seaover_company_code"].Value = list[i].seaover_company_code;
                dgvSelectCompany.Rows[n].Cells["select_company"].Value = list[i].company;
                dgvSelectCompany.Rows[n].Cells["select_registration_number"].Value = list[i].registration_number;
                dgvSelectCompany.Rows[n].Cells["select_ceo"].Value = list[i].ceo;
                dgvSelectCompany.Rows[n].Cells["select_manager"].Value = list[i].manager;
            }
        }

        #region Method
        public List<SelectCompanyModel> GetCompanys()
        {
            this.ShowDialog();

            return list;
        }
        private void AddCompanys()
        {
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                if (dgvCompany.Rows[i].Selected)
                {
                    bool isExist = false;
                    for (int j = 0; j < dgvSelectCompany.Rows.Count; j++)
                    {
                        if (dgvSelectCompany.Rows[j].Cells["select_seaover_company_code"].Value == dgvCompany.Rows[i].Cells["seaover_company_code"].Value)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        int n = dgvSelectCompany.Rows.Add();
                        dgvSelectCompany.Rows[n].Cells["select_seaover_company_code"].Value = dgvCompany.Rows[i].Cells["seaover_company_code"].Value;
                        dgvSelectCompany.Rows[n].Cells["select_company"].Value = dgvCompany.Rows[i].Cells["company"].Value;
                        dgvSelectCompany.Rows[n].Cells["select_registration_number"].Value = dgvCompany.Rows[i].Cells["registration_number"].Value;
                        dgvSelectCompany.Rows[n].Cells["select_ceo"].Value = dgvCompany.Rows[i].Cells["ceo"].Value;
                        dgvSelectCompany.Rows[n].Cells["select_manager"].Value = dgvCompany.Rows[i].Cells["manager"].Value;
                    }
                }
            }
        }
        #endregion


        #region Button
        private void btnSearching_Click(object sender, EventArgs e)
        {
            dgvCompany.Rows.Clear();
            DataTable companyDt = seaoverCompanyRepository.GetSeaoverCompanyInfo(txtCompany.Text, "", txtRegistrationNumber.Text, txtCeo.Text, txtManager.Text);
            if (companyDt.Rows.Count > 0)
            {
                for (int i = 0; i < companyDt.Rows.Count; i++)
                {
                    int n = dgvCompany.Rows.Add();
                    dgvCompany.Rows[n].Cells["seaover_company_code"].Value = companyDt.Rows[i]["거래처코드"].ToString();
                    dgvCompany.Rows[n].Cells["company"].Value = companyDt.Rows[i]["거래처명"].ToString();
                    dgvCompany.Rows[n].Cells["registration_number"].Value = companyDt.Rows[i]["사업자번호"].ToString();
                    dgvCompany.Rows[n].Cells["ceo"].Value = companyDt.Rows[i]["대표자명"].ToString();
                    dgvCompany.Rows[n].Cells["manager"].Value = companyDt.Rows[i]["매출자"].ToString();
                }
            }
            dgvCompany.Focus();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            AddCompanys();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            list = new List<SelectCompanyModel>();
            if (dgvSelectCompany.Rows.Count > 0)
            {
                for (int i = 0; i < dgvSelectCompany.Rows.Count; i++)
                {
                    SelectCompanyModel model = new SelectCompanyModel();
                    model.seaover_company_code = dgvSelectCompany.Rows[i].Cells["select_seaover_company_code"].Value.ToString();
                    model.company = dgvSelectCompany.Rows[i].Cells["select_company"].Value.ToString();
                    model.registration_number = dgvSelectCompany.Rows[i].Cells["select_registration_number"].Value.ToString();
                    model.ceo = dgvSelectCompany.Rows[i].Cells["select_ceo"].Value.ToString();
                    model.manager = dgvSelectCompany.Rows[i].Cells["select_manager"].Value.ToString();

                    list.Add(model);
                }
            }
            this.Dispose();
        }
        private void panel11_MouseClick(object sender, MouseEventArgs e)
        {
            AddCompanys();
        }

        #endregion

        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                bool isExist = false;
                for (int j = 0; j < dgvSelectCompany.Rows.Count; j++)
                {
                    if (dgvSelectCompany.Rows[j].Cells["select_seaover_company_code"].Value == dgvCompany.Rows[e.RowIndex].Cells["seaover_company_code"].Value)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    int n = dgvSelectCompany.Rows.Add();
                    dgvSelectCompany.Rows[n].Cells["select_seaover_company_code"].Value = dgvCompany.Rows[e.RowIndex].Cells["seaover_company_code"].Value;
                    dgvSelectCompany.Rows[n].Cells["select_company"].Value = dgvCompany.Rows[e.RowIndex].Cells["company"].Value;
                    dgvSelectCompany.Rows[n].Cells["select_registration_number"].Value = dgvCompany.Rows[e.RowIndex].Cells["registration_number"].Value;
                    dgvSelectCompany.Rows[n].Cells["select_ceo"].Value = dgvCompany.Rows[e.RowIndex].Cells["ceo"].Value;
                    dgvSelectCompany.Rows[n].Cells["select_manager"].Value = dgvCompany.Rows[e.RowIndex].Cells["manager"].Value;
                }
            }
        }

        #region Key evnet
        private void txtCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearching.PerformClick();
        }
        private void SelectCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode) 
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = string.Empty;
                        txtRegistrationNumber.Text = string.Empty;
                        txtCeo.Text = string.Empty;
                        txtManager.Text = string.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
        }

        #endregion

        
    }
}
