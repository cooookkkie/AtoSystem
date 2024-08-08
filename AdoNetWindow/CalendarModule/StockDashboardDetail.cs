using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using Repositories;
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

namespace AdoNetWindow.CalendarModule
{
    public partial class StockDashboardDetail : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        IStockRepository stockrepository = new StockRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        DataTable customDt = null;
        double customRate;
        calendar cd;
        UsersModel um;
        public StockDashboardDetail(int typeIdx, calendar cal, UsersModel uModel)
        {
            InitializeComponent();
            cd = cal;
            um = uModel;
            switch (typeIdx)
            {
                case 4:
                    cbDivision.Text = "대행재고";
                    break;
                case 5:
                    cbDivision.Text = "유산스";
                    break;
                case 6:
                    cbDivision.Text = "미결제재고";
                    break;
            }
        }
        #region Method

        private void GetData()
        {
            if (string.IsNullOrEmpty(cbDivision.Text))
            {
                MessageBox.Show(this,"구분을 선택해주세요.");
                return;
            }
            //초기화
            dgvProduct.Rows.Clear();
            double total_amount = 0;
            customDt = customsRepository.GetPayInfo("");
            customRate = common.GetExchangeRateKEBBank("USD");
            //데이터 출력
            if (cbDivision.Text == "선T/T재고")
            {
                //선T/T재고
                DataTable ttModel = customsRepository.GetPrepayment();
                for (int i = 0; i < ttModel.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["ato_no"].Value = ttModel.Rows[i]["ato_no"].ToString();
                    dgvProduct.Rows[n].Cells["amount"].Value = (Convert.ToDouble(ttModel.Rows[i]["total_price"].ToString()) * customRate).ToString("#,##0");
                    DateTime dt;
                    if (DateTime.TryParse(ttModel.Rows[i]["payment_date"].ToString(), out dt))
                        dgvProduct.Rows[n].Cells["payment_date"].Value = dt.ToString("yyyy-MM-dd");
                    else
                        dgvProduct.Rows[n].Cells["payment_date"].Value = ttModel.Rows[i]["payment_date"].ToString();
                    total_amount += (Convert.ToDouble(ttModel.Rows[i]["total_price"].ToString()) * customRate);
                }
                //총 금액
            }
            //나머지
            else
            { 
                DataTable productDt = stockrepository.GetDashboardDetail(cbDivision.Text);
                if (productDt.Rows.Count > 0)
                {    
                    for (int i = 0; i < productDt.Rows.Count; i++)
                    {
                        int n = dgvProduct.Rows.Add();
                        DataGridViewRow row = dgvProduct.Rows[n];

                        row.Cells["ato_no"].Value = productDt.Rows[i]["AtoNo"].ToString();
                        row.Cells["amount"].Value = Convert.ToDouble(productDt.Rows[i]["재고금액"].ToString()).ToString("#,##0");

                        if (customDt != null && !string.IsNullOrEmpty(productDt.Rows[i]["AtoNo"].ToString()))
                        { 
                            string whr = "ato_no= '" + productDt.Rows[i]["AtoNo"].ToString() + "'";
                            DataRow[] dtRow = customDt.Select(whr);
                            if (dtRow.Length > 0)
                            {
                                for (int j = 0; j < dtRow.Length; j++)
                                {
                                    if (dtRow[j]["payment_date"] != null && !string.IsNullOrEmpty(dtRow[j]["payment_date"].ToString()))
                                    {
                                        DateTime dt;
                                        if (DateTime.TryParse(dtRow[j]["payment_date"].ToString(), out dt))
                                            row.Cells["payment_date"].Value = dt.ToString("yyyy-MM-dd");
                                        else
                                            row.Cells["payment_date"].Value = dtRow[j]["payment_date"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        total_amount += Convert.ToDouble(productDt.Rows[i]["재고금액"].ToString());
                    }
                }
            }
            txtTotalAmount.Text = total_amount.ToString("#,##0");
        }
        #endregion

        #region Button, ComboBox event
        private void cbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region key event
        private void StockDashboardDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.M:
                        cbDivision.Focus();
                        break;
                    case Keys.Q:
                        GetData();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        this.Dispose();
                        break;
                }
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

                    
                    if (dgvProduct.Rows[row].Cells["ato_no"].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[row].Cells["ato_no"].Value.ToString()))
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("팬딩내역");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvProduct, e.Location);
                    }
                        
                }
            }
            catch
            { }
        }

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    //Selection
                    dgvProduct.ClearSelection();
                    DataGridViewRow selectRow = this.dgvProduct.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;
                }
            }
            catch
            {
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (dgvProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row = dgvProduct.SelectedRows[0];
                    if (row.Index < 0)
                    {
                        return;
                    }
                    switch (e.ClickedItem.Text)
                    {
                        case "팬딩내역":
                            Pending.UnPendingManager upm = new UnPendingManager(cd, um, dgvProduct.SelectedRows[0].Cells["ato_no"].Value.ToString(), null);
                            upm.Show();
                            break;
                    }
                }
                catch
                { }
            }
        }
        #endregion
    }
}
