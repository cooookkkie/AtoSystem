using AdoNetWindow.Model;
using Repositories;
using Repositories.Company;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement
{
    public partial class DuplicateCompany : Form
    {
        ICompanySaleInfoRepository companySaleInfoRepository = new CompanySaleInfoRepository();
        ISalesRepository salesRepository = new SalesRepository();
        ICommonRepository commonRepository =new CommonRepository();
        ICompanyRepository companyrepository = new CompanyRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        bool isAdd;
        public DuplicateCompany(UsersModel uModel, bool isAddFlag = false)
        {
            InitializeComponent();
            um = uModel;
            isAdd = isAddFlag;
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
            }
            txtTotal.Text = "1000";
            this.ActiveControl = dgvCompany;
        }

        #region Main method
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
                MessageBox.Show("거래처명은 필수입니다.");
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

                if (tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel += "0";
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

                if (tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel += "0";
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

                if (tel.Substring(0, 2) != "15" && tel.Substring(0, 2) != "16" && tel.Substring(0, 2) != "18" && tel.Substring(0, 1) != "0")
                    tel += "0";
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

        #region Button
        private void cbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            if(isAdd)
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
                MessageBox.Show("등록할 데이터가 없습니다.");
                return;
            }
            else
            {
                //삭제되지 않은 거래처
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    if (dgvCompany.Rows[i].Cells["is_duplicate"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["is_duplicate"].Value.ToString()))
                    {
                        if (MessageBox.Show(new Form { TopMost = true }, "중복 및 취급X 내역이 남아있습니다. 무시하고 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;
                        else
                            break;
                    }
                }
            }
            //데이터 유효성검사
            int errCnt = 0;
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                if (dgvCompany.Rows[i].Cells["company"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["company"].Value.ToString())
                    && ((dgvCompany.Rows[i].Cells["tel"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["tel"].Value.ToString()))
                    || (dgvCompany.Rows[i].Cells["fax"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["fax"].Value.ToString()))
                    || (dgvCompany.Rows[i].Cells["phone"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["phone"].Value.ToString())))
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
                    MessageBox.Show("중복검사를 하지 않은 내역이 있습니다. 중복검사 검사후 등록해주시기 바랍니다!");
                    return;
                }
            }
            //데이터 유형검사 에러 메세지
            if (errCnt > 0)
            {
                if (MessageBox.Show("전화번호 형식에 맞지않는 값이 있습니다. 무시하고 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }
            //거래처 구분
            SelectCompanyDivision scd = new SelectCompanyDivision();
            scd.Owner = this;
            string division = scd.SelectDivision();
            if (division == null)
                return;

            //마지막 MSG
            if (MessageBox.Show("[" + division + "] 으로 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            //데이터 등록
            List<StringBuilder> sqlList = new List<StringBuilder>();
            int id = commonRepository.GetNextId("t_company", "id");
            for (int i = 0; i < dgvCompany.Rows.Count; i++)
            {
                DataGridViewRow row = dgvCompany.Rows[i];
                for (int j = 0; j < dgvCompany.ColumnCount; j++)
                {
                    if (row.Cells[j].Value == null)
                        row.Cells[j].Value = "";
                }
                //INSERT MODEL
                if (row.Cells["company"].Value != null && !string.IsNullOrEmpty(row.Cells["company"].Value.ToString()))
                { 
                    CompanyModel cm = new CompanyModel();
                    cm.id = id;
                    cm.division = "매출처";
                    cm.registration_number = row.Cells["registration_number"].Value.ToString();
                    cm.group_name = row.Cells["group_name"].Value.ToString();
                    cm.name = row.Cells["company"].Value.ToString();
                    cm.origin = "국내";
                    cm.address = row.Cells["address"].Value.ToString();
                    cm.ceo = row.Cells["ceo"].Value.ToString();
                    cm.fax = row.Cells["fax"].Value.ToString();
                    cm.tel = row.Cells["tel"].Value.ToString();
                    cm.phone = row.Cells["phone"].Value.ToString();
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
                    cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                    cm.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                    cm.edit_user = um.user_name;

                    //거래처구분
                    if (division == "공용DATA")
                    {
                        cm.ato_manager = "";

                        cm.isPotential1 = false;
                        cm.isPotential2 = false;
                        cm.isNonHandled = false;
                        cm.isOutBusiness = false;
                        cm.isNotSendFax = false;
                    }
                    else if (division == "무작위DATA")
                    {
                        cm.ato_manager = row.Cells["edit_user"].Value.ToString();

                        cm.isPotential1 = false;
                        cm.isPotential2 = false;
                        cm.isNonHandled = false;
                        cm.isOutBusiness = false;
                        cm.isNotSendFax = false;
                    }
                    else if (division == "잠재1")
                    {
                        cm.ato_manager = row.Cells["edit_user"].Value.ToString();

                        cm.isPotential1 = true;
                        cm.isPotential2 = false;
                        cm.isNonHandled = false;
                        cm.isOutBusiness = false;
                        cm.isNotSendFax = false;
                    }
                    else if (division == "잠재2")
                    {
                        cm.ato_manager = row.Cells["edit_user"].Value.ToString();

                        cm.isPotential1 = false;
                        cm.isPotential2 = true;
                        cm.isNonHandled = false;
                        cm.isOutBusiness = false;
                        cm.isNotSendFax = false;
                    }
                    else
                    {
                        cm.isPotential1 = false;
                        cm.isPotential2 = false;
                        cm.edit_user = "";
                        if (division.Contains("취급X"))
                            cm.isNonHandled = true;
                        else
                            cm.isNonHandled = false;

                        if (division.Contains("팩스X"))
                            cm.isNotSendFax = true;
                        else
                            cm.isNotSendFax = false;

                        if (division.Contains("폐업"))
                            cm.isOutBusiness = true;
                        else
                            cm.isOutBusiness = false;
                    }

                    //Insert
                    StringBuilder sql = companyrepository.InsertCompany(cm);
                    sqlList.Add(sql);

                    id++;
                }
            }
            //Execute
            int results = commonRepository.UpdateTran(sqlList);
            if (results == -1)
                MessageBox.Show("등록중 에러가 발생하였습니다.");
            else
                MessageBox.Show("등록완료");
        }

        private void btnDeleteDuplicate_Click(object sender, EventArgs e)
        {
            if (dgvCompany.Rows.Count > 0)
            {
                for (int i = dgvCompany.Rows.Count - 1; i >= 0; i--)
                {
                    if(dgvCompany.Rows[i].Cells["is_duplicate"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["is_duplicate"].Value.ToString()))
                        dgvCompany.Rows.Remove(dgvCompany.Rows[i]);
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            if (dgvCompany.Rows.Count > 0)
            {
                //유효성검사
                dgvCompany.EndEdit();
                int errCnt = 0;
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    if (dgvCompany.Rows[i].Cells["company"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["company"].Value.ToString())
                        && ((dgvCompany.Rows[i].Cells["tel"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["tel"].Value.ToString()))
                        || (dgvCompany.Rows[i].Cells["fax"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["fax"].Value.ToString()))
                        || (dgvCompany.Rows[i].Cells["phone"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["phone"].Value.ToString()))
                        || (dgvCompany.Rows[i].Cells["registration_number"].Value != null && !string.IsNullOrEmpty(dgvCompany.Rows[i].Cells["registration_number"].Value.ToString()))
                        )
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
                }
                if (errCnt > 0)
                {
                    if (MessageBox.Show("전화번호 형식에 맞지않는 값이 있습니다. 무시하고 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }

                //입력데이터 -> Datatable
                DataTable inputDt = common.ConvertDgvToDataTable(dgvCompany);
                //상호명 없으행 삭제
                for (int i = inputDt.Rows.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrEmpty(inputDt.Rows[i]["company"].ToString().Trim()))
                        inputDt.Rows.Remove(inputDt.Rows[i]);
                    else
                        inputDt.Rows[i]["is_duplicate"] = "";
                }


                //Seaover거래처
                DataTable seaoverDt = salesRepository.GetDuplicateSeaoverList();
                seaoverDt = SetSeaoverDatatable(seaoverDt);
                seaoverDt.AcceptChanges();
                var seaover1 = (from p in inputDt.AsEnumerable()
                                join t in seaoverDt.AsEnumerable()
                                on p.Field<string>("tel") equals t.Field<string>("tel")
                                select new
                                {
                                    company = t.Field<string>("company"),
                                    current_sale_date = t.Field<string>("current_sale_date"),
                                    tel = p.Field<string>("tel"),
                                    fax = p.Field<string>("fax"),
                                    phone = p.Field<string>("phone"),
                                    is_duplicate = "SEA(TEL)"
                                }).ToList();
                var seaover2 = (from p in inputDt.AsEnumerable()
                                join t in seaoverDt.AsEnumerable()
                                on p.Field<string>("fax") equals t.Field<string>("tel")
                                select new
                                {
                                    company = t.Field<string>("company"),
                                    current_sale_date = t.Field<string>("current_sale_date"),
                                    tel = p.Field<string>("tel"),
                                    fax = p.Field<string>("fax"),
                                    phone = p.Field<string>("phone"),
                                    is_duplicate = "SEA(FAX)"
                                }).ToList();
                var seaover3 = (from p in inputDt.AsEnumerable()
                                join t in seaoverDt.AsEnumerable()
                                on p.Field<string>("phone") equals t.Field<string>("tel")
                                select new
                                {
                                    company = t.Field<string>("company"),
                                    current_sale_date = t.Field<string>("current_sale_date"),
                                    tel = p.Field<string>("tel"),
                                    fax = p.Field<string>("fax"),
                                    phone = p.Field<string>("phone"),
                                    is_duplicate = "SEA(휴대폰)"
                                }).ToList();
                var seaover7 = (from p in inputDt.AsEnumerable()
                                join t in seaoverDt.AsEnumerable()
                                on p.Field<string>("registration_number") equals t.Field<string>("registration_number")
                                select new
                                {
                                    company = t.Field<string>("company"),
                                    current_sale_date = t.Field<string>("current_sale_date"),
                                    tel = p.Field<string>("tel"),
                                    fax = p.Field<string>("fax"),
                                    phone = p.Field<string>("phone"),
                                    is_duplicate = "SEA(사업자번호)"
                                }).ToList();
                //List -> Datatable
                DataTable seaoverDt1 = ConvertListToDatatable(seaover1);
                DataTable seaoverDt2 = ConvertListToDatatable(seaover2);
                DataTable seaoverDt3 = ConvertListToDatatable(seaover3);
                DataTable seaoverDt7 = ConvertListToDatatable(seaover7);


                //취급X 중복데이터 추출
                DataTable companyDt = companyrepository.GetDuplicateList();
                var tel = (from p in inputDt.AsEnumerable()
                                  join t in companyDt.AsEnumerable()
                                  on p.Field<string>("tel") equals t.Field<string>("tel")
                                  select new
                                  {
                                      company = p.Field<string>("company"),
                                      tel = p.Field<string>("tel"),
                                      fax = p.Field<string>("fax"),
                                      phone = p.Field<string>("phone"),
                                      is_duplicate = t.Field<string>("is_duplicate")
                                  }).ToList();
                var fax = (from p in inputDt.AsEnumerable()
                                    join t in companyDt.AsEnumerable()
                                    on p.Field<string>("fax") equals t.Field<string>("tel")
                                    select new
                                    {
                                        company = p.Field<string>("company"),
                                        tel = p.Field<string>("tel"),
                                        fax = p.Field<string>("fax"),
                                        phone = p.Field<string>("phone"),
                                        is_duplicate = t.Field<string>("is_duplicate")
                                    }).ToList();
                var phone = (from p in inputDt.AsEnumerable()
                                    join t in companyDt.AsEnumerable()
                                    on p.Field<string>("phone") equals t.Field<string>("tel")
                                    select new
                                    {
                                        company = p.Field<string>("company"),
                                        tel = p.Field<string>("tel"),
                                        fax = p.Field<string>("fax"),
                                        phone = p.Field<string>("phone"),
                                        is_duplicate = t.Field<string>("is_duplicate")
                                    }).ToList();
                //List -> Datatable
                DataTable telDt = ConvertListToDatatable(tel);
                DataTable faxDt = ConvertListToDatatable(fax);
                DataTable phoneDt = ConvertListToDatatable(phone);

                bool isCompleteFlag = false;
                for (int i = 0; i < inputDt.Rows.Count; i++)
                {
                    if ((seaoverDt1 == null || seaoverDt1.Rows.Count == 0)  && (seaoverDt2 == null || seaoverDt2.Rows.Count == 0) && (seaoverDt3 == null || seaoverDt3.Rows.Count == 0) && (seaoverDt7 == null || seaoverDt7.Rows.Count == 0)
                        && (telDt == null || telDt.Rows.Count == 0) && (faxDt == null || faxDt.Rows.Count == 0) && (phoneDt == null || phoneDt.Rows.Count == 0))
                        isCompleteFlag = true;

                    //SEAOVER 거래처
                    if (!isCompleteFlag)
                    { 
                        if (seaoverDt1 != null && seaoverDt1.Rows.Count > 0
                            && !string.IsNullOrEmpty(inputDt.Rows[i]["company"].ToString()) && !string.IsNullOrEmpty(inputDt.Rows[i]["tel"].ToString()))
                        {
                            for (int j = 0; j < seaoverDt1.Rows.Count; j++)
                            {
                                if (string.IsNullOrEmpty(seaoverDt1.Rows[j]["tel"].ToString().Trim()))
                                {
                                    seaoverDt1.Rows.Remove(seaoverDt1.Rows[j]);
                                    seaoverDt1.AcceptChanges();
                                }
                                else if (inputDt.Rows[i]["tel"].ToString() == seaoverDt1.Rows[j]["tel"].ToString()
                                    || inputDt.Rows[i]["fax"].ToString() == seaoverDt1.Rows[j]["tel"].ToString()
                                    || inputDt.Rows[i]["phone"].ToString() == seaoverDt1.Rows[j]["tel"].ToString())
                                {
                                    inputDt.Rows[i]["company"] = seaoverDt1.Rows[j]["company"].ToString();
                                    inputDt.Rows[i]["current_sale_date"] = seaoverDt1.Rows[j]["current_sale_date"].ToString();

                                    if (inputDt.Rows[i]["is_duplicate"].ToString() != seaoverDt1.Rows[j]["is_duplicate"].ToString())
                                    {
                                        if (!inputDt.Rows[i]["is_duplicate"].ToString().Contains(seaoverDt1.Rows[j]["is_duplicate"].ToString()))
                                            inputDt.Rows[i]["is_duplicate"] += " " + seaoverDt1.Rows[j]["is_duplicate"].ToString();
                                    }
                                    else
                                        inputDt.Rows[i]["is_duplicate"] = seaoverDt1.Rows[j]["is_duplicate"].ToString().Trim();

                                    seaoverDt1.Rows.Remove(seaoverDt1.Rows[j]);
                                    seaoverDt1.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        if (seaoverDt2 != null && seaoverDt2.Rows.Count > 0
                            && !string.IsNullOrEmpty(inputDt.Rows[i]["company"].ToString()) && !string.IsNullOrEmpty(inputDt.Rows[i]["tel"].ToString()))
                        {
                            for (int j = 0; j < seaoverDt2.Rows.Count; j++)
                            {
                                if (string.IsNullOrEmpty(seaoverDt2.Rows[j]["tel"].ToString().Trim()))
                                {
                                    seaoverDt2.Rows.Remove(seaoverDt2.Rows[j]);
                                    seaoverDt2.AcceptChanges();
                                }
                                else if (inputDt.Rows[i]["tel"].ToString() == seaoverDt2.Rows[j]["tel"].ToString()
                                    || inputDt.Rows[i]["fax"].ToString() == seaoverDt2.Rows[j]["tel"].ToString()
                                    || inputDt.Rows[i]["phone"].ToString() == seaoverDt2.Rows[j]["tel"].ToString())
                                {
                                    inputDt.Rows[i]["company"] = seaoverDt2.Rows[j]["company"].ToString();
                                    inputDt.Rows[i]["current_sale_date"] = seaoverDt2.Rows[j]["current_sale_date"].ToString();

                                    if (inputDt.Rows[i]["is_duplicate"].ToString() != seaoverDt2.Rows[j]["is_duplicate"].ToString())
                                    {
                                        if (!inputDt.Rows[i]["is_duplicate"].ToString().Contains(seaoverDt2.Rows[j]["is_duplicate"].ToString()))
                                            inputDt.Rows[i]["is_duplicate"] += " " + seaoverDt2.Rows[j]["is_duplicate"].ToString();
                                    }
                                    else
                                        inputDt.Rows[i]["is_duplicate"] = seaoverDt2.Rows[j]["is_duplicate"].ToString().Trim();

                                    seaoverDt2.Rows.Remove(seaoverDt2.Rows[j]);
                                    seaoverDt2.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        if (seaoverDt3 != null && seaoverDt3.Rows.Count > 0
                            && !string.IsNullOrEmpty(inputDt.Rows[i]["company"].ToString()) && !string.IsNullOrEmpty(inputDt.Rows[i]["tel"].ToString()))
                        {
                            for (int j = 0; j < seaoverDt3.Rows.Count; j++)
                            {
                                if (string.IsNullOrEmpty(seaoverDt3.Rows[j]["tel"].ToString().Trim()))
                                {
                                    seaoverDt3.Rows.Remove(seaoverDt3.Rows[j]);
                                    seaoverDt3.AcceptChanges();
                                }
                                else if (inputDt.Rows[i]["tel"].ToString() == seaoverDt3.Rows[j]["tel"].ToString()
                                    || inputDt.Rows[i]["fax"].ToString() == seaoverDt3.Rows[j]["tel"].ToString()
                                    || inputDt.Rows[i]["phone"].ToString() == seaoverDt3.Rows[j]["tel"].ToString())
                                {
                                    inputDt.Rows[i]["company"] = seaoverDt3.Rows[j]["company"].ToString();
                                    inputDt.Rows[i]["current_sale_date"] = seaoverDt3.Rows[j]["current_sale_date"].ToString();

                                    if (inputDt.Rows[i]["is_duplicate"].ToString() != seaoverDt3.Rows[j]["is_duplicate"].ToString())
                                    {
                                        if (!inputDt.Rows[i]["is_duplicate"].ToString().Contains(seaoverDt3.Rows[j]["is_duplicate"].ToString()))
                                            inputDt.Rows[i]["is_duplicate"] += " " + seaoverDt3.Rows[j]["is_duplicate"].ToString();
                                    }    
                                    else
                                        inputDt.Rows[i]["is_duplicate"] = seaoverDt3.Rows[j]["is_duplicate"].ToString().Trim();

                                    seaoverDt3.Rows.Remove(seaoverDt3.Rows[j]);
                                    seaoverDt3.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        if (seaoverDt7 != null && seaoverDt7.Rows.Count > 0
                            && !string.IsNullOrEmpty(inputDt.Rows[i]["company"].ToString()) && !string.IsNullOrEmpty(inputDt.Rows[i]["registration_number"].ToString()))
                        {
                            for (int j = 0; j < seaoverDt7.Rows.Count; j++)
                            {
                                if (string.IsNullOrEmpty(seaoverDt7.Rows[j]["registration_number"].ToString().Trim()))
                                {
                                    seaoverDt7.Rows.Remove(seaoverDt7.Rows[j]);
                                    seaoverDt7.AcceptChanges();
                                }
                                else if (inputDt.Rows[i]["registration_number"].ToString() == seaoverDt7.Rows[j]["registration_number"].ToString())
                                {
                                    inputDt.Rows[i]["company"] = seaoverDt7.Rows[j]["company"].ToString();
                                    inputDt.Rows[i]["current_sale_date"] = seaoverDt7.Rows[j]["current_sale_date"].ToString();

                                    if (inputDt.Rows[i]["is_duplicate"].ToString() != seaoverDt7.Rows[j]["is_duplicate"].ToString())
                                    {
                                        if (!inputDt.Rows[i]["is_duplicate"].ToString().Contains(seaoverDt7.Rows[j]["is_duplicate"].ToString()))
                                            inputDt.Rows[i]["is_duplicate"] += " " + seaoverDt7.Rows[j]["is_duplicate"].ToString();
                                    }
                                    else
                                        inputDt.Rows[i]["is_duplicate"] = seaoverDt7.Rows[j]["is_duplicate"].ToString().Trim();

                                    seaoverDt7.Rows.Remove(seaoverDt7.Rows[j]);
                                    seaoverDt7.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        //취급X tel
                        if (telDt != null && telDt.Rows.Count > 0 
                            && !string.IsNullOrEmpty(inputDt.Rows[i]["company"].ToString()) && !string.IsNullOrEmpty(inputDt.Rows[i]["tel"].ToString()))
                        { 
                            for (int j = 0; j < telDt.Rows.Count; j++)
                            {
                                if (inputDt.Rows[i]["company"].ToString() == telDt.Rows[j]["company"].ToString()
                                    && inputDt.Rows[i]["tel"].ToString() == telDt.Rows[j]["tel"].ToString())
                                {
                                    inputDt.Rows[i]["company"] = telDt.Rows[j]["company"].ToString();
                                    if (inputDt.Rows[i]["is_duplicate"].ToString() != telDt.Rows[j]["is_duplicate"].ToString())
                                    {
                                        if (!inputDt.Rows[i]["is_duplicate"].ToString().Contains(telDt.Rows[j]["is_duplicate"].ToString()))
                                            inputDt.Rows[i]["is_duplicate"] += " " + telDt.Rows[j]["is_duplicate"].ToString();
                                    }   
                                    else
                                        inputDt.Rows[i]["is_duplicate"] = telDt.Rows[j]["is_duplicate"].ToString();

                                    telDt.Rows.Remove(telDt.Rows[j]);
                                    telDt.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        //취급X fax
                        if (faxDt != null && faxDt.Rows.Count > 0
                            && !string.IsNullOrEmpty(inputDt.Rows[i]["company"].ToString()) && !string.IsNullOrEmpty(inputDt.Rows[i]["fax"].ToString()))
                        {
                            for (int j = 0; j < faxDt.Rows.Count; j++)
                            {
                                if (inputDt.Rows[i]["company"].ToString() == faxDt.Rows[j]["company"].ToString()
                                    && inputDt.Rows[i]["fax"].ToString() == faxDt.Rows[j]["fax"].ToString())
                                {
                                    inputDt.Rows[i]["company"] = faxDt.Rows[j]["company"].ToString();
                                    if (inputDt.Rows[i]["is_duplicate"].ToString() != faxDt.Rows[j]["is_duplicate"].ToString())
                                    {
                                        if (!inputDt.Rows[i]["is_duplicate"].ToString().Contains(faxDt.Rows[j]["is_duplicate"].ToString()))
                                            inputDt.Rows[i]["is_duplicate"] += " " + faxDt.Rows[j]["is_duplicate"].ToString();
                                    }
                                    else
                                        inputDt.Rows[i]["is_duplicate"] = faxDt.Rows[j]["is_duplicate"].ToString();
                                    faxDt.Rows.Remove(faxDt.Rows[j]);
                                    faxDt.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        //취급X phone
                        if (phoneDt != null && phoneDt.Rows.Count > 0
                            && !string.IsNullOrEmpty(inputDt.Rows[i]["company"].ToString()) && !string.IsNullOrEmpty(inputDt.Rows[i]["phone"].ToString()))
                        {
                            for (int j = 0; j < phoneDt.Rows.Count; j++)
                            {
                                if (inputDt.Rows[i]["company"].ToString() == phoneDt.Rows[j]["company"].ToString()
                                    && inputDt.Rows[i]["phone"].ToString() == phoneDt.Rows[j]["phone"].ToString())
                                {

                                    inputDt.Rows[i]["company"] = phoneDt.Rows[j]["company"].ToString();
                                    if (inputDt.Rows[i]["is_duplicate"].ToString() != phoneDt.Rows[j]["is_duplicate"].ToString())
                                    {
                                        if (!inputDt.Rows[i]["is_duplicate"].ToString().Contains(phoneDt.Rows[j]["is_duplicate"].ToString()))
                                            inputDt.Rows[i]["is_duplicate"] += "," + phoneDt.Rows[j]["is_duplicate"].ToString();
                                    }
                                    else
                                        inputDt.Rows[i]["is_duplicate"] = phoneDt.Rows[j]["is_duplicate"].ToString();

                                    phoneDt.Rows.Remove(phoneDt.Rows[j]);
                                    phoneDt.AcceptChanges();
                                    break;
                                }
                            }
                        }
                    }
                    //중복검사 완료여부
                    inputDt.Rows[i]["complete_duplicate"] = true;
                }
                //반영
                inputDt.AcceptChanges();
                dgvCompany.Rows.Clear();
                for (int i = 0; i < inputDt.Rows.Count; i++)
                {
                    int n = dgvCompany.Rows.Add();
                    DataGridViewRow row = dgvCompany.Rows[n];
                    row.Cells["division"].Value = inputDt.Rows[i]["division"].ToString();
                    row.Cells["group_name"].Value = inputDt.Rows[i]["group_name"].ToString();
                    row.Cells["company"].Value = inputDt.Rows[i]["company"].ToString();
                    row.Cells["ceo"].Value = inputDt.Rows[i]["ceo"].ToString();
                    row.Cells["tel"].Value = inputDt.Rows[i]["tel"].ToString();
                    row.Cells["fax"].Value = inputDt.Rows[i]["fax"].ToString();
                    row.Cells["phone"].Value = inputDt.Rows[i]["phone"].ToString();
                    row.Cells["registration_number"].Value = inputDt.Rows[i]["registration_number"].ToString();
                    row.Cells["distribution"].Value = inputDt.Rows[i]["distribution"].ToString();
                    row.Cells["handling_item"].Value = inputDt.Rows[i]["handling_item"].ToString();
                    row.Cells["address"].Value = inputDt.Rows[i]["address"].ToString();
                    row.Cells["manager"].Value = inputDt.Rows[i]["manager"].ToString();
                    row.Cells["position"].Value = inputDt.Rows[i]["position"].ToString();
                    row.Cells["email"].Value = inputDt.Rows[i]["email"].ToString();
                    row.Cells["remark"].Value = inputDt.Rows[i]["remark"].ToString();
                    row.Cells["web"].Value = inputDt.Rows[i]["web"].ToString();
                    row.Cells["edit_user"].Value = inputDt.Rows[i]["edit_user"].ToString();

                    row.Cells["complete_duplicate"].Value = Convert.ToBoolean(inputDt.Rows[i]["complete_duplicate"].ToString());
                    row.Cells["is_duplicate"].Value = inputDt.Rows[i]["is_duplicate"].ToString();
                    DateTime sale_date;
                    if(DateTime.TryParse(inputDt.Rows[i]["current_sale_date"].ToString(), out sale_date))
                        row.Cells["current_sale_date"].Value = sale_date.ToString("yyyy-MM-dd");

                }
            }
            SetRecordCounting();
            //MessageBox.Show("완료");
        }

        private DataTable SetSeaoverDatatable(DataTable seaoverDt)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("company", typeof(string));
            dt.Columns.Add("current_sale_date", typeof(string));
            dt.Columns.Add("tel", typeof(string));
            dt.Columns.Add("registration_number", typeof(string));
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
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["전화번호2"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["전화번호2"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["팩스번호1"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["팩스번호1"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["팩스번호2"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["팩스번호2"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["휴대폰1"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["휴대폰1"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dt.Rows.Add(dr);
                    }
                    if (!string.IsNullOrEmpty(row["휴대폰2"].ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["company"] = row["거래처명"].ToString();
                        dr["current_sale_date"] = row["최종매출일자"].ToString();
                        dr["tel"] = row["휴대폰2"].ToString();
                        dr["registration_number"] = row["사업자번호"].ToString();
                        dt.Rows.Add(dr);
                    }
                }   
            }
            return dt;
        }

        #endregion

        #region Key event
        private void txtGroupName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (MessageBox.Show("변경내용을 일괄적용하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
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
                    case Keys.V:
                        string clipText = Clipboard.GetText();
                        if (string.IsNullOrEmpty(clipText) == false)
                        {
                            string[] lines = clipText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                            int idx = 0;
                            if(dgvCompany.SelectedCells.Count > 0)
                                idx = dgvCompany.SelectedCells[0].RowIndex;

                            if (lines.Length > dgvCompany.Rows.Count - idx)
                                dgvCompany.Rows.Insert(dgvCompany.Rows.Count, lines.Length - dgvCompany.Rows.Count - idx);
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
                        dgvCompany.Rows.Clear();
                        for (int i = 0; i < 1000; i++)
                        {
                            int n = dgvCompany.Rows.Add();
                            dgvCompany.Rows[n].Cells["edit_user"].Value = um.user_name;
                        }
                        SetRecordCounting();
                        break;
                }
            }
        }
        private void txtTotal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int page;
                if (!int.TryParse(txtTotal.Text, out page))
                {
                    MessageBox.Show("페이지수는 숫자(정수) 형식으로만 입력해주세요!");
                    return;
                }
                if (page - 1 < 1)
                {
                    MessageBox.Show("페이지수는 숫자(정수) 형식으로만 입력해주세요!");
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
                    MessageBox.Show("현재 페이지수 보다는 크게 입력해주세요!\n  현재페이지 : " + dgvCompany.Rows.Count.ToString());
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
                    MessageBox.Show("페이지수는 숫자(정수) 형식으로만 입력해주세요!");
                    return;
                }
                if (page - 1 < 1)
                {
                    MessageBox.Show("페이지수는 숫자(정수) 형식으로만 입력해주세요!");
                    return;
                }
                page--;


                if (dgvCompany.Rows.Count <= page)
                {
                    MessageBox.Show("현재 페이지수 보다는 같거나 작게 입력해주세요!\n  현재페이지 : " + dgvCompany.Rows.Count.ToString());
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

        #endregion        
    }
}
