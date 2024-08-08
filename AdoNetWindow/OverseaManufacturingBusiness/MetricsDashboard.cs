using AdoNetWindow.Model;
using Repositories.OverseaManufacturing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.OverseaManufacturingBusiness
{
    public partial class MetricsDashboard : Form
    {
        IOverseaManufacturingRepository overseaManufacturingRepository = new OverseaManufacturingRepository();

        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um;
        public MetricsDashboard(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void MetricsDashboard_Load(object sender, EventArgs e)
        {
            txtSttdate.Text = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            SetHeaderStyle();
            //productDt = overseaManufacturingRepository.GetProduct("", "");
        }

        #region Button
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvProduct.Rows.Clear();
            DgvColumnsClear();
        }
        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            SearchProduct sp = new SearchProduct(um, this);
            sp.ShowDialog();
        }
        private void SetHeaderStyle()
        {
            dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.Beige;
            Color lightBlue = Color.FromArgb(221, 235, 247);
            dgvProduct.Columns["product_kor"].HeaderCell.Style.BackColor = lightBlue;
            dgvProduct.Columns["product_eng"].HeaderCell.Style.BackColor = lightBlue;
        }
        private void btnSearchMetrics_Click(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(txtSttdate.Text, out DateTime sttdate))
            {
                messageBox.Show(this, "시작일 값을 확인해주세요!");
                return;
            }
            else if (!DateTime.TryParse(txtEnddate.Text, out DateTime enddate))
            {
                messageBox.Show(this, "종료일 값을 확인해주세요!");
                return;
            }
            else if (dgvProduct.Rows.Count < 1)
            {
                messageBox.Show(this, "품목을 추가해주세요.");
                return;
            }
            else if (dgvProduct.Columns.Count <= 2)
            {
                messageBox.Show(this, "거래처를 추가해주세요.");
                return;
            }

            //합계란 삭제
            if(dgvProduct.Rows[dgvProduct.Rows.Count - 1].Cells["product_kor"].Value.ToString() == "합계")
                dgvProduct.Rows.Remove(dgvProduct.Rows[dgvProduct.Rows.Count - 1]);

            //초기화
            string product_kor = "";
            string product_eng = "";
            string company = "";
            foreach (DataGridViewRow row in this.dgvProduct.Rows)
            {
                for (int i = 2; i < dgvProduct.Columns.Count; i++)
                {
                    row.Cells[i].Value = "0";
                    if (!company.Contains(dgvProduct.Columns[i].Name))
                        company += $"^{dgvProduct.Columns[i].Name}";
                }

                //품명(한글)만 매칭
                if (rbOnlyKorean.Checked && row.Cells["product_kor"].Value != null && !string.IsNullOrEmpty(row.Cells["product_kor"].Value.ToString()))
                    product_kor += $"^{row.Cells["product_kor"].Value.ToString()}";
                //품명(영어)만 매칭
                else if (rbOnlyEnglish.Checked && row.Cells["product_eng"].Value != null && !string.IsNullOrEmpty(row.Cells["product_eng"].Value.ToString()))
                    product_eng += $"^{row.Cells["product_eng"].Value.ToString()}";
                //둘다 매칭
                else if (rbAllLanguage.Checked
                    && row.Cells["product_kor"].Value != null && !string.IsNullOrEmpty(row.Cells["product_kor"].Value.ToString())
                    && row.Cells["product_eng"].Value != null && !string.IsNullOrEmpty(row.Cells["product_eng"].Value.ToString()))
                {
                    product_kor += $"^{row.Cells["product_kor"].Value.ToString()}";
                    product_eng += $"^{row.Cells["product_eng"].Value.ToString()}";
                }
            }

            string search_type;
            string search_col_name;
            if (rbImporter.Checked)
            {
                search_type = "수입처";
                search_col_name = "importer";
            }
            else
            {
                search_type = "제조공장";
                search_col_name = "manufacturing";
            }

            //데이터 출력
            Dictionary<string, int> companyDic = new Dictionary<string, int>();
            DataTable metricsDt = overseaManufacturingRepository.GetData(txtSttdate.Text, txtEnddate.Text, product_kor, product_eng, search_type, company, rbExactly.Checked);

            if (dgvProduct.Columns.Count > 2)
            {
                for (int i = 2; i < dgvProduct.Columns.Count; i++)
                    companyDic.Add(dgvProduct.Columns[i].Name, 0);
            }

            if (metricsDt.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    if (dgvProduct.Rows[i].Cells["product_kor"].Value != null)
                    {
                        for (int j = 2; j < dgvProduct.ColumnCount; j++)
                        {
                            int cnt = 0;

                            string whr = "";
                            if (rbOnlyKorean.Checked && dgvProduct.Rows[i].Cells["product_kor"].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[i].Cells["product_kor"].Value.ToString()))
                                whr = $"pname_kor = '{dgvProduct.Rows[i].Cells["product_kor"].Value.ToString()}' AND {search_col_name} = '{dgvProduct.Columns[j].Name}'";
                            else if (rbOnlyEnglish.Checked && dgvProduct.Rows[i].Cells["product_eng"].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[i].Cells["product_eng"].Value.ToString()))
                                whr = $"pname_eng = '{dgvProduct.Rows[i].Cells["product_eng"].Value.ToString()}' AND {search_col_name} = '{dgvProduct.Columns[j].Name}'";
                            else if (rbAllLanguage.Checked
                                && dgvProduct.Rows[i].Cells["product_kor"].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[i].Cells["product_kor"].Value.ToString())
                                && dgvProduct.Rows[i].Cells["product_eng"].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[i].Cells["product_eng"].Value.ToString()))
                                whr = $"pname_kor = '{dgvProduct.Rows[i].Cells["product_kor"].Value.ToString()}' AND pname_eng = '{dgvProduct.Rows[i].Cells["product_eng"].Value.ToString()}' AND {search_col_name} = '{dgvProduct.Columns[j].Name}'";

                            DataRow[] dr = metricsDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                
                                if (dgvProduct.Rows[i].Cells[j].Value == null || !int.TryParse(dgvProduct.Rows[i].Cells[j].Value.ToString(), out cnt))
                                    cnt = 0;

                                cnt += dr.Length;
                                dgvProduct.Rows[i].Cells[j].Value = cnt;
                            }

                            //Dic 
                            if (companyDic.ContainsKey(dgvProduct.Columns[j].Name))
                            {
                                int pre_cnt = companyDic[dgvProduct.Columns[j].Name];
                                companyDic[dgvProduct.Columns[j].Name] = pre_cnt + cnt;
                            }
                            else
                            {
                                companyDic.Add(dgvProduct.Columns[j].Name, cnt);
                            }
                        }
                    }
                }
            }
            //합계란 추가
            int n = dgvProduct.Rows.Add();
            foreach (string key in companyDic.Keys)
            {
                dgvProduct.Rows[n].DefaultCellStyle.BackColor = Color.LightGray;
                dgvProduct.Rows[n].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
                dgvProduct.Rows[n].Cells["product_kor"].Value = "합계";
                dgvProduct.Rows[n].Cells[key].Value = companyDic[key];
            }
        }
        private void btnSttDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEndDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnSearchCompany_Click(object sender, EventArgs e)
        {
            string search_type;
            if (rbImporter.Checked)
                search_type = "수입처";
            else
                search_type = "제조공장";
            SearchCompany sc = new SearchCompany(um, this, search_type);
            sc.ShowDialog();
        }
        

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Method
        private void DgvColumnsClear()
        {
            for (int i = dgvProduct.Columns.Count - 1; i >= 2; i--)
                dgvProduct.Columns.Remove(dgvProduct.Columns[i]);
        }
        public void AddCompany(string company)
        {
            if (string.IsNullOrEmpty(company.Trim()))
            {
                //messageBox.Show(this, "거래처를 검색해주세요.");
                return;
            }

            foreach (DataGridViewColumn col in dgvProduct.Columns)
            {
                if (col.Name == company.Trim())
                {
                    //messageBox.Show(this, "이미 추가된 거래처입니다.");
                    return;
                }
            }
            //추가
            dgvProduct.Columns.Add(company, company);
            dgvProduct.Columns[company].Width = 200;
            dgvProduct.Columns[company].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        public void AddProduct(string pname_kor, string pname_eng)
        {
            if (string.IsNullOrEmpty(pname_kor.Trim()) && string.IsNullOrEmpty(pname_eng.Trim()))
            {
                //messageBox.Show(this, "거래처를 검색해주세요.");
                return;
            }

            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                if (row.Cells["product_kor"].Value.ToString() == pname_kor && row.Cells["product_eng"].Value.ToString() == pname_eng)
                {
                    //messageBox.Show(this, "이미 추가된 거래처입니다.");
                    return;
                }
            }
            //추가
            int n = dgvProduct.Rows.Add();
            dgvProduct.Rows[n].Cells["product_kor"].Value = pname_kor;
            dgvProduct.Rows[n].Cells["product_eng"].Value = pname_eng;
        }
        private AutoCompleteStringCollection SetCollection(DataTable dataTable, int colIdx)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                    collection.Add(dataTable.Rows[i][colIdx].ToString());
            }

            return collection;
        }
        #endregion

        #region Datagridview 자동완성
        /*TextBox autoText = null;
        private void dgvProduct_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewRow curRow = dgvProduct.CurrentCell.OwningRow;
            string titleText = dgvProduct.CurrentCell.OwningColumn.Name;
            if (titleText.Contains("product"))
            {
                if (e.Control is DataGridViewComboBoxEditingControl comboBoxEditingControl)
                {
                    // 자동완성 드롭다운 활성화
                    comboBoxEditingControl.DropDownStyle = ComboBoxStyle.DropDown;
                    comboBoxEditingControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxEditingControl.AutoCompleteSource = AutoCompleteSource.ListItems;
                }

                //자동완성 드롭박스
                autoText = e.Control as TextBox;
                AutoComplete(titleText);
            }
            else
            {
                if (autoText != null)
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.None;
                    autoText.AutoCompleteSource = AutoCompleteSource.None;
                    autoText.AutoCompleteCustomSource = null;
                }
            }
        }
        private void AutoComplete(string titleText)
        {
            if (autoText == null)
                return;

            if (titleText.Contains("product"))
            {
                if (autoText != null)
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                    autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    if (titleText.Contains("kor"))
                        autoText.AutoCompleteCustomSource = productKorDataCollection;
                    else if (titleText.Contains("eng"))
                        autoText.AutoCompleteCustomSource = productEngDataCollection;
                }
                else
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.None;
                    autoText.AutoCompleteSource = AutoCompleteSource.None;
                    autoText.AutoCompleteCustomSource = null;
                }
            }
            else
            {
                autoText.AutoCompleteMode = AutoCompleteMode.None;
                autoText.AutoCompleteSource = AutoCompleteSource.None;
                autoText.AutoCompleteCustomSource = null;
            }
        }*/
        #endregion

        #region Key event
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tbb = (TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
                if (tbb.Name != "txtSttdate" && tbb.Name != "txtEnddate")
                {
                    DateTime dt;
                    if (DateTime.TryParse(tbb.Text, out dt))
                        tbb.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }
        private void MetricsDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearchMetrics.PerformClick();
                        break;
                    case Keys.S:
                        btnSearchCompany.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        rbImporter.Checked = true;
                        DgvColumnsClear();
                        break;
                    case Keys.F3:
                        rbManufacturing.Checked = true;
                        DgvColumnsClear();
                        break;
                    case Keys.F4:
                        btnSearchProduct.PerformClick();
                        break;
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                    case Keys.F6:
                        btnSearchCompany.PerformClick();
                        break;
                }
            }
        }





        #endregion

        
    }
}
