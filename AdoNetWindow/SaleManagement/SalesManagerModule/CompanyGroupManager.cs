
using AdoNetWindow.Model;
using AdoNetWindow.RecoveryPrincipal;
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

namespace AdoNetWindow.SaleManagement
{
    public partial class CompanyGroupManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        ICompanyGroupRepository companyGroupRepository = new CompanyGroupRepository();
        SalesManager sm = null;
        SalesPartnerManager2 spm2 = null;
        UsersModel um;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public CompanyGroupManager(UsersModel um, SalesManager sm, List<DataGridViewRow> companyList)
        {
            InitializeComponent();
            this.um = um;
            this.sm = sm;
            for (int i = 0; i < companyList.Count; i++)
            {
                if (companyList[i].Cells["seaover_company_code"].Value != null && !string.IsNullOrEmpty(companyList[i].Cells["seaover_company_code"].Value.ToString().Trim()))
                {
                    int n = dgvCompany.Rows.Add();
                    dgvCompany.Rows[n].Cells["company_id"].Value = companyList[i].Cells["seaover_company_code"].Value.ToString();
                    dgvCompany.Rows[n].Cells["company"].Value = companyList[i].Cells["company"].Value.ToString();
                    dgvCompany.Rows[n].Cells["registration_number"].Value = companyList[i].Cells["registration_number"].Value.ToString();
                    dgvCompany.Rows[n].Cells["ceo"].Value = companyList[i].Cells["ceo"].Value.ToString();
                    dgvCompany.Rows[n].Cells["tel"].Value = companyList[i].Cells["tel"].Value.ToString();
                    dgvCompany.Rows[n].Cells["fax"].Value = companyList[i].Cells["fax"].Value.ToString();
                    dgvCompany.Rows[n].Cells["phone"].Value = companyList[i].Cells["phone"].Value.ToString();

                    dgvCompany.Rows[n].Cells["ato_manager"].Value = companyList[i].Cells["ato_manager"].Value.ToString();
                    dgvCompany.Rows[n].Cells["sales_edit_user"].Value = companyList[i].Cells["sales_edit_user"].Value.ToString();
                    dgvCompany.Rows[n].Cells["sales_contents"].Value = companyList[i].Cells["sales_contents"].Value.ToString();
                    dgvCompany.Rows[n].Cells["sales_updatetime"].Value = companyList[i].Cells["sales_updatetime"].Value.ToString();
                    dgvCompany.Rows[n].Cells["sales_remark"].Value = companyList[i].Cells["sales_remark"].Value.ToString();

                    dgvCompany.Rows[n].Cells["table_index"].Value = companyList[i].Cells["table_index"].Value.ToString();
                }
            }
        }
        public CompanyGroupManager(UsersModel um, SalesPartnerManager2 spm2, List<DataGridViewRow> companyList)
        {
            InitializeComponent();
            this.um = um;
            this.spm2 = spm2;
            for (int i = 0; i < companyList.Count; i++)
            {
                if (companyList[i].Cells["seaover_company_code"].Value != null && !string.IsNullOrEmpty(companyList[i].Cells["seaover_company_code"].Value.ToString().Trim()))
                {
                    int n = dgvCompany.Rows.Add();
                    dgvCompany.Rows[n].Cells["company_id"].Value = companyList[i].Cells["seaover_company_code"].Value.ToString();
                    dgvCompany.Rows[n].Cells["company"].Value = companyList[i].Cells["company"].Value.ToString();
                    dgvCompany.Rows[n].Cells["registration_number"].Value = "-";
                    dgvCompany.Rows[n].Cells["ceo"].Value = "-";
                    dgvCompany.Rows[n].Cells["tel"].Value = "-";
                    dgvCompany.Rows[n].Cells["fax"].Value = "-";
                    dgvCompany.Rows[n].Cells["phone"].Value = "-";

                    dgvCompany.Rows[n].Cells["ato_manager"].Value = "-";
                    dgvCompany.Rows[n].Cells["sales_edit_user"].Value = "-";
                    dgvCompany.Rows[n].Cells["sales_contents"].Value = "-";
                    dgvCompany.Rows[n].Cells["sales_updatetime"].Value = "-";
                    dgvCompany.Rows[n].Cells["sales_remark"].Value = "-";

                    dgvCompany.Rows[n].Cells["table_index"].Value = companyList[i].Cells["table_index"].Value.ToString();
                }
            }
        }

