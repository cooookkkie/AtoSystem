using AdoNetWindow.Model;
using AdoNetWindow.SEAOVER.TwoLine;
using Repositories;
using Repositories.Config;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AdoNetWindow.SEAOVER.TwoLine.ProductUnit;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.SEAOVER.OneLine
{
    public partial class OneLineForm : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IFormChangedDataRepository formChangedDataRepository = new FormChangedDataRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IBookmarkRepository bookmarkRepository = new BookmarkRepository();
        IUsersRepository usersRepository = new UsersRepository();
        IStockRepository stockRepository = new StockRepository();
        UsersModel um;
        List<OneFormDataModel> clipboardList = new List<OneFormDataModel>();
        int unitHeight = 14;
        bool processingFlag;
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        private System.Windows.Threading.DispatcherTimer timer;
        private int loadingCnt = 0;
        bool isExcelPriceVisible = true;
        Libs.Tools.Common common = new Libs.Tools.Common();
        public OneLineForm(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }
        private void OneLineForm_Load(object sender, EventArgs e)
        {
            string managerTxt;
            if (um.grade == null)
                managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            else if (string.IsNullOrEmpty(um.grade))
                managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            else
                managerTxt = "담당자 : " + um.user_name.ToString() + " " + um.grade.ToString() + " (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";

            lbRemark.Text = managerTxt + " / " + um.form_remark;
            SetHeaderStyle();
            timer_start();
        }

        #region Method
        public bool SearchingTxt(string find, string except)
        {
            if (!string.IsNullOrEmpty(find))
            {
                string[] find_txt = find.Split(',');
                string[] except_txt = except.Split(',');

                int rowidx = 0;
                if (dgvProduct.SelectedCells.Count > 0)
                    rowidx = dgvProduct.SelectedCells[0].RowIndex;
                if (dgvProduct.Rows.Count > 0)
                {

                    //현재행부터 끝까지
                    for (int i = rowidx; i < dgvProduct.Rows.Count; i++)
                    {

                        DataGridViewCell cell = null;
                        if (i == rowidx)
                        {
                            bool is_find = false;
                            for (int j = dgvProduct.SelectedCells[0].ColumnIndex; j < dgvProduct.ColumnCount; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                {
                                    cell = dgvProduct.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {

                                        string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvProduct.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                            if (!is_find)
                            {
                                for (int j = 0; j < dgvProduct.ColumnCount; j++)
                                {
                                    if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                    {
                                        cell = dgvProduct.Rows[i].Cells[j];
                                        if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                        {
                                            string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                            //찾을 단어
                                            if (!string.IsNullOrEmpty(find))
                                            {
                                                for (int k = 0; k < find_txt.Length; k++)
                                                {
                                                    if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                    {
                                                        is_find = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            //제외 단어
                                            if (is_find && !string.IsNullOrEmpty(except))
                                            {
                                                for (int k = 0; k < except_txt.Length; k++)
                                                {
                                                    if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                    {
                                                        is_find = false;
                                                        break;
                                                    }
                                                }
                                            }
                                            //찾기 성공!
                                            if (is_find)
                                            {
                                                dgvProduct.CurrentCell = cell;
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            for (int j = 0; j < dgvProduct.ColumnCount; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                {
                                    cell = dgvProduct.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvProduct.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //첨부터 현재행까지
                    for (int i = 0; i <= rowidx; i++)
                    {
                        DataGridViewCell cell = null;
                        if (i == rowidx)
                        {
                            for (int j = 0; j < dgvProduct.SelectedCells[0].ColumnIndex; j++)
                            //for (int j = dgvBusiness.SelectedCells[0].ColumnIndex; j < dgvBusiness.ColumnCount; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                {
                                    cell = dgvProduct.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvProduct.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < dgvProduct.ColumnCount; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                {
                                    cell = dgvProduct.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvProduct.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }


                        if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                        {
                            bool is_find = false;
                            string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                            //찾을 단어
                            if (!string.IsNullOrEmpty(find))
                            {
                                for (int k = 0; k < find_txt.Length; k++)
                                {
                                    if (!string.IsNullOrEmpty(find_txt[k].Trim()) && val.Contains(find_txt[k].Trim()))
                                    {
                                        is_find = true;
                                        break;
                                    }
                                }
                            }
                            //제외 단어
                            if (is_find && !string.IsNullOrEmpty(except))
                            {
                                for (int k = 0; k < except_txt.Length; k++)
                                {
                                    if (!string.IsNullOrEmpty(except_txt[k].Trim()) && val.Contains(except_txt[k].Trim()))
                                    {
                                        is_find = false;
                                        break;
                                    }
                                }
                            }
                            //찾기 성공!
                            if (is_find)
                            {
                                dgvProduct.CurrentCell = cell;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public void UpdateCommit(SeaoverPriceModel model, ProductUnit.UnitType ut, string updateTxt)
        {
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1 && model != null)
            {
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    string cate1 = dgv.Rows[i].Cells["category_code"].Value.ToString();
                    string cate2 = model.category_code.ToString();
                    if (!string.IsNullOrEmpty(cate1))
                    {
                        if (cate1.Substring(1) == "C" || cate1.Substring(1) == "F")
                        {
                            if (cate1.Length >= 1)
                                cate1 = cate1.Substring(1);
                            if (cate2.Length >= 1)
                                cate2 = cate2.Substring(1);
                        }
                        else
                        {
                            if (cate1.Length >= 2)
                                cate1 = cate1.Substring(0, 2);
                            if (cate2.Length >= 2)
                                cate2 = cate2.Substring(0, 2);
                        }
                    }
                    else
                    {
                        cate1 = cate1;
                        cate2 = cate2;
                    }


                    switch (ut)
                    {
                        case UnitType.Category:

                            if (cate1 == cate2)
                            {
                                model.category = updateTxt;
                                dgv.Rows[i].Cells["category"].Value = updateTxt;
                            }
                            break;

                        case UnitType.Product:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString())
                            {
                                /*model.product = updateTxt;*/
                                dgv.Rows[i].Cells["product"].Value = updateTxt;
                            }
                            break;
                        case UnitType.Origin:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                            && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString())
                            {
                                /*model.origin = updateTxt;*/
                                dgv.Rows[i].Cells["origin"].Value = updateTxt;
                            }
                            break;
                        case UnitType.sizes1:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                            && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString()
                            && dgv.Rows[i].Cells["sizes_code"].Value.ToString() == model.sizes_code.ToString())
                            {
                                /*model.sizes = updateTxt;*/
                                dgv.Rows[i].Cells["sizes1"].Value = updateTxt;
                            }
                            break;
                        case UnitType.sizes2:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                            && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString()
                            && dgv.Rows[i].Cells["sizes_code"].Value.ToString() == model.sizes_code.ToString())
                            {
                                /*model.sizes = updateTxt;*/
                                dgv.Rows[i].Cells["sizes2"].Value = updateTxt;
                            }
                            break;
                        case UnitType.costUnit:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                            && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString()
                            && dgv.Rows[i].Cells["sizes1"].Value.ToString() == model.sizes1.ToString()
                            && dgv.Rows[i].Cells["sizes2"].Value.ToString() == model.sizes2.ToString()
                            && dgv.Rows[i].Cells["unit"].Value.ToString() == model.unit.ToString()
                            && dgv.Rows[i].Cells["cost_unit"].Value.ToString() == model.cost_unit.ToString())
                            {
                                /*model.note = updateTxt;*/
                                dgv.Rows[i].Cells["cost_unit"].Value = updateTxt;
                            }
                            break;
                        case UnitType.trayPrice:

                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                                && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString()
                                && dgv.Rows[i].Cells["sizes1"].Value.ToString() == model.sizes1.ToString()
                                && dgv.Rows[i].Cells["sizes2"].Value.ToString() == model.sizes2.ToString()
                                && dgv.Rows[i].Cells["unit"].Value.ToString() == model.unit.ToString()
                                && dgv.Rows[i].Cells["cost_unit"].Value.ToString() == model.cost_unit.ToString()
                                && dgv.Rows[i].Cells["tray_price"].Value.ToString() == model.tray_price.ToString()
                                )
                            {
                                dgv.Rows[i].Cells["tray_price"].Value = updateTxt;
                            }
                            break;
                        case UnitType.boxPrice:

                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                                && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString()
                                && dgv.Rows[i].Cells["sizes1"].Value.ToString() == model.sizes1.ToString()
                                && dgv.Rows[i].Cells["sizes2"].Value.ToString() == model.sizes2.ToString()
                                && dgv.Rows[i].Cells["unit"].Value.ToString() == model.unit.ToString()
                                && dgv.Rows[i].Cells["cost_unit"].Value.ToString() == model.cost_unit.ToString()
                                && dgv.Rows[i].Cells["box_price"].Value.ToString() == model.sales_price.ToString()
                                )
                            {
                                dgv.Rows[i].Cells["box_price"].Value = updateTxt;
                            }
                            break;
                        case UnitType.isTax:

                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                                && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString()
                                && dgv.Rows[i].Cells["sizes1"].Value.ToString() == model.sizes1.ToString()
                                && dgv.Rows[i].Cells["sizes2"].Value.ToString() == model.sizes2.ToString()
                                && dgv.Rows[i].Cells["unit"].Value.ToString() == model.unit.ToString()
                                && dgv.Rows[i].Cells["cost_unit"].Value.ToString() == model.cost_unit.ToString()
                                && dgv.Rows[i].Cells["tray_price"].Value.ToString() == model.tray_price.ToString()
                                && dgv.Rows[i].Cells["box_price"].Value.ToString() == model.sales_price.ToString()
                                && dgv.Rows[i].Cells["is_tax"].Value.ToString() == model.is_tax.ToString()
                                )
                            {
                                dgv.Rows[i].Cells["is_tax"].Value = updateTxt;
                            }
                            break;
                    }
                }
            }
            CreateForm();
        }
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvProduct;
            dgv.Columns["category"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["category"].HeaderCell.Style.ForeColor = Color.Yellow;
            //dgv.Columns["category"].DefaultCellStyle.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            dgv.Columns["category"].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 153);

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes1"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["sizes1"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes2"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["sizes2"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["weight"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["weight"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["cost_unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["cost_unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["price_unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["price_unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["manager1"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["manager1"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["tray_price"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["tray_price"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["tray_price"].DefaultCellStyle.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            dgv.Columns["tray_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);

            dgv.Columns["box_price"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["box_price"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["box_price"].DefaultCellStyle.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            dgv.Columns["box_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);

            dgv.Columns["division"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["division"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["is_tax"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204);
            dgv.Columns["remark"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204);
            dgv.Columns["page"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204);
            dgv.Columns["row"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204);
        }
        private void InsertForm()
        {
            int id = seaoverRepository.getNextId();
            
            List<StringBuilder> sqlList = new List<StringBuilder>();
            InsertProductPrice(id, sqlList, "신규등록");
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void UpdateForm()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서(초밥류)", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            UsersModel cum = new UsersModel();
            cum = usersRepository.GetUserInfo2(lbCreateUser.Text);
            if (cum != null)
            {
                int id = Convert.ToInt16(lbId.Text);
                if (!Int32.TryParse(lbId.Text, out id))
                {
                    MessageBox.Show(this, "품목서 정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }
                //품목서 유효성검사
                List<FormDataModel> model = seaoverRepository.GetFormData("", id.ToString());
                if (model.Count == 0)
                {
                    MessageBox.Show(this, "품목서 정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }

                //잠금이 되있을 경우
                if (model[0].is_rock)
                {
                    FormPasswordManager fpm = new FormPasswordManager(model[0].password);
                    fpm.Owner = this;
                    if (!fpm.CheckPassword())
                    {
                        MessageBox.Show(this, "비밀번호가 틀렸습니다.");
                        this.Activate();
                        return;
                    }
                }

                try
                {
                    List<StringBuilder> sqlList = new List<StringBuilder>();    
                    StringBuilder sql = seaoverRepository.DeleteFormData(id.ToString());
                    sqlList.Add(sql);

                    InsertProductPrice(id, sqlList, "수정");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "수정할 내역을 불러와주세요.");
                    this.Activate();
                }
            }
            else
            {
                MessageBox.Show(this, "유저정보를 찾을 수 없습니다.");
                this.Activate();
            }
        }
        private void InsertProductPrice(int id, List<StringBuilder> sqlList, string mode)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서(초밥류)", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (string.IsNullOrEmpty(txtFormName.Text))
            {
                MessageBox.Show(this, "품목서 제목을 입력해주세요.");
                this.Activate();
                return;
            }
            StringBuilder sql;
            if (mode != "갱신")
            {
                if (MessageBox.Show(this, "취급품목서 내용을 " + mode + "하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }
            lbId.Text = id.ToString();
            //lbEditUser.Text = um.user_name.ToString();
            lbEditUser.Text = um.user_name;
            lbUpdatetime.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //lbUpdatedate.Text = "작성일자 : " + DateTime.Now.ToString("yyyy-MM-dd");
            //lbRemark.Text = "/ *단가 조정 필요시 문의바람* (작성일자:" + DateTime.Now.ToString("yyyy-MM-dd") + ")";
            lbEditDate.Text = "작성일자 : " + DateTime.Now.ToString("yyyy-MM-dd");

            double s;
            if (dgvProduct.Rows.Count > 0)
            {
                SetPage();
                int cnt = 1;

                for (int i = 0; i < dgvProduct.Rows.Count ; i++)
                {
                    foreach (DataGridViewCell c in dgvProduct.Rows[i].Cells)
                    {
                        if (c.Value == null)
                            c.Value = "";
                    }

                    FormDataModel model = new FormDataModel();
                    model.id = id;
                    model.sid = cnt;
                    model.form_type = 3;
                    model.form_name = txtFormName.Text;
                    if (string.IsNullOrEmpty(dgvProduct.Rows[i].Cells["accent"].Value.ToString()))
                        model.accent = false;
                    else
                        model.accent = Convert.ToBoolean(dgvProduct.Rows[i].Cells["accent"].Value);
                    model.category = dgvProduct.Rows[i].Cells["category"].Value.ToString();
                    model.category_code = dgvProduct.Rows[i].Cells["category_code"].Value.ToString();
                    model.product_code = dgvProduct.Rows[i].Cells["product_code"].Value.ToString();
                    model.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                    model.origin_code = dgvProduct.Rows[i].Cells["origin_code"].Value.ToString();
                    model.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                    
                    model.sizes_code = dgvProduct.Rows[i].Cells["sizes_code"].Value.ToString();
                    model.sizes = dgvProduct.Rows[i].Cells["sizes1"].Value.ToString();
                    model.sizes2 = dgvProduct.Rows[i].Cells["sizes2"].Value.ToString();

                    model.purchase_price = 0;
                    if (dgvProduct.Rows[i].Cells["tray_price"].Value == null)
                        model.tray_price = "-";
                    else
                        model.tray_price = dgvProduct.Rows[i].Cells["tray_price"].Value.ToString();

                    double box_price;
                    if (!double.TryParse(dgvProduct.Rows[i].Cells["box_price"].Value.ToString(), out box_price))
                    {
                        if (dgvProduct.Rows[i].Cells["box_price"].Value.ToString() == "-")
                            box_price = -1;
                        else
                            box_price = -2;
                    }
                    model.sales_price = box_price;

                    model.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                    model.price_unit = dgvProduct.Rows[i].Cells["price_unit"].Value.ToString();
                    model.unit_count = dgvProduct.Rows[i].Cells["unit_count"].Value.ToString();
                    model.seaover_unit = dgvProduct.Rows[i].Cells["seaover_unit"].Value.ToString();
                    model.weight = dgvProduct.Rows[i].Cells["weight"].Value.ToString();

                    model.division = dgvProduct.Rows[i].Cells["division"].Value.ToString(); ;
                    model.page = Convert.ToInt16(dgvProduct.Rows[i].Cells["page"].Value);
                    model.cnt = Convert.ToInt16(dgvProduct.Rows[i].Cells["cnt"].Value);
                    model.row_index = Convert.ToInt16(dgvProduct.Rows[i].Cells["row"].Value);
                    model.area = "C";

                    if (mode == "수정")
                        model.create_user = lbCreateUser.Text;
                    else if (mode == "신규등록")
                        model.create_user = um.user_name;
                    else
                        model.create_user = lbCreateUser.Text;

                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    //메모변경 
                    string managerTxt;
                    if (um.grade == null)
                        managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
                    else if (string.IsNullOrEmpty(um.grade))
                        managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
                    else
                        managerTxt = "담당자 : " + um.user_name.ToString() + " " + um.grade.ToString() + " (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";

                    managerTxt += " / " + um.form_remark;
                    if (lbRemark.Text != managerTxt)
                    {
                        model.form_remark = lbRemark.Text;
                    }

                    model.remark = dgvProduct.Rows[i].Cells["remark"].Value.ToString();
                    model.is_tax = dgvProduct.Rows[i].Cells["is_tax"].Value.ToString();
                    model.cost_unit = dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString();
                    model.edit_user = um.user_name;

                    //Form data
                    sql = seaoverRepository.InsertFormData2(model);
                    sqlList.Add(sql);

                    cnt += 1;
                }

                int results = seaoverRepository.UpdateTrans(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    MessageBox.Show(this, "완료");
                    this.Activate();
                }
            }
        }

        private void CreateForm()
        {
            int total_page;
            if (!int.TryParse(txtTotalPage.Text, out total_page))
                total_page = 1;
            lbTotal.Text = "/ " + total_page.ToString();
            int page;
            if (!int.TryParse(txtCurPage.Text, out page))
                page = 1;
            lbCurPage.Text = page.ToString();
            txtCurPage.Text = page.ToString();

            lbFormName.Text = txtFormName.Text;
            lbEditDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            SetPage();
            SetOneFormProduct(page);
        }
        private void SetPage()
        {
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    if (dgvProduct.Rows[i].Visible == false)
                        dgvProduct.Rows[i].Visible = true;
                }
            }
            dgvProduct.EndEdit();
            if (dgvProduct.Rows.Count > 0)
            {
                int cnt = 0;
                int page = 1;
                int row_index = 0;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    int row;
                    if (dgvProduct.Rows[i].Cells["row"].Value == null || !int.TryParse(dgvProduct.Rows[i].Cells["row"].Value.ToString(), out row))
                        row = 0;
                    row_index += row;
                    dgvProduct.Rows[i].Cells["cnt"].Value = row_index;
                    dgvProduct.Rows[i].Cells["page"].Value = page;
                    //넘치면
                    if (row_index >= 60 && i < dgvProduct.Rows.Count - 1)
                    {
                        page++;
                        row_index = 0;
                    }
                    cnt += row;
                    
                }
                double total_page;
                if (!double.TryParse(txtTotalPage.Text, out total_page))
                    total_page = page;

                if(total_page <= page)
                    txtTotalPage.Text = page.ToString();
            }
        }

        //1줄 품목서
        private void SetOneFormProduct(int page)
        {
            lCategory.Controls.Clear();
            lProduct.Controls.Clear();
            lOrigin.Controls.Clear();
            lSize.Controls.Clear();
            lSize2.Controls.Clear();
            lUnit.Controls.Clear();
            lPack.Controls.Clear();
            lTrayPrice.Controls.Clear();
            lBoxPrice.Controls.Clear();
            lIstax.Controls.Clear();
            lRemark.Controls.Clear();

            DataGridView dgv = dgvProduct;
            ProductUnit ui;
            SeaoverPriceModel model;
            int cHeight;
            int pHeight;
            int oHeight;
            int s1Height;
            int s2Height;
            int wHeight;
            int uHeight;
            int tHeight;
            int bHeight;
            int iHeight;
            int rHeight;


            int row = 60;
            double s;
            if (dgv.Rows.Count > 1)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        if (dgv.Rows[i].Cells[j].Value == null)
                            dgv.Rows[i].Cells[j].Value = "";
                    }
                }
                dgvProduct.EndEdit();

                //Page Left Product Unit Setting==============================================================================
                //Page Product Unit Height
                cHeight = unitHeight;
                pHeight = unitHeight;
                oHeight = unitHeight;
                s1Height = unitHeight;
                s2Height = unitHeight;
                wHeight = unitHeight;
                uHeight = unitHeight;
                tHeight = unitHeight;
                bHeight = unitHeight;
                iHeight = unitHeight;
                rHeight = unitHeight;

                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dgv.Rows[i].Cells["product"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(dgv.Rows[i].Cells["cnt"].Value.ToString()))
                        {
                            break;
                        }
                        if ((int)dgv.Rows[i].Cells["cnt"].Value == 1)
                        {
                            //row = GetPageCount(page, dgv.Rows[i].Cells["area"].Value.ToString());
                            //unitHeight = (int)((double)843 / (double)row);
                            unitHeight = 14 * Convert.ToInt16(dgv.Rows[i].Cells["row"].Value);
                            cHeight = unitHeight;
                            pHeight = unitHeight;
                            oHeight = unitHeight;
                            s1Height = unitHeight;
                            s2Height = unitHeight;
                            wHeight = unitHeight;
                            uHeight = unitHeight;
                            tHeight = unitHeight;
                            bHeight = unitHeight;
                            iHeight = unitHeight;
                            rHeight = unitHeight;
                        }

                        if (!string.IsNullOrEmpty(dgv.Rows[i].Cells["page"].Value.ToString()))
                        {
                            if (Convert.ToInt32(dgv.Rows[i].Cells["page"].Value) == page)
                            {
                                model = new SeaoverPriceModel();
                                model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                model.sizes1 = dgv.Rows[i].Cells["sizes1"].Value.ToString();
                                model.sizes2 = dgv.Rows[i].Cells["sizes2"].Value.ToString();
                                //model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                model.cost_unit = dgv.Rows[i].Cells["cost_unit"].Value.ToString();
                                model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                                model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();

                                if (dgv.Rows[i].Cells["tray_price"].Value == null)
                                    model.tray_price = "-";
                                else
                                    model.tray_price = dgv.Rows[i].Cells["tray_price"].Value.ToString();

                                double box_price;
                                if (dgv.Rows[i].Cells["box_price"].Value == null || !double.TryParse(dgv.Rows[i].Cells["box_price"].Value.ToString(), out box_price))
                                    box_price = 0;
                                model.sales_price = box_price;

                                model.is_tax = dgv.Rows[i].Cells["is_tax"].Value.ToString();
                                model.remark = dgv.Rows[i].Cells["remark"].Value.ToString();

                                //model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                //model.sales_price = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString());
                                if (double.TryParse(dgv.Rows[i].Cells["box_price"].Value.ToString(), out s))
                                    model.sales_price = s;
                                else
                                {
                                    if (dgv.Rows[i].Cells["box_price"].Value.ToString() == "-")
                                        model.sales_price = -1;
                                    else if (dgv.Rows[i].Cells["box_price"].Value.ToString() == "-")
                                        model.sales_price = -2;
                                    else if (dgv.Rows[i].Cells["box_price"].Value.ToString().Contains("통관예정"))
                                        model.sales_price = -3;

                                }
                                //행수가 2 이상일 경우
                                if (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) > 1)
                                {
                                    cHeight = cHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    pHeight = pHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    oHeight = oHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    s1Height = s1Height + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    s2Height = s2Height + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    wHeight = wHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    uHeight = uHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    tHeight = tHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    bHeight = bHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    iHeight = iHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                    rHeight = rHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["row"].Value) - 1);
                                }

                                /*cHeight = cHeight;
                                pHeight = pHeight;
                                oHeight = oHeight;
                                wHeight = wHeight;
                                zHeight = zHeight;
                                rHeight = rHeight;*/

                                //머지항목=========================================================================================================
                                string current_page = dgv.Rows[i].Cells["page"].Value.ToString();
                                string next_page;
                                if ((dgv.Rows.Count - 1) > i)
                                    next_page = dgv.Rows[i + 1].Cells["page"].Value.ToString();
                                else
                                    next_page = "END";

                                bool isFlag = true;
                                //대분류
                                if (isFlag)
                                { 
                                    if (i == dgv.Rows.Count - 1 
                                        || dgv.Rows[i].Cells["category"].Value.ToString() != dgv.Rows[i + 1].Cells["category"].Value.ToString()
                                        || current_page != next_page)
                                    {
                                        ui = new ProductUnit(this, UnitType.Category, model, cHeight, 1);
                                        lCategory.Controls.Add(ui);
                                        cHeight = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.Product, model, pHeight, 1);
                                        lProduct.Controls.Add(ui);
                                        pHeight = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.Origin, model, oHeight, 1);
                                        lOrigin.Controls.Add(ui);
                                        oHeight = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.sizes1, model, s1Height, 1);
                                        lSize.Controls.Add(ui);
                                        s1Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.sizes2, model, s2Height, 1);
                                        lSize2.Controls.Add(ui);
                                        s2Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                        lUnit.Controls.Add(ui);
                                        wHeight = (int)unitHeight;

                                        isFlag = false;
                                    }
                                    else
                                    {
                                        cHeight += (int)unitHeight;
                                    }
                                }

                                //품목
                                if (isFlag)
                                {
                                    if (i == dgv.Rows.Count - 1
                                        || dgv.Rows[i].Cells["product"].Value.ToString() != dgv.Rows[i + 1].Cells["product"].Value.ToString()
                                        || current_page != next_page)
                                    {
                                        ui = new ProductUnit(this, UnitType.Product, model, pHeight, 1);
                                        lProduct.Controls.Add(ui);
                                        pHeight = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.Origin, model, oHeight, 1);
                                        lOrigin.Controls.Add(ui);
                                        oHeight = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.sizes1, model, s1Height, 1);
                                        lSize.Controls.Add(ui);
                                        s1Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.sizes2, model, s2Height, 1);
                                        lSize2.Controls.Add(ui);
                                        s2Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                        lUnit.Controls.Add(ui);
                                        wHeight = (int)unitHeight;

                                        isFlag = false;
                                    }
                                    else
                                    {
                                        pHeight += (int)unitHeight;
                                    }
                                }

                                //원산지
                                if (isFlag)
                                {
                                    if (i == dgv.Rows.Count - 1
                                        || dgv.Rows[i].Cells["origin"].Value.ToString() != dgv.Rows[i + 1].Cells["origin"].Value.ToString()
                                        || current_page != next_page)
                                    {
                                        ui = new ProductUnit(this, UnitType.Origin, model, oHeight, 1);
                                        lOrigin.Controls.Add(ui);
                                        oHeight = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.sizes1, model, s1Height, 1);
                                        lSize.Controls.Add(ui);
                                        s1Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.sizes2, model, s2Height, 1);
                                        lSize2.Controls.Add(ui);
                                        s2Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                        lUnit.Controls.Add(ui);
                                        wHeight = (int)unitHeight;

                                        isFlag = false;
                                    }
                                    else
                                    {
                                        oHeight += (int)unitHeight;
                                    }
                                }

                                //사이즈1
                                if (isFlag)
                                {
                                    if (i == dgv.Rows.Count - 1
                                        || dgv.Rows[i].Cells["sizes1"].Value.ToString() != dgv.Rows[i + 1].Cells["sizes1"].Value.ToString()
                                        || current_page != next_page)
                                    {
                                        ui = new ProductUnit(this, UnitType.sizes1, model, s1Height, 1);
                                        lSize.Controls.Add(ui);
                                        s1Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.sizes2, model, s2Height, 1);
                                        lSize2.Controls.Add(ui);
                                        s2Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                        lUnit.Controls.Add(ui);
                                        wHeight = (int)unitHeight;

                                        isFlag = false;
                                    }
                                    else
                                    {
                                        s1Height += (int)unitHeight;
                                    }
                                }

                                //사이즈2
                                if (isFlag)
                                {
                                    if (i == dgv.Rows.Count - 1
                                        || dgv.Rows[i].Cells["sizes2"].Value.ToString() != dgv.Rows[i + 1].Cells["sizes2"].Value.ToString()
                                        || current_page != next_page)
                                    {
                                        ui = new ProductUnit(this, UnitType.sizes2, model, s2Height, 1);
                                        lSize2.Controls.Add(ui);
                                        s2Height = (int)unitHeight;

                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                        lUnit.Controls.Add(ui);
                                        wHeight = (int)unitHeight;

                                        isFlag = false;
                                    }
                                    else
                                    {
                                        s2Height += (int)unitHeight;
                                    }
                                }

                                //단위
                                if (isFlag)
                                {
                                    if (i == dgv.Rows.Count - 1
                                        || dgv.Rows[i].Cells["unit"].Value.ToString() != dgv.Rows[i + 1].Cells["unit"].Value.ToString()
                                        || current_page != next_page)
                                    {
                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                        lUnit.Controls.Add(ui);
                                        wHeight = (int)unitHeight;

                                        isFlag = false;
                                    }
                                    else
                                    {
                                        wHeight += (int)unitHeight;
                                    }
                                }
                                //팩
                                ui = new ProductUnit(this, UnitType.costUnit, model, uHeight, 1);
                                lPack.Controls.Add(ui);
                                uHeight = (int)unitHeight;
                                //tray단가
                                ui = new ProductUnit(this, UnitType.trayPrice, model, tHeight, 1);
                                lTrayPrice.Controls.Add(ui);
                                tHeight = (int)unitHeight;
                                //box단가
                                ui = new ProductUnit(this, UnitType.boxPrice, model, bHeight, 1);
                                lBoxPrice.Controls.Add(ui);
                                bHeight = (int)unitHeight;
                                //관세
                                ui = new ProductUnit(this, UnitType.isTax, model, iHeight, 1);
                                lIstax.Controls.Add(ui);
                                iHeight = (int)unitHeight;
                                //비고
                                ui = new ProductUnit(this, UnitType.remark, model, rHeight, 1);
                                lRemark.Controls.Add(ui);
                                rHeight = (int)unitHeight;
                            }
                        }
                    }
                }
            }
        }
        public void SetClipboardModel(List<DataGridViewRow> row)
        {
            for (int i = 0; i < row.Count; i++)
            {
                OneFormDataModel model = new OneFormDataModel();

                model.category = row[i].Cells["대분류"].Value.ToString();
                model.category_code = row[i].Cells["대분류1"].Value.ToString();

                if (model.category_code != null && !string.IsNullOrEmpty(model.category_code))
                {
                    model.category_code1 = model.category_code.Substring(0, 1);

                    int d;
                    if (!int.TryParse(model.category_code.Substring(1, 1), out d))
                    {
                        model.category_code2 = model.category_code.ToString().Substring(1, 1);
                    }

                    if (model.category_code.Length <= 5)
                        model.category_code3 = model.category_code.Substring(model.category_code.Length - 3, 3);
                    else if (model.category_code2 != null && !string.IsNullOrEmpty(model.category_code2))
                        model.category_code3 = model.category_code.Substring(2, 3);
                    else
                        model.category_code3 = model.category_code.Substring(1, 3);
                }
                else
                {
                    model.category_code1 = model.category_code;
                    model.category_code2 = model.category_code;
                    model.category_code3 = model.category_code;
                }

                model.product = row[i].Cells["품명"].Value.ToString();
                model.product_code = row[i].Cells["품목코드"].Value.ToString();
                model.origin = row[i].Cells["원산지"].Value.ToString();
                model.origin_code = row[i].Cells["원산지코드"].Value.ToString();
                model.sizes1 = row[i].Cells["규격"].Value.ToString();
                //2023-08-01 사이즈2 삭제
                /*if (model.sizes1.Contains("G*"))
                {
                    string[] txt = model.sizes1.Split(Convert.ToChar("G"));
                    if (txt.Length > 0)
                    {
                        model.sizes1 = txt[0] + "G";
                        model.sizes2 = txt[1].Substring(1, txt[1].Length - 1);
                    }
                }*/
                model.sizes_code = row[i].Cells["규격코드"].Value.ToString();
                model.unit = row[i].Cells["단위"].Value.ToString();
                model.price_unit = row[i].Cells["가격단위"].Value.ToString();
                model.unit_count = row[i].Cells["단위수량"].Value.ToString();
                model.seaover_unit = row[i].Cells["SEAOVER단위"].Value.ToString();
                model.weight = row[i].Cells["SEAOVER단위"].Value.ToString() + "KG";

                double unit_count;
                if (!double.TryParse(row[i].Cells["단위수량"].Value.ToString(), out unit_count))
                    unit_count = 1;

                double unit_cost;
                if (!double.TryParse(row[i].Cells["트레이"].Value.ToString(), out unit_cost))
                    unit_cost = 0;

                double tray;
                //팩이면서 트레이가 0일경우 단위수량으로 대체
                /*if (model.price_unit.Contains("팩"))
                { 
                    if(unit_cost == 0)
                        tray = unit_count;
                    else
                        tray = unit_cost;
                }
                else
                    tray = unit_cost;

                if (tray == 0)*/
                    tray = unit_count;
                model.cost_unit = tray.ToString();
                double box_price, tray_price;
                if (!double.TryParse(row[i].Cells["매출단가"].Value.ToString(), out box_price))
                    box_price = 0;
                //팩일때
                if (model.price_unit.Contains("팩"))
                {
                    tray_price = box_price;
                    box_price = box_price * tray;
                }
                else
                    tray_price = box_price / tray;

                //1000이하 일때는 팩인데 잘못 기재된것으로 판단
                if (tray_price < 1000)
                {
                    if (!double.TryParse(row[i].Cells["매출단가"].Value.ToString(), out tray_price))
                        tray_price = 0;
                    box_price = tray_price * tray;
                }
                //무한대 처리가 되면 박스단가로 환산
                else if (double.IsInfinity(tray_price))
                    tray_price = box_price;

                model.sales_price = tray_price;
                model.sales_price = box_price;
                model.division = row[i].Cells["구분"].Value.ToString();
                model.tray_price = tray_price;
                model.is_tax = row[i].Cells["부가세"].Value.ToString();
                model.remark = "";
                model.page = 0;
                model.row_index = 1;

                clipboardList.Add(model);
            }
        }
        public void InputFormData(int form_id)
        {
            if (form_id > 0)
            {
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                dgvProduct.Rows.Clear();
                DataTable formDt = bookmarkRepository.GetFormData(form_id);
                if (formDt.Rows.Count > 0)
                {
                    lbId.Text = formDt.Rows[0]["id"].ToString();
                    txtFormName.Text = formDt.Rows[0]["form_name"].ToString();
                    lbUpdatetime.Text = Convert.ToDateTime(formDt.Rows[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                    lbEditUser.Text = formDt.Rows[0]["edit_user"].ToString();
                    lbCreateUser.Text = formDt.Rows[0]["create_user"].ToString();

                    if (Convert.ToBoolean(formDt.Rows[0]["is_rock"].ToString()))
                    {
                        btnSettingPassword.Text = "품목서 잠금해제";
                        btnSettingPassword.ForeColor = Color.Red;
                    }
                    else
                    {
                        btnSettingPassword.Text = "품목서 잠금";
                        btnSettingPassword.ForeColor = Color.Blue;
                    }

                    txtCurPage.Text = "1";

                    int total_page = 1;
                    for (int i = 0; i < formDt.Rows.Count; i++)
                    {
                        if (total_page < Convert.ToInt32(formDt.Rows[i]["page"].ToString()))
                            total_page = Convert.ToInt32(formDt.Rows[i]["page"].ToString());

                        int n = dgvProduct.Rows.Add();
                        DataGridViewRow row = dgvProduct.Rows[n];

                        row.Cells["category_code"].Value = formDt.Rows[i]["category_code"].ToString();
                        string category_code = formDt.Rows[i]["category_code"].ToString();
                        if (category_code != null && !string.IsNullOrEmpty(category_code))
                        {
                            dgvProduct.Rows[n].Cells["category_code1"].Value = category_code.Substring(0, 1);

                            int d;
                            if (!int.TryParse(category_code.Substring(1, 1), out d))
                            {
                                dgvProduct.Rows[n].Cells["category_code2"].Value = category_code.Substring(1, 1);
                            }

                            if (category_code.Length <= 5)
                                dgvProduct.Rows[n].Cells["category_code3"].Value = category_code.Substring(category_code.Length - 3, 3);
                            else if (dgvProduct.Rows[n].Cells["category_code2"].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[n].Cells["category_code2"].Value.ToString()))
                                dgvProduct.Rows[n].Cells["category_code3"].Value = category_code.Substring(2, 3);
                            else
                                dgvProduct.Rows[n].Cells["category_code3"].Value = category_code.Substring(1, 3);
                        }
                        else
                        {
                            dgvProduct.Rows[n].Cells["category_code1"].Value = category_code;
                            dgvProduct.Rows[n].Cells["category_code2"].Value = category_code;
                            dgvProduct.Rows[n].Cells["category_code3"].Value = category_code;
                        }

                        row.Cells["category"].Value = formDt.Rows[i]["category"].ToString();
                        row.Cells["product_code"].Value = formDt.Rows[i]["product_code"].ToString();
                        row.Cells["product"].Value = formDt.Rows[i]["product"].ToString();
                        row.Cells["sizes_code"].Value = formDt.Rows[i]["sizes_code"].ToString();
                        row.Cells["sizes1"].Value = formDt.Rows[i]["sizes"].ToString();
                        
                        row.Cells["origin_code"].Value = formDt.Rows[i]["origin_code"].ToString();
                        row.Cells["origin"].Value = formDt.Rows[i]["origin"].ToString();
                        row.Cells["unit"].Value = formDt.Rows[i]["unit"].ToString();
                        row.Cells["price_unit"].Value = formDt.Rows[i]["price_unit"].ToString();
                        row.Cells["unit_count"].Value = formDt.Rows[i]["unit_count"].ToString();
                        row.Cells["seaover_unit"].Value = formDt.Rows[i]["seaover_unit"].ToString();
                        row.Cells["weight"].Value = formDt.Rows[i]["weight"].ToString();
                        row.Cells["remark"].Value = formDt.Rows[i]["remark"].ToString();
                        row.Cells["row"].Value = formDt.Rows[i]["row_index"].ToString();
                        row.Cells["page"].Value = formDt.Rows[i]["page"].ToString();
                        row.Cells["division"].Value = formDt.Rows[i]["division"].ToString();

                        //초밥류에서 저장한 품목서 데이터
                        if (formDt.Rows[i]["form_type"].ToString() == "3")
                        {
                            row.Cells["sizes1"].Value = formDt.Rows[i]["sizes"].ToString();
                            row.Cells["sizes2"].Value = formDt.Rows[i]["sizes2"].ToString();
                            row.Cells["cost_unit"].Value = formDt.Rows[i]["cost_unit"].ToString();
                            //row.Cells["tray_price"].Value = formDt.Rows[i]["tray_price"].ToString();

                            double box_price;
                            if (!double.TryParse(formDt.Rows[i]["sales_price"].ToString(), out box_price))
                                box_price = 0;

                            if (box_price == -1)
                                row.Cells["box_price"].Value = "-";
                            else if (box_price == -2)
                                row.Cells["box_price"].Value = "문의";
                            else if (box_price == -3)
                                row.Cells["box_price"].Value = "★통관예정★";
                            else
                                row.Cells["box_price"].Value = box_price.ToString("#,##0");
                            row.Cells["is_tax"].Value = formDt.Rows[i]["is_tax"].ToString();
                        }
                        //초밥류에서 저장하지 않은 다른품목서 데이터
                        else
                        {
                            row.Cells["sizes1"].Value = formDt.Rows[i]["sizes"].ToString();
                            //2023-08-01 사이즈2 제거
                            /*if (formDt.Rows[i]["sizes"].ToString().Contains("G*"))
                            {
                                string[] txt = formDt.Rows[i]["sizes"].ToString().Split(Convert.ToChar("G"));
                                if (txt.Length > 0)
                                {
                                    row.Cells["sizes1"].Value = txt[0] + "G";
                                    row.Cells["sizes2"].Value = txt[1].Substring(1, txt[1].Length - 1);
                                }
                            }
                            else if (formDt.Rows[i]["sizes"].ToString().Contains("G\r") || formDt.Rows[i]["sizes"].ToString().Contains("G\n"))
                            {
                                string[] txt = formDt.Rows[i]["sizes"].ToString().Split(Convert.ToChar("G"));
                                if (txt.Length > 0)
                                {
                                    row.Cells["sizes1"].Value = txt[0] + "G";
                                    row.Cells["sizes2"].Value = txt[1].Substring(0, txt[1].Length - 1).Trim();
                                }
                            }*/
                            double cost_unit;
                            if (!double.TryParse(formDt.Rows[i]["cost_unit"].ToString(), out cost_unit))
                                cost_unit = 1;

                            row.Cells["cost_unit"].Value = cost_unit.ToString();
                            double box_price, tray_price;
                            if (!double.TryParse(formDt.Rows[i]["sales_price"].ToString(), out box_price))
                                box_price = 0;
                            if (box_price == -1)
                                row.Cells["box_price"].Value = "-";
                            else if (box_price == -2)
                                row.Cells["box_price"].Value = "문의";
                            else if (box_price == -3)
                                row.Cells["box_price"].Value = "★통관예정★";
                            else
                            {
                                //팩일때
                                if (formDt.Rows[i]["price_unit"].ToString().Contains("팩"))
                                {
                                    tray_price = box_price;
                                    box_price = box_price * cost_unit;
                                }
                                else
                                    tray_price = box_price / cost_unit;

                                //1000이하 일때는 팩인데 잘못 기재된것으로 판단
                                if (tray_price < 1000)
                                {
                                    if (!double.TryParse(formDt.Rows[i]["sales_price"].ToString(), out tray_price))
                                        tray_price = 0;
                                    box_price = tray_price * cost_unit;
                                }
                                //무한대 처리가 되면 박스단가로 환산
                                else if (double.IsInfinity(tray_price))
                                    tray_price = box_price;

                               
                                if (formDt.Rows[i]["price_unit"].ToString().Contains("팩"))
                                {
                                    row.Cells["box_price"].Value = box_price.ToString("#,##0");
                                    row.Cells["tray_price"].Value = tray_price.ToString("#,##0") + " / p";
                                }
                                else
                                {
                                    if (cost_unit > 1 || tray_price < 1000)
                                    {
                                        row.Cells["box_price"].Value = box_price.ToString("#,##0");
                                        row.Cells["tray_price"].Value = tray_price.ToString("#,##0") + " / p";
                                    }
                                    else
                                    {
                                        row.Cells["box_price"].Value = box_price.ToString("#,##0");
                                        row.Cells["tray_price"].Value = tray_price.ToString("#,##0") + " / kg";
                                    }
                                }
                            }
                        }
                    }
                    txtTotalPage.Text = total_page.ToString();
                }
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
            this.BringToFront();
            CreateForm();
        }
        public void PasteClipboard(bool selection_clear = false)
        {
            if(selection_clear)
                dgvProduct.ClearSelection();
            dgvProduct.EndEdit();
            if (clipboardList.Count > 0)
            {
                dgvProduct.AutoPaste(false);
                if (dgvProduct.SelectedRows.Count > 0)
                {
                    //Row 추가
                    int rowIndex = dgvProduct.SelectedRows[0].Index;
                    dgvProduct.Rows.Insert(rowIndex, clipboardList.Count);
                    for (int i = clipboardList.Count - 1; i >= 0; i--)
                    {
                        DataGridViewRow row = dgvProduct.Rows[rowIndex];

                        row.Cells["category"].Value = clipboardList[i].category;
                        row.Cells["category_code"].Value = clipboardList[i].category_code;
                        row.Cells["category_code1"].Value = clipboardList[i].category_code1;
                        row.Cells["category_code2"].Value = clipboardList[i].category_code2;
                        row.Cells["category_code3"].Value = clipboardList[i].category_code3;
                        row.Cells["product_code"].Value = clipboardList[i].product_code;
                        row.Cells["product"].Value = clipboardList[i].product;
                        row.Cells["origin_code"].Value = clipboardList[i].origin_code;
                        row.Cells["origin"].Value = clipboardList[i].origin;
                        row.Cells["sizes_code"].Value = clipboardList[i].sizes_code;
                        row.Cells["sizes1"].Value = clipboardList[i].sizes1;
                        row.Cells["sizes2"].Value = clipboardList[i].sizes2;
                        row.Cells["unit"].Value = clipboardList[i].unit;
                        row.Cells["price_unit"].Value = clipboardList[i].price_unit;
                        row.Cells["unit_count"].Value = clipboardList[i].unit_count;
                        row.Cells["seaover_unit"].Value = clipboardList[i].seaover_unit;
                        row.Cells["weight"].Value = clipboardList[i].weight;
                        row.Cells["cost_unit"].Value = clipboardList[i].cost_unit;

                        row.Cells["tray_price"].Value = clipboardList[i].tray_price_txt;
                        row.Cells["box_price"].Value = clipboardList[i].sales_price_txt;

                        /*if (clipboardList[i].tray_price == -1)
                            row.Cells["tray_price"].Value = "-";
                        else if (clipboardList[i].tray_price == -2)
                            row.Cells["tray_price"].Value = "문의";
                        else if (clipboardList[i].tray_price == -3)
                            row.Cells["tray_price"].Value = "★통관예정★";
                        else
                            row.Cells["tray_price"].Value = clipboardList[i].tray_price.ToString("#,##0");

                        if (clipboardList[i].sales_price == -1)
                            row.Cells["box_price"].Value = "-";
                        else if (clipboardList[i].tray_price == -2)
                            row.Cells["box_price"].Value = "문의";
                        else if (clipboardList[i].tray_price == -3)
                            row.Cells["box_price"].Value = "★통관예정★";
                        else
                            row.Cells["box_price"].Value = clipboardList[i].sales_price.ToString("#,##0");*/

                        row.Cells["is_tax"].Value = clipboardList[i].is_tax;
                        row.Cells["remark"].Value = clipboardList[i].remark;
                        row.Cells["row"].Value = clipboardList[i].row_index;
                        row.Cells["page"].Value = clipboardList[i].page;
                        row.Cells["edit_date"].Value = clipboardList[i].edit_date;
                        row.Cells["division"].Value = clipboardList[i].division;
                        rowIndex++;
                    }
                }
                else if (dgvProduct.SelectedCells.Count > 0)
                {
                    //Row 추가
                    int rowIndex = dgvProduct.SelectedCells[0].RowIndex;
                    dgvProduct.Rows.Insert(rowIndex, clipboardList.Count);

                    for (int i = clipboardList.Count - 1; i >= 0; i--)
                    {
                        DataGridViewRow row = dgvProduct.Rows[rowIndex];
                        row.Cells["category"].Value = clipboardList[i].category;
                        row.Cells["category_code"].Value = clipboardList[i].category_code;
                        row.Cells["category_code1"].Value = clipboardList[i].category_code1;
                        row.Cells["category_code2"].Value = clipboardList[i].category_code2;
                        row.Cells["category_code3"].Value = clipboardList[i].category_code3;
                        row.Cells["product_code"].Value = clipboardList[i].product_code;
                        row.Cells["product"].Value = clipboardList[i].product;
                        row.Cells["origin_code"].Value = clipboardList[i].origin_code;
                        row.Cells["origin"].Value = clipboardList[i].origin;
                        row.Cells["sizes_code"].Value = clipboardList[i].sizes_code;
                        row.Cells["sizes1"].Value = clipboardList[i].sizes1;
                        row.Cells["sizes2"].Value = clipboardList[i].sizes2;
                        row.Cells["unit"].Value = clipboardList[i].unit;
                        row.Cells["price_unit"].Value = clipboardList[i].price_unit;
                        row.Cells["unit_count"].Value = clipboardList[i].unit_count;
                        row.Cells["seaover_unit"].Value = clipboardList[i].seaover_unit;
                        row.Cells["weight"].Value = clipboardList[i].weight;
                        row.Cells["cost_unit"].Value = clipboardList[i].cost_unit;
                        row.Cells["tray_price"].Value = clipboardList[i].tray_price;
                        row.Cells["box_price"].Value = clipboardList[i].sales_price;

                        row.Cells["is_tax"].Value = clipboardList[i].is_tax;
                        row.Cells["remark"].Value = clipboardList[i].remark;
                        row.Cells["row"].Value = clipboardList[i].row_index;
                        row.Cells["page"].Value = clipboardList[i].page;
                        row.Cells["edit_date"].Value = clipboardList[i].edit_date;
                        row.Cells["division"].Value = clipboardList[i].division;

                        rowIndex++;
                    }
                }
                else
                {
                    for (int i = clipboardList.Count - 1; i >= 0; i--)
                    {
                        int rowIndex = dgvProduct.Rows.Add();
                        DataGridViewRow row = dgvProduct.Rows[rowIndex];
                        row.Cells["category"].Value = clipboardList[i].category;
                        row.Cells["category_code"].Value = clipboardList[i].category_code;
                        row.Cells["category_code1"].Value = clipboardList[i].category_code1;
                        row.Cells["category_code2"].Value = clipboardList[i].category_code2;
                        row.Cells["category_code3"].Value = clipboardList[i].category_code3;
                        row.Cells["product_code"].Value = clipboardList[i].product_code;
                        row.Cells["product"].Value = clipboardList[i].product;
                        row.Cells["origin_code"].Value = clipboardList[i].origin_code;
                        row.Cells["origin"].Value = clipboardList[i].origin;
                        row.Cells["sizes_code"].Value = clipboardList[i].sizes_code;
                        row.Cells["sizes1"].Value = clipboardList[i].sizes1;
                        row.Cells["sizes2"].Value = clipboardList[i].sizes2;
                        row.Cells["unit"].Value = clipboardList[i].unit;
                        row.Cells["price_unit"].Value = clipboardList[i].price_unit;
                        row.Cells["unit_count"].Value = clipboardList[i].unit_count;
                        row.Cells["seaover_unit"].Value = clipboardList[i].seaover_unit;
                        row.Cells["weight"].Value = clipboardList[i].weight;
                        row.Cells["cost_unit"].Value = clipboardList[i].cost_unit;
                        row.Cells["tray_price"].Value = clipboardList[i].tray_price;
                        row.Cells["box_price"].Value = clipboardList[i].sales_price;

                        row.Cells["is_tax"].Value = clipboardList[i].is_tax;
                        row.Cells["remark"].Value = clipboardList[i].remark;
                        row.Cells["row"].Value = clipboardList[i].row_index;
                        row.Cells["page"].Value = clipboardList[i].page;
                        row.Cells["edit_date"].Value = clipboardList[i].edit_date;
                        row.Cells["division"].Value = clipboardList[i].division;

                        rowIndex++;
                    }
                }
                clipboardList = new List<OneFormDataModel>();
            }
            else
            {
                dgvProduct.AutoPaste(true);
            }
        }
        private void SortSetting()
        {
            if (!string.IsNullOrEmpty(cbSort.Text))
            {
                string[] sortStrs = cbSort.Text.Split('+');
                string sortStr = "";
                if (sortStrs.Length > 0)
                {
                    for (int i = 0; i < sortStrs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(sortStr))
                            sortStr += ", " + SetsortStr(sortStrs[i]);
                        else
                            sortStr = SetsortStr(sortStrs[i]);
                    }
                }

                //사이즈 정렬 컬럼추가
                DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvProduct);
                tb.Columns.Add("product_sort", typeof(double)).SetOrdinal(1);
                tb.Columns.Add("sizes_sort", typeof(double)).SetOrdinal(1);
                tb.Columns.Add("sizes_sort2", typeof(double)).SetOrdinal(1);
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    string tmpStr = "";
                    double res;
                    string str = tb.Rows[i]["product"].ToString();
                    if (str.Contains("F"))
                    {
                        tb.Rows[i]["product_sort"] = 1;
                    }
                    else if (str.Contains("M"))
                    {
                        tb.Rows[i]["product_sort"] = 2;
                    }
                    else 
                    {
                        tb.Rows[i]["product_sort"] = 0;
                    }


                    str = tb.Rows[i]["sizes1"].ToString();
                    if (str.Length > 0)
                    {
                        double d = 0;
                        for (int j = 0; j < str.Length; j++)
                        {
                            string tempStr = str.Substring(0, j + 1);
                            if (!double.TryParse(tempStr, out d))
                            {
                                if (tempStr.Length == 1)
                                    tb.Rows[i]["sizes_sort"] = 99999;
                                else
                                {
                                    if (double.TryParse(tempStr.Substring(0, tempStr.Length - 1), out d))
                                        tb.Rows[i]["sizes_sort"] = d;
                                }
                                break;
                            }
                        }
                        //전부 숫자일 경우
                        if (d == 0)
                            d = 99999;
                        tb.Rows[i]["sizes_sort"] = d;

                    }
                    else
                        tb.Rows[i]["sizes_sort"] = 99999;

                    //S,M,L 특수정렬
                    if (str.Contains("S") && !str.Contains("브로큰"))
                    {
                        if (str.Contains("4"))
                            tb.Rows[i]["sizes_sort2"] = 111;
                        else if (str.Contains("3"))
                            tb.Rows[i]["sizes_sort2"] = 112;
                        else if (str.Contains("2"))
                            tb.Rows[i]["sizes_sort2"] = 113;
                        else if (str.Contains("1"))
                            tb.Rows[i]["sizes_sort2"] = 114;
                        else
                            tb.Rows[i]["sizes_sort2"] = 115;

                        tb.Rows[i]["sizes_sort"] = 99999;
                    }
                    else if (str.Contains("M") && !str.Contains("CM") && !str.Contains("브로큰"))
                    {
                        if (str.Contains("4"))
                            tb.Rows[i]["sizes_sort2"] = 121;
                        else if (str.Contains("3"))
                            tb.Rows[i]["sizes_sort2"] = 122;
                        else if (str.Contains("2"))
                            tb.Rows[i]["sizes_sort2"] = 123;
                        else if (str.Contains("1"))
                            tb.Rows[i]["sizes_sort2"] = 124;
                        else
                            tb.Rows[i]["sizes_sort2"] = 125;

                        tb.Rows[i]["sizes_sort"] = 99999;
                    }
                    else if (str.Contains("L") && !str.Contains("ML") && !str.Contains("브로큰"))
                    {
                        if (str.Contains("4"))
                            tb.Rows[i]["sizes_sort2"] = 131;
                        else if (str.Contains("3"))
                            tb.Rows[i]["sizes_sort2"] = 132;
                        else if (str.Contains("2"))
                            tb.Rows[i]["sizes_sort2"] = 133;
                        else if (str.Contains("1"))
                            tb.Rows[i]["sizes_sort2"] = 134;
                        else
                            tb.Rows[i]["sizes_sort2"] = 135;

                        tb.Rows[i]["sizes_sort"] = 99999;
                    }
                    else if (str.Contains("브로큰"))
                    {
                        if (str.Contains("S"))
                            tb.Rows[i]["sizes_sort2"] = 141;
                        else if (str.Contains("M"))
                            tb.Rows[i]["sizes_sort2"] = 142;
                        else if (str.Contains("L"))
                            tb.Rows[i]["sizes_sort2"] = 143;
                        else
                            tb.Rows[i]["sizes_sort2"] = 145;

                        tb.Rows[i]["sizes_sort"] = 99999;
                    }
                    else if (str.Contains("대"))
                        tb.Rows[i]["sizes_sort2"] = 1;
                    else if (str.Contains("중"))
                        tb.Rows[i]["sizes_sort2"] = 2;
                    else if (str.Contains("소"))
                        tb.Rows[i]["sizes_sort2"] = 3;

                }

                DataView tv = new DataView(tb);

                tv.Sort = sortStr + ", product_sort, product, sizes_sort, sizes1, sizes2";
                tb = tv.ToTable();

                //재출력
                dgvProduct.Rows.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    //Row 추가
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["category"].Value = tb.Rows[i]["category"].ToString();
                    dgvProduct.Rows[n].Cells["category_code"].Value = tb.Rows[i]["category_code"].ToString();
                    dgvProduct.Rows[n].Cells["category_code1"].Value = tb.Rows[i]["category_code1"].ToString();
                    dgvProduct.Rows[n].Cells["category_code2"].Value = tb.Rows[i]["category_code2"].ToString();
                    dgvProduct.Rows[n].Cells["category_code3"].Value = tb.Rows[i]["category_code3"].ToString();
                    dgvProduct.Rows[n].Cells["product_code"].Value = tb.Rows[i]["product_code"].ToString();
                    dgvProduct.Rows[n].Cells["product"].Value = tb.Rows[i]["product"].ToString();
                    dgvProduct.Rows[n].Cells["origin_code"].Value = tb.Rows[i]["origin_code"].ToString();
                    dgvProduct.Rows[n].Cells["origin"].Value = tb.Rows[i]["origin"].ToString();
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = tb.Rows[i]["sizes_code"].ToString();
                    dgvProduct.Rows[n].Cells["sizes1"].Value = tb.Rows[i]["sizes1"].ToString();
                    dgvProduct.Rows[n].Cells["sizes2"].Value = tb.Rows[i]["sizes2"].ToString();
                    dgvProduct.Rows[n].Cells["unit"].Value = tb.Rows[i]["unit"].ToString();
                    dgvProduct.Rows[n].Cells["price_unit"].Value = tb.Rows[i]["price_unit"].ToString();
                    dgvProduct.Rows[n].Cells["unit_count"].Value = tb.Rows[i]["unit_count"].ToString();
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = tb.Rows[i]["seaover_unit"].ToString();
                    dgvProduct.Rows[n].Cells["weight"].Value = tb.Rows[i]["weight"].ToString();
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = tb.Rows[i]["cost_unit"].ToString();
                    dgvProduct.Rows[n].Cells["tray_price"].Value = tb.Rows[i]["tray_price"].ToString();
                    dgvProduct.Rows[n].Cells["box_price"].Value = tb.Rows[i]["box_price"].ToString();
                    dgvProduct.Rows[n].Cells["page"].Value = tb.Rows[i]["page"].ToString();
                    dgvProduct.Rows[n].Cells["cnt"].Value = tb.Rows[i]["cnt"].ToString();
                    dgvProduct.Rows[n].Cells["row"].Value = tb.Rows[i]["row"].ToString();
                    dgvProduct.Rows[n].Cells["edit_date"].Value = tb.Rows[i]["edit_date"].ToString();
                    dgvProduct.Rows[n].Cells["is_tax"].Value = tb.Rows[i]["is_tax"].ToString();
                    dgvProduct.Rows[n].Cells["remark"].Value = tb.Rows[i]["remark"].ToString();
                    dgvProduct.Rows[n].Cells["division"].Value = tb.Rows[i]["division"].ToString();
                }
            }
        }
        private string SetsortStr(string str)
        {
            string results = "";
            if (str == "대분류")
            {
                results = "category";
            }
            else if (str == "대분류코드")
            {
                results = "category_code1, category_code2, category_code3";
            }
            else if (str == "품명")
            {
                results = "product_sort, product";
            }
            else if (str == "원산지")
            {
                results = "origin";
            }
            else if (str == "중량")
            {
                results = "unit";
            }
            else if (str == "사이즈")
            {
                results = "sizes_sort, sizes_sort2, sizes1";
            }

            return results;
        }
        private void SelectList()
        {
            SortSetting();
            string category = txtCategory.Text.Trim();
            string product = txtProduct.Text.Trim();
            string origin = txtOrigin.Text.Trim();
            string weight = txtUnit.Text.Trim();
            string sizes1 = txtSizes1.Text.Trim();
            string sizes2 = txtSizes2.Text.Trim();

            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1)
            {
                DataTable dt = Libs.Tools.Common.GetDataGridViewAsDataTable(dgv);

                if (string.IsNullOrEmpty(category)
                        && string.IsNullOrEmpty(product)
                        && string.IsNullOrEmpty(origin)
                        && string.IsNullOrEmpty(weight)
                        && string.IsNullOrEmpty(sizes1)
                        && string.IsNullOrEmpty(sizes2))
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        dgv.Rows[i].Visible = true;
                    }
                }
                else
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        dgv.Rows[i].Visible = true;
                    }


                    if (!string.IsNullOrEmpty(category))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["category"].Value.ToString().Contains(category))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(product))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["product"].Value.ToString().Contains(product))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(origin))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["origin"].Value.ToString().Contains(origin))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(weight))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["unit"].Value.ToString().Contains(weight))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sizes1))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["sizes1"].Value.ToString().Contains(sizes1))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sizes2))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["sizes2"].Value.ToString().Contains(sizes2))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                }
            }
        }
        private string GetHandlingProductTxt()
        {
            string HandlingProductTxt = "";
            if (dgvProduct.SelectedRows.Count > 0)
            {
                string headerTxt = "(광고)(주)아토무역\n";
                string titleTxt = DateTime.Now.ToString("yyyy.MM") + " " + txtFormName.Text + "\n";
                string managerTxt = um.tel + " " + um.user_name + " " + um.grade + "\n";
                string footerTxt = "\n\n수신을 원하지 않을경우\n문자나 전화 주시면 발송을 안하도록 하겠습니다.\n-------------------\n무료수신거부\n080-855-8825";
                int idx = dgvProduct.SelectedRows.Count;
                //첫번째  
                string categoryTxt = "";
                string productTxt = "";
                string originTxt = "";
                string tempTxt = "";
                string priceTxt = "";
                //순회 출력
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    if (dgvProduct.Rows[i].Selected)
                    { 
                        DataGridViewRow row = dgvProduct.Rows[i];
                        if (row.Cells["category"].Value != null && row.Cells["product"].Value != null)
                        {
                            //다른 대분류
                            if (categoryTxt != row.Cells["category"].Value.ToString().Replace("\n", "").Replace("\r", ""))
                            {
                                categoryTxt = row.Cells["category"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                productTxt = row.Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                originTxt = row.Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                //카테고리 and 품목
                                tempTxt = "\n--------------------------------\n【" + categoryTxt + "】\n--------------------------------" + "\n\n" + "♧ " + productTxt + "(" + originTxt + ") ♧\n";

                                //강조
                                if (row.Cells["accent"].Value.ToString() == "True")
                                    tempTxt += "★";
                                //사이즈, 단위
                                /*tempTxt += row.Cells["sizes1"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["sizes2"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "");*/
                                tempTxt += row.Cells["sizes1"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                //단가
                                /*tempTxt += " @(T) " + row.Cells["tray_price"].Value.ToString() 
                                        + " @(B) " + row.Cells["box_price"].Value.ToString();*/
                                priceTxt = "";
                                if (cbTrayPrice.Checked)
                                {
                                    if(row.Cells["price_unit"].Value.ToString().Contains("팩"))
                                        priceTxt += " @(T) " + row.Cells["tray_price"].Value.ToString().Replace("/ p", "").Trim();
                                    else 
                                        priceTxt += " @(KG) " + row.Cells["tray_price"].Value.ToString().Replace("/ kg", "").Trim();
                                }
                                
                                if (cbBoxPrice.Checked)
                                    priceTxt += " @(B) " + row.Cells["box_price"].Value.ToString();

                                tempTxt += priceTxt;

                                HandlingProductTxt += "\n" + tempTxt;
                            }
                            //같은 대분류 다른 품목
                            else if (categoryTxt == row.Cells["category"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                    && (productTxt != row.Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                    || productTxt == row.Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                    && originTxt != row.Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "")))
                            {
                                categoryTxt = row.Cells["category"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                productTxt = row.Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                originTxt = row.Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                //카테고리 and 품목
                                tempTxt = "\n" + "♧ " + productTxt + "(" + originTxt + ") ♧\n";

                                //강조
                                if (row.Cells["accent"].Value.ToString() == "True")
                                    tempTxt += "★";
                                //사이즈, 단위
                                /*tempTxt += row.Cells["sizes1"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["sizes2"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "");*/
                                tempTxt += row.Cells["sizes1"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                //단가
                                /*tempTxt += " @(T) " + row.Cells["tray_price"].Value.ToString()
                                        + " @(B) " + row.Cells["box_price"].Value.ToString();*/
                                priceTxt = "";
                                if (cbTrayPrice.Checked)
                                {
                                    if (row.Cells["price_unit"].Value.ToString().Contains("팩"))
                                        priceTxt += " @(T) " + row.Cells["tray_price"].Value.ToString().Replace("/ p", "").Trim();
                                    else
                                        priceTxt += " @(KG) " + row.Cells["tray_price"].Value.ToString().Replace("/ kg", "").Trim();
                                }

                                if (cbBoxPrice.Checked)
                                    priceTxt += " @(B) " + row.Cells["box_price"].Value.ToString();

                                tempTxt += priceTxt;

                                HandlingProductTxt += "\n" + tempTxt;
                            }
                            //같은 대분류 같은품목
                            else
                            {
                                //강조
                                tempTxt = "";
                                if (row.Cells["accent"].Value.ToString() == "True")
                                    tempTxt = "★";
                                //사이즈, 단위
                                /*tempTxt += row.Cells["sizes1"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["sizes2"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "");*/
                                tempTxt += row.Cells["sizes1"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                        + " " + row.Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "");
                                //단가
                                /*tempTxt += " @(T) " + row.Cells["tray_price"].Value.ToString()   
                                        + " @(B) " + row.Cells["box_price"].Value.ToString();*/
                                priceTxt = "";
                                if (cbTrayPrice.Checked)
                                {
                                    if (row.Cells["price_unit"].Value.ToString().Contains("팩"))
                                        priceTxt += " @(T) " + row.Cells["tray_price"].Value.ToString().Replace("/ p", "").Trim();
                                    else
                                        priceTxt += " @(KG) " + row.Cells["tray_price"].Value.ToString().Replace("/ kg", "").Trim();
                                }

                                if (cbBoxPrice.Checked)
                                    priceTxt += " @(B) " + row.Cells["box_price"].Value.ToString();

                                tempTxt += priceTxt;

                                HandlingProductTxt += "\n" + tempTxt;
                            }
                        }
                    }
                }
                HandlingProductTxt = headerTxt + titleTxt + managerTxt + HandlingProductTxt + footerTxt;
            }
            
            return HandlingProductTxt.Trim();
        }
        private void CutClipboard()
        {
            clipboardList = new List<OneFormDataModel>();
            for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
            {
                if (dgvProduct.Rows[i].Selected)
                {
                    OneFormDataModel model = new OneFormDataModel();
                    DataGridViewRow row = dgvProduct.Rows[i];
                    for (int j = 0; j < dgvProduct.Columns.Count; j++)
                    { 
                        if (row.Cells[j].Value == null)
                            row.Cells[j].Value = "";
                    }

                    model.category = row.Cells["category"].Value.ToString();
                    model.category_code = row.Cells["category_code"].Value.ToString();
                    model.product = row.Cells["product"].Value.ToString();
                    model.product_code = row.Cells["product_code"].Value.ToString();
                    model.origin = row.Cells["origin"].Value.ToString();
                    model.origin_code = row.Cells["origin_code"].Value.ToString();
                    model.sizes1 = row.Cells["sizes1"].Value.ToString();
                    model.sizes2 = row.Cells["sizes2"].Value.ToString();
                    model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                    model.unit = row.Cells["unit"].Value.ToString();
                    model.price_unit = row.Cells["price_unit"].Value.ToString();
                    model.unit_count = row.Cells["unit_count"].Value.ToString();
                    model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                    model.cost_unit = row.Cells["cost_unit"].Value.ToString();
                    model.weight = row.Cells["weight"].Value.ToString();

                    double box_price = 0;
                    if (row.Cells["box_price"].Value == null || !double.TryParse(row.Cells["box_price"].Value.ToString(), out box_price))
                    {
                        if (row.Cells["box_price"].Value == null)
                            box_price = 0;
                        else if(row.Cells["box_price"].Value.ToString() == "-")
                            box_price = -1;
                        else if (row.Cells["box_price"].Value.ToString() == "문의")
                            box_price = -2;
                        else if (row.Cells["box_price"].Value.ToString().Contains("통관예정"))
                            box_price = -3;
                        
                    }
                    model.sales_price_txt = row.Cells["box_price"].Value.ToString();
                    model.sales_price = box_price;

                    double tray_price = 0;
                    if (row.Cells["tray_price"].Value == null || !double.TryParse(row.Cells["tray_price"].Value.ToString(), out tray_price))
                    {
                        if (row.Cells["tray_price"].Value == null)
                            tray_price = 0;
                        else if (row.Cells["tray_price"].Value.ToString() == "-")
                            tray_price = -1;
                        else if (row.Cells["tray_price"].Value.ToString() == "문의")
                            tray_price = -2;
                        else if (row.Cells["tray_price"].Value.ToString().Contains("통관예정"))
                            box_price = -3;

                        
                    }
                    model.tray_price_txt = row.Cells["tray_price"].Value.ToString();
                    model.tray_price = tray_price;
                    model.is_tax = row.Cells["is_tax"].Value.ToString();
                    model.remark = row.Cells["remark"].Value.ToString();

                    int page;
                    if (row.Cells["page"].Value == null || !int.TryParse(row.Cells["page"].Value.ToString(), out page))
                        page = 0;

                    model.page = page;

                    int row_index;
                    if (row.Cells["row"].Value == null || !int.TryParse(row.Cells["row"].Value.ToString(), out row_index))
                        row_index = 0;

                    model.row_index = row_index;
                    model.edit_date = row.Cells["edit_date"].Value.ToString();
                    model.division = row.Cells["division"].Value.ToString();
                    bool accent = false;
                    if (row.Cells["accent"].Value == null || !bool.TryParse(row.Cells["accent"].Value.ToString(), out accent))
                        accent = false;
                    model.accent = accent;
                    clipboardList.Add(model);
                }
            }
        }
        private void PriceUpdate()
        {
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    if (dgvProduct.Rows[i].Visible == false)
                        dgvProduct.Rows[i].Visible = true;
                }
            }
            //업체별시세현황 스토어프로시져 호출
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this, "호출 내용이 없음");
                this.Activate();
                return;
            }
            //MSGBOX
            if (MessageBox.Show(this, "단가를 최신단가로 갱신하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataGridView dgv = dgvProduct;
                //단가갱신
                DataTable dt = new DataTable("SEAOVER");
                dt = seaoverRepository.GetProductTapble2();
                if (dt.Rows.Count > 0)
                {
                    if (dgv.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Visible)
                                dgv.Rows[i].Visible = true;

                            foreach (DataGridViewCell cell in dgv.Rows[i].Cells)
                            {
                                if (cell.Value == null)
                                    cell.Value = "";
                            }

                            //공백없애기
                            dgv.Rows[i].Cells["category"].Value = dgv.Rows[i].Cells["category"].Value.ToString().Trim();
                            dgv.Rows[i].Cells["origin"].Value = dgv.Rows[i].Cells["origin"].Value.ToString().Trim();
                            dgv.Rows[i].Cells["product"].Value = dgv.Rows[i].Cells["product"].Value.ToString().Trim();
                            dgv.Rows[i].Cells["sizes1"].Value = dgv.Rows[i].Cells["sizes1"].Value.ToString().Trim();
                            dgv.Rows[i].Cells["sizes2"].Value = dgv.Rows[i].Cells["sizes2"].Value.ToString().Trim();
                        }
                        dgv.Update();
                        //
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (dgv.Rows[i].Cells["box_price"].Value.ToString() != "-" 
                                && dgv.Rows[i].Cells["box_price"].Value.ToString() != "문의" 
                                && !dgv.Rows[i].Cells["box_price"].Value.ToString().Contains("통관예정"))
                            {
                                foreach (DataGridViewCell c in dgv.Rows[i].Cells)
                                {
                                    if (c.Value == null && c.ColumnIndex < 11)
                                        c.Value = "";
                                }

                                string whrStr = "";
                                whrStr = $"품목코드='{dgv.Rows[i].Cells["product_code"].Value}'"
                                    + $" AND 원산지코드='{dgv.Rows[i].Cells["origin_code"].Value}'"
                                    + $" AND 규격코드='{dgv.Rows[i].Cells["sizes_code"].Value}'";


                                if (string.IsNullOrEmpty(dgv.Rows[i].Cells["unit"].Value.ToString()))
                                    whrStr += $" AND 단위 IS NULL";
                                else
                                    whrStr += $" AND 단위='{dgv.Rows[i].Cells["unit"].Value}'";

                                if (string.IsNullOrEmpty(dgv.Rows[i].Cells["price_unit"].Value.ToString()))
                                    whrStr += $" AND 가격단위 IS NULL";
                                else
                                    whrStr += $" AND 가격단위='{dgv.Rows[i].Cells["price_unit"].Value}'";

                                if (string.IsNullOrEmpty(dgv.Rows[i].Cells["unit_count"].Value.ToString()))
                                    whrStr += $" AND 단위수량 IS NULL";
                                else
                                {
                                    if (dgv.Rows[i].Cells["unit_count"].Value.ToString() == "0")
                                        whrStr += $" AND (단위수량='0' OR 단위수량 IS NULL)";
                                    else
                                        whrStr += $" AND 단위수량='{dgv.Rows[i].Cells["unit_count"].Value}'";
                                }

                                if (string.IsNullOrEmpty(dgv.Rows[i].Cells["seaover_unit"].Value.ToString()))
                                    whrStr += $" AND SEAOVER단위 IS NULL";
                                else
                                    whrStr += $" AND SEAOVER단위='{dgv.Rows[i].Cells["seaover_unit"].Value}'";

                                DataRow[] dr = dt.Select(whrStr);
                                if (dr.Length > 0)
                                {
                                    DataTable tmpDt = dr.CopyToDataTable();
                                    tmpDt.Columns.Add("disc", typeof(bool));
                                    //중복체크
                                    Dictionary<string, int> discDic = new Dictionary<string, int>();
                                    for (int j = tmpDt.Rows.Count - 1; j >= 0; j--)
                                    {
                                        tmpDt.Rows[j]["disc"] = false;
                                        string keys = tmpDt.Rows[j]["품목코드"] + "/" + tmpDt.Rows[j]["원산지코드"] + "/" + tmpDt.Rows[j]["규격코드"] + "/" + tmpDt.Rows[j]["단위"]
                                             + "/" + tmpDt.Rows[j]["가격단위"] + "/" + tmpDt.Rows[j]["단위수량"] + "/" + tmpDt.Rows[j]["SEAOVER단위"];

                                        if (discDic.ContainsKey(keys))
                                            tmpDt.Rows[j]["disc"] = true;
                                        else
                                            discDic.Add(keys, j);
                                    }
                                    //중복삭제
                                    DataTable newDt = new DataTable();
                                    newDt.Columns.Add("원산지코드", typeof(string));
                                    newDt.Columns.Add("품목코드", typeof(string));
                                    newDt.Columns.Add("규격코드", typeof(string));
                                    newDt.Columns.Add("대분류", typeof(string));
                                    newDt.Columns.Add("단위", typeof(string));
                                    newDt.Columns.Add("가격단위", typeof(string));
                                    newDt.Columns.Add("단위수량", typeof(string));
                                    newDt.Columns.Add("SEAOVER단위", typeof(string));
                                    newDt.Columns.Add("매출단가", typeof(int));
                                    newDt.Columns.Add("단가수정일", typeof(string));
                                    newDt.Columns.Add("구분", typeof(string));
                                    newDt.Columns.Add("담당자1", typeof(string));
                                    newDt.Columns.Add("부가세유무", typeof(string));


                                    for (int j = tmpDt.Rows.Count - 1; j >= 0; j--)
                                    {
                                        if (!Convert.ToBoolean(tmpDt.Rows[j]["disc"]))
                                        {
                                            DataRow r = tmpDt.Rows[j];

                                            newDt.Rows.Add(r["원산지코드"], r["품목코드"], r["규격코드"], r["대분류"], r["단위"]
                                                , r["가격단위"], r["단위수량"], r["SEAOVER단위"], Convert.ToInt32(r["매출단가"]), r["단가수정일"], r["구분"], r["담당자1"], r["부가세유무"]);
                                        }
                                    }
                                    //출력
                                    if (newDt.Rows.Count > 0)
                                    {
                                        if (newDt.Rows.Count == 1)
                                        {
                                            //단품수식어
                                            string txtPrice = "";

                                            //선택한 단가
                                            double sales_price = Convert.ToInt32(newDt.Rows[0]["매출단가"]);
                                            dgv.Rows[i].Cells["box_price"].Value = sales_price.ToString("#,##0");
                                            double unit_count;
                                            if (dgv.Rows[i].Cells["cost_unit"].Value == null || !double.TryParse(dgv.Rows[i].Cells["cost_unit"].Value.ToString(), out unit_count))
                                                unit_count = 0;
                                            double unit = 1;
                                            if (dgv.Rows[i].Cells["seaover_unit"].Value == null || !double.TryParse(dgv.Rows[i].Cells["seaover_unit"].Value.ToString(), out unit))
                                                unit = 1;

                                            if (dgv.Rows[i].Cells["price_unit"].Value.ToString().Contains("팩")
                                                || dgv.Rows[i].Cells["price_unit"].Value.ToString().Contains("묶")
                                                || unit_count > 1)
                                            {
                                                if (unit_count > 1 && (dgv.Rows[i].Cells["price_unit"].Value == null || string.IsNullOrEmpty(dgv.Rows[i].Cells["price_unit"].Value.ToString())))
                                                    dgv.Rows[i].Cells["price_unit"].Value = "팩";

                                                dgv.Rows[i].Cells["tray_price"].Value = sales_price.ToString("#,##0") + " / p";
                                            }
                                            else
                                            {
                                                dgv.Rows[i].Cells["box_price"].Value = sales_price.ToString("#,##0");
                                            }

;
                                            if (newDt.Rows[0]["단가수정일"] == null || newDt.Rows[0]["단가수정일"].ToString() == "")
                                                dgv.Rows[i].Cells["edit_date"].Value = "";
                                            else
                                                dgv.Rows[i].Cells["edit_date"].Value = Convert.ToDateTime(newDt.Rows[0]["단가수정일"]).ToString("yyyy-MM-dd");

                                            dgv.Rows[i].Cells["manager1"].Value = newDt.Rows[0]["담당자1"];
                                            string is_tax = newDt.Rows[0]["부가세유무"].ToString();
                                            if (string.IsNullOrEmpty(is_tax))
                                                is_tax = "면세";
                                            dgv.Rows[i].Cells["is_tax"].Value = is_tax;
                                        }
                                        else
                                        {

                                            string txtPrice = "";
                                            //해당 Row 선택
                                            dgv.CurrentRow.Selected = false;
                                            dgv.Rows[i].Selected = true;
                                            dgv.FirstDisplayedScrollingRowIndex = i;
                                            //단가리스트 생성
                                            PriceSelectManager pm = new PriceSelectManager();
                                            Point p = dgv.PointToScreen(dgv.GetCellDisplayRectangle(0, i, false).Location);
                                            p = new Point(p.X - pm.Size.Width - dgv.ColumnHeadersHeight, p.Y - pm.Size.Height + dgv.Rows[i].Height);

                                            pm.StartPosition = FormStartPosition.Manual;
                                            string[] price = pm.Manager(newDt, p);
                                            //선택한 단가
                                            double sales_price = Convert.ToInt32(price[0]);
                                            double unit_count;
                                            if (dgv.Rows[i].Cells["cost_unit"].Value == null || !double.TryParse(dgv.Rows[i].Cells["cost_unit"].Value.ToString(), out unit_count))
                                                unit_count = 0;
                                            double unit = 1;
                                            if (dgv.Rows[i].Cells["seaover_unit"].Value == null || !double.TryParse(dgv.Rows[i].Cells["seaover_unit"].Value.ToString(), out unit))
                                                unit = 1;
                                            double box_price = 0;
                                            double tray_price = 0;

                                            if (dgv.Rows[i].Cells["price_unit"].Value.ToString().Contains("팩"))
                                            {
                                                box_price = sales_price * unit_count;
                                                tray_price = sales_price;
                                                txtPrice = " / p ";
                                            }
                                            else
                                            {
                                                txtPrice = " / kg ";
                                                tray_price = sales_price / unit_count;
                                                if (unit_count > 0)
                                                    txtPrice = " / p ";
                                                if (double.IsInfinity(tray_price))
                                                {
                                                    tray_price = box_price / unit;
                                                    txtPrice = " / kg";
                                                }
                                                else if (tray_price < 1000)
                                                {
                                                    tray_price = sales_price;
                                                    box_price = sales_price * unit_count;
                                                    txtPrice = " / p";
                                                }
                                                box_price = sales_price;
                                            }


                                            dgv.Rows[i].Cells["box_price"].Value = box_price.ToString("#,##0");
                                            dgv.Rows[i].Cells["tray_price"].Value = tray_price.ToString("#,##0");

                                            DateTime edit_date;
                                            if (DateTime.TryParse(price[1], out edit_date))
                                                dgv.Rows[i].Cells["edit_date"].Value = Convert.ToDateTime(price[1]).ToString("yyyy-MM-dd");
                                            else
                                                dgv.Rows[i].Cells["edit_date"].Value = "";
                                            dgv.Rows[i].Cells["manager1"].Value = price[2];
                                            dgv.Rows[i].Cells["is_tax"].Value = newDt.Rows[0]["부가세유무"];
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dgv.Rows[i].Visible = true;
                                dgv.CurrentRow.Selected = false;
                                dgv.Rows[i].Selected = true;
                                dgv.FirstDisplayedScrollingRowIndex = i;
                                dgv.Rows[i].Cells["tray_price"].Value = dgv.Rows[i].Cells["box_price"].Value;
                            }
                        }
                        //PriceUpdateSql();

                    }
                }
            }
        }
        #endregion

        #region Button
        private void btnSettingPassword_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(lbId.Text, out id))
            {
                MessageBox.Show(this, "품목서를 불러와주세요.");
                this.Activate();
                return;
            }
            List<FormDataModel> model = seaoverRepository.GetFormData("", id.ToString());
            if (model.Count == 0)
            {
                MessageBox.Show(this, "품목서를 정보를 찾을수 없습니다.");
                this.Activate();
                return;
            }
            FormPasswordManager fpm = new FormPasswordManager(model[0].password);
            fpm.Owner = this;
            //이미 잠금되있음
            if (model[0].is_rock)
            {
                if (fpm.CheckPassword())
                {
                    if (seaoverRepository.UpdateFormDataRock(id.ToString(), false, "") == -1)
                    {
                        MessageBox.Show(this, "설정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        btnSettingPassword.Text = "품목서 잠금";
                        btnSettingPassword.ForeColor = Color.Blue;
                    }
                }
                else
                {
                    MessageBox.Show(this, "비밀번호가 틀렷습니다.");
                    this.Activate();
                }
            }
            //잠금되있지 않음
            else
            {
                string password = fpm.SettingPassword();
                if (!string.IsNullOrEmpty(password))
                {
                    if (seaoverRepository.UpdateFormDataRock(id.ToString(), true, password) == -1)
                    {
                        MessageBox.Show(this, "설정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        btnSettingPassword.Text = "품목서 잠금해제";
                        btnSettingPassword.ForeColor = Color.Red;
                    }
                }
            }
        }
        private void btnSort_Click(object sender, EventArgs e)
        {
            SortSetting();
        }
        private void btnAutoSetting_Click(object sender, EventArgs e)
        {
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1)
            {
                SetPage();
                pageAutoSetting();
                CreateForm();
            }
        }
        private void btnRowRefresh_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                dgvProduct.Rows[i].Cells["row"].Value = 1;
            }
            SetPage();
            CreateForm();
        }
        private void btnLeft_Click(object sender, EventArgs e)
        {
            int total_page;
            if (!int.TryParse(txtTotalPage.Text, out total_page))
            {
                MessageBox.Show(this, "전체 페이지가 숫자형식 아닙니다.");
                this.Activate();
                return;
            }

            int current_page = total_page;
            if (!int.TryParse(txtCurPage.Text, out current_page))
                current_page = total_page;
            else
            {
                if (current_page - 1 > 0)
                    current_page--;
                else
                    current_page = total_page;

            }

            txtCurPage.Text = current_page.ToString();
            CreateForm();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            int total_page;
            if (!int.TryParse(txtTotalPage.Text, out total_page))
            {
                MessageBox.Show(this, "전체 페이지가 숫자형식 아닙니다.");
                this.Activate();
                return;
            }

            int current_page = 1;
            if (!int.TryParse(txtCurPage.Text, out current_page))
                current_page = 1;
            else
            {
                if (current_page + 1 <= total_page)
                    current_page++;
                else
                    current_page = 1;
            }

            txtCurPage.Text = current_page.ToString();
            CreateForm();
        }
        private void btnGetFormData_Click(object sender, EventArgs e)
        {
            try
            {
                GetFormData gfd = new GetFormData(um, this);
                gfd.Owner = this;
                gfd.Show();
            }
            catch
            { }
        }
        private void btnGetSeaover_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "GetProductList")
                {
                    isFormActive = true;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                }
            }

            if (!isFormActive)
            {
                try
                {
                    GetProductList pl = new GetProductList(um, this);
                    pl.Show();
                }
                catch
                { }
            }
        }
        private void btnMakeText_Click(object sender, EventArgs e)
        {
            dgvProduct.SelectAll();
            dgvProduct.EndEdit();
            Clipboard.SetText(GetHandlingProductTxt());
        }

        private void bntUpdate_Click(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서(초밥류)", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            UsersModel cum = new UsersModel();
            cum = usersRepository.GetUserInfo2(lbCreateUser.Text);
            if (cum != null)
            {
                if (um.user_id == cum.user_id || um.user_name == "관리자")
                {
                    try
                    {
                        int id = Convert.ToInt16(lbId.Text);
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        StringBuilder sql = seaoverRepository.DeleteFormData(id.ToString());
                        sqlList.Add(sql);

                        if (MessageBox.Show(this, "취급품목서 내용을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int results = seaoverRepository.UpdateTrans(sqlList);
                            if (results == -1)
                            {
                                MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            else
                                Refresh();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "수정할 내역을 불러와주세요.");
                        this.Activate();
                    }
                }
                else
                {
                    MessageBox.Show(this, "작성자만 품목서를 삭제할 수 있습니다.");
                    this.Activate();
                }
            }
            else
            {
                MessageBox.Show(this, "유저정보를 찾을 수 없습니다.");
                this.Activate();
            }
        }

        private void btnFormRefresh_Click(object sender, EventArgs e)
        {
            dgvProduct.Rows.Clear();
            txtFormName.Text = String.Empty;
            lbId.Text = "null";
            lbCreateUser.Text = "null";
            lbUpdatetime.Text = "null";
            lbEditUser.Text = "null";
            txtCategory.Text = String.Empty;
            txtProduct.Text = String.Empty;
            txtOrigin.Text = String.Empty;
            txtSizes1.Text = String.Empty;
            txtSizes2.Text = String.Empty;
            txtUnit.Text = String.Empty;


            lCategory.Controls.Clear();
            lProduct.Controls.Clear();
            lOrigin.Controls.Clear();
            lSize.Controls.Clear();
            lSize2.Controls.Clear();
            lUnit.Controls.Clear();
            lPack.Controls.Clear();
            lTrayPrice.Controls.Clear();
            lBoxPrice.Controls.Clear();
            lIstax.Controls.Clear();
            lRemark.Controls.Clear();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            //SetBasicSetting();
            if (MessageBox.Show(this, "단가최신화 전에 \n" +
                                        "  * 재고있는 품목 -> 단가 최신화 포함 \n" +
                                        "  * 품절된 품목 -> 단가 최신화 제외 \n" +
                                        "최산화 목록 먼저 최신화하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CheckInStockProduct();   //재입고된 품목확인
                CheckOutStockProduct();  //품절된 품목확인
            }
            PriceUpdate();
            CreateForm();
        }

        private void CheckInStockProduct()
        {
            DataGridView dgv = dgvProduct;

            int result = stockRepository.CallStoredProcedureSTOCK(um.seaover_id, DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable stockDt = stockRepository.GetExistStock();
            if (stockDt.Rows.Count > 0 && dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    if (row.Cells["box_price"].Value != null
                        && (row.Cells["box_price"].Value.ToString() == "-"
                        || row.Cells["box_price"].Value.ToString() == "문의"
                        || row.Cells["box_price"].Value.ToString().Contains("통관예정")))
                    {
                        string whr = $"품명코드 = '{row.Cells["product_code"].Value.ToString()}'"
                                + $" AND 원산지코드 = '{row.Cells["origin_code"].Value.ToString()}'"
                                + $" AND 규격코드 = '{row.Cells["sizes_code"].Value.ToString()}'"
                                + $" AND 단위 = '{row.Cells["seaover_unit"].Value.ToString()}'";
                        DataRow[] dr = stockDt.Select(whr);
                        if (dr.Length > 0)
                            row.Cells["box_price"].Value = "0";
                        else
                        {
                            whr = $"품명코드 = '{row.Cells["product_code"].Value.ToString()}'"
                                    + $" AND 원산지코드 = '{row.Cells["origin_code"].Value.ToString()}'"
                                    + $" AND 규격코드 = '{row.Cells["sizes_code"].Value.ToString()}'"
                                    + $" AND 단위 = '{row.Cells["unit"].Value.ToString()}'";
                            dr = stockDt.Select(whr);
                            if (dr.Length > 0)
                                row.Cells["box_price"].Value = "0";
                        }
                    }
                }
            }
        }

        private void CheckOutStockProduct(bool only_select_row = false)
        {
            int cnt = 0;
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            priceComparisonRepository.CallStoredProcedureSTOCK(um.seaover_id, DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable soldoutDt = priceComparisonRepository.GetSoldOutProduct();
            if (soldoutDt.Rows.Count > 0)
            {
                if (only_select_row)
                {
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            double sales_price;
                            if (row.Cells["box_price"].Value != null && double.TryParse(row.Cells["box_price"].Value.ToString(), out sales_price))
                            {
                                string whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                    + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                    + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                    + " AND 단위 = '" + row.Cells["seaover_unit"].Value + "'";
                                DataRow[] dtRow = soldoutDt.Select(whr);
                                if (dtRow.Length == 0)
                                {
                                    whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                    + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                    + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                    + " AND 단위 = '" + row.Cells["unit"].Value + "'";
                                    dtRow = soldoutDt.Select(whr);
                                }
                                //품절품목
                                if (dtRow.Length > 0)
                                {
                                    //재고수
                                    double total_stock = Convert.ToDouble(dtRow[0]["재고수"].ToString());
                                    //예약수
                                    double reserved_stock = Convert.ToDouble(dtRow[0]["예약수"].ToString());
                                    //관리자 예약
                                    double admin_reserved_stock = 0;
                                    string admin_reserved_stock_detail = dtRow[0]["예약상세"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(admin_reserved_stock_detail))
                                    {
                                        string txtDetail = "";
                                        string[] detail = admin_reserved_stock_detail.Split(' ');
                                        for (int j = 0; j < detail.Length; j++)
                                        {
                                            string d = detail[j].ToString();
                                            if (detail[j].ToString().Contains("관리자"))
                                            {
                                                if (detail[j].Substring(0, 1) == "/")
                                                {
                                                    detail[j] = detail[j].Substring(1, detail[j].Length - 1);
                                                }
                                                detail[j] = detail[j].Replace(",", "");
                                                int rStrok = Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                                admin_reserved_stock += Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                            }
                                            else
                                            {
                                                txtDetail += " " + detail[j].ToString();
                                            }
                                        }
                                        admin_reserved_stock_detail = txtDetail;
                                    }
                                    // 가용재고 = 재고수 - 예약수 + 관리자 예약
                                    double stock = total_stock - reserved_stock + admin_reserved_stock;


                                    if (total_stock == 0)
                                    {
                                        row.Cells["box_price"].Value = "-";
                                        cnt += 1;
                                    }
                                    else if (stock == 0)
                                    {
                                        row.Cells["box_price"].Value = "문의";
                                        cnt += 1;
                                    }
                                }
                                else
                                {
                                    row.Cells["box_price"].Value = "-";
                                    cnt += 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        double sales_price;
                        if (row.Cells["box_price"].Value != null && double.TryParse(row.Cells["box_price"].Value.ToString(), out sales_price))
                        {
                            string whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                + " AND 단위 = '" + row.Cells["seaover_unit"].Value + "'";
                            DataRow[] dtRow = soldoutDt.Select(whr);
                            if (dtRow.Length == 0)
                            {
                                whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                + " AND 단위 = '" + row.Cells["unit"].Value + "'";
                                dtRow = soldoutDt.Select(whr);
                            }
                            //품절품목
                            if (dtRow.Length > 0)
                            {
                                //재고수
                                double total_stock = Convert.ToDouble(dtRow[0]["재고수"].ToString());
                                //예약수
                                double reserved_stock = Convert.ToDouble(dtRow[0]["예약수"].ToString());
                                //관리자 예약
                                double admin_reserved_stock = 0;
                                string admin_reserved_stock_detail = dtRow[0]["예약상세"].ToString().Trim();
                                if (!string.IsNullOrEmpty(admin_reserved_stock_detail))
                                {
                                    string txtDetail = "";
                                    string[] detail = admin_reserved_stock_detail.Split(' ');
                                    for (int j = 0; j < detail.Length; j++)
                                    {
                                        string d = detail[j].ToString();
                                        if (detail[j].ToString().Contains("관리자"))
                                        {
                                            if (detail[j].Substring(0, 1) == "/")
                                            {
                                                detail[j] = detail[j].Substring(1, detail[j].Length - 1);
                                            }
                                            detail[j] = detail[j].Replace(",", "");
                                            int rStrok = Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                            admin_reserved_stock += Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                        }
                                        else
                                        {
                                            txtDetail += " " + detail[j].ToString();
                                        }
                                    }
                                    admin_reserved_stock_detail = txtDetail;
                                }
                                // 가용재고 = 재고수 - 예약수 + 관리자 예약
                                double stock = total_stock - reserved_stock + admin_reserved_stock;


                                if (total_stock == 0)
                                {
                                    row.Cells["box_price"].Value = "-";
                                    cnt += 1;
                                }
                                else if (stock == 0)
                                {
                                    row.Cells["box_price"].Value = "문의";
                                    cnt += 1;
                                }
                            }
                            else
                            {
                                row.Cells["box_price"].Value = "-";
                                cnt += 1;
                            }
                        }
                    }
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            //MessageBox.Show(this, cnt + "개 완료");
            this.Activate();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            SetBasicSetting();
            //PriceUpdate();
            CreateForm();
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서(초밥류)", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            SetPage();
            PriceUpdate();

            //MessageBox.Show(this,"Excel 파일을 생성하기 약간의 시간이 걸립니다. 우측하단의 상태 표시가 사라지면 자동으로 Excel파일이 열립니다.");
            DialogResult dr = MessageBox.Show(this, "취급품목서의 단가를 노출하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
                isExcelPriceVisible = true;
            else if (dr == DialogResult.No)
                isExcelPriceVisible = false;
            else
                return;

            bgwExcel.RunWorkerAsync();
        }
        private void SetBasicSetting()
        {
            if (MessageBox.Show(this, "명칭을 기본괎으로 변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            DataGridView dgv = dgvProduct;
            DataTable dt = new DataTable();
            dt = formChangedDataRepository.GetChangeCurrentData();
            Dictionary<string, int> tempStr;
            string whrStr = "";
            if (dgv.Rows.Count > 1)
            {
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    DataGridViewRow r = dgv.Rows[i];
                    DataTable tempDt = dt;
                    DataRow[] arrRows = null;

                    if (r.Cells["category_code"].Value != null && !string.IsNullOrEmpty(r.Cells["category_code"].Value.ToString()))
                    {
                        arrRows = tempDt.Select($"column_name='category' AND column_code = '{r.Cells["category_code"].Value.ToString()}'");
                        if (arrRows.Length > 0)
                            r.Cells["category"].Value = arrRows[0][4].ToString();
                    }
                    if (r.Cells["product_code"].Value != null)
                    {
                        tempDt = dt;
                        arrRows = null;
                        arrRows = tempDt.Select($"column_name='product' AND column_code = '{r.Cells["product_code"].Value.ToString()}'");
                        if (arrRows.Length > 0)
                            r.Cells["product"].Value = arrRows[0][4].ToString(); 
                    }
                    if (r.Cells["origin_code"].Value != null)
                    {
                        tempDt = dt;
                        arrRows = null;
                        arrRows = tempDt.Select($"column_name='origin' AND column_code = '{r.Cells["origin_code"].Value.ToString()}'");
                        if (arrRows.Length > 0)
                            r.Cells["origin"].Value = arrRows[0][4].ToString();
                    }
                    if (r.Cells["sizes_code"].Value != null && r.Cells["sizes1"].Value != null && r.Cells["product_code"].Value != null && r.Cells["origin_code"].Value != null)
                    {
                        whrStr = $"column_name='sizes1' AND column_code = '{r.Cells["product_code"].Value.ToString()}^{r.Cells["origin_code"].Value.ToString()}^{r.Cells["sizes_code"].Value.ToString()}^1'";
                        tempDt = dt;
                        arrRows = null;
                        arrRows = tempDt.Select(whrStr);
                        if (arrRows.Length > 0)
                            r.Cells["sizes1"].Value = arrRows[0][4].ToString();
                    }
                    if (r.Cells["sizes_code"].Value != null && r.Cells["sizes2"].Value != null && r.Cells["product_code"].Value != null && r.Cells["origin_code"].Value != null)
                    {
                        whrStr = $"column_name='sizes2' AND column_code = '{r.Cells["product_code"].Value.ToString()}^{r.Cells["origin_code"].Value.ToString()}^{r.Cells["sizes_code"].Value.ToString()}^2'";
                        tempDt = dt;
                        arrRows = null;
                        arrRows = tempDt.Select(whrStr);
                        if (arrRows.Length > 0)
                            r.Cells["sizes2"].Value = arrRows[0][4].ToString();
                    }

                    /*if (r.Cells["cost_unit"].Value != null && r.Cells["product_code"].Value != null && r.Cells["origin_code"].Value != null && r.Cells["sizes_code"].Value != null)
                    {
                        whrStr = $"column_name='weight' AND column_code = '"
                            + $"{r.Cells["product_code"].Value.ToString()}^{r.Cells["origin_code"].Value.ToString()}^{r.Cells["sizes_code"].Value.ToString()}"
                            + $"^{r.Cells["unit"].Value.ToString()}^{r.Cells["price_unit"].Value.ToString()}^{r.Cells["cost_unit"].Value.ToString()}'";
                        tempDt = dt;
                        arrRows = null;
                        arrRows = tempDt.Select(whrStr);
                        if (arrRows != null && arrRows.Length > 0)
                            r.Cells["cost_unit"].Value = arrRows[0][4].ToString();
                    }*/
                }
            }
        }

        #endregion

        #region Key event
        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {

            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        SelectList();
                        break;
                }
            }
        }
        private void OneLineForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnReturn.PerformClick();
                        break;
                    case Keys.W:
                        btnDefault.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.S:
                        btnUpdate.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtCategory.Text = String.Empty;
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes1.Text = String.Empty;
                        txtSizes2.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.F:
                        TextFinder tf = new TextFinder(this);
                        tf.Owner = this;
                        tf.Show();
                        break;
                    case Keys.V:
                        PasteClipboard();
                        break;
                    case Keys.X:
                        CutClipboard();
                        break;
                    case Keys.C:
                        CutClipboard();
                        break;
                    case Keys.E:
                        btnPreview.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        btnGetSeaover.PerformClick();
                        break;
                    case Keys.F5:
                        btnFormRefresh.PerformClick();
                        break;
                    case Keys.F6:
                        btnAutoSetting.PerformClick();
                        break;
                    case Keys.F7:
                        btnRowRefresh.PerformClick();
                        break;
                    case Keys.F8:
                        btnGetFormData.PerformClick();
                        break;
                }
            }
        }


        #endregion

        #region 담당자 전화번호 란
        private void lbRemark_DoubleClick(object sender, EventArgs e)
        {
            ManagerSelect ms = new ManagerSelect(um);
            string remark = ms.GetRemark();
            if (remark != null && !string.IsNullOrEmpty(remark))
            {
                Font font = AutoFontSize(lbRemark, remark);
                lbRemark.Text = remark;
                lbRemark.Font = font; 
            }
        }
        //자동 폰트 사이즈 정렬
        public Font AutoFontSize(Label label, String text)
        {
            Font ft;
            Graphics gp;
            SizeF sz;
            Single Faktor, FaktorX, FaktorY;

            gp = label.CreateGraphics();
            sz = gp.MeasureString(text, label.Font);

            gp.Dispose();

            FaktorX = (label.Width) / sz.Width;
            FaktorY = (label.Height) / sz.Height;

            if (FaktorX > FaktorY)
            {
                Faktor = FaktorY;
            }
            else
            {
                Faktor = FaktorX;
            }

            ft = label.Font;
            float f = ft.SizeInPoints * (Faktor) - 1;

            if (f < 8)
            {
                f = 8;
            }

            return new Font(ft.Name, f);
        }

        #endregion

        #region 페이지별 품목수 설정
        private void pageAutoSetting()
        {
            DataGridView dgv = dgvProduct;
            dgv.EndEdit();
            //int total_page = TotalPage;
            int total_page;
            if (!int.TryParse(txtTotalPage.Text, out total_page))
            {
                MessageBox.Show(this, "전체페이지를 입력해주세요.");
                this.Activate();
                return;
            }
            if (dgv.Rows.Count > 1)
            {
                int TotalPage = total_page;
                //현재까지 채워진 카운터
                int CurCount = 0;
                for (int i = 0; i < dgv.Rows.Count; i++)
                    CurCount += Convert.ToInt16(dgv.Rows[i].Cells["row"].Value);
                //총 필요한 카운터 수
                int totalCount = TotalPage * 60;
                //필요한 카툰터
                int diffCnt = totalCount - CurCount;
                int chk_row = 1;
                //row 늘리기
                retry1:
                if (diffCnt > 0)
                {
                    //\n 포함되면서 줄수가 작은 내역부터 조정
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (dgv.Rows[i].Cells["sizes2"].Value == null)
                            dgv.Rows[i].Cells["sizes2"].Value = "";

                        if (((dgv.Rows[i].Cells["product"].Value.ToString().Contains("\n") && dgv.Rows[i].Cells["product"].Value != null)
                            || (dgv.Rows[i].Cells["sizes1"].Value.ToString().Contains("\n") && dgv.Rows[i].Cells["sizes1"].Value != null)
                            || (dgv.Rows[i].Cells["sizes2"].Value.ToString().Contains("\n") && dgv.Rows[i].Cells["sizes2"].Value != null)
                            || (dgv.Rows[i].Cells["remark"].Value.ToString().Contains("\n") && dgv.Rows[i].Cells["remark"].Value != null))
                            && chk_row <= Convert.ToInt16(dgv.Rows[i].Cells["row"].Value.ToString())
                            && diffCnt > 0)
                        {
                            dgv.Rows[i].Cells["row"].Value = chk_row + 1;
                            diffCnt--;
                            if (diffCnt == 0)
                                break;
                        }
                    }

                    //\n 포함되면서 줄수가 작은 내역부터 조정
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (dgv.Rows[i].Cells["origin"].Value.ToString().Length > 2
                            && chk_row <= Convert.ToInt16(dgv.Rows[i].Cells["row"].Value.ToString())
                            && diffCnt > 0)
                        {
                            dgv.Rows[i].Cells["row"].Value = chk_row + 1;
                            diffCnt--;
                            if (diffCnt == 0)
                                break;
                        }
                    }

                    //\n 포함되면서 줄수가 작은 내역부터 조정
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (((dgv.Rows[i].Cells["category"].Value.ToString().Length > 5 && dgv.Rows[i].Cells["category"].Value != null)
                            || (dgv.Rows[i].Cells["origin"].Value.ToString().Length > 3 && dgv.Rows[i].Cells["origin"].Value != null))
                            && chk_row <= Convert.ToInt16(dgv.Rows[i].Cells["row"].Value.ToString())
                            && diffCnt > 0)
                        {
                            dgv.Rows[i].Cells["row"].Value = chk_row + 1;
                            diffCnt--;
                            if (diffCnt == 0)
                                break;
                        }
                    }

                    //아직 남았다면 다시 실행
                    if (diffCnt > 0)
                    {
                        chk_row++;
                        goto retry1;
                    }
                }
                SetPage();
                dgv.EndEdit();
                
            retry2:
                int cur_page = 1;
                int row_cnt = 0;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (cur_page.ToString() != dgv.Rows[i].Cells["page"].Value.ToString()
                        || i == dgv.Rows.Count - 1)
                    {
                        if (i == dgv.Rows.Count - 1)
                            row_cnt += Convert.ToInt16(dgv.Rows[i].Cells["row"].Value.ToString());
                        //넘치는 페이지 조정
                        if (row_cnt > 60)
                        {
                            int over_row = row_cnt - 60;   //조정해야하는 row
                            retry3:
                            //큰 row부터 조정
                            int max_row = 0;
                            for (int j = 0; j < i; j++)
                            {
                                if (cur_page.ToString() == dgv.Rows[j].Cells["page"].Value.ToString())
                                {
                                    if (max_row < Convert.ToInt16(dgv.Rows[j].Cells["row"].Value.ToString()))
                                        max_row = Convert.ToInt16(dgv.Rows[j].Cells["row"].Value.ToString());
                                }
                            }
                            //조정
                            for (int j = 0; j < i; j++)
                            {
                                if (cur_page.ToString() == dgv.Rows[j].Cells["page"].Value.ToString()
                                    && max_row == Convert.ToInt16(dgv.Rows[j].Cells["row"].Value.ToString())
                                    && max_row > 1)
                                {
                                    dgv.Rows[j].Cells["row"].Value = Convert.ToInt16(dgv.Rows[j].Cells["row"].Value.ToString()) - 1;
                                    over_row--;
                                    if (over_row == 0)
                                        break;
                                }
                            }
                            if (over_row > 0 && max_row > 1)
                                goto retry3;
                        }
                        //부족한 row 채우기
                        else if (row_cnt < 60)
                        {
                            
                            int short_row = 60 - row_cnt;
                            retry4:
                            //큰 row부터 조정
                            int min_row = 99;
                            for (int j = 0; j < i; j++)
                            {
                                if (cur_page.ToString() == dgv.Rows[j].Cells["page"].Value.ToString())
                                {
                                    if (min_row > Convert.ToInt16(dgv.Rows[j].Cells["row"].Value.ToString()))
                                        min_row = Convert.ToInt16(dgv.Rows[j].Cells["row"].Value.ToString());
                                }
                            }
                            //조정
                            for (int j = 0; j < i; j++)
                            {
                                if (cur_page.ToString() == dgv.Rows[j].Cells["page"].Value.ToString()
                                    && min_row == Convert.ToInt16(dgv.Rows[j].Cells["row"].Value.ToString()))
                                {
                                    dgv.Rows[j].Cells["row"].Value = Convert.ToInt16(dgv.Rows[j].Cells["row"].Value.ToString()) + 1;
                                    short_row--;
                                    if (short_row == 0)
                                        break;
                                }
                            }
                            if (short_row > 0)
                                goto retry4;
                        }

                        cur_page = Convert.ToInt16(dgv.Rows[i].Cells["page"].Value.ToString());
                        row_cnt = 0;
                    }
                    row_cnt += Convert.ToInt16(dgv.Rows[i].Cells["row"].Value.ToString());
                }
                /*CurCount = 0;
                for (int i = 0; i < dgv.Rows.Count; i++)
                    CurCount += Convert.ToInt16(dgv.Rows[i].Cells["row"].Value);
                diffCnt = totalCount - CurCount;
                if (diffCnt != 0)
                    goto retry2;*/

                //CreateForm();
            }
        }


        #endregion

        #region Datagridview event
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                if (dgvProduct.Columns[e.ColumnIndex].Name == "tray_price")
                {
                    if (row.Cells["tray_price"].Value == "문의" || row.Cells["tray_price"].Value == "-")
                        row.Cells["box_price"].Value = row.Cells["tray_price"].Value;
                    else
                    {
                        double tray_price;
                        if (row.Cells["tray_price"].Value == null || !double.TryParse(Regex.Replace(row.Cells["tray_price"].Value.ToString(), @"[^0-9]", ""), out tray_price))
                            tray_price = 0;
                        double unit_count;
                        if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out unit_count))
                            unit_count = 0;
                        if (unit_count == 0)
                            unit_count = 1;

                        if (unit_count > 1 && (row.Cells["price_unit"].Value == null || string.IsNullOrEmpty(row.Cells["price_unit"].Value.ToString().Trim())))
                            row.Cells["price_unit"].Value = "팩";

                        double unit = 1;
                        if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out unit))
                            unit = 1;

                        //박스단가 계산
                        double box_price = 0;
                        string txtPrice = "";
                        if (row.Cells["price_unit"].Value.ToString().Contains("팩") || row.Cells["price_unit"].Value.ToString().Contains("묶"))
                        {
                            box_price = tray_price * unit_count;
                            txtPrice = " / p";
                        }
                        else
                        {
                            box_price = tray_price * unit;
                            txtPrice = " / kg";
                        }
                        row.Cells["box_price"].Value = box_price.ToString("#,##0");
                        row.Cells["tray_price"].Value = tray_price.ToString("#,##0") + txtPrice;
                    }
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "box_price" || dgvProduct.Columns[e.ColumnIndex].Name == "cost_unit")
                {
                    if (row.Cells["box_price"].Value == "문의" || row.Cells["box_price"].Value == "-")
                        row.Cells["tray_price"].Value = row.Cells["box_price"].Value;
                    else
                    {
                        double box_price;
                        if (row.Cells["box_price"].Value == null || !double.TryParse(Regex.Replace(row.Cells["box_price"].Value.ToString(), @"[^0-9]", ""), out box_price))
                            box_price = 0;

                        double unit_count;
                        if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out unit_count))
                            unit_count = 1;

                        if (unit_count > 1 && (row.Cells["price_unit"].Value == null || string.IsNullOrEmpty(row.Cells["price_unit"].Value.ToString().Trim())))
                            row.Cells["price_unit"].Value = "팩";

                        double unit = 1;
                        if (row.Cells["seaover_unit"].Value == null || !double.TryParse(row.Cells["seaover_unit"].Value.ToString(), out unit))
                            unit = 1;

                        //단품단가 계산
                        double tray_price = 0;
                        string txtPrice = "";
                        if (row.Cells["price_unit"].Value.ToString().Contains("팩") || row.Cells["price_unit"].Value.ToString().Contains("묶"))
                        {
                            tray_price = box_price / unit_count;
                            txtPrice = " / p";
                        }
                        else
                        {
                            tray_price = box_price / unit;
                            txtPrice = " / kg";
                        }
                        row.Cells["box_price"].Value = box_price.ToString("#,##0");
                        row.Cells["tray_price"].Value = tray_price.ToString("#,##0") + txtPrice;
                    }
                }
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }
        #endregion

        #region 우클릭 메뉴
        ContextMenuStrip m = new ContextMenuStrip();
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
                    m = new ContextMenuStrip();
                    if (this.dgvProduct.Rows.Count > 0)
                    {
                        m.Items.Add("선택삭제(D)");
                        m.Items.Add("잘라내기(X)");
                        m.Items.Add("붙혀넣기(V)");
                        m.Items.Add("강조/취소하기");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("품절품목 일괄처리");
                        m.Items.Add("문자용 텍스트 복사");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                        m.Items.Add("박스단가로 트레이단가 계산");
                        m.Items.Add("트레이단가로 박스단가 계산");
                        ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
                        toolStripSeparator3.Name = "toolStripSeparator";
                        toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add("씨오버 명칭으로 변경");
                        m.Items.Add("명칭변경 설정 초기화");

                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                        //Create 
                        m.Show(dgvProduct, e.Location);
                        //Selection
                        /*PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;*/
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.D:
                    m.Items[0].PerformClick();
                    break;
                case Keys.X:
                    m.Items[1].PerformClick();
                    break;
                case Keys.V:
                    m.Items[2].PerformClick();
                    break;
            }
        }

        private void SetChangeTextRefresh(bool isInit)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                string msg_txt = "";
                if (isInit)
                    msg_txt = "\n * 관련 품목의 설정된 변경명칭 내역을 삭제합니다.";
                else
                    msg_txt = "\n * 변경명칭 내역은 삭제되지 않고 현재 품목서에서만 변경됩니다.";

                if (MessageBox.Show(this, "변경한 명칭내역을 초기화 하시겠습니까?" + msg_txt, "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                //씨오버 원본 데이터
                DataTable productDt = seaoverRepository.GetProductByCode("", "", "", "");
                DataRow[] dr;
                //삭제 and 원본 데이터 
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = new StringBuilder();
                foreach (DataGridViewRow r in dgvProduct.SelectedRows)
                {
                    if (r.Cells["category_code"].Value != null)
                    {
                        //삭제
                        string whr = $"column_name='category' AND column_code = '{r.Cells["category_code"].Value.ToString()}'";
                        sql = formChangedDataRepository.DeleteChangeData(whr);
                        sqlList.Add(sql);

                        r.Cells["category"].Value = GetCategory(r.Cells["category_code"].Value.ToString());
                    }
                    if (r.Cells["product_code"].Value != null)
                    {
                        string whr = $"column_name='product' AND column_code = '{r.Cells["product_code"].Value.ToString()}'";
                        sql = formChangedDataRepository.DeleteChangeData(whr);
                        sqlList.Add(sql);

                        whr = $"품목코드='{r.Cells["product_code"].Value.ToString()}'";
                        dr = productDt.Select(whr);
                        if (dr.Length > 0)
                            r.Cells["product"].Value = dr[0]["품명"].ToString();
                    }

                    if (r.Cells["sizes_code"].Value != null && r.Cells["product_code"].Value != null && r.Cells["origin"].Value != null)
                    {
                        string whr = $"column_name='sizes' AND column_code = '{r.Cells["product_code"].Value.ToString()}^{r.Cells["origin_code"].Value.ToString()}^{r.Cells["sizes_code"].Value.ToString()}'";
                        sql = formChangedDataRepository.DeleteChangeData(whr);
                        sqlList.Add(sql);

                        whr = $"품목코드='{r.Cells["product_code"].Value.ToString()}' AND 규격코드='{r.Cells["sizes_code"].Value.ToString()}' AND 원산지코드='{r.Cells["origin_code"].Value.ToString()}' AND SEAOVER단위 = '{r.Cells["weight"].Value.ToString().Replace("kg", "").Replace("KG", "").Replace("Kg", "")}'";
                        dr = productDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            //2023-08-01 사이즈2 삭제
                            /*string sizes = dr[0]["규격"].ToString();
                            if (sizes.Contains("G*"))
                            {
                                string[] txt = sizes.Split(Convert.ToChar("G"));
                                if (txt.Length > 0)
                                {
                                    r.Cells["sizes1"].Value = txt[0] + "G";
                                    r.Cells["sizes2"].Value = txt[1].Substring(1, txt[1].Length - 1);
                                }
                            }
                            else
                                r.Cells["sizes1"].Value = sizes;*/
                            r.Cells["sizes1"].Value = dr[0]["규격"].ToString();
                            r.Cells["unit"].Value = dr[0]["단위"].ToString();
                            r.Cells["unit_count"].Value = dr[0]["단위수량"].ToString();
                            r.Cells["cost_unit"].Value = dr[0]["단위수량"].ToString();
                            r.Cells["price_unit"].Value = dr[0]["가격단위"].ToString();
                            r.Cells["seaover_unit"].Value = dr[0]["SEAOVER단위"].ToString();
                            r.Cells["weight"].Value = dr[0]["SEAOVER단위"].ToString() + "KG";

                        }
                            
                    }
                }
                //Execute
                if (isInit && sqlList.Count > 0)
                {
                    if (commonRepository.UpdateTran(sqlList) == -1)
                    {
                        MessageBox.Show(this, "수정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                }
            }
        }
        private string GetCategory(string category_code)
        {
            if (category_code.Length < 2)
                return "";
            else if (category_code.Substring(0, 2) == "Aa")
                return "라운드새우류";
            else if (category_code.Substring(0, 2) == "Ab")
                return "새우살류";
            else if (category_code.Substring(0, 2) == "Ba")
                return "쭈꾸미류";
            else if (category_code.Substring(0, 2) == "Bb")
                return "낙지류";
            else if (category_code.Substring(0, 2) == "Bc")
                return "갑오징어류";
            else if (category_code.Substring(0, 2) == "Bd")
                return "오징어류";
            else if (category_code.Substring(0, 2) == "Be")
                return "문어류";
            else if (category_code.Substring(0, 1) == "C")
                return "갑각류";
            else if (category_code.Substring(0, 2) == "Da")
                return "어패류";
            else if (category_code.Substring(0, 2) == "Db")
                return "살류";
            else if (category_code.Substring(0, 2) == "Dc")
                return "해물류";
            else if (category_code.Substring(0, 2) == "Ea")
                return "초밥/일식류";
            else if (category_code.Substring(0, 2) == "Eb")
                return "기타가공품(튀김, 가금류)";
            else if (category_code.Substring(0, 2) == "Ec")
                return "기타가공품(야채, 과일)";
            else if (category_code.Substring(0, 2) == "F")
                return "선어류";
            else
                return "";
        }

        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "선택삭제(D)":
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow dr in dgvProduct.SelectedRows)
                        {
                            if (dr.Index < dgvProduct.Rows.Count - 1)
                            {
                                dgvProduct.Rows.Remove(dr);
                            }
                        }
                    }
                    break;
                case "잘라내기(X)":
                    CutClipboard();
                    for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dgvProduct.Rows[i].Selected)
                            dgvProduct.Rows.Remove(dgvProduct.Rows[i]);
                    }
                    break;
                case "붙혀넣기(V)":
                    if (dgvProduct.SelectedRows.Count > 0)
                        PasteClipboard();
                    else
                    {
                        MessageBox.Show(this, "행을 선택해주세요.");
                        this.Activate();
                    }
                    break;
                case "강조/취소하기":
                    if (dgvProduct.SelectedCells.Count > 0)
                    {
                        bool isAccent = !Convert.ToBoolean(dgvProduct.SelectedRows[0].Cells["accent"].Value);
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            if (row.Index < dgvProduct.Rows.Count - 1)
                                row.Cells["accent"].Value = isAccent;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "행을 선택해주세요.");
                        this.Activate();
                    }
                    break;
                case "품절품목 일괄처리":
                    if (dgvProduct.SelectedRows.Count > 0)
                    {   
                        priceComparisonRepository.CallStoredProcedureSTOCK(um.seaover_id, DateTime.Now.ToString("yyyy-MM-dd"));
                        DataTable soldoutDt = priceComparisonRepository.GetSoldOutProduct();
                        int cnt = 0;
                        if (soldoutDt.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                string whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                        + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                        + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                        + " AND 단위 = '" + row.Cells["seaover_unit"].Value + "'";
                                DataRow[] dtRow = soldoutDt.Select(whr);
                                if (dtRow.Length > 0)
                                {
                                    //재고수
                                    double total_stock = Convert.ToDouble(dtRow[0]["재고수"].ToString());
                                    //예약수
                                    double reserved_stock = Convert.ToDouble(dtRow[0]["예약수"].ToString());
                                    //관리자 예약
                                    double admin_reserved_stock = 0;
                                    string admin_reserved_stock_detail = dtRow[0]["예약상세"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(admin_reserved_stock_detail))
                                    {
                                        string txtDetail = "";
                                        string[] detail = admin_reserved_stock_detail.Split(' ');
                                        for (int j = 0; j < detail.Length; j++)
                                        {
                                            string d = detail[j].ToString();
                                            if (detail[j].ToString().Contains("관리자"))
                                            {
                                                if (detail[j].Substring(0, 1) == "/")
                                                {
                                                    detail[j] = detail[j].Substring(1, detail[j].Length - 1);
                                                }
                                                detail[j] = detail[j].Replace(",", "");
                                                int rStrok = Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                                admin_reserved_stock += Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                            }
                                            else
                                            {
                                                txtDetail += " " + detail[j].ToString();
                                            }
                                        }
                                        admin_reserved_stock_detail = txtDetail;
                                    }
                                    // 가용재고 = 재고수 - 예약수 + 관리자 예약
                                    double stock = total_stock - reserved_stock + admin_reserved_stock;


                                    if (total_stock == 0)
                                    {
                                        row.Cells["box_price"].Value = "-";
                                        cnt += 1;
                                    }
                                    else if (stock == 0)
                                    {
                                        row.Cells["box_price"].Value = "문의";
                                        cnt += 1;
                                    }
                                }
                            }
                        }
                        MessageBox.Show(this,cnt + "개 완료");
                        this.Activate();
                    }
                    break;
                case "문자용 텍스트 복사":
                    if (this.dgvProduct.SelectedRows.Count > 0)
                        Clipboard.SetText(GetHandlingProductTxt());
                    break;
                case "박스단가로 트레이단가 계산":
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            double unit_count;
                            if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                                unit_count = 1;

                            double box_price;
                            if (row.Cells["box_price"].Value == null || !double.TryParse(row.Cells["box_price"].Value.ToString(), out box_price))
                                box_price = 0;

                            double tray_price = box_price / unit_count;
                            row.Cells["tray_price"].Value = tray_price.ToString("#,##0");
                        }
                    }
                    break;
                case "트레이단가로 박스단가 계산":
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            double unit_count;
                            if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                                unit_count = 1;

                            double tray_price;
                            if (row.Cells["tray_price"].Value == null || !double.TryParse(row.Cells["tray_price"].Value.ToString(), out tray_price))
                                tray_price = 0;

                            double box_price = tray_price * unit_count;
                            row.Cells["box_price"].Value = box_price.ToString("#,##0");
                        }
                    }
                    break;
                case "씨오버 명칭으로 변경":
                    SetChangeTextRefresh(false);
                    break;
                case "명칭변경 설정 초기화":
                    SetChangeTextRefresh(true);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 로딩 효과
        private void timer_start()
        {
            this.timer = new System.Windows.Threading.DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            this.timer.Tick += timer_Tick;
            this.timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (processingFlag)
            {
                //모든 컨트롤 사용 비활성화
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
                pnGlass.Visible = true;
            }
            else
            {
                //모든 컨트롤 사용 활성화
                foreach (Control c in this.Controls)
                {
                    c.Enabled = true;
                }
                pnGlass.Visible = false;
            }
        }
        #endregion

        #region SEAOVER 데이터 불러오기, 엑셀내려받기
        private void bgwExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            excelOneFormCreate();
        }
        private void excelOneFormCreate()
        {
            processingFlag = true;
            DataGridView dgv = dgvProduct;
            double TotalPage;
            if (!double.TryParse(txtTotalPage.Text, out TotalPage))
            {
                MessageBox.Show(this, "품목서를 다시 불러와주세요.");
                this.Activate();
                return;
            }

            bool isSuccess = false;
            // ToDo
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;

                if (dgv.Rows.Count > 1)
                {
                    //시트복사
                    //double tPage = Convert.ToDouble(lbTotal.Text.Replace("/", "").Trim());
                    double tPage = TotalPage;
                    if (tPage > 0)
                    {
                        //index
                        int rowIndex = 7;
                        int colIndex = 2;
                        int cPage = 1;
                        setOneFormSheetBasicSetting(excelApp, workBook, workSheet, cPage, (int)tPage);
                        Excel.Worksheet xlSht = workBook.Sheets[1];
                        for (int i = 2; i <= tPage; i++)
                        {
                            xlSht.Copy(Type.Missing, workBook.Sheets[workBook.Sheets.Count]);
                            string formName = txtFormName.Text;
                            formName = formName.Replace("/", "").Replace(".", "").Replace(@"\", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("*", "");
                            if (formName.Length > 24)
                                formName = formName.Substring(0, 24);
                            workBook.Sheets[workBook.Sheets.Count].Name = formName + "(" + i + "-" + tPage + ")";
                            workBook.Sheets[workBook.Sheets.Count].Cells[2, 2].Value = formName + "(" + i + " / " + tPage + ")";
                        }

                        //단가출력
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            //Add Sheet
                            if (cPage != (int)dgv.Rows[i].Cells["page"].Value)
                            {
                                cellsMerge(workSheet);
                                cPage = (int)dgv.Rows[i].Cells["page"].Value;
                                /*workBook.Worksheets.Add(After: workSheet);
                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;
                                setOneFormSheetBasicSetting(excelApp, workBook, workSheet, cPage, (int)tPage);*/

                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;

                                rowIndex = 7;
                            }
                            else
                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;

                            //Output=================================================================================================
                            for (int j = 0; j < Convert.ToInt16(dgv.Rows[i].Cells["row"].Value); j++)
                            {
                                if (rowIndex + j <= 66)
                                {
                                    workSheet.Cells[rowIndex + j, colIndex].Value = dgv.Rows[i].Cells["category"].Value.ToString();

                                    bool isAccent = false;
                                    if (dgv.Rows[i].Cells["accent"].Value == null || !bool.TryParse(dgv.Rows[i].Cells["accent"].Value.ToString(), out isAccent))
                                        isAccent = false;

                                    if (isAccent)
                                        workSheet.Cells[rowIndex + j, colIndex + 1].Value = "★" + dgv.Rows[i].Cells["product"].Value.ToString();
                                    else
                                        workSheet.Cells[rowIndex + j, colIndex + 1].Value = dgv.Rows[i].Cells["product"].Value.ToString();

                                    string str = dgv.Rows[i].Cells["origin"].Value.ToString();
                                    string originStr = "";

                                    if (str.Length > 3)
                                    {
                                        int indexStart = 0;
                                        int indexEnd = 0;
                                        int iSplit = 2;
                                        int totalLength = str.Length;
                                        int forCount = totalLength / iSplit;


                                        for (int k = 0; k < forCount + 1; k++)
                                        {
                                            if (totalLength < indexStart + iSplit)
                                                indexEnd = totalLength - indexStart;
                                            else
                                                indexEnd = str.Substring(indexStart, iSplit).LastIndexOf("") + 1;

                                            originStr += str.Substring(indexStart, indexEnd) + "\r\n";
                                            indexStart += indexEnd;
                                        }
                                        if (originStr.Length > 4)
                                            originStr = originStr.Substring(0, originStr.Length - 4);
                                    }
                                    else
                                        originStr = str;

                                    workSheet.Cells[rowIndex + j, colIndex + 2].Value = originStr;

                                    workSheet.Cells[rowIndex + j, colIndex + 3].Value = dgv.Rows[i].Cells["sizes1"].Value.ToString();
                                    /*if (dgv.Rows[i].Cells["sizes2"].Value == null)
                                        dgv.Rows[i].Cells["sizes2"].Value = "";
                                    workSheet.Cells[rowIndex + j, colIndex + 4].Value = dgv.Rows[i].Cells["sizes2"].Value.ToString();*/
                                    if (dgv.Rows[i].Cells["weight"].Value == null)
                                        dgv.Rows[i].Cells["weight"].Value = "";
                                    workSheet.Cells[rowIndex + j, colIndex + 5].Value = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    if (dgv.Rows[i].Cells["cost_unit"].Value == null)
                                        dgv.Rows[i].Cells["cost_unit"].Value = "";
                                    else if (dgv.Rows[i].Cells["cost_unit"].Value.ToString() == "0" || dgv.Rows[i].Cells["cost_unit"].Value.ToString() == "1")
                                        dgv.Rows[i].Cells["cost_unit"].Value = "";
                                    workSheet.Cells[rowIndex + j, colIndex + 4].Value = dgv.Rows[i].Cells["cost_unit"].Value.ToString();

                                    //트레이 단가
                                    if (dgv.Rows[i].Cells["tray_price"].Value.ToString() == "0" || dgv.Rows[i].Cells["tray_price"].Value.ToString() == "-" || dgv.Rows[i].Cells["tray_price"].Value.ToString() == "")
                                        workSheet.Cells[rowIndex + j, colIndex + 6].Value = "-";
                                    else if (dgv.Rows[i].Cells["tray_price"].Value.ToString() == "문의")
                                        workSheet.Cells[rowIndex + j, colIndex + 6].Value = "문의";
                                    else
                                        workSheet.Cells[rowIndex + j, colIndex + 6].Value = dgv.Rows[i].Cells["tray_price"].Value.ToString();

                                    if (dgv.Rows[i].Cells["box_price"].Value.ToString() == "0" || dgv.Rows[i].Cells["box_price"].Value.ToString() == "-" || dgv.Rows[i].Cells["box_price"].Value.ToString() == "")
                                        workSheet.Cells[rowIndex + j, colIndex + 7].Value = "-";
                                    else if (dgv.Rows[i].Cells["box_price"].Value.ToString() == "문의")
                                        workSheet.Cells[rowIndex + j, colIndex + 7].Value = "문의";
                                    else
                                        workSheet.Cells[rowIndex + j, colIndex + 7].Value = dgv.Rows[i].Cells["box_price"].Value.ToString();

                                    workSheet.Cells[rowIndex + j, colIndex + 8].Value = dgv.Rows[i].Cells["is_tax"].Value.ToString();
                                    workSheet.Cells[rowIndex + j, colIndex + 9].Value = dgv.Rows[i].Cells["remark"].Value.ToString();
                                }
                            }

                            rowIndex += Convert.ToInt16(dgv.Rows[i].Cells["row"].Value);

                            //마지막 머지
                            if (i == dgv.Rows.Count - 1)
                                cellsMerge(workSheet);
                        }
                    }
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
                this.Activate();
                setAutomatic(excelApp, true);
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
            finally
            {
                setAutomatic(excelApp, true);
                //workBook.Worksheets.PrintPreview(true);
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
                if(isSuccess)
                    excelApp.Visible = true;
            }
            processingFlag = false;
        }
        private void cellsMerge(Excel.Worksheet wk)
        {
            //Merge
            int[] endRow = { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 };

            //Excel.Range rg1 = wk.Range[wk.Cells[7, 8], wk.Cells[66, 8]];
            //rg1.Merge();
            Excel.Range rg1;
            int col;
            string str1, str2;
            for (int j = 8; j < 68; j++)
            {
                //구분병합
                col = 2;
                str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                str2 = string.Concat(wk.Cells[j, col].Value, " ");
                if (str1 != str2 || j == 67)
                //if (wk.Cells[endRow[col - 2], col].Value != wk.Cells[j, col].Value)
                {
                    int tmp_row = j - endRow[col - 2];
                    string val = wk.Cells[endRow[col - 2], col].Value;
                    if (val == null)
                        val = "";
                    val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                    int valLen = val.Length;
                    //5행이하, 6글자 이상
                    if (valLen > 5 & tmp_row <= 5)
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((k + 1) % 2 == 0)
                            {
                                if ((stt_idx + 2) == valLen)
                                    tempStr += val.Substring(stt_idx, 2);
                                else
                                    tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                stt_idx += 2;
                            }
                        }
                        wk.Cells[endRow[col - 2], col].Value = tempStr;
                    }
                    //5행이하, 6글자 이상
                    else if (valLen > 1 & tmp_row <= 2)
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((k + 1) % 2 == 0)
                            {
                                if ((stt_idx + 2) == valLen)
                                    tempStr += val.Substring(stt_idx, 2);
                                else
                                    tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                stt_idx += 2;
                            }
                        }
                        wk.Cells[endRow[col - 2], col].Value = tempStr;
                    }
                    //6행이상, 6글자 이상
                    else
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((stt_idx + 1) == valLen)
                                tempStr += val.Substring(stt_idx, 1);
                            else
                                tempStr += val.Substring(stt_idx, 1) + "\r\n";
                            stt_idx += 1;
                        }
                        wk.Cells[endRow[col - 2], col].Value = tempStr;
                    }

                    //병합
                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                    rg1.Merge();
                    rg1.WrapText = true;
                    rg1.Interior.Color = Color.Beige;
                    endRow[col - 2] = j;

                    //품목, 원산지, 사이즈, 단위 병합
                    for (col = 3; col <= 11; col++)
                    {
                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                        rg1.Merge();
                        rg1.WrapText = true;
                        endRow[col - 2] = j;
                    }
                }
                else
                {
                    wk.Cells[j, col].Value = "";

                    //품목 병합
                    col = 3;
                    str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                    str2 = string.Concat(wk.Cells[j, col].Value, " ");
                    if (str1 != str2 || j == 67)
                    //if (wk.Cells[endRow[col - 2], col].Value != wk.Cells[j, col].Value)
                    {
                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                        rg1.Merge();
                        rg1.WrapText = true;
                        endRow[col - 2] = j;

                        //원산지, 사이즈, 단위 병합
                        for (col = 4; col <= 11; col++)
                        {
                            rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                            rg1.Merge();
                            rg1.WrapText = true;
                            endRow[col - 2] = j;
                        }
                    }
                    else
                    {
                        wk.Cells[j, col].Value = "";

                        //원산지 병합
                        col = 4;
                        str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                        str2 = string.Concat(wk.Cells[j, col].Value, " ");
                        if (str1 != str2 || j == 67)
                        //if (wk.Cells[endRow[col - 2], col].Value != wk.Cells[j, col].Value)
                        {
                            rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                            rg1.Merge();
                            rg1.WrapText = true;
                            endRow[col - 2] = j;

                            //원산지, 사이즈, 단위 병합
                            for (col = 5; col <= 11; col++)
                            {
                                rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                                rg1.Merge();
                                rg1.WrapText = true;
                                endRow[col - 2] = j;
                            }
                        }
                        else
                        {
                            wk.Cells[j, col].Value = "";

                            //사이즈 병합
                            col = 5;
                            str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                            str2 = string.Concat(wk.Cells[j, col].Value, " ");
                            if (str1 != str2 || j == 67)
                            //if (wk.Cells[endRow[col - 2], col].Value != wk.Cells[j, col].Value)
                            {
                                rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                                rg1.Merge();
                                rg1.WrapText = true;
                                endRow[col - 2] = j;

                                //원산지, 사이즈, 단위 병합
                                for (col = 6; col <= 11; col++)
                                {
                                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                                    rg1.Merge();
                                    rg1.WrapText = true;
                                    endRow[col - 2] = j;
                                }
                            }

                            else
                            {
                                wk.Cells[j, col].Value = "";
                                //단위, 사이즈, 산지순 병합
                                for (col = 11; col >= 6; col--)
                                {
                                    str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                                    str2 = string.Concat(wk.Cells[j, col].Value, " ");
                                    if (str1 != str2 || j == 67)
                                    {
                                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                                        rg1.Merge();
                                        rg1.WrapText = true;
                                        endRow[col - 2] = j;
                                    }
                                    else
                                        wk.Cells[j, col].Value = "";
                                }
                            }
                        }
                    }
                }

                /*//비고
                col = 11;
                str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                str2 = string.Concat(wk.Cells[j, col].Value, " ");
                if (str1 != str2 || j == 67)
                {
                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                    rg1.Merge();
                    rg1.WrapText = true;
                    endRow[col - 2] = j;
                }
                else
                    wk.Cells[j, col].Value = "";*/
            }
        }

        //Excel속도개선
        private void setAutomatic(Excel.Application excel, bool auto)
        {
            if (auto)
            {
                excel.DisplayAlerts = true;
                excel.Visible = true;
                excel.ScreenUpdating = true;
                excel.DisplayStatusBar = true;
                excel.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
                excel.EnableEvents = true;
            }
            else
            {
                excel.DisplayAlerts = false;
                excel.Visible = false;
                excel.ScreenUpdating = false;
                excel.DisplayStatusBar = false;
                excel.Calculation = Excel.XlCalculation.xlCalculationManual;
                excel.EnableEvents = false;
            }
        }

        //Excel Sheet Basic
        private void setOneFormSheetBasicSetting(Excel.Application excel, Excel.Workbook wb, Excel.Worksheet wk, int cPage, int tPage)
        {
            string formName = txtFormName.Text;
            formName = formName.Replace("/", "").Replace(".", "").Replace(@"\", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("*", "");
            if (formName.Length > 24)
                formName = formName.Substring(0, 24);
            workSheet.Name = formName + "(" + cPage + "-" + tPage + ")";

            Excel.Range rg1 = wk.Range[wk.Cells[1, 2], wk.Cells[67, 11]];
            rg1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rg1.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rg1.RowHeight = 37;


            //Column Width
            if (isExcelPriceVisible)
            {
                wk.Columns["A"].ColumnWidth = 2;
                wk.Columns["B"].ColumnWidth = 16;         //대분류

                wk.Columns["C"].ColumnWidth = 38;         //품목
                wk.Columns["D"].ColumnWidth = 22;         //원산지

                wk.Columns["E"].ColumnWidth = 38;         //사이즈
                //wk.Columns["F"].ColumnWidth = 38;         //규격

                wk.Columns["F"].ColumnWidth = 20;         //팩
                wk.Columns["G"].ColumnWidth = 20;         //단위

                wk.Columns["H"].ColumnWidth = 29;         //단가
                wk.Columns["I"].ColumnWidth = 29;         //단가

                wk.Columns["J"].ColumnWidth = 15;         //과세
                wk.Columns["K"].ColumnWidth = 43;         //비고
            }
            else
            {
                wk.Columns["A"].ColumnWidth = 2;
                wk.Columns["B"].ColumnWidth = 24;         //대분류

                wk.Columns["C"].ColumnWidth = 46;         //품목
                wk.Columns["D"].ColumnWidth = 30;         //원산지

                wk.Columns["E"].ColumnWidth = 46;         //사이즈
                //wk.Columns["F"].ColumnWidth = 38;         //규격

                wk.Columns["F"].ColumnWidth = 28;         //팩
                wk.Columns["G"].ColumnWidth = 28;         //단위

                wk.Columns["H"].ColumnWidth = 29;         //단가
                wk.Columns["H"].EntireColumn.Hidden = true;
                wk.Columns["I"].ColumnWidth = 29;         //단가
                wk.Columns["I"].EntireColumn.Hidden = true;

                wk.Columns["J"].ColumnWidth = 23;         //과세
                wk.Columns["K"].ColumnWidth = 51;         //비고
            }
            
            //Font Style
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 22;

            wk.Columns["D"].Font.Name = "맑은 고딕";
            wk.Columns["E"].Font.Name = "맑은 고딕";
            wk.Columns["F"].Font.Name = "맑은 고딕";
            wk.Columns["G"].Font.Name = "맑은 고딕";
            wk.Columns["H"].Font.Name = "맑은 고딕";
            wk.Columns["I"].Font.Name = "맑은 고딕";
            wk.Columns["J"].Font.Name = "맑은 고딕";
            wk.Columns["K"].Font.Name = "맑은 고딕";
            //wk.Columns["L"].Font.Name = "맑은 고딕";

            wk.Columns["I"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            wk.Columns["I"].NumberFormatLocal = "#,##0";
            wk.Columns["J"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            wk.Columns["J"].NumberFormatLocal = "#,##0";

            //고정값 및 타이틀
            rg1 = wk.Range[wk.Cells[1, 2], wk.Cells[1, 11]];
            rg1.Merge();
            wk.Cells[1, 2].value = "'(광고) ㈜아토무역 TEL : 051-256-3100 , FAX : 051-256-4100  부산광역시 서구 원양로 310     발신일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + "   수신인: 구매담당자님 귀하";
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 16;

            rg1 = wk.Range[wk.Cells[2, 2], wk.Cells[3, 11]];
            rg1.Merge();
            wk.Cells[2, 2].value = txtFormName.Text + " (" + cPage + "/" + tPage + ")";
            rg1.Font.Name = "HY견고딕";
            rg1.Font.Size = 40;
            rg1.Font.Bold = true;

            rg1 = wk.Range[wk.Cells[4, 2], wk.Cells[5, 11]];
            rg1.Merge();

            wk.Cells[4, 2].value = lbRemark.Text;
            wk.Cells[4, 2].Font.Name = "맑은 고딕";
            wk.Cells[4, 2].Font.Size = 28;
            wk.Cells[4, 2].Font.Bold = true;

            rg1 = wk.Range[wk.Cells[67, 2], wk.Cells[67, 11]];
            rg1.Merge();
            wk.Cells[67, 2].value = "'광고수신을 원하지 않는경우 무료전화 080-855-8825 로 연락주시기 바랍니다'";
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 16;

            //Title
            rg1 = wk.Range[wk.Cells[6, 2], wk.Cells[6, 11]];
            rg1.Font.Size = 20;
            rg1.Font.Bold = true;

            wk.Cells[6, 2].value = "구분";
            wk.Cells[6, 3].value = "품목";
            wk.Cells[6, 4].value = "산지";
            wk.Cells[6, 5].value = "사이즈";
            //wk.Cells[6, 6].value = "규격";
            wk.Cells[6, 6].value = "팩";
            wk.Cells[6, 7].value = "단위";
            wk.Cells[6, 8].value = "단품단가";
            wk.Cells[6, 9].value = "BOX 단가";
            wk.Cells[6, 10].value = "과세";
            wk.Cells[6, 11].value = "비고";
            rg1 = wk.Range[wk.Cells[6, 7], wk.Cells[6, 11]];
            rg1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            rg1.NumberFormatLocal = "@";

            //Border Line Style
            rg1 = wk.Range[wk.Cells[4, 2], wk.Cells[67, 11]];
            rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

            //엑셀 Zoom
            SetZoom(40, wb, excel);
            //인쇄영역 설정
            wk.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
            wk.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
            wk.PageSetup.PrintArea = "B1:K67";
            wk.PageSetup.FitToPagesTall = false;
            /*wk.PageSetup.CenterFooter = "Page &P page, &N page";*/
            wk.PageSetup.FooterMargin = 0;
            wk.PageSetup.PrintGridlines = false;
            wk.PageSetup.TopMargin = 0;
            wk.PageSetup.BottomMargin = 0;
            wk.PageSetup.LeftMargin = 0;
            wk.PageSetup.RightMargin = 0;
            wk.PageSetup.CenterHorizontally = true;
            wk.PageSetup.CenterVertically = true;
            wk.PageSetup.FitToPagesWide = 1;
            wk.Application.ActiveWindow.View = Excel.XlWindowView.xlPageBreakPreview;


            /*wk.VPageBreaks[1].DragOff(Excel.XlDirection.xlToRight, 1);
            wk.HPageBreaks[1].DragOff(Excel.XlDirection.xlDown, 1);*/
            try
            {
                wk.VPageBreaks[1].DragOff(Excel.XlDirection.xlToRight, 1);
            }
            catch (Exception ex)
            { }
            try
            {
                wk.HPageBreaks[1].DragOff(Excel.XlDirection.xlDown, 1);
            }
            catch (Exception ex)
            { }
            wk.Application.ActiveWindow.View = Excel.XlWindowView.xlNormalView;
        }

        /// <summary>
        /// 엑셀 객체 해재 메소드
        /// </summary>
        /// <param name="obj"></param>
        static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);   //엑셀객체 해제
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();  //가비지 수집
            }

        }

        // Zoom Event Handler
        public static void SetZoom(int zoomLevel,
                             Excel.Workbook wb,
                             Excel.Application excelInstance)
        {
            foreach (Excel.Worksheet ws in wb.Worksheets)
            {
                ws.Activate();
                excelInstance.ActiveWindow.Zoom = zoomLevel;
            }
        }

        public static Excel.Workbook Open(Excel.Application excelInstance,
                                   string fileName, bool readOnly = false,
                                   bool editable = true, bool updateLinks = true)
        {
            Excel.Workbook book = excelInstance.Workbooks.Open(
                fileName, updateLinks, readOnly);
            return book;
        }
        #endregion

        

        


        /*#region Print 
        int count = 0;
        int pageNo = 1;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            //유효성 검사
            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this, "출력할 내역이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                for (int i = 0; i < dgvProduct.Rows.Count - 1; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    if (row.Cells["page"].Value == null && row.Cells["area"].Value == null)
                    {
                        MessageBox.Show(this, "양식새로 고침후 다시 해주시기 바랍니다.");
                        this.Activate();
                        return;
                    }
                }
            }
            try
            {
                //재설정
                btnReturn.PerformClick();
                //프린터 설정
                count = 0;
                pageNo = 1;
                this.printDocument1 = new PrintDocument();
                if (rbTwoline.Checked)
                    this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(prtDoc_TwoLine_PrintPage);
                else if (rbOneline.Checked)
                    this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(prtDoc_OneLine_PrintPage);
                int pages = common.GetPageCount(printDocument1);
                //미리보고 폼
                Common.PrintManager.PrintManager pm = new Common.PrintManager.PrintManager(this, printDocument1, pages);
                pm.Show();
            }
            catch
            { }
        }
        //초기화
        public void InitVariable()
        {
            count = 0;
            pageNo = 1;
        }

        //두줄품목서 양식만들기
        private void prtDoc_TwoLine_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridView dgv = this.dgvProduct;

            int dialogHeight = printDocument1.DefaultPageSettings.PaperSize.Height - 60;    //페이지 전체넓이 printPreivew.Width  (가로모드는 반대로)
            int dialogWidth = printDocument1.DefaultPageSettings.PaperSize.Width - 61;           //페이지 전체넓이 printPreivew.Height  (가로모드는 반대로)

            StringFormat sf = new StringFormat();  //컬럼안에 있는 값들 가운데로 정렬하기 위해서.
            sf.Alignment = StringAlignment.Center;
            //Header
            string top_str = $"(광고) ㈜아토무역 TEL : 051-256-3100 , FAX : 051-256-4100  부산광역시 서구 원양로 310     발신일자 : {DateTime.Now.ToString("yyyy-MM-dd")}   수신인: 구매담당자님 귀하";
            e.Graphics.DrawString(top_str, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 20, 20);
            //Title
            Label lb = new Label();
            lb.Font = new Font("Arial", 16, FontStyle.Bold);
            Graphics g = lb.CreateGraphics();
            int txt_width = (int)g.MeasureString(txtFormName.Text, new Font("Arial", 20, FontStyle.Bold)).Width;
            e.Graphics.DrawString(txtFormName.Text + "   (" + pageNo + " / " + TotalPage + ")", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, (dialogWidth / 2) - (txt_width / 2), 45);
            //Footer
            string foot_str = "'광고수신을 원하지 않는 경우 무료전화 080-855-8825 로 연락주시기 바랍니다.'";
            e.Graphics.DrawString(foot_str, new Font("Arial", 8), Brushes.Black, 20, dialogHeight + 5);

            //전체테두리
            RectangleF dr = new RectangleF(20, 40, dialogWidth, dialogHeight);
            e.Graphics.DrawRectangle(Pens.Black, 20, 40, dialogWidth, dialogHeight - 40);
            e.Graphics.DrawString("", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, dr, sf);
            //담당자 + 품목서 비고
            dr = new RectangleF(20, 90, dialogWidth, 80);
            e.Graphics.DrawRectangle(Pens.Black, 20, 80, dialogWidth, 40);
            string manager = lbRemark.Text;
            Font f = AutoFontSize(lbRemark, manager);        //글자길이에 맞는 폰트
            e.Graphics.DrawString(manager, f, Brushes.Black, dr, sf);


            //품목정보 타이틀===========================================================================
            //Left
            Font font = new Font("Arial", 8, FontStyle.Regular);
            int col_width = 30;
            int str_width = 20;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("구분", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 90;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("품명", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 33;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("산지", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 70;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("중량", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 80;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("사이즈", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 80;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("단가", font, Brushes.Black, dr, sf);
            //Right
            str_width += col_width;
            col_width = 30;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("구분", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 90;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("품명", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 33;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("산지", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 70;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("중량", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 80;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("사이즈", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 80;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("단가", font, Brushes.Black, dr, sf);
            //=======================================================================================
            //column 순회
            sf.LineAlignment = StringAlignment.Center;
            Font bold_font = new Font("Arial", 8, FontStyle.Bold);
            Font regular_font = new Font("Arial", 7, FontStyle.Regular);
            int i, j;
            int row_cnt;
            float height = ((float)dialogHeight - 140) / 60;              //셀 하나의 높이 
            //Left area=================================================================
            //  1.Category
            col_width = 30;                 //셀 하나의 넓이
            str_width = 20;                 //셀 시작지점
            float str_height = 140;         //셀 시작높이
            float total_cell_height = 0;    //셀 총 높이
            float cell_height;              //하나의 내역셀 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);   //행수
                cell_height = height * row_cnt;                       //해당셀의 높이
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        string val = row.Cells["category"].Value.ToString();
                        if (val == null)
                            val = "";
                        val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        int valLen = val.Length;
                        string category;
                        //5행이하, 6글자 이상
                        if (valLen > 5 & total_cell_height <= height * 5)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //5행이하, 6글자 이상
                        else if (valLen > 1 & total_cell_height <= height * 2)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //6행이상, 6글자 이상
                        else
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((stt_idx + 1) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 1);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                }

                                stt_idx += 1;
                            }
                            category = tempStr;
                        }
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {
                        total_cell_height += cell_height;
                        if (row.Cells["category"].Value.ToString() != next_row.Cells["category"].Value.ToString())
                        {
                            string val = row.Cells["category"].Value.ToString();
                            if (val == null)
                                val = "";
                            val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                            int valLen = val.Length;
                            string category;
                            //5행이하, 6글자 이상
                            if (valLen > 5 & total_cell_height <= height * 5)
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((k + 1) % 2 == 0)
                                    {
                                        if ((stt_idx + 2) == valLen)
                                        {
                                            tempStr += val.Substring(stt_idx, 2);
                                        }
                                        else
                                        {
                                            tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                        }
                                        stt_idx += 2;
                                    }
                                }
                                category = tempStr;
                            }
                            //5행이하, 6글자 이상
                            else if (valLen > 1 & total_cell_height <= height * 2)
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((k + 1) % 2 == 0)
                                    {
                                        if ((stt_idx + 2) == valLen)
                                        {
                                            tempStr += val.Substring(stt_idx, 2);
                                        }
                                        else
                                        {
                                            tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                        }
                                        stt_idx += 2;
                                    }
                                }
                                category = tempStr;
                            }
                            //6행이상, 6글자 이상
                            else
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((stt_idx + 1) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 1);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                    }

                                    stt_idx += 1;
                                }
                                category = tempStr;
                            }

                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);
                            //e.Graphics.DrawString(row.Cells["category"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;

                    string val = row.Cells["category"].Value.ToString();
                    if (val == null)
                        val = "";
                    val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                    int valLen = val.Length;
                    string category;
                    //5행이하, 6글자 이상
                    if (valLen > 5 & total_cell_height <= height * 5)
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((k + 1) % 2 == 0)
                            {
                                if ((stt_idx + 2) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 2);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                }
                                stt_idx += 2;
                            }
                        }
                        category = tempStr;
                    }
                    //5행이하, 6글자 이상
                    else if (valLen > 1 & total_cell_height <= height * 2)
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((k + 1) % 2 == 0)
                            {
                                if ((stt_idx + 2) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 2);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                }
                                stt_idx += 2;
                            }
                        }
                        category = tempStr;
                    }
                    //6행이상, 6글자 이상
                    else
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((stt_idx + 1) == valLen)
                            {
                                tempStr += val.Substring(stt_idx, 1);
                            }
                            else
                            {
                                tempStr += val.Substring(stt_idx, 1) + "\r\n";
                            }

                            stt_idx += 1;
                        }
                        category = tempStr;
                    }
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //  2.Product
            str_width += col_width;         //셀 시작지점
            col_width = 90;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["product"].Value.ToString() != next_row.Cells["product"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //  3.Origin
            str_width += col_width;         //셀 시작지점
            col_width = 33;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;

                string origin = row.Cells["origin"].Value.ToString().Trim();
                if (origin.Length == 5)
                {
                    origin = origin.Substring(0, 4);
                }

                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        if (origin.Length == 3)
                            regular_font = new Font("Arial", 6, FontStyle.Regular);

                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                        regular_font = new Font("Arial", 7, FontStyle.Regular);
                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {
                        total_cell_height += cell_height;
                        if (row.Cells["origin"].Value.ToString() != next_row.Cells["origin"].Value.ToString())
                        {
                            if (origin.Length == 3)
                                regular_font = new Font("Arial", 6, FontStyle.Regular);

                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                            regular_font = new Font("Arial", 7, FontStyle.Regular);
                        }
                    }
                }
                else
                {
                    if (origin.Length >= 3)
                        regular_font = new Font("Arial", 6, FontStyle.Regular);

                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);

                    regular_font = new Font("Arial", 7, FontStyle.Regular);
                }
            }
            //  4.Weight
            str_width += col_width;         //셀 시작지점
            col_width = 70;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["weight"].Value.ToString() != next_row.Cells["weight"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  5.Sizes
            str_width += col_width;         //셀 시작지점
            col_width = 80;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sizes"].Value.ToString() != next_row.Cells["sizes"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  6.Sales_price
            str_width += col_width;         //셀 시작지점
            col_width = 80;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;

                double price;
                string txt_price = "";
                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out price))
                {
                    txt_price = row.Cells["sales_price"].Value.ToString();
                }
                else
                {
                    if (row.Cells["price_unit"].Value.ToString() == "팩")
                        txt_price = price.ToString("#,##0") + " /p";
                    else if (row.Cells["sales_price"].Value.ToString() == "kg" || row.Cells["sales_price"].Value.ToString() == "Kg" || row.Cells["sales_price"].Value.ToString() == "KG")
                        txt_price = price.ToString("#,##0") + " /k";
                    else
                        txt_price = price.ToString("#,##0");
                }
                //출력=====================================================================================================================
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sales_price"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sales_price"].Value.ToString() != next_row.Cells["sales_price"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //Right area=================================================================
            //  1.Category
            col_width = 30;                 //셀 하나의 넓이
            str_width = 403;                //셀 시작지점
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        string val = row.Cells["category"].Value.ToString();
                        if (val == null)
                            val = "";
                        val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        int valLen = val.Length;
                        string category;
                        //5행이하, 6글자 이상
                        if (valLen > 5 & total_cell_height <= height * 5)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //5행이하, 6글자 이상
                        else if (valLen > 1 & total_cell_height <= height * 2)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //6행이상, 6글자 이상
                        else
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((stt_idx + 1) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 1);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                }

                                stt_idx += 1;
                            }
                            category = tempStr;
                        }

                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["category"].Value.ToString() != next_row.Cells["category"].Value.ToString())
                        {
                            string val = row.Cells["category"].Value.ToString();
                            if (val == null)
                                val = "";
                            val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                            int valLen = val.Length;
                            string category;
                            //5행이하, 6글자 이상
                            if (valLen > 5 & total_cell_height <= height * 5)
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((k + 1) % 2 == 0)
                                    {
                                        if ((stt_idx + 2) == valLen)
                                        {
                                            tempStr += val.Substring(stt_idx, 2);
                                        }
                                        else
                                        {
                                            tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                        }
                                        stt_idx += 2;
                                    }
                                }
                                category = tempStr;
                            }
                            //5행이하, 6글자 이상
                            else if (valLen > 1 & total_cell_height <= height * 2)
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((k + 1) % 2 == 0)
                                    {
                                        if ((stt_idx + 2) == valLen)
                                        {
                                            tempStr += val.Substring(stt_idx, 2);
                                        }
                                        else
                                        {
                                            tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                        }
                                        stt_idx += 2;
                                    }
                                }
                                category = tempStr;
                            }
                            //6행이상, 6글자 이상
                            else
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((stt_idx + 1) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 1);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                    }

                                    stt_idx += 1;
                                }
                                category = tempStr;
                            }
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        string val = row.Cells["category"].Value.ToString();
                        if (val == null)
                            val = "";
                        val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        int valLen = val.Length;
                        string category;
                        //5행이하, 6글자 이상
                        if (valLen > 5 & total_cell_height <= height * 5)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //5행이하, 6글자 이상
                        else if (valLen > 1 & total_cell_height <= height * 2)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //6행이상, 6글자 이상
                        else
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((stt_idx + 1) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 1);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                }

                                stt_idx += 1;
                            }
                            category = tempStr;
                        }
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);
                    }
                }
            }
            //  2.Product
            str_width += col_width;         //셀 시작지점
            col_width = 90;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["product"].Value.ToString() != next_row.Cells["product"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                    }
                }
            }
            //  3.Origin
            str_width += col_width;         //셀 시작지점
            col_width = 33;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;

                string origin = row.Cells["origin"].Value.ToString().Trim();
                if (origin.Length == 5)
                {
                    origin = origin.Substring(0, 4);
                }

                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        if (origin.Length == 3)
                            regular_font = new Font("Arial", 6, FontStyle.Regular);
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);

                        regular_font = new Font("Arial", 7, FontStyle.Regular);
                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["origin"].Value.ToString() != next_row.Cells["origin"].Value.ToString())
                        {
                            if (origin.Length == 3)
                                regular_font = new Font("Arial", 6, FontStyle.Regular);
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;

                            regular_font = new Font("Arial", 7, FontStyle.Regular);
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        if (origin.Length == 3)
                            regular_font = new Font("Arial", 6, FontStyle.Regular);
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);

                        regular_font = new Font("Arial", 7, FontStyle.Regular);
                    }
                }
            }
            //  4.Weight
            str_width += col_width;         //셀 시작지점
            col_width = 70;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["weight"].Value.ToString() != next_row.Cells["weight"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                    }
                }
            }
            //  5.Sizes
            str_width += col_width;         //셀 시작지점
            col_width = 80;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sizes"].Value.ToString() != next_row.Cells["sizes"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                    }
                }
            }
            //  6.Sales Price
            str_width += col_width;         //셀 시작지점
            col_width = 80;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;

                double price;
                string txt_price = "";
                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out price))
                {
                    txt_price = row.Cells["sales_price"].Value.ToString();
                }
                else
                {
                    if (row.Cells["price_unit"].Value.ToString() == "팩")
                        txt_price = price.ToString("#,##0") + " /p";
                    else if (row.Cells["sales_price"].Value.ToString() == "kg" || row.Cells["sales_price"].Value.ToString() == "Kg" || row.Cells["sales_price"].Value.ToString() == "KG")
                        txt_price = price.ToString("#,##0") + " /k";
                    else
                        txt_price = price.ToString("#,##0");
                }

                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sales_price"].Value.ToString() != next_row.Cells["sales_price"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);
                    }
                }
            }



            //==========================================================================
            //Page++
            if (pageNo < TotalPage)
            {
                e.HasMorePages = true;
                pageNo++;
                return;
            }
        }
        //한줄품목서 양식만들기
        private void prtDoc_OneLine_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridView dgv = this.dgvProduct;

            int dialogHeight = printDocument1.DefaultPageSettings.PaperSize.Height - 60;    //페이지 전체넓이 printPreivew.Width  (가로모드는 반대로)
            int dialogWidth = printDocument1.DefaultPageSettings.PaperSize.Width - 61;           //페이지 전체넓이 printPreivew.Height  (가로모드는 반대로)

            StringFormat sf = new StringFormat();  //컬럼안에 있는 값들 가운데로 정렬하기 위해서.
            sf.Alignment = StringAlignment.Center;
            //Header
            string top_str = $"(광고) ㈜아토무역 TEL : 051-256-3100 , FAX : 051-256-4100  부산광역시 서구 원양로 310     발신일자 : {DateTime.Now.ToString("yyyy-MM-dd")}   수신인: 구매담당자님 귀하";
            e.Graphics.DrawString(top_str, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 20, 20);
            //Title
            Label lb = new Label();
            lb.Font = new Font("Arial", 16, FontStyle.Bold);
            Graphics g = lb.CreateGraphics();
            int txt_width = (int)g.MeasureString(txtFormName.Text, new Font("Arial", 20, FontStyle.Bold)).Width;
            e.Graphics.DrawString(txtFormName.Text + "   (" + pageNo + " / " + TotalPage + ")", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, (dialogWidth / 2) - (txt_width / 2), 45);
            //Footer
            string foot_str = "'광고수신을 원하지 않는 경우 무료전화 080-855-8825 로 연락주시기 바랍니다.'";
            e.Graphics.DrawString(foot_str, new Font("Arial", 8), Brushes.Black, 20, dialogHeight + 5);

            //전체테두리
            RectangleF dr = new RectangleF(20, 40, dialogWidth, dialogHeight);
            e.Graphics.DrawRectangle(Pens.Black, 20, 40, dialogWidth, dialogHeight - 40);
            e.Graphics.DrawString("", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, dr, sf);
            //담당자 + 품목서 비고
            dr = new RectangleF(20, 90, dialogWidth, 80);
            e.Graphics.DrawRectangle(Pens.Black, 20, 80, dialogWidth, 40);
            string manager = lbRemark.Text;
            Font f = AutoFontSize(lbRemark, manager);        //글자길이에 맞는 폰트
            e.Graphics.DrawString(manager, f, Brushes.Black, dr, sf);


            //품목정보 타이틀===========================================================================
            Font font = new Font("Arial", 8, FontStyle.Regular);
            int col_width = 60;
            int str_width = 20;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("구분", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 180;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("품명", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 66;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("산지", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 140;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("중량", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 160;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("사이즈", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 160;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("단가", font, Brushes.Black, dr, sf);
            //=======================================================================================
            //column 순회
            sf.LineAlignment = StringAlignment.Center;
            Font bold_font = new Font("Arial", 8, FontStyle.Bold);
            Font regular_font = new Font("Arial", 7, FontStyle.Regular);
            int i, j;
            int row_cnt;
            float height = ((float)dialogHeight - 140) / 60;              //셀 하나의 높이 
            //  1.Category
            col_width = 60;                 //셀 하나의 넓이
            str_width = 20;                 //셀 시작지점
            float str_height = 140;         //셀 시작높이
            float total_cell_height = 0;    //셀 총 높이
            float cell_height;              //하나의 내역셀 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);   //행수
                cell_height = height * row_cnt;                       //해당셀의 높이
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["category"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {
                        total_cell_height += cell_height;
                        if (row.Cells["category"].Value.ToString() != next_row.Cells["category"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["category"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["category"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //  2.Product
            str_width += col_width;         //셀 시작지점
            col_width = 180;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["product"].Value.ToString() != next_row.Cells["product"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //  3.Origin
            str_width += col_width;         //셀 시작지점
            col_width = 66;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                string origin = row.Cells["origin"].Value.ToString().Trim();
                if (origin.Length == 3)
                {
                    origin = origin.Substring(0, 2);
                }
                else if (origin.Length == 5)
                {
                    origin = origin.Substring(0, 4);
                }

                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["origin"].Value.ToString() != next_row.Cells["origin"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  4.Weight
            str_width += col_width;         //셀 시작지점
            col_width = 140;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["weight"].Value.ToString() != next_row.Cells["weight"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  5.Sizes
            str_width += col_width;         //셀 시작지점
            col_width = 160;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sizes"].Value.ToString() != next_row.Cells["sizes"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  6.Sales_price
            str_width += col_width;         //셀 시작지점
            col_width = 160;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sales_price"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sales_price"].Value.ToString() != next_row.Cells["sales_price"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["sales_price"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["sales_price"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //==========================================================================
            //Page++
            if (pageNo < TotalPage)
            {
                e.HasMorePages = true;
                pageNo++;
                return;
            }
        }
        #endregion Method*/
    }
}
