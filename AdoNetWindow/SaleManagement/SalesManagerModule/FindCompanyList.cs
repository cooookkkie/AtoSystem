using AdoNetWindow.Model;
using Repositories;
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
    public partial class FindCompanyList : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um;
        SalesManager sm = null;
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public FindCompanyList(UsersModel um, SalesManager sm, string company, string tel, string rNum, string handling_item, string distribution, bool isAtoManagerEnabled = false)
        {
            InitializeComponent();
            this.um = um;
            this.sm = sm;
            txtCompany.Text = company;  
            txtTel.Text = tel;  
            txtRegistrationNumber.Text = rNum;
            txtHandlingItem.Text = handling_item;
            txtDistribution.Text = distribution;

            txtAtoManager.Enabled = false;
            if (isAtoManagerEnabled)
                txtAtoManager.Text = "";
            else
                txtAtoManager.Text = um.user_name;
                
            GetData();
        }

        #region Button
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void GetData()
        {
            dgvCompany.Rows.Clear();

            if (string.IsNullOrEmpty(txtCompany.Text.Trim()) && string.IsNullOrEmpty(txtTel.Text.Trim()) && string.IsNullOrEmpty(txtRegistrationNumber.Text.Trim())
                && string.IsNullOrEmpty(txtHandlingItem.Text.Trim()) && string.IsNullOrEmpty(txtDistribution.Text.Trim()))
            {
                messageBox.Show(this, "검색항목을 입력해주세요!");
                this.Activate();
                return;
            }

            else if (string.IsNullOrEmpty(txtCompany.Text.Trim()) && string.IsNullOrEmpty(txtTel.Text.Trim()) && string.IsNullOrEmpty(txtRegistrationNumber.Text.Trim()) && string.IsNullOrEmpty(txtAtoManager.Text.Trim())
                && (!string.IsNullOrEmpty(txtHandlingItem.Text.Trim()) || !string.IsNullOrEmpty(txtDistribution.Text.Trim())))
            {
                messageBox.Show(this, "취급품목, 유통 검색시 담당자는 필수 항목입니다!");
                this.Activate();
                return;
            }
            else if (!string.IsNullOrEmpty(txtTel.Text.Trim()) && txtTel.Text.Trim().Length < 4)
            {
                messageBox.Show(this, "전화번호 검색시 4글자 이상 이력은 필수입니다!");
                this.Activate();
                return;
            }




            if (sm != null)
            {
                DataTable atoDt = sm.GetAtoDt();
                if (atoDt != null && atoDt.Rows.Count > 0)
                {
                    for (int i = 0; i < atoDt.Rows.Count; i++)
                    {
                        //대표 거래처ID
                        int main_id;
                        if (!int.TryParse(atoDt.Rows[i]["main_id"].ToString(), out main_id))
                            main_id = 0;
                        //서브 거래처ID
                        int sub_id;
                        if (!int.TryParse(atoDt.Rows[i]["sub_id"].ToString(), out sub_id))
                            sub_id = 0;

                        //삭제거래처
                        bool isDelete;
                        if (!bool.TryParse(atoDt.Rows[i]["isDelete"].ToString(), out isDelete))
                            isDelete = false;

                        //거래중
                        bool isTrading;
                        if (!bool.TryParse(atoDt.Rows[i]["isTrading"].ToString(), out isTrading))
                            isTrading = false;

                        //잠재1, 잠재2
                        bool isPotential1, isPotential2;
                        if (!bool.TryParse(atoDt.Rows[i]["isPotential1"].ToString(), out isPotential1))
                            isPotential1 = false;
                        if (!bool.TryParse(atoDt.Rows[i]["isPotential2"].ToString(), out isPotential2))
                            isPotential2 = false;


                        //영업금지 거래처
                        bool isNonHandled, isNotSendFax, isOutBusiness;
                        if (!bool.TryParse(atoDt.Rows[i]["isNonHandled"].ToString(), out isNonHandled))
                            isNonHandled = false;
                        if (!bool.TryParse(atoDt.Rows[i]["isNotSendFax"].ToString(), out isNotSendFax))
                            isNotSendFax = false;
                        if (!bool.TryParse(atoDt.Rows[i]["isOutBusiness"].ToString(), out isOutBusiness))
                            isOutBusiness = false;

                        string ato_manager = atoDt.Rows[i]["ato_manager"].ToString();
                        string division = "";

                        if (isNonHandled)
                            division = "취급X";
                        else if (isOutBusiness)
                            division = "폐업";
                        else if (isTrading)
                            division = "거래중";
                        else if (string.IsNullOrEmpty(ato_manager))
                            division = "공용DATA";
                        else if (isPotential1)
                            division = "잠재1";
                        else if (isPotential2)
                            division = "잠재2";
                        else
                            division = "내DATA";

                        //삭제아닌 거래처
                        if (!isDelete)
                        {
                            bool isOutput = true;
                            //거래처검색
                            string search_company = txtCompany.Text;
                            if (isOutput && !string.IsNullOrEmpty(search_company))
                            {
                                isOutput = false;
                                string[] temps = search_company.Split(' ');
                                if (temps.Length > 0)
                                {
                                    foreach (string temp in temps)
                                    {
                                        if (!string.IsNullOrEmpty(temp.Trim()))
                                        {
                                            if (atoDt.Rows[i]["company"].ToString().Contains(temp.Trim()))
                                            {
                                                isOutput = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            //전화번호검색
                            string search_tel = txtTel.Text;
                            if (isOutput && !string.IsNullOrEmpty(search_tel))
                            {
                                isOutput = false;
                                string[] temps = search_tel.Split(' ');
                                if (temps.Length > 0)
                                {
                                    foreach (string temp in temps)
                                    {
                                        if (!string.IsNullOrEmpty(temp.Trim()))
                                        {
                                            string temp_tel = temp.Replace("-", "").Replace(".", "").Replace(")", "").Replace("(", "").Replace(" ", "");
                                            string tel = atoDt.Rows[i]["tel"].ToString().Replace("-", "").Replace(".", "").Replace(")", "").Replace("(", "").Replace(" ", "");
                                            string fax = atoDt.Rows[i]["fax"].ToString().Replace("-", "").Replace(".", "").Replace(")", "").Replace("(", "").Replace(" ", "");
                                            string phone = atoDt.Rows[i]["phone"].ToString().Replace("-", "").Replace(".", "").Replace(")", "").Replace("(", "").Replace(" ", "");
                                            string other_phone = atoDt.Rows[i]["other_phone"].ToString().Replace("-", "").Replace(".", "").Replace(")", "").Replace("(", "").Replace(" ", "");

                                            if (tel.Contains(temp_tel) || fax.Contains(temp_tel) || phone.Contains(temp_tel) || other_phone.Contains(temp_tel))
                                            {
                                                isOutput = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            //사업자검색
                            string search_registration_number = txtRegistrationNumber.Text;
                            if (isOutput && !string.IsNullOrEmpty(search_registration_number))
                            {
                                isOutput = false;
                                string[] temps = search_registration_number.Split(' ');
                                if (temps.Length > 0)
                                {
                                    foreach (string temp in temps)
                                    {
                                        if (!string.IsNullOrEmpty(temp.Trim()))
                                        {
                                            if (atoDt.Rows[i]["registration_number"].ToString().Contains(temp.Trim()))
                                            {
                                                isOutput = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            //취급품목
                            string search_handling_item = txtHandlingItem.Text;
                            if (isOutput && !string.IsNullOrEmpty(search_handling_item))
                            {
                                isOutput = false;
                                string[] temps = search_handling_item.Split(' ');
                                if (temps.Length > 0)
                                {
                                    foreach (string temp in temps)
                                    {
                                        if (!string.IsNullOrEmpty(temp.Trim()))
                                        {
                                            if (common.isContains(atoDt.Rows[i]["handling_item"].ToString(), temp.Trim()))
                                            {
                                                isOutput = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!isOutput)
                                    {
                                        foreach (string temp in temps)
                                        {
                                            if (!string.IsNullOrEmpty(temp.Trim()))
                                            {
                                                if (common.isContains(atoDt.Rows[i]["seaover_handling_item"].ToString(), temp.Trim()))
                                                {
                                                    isOutput = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //유통
                            string search_distribiton = txtDistribution.Text;
                            if (isOutput && !string.IsNullOrEmpty(search_distribiton))
                            {
                                isOutput = false;
                                string[] temps = search_distribiton.Split(' ');
                                if (temps.Length > 0)
                                {
                                    foreach (string temp in temps)
                                    {
                                        if (!string.IsNullOrEmpty(temp.Trim()))
                                        {
                                            if (common.isContains(atoDt.Rows[i]["distribution"].ToString(), temp.Trim()))
                                            {
                                                isOutput = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            //담당자
                            string input_ato_manager = txtAtoManager.Text;
                            if (isOutput && !string.IsNullOrEmpty(input_ato_manager))
                            {
                                isOutput = false;
                                string[] temps = input_ato_manager.Split(' ');
                                if (temps.Length > 0)
                                {
                                    foreach (string temp in temps)
                                    {
                                        if (!string.IsNullOrEmpty(temp.Trim()))
                                        {
                                            if (atoDt.Rows[i]["ato_manager"].ToString().Contains(temp.Trim()))
                                            {
                                                isOutput = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            //출력
                            if (isOutput)
                            {
                                int n = dgvCompany.Rows.Add();
                                dgvCompany.Rows[n].Cells["company_id"].Value = atoDt.Rows[i]["id"].ToString();
                                dgvCompany.Rows[n].Cells["division"].Value = division;
                                dgvCompany.Rows[n].Cells["company"].Value = atoDt.Rows[i]["company"].ToString();
                                dgvCompany.Rows[n].Cells["ceo"].Value = atoDt.Rows[i]["ceo"].ToString();
                                dgvCompany.Rows[n].Cells["tel"].Value = HideTxt(atoDt.Rows[i]["tel"].ToString());
                                dgvCompany.Rows[n].Cells["fax"].Value = HideTxt(atoDt.Rows[i]["fax"].ToString());
                                dgvCompany.Rows[n].Cells["phone"].Value = HideTxt(atoDt.Rows[i]["phone"].ToString());
                                dgvCompany.Rows[n].Cells["registration_number"].Value = HideTxt(atoDt.Rows[i]["registration_number"].ToString());
                                dgvCompany.Rows[n].Cells["ato_manager"].Value = atoDt.Rows[i]["ato_manager"].ToString();
                                dgvCompany.Rows[n].Cells["handling_item"].Value = atoDt.Rows[i]["handling_item"].ToString();
                                dgvCompany.Rows[n].Cells["seaover_handling_item"].Value = atoDt.Rows[i]["seaover_handling_item"].ToString();
                                dgvCompany.Rows[n].Cells["distribution"].Value = atoDt.Rows[i]["distribution"].ToString();
                                dgvCompany.Rows[n].Cells["isNotSendFax"].Value = isNotSendFax;
                                dgvCompany.Rows[n].Cells["table_index"].Value = atoDt.Rows[i]["table_index"].ToString();
                                dgvCompany.Rows[n].Cells["seaover_company_code"].Value = atoDt.Rows[i]["seaover_company_code"].ToString();
                                if (!string.IsNullOrEmpty(atoDt.Rows[i]["seaover_company_code"].ToString()))
                                {
                                    dgvCompany.Rows[n].HeaderCell.Value = "S";
                                    dgvCompany.Rows[n].HeaderCell.Style.Font = new Font("굴림", 9, FontStyle.Bold);
                                    dgvCompany.Rows[n].HeaderCell.Style.ForeColor = Color.Red;
                                }
                            }
                        }
                    }
                }
            }
        }

        private string HideTxt(string tel)
        {
            if (!string.IsNullOrEmpty(tel.Trim()))
            {
                if (tel.Length == 10)
                    tel = tel.Substring(0, 3) + "***" + tel.Substring(6, 3);
                else if (tel.Length == 11)
                    tel = tel.Substring(0, 3) + "****" + tel.Substring(6, 3);
                else 
                {
                    int sttIndex = (int)Math.Truncate((double)tel.Length / 3);
                    int endIndex = (int)Math.Truncate(((double)tel.Length / 3) * 2);

                    tel = tel.Substring(0, sttIndex) + "****" + tel.Substring(endIndex, tel.Length - endIndex);
                }
            }

            return tel;   
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void FindCompanyList_KeyDown(object sender, KeyEventArgs e)
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
                    case Keys.M:
                        txtCompany.Focus();
                        break;
                    case Keys.N:
                        txtCompany.Text = String.Empty;
                        txtTel.Text = String.Empty;
                        txtRegistrationNumber.Text = String.Empty;
                        txtHandlingItem.Text = String.Empty;
                        txtDistribution.Text = String.Empty;
                        txtCompany.Focus();
                        break;
                }
            }
        }
        private void txtCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearching.PerformClick();
        }
        #endregion

        private void dgvCompany_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvCompany.EndEdit();
                if (dgvCompany.Columns[e.ColumnIndex].Name == "btnUpdate")
                {
                    int table_index;
                    if (dgvCompany.Rows[e.RowIndex].Cells["table_index"].Value == null || !Int32.TryParse(dgvCompany.Rows[e.RowIndex].Cells["table_index"].Value.ToString(), out table_index))
                    {
                        messageBox.Show(this,"데이터 정보를 찾을 수 없습니다.");
                        this.Activate();
                        return;
                    }


                    int company_id;
                    if (dgvCompany.Rows[e.RowIndex].Cells["company_id"].Value != null && Int32.TryParse(dgvCompany.Rows[e.RowIndex].Cells["company_id"].Value.ToString(), out company_id) && company_id > 0)
                    {
                        bool isNotSendFax;
                        if (dgvCompany.Rows[e.RowIndex].Cells["isNotSendFax"].Value == null || !bool.TryParse(dgvCompany.Rows[e.RowIndex].Cells["isNotSendFax"].Value.ToString(), out isNotSendFax))
                            isNotSendFax = false;

                        StringBuilder sql = commonRepository.UpdateData("t_company", $"isNotSendFax = {isNotSendFax}", $"id = {company_id}");
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        sqlList.Add(sql);

                        if (commonRepository.UpdateTran(sqlList) == -1)
                        {
                            messageBox.Show(this,"수정중 에러가 발생했습니다.");
                            this.Activate();
                        }
                        else
                        {
                            if(sm != null)
                                sm.UpdateAtoDt(table_index, "isNotSendFax", isNotSendFax);
                            messageBox.Show(this,"완료");
                            this.Activate();
                        }
                    }
                }
            }
        }
    }
}
