using AdoNetWindow.Model;
using Repositories;
using Repositories.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.AddSaleCompany
{
    public partial class FinanceManager : Form
    {
        ICompanyFinanceRepository companyFinanceRepository = new CompanyFinanceRepository();
        ICommonRepository commonRepository = new CommonRepository();    
        AddCompanyInfo aci = null;
        UsersModel um;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        string company_id;
        public FinanceManager(AddCompanyInfo aci, UsersModel um, string company_id)
        {
            InitializeComponent();
            this.aci = aci;
            this.um = um;
            this.txtYear.Text = DateTime.Now.Year.ToString();
            this.company_id = company_id;
        }

        public FinanceManager(AddCompanyInfo aci, UsersModel um, string year, string capital, string debt, string sales, string net_income, string company_id)
        {
            InitializeComponent();
            this.aci = aci;
            this.um = um;
            this.txtYear.Text = year;
            this.txtCapitalAmount.Text = capital;
            this.txtDebtAmount.Text = debt;
            this.txtSalesAmount.Text = sales;
            this.txtNetIncomeAmount.Text = net_income;
            this.company_id = company_id;
        }

        #region Method
        private string GetN2(string A)
        {
            double B = Convert.ToDouble(A);
            string result;
            if (B == (int)B)
                result = B.ToString("#,##0");
            else
                result = B.ToString("N2");

            return result;
        }
        private bool isValidate()
        {
            if (!int.TryParse(txtYear.Text, out int year) || year < 1900)
            {
                messageBox.Show(this, "연도값을 확인해주세요!");
                return false;
            }

            else if (!double.TryParse(txtCapitalAmount.Text, out double capital))
            {
                messageBox.Show(this, "자본총계 값을 확인해주세요!");
                return false;
            }
            else if (!double.TryParse(txtDebtAmount.Text, out double debt))
            {
                messageBox.Show(this, "부채총계 값을 확인해주세요!");
                return false;
            }
            else if (!double.TryParse(txtSalesAmount.Text, out double sales))
            {
                messageBox.Show(this, "매출총액 값을 확인해주세요!");
                return false;
            }
            else if (!double.TryParse(txtNetIncomeAmount.Text, out double net_income))
            {
                messageBox.Show(this, "당기순이익 값을 확인해주세요!");
                return false;
            }
            else
                return true;

        }
        #endregion

        #region Button
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!isValidate())
                return;

            /*if (messageBox.Show(this, "삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;*/

            CompanyFinanceModel model = new CompanyFinanceModel();

            if (int.TryParse(txtYear.Text, out int year))
                model.year = year;
            //삭제
            if (aci != null)
                aci.DeleteCompanyFinance(model);
            messageBox.Show(this, "삭제완료");
        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            if (!isValidate())
                return;

            /*if (messageBox.Show(this, "등록/수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;*/

            CompanyFinanceModel model = new CompanyFinanceModel();

            if (int.TryParse(txtYear.Text, out int year))
                model.year = year;

            if (double.TryParse(txtCapitalAmount.Text, out double capital))
                model.capital_amount = capital;

            if (double.TryParse(txtDebtAmount.Text, out double debt))
                model.debt_amount = debt;

            if (double.TryParse(txtSalesAmount.Text, out double sales))
                model.sales_amount = sales;

            if (double.TryParse(txtNetIncomeAmount.Text, out double net_income))
                model.net_income_amount = net_income;
            //등록
            if (aci != null)
                aci.AddCompanyFinance(model);
            messageBox.Show(this, "등록완료");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void FinanceManager_KeyDown(object sender, KeyEventArgs e)
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
        private void txtCapitalAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                try
                {
                    tb.Text = GetN2(tb.Text);
                    tb.SelectionStart = tb.TextLength; //** 캐럿을 맨 뒤로 보낸다...
                    tb.SelectionLength = 0;
                }
                catch (Exception ex)
                { }
            }
        }
        #endregion


        
    }
}
