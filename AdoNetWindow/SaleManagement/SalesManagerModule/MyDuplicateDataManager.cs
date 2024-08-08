using AdoNetWindow.Model;
using Repositories;
using Repositories.Company;
using Repositories.SalesPartner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement
{
    public partial class MyDuplicateDataManager : Form
    {
        ICompanyRepository companyRepository = new CompanyRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ICompanyGroupRepository companyGroupRepository = new CompanyGroupRepository();
        UsersModel um;
        DataTable atoDt = null;
        DataTable companyDt = null;
        SalesManager sm = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public MyDuplicateDataManager(UsersModel um, SalesManager sm, DataTable atoDt)
        {
            InitializeComponent();
            this.atoDt = atoDt.Copy(); 
            this.um = um;
            this.sm = sm;
            txtAtoManager.Text = um.user_name;
        }
        public int GetDulicate()
        {
            int n = GetData();
            
            return n;
        }
        private void MyDuplicateDataManager_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //int n = GetData();

            if (um.auth_level < 90)
            {
                txtAtoManager.Enabled = false;
                cbCommonData.Enabled = false;
            }
        }

        private DataTable SetData()
        {
            DataTable tempDt = atoDt.Clone();
            tempDt.Rows.Clear();

            if (atoDt != null && atoDt.Rows.Count > 0)
            {
                string whr = $"main_id = 0 ";
                DataRow[] atoDr = atoDt.Select(whr);
                if (atoDr.Length > 0)
                {
                    foreach (DataRow dr in atoDr)
                    {
                        bool isOutput = true;

                        //영업금지 거래처
                        bool isNonHandled, isNotSendFax, isOutBusiness;
                        if (!bool.TryParse(dr["isNonHandled"].ToString(), out isNonHandled))
                            isNonHandled = false;
                        if (!bool.TryParse(dr["isNotSendFax"].ToString(), out isNotSendFax))
                            isNotSendFax = false;
                        if (!bool.TryParse(dr["isOutBusiness"].ToString(), out isOutBusiness))
                            isOutBusiness = false;

                        //거래중
                        bool isTrading;
                        if (!bool.TryParse(dr["isTrading"].ToString(), out isTrading))
                            isTrading = false;

                        //잠재1, 잠재2
                        bool isPotential1, isPotential2;
                        if (!bool.TryParse(dr["isPotential1"].ToString(), out isPotential1))
                            isPotential1 = false;
                        if (!bool.TryParse(dr["isPotential2"].ToString(), out isPotential2))
                            isPotential2 = false;

                        string division;
                        if (string.IsNullOrEmpty(dr["ato_manager"].ToString().Trim()))
                            division = "공용DATA";
                        else if (isTrading)
                            division = "거래중";
                        else if (isPotential1)
                            division = "잠재1";
                        else if (isPotential2)
                            division = "잠재2";
                        else
                            division = "내DATA";

                        //출력여부============================================================================
                        if (isNonHandled || isOutBusiness)
                            isOutput = false;

                        //거래중
                        bool isDelete;
                        if (!bool.TryParse(dr["isDelete"].ToString(), out isDelete))
                            isDelete = false;
                        if (isOutput)
                            isOutput = !isDelete;

                        //숨김거래처
                        bool isHide;
                        if (!bool.TryParse(dr["isHide"].ToString(), out isHide))
                            isHide = false;
                        if (isOutput)
                            isOutput = !isHide;

                        //서브거래처
                        int sub_id;
                        if (!int.TryParse(dr["sub_id"].ToString(), out sub_id))
                            sub_id = 0;
                        if (sub_id == 1)
                            isOutput = false;

                        //탭별검색
                        string select_tab = "";
                        if (cbCommonData.Checked)
                            select_tab += " 공용DATA";
                        if (cbMydata.Checked)
                            select_tab += " 내DATA";
                        if (cbPotential1.Checked)
                            select_tab += " 잠재1";
                        if (cbPotential2.Checked)
                            select_tab += " 잠재2";
                        if (cbTrading.Checked)
                            select_tab += " 거래중";
                        if (isOutput && !select_tab.Contains(division))
                            isOutput = false;

                        //담당자 검색
                        if (isOutput && division != "공용DATA" && !string.IsNullOrEmpty(txtAtoManager.Text.Trim()))
                        {
                            string[] managers = txtAtoManager.Text.Trim().Split(' ');
                            if (managers.Length > 0)
                            {
                                isOutput = false;
                                for (int j = 0; j < managers.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(managers[j].Trim()) && dr["ato_manager"].ToString().Contains(managers[j]))
                                    {
                                        isOutput = true;
                                        break;
                                    }
                                }
                            }
                        }

                        //구분 검색
                        if (isOutput && !string.IsNullOrEmpty(txtDivision.Text.Trim()))
                        {
                            string[] divisions = txtDivision.Text.Trim().Split(' ');
                            if (divisions.Length > 0)
                            {
                                isOutput = false;
                                for (int j = 0; j < divisions.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(divisions[j].Trim()) && division.Contains(divisions[j]))
                                    {
                                        isOutput = true;
                                        break;
                                    }
                                }
                            }
                        }


                        //출력
                        if (isOutput)
                        {
                            DataRow ddr = tempDt.NewRow();
                            for (int j = 0; j < atoDt.Columns.Count; j++)
                                ddr[j] = dr[j];
                            ddr["division"] = division;
                            ddr["table_index"] = dr["table_index"].ToString();
                            tempDt.Rows.Add(ddr);
                        }
                    }
                }
            }

            return tempDt;
        }
        private int GetData()
        {
            companyDt = SetData();
            dgvData.Rows.Clear();
            if (companyDt != null && companyDt.Rows.Count > 0)
            {
                for (int i = 0; i < companyDt.Rows.Count; i++)
                {
                    int n = dgvData.Rows.Add();
                    dgvData.Rows[n].Cells["company_id"].Value = companyDt.Rows[i]["id"].ToString();
                    dgvData.Rows[n].Cells["seaover_company_code"].Value = companyDt.Rows[i]["seaover_company_code"].ToString();
                    dgvData.Rows[n].Cells["division"].Value = companyDt.Rows[i]["division"].ToString();
                    dgvData.Rows[n].Cells["company"].Value = companyDt.Rows[i]["company"].ToString();
                    dgvData.Rows[n].Cells["registration_number"].Value = companyDt.Rows[i]["registration_number"].ToString();
                    dgvData.Rows[n].Cells["ceo"].Value = companyDt.Rows[i]["ceo"].ToString();
                    dgvData.Rows[n].Cells["tel"].Value = companyDt.Rows[i]["tel"].ToString();
                    dgvData.Rows[n].Cells["fax"].Value = companyDt.Rows[i]["fax"].ToString();
                    dgvData.Rows[n].Cells["phone"].Value = companyDt.Rows[i]["phone"].ToString();
                    dgvData.Rows[n].Cells["other_phone"].Value = companyDt.Rows[i]["other_phone"].ToString();
                    dgvData.Rows[n].Cells["ato_manager"].Value = companyDt.Rows[i]["ato_manager"].ToString();
                    dgvData.Rows[n].Cells["table_index"].Value = companyDt.Rows[i]["table_index"].ToString();
                    dgvData.Rows[n].Cells["sales_updatetime"].Value = companyDt.Rows[i]["current_sale_date"].ToString();
                    dgvData.Rows[n].Cells["sales_remark"].Value = companyDt.Rows[i]["sales_remark"].ToString();
                    dgvData.Rows[n].Cells["sales_edit_user"].Value = companyDt.Rows[i]["sales_edit_user"].ToString();
                    dgvData.Rows[n].Cells["sales_contents"].Value = companyDt.Rows[i]["sales_contents"].ToString();

                    DateTime sales_updatetime;
                    if(DateTime.TryParse(companyDt.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                        dgvData.Rows[n].Cells["sales_updatetime"].Value = sales_updatetime.ToString("yyyy-MM-dd");

                }
            }
            return CheckDuplicateAsOne2();
        }

        Dictionary<int, List<DataRow>> duplicateDic = new Dictionary<int, List<DataRow>>();
        Libs.Tools.Common common = new Libs.Tools.Common();
        private int CheckDuplicateAsOne2()
        {
            int duplicate_id = 0;
            duplicateDic = new Dictionary<int, List<DataRow>>();
            if (dgvData.Rows.Count > 0 && companyDt != null && companyDt.Rows.Count > 0)
            {
                //중복ID 초기화
                for (int i = 0; i < dgvData.Rows.Count; i++)
                    dgvData.Rows[i].Cells["duplicate_id"].Value = string.Empty;

                //입력데이터 -> Datatable
                DataTable inputDt = common.ConvertDgvToDataTable(dgvData);
                inputDt.Columns.Add("rowIndex1", typeof(Int32));
                for (int i = 0; i < inputDt.Rows.Count; i++)
                {
                    inputDt.Rows[i]["rowindex1"] = i;
                    int division_int = 0;
                    if (!string.IsNullOrEmpty(inputDt.Rows[i]["seaover_company_code"].ToString()))
                        division_int = 9;
                    else if (inputDt.Rows[i]["division"].ToString() == "거래중" && !string.IsNullOrEmpty(inputDt.Rows[i]["seaover_company_code"].ToString()))
                        division_int = 8;
                    else if (inputDt.Rows[i]["division"].ToString() == "거래중" && string.IsNullOrEmpty(inputDt.Rows[i]["seaover_company_code"].ToString()))
                        division_int= 7;
                    else if (inputDt.Rows[i]["division"].ToString() == "잠재1")
                        division_int = 6;
                    else if (inputDt.Rows[i]["division"].ToString() == "잠재2")
                        division_int = 5;
                    else if (inputDt.Rows[i]["division"].ToString() == "내DATA")
                        division_int = 4;
                    else if (inputDt.Rows[i]["division"].ToString() == "공용DATA")
                        division_int = 3;

                    inputDt.Rows[i]["division_int"] = division_int;
                    dgvData.Rows[i].Cells["division_int"].Value = division_int;
                }
                inputDt.AcceptChanges();
                inputDt = SetAtoDatatable2(inputDt);
                inputDt.Columns.Add("duplicate_id_int", typeof(int));
                inputDt.Columns.Add("rowIndex2", typeof(Int32));
                inputDt.AcceptChanges();

                DataTable tempDt = common.ConvertDgvToDataTable(dgvData);
                tempDt.Columns.Add("rowIndex", typeof(Int32));

                for (int i = 0; i < tempDt.Rows.Count; i++)
                    tempDt.Rows[i]["rowindex"] = i;
                tempDt.AcceptChanges();
                tempDt = SetSeaoverDatatable(tempDt);             //중복비교하기 위한 테이블

                // 데이터 중복추출, PLINQ 사용
                DataTable duplicateDt1 = null;
                DataRow[] tempDr = inputDt.Select("tel <> ''");
                DataRow[] tempDr2 = tempDt.Select("tel <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    DataTable rnDt2 = tempDr2.CopyToDataTable();
                    var duplicateVar1 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("tel") equals t.Field<string>("tel")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            rowindex1 = p.Field<Int32>("rowindex1"),
                                            rowindex2 = (t == null) ? "" : t.Field<string>("rowindex"),
                                            division_int = p.Field<int>("division_int"),
                                            idx = (t == null) ? "" : t.Field<string>("idx"),
                                            tel = (t == null) ? "" : t.Field<string>("tel"),
                                            is_duplicate = (t == null) ? "" : t.Field<string>("is_duplicate"),
                                            company_id = (t == null) ? "" : t.Field<string>("id")
                                        };
                    duplicateDt1 = common.ConvertListToDatatable(duplicateVar1);
                    duplicateDt1.AcceptChanges();
                }

                //사업자번호 확인
                tempDr = inputDt.Select("registration_number <> ''");
                tempDr2 = tempDt.Select("registration_number <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    DataTable rnDt2 = tempDr2.CopyToDataTable();
                    var duplicateVar5 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("registration_number") equals t.Field<string>("registration_number")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            rowindex1 = p.Field<Int32>("rowindex1"),
                                            rowindex2 = (t == null) ? "" : t.Field<string>("rowindex"),
                                            division_int = p.Field<int>("division_int"),
                                            idx = (t == null) ? "" : t.Field<string>("idx"),
                                            tel = (t == null) ? "" : t.Field<string>("registration_number"),
                                            is_duplicate = (t == null) ? "" : t.Field<string>("is_duplicate"),
                                            company_id = (t == null) ? "" : t.Field<string>("id")
                                        };
                    DataTable duplicateDt5 = common.ConvertListToDatatable(duplicateVar5);
                    duplicateDt5.AcceptChanges();
                    if (duplicateDt1 != null && duplicateDt1.Rows.Count > 0)
                        duplicateDt1.Merge(duplicateDt5);
                    else
                        duplicateDt1 = duplicateDt5;
                }
                //중복데이터
                DataRow[] resultDr = duplicateDt1.Select(" company_id <> ''  AND tel <> '' AND rowindex2 <> '' ");
                if (resultDr.Length > 0)
                {
                    DataTable resultDt = resultDr.CopyToDataTable();
                    // 추출된 중복 데이터를 순회하며 데이터가공
                    foreach (DataRow duplicate in resultDt.Rows)
                    {
                        if (int.TryParse(duplicate["rowindex1"].ToString(), out int rowindex1) && int.TryParse(duplicate["rowindex2"].ToString(), out int rowindex2))
                        {
                            bool isExist = false;

                            if (duplicateDic.ContainsKey(rowindex1))
                            {
                                List<DataRow> duplicateDr = duplicateDic[rowindex1];

                                foreach (DataRow dr in duplicateDr)
                                {
                                    if (dr["idx"].ToString() == duplicate["idx"].ToString())
                                        isExist = true;
                                }
                                if (!isExist)
                                    duplicateDr.Add(duplicate);

                                duplicateDic[rowindex1] = duplicateDr;
                            }
                            else
                            {
                                List<DataRow> dr = new List<DataRow>();
                                dr.Add(duplicate);
                                duplicateDic[rowindex1] = dr;
                            }
                            //신규추가
                            if (!isExist)
                            {
                                if (dgvData.Rows[rowindex2].Cells["duplicate_id"].Value == null || string.IsNullOrEmpty(dgvData.Rows[rowindex2].Cells["duplicate_id"].Value.ToString()))
                                {
                                    dgvData.Rows[rowindex2].Cells["duplicate_id"].Value = rowindex1;
                                }
                                else if (dgvData.Rows[rowindex2].Cells["duplicate_id"].Value.ToString() == rowindex1.ToString())
                                {
                                    //통합
                                    List<DataRow> mainDr = duplicateDic[rowindex1];
                                    List<DataRow> subDr = duplicateDic[Convert.ToInt32(dgvData.Rows[rowindex2].Cells["duplicate_id"].Value.ToString())];
                                    foreach (DataRow dr in subDr)
                                        mainDr.Add(dr);
                                    duplicateDic[rowindex1] = mainDr;
                                    duplicateDic.Remove(Convert.ToInt32(dgvData.Rows[rowindex2].Cells["duplicate_id"].Value.ToString()));
                                    dgvData.Rows[rowindex2].Cells["duplicate_id"].Value = string.Empty;
                                }
                            }
                        }
                    }   
                }

                //중복데이터 정리
                int duplicate_id_int = 1;
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    row.Cells["duplicate_id"].Value = string.Empty;

                    int rowindex = row.Index;
                    if (duplicateDic.ContainsKey(rowindex))
                    { 
                        List<DataRow> duplicateDr = duplicateDic[rowindex];
                        if (duplicateDr.Count > 1)
                        {
                            foreach (DataRow dr in duplicateDr)
                            {
                                if (int.TryParse(dr["rowindex2"].ToString(), out int rowindex2))
                                    dgvData.Rows[rowindex2].Cells["duplicate_id"].Value = duplicate_id_int;
                            }
                            duplicate_id_int++;
                        }
                    }
                }
               
                //재출력
                inputDt = common.ConvertDgvToDataTable(dgvData);
                inputDt.AcceptChanges();
                DataView dv = new DataView(inputDt);
                dv.Sort = "duplicate_id ASC, division_int DESC, sales_updatetime DESC";
                inputDt = dv.ToTable();

                dgvData.Rows.Clear();
                for (int i = 0; i < inputDt.Rows.Count; i++)
                {
                    int temp_duplicate_id;
                    if (!int.TryParse(inputDt.Rows[i]["duplicate_id"].ToString(), out temp_duplicate_id))
                        temp_duplicate_id = 999999;

                    if (temp_duplicate_id < 999999)
                    {
                        int n = dgvData.Rows.Add();
                        dgvData.Rows[n].Cells["company_id"].Value = inputDt.Rows[i]["company_id"].ToString();
                        dgvData.Rows[n].Cells["seaover_company_code"].Value = inputDt.Rows[i]["seaover_company_code"].ToString();
                        dgvData.Rows[n].Cells["division"].Value = inputDt.Rows[i]["division"].ToString();
                        dgvData.Rows[n].Cells["company"].Value = inputDt.Rows[i]["company"].ToString();
                        dgvData.Rows[n].Cells["registration_number"].Value = inputDt.Rows[i]["registration_number"].ToString();
                        dgvData.Rows[n].Cells["ceo"].Value = inputDt.Rows[i]["ceo"].ToString();
                        dgvData.Rows[n].Cells["tel"].Value = inputDt.Rows[i]["tel"].ToString();
                        dgvData.Rows[n].Cells["fax"].Value = inputDt.Rows[i]["fax"].ToString();
                        dgvData.Rows[n].Cells["phone"].Value = inputDt.Rows[i]["phone"].ToString();
                        dgvData.Rows[n].Cells["other_phone"].Value = inputDt.Rows[i]["other_phone"].ToString();
                        dgvData.Rows[n].Cells["ato_manager"].Value = inputDt.Rows[i]["ato_manager"].ToString();
                        dgvData.Rows[n].Cells["table_index"].Value = inputDt.Rows[i]["table_index"].ToString();
                        dgvData.Rows[n].Cells["duplicate_id"].Value = temp_duplicate_id;
                        DateTime sales_updatetime;
                        if (DateTime.TryParse(inputDt.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                            dgvData.Rows[n].Cells["sales_updatetime"].Value = sales_updatetime.ToString("yyyy-MM-dd");
                        dgvData.Rows[n].Cells["sales_contents"].Value = inputDt.Rows[i]["sales_contents"].ToString();
                        dgvData.Rows[n].Cells["sales_remark"].Value = inputDt.Rows[i]["sales_remark"].ToString();
                        dgvData.Rows[n].Cells["sales_edit_user"].Value = inputDt.Rows[i]["sales_edit_user"].ToString();
                    }
                }
            }
            SetRowColor();
            return duplicate_id;
        }
        private int CheckDuplicateAsOne()
        {
            int duplicate_id = 0;
            duplicateDic = new Dictionary<int, List<DataRow>>();
            if (dgvData.Rows.Count > 0 && companyDt != null && companyDt.Rows.Count > 0)
            {
                //중복ID 초기화
                for (int i = 0; i < dgvData.Rows.Count; i++)
                    dgvData.Rows[i].Cells["duplicate_id"].Value = string.Empty;

                //중복데이터
                DataRow[] noDeleteDt = companyDt.Select("isDelete = false");
                if (noDeleteDt.Length > 0)
                {
                    DataTable tempDt = SetSeaoverDatatable(noDeleteDt.CopyToDataTable());             //중복비교하기 위한 테이블
                    for (int i = 0; i < dgvData.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvData.Rows[i];

                        if (string.IsNullOrWhiteSpace(row.Cells["duplicate_id"].Value.ToString()))
                        {
                            //INPUTDT JOIN NOSEAOVERDT 
                            DataRow[] seaover1 = tempDt.Select($"(tel = '{ValidationTelString(row.Cells["tel"].Value.ToString())}' OR tel = '{row.Cells["tel"].Value.ToString()}') AND tel <> ''");
                            DataTable seaoverDt1 = new DataTable();
                            if (seaover1.Length > 0)
                                seaoverDt1 = seaover1.CopyToDataTable();

                            DataRow[] seaover2 = tempDt.Select($"(tel = '{ValidationTelString(row.Cells["fax"].Value.ToString())}' OR tel = '{row.Cells["fax"].Value.ToString()}') AND tel <> ''");
                            DataTable seaoverDt2 = new DataTable();
                            if (seaover2.Length > 0)
                                seaoverDt2 = seaover2.CopyToDataTable();

                            DataRow[] seaover3 = tempDt.Select($"(tel = '{ValidationTelString(row.Cells["phone"].Value.ToString())}' OR tel = '{row.Cells["phone"].Value.ToString()}') AND tel <> ''");
                            DataTable seaoverDt3 = new DataTable();
                            if (seaover3.Length > 0)
                                seaoverDt3 = seaover3.CopyToDataTable();

                            DataRow[] seaover4 = tempDt.Select($"registration_number = '{row.Cells["registration_number"].Value.ToString()}'  AND registration_number <> ''");
                            DataTable seaoverDt4 = new DataTable();
                            if (seaover4.Length > 0)
                                seaoverDt4 = seaover4.CopyToDataTable();

                            //초기화
                            if (duplicateDic.ContainsKey(i))
                                duplicateDic[i] = new List<DataRow>();

                            DataTable duplicateDt = new DataTable();
                            duplicateDt.Columns.Add("company_id", typeof(string));
                            duplicateDt.Columns.Add("company", typeof(string));
                            duplicateDt.Columns.Add("seaover_company_code", typeof(string));
                            duplicateDt.Columns.Add("ato_manager", typeof(string));
                            duplicateDt.Columns.Add("division", typeof(string));
                            duplicateDt.Columns.Add("idx", typeof(string));

                            //atoDt index
                            int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());

                            if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0)
                            {
                                for (int j = 0; j < seaoverDt1.Rows.Count; j++)
                                {
                                    DataRow newDr = duplicateDt.NewRow();
                                    newDr["company_id"] = seaoverDt1.Rows[j]["id"].ToString();
                                    newDr["company"] = seaoverDt1.Rows[j]["company"].ToString();
                                    newDr["seaover_company_code"] = seaoverDt1.Rows[j]["seaover_company_code"].ToString();
                                    newDr["ato_manager"] = seaoverDt1.Rows[j]["ato_manager"].ToString();
                                    newDr["division"] = seaoverDt1.Rows[j]["is_duplicate"].ToString();
                                    newDr["idx"] = seaoverDt1.Rows[j]["idx"].ToString();


                                    if (duplicateDic.ContainsKey(table_index))
                                    {
                                        List<DataRow> list = duplicateDic[table_index];

                                        bool isExist = false;
                                        for (int k = 0; k < list.Count; k++)
                                        {
                                            if (seaoverDt1.Rows[j]["idx"].ToString() == list[k]["idx"].ToString())
                                                isExist = true;
                                        }

                                        if (!isExist)
                                        {
                                            list.Add(newDr);
                                            duplicateDic[table_index] = list;
                                        }
                                    }
                                    else
                                    {
                                        List<DataRow> list = new List<DataRow>();
                                        list.Add(newDr);
                                        duplicateDic[table_index] = list;
                                    }
                                }
                            }

                            if (seaoverDt2 != null && seaoverDt2.Rows.Count > 0)
                            {
                                for (int j = 0; j < seaoverDt2.Rows.Count; j++)
                                {
                                    DataRow newDr = duplicateDt.NewRow();
                                    newDr["company_id"] = seaoverDt2.Rows[j]["id"].ToString();
                                    newDr["company"] = seaoverDt2.Rows[j]["company"].ToString();
                                    newDr["seaover_company_code"] = seaoverDt2.Rows[j]["seaover_company_code"].ToString();
                                    newDr["ato_manager"] = seaoverDt2.Rows[j]["ato_manager"].ToString();
                                    newDr["division"] = seaoverDt2.Rows[j]["is_duplicate"].ToString();
                                    newDr["idx"] = seaoverDt2.Rows[j]["idx"].ToString();

                                    if (duplicateDic.ContainsKey(table_index))
                                    {
                                        List<DataRow> list = duplicateDic[table_index];

                                        bool isExist = false;
                                        for (int k = 0; k < list.Count; k++)
                                        {
                                            if (seaoverDt2.Rows[j]["idx"].ToString() == list[k]["idx"].ToString())
                                                isExist = true;
                                        }


                                        if (!isExist)
                                        {
                                            list.Add(newDr);
                                            duplicateDic[table_index] = list;
                                        }
                                    }
                                    else
                                    {
                                        List<DataRow> list = new List<DataRow>();
                                        list.Add(newDr);
                                        duplicateDic[table_index] = list;
                                    }
                                }
                            }

                            if (seaoverDt3 != null && seaoverDt3.Rows.Count > 0)
                            {
                                for (int j = 0; j < seaoverDt3.Rows.Count; j++)
                                {
                                    DataRow newDr = duplicateDt.NewRow();
                                    newDr["company_id"] = seaoverDt3.Rows[j]["id"].ToString();
                                    newDr["company"] = seaoverDt3.Rows[j]["company"].ToString();
                                    newDr["seaover_company_code"] = seaoverDt3.Rows[j]["seaover_company_code"].ToString();
                                    newDr["ato_manager"] = seaoverDt3.Rows[j]["ato_manager"].ToString();
                                    newDr["division"] = seaoverDt3.Rows[j]["is_duplicate"].ToString();
                                    newDr["idx"] = seaoverDt3.Rows[j]["idx"].ToString();

                                    if (duplicateDic.ContainsKey(table_index))
                                    {
                                        List<DataRow> list = duplicateDic[table_index];

                                        bool isExist = false;
                                        for (int k = 0; k < list.Count; k++)
                                        {
                                            if (seaoverDt3.Rows[j]["idx"].ToString() == list[k]["idx"].ToString())
                                                isExist = true;
                                        }


                                        if (!isExist)
                                        {
                                            list.Add(newDr);
                                            duplicateDic[table_index] = list;
                                        }
                                    }
                                    else
                                    {
                                        List<DataRow> list = new List<DataRow>();
                                        list.Add(newDr);
                                        duplicateDic[table_index] = list;
                                    }
                                }
                            }

                            //사업자등록번호 중복
                            if (seaoverDt4 != null && seaoverDt4.Rows.Count > 0 && !string.IsNullOrEmpty(row.Cells["registration_number"].Value.ToString()))
                            {
                                for (int j = 0; j < seaoverDt4.Rows.Count; j++)
                                {
                                    DataRow newDr = duplicateDt.NewRow();
                                    newDr["company_id"] = seaoverDt4.Rows[j]["id"].ToString();
                                    newDr["company"] = seaoverDt4.Rows[j]["company"].ToString();
                                    newDr["seaover_company_code"] = seaoverDt4.Rows[j]["seaover_company_code"].ToString();
                                    newDr["ato_manager"] = seaoverDt4.Rows[j]["ato_manager"].ToString();
                                    newDr["division"] = seaoverDt4.Rows[j]["is_duplicate"].ToString();
                                    newDr["idx"] = seaoverDt4.Rows[j]["idx"].ToString();

                                    if (duplicateDic.ContainsKey(table_index))
                                    {
                                        List<DataRow> list = duplicateDic[table_index];

                                        bool isExist = false;
                                        for (int k = 0; k < list.Count; k++)
                                        {
                                            if (seaoverDt4.Rows[j]["idx"].ToString() == list[k]["idx"].ToString())
                                                isExist = true;
                                        }


                                        if (!isExist)
                                        {
                                            list.Add(newDr);
                                            duplicateDic[table_index] = list;
                                        }
                                    }
                                    else
                                    {
                                        List<DataRow> list = new List<DataRow>();
                                        list.Add(newDr);
                                        duplicateDic[table_index] = list;
                                    }
                                }
                            }

                            //중복결과 출력
                            if (duplicateDic.ContainsKey(table_index))
                            {
                                List<DataRow> list = duplicateDic[table_index];
                                if (list.Count > 1)
                                {
                                    duplicate_id++;
                                    for (int k = 0; k < list.Count; k++)
                                    {
                                        int rowIndex = Convert.ToInt32(list[k]["idx"].ToString());

                                        //이미 중복ID가 있으면 통합
                                        if (dgvData.Rows[rowIndex].Cells["duplicate_id"].Value != null
                                            && !string.IsNullOrEmpty(dgvData.Rows[rowIndex].Cells["duplicate_id"].Value.ToString())
                                            && int.TryParse(dgvData.Rows[rowIndex].Cells["duplicate_id"].Value.ToString(), out int pre_duplicate_id))
                                        {
                                            if (pre_duplicate_id < 999999)
                                            {
                                                for (int l = 0; l < i; l++)
                                                {
                                                    if (dgvData.Rows[l].Cells["duplicate_id"].Value != null && dgvData.Rows[l].Cells["duplicate_id"].Value.ToString() == pre_duplicate_id.ToString())
                                                        dgvData.Rows[l].Cells["duplicate_id"].Value = duplicate_id;
                                                }
                                            }
                                        }

                                        dgvData.Rows[rowIndex].Cells["duplicate_id"].Value = duplicate_id;
                                    }
                                }
                                else
                                    dgvData.Rows[i].Cells["duplicate_id"].Value = 999999;
                            }
                            else
                                dgvData.Rows[i].Cells["duplicate_id"].Value = 999999;
                        }
                    }

                    DataTable resultDt = common.ConvertDgvToDataTable(dgvData);
                    resultDt.Columns.Add("duplicate_id_int", typeof(int));
                    resultDt.Columns.Add("division_int", typeof(int));
                    for (int i = 0; i < resultDt.Rows.Count; i++)
                    {
                        int temp_duplicate_id;
                        if (!int.TryParse(resultDt.Rows[i]["duplicate_id"].ToString(), out temp_duplicate_id))
                            temp_duplicate_id = 999999;
                        resultDt.Rows[i]["duplicate_id_int"] = temp_duplicate_id;

                        int division_int = 0;
                        switch (resultDt.Rows[i]["division"].ToString())
                        {
                            case "거래중":
                                division_int = 8;
                                break;
                            case "잠재1":
                                division_int = 7;
                                break;
                            case "잠재2":
                                division_int = 6;
                                break;
                            case "내DATA":
                                division_int = 5;
                                break;
                            case "공용DATA":
                                division_int = 4;
                                break;
                            default:
                                division_int = 3;
                                break;
                        }

                        if (!string.IsNullOrEmpty(resultDt.Rows[i]["seaover_company_code"].ToString()))
                            division_int++;
                        else if (division_int == 8)
                            division_int--;

                        resultDt.Rows[i]["division_int"] = division_int;
                    }

                    resultDt.AcceptChanges();
                    DataView dv = new DataView(resultDt);
                    dv.Sort = "duplicate_id_int ASC, division_int DESC, sales_updatetime DESC";
                    resultDt = dv.ToTable();

                    dgvData.Rows.Clear();
                    for (int i = 0; i < resultDt.Rows.Count; i++)
                    {
                        int temp_duplicate_id;
                        if (!int.TryParse(resultDt.Rows[i]["duplicate_id_int"].ToString(), out temp_duplicate_id))
                            temp_duplicate_id = 999999;

                        if (temp_duplicate_id < 999999)
                        {
                            int n = dgvData.Rows.Add();
                            dgvData.Rows[n].Cells["company_id"].Value = resultDt.Rows[i]["company_id"].ToString();
                            dgvData.Rows[n].Cells["seaover_company_code"].Value = resultDt.Rows[i]["seaover_company_code"].ToString();
                            dgvData.Rows[n].Cells["division"].Value = resultDt.Rows[i]["division"].ToString();
                            dgvData.Rows[n].Cells["company"].Value = resultDt.Rows[i]["company"].ToString();
                            dgvData.Rows[n].Cells["registration_number"].Value = resultDt.Rows[i]["registration_number"].ToString();
                            dgvData.Rows[n].Cells["ceo"].Value = resultDt.Rows[i]["ceo"].ToString();
                            dgvData.Rows[n].Cells["tel"].Value = resultDt.Rows[i]["tel"].ToString();
                            dgvData.Rows[n].Cells["fax"].Value = resultDt.Rows[i]["fax"].ToString();
                            dgvData.Rows[n].Cells["phone"].Value = resultDt.Rows[i]["phone"].ToString();
                            dgvData.Rows[n].Cells["ato_manager"].Value = resultDt.Rows[i]["ato_manager"].ToString();
                            dgvData.Rows[n].Cells["table_index"].Value = resultDt.Rows[i]["table_index"].ToString();
                            dgvData.Rows[n].Cells["duplicate_id"].Value = temp_duplicate_id;
                            DateTime sales_updatetime;
                            if (DateTime.TryParse(resultDt.Rows[i]["sales_updatetime"].ToString(), out sales_updatetime))
                                dgvData.Rows[n].Cells["sales_updatetime"].Value = sales_updatetime.ToString("yyyy-MM-dd");
                            dgvData.Rows[n].Cells["sales_contents"].Value = resultDt.Rows[i]["sales_contents"].ToString();
                            dgvData.Rows[n].Cells["sales_remark"].Value = resultDt.Rows[i]["sales_remark"].ToString();
                            dgvData.Rows[n].Cells["sales_edit_user"].Value = resultDt.Rows[i]["sales_edit_user"].ToString();
                        }
                    }
                }
            }
            SetRowColor();
            return duplicate_id;
        }
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

        #region 중복 Datatable 만들기
        private DataTable SetAtoDatatable2(DataTable seaoverDt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("company", typeof(string));
            dt.Columns.Add("tel", typeof(string));
            dt.Columns.Add("registration_number", typeof(string));
            dt.Columns.Add("rowindex1", typeof(Int32));
            dt.Columns.Add("division_int", typeof(int));
            dt.Columns.Add("duplicate_id", typeof(string));
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                int idx = 0;
                foreach (DataRow row in seaoverDt.Rows)
                {
                    if (row["company_id"].ToString() == "100101")
                        seaoverDt.AcceptChanges();

                    idx--;
                    bool isAdd = false;
                    string tel = row["tel"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = row["fax"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = row["phone"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    tel = row["other_phone"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length >= 10 && tel2.Length > 0)
                            {
                                int sttNum = Convert.ToInt16(tel1.Substring(tel1.Length - 1, 1));
                                int endNum = Convert.ToInt16(tel2);

                                for (int j = sttNum; j <= endNum; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[0].Trim()))
                                    {
                                        string rsTel = tels[0].Substring(0, tels[0].Length - 1) + j.ToString();
                                        DataRow dr = GetDatarow(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = GetDatarow(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = GetDatarow(dt, row, tel, idx);
                            dt.Rows.Add(dr);
                        }
                        isAdd = true;
                    }

                    string registration_number = row["registration_number"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!isAdd && !string.IsNullOrEmpty(registration_number))
                    {
                        DataRow dr = GetDatarow(dt, row, "", idx);
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
        private DataTable SetSeaoverDatatable(DataTable seaoverDt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("idx", typeof(string));
            dt.Columns.Add("tel", typeof(string));
            dt.Columns.Add("rowindex", typeof(string));
            dt.Columns.Add("registration_number", typeof(string));
            dt.Columns.Add("is_duplicate", typeof(string));
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                int idx = -1;
                foreach (DataRow row in seaoverDt.Rows)
                {
                    idx++;
                    bool isAdd = false;
                    string tel = row["tel"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
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
                                string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                if (!string.IsNullOrEmpty(rsTel))
                                {
                                    if (j > 0 && tels[0].Length > rsTel.Length)
                                        rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                    DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
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
                        isAdd = true;
                    }

                    tel = row["fax"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');
                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
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
                        isAdd = true;
                    }

                    tel = row["phone"].ToString().Trim().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
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
                        isAdd = true;
                    }

                    tel = row["other_phone"].ToString().Trim().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('/'))
                        {
                            if (tel.Substring(0, 1) == "/")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split('/');
                            for (int j = 0; j < tels.Length; j++)
                            {
                                if (!string.IsNullOrEmpty(tels[j].Trim()))
                                {
                                    string rsTel = Regex.Replace(tels[j].Trim(), @"[^0-9]", "").ToString();
                                    if (!string.IsNullOrEmpty(rsTel))
                                    {
                                        if (j > 0 && tels[0].Length > rsTel.Length)
                                            rsTel = tels[0].Substring(0, tels[0].Length - rsTel.Length) + rsTel;

                                        DataRow dr = GetDatarow2(dt, row, rsTel, idx);
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else if (tel.Contains('~'))
                        {
                            string[] tels = tel.Split('~');

                            string tel1 = Regex.Replace(tels[0].Trim(), @"[^0-9]", "").ToString();
                            string tel2 = Regex.Replace(tels[1].Trim(), @"[^0-9]", "").ToString();
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
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
                        isAdd = true;
                    }

                    string registration_number = row["registration_number"].ToString().Trim().Replace("-", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(registration_number) && !isAdd)
                    {
                        DataRow dr = GetDatarow2(dt, row, "", idx);
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
        private DataRow GetDatarow(DataTable dt, DataRow row, string tel, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = row["company_id"].ToString();
            dr["company"] = row["company"].ToString();
            tel = Regex.Replace(tel, @"[^0-9]", "").ToString();
            dr["tel"] = tel;
            dr["registration_number"] = row["registration_number"].ToString();
            dr["rowindex1"] = Convert.ToInt32(row["rowindex1"].ToString());
            dr["division_int"] = Convert.ToInt32(row["division_int"].ToString());
            dr["duplicate_id"] = row["duplicate_id"].ToString();

            return dr;
        }
        private DataRow GetDatarow2(DataTable dt, DataRow row, string tel, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["id"] = row["company_id"].ToString();
            dr["idx"] = idx.ToString();
            tel = Regex.Replace(tel, @"[^0-9]", "").ToString();
            dr["tel"] = tel.ToString();
            dr["registration_number"] = row["registration_number"].ToString();
            dr["rowindex"] = row["rowindex"].ToString();
            
            int division_int = 0;
            switch (row["division"].ToString())
            {
                case "거래중":
                    division_int = 8;
                    break;
                case "잠재1":
                    division_int = 7;
                    break;
                case "잠재2":
                    division_int = 6;
                    break;
                case "내DATA":
                    division_int = 5;
                    break;
                case "공용DATA":
                    division_int = 4;
                    break;
                default:
                    division_int = 3;
                    break;
            }

            if (!string.IsNullOrEmpty(row["seaover_company_code"].ToString()))
                division_int++;
            else if (division_int == 8)
                division_int--;

            dr["is_duplicate"] = division_int;


            return dr;
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int n = GetData();   
        }
        #endregion

        #region Key event
        private void txtDivision_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int n = GetData();
            }
                
        }
        private void MyDuplicateDataManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtDivision.Focus();
                        break;
                    case Keys.N:
                        //txtAtoManager.Text = String.Empty;
                        txtDivision.Text = String.Empty;
                        txtDivision.Focus();
                        break;
                }
            }
        }
        #endregion

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int main_id = commonRepository.GetNextId("t_company_group", "main_id");
                if (dgvData.Columns[e.ColumnIndex].Name == "delete")
                {
                    integratedCompanyInfo(e.RowIndex, main_id);
                    messageBox.Show(this,"통합완료");
                    this.Activate();
                }        
            }
        }
        private void DeleteCompanyInfo(int rowIndex, bool isRefresh = true)
        {
            dgvData.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            DataGridViewRow row = dgvData.Rows[rowIndex];

            int id;
            if (!int.TryParse(row.Cells["company_id"].Value.ToString(), out id))
                id = 0;
            //SEAOVER 거래처일 경우
            if (id == 0)
            {
                messageBox.Show(this, "등록되지 않은 거래처입니다.( 혹은 SEAOVER 거래처입니다.)");
                this.Activate();
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
            sModel.contents = "거래처 중복삭제";
            sModel.log = "";
            sModel.remark = "";
            sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sModel.edit_user = um.user_name;

            sql = salesPartnerRepository.InsertPartnerSales(sModel);
            sqlList.Add(sql);

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
                    int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                    atoDt = sm.SetCompanyData(table_index, true);
                    if (isRefresh)
                    {
                        messageBox.Show(this,"삭제완료");
                        int n = GetData();
                        this.Activate();
                    }
                }
            }
        }

        private void integratedCompanyInfo(int rowIndex, int main_id, bool isMsg = true, bool isRefresh = true)
        {
            dgvData.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            //현재 row
            DataGridViewRow main_row = dgvData.Rows[rowIndex];
            //중복ID
            string duplicate_id = dgvData.Rows[rowIndex].Cells["duplicate_id"].Value.ToString();

            int main_company_id;
            if (dgvData.Rows[rowIndex].Cells["company_id"].Value == null || !int.TryParse(dgvData.Rows[rowIndex].Cells["company_id"].Value.ToString().ToLower(), out main_company_id))
                main_company_id = 0;

            //SEAOVER 거래처일 경우
            if (main_row.Cells["seaover_company_code"].Value == null || string.IsNullOrEmpty(main_row.Cells["seaover_company_code"].Value.ToString()) && main_company_id == 0)
            {
                messageBox.Show(this, "등록되지 않은 거래처입니다.( 혹은 SEAOVER 거래처입니다.)");
                this.Activate();
                return;
            }
            else if(main_row.Cells["seaover_company_code"].Value == null || string.IsNullOrEmpty(main_row.Cells["seaover_company_code"].Value.ToString()))
            {
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (duplicate_id == dgvData.Rows[i].Cells["duplicate_id"].Value.ToString()
                        && dgvData.Rows[i].Cells["seaover_company_code"].Value != null && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["seaover_company_code"].Value.ToString()))
                    {
                        messageBox.Show(this, "씨오버 거래처가 있다면 씨오버 거래처로 통합해야만 합니다!");
                        this.Activate();
                        return;
                    }
                }
            }
            //확인 메세지
            if (isMsg)
            {
                if (messageBox.Show(this, $"중복 데이터를 하나만 남기고 통합하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            //데이터 합치기
            string registration_number = "", ceo = "", tel = "", fax = "", phone = "", other_phone = "";
            
            List<DataGridViewRow> deleteList = new List<DataGridViewRow>();
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                if (duplicate_id == dgvData.Rows[i].Cells["duplicate_id"].Value.ToString())
                {
                    deleteList.Add(dgvData.Rows[i]);

                    if (dgvData.Rows[i].Cells["registration_number"].Value != null && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["registration_number"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(registration_number))
                            registration_number = dgvData.Rows[i].Cells["registration_number"].Value.ToString();
                        else if (!registration_number.Replace("-", "").Replace(" ", "").Contains(dgvData.Rows[i].Cells["registration_number"].Value.ToString().Replace("-", "").Replace(" ", "")))
                            registration_number += ", " + dgvData.Rows[i].Cells["registration_number"].Value.ToString();
                    }

                    if (dgvData.Rows[i].Cells["ceo"].Value != null && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["ceo"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(ceo))
                            ceo = dgvData.Rows[i].Cells["ceo"].Value.ToString();
                        else if (!ceo.Replace("-", "").Replace(" ", "").Contains(dgvData.Rows[i].Cells["ceo"].Value.ToString().Replace("-", "").Replace(" ", "")))
                            ceo += ", " + dgvData.Rows[i].Cells["ceo"].Value.ToString();
                    }

                    if (dgvData.Rows[i].Cells["tel"].Value != null && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["tel"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(tel))
                            tel = dgvData.Rows[i].Cells["tel"].Value.ToString();
                        else if (!tel.Replace("-", "").Replace(" ", "").Contains(dgvData.Rows[i].Cells["tel"].Value.ToString().Replace("-", "").Replace(" ", "")))
                            tel += ", " + dgvData.Rows[i].Cells["tel"].Value.ToString();
                    }

                    if (dgvData.Rows[i].Cells["fax"].Value != null && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["fax"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(fax))
                            fax = dgvData.Rows[i].Cells["fax"].Value.ToString();
                        else if (!fax.Replace("-", "").Replace(" ", "").Contains(dgvData.Rows[i].Cells["fax"].Value.ToString().Replace("-", "").Replace(" ", "")))
                            fax += ", " + dgvData.Rows[i].Cells["fax"].Value.ToString();
                    }

                    if (dgvData.Rows[i].Cells["phone"].Value != null && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["phone"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(phone))
                            phone = dgvData.Rows[i].Cells["phone"].Value.ToString();
                        else if (!phone.Replace("-", "").Replace(" ", "").Contains(dgvData.Rows[i].Cells["phone"].Value.ToString().Replace("-", "").Replace(" ", "")))
                            phone += ", " + dgvData.Rows[i].Cells["phone"].Value.ToString();
                    }

                    if (dgvData.Rows[i].Cells["other_phone"].Value != null && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["other_phone"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(other_phone))
                            other_phone = dgvData.Rows[i].Cells["other_phone"].Value.ToString();
                        else if (!other_phone.Replace("-", "").Replace(" ", "").Contains(dgvData.Rows[i].Cells["other_phone"].Value.ToString().Replace("-", "").Replace(" ", "")))
                            other_phone += ", " + dgvData.Rows[i].Cells["other_phone"].Value.ToString();
                    }
                }
            }

            //데이터 통합반영
            for (int i = 0; i < deleteList.Count; i++)
            {
                int temp_id;
                if (!int.TryParse(deleteList[i].Cells["company_id"].Value.ToString(), out temp_id))
                    temp_id = 0;

                //거래처 통합
                //씨오버 거래처 일 경우
                if (deleteList[i].Cells["seaover_company_code"].Value != null && !string.IsNullOrEmpty(deleteList[i].Cells["seaover_company_code"].Value.ToString()))
                {
                    CompanyGroupModel model = new CompanyGroupModel();
                    if (rowIndex == deleteList[i].Index)
                        model.sub_id = 0;
                    else
                        model.sub_id = 1;

                    model.main_id = main_id;
                    model.company_id = deleteList[i].Cells["seaover_company_code"].Value.ToString();
                    model.company = deleteList[i].Cells["company"].Value.ToString();
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //이미 등록된 그룹 삭제
                    /*sql = companyGroupRepository.DeleteCompanyGroup(model.company_id);
                    sqlList.Add(sql);*/
                    //새로 그룹 등록
                    sql = companyGroupRepository.InsertCompanyGroup(model);
                    sqlList.Add(sql);
                    //AtoDt 반영
                    int table_index = Convert.ToInt32(deleteList[i].Cells["table_index"].Value.ToString());
                    sm.UpdateAtoDt(table_index, "main_id", model.main_id);
                    sm.UpdateAtoDt(table_index, "sub_id", model.sub_id);

                    if (model.sub_id == 0)
                    {
                        sm.UpdateAtoDt(table_index, "ato_manager", main_row.Cells["ato_manager"].Value.ToString());
                        sm.UpdateAtoDt(table_index, "sales_edit_user", main_row.Cells["sales_edit_user"].Value.ToString());
                        sm.UpdateAtoDt(table_index, "sales_contents", main_row.Cells["sales_contents"].Value.ToString());
                        sm.UpdateAtoDt(table_index, "sales_updatetime", main_row.Cells["sales_updatetime"].Value.ToString());
                        sm.UpdateAtoDt(table_index, "sales_remark", main_row.Cells["sales_remark"].Value.ToString());
                    }
                    /*sm.SetGroupCompany(table_index, main_id, model.sub_id
                                    , main_row.Cells["ato_manager"].Value.ToString()
                                    , main_row.Cells["sales_edit_user"].Value.ToString()
                                    , main_row.Cells["sales_contents"].Value.ToString()
                                    , main_row.Cells["sales_updatetime"].Value.ToString()
                                    , main_row.Cells["sales_remark"].Value.ToString());*/
                }
                //거래처 삭제
                else if (temp_id > 0 && rowIndex != deleteList[i].Index)
                {
                    //삭제
                    sql = companyRepository.DeleteCompany(temp_id, true);
                    sqlList.Add(sql);
                    //영업내역
                    CompanySalesModel sModel = new CompanySalesModel();
                    sModel.company_id = temp_id;
                    sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", temp_id.ToString());
                    sModel.is_sales = false;
                    sModel.contents = "거래처 중복삭제";
                    sModel.log = "";
                    sModel.remark = "";
                    sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sModel.edit_user = um.user_name;

                    sql = salesPartnerRepository.InsertPartnerSales(sModel);
                    sqlList.Add(sql);


                    //데이터반영
                    int table_index = Convert.ToInt32(deleteList[i].Cells["table_index"].Value.ToString());
                    //atoDt = sm.SetCompanyData(table_index, true, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "거래처 중복삭제", um.user_name);
                    sm.UpdateAtoDt(table_index, "isDelete", true);
                    sm.UpdateAtoDt(table_index, "sales_updatetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sm.UpdateAtoDt(table_index, "sales_contents", "거래처 중복삭제");
                    sm.UpdateAtoDt(table_index, "sales_edit_user", um.user_name);
                }
                //메인거래처(씨오버 아닌 거래처)
                else if (temp_id > 0 && rowIndex == deleteList[i].Index)
                {
                    UpdateCompanyInfo(rowIndex, registration_number, ceo, tel, fax, phone, other_phone);
                }
                    
            }

            //Execute
            
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this,"수정중 에러가 발생하였습니다.");
                    this.Activate();
                    return;
                }
                else
                {
                    if (isRefresh)
                    {
                        atoDt = sm.GetAtoDt();
                        int n = GetData();
                    }
                }
            }
        }


        private void UpdateCompanyInfo(int rowIndex, string registration_number, string ceo, string tel, string fax, string phone, string other_phone)
        {
            dgvData.EndEdit();
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            DataGridViewRow row = dgvData.Rows[rowIndex];

            int id;
            if (!int.TryParse(row.Cells["company_id"].Value.ToString(), out id))
                id = 0;
            //SEAOVER 거래처일 경우
            if (id == 0)
            {
                messageBox.Show(this, "등록되지 않은 거래처입니다.( 혹은 SEAOVER 거래처입니다.)");
                this.Activate();
                return;
            }
            
            sql = companyRepository.UpdateCompany(id, registration_number, ceo, tel, fax, phone, other_phone);
            sqlList.Add(sql);

            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this,"수정중 에러가 발생하였습니다.");
                    this.Activate();
                    return;
                }
                else
                {
                    int table_index = Convert.ToInt32(row.Cells["table_index"].Value.ToString());
                    sm.UpdateAtoDt(table_index, "registration_number", registration_number);
                    sm.UpdateAtoDt(table_index, "ceo", ceo);
                    sm.UpdateAtoDt(table_index, "tel", tel);
                    sm.UpdateAtoDt(table_index, "fax", fax);
                    sm.UpdateAtoDt(table_index, "phone", phone);
                    sm.UpdateAtoDt(table_index, "other_phone", other_phone);
                }
            }
            this.BringToFront();
        }

        private void SetRowColor()
        {
            if (dgvData.Rows.Count > 0)
            {
                int idx = 0;
                if (dgvData.Rows[0].Cells["duplicate_id"].Value == null)
                    dgvData.Rows[0].Cells["duplicate_id"].Value = string.Empty;

                int tempId;
                if (dgvData.Rows[0].Cells["duplicate_id"].Value == null || !int.TryParse(dgvData.Rows[0].Cells["duplicate_id"].Value.ToString(), out tempId))
                    tempId = 0;

                string temp_id_txt = "";
                int temp_id = -1;

                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    //중복 구분
                    int duplicate_id;
                    if (dgvData.Rows[i].Cells["duplicate_id"].Value == null || !int.TryParse(dgvData.Rows[i].Cells["duplicate_id"].Value.ToString(), out duplicate_id))
                        duplicate_id = 0;

                    //다를 경우
                    if (temp_id_txt != duplicate_id.ToString())
                    {
                        temp_id++;
                        temp_id_txt = duplicate_id.ToString();
                    }


                    if (temp_id % 2 == 0)
                        dgvData.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    else
                        dgvData.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;

                    //씨오버구분
                    if (dgvData.Rows[i].Cells["seaover_company_code"].Value != null & !string.IsNullOrEmpty(dgvData.Rows[i].Cells["seaover_company_code"].Value.ToString()))
                    {
                        dgvData.Rows[i].HeaderCell.Style.ForeColor = Color.Red;
                        dgvData.Rows[i].HeaderCell.Value = "S"; 
                    }
                }
            }
        }

        private void btnDuplicateDelete_Click(object sender, EventArgs e)
        {
            if (messageBox.Show(this, $"중복 데이터를 하나만 남기고 통합하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            //삭제, 통합
            string temp_duplicate_id = "";
            int top_rowiindex = -1;
            int main_id = commonRepository.GetNextId("t_company_group", "main_id");
            for (int i = 0; i < dgvData.Rows.Count; i++)
            {
                string duplicate_id = dgvData.Rows[i].Cells["duplicate_id"].Value.ToString();
                if (temp_duplicate_id != duplicate_id)
                {
                    top_rowiindex = i;
                    temp_duplicate_id = duplicate_id;
                    integratedCompanyInfo(top_rowiindex, main_id, false, false);
                    main_id++;
                }
            }
            messageBox.Show(this,"통합완료");
            this.Activate();
            atoDt = sm.GetAtoDt();
            int n = GetData();
        }

        #region 우클릭 메뉴
        private void dgvData_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgvData.SelectedRows.Count == 1)
                {
                    dgvData.ClearSelection();
                    dgvData.Rows[e.RowIndex].Selected = true;
                }
                else if (dgvData.SelectedRows.Count == 0)
                    dgvData.Rows[e.RowIndex].Selected = true;
            }
        }
        ContextMenuStrip m;
        private void dgvData_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvData.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    m = new ContextMenuStrip();
                    m.Items.Add("삭제");
                    if (dgvData.SelectedRows.Count > 1)
                        m.Items.Add("대표 거래처 설정");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvData, e.Location);
                }
            }
            catch (Exception ex)
            {
                messageBox.Show(this,ex.Message);
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
                        case "통합":
                            if (messageBox.Show(this, $"[{dgvData.Rows[rowindex].Cells["company"].Value.ToString()}]를 통합하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                int main_id = commonRepository.GetNextId("t_company_group", "main_id");
                                integratedCompanyInfo(rowindex, main_id);
                                messageBox.Show(this,"통합완료");
                                this.Activate();
                            }
                            break;
                        case "대표 거래처 설정":
                            List<DataGridViewRow> list = new List<DataGridViewRow>();
                            for (int i = 0; i < dgvData.Rows.Count; i++)
                            {
                                if (dgvData.Rows[i].Selected
                                    && dgvData.Rows[i].Cells["seaover_company_code"].Value != null
                                    && !string.IsNullOrEmpty(dgvData.Rows[i].Cells["seaover_company_code"].Value.ToString().Trim()))
                                    list.Add(dgvData.Rows[i]);
                            }

                            if (list.Count > 0)
                            {
                                try
                                {
                                    CompanyGroupManager cgm = new CompanyGroupManager(um, sm, list);
                                    cgm.Owner = this;
                                    cgm.ShowDialog();
                                }
                                catch
                                { }
                                atoDt = sm.GetAtoDt();
                                int n = GetData();

                            }
                            else
                            {
                                messageBox.Show(this,"SEAOVER 거래처만 설정할 수 있습니다.");
                                this.Activate();
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
        #endregion

        
    }
}
