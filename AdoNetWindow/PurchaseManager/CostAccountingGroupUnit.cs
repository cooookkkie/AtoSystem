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

namespace AdoNetWindow.PurchaseManager
{
    public partial class CostAccountingGroupUnit : UserControl
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        CostAccountingGroup cag;
        double exchangeRate;
        double trqPrice;
        public int control_id;
        public CostAccountingGroupUnit(UsersModel uModel, CostAccountingGroup caGroup, double exRate, double trq_price)
        {
            InitializeComponent();
            um = uModel;
            cag = caGroup;
            exchangeRate = exRate;
            trqPrice = trq_price;
        }

        private void CostAccountingGroupUnit_Load(object sender, EventArgs e)
        {
            SetColumn();
        }

        #region Method
        public void SetMargin(bool isPurchasePrice, bool isPerBox)
        {
            this.isPurchasePrice = isPurchasePrice;
            if (this.isPurchasePrice)
            {
                dgvProduct.Columns["purchase_price1"].Visible = true;
                dgvProduct.Columns["domestic_sales_price1"].Visible = false;
                /*if (isPerBox)
                {
                    dgvProduct.Columns["per_box_purchase_price"].Visible = true;
                    dgvProduct.Columns["per_box_domestic_sales_price"].Visible = false;
                }*/
            }
            else
            {
                dgvProduct.Columns["purchase_price1"].Visible = false;
                dgvProduct.Columns["domestic_sales_price1"].Visible = true;
                /*if (isPerBox)
                {
                    dgvProduct.Columns["per_box_purchase_price"].Visible = false;
                    dgvProduct.Columns["per_box_domestic_sales_price"].Visible = true;
                }*/
            }

            calculate();
            calculateAssort();
        }
        public void SetPerbox(bool isPurchasePrice, bool isPerBox)
        {
            this.isPurchasePrice = isPurchasePrice;
            dgvProduct.Columns["per_box_cost_price"].Visible = isPerBox;
            if (this.isPurchasePrice)
            {
                dgvProduct.Columns["per_box_purchase_price"].Visible = isPerBox;
                dgvProduct.Columns["per_box_domestic_sales_price"].Visible = false;
            }
            else
            {
                dgvProduct.Columns["per_box_purchase_price"].Visible = false;
                dgvProduct.Columns["per_box_domestic_sales_price"].Visible = isPerBox;
            }
            calculate();
            calculateAssort();
        }
        public void Sorting(int sortType)
        {
            if (dgvProduct.Rows.Count > 0)
            { 
                DataTable dt = common.ConvertDgvToDataTable(dgvProduct);
                dt.Columns.Add("sizes_sort", typeof(double)).SetOrdinal(1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double sizes2;
                    if (dt.Rows[i]["sizes2"] == null || !double.TryParse(dt.Rows[i]["sizes2"].ToString(), out sizes2))
                        sizes2 = 0;
                    dt.Rows[i]["sizes_sort"] = sizes2;
                }
                DataView dv = new DataView(dt);
                switch (sortType)
                {
                    case 1:
                        dv.Sort = "product, origin, sizes_sort, sizes";
                        break;
                    case 2:
                        dv.Sort = "company, product, origin, sizes_sort, sizes";
                        break;
                    case 3:
                        dv.Sort = "product, origin, sizes_sort, sizes, unit_price";
                        break;
                    case 4:
                        dv.Sort = "unit_price, product, origin, sizes_sort, sizes";
                        break;
                    case 5:
                        dv.Sort = "product, origin, sizes_sort, sizes, updatetime";
                        break;
                    case 6:
                        dv.Sort = "updatetime, product, origin, sizes_sort, sizes";
                        break;
                }
                dt = dv.ToTable();

                dgvProduct.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = dgvProduct.Rows[n];

                    row.Cells["product"].Value = dt.Rows[i]["product"].ToString();
                    row.Cells["origin"].Value = dt.Rows[i]["origin"].ToString();
                    row.Cells["sizes"].Value = dt.Rows[i]["sizes"].ToString();
                    row.Cells["sizes2"].Value = dt.Rows[i]["sizes2"].ToString();
                    row.Cells["weight_calculate"].Value = Convert.ToBoolean(dt.Rows[i]["weight_calculate"].ToString());
                    row.Cells["box_weight"].Value = dt.Rows[i]["box_weight"].ToString();
                    row.Cells["cost_unit"].Value = dt.Rows[i]["cost_unit"].ToString();

                    row.Cells["updatetime"].Value = dt.Rows[i]["updatetime"].ToString();
                    row.Cells["unit_price"].Value = dt.Rows[i]["unit_price"].ToString();
                    row.Cells["company"].Value = dt.Rows[i]["company"].ToString();
                    row.Cells["exchange_rate"].Value = dt.Rows[i]["exchange_rate"].ToString();
                    row.Cells["custom"].Value = dt.Rows[i]["custom"].ToString();
                    row.Cells["tax"].Value = dt.Rows[i]["tax"].ToString();
                    row.Cells["incidental_expense"].Value = dt.Rows[i]["incidental_expense"].ToString();
                    row.Cells["purchase_price1"].Value = dt.Rows[i]["purchase_price1"].ToString();
                    row.Cells["domestic_sales_price1"].Value = dt.Rows[i]["domestic_sales_price1"].ToString();
                    row.Cells["assort"].Value = dt.Rows[i]["assort"].ToString();
                }
            }
        }
        public void SetExchangeRateTrq(double eRate, double trq)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            exchangeRate = eRate;
            trqPrice = trq;

            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            { 
                dgvProduct.Rows[i].Cells["exchange_rate"].Value = eRate.ToString("#,##0.00");
            }

            calculate();
            calculateAssort();
            calculateWeight();
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        public void SetId(int id)
        {
            control_id = id;
            lbTitle.Text = id.ToString();
        }
        public DataGridView GetDatagridview()
        {
            return dgvProduct;
        }

        public void GetTotal(out int total_weight, out int total_income_amount1, out int total_income_amount2)
        {
            if (!int.TryParse(txtTotalWeghit.Text, out total_weight))
                total_weight = 0;
            if (!int.TryParse(txtTotalWeghit.Text, out total_income_amount1))
                total_income_amount1 = 0;
            if (!int.TryParse(txtTotalWeghit.Text, out total_income_amount2))
                total_income_amount2 = 0;
        }
        private void SetColumn()
        {
            /*this.dgvProduct.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!*/
            this.dgvProduct.DeleteHighHeader();
            this.dgvProduct.Init(false);
            this.dgvProduct.DefaultCellStyle.Font = new Font("중고딕", 8, FontStyle.Regular);

            //Multi Header Setting
            this.dgvProduct.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            this.dgvProduct.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProduct.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            this.dgvProduct.AllowUserToResizeColumns = true;
            this.dgvProduct.AllowUserToResizeRows = false;
            /*this.dgvProduct.RowTemplate.Height = 23;
            this.dgvProduct.ColumnHeadersHeight = 46;  // 2줄 기준*/

            //첫번째 헤더
            /*this.dgvProduct.AddHighHeader(0, 5, "품목정보", "LightSteelBlue");
            this.dgvProduct.AddHighHeader(6, 13, "수입원가 및 마진율", "DarkOrange");
            this.dgvProduct.AddHighHeader(14, 19, "국내시세 및 마진율", "Gainsboro");
            this.dgvProduct.AddHighHeader(20, 22, "TRQ", "LightGreen");*/

            //두번째 헤더
            this.dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            this.dgvProduct.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["origin"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["sizes"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["weight_calculate"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["box_weight"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["box_weight"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["cost_unit"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvProduct.Columns["cost_unit"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            this.dgvProduct.Columns["updatetime"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["updatetime"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["company"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["company"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["unit_price"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["unit_price"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["custom"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["custom"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["tax"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["tax"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["incidental_expense"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            this.dgvProduct.Columns["incidental_expense"].DefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);

            this.dgvProduct.Columns["exchange_rate"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["exchange_rate"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["cost_price"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["cost_price"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["purchase_price1"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["purchase_price1"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["domestic_sales_price1"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["domestic_sales_price1"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);

            /*this.dgvProduct.Columns["sales_price"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["sales_price"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["sales_price"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);
            this.dgvProduct.Columns["purchase_price"].HeaderCell.Style.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["purchase_price"].DefaultCellStyle.BackColor = Color.FromArgb(216, 190, 190);
            this.dgvProduct.Columns["purchase_price"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);*/

            this.dgvProduct.Columns["trq"].HeaderCell.Style.BackColor = Color.FromArgb(226, 239, 218);
            this.dgvProduct.Columns["trq"].DefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218);


            //컬럼 넓이
            this.dgvProduct.Columns["incidental_expense"].HeaderCell.Style.Font = new Font("중고딕", 7, FontStyle.Regular);

            this.dgvProduct.Columns["cost_price"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);
            this.dgvProduct.Columns["unit_price"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);
            this.dgvProduct.Columns["trq"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Bold);
            this.dgvProduct.Columns["trq_margin"].DefaultCellStyle.Font = new Font("중고딕", 7, FontStyle.Regular);


            //Alingment
            dgvProduct.Columns["box_weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            for (int i = 3; i < dgvProduct.ColumnCount; i++)
            {
                if (dgvProduct.Columns[i].Name != "company" && dgvProduct.Columns[i].Name != "updatetime")
                    dgvProduct.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            this.dgvProduct.ColumnHeadersDefaultCellStyle.Font = new Font("중고딕", 9, FontStyle.Bold);

            dgvProduct.Columns["sizes2"].Visible = false;
        }

        bool isPurchasePrice = true;
        public void SetProduct(List<DataGridViewRow> product, bool isPurchasePrice)
        {
            this.isPurchasePrice = isPurchasePrice;
            if (isPurchasePrice)
            {
                dgvProduct.Columns["purchase_price1"].Visible = true;
                dgvProduct.Columns["domestic_sales_price1"].Visible = false;
            }
            else
            {
                dgvProduct.Columns["purchase_price1"].Visible = false;
                dgvProduct.Columns["domestic_sales_price1"].Visible = true;
            }
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            for (int i = 0; i < product.Count; i++)
            {
                int n = this.dgvProduct.Rows.Add();
                DataGridViewRow row = this.dgvProduct.Rows[n];
                DataGridViewRow copyRow = product[i];

                row.Cells["product"].Value = copyRow.Cells["product"].Value.ToString();
                row.Cells["origin"].Value = copyRow.Cells["origin"].Value.ToString();
                row.Cells["sizes"].Value = copyRow.Cells["sizes"].Value.ToString();
                double sizes2;
                if (copyRow.Cells["sizes2"].Value == null || !double.TryParse(copyRow.Cells["sizes2"].Value.ToString(), out sizes2))
                    row.Cells["sizes2"].Value = copyRow.Cells["sizes2"].Value;
                else
                    row.Cells["sizes2"].Value = sizes2;
                bool weight_calculate = Convert.ToBoolean(copyRow.Cells["weight_calculate"].Value);
                row.Cells["weight_calculate"].Value = weight_calculate;
                row.Cells["box_weight"].Value = copyRow.Cells["box_weight"].Value.ToString();
                row.Cells["cost_unit"].Value = copyRow.Cells["cost_unit"].Value.ToString();
                row.Cells["updatetime"].Value = copyRow.Cells["updatetime"].Value.ToString();
                row.Cells["unit_price"].Value = copyRow.Cells["unit_price"].Value.ToString();
                row.Cells["company"].Value = copyRow.Cells["company"].Value.ToString();
                row.Cells["exchange_rate"].Value = exchangeRate.ToString("#,##0.00");
                row.Cells["custom"].Value = copyRow.Cells["custom"].Value.ToString();
                row.Cells["tax"].Value = copyRow.Cells["tax"].Value.ToString();
                row.Cells["incidental_expense"].Value = copyRow.Cells["incidental_expense"].Value.ToString();
                row.Cells["fixed_tariff"].Value = copyRow.Cells["fixed_tariff"].Value.ToString();

                row.Cells["purchase_price1"].Value = copyRow.Cells["purchase_price1"].Value.ToString();
                row.Cells["domestic_sales_price1"].Value = copyRow.Cells["domestic_sales_price1"].Value.ToString();
            }
            calculate();
            calculateAssort();
            calculateWeight();

            if (dgvProduct.Rows.Count > 0)
            {
                int height = (dgvProduct.Rows.Count * 23) + 40 + 52;
                this.Height = height;
            }

            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }

        private void calculate()
        {
            dgvProduct.EndEdit();
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    // Null Value change
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value == null || cell.Value.ToString() == "")
                            cell.Value = 0;
                    }
                    double purchase_price = Convert.ToDouble(row.Cells["unit_price"].Value);
                    double custom = Convert.ToDouble(row.Cells["custom"].Value) / 100;
                    double tax = Convert.ToDouble(row.Cells["tax"].Value) / 100;
                    double incidental_expense = Convert.ToDouble(row.Cells["incidental_expense"].Value) / 100;
                    double fixed_tariff;
                    if (row.Cells["fixed_tariff"].Value == null || !double.TryParse(row.Cells["fixed_tariff"].Value.ToString(), out fixed_tariff))
                        fixed_tariff = 0;
                    fixed_tariff /= 1000;
                    double box_weight;
                    string txt = row.Cells["box_weight"].Value.ToString().Replace("벌크", "").Trim();
                    if (!double.TryParse(txt, out box_weight))
                        box_weight = 0;
                    double cost_unit = Convert.ToDouble(row.Cells["cost_unit"].Value);
                    //계산방식
                    double unit;
                    if (Convert.ToBoolean(row.Cells["weight_calculate"].Value))
                        unit = box_weight;
                    else
                        unit = cost_unit;

                    //환율
                    double exchange_rate = Convert.ToDouble(row.Cells["exchange_rate"].Value);
                    //원가계산
                    double cost_price = unit * purchase_price * exchange_rate;
                    if (fixed_tariff > 0)
                    {
                        cost_price += (fixed_tariff * unit * exchange_rate * custom);
                        if (tax > 0)
                            cost_price *= (tax + 1);
                        if (incidental_expense > 0)
                            cost_price *= (incidental_expense + 1);
                    }
                    else
                    {
                        if (custom > 0)
                            cost_price *= (custom + 1);
                        if (tax > 0)
                            cost_price *= (tax + 1);
                        if (incidental_expense > 0)
                            cost_price *= (incidental_expense + 1);
                    }
                    //원가
                    row.Cells["cost_price"].Value = cost_price.ToString("#,##0");
                    //TRQ
                    double trq = trqPrice;
                    double sales_price;
                    if (isPurchasePrice)
                    {
                        if (row.Cells["purchase_price1"].Value == null || !double.TryParse(row.Cells["purchase_price1"].Value.ToString(), out sales_price))
                            sales_price = 0;
                    }
                    else
                    {
                        if (row.Cells["domestic_sales_price1"].Value == null || !double.TryParse(row.Cells["domestic_sales_price1"].Value.ToString(), out sales_price))
                            sales_price = 0;
                    }

                    double margin_price = 0;
                    double margin_rate = 0;
                    margin_price = sales_price - cost_price;
                    margin_rate = ((sales_price - cost_price) / sales_price) * 100;

                    if (double.IsNaN(margin_rate))
                        margin_rate = 0;
                    row.Cells["margin_rate"].Value = margin_rate.ToString("#,##0.00") + "%";


                    //TRQ
                    double trq_price = (purchase_price * unit * (1 + tax + incidental_expense) * exchange_rate) + (box_weight * trq);
                    row.Cells["trq"].Value = trq_price.ToString("#,##0");

                    //TRQ 마진
                    double trq_margin = (((sales_price - Convert.ToDouble(row.Cells["trq"].Value.ToString().Replace(",", ""))) / sales_price) * 100);
                    row.Cells["trq_margin"].Value = trq_margin.ToString("#,##0.00") + "%";
                }
            }
        }
        private void calculateAssort()
        {
            dgvProduct.EndEdit();
            if (dgvProduct.Rows.Count > 0)
            {
                double total_assort_margin1 = 0;
                double total_assort_margin2 = 0;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    double assort;
                    if (row.Cells["assort"].Value == null || !double.TryParse(row.Cells["assort"].Value.ToString(), out assort))
                        assort = 0;

                    double cost_price;
                    if (row.Cells["cost_price"].Value == null || !double.TryParse(row.Cells["cost_price"].Value.ToString(), out cost_price))
                        cost_price = 0;

                    double domestic_sales_price1;
                    if (isPurchasePrice)
                    {
                        if (row.Cells["purchase_price1"].Value == null || !double.TryParse(row.Cells["purchase_price1"].Value.ToString(), out domestic_sales_price1))
                            domestic_sales_price1 = 0;
                    }
                    else
                    {
                        if (row.Cells["domestic_sales_price1"].Value == null || !double.TryParse(row.Cells["domestic_sales_price1"].Value.ToString(), out domestic_sales_price1))
                            domestic_sales_price1 = 0;
                    }

                    double trq;
                    if (row.Cells["trq"].Value == null || !double.TryParse(row.Cells["trq"].Value.ToString(), out trq))
                        trq = 0;

                    //마진1
                    double assort_margin1 = domestic_sales_price1 - cost_price;
                    total_assort_margin1 += (assort_margin1 * assort);
                    row.Cells["income_amount1"].Value = (assort_margin1 * assort).ToString("#,##0");
                    //마진2
                    double assort_margin2 = domestic_sales_price1 - trq;
                    //row.Cells["assort_margin2"].Value = assort_margin2.ToString("#,##0");
                    total_assort_margin2 += (assort_margin2 * assort);
                    row.Cells["income_amount2"].Value = (assort_margin2 * assort).ToString("#,##0");
                }

                if (double.IsNaN(total_assort_margin1))
                    total_assort_margin1 = 0;
                if (double.IsNaN(total_assort_margin2))
                    total_assort_margin2 = 0;

                txtTotalAssortMargin1.Text = total_assort_margin1.ToString("#,##0");
                txtTotalAssortMargin2.Text = total_assort_margin2.ToString("#,##0");
            }
        }
        private void calculateWeight()
        {
            dgvProduct.EndEdit();
            double total_weight = 0;
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                DataGridViewRow row = dgvProduct.Rows[i];
                double assort;
                if (row.Cells["assort"].Value == null || !double.TryParse(row.Cells["assort"].Value.ToString(), out assort))
                    assort = 0;
                double box_weight;
                if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                    box_weight = 1;
                total_weight += assort * box_weight;
            }
            txtTotalWeghit.Text = total_weight.ToString("#,##0");

            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                DataGridViewRow row = dgvProduct.Rows[i];
                double box_weight;
                if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                    box_weight = 1;

                double assort;
                if (row.Cells["assort"].Value == null || !double.TryParse(row.Cells["assort"].Value.ToString(), out assort))
                    assort = 0;

                //비율
                row.Cells["assort_weight"].Value = (assort * box_weight).ToString("#,##0.00");
                double weight_rate = (assort * box_weight) / total_weight * 100;
                if(double.IsNaN(weight_rate))
                    weight_rate = 0;
                row.Cells["weight_rate"].Value = weight_rate.ToString("#,##0.00") + "%";
            }
        }

        public void setPaste(bool paste)
        {
            this.dgvProduct.setPaste(paste);
        }
        #endregion

        #region Key event
        private void CostAccountingGroupUnit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        this.dgvProduct.setPaste();
                        cag.clipboard = new List<DataGridViewRow>();
                        break;
                    case Keys.V:
                        if (cag.clipboard.Count > 0)
                        {
                            this.dgvProduct.setPaste(false);
                            SetProduct(cag.clipboard, isPurchasePrice);
                        }
                        break;
                }
            }
        }
        private void dgvProduct_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        this.dgvProduct.setPaste();
                        cag.clipboard = new List<DataGridViewRow>();
                        break;
                        cag.clipboard = new List<DataGridViewRow>();
                    case Keys.V:
                        if (cag.clipboard.Count > 0)
                            SetProduct(cag.clipboard, isPurchasePrice);
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnClose_Click(object sender, EventArgs e)
        {
            cag.DeleteControl(this);
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            dgvProduct.EndEdit();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "assort")
                {
                    calculateAssort();
                    calculateWeight();
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "weight_calculate")
                {
                    calculate();
                    calculateAssort();
                    calculateWeight();
                }
                else
                {
                    calculate();
                    calculateAssort();
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvProduct.EndEdit();
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "weight_calculate")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !isChecked;
                    calculate();
                    calculateAssort();
                }
            }
        }

        #endregion

    }
}
