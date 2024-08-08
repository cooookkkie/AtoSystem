using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    public partial class SearchCompany : Form
    {
        UsersModel um;
        DataTable companyDt;
        string cname = null;
        string ccode = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public SearchCompany(UsersModel um, DataTable companyDt, string search_txt, bool isMyCompany)
        {
            InitializeComponent();
            this.um = um;
            this.companyDt = companyDt;
            txtCompany.Text = search_txt;
            cbMyCompany.Checked = isMyCompany;
        }

        #region Method
        public string GetCompanyCode(out string company)
        {
            GetData();

            try
            {
                this.ShowDialog();
            }
            catch
            { }
            company = cname;
            return ccode;
        }
        private void GetData()
        {
            dgvCompany.Rows.Clear();
            if (companyDt != null && companyDt.Rows.Count > 0)
            {
                DataTable resultDt;
                if (!string.IsNullOrEmpty(txtCompany.Text.Trim()))
                {
                    DataRow[] dr = companyDt.Select($"매출처 LIKE '%{txtCompany.Text.Trim()}%'");

                    if(!cbMyCompany.Checked)
                        dr = companyDt.Select($"매출처 LIKE '%{txtCompany.Text.Trim()}%'");
                    else
                        dr = companyDt.Select($"매출처 LIKE '%{txtCompany.Text.Trim()}%' AND 매출자 = '{um.user_name}'");

                    if (dr.Length > 0)
                        resultDt = dr.CopyToDataTable();
                    else
                        resultDt = null;
                }
                else
                    resultDt = companyDt;

                if (resultDt != null)
                {
                    for (int i = 0; i < resultDt.Rows.Count; i++)
                    {
                        int n = dgvCompany.Rows.Add();
                        dgvCompany.Rows[n].Cells["company_code"].Value = resultDt.Rows[i]["매출처코드"].ToString();
                        dgvCompany.Rows[n].Cells["company"].Value = resultDt.Rows[i]["매출처"].ToString();
                    }
                }

                dgvCompany.Focus();
                this.ActiveControl = dgvCompany;
            }
        }
        #endregion

        #region Key event
        private void SearchCompany_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSeaching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = String.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            { }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (dgvCompany.Focused && dgvCompany.SelectedRows.Count > 0)
                        {
                            cname = dgvCompany.SelectedRows[0].Cells["company"].Value.ToString();
                            ccode = dgvCompany.SelectedRows[0].Cells["company_code"].Value.ToString();
                            this.Dispose();
                        }
                        break;
                    case Keys.Escape:
                        cname = null;
                        ccode = null;
                        this.Dispose();
                        break;
                }
            }
        }
        private void txtCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetData();
            else if (e.KeyCode == Keys.Down)
                dgvCompany.Focus();
        }
        #endregion

        #region Button
        private void btnSeaching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            cname = null;
            ccode = null;
            this.Dispose();
        }
        #endregion

        #region Datagridview event
        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                cname = dgvCompany.Rows[e.RowIndex].Cells["company"].Value.ToString();
                ccode = dgvCompany.Rows[e.RowIndex].Cells["company_code"].Value.ToString();
                this.Dispose();
            }
        }
        #endregion
    }
}
