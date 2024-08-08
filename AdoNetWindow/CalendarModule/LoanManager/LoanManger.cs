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

namespace AdoNetWindow.CalendarModule.LoanManager
{
    public partial class LoanManger : Form
    {
        ILoanRepository loanRepository = new LoanRepository();
        public LoanManger()
        {
            InitializeComponent(); 
        }

        private void LoanManger_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void txtLoanLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(46)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void GetData()
        {
            lvBankList.Items.Clear();
            List<LoanModel> model = new List<LoanModel>();
            model = loanRepository.GetLoanList();
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = model[i].bank.ToString();
                    lvi.SubItems.Add(model[i].division.ToString());
                    lvi.SubItems.Add(model[i].usance_loan_limit.ToString("#,##0"));
                    lvi.SubItems.Add(model[i].atsight_loan_limit.ToString("#,##0"));

                    lvBankList.Items.Add(lvi);
                }
            }
        }

        private bool Validation()
        {
            bool isFlag = true;
            double loanLimit;
            if (string.IsNullOrEmpty(txtBank.Text))
            {
                isFlag = false;
                MessageBox.Show(this, "은행명을 입력해주세요.");
            }
            else if (string.IsNullOrEmpty(txtDivision.Text))
            {
                isFlag = false;
                MessageBox.Show(this, "구분을 입력해주세요.");
            }
            else if (string.IsNullOrEmpty(txtUsanceLimit.Text))
            {
                isFlag = false;
                MessageBox.Show(this, "유산스 한도를 입력해주세요.");
            }
            else if (!double.TryParse(txtUsanceLimit.Text.Replace(",", ""), out loanLimit))
            {
                isFlag = false;
                MessageBox.Show(this, "유산스 한도는 숫자형식으로만 입력해주세요.");
            }
            else if (string.IsNullOrEmpty(txtAtsightLimit.Text))
            {
                isFlag = false;
                MessageBox.Show(this, "일람불 한도를 입력해주세요.");
            }
            else if (!double.TryParse(txtAtsightLimit.Text.Replace(",", ""), out loanLimit))
            {
                isFlag = false;
                MessageBox.Show(this, "일람불 한도는 숫자형식으로만 입력해주세요.");
            }

            return isFlag;
        }

        private void InsertData()
        {
            if (!Validation())
            {
                return;
            }
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();

            sql = loanRepository.DeleteLoan(txtBank.Text, txtDivision.Text);
            sqlList.Add(sql);
            sql = loanRepository.InsertLoan(txtBank.Text, txtDivision.Text, txtUsanceLimit.Text.Replace(",", ""), txtAtsightLimit.Text.Replace(",", ""));
            sqlList.Add(sql);

            int results = loanRepository.UpdateTran(sqlList);
            if (results == -1)
            {
                MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
            }
            else
            {
                txtBank.Text = "";
                txtDivision.Text = "";
                txtUsanceLimit.Text = "";
                txtAtsightLimit.Text = "";
                txtBank.Focus();
                GetData();
            }
        }
        private void DeleteData()
        {
            if (!Validation())
            {
                return;
            }
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();

            sql = loanRepository.DeleteLoan(txtBank.Text, txtDivision.Text);
            sqlList.Add(sql);

            int results = loanRepository.UpdateTran(sqlList);
            if (results == -1)
            {
                MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
            }
            else
            {
                txtBank.Text = "";
                txtDivision.Text = "";
                txtUsanceLimit.Text = "";
                txtAtsightLimit.Text = "";
                txtBank.Focus();
                GetData();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void LoanManger_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    
                }
            }
            else if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        {
                            GetData();
                            break;
                        }
                    case Keys.A:
                        {
                            InsertData();
                            break;
                        }
                    case Keys.D:
                        {
                            DeleteData();
                            break;
                        }
                    case Keys.N:
                        {
                            txtBank.Text = "";
                            txtDivision.Text = "";
                            txtUsanceLimit.Text = "";
                            txtAtsightLimit.Text = "";
                            txtBank.Focus();
                            break;
                        }
                    case Keys.M:
                        {
                            txtBank.Focus();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
            else
            {
               
            }
        }

        private void lvBankList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvBankList.SelectedItems.Count != 0)
            {
                int selectRow = lvBankList.SelectedItems[0].Index;
                txtBank.Text = lvBankList.Items[selectRow].SubItems[0].Text;
                txtDivision.Text = lvBankList.Items[selectRow].SubItems[1].Text;
                txtUsanceLimit.Text = lvBankList.Items[selectRow].SubItems[2].Text;
                txtAtsightLimit.Text = lvBankList.Items[selectRow].SubItems[3].Text;
            }
        }
    }
}
