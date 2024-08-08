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

namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    public partial class ManagerChanger : Form
    {
        ICompanyRepository companyRepository = new CompanyRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ICommonRepository commonRepository = new CommonRepository();    
        Libs.MessageBox messageBox = new Libs.MessageBox();
        SalesManager sm;
        UsersModel um;
        public ManagerChanger(UsersModel um, SalesManager sm, List<DataGridViewRow> rowList)
        {
            InitializeComponent();
            this.sm = sm;
            this.um = um;
            for(int i = 0;i < rowList.Count; i++) 
            {
                int n = dgvCompany.Rows.Add();
                dgvCompany.Rows[n].Cells["company_id"].Value = rowList[i].Cells["id"].Value.ToString();
                dgvCompany.Rows[n].Cells["table_index"].Value = rowList[i].Cells["table_index"].Value.ToString();
                dgvCompany.Rows[n].Cells["category"].Value = rowList[i].Cells["category"].Value.ToString();
                dgvCompany.Rows[n].Cells["company"].Value = rowList[i].Cells["company"].Value.ToString();
                dgvCompany.Rows[n].Cells["pre_manager"].Value = rowList[i].Cells["pre_ato_manager"].Value.ToString();
                dgvCompany.Rows[n].Cells["div"].Value = "→";
            }
        }

        #region Button
        private void btnInsert_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dgvCompany.Rows.Count; i++) 
            {
                if (dgvCompany.Rows[i].Cells["after_manager"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["after_manager"].Value.ToString()))
                {
                    messageBox.Show(this, "변경할 담당자가 입력되지 않았습니다!");
                    dgvCompany.CurrentCell = dgvCompany.Rows[i].Cells["after_manager"];
                    return;
                }
            }

            if (messageBox.Show(this, $"선택한 거래처를 변경할 담당자 거래처로 이동시키겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;


            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            foreach (DataGridViewRow row in dgvCompany.Rows)
            {
                string[] col = new string[3];
                col[0] = "ato_manager";
                col[1] = "updatetime";
                col[2] = "edit_user";
                string[] val = new string[3];
                val[0] = "'" + row.Cells["after_manager"].Value.ToString() + "'";
                val[1] = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                val[2] = "'" + um.user_name + "'";
                string id = row.Cells["company_id"].Value.ToString();
                //거래처 정보수정
                sql = companyRepository.UpdateCompanyColumns(col, val, $"id={id}");
                sqlList.Add(sql);

                //거래처 영업내용============================================================================
                CompanySalesModel sModel = new CompanySalesModel();
                sModel.company_id = Convert.ToInt32(id);
                sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
                sModel.is_sales = false;
                sModel.contents = "담당자 변경";
                sModel.edit_user = um.user_name;
                sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                sModel.from_ato_manager = row.Cells["pre_manager"].Value.ToString();
                sModel.to_ato_manager = row.Cells["after_manager"].Value.ToString();
                sModel.from_category = row.Cells["category"].Value.ToString();
                sModel.to_category = row.Cells["category"].Value.ToString();

                //거래처 영업로그
                sql = salesPartnerRepository.InsertPartnerSales(sModel);
                sqlList.Add(sql);
            }

            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                    messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                else
                {
                    //데이터 반영
                    foreach (DataGridViewRow row in dgvCompany.Rows)
                    {
                        if (sm != null)
                        {
                            int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                            sm.UpdateAtoDt(table_index, "sales_updatetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            sm.UpdateAtoDt(table_index, "sales_edit_user", um.user_name);
                            sm.UpdateAtoDt(table_index, "sales_contents", "담당자 변경");
                            sm.UpdateAtoDt(table_index, "pre_ato_manager", row.Cells["pre_manager"].Value.ToString());
                            sm.UpdateAtoDt(table_index, "ato_manager", row.Cells["after_manager"].Value.ToString());
                            //sm.UpdateAtoDt(table_index, "pre_category", row.Cells["category"].Value.ToString());
                            sm.UpdateAtoDt(table_index, "category", row.Cells["category"].Value.ToString());
                        }
                    }

                    messageBox.Show(this, "완료");
                    this.Dispose();
                }
            }
            else
                messageBox.Show(this, "등록할 내역이 없습니다.");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void ManagerChanger_KeyDown(object sender, KeyEventArgs e)
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
