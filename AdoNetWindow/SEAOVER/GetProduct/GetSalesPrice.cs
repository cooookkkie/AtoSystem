using AdoNetWindow.Model;
using AdoNetWindow.SEAOVER.GetSales;
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

namespace AdoNetWindow.SaleManagement
{
    public partial class GetSalesPrice : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        DailyBusiness.DailyBusiness db = null;
        UsersModel um;
        public GetSalesPrice(UsersModel um, DailyBusiness.DailyBusiness db, string product, string origin, string sizes, string unit)
        {
            InitializeComponent();
            this.ActiveControl = txtProduct;
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
            //this.txtOrigin.Text = origin;
            this.txtSizes.Text = sizes;
            this.txtUnit.Text = unit;

            dgvProduct.MultiSelect = false;

            if (!string.IsNullOrEmpty(txtProduct.Text.Trim())
                || !string.IsNullOrEmpty(txtOrigin.Text.Trim())
                || !string.IsNullOrEmpty(txtSizes.Text.Trim())
                || !string.IsNullOrEmpty(txtUnit.Text.Trim()))
                GetData();

            if (dgvProduct.Rows.Count > 0)
            {
                this.ActiveControl = dgvProduct;
                dgvProduct.CurrentCell = dgvProduct.Rows[0].Cells["input_qty"];
            }
                
        }
        public GetSalesPrice(UsersModel um, DailyBusiness.DailyBusiness db)
        {
            InitializeComponent();
            this.ActiveControl = txtProduct;
            this.um = um;
            this.db = db;
        }

        private void GetSalesPrice_Load(object sender, EventArgs e)
        {
            SetHeaderStyle();   
        }

        #region Method
        public void Searching(string product, string origin, string sizes, string unit)
        {
            txtDivision.Text = String.Empty;
            txtProduct.Text = String.Empty;
            txtOrigin.Text = String.Empty;
            txtSizes.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtWarehouse.Text = String.Empty;
            txtPurchaseCompany.Text = String.Empty;

            string[] productTxt = product.Split(',');
            if (productTxt.Length == 1)
                this.txtProduct.Text = productTxt[0].Trim();
            else
            {
                this.txtOrigin.Text = productTxt[0].Trim();
                this.txtProduct.Text = productTxt[1].Trim();
            }
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtUnit.Text = unit;

            if(!string.IsNullOrEmpty(txtProduct.Text.Trim())
                || !string.IsNullOrEmpty(txtOrigin.Text.Trim())
                || !string.IsNullOrEmpty(txtSizes.Text.Trim())
                || !string.IsNullOrEmpty(txtUnit.Text.Trim()))
                GetData();
            this.Activate();
        }
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvProduct;

            dgv.Columns["division"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["division"].HeaderCell.Style.ForeColor = Color.Yellow;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);

