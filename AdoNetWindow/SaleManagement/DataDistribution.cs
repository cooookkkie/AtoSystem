using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement.SalesManagerModule;
using Repositories;
using Repositories.Company;
using Repositories.Config;
using Repositories.SalesPartner;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement
{
    public partial class DataDistribution : Form
    {
        ICompanyGroupRepository companyGroupRepository = new CompanyGroupRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ISalesRepository salesRepository = new SalesRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IUsersRepository usersRepository = new UsersRepository();
        IDepartmentRepository departmentRepository = new DepartmentRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();

        DataTable atoDt = new DataTable();
        DataTable mainDt = new DataTable();

        SalesManager sm = null;
        UsersModel um;

        Libs.MessageBox messageBox = new Libs.MessageBox();

        Timer tm = new Timer();
        private System.Threading.Thread t1;  // 스레드 선언
        public DataDistribution(UsersModel um, SalesManager sm)
        {
            InitializeComponent();
            this.um = um;
            this.sm = sm;
            RefreshTable();
        }
        public DataDistribution(UsersModel um, DataTable companyDt, SalesManager sm)
        {
            InitializeComponent();
            this.um = um;
            this.sm = sm;
            RefreshTable();
            mainDt = companyDt;
            lbCommonDataCount.Text = mainDt.Rows.Count.ToString("#,##0");
        }

        private void DataDistribution_Load(object sender, EventArgs e)
        {
            pnGlass.Visible = false;
            pnGlass.Enabled = false;
            if (runCnt == 0)
            {
                GetDepartment();
                GetEmployee();
                SetData();
                DataDuplicate();
            }
                /*tm.Tick += new EventHandler(timer1_Tick);
                tm.Interval = 1000;
                tm.Start();*/
        }
        int runCnt = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (runCnt == 0)
            {
                GetDepartment();
                GetEmployee();
                SetData();
                DataDuplicate();

                //모든 컨트롤 사용 활성화
                foreach (Control c in this.Controls)
                {
                    c.Enabled = true;
                }
                pnGlass.Visible = false;
                pnGlass.Enabled = false;
                runCnt++;
                tm.Stop();
            }
        }
        #region Method

        private void DataDuplicate()
        {
            if (dgvData.Rows.Count > 0)
            {
                //유효성검사, 초기화
                dgvData.EndEdit();
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (dgvData.Rows[i].Cells["registration_number"].Value == null || string.IsNullOrEmpty(dgvData.Rows[i].Cells["registration_number"].Value.ToString()))
                        dgvData.Rows[i].Cells["registration_number"].Value = " ";
                }

                Dictionary<int, List<DataRow>> duplicateDic = new Dictionary<int, List<DataRow>>();

                //입력데이터 -> Datatable
                DataTable inputDt = common.ConvertDgvToDataTable(dgvData);
                inputDt.Columns.Add("rowIndex", typeof(Int32));
                for (int i = 0; i < inputDt.Rows.Count; i++)
                    inputDt.Rows[i]["rowindex"] = i;
                inputDt.AcceptChanges();


                //매출처 정보
                DataTable tempDt = SetSeaoverDatatable(atoDt);           //Ato전산에만 있는 거래처

                DataTable duplicateDt = null;
                // 데이터 중복추출, PLINQ 사용
                DataRow[] tempDr = inputDt.Select("tel <> ''");
                DataRow[] tempDr2 = tempDt.Select("tel <> ''");
                DataTable rnDt2 = tempDr2.CopyToDataTable();
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    var duplicateVar1 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("tel") equals t.Field<string>("tel")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            rowindex = p.Field<Int32>("rowindex"),
                                            table_index = (t == null) ? "-1" : t.Field<string>("table_index"),
                                            division = (t == null) ? "" : t.Field<string>("division"),
                                            tel = p.Field<string>("tel"),
                                            ato_manager = (t == null) ? "" : t.Field<string>("ato_manager")
                                        };
                    DataTable duplicateDt1 = ConvertListToDatatable(duplicateVar1);
                    duplicateDt1.AcceptChanges();
                    duplicateDt = duplicateDt1;
                }

                tempDr = inputDt.Select("fax <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    var duplicateVar2 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("fax") equals t.Field<string>("tel")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            rowindex = p.Field<Int32>("rowindex"),
                                            table_index = (t == null) ? "-1" : t.Field<string>("table_index"),
                                            division = (t == null) ? "" : t.Field<string>("division"),
                                            tel = p.Field<string>("fax"),
                                            ato_manager = (t == null) ? "" : t.Field<string>("ato_manager")
                                        };
                    DataTable duplicateDt2 = ConvertListToDatatable(duplicateVar2);
                    duplicateDt2.AcceptChanges();
                    if(duplicateDt == null)
                        duplicateDt = duplicateDt2;
                    else
                        duplicateDt.Merge(duplicateDt2);
                }

                tempDr = inputDt.Select("phone <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    var duplicateVar3 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("phone") equals t.Field<string>("tel")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            rowindex = p.Field<Int32>("rowindex"),
                                            table_index = (t == null) ? "-1" : t.Field<string>("table_index"),
                                            division = (t == null) ? "" : t.Field<string>("division"),
                                            tel = p.Field<string>("phone"),
                                            ato_manager = (t == null) ? "" : t.Field<string>("ato_manager")
                                        };
                    DataTable duplicateDt3 = ConvertListToDatatable(duplicateVar3);
                    duplicateDt3.AcceptChanges();
                    if (duplicateDt == null)
                        duplicateDt = duplicateDt3;
                    else
                        duplicateDt.Merge(duplicateDt3);
                }

                tempDr = inputDt.Select("other_phone <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    var duplicateVar4 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("other_phone") equals t.Field<string>("tel")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            rowindex = p.Field<Int32>("rowindex"),
                                            table_index = (t == null) ? "-1" : t.Field<string>("table_index"),
                                            division = (t == null) ? "" : t.Field<string>("division"),
                                            tel = p.Field<string>("other_phone"),
                                            ato_manager = (t == null) ? "" : t.Field<string>("ato_manager")
                                        };
                    DataTable duplicateDt4 = ConvertListToDatatable(duplicateVar4);
                    duplicateDt4.AcceptChanges();
                    if (duplicateDt == null)
                        duplicateDt = duplicateDt4;
                    else
                        duplicateDt.Merge(duplicateDt4);
                }

                //사업자번호 확인
                tempDr = inputDt.Select("registration_number <> ''");
                tempDr2 = tempDt.Select("registration_number <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    rnDt2 = tempDr2.CopyToDataTable();
                    var duplicateVar5 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("registration_number") equals t.Field<string>("registration_number")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            rowindex = p.Field<Int32>("rowindex"),
                                            table_index = (t == null) ? "-1" : t.Field<string>("table_index"),
                                            division = (t == null) ? "" : t.Field<string>("division"),
                                            tel = p.Field<string>("registration_number"),
                                            ato_manager = (t == null) ? "" : t.Field<string>("ato_manager")
                                        };
                    DataTable duplicateDt5 = ConvertListToDatatable(duplicateVar5);
                    duplicateDt5.AcceptChanges();
                    if (duplicateDt == null)
                        duplicateDt = duplicateDt5;
                    else
                        duplicateDt.Merge(duplicateDt5);
                }
                //중복삭제
                DataRow[] resultDr = duplicateDt.Select("division <> '' AND division <> '취급X' AND tel <> ''");
                if (resultDr.Length > 0)
                {
                    DataTable resultDt = resultDr.CopyToDataTable();
                    resultDt = resultDt.DefaultView.ToTable(true, new string[] { "rowindex", "table_index", "division", "ato_manager" });
                    resultDt.AcceptChanges();

                    // 추출된 중복 데이터를 순회하며 데이터가공
                    foreach (DataRow duplicate in resultDt.Rows)
                    {
                        int rowindex;
                        if (int.TryParse(duplicate["rowindex"].ToString(), out rowindex) && !string.IsNullOrEmpty(duplicate["ato_manager"].ToString()) 
                            && duplicate["division"].ToString() != "취급X"
                            && duplicate["division"].ToString() != "삭제거래처")
                        {

                            if (dgvData.Rows[rowindex].Cells["exist_ato_manager"].Value == null)
                                dgvData.Rows[rowindex].Cells["exist_ato_manager"].Value = duplicate["ato_manager"].ToString();
                            else if(!dgvData.Rows[rowindex].Cells["exist_ato_manager"].Value.ToString().Contains(duplicate["ato_manager"].ToString()))
                                dgvData.Rows[rowindex].Cells["exist_ato_manager"].Value += " " +  duplicate["ato_manager"].ToString();
                        }
                    }
                }
            }
        }


        private void GetCount()
        {
            dgvData.EndEdit();
            int cnt = 0;
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                bool isCheck = Convert.ToBoolean(dgvData.Rows[i].Cells["chk"].Value);
                if (isCheck)
                    cnt++;
            }
            lbSelectData.Text = cnt.ToString("#,##0");

            //분배된 사용자 수
            for (int i = 0; i < dgvSelectEmployee.Rows.Count; i++)
            {
                cnt = 0;
                double sales_amount = 0;
                double total_sales_amount = 0;
                for (int j = 0; j < dgvData.Rows.Count; j++)
                {
                    if (dgvSelectEmployee.Rows[i].Cells["select_user_name"].Value == dgvData.Rows[j].Cells["ato_manager"].Value)
                    {
                        cnt++;

                        if (dgvData.Rows[j].Cells["current_sales_amount"].Value == null || !double.TryParse(dgvData.Rows[j].Cells["current_sales_amount"].Value.ToString(), out sales_amount))
                            sales_amount = 0;
                        total_sales_amount += sales_amount;
                    }

                    dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value = cnt.ToString("#,##0");
                    dgvSelectEmployee.Rows[i].Cells["total_sales_amount"].Value = total_sales_amount.ToString("#,##0");
                }
            }

        }
        private void SetData()
        {
            if (mainDt != null && mainDt.Rows.Count > 0)
            {
                //매출현황
                DataTable salesAmountDt = salesRepository.GetSalesAmount(DateTime.Now.AddMonths(-1).AddYears(-1), DateTime.Now.AddMonths(-1));

                //출력
                dgvData.Rows.Clear();
                for (int i = 0; i < mainDt.Rows.Count; i++)
                {
                    if (!mainDt.Rows[i]["company"].ToString().Contains("(S)") && !mainDt.Rows[i]["company"].ToString().Contains("소송"))
                    {
                        int n = dgvData.Rows.Add();
                        dgvData.Rows[n].Cells["chk"].Value = true;
                        dgvData.Rows[n].Cells["company_id"].Value = mainDt.Rows[i]["id"].ToString();
                        dgvData.Rows[n].Cells["company"].Value = mainDt.Rows[i]["company"].ToString();
                        dgvData.Rows[n].Cells["seaover_company_code"].Value = mainDt.Rows[i]["seaover_company_code"].ToString();
                        dgvData.Rows[n].Cells["tel"].Value = mainDt.Rows[i]["tel"].ToString();
                        dgvData.Rows[n].Cells["fax"].Value = mainDt.Rows[i]["fax"].ToString();
                        dgvData.Rows[n].Cells["phone"].Value = mainDt.Rows[i]["phone"].ToString();
                        dgvData.Rows[n].Cells["other_phone"].Value = mainDt.Rows[i]["other_phone"].ToString();
                        dgvData.Rows[n].Cells["registration_number"].Value = mainDt.Rows[i]["registration_number"].ToString();

                        if (!string.IsNullOrEmpty(mainDt.Rows[i]["seaover_company_code"].ToString()))
                        {
                            DataRow[] dr = salesAmountDt.Select($"거래처코드 = '{mainDt.Rows[i]["seaover_company_code"].ToString()}'");
                            if (dr.Length > 0)
                            {
                                double sales_amount;
                                if (!double.TryParse(dr[0]["매출금액"].ToString(), out sales_amount))
                                    sales_amount = 0;
                                dgvData.Rows[n].Cells["current_sales_amount"].Value = sales_amount.ToString("#,##0");

                            }
                        }

                        dgvData.Rows[n].Cells["industry_type"].Value = mainDt.Rows[i]["industry_type"].ToString();
                        int industry_type_rank = sm.GetIndustryTypeRank(mainDt.Rows[i]["industry_type"].ToString());
                        dgvData.Rows[n].Cells["industry_type_rank"].Value = industry_type_rank;
                    }
                }
            }
        }
        #region 중복거래처 비교테이블 만들기
        private string ValidationTelString(string tel)
        {
            if (tel == null)
                tel = "";
            tel = tel.Replace("/", "");

            if (!string.IsNullOrEmpty(tel))
            {
                tel = tel.Replace(" ", "");
                tel = tel.Replace("-", "");
                tel = tel.Replace("(", "");
                tel = tel.Replace(")", "");

                if (tel.Length == 8 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "010" + tel;
                else if (tel.Length > 2 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel += "0";

            }
            return tel;
        }
        private DataTable SetSeaoverDatatable(DataTable seaoverDt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("idx", typeof(string));
            dt.Columns.Add("company", typeof(string));
            dt.Columns.Add("current_sale_date", typeof(string));
            dt.Columns.Add("seaover_company_code", typeof(string));
            dt.Columns.Add("tel", typeof(string));
            dt.Columns.Add("registration_number", typeof(string));
            dt.Columns.Add("ato_manager", typeof(string));
            dt.Columns.Add("isNonHandled", typeof(string));
            dt.Columns.Add("isOutBusiness", typeof(string));
            dt.Columns.Add("isNotSendFax", typeof(string));
            dt.Columns.Add("isPotential1", typeof(string));
            dt.Columns.Add("isPotential2", typeof(string));
            dt.Columns.Add("isTrading", typeof(string));
            dt.Columns.Add("division", typeof(string));
            dt.Columns.Add("table_index", typeof(string));
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                int idx = 0;
                foreach (DataRow row in seaoverDt.Rows)
                {
                    //TEL
                    idx--;
                    string tel = row["tel"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 1; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel;
                                    if (tels[0].Length > tels[j].Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - (tels[0].Length - tels[j].Length)) + tels[j];
                                    else
                                        rsTel = tels[j];

                                    DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 1; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel;
                                    if (tels[0].Length > tels[j].Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - (tels[0].Length - tels[j].Length)) + tels[j];
                                    else
                                        rsTel = tels[j];

                                    DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0], @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1], @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow2(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }

                    }

                    tel = row["fax"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 1; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel;
                                    if (tels[0].Length > tels[j].Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - (tels[0].Length - tels[j].Length)) + tels[j];
                                    else
                                        rsTel = tels[j];

                                    DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 1; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel;
                                    if (tels[0].Length > tels[j].Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - (tels[0].Length - tels[j].Length)) + tels[j];
                                    else
                                        rsTel = tels[j];

                                    DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0], @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1], @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow2(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                    }

                    tel = row["phone"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 1; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel;
                                    if (tels[0].Length > tels[j].Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - (tels[0].Length - tels[j].Length)) + tels[j];
                                    else
                                        rsTel = tels[j];

                                    DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 1; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel;
                                    if (tels[0].Length > tels[j].Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - (tels[0].Length - tels[j].Length)) + tels[j];
                                    else
                                        rsTel = tels[j];

                                    DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0], @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1], @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow2(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            return dt;
        }
        private DataRow GetDatarow2(DataTable dt, DataRow row, string tel, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = row["id"].ToString();
            dr["idx"] = idx.ToString();
            dr["company"] = row["company"].ToString();
            dr["current_sale_date"] = row["current_sale_date"].ToString();
            dr["seaover_company_code"] = row["seaover_company_code"].ToString();
            dr["tel"] = tel;
            dr["registration_number"] = row["registration_number"].ToString();
            dr["ato_manager"] = row["ato_manager"].ToString();
            dr["isNonHandled"] = row["isNonHandled"].ToString();
            dr["isOutBusiness"] = row["isOutBusiness"].ToString();
            dr["isNotSendFax"] = row["isNotSendFax"].ToString();
            dr["isTrading"] = row["isTrading"].ToString();
            dr["isPotential1"] = row["isPotential1"].ToString();
            dr["isPotential2"] = row["isPotential2"].ToString();
            dr["table_index"] = row["table_index"].ToString();
            //카테고리
            bool isDelete;
            if (!bool.TryParse(row["isDelete"].ToString(), out isDelete))
                isDelete = false;
            bool isNonHandled;
            if (!bool.TryParse(row["isNonHandled"].ToString(), out isNonHandled))
                isNonHandled = false;
            bool isOutBusiness;
            if (!bool.TryParse(row["isOutBusiness"].ToString(), out isOutBusiness))
                isOutBusiness = false;
            bool isNotSendFax;
            if (!bool.TryParse(row["isNotSendFax"].ToString(), out isNotSendFax))
                isNotSendFax = false;
            bool isTrading;
            if (!bool.TryParse(row["isTrading"].ToString(), out isTrading))
                isTrading = false;
            bool isPotential1;
            if (!bool.TryParse(row["isPotential1"].ToString(), out isPotential1))
                isPotential1 = false;
            bool isPotential2;
            if (!bool.TryParse(row["isPotential2"].ToString(), out isPotential2))
                isPotential2 = false;

            string division;
            if (isDelete)
                division = "삭제거래처";
            else if (isNonHandled)
                division = "취급X";
            else if (isOutBusiness)
                division = "폐업";
            else if (isTrading)
                division = "거래중";
            else if (string.IsNullOrEmpty(row["ato_manager"].ToString()))
                division = "공용DATA";
            else if (isPotential1)
                division = "잠재1";
            else if (isPotential2)
                division = "잠재2";
            else
                division = "내DATA";

            dr["division"] = division;

            return dr;
        }
        #endregion

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

            DataColumn col30 = new DataColumn();
            col30.DataType = System.Type.GetType("System.Int32");
            col30.AllowDBNull = false;
            col30.ColumnName = "table_div";
            col30.Caption = "table_div";
            col30.DefaultValue = 0;
            dt.Columns.Add(col30);

            DataColumn col31 = new DataColumn();
            col31.DataType = System.Type.GetType("System.Int32");
            col31.AllowDBNull = false;
            col31.ColumnName = "table_index";
            col31.Caption = "table_index";
            col31.DefaultValue = 0;
            dt.Columns.Add(col31);

            return dt;
        }
        private void GetData()
        {
            //데이터 출력====================================================================================
            //매출처 정보
            DataTable companyDt = salesPartnerRepository.GetSalesPartner("", "", false, "", "", "", " ", true, false, false, false, false, false, false, false, "");

            //씨오버 거래처 
            DataTable seaoverDt = salesRepository.GetSaleCompany("", false, "", "", "", "", "", "", true, true);

            //아토 전산에만 저장된 거래처정보
            DataRow[] atoCompanyDt = companyDt.Select("seaover_company_code = '' AND isDelete = 'FALSE'");
            DataTable noSeaoverDt = new DataTable();
            if (atoCompanyDt.Length > 0)
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
                                  other_phone = p.Field<string>("기타연락처"),
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
                                  current_sale_date = p.Field<string>("최종매출일자"),
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
            DataTable seaoverDt1 = common.ConvertListToDatatable(seaoverTemp);


            //초기화
            mainDt = SetTable();
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
                    if (isOutput && (isNonHandled || isOutBusiness))
                        isOutput = false;

                    //소송거래처 제외
                    if (isOutput && (noSeaoverDt.Rows[i]["name"].ToString().Contains("소송") || noSeaoverDt.Rows[i]["name"].ToString().Contains("(S)")))
                        isOutput = false;
                    //거래중 거래처 제외
                    bool isTrading;
                    if (!bool.TryParse(noSeaoverDt.Rows[i]["isTrading"].ToString(), out isTrading))
                        isTrading = false;
                    if (isOutput && isTrading)
                        isOutput = false;

                    //출력
                    if (isOutput)
                    {
                        DataRow row = mainDt.NewRow();
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
                        row["table_div"] = 1;
                        row["table_index"] = i;


                        mainDt.Rows.Add(row);
                    }
                }

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
                            /*if (sale_contents == "씨오버 매출")
                                sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                            sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = seaoverDt1.Rows[i]["sales_remark"].ToString();
                        }


                        //출력여부============================================================================
                        bool isOutput = false;
                        if (string.IsNullOrEmpty(ato_manager))
                            isOutput = true;

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
                        if (isOutput && (isNonHandled || isOutBusiness))
                            isOutput = false;
                        if (isOutput && (seaoverDt1.Rows[i]["company"].ToString().Contains("소송") || seaoverDt1.Rows[i]["company"].ToString().Contains("(S)")))
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
                            DataRow row = mainDt.NewRow();
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
                            row["table_div"] = 2;
                            row["table_index"] = i;

                            mainDt.Rows.Add(row);
                        }
                    }
                }
            }
            mainDt.AcceptChanges();
            lbCommonDataCount.Text = mainDt.Rows.Count.ToString("#,##0");
            lbSelectData.Text = mainDt.Rows.Count.ToString("#,##0");
        }
        public void RefreshTable()
        {
            //매출처 정보
            DataTable companyDt = salesPartnerRepository.GetSalesPartner2("", "", false, ""
                                            , "", "", " ", true, false, false, false, false, false, false, false, "");

            //최근영업내용
            DataTable salesDt = salesPartnerRepository.GetCurrentCompanySales();


            //씨오버 거래정보 + 아토전산 거래처
            var atoTemp = from p in companyDt.AsEnumerable()
                          join t in salesDt.AsEnumerable()
                          on p.Field<Int32>("id") equals t.Field<Int32>("company_id")
                          into outer
                          from t in outer.DefaultIfEmpty()
                          select new
                          {
                              id = p.Field<Int32>("id"),
                              group_name = p.Field<string>("group_name"),
                              company = p.Field<string>("name"),
                              seaover_company_code = p.Field<string>("seaover_company_code"),
                              tel = p.Field<string>("tel"),
                              fax = p.Field<string>("fax"),
                              phone = p.Field<string>("phone"),
                              other_phone = p.Field<string>("other_phone"),
                              ceo = p.Field<string>("ceo"),
                              registration_number = p.Field<string>("registration_number"),
                              address = p.Field<string>("address"),
                              origin = p.Field<string>("origin"),
                              company_manager = p.Field<string>("company_manager"),
                              company_manager_position = p.Field<string>("company_manager_position"),
                              email = p.Field<string>("email"),
                              web = p.Field<string>("web"),
                              sns1 = p.Field<string>("sns1"),
                              sns2 = p.Field<string>("sns2"),
                              sns3 = p.Field<string>("sns3"),

                              isManagement1 = p.Field<string>("isManagement1"),
                              isManagement2 = p.Field<string>("isManagement2"),
                              isManagement3 = p.Field<string>("isManagement3"),
                              isManagement4 = p.Field<string>("isManagement4"),
                              isHide = p.Field<string>("isHide"),
                              createtime = p.Field<string>("createtime"),

                              ato_manager = p.Field<string>("ato_manager"),
                              distribution = p.Field<string>("distribution"),
                              handling_item = p.Field<string>("handling_item"),
                              remark = p.Field<string>("remark"),
                              remark2 = p.Field<string>("remark2"),
                              isPotential1 = p.Field<string>("isPotential1"),
                              isPotential2 = p.Field<string>("isPotential2"),
                              isTrading = p.Field<string>("isTrading"),

                              current_sale_date = "",
                              current_sale_manager = "",

                              isNonHandled = p.Field<string>("isNonHandled"),
                              isOutBusiness = p.Field<string>("isOutBusiness"),
                              isNotSendFax = p.Field<string>("isNotSendFax"),

                              sales_updatetime = (t == null) ? "" : t.Field<string>("updatetime"),
                              sales_edit_user = (t == null) ? "" : t.Field<string>("edit_user"),
                              sales_contents = (t == null) ? "" : t.Field<string>("contents"),
                              sales_log = (t == null) ? "" : t.Field<string>("log"),
                              sales_remark = (t == null) ? "" : t.Field<string>("remark"),
                              division = "ATO",

                              duplicate_common_count = 0,
                              duplicate_myData_count = 0,
                              duplicate_potential1_count = 0,
                              duplicate_potential2_count = 0,
                              duplicate_trading_count = 0,
                              duplicate_nonHandled_count = 0,
                              duplicate_notSendFax_count = 0,
                              duplicate_outBusiness_count = 0,
                              duplicate_result = "",

                              isDelete = p.Field<string>("isDelete"),

                              industry_type = p.Field<string>("industry_type"),
                              industry_type_rank = 0,

                              main_id = 0,
                              sub_id = 0
                          };
            companyDt = ConvertListToDatatable(atoTemp);
            companyDt.AcceptChanges();

            //거래처별 그룹
            DataTable groupDt = companyGroupRepository.GetCompanyGroup();

            //아토 전산에만 저장된 거래처정보
            DataRow[] atoCompanyDt = companyDt.Select("seaover_company_code = ''");
            atoDt = new DataTable();
            if (atoCompanyDt.Length > 0)
                atoDt = atoCompanyDt.CopyToDataTable();

            //씨오버 거래처 
            DataTable seaDt = salesRepository.GetSaleCompany("", false, "", "", "", "", "", "", false, false);
            //씨오버 거래정보 + 아토전산 거래처
            var seaoverTemp = from p in seaDt.AsEnumerable()
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
                                  other_phone = p.Field<string>("기타연락처"),
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

                                  isManagement1 = (t == null) ? "FALSE" : t.Field<string>("isManagement1"),
                                  isManagement2 = (t == null) ? "FALSE" : t.Field<string>("isManagement2"),
                                  isManagement3 = (t == null) ? "FALSE" : t.Field<string>("isManagement3"),
                                  isManagement4 = (t == null) ? "FALSE" : t.Field<string>("isManagement4"),
                                  isHide = (t == null) ? "FALSE" : t.Field<string>("isHide"),
                                  createtime = (t == null) ? DateTime.Now.ToString("yyyy-MM-dd") : t.Field<string>("createtime"),

                                  ato_manager = (t == null) ? p.Field<string>("매출자") : t.Field<string>("ato_manager"),
                                  distribution = (t == null) ? "" : t.Field<string>("distribution"),
                                  handling_item = (t == null) ? "" : t.Field<string>("handling_item"),
                                  remark = p.Field<string>("참고사항"),
                                  remark2 = (t == null) ? "" : t.Field<string>("remark2"),
                                  isPotential1 = (t == null) ? "FALSE" : t.Field<string>("isPotential1"),
                                  isPotential2 = (t == null) ? "FALSE" : t.Field<string>("isPotential2"),
                                  isTrading = (t == null) ? "TRUE" : t.Field<string>("isTrading"),
                                  current_sale_date = p.Field<string>("최종매출일자"),
                                  current_sale_manager = p.Field<string>("매출자"),

                                  isNonHandled = (t == null) ? "" : t.Field<string>("isNonHandled"),
                                  isOutBusiness = (t == null) ? p.Field<string>("폐업유무") : t.Field<string>("isOutBusiness"),
                                  isNotSendFax = (t == null) ? "" : t.Field<string>("isNotSendFax"),
                                  sales_updatetime = (t == null) ? "" : t.Field<string>("sales_updatetime"),
                                  sales_edit_user = (t == null) ? "" : t.Field<string>("sales_edit_user"),
                                  sales_contents = (t == null) ? "" : t.Field<string>("sales_contents"),
                                  sales_log = (t == null) ? "" : t.Field<string>("sales_log"),
                                  sales_remark = (t == null) ? "" : t.Field<string>("sales_remark"),
                                  division = "SEAOVER",

                                  duplicate_common_count = 0,
                                  duplicate_myData_count = 0,
                                  duplicate_potential1_count = 0,
                                  duplicate_potential2_count = 0,
                                  duplicate_trading_count = 0,
                                  duplicate_nonHandled_count = 0,
                                  duplicate_notSendFax_count = 0,
                                  duplicate_outBusiness_count = 0,
                                  duplicate_result = "",

                                  isDelete = (t == null) ? "FALSE" : t.Field<string>("isDelete"),

                                  industry_type = (t == null) ? "" : t.Field<string>("industry_type"),
                                  industry_type_rank = 0

                              };
            DataTable seaoverDt = ConvertListToDatatable(seaoverTemp);
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                //그룹별 거래처
                var seaoverTemp2 = from p in seaoverDt.AsEnumerable()
                                   join t in groupDt.AsEnumerable()
                                   on p.Field<string>("seaover_company_code") equals t.Field<string>("company_id")
                                   into outer
                                   from t in outer.DefaultIfEmpty()
                                   select new
                                   {
                                       id = p.Field<Int32>("id"),
                                       group_name = p.Field<string>("group_name"),
                                       company = p.Field<string>("company"),
                                       seaover_company_code = p.Field<string>("seaover_company_code"),
                                       tel = p.Field<string>("tel"),
                                       fax = p.Field<string>("fax"),
                                       phone = p.Field<string>("phone"),
                                       other_phone = p.Field<string>("other_phone"),
                                       ceo = p.Field<string>("ceo"),
                                       registration_number = p.Field<string>("registration_number"),
                                       address = p.Field<string>("address"),

                                       origin = p.Field<string>("origin"),
                                       company_manager = p.Field<string>("company_manager"),
                                       company_manager_position = p.Field<string>("company_manager_position"),
                                       email = p.Field<string>("email"),
                                       web = p.Field<string>("web"),
                                       sns1 = p.Field<string>("sns1"),
                                       sns2 = p.Field<string>("sns2"),
                                       sns3 = p.Field<string>("sns3"),

                                       isManagement1 = p.Field<string>("isManagement1"),
                                       isManagement2 = p.Field<string>("isManagement2"),
                                       isManagement3 = p.Field<string>("isManagement3"),
                                       isManagement4 = p.Field<string>("isManagement4"),
                                       isHide = p.Field<string>("isHide"),
                                       createtime = p.Field<string>("createtime"),

                                       ato_manager = p.Field<string>("ato_manager"),
                                       distribution = p.Field<string>("distribution"),
                                       handling_item = p.Field<string>("handling_item"),
                                       remark = p.Field<string>("remark"),
                                       remark2 = p.Field<string>("remark2"),
                                       isPotential1 = p.Field<string>("isPotential1"),
                                       isPotential2 = p.Field<string>("isPotential2"),
                                       isTrading = p.Field<string>("isTrading"),
                                       current_sale_date = p.Field<string>("current_sale_date"),
                                       current_sale_manager = p.Field<string>("current_sale_manager"),

                                       isNonHandled = p.Field<string>("isNonHandled"),
                                       isOutBusiness = p.Field<string>("isOutBusiness"),
                                       isNotSendFax = p.Field<string>("isNotSendFax"),
                                       sales_updatetime = p.Field<string>("sales_updatetime"),
                                       sales_edit_user = p.Field<string>("sales_edit_user"),
                                       sales_contents = p.Field<string>("sales_contents"),
                                       sales_log = p.Field<string>("sales_log"),
                                       sales_remark = p.Field<string>("sales_remark"),
                                       division = p.Field<string>("division"),

                                       duplicate_common_count = p.Field<Int32>("duplicate_common_count"),
                                       duplicate_myData_count = p.Field<Int32>("duplicate_myData_count"),
                                       duplicate_potential1_count = p.Field<Int32>("duplicate_potential1_count"),
                                       duplicate_potential2_count = p.Field<Int32>("duplicate_potential2_count"),
                                       duplicate_trading_count = p.Field<Int32>("duplicate_trading_count"),
                                       duplicate_nonHandled_count = p.Field<Int32>("duplicate_nonHandled_count"),
                                       duplicate_notSendFax_count = p.Field<Int32>("duplicate_notSendFax_count"),
                                       duplicate_outBusiness_count = p.Field<Int32>("duplicate_outBusiness_count"),
                                       duplicate_result = p.Field<string>("duplicate_result"),

                                       isDelete = p.Field<string>("isDelete"),

                                       industry_type = p.Field<string>("industry_type"),
                                       industry_type_rank = 0,

                                       main_id = (t == null) ? 0 : t.Field<Int32>("main_id"),
                                       sub_id = (t == null) ? 0 : t.Field<Int32>("sub_id")
                                   };
                seaoverDt = ConvertListToDatatable(seaoverTemp2);

                for (int i = 0; i < seaoverDt.Rows.Count; i++)
                {
                    //삭제거래처
                    bool isDelete;
                    if (!bool.TryParse(seaoverDt.Rows[i]["isDelete"].ToString(), out isDelete))
                        isDelete = false;
                    if (!isDelete)
                    {
                        // ** 매출 || 영업중 먼저인 내역이 우선 **

                        //최근 매출정보
                        DateTime current_sale_date;
                        if (!DateTime.TryParse(seaoverDt.Rows[i]["current_sale_date"].ToString(), out current_sale_date))
                            current_sale_date = new DateTime(1900, 1, 1);

                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(seaoverDt.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        //2023-09-13
                        //시간까지 체크하니 테스트가 안됨 그날 매출달면 그냥 바로 거래중으로 적용
                        current_sale_date = new DateTime(current_sale_date.Year, current_sale_date.Month, current_sale_date.Day);
                        sales_updatetime = new DateTime(sales_updatetime.Year, sales_updatetime.Month, sales_updatetime.Day);

                        string sale_edit_user;
                        string sale_date;
                        string sale_contents;
                        string sale_remark;
                        string ato_manager;
                        if (current_sale_date >= sales_updatetime)
                        {
                            seaoverDt.Rows[i]["isTrading"] = "TRUE";
                            ato_manager = seaoverDt.Rows[i]["current_sale_manager"].ToString();
                            sale_edit_user = seaoverDt.Rows[i]["current_sale_manager"].ToString();
                            sale_contents = "씨오버 매출";
                            sale_date = current_sale_date.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = "";
                        }
                        else
                        {
                            ato_manager = seaoverDt.Rows[i]["ato_manager"].ToString();
                            sale_edit_user = seaoverDt.Rows[i]["sales_edit_user"].ToString();
                            sale_contents = seaoverDt.Rows[i]["sales_contents"].ToString();
                            /*if (sale_contents == "씨오버 매출")
                                sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                            sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                            sale_remark = seaoverDt.Rows[i]["sales_remark"].ToString();
                        }
                        //최신화
                        seaoverDt.Rows[i]["ato_manager"] = ato_manager;
                        seaoverDt.Rows[i]["sales_updatetime"] = sale_date;
                        seaoverDt.Rows[i]["sales_edit_user"] = sale_edit_user;
                        seaoverDt.Rows[i]["sales_contents"] = sale_contents;
                        seaoverDt.Rows[i]["sales_remark"] = sale_remark;
                        //거래처코드가 없는데 거래중은 자동삭제
                        bool trading;
                        if (!bool.TryParse(seaoverDt.Rows[i]["isTrading"].ToString(), out trading))
                            trading = false;
                        if (string.IsNullOrEmpty(seaoverDt.Rows[i]["seaover_company_code"].ToString()) && trading)
                            seaoverDt.Rows[i]["isDelete"] = "FALSE";
                    }
                }
                seaoverDt.AcceptChanges();

                //대표거래처 최종수정일 최신화
                for (int i = 0; i < seaoverDt.Rows.Count; i++)
                {
                    //대표거래처 최종수정일 최신화
                    int main_id;
                    if (!int.TryParse(seaoverDt.Rows[i]["main_id"].ToString(), out main_id))
                        main_id = 0;
                    int sub_id;
                    if (!int.TryParse(seaoverDt.Rows[i]["sub_id"].ToString(), out sub_id))
                        sub_id = 0;
                    if (main_id > 0 && sub_id == 0)
                    {
                        //최근 영업정보
                        DateTime sales_updatetime;
                        if (!DateTime.TryParse(seaoverDt.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                            sales_updatetime = new DateTime(1900, 1, 1);

                        string ato_manager = seaoverDt.Rows[i]["ato_manager"].ToString();
                        string sale_edit_user = seaoverDt.Rows[i]["sales_edit_user"].ToString();
                        string sale_contents = seaoverDt.Rows[i]["sales_contents"].ToString();
                        /*if (sale_contents == "씨오버 매출")
                            sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                        string sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                        string sale_remark = seaoverDt.Rows[i]["sales_remark"].ToString();

                        DataRow[] subDr = seaoverDt.Select($"main_id = {main_id} AND isDelete = 'FALSE'");
                        for (int j = 0; j < subDr.Length; j++)
                        {
                            DateTime tempUpdatetime;
                            if (DateTime.TryParse(subDr[j]["sales_updatetime"].ToString(), out tempUpdatetime))
                            {
                                if (sales_updatetime < tempUpdatetime)
                                {
                                    sales_updatetime = tempUpdatetime;
                                    ato_manager = subDr[j]["ato_manager"].ToString();
                                    sale_edit_user = subDr[j]["sales_edit_user"].ToString();
                                    sale_contents = subDr[j]["sales_contents"].ToString();
                                    /*if (sale_contents == "씨오버 매출")
                                        sale_contents = seaoverDt1.Rows[i]["sales_log"].ToString();*/
                                    sale_date = sales_updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                                    sale_remark = subDr[j]["sales_remark"].ToString();
                                }
                            }
                        }
                        //최신화
                        seaoverDt.Rows[i]["ato_manager"] = ato_manager;
                        seaoverDt.Rows[i]["sales_updatetime"] = sale_date;
                        seaoverDt.Rows[i]["sales_edit_user"] = sale_edit_user;
                        seaoverDt.Rows[i]["sales_contents"] = sale_contents;
                        seaoverDt.Rows[i]["sales_remark"] = sale_remark;

                    }
                }

            }
            seaoverDt.AcceptChanges();
            atoDt.Merge(seaoverDt);
            atoDt.Columns.Add("table_index", typeof(Int32));
            atoDt.AcceptChanges();
            for (int i = 0; i < atoDt.Rows.Count; i++)
                atoDt.Rows[i]["table_index"] = i;

            //초기화
            mainDt = new DataTable();
            DataRow[] mainDr = atoDt.Select("ato_manager = '' AND isNonHandled = 'FALSE' AND isOutBusiness = 'FALSE' AND isDelete = 'FALSE' AND isHide = 'FALSE'");
            if (mainDr.Length > 0)
                mainDt = mainDr.CopyToDataTable();

        }
        public void GetDepartment()
        {
            cbDepartment.Items.Clear();
            DataTable departmentDt = departmentRepository.GetDepartment("", "");
            if (departmentDt.Rows.Count > 0)
            {
                for (int i = 0; i < departmentDt.Rows.Count; i++)
                    cbDepartment.Items.Add(departmentDt.Rows[i]["name"].ToString());
            }
        }
        private void GetEmployee()
        {
            dgvEmplyee.Rows.Clear();

            DataTable employeeDt = usersRepository.GetUsers("", cbDepartment.Text, txtTeam.Text, txtEmplyeeName.Text, "승인", "");
            if (employeeDt.Rows.Count > 0)
            {
                for (int i = 0; i < employeeDt.Rows.Count; i++)
                {
                    int n = dgvEmplyee.Rows.Add();
                    dgvEmplyee.Rows[n].Cells["user_id"].Value = employeeDt.Rows[i]["user_id"].ToString();
                    dgvEmplyee.Rows[n].Cells["user_name"].Value = employeeDt.Rows[i]["user_name"].ToString();
                    dgvEmplyee.Rows[n].Cells["department"].Value = employeeDt.Rows[i]["department"].ToString();
                    dgvEmplyee.Rows[n].Cells["team"].Value = employeeDt.Rows[i]["team"].ToString();
                }
            }

            GetCompanyCount();
        }

        #endregion

        #region 사용자 추가, 삭제
        private void dgvEmplyee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvEmplyee.Columns[e.ColumnIndex].Name == "btnAdd")
                {
                    DataGridViewRow row = dgvEmplyee.Rows[e.RowIndex];
                    string user_id = row.Cells["user_id"].Value.ToString();

                    for (int i = 0; i < dgvSelectEmployee.Rows.Count; i++)
                    {
                        if (dgvSelectEmployee.Rows[i].Cells["select_user_id"].Value.ToString().Equals(user_id))
                        {
                            messageBox.Show(this,"이미 선택내역에 존재하는 사용자입니다.", "오류");
                            this.Activate();
                            return; 
                        }
                    }

                    int n = dgvSelectEmployee.Rows.Add();
                    dgvSelectEmployee.Rows[n].Cells["select_user_id"].Value = user_id;
                    dgvSelectEmployee.Rows[n].Cells["select_department"].Value = row.Cells["department"].Value;
                    dgvSelectEmployee.Rows[n].Cells["select_user_name"].Value = row.Cells["user_name"].Value;
                    dgvSelectEmployee.Rows[n].Cells["select_team"].Value = row.Cells["team"].Value;
                }
            }
        }

        private void dgvSelectEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSelectEmployee.Columns[e.ColumnIndex].Name == "btnDelete")
                {

                    DataGridViewRow row = dgvSelectEmployee.Rows[e.RowIndex];
                    dgvSelectEmployee.Rows.Remove(row);
                }
            }
        }
        #endregion

        #region 분배 event
        private void dgvSelectEmployee_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvSelectEmployee.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelectEmployee_CellValueChanged);
                dgvSelectEmployee.EndEdit();
                if (dgvSelectEmployee.Columns[e.ColumnIndex].Name == "distribution_cnt")
                {
                    double total_data;
                    if (!double.TryParse(lbCommonDataCount.Text, out total_data))
                        total_data = 0;

                    //총 직원수
                    //txtTotalEmployeeCount.Text = dgvSelectEmployee.Rows.Count.ToString("#,##0");
                    lbTotalEmployeeCount.Text = dgvSelectEmployee.Rows.Count.ToString("#,##0");
                    //총수량
                    double total_count = 0;
                    for (int i = 0; i < dgvSelectEmployee.Rows.Count; i++)
                    {
                        double count;
                        if (dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value == null || !double.TryParse(dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value.ToString(), out count))
                            count = 0;

                        total_count += count;
                    }
                    txtTotalDistributionCount.Text = total_count.ToString("#,##0");

                    //분베율
                    double total_rate = 0;
                    for (int i = 0; i < dgvSelectEmployee.Rows.Count; i++)
                    {
                        double count;
                        if (dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value == null || !double.TryParse(dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value.ToString(), out count))
                            count = 0;

                        dgvSelectEmployee.Rows[i].Cells["distribution_rate"].Value = (count / total_count * 100).ToString("#,##0.00");
                        total_rate += (count / total_count * 100);
                    }

                    txtTotalRate.Text = total_rate.ToString("#,##0.00");

                    /*if (total_data < total_count)
                        messageBox.Show(this,"총 공용DATA 수량보다 입력한 수량이 많습니다.");*/
                }
                else if (dgvSelectEmployee.Columns[e.ColumnIndex].Name == "distribution_rate")
                {
                    double total_data;
                    if (!double.TryParse(lbCommonDataCount.Text, out total_data))
                        total_data = 0;

                    double total_count = 0;
                    double total_rate = 0;
                    for (int i = 0; i < dgvSelectEmployee.Rows.Count; i++)
                    {
                        double rate;
                        if (dgvSelectEmployee.Rows[i].Cells["distribution_rate"].Value == null || !double.TryParse(dgvSelectEmployee.Rows[i].Cells["distribution_rate"].Value.ToString(), out rate))
                            rate = 0;

                        dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value = Math.Truncate(total_data * (rate / 100)).ToString("#,##0");

                        total_count += Math.Truncate(total_data * (rate / 100));
                        total_rate += rate;
                    }
                    txtTotalRate.Text = total_rate.ToString("#,##0.00");
                    txtTotalDistributionCount.Text = total_count.ToString("#,##0");
                }

                this.dgvSelectEmployee.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelectEmployee_CellValueChanged);
            }
        }
        #endregion

        #region Key event
        private void cbDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetEmployee();
        }
        private void DataDistribution_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
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

        #region Button, Combox
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnCheckToggle_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0)
            {
                bool isCheck = !Convert.ToBoolean(dgvData.Rows[0].Cells["chk"].Value.ToString());
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    dgvData.Rows[i].Cells["chk"].Value = isCheck;
                }
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            dgvSelectEmployee.EndEdit();

            double total_data;
            if (!double.TryParse(lbSelectData.Text, out total_data))
                total_data = 0;
            //총수량
            double total_count = 0;
            for (int i = 0; i < dgvSelectEmployee.Rows.Count; i++)
            {
                double count;
                if (dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value == null || !double.TryParse(dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value.ToString(), out count))
                    count = 0;

                total_count += count;
            }
            //유효성검사
            if (total_data < total_count)
            {
                messageBox.Show(this, "총 공용DATA 수량보다 입력한 수량이 많습니다.");
                this.Activate();
                return;
            }

            //Msg
            if (messageBox.Show(this, "데이터를 분배하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;


            //수정
            Random ran = new Random();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                bool isChecked = Convert.ToBoolean(dgvData.Rows[i].Cells["chk"].Value);
                int company_id = Convert.ToInt32(dgvData.Rows[i].Cells["company_id"].Value);
                if (isChecked)
                {
                    if (dgvData.Rows[i].Cells["ato_manager"].Value == null || string.IsNullOrEmpty(dgvData.Rows[i].Cells["ato_manager"].Value.ToString()))
                    {
                        dgvData.CurrentCell = dgvData.Rows[i].Cells["ato_manager"];
                        messageBox.Show(this, "분배되지 않은 거래처가 있습니다.");
                        this.Activate();
                        return;
                    }

                    //거래처 담당자 할당
                    sql = companyRepository.UpdateAtoManger(company_id, dgvData.Rows[i].Cells["ato_manager"].Value.ToString(), um.user_name);
                    sqlList.Add(sql);
                    //이력 저장
                    CompanySalesModel sModel = new CompanySalesModel();
                    sModel.company_id = company_id;
                    sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", company_id.ToString());
                    sModel.is_sales = false;
                    sModel.contents = "공용DATA -> 내DATA 분배";
                    sModel.log = " | 공용DATA";
                    sModel.remark = "";
                    sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sModel.edit_user = um.user_name;

                    sModel.from_ato_manager = "";
                    sModel.to_ato_manager = dgvData.Rows[i].Cells["ato_manager"].Value.ToString();
                    sModel.from_category = "공용DATA";
                    sModel.to_category = "내DATA";

                    sql = salesPartnerRepository.InsertPartnerSales(sModel);
                    sqlList.Add(sql);
                }
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
                    //Table 수정
                    DataTable atoDt = sm.GetAtoDt();
                    for (int i = dgvData.Rows.Count - 1; i >= 0; i--)
                    {
                        DataGridViewRow row = dgvData.Rows[i];
                        bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                        if (isChecked)
                        {
                            int company_id = Convert.ToInt32(row.Cells["company_id"].Value);

                            DataRow[] atoDr = atoDt.Select($"id = {company_id}");
                            if (atoDr.Length > 0)
                            {   
                                int idx = Convert.ToInt32(atoDr[0]["table_index"].ToString());
                                atoDt.Rows[idx]["ato_manager"] = row.Cells["ato_manager"].Value.ToString();
                                atoDt.Rows[idx]["isTrading"] = "false";
                                atoDt.Rows[idx]["isPotential1"] = "false";
                                atoDt.Rows[idx]["isPotential2"] = "false";
                                atoDt.Rows[idx]["isNonHandled"] = "false";
                                atoDt.Rows[idx]["isNotSendFax"] = "false";
                                atoDt.Rows[idx]["isOutBusiness"] = "false";

                                atoDt.Rows[idx]["sales_contents"] = "공용DATA -> 내DATA 전환";
                                atoDt.Rows[idx]["sales_updatetime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                atoDt.Rows[idx]["sales_edit_user"] = um.user_name.ToString();
                            }                            
                            dgvData.Rows.Remove(row);
                        }
                    }
                    sm.SetAtoDt(atoDt);
                    messageBox.Show(this,"완료");
                    this.Activate();
                }       
            }
        }
        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEmployee();
        }
        private void btnAllDelete_Click(object sender, EventArgs e)
        {
            dgvSelectEmployee.Rows.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSelectEmployee.Rows.Count; i++)
            {
                dgvSelectEmployee.Rows[i].Cells["distribution_cnt"].Value = 0;
            }
        }

        private void btnEvenDistribution_Click(object sender, EventArgs e)
        {
            //GetCount();
            int dataCnt;
            if (!int.TryParse(lbSelectData.Text.Replace(",", ""), out dataCnt))
                dataCnt = 0;
            if (dataCnt == 0)
            {
                messageBox.Show(this, "분배할 데이터가 없습니다.");
                this.Activate();
                return;
            }    

            if(dgvSelectEmployee.Rows.Count == 0)
            {
                messageBox.Show(this, "분배할 사용자가 없습니다.");
                this.Activate();
                return;
            }
            this.dgvData.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellValueChanged);

            //분배
            int ramNum = 0;
            int[] ramArr = new int[dgvSelectEmployee.Rows.Count];
            for (int i = 0; i < dgvSelectEmployee.Rows.Count; i++)
                ramArr[i] = i;
            Random randomObj = new Random();
            ramArr = ramArr.OrderBy(XmlReadMode => randomObj.Next()).ToArray();

            //순도까지 확인한 분배
            List<string> rankList = new List<string>();
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (!rankList.Contains(row.Cells["industry_type_rank"].Value.ToString()))
                    rankList.Add(row.Cells["industry_type_rank"].Value.ToString());
            }


            //이미 존재해서 다시해야하는 거래처
            List<DataGridViewRow> retryList = new List<DataGridViewRow>();
            List<DataGridViewRow> fullExistList = new List<DataGridViewRow>();
            foreach (string rank in rankList)
            {
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvData.Rows[i].Cells["chk"].Value) && dgvData.Rows[i].Cells["industry_type_rank"].Value.ToString().Equals(rank))
                    {
                        //다시
                        if (ramNum == dgvSelectEmployee.Rows.Count)
                            ramNum = 0;
                        //사용자 분배
                        if (dgvData.Rows[i].Cells["exist_ato_manager"].Value != null && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["exist_ato_manager"].Value.ToString()))
                        {
                            int duplicate_cnt = 0;
                            foreach (DataGridViewRow row in dgvSelectEmployee.Rows)
                            {
                                if (dgvData.Rows[i].Cells["exist_ato_manager"].Value.ToString().Contains(row.Cells["select_user_name"].Value.ToString()))
                                    duplicate_cnt++;
                            }

                            if (duplicate_cnt > 0)
                            {
                                if (dgvSelectEmployee.Rows.Count == duplicate_cnt)
                                    fullExistList.Add(dgvData.Rows[i]);
                                else
                                    retryList.Add(dgvData.Rows[i]);
                            }
                            else
                            {
                                dgvData.Rows[i].Cells["ato_manager"].Value = dgvSelectEmployee.Rows[ramArr[ramNum]].Cells["select_user_name"].Value.ToString();
                                ramNum++;
                            }
                        }
                        else
                        {
                            dgvData.Rows[i].Cells["ato_manager"].Value = dgvSelectEmployee.Rows[ramArr[ramNum]].Cells["select_user_name"].Value.ToString();
                            ramNum++;
                        }
                    }
                }
                //기존 담당자가 선택담당자와 겹처 재분배
                if (retryList.Count > 0)
                {
                retry:
                    List<DataGridViewRow> reretryList = new List<DataGridViewRow>(retryList.Count);
                    List<string> userList = new List<string>(dgvSelectEmployee.Rows.Count);
                    foreach (DataGridViewRow row in retryList)
                    {
                        bool isCancle = false;
                        if (userList.Count == dgvSelectEmployee.Rows.Count)
                            userList = new List<string>(dgvSelectEmployee.Rows.Count);

                        int idx = 0;
                        string user_name = dgvSelectEmployee.Rows[idx].Cells["select_user_name"].Value.ToString();
                        while (!isCancle && (userList.Contains(user_name) || row.Cells["exist_ato_manager"].Value.ToString().Contains(user_name)))
                        {
                            idx++;
                            if (userList.Count == dgvSelectEmployee.Rows.Count || idx == dgvSelectEmployee.Rows.Count)
                                isCancle = true;
                            else
                                user_name = dgvSelectEmployee.Rows[idx].Cells["select_user_name"].Value.ToString();
                        }

                        if (!isCancle)
                        {
                            row.Cells["ato_manager"].Value = user_name;
                            userList.Add(user_name);
                        }
                        else
                            reretryList.Add(row);
                    }

                    if (reretryList.Count > 0)
                    {
                        retryList = reretryList;
                        goto retry;
                    }

                }
            }


            

            this.dgvData.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellValueChanged);
            GetCount();

            if (fullExistList.Count > 0)
            {
                foreach (DataGridViewRow row in fullExistList)
                {
                    row.HeaderCell.Style.BackColor = Color.Red;
                }
                dgvData.FirstDisplayedScrollingRowIndex = fullExistList[0].Index;
                messageBox.Show(this,"기존 담당자가 모두 존재하여 분배되지 않은 거래처수가 " + retryList.Count + "개 발견되었습니다!");
                this.Activate();
            }
            else
            {
                messageBox.Show(this,"분배완료");
                this.Activate();
            }        
        }
        #endregion

        #region 담당자별 거래처 Datatable
        private void GetCompanyCount()
        {
            //매출처 정보
            DataTable companyDt = salesPartnerRepository.GetSalesPartner("", "", false, "", "", "", " ", true, false, false, false, false, false, false, false, "");

            //씨오버 거래처 
            DataTable seaoverDt = salesRepository.GetSaleCompany("", false, "", "", "", "", "", "", true, true);

            //아토 전산에만 저장된 거래처정보
            DataRow[] atoCompanyDt = companyDt.Select("seaover_company_code = '' AND isDelete = 'FALSE'");
            DataTable noSeaoverDt = new DataTable();
            if (atoCompanyDt.Length > 0)
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
                                  current_sale_date = p.Field<string>("최종매출일자"),
                                  current_sale_manager = p.Field<string>("매출자"),

                                  isNonHandled = (t == null) ? "false" : t.Field<string>("isNonHandled"),
                                  isOutBusiness = (t == null) ? p.Field<string>("폐업유무") : t.Field<string>("isOutBusiness"),
                                  isNotSendFax = (t == null) ? "false" : t.Field<string>("isNotSendFax"),
                                  sales_updatetime = (t == null) ? "" : t.Field<string>("sales_updatetime"),
                                  sales_edit_user = (t == null) ? "" : t.Field<string>("sales_edit_user"),
                                  sales_contents = (t == null) ? "" : t.Field<string>("sales_contents"),
                                  sales_log = (t == null) ? "" : t.Field<string>("sales_log"),
                                  sales_remark = (t == null) ? "" : t.Field<string>("sales_remark"),

                                  isDelete = (t == null) ? "FALSE" : t.Field<string>("isDelete")
                              };
            DataTable seaoverDt1 = ConvertListToDatatable(seaoverTemp);
            for (int i = 0; i < seaoverDt1.Rows.Count; i++)
            {
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

                seaoverDt1.Rows[i]["sales_edit_user"] = sale_edit_user;
                //seaoverDt1.Rows[i]["sales_date"] = sale_date;
                //seaoverDt1.Rows[i]["sales_contents"] = sale_contents;
                //seaoverDt1.Rows[i]["sales_remark"] = sale_remark;
                seaoverDt1.Rows[i]["ato_manager"] = ato_manager;
            }
            seaoverDt1.AcceptChanges();


            //출력
            string whr;
            DataRow[] noSeaoverDr, seaoverDr;
            for (int i = 0; i < dgvEmplyee.Rows.Count; i++)
            {
                //무작위(아토)=======================================================================================================
                int random_count = 0;
                if (noSeaoverDt != null && noSeaoverDt.Rows.Count > 0)
                {
                    whr = "isPotential1 = 'false' AND isPotential2 = 'false' "
                        + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                        + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";
                    noSeaoverDr = noSeaoverDt.Select(whr);
                    random_count += noSeaoverDr.Length;
                }

                //무작위(씨오버)
                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    whr = "isDelete = 'false'                                       "
                         + "  AND isPotential1 = 'false' AND isPotential2 = 'false' "
                         + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                         + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";

                    seaoverDr = seaoverDt1.Select(whr);
                    random_count += seaoverDr.Length;
                }
                dgvEmplyee.Rows[i].Cells["random_count"].Value = random_count.ToString("#,##0");

                //잠재1(아토)=======================================================================================================
                int potential1_count = 0;
                if (noSeaoverDt != null && noSeaoverDt.Rows.Count > 0)
                {
                    whr = "isDelete = 'false' AND seaover_company_code = '' "
                        + "  AND isPotential1 = 'true' AND isPotential2 = 'false' "
                        + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                        + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";
                    noSeaoverDr = noSeaoverDt.Select(whr);
                    potential1_count += noSeaoverDr.Length;
                }

                //잠재1(씨오버)
                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    whr = "isDelete = 'false'  "
                         + "  AND isPotential1 = 'true' AND isPotential2 = 'false' "
                         + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                         + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";

                    seaoverDr = seaoverDt1.Select(whr);
                    potential1_count += seaoverDr.Length;
                }
                dgvEmplyee.Rows[i].Cells["potential1_count"].Value = potential1_count.ToString("#,##0");

                //잠재2(아토)=======================================================================================================
                int potential2_count = 0;
                if (noSeaoverDt != null && noSeaoverDt.Rows.Count > 0)
                {
                    whr = "isDelete = 'false' AND seaover_company_code = '' "
                        + "  AND isPotential1 = 'false' AND isPotential2 = 'true' "
                        + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                        + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";
                    noSeaoverDr = noSeaoverDt.Select(whr);
                    potential2_count += noSeaoverDr.Length;
                }

                //잠재2(씨오버)
                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    whr = "isDelete = 'false' "
                         + "  AND isPotential1 = 'false' AND isPotential2 = 'true' "
                         + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                         + $" AND isTrading = 'false' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";

                    seaoverDr = seaoverDt1.Select(whr);
                    potential2_count += seaoverDr.Length;
                }
                dgvEmplyee.Rows[i].Cells["potential2_count"].Value = potential2_count.ToString("#,##0");

                //거래중(씨오버)=======================================================================================================
                int trading_count = 0;
                if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                {
                    whr = "isDelete = 'false'  "
                         + "  AND isNonHandled = 'false' AND isOutBusiness = 'false' AND isNotSendFax = 'false' "
                         + $" AND isTrading = 'true' AND ato_manager = '{dgvEmplyee.Rows[i].Cells["user_name"].Value.ToString()}'";

                    seaoverDr = seaoverDt1.Select(whr);
                    trading_count += seaoverDr.Length;
                }
                dgvEmplyee.Rows[i].Cells["trading_count"].Value = trading_count.ToString("#,##0");
            }

        }
        /*public static DataTable ConvertListToDatatable(IEnumerable<dynamic> v)
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
        }*/
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
        #endregion


        #region Datagrieview event
        private void dgvData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvData.Columns[e.ColumnIndex].Name == "chk" || dgvData.Columns[e.ColumnIndex].Name == "ato_manager")
                    GetCount();
            }
        }
        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            GetCount();
        }


        #endregion

        ContextMenuStrip m;
        private void dgvData_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    if (dgvData.SelectedRows.Count == 0)
                        return;
                    //한영변환
                    //ChangeIME(false);

                    hitTestInfo = dgvData.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    m = new ContextMenuStrip();
                    m.Items.Add("선택/해제");


                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvData, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch (Exception ex)
            {
                messageBox.Show(this, ex.Message);
                this.Activate();
            }
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvData.SelectedRows[0];
                    if (dr.Index < 0)
                        return;
                    int rowindex = dr.Index;

                    //함수호출
                    switch (e.ClickedItem.Text)
                    {
                        case "선택/해제":
                            {
                                bool isChecked;
                                if (dr.Cells["chk"].Value == null || !bool.TryParse(dr.Cells["chk"].Value.ToString(), out isChecked))
                                    isChecked = false;

                                foreach (DataGridViewRow row in dgvData.SelectedRows)
                                    row.Cells["chk"].Value = !isChecked;
                            }
                            break;
                        default:
                            break;

                    }
                }
                catch
                { }
            }
        }

        private void dgvData_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && dgvData.SelectedRows.Count <= 1)
                {
                    dgvData.ClearSelection();
                    dgvData.Rows[e.RowIndex].Selected = true;
                }
            }
        }
    }
}
