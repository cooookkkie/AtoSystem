using AdoNetWindow.Model;
using Repositories;
using Repositories.SalesPartner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement
{
    public partial class MySalesManager : Form
    {
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um; 
        Libs.Tools.Common common = new Libs.Tools.Common();
        public MySalesManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }
        private void MySalesManager_Load(object sender, EventArgs e)
        {
            txtSttdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtManager.Text = um.user_name;
            if (um.auth_level < 50)
            {
                lbManager.Visible = false;
                txtManager.Visible = false;
            }
            btnSearching.PerformClick();
        }

        #region Button
        private void btnSttdateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEnddateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            dgvSales.Rows.Clear();
            DataTable saleDt = salesPartnerRepository.GetSaleInfo(txtSttdate.Text, txtEnddate.Text, "", txtCompany.Text, cbExactly.Checked, txtManager.Text);
            if (saleDt.Rows.Count > 0)
            {
                this.dgvSales.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
                for (int i = 0; i < saleDt.Rows.Count; i++)
                {
                    int n = dgvSales.Rows.Add();
                    DataGridViewRow row = dgvSales.Rows[n];

                    row.Cells["company_id"].Value = saleDt.Rows[i]["company_id"].ToString();
                    row.Cells["sub_id"].Value = saleDt.Rows[i]["sub_id"].ToString();
                    row.Cells["company"].Value = saleDt.Rows[i]["company"].ToString();

                    DateTime dt;
                    if (DateTime.TryParse(saleDt.Rows[i]["updatetime"].ToString(), out dt))
                    {
                        row.Cells["updatetime_detail"].Value = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        row.Cells["updatetime"].Value = dt.ToString("yyyy-MM-dd");
                        //오후, 오전
                        int hour = dt.Hour;
                        if (hour < 13)
                        {
                            row.Cells["am"].Value = true;
                            row.Cells["pm"].Value = false;
                        }
                        else
                        {
                            row.Cells["am"].Value = false;
                            row.Cells["pm"].Value = true;
                        }
                    }

                    bool is_sales;
                    if (!bool.TryParse(saleDt.Rows[i]["is_sales"].ToString(), out is_sales))
                        is_sales = false;
                    row.Cells["is_sales"].Value = is_sales;

                    row.Cells["sale_contents"].Value = saleDt.Rows[i]["log"].ToString();
                    row.Cells["sale_remark"].Value = saleDt.Rows[i]["remark"].ToString();
                    row.Cells["edit_user"].Value = saleDt.Rows[i]["edit_user"].ToString();

                    //잠재1, 잠재2
                    bool isPotential;
                    if(!bool.TryParse(saleDt.Rows[i]["isPotential1"].ToString(), out isPotential))
                        isPotential = false;
                    row.Cells["isPotential1"].Value = isPotential;
                    if (!bool.TryParse(saleDt.Rows[i]["isPotential2"].ToString(), out isPotential))
                        isPotential = false;
                    row.Cells["isPotential2"].Value = isPotential;
                    
                }
                this.dgvSales.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellValueChanged);
            }
            SetCount();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            dgvSales.EndEdit();
            if (dgvSales.Rows.Count > 0)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < dgvSales.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvSales.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        CompanySalesModel model = new CompanySalesModel();
                        int company_id = 0;
                        if (dgvSales.Rows[i].Cells["company_id"].Value == null || !int.TryParse(dgvSales.Rows[i].Cells["company_id"].Value.ToString(), out company_id))
                        {
                            MessageBox.Show(dgvSales.Rows[i].Cells["company"].Value.ToString() + " 거래처의 정보를 찾을 수 없습니다.");
                            return;
                        }
                        model.company_id = company_id;

                        int sub_id = 0;
                        if (dgvSales.Rows[i].Cells["sub_id"].Value == null || !int.TryParse(dgvSales.Rows[i].Cells["sub_id"].Value.ToString(), out sub_id))
                        {
                            MessageBox.Show(dgvSales.Rows[i].Cells["company"].Value.ToString() + " 거래처의 정보를 찾을 수 없습니다.");
                            return;
                        }
                        model.sub_id = sub_id;  

                        DateTime updatetime;
                        if (dgvSales.Rows[i].Cells["updatetime_detail"].Value != null && DateTime.TryParse(dgvSales.Rows[i].Cells["updatetime_detail"].Value.ToString(), out updatetime))
                        {
                            if (Convert.ToBoolean(dgvSales.Rows[i].Cells["am"].Value))
                                model.updatetime = updatetime.ToString("yyyy-MM-dd") + " 12:00:00";
                            else
                                model.updatetime = updatetime.ToString("yyyy-MM-dd") + " 13:00:00";
                        }

                        bool is_sales;
                        if (dgvSales.Rows[i].Cells["is_sales"].Value == null || !bool.TryParse(dgvSales.Rows[i].Cells["is_sales"].Value.ToString(), out is_sales))
                            is_sales = false;
                        model.is_sales = is_sales;

                        if (dgvSales.Rows[i].Cells["sale_log"].Value == null)
                            dgvSales.Rows[i].Cells["sale_log"].Value = "";
                        model.log = dgvSales.Rows[i].Cells["sale_log"].Value.ToString();
                        if (dgvSales.Rows[i].Cells["sale_contents"].Value == null)
                            dgvSales.Rows[i].Cells["sale_contents"].Value = "";
                        model.contents = dgvSales.Rows[i].Cells["sale_contents"].Value.ToString();
                        if (dgvSales.Rows[i].Cells["edit_user"].Value == null)
                            dgvSales.Rows[i].Cells["edit_user"].Value = "";
                        model.edit_user = dgvSales.Rows[i].Cells["edit_user"].Value.ToString();
                        if (dgvSales.Rows[i].Cells["sale_remark"].Value == null)
                            dgvSales.Rows[i].Cells["sale_remark"].Value = "";
                        model.remark = dgvSales.Rows[i].Cells["sale_remark"].Value.ToString();

                        StringBuilder sql = salesPartnerRepository.UpdateSalesInfo(model);
                        sqlList.Add(sql);
                    }
                }
                if (sqlList.Count > 0)
                {
                    if (commonRepository.UpdateTran(sqlList) == -1)
                        MessageBox.Show("수정중 에러가 발생하였습니다.");
                    else
                        btnSearching.PerformClick();
                }
            }
        }
        #endregion

        #region Method
        private void SetCount()
        {
            dgvSales.EndEdit();
            txtTotal.Text = "0";
            txtAm.Text = "0";
            txtPm.Text = "0";
            txtPotential1.Text = "0";
            txtPotential2.Text = "0";
            if (dgvSales.Rows.Count > 0)
            {
                int total = 0, am = 0, pm = 0, potential1 = 0, potential2 = 0;


                for (int i = 0; i < dgvSales.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvSales.Rows[i];
                    if (Convert.ToBoolean(row.Cells["is_sales"].Value))
                    {
                        if (Convert.ToBoolean(row.Cells["am"].Value))
                            am++;
                        else
                            pm++;

                        if (Convert.ToBoolean(row.Cells["isPotential1"].Value))
                            potential1++;
                        else if (Convert.ToBoolean(row.Cells["isPotential2"].Value))
                            potential2++;
                    }
                }
                total = am + pm;
                txtTotal.Text = total.ToString("#,##0");
                txtAm.Text = am.ToString("#,##0");
                txtPm.Text = pm.ToString("#,##0");
                txtPotential1.Text = potential1.ToString("#,##0");
                txtPotential2.Text = potential2.ToString("#,##0");
            }
        }
        #endregion

        #region Datagridview event
        private void dgvSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSales.EndEdit();
            if (e.RowIndex >= 0)
            {
                bool isChecked;
                if (dgvSales.Columns[e.ColumnIndex].Name == "am")
                {
                    isChecked = Convert.ToBoolean(dgvSales.Rows[e.RowIndex].Cells["am"].Value);
                    dgvSales.Rows[e.RowIndex].Cells["am"].Value = isChecked;
                    dgvSales.Rows[e.RowIndex].Cells["pm"].Value = !isChecked;
                    SetCount();
                }
                else if (dgvSales.Columns[e.ColumnIndex].Name == "pm")
                {
                    isChecked = Convert.ToBoolean(dgvSales.Rows[e.RowIndex].Cells["pm"].Value);
                    dgvSales.Rows[e.RowIndex].Cells["pm"].Value = isChecked;
                    dgvSales.Rows[e.RowIndex].Cells["am"].Value = !isChecked;
                    SetCount();
                }
            }
        }

        private void dgvSales_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvSales.Columns[e.ColumnIndex].Name != "chk")
                    dgvSales.Rows[e.RowIndex].Cells["chk"].Value = true;
                else
                {
                    dgvSales.EndEdit();
                    if (Convert.ToBoolean(dgvSales.Rows[e.RowIndex].Cells["chk"].Value))
                        dgvSales.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.Red;
                    else
                        dgvSales.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.White;
                }
            }
        }

        #endregion

        #region Key event
        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
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
        private void MySalesManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = String.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
        }
        #endregion

    }
}
