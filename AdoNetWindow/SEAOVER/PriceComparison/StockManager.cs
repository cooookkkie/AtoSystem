using AdoNetWindow.Model;
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

namespace AdoNetWindow.SEAOVER.PriceComparison
{    
    public partial class StockManager : Form
    {
        IConfigRepository configRepository = new ConfigRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        ShortManagerModel model;
        Libs.Tools.Common common = new Libs.Tools.Common();
        public StockManager(ShortManagerModel pcModel, int production_days = 20)
        {
            InitializeComponent();
            model = pcModel;
            txtMakeTerm.Text = production_days.ToString();
        }

        private void StockManager_Load(object sender, EventArgs e)
        {
            this.dgvContract.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
            SetData();
            GetUnpendingData();
            SetHeaderStyle();
            if (SortSetting())
            {
                CalculateStock();
            }
            this.dgvContract.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContract_CellValueChanged);
        }
        #region Method
        private bool Validation()
        {
            dgvContract.EndEdit();
            if (dgvContract.Rows.Count > 0)
            {
                //유효성검사
                foreach (DataGridViewRow row in dgvContract.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.ColumnIndex > 0 && cell.Value == null)
                        {
                            cell.Value = "";
                        }
                    }
                }
                for (int i = 0; i < dgvContract.Rows.Count; i++)
                {
                    DateTime tmpDt;
                    double tmpInt;

                    if (!DateTime.TryParse(dgvContract.Rows[i].Cells["etd"].Value.ToString(), out tmpDt)
                        && !DateTime.TryParse(dgvContract.Rows[i].Cells["eta"].Value.ToString(), out tmpDt))
                    {
                        MessageBox.Show(this,"ETD 또는 ETA를 입력해주세요.");
                        this.Activate();
                        return false;
                    }
                    else if (!double.TryParse(dgvContract.Rows[i].Cells["pending_term"].Value.ToString(), out tmpInt))
                    {
                        MessageBox.Show(this,"통관기간을 입력해주세요.");
                        this.Activate();
                        return false;
                    }
                    else if (!double.TryParse(dgvContract.Rows[i].Cells["qty"].Value.ToString(), out tmpInt))
                    {
                        MessageBox.Show(this,"입고수를 입력해주세요.");
                        this.Activate();
                        return false;
                    }
                }

