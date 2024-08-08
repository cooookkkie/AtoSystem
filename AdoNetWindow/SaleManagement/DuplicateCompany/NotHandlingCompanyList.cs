using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.DuplicateCompany
{
    public partial class NotHandlingCompanyList : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        DataTable resultDt = new DataTable();
        DuplicateCompany dc = null;
        SalesManager sm = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public NotHandlingCompanyList(List<DataGridViewRow> NoneHandldedList, DuplicateCompany dc)
        {
            InitializeComponent();
            this.dc = dc;
            foreach (DataGridViewRow row in NoneHandldedList)
            {
                int n = dgvCompany.Rows.Add();

                dgvCompany.Rows[n].Cells["rowindex"].Value = row.Index;
                dgvCompany.Rows[n].Cells["company"].Value = row.Cells["company"].Value;
                dgvCompany.Rows[n].Cells["registration_number"].Value = row.Cells["registration_number"].Value;
                dgvCompany.Rows[n].Cells["tel"].Value = row.Cells["tel"].Value;
                dgvCompany.Rows[n].Cells["fax"].Value = row.Cells["fax"].Value;
                dgvCompany.Rows[n].Cells["phone"].Value = row.Cells["phone"].Value;
                dgvCompany.Rows[n].Cells["other_phone"].Value = row.Cells["other_phone"].Value;
                dgvCompany.Rows[n].Cells["isDelete"].Value = true;
            }
        }

        public NotHandlingCompanyList(List<DataGridViewRow> NoneHandldedList, SalesManager sm)
        {
            InitializeComponent();
            this.sm = sm;
            foreach (DataGridViewRow row in NoneHandldedList)
            {
                int n = dgvCompany.Rows.Add();

                dgvCompany.Rows[n].Cells["rowindex"].Value = row.Index;
                dgvCompany.Rows[n].Cells["company"].Value = row.Cells["input_company"].Value;
                dgvCompany.Rows[n].Cells["registration_number"].Value = row.Cells["input_registration_number"].Value;
                dgvCompany.Rows[n].Cells["tel"].Value = row.Cells["input_tel"].Value;
                dgvCompany.Rows[n].Cells["fax"].Value = row.Cells["input_fax"].Value;
                dgvCompany.Rows[n].Cells["phone"].Value = row.Cells["input_phone"].Value;
                dgvCompany.Rows[n].Cells["other_phone"].Value = row.Cells["input_other_phone"].Value;
                dgvCompany.Rows[n].Cells["isDelete"].Value = true;
            }
        }


        public DataTable GetNoneHandledList()
        {
            this.ShowDialog();

            return resultDt;
        }

        #region Button
        private void btnIgnore_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvCompany.Rows)
                row.Cells["isIgnore"].Value = true;
        }

        private void btnRecovery_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvCompany.Rows)
                row.Cells["isRecovery"].Value = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvCompany.Rows)
                row.Cells["isDelete"].Value = true;
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            dgvCompany.EndEdit();
            resultDt = common.ConvertDgvToDataTable(dgvCompany);
            this.Close();
        }
        #endregion

        #region Datagridveiw event
        private void dgvCompany_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvCompany.Columns[e.ColumnIndex].Name == "isIgnore" && Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    dgvCompany.Rows[e.RowIndex].Cells["isRecovery"].Value = false;
                    dgvCompany.Rows[e.RowIndex].Cells["isDelete"].Value = false;
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "isRecovery" && Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    dgvCompany.Rows[e.RowIndex].Cells["isIgnore"].Value = false;
                    dgvCompany.Rows[e.RowIndex].Cells["isDelete"].Value = false;
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "isDelete" && Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    dgvCompany.Rows[e.RowIndex].Cells["isIgnore"].Value = false;
                    dgvCompany.Rows[e.RowIndex].Cells["isRecovery"].Value = false;
                }
                dgvCompany.EndEdit();
            }
        }

        private void dgvCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvCompany.Columns[e.ColumnIndex].Name == "isIgnore"
                    || dgvCompany.Columns[e.ColumnIndex].Name == "isRecovery"
                    || dgvCompany.Columns[e.ColumnIndex].Name == "isDelete")
                {
                    bool isChecked = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !isChecked;

                }
            }
        }
        #endregion

        #region Key event
        private void NotHandlingCompanyList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { }
            else if (e.Modifiers == Keys.Alt)
            { }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnRegister.PerformClick();
                        break;
                }
            }
        }
        #endregion

        private void NotHandlingCompanyList_FormClosing(object sender, FormClosingEventArgs e)
        {
            dgvCompany.EndEdit();
            resultDt = common.ConvertDgvToDataTable(dgvCompany);
        }
    }
}
