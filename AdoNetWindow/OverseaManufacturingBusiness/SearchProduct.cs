using AdoNetWindow.Model;
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
    public partial class SearchProduct : Form
    {
        IOverseaManufacturingRepository overseaManufacturingRepository = new OverseaManufacturingRepository();
        UsersModel um;
        MetricsDashboard md;
        public SearchProduct(UsersModel um, MetricsDashboard md)
        {
            InitializeComponent();
            this.um = um;
            this.md = md;
        }

        #region Button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                    md.AddProduct(row.Cells["pname_kor"].Value.ToString(), row.Cells["pname_eng"].Value.ToString());
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvProduct.Rows.Clear();

            DataTable productDt = overseaManufacturingRepository.GetProduct(txtPnameKor.Text, txtPnameEng.Text);
            for (int i = 0; i < productDt.Rows.Count; i++)
            {
                int n = dgvProduct.Rows.Add();
                dgvProduct.Rows[n].Cells[0].Value = productDt.Rows[i][0].ToString();
                dgvProduct.Rows[n].Cells[1].Value = productDt.Rows[i][1].ToString();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region key event
        private void txtPnameKor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }

        private void SearchProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnAdd.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;

                }
            }
        }

        Libs.Tools.Common common = new Libs.Tools.Common();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control tb = common.FindFocusedControl(this);
            if ((txtPnameKor.Focused || txtPnameEng.Focused) && keyData == Keys.Down)
            {
                dgvProduct.Focus();
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                md.AddProduct(dgvProduct.Rows[e.RowIndex].Cells["pname_kor"].Value.ToString(), dgvProduct.Rows[e.RowIndex].Cells["pname_eng"].Value.ToString());
            }
        }
    }
}
