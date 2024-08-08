using AdoNetWindow.Model;
using Microsoft.VisualBasic.ApplicationServices;
using Repositories;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.SalesDashboard2
{

    public partial class salesDashboard2 : Form
    {
        public class SelectUserModel
        {
            public string user_id { get; set; }
            public string department { get; set; }
            public string team { get; set; }
            public string grade { get; set; }
            public string user_name { get; set; }
        }
        public class SelectProductModel
        {
            public string product { get; set; }
            public string origin { get; set; }
        }

        public class SelectCompanyModel
        {
            public string seaover_company_code { get; set; }
            public string company { get; set; }
            public string registration_number { get; set; }
            public string ceo { get; set; }
            public string manager { get; set; }
        }

        IUsersRepository usersRepository = new UsersRepository();
        ISalesRepository SalesRepository = new SalesRepository();
        List<SelectUserModel> selectUserModel = new List<SelectUserModel>();
        List<SelectProductModel> selectProductModel = new List<SelectProductModel>();
        List<SelectCompanyModel> selectCompanyModel = new List<SelectCompanyModel>();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        public salesDashboard2(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void salesDashboard2_Load(object sender, EventArgs e)
        {
            for (int i = 2016; i <= DateTime.Now.Year; i++)
            {
                cbSttYear.Items.Add(i.ToString());
                cbEndYear.Items.Add(i.ToString());
            }

            txtSttdate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        #region Method

        #region 담당자별 매출현황
        private void GetUsers()
        {
            SelectUser su = new SelectUser(this, selectUserModel);
            selectUserModel = su.GetUsers();
            SetUnit();
        }
        private void SetUnit()
        {
            //검색기간
            int sttYear;
            if (!int.TryParse(cbSttYear.Text, out sttYear))
                messageBox.Show(this, "검색기간을 확인해주세요!");
            int endYear;
            if (!int.TryParse(cbEndYear.Text, out endYear))
                messageBox.Show(this, "검색기간을 확인해주세요!");
            //검색타임
            int dashboardType = rbSalesAmount.Checked ? 1 : rbCompanyCount.Checked ? 2 : 3;
            //출력=================================================================================================
            if (selectUserModel != null && selectUserModel.Count > 0)
            {
                for (int i = 0; i < selectUserModel.Count; i++)
                {
                    bool isExist = false;
                    foreach (SalesDashboardUnit units in pnSalesDashboard.Controls)
                    {
                        if (selectUserModel[i].user_id.Equals(units.GetUserid()))
                        {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist) 
                    {
                        SalesDashboardUnit unit = new SalesDashboardUnit(sttYear, endYear
                                                                        , selectUserModel[i].user_id, selectUserModel[i].department, selectUserModel[i].team, selectUserModel[i].user_name
                                                                        , dashboardType, cbIncreaseRate.Checked, txtProduct.Text, txtOrigin.Text, txtSizes.Text);
                        pnSalesDashboard.Controls.Add(unit);
                    }
                }
            }
        }
        private void SetProduct(bool isGetData = true)
        {
            //출력=================================================================================================
            dgvSales2.Rows.Clear();
            if (selectProductModel!= null && selectProductModel.Count > 0)
            {
                for (int i = 0; i < selectProductModel.Count; i++)
                {
                    bool isExist = false;
                    foreach (DataGridViewRow row in dgvSales2.Rows)
                    {
                        if (row.Cells["product"].Value.ToString().Equals(selectProductModel[i].product)
                            && row.Cells["origin"].Value.ToString().Equals(selectProductModel[i].origin))
                        {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist)
                    {
                        int n = dgvSales2.Rows.Add();
                        dgvSales2.Rows[n].Cells["product"].Value = selectProductModel[i].product;
                        dgvSales2.Rows[n].Cells["origin"].Value = selectProductModel[i].origin;
                    }
                }
                int m = dgvSales2.Rows.Add();
                dgvSales2.Rows[m].Cells["product"].Value = "합계";
                dgvSales2.Rows[m].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
                dgvSales2.Rows[m].DefaultCellStyle.BackColor = Color.Beige;

            }
            if(isGetData)
                GetProductSales();
        }
        #endregion

        #region 품목별 매출현황
        private void GetUsers(string department, string team, string grade, string user_name, bool isGetData = true)
        {
            

            DataGridView currentDgv = dgvSales2;
            if (tabControl1.SelectedIndex == 1)
            {
                if (selectProductModel.Count == 0)
                {
                    SelectProduct sp = new SelectProduct(um, this, selectProductModel);
                    selectProductModel = sp.GetProducts();
                    SetProduct(false);
                }
                currentDgv = dgvSales2;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                if (selectCompanyModel.Count == 0)
                {
                    SelectCompany sc = new SelectCompany(um, this, selectCompanyModel);
                    selectCompanyModel = sc.GetCompanys();
                    SetCompany(false);
                }
                currentDgv = dgvSales3;
            }

            //dgvSales2.Rows.Clear();
            for (int i = currentDgv.ColumnCount - 1; i >= 2; i--)
                currentDgv.Columns.Remove(currentDgv.Columns[i]);

            DataTable userDt = usersRepository.GetUsers("", department, team, user_name, "승인", grade);
            if (userDt.Rows.Count > 0)
            {
                for (int i = 0; i < userDt.Rows.Count; i++)
                {
                    bool isExist = false;
                    foreach (DataGridViewColumn col in currentDgv.Columns)
                    {
                        if (col.Name.Equals(userDt.Rows[i]["user_id"].ToString()))
                        {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist)
                        currentDgv.Columns.Add(userDt.Rows[i]["user_id"].ToString(), userDt.Rows[i]["user_name"].ToString());
                }
                currentDgv.Columns.Add("sum", "SUM");
                currentDgv.Columns["sum"].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
                currentDgv.Columns["sum"].DefaultCellStyle.BackColor = Color.Beige;
            }
            if (isGetData)
            {
                if (tabControl1.SelectedIndex == 1)
                    GetProductSales();
                else if (tabControl1.SelectedIndex == 2)
                    GetCompanySales();
            }
        }

        private void GetProductSales()
        {
            //검색기간
            DateTime sttdate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate))
            {
                messageBox.Show(this, "검색기간을 다시 확인해주세요.");
                return;
            }
            DateTime enddate;
            if (!DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                messageBox.Show(this, "검색기간을 다시 확인해주세요.");
                return;
            }
            //검색 유저
            string usernameList = "";
            foreach (DataGridViewColumn col in dgvSales2.Columns)
            {
                usernameList += " " + col.HeaderText;
            }
            usernameList = usernameList.Trim();
            //검색 품명
            List<string> productLIst = new List<string>();
            foreach (SelectProductModel model in selectProductModel)
                productLIst.Add(model.product + "^" + model.origin);


            //데이터 출력
            DataTable productSalesDt = SalesRepository.GetSalesDashboardByProduct(sttdate, enddate, productLIst, usernameList);
            if(productSalesDt.Rows.Count > 0) 
            {
                foreach (DataGridViewRow row in dgvSales2.Rows)
                {
                    if (row.Index < dgvSales2.RowCount - 1)
                    {
                        double column_sum = 0;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.OwningColumn.Name != "product" && cell.OwningColumn.Name != "origin" && cell.OwningColumn.Name != "sum")
                            {
                                string whr = $"품명 = '{row.Cells["product"].Value.ToString()}' AND 원산지 = '{row.Cells["origin"].Value.ToString()}' AND 매출자 = '{cell.OwningColumn.HeaderText}'";
                                DataRow[] productSalesDr = productSalesDt.Select(whr);
                                if (productSalesDr.Length > 0)
                                {
                                    cell.Value = Convert.ToDouble(productSalesDr[0]["매출금액"].ToString()).ToString("#,##0");
                                    column_sum += Convert.ToDouble(productSalesDr[0]["매출금액"].ToString());
                                }
                            }
                        }
                        row.Cells["sum"].Value = column_sum.ToString("#,##0");
                    }
                }

                //세로합계
                if (dgvSales2.Rows.Count > 0)
                {
                    foreach (DataGridViewColumn col in dgvSales2.Columns)
                    {
                        if (col.Name != "product" && col.Name != "origin")
                        { 
                            double row_sum = 0;
                            foreach (DataGridViewRow row in dgvSales2.Rows)
                            {
                                if (dgvSales2.Rows.Count - 1 > row.Index)
                                {
                                    double sales_amount;
                                    if (row.Cells[col.Index].Value == null || !double.TryParse(row.Cells[col.Index].Value.ToString(), out sales_amount))
                                        sales_amount = 0;
                                    row_sum += sales_amount;
                                }
                            }
                            dgvSales2.Rows[dgvSales2.Rows.Count - 1].Cells[col.Index].Value = row_sum.ToString("#,##0");
                        }
                    }
                }
            }

            if (dgvSales2.ColumnCount > 2)
            {
                for (int i = 2; i < dgvSales2.ColumnCount; i++)
                    dgvSales2.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void GetProduct()
        {
            SelectProduct sp = new SelectProduct(um, this, selectProductModel);
            selectProductModel = sp.GetProducts();

            SetProduct(false);
            GetUsers(txtDepartment.Text, txtTeam.Text, txtGrade.Text, txtUsername.Text, false);
            GetProductSales();
        }
        private void GetCompany()
        {
            SelectCompany sc = new SelectCompany(um, this, selectCompanyModel);
            selectCompanyModel = sc.GetCompanys();

            SetCompany(false);
            GetUsers(txtDepartment.Text, txtTeam.Text, txtGrade.Text, txtUsername.Text, false);
            GetCompanySales();
        }
        #endregion

        #region 거래처별 매출현황
        private void SetCompany(bool isGetData = true)
        {
            //출력=================================================================================================
            dgvSales3.Rows.Clear();
            if (selectCompanyModel != null && selectCompanyModel.Count > 0)
            {
                for (int i = 0; i < selectCompanyModel.Count; i++)
                {
                    bool isExist = false;
                    foreach (DataGridViewRow row in dgvSales3.Rows)
                    {
                        if (row.Cells["seaover_company_code"].Value.ToString().Equals(selectCompanyModel[i].seaover_company_code))
                        {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist)
                    {
                        int n = dgvSales3.Rows.Add();
                        dgvSales3.Rows[n].Cells["seaover_company_code"].Value = selectCompanyModel[i].seaover_company_code;
                        dgvSales3.Rows[n].Cells["company"].Value = selectCompanyModel[i].company;
                    }
                }
                int m = dgvSales3.Rows.Add();
                dgvSales3.Rows[m].Cells["company"].Value = "합계";
                dgvSales3.Rows[m].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
                dgvSales3.Rows[m].DefaultCellStyle.BackColor = Color.Beige;

            }
            if (isGetData)
                GetCompanySales();
        }
        private void GetCompanySales()
        {
            //검색기간
            DateTime sttdate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate))
            {
                messageBox.Show(this, "검색기간을 다시 확인해주세요.");
                return;
            }
            DateTime enddate;
            if (!DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                messageBox.Show(this, "검색기간을 다시 확인해주세요.");
                return;
            }
            //검색 유저
            string usernameList = "";
            foreach (DataGridViewColumn col in dgvSales3.Columns)
            {
                usernameList += " " + col.HeaderText;
            }
            usernameList = usernameList.Trim();

            //데이터 출력
            DataTable companySalesDt = SalesRepository.GetSalesDashboardByCompany(sttdate, enddate, usernameList);
            if (companySalesDt.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvSales3.Rows)
                {
                    if (row.Index < dgvSales3.RowCount - 1)
                    {
                        double column_sum = 0;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.OwningColumn.Name != "seaover_company_code" && cell.OwningColumn.Name != "company" && cell.OwningColumn.Name != "sum")
                            {
                                string whr = $"매출처코드 = '{row.Cells["seaover_company_code"].Value.ToString()}' AND 매출자 = '{cell.OwningColumn.HeaderText}'";
                                DataRow[] companySalesDr = companySalesDt.Select(whr);
                                if (companySalesDr.Length > 0)
                                {
                                    cell.Value = Convert.ToDouble(companySalesDr[0]["매출금액"].ToString()).ToString("#,##0");
                                    column_sum += Convert.ToDouble(companySalesDr[0]["매출금액"].ToString());
                                }
                            }
                        }
                        row.Cells["sum"].Value = column_sum.ToString("#,##0");
                    }
                }

                //세로합계
                if (dgvSales3.Rows.Count > 0)
                {
                    foreach (DataGridViewColumn col in dgvSales3.Columns)
                    {
                        if (col.Name != "seaover_company_code" && col.Name != "company")
                        {
                            double row_sum = 0;
                            foreach (DataGridViewRow row in dgvSales3.Rows)
                            {
                                if (dgvSales3.Rows.Count - 1 > row.Index)
                                {
                                    double sales_amount;
                                    if (row.Cells[col.Index].Value == null || !double.TryParse(row.Cells[col.Index].Value.ToString(), out sales_amount))
                                        sales_amount = 0;
                                    row_sum += sales_amount;
                                }
                            }
                            dgvSales3.Rows[dgvSales3.Rows.Count - 1].Cells[col.Index].Value = row_sum.ToString("#,##0");
                        }
                    }
                }

                if (dgvSales3.ColumnCount > 2)
                {
                    for (int i = 2; i < dgvSales3.ColumnCount; i++)
                        dgvSales3.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
        #endregion
        private void RefreshDashboard()
        {
            if (selectUserModel.Count == 0)
                GetUsers();

            int dashboardType;
            if (rbSalesAmount.Checked)
                dashboardType = 1;
            else
                dashboardType = 2;

            foreach (SalesDashboardUnit unit in pnSalesDashboard.Controls)
                unit.SetDashboardType(dashboardType, txtProduct.Text, txtOrigin.Text, txtSizes.Text);
        }
        #endregion


        #region Button, Checkbox, RadioButton
        private void btnGetUsers_Click(object sender, EventArgs e)
        {
            GetUsers();
        }
        private void btnGetCompany_Click(object sender, EventArgs e)
        {
            GetCompany();
        }
        
        private void btnGetProduct_Click(object sender, EventArgs e)
        {
            GetProduct();
        }
        private void btnCalendarSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtSttdate.Text = sdate;
            }
        }

        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtEnddate.Text = sdate;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                pnSalesDashboard.Controls.Clear();
            else if (tabControl1.SelectedIndex == 1)
                dgvSales2.Rows.Clear();
            else if (tabControl1.SelectedIndex == 2)
                dgvSales3.Rows.Clear();
        }
        private void cbIncreaseRate_CheckedChanged(object sender, EventArgs e)
        {
            if (pnSalesDashboard.Controls.Count > 0)
            {
                foreach (SalesDashboardUnit unit in pnSalesDashboard.Controls)
                    unit.AutoIncreaseVisible(cbIncreaseRate.Checked);
            }
        }
        private void btnSelectUsers_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                RefreshDashboard();
            //GetUsers();
            else if (tabControl1.SelectedIndex == 1)
                GetUsers(txtDepartment.Text, txtTeam.Text, txtGrade.Text, txtUsername.Text, true);
            else if (tabControl1.SelectedIndex == 2)
                GetUsers(txtDepartment.Text, txtTeam.Text, txtGrade.Text, txtUsername.Text, true);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void rbSalesAmount_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDashboard();
        }
        #endregion

        #region Key event
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                System.Windows.Forms.TextBox tbb = (System.Windows.Forms.TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
                if (tbb.Name != "txtSttdate" && tbb.Name != "txtEnddate")
                {
                    DateTime dt;
                    if (DateTime.TryParse(tbb.Text, out dt))
                        tbb.Text = dt.ToString("yyyy-MM-dd");
                }


                GetUsers(txtDepartment.Text, txtTeam.Text, txtGrade.Text, txtUsername.Text);
            }
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                RefreshDashboard();
        }
        private void salesDashboard2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.N:
                        if (tabControl1.SelectedIndex == 0)
                        {
                            txtProduct.Text = string.Empty;
                            txtOrigin.Text = string.Empty;
                            txtSizes.Text = string.Empty;
                            txtProduct.Focus();
                        }
                        else if (tabControl1.SelectedIndex == 1)
                        {
                            txtDepartment.Text = string.Empty;
                            txtTeam.Text = string.Empty;
                            txtGrade.Text = string.Empty;
                            txtUsername.Text = string.Empty;
                            txtUsername.Focus();
                        }
                        break;
                    case Keys.M:
                        if (tabControl1.SelectedIndex == 0)
                            txtProduct.Focus();
                        else if (tabControl1.SelectedIndex == 1)
                            txtUsername.Focus();
                        break;
                    case Keys.Q:
                        btnSelect.PerformClick();
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
                    case Keys.F9:
                        if (tabControl1.SelectedIndex == 0)
                            btnGetUsers.PerformClick();
                        else if (tabControl1.SelectedIndex == 1)
                            btnGetProduct.PerformClick();
                        else if (tabControl1.SelectedIndex == 2)
                            btnGetCompany.PerformClick();
                        break;
                }
            }
        }


        #endregion


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                btnGetUsers.Visible = true;
                btnGetProduct.Visible = false;
                btnGetCompany.Visible = false;
                btnTargetProduct.Visible = false;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                btnGetUsers.Visible = false;
                btnGetProduct.Visible = true;
                btnGetCompany.Visible = false;
                btnTargetProduct.Visible = true;
                tabControl1.SelectedTab.Controls.Add(this.pnUserSearching);
                tabControl1.SelectedTab.Controls.Add(this.dgvSales2);
                this.pnUserSearching.BringToFront();

                this.dgvSales2.BringToFront();

            }
            else if (tabControl1.SelectedIndex == 2)
            {
                btnGetUsers.Visible = false;
                btnGetProduct.Visible = false;
                btnGetCompany.Visible = true;
                btnTargetProduct.Visible = false;
                tabControl1.SelectedTab.Controls.Add(this.pnUserSearching);
                this.pnUserSearching.BringToFront();
                this.dgvSales3.BringToFront();
            }
        }

        
    }
}

