using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement;
using AdoNetWindow.SaleManagement.DailyBusiness;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.GetSales
{
    public partial class GetSales : Form
    {
        ISalesRepository salesRepository = new SalesRepository();
        UsersModel um;
        DailyBusiness db = null;
        public GetSales(UsersModel um, DailyBusiness db)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;
        }
        public GetSales(UsersModel um, DailyBusiness db, string sale_company, string product, string origin, string sizes, string unit)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;

            string[] productTxt = product.Split(',');

            if (productTxt.Length == 1)
                this.txtProduct.Text = productTxt[0].Trim();
            else
            {
                this.txtOrigin.Text = productTxt[0].Trim();
                this.txtProduct.Text = productTxt[1].Trim();
            }
            this.txtSizes.Text = sizes;
            this.txtUnit.Text = unit;
            this.txtSaleCompany.Text = sale_company;

            txtSttdate.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            dgvSales.MultiSelect = false;
            if (!string.IsNullOrEmpty(txtSaleCompany.Text.Trim())
                || !string.IsNullOrEmpty(txtProduct.Text.Trim())
                || !string.IsNullOrEmpty(txtOrigin.Text.Trim())
                || !string.IsNullOrEmpty(txtSizes.Text.Trim())
                || !string.IsNullOrEmpty(txtUnit.Text.Trim()))
                GetData();
            if (dgvSales.Rows.Count > 0)
                this.ActiveControl = dgvSales;
        }

        private void GetSales_Load(object sender, EventArgs e)
        {
            txtSttdate.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            SetHeaderStyle();
        }

        #region Method
        public void Searching(string company, string product, string origin, string sizes, string unit)
        {
            txtSaleCompany.Text = String.Empty;
            txtProduct.Text = String.Empty;
            txtOrigin.Text = String.Empty;
            txtSizes.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtPurchaseCompany.Text = String.Empty;

            txtSaleCompany.Text = company;
            txtProduct.Text = product;
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtUnit.Text = unit;

            if(!string.IsNullOrEmpty(txtSaleCompany.Text.Trim())
                || !string.IsNullOrEmpty(txtProduct.Text.Trim())
                || !string.IsNullOrEmpty(txtOrigin.Text.Trim())
                || !string.IsNullOrEmpty(txtSizes.Text.Trim())
                || !string.IsNullOrEmpty(txtUnit.Text.Trim()))
                GetData();  

            this.Activate();
        }
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvSales;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(153, 204, 255);
            dgv.Columns["sale_qty"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 255);
            dgv.Columns["sale_price"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 255);
            dgv.Columns["purchase_price"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 255);
            dgv.Columns["purchase_company"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 255);

            dgv.Columns["sale_date"].DefaultCellStyle.BackColor = Color.FromArgb(247, 247, 189);
            dgv.Columns["sale_company"].DefaultCellStyle.BackColor = Color.FromArgb(247, 247, 189);
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(247, 247, 189);
            dgv.Columns["sale_qty"].DefaultCellStyle.BackColor = Color.FromArgb(247, 247, 189);
            dgv.Columns["sale_price"].DefaultCellStyle.BackColor = Color.FromArgb(247, 247, 189);
            dgv.Columns["purchase_price"].DefaultCellStyle.BackColor = Color.FromArgb(0, 204, 255);

        }

        private void GetData()
        {
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate))
            {
                MessageBox.Show(this, "매출기간을 다시 확인해주세요.");
                this.Activate();
                return;
            }
            else if (!DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "매출기간을 다시 확인해주세요.");
                this.Activate();
                return;
            }
            /*else if (rbCurrent.Checked && !string.IsNullOrEmpty(txtSaleCompany.Text.Trim()))
            {
                MessageBox.Show(this,"최근품목일 경우 매출처를 검색해주시기 바랍니다.");
                return;
            }*/
            dgvSales.Rows.Clear();
            DataTable salesDt = new DataTable();
            if(rbAll.Checked)
                salesDt = salesRepository.GetSales(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), txtSaleCompany.Text, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtPurchaseCompany.Text);
            else if (rbCurrent.Checked)
                salesDt = salesRepository.GetCurrentSales(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), txtSaleCompany.Text, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtPurchaseCompany.Text);
            //Data 출력
            if (salesDt.Rows.Count > 0)
            {
                for (int i = 0; i < salesDt.Rows.Count; i++)
                {
                    int n = dgvSales.Rows.Add();
                    DataGridViewRow row = dgvSales.Rows[n];

                    DateTime sale_date;
                    if (DateTime.TryParse(salesDt.Rows[i]["매출일자"].ToString(), out sale_date))
                        row.Cells["sale_date"].Value = sale_date.ToString("yyyy-MM-dd");

                    row.Cells["division"].Value = salesDt.Rows[i]["구분"].ToString();
                    row.Cells["sale_company"].Value = salesDt.Rows[i]["매출처"].ToString();
                    row.Cells["product"].Value = salesDt.Rows[i]["품명"].ToString();
                    row.Cells["origin"].Value = salesDt.Rows[i]["원산지"].ToString();
                    row.Cells["sizes"].Value = salesDt.Rows[i]["규격"].ToString();
                    row.Cells["unit"].Value = salesDt.Rows[i]["단위"].ToString();
                    row.Cells["warehouse"].Value = salesDt.Rows[i]["보관처"].ToString();

                    double sale_qty;
                    if (!double.TryParse(salesDt.Rows[i]["매출수"].ToString(), out sale_qty))
                        sale_qty = 0;
                    row.Cells["sale_qty"].Value = sale_qty.ToString("#,##0");

                    double sale_price;
                    if (!double.TryParse(salesDt.Rows[i]["매출단가"].ToString(), out sale_price))
                        sale_price = 0;
                    row.Cells["sale_price"].Value = sale_price.ToString("#,##0");

                    double vat;
                    if (!double.TryParse(salesDt.Rows[i]["부가세"].ToString(), out vat))
                        vat = 0;
                    if (vat > 0)
                        row.Cells["is_tax"].Value = true;
                    else
                        row.Cells["is_tax"].Value = false;

                    double purchase_price;
                    if (!double.TryParse(salesDt.Rows[i]["매입단가"].ToString(), out purchase_price))
                        purchase_price = 0;
                    row.Cells["purchase_price"].Value = purchase_price.ToString("#,##0");
                    row.Cells["purchase_company"].Value = salesDt.Rows[i]["매입처"].ToString();
                    row.Cells["manager"].Value = salesDt.Rows[i]["매출자"].ToString();
                }
                dgvSales.Focus();
            }
        }
        #endregion

        #region Key event
        private void GetSales_KeyDown(object sender, KeyEventArgs e)
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
                    case Keys.S:
                        //btnSelect.PerformClick();
                        break;
                    case Keys.M:
                        txtSaleCompany.Focus();
                        break;
                    case Keys.N:
                        txtSaleCompany.Text = String.Empty;
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtPurchaseCompany.Text = String.Empty;

                        txtSaleCompany.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        /*if (dgvSales.Focused && dgvSales.SelectedRows.Count > 0)
                        {
                            List<DataGridViewRow> list = new List<DataGridViewRow>();

                            for (int i = 0; i < dgvSales.Rows.Count; i++)
                            {
                                if (dgvSales.Rows[i].Selected)
                                    list.Add(dgvSales.Rows[i]);
                            }

                            if (db != null)
                            {
                                db.InputSale(list, dgvSales.MultiSelect, false);
                                if (!dgvSales.MultiSelect)
                                    this.Dispose();
                            }
                        }*/
                        break;

                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        {
                            FormCollection fc = Application.OpenForms;
                            bool isFormActive = false;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "GetSalesPrice")
                                {
                                    frm.Activate();
                                    isFormActive = true;
                                }
                            }
                            if (!isFormActive && db != null)
                            {
                                GetSalesPrice gsp = new GetSalesPrice(um, db);
                                gsp.Show();
                            }
                            e.Handled = true;
                        }
                        break;
                    case Keys.F2:
                        rbAll.Checked = !rbAll.Checked;
                        break;
                    case Keys.F3:
                        if (db != null)
                            db.Activate();
                        e.Handled = true;
                        break;
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;

                }
            }
        }

        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                if (tb.Name == "txtSttdate" || tb.Name == "txtEnddate")
                {
                    DateTime tmpDate;
                    if (DateTime.TryParse(tb.Text, out tmpDate))
                        tb.Text = tmpDate.ToString("yyyy-MM-dd");
                }
                GetData();
            }
        }
        #endregion

        #region Button
        private void btnSttdateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEnddateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            /*if (dgvSales.SelectedRows.Count == 0)
                MessageBox.Show(this,"매출내역을 선택해주세요.");
            else 
            {
                if (dgvSales.SelectedRows.Count > 0)
                {
                    List<DataGridViewRow> list = new List<DataGridViewRow>();

                    for (int i = 0; i < dgvSales.Rows.Count; i++)
                    {
                        if (dgvSales.Rows[i].Selected)
                            list.Add(dgvSales.Rows[i]);
                    }

                    if (db != null)
                    {
                        db.InputSale(list, dgvSales.MultiSelect, false);
                        if (!dgvSales.MultiSelect)
                            this.Dispose();
                    }
                }
            }*/
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Datagridview event
        private void dgvSales_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
            e.ThrowException = false;
        }
        private void dgvSales_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                /*if (dgvSales.SelectedRows.Count > 0)
                {
                    List<DataGridViewRow> list = new List<DataGridViewRow>();
                    list.Add(dgvSales.Rows[e.RowIndex]);

                    if (db != null)
                    {
                        db.InputSale(list, dgvSales.MultiSelect, false);
                        if (!dgvSales.MultiSelect)
                            this.Dispose();
                    }
                }*/
            }
        }


        #endregion

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            rbCurrent.Checked = !rbAll.Checked;
        }
    }
}
