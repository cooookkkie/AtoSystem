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

namespace AdoNetWindow.Pending
{
    public partial class ProductSearching : Form
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        string[] productInfo = new string[4];
        UsersModel um;

        public ProductSearching(UsersModel uModel, string product, string origin, string sizes, string unit = "")
        {
            um = uModel;
            CallProcedure();
            InitializeComponent();
            SetDgvStyleSetting();
            txtProduct.Text = product;
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtUnit.Text = unit;
        }

        #region Method
        private void SetDgvStyleSetting()
        {
            DataGridView dgv = dgvProduct;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["unit"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["whether_use"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["whether_use"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);
        }
        public string[] GetProduct(Point p)
        {
            GetData();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
            this.Location = p;
            return productInfo;
        }
        //SEAOVER 프로시져 호출하기
        private void CallProcedure()
        {
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //Seaover 사번이 없는경우 수정
            if (um.seaover_id == null || string.IsNullOrEmpty(um.seaover_id))
            {
                MessageBox.Show(this, "내정보에서 SEAOVER 사번을 입력해주세요.");
                this.Activate();
                Config.EditMyInfo emi = new Config.EditMyInfo(um);
                um = emi.UpdateSeaoverId();
            }
            string seaover_id = um.seaover_id;
            //업체별시세현황 스토어프로시져 호출
            if (seaoverRepository.CallStoredProcedure(seaover_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this,"호출 내용이 없음");
                this.Activate();
                return;
            }
        }
        private void GetData()
        {
            this.dgvProduct.SelectionChanged -= new System.EventHandler(this.dgvProduct_SelectionChanged);
            dgvProduct.Rows.Clear();
            DataTable seaoverProductDt = seaoverRepository.GetProductTable2(txtProduct.Text, false, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            DataTable customsProductDt = customsRepository.GetProductTable(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            bool isUserd = cbIsUsed.Checked;
            if (seaoverProductDt.Rows.Count > 0)
            {
                DataTable otherCostDt = productOtherCostRepository.GetProduct(txtProduct.Text, false, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
                for (int i = 0; i < seaoverProductDt.Rows.Count; i++)
                {
                    //사용한 품목만
                    if (!isUserd)
                    {
                        if (customsProductDt.Rows.Count > 0)
                        {
                            string whr = "product = '" + seaoverProductDt.Rows[i]["품명"].ToString() + "'"
                                + " AND origin = '" + seaoverProductDt.Rows[i]["원산지"].ToString() + "'"
                                + " AND sizes = '" + seaoverProductDt.Rows[i]["규격"].ToString() + "'"
                                + " AND box_weight = '" + seaoverProductDt.Rows[i]["SEAOVER단위"].ToString() + "'";
                            DataRow[] dtRow = customsProductDt.Select(whr);

                            if (dtRow.Length > 0)
                            {
                                int n = dgvProduct.Rows.Add();

                                dgvProduct.Rows[n].Cells["product"].Value = seaoverProductDt.Rows[i]["품명"].ToString();
                                dgvProduct.Rows[n].Cells["origin"].Value = seaoverProductDt.Rows[i]["원산지"].ToString();
                                dgvProduct.Rows[n].Cells["sizes"].Value = seaoverProductDt.Rows[i]["규격"].ToString();
                                dgvProduct.Rows[n].Cells["unit"].Value = seaoverProductDt.Rows[i]["SEAOVER단위"].ToString();
                                dgvProduct.Rows[n].Cells["whether_use"].Value = "★";


                                //품목 추가등록정보
                                DataRow[] arrRows = null;
                                arrRows = otherCostDt.Select($"product = '{seaoverProductDt.Rows[i]["품명"].ToString()}' " +
                                                             $"AND origin = '{seaoverProductDt.Rows[i]["원산지"].ToString()}'" +
                                                             $"AND sizes = '{seaoverProductDt.Rows[i]["규격"].ToString()}'" +
                                                             $"AND unit = '{seaoverProductDt.Rows[i]["SEAOVER단위"].ToString()}'");
                                if (arrRows.Length > 0)
                                {
                                    dgvProduct.Rows[n].Cells["cost_unit"].Value = arrRows[0]["cost_unit"].ToString();
                                }
                            }
                        }
                    }
                    //모든 품목
                    else
                    { 
                        int n = dgvProduct.Rows.Add();

                        dgvProduct.Rows[n].Cells["product"].Value = seaoverProductDt.Rows[i]["품명"].ToString();
                        dgvProduct.Rows[n].Cells["origin"].Value = seaoverProductDt.Rows[i]["원산지"].ToString();
                        dgvProduct.Rows[n].Cells["sizes"].Value = seaoverProductDt.Rows[i]["규격"].ToString();
                        dgvProduct.Rows[n].Cells["unit"].Value = seaoverProductDt.Rows[i]["SEAOVER단위"].ToString();

                        if (customsProductDt.Rows.Count > 0)
                        {
                            string whr = "product = '" + seaoverProductDt.Rows[i]["품명"].ToString() + "'"
                                + " AND origin = '" + seaoverProductDt.Rows[i]["원산지"].ToString() + "'"
                                + " AND sizes = '" + seaoverProductDt.Rows[i]["규격"].ToString() + "'"
                                + " AND box_weight = '" + seaoverProductDt.Rows[i]["SEAOVER단위"].ToString() + "'";
                            DataRow[] dtRow = customsProductDt.Select(whr);

                            if (dtRow.Length > 0)
                            {
                                dgvProduct.Rows[n].Cells["whether_use"].Value = "★";
                            }
                        }
                        //품목 추가등록정보
                        DataRow[] arrRows = null;
                        arrRows = otherCostDt.Select($"product = '{seaoverProductDt.Rows[i]["품명"].ToString()}' " +
                                                     $"AND origin = '{seaoverProductDt.Rows[i]["원산지"].ToString()}'" +
                                                     $"AND sizes = '{seaoverProductDt.Rows[i]["규격"].ToString()}'" +
                                                     $"AND unit = '{seaoverProductDt.Rows[i]["SEAOVER단위"].ToString()}'");
                        if (arrRows.Length > 0)
                        {
                            dgvProduct.Rows[n].Cells["cost_unit"].Value = arrRows[0]["cost_unit"].ToString();
                        }
                    }
                }
                //정렬
                DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvProduct);
                DataView tv = new DataView(tb);
                if (tb.Rows.Count > 0)
                {
                    tv.Sort = " whether_use DESC, product, origin, sizes";
                    tb = tv.ToTable();
                    //재출력
                    dgvProduct.Rows.Clear();
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {
                        //Row 추가
                        int n = dgvProduct.Rows.Add();
                        dgvProduct.Rows[n].Cells["product"].Value = tb.Rows[i]["product"].ToString();
                        dgvProduct.Rows[n].Cells["origin"].Value = tb.Rows[i]["origin"].ToString();
                        dgvProduct.Rows[n].Cells["sizes"].Value = tb.Rows[i]["sizes"].ToString();
                        dgvProduct.Rows[n].Cells["unit"].Value = tb.Rows[i]["unit"].ToString();
                        dgvProduct.Rows[n].Cells["whether_use"].Value = tb.Rows[i]["whether_use"].ToString();
                    }
                }
                dgvProduct.Focus();
            }
            this.dgvProduct.SelectionChanged += new System.EventHandler(this.dgvProduct_SelectionChanged);
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                int rowIndex = dgvProduct.SelectedRows[0].Index;

                // 모든 행의 CheckBox값을 False로 설정
                for (int i = 0; i < dgvProduct.RowCount; i++) 
                { 
                    dgvProduct[0, i].Value = false; 
                    // 0은 CheckBox가 있는 열 번호
                }

                dgvProduct.Rows[rowIndex].Cells["select"].Value = true;
            }
        }
        private void dgvProduct_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                productInfo[0] = dgvProduct.SelectedRows[0].Cells["product"].Value.ToString();
                productInfo[1] = dgvProduct.SelectedRows[0].Cells["origin"].Value.ToString();
                productInfo[2] = dgvProduct.SelectedRows[0].Cells["sizes"].Value.ToString();
                productInfo[3] = dgvProduct.SelectedRows[0].Cells["unit"].Value.ToString();
                this.Dispose();
            }
        }
        #endregion

        #region Key event
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            if (tb.Name != "dgvProduct")
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

                return base.ProcessCmdKey(ref msg, keyData);
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers != Keys.Control && e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void ProductSearching_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        {
                            GetData();
                            break;
                        }
                    case Keys.S:
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            productInfo[0] = dgvProduct.SelectedRows[0].Cells["product"].Value.ToString();
                            productInfo[1] = dgvProduct.SelectedRows[0].Cells["origin"].Value.ToString();
                            productInfo[2] = dgvProduct.SelectedRows[0].Cells["sizes"].Value.ToString();
                            productInfo[3] = dgvProduct.SelectedRows[0].Cells["unit"].Value.ToString();
                            this.Dispose();
                        }
                        else
                        {
                            MessageBox.Show(this,"품목을 선택해주세요.");
                            this.Activate();
                        }
                        break;
                    case Keys.N:
                        {
                            txtProduct.Text = "";
                            txtOrigin.Text = "";
                            txtSizes.Text = "";
                            txtUnit.Text = "";
                            //txtDivision.Text = "";
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
            else if (e.Modifiers == Keys.Control)
            {
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        {
                            cbIsUsed.Checked = !cbIsUsed.Checked;
                            break;
                        }
                }
            }
        }
        #endregion

        #region Button
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                productInfo[0] = dgvProduct.SelectedRows[0].Cells["product"].Value.ToString();
                productInfo[1] = dgvProduct.SelectedRows[0].Cells["origin"].Value.ToString();
                productInfo[2] = dgvProduct.SelectedRows[0].Cells["sizes"].Value.ToString();
                productInfo[3] = dgvProduct.SelectedRows[0].Cells["unit"].Value.ToString();
                this.Dispose();
            }
            else
            {
                MessageBox.Show(this,"품목을 선택해주세요.");
                this.Activate();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            productInfo = null;
            this.Dispose();
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion
        
    }
}
