using AdoNetWindow.Common.FaxManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.DuplicateCompany
{
    public partial class ExcelColumnManager : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        DuplicateCompany dc = null;
        FaxManager fm = null;
        DataTable resultDt = null;
        public ExcelColumnManager(DuplicateCompany dc, DataTable columnDt)
        {
            InitializeComponent();
            this.dc = dc;
            if (columnDt != null && columnDt.Columns.Count > 0)
            {
                for (int i = 0; i < columnDt.Columns.Count; i++) 
                {
                    int n = dgvColumns.Rows.Add();
                    dgvColumns.Rows[n].Cells["excel_column"].Value = columnDt.Columns[i].ToString();
                    dgvColumns.Rows[n].Cells["div"].Value = "➞";
                    DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("거래처명");
                    cCell.Items.Add("사업자번호");
                    cCell.Items.Add("주소");
                    cCell.Items.Add("대표자");
                    cCell.Items.Add("전화번호");
                    cCell.Items.Add("팩스번호");
                    cCell.Items.Add("휴대폰");
                    cCell.Items.Add("기타연락처");
                    cCell.Items.Add("E-MAIL");
                    cCell.Items.Add("비고");
                    cCell.Items.Add("유통");
                    cCell.Items.Add("취급품목");
                    cCell.Items.Add("업종");

                    cCell.Items.Add("연도");
                    cCell.Items.Add("자본총계");
                    cCell.Items.Add("부채총계");
                    cCell.Items.Add("매출총액");
                    cCell.Items.Add("당기순이익");

                    dgvColumns.Rows[n].Cells["input_column"] = cCell;

                    if (cCell.Items.Contains(columnDt.Columns[i].ToString()))
                        dgvColumns.Rows[n].Cells["input_column"].Value = columnDt.Columns[i].ToString();
                    else
                    {
                        switch (columnDt.Columns[i].ToString().Replace(" ", "").Trim())
                        {
                            case "거래처":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "거래처명";
                                break;
                            case "상호":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "거래처명";
                                break;
                            case "상호명":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "거래처명";
                                break;
                            case "업체":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "거래처명";
                                break;
                            case "업체명":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "거래처명";
                                break;
                            case "회사명":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "거래처명";
                                break;

                            case "전화":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "전화번호";
                                break;
                            case "번호":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "전화번호";
                                break;
                            case "TEL":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "전화번호";
                                break;
                            case "Tel":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "전화번호";
                                break;
                            case "tel":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "전화번호";
                                break;

                            case "팩스":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "FAX":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "Fax":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "fax":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;

                            case "휴대폰번호":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "휴대폰";
                                break;
                            case "PHONE":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "휴대폰";
                                break;
                            case "Phone":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "휴대폰";
                                break;
                            case "phone":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "휴대폰";
                                break;

                            case "email":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "E-MAIL";
                                break;
                            case "이메일":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "E-MAIL";
                                break;
                            case "e-mail":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "E-MAIL";
                                break;
                            case "mail":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "E-MAIL";
                                break;


                            case "CEO":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "대표자명";
                                break;
                            case "대표":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "대표자명";
                                break;
                            case "사장":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "대표자명";
                                break;
                        }
                    }
                }
            }
        }
        public ExcelColumnManager(FaxManager fm, DataTable columnDt)
        {
            InitializeComponent();
            this.fm = fm;
            if (columnDt != null && columnDt.Columns.Count > 0)
            {
                for (int i = 0; i < columnDt.Columns.Count; i++)
                {
                    int n = dgvColumns.Rows.Add();
                    dgvColumns.Rows[n].Cells["excel_column"].Value = columnDt.Columns[i].ToString();
                    dgvColumns.Rows[n].Cells["div"].Value = "➞";
                    DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("팩스번호");
                    cCell.Items.Add("To");
                    cCell.Items.Add("첨부파일");

                    dgvColumns.Rows[n].Cells["input_column"] = cCell;

                    if (cCell.Items.Contains(columnDt.Columns[i].ToString()))
                        dgvColumns.Rows[n].Cells["input_column"].Value = columnDt.Columns[i].ToString();
                    else
                    {
                        switch (columnDt.Columns[i].ToString().Replace(" ", "").Trim())
                        {
                            case "FAX":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "Fax":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "fax":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "FAX 번호":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "팩스":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "팩스번호":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;
                            case "번호":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "팩스번호";
                                break;

                            case "귀하":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "To";
                                break;
                            case "수신처":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "To";
                                break;
                            case "받는곳":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "To";
                                break;
                            case "거래처":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "To";
                                break;
                            case "담당자":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "To";
                                break;

                            case "FILE":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "첨부파일";
                                break;
                            case "File":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "첨부파일";
                                break;
                            case "file":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "첨부파일";
                                break;
                            case "파일":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "첨부파일";
                                break;
                            case "첨부파일":
                                dgvColumns.Rows[n].Cells["input_column"].Value = "첨부파일";
                                break;
                        }
                    }
                }
            }
        }

        #region Method
        public DataTable GetColumnSetting()
        {
            this.ShowDialog();


            return resultDt;
        }
        #endregion

        #region Button
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            dgvColumns.EndEdit();
            resultDt = common.ConvertDgvToDataTable(dgvColumns);
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            resultDt = null;
            this.Dispose();
        }
        #endregion

        #region Datagridview evnet
        private void dgvColumns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvColumns.Columns[e.ColumnIndex].Name == "input_column")
                {
                    if (dc != null)
                    {
                        switch (dgvColumns.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
                        {
                            case "상호":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "company";
                                break;
                            case "거래처":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "company";
                                break;
                            case "거래처명":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "company";
                                break;
                            case "사업자번호":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "registration_number";
                                break;
                            case "사업장":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "address";
                                break;
                            case "사업장주소":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "address";
                                break;
                            case "주소":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "address";
                                break;
                            case "대표자":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "ceo";
                                break;
                            case "대표자성명":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "ceo";
                                break;
                            case "전화번호":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "tel";
                                break;
                            case "팩스번호":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "fax";
                                break;
                            case "휴대폰":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "phone";
                                break;
                            case "기타연락처":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "other_phone";
                                break;
                            case "E-MAIL":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "email";
                                break;
                            case "EMAIL":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "email";
                                break;
                            case "이메일":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "email";
                                break;
                            case "비고":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "remark";
                                break;
                            case "유통":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "distribution";
                                break;
                            case "취급품목":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "handling_item";
                                break;
                            case "업종":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "industry_type";
                                break;

                            case "연도":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "finance_year";
                                break;
                            case "자본총계":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "capital_amount";
                                break;
                            case "부채총계":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "debt_amount";
                                break;
                            case "매출총액":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "sales_amount";
                                break;
                            case "당기순이익":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "net_income_amount";
                                break;
                            case null:
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "";
                                break;
                            case "":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "";
                                break;
                        }
                    }
                    else if (fm != null)
                    {
                        switch (dgvColumns.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
                        {
                            case "팩스번호":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "fax_number";
                                break;
                            case "To":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "name";
                                break;
                            case "첨부파일":
                                dgvColumns.Rows[e.RowIndex].Cells["input_column_name"].Value = "attachment_path";
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Key event
        private void ExcelColumnManager_KeyDown(object sender, KeyEventArgs e)
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
    }
}
