using AdoNetWindow.Model;
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

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class HideProductManagement : Form
    {
        UsersModel um;
        IHideRepository hideRepository = new HideRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();

        string category;
        string product; 
        string origin;
        string sizes;
        string seaover_unit;

        bool isUpdate = false;

        public HideProductManagement(string category1, string product1, string origin1, string sizes1, string seaover_unit1, UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            category = category1;
            product = product1;
            origin = origin1;            
            sizes = sizes1;
            seaover_unit = seaover_unit1;

            lbProduct.Text = product1;
            lbOrigin.Text = origin;
            lbSizes.Text = sizes1;
            lbUnit.Text = seaover_unit1;
            txtUntildate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            SetHeaderStyle();
            GetHide();
        }

        #region Method
        public bool SetHide(Point p)
        {
            this.Location = p;
            this.ShowDialog();

            return isUpdate;
        }

        private void SetHeaderStyle()
        {
            dgvHide.Columns["id"].Visible = false;
            dgvHide.Columns["hide_mode"].Visible = false;
            dgvHide.Columns["hide_details"].Visible = false;
        }

        //설정내역 불러오기
        public void GetHide()
        {
            dgvHide.Rows.Clear();
            List<HideModel> model = hideRepository.GetHide(category, product, origin, sizes, seaover_unit);
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    int n = dgvHide.Rows.Add();
                    dgvHide.Rows[n].Cells["id"].Value = model[i].id;
                    dgvHide.Rows[n].Cells["edit_user"].Value = model[i].edit_user;
                    dgvHide.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(model[i].updatetime).ToString("yyyy-MM-dd");
                    dgvHide.Rows[n].Cells["until_date"].Value = Convert.ToDateTime(model[i].until_date).ToString("yyyy-MM-dd");
                    dgvHide.Rows[n].Cells["hide_mode"].Value = model[i].hide_mode;
                    dgvHide.Rows[n].Cells["hide_details"].Value = model[i].hide_details;
                    dgvHide.Rows[n].Cells["remark"].Value = model[i].remark;
                }
                //첫번째 내역 강제선택====================================
                DataGridViewRow row = dgvHide.Rows[0];
                row.Cells["chk"].Value = true;
                //선택한 내역 가져오기
                lbId.Text = row.Cells["id"].Value.ToString();
                txtRemark.Text = row.Cells["remark"].Value.ToString();
                txtUntildate.Text = row.Cells["until_date"].Value.ToString();
                //전체 제외
                if (row.Cells["hide_mode"].Value.ToString() == "all")
                {
                    rbAll.Checked = true;
                }
                //재고수 제외
                else if (row.Cells["hide_mode"].Value.ToString() == "stock")
                {
                    rbStock.Checked = true;
                    cbUp.Checked = false;
                    cbDown.Checked = false;

                    string[] detail = row.Cells["hide_details"].Value.ToString().Split('|');

                    foreach (var d in detail)
                    {
                        if (!string.IsNullOrEmpty(d.Trim()))
                        {
                            string[] stock = d.Split('_');
                            if (stock.Length > 1)
                            {
                                if (stock[0] == "up")
                                {
                                    cbUp.Checked = true;
                                    txtUpStock.Text = stock[1];
                                }
                                else if (stock[0] == "down")
                                {
                                    cbDown.Checked = true;
                                    txtDownStock.Text = stock[1];
                                }
                            }
                        }
                    }
                }
                dgvHide.ClearSelection();
            }
        }
        //삭제하기

        #endregion

        #region Ridio, Button, Checkbox 
        private void cbUp_CheckedChanged(object sender, EventArgs e)
        {
            txtUpStock.Enabled = cbUp.Checked;
        }

        private void cbDown_CheckedChanged(object sender, EventArgs e)
        {
            txtDownStock.Enabled = cbDown.Checked;
        }
        private void btnUntildateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtUntildate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(lbId.Text, out id))
            {
                MessageBox.Show(this, "삭제할 내역을 선택해주세요.");
                this.Activate();
                return;
            }            
            StringBuilder sql = hideRepository.DeleteHide(id);
            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);
            int result = hideRepository.UpdateTrans(sqlList);
            if (result == -1)
            {
                MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                lbId.Text = "null";
                isUpdate = true;
                GetHide();
                return;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            HideModel model = new HideModel();
            model.id = commonRepository.GetNextId("t_hide", "id");
            model.division = "업체별시세현황";
            model.category = category;
            model.product = product;
            model.origin = origin;
            model.sizes = sizes;
            model.seaover_unit = seaover_unit;
            model.edit_user = um.user_name;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
            model.remark = txtRemark.Text;

            DateTime until_date;
            if (!DateTime.TryParse(txtUntildate.Text, out until_date))
            {
                MessageBox.Show(this, "제외일자는 날짜형식('yyyy-mm-dd')으로 입력해주세요.");
                this.Activate();
                return;
            }
            model.until_date = until_date.ToString("yyyy-MM-dd");
            if (rbAll.Checked)
            {
                model.hide_mode = "all";
            }
            else
            {
                double stock;
                model.hide_mode = "stock";
                if (!cbDown.Checked && !cbUp.Checked)
                {
                    MessageBox.Show(this, "[▲ (올림)] 또는 [▼ (내림)]을 선택해주셔야 합니다.");
                    this.Activate();
                    return;
                }
                //Up
                if (cbUp.Checked)
                {   
                    if (!double.TryParse(txtUpStock.Text, out stock))
                    {
                        MessageBox.Show(this, "[▲ (올림) - 재고수]는 숫자 형식으로만 설정해주세요. ");
                        this.Activate();
                        return;
                    }
                    model.hide_details += "up_" + stock + "|";
                }
                //Down
                if (cbDown.Checked)
                {
                    if (!double.TryParse(txtUpStock.Text, out stock))
                    {
                        MessageBox.Show(this, "[▼ (내림) - 재고수]는 숫자 형식으로만 설정해주세요. ");
                        this.Activate();
                        return;
                    }
                    model.hide_details += "down_" + stock + "|";
                }
            }
            StringBuilder sql = hideRepository.InsertHide(model);
            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);
            int result = hideRepository.UpdateTrans(sqlList);
            if (result == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                isUpdate = true;
                GetHide();
                return;
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void rbStock_CheckedChanged(object sender, EventArgs e)
        {
            if (rbStock.Checked)
            {
                cbUp.Enabled = true;
                cbDown.Enabled = true;
                txtUpStock.Enabled = true;
                txtDownStock.Enabled = true;
            }
            else
            {
                cbUp.Enabled = false;
                cbDown.Enabled = false;
                txtUpStock.Enabled = false;
                txtDownStock.Enabled = false;
            }
        }
        #endregion

        #region Key Event
        private void HideProductManagement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                    case Keys.A:
                        {
                            btnInsert.PerformClick();
                            break;
                        }
                    case Keys.D:
                        {
                            btnDelete.PerformClick();
                            break;
                        }
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
        }
        private void txtUntildate_KeyDown(object sender, KeyEventArgs e)
        {
            Control tb = (Control)sender;
            if (e.KeyCode == Keys.Enter)
            {
                tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                //날짜 완성
                Control tbb = (Control)sender;
                tbb.Text = common.strDatetime(tbb.Text);
            }
        }
        private void txtDownStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(46) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))
                e.Handled = true;
        }

        #endregion

        #region Datagridview event
        private void dgvHide_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvHide.Rows[e.RowIndex];
                dgvHide.ClearSelection();
                foreach (DataGridViewRow r in dgvHide.Rows)
                {
                    r.Cells["chk"].Value = false;
                }
                row.Cells["chk"].Value = true;
                //선택한 내역 가져오기====================================
                lbId.Text = row.Cells["id"].Value.ToString();
                txtRemark.Text = row.Cells["remark"].Value.ToString();
                txtUntildate.Text = row.Cells["until_date"].Value.ToString();

                //전체 제외
                if (row.Cells["hide_mode"].Value.ToString() == "all")
                {
                    rbAll.Checked = true;
                }
                //재고수 제외
                else if (row.Cells["hide_mode"].Value.ToString() == "stock")
                {
                    rbStock.Checked = true;
                    cbUp.Checked = false;
                    cbDown.Checked = false;

                    string[] detail = row.Cells["hide_details"].Value.ToString().Split('|');

                    foreach (var d in detail)
                    {
                        if (!string.IsNullOrEmpty(d.Trim()))
                        {
                            string[] stock = d.Split('_');
                            if (stock.Length > 1)
                            {
                                if (stock[0] == "up")
                                {
                                    cbUp.Checked = true;
                                    txtUpStock.Text = stock[1];
                                }
                                else if (stock[0] == "down")
                                {
                                    cbDown.Checked = true;
                                    txtDownStock.Text = stock[1];
                                }
                            }
                        }
                    }
                }
            }
        }

        private void dgvHide_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DateTime until_date;
                if (DateTime.TryParse(dgvHide.Rows[e.RowIndex].Cells["until_date"].Value.ToString(), out until_date))
                {
                    if (until_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        dgvHide.Rows[e.RowIndex].DefaultCellStyle.BackColor= Color.Gray;
                    }
                    else
                    {
                        dgvHide.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }
        #endregion
    }
}
