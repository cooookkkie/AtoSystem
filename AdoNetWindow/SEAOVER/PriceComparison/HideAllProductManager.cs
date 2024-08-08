using AdoNetWindow.Model;
using Repositories;
using System;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class HideAllProductManager : Form
    {
        bool isApply = false;
        HideModel model = new HideModel();

        public HideAllProductManager()
        {
            InitializeComponent();
            txtUntildate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        #region Method
        public bool SetHIdeProduct(out HideModel hideModel)
        {
            this.ShowDialog();

            hideModel = model;
            return isApply;
        }
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
        
        private void btnInsert_Click(object sender, EventArgs e)
        {
            model = new HideModel();
            model.division = "업체별시세현황";
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
            isApply = true;
            this.Dispose();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            isApply = false;
            model = null;
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

        #region Key event
        private void txtUntildate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(46) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))
                e.Handled = true;
        }
        
        private void HideAllProductManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode)
                {
                    case Keys.A:
                        btnInsert.PerformClick();
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
