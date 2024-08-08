using AdoNetWindow.Model;
using Repositories;
using Repositories.SalesPartner;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement
{
    public partial class SalesManager : Form
    {
        UsersModel um;
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        ISalesRepository salesRepository = new SalesRepository();
        ICommonRepository commonRepository = new CommonRepository();
        DataTable mainDt = new DataTable();
        DataTable Dt0 = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable Dt3 = new DataTable();
        DataTable Dt4 = new DataTable();
        DataTable Dt5 = new DataTable();

        public SalesManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void SalesManager_Load(object sender, EventArgs e)
        {
            mainDt = SetTable();
            Dt0 = SetTable();
            Dt1 = SetTable();
            Dt2 = SetTable();
            Dt3 = SetTable();
            Dt4 = SetTable();
            Dt5 = SetTable();
            txtManager.Text = um.user_name;
            if (um.auth_level >= 90)
            {
                lbManager.Visible = true;
                txtManager.Visible = true;
            }
            else
            {
                lbManager.Visible = false;
                txtManager.Visible = false;
                foreach (TabPage page in tcMain.TabPages)
                {
                    if (page.Name == "tabCommonData")
                        tcMain.TabPages.Remove(page);
                }
            }
        }

        #region Method
        private DataTable SetTable()
        {
            DataTable dt = new DataTable();

            DataColumn col01 = new DataColumn();
            col01.DataType = System.Type.GetType("System.Boolean");
            col01.AllowDBNull = false;
            col01.ColumnName = "chk";
            col01.Caption = "";
            col01.DefaultValue = false; 
            dt.Columns.Add(col01);

            DataColumn col02 = new DataColumn();
            col02.DataType = System.Type.GetType("System.Double");
            col02.AllowDBNull = true;
            col02.ColumnName = "id";
            col02.Caption = "id";
            col02.DefaultValue = null;
            dt.Columns.Add(col02);

            DataColumn col03 = new DataColumn();
            col03.DataType = System.Type.GetType("System.String");
            col03.AllowDBNull = false;
            col03.ColumnName = "group_name";
            col03.Caption = "그룹";
            col03.DefaultValue = "";
            dt.Columns.Add(col03);

            DataColumn col04 = new DataColumn();
            col04.DataType = System.Type.GetType("System.String");
            col04.AllowDBNull = false;
            col04.ColumnName = "company";
            col04.Caption = "거래처";
            col04.DefaultValue = "";
            dt.Columns.Add(col04);

            DataColumn col08 = new DataColumn();
            col08.DataType = System.Type.GetType("System.String");
            col08.AllowDBNull = false;
            col08.ColumnName = "registration_number";
            col08.Caption = "사업자번호";
            col08.DefaultValue = "";
            dt.Columns.Add(col08);

            DataColumn col081 = new DataColumn();
            col081.DataType = System.Type.GetType("System.String");
            col081.AllowDBNull = false;
            col081.ColumnName = "origin";
            col081.Caption = "국가";
            col081.DefaultValue = "";
            dt.Columns.Add(col081);

            DataColumn col090 = new DataColumn();
            col090.DataType = System.Type.GetType("System.String");
            col090.AllowDBNull = false;
            col090.ColumnName = "address";
            col090.Caption = "주소";
            col090.DefaultValue = "";
            dt.Columns.Add(col090);

            DataColumn col09 = new DataColumn();
            col09.DataType = System.Type.GetType("System.String");
            col09.AllowDBNull = false;
            col09.ColumnName = "ceo";
            col09.Caption = "대표자";
            col09.DefaultValue = "";
            dt.Columns.Add(col09);

            DataColumn col05 = new DataColumn();
            col05.DataType = System.Type.GetType("System.String");
            col05.AllowDBNull = false;
            col05.ColumnName = "tel";
            col05.Caption = "전화번호";
            col05.DefaultValue = "";
            dt.Columns.Add(col05);

            DataColumn col06 = new DataColumn();
            col06.DataType = System.Type.GetType("System.String");
            col06.AllowDBNull = false;
            col06.ColumnName = "fax";
            col06.Caption = "팩스번호";
            col06.DefaultValue = "";
            dt.Columns.Add(col06);

            DataColumn col07 = new DataColumn();
            col07.DataType = System.Type.GetType("System.String");
            col07.AllowDBNull = false;
            col07.ColumnName = "phone";
            col07.Caption = "휴대폰";
            col07.DefaultValue = "";
            dt.Columns.Add(col07);

            DataColumn col071 = new DataColumn();
            col071.DataType = System.Type.GetType("System.String");
            col071.AllowDBNull = false;
            col071.ColumnName = "company_manager";
            col071.Caption = "업체 담당자";
            col071.DefaultValue = "";
            dt.Columns.Add(col071);

            DataColumn col072 = new DataColumn();
            col072.DataType = System.Type.GetType("System.String");
            col072.AllowDBNull = false;
            col072.ColumnName = "company_manager_position";
            col072.Caption = "업체 담당자 직책";
            col072.DefaultValue = "";
            dt.Columns.Add(col072);

            DataColumn col21 = new DataColumn();
            col21.DataType = System.Type.GetType("System.String");
            col21.AllowDBNull = false;
            col21.ColumnName = "seaover_company_code";
            col21.Caption = "거래처코드";
            col21.DefaultValue = "";
            dt.Columns.Add(col21);

            DataColumn col211 = new DataColumn();
            col211.DataType = System.Type.GetType("System.String");
            col211.AllowDBNull = false;
            col211.ColumnName = "email";
            col211.Caption = "Email";
            col211.DefaultValue = "";
            dt.Columns.Add(col211);

            DataColumn col19 = new DataColumn();
            col19.DataType = System.Type.GetType("System.String");
            col19.AllowDBNull = false;
            col19.ColumnName = "remark";
            col19.Caption = "비고";
            col19.DefaultValue = "";
            dt.Columns.Add(col19);

            DataColumn col190 = new DataColumn();
            col190.DataType = System.Type.GetType("System.String");
            col190.AllowDBNull = false;
            col190.ColumnName = "remark2";
            col190.Caption = "비고2";
            col190.DefaultValue = "";
            dt.Columns.Add(col190);

            DataColumn col191 = new DataColumn();
            col191.DataType = System.Type.GetType("System.String");
            col191.AllowDBNull = false;
            col191.ColumnName = "sns1";
            col191.Caption = "sns1";
            col191.DefaultValue = "";
            dt.Columns.Add(col191);

            DataColumn col192 = new DataColumn();
            col192.DataType = System.Type.GetType("System.String");
            col192.AllowDBNull = false;
            col192.ColumnName = "sns2";
            col192.Caption = "sns2";
            col192.DefaultValue = "";
            dt.Columns.Add(col192);

            DataColumn col193 = new DataColumn();
            col193.DataType = System.Type.GetType("System.String");
            col193.AllowDBNull = false;
            col193.ColumnName = "sns3";
            col193.Caption = "sns3";
            col193.DefaultValue = "";
            dt.Columns.Add(col193);

            DataColumn col194 = new DataColumn();
            col194.DataType = System.Type.GetType("System.String");
            col194.AllowDBNull = false;
            col194.ColumnName = "web";
            col194.Caption = "web";
            col194.DefaultValue = "";
            dt.Columns.Add(col194);

            DataColumn col195 = new DataColumn();
            col195.DataType = System.Type.GetType("System.Boolean");
            col195.AllowDBNull = false;
            col195.ColumnName = "isManagement1";
            col195.Caption = "isManagement1";
            col195.DefaultValue = false;
            dt.Columns.Add(col195);

            DataColumn col196 = new DataColumn();
            col196.DataType = System.Type.GetType("System.Boolean");
            col196.AllowDBNull = false;
            col196.ColumnName = "isManagement2";
            col196.Caption = "isManagement2";
            col196.DefaultValue = false;
            dt.Columns.Add(col196);

            DataColumn col197 = new DataColumn();
            col197.DataType = System.Type.GetType("System.Boolean");
            col197.AllowDBNull = false;
            col197.ColumnName = "isManagement3";
            col197.Caption = "isManagement3";
            col197.DefaultValue = false;
            dt.Columns.Add(col197);

            DataColumn col198 = new DataColumn();
            col198.DataType = System.Type.GetType("System.Boolean");
            col198.AllowDBNull = false;
            col198.ColumnName = "isManagement4";
            col198.Caption = "isManagement4";
            col198.DefaultValue = false;
            dt.Columns.Add(col198);

            DataColumn col199 = new DataColumn();
            col199.DataType = System.Type.GetType("System.Boolean");
            col199.AllowDBNull = false;
            col199.ColumnName = "isHide";
            col199.Caption = "isHide";
            col199.DefaultValue = false;
            dt.Columns.Add(col199);

            DataColumn col091 = new DataColumn();
            col091.DataType = System.Type.GetType("System.String");
            col091.AllowDBNull = false;
            col091.ColumnName = "distribution";
            col091.Caption = "유통";
            col091.DefaultValue = "";
            dt.Columns.Add(col091);

            DataColumn col092 = new DataColumn();
            col092.DataType = System.Type.GetType("System.String");
            col092.AllowDBNull = false;
            col092.ColumnName = "handling_item";
            col092.Caption = "취급품목";
            col092.DefaultValue = "";
            dt.Columns.Add(col092);

            DataColumn col093 = new DataColumn();
            col093.DataType = System.Type.GetType("System.String");
            col093.AllowDBNull = false;
            col093.ColumnName = "createtime";
            col093.Caption = "생성일";
            col093.DefaultValue = "";
            dt.Columns.Add(col093);



            DataColumn col101 = new DataColumn();
            col101.DataType = System.Type.GetType("System.String");
            col101.AllowDBNull = false;
            col101.ColumnName = "div0";
            col101.Caption = "";
            col101.DefaultValue = "";
            dt.Columns.Add(col101);

            DataColumn col102 = new DataColumn();
            col102.DataType = System.Type.GetType("System.Boolean");
            col102.AllowDBNull = false;
            col102.ColumnName = "isPotential1";
            col102.Caption = "잠재1";
            col102.DefaultValue = false;
            dt.Columns.Add(col102);

            DataColumn col103 = new DataColumn();
            col103.DataType = System.Type.GetType("System.Boolean");
            col103.AllowDBNull = false;
            col103.ColumnName = "isPotential2";
            col103.Caption = "잠재2";
            col103.DefaultValue = false;
            dt.Columns.Add(col103);

            DataColumn col1031 = new DataColumn();
            col1031.DataType = System.Type.GetType("System.Boolean");
            col1031.AllowDBNull = false;
            col1031.ColumnName = "isTrading";
            col1031.Caption = "거래중";
            col1031.DefaultValue = false;
            dt.Columns.Add(col1031);

            DataColumn col10 = new DataColumn();
            col10.DataType = System.Type.GetType("System.String");
            col10.AllowDBNull = false;
            col10.ColumnName = "div1";
            col10.Caption = "";
            col10.DefaultValue = "";
            dt.Columns.Add(col10);

            DataColumn col18 = new DataColumn();
            col18.DataType = System.Type.GetType("System.String");
            col18.AllowDBNull = false;
            col18.ColumnName = "sales_updatetime";
            col18.Caption = "최근영업일자";
            col18.DefaultValue = "";
            dt.Columns.Add(col18);

            DataColumn col15 = new DataColumn();
            col15.DataType = System.Type.GetType("System.String");
            col15.AllowDBNull = false;
            col15.ColumnName = "sales_contents";
            col15.Caption = "최근영업내용";
            col15.DefaultValue = "";
            dt.Columns.Add(col15);

            DataColumn col152 = new DataColumn();
            col152.DataType = System.Type.GetType("System.String");
            col152.AllowDBNull = false;
            col152.ColumnName = "sales_remark";
            col152.Caption = "비고";
            col152.DefaultValue = "";
            dt.Columns.Add(col152);

            DataColumn col20 = new DataColumn();
            col20.DataType = System.Type.GetType("System.String");
            col20.AllowDBNull = false;
            col20.ColumnName = "sales_edit_user";
            col20.Caption = "수정자";
            col20.DefaultValue = "";
            dt.Columns.Add(col20);

            DataColumn col23 = new DataColumn();
            col23.DataType = System.Type.GetType("System.String");
            col23.AllowDBNull = false;
            col23.ColumnName = "div2";
            col23.Caption = "";
            col23.DefaultValue = "";
            dt.Columns.Add(col23);

            DataColumn col24 = new DataColumn();
            col24.DataType = System.Type.GetType("System.Boolean");
            col24.AllowDBNull = false;
            col24.ColumnName = "isNonHandled";
            col24.Caption = "취급X";
            col24.DefaultValue = false;
            dt.Columns.Add(col24);

            DataColumn col26 = new DataColumn();
            col26.DataType = System.Type.GetType("System.Boolean");
            col26.AllowDBNull = false;
            col26.ColumnName = "isNotSendFax";
            col26.Caption = "팩스X";
            col26.DefaultValue = false;
            dt.Columns.Add(col26);

            DataColumn col25 = new DataColumn();
            col25.DataType = System.Type.GetType("System.Boolean");
            col25.AllowDBNull = false;
            col25.ColumnName = "isOutBusiness";
            col25.Caption = "폐업";
            col25.DefaultValue = false;
            dt.Columns.Add(col25);

            DataColumn col28 = new DataColumn();
            col28.DataType = System.Type.GetType("System.String");
            col28.AllowDBNull = false;
            col28.ColumnName = "div3";
            col28.Caption = "";
            col28.DefaultValue = "";
            dt.Columns.Add(col28);

            DataColumn col27 = new DataColumn();
            col27.DataType = System.Type.GetType("System.String");
            col27.AllowDBNull = false;
            col27.ColumnName = "ato_manager";
            col27.Caption = "아토담당";
            col27.DefaultValue = "";
            dt.Columns.Add(col27);

            return dt;
        }

        private void SetHeaderStyle()
        {
            dgvCompany.EndEdit();
            txtTotalRecord.Text = dgvCompany.Rows.Count.ToString("#,##0");
            txtCurrentRecord.Text = "0";

            //Header style
            dgvCompany.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 11, FontStyle.Bold);
            dgvCompany.DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Regular);

            if (dgvCompany.Columns.Count > 0)
            {
                for (int i = 1; i < dgvCompany.Columns.Count; i++)
                    dgvCompany.Columns[i].HeaderText = mainDt.Columns[i - 1].Caption;

                ((DataGridViewImageColumn)dgvCompany.Columns["image"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvCompany.Columns["image"].DefaultCellStyle.NullValue = null;

                dgvCompany.Columns["chk"].Width = 30;
                dgvCompany.Columns["div0"].Width = 10;
                dgvCompany.Columns["div1"].Width = 10;
                dgvCompany.Columns["div2"].Width = 10;
                dgvCompany.Columns["div3"].Width = 10;

                dgvCompany.Columns["address"].Width = 150;
                dgvCompany.Columns["distribution"].Width = 50;
                dgvCompany.Columns["ceo"].Width = 60;

                dgvCompany.Columns["sales_edit_user"].Width = 60;
                dgvCompany.Columns["sales_updatetime"].Width = 100;
                dgvCompany.Columns["sales_contents"].Width = 200;
                dgvCompany.Columns["isPotential1"].Width = 60;
                dgvCompany.Columns["ispotential2"].Width = 60;

                dgvCompany.Columns["isNonHandled"].Width = 60;
                dgvCompany.Columns["isNotSendFax"].Width = 60;
                dgvCompany.Columns["isOutBusiness"].Width = 50;
                dgvCompany.Columns["ato_manager"].Width = 70;
                dgvCompany.Columns["company"].Width = 150;


                dgvCompany.Columns["id"].Visible = false;
                dgvCompany.Columns["origin"].Visible = false;
                dgvCompany.Columns["company_manager"].Visible = false;
                dgvCompany.Columns["company_manager_position"].Visible = false;
                dgvCompany.Columns["email"].Visible = false;
                dgvCompany.Columns["sns1"].Visible = false;
                dgvCompany.Columns["sns2"].Visible = false;
                dgvCompany.Columns["sns3"].Visible = false;
                dgvCompany.Columns["web"].Visible = false;
                dgvCompany.Columns["remark2"].Visible = false;
                dgvCompany.Columns["isManagement1"].Visible = false;
                dgvCompany.Columns["isManagement2"].Visible = false;
                dgvCompany.Columns["isManagement3"].Visible = false;
                dgvCompany.Columns["isManagement4"].Visible = false;
                dgvCompany.Columns["isHide"].Visible = false;
                dgvCompany.Columns["isTrading"].Visible = false;
                dgvCompany.Columns["createtime"].Visible = false;
                dgvCompany.Columns["seaover_company_code"].Visible = false;

                dgvCompany.Columns["isOutBusiness"].Visible = !cbNotOutOfBusiness.Checked;
            }
        }

        private bool UpdateCompanyInfo(int updateType = 1)
        {
            //=====================
            //=====================
            // 수정 타입
            // 1 : 거래처 수정
            // 2 : 공용DB 전환
            // 3 : 무작위DB 전환
            // 4 : 잠재1 전환
            // 5 : 잠재2 전환
            // 6 : 취급X 전환
            // 7 : 팩스X 전환
            // 8 : 폐업 전환
            //=====================
            //=====================

            switch (updateType)
            {
                //공용DB 전환
                case 2:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                        {
                            row.Cells["ato_manager"].Value = string.Empty;
                            row.Cells["isTrading"].Value = false;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;
                        }
                    }    
                    break;
                //무작위DB 전환
                case 3:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                        {
                            row.Cells["ato_manager"].Value = um.user_name;
                            row.Cells["isPotential1"].Value = false;
                            row.Cells["isPotential2"].Value = false;
                            row.Cells["isTrading"].Value = false;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;
                        }       
                    }
                    break;
                //잠재1 전환
                case 4:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                        {
                            row.Cells["ato_manager"].Value = um.user_name;
                            row.Cells["isPotential1"].Value = true;
                            row.Cells["isPotential2"].Value = false;
                            row.Cells["isTrading"].Value = false;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;
                        }
                    }
                    break;
                //잠재2 전환
                case 5:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                        {
                            row.Cells["ato_manager"].Value = um.user_name;
                            row.Cells["isPotential1"].Value = false;
                            row.Cells["isPotential2"].Value = true;
                            row.Cells["isTrading"].Value = false;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;
                        }
                    }
                    break;
                //취급X 전환
                case 6:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                            row.Cells["isNonHandled"].Value = true;
                    }
                    break;
                //팩스X 전환
                case 7:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                            row.Cells["isNotSendFax"].Value = true;
                    }
                    break;
                //폐업 전환
                case 8:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                            row.Cells["isOutBusiness"].Value = true;
                    }
                    break;
                //거래중 전환
                case 9:
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                        {
                            row.Cells["isPotential1"].Value = false;
                            row.Cells["isPotential2"].Value = false;
                            row.Cells["isTrading"].Value = true;

                            row.Cells["isNonHandled"].Value = false;
                            row.Cells["isNotSendFax"].Value = false;
                            row.Cells["isOutBusiness"].Value = false;

                            row.Cells["ato_manager"].Value = um.user_name;
                        }    
                    }
                    break;
            }

            //신규ID
            int new_id= commonRepository.GetNextId("t_company", "id");
            
            //거래처 수정
            dgvCompany.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];
                bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                if (isChecked)
                {

                    //기존ID가 없을경우 신규ID
                    int id;
                    if (!int.TryParse(row.Cells["id"].Value.ToString(), out id))
                        id = 0;
                    if (id == 0)
                    {
                        id = new_id;
                        new_id++;
                    }
                        
                    CompanyModel model = new CompanyModel();
                    model.id = id;
                    model.division = "매출처";
                    model.group_name = row.Cells["group_name"].Value.ToString();
                    model.name = row.Cells["company"].Value.ToString();
                    model.registration_number = row.Cells["registration_number"].Value.ToString();
                    model.ceo = row.Cells["ceo"].Value.ToString();
                    model.tel = row.Cells["tel"].Value.ToString();
                    model.fax = row.Cells["fax"].Value.ToString();
                    model.phone = row.Cells["phone"].Value.ToString();
                    model.distribution = row.Cells["distribution"].Value.ToString();
                    model.handling_item = row.Cells["handling_item"].Value.ToString();
                    model.address = row.Cells["address"].Value.ToString();
                    model.origin = row.Cells["origin"].Value.ToString();
                    model.ato_manager = row.Cells["ato_manager"].Value.ToString();

                    model.email = row.Cells["email"].Value.ToString();
                    model.web = row.Cells["web"].Value.ToString();
                    model.sns1 = row.Cells["sns1"].Value.ToString();
                    model.sns2 = row.Cells["sns2"].Value.ToString();
                    model.sns3 = row.Cells["sns3"].Value.ToString();
                    model.company_manager = row.Cells["company_manager"].Value.ToString();
                    model.company_manager_position = row.Cells["company_manager_position"].Value.ToString();
                    model.seaover_company_code= row.Cells["seaover_company_code"].Value.ToString();

                    bool isManagement1, isManagement2, isManagement3, isManagement4;
                    if (row.Cells["isManagement1"].Value == null || !bool.TryParse(row.Cells["isManagement1"].Value.ToString(), out isManagement1))
                        isManagement1 = false;
                    if (row.Cells["isManagement2"].Value == null || !bool.TryParse(row.Cells["isManagement2"].Value.ToString(), out isManagement2))
                        isManagement2 = false;
                    if (row.Cells["isManagement3"].Value == null || !bool.TryParse(row.Cells["isManagement3"].Value.ToString(), out isManagement3))
                        isManagement3 = false;
                    if (row.Cells["isManagement4"].Value == null || !bool.TryParse(row.Cells["isManagement4"].Value.ToString(), out isManagement4))
                        isManagement4 = false;
                    model.isManagement1 = isManagement1;
                    model.isManagement2 = isManagement2;
                    model.isManagement3 = isManagement3;
                    model.isManagement4 = isManagement4;

                    bool isHide;
                    if (row.Cells["isHide"].Value == null || !bool.TryParse(row.Cells["isHide"].Value.ToString(), out isHide))
                        isHide = false;
                    model.isHide = isHide;
                    model.isDelete = false;

                    DateTime createtime;
                    if (row.Cells["remark"].Value == null || !DateTime.TryParse(row.Cells["createtime"].Value.ToString(), out createtime))
                        createtime = DateTime.Now;
                    model.createtime = createtime.ToString("yyyy-MM-dd HH:mm:ss");
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    model.remark = row.Cells["remark"].Value.ToString();
                    model.remark2 = row.Cells["remark2"].Value.ToString();

                    model.isPotential1 = Convert.ToBoolean(row.Cells["isPotential1"].Value);
                    model.isPotential2 = Convert.ToBoolean(row.Cells["isPotential2"].Value);

                    model.isTrading = Convert.ToBoolean(row.Cells["isTrading"].Value);

                    model.isNonHandled = Convert.ToBoolean(row.Cells["isNonHandled"].Value);
                    model.isNotSendFax = Convert.ToBoolean(row.Cells["isNotSendFax"].Value);
                    model.isOutBusiness = Convert.ToBoolean(row.Cells["isOutBusiness"].Value);

                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-ss HH:mm:ss");

                    //거래처 삭제
                    sql = companyRepository.RealDeleteCompany(id);
                    sqlList.Add(sql);

                    //거래처 정보 재등록
                    sql = companyRepository.InsertCompany(model);
                    sqlList.Add(sql);

                    //===========================================================================================
                    //거래처 영업내용============================================================================

                    bool is_sales = false;
                    string sales_log = "";

                    switch (updateType)
                    {
                        //공용DB 전환
                        case 2:
                            {
                                is_sales  = false;
                                sales_log = "공용DATA 전환";
                            }
                            break;
                        //무작위DB 전환
                        case 3:
                            {
                                is_sales = false;
                                sales_log = "무작위DB 전환";
                            }
                            break;
                        //잠재1 전환
                        case 4:
                            {
                                is_sales = true;
                                sales_log = "잠재1 전환";
                            }
                            break;
                        //잠재2 전환
                        case 5:
                            {
                                is_sales = true;
                                sales_log = "잠재2 전환";
                            }
                            break;
                        //취급X 전환
                        case 6:
                            {
                                is_sales = true;
                                sales_log = "취급X 전환";
                            }
                            break;
                        //팩스X 전환
                        case 7:
                            {
                                is_sales = true;
                                sales_log = "팩스X 전환";
                            }
                            break;
                        //폐업 전환
                        case 8:
                            {
                                is_sales = true;
                                sales_log = "폐업 전환";
                            }
                            break;
                        //거래중 전환
                        case 9:
                            {
                                is_sales = true;
                                sales_log = "거래중(SEAOVER (F6)) 전환";
                            }
                            break;
                    }

                    CompanySalesModel sModel = new CompanySalesModel();
                    sModel.company_id = id;
                    sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", id.ToString());
                    sModel.is_sales = false;
                    sModel.contents = row.Cells["sales_contents"].Value.ToString();
                    sModel.log = sales_log;
                    sModel.remark = row.Cells["sales_remark"].Value.ToString();
                    sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sModel.edit_user = um.user_name;

                    sql = salesPartnerRepository.InsertPartnerSales(sModel);
                    sqlList.Add(sql);
                    //===========================================================================================
                    //거래처 영업내용============================================================================
                }
            }
            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    MessageBox.Show("수정중 에러가 발생하였습니다.");
                    return false;
                }
                else
                    return true;

            }
            return true;
        }

        private bool DeleteCompanyInfo()
        {
            dgvCompany.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];
                bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                if (isChecked)
                {
                    int id;
                    if (!int.TryParse(row.Cells["id"].Value.ToString(), out id))
                        id = 0;
                    //SEAOVER 거래처일 경우
                    if (id == 0 && !AddCompany(row.Cells["company"].Value.ToString(), out id))
                    {
                        MessageBox.Show("SEAOVER 거래처입니다.");
                        return false;
                    }
                    else
                        row.Cells["id"].Value = id.ToString();

                    //거래처 삭제
                    sql = companyRepository.DeleteCompany(id);
                    sqlList.Add(sql);
                }
            }
            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    MessageBox.Show("수정중 에러가 발생하였습니다.");
                    return false;
                }
                else
                    return true;

            }
            return true;
        }

        private bool AddCompany(string company, out int company_id)
        {
            /*company_id = 0;
            //Seaover 거래처 내역
            DataTable companyDt = companyRepository.GetProductPriceInfo(company);
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show("거래처 정보를 찾을 수 없습니다.");
                return false;
            }
            //Ato 전산에 등록된 거래처 내역
            double ato_capital_rate = 0;
            string ato_capital_rate_updatetime = "";
            CompanyModel company_model = companyRepository.GetCompanyAsOne2(company, companyDt.Rows[0]["거래처코드"].ToString());
            if (company_model == null)
            {
                company_model = new CompanyModel();
                company_model.id = commonRepository.GetNextId("t_company", "id");
                company_model.division = "매출처";
                company_model.name = companyDt.Rows[0]["거래처명"].ToString();
                company_model.seaover_company_code = companyDt.Rows[0]["거래처코드"].ToString();
                company_model.origin = "국내";
                company_model.fax = companyDt.Rows[0]["팩스번호"].ToString();
                company_model.tel = companyDt.Rows[0]["전화번호"].ToString();
                company_model.kakao = companyDt.Rows[0]["휴대폰"].ToString();
                company_model.ceo = companyDt.Rows[0]["대표자명"].ToString();
                company_model.remark = companyDt.Rows[0]["참고사항"].ToString();
                company_model.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                company_model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                company_model.edit_user = "";
                company_model.isManagement = false;
                company_model.isManagement2 = false;
                company_model.isManagement3 = false;
                company_model.isManagement4 = false;
                company_model.isHide = false;
                company_model.isPotential1 = false;
                company_model.isPotential2 = false;
                company_model.ato_capital_rate = ato_capital_rate;
                company_model.ato_capital_rate_updatetime = ato_capital_rate_updatetime;

                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = companyRepository.InsertCompany(company_model);
                sqlList.Add(sql);

                //Execute
                int results = commonRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show("등록중 에러가 발생하였습니다.");
                    return false;
                }
            }
            company_id = company_model.id;
            return true;*/
            company_id = 1;
            return false;
        }

        public static DataTable ConvertListToDatatable(IEnumerable<dynamic> v)
        {
            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();

            DataTable table = new DataTable();
            foreach (var info in infos)
            {
                Type propType = info.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                else
                    table.Columns.Add(info.Name, info.PropertyType);
            }

            DataRow row;

            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;
                }
                table.Rows.Add(row);
            }
            table.AcceptChanges();
            return table;
        }
        private void GetData()
        {
            this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            //초기화
            mainDt = SetTable();
            dgvCompany.DataSource = null;

            //영업, 매출제한
            string saleTelDate;
            switch (cbCurrentSaleTelDate.Text)
            {
                case "15일 이상 지난":
                    saleTelDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
                    break;
                case "한달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    break;
                case "45일 이상 지난":
                    saleTelDate = DateTime.Now.AddDays(-45).ToString("yyyy-MM-dd");
                    break;
                case "두달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");
                    break;
                case "세달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                    break;
                case "여섯달 이상 지난":
                    saleTelDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                    break;
                default:
                    saleTelDate = "";
                    break;
            }

            //데이터 출력====================================================================================
            //매출처 정보
            DataTable companyDt;
            if (tcMain.SelectedTab.Text == "영업금지 거래처 (F7)")
                companyDt = salesPartnerRepository.GetSalesPartner(txtGroupName.Text, txtCompany.Text, cbExactly.Checked, txtTel.Text.Replace("-", "").Replace(" ", "")
                                            , txtRNum.Text, txtCeo.Text, " ", true, false, false, true, false, !cbNotOutOfBusiness.Checked, false, false, saleTelDate);
            else
                companyDt = salesPartnerRepository.GetSalesPartner(txtGroupName.Text, txtCompany.Text, cbExactly.Checked, txtTel.Text.Replace("-", "").Replace(" ", "")
                                            , txtRNum.Text, txtCeo.Text, " ", true, false, false, false, false, !cbNotOutOfBusiness.Checked, false, false, saleTelDate);
            

            //씨오버 거래처 
            DataTable seaoverDt = salesRepository.GetSaleCompany(txtCompany.Text, cbExactly.Checked, txtTel.Text, txtTel.Text, txtTel.Text, "", "", cbNotOutOfBusiness.Checked, cbLitigation.Checked);

            //아토 전산에만 저장된 거래처정보
            DataRow[] atoCompanyDt = companyDt.Select("seaover_company_code = '' AND isDelete = 'FALSE'");
            DataTable noSeaoverDt = new DataTable();
            if(atoCompanyDt.Length > 0)
                noSeaoverDt = atoCompanyDt.CopyToDataTable();

            //씨오버 거래정보 + 아토전산 거래처
            var seaoverTemp = from p in seaoverDt.AsEnumerable()
                       join t in companyDt.AsEnumerable()
                       on p.Field<string>("거래처코드") equals t.Field<string>("seaover_company_code")
                       into outer
                       from t in outer.DefaultIfEmpty()
                       select new
                       {
                           id = (t == null) ? 0 : t.Field<Int32>("id"),
                           group_name = (t == null) ? "" : t.Field<string>("group_name"),
                           company = p.Field<string>("거래처명"),
                           seaover_company_code = p.Field<string>("거래처코드"),
                           tel = p.Field<string>("전화번호"),
                           fax = p.Field<string>("팩스번호"),
                           phone = p.Field<string>("휴대폰"),
                           ceo = p.Field<string>("대표자명"),
                           registration_number = p.Field<string>("사업자번호"),
                           address = (t == null) ? "" : t.Field<string>("address"),

                           origin = (t == null) ? "" : t.Field<string>("origin"),
                           company_manager = (t == null) ? "" : t.Field<string>("company_manager"),
                           company_manager_position = (t == null) ? "" : t.Field<string>("company_manager_position"),
                           email = (t == null) ? "" : t.Field<string>("email"),
                           web = (t == null) ? "" : t.Field<string>("web"),
                           sns1 = (t == null) ? "" : t.Field<string>("sns1"),
                           sns2 = (t == null) ? "" : t.Field<string>("sns2"),
                           sns3 = (t == null) ? "" : t.Field<string>("sns3"),

                           isManagement1 = (t == null) ? "false" : t.Field<string>("isManagement1"),
                           isManagement2 = (t == null) ? "false" : t.Field<string>("isManagement2"),
                           isManagement3 = (t == null) ? "false" : t.Field<string>("isManagement3"),
                           isManagement4 = (t == null) ? "false" : t.Field<string>("isManagement4"),
                           isHide = (t == null) ? "false" : t.Field<string>("isHide"),
                           createtime = (t == null) ? DateTime.Now.ToString("yyyy-MM-dd") : t.Field<string>("createtime"),

                           ato_manager = (t == null) ? p.Field<string>("매출자") : t.Field<string>("ato_manager"),
                           distribution = (t == null) ? "" : t.Field<string>("distribution"),
                           handling_item = (t == null) ? "" : t.Field<string>("handling_item"),
                           remark = p.Field<string>("참고사항"),
                           remark2 = (t == null) ? "" : t.Field<string>("remark2"),
                           isPotential1 = (t == null) ? "false" : t.Field<string>("isPotential1"),
                           isPotential2 = (t == null) ? "false" : t.Field<string>("isPotential2"),
                           isTrading = (t == null) ? "true" : t.Field<string>("isTrading"),
                           current_sale_date = p.Field<DateTime>("최종매출일자"),
                           current_sale_manager = p.Field<string>("매출자"),

                           isNonHandled = (t == null) ? "" : t.Field<string>("isNonHandled"),
                           isOutBusiness = (t == null) ? p.Field<string>("폐업유무") : t.Field<string>("isOutBusiness"),
                           isNotSendFax = (t == null) ? "" : t.Field<string>("isNotSendFax"),
                           sales_updatetime = (t == null) ? "" : t.Field<string>("sales_updatetime"),
                           sales_edit_user = (t == null) ? "" : t.Field<string>("sales_edit_user"),
                           sales_contents = (t == null) ? "" : t.Field<string>("sales_contents"),
                           sales_log = (t == null) ? "" : t.Field<string>("sales_log"),
                           sales_remark = (t == null) ? "" : t.Field<string>("sales_remark"),

                           isDelete = (t == null) ? "FALSE" : t.Field<string>("isDelete")
                       };
            DataTable seaoverDt1 = ConvertListToDatatable(seaoverTemp);


            if (tcMain.SelectedTab.Text == "공용DATA (F1)")
            {
                //초기화
                Dt0 = SetTable();
                //저장된 매출처먼저 출력
                if (noSeaoverDt.Rows.Count > 0 || seaoverDt1.Rows.Count > 0)
                {
                    for (int i = 0; i < noSeaoverDt.Rows.Count; i++)
                    {
                        bool isOutput = false;
                        //담당자 거래처
                        if (string.IsNullOrEmpty(noSeaoverDt.Rows[i]["ato_manager"].ToString().Trim()))
                            isOutput = true;
                        //잠재1, 잠재2
                        bool isPotential1, isPotential2;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential1"].ToString(), out isPotential1))
                            isPotential1 = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential2"].ToString(), out isPotential2))
                            isPotential2 = false;
                        //if (isOutput && )
                        //영업금지 거래처
                        bool isNonHandled, isNotSendFax, isOutBusiness;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                            isNonHandled = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                            isNotSendFax = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                            isOutBusiness = false;
                        if (isOutput && (isNonHandled || isNotSendFax || isOutBusiness))
                            isOutput = false;

                        //거래중 거래처 제외
                        bool isTrading;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isTrading"].ToString(), out isTrading))
                            isTrading = false;
                        if (isOutput && isTrading)
                            isOutput = false;

                        //소송거래처 제외
                        if (isOutput && cbLitigation.Checked
                            && (noSeaoverDt.Rows[i]["name"].ToString().Contains("(S)") || noSeaoverDt.Rows[i]["name"].ToString().Contains("소송")))
                            isOutput = false;

                        //출력
                        if (isOutput)
                        {
                            DataRow row = Dt0.NewRow();
                            row["id"] = noSeaoverDt.Rows[i]["id"].ToString();
                            row["group_name"] = noSeaoverDt.Rows[i]["group_name"].ToString();

                            //정보가리기
                            if (um.auth_level < 50)
                            {
                                row["company"] = SetSecurityTxt(noSeaoverDt.Rows[i]["name"].ToString());
                                row["tel"] = SetSecurityTxt(noSeaoverDt.Rows[i]["tel"].ToString(), 4);
                                row["fax"] = SetSecurityTxt(noSeaoverDt.Rows[i]["fax"].ToString(), 4);
                                row["phone"] = SetSecurityTxt(noSeaoverDt.Rows[i]["phone"].ToString(), 4);
                                row["registration_number"] = SetSecurityTxt(noSeaoverDt.Rows[i]["registration_number"].ToString(), 4);
                            }
                            else
                            {
                                row["company"] = noSeaoverDt.Rows[i]["name"].ToString();
                                row["tel"] = noSeaoverDt.Rows[i]["tel"].ToString();
                                row["fax"] = noSeaoverDt.Rows[i]["fax"].ToString();
                                row["phone"] = noSeaoverDt.Rows[i]["phone"].ToString();
                                row["registration_number"] = noSeaoverDt.Rows[i]["registration_number"].ToString();
                            }


                            row["ceo"] = noSeaoverDt.Rows[i]["ceo"].ToString();
                            row["address"] = noSeaoverDt.Rows[i]["address"].ToString();
                            row["remark"] = noSeaoverDt.Rows[i]["remark"].ToString();
                            row["distribution"] = noSeaoverDt.Rows[i]["distribution"].ToString();
                            row["handling_item"] = noSeaoverDt.Rows[i]["handling_item"].ToString();

                            row["origin"] = noSeaoverDt.Rows[i]["origin"].ToString();
                            row["remark2"] = noSeaoverDt.Rows[i]["remark2"].ToString();
                            row["company_manager"] = noSeaoverDt.Rows[i]["company_manager"].ToString();
                            row["company_manager_position"] = noSeaoverDt.Rows[i]["company_manager_position"].ToString();
                            row["email"] = noSeaoverDt.Rows[i]["email"].ToString();
                            row["web"] = noSeaoverDt.Rows[i]["web"].ToString();
                            row["sns1"] = noSeaoverDt.Rows[i]["sns1"].ToString();
                            row["sns2"] = noSeaoverDt.Rows[i]["sns2"].ToString();
                            row["sns3"] = noSeaoverDt.Rows[i]["sns3"].ToString();
                            bool isManagement1, isManagement2, isManagement3, isManagement4;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                isManagement1 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                isManagement2 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                isManagement3 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                isManagement4 = false;
                            row["isManagement1"] = isManagement1;
                            row["isManagement2"] = isManagement2;
                            row["isManagement3"] = isManagement3;
                            row["isManagement4"] = isManagement4;
                            bool isHide;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isHide"].ToString(), out isHide))
                                isHide = false;
                            row["isHide"] = isHide;
                            row["createtime"] = noSeaoverDt.Rows[i]["createtime"].ToString();

                            row["ato_manager"] = noSeaoverDt.Rows[i]["ato_manager"].ToString();
                            //잠재1, 잠재2
                            row["isPotential1"] = isPotential1;
                            row["isPotential2"] = isPotential2;
                            //영업금지 거래처
                            row["isNonHandled"] = isNonHandled;
                            row["isNotSendFax"] = isNotSendFax;
                            row["isOutBusiness"] = isOutBusiness;

                            //최근 영업정보
                            DateTime updatetime;
                            if (DateTime.TryParse(noSeaoverDt.Rows[i]["sales_updatetime"].ToString(), out updatetime))
                                row["sales_updatetime"] = updatetime.ToString("yyyy-MM-dd");
                            else
                                row["sales_updatetime"] = "";
                            //최근 영업내용                            
                            row["sales_contents"] = noSeaoverDt.Rows[i]["sales_contents"].ToString();
                            row["sales_remark"] = noSeaoverDt.Rows[i]["sales_remark"].ToString();
                            row["sales_edit_user"] = noSeaoverDt.Rows[i]["sales_edit_user"].ToString();
                            Dt0.Rows.Add(row);
                        }
                    }

                    //씨오버 등록거래처 출력
                    if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < seaoverDt1.Rows.Count; i++)
                        {
                            //삭제거래처
                            bool isDelete;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isDelete"].ToString(), out isDelete))
                                isDelete = false;
                            if (!isDelete)
                            {
                                // ** 매출 || 영업중 먼저인 내역이 우선 **

                                //최근 매출정보
                                DateTime current_sale_date;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                                    current_sale_date = new DateTime(1900, 1, 1);

                                //최근 영업정보
                                DateTime sales_updatetime;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                                    sales_updatetime = new DateTime(1900, 1, 1);

                                string sale_edit_user;
                                string sale_date;
                                string sale_contents;
                                string sale_remark;
                                string ato_manager;
                                if (current_sale_date >= sales_updatetime)
                                {
                                    seaoverDt1.Rows[i]["isTrading"] = "true";
                                    seaoverDt1.AcceptChanges();
                                    ato_manager = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_contents = "씨오버 매출";
                                    sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = "";
                                }
                                else
                                {
                                    ato_manager = seaoverDt1.Rows[i]["ato_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["sales_edit_user"].ToString();
                                    sale_contents = seaoverDt1.Rows[i]["sales_contents"].ToString();
                                    if (sale_contents == "씨오버 매출")
                                        sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();
                                    sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = seaoverDt1.Rows[i]["sales_remark"].ToString();
                                }


                                //출력여부============================================================================
                                bool isOutput = false;
                                if (string.IsNullOrEmpty(ato_manager))
                                    isOutput = true;
                                //소송거래처 제외
                                if (isOutput && cbLitigation.Checked
                                    && (seaoverDt1.Rows[i]["company"].ToString().Contains("(S)") || seaoverDt1.Rows[i]["company"].ToString().Contains("소송")))
                                    isOutput = false;

                                //잠재1, 잠재2
                                bool isPotential1, isPotential2;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential1"].ToString(), out isPotential1))
                                    isPotential1 = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential2"].ToString(), out isPotential2))
                                    isPotential2 = false;
                                //if (isOutput && )
                                //영업금지 거래처
                                bool isNonHandled, isNotSendFax, isOutBusiness;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                                    isNonHandled = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                                    isNotSendFax = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                                    isOutBusiness = false;
                                if (isOutput && (isNonHandled || isNonHandled || isOutBusiness))
                                    isOutput = false;

                                //거래중 거래처 제외
                                bool isTrading;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isTrading"].ToString(), out isTrading))
                                    isTrading = false;
                                if (isOutput && isTrading)
                                    isOutput = false;

                                //출력
                                if (isOutput)
                                {
                                    DataRow row = Dt0.NewRow();
                                    row["id"] = seaoverDt1.Rows[i]["id"].ToString();
                                    row["group_name"] = seaoverDt1.Rows[i]["group_name"].ToString();
                                    row["company"] = seaoverDt1.Rows[i]["company"].ToString();
                                    row["seaover_company_code"] = seaoverDt1.Rows[i]["seaover_company_code"].ToString();
                                    row["tel"] = seaoverDt1.Rows[i]["tel"].ToString();
                                    row["fax"] = seaoverDt1.Rows[i]["fax"].ToString();
                                    row["phone"] = seaoverDt1.Rows[i]["phone"].ToString();
                                    row["registration_number"] = seaoverDt1.Rows[i]["registration_number"].ToString();
                                    row["ceo"] = seaoverDt1.Rows[i]["ceo"].ToString();
                                    row["address"] = seaoverDt1.Rows[i]["address"].ToString();
                                    row["remark"] = seaoverDt1.Rows[i]["remark"].ToString();
                                    row["distribution"] = seaoverDt1.Rows[i]["distribution"].ToString();
                                    row["handling_item"] = seaoverDt1.Rows[i]["handling_item"].ToString();

                                    row["origin"] = seaoverDt1.Rows[i]["origin"].ToString();
                                    row["remark2"] = seaoverDt1.Rows[i]["remark2"].ToString();
                                    row["company_manager"] = seaoverDt1.Rows[i]["company_manager"].ToString();
                                    row["company_manager_position"] = seaoverDt1.Rows[i]["company_manager_position"].ToString();
                                    row["email"] = seaoverDt1.Rows[i]["email"].ToString();
                                    row["web"] = seaoverDt1.Rows[i]["web"].ToString();
                                    row["sns1"] = seaoverDt1.Rows[i]["sns1"].ToString();
                                    row["sns2"] = seaoverDt1.Rows[i]["sns2"].ToString();
                                    row["sns3"] = seaoverDt1.Rows[i]["sns3"].ToString();
                                    bool isManagement1, isManagement2, isManagement3, isManagement4;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                        isManagement1 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                        isManagement2 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                        isManagement3 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                        isManagement4 = false;
                                    row["isManagement1"] = isManagement1;
                                    row["isManagement2"] = isManagement2;
                                    row["isManagement3"] = isManagement3;
                                    row["isManagement4"] = isManagement4;
                                    bool isHide;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isHide"].ToString(), out isHide))
                                        isHide = false;
                                    row["isHide"] = isHide;
                                    row["createtime"] = seaoverDt1.Rows[i]["createtime"].ToString();

                                    //잠재1, 잠재2
                                    row["isPotential1"] = isPotential1;
                                    row["isPotential2"] = isPotential2;
                                    //영업금지 거래처
                                    row["isNonHandled"] = isNonHandled;
                                    row["isNotSendFax"] = isNotSendFax;
                                    row["isOutBusiness"] = isOutBusiness;
                                    //최금영업내역
                                    row["sales_updatetime"] = sale_date;
                                    row["sales_contents"] = sale_contents;
                                    row["sales_remark"] = sale_remark;
                                    row["sales_edit_user"] = sale_edit_user;
                                    row["ato_manager"] = ato_manager;

                                    Dt0.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
                mainDt = Dt0;
            }
            else if (tcMain.SelectedTab.Text == "무작위DATA (F2)")
            {
                //초기화
                Dt1 = SetTable();
                //저장된 매출처먼저 출력
                if (noSeaoverDt.Rows.Count > 0 || seaoverDt1.Rows.Count > 0)
                {
                    for (int i = 0; i < noSeaoverDt.Rows.Count; i++)
                    {
                        bool isOutput = false;
                        //담당자 거래처
                        if (!string.IsNullOrEmpty(noSeaoverDt.Rows[i]["ato_manager"].ToString().Trim()))
                        {
                            if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                            {
                                string[] managers = txtManager.Text.Split(' ');
                                for (int j = 0; j < managers.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(managers[j]) && noSeaoverDt.Rows[i]["ato_manager"].ToString().Contains(managers[j]))
                                    {
                                        isOutput = true;
                                        break;
                                    }
                                }
                            }
                            else
                                isOutput = true;
                        }
                        //잠재1, 잠재2
                        bool isPotential1, isPotential2;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential1"].ToString(), out isPotential1))
                            isPotential1 = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential2"].ToString(), out isPotential2))
                            isPotential2 = false;
                        if (isOutput && (isPotential1 || isPotential2))
                            isOutput = false;
                        //영업금지 거래처
                        bool isNonHandled, isNotSendFax, isOutBusiness;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                            isNonHandled = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                            isNotSendFax = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                            isOutBusiness = false;
                        if (isOutput && (isNonHandled || isNotSendFax || isOutBusiness))
                            isOutput = false;

                        //거래중 거래처 제외
                        bool isTrading;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isTrading"].ToString(), out isTrading))
                            isTrading = false;
                        if (isOutput && isTrading)
                            isOutput = false;

                        //소송거래처 제외
                        if (isOutput && cbLitigation.Checked
                            && (noSeaoverDt.Rows[i]["name"].ToString().Contains("(S)") || noSeaoverDt.Rows[i]["name"].ToString().Contains("소송")))
                            isOutput = false;

                        //출력
                        if (isOutput)
                        {
                            DataRow row = Dt1.NewRow();
                            row["id"] = noSeaoverDt.Rows[i]["id"].ToString();
                            row["group_name"] = noSeaoverDt.Rows[i]["group_name"].ToString();
                            row["company"] = noSeaoverDt.Rows[i]["name"].ToString();
                            row["tel"] = noSeaoverDt.Rows[i]["tel"].ToString();
                            row["fax"] = noSeaoverDt.Rows[i]["fax"].ToString();
                            row["phone"] = noSeaoverDt.Rows[i]["phone"].ToString();
                            row["registration_number"] = noSeaoverDt.Rows[i]["registration_number"].ToString();
                            row["ceo"] = noSeaoverDt.Rows[i]["ceo"].ToString();
                            row["address"] = noSeaoverDt.Rows[i]["address"].ToString();
                            row["remark"] = noSeaoverDt.Rows[i]["remark"].ToString();
                            row["distribution"] = noSeaoverDt.Rows[i]["distribution"].ToString();
                            row["handling_item"] = noSeaoverDt.Rows[i]["handling_item"].ToString();
                            row["ato_manager"] = noSeaoverDt.Rows[i]["ato_manager"].ToString();

                            row["origin"] = noSeaoverDt.Rows[i]["origin"].ToString();
                            row["remark2"] = noSeaoverDt.Rows[i]["remark2"].ToString();
                            row["company_manager"] = noSeaoverDt.Rows[i]["company_manager"].ToString();
                            row["company_manager_position"] = noSeaoverDt.Rows[i]["company_manager_position"].ToString();
                            row["email"] = noSeaoverDt.Rows[i]["email"].ToString();
                            row["web"] = noSeaoverDt.Rows[i]["web"].ToString();
                            row["sns1"] = noSeaoverDt.Rows[i]["sns1"].ToString();
                            row["sns2"] = noSeaoverDt.Rows[i]["sns2"].ToString();
                            row["sns3"] = noSeaoverDt.Rows[i]["sns3"].ToString();
                            bool isManagement1, isManagement2, isManagement3, isManagement4;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                isManagement1 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                isManagement2 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                isManagement3 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                isManagement4 = false;
                            row["isManagement1"] = isManagement1;
                            row["isManagement2"] = isManagement2;
                            row["isManagement3"] = isManagement3;
                            row["isManagement4"] = isManagement4;
                            bool isHide;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isHide"].ToString(), out isHide))
                                isHide = false;
                            row["isHide"] = isHide;
                            row["createtime"] = noSeaoverDt.Rows[i]["createtime"].ToString();

                            //잠재1, 잠재2
                            row["isPotential1"] = isPotential1;
                            row["isPotential2"] = isPotential2;
                            //영업금지 거래처
                            row["isNonHandled"] = isNonHandled;
                            row["isNotSendFax"] = isNotSendFax;
                            row["isOutBusiness"] = isOutBusiness;

                            //최근 영업정보
                            DateTime updatetime;
                            if (DateTime.TryParse(noSeaoverDt.Rows[i]["sales_updatetime"].ToString(), out updatetime))
                                row["sales_updatetime"] = updatetime.ToString("yyyy-MM-dd");
                            else
                                row["sales_updatetime"] = "";
                            //최근 영업내용                            
                            row["sales_contents"] = noSeaoverDt.Rows[i]["sales_contents"].ToString();
                            row["sales_remark"] = noSeaoverDt.Rows[i]["sales_remark"].ToString();
                            row["sales_edit_user"] = noSeaoverDt.Rows[i]["sales_edit_user"].ToString();
                            Dt1.Rows.Add(row);
                        }
                    }

                    //씨오버 등록거래처 출력
                    if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < seaoverDt1.Rows.Count; i++)
                        {
                            //삭제거래처
                            bool isDelete;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isDelete"].ToString(), out isDelete))
                                isDelete = false;
                            if (!isDelete)
                            {
                                // ** 매출 || 영업중 먼저인 내역이 우선 **

                                //최근 매출정보
                                DateTime current_sale_date;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                                    current_sale_date = new DateTime(1900, 1, 1);

                                //최근 영업정보
                                DateTime sales_updatetime;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                                    sales_updatetime = new DateTime(1900, 1, 1);

                                string sale_edit_user;
                                string sale_date;
                                string sale_contents;
                                string sale_remark;
                                string ato_manager;


                                //출력여부============================================================================
                                //무작위Data에서는 실매출인 씨오버 거래처는 제외, 그위에 다른 작업이 덮어진 거래처만 출력 (거래중(SEAOVER (F6)) 탭이 따로있기 때문)
                                bool isOutput = false;
                                if (current_sale_date >= sales_updatetime)
                                {
                                    seaoverDt1.Rows[i]["isTrading"] = "true";
                                    seaoverDt1.AcceptChanges();
                                    ato_manager = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_contents = "씨오버 매출";
                                    sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = "";
                                }
                                else
                                {
                                    isOutput = true;
                                    ato_manager = seaoverDt1.Rows[i]["ato_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["sales_edit_user"].ToString();
                                    sale_contents = seaoverDt1.Rows[i]["sales_contents"].ToString();
                                    if (sale_contents == "씨오버 매출")
                                        sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();
                                    sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = seaoverDt1.Rows[i]["sales_remark"].ToString();
                                }

                                //담당자 검색
                                if (isOutput)
                                {
                                    if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                    {
                                        isOutput = false;
                                        string[] managers = txtManager.Text.Split(' ');
                                        for (int j = 0; j < managers.Length; j++)
                                        {
                                            if (!string.IsNullOrEmpty(managers[j]) && ato_manager.Contains(managers[j]))
                                            {
                                                isOutput = true;
                                                break;
                                            }
                                        }
                                    }
                                }


                                //소송거래처 제외
                                if (isOutput && cbLitigation.Checked
                                    && (seaoverDt1.Rows[i]["company"].ToString().Contains("(S)") || seaoverDt1.Rows[i]["company"].ToString().Contains("소송")))
                                    isOutput = false;

                                //잠재1, 잠재2
                                bool isPotential1, isPotential2;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential1"].ToString(), out isPotential1))
                                    isPotential1 = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential2"].ToString(), out isPotential2))
                                    isPotential2 = false;
                                if (isOutput && (isPotential1 || isPotential2))
                                    isOutput = false;
                                //영업금지 거래처
                                bool isNonHandled, isNotSendFax, isOutBusiness;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                                    isNonHandled = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                                    isNotSendFax = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                                    isOutBusiness = false;
                                if (isOutput && (isNonHandled || isNonHandled || isOutBusiness))
                                    isOutput = false;

                                //거래중 거래처 제외
                                bool isTrading;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isTrading"].ToString(), out isTrading))
                                    isTrading = false;
                                if (isOutput && isTrading)
                                    isOutput = false;

                                //출력
                                if (isOutput)
                                {
                                    DataRow row = Dt1.NewRow();
                                    row["id"] = seaoverDt1.Rows[i]["id"].ToString();
                                    row["group_name"] = seaoverDt1.Rows[i]["group_name"].ToString();
                                    row["company"] = seaoverDt1.Rows[i]["company"].ToString();
                                    row["seaover_company_code"] = seaoverDt1.Rows[i]["seaover_company_code"].ToString();
                                    row["tel"] = seaoverDt1.Rows[i]["tel"].ToString();
                                    row["fax"] = seaoverDt1.Rows[i]["fax"].ToString();
                                    row["phone"] = seaoverDt1.Rows[i]["phone"].ToString();
                                    row["registration_number"] = seaoverDt1.Rows[i]["registration_number"].ToString();
                                    row["ceo"] = seaoverDt1.Rows[i]["ceo"].ToString();
                                    row["address"] = seaoverDt1.Rows[i]["address"].ToString();
                                    row["remark"] = seaoverDt1.Rows[i]["remark"].ToString();
                                    row["distribution"] = seaoverDt1.Rows[i]["distribution"].ToString();
                                    row["handling_item"] = seaoverDt1.Rows[i]["handling_item"].ToString();

                                    row["origin"] = seaoverDt1.Rows[i]["origin"].ToString();
                                    row["remark2"] = seaoverDt1.Rows[i]["remark2"].ToString();
                                    row["company_manager"] = seaoverDt1.Rows[i]["company_manager"].ToString();
                                    row["company_manager_position"] = seaoverDt1.Rows[i]["company_manager_position"].ToString();
                                    row["email"] = seaoverDt1.Rows[i]["email"].ToString();
                                    row["web"] = seaoverDt1.Rows[i]["web"].ToString();
                                    row["sns1"] = seaoverDt1.Rows[i]["sns1"].ToString();
                                    row["sns2"] = seaoverDt1.Rows[i]["sns2"].ToString();
                                    row["sns3"] = seaoverDt1.Rows[i]["sns3"].ToString();
                                    bool isManagement1, isManagement2, isManagement3, isManagement4;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                        isManagement1 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                        isManagement2 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                        isManagement3 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                        isManagement4 = false;
                                    row["isManagement1"] = isManagement1;
                                    row["isManagement2"] = isManagement2;
                                    row["isManagement3"] = isManagement3;
                                    row["isManagement4"] = isManagement4;
                                    bool isHide;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isHide"].ToString(), out isHide))
                                        isHide = false;
                                    row["isHide"] = isHide;
                                    row["createtime"] = seaoverDt1.Rows[i]["createtime"].ToString();

                                    //잠재1, 잠재2
                                    row["isPotential1"] = isPotential1;
                                    row["isPotential2"] = isPotential2;
                                    //영업금지 거래처
                                    row["isNonHandled"] = isNonHandled;
                                    row["isNotSendFax"] = isNotSendFax;
                                    row["isOutBusiness"] = isOutBusiness;
                                    //최금영업내역
                                    row["sales_updatetime"] = sale_date;
                                    row["sales_contents"] = sale_contents;
                                    row["sales_remark"] = sale_remark;
                                    row["sales_edit_user"] = sale_edit_user;
                                    row["ato_manager"] = ato_manager;

                                    Dt1.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
                mainDt = Dt1;
            }
            else if (tcMain.SelectedTab.Text == "잠재1 (F3)")
            {
                //초기화
                Dt2 = SetTable();
                //저장된 매출처먼저 출력
                if (noSeaoverDt.Rows.Count > 0 || seaoverDt1.Rows.Count > 0)
                {
                    for (int i = 0; i < noSeaoverDt.Rows.Count; i++)
                    {
                        bool isOutput = false;
                        //담당자 거래처
                        if (!string.IsNullOrEmpty(noSeaoverDt.Rows[i]["ato_manager"].ToString().Trim()))
                        {
                            if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                            {
                                string[] managers = txtManager.Text.Split(' ');
                                for (int j = 0; j < managers.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(managers[j]) && noSeaoverDt.Rows[i]["ato_manager"].ToString().Contains(managers[j]))
                                    {
                                        isOutput = true;
                                        break;
                                    }
                                }
                            }
                            else
                                isOutput = true;
                        }
                        //잠재1, 잠재2
                        bool isPotential1, isPotential2;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential1"].ToString(), out isPotential1))
                            isPotential1 = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential2"].ToString(), out isPotential2))
                            isPotential2 = false;
                        if (isOutput && !isPotential1)
                            isOutput = false;
                        //영업금지 거래처
                        bool isNonHandled, isNotSendFax, isOutBusiness;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                            isNonHandled = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                            isNotSendFax = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                            isOutBusiness = false;
                        if (isOutput && (isNonHandled || isNotSendFax || isOutBusiness))
                            isOutput = false;

                        //거래중 거래처 제외
                        bool isTrading;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isTrading"].ToString(), out isTrading))
                            isTrading = false;
                        if (isOutput && isTrading)
                            isOutput = false;

                        //소송거래처 제외
                        if (isOutput && cbLitigation.Checked
                            && (noSeaoverDt.Rows[i]["name"].ToString().Contains("(S)") || noSeaoverDt.Rows[i]["name"].ToString().Contains("소송")))
                            isOutput = false;

                        //출력
                        if (isOutput)
                        {
                            DataRow row = Dt2.NewRow();
                            row["id"] = noSeaoverDt.Rows[i]["id"].ToString();
                            row["group_name"] = noSeaoverDt.Rows[i]["group_name"].ToString();
                            row["company"] = noSeaoverDt.Rows[i]["name"].ToString();
                            row["tel"] = noSeaoverDt.Rows[i]["tel"].ToString();
                            row["fax"] = noSeaoverDt.Rows[i]["fax"].ToString();
                            row["phone"] = noSeaoverDt.Rows[i]["phone"].ToString();
                            row["registration_number"] = noSeaoverDt.Rows[i]["registration_number"].ToString();
                            row["ceo"] = noSeaoverDt.Rows[i]["ceo"].ToString();
                            row["address"] = noSeaoverDt.Rows[i]["address"].ToString();
                            row["remark"] = noSeaoverDt.Rows[i]["remark"].ToString();
                            row["distribution"] = noSeaoverDt.Rows[i]["distribution"].ToString();
                            row["handling_item"] = noSeaoverDt.Rows[i]["handling_item"].ToString();
                            row["ato_manager"] = noSeaoverDt.Rows[i]["ato_manager"].ToString();

                            row["origin"] = noSeaoverDt.Rows[i]["origin"].ToString();
                            row["remark2"] = noSeaoverDt.Rows[i]["remark2"].ToString();
                            row["company_manager"] = noSeaoverDt.Rows[i]["company_manager"].ToString();
                            row["company_manager_position"] = noSeaoverDt.Rows[i]["company_manager_position"].ToString();
                            row["email"] = noSeaoverDt.Rows[i]["email"].ToString();
                            row["web"] = noSeaoverDt.Rows[i]["web"].ToString();
                            row["sns1"] = noSeaoverDt.Rows[i]["sns1"].ToString();
                            row["sns2"] = noSeaoverDt.Rows[i]["sns2"].ToString();
                            row["sns3"] = noSeaoverDt.Rows[i]["sns3"].ToString();
                            bool isManagement1, isManagement2, isManagement3, isManagement4;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                isManagement1 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                isManagement2 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                isManagement3 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                isManagement4 = false;
                            row["isManagement1"] = isManagement1;
                            row["isManagement2"] = isManagement2;
                            row["isManagement3"] = isManagement3;
                            row["isManagement4"] = isManagement4;
                            bool isHide;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isHide"].ToString(), out isHide))
                                isHide = false;
                            row["isHide"] = isHide;
                            row["createtime"] = noSeaoverDt.Rows[i]["createtime"].ToString();

                            //잠재1, 잠재2
                            row["isPotential1"] = isPotential1;
                            row["isPotential2"] = isPotential2;
                            //영업금지 거래처
                            row["isNonHandled"] = isNonHandled;
                            row["isNotSendFax"] = isNotSendFax;
                            row["isOutBusiness"] = isOutBusiness;

                            //최근 영업정보
                            DateTime updatetime;
                            if (DateTime.TryParse(noSeaoverDt.Rows[i]["sales_updatetime"].ToString(), out updatetime))
                                row["sales_updatetime"] = updatetime.ToString("yyyy-MM-dd");
                            else
                                row["sales_updatetime"] = "";
                            //최근 영업내용                            
                            row["sales_contents"] = noSeaoverDt.Rows[i]["sales_contents"].ToString();
                            row["sales_remark"] = noSeaoverDt.Rows[i]["sales_remark"].ToString();
                            row["sales_edit_user"] = noSeaoverDt.Rows[i]["sales_edit_user"].ToString();
                            Dt2.Rows.Add(row);
                        }
                    }

                    //씨오버 등록거래처 출력
                    if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < seaoverDt1.Rows.Count; i++)
                        {
                            //삭제거래처
                            bool isDelete;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isDelete"].ToString(), out isDelete))
                                isDelete = false;
                            if (!isDelete)
                            {
                                // ** 매출 || 영업중 먼저인 내역이 우선 **

                                //최근 매출정보
                                DateTime current_sale_date;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                                    current_sale_date = new DateTime(1900, 1, 1);

                                //최근 영업정보
                                DateTime sales_updatetime;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                                    sales_updatetime = new DateTime(1900, 1, 1);

                                string sale_edit_user;
                                string sale_date;
                                string sale_contents;
                                string sale_remark;
                                string ato_manager;
                                if (current_sale_date >= sales_updatetime)
                                {
                                    seaoverDt1.Rows[i]["isTrading"] = "true";
                                    seaoverDt1.AcceptChanges();
                                    ato_manager = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_contents = "씨오버 매출";
                                    sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = "";
                                }
                                else
                                {
                                    ato_manager = seaoverDt1.Rows[i]["ato_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["sales_edit_user"].ToString();
                                    sale_contents = seaoverDt1.Rows[i]["sales_contents"].ToString();
                                    if (sale_contents == "씨오버 매출")
                                        sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();
                                    sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = seaoverDt1.Rows[i]["sales_remark"].ToString();
                                }


                                //출력여부============================================================================
                                bool isOutput = false;
                                if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                {
                                    string[] managers = txtManager.Text.Split(' ');
                                    for (int j = 0; j < managers.Length; j++)
                                    {
                                        if (!string.IsNullOrEmpty(managers[j]) && ato_manager.Contains(managers[j]))
                                        {
                                            isOutput = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                    isOutput = true;
                                //소송거래처 제외
                                if (isOutput && cbLitigation.Checked
                                    && (seaoverDt1.Rows[i]["company"].ToString().Contains("(S)") || seaoverDt1.Rows[i]["company"].ToString().Contains("소송")))
                                    isOutput = false;

                                //잠재1, 잠재2
                                bool isPotential1, isPotential2;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential1"].ToString(), out isPotential1))
                                    isPotential1 = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential2"].ToString(), out isPotential2))
                                    isPotential2 = false;
                                if (isOutput && !isPotential1)
                                    isOutput = false;
                                //영업금지 거래처
                                bool isNonHandled, isNotSendFax, isOutBusiness;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                                    isNonHandled = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                                    isNotSendFax = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                                    isOutBusiness = false;
                                if (isOutput && (isNonHandled || isNonHandled || isOutBusiness))
                                    isOutput = false;

                                //거래중 거래처 제외
                                bool isTrading;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isTrading"].ToString(), out isTrading))
                                    isTrading = false;
                                if (isOutput && isTrading)
                                    isOutput = false;

                                //출력
                                if (isOutput)
                                {
                                    DataRow row = Dt2.NewRow();
                                    row["id"] = seaoverDt1.Rows[i]["id"].ToString();
                                    row["group_name"] = seaoverDt1.Rows[i]["group_name"].ToString();
                                    row["company"] = seaoverDt1.Rows[i]["company"].ToString();
                                    row["seaover_company_code"] = seaoverDt1.Rows[i]["seaover_company_code"].ToString();
                                    row["tel"] = seaoverDt1.Rows[i]["tel"].ToString();
                                    row["fax"] = seaoverDt1.Rows[i]["fax"].ToString();
                                    row["phone"] = seaoverDt1.Rows[i]["phone"].ToString();
                                    row["registration_number"] = seaoverDt1.Rows[i]["registration_number"].ToString();
                                    row["ceo"] = seaoverDt1.Rows[i]["ceo"].ToString();
                                    row["address"] = seaoverDt1.Rows[i]["address"].ToString();
                                    row["remark"] = seaoverDt1.Rows[i]["remark"].ToString();
                                    row["distribution"] = seaoverDt1.Rows[i]["distribution"].ToString();
                                    row["handling_item"] = seaoverDt1.Rows[i]["handling_item"].ToString();

                                    row["origin"] = seaoverDt1.Rows[i]["origin"].ToString();
                                    row["remark2"] = seaoverDt1.Rows[i]["remark2"].ToString();
                                    row["company_manager"] = seaoverDt1.Rows[i]["company_manager"].ToString();
                                    row["company_manager_position"] = seaoverDt1.Rows[i]["company_manager_position"].ToString();
                                    row["email"] = seaoverDt1.Rows[i]["email"].ToString();
                                    row["web"] = seaoverDt1.Rows[i]["web"].ToString();
                                    row["sns1"] = seaoverDt1.Rows[i]["sns1"].ToString();
                                    row["sns2"] = seaoverDt1.Rows[i]["sns2"].ToString();
                                    row["sns3"] = seaoverDt1.Rows[i]["sns3"].ToString();
                                    bool isManagement1, isManagement2, isManagement3, isManagement4;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                        isManagement1 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                        isManagement2 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                        isManagement3 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                        isManagement4 = false;
                                    row["isManagement1"] = isManagement1;
                                    row["isManagement2"] = isManagement2;
                                    row["isManagement3"] = isManagement3;
                                    row["isManagement4"] = isManagement4;
                                    bool isHide;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isHide"].ToString(), out isHide))
                                        isHide = false;
                                    row["isHide"] = isHide;
                                    row["createtime"] = seaoverDt1.Rows[i]["createtime"].ToString();

                                    //잠재1, 잠재2
                                    row["isPotential1"] = isPotential1;
                                    row["isPotential2"] = isPotential2;
                                    //영업금지 거래처
                                    row["isNonHandled"] = isNonHandled;
                                    row["isNotSendFax"] = isNotSendFax;
                                    row["isOutBusiness"] = isOutBusiness;
                                    //최금영업내역
                                    row["sales_updatetime"] = sale_date;
                                    row["sales_contents"] = sale_contents;
                                    row["sales_remark"] = sale_remark;
                                    row["sales_edit_user"] = sale_edit_user;
                                    row["ato_manager"] = ato_manager;

                                    Dt2.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
                mainDt = Dt2;
            }
            else if (tcMain.SelectedTab.Text == "잠재2 (F4)")
            {
                //초기화
                Dt3 = SetTable();
                //저장된 매출처먼저 출력
                if (noSeaoverDt.Rows.Count > 0 || seaoverDt1.Rows.Count > 0)
                {
                    for (int i = 0; i < noSeaoverDt.Rows.Count; i++)
                    {
                        bool isOutput = false;
                        //담당자 거래처
                        if (!string.IsNullOrEmpty(noSeaoverDt.Rows[i]["ato_manager"].ToString().Trim()))
                        {
                            if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                            {
                                string[] managers = txtManager.Text.Split(' ');
                                for (int j = 0; j < managers.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(managers[j]) && noSeaoverDt.Rows[i]["ato_manager"].ToString().Contains(managers[j]))
                                    {
                                        isOutput = true;
                                        break;
                                    }
                                }
                            }
                            else
                                isOutput = true;
                        }
                        //잠재1, 잠재2
                        bool isPotential1, isPotential2;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential1"].ToString(), out isPotential1))
                            isPotential1 = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential2"].ToString(), out isPotential2))
                            isPotential2 = false;
                        if (isOutput && !isPotential2)
                            isOutput = false;
                        //영업금지 거래처
                        bool isNonHandled, isNotSendFax, isOutBusiness;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                            isNonHandled = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                            isNotSendFax = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                            isOutBusiness = false;
                        if (isOutput && (isNonHandled || isNotSendFax || isOutBusiness))
                            isOutput = false;

                        //거래중 거래처 제외
                        bool isTrading;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isTrading"].ToString(), out isTrading))
                            isTrading = false;
                        if (isOutput && isTrading)
                            isOutput = false;

                        //소송거래처 제외
                        if (isOutput && cbLitigation.Checked
                            && (noSeaoverDt.Rows[i]["name"].ToString().Contains("(S)") || noSeaoverDt.Rows[i]["name"].ToString().Contains("소송")))
                            isOutput = false;

                        //출력
                        if (isOutput)
                        {
                            DataRow row = Dt3.NewRow();
                            row["id"] = noSeaoverDt.Rows[i]["id"].ToString();
                            row["group_name"] = noSeaoverDt.Rows[i]["group_name"].ToString();
                            row["company"] = noSeaoverDt.Rows[i]["name"].ToString();
                            row["tel"] = noSeaoverDt.Rows[i]["tel"].ToString();
                            row["fax"] = noSeaoverDt.Rows[i]["fax"].ToString();
                            row["phone"] = noSeaoverDt.Rows[i]["phone"].ToString();
                            row["registration_number"] = noSeaoverDt.Rows[i]["registration_number"].ToString();
                            row["ceo"] = noSeaoverDt.Rows[i]["ceo"].ToString();
                            row["address"] = noSeaoverDt.Rows[i]["address"].ToString();
                            row["remark"] = noSeaoverDt.Rows[i]["remark"].ToString();
                            row["distribution"] = noSeaoverDt.Rows[i]["distribution"].ToString();
                            row["handling_item"] = noSeaoverDt.Rows[i]["handling_item"].ToString();
                            row["ato_manager"] = noSeaoverDt.Rows[i]["ato_manager"].ToString();

                            row["origin"] = noSeaoverDt.Rows[i]["origin"].ToString();
                            row["remark2"] = noSeaoverDt.Rows[i]["remark2"].ToString();
                            row["company_manager"] = noSeaoverDt.Rows[i]["company_manager"].ToString();
                            row["company_manager_position"] = noSeaoverDt.Rows[i]["company_manager_position"].ToString();
                            row["email"] = noSeaoverDt.Rows[i]["email"].ToString();
                            row["web"] = noSeaoverDt.Rows[i]["web"].ToString();
                            row["sns1"] = noSeaoverDt.Rows[i]["sns1"].ToString();
                            row["sns2"] = noSeaoverDt.Rows[i]["sns2"].ToString();
                            row["sns3"] = noSeaoverDt.Rows[i]["sns3"].ToString();
                            bool isManagement1, isManagement2, isManagement3, isManagement4;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                isManagement1 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                isManagement2 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                isManagement3 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                isManagement4 = false;
                            row["isManagement1"] = isManagement1;
                            row["isManagement2"] = isManagement2;
                            row["isManagement3"] = isManagement3;
                            row["isManagement4"] = isManagement4;
                            bool isHide;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isHide"].ToString(), out isHide))
                                isHide = false;
                            row["isHide"] = isHide;
                            row["createtime"] = noSeaoverDt.Rows[i]["createtime"].ToString();

                            //잠재1, 잠재2
                            row["isPotential1"] = isPotential1;
                            row["isPotential2"] = isPotential2;
                            //영업금지 거래처
                            row["isNonHandled"] = isNonHandled;
                            row["isNotSendFax"] = isNotSendFax;
                            row["isOutBusiness"] = isOutBusiness;

                            //최근 영업정보
                            DateTime updatetime;
                            if (DateTime.TryParse(noSeaoverDt.Rows[i]["sales_updatetime"].ToString(), out updatetime))
                                row["sales_updatetime"] = updatetime.ToString("yyyy-MM-dd");
                            else
                                row["sales_updatetime"] = "";
                            //최근 영업내용                            
                            row["sales_contents"] = noSeaoverDt.Rows[i]["sales_contents"].ToString();
                            row["sales_remark"] = noSeaoverDt.Rows[i]["sales_remark"].ToString();
                            row["sales_edit_user"] = noSeaoverDt.Rows[i]["sales_edit_user"].ToString();
                            Dt3.Rows.Add(row);
                        }
                    }

                    //씨오버 등록거래처 출력
                    if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < seaoverDt1.Rows.Count; i++)
                        {
                            //삭제거래처
                            bool isDelete;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isDelete"].ToString(), out isDelete))
                                isDelete = false;
                            if (!isDelete)
                            {
                                // ** 매출 || 영업중 먼저인 내역이 우선 **

                                //최근 매출정보
                                DateTime current_sale_date;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                                    current_sale_date = new DateTime(1900, 1, 1);

                                //최근 영업정보
                                DateTime sales_updatetime;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                                    sales_updatetime = new DateTime(1900, 1, 1);

                                string sale_edit_user;
                                string sale_date;
                                string sale_contents;
                                string sale_remark;
                                string ato_manager;
                                if (current_sale_date >= sales_updatetime)
                                {
                                    seaoverDt1.Rows[i]["isTrading"] = "true";
                                    seaoverDt1.AcceptChanges();
                                    ato_manager = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_contents = "씨오버 매출";
                                    sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = "";
                                }
                                else
                                {
                                    ato_manager = seaoverDt1.Rows[i]["ato_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["sales_edit_user"].ToString();
                                    sale_contents = seaoverDt1.Rows[i]["sales_contents"].ToString();
                                    if (sale_contents == "씨오버 매출")
                                        sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();
                                    sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = seaoverDt1.Rows[i]["sales_remark"].ToString();
                                }


                                //출력여부============================================================================
                                bool isOutput = false;
                                if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                {
                                    string[] managers = txtManager.Text.Split(' ');
                                    for (int j = 0; j < managers.Length; j++)
                                    {
                                        if (!string.IsNullOrEmpty(managers[j]) && ato_manager.Contains(managers[j]))
                                        {
                                            isOutput = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                    isOutput = true;
                                //소송거래처 제외
                                if (isOutput && cbLitigation.Checked
                                    && (seaoverDt1.Rows[i]["company"].ToString().Contains("(S)") || seaoverDt1.Rows[i]["company"].ToString().Contains("소송")))
                                    isOutput = false;

                                //잠재1, 잠재2
                                bool isPotential1, isPotential2;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential1"].ToString(), out isPotential1))
                                    isPotential1 = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential2"].ToString(), out isPotential2))
                                    isPotential2 = false;
                                if (isOutput && !isPotential2)
                                    isOutput = false;
                                //영업금지 거래처
                                bool isNonHandled, isNotSendFax, isOutBusiness;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                                    isNonHandled = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                                    isNotSendFax = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                                    isOutBusiness = false;
                                if (isOutput && (isNonHandled || isNonHandled || isOutBusiness))
                                    isOutput = false;

                                //거래중 거래처 제외
                                bool isTrading;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isTrading"].ToString(), out isTrading))
                                    isTrading = false;
                                if (isOutput && isTrading)
                                    isOutput = false;

                                //출력
                                if (isOutput)
                                {
                                    DataRow row = Dt3.NewRow();
                                    row["id"] = seaoverDt1.Rows[i]["id"].ToString();
                                    row["group_name"] = seaoverDt1.Rows[i]["group_name"].ToString();
                                    row["company"] = seaoverDt1.Rows[i]["company"].ToString();
                                    row["seaover_company_code"] = seaoverDt1.Rows[i]["seaover_company_code"].ToString();
                                    row["tel"] = seaoverDt1.Rows[i]["tel"].ToString();
                                    row["fax"] = seaoverDt1.Rows[i]["fax"].ToString();
                                    row["phone"] = seaoverDt1.Rows[i]["phone"].ToString();
                                    row["registration_number"] = seaoverDt1.Rows[i]["registration_number"].ToString();
                                    row["ceo"] = seaoverDt1.Rows[i]["ceo"].ToString();
                                    row["address"] = seaoverDt1.Rows[i]["address"].ToString();
                                    row["remark"] = seaoverDt1.Rows[i]["remark"].ToString();
                                    row["distribution"] = seaoverDt1.Rows[i]["distribution"].ToString();
                                    row["handling_item"] = seaoverDt1.Rows[i]["handling_item"].ToString();

                                    row["origin"] = seaoverDt1.Rows[i]["origin"].ToString();
                                    row["remark2"] = seaoverDt1.Rows[i]["remark2"].ToString();
                                    row["company_manager"] = seaoverDt1.Rows[i]["company_manager"].ToString();
                                    row["company_manager_position"] = seaoverDt1.Rows[i]["company_manager_position"].ToString();
                                    row["email"] = seaoverDt1.Rows[i]["email"].ToString();
                                    row["web"] = seaoverDt1.Rows[i]["web"].ToString();
                                    row["sns1"] = seaoverDt1.Rows[i]["sns1"].ToString();
                                    row["sns2"] = seaoverDt1.Rows[i]["sns2"].ToString();
                                    row["sns3"] = seaoverDt1.Rows[i]["sns3"].ToString();
                                    bool isManagement1, isManagement2, isManagement3, isManagement4;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                        isManagement1 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                        isManagement2 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                        isManagement3 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                        isManagement4 = false;
                                    row["isManagement1"] = isManagement1;
                                    row["isManagement2"] = isManagement2;
                                    row["isManagement3"] = isManagement3;
                                    row["isManagement4"] = isManagement4;
                                    bool isHide;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isHide"].ToString(), out isHide))
                                        isHide = false;
                                    row["isHide"] = isHide;
                                    row["createtime"] = seaoverDt1.Rows[i]["createtime"].ToString();

                                    //잠재1, 잠재2
                                    row["isPotential1"] = isPotential1;
                                    row["isPotential2"] = isPotential2;
                                    //영업금지 거래처
                                    row["isNonHandled"] = isNonHandled;
                                    row["isNotSendFax"] = isNotSendFax;
                                    row["isOutBusiness"] = isOutBusiness;
                                    //최금영업내역
                                    row["sales_updatetime"] = sale_date;
                                    row["sales_contents"] = sale_contents;
                                    row["sales_remark"] = sale_remark;
                                    row["sales_edit_user"] = sale_edit_user;
                                    row["ato_manager"] = ato_manager;

                                    Dt3.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
                mainDt = Dt3;
            }
            else if (tcMain.SelectedTab.Text == "SEAOVER (F6)")
            {
                //초기화
                Dt4 = SetTable();
                //저장된 매출처먼저 출력
                if (noSeaoverDt.Rows.Count > 0 || (seaoverDt1 != null && seaoverDt1.Rows.Count > 0))
                {
                    //씨오버 등록거래처 출력
                    for (int i = 0; i < seaoverDt1.Rows.Count; i++)
                    {
                        //삭제거래처
                        bool isDelete;
                        if (!bool.TryParse(seaoverDt1.Rows[i]["isDelete"].ToString(), out isDelete))
                            isDelete = false;
                        if (!isDelete)
                        {
                            // ** 매출 || 영업중 먼저인 내역이 우선 **

                            //최근 매출정보
                            DateTime current_sale_date;
                            if (!DateTime.TryParse(seaoverDt1.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                                current_sale_date = new DateTime(1900, 1, 1);

                            //최근 영업정보
                            DateTime sales_updatetime;
                            if (!DateTime.TryParse(seaoverDt1.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                                sales_updatetime = new DateTime(1900, 1, 1);

                            //출력여부============================================================================
                            //무작위Data에서도 씨오버 거래처(덮어씌여지지 않은) 빠지고 여기서도 덮어씌여진 씨오버는 빠진다
                            bool isOutput = true;

                            string sale_edit_user;
                            string sale_date;
                            string sale_contents;
                            string sale_remark;
                            string ato_manager;
                            if (current_sale_date >= sales_updatetime)
                            {
                                seaoverDt1.Rows[i]["isTrading"] = "true";
                                seaoverDt1.AcceptChanges();
                                ato_manager = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                sale_edit_user = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                sale_contents = "씨오버 매출";
                                sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                                sale_remark = "";
                            }
                            else
                            {
                                ato_manager = seaoverDt1.Rows[i]["ato_manager"].ToString();
                                sale_edit_user = seaoverDt1.Rows[i]["sales_edit_user"].ToString();
                                sale_contents = seaoverDt1.Rows[i]["sales_contents"].ToString();
                                sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                sale_remark = seaoverDt1.Rows[i]["sales_remark"].ToString();
                            }


                            //담당자 검색
                            if (isOutput)
                            {
                                isOutput = false;
                                if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                                {
                                    string[] managers = txtManager.Text.Split(' ');
                                    for (int j = 0; j < managers.Length; j++)
                                    {
                                        if (!string.IsNullOrEmpty(managers[j]) && ato_manager.Contains(managers[j]))
                                        {
                                            isOutput = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                    isOutput = true;
                            }
                            //소송거래처 제외
                            if (isOutput && cbLitigation.Checked
                                && (seaoverDt1.Rows[i]["company"].ToString().Contains("(S)") || seaoverDt1.Rows[i]["company"].ToString().Contains("소송")))
                                isOutput = false;

                            //잠재1, 잠재2
                            bool isPotential1, isPotential2;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential1"].ToString(), out isPotential1))
                                isPotential1 = false;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential2"].ToString(), out isPotential2))
                                isPotential2 = false;
                            /*if (isOutput && !isPotential1)
                                isOutput = false;*/
                            //영업금지 거래처
                            bool isNonHandled, isNotSendFax, isOutBusiness;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                                isNonHandled = false;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                                isNotSendFax = false;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                                isOutBusiness = false;
                            if (isOutput && (isNonHandled || isNotSendFax || isOutBusiness))
                                isOutput = false;

                            //거래중 거래처 제외
                            bool isTrading;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isTrading"].ToString(), out isTrading))
                                isTrading = false;
                            if (isOutput && !isTrading)
                                isOutput = false;

                            //출력
                            if (isOutput)
                            {
                                DataRow row = Dt4.NewRow();
                                row["id"] = seaoverDt1.Rows[i]["id"].ToString();
                                row["group_name"] = seaoverDt1.Rows[i]["group_name"].ToString();
                                row["company"] = seaoverDt1.Rows[i]["company"].ToString();
                                row["seaover_company_code"] = seaoverDt1.Rows[i]["seaover_company_code"].ToString();
                                row["tel"] = seaoverDt1.Rows[i]["tel"].ToString();
                                row["fax"] = seaoverDt1.Rows[i]["fax"].ToString();
                                row["phone"] = seaoverDt1.Rows[i]["phone"].ToString();
                                row["registration_number"] = seaoverDt1.Rows[i]["registration_number"].ToString();
                                row["ceo"] = seaoverDt1.Rows[i]["ceo"].ToString();
                                row["address"] = seaoverDt1.Rows[i]["address"].ToString();
                                row["remark"] = seaoverDt1.Rows[i]["remark"].ToString();
                                row["distribution"] = seaoverDt1.Rows[i]["distribution"].ToString();
                                row["handling_item"] = seaoverDt1.Rows[i]["handling_item"].ToString();

                                row["origin"] = seaoverDt1.Rows[i]["origin"].ToString();
                                row["remark2"] = seaoverDt1.Rows[i]["remark2"].ToString();
                                row["company_manager"] = seaoverDt1.Rows[i]["company_manager"].ToString();
                                row["company_manager_position"] = seaoverDt1.Rows[i]["company_manager_position"].ToString();
                                row["email"] = seaoverDt1.Rows[i]["email"].ToString();
                                row["web"] = seaoverDt1.Rows[i]["web"].ToString();
                                row["sns1"] = seaoverDt1.Rows[i]["sns1"].ToString();
                                row["sns2"] = seaoverDt1.Rows[i]["sns2"].ToString();
                                row["sns3"] = seaoverDt1.Rows[i]["sns3"].ToString();
                                bool isManagement1, isManagement2, isManagement3, isManagement4;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                    isManagement1 = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                    isManagement2 = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                    isManagement3 = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                    isManagement4 = false;
                                row["isManagement1"] = isManagement1;
                                row["isManagement2"] = isManagement2;
                                row["isManagement3"] = isManagement3;
                                row["isManagement4"] = isManagement4;
                                bool isHide;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isHide"].ToString(), out isHide))
                                    isHide = false;
                                row["isHide"] = isHide;
                                row["createtime"] = seaoverDt1.Rows[i]["createtime"].ToString();

                                //잠재1, 잠재2
                                row["isPotential1"] = isPotential1;
                                row["isPotential2"] = isPotential2;
                                //영업금지 거래처
                                row["isNonHandled"] = isNonHandled;
                                row["isNotSendFax"] = isNotSendFax;
                                row["isOutBusiness"] = isOutBusiness;
                                //최금영업내역
                                row["sales_updatetime"] = sale_date;
                                row["sales_contents"] = sale_contents;
                                row["sales_remark"] = sale_remark;
                                row["sales_edit_user"] = sale_edit_user;
                                row["ato_manager"] = ato_manager;

                                Dt4.Rows.Add(row);
                            }
                        }
                    }
                }
                mainDt = Dt4;
            }
            else if (tcMain.SelectedTab.Text == "영업금지 거래처 (F7)")
            {
                //초기화
                Dt5 = SetTable();
                //저장된 매출처먼저 출력
                if (noSeaoverDt.Rows.Count > 0 || seaoverDt1.Rows.Count > 0)
                {
                    for (int i = 0; i < noSeaoverDt.Rows.Count; i++)
                    {
                        bool isOutput = true;
                        //담당자 거래처
                        /*if (!string.IsNullOrEmpty(companyDt.Rows[i]["ato_manager"].ToString().Trim()) && companyDt.Rows[i]["ato_manager"].ToString().Trim() == txtManager.Text)
                            isOutput = true;*/
                        //잠재1, 잠재2
                        bool isPotential1, isPotential2;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential1"].ToString(), out isPotential1))
                            isPotential1 = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isPotential2"].ToString(), out isPotential2))
                            isPotential2 = false;
                        /*if (isOutput && !isPotential1)
                            isOutput = false;*/
                        //영업금지 거래처
                        bool isNonHandled, isNotSendFax, isOutBusiness;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                            isNonHandled = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                            isNotSendFax = false;
                        if (!bool.TryParse(noSeaoverDt.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                            isOutBusiness = false;
                        if (isOutput && (!isNonHandled && !isNotSendFax && !isOutBusiness))
                            isOutput = false;

                        //소송거래처 제외
                        if (isOutput && cbLitigation.Checked
                            && (noSeaoverDt.Rows[i]["name"].ToString().Contains("(S)") || noSeaoverDt.Rows[i]["name"].ToString().Contains("소송")))
                            isOutput = false;

                        //폐업가리기
                        if (cbNotOutOfBusiness.Checked)
                        { 
                            if(isOutput && isOutBusiness)
                                isOutput = false;
                        }
                        //출력
                        if (isOutput)
                        {
                            DataRow row = Dt5.NewRow();
                            row["id"] = noSeaoverDt.Rows[i]["id"].ToString();
                            row["group_name"] = noSeaoverDt.Rows[i]["group_name"].ToString();
                            row["company"] = noSeaoverDt.Rows[i]["name"].ToString();
                            row["tel"] = noSeaoverDt.Rows[i]["tel"].ToString();
                            row["fax"] = noSeaoverDt.Rows[i]["fax"].ToString();
                            row["phone"] = noSeaoverDt.Rows[i]["phone"].ToString();
                            row["registration_number"] = noSeaoverDt.Rows[i]["registration_number"].ToString();
                            row["ceo"] = noSeaoverDt.Rows[i]["ceo"].ToString();
                            row["address"] = noSeaoverDt.Rows[i]["address"].ToString();
                            row["remark"] = noSeaoverDt.Rows[i]["remark"].ToString();
                            row["distribution"] = noSeaoverDt.Rows[i]["distribution"].ToString();
                            row["handling_item"] = noSeaoverDt.Rows[i]["handling_item"].ToString();
                            row["ato_manager"] = noSeaoverDt.Rows[i]["ato_manager"].ToString();

                            row["origin"] = noSeaoverDt.Rows[i]["origin"].ToString();
                            row["remark2"] = noSeaoverDt.Rows[i]["remark2"].ToString();
                            row["company_manager"] = noSeaoverDt.Rows[i]["company_manager"].ToString();
                            row["company_manager_position"] = noSeaoverDt.Rows[i]["company_manager_position"].ToString();
                            row["email"] = noSeaoverDt.Rows[i]["email"].ToString();
                            row["web"] = noSeaoverDt.Rows[i]["web"].ToString();
                            row["sns1"] = noSeaoverDt.Rows[i]["sns1"].ToString();
                            row["sns2"] = noSeaoverDt.Rows[i]["sns2"].ToString();
                            row["sns3"] = noSeaoverDt.Rows[i]["sns3"].ToString();
                            bool isManagement1, isManagement2, isManagement3, isManagement4;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                isManagement1 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                isManagement2 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                isManagement3 = false;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                isManagement4 = false;
                            row["isManagement1"] = isManagement1;
                            row["isManagement2"] = isManagement2;
                            row["isManagement3"] = isManagement3;
                            row["isManagement4"] = isManagement4;
                            bool isHide;
                            if (!bool.TryParse(noSeaoverDt.Rows[i]["isHide"].ToString(), out isHide))
                                isHide = false;
                            row["isHide"] = isHide;
                            row["createtime"] = noSeaoverDt.Rows[i]["createtime"].ToString();

                            //잠재1, 잠재2
                            row["isPotential1"] = isPotential1;
                            row["isPotential2"] = isPotential2;
                            //영업금지 거래처
                            row["isNonHandled"] = isNonHandled;
                            row["isNotSendFax"] = isNotSendFax;
                            row["isOutBusiness"] = isOutBusiness;

                            //최근 영업정보
                            DateTime updatetime;
                            if (DateTime.TryParse(noSeaoverDt.Rows[i]["sales_updatetime"].ToString(), out updatetime))
                                row["sales_updatetime"] = updatetime.ToString("yyyy-MM-dd");
                            else
                                row["sales_updatetime"] = "";
                            //최근 영업내용                            
                            row["sales_contents"] = noSeaoverDt.Rows[i]["sales_contents"].ToString();
                            row["sales_remark"] = noSeaoverDt.Rows[i]["sales_remark"].ToString();
                            row["sales_edit_user"] = noSeaoverDt.Rows[i]["sales_edit_user"].ToString();
                            Dt5.Rows.Add(row);
                        }
                    }

                    //씨오버 등록거래처 출력
                    if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < seaoverDt1.Rows.Count; i++)
                        {
                            //삭제거래처
                            bool isDelete;
                            if (!bool.TryParse(seaoverDt1.Rows[i]["isDelete"].ToString(), out isDelete))
                                isDelete = false;
                            if (!isDelete)
                            {
                                // ** 매출 || 영업중 먼저인 내역이 우선 **

                                //최근 매출정보
                                DateTime current_sale_date;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                                    current_sale_date = new DateTime(1900, 1, 1);

                                //최근 영업정보
                                DateTime sales_updatetime;
                                if (!DateTime.TryParse(seaoverDt1.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                                    sales_updatetime = new DateTime(1900, 1, 1);

                                string sale_edit_user;
                                string sale_date;
                                string sale_contents;
                                string sale_remark;
                                string ato_manager;
                                if (current_sale_date >= sales_updatetime)
                                {
                                    ato_manager = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["current_sale_manager"].ToString();
                                    sale_contents = "씨오버 매출";
                                    sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = "";
                                }
                                else
                                {
                                    ato_manager = seaoverDt1.Rows[i]["ato_manager"].ToString();
                                    sale_edit_user = seaoverDt1.Rows[i]["sales_edit_user"].ToString();
                                    sale_contents = seaoverDt1.Rows[i]["sales_contents"].ToString();
                                    sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = seaoverDt1.Rows[i]["sales_remark"].ToString();
                                }


                                //출력여부============================================================================
                                bool isOutput = true;
                                /*if (!string.IsNullOrEmpty(ato_manager))
                                {
                                    string[] managers = txtManager.Text.Split(' ');
                                    for (int j = 0; j < managers.Length; j++)
                                    {
                                        if (!string.IsNullOrEmpty(managers[j]) && ato_manager.Contains(managers[j]))
                                        {
                                            isOutput = true;
                                            break;
                                        }
                                    }
                                }*/
                                //소송거래처 제외
                                if (isOutput && cbLitigation.Checked
                                    && (seaoverDt1.Rows[i]["company"].ToString().Contains("(S)") || seaoverDt1.Rows[i]["company"].ToString().Contains("소송")))
                                    isOutput = false;


                                //잠재1, 잠재2
                                bool isPotential1, isPotential2;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential1"].ToString(), out isPotential1))
                                    isPotential1 = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isPotential2"].ToString(), out isPotential2))
                                    isPotential2 = false;
                                /*if (isOutput && !isPotential1)
                                    isOutput = false;*/
                                //영업금지 거래처
                                bool isNonHandled, isNotSendFax, isOutBusiness;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                                    isNonHandled = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                                    isNotSendFax = false;
                                if (!bool.TryParse(seaoverDt1.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                                    isOutBusiness = false;
                                if (isOutput && (!isNonHandled && !isNonHandled && !isOutBusiness))
                                    isOutput = false;
                                //폐업가리기
                                if (cbNotOutOfBusiness.Checked)
                                {
                                    if (isOutput && isOutBusiness)
                                        isOutput = false;
                                }
                                //출력
                                if (isOutput)
                                {
                                    DataRow row = Dt5.NewRow();
                                    row["id"] = seaoverDt1.Rows[i]["id"].ToString();
                                    row["group_name"] = seaoverDt1.Rows[i]["group_name"].ToString();
                                    row["company"] = seaoverDt1.Rows[i]["company"].ToString();
                                    row["seaover_company_code"] = seaoverDt1.Rows[i]["seaover_company_code"].ToString();
                                    row["tel"] = seaoverDt1.Rows[i]["tel"].ToString();
                                    row["fax"] = seaoverDt1.Rows[i]["fax"].ToString();
                                    row["phone"] = seaoverDt1.Rows[i]["phone"].ToString();
                                    row["registration_number"] = seaoverDt1.Rows[i]["registration_number"].ToString();
                                    row["ceo"] = seaoverDt1.Rows[i]["ceo"].ToString();
                                    row["address"] = seaoverDt1.Rows[i]["address"].ToString();
                                    row["remark"] = seaoverDt1.Rows[i]["remark"].ToString();
                                    row["distribution"] = seaoverDt1.Rows[i]["distribution"].ToString();
                                    row["handling_item"] = seaoverDt1.Rows[i]["handling_item"].ToString();

                                    row["origin"] = seaoverDt1.Rows[i]["origin"].ToString();
                                    row["remark2"] = seaoverDt1.Rows[i]["remark2"].ToString();
                                    row["company_manager"] = seaoverDt1.Rows[i]["company_manager"].ToString();
                                    row["company_manager_position"] = seaoverDt1.Rows[i]["company_manager_position"].ToString();
                                    row["email"] = seaoverDt1.Rows[i]["email"].ToString();
                                    row["web"] = seaoverDt1.Rows[i]["web"].ToString();
                                    row["sns1"] = seaoverDt1.Rows[i]["sns1"].ToString();
                                    row["sns2"] = seaoverDt1.Rows[i]["sns2"].ToString();
                                    row["sns3"] = seaoverDt1.Rows[i]["sns3"].ToString();
                                    bool isManagement1, isManagement2, isManagement3, isManagement4;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement1"].ToString(), out isManagement1))
                                        isManagement1 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement2"].ToString(), out isManagement2))
                                        isManagement2 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement3"].ToString(), out isManagement3))
                                        isManagement3 = false;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isManagement4"].ToString(), out isManagement4))
                                        isManagement4 = false;
                                    row["isManagement1"] = isManagement1;
                                    row["isManagement2"] = isManagement2;
                                    row["isManagement3"] = isManagement3;
                                    row["isManagement4"] = isManagement4;
                                    bool isHide;
                                    if (!bool.TryParse(seaoverDt1.Rows[i]["isHide"].ToString(), out isHide))
                                        isHide = false;
                                    row["isHide"] = isHide;
                                    row["createtime"] = seaoverDt1.Rows[i]["createtime"].ToString();

                                    //잠재1, 잠재2
                                    row["isPotential1"] = isPotential1;
                                    row["isPotential2"] = isPotential2;
                                    //영업금지 거래처
                                    row["isNonHandled"] = isNonHandled;
                                    row["isNotSendFax"] = isNotSendFax;
                                    row["isOutBusiness"] = isOutBusiness;
                                    //최금영업내역
                                    row["sales_updatetime"] = sale_date;
                                    row["sales_contents"] = sale_contents;
                                    row["sales_remark"] = sale_remark;
                                    row["sales_edit_user"] = sale_edit_user;
                                    row["ato_manager"] = ato_manager;

                                    Dt5.Rows.Add(row);
                                }
                            }
                        }
                    }
                }
                mainDt = Dt5;
            }
            //정렬하기
            DataView dv = new DataView(mainDt);
            dv.Sort = GetOrderByTxt(cbSortType.Text);
            mainDt = dv.ToTable();

            dgvCompany.DataSource = mainDt;
            SetHeaderStyle();
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
        }

        private string GetOrderByTxt(string sortType)
        {
            if (string.IsNullOrEmpty(sortType.Trim()))
                return null;

            string[] sort = sortType.Split('+');
            string result = "";
            if (dgvCompany.Columns.Count > 0)
            {
                for (int i = 0; i < sort.Length; i++)
                {
                    if (!string.IsNullOrEmpty(sort[i].Trim()))
                    {
                        for (int j = 0; j < mainDt.Columns.Count; j++)
                        {
                            if (sort[i] == mainDt.Columns[j].Caption)
                            {
                                if (string.IsNullOrEmpty(result))
                                    result = mainDt.Columns[j].ColumnName;
                                else
                                    result += "," + mainDt.Columns[j].ColumnName;

                                break;
                            }
                        }
                    }
                }

                return result;
            }
            else
                return null;
        }

        private string SetSecurityTxt(string txt, int visibleLength = 1)
        {
            //securityType 1: 반가리기
            //securityType 2 부터는 보여지는 글자수

            if (!string.IsNullOrEmpty(txt) && txt.Length > 2)
            {
                if (visibleLength == 1)
                {
                    string resultTxt = txt.Substring(0, txt.Length / 2);

                    for (int i = txt.Length; i < txt.Length; i++)
                        resultTxt += "*";
                    return resultTxt;
                }
                else
                {
                    if (visibleLength < txt.Length)
                        visibleLength = txt.Length;


                    string resultTxt = txt.Substring(0, visibleLength - 1);

                    for (int i = visibleLength - 1; i < txt.Length; i++)
                        resultTxt += "*";
                    return resultTxt;

                }
            }
            else
                return txt;

        }

        #endregion

        #region Button
        private void btnDistribution_Click(object sender, EventArgs e)
        {
            DataDistribution dd = new DataDistribution(um);
            dd.Owner = this;
            dd.Show();
        }
        private void cbNotOutOfBusiness_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["isOutBusiness"].Visible = !cbNotOutOfBusiness.Checked;
        }
        private void btnMySales_Click(object sender, EventArgs e)
        {
            try
            {
                MySalesManager msm = new MySalesManager(um);
                msm.Show();
            }
            catch
            { }
        }
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            try
            {
                DuplicateCompany dc = new DuplicateCompany(um, true);
                dc.Show();
            }
            catch
            { }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (UpdateCompanyInfo(1))
                GetData();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void txtGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetData();

        }
        private void SalesManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.S:
                        btnAddCompany.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtGroupName.Text = String.Empty;
                        txtCompany.Text = String.Empty;
                        txtTel.Text = String.Empty;
                        txtCeo.Text = String.Empty;
                        txtRNum.Text = String.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (dgvCompany.Rows.Count > 0 && dgvCompany.SelectedCells.Count > 0 && dgvCompany.SelectedCells[0].RowIndex >= 0)
                        {
                            if (dgvCompany.Columns[dgvCompany.SelectedCells[0].ColumnIndex].Name == "is_potentail1"
                                || dgvCompany.Columns[dgvCompany.SelectedCells[0].ColumnIndex].Name == "is_potentail2"
                                || dgvCompany.Columns[dgvCompany.SelectedCells[0].ColumnIndex].Name == "is_non_handled"
                                || dgvCompany.Columns[dgvCompany.SelectedCells[0].ColumnIndex].Name == "is_out_business"
                                || dgvCompany.Columns[dgvCompany.SelectedCells[0].ColumnIndex].Name == "is_not_send_fax")
                            {
                                dgvCompany.SelectedCells[0].Value = !Convert.ToBoolean(dgvCompany.SelectedCells[0].Value);
                            }
                        }
                        break;
                }
            }
            else
            {
                int idx;
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        tcMain.SelectTab("tabCommonData");
                        break;
                    case Keys.F2:
                        tcMain.SelectTab("tabRamdomData");
                        break;
                    case Keys.F3:
                        tcMain.SelectTab("tabCompany");
                        break;
                    case Keys.F4:
                        tcMain.SelectTab("tabCompany2");
                        break;
                    case Keys.F6:
                        tcMain.SelectTab("tabSeaover");
                        break;
                    case Keys.F7:
                        tcMain.SelectTab("tabNotTreatment");
                        break;
                }
            }
        }
        #endregion

        #region 우클릭 메뉴
        ContextMenuStrip m;
        private void dgvCompany_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvCompany.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    m = new ContextMenuStrip();


                    m.Items.Add("거래처 정보수정(A)");
                    if (um.auth_level >= 90)
                        m.Items.Add("거래처 삭제");
                    ToolStripSeparator toolStripSeparator0 = new ToolStripSeparator();
                    toolStripSeparator0.Name = "toolStripSeparator";
                    toolStripSeparator0.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator0);
                    m.Items.Add("공용DATA 전환");
                    m.Items.Add("무작위DATA 전환");
                    ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                    toolStripSeparator1.Name = "toolStripSeparator";
                    toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator1);
                    m.Items.Add("잠재1 전환");
                    m.Items.Add("잠재2 전환");
                    m.Items.Add("거래중(SEAOVER (F6)) 전환");
                    ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                    toolStripSeparator2.Name = "toolStripSeparator";
                    toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator2);
                    m.Items.Add("취급X 전환");
                    m.Items.Add("팩스X 전환");
                    m.Items.Add("폐업 전환");

                    if (tcMain.SelectedTab.Text == "SEAOVER (F6)")
                    {
                        ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
                        toolStripSeparator3.Name = "toolStripSeparator";
                        toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator3);
                        m.Items.Add("취급품목 관리");
                    }

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvCompany, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvCompany.SelectedRows.Count > 0)
            {
                this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                try
                {
                    DataGridViewRow dr = dgvCompany.SelectedRows[0];
                    if (dr.Index < 0)
                        return;
                    int rowindex = dr.Index;

                    //선택한 내역 선택
                    foreach (DataGridViewRow row in dgvCompany.Rows)
                        row.Cells["chk"].Value = false;
                    foreach (DataGridViewRow row in dgvCompany.SelectedRows)
                        row.Cells["chk"].Value = true;

                    //함수호출
                    switch (e.ClickedItem.Text)
                    {
                        case "거래처 정보수정(A)":
                            
                            if (UpdateCompanyInfo(1))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;
                                }
                            }
                            break;
                        case "공용DATA 전환":
                            if (MessageBox.Show(new Form { TopMost = true }, "모든 담당자가 확인할 수 있는 공용DATA로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                               
                            if (UpdateCompanyInfo(2))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                    {
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;

                                        if (tcMain.SelectedTab.Text != "공용ATA 전환 (F2)")
                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                    }
                                }
                            }
                            break;
                        case "무작위DATA 전환":
                            if (MessageBox.Show(new Form { TopMost = true }, um.user_name + "님만 확인할 수 있는 무작위DATA로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                                
                            if (UpdateCompanyInfo(3))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                    {
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;

                                        if (tcMain.SelectedTab.Text != "무작위DATA 전환 (F2)")
                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                    }
                                }
                            }
                            break;
                        case "잠재1 전환":

                            if (MessageBox.Show(new Form { TopMost = true }, "잠재1로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;

                            if (UpdateCompanyInfo(4))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                    {
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;

                                        if (tcMain.SelectedTab.Text != "잠재1 전환 (F3)")
                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                    }
                                }
                            }
                            break;
                        case "잠재2 전환":
                            if (MessageBox.Show(new Form { TopMost = true }, "잠재2로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(5))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                    {
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;

                                        if (tcMain.SelectedTab.Text != "잠재2 전환 (F4)")
                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                    }
                                }
                            }
                            break;

                        case "거래중(SEAOVER (F6)) 전환":
                            if (MessageBox.Show(new Form { TopMost = true }, "거래중(SEAOVER (F6)) 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(9))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                    {
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;

                                        if (tcMain.SelectedTab.Text != "SEAOVER (F6)")
                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                    }
                                }
                            }
                            break;
                        case "취급X 전환":
                            if (MessageBox.Show(new Form { TopMost = true }, "[영업금지 거래처] 취급X 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(6))
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                    {
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;

                                        if (tcMain.SelectedTab.Text != "영업금지 거래처 (F7)")
                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                    }
                                }
                            break;
                        case "팩스X 전환":
                            if (MessageBox.Show(new Form { TopMost = true }, "[영업금지 거래처] 팩스X 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;

                            if (UpdateCompanyInfo(7))
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                    {
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;

                                        if (tcMain.SelectedTab.Text != "영업금지 거래처 (F7)")
                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                    }
                                }
                            break;
                        case "폐업 전환":
                            if (MessageBox.Show(new Form { TopMost = true }, "[영업금지 거래처] 폐업 거래처로 전환됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                            if (UpdateCompanyInfo(8))
                            {
                                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                                {
                                    if (Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value))
                                    {
                                        dgvCompany.Rows[i].Cells["chk"].Value = false;

                                        if (tcMain.SelectedTab.Text != "영업금지 거래처 (F7)")
                                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                                    }
                                }
                            }
                            break;
                        case "취급품목 관리":
                            {
                                try
                                {
                                    SaleProductBySeaover spb = new SaleProductBySeaover(um, dr.Cells["seaover_company_code"].Value.ToString(), dr.Cells["company"].Value.ToString());
                                    spb.Show();
                                }
                                catch
                                { }
                            }
                            break;
                        case "거래처 삭제":
                            {
                                if (MessageBox.Show(new Form { TopMost = true }, "거래처 정보를 삭제하시겠습니까?\n * SEAOVER업체일 경우 숨김처리 됩니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;
                                if (DeleteCompanyInfo())
                                {
                                    foreach (DataGridViewRow row in dgvCompany.Rows)
                                    {
                                        if (Convert.ToBoolean(row.Cells["chk"].Value))
                                            dgvCompany.Rows.Remove(row);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch
                { }
                this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            }
        }
        private void cms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.A:
                    if (dgvCompany.SelectedRows.Count > 0)
                        m.Items[0].PerformClick();
                    break;
                case Keys.D:

                    break;
            }
        }
        private void dgvCompany_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && dgvCompany.SelectedRows.Count <= 1)
                {
                    dgvCompany.ClearSelection();
                    dgvCompany.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        #endregion

        #region Datagrieveiw, tabControl event
        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            tcMain.SelectedTab.Controls.Add(this.pnRecord);
            this.pnRecord.BringToFront();
            tcMain.SelectedTab.Controls.Add(this.pnSearch);
            this.pnSearch.BringToFront();
            if (tcMain.SelectedTab.Text == "공용DATA (F1)")
                mainDt = Dt0;
            else if (tcMain.SelectedTab.Text == "무작위DATA (F2)")
                mainDt = Dt1;
            else if (tcMain.SelectedTab.Text == "잠재1 (F3)")
                mainDt = Dt2;
            else if (tcMain.SelectedTab.Text == "잠재2 (F4)")
                mainDt = Dt3;
            else if (tcMain.SelectedTab.Text == "SEAOVER (F6)")               
                mainDt = Dt4;
            else if (tcMain.SelectedTab.Text == "영업금지 거래처 (F7)")
                mainDt = Dt5;
            tcMain.SelectedTab.Controls.Add(this.dgvCompany);
            this.dgvCompany.BringToFront();

            dgvCompany.DataSource = mainDt;
            SetHeaderStyle();
        }

        private void dgvCompany_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                dgvCompany.EndEdit();
                this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            }
        }
        private void dgvCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                //dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                dgvCompany.EndEdit();
                if (dgvCompany.Columns[e.ColumnIndex].Name == "status_absence")
                {
                    bool isCheck = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                    dgvCompany.Rows[e.RowIndex].Cells["status_sales"].Value = !isCheck;
                    dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                }
                else if (dgvCompany.Columns[e.ColumnIndex].Name == "status_sales")
                {
                    bool isCheck = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                    dgvCompany.Rows[e.RowIndex].Cells["status_absence"].Value = !isCheck;
                    dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                }
                dgvCompany.EndEdit();
                this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            }
        }
        private void dgvCompany_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewColumn col = dgvCompany.Columns[e.ColumnIndex];
                //체크여부
                if (col.Name == "chk")
                {
                    bool isChecked = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells["chk"].Value);
                    if (isChecked)
                        dgvCompany.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.Red;
                    else
                        dgvCompany.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.White;
                }
                else if (col.Name == "image")
                {
                    string company = dgvCompany.Rows[e.RowIndex].Cells["company"].Value.ToString();
                    if (company.Contains("(선)") || company.Contains("tjs"))
                        dgvCompany.Rows[e.RowIndex].Cells["image"].Value = Properties.Resources.Money_icon;
                    else if (company.Contains("(악)") || company.Contains("dkr"))
                        dgvCompany.Rows[e.RowIndex].Cells["image"].Value = Properties.Resources.Devil_icon;
                    else
                        dgvCompany.Rows[e.RowIndex].Cells["image"].Value = null;
                }
                else if (col.Name.Contains("div"))
                {
                    dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gray;
                }
                else if (col.Name == "sales_current_date" || col.Name == "sales_updatetime")
                {
                    DateTime dt;
                    if (dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && DateTime.TryParse(dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out dt))
                    {
                        TimeSpan dateDiff = DateTime.Now - dt;
                        double diffMonth = dateDiff.Days / 30;

                        if (diffMonth > 3)
                        {
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Blue;
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                        }
                        else if (diffMonth > 2)
                        {
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.DeepSkyBlue;
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                        }
                        else if (diffMonth > 1)
                        {
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.LightBlue;
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                        }
                        else
                        {
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                            dgvCompany.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("나눔고딕", 9, FontStyle.Regular);
                        }
                    }
                }
            }
        }
        private void dgvCompany_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCompany.SelectedCells.Count > 0)
                txtCurrentRecord.Text = (dgvCompany.SelectedCells[0].RowIndex + 1).ToString("#,##0");
        }
        #endregion
    }
}
