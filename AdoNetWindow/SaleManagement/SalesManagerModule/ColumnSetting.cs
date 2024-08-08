using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    public partial class ColumnSetting : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        DataTable resultDt = null;
        DataGridView dgv;
        public ColumnSetting(DataGridView dgv)
        {
            InitializeComponent();
            this.dgv = dgv;
        }

        private void ColumnSetting_Load(object sender, EventArgs e)
        {
            AddColumnInfo();
        }

        #region Method
        private void AddColumnInfo()
        {
            dgvColumn.Rows.Clear();

            int n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "거래처명";
            dgvColumn.Rows[n].Cells["column_name"].Value = "company";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["company"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["company"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "그룹";
            dgvColumn.Rows[n].Cells["column_name"].Value = "group_name";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["group_name"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["group_name"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "사업자번호";
            dgvColumn.Rows[n].Cells["column_name"].Value = "registration_number";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["registration_number"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["registration_number"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "주소";
            dgvColumn.Rows[n].Cells["column_name"].Value = "address";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["address"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["address"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "대표자";
            dgvColumn.Rows[n].Cells["column_name"].Value = "ceo";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["ceo"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["ceo"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "전화번호";
            dgvColumn.Rows[n].Cells["column_name"].Value = "tel";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["tel"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["tel"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "팩스번호";
            dgvColumn.Rows[n].Cells["column_name"].Value = "fax";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["fax"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["fax"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "휴대폰";
            dgvColumn.Rows[n].Cells["column_name"].Value = "phone";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["phone"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["phone"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "기타연락처";
            dgvColumn.Rows[n].Cells["column_name"].Value = "other_phone";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["other_phone"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["other_phone"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "Email";
            dgvColumn.Rows[n].Cells["column_name"].Value = "email";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["email"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["email"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "유통";
            dgvColumn.Rows[n].Cells["column_name"].Value = "distribution";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["distribution"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["distribution"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "취급품목1";
            dgvColumn.Rows[n].Cells["column_name"].Value = "handling_item";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["handling_item"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["handling_item"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "취급품목2";
            dgvColumn.Rows[n].Cells["column_name"].Value = "seaover_handling_item";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["seaover_handling_item"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["seaover_handling_item"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "비고";
            dgvColumn.Rows[n].Cells["column_name"].Value = "remark";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["remark"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["remark"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "비고2";
            dgvColumn.Rows[n].Cells["column_name"].Value = "remark5";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["remark4"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["remark4"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "비고3";
            dgvColumn.Rows[n].Cells["column_name"].Value = "remark6";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["remark4"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["remark4"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "상태";
            dgvColumn.Rows[n].Cells["column_name"].Value = "remark4";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["remark4"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["remark4"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "업종";
            dgvColumn.Rows[n].Cells["column_name"].Value = "industry_type";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["industry_type"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["industry_type"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "업종2";
            dgvColumn.Rows[n].Cells["column_name"].Value = "industry_type2";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["industry_type2"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["industry_type2"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "업종순위";
            dgvColumn.Rows[n].Cells["column_name"].Value = "industry_type_rank";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["industry_type_rank"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["industry_type_rank"].Width;


            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "아토담당";
            dgvColumn.Rows[n].Cells["column_name"].Value = "ato_manager";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["ato_manager"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["ato_manager"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "취급X";
            dgvColumn.Rows[n].Cells["column_name"].Value = "isNonHandled";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["isNonHandled"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["isNonHandled"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "팩스X";
            dgvColumn.Rows[n].Cells["column_name"].Value = "isNotSendFax";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["isNotSendFax"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["isNotSendFax"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "수정일";
            dgvColumn.Rows[n].Cells["column_name"].Value = "sales_updatetime";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["sales_updatetime"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["sales_updatetime"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "매출일";
            dgvColumn.Rows[n].Cells["column_name"].Value = "current_sale_date";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["current_sale_date"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["current_sale_date"].Width;

            n = dgvColumn.Rows.Add();
            dgvColumn.Rows[n].Cells["column_header_txt"].Value = "최근영업내용";
            dgvColumn.Rows[n].Cells["column_name"].Value = "sales_contents";
            dgvColumn.Rows[n].Cells["isVisible"].Value = dgv.Columns["sales_contents"].Visible;
            dgvColumn.Rows[n].Cells["column_width"].Value = dgv.Columns["sales_contents"].Width;
        }

        public DataTable GetColumnSetting()
        {
            this.ShowDialog();

            return this.resultDt;
        }
        #endregion


        #region Key event
        private void ColumnSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode) 
                {
                    case Keys.A:
                        btnRegistration.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnRegistration_Click(object sender, EventArgs e)
        {
            resultDt = common.ConvertDgvToDataTable(dgvColumn);
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            resultDt = null;
            this.Dispose();
        }
        #endregion

        #region Datagridview event
        private void dgvColumn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvColumn.Columns[e.ColumnIndex].Name == "isVisible")
                {
                    bool isChecked;
                    if (dgvColumn.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !bool.TryParse(dgvColumn.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out isChecked))
                        isChecked = false;

                    dgvColumn.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !isChecked;
                    dgvColumn.EndEdit();
                }
            }
        }
        #endregion
    }
}
