using AdoNetWindow.PurchaseManager;
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
    public partial class ExcelManager : Form
    {
        List<string> colText = new List<string>();
        List<string> colName = new List<string>();
        PriceComparison pc = null;
        CostAccounting ca = null;
        bool isExcel = true;
        public ExcelManager(PriceComparison priceComparison, List<string> colTxt, List<string> colNme)
        {
            InitializeComponent();
            colText = colTxt;
            colName = colNme;
            pc = priceComparison;
        }
        public ExcelManager(CostAccounting costAccounting, List<string> colTxt, List<string> colNme, bool excel = true)
        {
            InitializeComponent();
            colText = colTxt;
            colName = colNme;
            ca = costAccounting;
            isExcel = excel;
        }

        private void ExcelManager_Load(object sender, EventArgs e)
        {
            dgvColumns.Columns["col_name"].Visible = false;
            for (int i = 0; i < colText.Count; i++)
            {
                int n = dgvColumns.Rows.Add();
                //dgvColumns.Rows[n].Cells["chk"].Value = true;
                dgvColumns.Rows[n].Cells["col_text"].Value = colText[i];
                dgvColumns.Rows[n].Cells["col_name"].Value = colName[i];
                if (!isExcel)
                {
                    if (colName[i] == "product"
                      || colName[i] == "origin"
                      || colName[i] == "sizes"
                      || colName[i] == "unit"
                      || colName[i] == "box_weight"
                      || colName[i] == "cost_unit"
                      || colName[i] == "company"
                      || colName[i] == "updatetime"
                      || colName[i] == "unit_price"
                      || colName[i] == "assort"
                      || colName[i] == "custom"
                      || colName[i] == "tax"
                      || colName[i] == "assort_margin1"
                      || colName[i] == "incidental_expense"
                      || colName[i] == "exchange_rate"
                      || colName[i] == "cost_price")
                    {
                        dgvColumns.Rows[n].Cells["chk"].Value = true;
                    }
                }
            }
            SetHeaderStyle();
        }
        #region Button Click
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvColumns.Rows.Count == 0)
            {
                this.Dispose();
            }
            else
            {
                int cnt = 0;
                for (int i = 0; i < colText.Count; i++)
                {
                    bool isCheck = Convert.ToBoolean(dgvColumns.Rows[i].Cells["chk"].Value);
                    if (isCheck)
                        cnt++;
                }

                if (cnt == 0)
                {
                    MessageBox.Show(this, "선택한 컬럼이 없습니다.");
                    this.Activate();
                    return;
                }
                //선택한 컬럼
                List<string> col_list = new List<string>();
                for (int i = 0; i < colText.Count; i++)
                {
                    bool isCheck = Convert.ToBoolean(dgvColumns.Rows[i].Cells["chk"].Value);
                    if (isCheck)
                    {
                        col_list.Add(dgvColumns.Rows[i].Cells["col_name"].Value.ToString());
                    }
                }
                //Excel 생성
                if (pc != null)
                    pc.GetExeclColumn(col_list);
                else if (ca != null)
                { 
                    if(isExcel)
                        ca.GetExeclColumn(col_list);
                    else
                        ca.GetColumnSetting(col_list);
                }
                    
                this.Dispose();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void ExcelManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnSelect.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        #endregion

        #region Datagridview event
        private void dgvColumns_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                bool isCheck = !Convert.ToBoolean(dgvColumns.Rows[0].Cells["chk"].Value);
                for (int i = 0; i < dgvColumns.Rows.Count; i++)
                {
                    dgvColumns.Rows[i].Cells["chk"].Value = isCheck;
                }
            }
        }
        #endregion

        #region Method
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvColumns;
            //기본파랑
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Regular);

            // 전체 컬럼의 Sorting 기능 차단 
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion
    }
}
