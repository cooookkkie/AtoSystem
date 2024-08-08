using AdoNetWindow.CalendarModule;
using AdoNetWindow.Model;
using Repositories;
using Repositories.Company;
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

namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    public partial class CompanyInsuranceManager : Form
    {
        ICompanyRepository companyRepository = new CompanyRepository();
        ICommonRepository commonRepository = new CommonRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um = null;
        calendar cal = null;
        public CompanyInsuranceManager(UsersModel um, calendar cal)
        {
            InitializeComponent();
            this.um = um;
            this.cal = cal;
            cbOnlyInsurance.Checked = true;
        }
        public CompanyInsuranceManager(UsersModel um, calendar cal, string company)
        {
            InitializeComponent();
            this.um = um;
            this.cal = cal;
            txtCompany.Text = company;
            cbExactly.Checked = true;
        }
        #region Method
        public void CalendarOpenAlarm(bool isMsg = false)
        {
            int cnt = GetDatatable();
            if (cnt > 0)
            {
                this.Show();
                this.Owner = cal;
            }

            else if (isMsg)
            {
                MessageBox.Show(this, "임박 품목내역이 없습니다.");
                this.Activate();
            }
        }
        public int GetDatatable() 
        {
            dgvCompany.Rows.Clear();
            int limit_count;
            if (!int.TryParse(cbLimitCount.Text, out limit_count))
                limit_count = 100;
            DataTable insuranceDt = companyRepository.GetInsuranceCompany(txtCompany.Text, cbExactly.Checked, txtCorporationNumber.Text, txtCeo.Text, limit_count, cbOnlyInsurance.Checked);
            if(insuranceDt.Rows.Count > 0 ) 
            {
                this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                for (int i = 0; i < insuranceDt.Rows.Count; i++)
                {
                    int n = dgvCompany.Rows.Add();
                    dgvCompany.Rows[n].Cells["company_id"].Value = insuranceDt.Rows[i]["id"].ToString();
                    dgvCompany.Rows[n].Cells["company"].Value = insuranceDt.Rows[i]["company"].ToString();
                    dgvCompany.Rows[n].Cells["ceo"].Value = insuranceDt.Rows[i]["ceo"].ToString();
                    dgvCompany.Rows[n].Cells["registration_number"].Value = insuranceDt.Rows[i]["registration_number"].ToString();
                    dgvCompany.Rows[n].Cells["corporation_number"].Value = insuranceDt.Rows[i]["corporation_number"].ToString();
                    dgvCompany.Rows[n].Cells["kc_number"].Value = insuranceDt.Rows[i]["kc_number"].ToString();

                    double insurance_amount;
                    if (!double.TryParse(insuranceDt.Rows[i]["insurance_amount"].ToString(), out insurance_amount))
                        insurance_amount = 0;
                    dgvCompany.Rows[n].Cells["insurance_amount"].Value = insurance_amount.ToString("#,##0");

                    DateTime insurance_sttdate;
                    if (DateTime.TryParse(insuranceDt.Rows[i]["insurance_sttdate"].ToString(), out insurance_sttdate))
                        dgvCompany.Rows[n].Cells["insurance_sttdate"].Value = insurance_sttdate.ToString("yyyy-MM-dd");

                    DateTime insurance_enddate;
                    if (DateTime.TryParse(insuranceDt.Rows[i]["insurance_enddate"].ToString(), out insurance_enddate))
                    {
                        dgvCompany.Rows[n].Cells["insurance_enddate"].Value = insurance_enddate.ToString("yyyy-MM-dd");

                        TimeSpan ts = DateTime.Now - insurance_enddate;
                        dgvCompany.Rows[n].Cells["left_days"].Value = ts.Days.ToString("#,##0");
                    }
                }
                this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            }
            return insuranceDt.Rows.Count;
        }
        #endregion

        #region Button
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetDatatable();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //데이터 유효성검사
            int updateCnt = 0;
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];
                if (row.Cells["chk"].Value != null && bool.TryParse(row.Cells["chk"].Value.ToString(), out bool isChecked) && isChecked)
                {

                    if (row.Cells["insurance_amount"].Value == null || !double.TryParse(row.Cells["insurance_amount"].Value.ToString(), out double insurance_amount))
                    {
                        dgvCompany.CurrentCell = row.Cells["insurance_amount"];
                        messageBox.Show(this, "보험금액을 확인해주세요.");
                        return;
                    }
                    if (row.Cells["insurance_enddate"].Value == null || !DateTime.TryParse(row.Cells["insurance_enddate"].Value.ToString(), out DateTime insurance_enddate))
                    {
                        dgvCompany.CurrentCell = row.Cells["insurance_enddate"];
                        messageBox.Show(this, "보험 종료일을 확인해주세요.");
                        return;
                    }
                    updateCnt++;
                }
            }

            if (messageBox.Show(this, $"{updateCnt.ToString("#,##0")}개의 거래처를 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];
                if (row.Cells["chk"].Value != null && bool.TryParse(row.Cells["chk"].Value.ToString(), out bool isChecked) && isChecked)
                {
                    double insurance_amount;
                    if (row.Cells["insurance_amount"].Value == null || !double.TryParse(row.Cells["insurance_amount"].Value.ToString(), out insurance_amount))
                        insurance_amount = 0;

                    string insurance_sttdate_txt = "";
                    if (row.Cells["insurance_sttdate"].Value != null && DateTime.TryParse(row.Cells["insurance_sttdate"].Value.ToString(), out DateTime insurance_sttdate))
                        insurance_sttdate_txt = insurance_sttdate.ToString("yyyy-MM-dd");

                    string insurance_enddate_txt = "";
                    if (row.Cells["insurance_enddate"].Value != null && DateTime.TryParse(row.Cells["insurance_enddate"].Value.ToString(), out DateTime insurance_enddate))
                        insurance_enddate_txt = insurance_enddate.ToString("yyyy-MM-dd");

                    StringBuilder sql = companyRepository.UpdateInsurance(row.Cells["company_id"].Value.ToString()
                                                                        , row.Cells["registration_number"].Value.ToString()
                                                                        , row.Cells["corporation_number"].Value.ToString()
                                                                        , row.Cells["ceo"].Value.ToString()
                                                                        , row.Cells["kc_number"].Value.ToString()
                                                                        , insurance_amount, insurance_sttdate_txt, insurance_enddate_txt);
                    sqlList.Add(sql);
                }
            }

            if (commonRepository.UpdateTran(sqlList) == -1)
                messageBox.Show(this, "수정중 에러가 발생하였습니다.");
            else
                messageBox.Show(this, "수정완료");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Datagridview event
        private void dgvCompany_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                dgvCompany.Rows[e.RowIndex].Cells[0].Value = true;
                if (dgvCompany.Columns[e.ColumnIndex].Name == "insurance_amount")
                {
                    if (dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && double.TryParse(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out double insurance_amount))
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = insurance_amount.ToString("#,##0");
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "insurance_sttdate" || dgvCompany.Columns[e.ColumnIndex].Name == "insurance_enddate")
                {
                    if (dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim()))
                    {
                        string temp_date = common.strDatetime(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        if (DateTime.TryParse(temp_date, out DateTime dt))
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dt.ToString("yyyy-MM-dd");

                        if (dgvCompany.Columns[e.ColumnIndex].Name == "insurance_enddate" && DateTime.TryParse(temp_date, out DateTime enddate))
                        {
                            TimeSpan ts = enddate - DateTime.Now;
                            dgvCompany.Rows[e.RowIndex].Cells["left_days"].Value = (ts.Days + 1).ToString("#,##0");
                        }
                    }
                }

                this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            }
        }

        private void dgvCompany_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    bool isChecked;
                    if (dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !bool.TryParse(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out isChecked))
                        isChecked = false;

                    if (isChecked)
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                    else
                        dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "insurance_enddate"
                    && dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null
                    && !string.IsNullOrEmpty(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                {
                    if (DateTime.TryParse(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out DateTime insurance_enddate)
                        && insurance_enddate < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        dgvCompany.Rows[e.RowIndex].Cells["kc_number"].Style.ForeColor = Color.LightGray;
                        dgvCompany.Rows[e.RowIndex].Cells["insurance_sttdate"].Style.ForeColor = Color.LightGray;
                        dgvCompany.Rows[e.RowIndex].Cells["insurance_enddate"].Style.ForeColor = Color.LightGray;
                    }
                }
            }
        }
        #endregion

        #region Key event
        private void CompanyInsuranceManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.A:
                        btnUpdate.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;

                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = string.Empty;
                        txtCorporationNumber.Text = string.Empty;
                        txtCeo.Text = string.Empty;
                        txtCompany.Focus();
                        break;

                }
            }
        }

        private void txtCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetDatatable();
        }
        #endregion
    }
}