                return true;
            }
            else
            {
                MessageBox.Show(this,"계산할 내역이 없습니다.");
                this.Activate();
                return false;
            }
        }
        private void OpenCalendar()
        {
            DataTable pendingDt = common.ConvertDgvToDataTable(dgvContract);
            List<shipmentModel> list = new List<shipmentModel>();
            if (dgvContract.Rows.Count > 0)
            {
                for (int i = 0; i < dgvContract.Rows.Count; i++)
                {
                    if (dgvContract.Rows[i].Cells["warehousing_date"].Value == null || dgvContract.Rows[i].Cells["warehousing_date"].Value.ToString() == "")
                    {
                        MessageBox.Show(this,"입고일자가 계산되지 않은내역이 있습니다.");
                        this.Activate();
                        return;
                    }

                    shipmentModel sModel = new shipmentModel();
                    sModel.etd = dgvContract.Rows[i].Cells["etd"].Value.ToString();
                    sModel.eta = dgvContract.Rows[i].Cells["warehousing_date"].Value.ToString();
                    sModel.qty = dgvContract.Rows[i].Cells["qty"].Value.ToString();
                    list.Add(sModel);
                }
            }

            int shipDays = Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
            //sttdate ~ enddate
            model.enddate = DateTime.Now.ToString("yyyy-MM-dd");
            StockManagement sm = new StockManagement(model, list, pendingDt);
            int mainFormX = this.Location.X;
            int mainFormY = this.Location.Y;
            int mainFormWidth = this.Size.Width;
            int mainFormHeight = this.Size.Height;

            int childFormWidth = sm.Size.Width;
            int childFormHeight = sm.Size.Height;

            sm.StartPosition = FormStartPosition.CenterParent;
            sm.Show();
            sm.Location = new Point(mainFormX + (mainFormWidth / 2) - (childFormWidth / 2), mainFormY + (mainFormHeight / 2) - (childFormHeight / 2));
        }
        //타이틀 및 현재 추천계약일 계산
        private void SetData()
        {
            txtProduct.Text = model.product;
            txtOrigin.Text = model.origin;
            txtSizes.Text = model.sizes;
            txtUnit.Text = model.unit;
            txtStock.Text = model.real_stock.ToString("#,##0");
            txtAvgStockDay.Text = model.avg_sales_day.ToString("#,##0");
            txtAvgStockMonth.Text = model.avg_sales_month.ToString("#,##0");
            txtRoundStock.Text = ((model.real_stock / model.avg_sales_day) / 21).ToString("#,##0.00");
            txtExhaust.Text = model.exhaust_date;

            CountryModel cModel = new CountryModel();
            cModel = configRepository.GetCountryConfigAsOne(model.origin);
            if (cModel != null)
            {
                txtShippingTerm.Text = cModel.delivery_days.ToString("#,##0");
            }
            else
            {
                txtShippingTerm.Text = "0";
            }

            //소진일자
            DateTime exhaust_date = Convert.ToDateTime(model.exhaust_date);

            //최소ETD
            int sDay = Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
            DateTime etd = new DateTime();
            common.GetMinusDay(exhaust_date, sDay, out etd);
            txtEtd.Text = etd.ToString("yyyy-MM-dd");

            //추천 계약일자
            sDay = Convert.ToInt16(txtMakeTerm.Text) + Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
            DateTime contract_date = new DateTime();
            common.GetMinusDay(exhaust_date, sDay, out contract_date);
            txtContractDate.Text = contract_date.ToString("yyyy-MM-dd");
        }
        //미통관내역 가져오기
        private void GetUnpendingData()
        {
            dgvContract.Rows.Clear();
            DataTable unpendingTable = customsRepository.GetUnpendingProduct(model.product, model.origin, model.sizes, model.unit, false, true);
            if (unpendingTable.Rows.Count > 0)
            { 
                for(int i = 0; i < unpendingTable.Rows.Count; i++)
                {
                    string bl_no = unpendingTable.Rows[i]["bl_no"].ToString();
                    string warehousing_date = unpendingTable.Rows[i]["warehousing_date"].ToString();
                    //계약만 한 상태
                    if (string.IsNullOrEmpty(bl_no) || !string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date))
                    {
                        DateTime warehouse_date;
                        if (DateTime.TryParse(unpendingTable.Rows[i]["eta"].ToString(), out warehouse_date))
                        {
                            int pending_day;
                            if(!int.TryParse(txtPendingTerm.Text, out pending_day))
                                pending_day = 0;

                            common.GetPlusDay(warehouse_date, pending_day, out warehouse_date);
                        }
                        else if (DateTime.TryParse(unpendingTable.Rows[i]["etd"].ToString(), out warehouse_date))
                        {
                            int shipment_day;
                            if (!int.TryParse(txtShippingTerm.Text, out shipment_day))
                                shipment_day = 0;
                            int pending_day;
                            if (!int.TryParse(txtPendingTerm.Text, out pending_day))
                                pending_day = 0;

                            common.GetPlusDay(warehouse_date, shipment_day + pending_day, out warehouse_date);
                        }
                        else
                        {
                            warehouse_date = DateTime.Now;   
                        }

                        if (warehouse_date <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            common.GetPlusDay(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")), 5, out warehouse_date);
                        unpendingTable.Rows[i]["warehousing_date"] = warehouse_date.ToString("yyyy-MM-dd");


                        int n = dgvContract.Rows.Add();

                        DataGridViewRow row = dgvContract.Rows[n];
                        row.Cells["etd"].Value = unpendingTable.Rows[i]["etd"].ToString();
                        row.Cells["eta"].Value = unpendingTable.Rows[i]["eta"].ToString();
                        row.Cells["warehousing_date"].Value = unpendingTable.Rows[i]["warehousing_date"].ToString();
                        row.Cells["pending_term"].Value = Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
                        row.Cells["qty"].Value = unpendingTable.Rows[i]["quantity_on_paper"].ToString();
                        row.Cells["ato_no"].Value = unpendingTable.Rows[i]["ato_no"].ToString();
                        row.Cells["contract_no"].Value = unpendingTable.Rows[i]["contract_no"].ToString();
                        row.Cells["bl_no"].Value = unpendingTable.Rows[i]["bl_no"].ToString();
                    }
                }
            }
        }
        //정렬하기
        private bool SortSetting()
        {
            DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvContract);
            if (tb.Rows.Count > 0)
            {
                tb.Columns.Add("warehousing_date_sort", typeof(DateTime)).SetOrdinal(1);
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    DateTime warehousing_date = new DateTime();
                    if (!DateTime.TryParse(tb.Rows[i]["warehousing_date"].ToString(), out warehousing_date))
                    {
                        warehousing_date = new DateTime(2999, 12, 31);
                    }
                    tb.Rows[i]["warehousing_date_sort"] = warehousing_date.ToString("yyyy-MM-dd");
                }
                //Sorting
                DataView tv = new DataView(tb);
                tv.Sort = "warehousing_date_sort";
                tb = tv.ToTable();
                //재출력
                dgvContract.Rows.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    int n = dgvContract.Rows.Add();

                    dgvContract.Rows[n].Cells["etd"].Value = tb.Rows[i]["etd"].ToString();
                    dgvContract.Rows[n].Cells["pending_term"].Value = tb.Rows[i]["pending_term"].ToString();
                    dgvContract.Rows[n].Cells["eta"].Value = tb.Rows[i]["eta"].ToString();
                    dgvContract.Rows[n].Cells["warehousing_date"].Value = tb.Rows[i]["warehousing_date"].ToString();
                    dgvContract.Rows[n].Cells["qty"].Value = tb.Rows[i]["qty"].ToString();
                    dgvContract.Rows[n].Cells["warehouse_qty"].Value = tb.Rows[i]["warehouse_qty"].ToString();
                    dgvContract.Rows[n].Cells["exhausted_date"].Value = tb.Rows[i]["exhausted_date"].ToString();
                    dgvContract.Rows[n].Cells["exhausted_day_count"].Value = tb.Rows[i]["exhausted_day_count"].ToString();
                    dgvContract.Rows[n].Cells["after_qty_exhausted_date"].Value = tb.Rows[i]["after_qty_exhausted_date"].ToString();
                    dgvContract.Rows[n].Cells["month_around"].Value = tb.Rows[i]["month_around"].ToString();
                    dgvContract.Rows[n].Cells["recommend_etd"].Value = tb.Rows[i]["recommend_etd"].ToString();
                    dgvContract.Rows[n].Cells["recommend_contract_date"].Value = tb.Rows[i]["recommend_contract_date"].ToString();
                    dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                    dgvContract.Rows[n].Cells["bl_no"].Value = tb.Rows[i]["bl_no"].ToString();
                    dgvContract.Rows[n].Cells["contract_no"].Value = tb.Rows[i]["contract_no"].ToString();
                }
                return true;
            }
            else
                return false;
        }
        //계산하기
        private bool CalculateStock()
        {
            double stock = Convert.ToInt32(txtStock.Text.Replace(",",""));
            DateTime exhaust_date = Convert.ToDateTime(txtExhaust.Text);
            DateTime sttdate = DateTime.Now;
            DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvContract);
            dgvContract.Rows.Clear();
            //재출력
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                int n = dgvContract.Rows.Add();

                dgvContract.Rows[n].Cells["etd"].Value = tb.Rows[i]["etd"].ToString();
                dgvContract.Rows[n].Cells["eta"].Value = tb.Rows[i]["eta"].ToString();
                dgvContract.Rows[n].Cells["warehousing_date"].Value = tb.Rows[i]["warehousing_date"].ToString();
                dgvContract.Rows[n].Cells["pending_term"].Value = tb.Rows[i]["pending_term"].ToString();
                dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                dgvContract.Rows[n].Cells["contract_no"].Value = tb.Rows[i]["contract_no"].ToString();
                dgvContract.Rows[n].Cells["bl_no"].Value = tb.Rows[i]["bl_no"].ToString();

                DateTime warehousing_date = new DateTime();
                if (DateTime.TryParse(tb.Rows[i]["warehousing_date"].ToString(), out warehousing_date))
                {
                    //입고일자 계산
                    dgvContract.Rows[n].Cells["warehousing_date"].Value = warehousing_date.ToString("yyyy-MM-dd");
                    //소진일자 계산, 입고후 재고 계산
                    if (exhaust_date > warehousing_date)
                    {
                        common.GetStock(sttdate, warehousing_date.AddDays(-1), model.avg_sales_day, stock, out stock);
                    }
                    else
                    {
                        stock = 0;
                        int Days = 0;
                        common.GetWorkDay(exhaust_date, warehousing_date.AddDays(-1), out Days);
                        dgvContract.Rows[n].Cells["exhausted_day_count"].Value = Days.ToString("#,##0");
                        if (Days > 0)
                        {
                            dgvContract.Rows[n].Cells["exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd") + " ~ "+ warehousing_date.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                    }
                    //입고수량
                    int qty;
                    if (tb.Rows[i]["qty"] == null || !int.TryParse(tb.Rows[i]["qty"].ToString().Replace("," , ""), out qty))
                        qty = 0;
                    //입고반영
                    stock += qty;
                    //수량, 재고
                    dgvContract.Rows[n].Cells["qty"].Value = qty.ToString("#,##0");
                    dgvContract.Rows[n].Cells["warehouse_qty"].Value = stock.ToString("#,##0");
                    //회전율
                    dgvContract.Rows[n].Cells["month_around"].Value = ((stock / model.avg_sales_day) / 21).ToString("#,##0.00");
                    //소진일자 계산
                    sttdate = warehousing_date;
                    //그날 판매량 제외
                    double exhausted_cnt = 0;
                    //stock -= model.avg_sales_day;
                    common.GetExhausedDateDayd(sttdate, stock, model.avg_sales_day, 0, null, out exhaust_date, out exhausted_cnt);

                    if (exhaust_date.Year != 1900)
                    {   
                        dgvContract.Rows[n].Cells["after_qty_exhausted_date"].Value = exhaust_date.ToString("yyyy-MM-dd");
                        //최소ETD
                        int sDay = Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
                        DateTime tmp_etd = new DateTime();
                        common.GetMinusDay(exhaust_date, sDay, out tmp_etd);
                        dgvContract.Rows[n].Cells["recommend_etd"].Value = tmp_etd.ToString("yyyy-MM-dd");
                        //추천 계약일자
                        sDay = Convert.ToInt16(txtMakeTerm.Text) + Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
                        DateTime contract_date = new DateTime();
                        common.GetMinusDay(exhaust_date, sDay, out contract_date);
                        dgvContract.Rows[n].Cells["recommend_contract_date"].Value = contract_date.ToString("yyyy-MM-dd");
                    }
                }
                dgvContract.Rows[n].Cells["ato_no"].Value = tb.Rows[i]["ato_no"].ToString();
                dgvContract.Rows[n].Cells["contract_no"].Value = tb.Rows[i]["contract_no"].ToString();
                dgvContract.Rows[n].Cells["bl_no"].Value = tb.Rows[i]["bl_no"].ToString();
            }
            return true;
        }
        //Datagridview Header style 
        private void SetHeaderStyle()
        {
            dgvContract.Init();
            DataGridView dgv = dgvContract;
            //헤더 디자인
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["etd"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgv.Columns["eta"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgv.Columns["exhausted_date"].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns["exhausted_day_count"].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns["recommend_etd"].DefaultCellStyle.ForeColor = Color.Red;
            dgv.Columns["recommend_contract_date"].DefaultCellStyle.ForeColor = Color.Red;

            // 전체 컬럼의 Sorting 기능 차단 
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        //내역삭제
        private void DeletePending()
        {
            for (int i = 0; i < dgvContract.Rows.Count; i++)
            {
                var cbxCell = (DataGridViewCheckBoxCell)dgvContract.Rows[i].Cells["chk"];
                //선택했을때만
                if (cbxCell.Value != null && (bool)cbxCell.Value)
                {
                    dgvContract.Rows.Remove(dgvContract.Rows[i]);
                }
            }
        }
        #endregion

        #region Datagridview Cell Click
        private void dgvContract_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvContract.Columns[e.ColumnIndex].Name == "etd")
                {
                    Common.Calendar cal = new Common.Calendar();
                    string sDate = cal.GetDate(true);
                    if (sDate != null)
                    {
                        DateTime tmpDate = Convert.ToDateTime(sDate);
                        dgvContract.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = tmpDate.ToString("yyyy-MM-dd");
                        dgvContract.EndEdit();
                    }
                }
            }
        }
        private void txtExhaust_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Common.Calendar cal = new Common.Calendar();
            string sDate = cal.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtExhaust.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void txtInsertEtd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Common.Calendar cal = new Common.Calendar();
            string sDate = cal.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                tb.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void dgvContract_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                if (dgvContract.Columns[e.ColumnIndex].Name == "etd")
                {
                    if (dgvContract.Rows[e.RowIndex].Cells["etd"].Value != null  && dgvContract.Rows[e.RowIndex].Cells["pending_term"].Value != null)
                    {
                        int pending_day;
                        if (int.TryParse(dgvContract.Rows[e.RowIndex].Cells["pending_term"].Value.ToString(), out pending_day))
                        {
                            DateTime dt;
                            if (DateTime.TryParse(dgvContract.Rows[e.RowIndex].Cells[0].Value.ToString(), out dt))
                            {
                                common.GetPlusDay(dt, pending_day, out dt);
                                dgvContract.Rows[e.RowIndex].Cells["warehousing_date"].Value = dt.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Key event
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            if (tb.Name != "dgvContract")
            {
                switch (keyData)
                {
                    case Keys.Left:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Right:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                    case Keys.Up:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Down:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void StockManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        {
                            if (SortSetting())
                                CalculateStock();
                            break;
                        }
                    /*case Keys.D:
                        {
                            DeletePending();
                            break;
                        }*/
                    case Keys.W:
                        {
                            OpenCalendar();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {

                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    
                }
            }
        }
        
        private void txtInsertPendingterm_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(46)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Button Click
        private void btnCalendar_Click(object sender, EventArgs e)
        {
            if (SortSetting())
            {
                OpenCalendar();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeletePending();
        }
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            dgvContract.EndEdit();
            if (SortSetting())
            {
                CalculateStock();
            }
        }
        private void btnPlus_Click(object sender, EventArgs e)
        {
            int n = dgvContract.Rows.Add();
            int term;
            try {
                term = Convert.ToInt16(txtShippingTerm.Text) + Convert.ToInt16(txtPendingTerm.Text);
            }
            catch
            {
                term = 0;
            }
            dgvContract.Rows[n].Cells["pending_term"].Value = term;
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (dgvContract.Rows.Count > 0)
            {
                dgvContract.Rows.Remove(dgvContract.Rows[dgvContract.Rows.Count - 1]);
            }
        }
        #endregion

    }
}
