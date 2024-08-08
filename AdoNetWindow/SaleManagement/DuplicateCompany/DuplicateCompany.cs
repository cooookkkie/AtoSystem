using AdoNetWindow.Common;
using AdoNetWindow.Model;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;
using Repositories;
using Repositories.Company;
using Repositories.SalesPartner;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.SaleManagement.DuplicateCompany
{
    public partial class DuplicateCompany : Form
    {
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        ISalesRepository salesRepository = new SalesRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ICompanyRepository companyrepository = new CompanyRepository();
        ICompanyFinanceRepository companyFinanceRepository = new CompanyFinanceRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        bool isAdd;

        static Excel.Application excelApp = null;
        static Excel.Workbook workBook = null;
        static Excel.Worksheet workSheet = null;
        SalesManager sm = null;
        DataTable atoDt = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public DuplicateCompany(UsersModel uModel, SalesManager sm, DataTable atoDt, bool isAddFlag = false)
        {
            InitializeComponent();
            um = uModel;
            isAdd = isAddFlag;
            this.sm = sm;
            this.atoDt = atoDt;

            bool isExist = false;
            for (int i = 0; i < atoDt.Columns.Count; i++)
            {
                if (atoDt.Columns[i].ColumnName == "table_index")
                    isExist = true;
            }
            if(!isExist)
                atoDt.Columns.Add("table_index", typeof(int));

            for (int i = 0; i < atoDt.Rows.Count; i++)
            {
                atoDt.Rows[i]["table_index"] = i;
            }
            atoDt.AcceptChanges();

            if (isAdd)
                btnAddCompany.Visible = true;
            else
                btnAddCompany.Visible = false;
        }
        private void AddBusinessCompany_Load(object sender, EventArgs e)
        {
            salesRepository.SetSeaoverId(um.seaover_id);
            for (int i = 0; i < 1000; i++)
            {
                int n = dgvCompany.Rows.Add();
                dgvCompany.Rows[n].Cells["edit_user"].Value = um.user_name;
                dgvCompany.Rows[n].Cells["complete_duplicate"].Value = false;
            }
            txtTotal.Text = "1000";
            this.ActiveControl = dgvCompany;
        }

        #region Main method
        public void ExcelImport(string fileName, DataGridView dgv)
        {
            // 엑셀 문서 내용 추출
            string connectionString = string.Empty;
            string sheetName = string.Empty;

            // 파일 확장자 검사
            if (File.Exists(fileName))
            {
                /*if (Path.GetExtension(fileName).ToLower() == ".xls")
                {   // Microsoft.Jet.OLEDB.4.0 은 32 bit 에서만 동작되므로 빌드 할때 반드시 32bit로 할것(64bit로 하면 에러남)
                    connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0};Extended Properties=Excel 8.0;", fileName);
                }
                else if (Path.GetExtension(fileName).ToLower() == ".xlsx")
                {
                    connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=Excel 12.0;", fileName);
                }*/

                if (Path.GetExtension(fileName).ToLower().Contains(".xls"))
                {
                    connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=Excel 12.0;", fileName);
                }
                else
                    messageBox.Show(this, "엑셀파일이 아닙니다.(.xls*");
            }

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    con.Close();
                }
            }
            //엑셀 시트 이름 설정
            DataSet data = new DataSet();
            DataTable dt = new DataTable();

            string strQuery = "SELECT * FROM [" + sheetName + "]";
            OleDbConnection oleConn = new OleDbConnection(connectionString);
            oleConn.Open();

            OleDbCommand oleCmd = new OleDbCommand(strQuery, oleConn);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(oleCmd);

            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            data.Tables.Add(dataTable);

            //엑셀 컬럼 정보
            dgv.Rows.Clear();
            ExcelColumnManager ecm = new ExcelColumnManager(this, dataTable);
            ecm.Owner = this;
            DataTable columnDt = ecm.GetColumnSetting();
            if (columnDt != null)
            {
                Dictionary<string, string> columnDic = new Dictionary<string, string>();
                DataRow[] columnDr = columnDt.Select("input_column <> ''");
                if (columnDr.Length > 0)
                {
                    for (int i = 0; i < columnDr.Length; i++)
                    {
                        if (!columnDic.ContainsKey(columnDr[i]["excel_column"].ToString()))
                            columnDic.Add(columnDr[i]["excel_column"].ToString(), columnDr[i]["input_column_name"].ToString());
                        else
                            columnDic[columnDr[i]["excel_column"].ToString()] = columnDr[i]["input_column_name"].ToString();
                    }

                    //datagridview에 datatable(엑셀 데이터) 담기
                    //dgv.DataSource = dataTable;
                    dgv.Rows.Clear();
                    if (columnDic.Keys.Count > 0)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            int n = dgv.Rows.Add();
                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                if (columnDic.ContainsKey(dataTable.Columns[j].ColumnName))                                    
                                    dgv.Rows[n].Cells[columnDic[dataTable.Columns[j].ColumnName]].Value = dataTable.Rows[i][j].ToString();
                            }
                        }
                    }
                }
            }
            dataTable.Dispose();
            dataAdapter.Dispose();
            oleCmd.Dispose();

            oleConn.Close();
            oleConn.Dispose();


        }

        public void RemoveDuplicateCompany2()
        {
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            dgvCompany.EndEdit();
            for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
            {
                bool isRemove = false;

                dgvCompany.Rows[i].Cells["complete_duplicate"].Value = true;
                if (duplicateDic.ContainsKey(i))
                {
                    List<DataRow> list = duplicateDic[i];
                    DateTime min_current_sale_date = new DateTime(1900, 1, 1);
                    int common = 0, mydata = 0, potential1 = 0, petential2 = 0, trading = 0, nonehandled = 0, notFax = 0, outOfBusiness = 0;
                    List<string> managerList = new List<string>();
                    for (int k = 0; k < list.Count; k++)
                    {
                        //중복거래처 카테고리
                        if (!managerList.Contains(list[k]["ato_manager"].ToString()))
                        {
                            managerList.Add(list[k]["ato_manager"].ToString());
                            switch (list[k]["division"].ToString())
                            {
                                case "공용DATA":
                                    common++;
                                    break;
                                case "내DATA":
                                    mydata++;
                                    break;
                                case "잠재1":
                                    potential1++;
                                    break;
                                case "잠재2":
                                    petential2++;
                                    break;
                                case "거래중":
                                    {
                                        trading++;
                                        //최근매출
                                        DateTime current_sale_date;
                                        if (DateTime.TryParse(list[k]["current_sale_date"].ToString(), out current_sale_date) && min_current_sale_date < current_sale_date)
                                            min_current_sale_date = current_sale_date;
                                    }
                                    break;
                                case "취급X":
                                    nonehandled++;
                                    break;
                                case "팩스X":
                                    notFax++;
                                    break;
                                case "폐업":
                                    outOfBusiness++;
                                    break;
                            }
                        }
                    }
                    //최근매출 8개월 미만 SEAOVER 거래처
                    if (trading > 0 && min_current_sale_date >= DateTime.Now.AddMonths(-8))
                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                    //중복거래처 3개 초과 삭제
                    else if (3 <= common + mydata + potential1 + petential2 + trading)
                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                }
            }           

            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this, "데이터 등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
            }
        }
        public void RemoveDuplicateCompany(DataTable limitDt)
        {
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            dgvCompany.EndEdit();
            for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
            {
                bool isRemove = false;

                if (duplicateDic.ContainsKey(i))
                {
                    List<DataRow> list = duplicateDic[i];

                    for (int j = 0; j < limitDt.Rows.Count; j++)
                    {
                        if (isRemove)
                            break;

                        bool isLimintCount, isLimintTerms;
                        int limitCount, limitTerms;
                        //거래처 제한수
                        if (!bool.TryParse(limitDt.Rows[j]["cbLimitCount"].ToString(), out isLimintCount))
                            isLimintCount = false;
                        if (isLimintCount)
                        {
                            if (!int.TryParse(limitDt.Rows[j]["limit_count"].ToString(), out limitCount))
                                limitCount = 0;

                            int duplicateCnt = 0;
                            if (limitDt.Rows[j]["division"].ToString() == "중복 거래처")
                            {
                                int common = 0, mydata = 0, potential1 = 0, petential2 = 0, trading = 0, nonehandled = 0, notFax = 0, outOfBusiness = 0;
                                List<string> managerList = new List<string>();
                                for (int k = 0; k < list.Count; k++)
                                {
                                    if (!managerList.Contains(list[k]["ato_manager"].ToString()))
                                    {
                                        managerList.Add(list[k]["ato_manager"].ToString());
                                        switch (list[k]["division"].ToString())
                                        {
                                            case "공용DATA":
                                                common++;
                                                break;
                                            case "내DATA":
                                                mydata++;
                                                break;
                                            case "잠재1":
                                                potential1++;
                                                break;
                                            case "잠재2":
                                                petential2++;
                                                break;
                                            case "거래중":
                                                trading++;
                                                break;
                                            case "취급X":
                                                nonehandled++;
                                                break;
                                            case "팩스X":
                                                notFax++;
                                                break;
                                            case "폐업":
                                                outOfBusiness++;
                                                break;
                                        }
                                    }
                                }
                                //삭제확정
                                duplicateCnt += common + mydata + potential1 + petential2 + trading;
                                if (limitCount <= duplicateCnt)
                                {
                                    isRemove = true;
                                    break;
                                }
                            }
                            else if (limitDt.Rows[j]["division"].ToString() == "8개월 미만 영업 SEAOVER거래처")
                            {
                                int common = 0, mydata = 0, potential1 = 0, petential2 = 0, trading = 0, nonehandled = 0, notFax = 0, outOfBusiness = 0;
                                List<string> managerList = new List<string>();
                                for (int k = 0; k < list.Count; k++)
                                {
                                    if (!managerList.Contains(list[k]["ato_manager"].ToString()))
                                    {
                                        managerList.Add(list[k]["ato_manager"].ToString());
                                        switch (list[k]["is_duplicate"].ToString())
                                        {
                                            case "공용DATA":
                                                common++;
                                                break;
                                            case "내DATA":
                                                mydata++;
                                                break;
                                            case "잠재1":
                                                potential1++;
                                                break;
                                            case "잠재2":
                                                petential2++;
                                                break;
                                            case "거래중":
                                                trading++;
                                                break;
                                            case "취급X":
                                                nonehandled++;
                                                break;
                                            case "팩스X":
                                                notFax++;
                                                break;
                                            case "폐업":
                                                outOfBusiness++;
                                                break;
                                        }
                                    }
                                }
                                //삭제확정
                                duplicateCnt += trading;
                                //삭제확정
                                if (limitCount <= duplicateCnt)
                                {
                                    isRemove = true;
                                    break;
                                }
                            }
                            else if (limitDt.Rows[j]["division"].ToString() == "취급X")
                            {
                                List<string> managerList = new List<string>();
                                for (int k = 0; k < list.Count; k++)
                                {
                                    if (list[k]["division"].ToString() == "취급X")
                                    {
                                        //취급X 수정일자 최신화============================================================================
                                        int company_id = Convert.ToInt32(list[k]["company_id"].ToString());
                                        CompanySalesModel sModel = new CompanySalesModel();
                                        sModel.company_id = company_id;
                                        sModel.sub_id = commonRepository.GetNextId("t_company_sales", "sub_id", "company_id", company_id.ToString());
                                        sModel.is_sales = false;
                                        sModel.contents = "취급X 중복검사";
                                        sModel.log = "";
                                        sModel.remark = "";
                                        sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        sModel.edit_user = um.user_name;

                                        sql = salesPartnerRepository.InsertPartnerSales(sModel);
                                        sqlList.Add(sql);

                                        isRemove = true;

                                        int table_index = Convert.ToInt32(list[k]["table_index"].ToString());
                                        sm.UpdateAtoDt(table_index, "sales_contents", "취급X 중복검사");
                                        sm.UpdateAtoDt(table_index, "sales_updatetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    }
                                }
                            }
                        }

                        //거래처 영업기간 제한
                        if (!bool.TryParse(limitDt.Rows[j]["cbLimitTerms"].ToString(), out isLimintTerms))
                            isLimintTerms = false;
                        if (!isRemove && isLimintTerms)
                        {
                            if (!int.TryParse(limitDt.Rows[j]["limit_terms"].ToString(), out limitTerms))
                                limitTerms = 0;

                            for (int k = 0; k < list.Count; k++)
                            {
                                string division = list[k]["division"].ToString();
                                DateTime updatetime;
                                if (DateTime.TryParse(list[k]["current_sale_date"].ToString(), out updatetime))
                                {
                                    TimeSpan sp = DateTime.Now - updatetime;
                                    if (sp.Days / 30 < limitTerms)
                                    {
                                        if (limitDt.Rows[j]["division"].ToString() == "중복 거래처")
                                        {
                                            isRemove = true;
                                            break;
                                        }
                                        else if (limitDt.Rows[j]["division"].ToString() == "1년미만 영업 SEAOVER거래처" && division == "거래중")
                                        {
                                            isRemove = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                //삭제
                if (isRemove)
                    dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
            }

            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    messageBox.Show(this, "데이터 등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
            }
        }

        //Record수 세기
        private void SetRecordCounting()
        {
            dgvCompany.Update();
            txtTotal.Text = dgvCompany.Rows.Count.ToString();
            txtCurrent.Text = "0";
            if (dgvCompany.Rows.Count > 0)
            {
                if (dgvCompany.SelectedCells.Count > 0)
                    txtCurrent.Text = (dgvCompany.SelectedCells[0].RowIndex + 1).ToString();
                else if (dgvCompany.SelectedRows.Count > 0)
                    txtCurrent.Text = (dgvCompany.SelectedRows[0].Index + 1).ToString();
            }
        }
        //List => Datatable
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
        private bool isValidation(DataGridViewRow row)
        {
            bool isFlag = true;

            //거래처명
            if (row.Cells["company"].Value == null || string.IsNullOrEmpty(row.Cells["company"].Value.ToString().Trim()))
            {
                messageBox.Show(this,"거래처명은 필수입니다.");
                this.Activate();
                row.Selected = true;
                return false;
            }
            row.Cells["company"].Value = row.Cells["company"].Value.ToString().Replace("'", "");
            //전화번호
            if (row.Cells["tel"].Value == null)
                row.Cells["tel"].Value = "";
            row.Cells["tel"].Value = row.Cells["tel"].Value.ToString().Replace("/", "");
            if (row.Cells["fax"].Value == null)
                row.Cells["fax"].Value = "";
            row.Cells["fax"].Value = row.Cells["fax"].Value.ToString().Replace("/", "");
            if (row.Cells["phone"].Value == null)
                row.Cells["phone"].Value = "";
            row.Cells["phone"].Value = row.Cells["phone"].Value.ToString().Replace("/", "");
            if (row.Cells["other_phone"].Value == null)
                row.Cells["other_phone"].Value = "";
            row.Cells["other_phone"].Value = row.Cells["other_phone"].Value.ToString().Replace("/", "");


            if (row.Cells["tel"].Value != null && !string.IsNullOrEmpty(row.Cells["tel"].Value.ToString()))
            {
                string tel = row.Cells["tel"].Value.ToString();
                tel = tel.Replace(" ", "");
                tel = tel.Replace("-", "");
                tel = tel.Replace("(", "");
                tel = tel.Replace(")", "");

                if (tel.Length < 7)
                {
                    isFlag = false;
                    return isFlag;
                }

                if(tel.Length == 8 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "010" + tel;
                else if (tel.Length > 2 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "0" + tel;
                row.Cells["tel"].Value = tel;
                double n;
                bool isNumeric = double.TryParse(tel, out n);
                if (!isNumeric)
                {
                    isFlag = false;
                    return isFlag;
                }
                else if (!(tel.Length == 8 || tel.Length == 10 || tel.Length == 11))
                {
                    isFlag = false;
                    return isFlag;
                }
            }
            //팩스번호
            if (row.Cells["fax"].Value != null && !string.IsNullOrEmpty(row.Cells["fax"].Value.ToString()))
            {
                string tel = row.Cells["fax"].Value.ToString();
                tel = tel.Replace(" ", "");
                tel = tel.Replace("-", "");
                tel = tel.Replace("(", "");
                tel = tel.Replace(")", "");

                if (tel.Length < 7)
                {
                    isFlag = false;
                    return isFlag;
                }

                if (tel.Length == 8 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "010" + tel;
                else if (tel.Length > 2 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "0" + tel;
                row.Cells["fax"].Value = tel;
                double n;
                bool isNumeric = double.TryParse(tel, out n);
                if (!isNumeric)
                {
                    isFlag = false;
                    return isFlag;
                }
                else if (!(tel.Length == 8 || tel.Length == 10 || tel.Length == 11))
                {
                    isFlag = false;
                    return isFlag;
                }
            }
            //휴대폰
            if (row.Cells["phone"].Value != null && !string.IsNullOrEmpty(row.Cells["phone"].Value.ToString()))
            {
                string tel = row.Cells["phone"].Value.ToString();
                tel = tel.Replace(" ", "");
                tel = tel.Replace("-", "");
                tel = tel.Replace("(", "");
                tel = tel.Replace(")", "");

                if (tel.Length < 7)
                {
                    isFlag = false;
                    return isFlag;
                }

                if (tel.Length == 8 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "010" + tel;
                else if (tel.Length > 2 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "0" + tel;
                row.Cells["phone"].Value = tel;
                double n;
                bool isNumeric = double.TryParse(tel, out n);
                if (!isNumeric)
                {
                    isFlag = false;
                    return isFlag;
                }
                else if (!(tel.Length == 8 || tel.Length == 10 || tel.Length == 11))
                {
                    isFlag = false;
                    return isFlag;
                }
            }
            //기타연락처
            if (row.Cells["other_phone"].Value != null && !string.IsNullOrEmpty(row.Cells["other_phone"].Value.ToString()))
            {
                string tel = row.Cells["other_phone"].Value.ToString();
                tel = tel.Replace(" ", "");
                tel = tel.Replace("-", "");
                tel = tel.Replace("(", "");
                tel = tel.Replace(")", "");

                if (tel.Length < 7)
                {
                    isFlag = false;
                    return isFlag;
                }

                if (tel.Length == 8 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "010" + tel;
                else if (tel.Length > 2 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "0" + tel;
                row.Cells["other_phone"].Value = tel;
                double n;
                bool isNumeric = double.TryParse(tel, out n);
                if (!isNumeric)
                {
                    isFlag = false;
                    return isFlag;
                }
                else if (!(tel.Length == 8 || tel.Length == 10 || tel.Length == 11))
                {
                    isFlag = false;
                    return isFlag;
                }
            }
            //사업자등록번호
            if (row.Cells["registration_number"].Value != null && !string.IsNullOrEmpty(row.Cells["registration_number"].Value.ToString()))
            {
                string registration_number = row.Cells["registration_number"].Value.ToString();
                registration_number = registration_number.Replace(" ", "");
                registration_number = registration_number.Replace("-", "");

                row.Cells["registration_number"].Value = registration_number;
            }
            return isFlag;
        }
        #endregion

        #region Button, Checkbox, ComboBox
        private void cbFinance_CheckedChanged(object sender, EventArgs e)
        {
            dgvCompany.Columns["div"].Visible = cbFinance.Checked;
            dgvCompany.Columns["finance_year"].Visible = cbFinance.Checked;
            dgvCompany.Columns["capital_amount"].Visible = cbFinance.Checked;
            dgvCompany.Columns["debt_amount"].Visible = cbFinance.Checked;
            dgvCompany.Columns["sales_amount"].Visible = cbFinance.Checked;
            dgvCompany.Columns["net_income_amount"].Visible = cbFinance.Checked;
        }

        private void cbHybridAddCompany_CheckedChanged(object sender, EventArgs e)
        {
            cbAddNewCompany.Checked = false;
            cbOldCompanyUpdate.Checked = false;
        }
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (!cbDuplicate.Checked && !cbNoneHandling.Checked && !cbNotSendFax.Checked)
            {
                messageBox.Show(this, "삭제할 중복 체크박스를 선택해주세요!");
                return;
            }

            if (messageBox.Show(this, "선택한 중복건 행 삭제 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;


            for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
            {
                int common_duplicate_count;
                if (dgvCompany.Rows[i].Cells["common_duplicate_count"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["common_duplicate_count"].Value.ToString(), out common_duplicate_count))
                    common_duplicate_count = 0;

                int mydata_duplicate_count;
                if (dgvCompany.Rows[i].Cells["mydata_duplicate_count"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["mydata_duplicate_count"].Value.ToString(), out mydata_duplicate_count))
                    mydata_duplicate_count = 0;

                int potential_duplicate_count;
                if (dgvCompany.Rows[i].Cells["potential_duplicate_count"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["potential_duplicate_count"].Value.ToString(), out potential_duplicate_count))
                    potential_duplicate_count = 0;

                int potential2_duplicate_count;
                if (dgvCompany.Rows[i].Cells["potential2_duplicate_count"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["potential2_duplicate_count"].Value.ToString(), out potential2_duplicate_count))
                    potential2_duplicate_count = 0;

                int isTrading_duplicate_count;
                if (dgvCompany.Rows[i].Cells["isTrading_duplicate_count"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["isTrading_duplicate_count"].Value.ToString(), out isTrading_duplicate_count))
                    isTrading_duplicate_count = 0;

                int nonehandled_duplicate_count;
                if (dgvCompany.Rows[i].Cells["nonehandled_duplicate_count"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["nonehandled_duplicate_count"].Value.ToString(), out nonehandled_duplicate_count))
                    nonehandled_duplicate_count = 0;

                if (dgvCompany.Rows[i].Cells["is_duplicate"].Value != null
                    && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["is_duplicate"].Value.ToString())
                    && common.isContains(dgvCompany.Rows[i].Cells["is_duplicate"].Value.ToString(), "취급X"))
                    nonehandled_duplicate_count++;

                int notSendFax_duplicate_count;
                if (dgvCompany.Rows[i].Cells["notSendFax_duplicate_count"].Value == null || !int.TryParse(dgvCompany.Rows[i].Cells["notSendFax_duplicate_count"].Value.ToString(), out notSendFax_duplicate_count))
                    notSendFax_duplicate_count = 0;

                bool isNotSendFax;
                if (dgvCompany.Rows[i].Cells["isNotSendFax"].Value == null || bool.TryParse(dgvCompany.Rows[i].Cells["isNotSendFax"].Value.ToString(), out isNotSendFax))
                    isNotSendFax = false;

                //중복이 하나라도 있으면 삭제
                if (cbDuplicate.Checked && (common_duplicate_count + mydata_duplicate_count + potential_duplicate_count + potential2_duplicate_count + isTrading_duplicate_count) > 0)
                    dgvCompany.Rows.RemoveAt(i);
                //취급X 삭제
                if (cbNoneHandling.Checked && nonehandled_duplicate_count > 0)
                    dgvCompany.Rows.RemoveAt(i);
                //팩스X 삭제
                if (cbNotSendFax.Checked && isNotSendFax)
                    dgvCompany.Rows.RemoveAt(i);
            }

            messageBox.Show(this, "완료!");
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            GetExeclColumn();
        }
        private void btnGetExcelData_Click(object sender, EventArgs e)
        {
            try
            {
                //파일 선택 창 오픈
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    //파일 형식 필터
                    dlg.Filter = "Excel Files(2007이상)|*.xls*";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        //파일 위치 선택 후 Ok 클릭 했을 경우 Import 메서드를 호출
                        ExcelImport(dlg.FileName, dgvCompany);
                    }
                }
            }
            catch (Exception ex)
            {
                messageBox.Show(this,ex.Message);
                this.Activate();
                throw;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtDataSource.Text = String.Empty;
            dgvCompany.Columns["is_duplicate"].Visible = true;
            dgvCompany.Columns["common_duplicate_count"].Visible = false;
            dgvCompany.Columns["mydata_duplicate_count"].Visible = false;
            dgvCompany.Columns["potential_duplicate_count"].Visible = false;
            dgvCompany.Columns["potential2_duplicate_count"].Visible = false;
            dgvCompany.Columns["isTrading_duplicate_count"].Visible = false;
            dgvCompany.Rows.Clear();
            for (int i = 0; i < 1000; i++)
            {
                int n = dgvCompany.Rows.Add();
                dgvCompany.Rows[n].Cells["edit_user"].Value = um.user_name;
                dgvCompany.Rows[n].Cells["complete_duplicate"].Value = false;
            }
            SetRecordCounting();
        }
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            if (isAdd)
                InsertNotTreatCompany();
            /*if (um.auth_level > 50)
                InsertCompany();
            else
                InsertNotTreatCompany();*/
        }
        private void InsertNotTreatCompany()
        {
            //유효성검사
            if (!isAdd)
                return;
            if (dgvCompany.Rows.Count == 0)
            {
                messageBox.Show(this,"등록할 데이터가 없습니다.");
                this.Activate();
                return;
            }
            //데이터 유효성검사
            List<DataGridViewRow> NoneHandledList = new List<DataGridViewRow>();
            int errCnt = 0;
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                foreach (DataGridViewCell cell in dgvCompany.Rows[i].Cells)
                {
                    if (cell.OwningColumn.Name == "complete_duplicate" && (cell.Value == null || cell.Value.ToString() == String.Empty))
                        cell.Value = false;
                    else if (cell.Value == null)
                        cell.Value = string.Empty;
                }

                if (dgvCompany.Rows[i].Cells["company"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["company"].Value.ToString())
                    && ((dgvCompany.Rows[i].Cells["tel"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["tel"].Value.ToString()))
                    || (dgvCompany.Rows[i].Cells["fax"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["fax"].Value.ToString()))
                    || (dgvCompany.Rows[i].Cells["phone"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["phone"].Value.ToString()))
                    || (dgvCompany.Rows[i].Cells["other_phone"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["other_phone"].Value.ToString())))
                    )
                {
                    bool isFlag = isValidation(dgvCompany.Rows[i]);
                    if (!isFlag)
                    {
                        errCnt++;
                        dgvCompany.Rows[i].HeaderCell.Style.BackColor = Color.Red;
                    }
                    else
                        dgvCompany.Rows[i].HeaderCell.Style.BackColor = Color.LightGray;
                }
                else if (!Convert.ToBoolean(dgvCompany.Rows[i].Cells["complete_duplicate"].Value))
                {
                    messageBox.Show(this,"중복검사를 하지 않은 내역이 있습니다. 중복검사 검사후 등록해주시기 바랍니다!");
                    this.Activate();
                    return;
                }

                //취급X거래처
                if (dgvCompany.Rows[i].Cells["nonehandled_duplicate_count"].Value != null && int.TryParse(dgvCompany.Rows[i].Cells["nonehandled_duplicate_count"].Value.ToString(), out int nonehandled_duplicate_count) && nonehandled_duplicate_count > 0)
                    NoneHandledList.Add(dgvCompany.Rows[i]);
                //폐업거래처
                else if (dgvCompany.Rows[i].Cells["outOfBusiness_duplicate_count"].Value != null && int.TryParse(dgvCompany.Rows[i].Cells["outOfBusiness_duplicate_count"].Value.ToString(), out int outOfBusiness_duplicate_count) && outOfBusiness_duplicate_count > 0)
                    NoneHandledList.Add(dgvCompany.Rows[i]);

                
            }
            //데이터 유형검사 에러 메세지
            if (errCnt > 0)
            {
                if (messageBox.Show(this, "전화번호 형식에 맞지않는 값이 있습니다. 무시하고 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }
            //취급X 거래처 처리
            if (NoneHandledList.Count > 0)
            {
                try
                {
                    NotHandlingCompanyList nhcl = new NotHandlingCompanyList(NoneHandledList, this);
                    DataTable resultDt =  nhcl.GetNoneHandledList();
                    if(resultDt != null && resultDt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in resultDt.Rows)
                        {
                            int rowindex = Convert.ToInt32(dr["rowindex"].ToString());
                            dgvCompany.Rows[rowindex].Cells["isIgnore"].Value = Convert.ToBoolean(dr["isIgnore"].ToString());
                            dgvCompany.Rows[rowindex].Cells["isRecovery"].Value = Convert.ToBoolean(dr["isRecovery"].ToString());
                            dgvCompany.Rows[rowindex].Cells["isDelete"].Value = Convert.ToBoolean(dr["isDelete"].ToString());

                        }
                    }
                    //삭제거래처는 삭제
                    dgvCompany.EndEdit();
                    for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dgvCompany.Rows[i].Cells["isDelete"].Value != null 
                            && bool.TryParse(dgvCompany.Rows[i].Cells["isDelete"].Value.ToString(), out bool isDelete) 
                            && isDelete)
                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                    }
                }
                catch
                { }
            }


            //거래처 구분
            SelectCompanyDivision scd = new SelectCompanyDivision(um);

            string[] duplicates;
            int potential_limit_count;
            string division = scd.SelectDivision(out duplicates, out potential_limit_count);
            if (division == null)
                return;
            else if(division != "공용DATA" && division != "취급X" && division != "팩스X")
            {
                foreach (DataGridViewRow dr in dgvCompany.Rows)
                {
                    if (dr.Cells["edit_user"].Value == null || string.IsNullOrEmpty(dr.Cells["edit_user"].Value.ToString()))
                    {
                        if (messageBox.Show(this, "담당자가 입력되지 않은 행이 발견되었습니다. 현재 사용자를 담당자로 입력후 저장을 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;

                        foreach (DataGridViewRow ddr in dgvCompany.Rows)
                        {
                            if (ddr.Cells["edit_user"].Value == null || string.IsNullOrEmpty(ddr.Cells["edit_user"].Value.ToString()))
                                ddr.Cells["edit_user"].Value = um.user_name;
                        }

                        break;
                    }
                }
            }

            //마지막 MSG
            if (messageBox.Show(this, "[" + division + "] 으로 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            //ProgressBar
            Common.ProgressBar pb = new Common.ProgressBar();
            pb.SetStyle(3);
            pb.TopMost = true;
            pb.Show();
            pb.Update();

            //실행시간
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();

            //데이터 등록
            List<CompanyModel> insertList = new List<CompanyModel>();
            List<CompanyModel> updateList = new List<CompanyModel>();
            List<CompanyModel> DeleteList = new List<CompanyModel>();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            int id = commonRepository.GetNextId("t_company", "id");
            //int table_index = atoDt.Rows.Count;
            //for (int i = 0; i < dgvCompany.Rows.Count; i++)
            foreach(DataGridViewRow row in dgvCompany.Rows)
            {
                //DataGridViewRow row = dgvCompany.Rows[i];

                /*foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                        cell.Value = string.Empty;
                }*/
                //INSERT MODEL
                if (row.Cells["company"].Value != null && !string.IsNullOrEmpty(row.Cells["company"].Value.ToString()))
                {
                    CompanyModel cm = new CompanyModel();
                    cm.id = id;
                    cm.division = "매출처";
                    cm.registration_number = row.Cells["registration_number"].Value.ToString();
                    cm.group_name = row.Cells["group_name"].Value.ToString();
                    cm.name = row.Cells["company"].Value.ToString();
                    //cm.seaover_company_code = row.Cells["seaover_company_code"].Value.ToString();
                    cm.origin = "국내";
                    cm.address = row.Cells["address"].Value.ToString();
                    cm.ceo = row.Cells["ceo"].Value.ToString();
                    cm.fax = row.Cells["fax"].Value.ToString();
                    cm.tel = row.Cells["tel"].Value.ToString();
                    cm.phone = row.Cells["phone"].Value.ToString();
                    cm.other_phone = row.Cells["other_phone"].Value.ToString();
                    cm.address = row.Cells["address"].Value.ToString();
                    cm.company_manager = row.Cells["manager"].Value.ToString();
                    cm.company_manager_position = row.Cells["position"].Value.ToString();
                    cm.email = row.Cells["email"].Value.ToString();
                    cm.sns1 = "";
                    cm.sns2 = "";
                    cm.sns3 = "";
                    cm.web = row.Cells["web"].Value.ToString();
                    cm.remark = row.Cells["remark"].Value.ToString();
                    cm.distribution = row.Cells["distribution"].Value.ToString();
                    cm.handling_item = row.Cells["handling_item"].Value.ToString();
                    cm.remark2 = "";

                    cm.isManagement1 = false;
                    cm.isManagement2 = false;
                    cm.isManagement3 = false;
                    cm.isManagement4 = false;
                    cm.isHide = false;
                    cm.isDelete = false;
                    cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    cm.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    cm.edit_user = um.user_name;
                    cm.table_index = row.Cells["duplicate_table_index"].Value.ToString();
                    cm.industry_type = row.Cells["industry_type"].Value.ToString();

                    bool isNotSendFax;
                    if (row.Cells["isNotSendFax"].Value == null || !bool.TryParse(row.Cells["isNotSendFax"].Value.ToString(), out isNotSendFax))
                        isNotSendFax = false;
                    cm.isNotSendFax = isNotSendFax;

                    //거래처구분
                    switch (division)
                    {
                        case "공용DATA":
                            cm.ato_manager = "";
                            cm.isPotential1 = false;
                            cm.isPotential2 = false;
                            cm.isNonHandled = false;
                            cm.isOutBusiness = false;
                            
                            break;
                        case "내DATA":
                            cm.ato_manager = row.Cells["edit_user"].Value.ToString();
                            cm.isPotential1 = false;
                            cm.isPotential2 = false;
                            cm.isNonHandled = false;
                            cm.isOutBusiness = false;

                            break;
                        case "잠재1":
                            cm.ato_manager = row.Cells["edit_user"].Value.ToString();

                            cm.isPotential1 = true;
                            cm.isPotential2 = false;
                            cm.isNonHandled = false;
                            cm.isOutBusiness = false;
                            
                            break;
                        case "잠재2":
                            cm.ato_manager = row.Cells["edit_user"].Value.ToString();

                            cm.isPotential1 = false;
                            cm.isPotential2 = true;
                            cm.isNonHandled = false;
                            cm.isOutBusiness = false;
                            
                            break;
                        default:
                            cm.isPotential1 = false;
                            cm.isPotential2 = false;
                            //cm.edit_user = "";
                            if (division.Contains("취급X"))
                                cm.isNonHandled = true;
                            else
                                cm.isNonHandled = false;

                            if (division.Contains("팩스X"))
                                cm.isNotSendFax = true;

                            if (division.Contains("폐업"))
                                cm.isOutBusiness = true;
                            else
                                cm.isOutBusiness = false;
                            break;
                    }

                    int year;
                    //Insert
                    StringBuilder sql;
                    //기존데이터가 있으면 최신화 없으면 신규등록
                    if (cbHybridAddCompany.Checked)
                    {
                        //없는 경우 신규등록
                        if (row.Cells["duplicate_company_id"].Value == null || string.IsNullOrEmpty(row.Cells["duplicate_company_id"].Value.ToString()))
                        {
                            //취급X 거래처 복구 경우
                            if (NoneHandledList.Count > 0
                                && row.Cells["isRecovery"].Value != null && bool.TryParse(row.Cells["isRecovery"].Value.ToString(), out bool isRecovery) && isRecovery)
                            {
                                string[] duplicate_ids = row.Cells["duplicate_table_index"].Value.ToString().Split(' ');
                                if (duplicate_ids.Length > 0)
                                {
                                    foreach (string duplicate_id in duplicate_ids)
                                    {
                                        if (!string.IsNullOrEmpty(duplicate_id) && Int32.TryParse(duplicate_id, out Int32 table_index)
                                            && bool.TryParse(atoDt.Rows[table_index]["isNonHandled"].ToString(), out bool isNonHandled) && isNonHandled)
                                        {
                                            //데이터 통합
                                            if (string.IsNullOrEmpty(cm.ceo))
                                                cm.ceo = atoDt.Rows[table_index]["ceo"].ToString();
                                            if (string.IsNullOrEmpty(cm.registration_number))
                                                cm.registration_number = atoDt.Rows[table_index]["registration_number"].ToString();
                                            if (string.IsNullOrEmpty(cm.tel))
                                                cm.tel = atoDt.Rows[table_index]["tel"].ToString();
                                            if (string.IsNullOrEmpty(cm.fax))
                                                cm.fax = atoDt.Rows[table_index]["fax"].ToString();
                                            if (string.IsNullOrEmpty(cm.phone))
                                                cm.phone = atoDt.Rows[table_index]["phone"].ToString();
                                            if (string.IsNullOrEmpty(cm.other_phone))
                                                cm.other_phone = atoDt.Rows[table_index]["other_phone"].ToString();
                                            if (string.IsNullOrEmpty(cm.email))
                                                cm.email = atoDt.Rows[table_index]["email"].ToString();
                                            if (string.IsNullOrEmpty(cm.address))
                                                cm.address = atoDt.Rows[table_index]["address"].ToString();
                                            if (string.IsNullOrEmpty(cm.handling_item))
                                                cm.handling_item = atoDt.Rows[table_index]["handling_item"].ToString();
                                            if (string.IsNullOrEmpty(cm.distribution))
                                                cm.distribution = atoDt.Rows[table_index]["distribution"].ToString();
                                            if (string.IsNullOrEmpty(cm.payment_date))
                                                cm.payment_date = atoDt.Rows[table_index]["payment_date"].ToString();
                                            if (string.IsNullOrEmpty(cm.remark))
                                                cm.remark = atoDt.Rows[table_index]["remark"].ToString();
                                            if (string.IsNullOrEmpty(cm.industry_type))
                                                cm.industry_type = atoDt.Rows[table_index]["industry_type"].ToString();
                                            if (!cm.isNotSendFax)
                                                cm.isNotSendFax = Convert.ToBoolean(atoDt.Rows[table_index]["isNotSendFax"].ToString());

                                            //취급X 거래처 -> 삭제거래처
                                            CompanyModel dcm = new CompanyModel();
                                            dcm.table_index = table_index.ToString();
                                            DeleteList.Add(dcm);

                                            //실제 서버삭제
                                            sql = companyrepository.DeleteCompany(Convert.ToInt32(atoDt.Rows[table_index]["id"].ToString()), true);
                                            sqlList.Add(sql);
                                        }
                                    }
                                }
                            }
                            //팩스X 기존 거래처는 수정하면되는데 신규 팩스x는 있을곳이 애매하다. 그래서 isHide로 팩스x 탭에서만 보이게
                            //2023-11-16 수정
                            if (division == "팩스X")
                                cm.isHide = true;
                            //데이터 등록
                            sql = companyrepository.InsertCompany(cm);
                            sqlList.Add(sql);
                            insertList.Add(cm);

                            //재무재표 등록
                            if (row.Cells["finance_year"].Value != null && int.TryParse(row.Cells["finance_year"].Value.ToString(), out year))
                            {
                                CompanyFinanceModel fModel = new CompanyFinanceModel();
                                fModel.year = year;

                                if (row.Cells["capital_amount"].Value != null && int.TryParse(row.Cells["capital_amount"].Value.ToString(), out int capital_amount))
                                    fModel.capital_amount = capital_amount;

                                if (row.Cells["debt_amount"].Value != null && int.TryParse(row.Cells["debt_amount"].Value.ToString(), out int debt_amount))
                                    fModel.debt_amount = debt_amount;

                                if (row.Cells["sales_amount"].Value != null && int.TryParse(row.Cells["sales_amount"].Value.ToString(), out int sales_amount))
                                    fModel.sales_amount = sales_amount;

                                if (row.Cells["net_income_amount"].Value != null && int.TryParse(row.Cells["net_income_amount"].Value.ToString(), out int net_income_amount))
                                    fModel.net_income_amount = net_income_amount;

                                //매출금액이 있는 내용 기준으로 등록
                                if (fModel.capital_amount > 0 || fModel.debt_amount > 0 || fModel.sales_amount > 0 || fModel.net_income_amount > 0)

                                {
                                    fModel.edit_user = um.user_name;
                                    fModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                    fModel.company_id = cm.id;
                                    sql = companyFinanceRepository.DeleteCompanyFinanc(year.ToString(), cm.id.ToString());
                                    sqlList.Add(sql);

                                    sql = companyFinanceRepository.InsertCompanyFinanc(fModel);
                                    sqlList.Add(sql);
                                }
                            }
                        }
                        //있으면 기존내역 수정
                        else
                        {
                            string[] table_indexs = row.Cells["duplicate_table_index"].Value.ToString().Trim().Split(' ');
                            foreach (string table_index in table_indexs)
                            {
                                if (int.TryParse(table_index, out int idx) && int.TryParse(atoDt.Rows[idx]["id"].ToString(), out int company_id) && company_id > 0)
                                {
                                    DataRow atoDr = atoDt.Rows[idx];

                                    if (string.IsNullOrEmpty(atoDr["group_name"].ToString()))
                                        atoDr["group_name"] = cm.group_name;

                                    if (string.IsNullOrEmpty(atoDr["ceo"].ToString()))
                                        atoDr["ceo"] = cm.ceo;

                                    if (string.IsNullOrEmpty(atoDr["registration_number"].ToString()))
                                        atoDr["registration_number"] = cm.registration_number;

                                    if (string.IsNullOrEmpty(atoDr["tel"].ToString()))
                                        atoDr["tel"] = cm.tel;

                                    if (string.IsNullOrEmpty(atoDr["fax"].ToString()))
                                        atoDr["fax"] = cm.fax;

                                    if (string.IsNullOrEmpty(atoDr["phone"].ToString()))
                                        atoDr["phone"] = cm.phone;

                                    if (string.IsNullOrEmpty(atoDr["other_phone"].ToString()))
                                        atoDr["other_phone"] = cm.other_phone;

                                    if (string.IsNullOrEmpty(atoDr["email"].ToString()))
                                        atoDr["email"] = cm.email;

                                    if (string.IsNullOrEmpty(atoDr["address"].ToString()))
                                        atoDr["address"] = cm.address;

                                    if (string.IsNullOrEmpty(atoDr["handling_item"].ToString()))
                                        atoDr["handling_item"] = cm.handling_item;

                                    if (string.IsNullOrEmpty(atoDr["distribution"].ToString()))
                                        atoDr["distribution"] = cm.distribution;

                                    if (string.IsNullOrEmpty(atoDr["remark"].ToString()))
                                        atoDr["remark"] = cm.remark;

                                    if (string.IsNullOrEmpty(atoDr["industry_type"].ToString()))
                                        atoDr["industry_type"] = cm.industry_type;

                                    //수정컬럼
                                    string[] col = new string[15];
                                    col[0] = "group_name";
                                    col[1] = "ceo";
                                    col[2] = "registration_number";
                                    col[3] = "tel";
                                    col[4] = "fax";
                                    col[5] = "phone";
                                    col[6] = "other_phone";
                                    col[7] = "email";
                                    col[8] = "address";
                                    col[9] = "handling_item";
                                    col[10] = "distribution";
                                    col[11] = "remark";
                                    col[12] = "industry_type";
                                    col[13] = "isNotSendFax";
                                    col[14] = "edit_user";
                                    //수정값
                                    string[] val = new string[15];
                                    val[0] = "'" + atoDr["group_name"].ToString() + "'";
                                    val[1] = "'" + atoDr["ceo"].ToString() + "'";
                                    val[2] = "'" + atoDr["registration_number"].ToString() + "'";
                                    val[3] = "'" + atoDr["tel"].ToString() + "'";
                                    val[4] = "'" + atoDr["fax"].ToString() + "'";
                                    val[5] = "'" + atoDr["phone"].ToString() + "'";
                                    val[6] = "'" + atoDr["other_phone"].ToString() + "'";
                                    val[7] = "'" + common.AddSlashes(atoDr["email"].ToString()) + "'";
                                    val[8] = "'" + common.AddSlashes(atoDr["address"].ToString()) + "'";
                                    val[9] = "'" + common.AddSlashes(atoDr["handling_item"].ToString()) + "'";
                                    val[10] = "'" + common.AddSlashes(atoDr["distribution"].ToString()) + "'";
                                    val[11] = "'" + common.AddSlashes(atoDr["remark"].ToString()) + "'";
                                    val[12] = "'" + common.AddSlashes(atoDr["industry_type"].ToString()) + "'";
                                    val[13] = cm.isNotSendFax.ToString();
                                    val[14] = "'" + um.user_name + "'";

                                    //조건
                                    string whr = $"id = {atoDr["id"].ToString()}";
                                    //Update data
                                    sql = companyrepository.UpdateCompanyColumns(col, val, whr);
                                    sqlList.Add(sql);

                                    //재무재표 등록
                                    if (cbFinance.Checked && row.Cells["finance_year"].Value != null && int.TryParse(row.Cells["finance_year"].Value.ToString(), out year))
                                    {
                                        CompanyFinanceModel fModel = new CompanyFinanceModel();
                                        fModel.year = year;

                                        if (row.Cells["capital_amount"].Value != null && int.TryParse(row.Cells["capital_amount"].Value.ToString(), out int capital_amount))
                                            fModel.capital_amount = capital_amount;

                                        if (row.Cells["debt_amount"].Value != null && int.TryParse(row.Cells["debt_amount"].Value.ToString(), out int debt_amount))
                                            fModel.debt_amount = debt_amount;

                                        if (row.Cells["sales_amount"].Value != null && int.TryParse(row.Cells["sales_amount"].Value.ToString(), out int sales_amount))
                                            fModel.sales_amount = sales_amount;

                                        if (row.Cells["net_income_amount"].Value != null && int.TryParse(row.Cells["net_income_amount"].Value.ToString(), out int net_income_amount))
                                            fModel.net_income_amount = net_income_amount;

                                        //매출금액이 있는 내용 기준으로 등록
                                        if (fModel.sales_amount > 0)
                                        {
                                            fModel.edit_user = um.user_name;
                                            fModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            fModel.company_id = company_id;
                                            sql = companyFinanceRepository.DeleteCompanyFinanc(year.ToString(), fModel.company_id.ToString());
                                            sqlList.Add(sql);

                                            sql = companyFinanceRepository.InsertCompanyFinanc(fModel);
                                            sqlList.Add(sql);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //신규등록
                        if (cbAddNewCompany.Checked)
                        {
                            //취급X 거래처 복구 경우
                            if (NoneHandledList.Count > 0
                                && row.Cells["isRecovery"].Value != null && bool.TryParse(row.Cells["isRecovery"].Value.ToString(), out bool isRecovery) && isRecovery)
                            {
                                string[] duplicate_ids = row.Cells["duplicate_table_index"].Value.ToString().Split(' ');
                                if (duplicate_ids.Length > 0)
                                {
                                    foreach (string duplicate_id in duplicate_ids)
                                    {
                                        if (!string.IsNullOrEmpty(duplicate_id) && Int32.TryParse(duplicate_id, out Int32 table_index)
                                            && bool.TryParse(atoDt.Rows[table_index]["isNonHandled"].ToString(), out bool isNonHandled) && isNonHandled)
                                        {
                                            //데이터 통합
                                            if (string.IsNullOrEmpty(cm.ceo))
                                                cm.ceo = atoDt.Rows[table_index]["ceo"].ToString();
                                            if (string.IsNullOrEmpty(cm.registration_number))
                                                cm.registration_number = atoDt.Rows[table_index]["registration_number"].ToString();
                                            if (string.IsNullOrEmpty(cm.tel))
                                                cm.tel = atoDt.Rows[table_index]["tel"].ToString();
                                            if (string.IsNullOrEmpty(cm.fax))
                                                cm.fax = atoDt.Rows[table_index]["fax"].ToString();
                                            if (string.IsNullOrEmpty(cm.phone))
                                                cm.phone = atoDt.Rows[table_index]["phone"].ToString();
                                            if (string.IsNullOrEmpty(cm.other_phone))
                                                cm.other_phone = atoDt.Rows[table_index]["other_phone"].ToString();
                                            if (string.IsNullOrEmpty(cm.email))
                                                cm.email = atoDt.Rows[table_index]["email"].ToString();
                                            if (string.IsNullOrEmpty(cm.address))
                                                cm.address = atoDt.Rows[table_index]["address"].ToString();
                                            if (string.IsNullOrEmpty(cm.handling_item))
                                                cm.handling_item = atoDt.Rows[table_index]["handling_item"].ToString();
                                            if (string.IsNullOrEmpty(cm.distribution))
                                                cm.distribution = atoDt.Rows[table_index]["distribution"].ToString();
                                            if (string.IsNullOrEmpty(cm.payment_date))
                                                cm.payment_date = atoDt.Rows[table_index]["payment_date"].ToString();
                                            if (string.IsNullOrEmpty(cm.remark))
                                                cm.remark = atoDt.Rows[table_index]["remark"].ToString();
                                            if (string.IsNullOrEmpty(cm.industry_type))
                                                cm.remark = atoDt.Rows[table_index]["industry_type"].ToString();
                                            if (!cm.isNotSendFax)
                                                cm.isNotSendFax = Convert.ToBoolean(atoDt.Rows[table_index]["isNotSendFax"].ToString());

                                            //취급X 거래처 -> 삭제거래처
                                            CompanyModel dcm = new CompanyModel();
                                            dcm.table_index = table_index.ToString();
                                            DeleteList.Add(dcm);

                                            //실제 서버삭제
                                            sql = companyrepository.DeleteCompany(Convert.ToInt32(atoDt.Rows[table_index]["id"].ToString()), true);
                                            sqlList.Add(sql);
                                        }
                                    }
                                }
                            }
                            //2023-11-16 수정
                            //팩스X 기존 거래처는 수정하면되는데 신규 팩스x는 있을곳이 애매하다. 그래서 isHide로 팩스x 탭에서만 보이게
                            if (division == "팩스X")
                                cm.isHide = true;
                            sql = companyrepository.InsertCompany(cm);
                            sqlList.Add(sql);
                            insertList.Add(cm);

                            //재무재표 등록
                            if (cbFinance.Checked && row.Cells["finance_year"].Value != null && int.TryParse(row.Cells["finance_year"].Value.ToString(), out year))
                            {
                                CompanyFinanceModel fModel = new CompanyFinanceModel();
                                fModel.year = year;

                                if (row.Cells["capital_amount"].Value != null && double.TryParse(row.Cells["capital_amount"].Value.ToString(), out double capital_amount))
                                    fModel.capital_amount = capital_amount;

                                if (row.Cells["debt_amount"].Value != null && double.TryParse(row.Cells["debt_amount"].Value.ToString(), out double debt_amount))
                                    fModel.debt_amount = debt_amount;

                                if (row.Cells["sales_amount"].Value != null && double.TryParse(row.Cells["sales_amount"].Value.ToString(), out double sales_amount))
                                    fModel.sales_amount = sales_amount;

                                if (row.Cells["net_income_amount"].Value != null && double.TryParse(row.Cells["net_income_amount"].Value.ToString(), out double net_income_amount))
                                    fModel.net_income_amount = net_income_amount;

                                //매출금액이 있는 내용 기준으로 등록
                                if (fModel.sales_amount > 0)
                                {
                                    fModel.edit_user = um.user_name;
                                    fModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    fModel.company_id = cm.id;
                                    sql = companyFinanceRepository.DeleteCompanyFinanc(year.ToString(), cm.id.ToString());
                                    sqlList.Add(sql);

                                    sql = companyFinanceRepository.InsertCompanyFinanc(fModel);
                                    sqlList.Add(sql);
                                }
                            }
                        }
                        //기존데이터 업데이트
                        if (cbOldCompanyUpdate.Checked)
                        {
                            string[] table_indexs = row.Cells["duplicate_table_index"].Value.ToString().Trim().Split(' ');
                            foreach (string table_index in table_indexs)
                            {
                                if (int.TryParse(table_index, out int idx) && int.TryParse(atoDt.Rows[idx]["id"].ToString(), out int company_id) && company_id > 0)
                                {
                                    DataRow atoDr = atoDt.Rows[idx];

                                    if (string.IsNullOrEmpty(atoDr["group_name"].ToString()))
                                        atoDr["group_name"] = cm.group_name;

                                    if (string.IsNullOrEmpty(atoDr["ceo"].ToString()))
                                        atoDr["ceo"] = cm.ceo;

                                    if (string.IsNullOrEmpty(atoDr["registration_number"].ToString()))
                                        atoDr["registration_number"] = cm.registration_number;

                                    if (string.IsNullOrEmpty(atoDr["tel"].ToString()))
                                        atoDr["tel"] = cm.tel;

                                    if (string.IsNullOrEmpty(atoDr["fax"].ToString()))
                                        atoDr["fax"] = cm.fax;

                                    if (string.IsNullOrEmpty(atoDr["phone"].ToString()))
                                        atoDr["phone"] = cm.phone;

                                    if (string.IsNullOrEmpty(atoDr["other_phone"].ToString()))
                                        atoDr["other_phone"] = cm.other_phone;

                                    if (string.IsNullOrEmpty(atoDr["email"].ToString()))
                                        atoDr["email"] = cm.email;

                                    if (string.IsNullOrEmpty(atoDr["address"].ToString()))
                                        atoDr["address"] = cm.address;

                                    if (string.IsNullOrEmpty(atoDr["handling_item"].ToString()))
                                        atoDr["handling_item"] = cm.handling_item;

                                    if (string.IsNullOrEmpty(atoDr["distribution"].ToString()))
                                        atoDr["distribution"] = cm.distribution;

                                    if (string.IsNullOrEmpty(atoDr["remark"].ToString()))
                                        atoDr["remark"] = cm.remark;

                                    if (string.IsNullOrEmpty(atoDr["industry_type"].ToString()))
                                        atoDr["industry_type"] = cm.industry_type;

                                    //수정컬럼
                                    string[] col = new string[15];
                                    col[0] = "group_name";
                                    col[1] = "ceo";
                                    col[2] = "registration_number";
                                    col[3] = "tel";
                                    col[4] = "fax";
                                    col[5] = "phone";
                                    col[6] = "other_phone";
                                    col[7] = "email";
                                    col[8] = "address";
                                    col[9] = "handling_item";
                                    col[10] = "distribution";
                                    col[11] = "remark";
                                    col[12] = "industry_type";
                                    col[13] = "isNotSendFax";
                                    col[14] = "edit_user";
                                    //수정값
                                    string[] val = new string[15];
                                    val[0] = "'" + common.AddSlashes(atoDr["group_name"].ToString()) + "'";
                                    val[1] = "'" + common.AddSlashes(atoDr["ceo"].ToString()) + "'";
                                    val[2] = "'" + common.AddSlashes(atoDr["registration_number"].ToString()) + "'";
                                    val[3] = "'" + common.AddSlashes(atoDr["tel"].ToString()) + "'";
                                    val[4] = "'" + common.AddSlashes(atoDr["fax"].ToString()) + "'";
                                    val[5] = "'" + common.AddSlashes(atoDr["phone"].ToString()) + "'";
                                    val[6] = "'" + common.AddSlashes(atoDr["other_phone"].ToString()) + "'";
                                    val[7] = "'" + common.AddSlashes(atoDr["email"].ToString()) + "'";
                                    val[8] = "'" + common.AddSlashes(atoDr["address"].ToString()) + "'";
                                    val[9] = "'" + common.AddSlashes(atoDr["handling_item"].ToString()) + "'";
                                    val[10] = "'" + common.AddSlashes(atoDr["distribution"].ToString()) + "'";
                                    val[11] = "'" + common.AddSlashes(atoDr["remark"].ToString()) + "'";
                                    val[12] = "'" + common.AddSlashes(atoDr["industry_type"].ToString()) + "'";
                                    val[13] = cm.isNotSendFax.ToString();
                                    val[14] = "'" + um.user_name + "'";

                                    //조건
                                    string whr = $"id = {company_id}";
                                    //Update data
                                    sql = companyrepository.UpdateCompanyColumns(col, val, whr);
                                    sqlList.Add(sql);

                                    //재무재표 등록
                                    if (cbFinance.Checked && row.Cells["finance_year"].Value != null && int.TryParse(row.Cells["finance_year"].Value.ToString(), out year))
                                    {
                                        CompanyFinanceModel fModel = new CompanyFinanceModel();
                                        fModel.year = year;

                                        if (row.Cells["capital_amount"].Value != null && double.TryParse(row.Cells["capital_amount"].Value.ToString(), out double capital_amount))
                                            fModel.capital_amount = capital_amount;

                                        if (row.Cells["debt_amount"].Value != null && double.TryParse(row.Cells["debt_amount"].Value.ToString(), out double debt_amount))
                                            fModel.debt_amount = debt_amount;

                                        if (row.Cells["sales_amount"].Value != null && double.TryParse(row.Cells["sales_amount"].Value.ToString(), out double sales_amount))
                                            fModel.sales_amount = sales_amount;

                                        if (row.Cells["net_income_amount"].Value != null && double.TryParse(row.Cells["net_income_amount"].Value.ToString(), out double net_income_amount))
                                            fModel.net_income_amount = net_income_amount;

                                        //매출금액이 있는 내용 기준으로 등록
                                        if (fModel.sales_amount > 0)
                                        {
                                            fModel.edit_user = um.user_name;
                                            fModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            fModel.company_id = company_id;
                                            sql = companyFinanceRepository.DeleteCompanyFinanc(year.ToString(), fModel.company_id.ToString());
                                            sqlList.Add(sql);

                                            sql = companyFinanceRepository.InsertCompanyFinanc(fModel);
                                            sqlList.Add(sql);
                                        }
                                    }
                                }
                            }
                        }
                    }


                    //영업내역
                    CompanySalesModel sModel = new CompanySalesModel();
                    sModel.company_id = id;
                    sModel.sub_id = 1;
                    sModel.is_sales = false;
                    sModel.contents = division;
                    sModel.log = "|" + txtDataSource.Text;
                    sModel.remark = "";
                    sModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sModel.edit_user = um.user_name;

                    sModel.from_ato_manager = "";
                    sModel.to_ato_manager = cm.ato_manager;
                    sModel.from_category = "";
                    sModel.to_category= division;

                    sql = salesPartnerRepository.InsertPartnerSales(sModel);
                    sqlList.Add(sql);

                    id++;
                }
            }

            //스톱워치1 종료
            stopwatch1.Stop();
            //스톱워치2 시작
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();

            //Execute
            int results = commonRepository.UpdateTran(sqlList);
            if (results == -1)
            {
                stopwatch2.Stop();
                pb.Close();
                messageBox.Show(this,"등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                stopwatch2.Stop();
                pb.Close();
                messageBox.Show(this,"등록완료" + $"\n 실행1 : {(stopwatch1.ElapsedMilliseconds / 1000).ToString()} \n 실행2 : {(stopwatch2.ElapsedMilliseconds / 1000).ToString("#,##0")} \n 총 실행시간 : {((stopwatch1.ElapsedMilliseconds + stopwatch2.ElapsedMilliseconds) / 1000).ToString("#,##0")}");
                this.Activate();
                if (sm != null)
                {
                    sm.InsertAtoDt(insertList);
                    sm.UpdateAtoDt(updateList);
                    sm.DeleteAtoDt(DeleteList);
                }
            }
            pb.Close();
        }


        private void btnDeleteDuplicate_Click(object sender, EventArgs e)
        {
            RemoveDuplicateCompanyManager rdc = new RemoveDuplicateCompanyManager(um, this);
            rdc.Show();
            /*if (dgvCompany.Rows.Count > 0)
            {
                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvCompany.Rows[i].Cells["is_duplicate"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["is_duplicate"].Value.ToString()))
                    {
                        bool isRemove = false;
                        string is_duplicate = dgvCompany.Rows[i].Cells["is_duplicate"].Value.ToString();

                        if (cbNonHandled.Checked && is_duplicate.Contains(cbNonHandled.Text))
                            isRemove = true;
                        if (cbNotSendFax.Checked && is_duplicate.Contains(cbNotSendFax.Text))
                            isRemove = true;
                        if (cbRetire.Checked && is_duplicate.Contains(cbRetire.Text))
                            isRemove = true;
                        if (cbCommonData.Checked && is_duplicate.Contains(cbCommonData.Text))
                            isRemove = true;
                        if (cbMyData.Checked && is_duplicate.Contains(cbMyData.Text))
                            isRemove = true;
                        if (cbPotential1.Checked && is_duplicate.Contains(cbPotential1.Text))
                            isRemove = true;
                        if (cbPotential2.Checked && is_duplicate.Contains(cbPotential2.Text))
                            isRemove = true;
                        if (cbTrading.Checked && is_duplicate.Contains(cbTrading.Text))
                            isRemove = true;

                        if(isRemove)
                            dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                    }
                }
            }*/
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            RemoveDuplicateCompanyMessageBox rdcm = new RemoveDuplicateCompanyMessageBox();
            DataDuplicate();
            if (rdcm.RemoveDuplicateCompanyMessageBoxResult())
            {
                RemoveDuplicateCompany2();
                messageBox.Show(this, "완료");
                this.Activate();
            }
            else
            {
                foreach (DataGridViewRow row in dgvCompany.Rows)
                    row.Cells["complete_duplicate"].Value = false;

            }


            /*DataDuplicate();
            //CheckDuplicate();
            try
            {
                RemoveDuplicateCompanyManager rdc = new RemoveDuplicateCompanyManager(um, this);
                rdc.TopMost = true;
                DataTable limitSettingDt = rdc.GetLimitSetting();
                RemoveDuplicateCompany(limitSettingDt);
                MessageBox.Show(this,"완료");
                this.Activate();
            }
            catch
            { }*/
        }

        #region 데이터 중복검사
        private void DataDuplicate()
        {
            if (dgvCompany.Rows.Count > 0)
            {
                //유효성검사, 초기화
                dgvCompany.EndEdit();
                duplicateDic = new Dictionary<int, List<DataRow>>();

                int errCnt = 0;
                int emptyCnt = 0;
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    //초기화
                    dgvCompany.Rows[i].Cells["is_duplicate"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["common_duplicate_count"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["mydata_duplicate_count"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["potential_duplicate_count"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["potential2_duplicate_count"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["isTrading_duplicate_count"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["nonehandled_duplicate_count"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["notSendFax_duplicate_count"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["outOfBusiness_duplicate_count"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["duplicate_log"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["duplicate_company_id"].Value = string.Empty;
                    dgvCompany.Rows[i].Cells["complete_duplicate"].Value = false;

                    //데이터 없는 경우
                    if (dgvCompany.Rows[i].Cells["company"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["company"].Value.ToString())
                        && (dgvCompany.Rows[i].Cells["registration_number"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["registration_number"].Value.ToString()))
                        && (dgvCompany.Rows[i].Cells["tel"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["tel"].Value.ToString()))
                        && (dgvCompany.Rows[i].Cells["fax"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["fax"].Value.ToString()))
                        && (dgvCompany.Rows[i].Cells["phone"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["phone"].Value.ToString()))
                        && (dgvCompany.Rows[i].Cells["other_phone"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["other_phone"].Value.ToString()))
                        && (dgvCompany.Rows[i].Cells["ceo"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["ceo"].Value.ToString()))
                        && (dgvCompany.Rows[i].Cells["address"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["address"].Value.ToString()))
                        && (dgvCompany.Rows[i].Cells["email"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["email"].Value.ToString())))
                    {
                        emptyCnt++;
                        dgvCompany.Rows[i].HeaderCell.Style.BackColor = Color.DarkRed;
                    }
                    //데이터 유효성검사
                    else if (dgvCompany.Rows[i].Cells["company"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["company"].Value.ToString())
                        && ((dgvCompany.Rows[i].Cells["tel"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["tel"].Value.ToString()))
                        || (dgvCompany.Rows[i].Cells["fax"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["fax"].Value.ToString()))
                        || (dgvCompany.Rows[i].Cells["phone"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["phone"].Value.ToString()))
                        || (dgvCompany.Rows[i].Cells["other_phone"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["other_phone"].Value.ToString()))
                        || (dgvCompany.Rows[i].Cells["registration_number"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["registration_number"].Value.ToString()))))
                    {
                        bool isFlag = isValidation(dgvCompany.Rows[i]);
                        if (!isFlag)
                        {
                            errCnt++;
                            dgvCompany.Rows[i].HeaderCell.Style.BackColor = Color.Red;
                        }
                        else
                            dgvCompany.Rows[i].HeaderCell.Style.BackColor = Color.LightGray;
                    }
                }

                //거래처명 없으면 삭제
                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvCompany.Rows[i].Cells["company"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["company"].Value.ToString()))
                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                }

                //Err Msg
                if (errCnt > 0)
                {
                    if (messageBox.Show(this, "전화번호 형식에 맞지않는 값이 있습니다. 무시하고 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                }
                if (emptyCnt > 0)
                {
                    if (messageBox.Show(this, "거래처명 외 데이터가 입력되지 않은 내역이 있습니다. 삭제후 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dgvCompany.Rows[i].HeaderCell.Style.BackColor == Color.DarkRed)
                                dgvCompany.Rows.RemoveAt(i);
                        }
                    }
                }

                //입력데이터 -> Datatable
                DataTable inputDt = common.ConvertDgvToDataTable(dgvCompany);
                inputDt.Columns.Add("rowIndex", typeof(Int32));
                for (int i = 0; i < inputDt.Rows.Count; i++)
                    inputDt.Rows[i]["rowindex"] = i;
                inputDt.AcceptChanges();

                //입력한 정보
                inputDt = SetAtoDatatable(inputDt);
                //매출처 정보
                DataTable tempDt = SetSeaoverDatatable(atoDt);           //Ato전산에만 있는 거래처

                DataTable duplicateDt = null;
                // 데이터 중복추출, PLINQ 사용
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
                                            rowindex = p.Field<Int32>("rowindex"),
                                            division = (t == null) ? "" : t.Field<string>("division"),
                                            idx = (t == null) ? "" : t.Field<string>("idx"),
                                            tel = (t == null) ? "" : t.Field<string>("tel"),
                                            ato_manager = (t == null) ? "" : t.Field<string>("ato_manager"),
                                            table_index = (t == null) ? "" : t.Field<string>("table_index"),
                                            company_id = (t == null) ? "" : t.Field<string>("id"),
                                            isNotSendFax = (t == null) ? "FALSE" : t.Field<string>("isNotSendFax"),
                                            current_sale_date = (t == null) ? "" : t.Field<string>("current_sale_date"),
                                        };
                    DataTable duplicateDt1 = ConvertListToDatatable(duplicateVar1);
                    if (duplicateDt1 != null)
                    {
                        duplicateDt1.AcceptChanges();
                        duplicateDt = duplicateDt1;
                    }
                }
                //사업자번호 확인
                tempDr = inputDt.Select("registration_number <> ''");
                tempDr2 = tempDt.Select("registration_number <> ''");
                if (tempDr.Length > 0 && tempDr2.Length > 0)
                {
                    DataTable rnDt = tempDr.CopyToDataTable();
                    DataTable rnDt2 = tempDr2.CopyToDataTable();
                    var duplicateVar4 = from p in rnDt.AsEnumerable()
                                        join t in rnDt2.AsEnumerable()
                                        on p.Field<string>("registration_number") equals t.Field<string>("registration_number")
                                        into outer
                                        from t in outer.DefaultIfEmpty()
                                        select new
                                        {
                                            rowindex = p.Field<Int32>("rowindex"),
                                            division = (t == null) ? "" : t.Field<string>("division"),
                                            idx = (t == null) ? "" : t.Field<string>("idx"),
                                            tel = (t == null) ? "" : t.Field<string>("registration_number"),
                                            ato_manager = (t == null) ? "" : t.Field<string>("ato_manager"),
                                            table_index = (t == null) ? "" : t.Field<string>("table_index"),
                                            company_id = (t == null) ? "" : t.Field<string>("id"),
                                            isNotSendFax = (t == null) ? "FALSE" : t.Field<string>("isNotSendFax"),
                                            current_sale_date = (t == null) ? "" : t.Field<string>("current_sale_date")
                                        };
                    DataTable duplicateDt4 = ConvertListToDatatable(duplicateVar4);
                    if (duplicateDt4 != null)
                    {
                        duplicateDt4.AcceptChanges();
                        if (duplicateDt != null)
                        {
                            duplicateDt.Merge(duplicateDt4);
                            duplicateDt.AcceptChanges();
                        }
                    }
                }
                //중복삭제
                if (duplicateDt != null)
                {
                    DataRow[] resultDr = duplicateDt.Select("division <> '' AND company_id <> ''  AND tel <> ''");
                    if (resultDr.Length > 0)
                    {
                        DataTable resultDt = resultDr.CopyToDataTable();
                        // 추출된 중복 데이터를 순회하며 데이터가공
                        foreach (DataRow duplicate in resultDt.Rows)
                        {
                            int rowindex;
                            if (int.TryParse(duplicate["rowindex"].ToString(), out rowindex))
                            {
                                bool isExist = false;
                                if (duplicateDic.ContainsKey(rowindex))
                                {
                                    List<DataRow> duplicateDr = duplicateDic[rowindex];

                                    if (duplicate["division"].ToString() == "공용DATA" || duplicate["division"].ToString() == "취급X")
                                    {
                                        foreach (DataRow dr in duplicateDr)
                                        {
                                            if (dr["idx"].ToString() == duplicate["idx"].ToString())
                                                isExist = true;
                                        }
                                        if (!isExist)
                                            duplicateDr.Add(duplicate);
                                    }
                                    else
                                    {
                                        foreach (DataRow dr in duplicateDr)
                                        {
                                            if (dr["idx"].ToString() == duplicate["idx"].ToString() || dr["ato_manager"].ToString() == duplicate["ato_manager"].ToString())
                                                isExist = true;
                                        }
                                        if (!isExist)
                                            duplicateDr.Add(duplicate);
                                    }

                                    duplicateDic[rowindex] = duplicateDr;
                                }
                                else
                                {
                                    List<DataRow> dr = new List<DataRow>();
                                    dr.Add(duplicate);
                                    duplicateDic[rowindex] = dr;
                                }

                                //중복데이터 갱신===============================================================================================
                                if (!isExist)
                                {
                                    //팩스X
                                    bool isNotSendFax;
                                    if (dgvCompany.Rows[rowindex].Cells["isNotSendFax"].Value == null || !bool.TryParse(dgvCompany.Rows[rowindex].Cells["isNotSendFax"].Value.ToString(), out isNotSendFax))
                                        isNotSendFax = false;

                                    if (!isNotSendFax && bool.TryParse(duplicate["isNotSendFax"].ToString(), out bool temp_isNotSendFax) && temp_isNotSendFax)
                                        dgvCompany.Rows[rowindex].Cells["isNotSendFax"].Value = temp_isNotSendFax;

                                    //중복 거래처ID, Table_index
                                    if (duplicate["company_id"].ToString() != "0")
                                    {
                                        dgvCompany.Rows[rowindex].Cells["duplicate_company_id"].Value += " " + duplicate["company_id"].ToString();
                                        dgvCompany.Rows[rowindex].Cells["duplicate_table_index"].Value += " " + duplicate["table_index"].ToString();
                                    }
                                    //중복 카테고리
                                    switch (duplicate["division"].ToString())
                                    {
                                        case "공용DATA":
                                            {
                                                int count;
                                                if (dgvCompany.Rows[rowindex].Cells["common_duplicate_count"].Value == null ||
                                                    !int.TryParse(dgvCompany.Rows[rowindex].Cells["common_duplicate_count"].Value.ToString(), out count))
                                                    count = 0;
                                                count++;
                                                dgvCompany.Rows[rowindex].Cells["common_duplicate_count"].Value = count;
                                            }
                                            break;
                                        case "내DATA":
                                            {
                                                int count;
                                                if (dgvCompany.Rows[rowindex].Cells["mydata_duplicate_count"].Value == null ||
                                                    !int.TryParse(dgvCompany.Rows[rowindex].Cells["mydata_duplicate_count"].Value.ToString(), out count))
                                                    count = 0;
                                                count++;
                                                dgvCompany.Rows[rowindex].Cells["mydata_duplicate_count"].Value = count;
                                            }
                                            break;
                                        case "잠재1":
                                            {
                                                int count;
                                                if (dgvCompany.Rows[rowindex].Cells["potential_duplicate_count"].Value == null ||
                                                    !int.TryParse(dgvCompany.Rows[rowindex].Cells["potential_duplicate_count"].Value.ToString(), out count))
                                                    count = 0;
                                                count++;
                                                dgvCompany.Rows[rowindex].Cells["potential_duplicate_count"].Value = count;
                                            }
                                            break;
                                        case "잠재2":
                                            {
                                                int count;
                                                if (dgvCompany.Rows[rowindex].Cells["potential2_duplicate_count"].Value == null ||
                                                    !int.TryParse(dgvCompany.Rows[rowindex].Cells["potential2_duplicate_count"].Value.ToString(), out count))
                                                    count = 0;
                                                count++;
                                                dgvCompany.Rows[rowindex].Cells["potential2_duplicate_count"].Value = count;
                                            }
                                            break;
                                        case "거래중":
                                            {
                                                int count;
                                                if (dgvCompany.Rows[rowindex].Cells["isTrading_duplicate_count"].Value == null ||
                                                    !int.TryParse(dgvCompany.Rows[rowindex].Cells["isTrading_duplicate_count"].Value.ToString(), out count))
                                                    count = 0;
                                                count++;
                                                dgvCompany.Rows[rowindex].Cells["isTrading_duplicate_count"].Value = count;
                                            }
                                            break;
                                        case "취급X":
                                            {
                                                int count;
                                                if (dgvCompany.Rows[rowindex].Cells["nonehandled_duplicate_count"].Value == null ||
                                                    !int.TryParse(dgvCompany.Rows[rowindex].Cells["nonehandled_duplicate_count"].Value.ToString(), out count))
                                                    count = 0;
                                                count++;
                                                dgvCompany.Rows[rowindex].Cells["nonehandled_duplicate_count"].Value = count;
                                            }
                                            break;
                                        case "팩스X":
                                            {
                                                int count;
                                                if (dgvCompany.Rows[rowindex].Cells["notSendFax_duplicate_count"].Value == null ||
                                                    !int.TryParse(dgvCompany.Rows[rowindex].Cells["notSendFax_duplicate_count"].Value.ToString(), out count))
                                                    count = 0;
                                                count++;
                                                dgvCompany.Rows[rowindex].Cells["notSendFax_duplicate_count"].Value = count;
                                            }
                                            break;
                                        case "폐업":
                                            {
                                                int count;
                                                if (dgvCompany.Rows[rowindex].Cells["outOfBusiness_duplicate_count"].Value == null ||
                                                    !int.TryParse(dgvCompany.Rows[rowindex].Cells["outOfBusiness_duplicate_count"].Value.ToString(), out count))
                                                    count = 0;
                                                count++;
                                                dgvCompany.Rows[rowindex].Cells["outOfBusiness_duplicate_count"].Value = count;
                                            }
                                            break;

                                    }

                                    //데이터 최신화
                                    DataRow duplicateDr = atoDt.Rows[Convert.ToInt32(duplicate["table_index"].ToString())];
                                    string registration_number = duplicateDr["registration_number"].ToString();
                                    string tel = duplicateDr["tel"].ToString();
                                    string phone = duplicateDr["phone"].ToString();
                                    string fax = duplicateDr["fax"].ToString();
                                    string address = duplicateDr["address"].ToString();
                                    string other_phone = duplicateDr["other_phone"].ToString();
                                    string email = duplicateDr["email"].ToString();
                                    string ceo = duplicateDr["ceo"].ToString();

                                    if (dgvCompany.Rows[rowindex].Cells["registration_number"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[rowindex].Cells["registration_number"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["registration_number"].Value = registration_number;

                                    if (dgvCompany.Rows[rowindex].Cells["email"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[rowindex].Cells["email"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["email"].Value = email;

                                    if (dgvCompany.Rows[rowindex].Cells["ceo"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[rowindex].Cells["ceo"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["ceo"].Value = ceo;

                                    if (dgvCompany.Rows[rowindex].Cells["tel"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[rowindex].Cells["tel"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["tel"].Value = tel;
                                    else if (dgvCompany.Rows[rowindex].Cells["tel"].Value.ToString().Contains(tel) || tel.Contains(dgvCompany.Rows[rowindex].Cells["tel"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["tel"].Value = dgvCompany.Rows[rowindex].Cells["tel"].Value.ToString() + "/" + tel;

                                    if (dgvCompany.Rows[rowindex].Cells["phone"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[rowindex].Cells["phone"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["phone"].Value = phone;
                                    else if (dgvCompany.Rows[rowindex].Cells["phone"].Value.ToString().Contains(phone) || phone.Contains(dgvCompany.Rows[rowindex].Cells["phone"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["phone"].Value = dgvCompany.Rows[rowindex].Cells["phone"].Value.ToString() + "/" + phone;

                                    if (dgvCompany.Rows[rowindex].Cells["fax"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[rowindex].Cells["fax"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["fax"].Value = fax;
                                    else if (dgvCompany.Rows[rowindex].Cells["fax"].Value.ToString().Contains(fax) || fax.Contains(dgvCompany.Rows[rowindex].Cells["fax"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["fax"].Value = dgvCompany.Rows[rowindex].Cells["fax"].Value.ToString() + "/" + fax;

                                    if (dgvCompany.Rows[rowindex].Cells["address"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[rowindex].Cells["address"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["address"].Value = address;
                                    else if (dgvCompany.Rows[rowindex].Cells["address"].Value.ToString().Contains(address) || address.Contains(dgvCompany.Rows[rowindex].Cells["address"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["address"].Value = dgvCompany.Rows[rowindex].Cells["address"].Value.ToString() + "/" + address;

                                    if (dgvCompany.Rows[rowindex].Cells["other_phone"].Value == null || string.IsNullOrEmpty(dgvCompany.Rows[rowindex].Cells["other_phone"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["other_phone"].Value = other_phone;
                                    else if (dgvCompany.Rows[rowindex].Cells["other_phone"].Value.ToString().Contains(other_phone) || other_phone.Contains(dgvCompany.Rows[rowindex].Cells["other_phone"].Value.ToString()))
                                        dgvCompany.Rows[rowindex].Cells["other_phone"].Value = dgvCompany.Rows[rowindex].Cells["other_phone"].Value.ToString() + "/" + other_phone;
                                }
                            }
                        }
                        //중복결과
                        foreach (DataGridViewRow row in dgvCompany.Rows)
                        {
                            int common_duplicate_count;
                            if (row.Cells["common_duplicate_count"].Value == null || !int.TryParse(row.Cells["common_duplicate_count"].Value.ToString(), out common_duplicate_count))
                                common_duplicate_count = 0;

                            int mydata_duplicate_count;
                            if (row.Cells["mydata_duplicate_count"].Value == null || !int.TryParse(row.Cells["mydata_duplicate_count"].Value.ToString(), out mydata_duplicate_count))
                                mydata_duplicate_count = 0;

                            int potential_duplicate_count;
                            if (row.Cells["potential_duplicate_count"].Value == null || !int.TryParse(row.Cells["potential_duplicate_count"].Value.ToString(), out potential_duplicate_count))
                                potential_duplicate_count = 0;

                            int potential2_duplicate_count;
                            if (row.Cells["potential2_duplicate_count"].Value == null || !int.TryParse(row.Cells["potential2_duplicate_count"].Value.ToString(), out potential2_duplicate_count))
                                potential2_duplicate_count = 0;

                            int isTrading_duplicate_count;
                            if (row.Cells["isTrading_duplicate_count"].Value == null || !int.TryParse(row.Cells["isTrading_duplicate_count"].Value.ToString(), out isTrading_duplicate_count))
                                isTrading_duplicate_count = 0;

                            int notSendFax_duplicate_count;
                            if (row.Cells["notSendFax_duplicate_count"].Value == null || !int.TryParse(row.Cells["notSendFax_duplicate_count"].Value.ToString(), out notSendFax_duplicate_count))
                                notSendFax_duplicate_count = 0;

                            int nonehandled_duplicate_count;
                            if (row.Cells["nonehandled_duplicate_count"].Value == null || !int.TryParse(row.Cells["nonehandled_duplicate_count"].Value.ToString(), out nonehandled_duplicate_count))
                                nonehandled_duplicate_count = 0;

                            int outOfBusiness_duplicate_count;
                            if (row.Cells["outOfBusiness_duplicate_count"].Value == null || !int.TryParse(row.Cells["outOfBusiness_duplicate_count"].Value.ToString(), out outOfBusiness_duplicate_count))
                                outOfBusiness_duplicate_count = 0;


                            string division = "";
                            if (common_duplicate_count > 0)
                                division += " 공용DATA:" + common_duplicate_count;
                            if (mydata_duplicate_count > 0)
                                division += " 내DATA:" + mydata_duplicate_count;
                            if (potential_duplicate_count > 0)
                                division += " 잠재1:" + potential_duplicate_count;
                            if (potential2_duplicate_count > 0)
                                division += " 잠재2:" + potential2_duplicate_count;
                            if (isTrading_duplicate_count > 0)
                                division += " 거래중:" + isTrading_duplicate_count;
                            if (nonehandled_duplicate_count > 0)
                                division += " 취급X:" + nonehandled_duplicate_count;
                            if (notSendFax_duplicate_count > 0)
                                division += " 팩스X:" + notSendFax_duplicate_count;
                            if (outOfBusiness_duplicate_count > 0)
                                division += " 폐업:" + outOfBusiness_duplicate_count;

                            row.Cells["is_duplicate"].Value = division.Trim().Replace(" ", ", ");
                        }
                    }

                    //검사완료
                    foreach (DataGridViewRow row in dgvCompany.Rows)
                        row.Cells["complete_duplicate"].Value = true;
                }
            }
        }
        #endregion





        Dictionary<int, List<DataRow>> duplicateDic = new Dictionary<int, List<DataRow>>();
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

                if (tel.Length == 8 && tel.Substring(0, 3) != "010")
                    tel = "010" + tel;
                else if (tel.Length > 2 && tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel = "0" + tel;

            }
            return tel;
        }
        private DataTable SetSeaoverDatatable1(DataTable seaoverDt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("company", typeof(string));
            dt.Columns.Add("current_sale_date", typeof(string));
            dt.Columns.Add("tel", typeof(string));
            dt.Columns.Add("registration_number", typeof(string));
            dt.Columns.Add("ato_manager", typeof(string));
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                foreach (DataRow row in seaoverDt.Rows)
                {
                    if (!string.IsNullOrEmpty(row["전화번호1"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["전화번호1"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dr["ato_manager"] = row["매출자"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["전화번호2"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["전화번호2"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dr["ato_manager"] = row["매출자"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["팩스번호1"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["팩스번호1"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dr["ato_manager"] = row["매출자"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["팩스번호2"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["팩스번호2"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dr["ato_manager"] = row["매출자"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["휴대폰1"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["휴대폰1"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dr["ato_manager"] = row["매출자"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["휴대폰2"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["휴대폰2"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dr["ato_manager"] = row["매출자"].ToString();
                        dt.Rows.Add(dr);
                    }
                }   
            }
            return dt;
        }
        private DataTable SetAtoDatatable(DataTable seaoverDt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("rowindex", typeof(Int32));
            dt.Columns.Add("tel", typeof(string));
            dt.Columns.Add("registration_number", typeof(string));
            if (seaoverDt != null && seaoverDt.Rows.Count > 0)
            {
                int idx = 0;
                foreach (DataRow row in seaoverDt.Rows)
                {
                    idx++;
                    string tel = ValidationTelString(row["tel"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
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
                            for (int j = 1; j < tels.Length; j++)
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
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
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

                    }

                    tel = ValidationTelString(row["fax"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
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
                            for (int j = 1; j < tels.Length; j++)
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
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
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
                    }

                    tel = ValidationTelString(row["phone"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
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
                            for (int j = 1; j < tels.Length; j++)
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
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
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
                    }

                    tel = ValidationTelString(row["other_phone"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
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
                            for (int j = 1; j < tels.Length; j++)
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
                            if (tel1.Length > 0 && tel2.Length > 0 && tel1.Length > tel2.Length)
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
                    }

                    tel = ValidationTelString(row["registration_number"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        DataRow dr = GetDatarow(dt, row, tel, idx);
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
                    string tel = ValidationTelString(row["tel"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

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
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

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
                            if (tel1.Length > 0 && tel2.Length > 0 && tel2.Length <= 3 && tel1.Length > tel2.Length)
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

                    tel = ValidationTelString(row["fax"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

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
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

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
                            if (tel1.Length > 0 && tel2.Length > 0 && tel2.Length <= 3 && tel1.Length > tel2.Length)
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

                    tel = ValidationTelString(row["phone"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

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
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

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
                            if (tel1.Length > 0 && tel2.Length > 0 && tel2.Length <= 3 && tel1.Length > tel2.Length)
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

                    tel = ValidationTelString(row["other_phone"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
                    if (!string.IsNullOrEmpty(tel))
                    {
                        if (tel.Contains(','))
                        {
                            if (tel.Substring(0, 1) == ",")
                                tel = tel.Substring(1, tel.Length - 1);

                            string[] tels = tel.Split(',');
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

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
                            if (tels.Length == 1)
                            {
                                DataRow dr = GetDatarow2(dt, row, tel, idx);
                                dt.Rows.Add(dr);
                            }
                            else
                            {
                                for (int j = 0; j < tels.Length; j++)
                                {
                                    if (!string.IsNullOrEmpty(tels[j].Trim()))
                                    {
                                        string rsTel;
                                        if (tels[0].Length > tels[j].Length)
                                            rsTel = tels[0].Substring(0, (tels[0].Length - tels[j].Length)) + tels[j];
                                        else
                                            rsTel = tels[j];

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
                            if (tel1.Length > 0 && tel2.Length > 0 && tel2.Length <= 3 && tel1.Length > tel2.Length)
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
            bool isHide;
            if (!bool.TryParse(row["isHide"].ToString(), out isHide))
                isHide = false;
            bool isPotential1;
            if (!bool.TryParse(row["isPotential1"].ToString(), out isPotential1))
                isPotential1 = false;
            bool isPotential2;
            if (!bool.TryParse(row["isPotential2"].ToString(), out isPotential2))
                isPotential2 = false;

            string division;
            if (isHide && isNotSendFax)
                division = "팩스X";
            else if(isDelete)
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
        private DataRow GetDatarow(DataTable dt, DataRow row, string tel, int idx)
        {
            DataRow dr = dt.NewRow();
            dr["rowindex"] = Convert.ToInt32(row["rowindex"].ToString());
            dr["registration_number"] = row["registration_number"].ToString();
            dr["tel"] = tel;
            return dr;
        }

        #endregion

        #region Key event
        private void txtGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (messageBox.Show(this, "변경내용을 일괄적용하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    dgvCompany.Rows[i].Cells["group_name"].Value = txtGroupName.Text;
            }
        }

        
        private void AddBusinessCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnAddCompany.PerformClick();
                        break;
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.D:
                        string tempData = "";
                        int pre_col_idx = -1;
                        for (int i = 0; i < dgvCompany.ColumnCount; i++)
                        {
                            for (int j = 0; j < dgvCompany.Rows.Count; j++)
                            {
                                if (dgvCompany.Rows[j].Cells[i].Selected)
                                {
                                    if (dgvCompany.Rows[j].Cells[i].Value == null)
                                        dgvCompany.Rows[j].Cells[i].Value = string.Empty;

                                    if (pre_col_idx != i)
                                    {
                                        tempData = dgvCompany.Rows[j].Cells[i].Value.ToString();
                                        pre_col_idx = i;
                                    }
                                    dgvCompany.Rows[j].Cells[i].Value = tempData;
                                }
                            }
                        }
                        break;
                    case Keys.V:
                        string clipText = Clipboard.GetText();
                        if (string.IsNullOrEmpty(clipText) == false)
                        {
                            string[] lines = clipText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                            int idx = 0;
                            if(dgvCompany.SelectedCells.Count > 0)
                                idx = dgvCompany.SelectedCells[0].RowIndex;

                            if (lines.Length > dgvCompany.Rows.Count - idx)
                                dgvCompany.Rows.Insert(dgvCompany.Rows.Count, lines.Length - (dgvCompany.Rows.Count - idx));
                        }
                        dgvCompany.EndEdit();
                        dgvCompany.Paste();
                    break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control tb = common.FindFocusedControl(this);

            //위로 이동
            if (dgvCompany.Focused && keyData == (Keys.Control | Keys.Up))
            {
                if (dgvCompany.Rows.Count > 0 && dgvCompany.CurrentCell != null)
                {
                    int row = dgvCompany.CurrentCell.RowIndex;
                    int col = dgvCompany.CurrentCell.ColumnIndex;

                    if (row - 1 >= 0)
                    {
                        string currentValue = string.Empty;
                        if (dgvCompany.Rows[row].Cells[col].Value != null)
                            currentValue = dgvCompany.Rows[row].Cells[col].Value.ToString();

                        for (int i = row - 1; i >= 0; i--)
                        {
                            string tempValue = string.Empty;
                            if (dgvCompany.Rows[i].Cells[col].Value != null)
                                tempValue = dgvCompany.Rows[i].Cells[col].Value.ToString();

                            if (currentValue != tempValue || i == 0)
                            {
                                if (i - 17 >= 0)
                                    dgvCompany.FirstDisplayedScrollingRowIndex = i - 17;
                                else
                                    dgvCompany.FirstDisplayedScrollingRowIndex = 0;
                                dgvCompany.CurrentCell = dgvCompany.Rows[i].Cells[col];
                                break;
                            }
                        }
                    }
                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            //아래로 이동
            else if (dgvCompany.Focused && keyData == (Keys.Control | Keys.Down))
            {
                if (dgvCompany.Rows.Count > 0 && dgvCompany.CurrentCell != null)
                {
                    int row = dgvCompany.CurrentCell.RowIndex;
                    int col = dgvCompany.CurrentCell.ColumnIndex;

                    if (row + 1 < dgvCompany.Rows.Count)
                    {
                        string currentValue = string.Empty;
                        if (dgvCompany.Rows[row].Cells[col].Value != null)
                            currentValue = dgvCompany.Rows[row].Cells[col].Value.ToString();

                        for (int i = row + 1; i < dgvCompany.Rows.Count; i++)
                        {
                            string tempValue = string.Empty;
                            if (dgvCompany.Rows[i].Cells[col].Value != null)
                                tempValue = dgvCompany.Rows[i].Cells[col].Value.ToString();

                            if (currentValue != tempValue || dgvCompany.Rows.Count - 1 == i)
                            {
                                dgvCompany.CurrentCell = dgvCompany.Rows[i].Cells[col];
                                if (i - 15 >= 0)
                                    dgvCompany.FirstDisplayedScrollingRowIndex = i - 15;
                                else
                                    dgvCompany.FirstDisplayedScrollingRowIndex = 0;

                                break;
                            }
                        }
                    }
                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txtTotal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int page;
                if (!int.TryParse(txtTotal.Text, out page))
                {
                    messageBox.Show(this,"페이지수는 숫자(정수) 형식으로만 입력해주세요!");
                    this.Activate();
                    return;
                }
                if (page - 1 < 1)
                {
                    messageBox.Show(this,"페이지수는 숫자(정수) 형식으로만 입력해주세요!");
                    this.Activate();
                    return;
                }

                if (dgvCompany.Rows.Count < page)
                {
                    int currentPage = dgvCompany.Rows.Count;
                    for (int i = 1; i <= page - currentPage; i++)
                    {
                        int n = dgvCompany.Rows.Add();
                        dgvCompany.Rows[n].Cells["edit_user"].Value = um.user_name;
                    }
                    SetRecordCounting();
                }
                else
                {
                    messageBox.Show(this,"현재 페이지수 보다는 크게 입력해주세요!\n  현재페이지 : " + dgvCompany.Rows.Count.ToString());
                    this.Activate();
                    return;
                }
            }
        }

        private void txtCurrent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int page;
                if (!int.TryParse(txtCurrent.Text, out page))
                {
                    messageBox.Show(this,"페이지수는 숫자(정수) 형식으로만 입력해주세요!");
                    this.Activate();
                    return;
                }
                if (page - 1 < 1)
                {
                    messageBox.Show(this,"페이지수는 숫자(정수) 형식으로만 입력해주세요!");
                    this.Activate();
                    return;
                }
                page--;


                if (dgvCompany.Rows.Count <= page)
                {
                    messageBox.Show(this,"현재 페이지수 보다는 같거나 작게 입력해주세요!\n  현재페이지 : " + dgvCompany.Rows.Count.ToString());
                    this.Activate();
                    return;
                }
                else
                {
                    dgvCompany.ClearSelection();
                    dgvCompany.Rows[page].Selected = true;
                    dgvCompany.FirstDisplayedScrollingRowIndex = page;
                    SetRecordCounting();
                }
            }
        }

        #endregion

        #region Datagridview event
        private void dgvCompany_SelectionChanged(object sender, EventArgs e)
        {
            SetRecordCounting();
        }
        private void dgvCompany_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //MessageBox.Show(this,"입력값이 잘못되었습니다. 다시 확인해주세요.");

            e.Cancel = false;
            e.ThrowException = false;
        }
        private void dgvCompany_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvCompany.Columns[e.ColumnIndex].Name == "is_duplicate")
                {
                    DataGridViewRow row = dgvCompany.Rows[e.RowIndex];

                    int common;
                    if (row.Cells["common_duplicate_count"].Value == null || !int.TryParse(row.Cells["common_duplicate_count"].Value.ToString(), out common))
                        common = 0;

                    int mydata;
                    if (row.Cells["mydata_duplicate_count"].Value == null || !int.TryParse(row.Cells["mydata_duplicate_count"].Value.ToString(), out mydata))
                        mydata = 0;

                    int potential1;
                    if (row.Cells["potential_duplicate_count"].Value == null || !int.TryParse(row.Cells["potential_duplicate_count"].Value.ToString(), out potential1))
                        potential1 = 0;

                    int petential2;
                    if (row.Cells["potential2_duplicate_count"].Value == null || !int.TryParse(row.Cells["potential2_duplicate_count"].Value.ToString(), out petential2))
                        petential2 = 0;

                    int trading;
                    if (row.Cells["isTrading_duplicate_count"].Value == null || !int.TryParse(row.Cells["isTrading_duplicate_count"].Value.ToString(), out trading))
                        trading = 0;

                    int nonehandled;
                    if (row.Cells["nonehandled_duplicate_count"].Value == null || !int.TryParse(row.Cells["nonehandled_duplicate_count"].Value.ToString(), out nonehandled))
                        nonehandled = 0;

                    int notFax;
                    if (row.Cells["notSendFax_duplicate_count"].Value == null || !int.TryParse(row.Cells["notSendFax_duplicate_count"].Value.ToString(), out notFax))
                        notFax = 0;

                    int outOfBusiness;
                    if (row.Cells["outOfBusiness_duplicate_count"].Value == null || !int.TryParse(row.Cells["outOfBusiness_duplicate_count"].Value.ToString(), out outOfBusiness))
                        outOfBusiness = 0;


                    string duplicate_tooltip = "";
                    if (common > 0)
                        duplicate_tooltip += "\n공용DATA : " + common.ToString("#,##0");
                    if (mydata > 0)
                        duplicate_tooltip += "\n내DATA : " + mydata.ToString("#,##0");
                    if (potential1 > 0)
                        duplicate_tooltip += "\n잠재1 : " + potential1.ToString("#,##0");
                    if (petential2 > 0)
                        duplicate_tooltip += "\n잠재2 : " + petential2.ToString("#,##0");
                    if (trading > 0)
                        duplicate_tooltip += "\n거래중 : " + trading.ToString("#,##0");
                    if (nonehandled > 0)
                        duplicate_tooltip += "\n취급X : " + nonehandled.ToString("#,##0");
                    if (notFax > 0)
                        duplicate_tooltip += "\n팩스X : " + notFax.ToString("#,##0");
                    if (outOfBusiness > 0)
                        duplicate_tooltip += "\n폐업 : " + outOfBusiness.ToString("#,##0");

                    row.Cells["is_duplicate"].ToolTipText = duplicate_tooltip.Trim();
                }
            }
        }
        #endregion

        #region 페업확인
        private void btnCheckRetire_Click(object sender, EventArgs e)
        {
            GetApiAsync();
        }
        private async Task GetApiAsync()
        {
            if (dgvCompany.Rows.Count == 0)
                return;

            var httpClient = new HttpClient();
            var serviceKey = "%2BSPXqSxPP9bwz%2Fgv5Dc7LCFMneAJ%2Fj%2FINtS%2ByapPN4QfMH7W81e%2Fli4cYtVZCItRwp4kFNM7wRRFPCWYabjJig%3D%3D";
            var serviceUrl = $"http://api.odcloud.kr/api/nts-businessman/v1/status?serviceKey={serviceKey}&returnType=XML";

            ProcessBar pb = new ProcessBar(dgvCompany.Rows.Count);
            pb.Show();

            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                if (dgvCompany.Rows[i].Cells["registration_number"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["registration_number"].Value.ToString()))
                {
                    string registration_number = dgvCompany.Rows[i].Cells["registration_number"].Value.ToString().Replace("-", "").Replace(" ", "");
                    var business = new Dictionary<string, string[]>
                    {
                        ["b_no"] = new string[] { registration_number },
                    };
                    var jsonData = JsonConvert.SerializeObject(business);

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(serviceUrl, content);
                    string resultXml = await result.Content.ReadAsStringAsync();
                    MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(resultXml));
                    XmlDocument xmlFile = new XmlDocument();
                    xmlFile.Load(ms);
                    XmlNodeList xmlList = xmlFile.GetElementsByTagName("BStt");
                    foreach (XmlNode item in xmlList)
                    {
                        if (item.InnerText == "폐업자")
                            dgvCompany.Rows[i].Cells["is_duplicate"].Value = "폐업";
                    }

                    pb.AddProcessing();
                }
            }

            pb.Close();
        }
        #endregion


        #region Excel download
        public void GetExeclColumn()
        {
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기
                Microsoft.Office.Interop.Excel.Worksheet wk = workSheet;

                setAutomatic(excelApp, false);

                //Data
                Excel.Range rng = workSheet.get_Range("A1", "CG" + (dgvCompany.Rows.Count + 1));
                object[,] only_data = (object[,])rng.get_Value();

                if (dgvCompany.Rows.Count > 0)
                {
                    int row = dgvCompany.Rows.Count + 1;
                    int column = dgvCompany.ColumnCount;
                    object[,] data = new object[row, column];

                    data = only_data;

                    //Header
                    for (int i = 0; i < column; i++)
                    {
                        //wk.Cells[1, i + 1].value = dgvProduct.Columns[col_name[i]].HeaderText;

                        data[1, i + 1] = dgvCompany.Columns[i].HeaderText;
                    }

                    //row data
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvCompany.ColumnCount; j++)
                        {
                            if(dgvCompany.Rows[i].Cells[j].Value != null)
                                only_data[i + 2, j + 1] = dgvCompany.Rows[i].Cells[j].Value.ToString();
                        }
                    }

                    rng.Value = data;

                }
                //Title
                int col_cnt = dgvCompany.ColumnCount;
                Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.Font.Size = 11;
                rg1.Font.Bold = true;
                //Border Line Style
                rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.RowHeight = 18;
                rg1.ColumnWidth = 15;
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
                rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

                rg1 = wk.Range[wk.Cells[2, 1], wk.Cells[dgvCompany.Rows.Count + 1, col_cnt]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //속도개선 ON
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
                this.Activate();
                setAutomatic(excelApp, true);
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
            finally
            {
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
        }

        //Excel속도개선
        private void setAutomatic(Excel.Application excel, bool auto)
        {
            if (auto)
            {
                excel.DisplayAlerts = true;
                excel.Visible = true;
                excel.ScreenUpdating = true;
                excel.DisplayStatusBar = true;
                excel.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
                excel.EnableEvents = true;
            }
            else
            {
                excel.DisplayAlerts = false;
                excel.Visible = false;
                excel.ScreenUpdating = false;
                excel.DisplayStatusBar = false;
                excel.Calculation = Excel.XlCalculation.xlCalculationManual;
                excel.EnableEvents = false;
            }
        }
        /// <summary>
        /// 엑셀 객체 해재 메소드
        /// </summary>
        /// <param name="obj"></param>
        static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);   //엑셀객체 해제
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();  //가비지 수집
            }
        }
        #endregion

        
    }
}

