using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.ProductSalesManager
{
    public partial class ProductSalesManager : Form
    {
        IStockRepository stockRepository = new StockRepository();
        IStockLimitRepository stockLimitRepository = new StockLimitRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        UsersModel um;
        public ProductSalesManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel; 
        }

        private void ProductSalesManager_Load(object sender, EventArgs e)
        {
            //컬럼 헤더스타일
            SetColumnHeaderStyle();
            //
            dgvProduct.Columns["min_unit"].Visible = false;
            for (int i = 2020; i <= DateTime.Now.Year; i++)
            { 
                cbSttYear.Items.Add(i.ToString());
                cbEndYear.Items.Add(i.ToString());
            }
            cbSttYear.Text = DateTime.Now.AddMonths(-12).Year.ToString();
            cbSttMonth.Text = DateTime.Now.AddMonths(-12).Month.ToString();
            cbEndYear.Text = DateTime.Now.Year.ToString();
            cbEndMonth.Text = DateTime.Now.Month.ToString();

            
            //씨오버 재고 테이블 호출
            CallStockProcedure();
        }

        #region Method
        private void CallStockProcedure()
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (stockRepository.CallStoredProcedureSTOCK(user_id, eDate) == 0)
                {

                    MessageBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
                this.Activate();
                return;
            }
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void InsertData()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "품명별 매출한도", "is_visible"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvProduct.Rows.Count > 0)
            {
                if (MessageBox.Show(this,"등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    StringBuilder sql = new StringBuilder();

                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value))
                        { 
                            ProductLimitModel model = new ProductLimitModel();
                            if (dgvProduct.Rows[i].Cells["product"].Value.ToString() == "SUM")
                            {
                                model.product = dgvProduct.Rows[i - 1].Cells["product"].Value.ToString();
                                model.origin = dgvProduct.Rows[i - 1].Cells["origin"].Value.ToString();
                                model.sizes = dgvProduct.Rows[i - 1].Cells["sizes"].Value.ToString();
                                model.weight = dgvProduct.Rows[i - 1].Cells["weight"].Value.ToString();
                                model.unit = "all";
                            }
                            else
                            {
                                model.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                                model.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                                model.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                                model.weight = dgvProduct.Rows[i].Cells["weight"].Value.ToString();
                                model.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                            }
                            double limit_qty;
                            if (dgvProduct.Rows[i].Cells["limit_qty"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["limit_qty"].Value.ToString(), out limit_qty))
                                limit_qty = 0;  
                            model.limit_qty = limit_qty;

                            double limit_amount;
                            if (dgvProduct.Rows[i].Cells["limit_amount"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["limit_amount"].Value.ToString(), out limit_amount))
                                limit_amount = 0;
                            model.limit_amount = limit_amount;

                            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                            model.edit_user = um.user_name;

                            sql = stockLimitRepository.DeleteProduct(model);
                            sqlList.Add(sql);

                            sql = stockLimitRepository.InsertProduct(model);
                            sqlList.Add(sql);
                        }
                    }
                    //Execute
                    if (sqlList.Count > 0)
                    {
                        if (stockLimitRepository.UpdateTrans(sqlList) == -1)
                        {
                            MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                        else
                        {
                            MessageBox.Show(this, "등록성공");
                            this.Activate();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this,"등록할 내역이 없습니다.");
                this.Activate();
            }
        }
        private void GetData()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            //유효성 검사
            dgvProduct.Rows.Clear();
            DateTime sttdate, enddate;
            int temp;
            if (string.IsNullOrEmpty(cbSttYear.Text) || !int.TryParse(cbSttYear.Text, out temp))
            {
                MessageBox.Show(this,"검색기간 값을 확인해주세요!");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(cbSttMonth.Text) || !int.TryParse(cbSttMonth.Text, out temp))
            {
                MessageBox.Show(this,"검색기간 값을 확인해주세요!");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(cbEndYear.Text) || !int.TryParse(cbEndYear.Text, out temp))
            {
                MessageBox.Show(this,"검색기간 값을 확인해주세요!");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(cbEndMonth.Text) || !int.TryParse(cbEndMonth.Text, out temp))
            {
                MessageBox.Show(this,"검색기간 값을 확인해주세요!");
                this.Activate();
                return;
            }
            else
            {
                try
                {
                    sttdate = new DateTime(Convert.ToInt16(cbSttYear.Text), Convert.ToInt16(cbSttMonth.Text), 1);
                    enddate = new DateTime(Convert.ToInt16(cbEndYear.Text), Convert.ToInt16(cbEndMonth.Text), 1);
                    if (sttdate > enddate)
                    {
                        MessageBox.Show(this,"검색기간 종료월이 시작월보다 작습니다!");
                        this.Activate();
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(this,"검색기간 값을 확인해주세요!");
                    this.Activate();
                    return;
                }
            }
            //품몽정보
            DataTable productDt = stockRepository.GetProductSales(sttdate, enddate, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            //설정정보
            DataTable limitDt = stockLimitRepository.GetProductLimit(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            //무역담당자
            DataTable tmDt = purchasePriceRepository.GetTradeManaer(txtProduct.Text, txtOrigin.Text, txtSizes.Text, "", txtManager.Text.Trim());
            if (productDt.Rows.Count > 0)
            {
                string whr;
                DataRow[] limitDr;
                foreach (System.Data.DataColumn col in productDt.Columns)
                {
                    col.ReadOnly = false;
                }
                int n;
                DataGridViewRow row;
                string product = productDt.Rows[0]["품명"].ToString();
                string origin = productDt.Rows[0]["원산지"].ToString();
                double[] total_qty = new double[14];
                double[] total_amount = new double[14];
                //총합 초기화
                for (int i = 0; i < 12; i++)
                {
                    total_qty[i] = 0;
                    total_amount[i] = 0;
                }
                //데이터 순회출력==================================================
                bool isOutput = true;
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    isOutput = true;
                    //데이터 무역담당자 검색시
                    whr = "product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                            + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                            + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'";
                    DataRow[] tmRow = tmDt.Select(whr);
                    if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                    {
                        if (tmRow.Length == 0)
                            isOutput = false;
                    }
                    //출력=================================================================
                    if (isOutput)
                    { 
                        //매출수 변환(1kg)
                        double min_unit;
                        double unit;
                        double[] sales_price = new double[14];
                        if (double.TryParse(productDt.Rows[i]["최소단위"].ToString(), out min_unit) && double.TryParse(productDt.Rows[i]["단위"].ToString(), out unit))
                        {
                            if (unit > 0 && min_unit > 0)
                            {
                                for (int j = 0; j <= 13; j++)
                                {
                                    sales_price[j] = Convert.ToDouble(productDt.Rows[i]["매출수" + j.ToString()].ToString()) / unit * min_unit;
                                }
                            }
                        }

                        //SUM 행 추가============================================================
                        if (product != productDt.Rows[i]["품명"].ToString()
                            || origin != productDt.Rows[i]["원산지"].ToString())
                        {
                            n = dgvProduct.Rows.Add();
                            row = dgvProduct.Rows[n];
                            row.Cells["product"].Value = "SUM";
                            //월별 매출평균
                            for (int j = 1; j <= 14; j++)
                            {
                                row.Cells[((j - 1) * 2) + 12].Value = total_qty[j - 1].ToString("#,##0");
                                row.Cells[((j - 1) * 2) + 13].Value = total_amount[j - 1].ToString("#,##0");
                            }
                            //Style
                            row.DefaultCellStyle.BackColor = Color.LightGray;
                            row.DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);

                            //설정값
                            whr = "product = '" + product + "'"
                                 + " AND origin = '" + origin + "'"
                                 + " AND unit = 'all'";
                            limitDr = limitDt.Select(whr);
                            if (limitDr.Length > 0)
                            {
                                row.Cells["limit_qty"].Value = Convert.ToDouble(limitDr[0]["limit_qty"].ToString()).ToString("#,##0");
                                row.Cells["limit_amount"].Value = Convert.ToDouble(limitDr[0]["limit_amount"].ToString()).ToString("#,##0");
                                row.Cells["updatetime"].Value = Convert.ToDateTime(limitDr[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                            }

                            //최신화
                            product = productDt.Rows[i]["품명"].ToString();
                            origin = productDt.Rows[i]["원산지"].ToString();
                            for (int j = 0; j <= 13; j++)
                            {
                                total_qty[j] = 0;
                                total_amount[j] = 0;
                            }
                        }

                        //월별 매출내역===================================================================================
                        //월별 매출합계
                        for (int j = 0; j <= 12; j++)
                        {
                            total_qty[j] += sales_price[j];
                            total_amount[j] += Convert.ToDouble(productDt.Rows[i]["매출금액" + (j).ToString()].ToString());
                        }

                        //Add row
                        n = dgvProduct.Rows.Add();
                        row = dgvProduct.Rows[n];
                        row.Cells["product"].Value = productDt.Rows[i]["품명"].ToString();
                        row.Cells["origin"].Value = productDt.Rows[i]["원산지"].ToString();
                        row.Cells["sizes"].Value = productDt.Rows[i]["규격"].ToString();
                        row.Cells["unit"].Value = productDt.Rows[i]["단위"].ToString();
                        row.Cells["weight"].Value = productDt.Rows[i]["최소단위"].ToString();
                        row.Cells["stock"].Value = Convert.ToDouble(productDt.Rows[i]["재고수"].ToString()).ToString("#,##0");
                        row.Cells["stock_amount"].Value = Convert.ToDouble(productDt.Rows[i]["재고금액"].ToString()).ToString("#,##0");

                        //월별 매출평균=====================================
                        for (int j = 1; j <= 14; j++)
                        {
                            row.Cells[((j - 1) * 2) + 12].Value = Convert.ToDouble(productDt.Rows[i]["매출수" + (j - 1).ToString()]).ToString("#,##0");
                            row.Cells[((j - 1) * 2) + 13].Value = Convert.ToDouble(productDt.Rows[i]["매출금액" + (j - 1).ToString()]).ToString("#,##0");
                        }
                        //무역담당자========================================
                        if (tmRow.Length > 0)
                        {
                            row.Cells["manager"].Value = tmRow[0]["manager"].ToString();
                        }
                        //설정값============================================
                        whr = "product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                             + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                             + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                             + " AND unit = '" + productDt.Rows[i]["단위"].ToString() + "'";
                        limitDr = limitDt.Select(whr);
                        if (limitDr.Length > 0)
                        {
                            row.Cells["limit_qty"].Value = Convert.ToDouble(limitDr[0]["limit_qty"].ToString()).ToString("#,##0");
                            row.Cells["limit_amount"].Value = Convert.ToDouble(limitDr[0]["limit_amount"].ToString()).ToString("#,##0");
                            row.Cells["updatetime"].Value = Convert.ToDateTime(limitDr[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                        }

                        
                    }
                }
                //마지막 SUM 행 추가=====================================
                if (isOutput && dgvProduct.Rows.Count > 0)
                { 
                    n = dgvProduct.Rows.Add();
                    row = dgvProduct.Rows[n];
                    row.Cells["product"].Value = "SUM";
                    //월별 매출평균==========================================
                    for (int j = 1; j <= 14; j++)
                    {
                        row.Cells[((j - 1) * 2) + 12].Value = total_qty[j - 1].ToString("#,##0");
                        row.Cells[((j - 1) * 2) + 13].Value = total_amount[j - 1].ToString("#,##0");
                    }
                    //설정값=================================================
                    whr = "product = '" + dgvProduct.Rows[n - 1].Cells["product"].Value.ToString() + "'"
                         + " AND origin = '" + dgvProduct.Rows[n - 1].Cells["origin"].Value.ToString() + "'"
                         + " AND sizes = '" + dgvProduct.Rows[n - 1].Cells["sizes"].Value.ToString() + "'"
                         + " AND unit = 'all'";
                    limitDr = limitDt.Select(whr);
                    if (limitDr.Length > 0)
                    {
                        row.Cells["limit_qty"].Value = Convert.ToDouble(limitDr[0]["limit_qty"].ToString()).ToString("#,##0");
                        row.Cells["limit_amount"].Value = Convert.ToDouble(limitDr[0]["limit_amount"].ToString()).ToString("#,##0");
                    }

                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                }

            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private void SetColumnHeaderStyle()
        {
            DataGridView dgv = dgvProduct;
            //            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            for (int i = 4; i < dgv.Columns.Count; i++)
            {
                if(dgv.Columns[i].Name != "updatetime")
                    dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            Color colBlue = Color.FromArgb(221, 235, 247);
            dgv.Columns[0].HeaderCell.Style.BackColor = colBlue;
            dgv.Columns[1].HeaderCell.Style.BackColor = colBlue;
            dgv.Columns[2].HeaderCell.Style.BackColor = colBlue;
            dgv.Columns[3].HeaderCell.Style.BackColor = colBlue;
            dgv.Columns[4].HeaderCell.Style.BackColor = colBlue;
            dgv.Columns[5].HeaderCell.Style.BackColor = colBlue;
            dgv.Columns[6].HeaderCell.Style.BackColor = colBlue;

            Color colGreen = Color.FromArgb(226, 239, 218);
            dgv.Columns[7].HeaderCell.Style.BackColor = colGreen;
            dgv.Columns[8].HeaderCell.Style.BackColor = colGreen;
             
            dgv.Columns[9].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns[10].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns[11].HeaderCell.Style.BackColor = Color.Beige;
            Color colYellow = Color.FromArgb(255, 251, 239);
            dgv.Columns[9].DefaultCellStyle.BackColor = colYellow;
            dgv.Columns[10].DefaultCellStyle.BackColor = colYellow;
            dgv.Columns[11].DefaultCellStyle.BackColor = colYellow;

            dgv.Columns[12].HeaderCell.Style.BackColor = Color.Tan;
            dgv.Columns[13].HeaderCell.Style.BackColor = Color.Tan;

            dgv.Columns[14].HeaderCell.Style.BackColor = Color.DarkGray;
            dgv.Columns[15].HeaderCell.Style.BackColor = Color.DarkGray;
        }

        #endregion

        #region Button
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertData();
        }
        #endregion

        #region Key event
        private void ProductSalesManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        InsertData();
                        break;
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.X:
                        GetData();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtProduct.Focus();
                        break;

                }
            }
        }
        #endregion

        #region Datagridview 멀티헤더
        private void dgvProduct_Paint(object sender, PaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            string[] strHeaders = { "품목정보", "재고정보", "설정", "평균", "총합", "1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월"};
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            // 품목정보
            {
                Rectangle r1 = gv.GetCellDisplayRectangle(0, -1, false);
                int width1 = gv.GetCellDisplayRectangle(1, -1, false).Width;
                int width2 = gv.GetCellDisplayRectangle(2, -1, false).Width;
                int width3 = gv.GetCellDisplayRectangle(3, -1, false).Width;
                int width4 = gv.GetCellDisplayRectangle(4, -1, false).Width;
                int width5 = gv.GetCellDisplayRectangle(5, -1, false).Width;
                int width6 = gv.GetCellDisplayRectangle(6, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 + width2 + width3 + width4 + width5 + width6 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(221, 235, 247);
                e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                e.Graphics.DrawString(strHeaders[0], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            // 재고
            {
                Color col = Color.FromArgb(226, 239, 218);
                Rectangle r1 = gv.GetCellDisplayRectangle(7, -1, false);
                int width1 = gv.GetCellDisplayRectangle(8, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(221, 235, 247);
                e.Graphics.FillRectangle(new SolidBrush(col), r1);
                e.Graphics.DrawString(strHeaders[1], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            // 설정
            {
                Color col = Color.Beige;
                Rectangle r1 = gv.GetCellDisplayRectangle(9, -1, false);
                int width1 = gv.GetCellDisplayRectangle(10, -1, false).Width;
                int width2 = gv.GetCellDisplayRectangle(11, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 + width2 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(221, 235, 247);
                e.Graphics.FillRectangle(new SolidBrush(col), r1);
                e.Graphics.DrawString(strHeaders[2], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            // 월별평균, 합계, 월별
            {
                for (int i = 1; i <= 14; i++)
                {
                    Rectangle r2 = gv.GetCellDisplayRectangle(((i - 1) * 2) + 12, -1, false);
                    int width = gv.GetCellDisplayRectangle(((i - 1) * 2) + 13, -1, false).Width;
                    r2.X += 1;
                    r2.Y += 1;
                    r2.Width = r2.Width + width - 2;
                    r2.Height = (r2.Height / 2) - 2;
                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r2);

                    Color col;
                    if (i == 1)
                        col = Color.Tan;
                    else if (i == 2)
                        col = Color.DarkGray;
                    else
                        col = gv.ColumnHeadersDefaultCellStyle.BackColor;

                    e.Graphics.FillRectangle(new SolidBrush(col), r2);
                    e.Graphics.DrawString(strHeaders[i + 2], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r2, format);
                }   
            }

        }
        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r = e.CellBounds;
                r.Y += e.CellBounds.Height / 2;
                r.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r, true);
                e.PaintContent(r);
                e.Handled = true;
            }
            else if( e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "limit_qty" || dgvProduct.Columns[e.ColumnIndex].Name == "limit_amount")
                {
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

                    double stock;
                    if (row.Cells["stock"].Value == null || !double.TryParse(row.Cells["stock"].Value.ToString(), out stock))
                        stock = 0;

                    double stock_amount;
                    if (row.Cells["stock_amount"].Value == null || !double.TryParse(row.Cells["stock_amount"].Value.ToString(), out stock_amount))
                        stock_amount = 0;

                    double limit_qty;
                    if (row.Cells["limit_qty"].Value == null || !double.TryParse(row.Cells["limit_qty"].Value.ToString(), out limit_qty))
                        limit_qty = 0;

                    double limit_amount;
                    if (row.Cells["limit_amount"].Value == null || !double.TryParse(row.Cells["limit_amount"].Value.ToString(), out limit_amount))
                        limit_amount = 0;

                    //Limit check
                    if (stock <= limit_qty)
                    {
                        row.Cells["limit_qty"].Style.ForeColor = Color.Red;
                        row.Cells["limit_qty"].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    }
                    else
                    {
                        row.Cells["limit_qty"].Style.ForeColor = Color.Black;
                        row.Cells["limit_qty"].Style.Font = new Font("나눔고딕", 9, FontStyle.Regular);
                    }

                    if (stock_amount <= limit_amount)
                    {
                        row.Cells["limit_amount"].Style.ForeColor = Color.Red;
                        row.Cells["limit_amount"].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    }
                    else
                    {
                        row.Cells["limit_amount"].Style.ForeColor = Color.Black;
                        row.Cells["limit_amount"].Style.Font = new Font("나눔고딕", 9, FontStyle.Regular);
                    }

                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "chk")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["chk"].Value);
                    if (isChecked)
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.Red;
                    else
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.LightGray;
                }
            }
        }

        private void dgvProduct_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void dgvProduct_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 & e.RowIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "limit_qty" || dgvProduct.Columns[e.ColumnIndex].Name == "limit_amount")
                {
                    dgvProduct.Rows[e.RowIndex].Cells["chk"].Value = true;
                }
            }
        }
        #endregion
    }
}
