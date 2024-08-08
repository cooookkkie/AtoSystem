using AdoNetWindow.Model;
using MySqlX.XDevAPI.Relational;
using Repositories.OverseaManufacturing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.OverseaManufacturingBusiness
{
    public partial class SearchCompany : Form
    {
        IOverseaManufacturingRepository overseaManufacturingRepository = new OverseaManufacturingRepository();
        MetricsDashboard md;
        UsersModel um;
        string search_type;

        public SearchCompany(UsersModel um, MetricsDashboard md, string search_type)
        {
            InitializeComponent();
            this.um = um;
            this.search_type = search_type;
            this.md = md;
        }

        #region key event
        private void SearchCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.A:
                        btnAdd.PerformClick();
                        break;

                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = string.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
        }

        private void txtCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        Libs.Tools.Common common = new Libs.Tools.Common();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control tb = common.FindFocusedControl(this);
            if (txtCompany.Focused && keyData == Keys.Down)
            {
                dgvCompany.Focus();
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvCompany.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvCompany.SelectedRows)
                    md.AddCompany(row.Cells["company"].Value.ToString());
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvCompany.Rows.Clear();

            DataTable company = overseaManufacturingRepository.GetCompany(search_type, txtCompany.Text);
            for (int i = 0; i < company.Rows.Count; i++)
            {
                int n = dgvCompany.Rows.Add();
                dgvCompany.Rows[n].Cells["company"].Value = company.Rows[i][0].ToString();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                md.AddCompany(dgvCompany.Rows[e.RowIndex].Cells["company"].Value.ToString());
            }
        }
    }
}