            dgv.Columns["sale_price"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["sale_price"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["sale_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);


            dgv.Columns["purchase_price"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["purchase_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["purchase_price"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);


            dgv.Columns["purchase_date"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["purchase_date"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["purchase_company"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204);
            dgv.Columns["purchase_company"].HeaderCell.Style.ForeColor= Color.Black;
            dgv.Columns["remark1"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204);
            dgv.Columns["remark1"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["remark2"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204);
            dgv.Columns["remark2"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["input_qty"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv.Columns["input_qty"].Width = 50;
            dgv.Columns["input_qty"].DefaultCellStyle.BackColor = Color.Beige;
        }
        private void GetData()
        {
            CallProductProcedure();
            dgvProduct.Rows.Clear();

            DataTable productDt = seaoverRepository.GetProductInfo("", "", txtDivision.Text, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtWarehouse.Text, txtPurchaseCompany.Text, false);
            if (productDt.Rows.Count > 0)
            {
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = dgvProduct.Rows[n];

                    row.Cells["division"].Value = productDt.Rows[i]["구분"].ToString();
                    row.Cells["product"].Value = productDt.Rows[i]["품명"].ToString();
                    row.Cells["origin"].Value = productDt.Rows[i]["원산지"].ToString();
                    row.Cells["sizes"].Value = productDt.Rows[i]["규격"].ToString();
                    row.Cells["unit"].Value = productDt.Rows[i]["단위"].ToString();
                    row.Cells["seaover_unit"].Value = productDt.Rows[i]["SEAOVER단위"].ToString();

                    row.Cells["unit_count"].Value = productDt.Rows[i]["단위수량"].ToString();
                    row.Cells["price_unit"].Value = productDt.Rows[i]["가격단위"].ToString();
                    row.Cells["package_cnt"].Value = productDt.Rows[i]["묶음수"].ToString();

                    double sale_price;
                    if (!double.TryParse(productDt.Rows[i]["매출단가"].ToString(), out sale_price))
                        sale_price = 0;
                    row.Cells["sale_price"].Value = sale_price.ToString("#,##0");

                    double purchase_price;
                    if (!double.TryParse(productDt.Rows[i]["매입단가"].ToString(), out purchase_price))
                        purchase_price = 0;

                    row.Cells["purchase_price"].Value = purchase_price.ToString("#,##0");
                    row.Cells["purchase_company"].Value = productDt.Rows[i]["매입처"].ToString();
                    row.Cells["warehouse"].Value = productDt.Rows[i]["보관처"].ToString();
                    DateTime purchase_date;
                    if (DateTime.TryParse(productDt.Rows[i]["매입일자"].ToString(), out purchase_date))
                        row.Cells["purchase_date"].Value = purchase_date.ToString("yyyy-MM-dd");


                    row.Cells["remark1"].Value = productDt.Rows[i]["비고1"].ToString();
                    row.Cells["remark2"].Value = productDt.Rows[i]["비고2"].ToString();
                    //row.Cells["remark3"].Value = productDt.Rows[i]["비고3"].ToString();
                }
                //dgvProduct.Focus();
            }
        }
        private void CallProductProcedure()
        {
            //업체별시세현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                ////업체별시세현황 스토어프로시져 호출
                if (seaoverRepository.CallStoredProcedure(user_id, sDate, eDate) == 0)
                {
                    MessageBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
                this.Activate();
            }
        }
        #endregion

        #region Button
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvProduct.Focused && dgvProduct.SelectedRows.Count > 0)
            {
                List<DataGridViewRow> list = new List<DataGridViewRow>();

                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    if (dgvProduct.Rows[i].Selected)
                        list.Add(dgvProduct.Rows[i]);
                }

                if (db != null)
                    db.InputProduct(list, dgvProduct.MultiSelect, true);

                //한줄 변경
                if (!dgvProduct.MultiSelect)
                    this.Dispose();
            }
            else if (dgvProduct.Focused)
            {
                MessageBox.Show(this, "내역을 선택해주세요.");
                this.Activate();
            }
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
        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                List<DataGridViewRow> list = new List<DataGridViewRow>();
                list.Add(dgvProduct.Rows[e.RowIndex]);
                if (db != null)
                {
                    db.InputProduct(list, dgvProduct.MultiSelect, false);
                    //한줄 변경
                    if (!dgvProduct.MultiSelect)
                        this.Dispose();
                }
                    
            }
        }
        #endregion

        #region Key event
        Libs.Tools.Common common = new Libs.Tools.Common();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control tb = common.FindFocusedControl(this);
            
            if (keyData == Keys.Down 
                && (tb.Name == "txtDivision"
                || tb.Name == "txtProduct"
                || tb.Name == "txtOrigin"
                || tb.Name == "txtSizes"
                || tb.Name == "txtUnit"
                || tb.Name == "txtWarehouse"
                || tb.Name == "txtPurchaseCompany"))
            {
                if (dgvProduct.Rows.Count > 0)
                {
                    dgvProduct.CurrentCell = dgvProduct.Rows[0].Cells["input_qty"];
                    dgvProduct.Focus();
                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        private void GetSalesPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.S:
                        btnSelect.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtDivision.Text = String.Empty;
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtWarehouse.Text = String.Empty;
                        txtPurchaseCompany.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        {
                            if (dgvProduct.Focused && dgvProduct.SelectedRows.Count > 0)
                            {
                                List<DataGridViewRow> list = new List<DataGridViewRow>();

                                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                                {
                                    if (dgvProduct.Rows[i].Selected)
                                        list.Add(dgvProduct.Rows[i]);
                                }

                                if (db != null)
                                    db.InputProduct(list, dgvProduct.MultiSelect, false);
                                //this.Dispose();
                            }
                            else if (dgvProduct.Focused)
                            {
                                MessageBox.Show(this, "내역을 선택해주세요.");
                                this.Activate();
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F2:
                        {
                            FormCollection fc = Application.OpenForms;
                            bool isFormActive = false;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "GetSales")
                                {
                                    frm.Activate();
                                    isFormActive = true;
                                }
                            }

                            if (!isFormActive && db != null)
                            {
                                GetSales gsp = new GetSales(um, db);
                                gsp.Show();
                            }
                            e.Handled = true;
                        }
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
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetData();
        }
        #endregion

        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "input_qty")
                {
                    List<DataGridViewRow> list = new List<DataGridViewRow>();
                    list.Add(dgvProduct.Rows[e.RowIndex]);

                    db.InputProduct3(list, true, false);
                }
            }
        }
    }
}
