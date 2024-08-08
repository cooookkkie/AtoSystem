using AdoNetWindow.Model;
using AdoNetWindow.Product;
using Repositories;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ScottPlot;
using ScottPlot.Plottable;
using ZedGraph;

namespace AdoNetWindow.PurchaseManager
{
    public partial class GraphManager : Form
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        Common.Calendar calendar = new Common.Calendar();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        Plot plot;

        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        public GraphManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            txtSttdate.Text = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //Datagridview ColumnHeader style 
            this.dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvPurchasePrice.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218);
            //Chart
            zedGraphControl1.PointValueEvent += new ZedGraph.ZedGraphControl.PointValueHandler(zedGraphControl1_PointValueEvent);
            zedGraphControl1.IsShowPointValues = true;
            SetChartSetting();
        }

        public GraphManager(UsersModel uModel, List<string[]> selectProduct)
        {
            InitializeComponent();
            um = uModel;
            txtSttdate.Text = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //Datagridview ColumnHeader style 
            this.dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            this.dgvPurchasePrice.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218);
            //Product setting
            for (int i = 0; i < selectProduct.Count; i++)
            { 
                int n = dgvProduct.Rows.Add();
                dgvProduct.Rows[n].Cells["product"].Value = selectProduct[i][0];
                dgvProduct.Rows[n].Cells["origin"].Value = selectProduct[i][1];
                dgvProduct.Rows[n].Cells["sizes"].Value = selectProduct[i][2];
                dgvProduct.Rows[n].Cells["unit"].Value = selectProduct[i][3];
                dgvProduct.Rows[n].Cells["cost_unit"].Value = selectProduct[i][4];
                dgvProduct.Rows[n].Cells["price"].Value = selectProduct[i][5];
                dgvProduct.Rows[n].Cells["chk"].Value = true;
            }
            //Chart
            zedGraphControl1.PointValueEvent += new ZedGraph.ZedGraphControl.PointValueHandler(zedGraphControl1_PointValueEvent);
            zedGraphControl1.IsShowPointValues = true;
            SetChartSetting();
            SetChart();
        }

        #region Method
        private void SetChart()
        {
            dgvProduct.EndEdit();
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate))
            {
                MessageBox.Show(this, "검색기간을 확인해주세요.");
                this.Activate();
                return;
            }
            if (!DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "검색기간을 확인해주세요.");
                this.Activate();
                return;
            }

            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();

            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                    if (isChecked)
                    {
                        DataTable purchasePriceDt = customsRepository.GetOfferPriceList(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                            , row.Cells["product"].Value.ToString()
                            , row.Cells["origin"].Value.ToString()
                            , row.Cells["sizes"].Value.ToString()
                            , row.Cells["unit"].Value.ToString());
                        if (purchasePriceDt.Rows.Count > 0)
                        {
                            int pointCount = purchasePriceDt.Rows.Count;
                            double[] xs1 = new double[pointCount];
                            double[] ys1 = new double[pointCount];
                            for (int j = 0; j < pointCount; j++)
                            {
                                //오퍼가 최대, 최소
                                ys1[j] = Convert.ToDouble(purchasePriceDt.Rows[j]["purchase_price"].ToString());
                                //날짜 최대, 최소
                                DateTime dt = Convert.ToDateTime(purchasePriceDt.Rows[j]["updatetime"].ToString());
                                xs1[j] = new XDate(dt);
                            }

                            // plot the data as curves
                            string product = row.Cells["product"].Value.ToString()
                                        + " | " + row.Cells["origin"].Value.ToString()
                                        + " | " + row.Cells["sizes"].Value.ToString()
                                        + " | " + row.Cells["unit"].Value.ToString();
                            var curve1 = zedGraphControl1.GraphPane.AddCurve(product, xs1, ys1, ColorList(i));
                        }
                    }
                }
            }
            // auto-axis and update the display
            //zedGraphControl1.GraphPane.XAxis.ResetAutoScale(zedGraphControl1.GraphPane, CreateGraphics());
            zedGraphControl1.GraphPane.YAxis.ResetAutoScale(zedGraphControl1.GraphPane, CreateGraphics());
            zedGraphControl1.Refresh();
        }


        private void SetChartSetting()
        {
            // GraphPane SETTINGS
            GraphPane myPane02m = zedGraphControl1.GraphPane;
            // Y AXIS SETTINGS
            myPane02m.YAxis.Title.Text = "";                       // Setta como Legenda (Particles) no Eixo Y
            myPane02m.YAxis.MajorGrid.IsVisible = true;                     // Setta Linhas no Eixo Y ou seja na Horizontal.
            myPane02m.YAxis.MajorGrid.DashOff = 5;                          // Seta a Intensidade da Linha no Eixo Y.

            // X AXIS SETTINGS
            myPane02m.XAxis.MajorGrid.DashOff = 5;                          // Seta a Intensidade da Linha no Eixo X.
            myPane02m.XAxis.MajorGrid.IsVisible = true;                     // Setta Linhas no Eixo X ou seja na Vertical.
            myPane02m.XAxis.MajorTic.IsOpposite = false;
            myPane02m.XAxis.MinorTic.IsAllTics = false; // mudei aqui para ver se para de pular o grafico.

            myPane02m.XAxis.Scale.FontSpec.Family = "Arial, Narrow";          // Setta a Fonte da Scale no Eixo X.
            myPane02m.XAxis.Scale.FontSpec.FontColor = Color.Fuchsia;       // Setta a Cor da Legenda do Dado que Entrara no Eixo X.
            myPane02m.XAxis.Scale.FontSpec.IsBold = true;                   // Setta Negrito na Scale no Eixo X.
            myPane02m.XAxis.Scale.FontSpec.Size = 10;                       // Setta o Tamanho da Fonte da Scale no Eixo X.
            myPane02m.XAxis.Scale.Format = "MM/dd";            // Setta o formato de data e hora e o \n faz uma mudança de linha.

            // Set the initial viewed range
            /*myPane02m.XAxis.Scale.Min = new XDate(DateTime.Now);            // We want to use time from now
            myPane02m.XAxis.Scale.Max = new XDate(DateTime.Now.AddDays(10));        // to 5 min per default*/
            //myPane02m.XAxis.Scale.MinGrace = 1;
            //myPane02m.XAxis.Scale.MaxGrace = 10;

            myPane02m.XAxis.Title.FontSpec.FontColor = Color.DarkViolet;    // Setta a Cor do Titulo no Eixo X.
            myPane02m.XAxis.Title.Text = "Date";                     // Setta a Legenda Date & Time no Eixo X.

            myPane02m.XAxis.Type = ZedGraph.AxisType.Date;                             // Setta o Tipo do Eixo X como Data.
            myPane02m.Legend.Position = ZedGraph.LegendPos.TopCenter;
        }
        public Color ColorList(int idx)
        {
            Color col;
            switch (idx)
            {
                case 1:
                    col = Color.Red;
                    break;
                case 2:
                    col = Color.Orange;
                    break;
                case 3:
                    col = Color.DarkSalmon;
                    break;
                case 4:
                    col = Color.Green;
                    break;
                case 5:
                    col = Color.Blue;
                    break;
                case 6:
                    col = Color.DarkBlue;
                    break;
                case 7:
                    col = Color.Violet;
                    break;
                case 8:
                    col = Color.Beige;
                    break;
                case 10:
                    col = Color.Coral;
                    break;
                default:
                    col = Color.Black;
                    break;
            }

            return col;
        }

        public void AddProduct(List<string[]> selectProduct, bool isSetChart = false)
        {
            if (selectProduct != null && selectProduct.Count > 0)
            {
                for (int i = 0; i < selectProduct.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["product"].Value = selectProduct[i][0];
                    dgvProduct.Rows[n].Cells["origin"].Value = selectProduct[i][1];
                    dgvProduct.Rows[n].Cells["sizes"].Value = selectProduct[i][2];
                    dgvProduct.Rows[n].Cells["unit"].Value = selectProduct[i][3];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = selectProduct[i][4];
                }
            }
            if (isSetChart)
                SetChart();
        }
        #endregion

        #region Key event
        private void GraphManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.M:
                        txtSttdate.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        btnGetProduct.PerformClick();
                        break;

                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                }
            }
        }
        private void txtSttdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tbb = (TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
                DateTime dt;
                if (DateTime.TryParse(tbb.Text, out dt))
                {
                    tbb.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }
        #endregion

        #region Button Click
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnCalendarSttdate_Click(object sender, EventArgs e)
        {
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtSttdate.Text = sdate;
            }
        }

        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtEnddate.Text = sdate;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvProduct.Rows.Clear();
            dgvPurchasePrice.Rows.Clear();
            zedGraphControl1.GraphPane.CurveList.Clear();
        }

        private void btnGetProduct_Click(object sender, EventArgs e)
        {
            ProductManager ps = new ProductManager(um, this, true);
            // 부모 Form의 좌표, 크기를 계산
            int mainformX = this.Location.X;
            int mainformY = this.Location.Y;
            int mainfromWidth = this.Size.Width;
            int mainfromHeight = this.Size.Height;

            // 자식 Form의 크기를 계산
            int childformwidth = ps.Size.Width;
            int childformheight = ps.Size.Height;
            Point p = new Point(mainformX + (mainfromWidth / 2) - (childformwidth / 2), mainformY + (mainfromHeight / 2) - (childformheight / 2));

            ps.ShowDialog();

            SetChart();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SetChart();
        }
        #endregion

        #region dgvProduct event
        string zedGraphControl1_PointValueEvent(ZedGraph.ZedGraphControl sender, ZedGraph.GraphPane pane, ZedGraph.CurveItem curve, int iPt)
        {
            DateTime sttdate, enddate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate))
            {
                MessageBox.Show(this, "검색기간을 확인해주세요.");
                this.Activate();
                return "";
            }
            if (!DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "검색기간을 확인해주세요.");
                this.Activate();
                return "";
            }
            string[] product = curve.Label.Text.Split('|');
            string company = "정보를 찾을 수 없습니다.";
            DataTable purchasePriceDt = customsRepository.GetOfferPriceList(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                                        , product[0].Trim(), product[1].Trim(), product[2].Trim(), product[3].Trim()
                                                                        , curve[iPt].Y.ToString(), DateTime.FromOADate(Convert.ToDouble(curve[iPt].X.ToString())).ToString("yyyy-MM-dd"));

            string division = "";
            if (purchasePriceDt.Rows.Count > 0)
            {
                company = purchasePriceDt.Rows[0]["company"].ToString();
                if (purchasePriceDt.Rows[0]["division"].ToString() == "1")
                    division = "오퍼내역";
                else
                    division = "팬딩내역";
            }



            return "오퍼일자 = " + DateTime.FromOADate(Convert.ToDouble(curve[iPt].X.ToString())).ToString("yyyy-MM-dd")
                + "\n오퍼가 = " + curve[iPt].Y.ToString()
                + "\n거래처 = " + company
                + "\n품목 = " + curve.Label.Text
                + "\n\n출처 : " + division;
        }
        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DateTime sttdate, enddate;
                if (!DateTime.TryParse(txtSttdate.Text, out sttdate))
                {
                    MessageBox.Show(this, "검색기간을 확인해주세요.");
                    this.Activate();
                    return;
                }
                if (!DateTime.TryParse(txtEnddate.Text, out enddate))
                {
                    MessageBox.Show(this, "검색기간을 확인해주세요.");
                    this.Activate();
                    return;
                }
                DataGridView dgv = dgvPurchasePrice;
                dgv.Rows.Clear();
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                DataTable purchasePriceDt = purchasePriceRepository.GetPurchasePrice(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                    , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                    , "");
                if (purchasePriceDt.Rows.Count > 0)
                {
                    for (int i = 0; i < purchasePriceDt.Rows.Count; i++)
                    {
                        int n = dgv.Rows.Add();
                        dgv.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(purchasePriceDt.Rows[i]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                        dgv.Rows[n].Cells["purchase_price"].Value = purchasePriceDt.Rows[i]["purchase_price"].ToString();
                        dgv.Rows[n].Cells["company"].Value = purchasePriceDt.Rows[i]["company"].ToString();
                    }
                }
                //Chart
                if (dgvProduct.Columns[e.ColumnIndex].Name == "chk")
                {
                    dgvProduct.Rows[e.RowIndex].Cells["chk"].Value = !Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["chk"].Value);
                    dgvProduct.EndEdit();
                    SetChart();

                }
                //단가선
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                double price;
                if (row.Cells["price"].Value == null || !double.TryParse(row.Cells["price"].Value.ToString(), out price))
                {
                    price = 0;
                }

                var xmax = zedGraphControl1.GraphPane.XAxis.Scale.Max;
                var xmin = zedGraphControl1.GraphPane.XAxis.Scale.Min;
                LineObj line = new LineObj(Color.Violet, xmin, price, xmax, price);
                line.ZOrder = ZOrder.E_BehindCurves;
                line.Tag = price.ToString("#,##0.00");
                zedGraphControl1.GraphPane.GraphObjList.Add(line);
                zedGraphControl1.Refresh();
            }
        }
        #endregion

        #region dgvPurchasePrice event
        //Datagridview rowheader numbering
        private void dgvPurchasePrice_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        #endregion

    }
}