        #region Datagridview event
        private void dgvCompany_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCompany.Rows.Count > 0)
            {
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    dgvCompany.Rows[i].Cells["chk"].Value = false;
                    dgvCompany.Rows[i].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Regular);
                }

                dgvCompany.Rows[dgvCompany.CurrentCell.RowIndex].Cells["chk"].Value = true;
                dgvCompany.Rows[dgvCompany.CurrentCell.RowIndex].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            }
        }
        #endregion

        #region Key event
        private void CompanyGroupManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                { 
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnRegister.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnRegister_Click(object sender, EventArgs e)
        {
            //대표 거래처  
            int main_idx = -1;
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                {
                    main_idx = i;
                    break;
                }
            }
            if (main_idx == -1)
            {
                messageBox.Show(this,"대표 거래처를 선택해주세요.");
                this.Activate();
            }
            else
            {
                int main_id = commonRepository.GetNextId("t_company_group", "main_id");
                string main_company_id = dgvCompany.Rows[main_idx].Cells["company_id"].Value.ToString();
                //기존 대표품목 삭제
                List<StringBuilder> sqlList = new List<StringBuilder>();


                //재등록
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    CompanyGroupModel model = new CompanyGroupModel();
                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                        model.sub_id = 0;
                    else
                        model.sub_id = 1;

                    model.main_id = main_id;
                    model.company_id = dgvCompany.Rows[i].Cells["company_id"].Value.ToString(); ;
                    model.company = dgvCompany.Rows[i].Cells["company"].Value.ToString(); ;
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //이미 등록된 그룹 삭제
                    StringBuilder sql = companyGroupRepository.DeleteCompanyGroup(model.company_id);
                    sqlList.Add(sql);
                    //새로 그룹 등록
                    sql = companyGroupRepository.InsertCompanyGroup(model);
                    sqlList.Add(sql);
                }
                //Execute
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this,"등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    if (sm != null)
                    {
                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(dgvCompany.Rows[0].Cells["sales_updatetime"].Value.ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        string ato_manager = dgvCompany.Rows[0].Cells["ato_manager"].Value.ToString();
                        string sale_edit_user = dgvCompany.Rows[0].Cells["sales_edit_user"].Value.ToString();
                        string sale_contents = dgvCompany.Rows[0].Cells["sales_contents"].Value.ToString();
                        string sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                        string sale_remark = dgvCompany.Rows[0].Cells["sales_remark"].Value.ToString();

                        for (int i = 0; i < dgvCompany.Rows.Count; i++)
                        {
                            //최근 영업정보
                            DateTime temp_updatetime;
                            if (!DateTime.TryParse(dgvCompany.Rows[i].Cells["sales_updatetime"].Value.ToString(), out temp_updatetime))
                                temp_updatetime = new DateTime(1900, 1, 1);

                            if (sales_updatetime < temp_updatetime)
                            {
                                sales_updatetime = temp_updatetime;
                                ato_manager = dgvCompany.Rows[i].Cells["ato_manager"].Value.ToString();
                                sale_edit_user = dgvCompany.Rows[i].Cells["sales_edit_user"].Value.ToString();
                                sale_contents = dgvCompany.Rows[i].Cells["sales_contents"].Value.ToString();
                                sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                sale_remark = dgvCompany.Rows[i].Cells["sales_remark"].Value.ToString();

                            }
                        }

                        //최신화
                        for (int i = 0; i < dgvCompany.Rows.Count; i++)
                        {
                            int rowIndex = Convert.ToInt32(dgvCompany.Rows[i].Cells["table_index"].Value);
                            int sub_id = 1;
                            if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                sub_id = 0;
                            if(sm != null)
                                sm.SetGroupCompany(rowIndex, main_id, sub_id, ato_manager, sale_edit_user, sale_contents, sale_date, sale_remark);
                            
                        }
                        if (sm != null)
                            sm.GetData();
                    }
                    if (sm != null)
                        messageBox.Show(this,"등록완료");
                    if (spm2 != null)
                        spm2.SetGroupCompany();
                    this.Activate();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion
    }
}
