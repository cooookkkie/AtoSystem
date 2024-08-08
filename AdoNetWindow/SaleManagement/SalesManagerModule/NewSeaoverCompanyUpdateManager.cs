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
    public partial class NewSeaoverCompanyUpdateManager : Form
    {
        ICompanyRepository companyRepository = new CompanyRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        SalesManager sm;
        UsersModel um;
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public NewSeaoverCompanyUpdateManager(UsersModel um, SalesManager sm, List<string[]> updateList)
        {
            InitializeComponent();
            this.um = um;
            this.sm = sm;

            foreach (string[] list in updateList)
            {
                int n = dgvCompany.Rows.Add();
                dgvCompany.Rows[n].Cells["company_id"].Value = list[0];
                dgvCompany.Rows[n].Cells["division"].Value = list[1];
                dgvCompany.Rows[n].Cells["ato_company"].Value = list[2];
                dgvCompany.Rows[n].Cells["ato_manager"].Value = list[3];
                dgvCompany.Rows[n].Cells["table_index"].Value = list[4];
                dgvCompany.Rows[n].Cells["arrow"].Value = "➞";
                dgvCompany.Rows[n].Cells["seaover_company"].Value = list[5];
                dgvCompany.Rows[n].Cells["seaover_company_code"].Value = list[6];
                dgvCompany.Rows[n].Cells["sales_manager"].Value = list[7];
                dgvCompany.Rows[n].Cells["current_sales_date"].Value = list[8];
                /*DateTime current_sales_date;
                if (DateTime.TryParse(updateList[i][8], out current_sales_date) && DateTime.Now.AddMonths(-8) <= current_sales_date)
                    dgvCompany.Rows[n].Cells["current_sales_date"].Value = current_sales_date.ToString("yyyy-MM-dd");*/    
            }
            //정렬
            if (dgvCompany.Rows.Count > 0)
            {
                DataTable companyDt = common.ConvertDgvToDataTable(dgvCompany);
                DataView dv = new DataView(companyDt);
                dv.Sort = "current_sales_date DESC";
                companyDt = dv.ToTable();

                dgvCompany.Rows.Clear();
                for (int i = 0; i < companyDt.Rows.Count; i++)
                {
                    int n = dgvCompany.Rows.Add();
                    dgvCompany.Rows[n].Cells["company_id"].Value = companyDt.Rows[i]["company_id"].ToString();
                    dgvCompany.Rows[n].Cells["division"].Value = companyDt.Rows[i]["division"].ToString();
                    dgvCompany.Rows[n].Cells["ato_company"].Value = companyDt.Rows[i]["ato_company"].ToString();
                    dgvCompany.Rows[n].Cells["ato_manager"].Value = companyDt.Rows[i]["ato_manager"].ToString();
                    dgvCompany.Rows[n].Cells["table_index"].Value = companyDt.Rows[i]["table_index"].ToString();
                    dgvCompany.Rows[n].Cells["arrow"].Value = "➞";
                    dgvCompany.Rows[n].Cells["seaover_company"].Value = companyDt.Rows[i]["seaover_company"].ToString();
                    dgvCompany.Rows[n].Cells["seaover_company_code"].Value = companyDt.Rows[i]["seaover_company_code"].ToString();
                    dgvCompany.Rows[n].Cells["sales_manager"].Value = companyDt.Rows[i]["sales_manager"].ToString();
                    DateTime current_sales_date;
                    if (DateTime.TryParse(companyDt.Rows[i]["current_sales_date"].ToString(), out current_sales_date))
                    {
                        dgvCompany.Rows[n].Cells["current_sales_date"].Value = current_sales_date.ToString("yyyy-MM-dd");
                        if (DateTime.Now.AddMonths(-8) > current_sales_date)
                            dgvCompany.Rows[n].DefaultCellStyle.ForeColor = Color.LightGray;
                    }                        
                }
            }
        }
        private void NewSeaoverCompanyUpdateManager_Load(object sender, EventArgs e)
        {
        }

        #region Key event
        private void NewSeaoverCompanyUpdateManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {

            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnRegistrator.PerformClick();
                        break;
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnRegistrator_Click(object sender, EventArgs e)
        {
            dgvCompany.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];

                int id;
                if (!int.TryParse(row.Cells["company_id"].Value.ToString(), out id))
                    id = 0;
                //SEAOVER 거래처일 경우
                if (id == 0)
                {
                    dgvCompany.ClearSelection();
                    row.Selected = true;
                    dgvCompany.FirstDisplayedScrollingRowIndex = row.Index;
                    messageBox.Show(this, "등록되지 않은 거래처입니다.( 혹은 SEAOVER 거래처입니다.)");
                    this.Activate();
                    return;
                }

                //거래처 삭제
                sql = companyRepository.RealDeleteCompany(id);
                sqlList.Add(sql);
                //영업내역
                CompanySalesModel sModel = new CompanySalesModel();
                sModel.company_id = id;
                sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
                sModel.is_sales = false;
                sModel.contents = "SEAOVER 중복삭제";
                sModel.log = "";
                sModel.remark = "";
                sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sModel.edit_user = um.user_name;

                sql = salesPartnerRepository.InsertPartnerSales(sModel);
                sqlList.Add(sql);
            }
            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this,"수정중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                        DataTable atoDt = sm.HideCompanyData(table_index, true);
                    }
                    messageBox.Show(this,"삭제완료");
                    this.Activate();
                    sm.GetData(false, false);
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            /*this.Dispose();*/
            dgvCompany.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];

                int id;
                if (!int.TryParse(row.Cells["company_id"].Value.ToString(), out id))
                    id = 0;
                //SEAOVER 거래처일 경우
                if (id == 0)
                {
                    dgvCompany.ClearSelection();
                    row.Selected = true;
                    dgvCompany.FirstDisplayedScrollingRowIndex = row.Index;
                    messageBox.Show(this,"등록되지 않은 거래처입니다.( 혹은 SEAOVER 거래처입니다.)");
                    this.Activate();
                    this.Dispose();
                    return;
                }

                //거래처 삭제
                sql = companyRepository.DeleteCompany(id, true);
                sqlList.Add(sql);
                //영업내역
                CompanySalesModel sModel = new CompanySalesModel();
                sModel.company_id = id;
                sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
                sModel.is_sales = false;
                sModel.contents = "SEAOVER 중복삭제";
                sModel.log = "";
                sModel.remark = "";
                sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sModel.edit_user = um.user_name;

                sql = salesPartnerRepository.InsertPartnerSales(sModel);
                sqlList.Add(sql);
            }
            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this,"수정중 에러가 발생하였습니다.");
                    this.Activate();
                    this.Dispose();
                }
                else
                {
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                        DataTable atoDt = sm.SetCompanyData(table_index, true);
                    }
                    messageBox.Show(this,"삭제완료");
                    this.Activate();
                    sm.GetData(false, false);
                    this.Dispose();
                }
            }
        }

        #endregion

        private void NewSeaoverCompanyUpdateManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnExit.PerformClick();
        }
    }
}
