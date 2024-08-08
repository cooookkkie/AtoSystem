using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.ProductCostComparison
{
    public partial class OfferPriceList : Form
    {
        private System.Windows.Threading.DispatcherTimer timer;
        string[] select = new string[4];
        private bool isVisible;
        public OfferPriceList()
        {
            InitializeComponent();
        }
        private void OfferPriceList_Load(object sender, EventArgs e)
        {
            //헤더 디자인
            dgvOfferPrice.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            timer_start();
        }
        #region Method

        public string[] Manager(DataRow[] dtRow , Point p)
        {
            DataGridView dgv = dgvOfferPrice;
            dgv.Columns["chk"].HeaderCell.Style.BackColor = Color.Red;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.Columns["offer_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            for (int i = 0; i < dtRow.Length; i++)
            {
                int n = dgvOfferPrice.Rows.Add();

                dgvOfferPrice.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(dtRow[i]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                dgvOfferPrice.Rows[n].Cells["offer_price"].Value = dtRow[i]["purchase_price"].ToString();
                dgvOfferPrice.Rows[n].Cells["fixed_tariff"].Value = dtRow[i]["fixed_tariff"].ToString();
                dgvOfferPrice.Rows[n].Cells["company"].Value = dtRow[i]["company"].ToString();
            }


            this.StartPosition = FormStartPosition.Manual;
            this.Location = p;
            this.ShowDialog();

            return select;
        }

        public string[] CostAccountingManager(DataRow[] dtRow, Point p)
        {
            DataGridView dgv = dgvOfferPrice;
            dgv.Columns["chk"].HeaderCell.Style.BackColor = Color.Red;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.Columns["offer_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["offer_price"].HeaderCell.Value = "매입가";
            dgv.Columns["updatetime"].HeaderCell.Value = "단가수정일";
            lbRemark.Text = "* 씨오버 업체별시세관리 매입내역입니다. 사용하실 매입가를 선택해주세요.";
            for (int i = 0; i < dtRow.Length; i++)
            {
                int n = dgvOfferPrice.Rows.Add();

                dgvOfferPrice.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(dtRow[i]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                dgvOfferPrice.Rows[n].Cells["offer_price"].Value = Convert.ToDouble(dtRow[i]["purchase_price"].ToString()).ToString("#,##0");
                dgvOfferPrice.Rows[n].Cells["company"].Value = dtRow[i]["company"].ToString();
            }


            this.StartPosition = FormStartPosition.Manual;
            this.Location = p;
            this.ShowDialog();

            return select;
        }
        #endregion

        #region 깜빡임 효과
        private void timer_start()
        {
            this.timer = new System.Windows.Threading.DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            this.timer.Tick += timer_Tick;
            this.timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (isVisible)
            {
                lbDirection.Visible = true;
            }
            else
            {
                lbDirection.Visible = false;
            }
            isVisible = !isVisible;
        }
        #endregion

        #region Datagridview event
        private void dgvOfferPrice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvOfferPrice.Columns[e.ColumnIndex].Name == "chk")
                {
                    foreach (DataGridViewCell cell in dgvOfferPrice.Rows[e.RowIndex].Cells)
                    {
                        if (cell.Value == null)
                            cell.Value = string.Empty;
                    }

                    select[0] = dgvOfferPrice.Rows[e.RowIndex].Cells["updatetime"].Value.ToString();
                    select[1] = dgvOfferPrice.Rows[e.RowIndex].Cells["offer_price"].Value.ToString();
                    select[2] = dgvOfferPrice.Rows[e.RowIndex].Cells["company"].Value.ToString();
                    select[3] = dgvOfferPrice.Rows[e.RowIndex].Cells["fixed_tariff"].Value.ToString();
                    //price = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["매출단가"].Value);
                    this.Dispose();
                }
            }
        }
        

        private void dgvOfferPrice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                select[0] = dgvOfferPrice.Rows[e.RowIndex].Cells["updatetime"].Value.ToString();
                select[1] = dgvOfferPrice.Rows[e.RowIndex].Cells["offer_price"].Value.ToString();
                select[2] = dgvOfferPrice.Rows[e.RowIndex].Cells["company"].Value.ToString();
                select[3] = dgvOfferPrice.Rows[e.RowIndex].Cells["fixed_tariff"].Value.ToString();
                //price = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["매출단가"].Value);
                this.Dispose();
            }
        }

        private void dgvOfferPrice_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        #region Key event
        private void OfferPriceList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        select = null;
                        this.Dispose();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        select = null;
                        this.Dispose();
                        break;
                    case Keys.Enter:
                        if (dgvOfferPrice.SelectedRows.Count > 0)
                        {
                            select[0] = dgvOfferPrice.SelectedRows[0].Cells["updatetime"].Value.ToString();
                            select[1] = dgvOfferPrice.SelectedRows[0].Cells["offer_price"].Value.ToString();
                            select[2] = dgvOfferPrice.SelectedRows[0].Cells["company"].Value.ToString();
                            select[3] = dgvOfferPrice.SelectedRows[0].Cells["fixed_tariff"].Value.ToString();
                            //price = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["매출단가"].Value);
                            this.Dispose();
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
