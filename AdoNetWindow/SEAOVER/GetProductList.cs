using AdoNetWindow.Model;
using AdoNetWindow.SEAOVER._2Line;
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

namespace AdoNetWindow.SEAOVER
{
    public partial class GetProductList : Form
    {
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        _2LineForm form = null;
        SimpleHandlingFormManager.SimpleHandlingFormManager simpleForm = null;
        OneLine.OneLineForm olf = null;
        UsersModel um;
        public GetProductList(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }
        public GetProductList(UsersModel uModel, _2LineForm LineForm)
        {
            InitializeComponent();
            form = LineForm;
            um = uModel;
        }
        public GetProductList(UsersModel uModel, SimpleHandlingFormManager.SimpleHandlingFormManager form)
        {
            InitializeComponent();
            simpleForm = form;
            um = uModel;
        }
        public GetProductList(UsersModel uModel, OneLine.OneLineForm olForm)
        {
            InitializeComponent();
            olf = olForm;
            um = uModel;
        }

        private void GetProductList_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.KeyPreview = true;
            Init_DataGridView();
            //업체별시세현황 스토어프로시져 호출
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this, "호출 내용이 없음");
                return;
            }
        }

        #region Method
        private void Init_DataGridView()
        {
            dgvProduct.DoubleBuffered(true);
        }
        private void CallProcedure()
        {
            //업체별시세현황 스토어프로시져 호출
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this, "호출 내용이 없음");
                return;
            }


            //품명별재고현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                if (priceComparisonRepository.CallStoredProcedureSTOCK(user_id, eDate) == 0)
                    MessageBox.Show(this, "호출 내용이 없음");
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
            }

        }
        private void GetProduct()
        {
            CallProcedure();
            dgvProduct.DataSource = null;
            int salePriceFilter = 1;
            if(rbIncrease.Checked)
                salePriceFilter = 2;
            else if (rbDecrease.Checked)
                salePriceFilter = 3;
            DataTable list = list = seaoverRepository.GetCurrentProduct(txtCategoryCode.Text, txtCategory.Text, txtProduct.Text, txtOrigin.Text, txtSizes.Text
                                                    , txtUnit.Text, txtDivision.Text, txtManager.Text, ""
                                                    , cbCategory.Checked, cbPrice.Checked, cbMinPrice.Checked, cbVat.Text, cbIsStock.Checked, salePriceFilter);

            if (list.Rows.Count > 0)
            {
                //품목서 기타정보
                DataTable produtOtherDt = productOtherCostRepository.GetProduct(txtProduct.Text, false, txtOrigin.Text, txtSizes.Text, "");
                if (produtOtherDt.Rows.Count > 0)
                {
                    list.Columns["트레이"].ReadOnly = false;
                    for (int i = 0; i < list.Rows.Count; i++)
                    {
                        string whr = "product = '" + list.Rows[i]["품명"].ToString() + "'"
                            + " AND origin = '" + list.Rows[i]["원산지"].ToString() + "'"
                            + " AND sizes = '" + list.Rows[i]["규격"].ToString() + "'";
                        DataRow[] dr = produtOtherDt.Select(whr);
                        if (dr.Length > 0)
                            list.Rows[i]["트레이"] = dr[0]["cost_unit"].ToString();
                    }
                    list.AcceptChanges();
                }

                //선적재고
                DataTable shippingDt = customsRepository.GetShippingStockList();
                if (shippingDt.Rows.Count > 0)
                {
                    list.Columns["선적수"].ReadOnly = false;
                    list.Columns["선적수"].MaxLength = 10;
                    for (int i = 0; i < list.Rows.Count; i++)
                    {
                        string whr = "product = '" + list.Rows[i]["품명"].ToString() + "'"
                            + " AND origin = '" + list.Rows[i]["원산지"].ToString() + "'"
                            + " AND sizes = '" + list.Rows[i]["규격"].ToString() + "'"
                            + " AND box_weight = '" + list.Rows[i]["SEAOVER단위"].ToString() + "'";
                        DataRow[] dr = shippingDt.Select(whr);
                        if (dr.Length > 0)
                            list.Rows[i]["선적수"] = dr[0]["qty"].ToString();
                    }
                    list.AcceptChanges();
                }

                if (cbIsStock.Checked)
                {
                    for (int i = list.Rows.Count - 1; i >= 0; i--)
                    {
                        double stock;
                        if (!double.TryParse(list.Rows[i]["재고수"].ToString(), out stock)) stock = 0;

                        double reserved;
                        if (!double.TryParse(list.Rows[i]["예약수"].ToString(), out reserved)) reserved = 0;

                        double shipping;
                        if (!double.TryParse(list.Rows[i]["선적수"].ToString(), out shipping)) shipping = 0;

                        if (stock - reserved + shipping <= 0)
                            list.Rows.Remove(list.Rows[i]);
                    }
                    list.AcceptChanges();
                }
                //창고검색
                if (list != null && list.Rows.Count > 0 && !string.IsNullOrEmpty(txtWarehouse.Text.Trim()))
                {
                    DataRow[] dr = list.Select($"창고 LIKE '%{txtWarehouse.Text.Trim()}%'");
                    if (dr.Length > 0)
                        list = dr.CopyToDataTable();
                    list.AcceptChanges();
                }


                dgvProduct.DataSource = list;
                SetHeaderStyle();
            }
        }
        private string GetHandlingProductTxt()
        {
            string HandlingProductTxt = "";
            if (dgvProduct.SelectedRows.Count > 0)
            {
                string headerTxt = "(광고)(주)아토무역\n";
                string titleTxt = DateTime.Now.ToString("yyyy.MM") + " (주)아토무역 취급품목서 \n";
                string managerTxt = um.tel + " " + um.user_name + " " + um.grade + "\n";
                string footerTxt = "\n\n수신을 원하지 않을경우\n문자나 전화 주시면 발송을 안하도록 하겠습니다.\n-------------------\n무료수신거부\n080-855-8825";

                int idx = dgvProduct.SelectedRows.Count - 1;
                string categoryTxt = dgvProduct.SelectedRows[idx].Cells["category"].Value.ToString();

                string weight = "";
                DataGridViewRow row = dgvProduct.SelectedRows[idx];
                //중량
                if (row.Cells["price_unit"].Value.ToString() == "팩")
                {
                    if (Convert.ToDouble(row.Cells["unit"].Value.ToString()) < 1)
                    {
                        weight = row.Cells["seaover_unit"].Value.ToString() + "kg \n (" + (Convert.ToDouble(row.Cells["unit"].Value.ToString()) * 1000).ToString() + "g x " + row.Cells["unit_count"].Value.ToString() + "p)";
                    }
                    else
                    {
                        weight = row.Cells["seaover_unit"].Value.ToString() + "kg \n (" + row.Cells["unit"].Value.ToString() + "k x " + row.Cells["unit_count"].Value.ToString() + "p)";
                    }
                }
                //kg
                else if (row.Cells["price_unit"].Value.ToString() == "kg" || row.Cells["price_unit"].Value.ToString() == "Kg" || row.Cells["price_unit"].Value.ToString() == "KG")
                {

                    if (Convert.ToDouble(row.Cells["unit"].Value.ToString()) < 1)
                    {
                        weight = row.Cells["seaover_unit"].Value.ToString() + "kg \n (" + (Convert.ToDouble(row.Cells["unit"].Value.ToString()) * 1000).ToString() + "g x " + row.Cells["unit_count"].Value.ToString() + "p)";
                    }
                    else
                    {
                        weight = row.Cells["seaover_unit"].Value.ToString() + "kg \n (" + row.Cells["unit"].Value.ToString() + "k x " + row.Cells["unit_count"].Value.ToString() + "p)";
                    }
                }
                //묶음
                else if (row.Cells["price_unit"].Value.ToString() == "묶" || row.Cells["price_unit"].Value.ToString() == "묶음")
                {
                    weight = row.Cells["unit"].Value.ToString() + "kg";
                }
                //나머지
                else
                {
                    weight = row.Cells["seaover_unit"].Value.ToString() + "kg";
                }
                weight = weight.Replace("\n", "").Replace("\r", "");


                string productTxt = "♧ " + dgvProduct.SelectedRows[idx].Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "") 
                    + " " + weight
                    + "(" + dgvProduct.SelectedRows[idx].Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "") + ")" + " ♧";
                string tempTxt = "-------------\n【" + categoryTxt + "】\n-------------\n\n" + productTxt;

                for (int i = dgvProduct.SelectedRows.Count - 1; i >= 0; i--)
                {
                    row = dgvProduct.SelectedRows[i];

                    if (row.Cells["category"].Value != null && row.Cells["product"].Value != null)
                    {
                        //중량
                        if (row.Cells["price_unit"].Value.ToString() == "팩")
                        {
                            if (Convert.ToDouble(row.Cells["unit"].Value.ToString()) < 1)
                            {
                                weight = row.Cells["seaover_unit"].Value.ToString() + "kg \n (" + (Convert.ToDouble(row.Cells["unit"].Value.ToString()) * 1000).ToString() + "g x " + row.Cells["unit_count"].Value.ToString() + "p)";
                            }
                            else
                            {
                                weight = row.Cells["seaover_unit"].Value.ToString() + "kg \n (" + row.Cells["unit"].Value.ToString() + "k x " + row.Cells["unit_count"].Value.ToString() + "p)";
                            }
                        }
                        //kg
                        else if (row.Cells["price_unit"].Value.ToString() == "kg" || row.Cells["price_unit"].Value.ToString() == "Kg" || row.Cells["price_unit"].Value.ToString() == "KG")
                        {

                            if (Convert.ToDouble(row.Cells["unit"].Value.ToString()) < 1)
                            {
                                weight = row.Cells["seaover_unit"].Value.ToString() + "kg \n (" + (Convert.ToDouble(row.Cells["unit"].Value.ToString()) * 1000).ToString() + "g x " + row.Cells["unit_count"].Value.ToString() + "p)";
                            }
                            else
                            {
                                weight = row.Cells["seaover_unit"].Value.ToString() + "kg \n (" + row.Cells["unit"].Value.ToString() + "k x " + row.Cells["unit_count"].Value.ToString() + "p)";
                            }
                        }
                        //묶음
                        else if (row.Cells["price_unit"].Value.ToString() == "묶" || row.Cells["price_unit"].Value.ToString() == "묶음")
                        {
                            weight = row.Cells["unit"].Value.ToString() + "kg";
                        }
                        //나머지
                        else
                        {
                            weight = row.Cells["seaover_unit"].Value.ToString() + "kg";
                        }
                        weight = weight.Replace("\n", "").Replace("\r", "");

                        //Text
                        if (categoryTxt != row.Cells["category"].Value.ToString())
                        {
                            HandlingProductTxt += tempTxt;
                            categoryTxt = dgvProduct.SelectedRows[i].Cells["category"].Value.ToString();
                            productTxt = "♧ " + dgvProduct.SelectedRows[i].Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "") 
                                + " " + weight
                                + "(" + dgvProduct.SelectedRows[i].Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "") + ")" + " ♧";
                            tempTxt = "\n\n-------------\n【" + categoryTxt + "】\n-------------\n\n" + productTxt + "\n" + row.Cells["sizes"].Value.ToString().Replace("\n", "").Replace("\r", "") + " @ " + row.Cells["sales_price"].Value;
                        }
                        else if (productTxt != "♧ " + dgvProduct.SelectedRows[i].Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "") 
                            + " " + weight
                            + "(" + dgvProduct.SelectedRows[i].Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "") + ")" + " ♧")
                        {
                            productTxt = "♧ " + dgvProduct.SelectedRows[i].Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                + " " + weight
                                + "(" + dgvProduct.SelectedRows[i].Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "") + ")" + " ♧";
                            tempTxt += "\n\n" + productTxt + "\n" + row.Cells["sizes"].Value.ToString().Replace("\n", "").Replace("\r", "") + " @ " + row.Cells["sales_price"].Value;
                        }
                        else
                        {
                            tempTxt += "\n" + row.Cells["sizes"].Value.ToString().Replace("\n", "").Replace("\r", "") + " @ " + row.Cells["sales_price"].Value;
                        }
                    }
                }
                HandlingProductTxt += tempTxt;
                HandlingProductTxt = headerTxt + titleTxt + managerTxt + HandlingProductTxt + footerTxt;
            }

            return HandlingProductTxt;
        }
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvProduct;

            //헤더 디자인
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.RosyBrown;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["대분류1"].Visible = false;
            dgv.Columns["대분류2"].Visible = false;
            dgv.Columns["대분류3"].Visible = false;
            dgv.Columns["원산지코드"].Visible = false;
            dgv.Columns["품목코드"].Visible = false;
            dgv.Columns["규격코드"].Visible = false;
            dgv.Columns["규격2"].Visible = false;
            dgv.Columns["규격3"].Visible = false;
            dgv.Columns["규격4"].Visible = false;
            dgv.Columns["창고"].Visible = false;

            dgv.Columns["대분류"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["대분류"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["대분류"].Width = 80;
            dgv.Columns["대분류"].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 153);

            dgv.Columns["원산지"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["원산지"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["원산지"].Width = 80;

            dgv.Columns["품명"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["품명"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["품명"].Width = 200;
            dgv.Columns["품명"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            //dgv.Columns["품명"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv.Columns["규격"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["규격"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["규격"].Width = 100;

            dgv.Columns["단위"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["단위"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["단위"].Width = 50;

            dgv.Columns["가격단위"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["가격단위"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["가격단위"].Width = 50;

            dgv.Columns["단위수량"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["단위수량"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["단위수량"].Width = 50;

            dgv.Columns["SEAOVER단위"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["SEAOVER단위"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["SEAOVER단위"].Width = 50;

            dgv.Columns["트레이"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["트레이"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["트레이"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["트레이"].Width = 50;

            dgv.Columns["매출단가"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["매출단가"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["매출단가"].Width = 100;
            dgv.Columns["매출단가"].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgv.Columns["매출단가"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);

            dgv.Columns["담당자1"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["담당자1"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["담당자1"].Width = 70;

            dgv.Columns["구분"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["구분"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["구분"].Width = 50;

            dgv.Columns["단가수정일"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["단가수정일"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["단가수정일"].Width = 100;
            dgv.Columns["단가수정일"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["단가수정일"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);

            dgv.Columns["비고1"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["비고1"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["비고1"].Width = 100;

            dgv.Columns["부가세"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["부가세"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["부가세"].Width = 50;

            dgv.Columns["재고수"].Width = 50;
            dgv.Columns["예약수"].Width = 50;
            dgv.Columns["선적수"].Width = 50;
            dgv.Columns["일반시세"].Width = 80;

            //dgv.Columns["매입단가"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["매출단가"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["매출단가"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["일반시세"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["일반시세"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["예약수"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["예약수"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["재고수"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["재고수"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["선적수"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["선적수"].DefaultCellStyle.Format = "#,##0";

            //재고현황 Tooltip
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells["재고수"].ToolTipText = dgv.Rows[i].Cells["창고"].Value.ToString().Replace(@"\n", "\n").Trim();
                dgv.Rows[i].Cells["예약수"].ToolTipText = dgv.Rows[i].Cells["창고"].Value.ToString().Replace(@"\n", "\n").Trim();
            }

        }

        private void CopyToClipboard()
        {
            dgvProduct.EndEdit();
            List<SeaoverCopyModel> clipboardModel = new List<SeaoverCopyModel>();
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            if (dgvProduct.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                {
                    if (row.Index < dgvProduct.Rows.Count)
                    {
                        SeaoverCopyModel model = new SeaoverCopyModel();
                        model.category = row.Cells["대분류"].Value.ToString();
                        if (row.Cells["대분류1"].Value != null && !string.IsNullOrEmpty(row.Cells["대분류1"].Value.ToString()))
                        { 
                            model.category1 = row.Cells["대분류1"].Value.ToString().Substring(0, 1);

                            int d;
                            if (!int.TryParse(row.Cells["대분류1"].Value.ToString().Substring(1, 1), out d))
                            {
                                model.category2 = row.Cells["대분류1"].Value.ToString().Substring(1, 1);
                            }

                            if (row.Cells["대분류1"].Value.ToString().Length <= 5)
                                model.category3 = row.Cells["대분류1"].Value.ToString().Substring(row.Cells["대분류1"].Value.ToString().Length - 3, 3);
                            else if(model.category2 != null && !string.IsNullOrEmpty(model.category2))
                                model.category3 = row.Cells["대분류1"].Value.ToString().Substring(2, 3);
                            else
                                model.category3 = row.Cells["대분류1"].Value.ToString().Substring(1, 3);


                            model.category_code = row.Cells["대분류1"].Value.ToString();
                        }
                        model.product_code = row.Cells["품목코드"].Value.ToString();
                        model.product = row.Cells["품명"].Value.ToString();
                        model.origin_code = row.Cells["원산지코드"].Value.ToString();
                        model.origin = row.Cells["원산지"].Value.ToString();
                        model.sizes_code = row.Cells["규격코드"].Value.ToString();
                        model.sizes = row.Cells["규격"].Value.ToString();
                        model.unit = row.Cells["단위"].Value.ToString();
                        model.unit_count = row.Cells["단위수량"].Value.ToString();
                        model.price_unit = row.Cells["가격단위"].Value.ToString();
                        model.seaover_unit = row.Cells["SEAOVER단위"].Value.ToString();
                        model.sales_price = Convert.ToDouble(row.Cells["매출단가"].Value.ToString()).ToString("#,##0");
                        model.purchase_price = "0";
                        model.division = row.Cells["구분"].Value.ToString();
                        model.edit_date = row.Cells["단가수정일"].Value.ToString();
                        model.manager1 = row.Cells["담당자1"].Value.ToString();
                        model.cnt = "";
                        model.page = "";
                        model.area = "";
                        model.row = "";

                        //중량
                        if (row.Cells["가격단위"].Value.ToString() == "팩")
                        {
                            if (Convert.ToDouble(row.Cells["단위"].Value) < 1)
                            {
                                model.weight = model.seaover_unit + "kg \n (" + (Convert.ToDouble(model.unit) * 1000).ToString() + "g x " + model.unit_count + "p)";
                            }
                            else
                            {
                                model.weight = model.seaover_unit + "kg \n (" + model.unit.ToString() + "k x " + model.unit_count + "p)";
                            }
                        }
                        //kg
                        else if (row.Cells["가격단위"].Value.ToString() == "kg" || row.Cells["가격단위"].Value.ToString() == "Kg" || row.Cells["가격단위"].Value.ToString() == "KG")
                        {

                            if (Convert.ToDouble(row.Cells["단위"].Value) < 1)
                            {
                                model.weight = model.seaover_unit + "kg \n (" + (Convert.ToDouble(model.unit) * 1000).ToString() + "g x " + model.unit_count + "p)";
                            }
                            else
                            {
                                model.weight = model.seaover_unit + "kg \n (" + model.unit.ToString() + "k x " + model.unit_count + "p)";
                            }
                        }
                        //묶음
                        else if (row.Cells["가격단위"].Value.ToString() == "묶" || row.Cells["가격단위"].Value.ToString() == "묶음")
                        {
                            model.weight = model.unit + "kg";
                        }
                        //나머지
                        else
                        {
                            model.weight = model.seaover_unit + "kg";
                        }
                        model.is_tax = row.Cells["부가세"].Value.ToString();

                        clipboardModel.Add(model);

                        rowList.Add(row);
                    }
                }
                if(form != null)
                    form.SetClipboardModel(clipboardModel);
                if(simpleForm != null)
                    simpleForm.SetClipboardModel(clipboardModel);
                if(olf != null)
                    olf.SetClipboardModel(rowList);
            }
        }

        private void InsertProduct() 
        {
            List<SeaoverPriceModel> list = new List<SeaoverPriceModel>();

            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
            {
                SeaoverPriceModel model = new SeaoverPriceModel();
                model.category = row.Cells["category"].Value.ToString();
                model.category_code = row.Cells["category_code"].Value.ToString();
                model.product_code = row.Cells["product_code"].Value.ToString();
                model.product = row.Cells["product"].Value.ToString();
                model.origin_code = row.Cells["origin_code"].Value.ToString();
                model.origin = row.Cells["origin"].Value.ToString();
                model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                model.sizes = row.Cells["sizes"].Value.ToString();
                model.unit = row.Cells["unit"].Value.ToString();
                model.unit_count = row.Cells["unit_count"].Value.ToString();
                model.price_unit = row.Cells["price_unit"].Value.ToString();
                model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                model.sales_price = Convert.ToDouble(row.Cells["sales_price"].Value.ToString());
                model.purchase_price = Convert.ToDouble(row.Cells["purchase_price"].Value.ToString());
                model.seaover_unit = row.Cells["SEAOVER단위"].Value.ToString();
                model.division = row.Cells["division"].Value.ToString();

                list.Add(model);
            }

            if (list.Count > 0)
            {
                if (form != null)
                    form.SetProduct(list);
            }
        }
        #endregion

        #region 우클릭 메뉴
        private void dgvProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("선택추가");
                    m.Items.Add("복사하기(C)");
                    m.Items.Add("문자용 텍스트 복사");
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.Show(dgvProduct, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch {}
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            dgvProduct.EndEdit();
            switch (e.ClickedItem.Text)
            {
                case "선택추가":
                    if(dgvProduct.SelectedRows.Count > 0)
                    {
                        List<DataGridViewRow> rowList = new List<DataGridViewRow>();
                        List<SeaoverPriceModel> list = new List<SeaoverPriceModel>();
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            SeaoverPriceModel model = new SeaoverPriceModel();
                            model.category = row.Cells["대분류"].Value.ToString();
                            model.category_code = row.Cells["대분류1"].Value.ToString();
                            model.product_code = row.Cells["품목코드"].Value.ToString();
                            model.product = row.Cells["품명"].Value.ToString();
                            model.origin_code = row.Cells["원산지코드"].Value.ToString();
                            model.origin = row.Cells["원산지"].Value.ToString();
                            model.sizes_code = row.Cells["규격코드"].Value.ToString();
                            model.sizes = row.Cells["규격"].Value.ToString();
                            model.unit = row.Cells["단위"].Value.ToString();
                            model.unit_count = row.Cells["단위수량"].Value.ToString();
                            model.price_unit = row.Cells["가격단위"].Value.ToString();
                            model.seaover_unit = row.Cells["SEAOVER단위"].Value.ToString();
                            model.sales_price = Convert.ToDouble(row.Cells["매출단가"].Value.ToString());
                            model.purchase_price = 0;
                            model.division = row.Cells["구분"].Value.ToString();
                            model.edit_date = row.Cells["단가수정일"].Value.ToString();
                            model.manager1 = row.Cells["담당자1"].Value.ToString();
                            model.is_tax = row.Cells["부가세"].Value.ToString();
                            list.Add(model);
                            rowList.Add(row);
                        }

                        if (list.Count > 0)
                        {
                            if (form != null)
                                form.SetProduct(list);
                            else if (olf != null)
                            {
                                olf.SetClipboardModel(rowList);
                                olf.PasteClipboard();
                            }
                            else if (simpleForm != null)
                            {
                                simpleForm.InputSeaoverProduct(list);
                            }
                        }
                    }
                    break;
                case "복사하기(C)":
                    CopyToClipboard();
                    break;
                case "문자용 텍스트 복사":
                    Clipboard.SetText(GetHandlingProductTxt());
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Key, Button event Handler
        private void GetProductList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        {
                            GetProduct();
                            break;
                        }
                    case Keys.N:
                        {
                            txtCategoryCode.Text = "";
                            txtCategory.Text = "";
                            txtProduct.Text = "";
                            txtOrigin.Text = "";
                            txtSizes.Text = "";
                            txtUnit.Text = "";
                            txtManager.Text = "";
                            txtWarehouse.Text = "";
                            txtProduct.Focus();
                            break;
                        }
                    case Keys.M:
                        {
                            txtProduct.Focus();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        {
                            cbCategory.Checked = !cbCategory.Checked;
                            break;
                        }
                    case Keys.F2:
                        {
                            cbPrice.Checked = !cbPrice.Checked;
                            break;
                        }
                    case Keys.F3:
                        {
                            cbMinPrice.Checked = !cbMinPrice.Checked;
                            break;
                        }
                    case Keys.F4:
                        {
                            cbIsStock.Checked = !cbIsStock.Checked;
                            break;
                        }
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertProduct();
        }
        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }
        private void txtCategoryCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }

        private void txtOrigin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }

        private void txtSizes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }

        private void txtDivision_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }

        private void txtManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }

        private void txtUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }

        private void dgvProduct_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            CopyToClipboard();
                            break;
                    }
                }
            }
            catch { }
        }


        #endregion

        #region Datagridview event

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if(dgvProduct.SelectedRows.Count == 1)
                    dgvProduct.ClearSelection();
                dgvProduct.Rows[e.RowIndex].Selected = true;
            }
        }
        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                List<SeaoverPriceModel> list = new List<SeaoverPriceModel>();
                SeaoverPriceModel model = new SeaoverPriceModel();
                model.category = row.Cells["대분류"].Value.ToString();
                model.category_code = row.Cells["대분류1"].Value.ToString();
                model.product_code = row.Cells["품목코드"].Value.ToString();
                model.product = row.Cells["품명"].Value.ToString();
                model.origin_code = row.Cells["원산지코드"].Value.ToString();
                model.origin = row.Cells["원산지"].Value.ToString();
                model.sizes_code = row.Cells["규격코드"].Value.ToString();
                model.sizes = row.Cells["규격"].Value.ToString();
                model.unit = row.Cells["단위"].Value.ToString();
                model.unit_count = row.Cells["단위수량"].Value.ToString();
                model.price_unit = row.Cells["가격단위"].Value.ToString();
                model.seaover_unit = row.Cells["SEAOVER단위"].Value.ToString();
                model.sales_price = Convert.ToDouble(row.Cells["매출단가"].Value.ToString());
                model.purchase_price = 0;
                model.division = row.Cells["구분"].Value.ToString();
                model.edit_date = row.Cells["단가수정일"].Value.ToString();
                model.manager1 = row.Cells["담당자1"].Value.ToString();
                model.is_tax = row.Cells["부가세"].Value.ToString();
                list.Add(model);
                if (form != null)
                    form.SetProduct(list);
            }
        }
        #endregion
    }
}

