using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.Company
{
    public partial class CompanyManager : Form
    {
        UsersModel um;
        ICompanyRepository companyRepository = new CompanyRepository();
        ICommonRepository commonRepository = new CommonRepository();
        bool isSelectMode = false;
        bool isMultiSelect = false;

        string selectCompany = null;
        Dictionary<int, string> selectCompanyList = null;
        PurchaseManager.PurchaseUnitPriceInfo pupi = null;
        Config.GroupManager gm = null;

        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        private string Excel03ConString = "Provider=MIcrosoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
        private string Excel07ConString = "Provider=MIcrosoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";

        public CompanyManager(UsersModel uModel, bool isSelect = false)
        {
            InitializeComponent();
            um = uModel;
            if (isSelect)
            {
                btnAdd.Visible = false;
                btnSelect.Visible = true;
                dgvCompany.Columns["chk"].Visible = false;
                dgvCompany.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            else
            {
                btnAdd.Visible = true;
                btnSelect.Visible = false;
            }
        }

        public CompanyManager(UsersModel uModel, Config.GroupManager gManager, bool isSelect = false)
        {
            InitializeComponent();
            um = uModel;
            if (isSelect)
            {
                btnAdd.Visible = false;
                btnSelect.Visible = true;
            }
            else
            {
                btnAdd.Visible = true;
                btnSelect.Visible = false;
            }
            dgvCompany.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCompany.Columns["chk"].Visible = false;
            gm = gManager;
        }

        public CompanyManager(UsersModel uModel, PurchaseManager.PurchaseUnitPriceInfo purchaseUnitPriceInfo, bool isSelect = false)
        {
            InitializeComponent();
            um = uModel;
            if (isSelect)
            {
                btnAdd.Visible = false;
                btnSelect.Visible = true;
            }
            else
            {
                btnAdd.Visible = true;
                btnSelect.Visible = false;
            }
            dgvCompany.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCompany.Columns["chk"].Visible = false;
            pupi = purchaseUnitPriceInfo;
        }

        private void CompanyManager_Load(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "거래처관리", "is_admin"))
                    cbLimitCopy.Visible = false;
                else
                    cbLimitCopy.Visible = true;
            }

            GetData();
            DgvHeaderStyleSetting();
            this.ActiveControl = txtName;

            //Image Column
            ((DataGridViewImageColumn)dgvCompany.Columns["img"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvCompany.Columns["img"].DefaultCellStyle.NullValue = null;
        }

        #region Method
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void Update()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "거래처관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            int cnt = 0;
            if (dgvCompany.Rows.Count == 0)
            {
                MessageBox.Show(this, "수정할 내역이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                        cnt++;
                }
                if (cnt == 0)
                {
                    MessageBox.Show(this, "수정할 내역이 없습니다.");
                    this.Activate();
                    return;
                }
            }
            //MSG
            if (MessageBox.Show(this, cnt + "개 거래처를 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = new StringBuilder();
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        DataGridViewRow row = dgvCompany.Rows[i];
                        CompanyModel cm = new CompanyModel();
                        cm.id = Convert.ToInt32(row.Cells["id"].Value.ToString());
                        cm.division = row.Cells["division"].Value.ToString();                        
                        cm.group_name = row.Cells["group_name"].Value.ToString();
                        cm.name = row.Cells["name"].Value.ToString();
                        cm.origin = row.Cells["origin"].Value.ToString();
                        cm.address = row.Cells["address"].Value.ToString();
                        cm.registration_number = row.Cells["registration_number"].Value.ToString();
                        cm.ceo = row.Cells["ceo"].Value.ToString();
                        cm.fax = row.Cells["fax"].Value.ToString();
                        cm.tel = row.Cells["tel"].Value.ToString();
                        cm.phone = row.Cells["phone"].Value.ToString();

                        cm.company_manager = row.Cells["company_manager"].Value.ToString();
                        cm.company_manager_position = row.Cells["company_manager_position"].Value.ToString();
                        cm.email = row.Cells["email"].Value.ToString();
                        cm.sns1 = row.Cells["sns1"].Value.ToString();
                        cm.sns2 = row.Cells["sns2"].Value.ToString();
                        cm.sns3 = row.Cells["sns3"].Value.ToString();
                        cm.web = row.Cells["web"].Value.ToString();
                        cm.remark = row.Cells["remark"].Value.ToString();

                        cm.ato_manager = row.Cells["ato_manager"].Value.ToString();
                        cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                        DateTime createtime;
                        if(row.Cells["createtime"].Value != null && DateTime.TryParse(row.Cells["createtime"].Value.ToString(), out createtime))
                            cm.createtime = createtime.ToString("yyyy-MM-dd");

                        bool isManagement1;
                        if (!bool.TryParse(row.Cells["isManagement1"].Value.ToString(), out isManagement1))
                            isManagement1 = false;
                        cm.isManagement1 = isManagement1;

                        bool isManagement2;
                        if (!bool.TryParse(row.Cells["isManagement2"].Value.ToString(), out isManagement2))
                            isManagement2 = false;
                        cm.isManagement2 = isManagement1;

                        bool isManagement3;
                        if (!bool.TryParse(row.Cells["isManagement3"].Value.ToString(), out isManagement3))
                            isManagement3 = false;
                        cm.isManagement3 = isManagement1;

                        bool isManagement4;
                        if (!bool.TryParse(row.Cells["isManagement4"].Value.ToString(), out isManagement4))
                            isManagement4 = false;
                        cm.isManagement4 = isManagement4;

                        bool isHide;
                        if (!bool.TryParse(row.Cells["isHide"].Value.ToString(), out isHide))
                            isHide = false;
                        cm.isHide = isHide;

                        bool isDelete;
                        if (!bool.TryParse(row.Cells["isDelete"].Value.ToString(), out isDelete))
                            isDelete = false;
                        cm.isDelete = isDelete;
                        //Insert
                        sql = companyRepository.UpdateCompany(cm);
                        sqlList.Add(sql);
                        
                    }
                }
                //Execute
                int results = commonRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                    GetData();
            }
        }
        public string GetCompany()
        {
            isSelectMode = true;
            this.ShowDialog();

            return selectCompany;
        }
        public Dictionary<int, string> GetCompanyList()
        {
            isSelectMode = true;
            isMultiSelect = true;
            this.ShowDialog();

            return selectCompanyList;
        }

        private void DgvHeaderStyleSetting()
        {
            //헤더 디자인
            DataGridView dgv = dgvCompany;
            dgvCompany.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!            
            dgv.Columns["id"].Visible = false;
            //dgv.Columns["btnProduct"].Visible = false;
            //dgv.Columns["product_details"].Visible = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkRed;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.KeyPreview = true;
            Init_DataGridView();
        }
        //DatagridView Double Buffered Setting
        private void Init_DataGridView()
        {
            dgvCompany.DoubleBuffered(true);
        }
        private void GetData()
        {
            dgvCompany.Rows.Clear();
            this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
            DataTable companyDt = companyRepository.GetCompany("매입처", txtGroup.Text.Trim(), txtName.Text.Trim(), cbExactly.Checked, txtOrigin.Text.Trim()
                                                , txtSkype.Text.Trim(), txtKakao.Text.Trim(), txtWechat.Text.Trim(), txtEmail.Text.Trim(), txtEditUser.Text.Trim());
            if (companyDt.Rows.Count > 0)
            {
                for (int i = 0; i < companyDt.Rows.Count; i++)
                {
                    int n = dgvCompany.Rows.Add();
                    DataGridViewRow row = dgvCompany.Rows[i];

                    row.Cells["id"].Value = companyDt.Rows[i]["id"].ToString();
                    row.Cells["division"].Value = companyDt.Rows[i]["division"].ToString();
                    row.Cells["group_name"].Value = companyDt.Rows[i]["group_name"].ToString();
                    row.Cells["registration_number"].Value = companyDt.Rows[i]["registration_number"].ToString();
                    row.Cells["name"].Value = companyDt.Rows[i]["name"].ToString();
                    row.Cells["origin"].Value = companyDt.Rows[i]["origin"].ToString();
                    row.Cells["address"].Value = companyDt.Rows[i]["address"].ToString();
                    row.Cells["ceo"].Value = companyDt.Rows[i]["ceo"].ToString();
                    row.Cells["fax"].Value = companyDt.Rows[i]["fax"].ToString();
                    row.Cells["tel"].Value = companyDt.Rows[i]["tel"].ToString();
                    row.Cells["phone"].Value = companyDt.Rows[i]["phone"].ToString();
                    row.Cells["company_manager"].Value = companyDt.Rows[i]["company_manager"].ToString();
                    row.Cells["company_manager_position"].Value = companyDt.Rows[i]["company_manager_position"].ToString();
                    row.Cells["email"].Value = companyDt.Rows[i]["email"].ToString();
                    row.Cells["sns1"].Value = companyDt.Rows[i]["sns1"].ToString();
                    row.Cells["sns2"].Value = companyDt.Rows[i]["sns2"].ToString();
                    row.Cells["sns3"].Value = companyDt.Rows[i]["sns3"].ToString();
                    row.Cells["web"].Value = companyDt.Rows[i]["web"].ToString();
                    row.Cells["remark"].Value = companyDt.Rows[i]["remark"].ToString();
                    row.Cells["ato_manager"].Value = companyDt.Rows[i]["ato_manager"].ToString();
                    row.Cells["seaover_company_code"].Value = companyDt.Rows[i]["seaover_company_code"].ToString();
                    
                    bool IsManagement;
                    if (!bool.TryParse(companyDt.Rows[i]["IsManagement1"].ToString(), out IsManagement))
                        IsManagement = false;
                    row.Cells["IsManagement1"].Value = IsManagement;
                    //맨앞 관심이미지
                    if (IsManagement)
                        row.Cells["img"].Value = Properties.Resources.Star_icon;
                    else
                        row.Cells["img"].Value = null;

                    if (!bool.TryParse(companyDt.Rows[i]["IsManagement2"].ToString(), out IsManagement))
                        IsManagement = false;
                    row.Cells["IsManagement2"].Value = IsManagement;
                    if (!bool.TryParse(companyDt.Rows[i]["IsManagement3"].ToString(), out IsManagement))
                        IsManagement = false;
                    row.Cells["IsManagement3"].Value = IsManagement;
                    if (!bool.TryParse(companyDt.Rows[i]["IsManagement4"].ToString(), out IsManagement))
                        IsManagement = false;
                    row.Cells["IsManagement4"].Value = IsManagement;

                    bool isHide;
                    if (!bool.TryParse(companyDt.Rows[i]["isHide"].ToString(), out isHide))
                        isHide = false;
                    row.Cells["isHide"].Value = isHide;
                    bool isDelete;
                    if (!bool.TryParse(companyDt.Rows[i]["isDelete"].ToString(), out isDelete))
                        isDelete = false;
                    row.Cells["isDelete"].Value = isDelete;

                    //수정, 생성
                    DateTime dt;
                    if (DateTime.TryParse(companyDt.Rows[i]["updatetime"].ToString(), out dt))
                        row.Cells["updatetime"].Value = dt.ToString("yyyy-MM-dd");

                    if (DateTime.TryParse(companyDt.Rows[i]["createtime"].ToString(), out dt))
                        row.Cells["createtime"].Value = dt.ToString("yyyy-MM-dd");
                    //row.Cells["ato_manager"].Value = companyDt.Rows[i]["ato_manager"].ToString();
                }
            }
            this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
        }
        private void InsertCompany()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "거래처관리", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            CompanyInfo ci = new CompanyInfo(um);
            ci.ShowDialog();
        }

        private void DeleteCompany(int id)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "거래처관리", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            if (MessageBox.Show(this, "거래처를 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                StringBuilder sql = new StringBuilder();
                sql = companyRepository.DeleteCompany(id);
                //거래처 정보
                List<StringBuilder> sqlList = new List<StringBuilder>();
                sqlList.Add(sql);
                int results = commonRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                    GetData();
            }
        }
        private void SetManageMentCompany(DataGridViewRow row)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "거래처관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            int id = Convert.ToInt32(row.Cells["id"].Value.ToString());
            //Ato 전산에 등록된 거래처 내역
            DataTable companyDt = companyRepository.GetCompany("", "", "", false, "", "", "", "", "", id.ToString());
            if (companyDt.Rows.Count == 0)
            {
                MessageBox.Show(this, "거래처정보를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
            //Update sql
            bool isManagement1;
            if (bool.TryParse(companyDt.Rows[0]["isManagement1"].ToString(), out isManagement1))
                isManagement1 = false;

            StringBuilder sql = commonRepository.UpdateData("t_company", $"isManagement1 = {!isManagement1}", $"id = {companyDt.Rows[0]["id"].ToString()}");
            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);
            //Execute
            int results = commonRepository.UpdateTran(sqlList);
            if (results == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                if (!isManagement1)
                    row.Cells["img"].Value = Properties.Resources.Star_icon;
                else
                    row.Cells["img"].Value = null;
            }
        }
        #endregion

        #region Key Event
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);

            if (keyData == (Keys.Control | Keys.C))
            {
                if (cbLimitCopy.Checked)
                {
                    Clipboard.Clear();
                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }

            switch (keyData)
            {
                case Keys.Up:
                    if (dgvCompany.Focused && dgvCompany.CurrentCell.RowIndex == 0)
                    {
                        txtName.Focus();
                        return true;
                    }
                    else if (tb != null && tb.Name != null && tb.Name.Length >= 3 && tb.Name.Substring(0, 3) == "txt")
                        return base.ProcessCmdKey(ref msg, keyData);
                    else
                        return base.ProcessCmdKey(ref msg, keyData);

                    return true;
                case Keys.Down:
                    tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);

                    if (dgvCompany.Focused && dgvCompany.CurrentCell.RowIndex >= 0)
                        return base.ProcessCmdKey(ref msg, keyData);
                    else if (tb != null && tb.Name != null && tb.Name.Length >= 3 && tb.Name.Substring(0, 3) == "txt")
                    {
                        if (dgvCompany.Rows.Count > 0)
                        {
                            dgvCompany.Focus();
                            dgvCompany.Rows[0].Cells["name"].Selected = true;
                        }
                    }
                    else
                        return base.ProcessCmdKey(ref msg, keyData);

                    return true;
                case Keys.Left:
                    {
                        if (tb != null && tb.Name != null && tb.Name.Length >= 3 && tb.Name.Substring(0, 3) == "txt")
                        {
                            if (string.IsNullOrEmpty(tb.Text))
                                tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                            else
                            {
                                TextBox txtBox = (TextBox)tb;
                                int line = txtBox.GetLineFromCharIndex(txtBox.SelectionStart);
                                int column = txtBox.SelectionStart - txtBox.GetFirstCharIndexFromLine(line);
                                if (column == 0)
                                    tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                                else
                                    return base.ProcessCmdKey(ref msg, keyData);
                            }
                        }
                        else
                            return base.ProcessCmdKey(ref msg, keyData);
                    }
                    return true;
                case Keys.Right:
                    {
                        if (tb != null && tb.Name != null && tb.Name.Length >= 3 && tb.Name.Substring(0, 3) == "txt")
                        {
                            if (string.IsNullOrEmpty(tb.Text))
                                tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                            else
                            {
                                TextBox txtBox = (TextBox)tb;
                                int line = txtBox.GetLineFromCharIndex(txtBox.SelectionStart);
                                int column = txtBox.SelectionStart - txtBox.GetFirstCharIndexFromLine(line);
                                if (column == txtBox.Text.Length)
                                    tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                                else
                                    return base.ProcessCmdKey(ref msg, keyData);
                            }
                        }
                        else
                            return base.ProcessCmdKey(ref msg, keyData);
                    }
                    return true;
                case Keys.Control | Keys.C:
                    if (cbLimitCopy.Checked)
                    {
                        Clipboard.Clear();
                        return true;
                    }
                    else
                        return base.ProcessCmdKey(ref msg, keyData);
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        private void CompanyManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        {
                            btnUpdate.PerformClick();
                            break;        
                        }
                    case Keys.S:
                        {
                            btnSelect.PerformClick();
                            break;
                        }
                    case Keys.Q:
                        {
                            GetData();
                            break;
                        }
                    case Keys.M:
                        {
                            txtName.Focus();
                            break;
                        }
                    case Keys.N:
                        {
                            cbDivision.Text = "";
                            txtGroup.Text = "";
                            txtName.Text = "";
                            txtOrigin.Text = "";
                            txtEditUser.Text = "";
                            txtSkype.Text = "";
                            txtKakao.Text = "";
                            txtWechat.Text = "";
                            txtEmail.Text = "";
                            txtName.Focus();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
            }
            else
            {
            }
        }
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        {
                            GetData();
                            break;
                        }
                }
            }
        }
        #endregion

        #region Button
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            InsertCompany();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (gm != null)
            {
                if (dgvCompany.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        if (dgvCompany.Rows[i].Selected)
                        {
                            string item_code = dgvCompany.Rows[i].Cells["id"].Value.ToString()
                                + "^" + dgvCompany.Rows[i].Cells["name"].Value.ToString();

                            gm.AddItem(item_code);
                        }
                    }
                }
            }
            else if (pupi != null)
            {
                Dictionary<int, string> selectCompanyList = new Dictionary<int, string>();
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    bool isChecked = dgvCompany.Rows[i].Selected;
                    if (isChecked)
                    {
                        int id = Convert.ToInt32(dgvCompany.Rows[i].Cells["id"].Value.ToString());
                        string company = dgvCompany.Rows[i].Cells["name"].Value.ToString();
                        selectCompanyList.Add(id, company);
                    }
                }
                pupi.AddCompany(selectCompanyList);
            }
            else
            {
                if (isSelectMode && dgvCompany.Rows.Count > 0)
                {
                    selectCompanyList = new Dictionary<int, string>();
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        bool isChecked = dgvCompany.Rows[i].Selected;
                        if (isChecked)
                        {
                            if (!isMultiSelect)
                            {
                                selectCompany = dgvCompany.Rows[i].Cells["name"].Value.ToString();
                                this.Dispose();
                                return;
                            }
                            else
                            {
                                int id = Convert.ToInt32(dgvCompany.Rows[i].Cells["id"].Value.ToString());
                                string company = dgvCompany.Rows[i].Cells["name"].Value.ToString();
                                selectCompanyList.Add(id, company);
                            }
                        }
                    }
                    this.Dispose();
                }
            }
        }
        #endregion

        #region Datagridview event
        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (pupi != null)
            {
                Dictionary<int, string> selectCompanyList = new Dictionary<int, string>();
                int id = Convert.ToInt32(dgvCompany.Rows[e.RowIndex].Cells["id"].Value.ToString());
                string company = dgvCompany.Rows[e.RowIndex].Cells["name"].Value.ToString();
                selectCompanyList.Add(id, company);
                pupi.AddCompany(selectCompanyList);
            }
            else if (gm != null)
            {
                if (dgvCompany.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvCompany.Rows.Count; i++)
                    {
                        if (dgvCompany.Rows[i].Selected)
                        {
                            string item_code = dgvCompany.Rows[i].Cells["id"].Value.ToString()
                                + "^" + dgvCompany.Rows[i].Cells["name"].Value.ToString();

                            gm.AddItem(item_code);
                        }
                    }
                }
            }
            else
            {
                if (btnSelect.Visible == true && dgvCompany.SelectedRows.Count > 0)
                {
                    if (!isMultiSelect)
                    {
                        dgvCompany.ClearSelection();
                        dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
                        selectCompany = dgvCompany.Rows[e.RowIndex].Cells["name"].Value.ToString();
                        this.Dispose();
                        return;
                    }
                }
            }
        }
        private void dgvCompany_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isSelectMode && dgvCompany.Rows.Count > 0)
            {
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvCompany.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        if (!isMultiSelect)
                        {
                            selectCompany = dgvCompany.Rows[i].Cells["name"].Value.ToString();
                            this.Dispose();
                            return;
                        }
                    }
                }
                this.Dispose();
            }
        }
        private void dgvCompany_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    bool isChecked = Convert.ToBoolean(dgvCompany.Rows[e.RowIndex].Cells["chk"].Value);
                    if (isChecked)
                    {
                        dgvCompany.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.Red;
                    }
                    else
                    {
                        dgvCompany.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.LightGray;
                    }
                }
            }
        }
        private void dgvCompany_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != 1)
            {
                dgvCompany.Rows[e.RowIndex].Cells["chk"].Value = true;
            }
        }
        private void dgvCompany_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                if (dgvCompany.SelectedRows.Count == 0)
                {
                    dgvCompany.ClearSelection();
                    dgvCompany.Rows[e.RowIndex].Selected = true;
                }
                else
                {
                    dgvCompany.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        #endregion

        #region 우클릭 Method
        //우클릭 메뉴 Create
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

                    ContextMenuStrip m = new ContextMenuStrip();
                    if (pupi == null && gm == null)
                    {
                        m.Items.Add("선택/해제");
                        ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                        toolStripSeparator.Name = "toolStripSeparator";
                        toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator);
                        m.Items.Add("수정");
                        m.Items.Add("삭제");
                        m.Items.Add("매입단가");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                        m.Items.Add("관심 거래처 등록/해제");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvCompany, e.Location);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (dgvCompany.SelectedRows.Count > 0)
                {
                    DataGridViewRow dr = dgvCompany.SelectedRows[0];
                    int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                    /*CompanyModel Setting*/
                    int id = Convert.ToInt32(dr.Cells["id"].Value);
                    DataTable companyDt = companyRepository.GetCompany("", "", "", false, "", "", "", "", "", id.ToString());
                    switch (e.ClickedItem.Text)
                    {
                        case "수정":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "기준정보", "거래처관리", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                DataTable cm = companyRepository.GetCompany("", "", "", false, "", "", "", "", "", "", id.ToString());
                                CompanyInfo ci = new CompanyInfo(um, cm);
                                ci.ShowDialog();
                                break;
                            }
                        case "삭제":
                            {
                                DeleteCompany(id);
                                break;
                            }
                        case "선택/해제":
                            {
                                if (dgvCompany.SelectedRows.Count > 0)
                                {
                                    bool isChecked = !Convert.ToBoolean(dgvCompany.SelectedRows[0].Cells["chk"].Value);
                                    this.dgvCompany.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                                    foreach (DataGridViewRow row in dgvCompany.SelectedRows)
                                    {
                                        row.Cells["chk"].Value = isChecked;
                                    }
                                    this.dgvCompany.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompany_CellValueChanged);
                                    dgvCompany.EndEdit();
                                }
                                break;
                            }
                        case "매입단가":
                            {
                                Dictionary<int, string> selectCompanyList = new Dictionary<int, string>();
                                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                                {
                                    bool isChecked = dgvCompany.Rows[i].Selected;
                                    if (isChecked)
                                    {
                                        string company = dgvCompany.Rows[i].Cells["name"].Value.ToString();
                                        selectCompanyList.Add(id, company);
                                    }
                                }
                                PurchaseManager.PurchaseUnitPriceInfo pupInfo = new PurchaseManager.PurchaseUnitPriceInfo(um);
                                pupInfo.AddCompany(selectCompanyList);
                                pupInfo.ShowDialog();
                                break;
                            }
                        case "관심 거래처 등록/해제":
                            SetManageMentCompany(dr);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
            }
        }
        #endregion

        #region Excel event
        //엑셀다운로드
        private void BtnRegistrationExcelDownload_Click(object sender, EventArgs e)
        {
            if (um.auth_level <= 90)
            {
                MessageBox.Show(this,"권한이 없습니다!");
                this.Activate();
                return;
            }
            excelDownload();
        }

        private void excelDownload()
        {
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기
                Microsoft.Office.Interop.Excel.Worksheet wk = workSheet;

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;
                Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, 18]];
                rg1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rg1.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                rg1.RowHeight = 18;
                rg1.ColumnWidth = 15;

                //Title
                rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, 18]];
                rg1.Font.Size = 11;
                rg1.Font.Bold = true;

                wk.Cells[1, 1].value = "그룹";
                wk.Cells[1, 2].value = "상호";
                wk.Cells[1, 3].value = "사업자번호";
                wk.Cells[1, 4].value = "국가";
                wk.Cells[1, 5].value = "주소";
                wk.Cells[1, 6].value = "대표자";
                wk.Cells[1, 7].value = "TEL";
                wk.Cells[1, 8].value = "FAX";
                wk.Cells[1, 9].value = "PHONE";

                wk.Cells[1, 10].value = "업체 담당자";
                wk.Cells[1, 11].value = "업체 담당자 직급";
                wk.Cells[1, 12].value = "이메일";
                wk.Cells[1, 13].value = "Skype (sns1)";
                wk.Cells[1, 14].value = "Kakao (sns2)";
                wk.Cells[1, 15].value = "Wechat (sns3)";
                wk.Cells[1, 16].value = "웹사이트";
                wk.Cells[1, 17].value = "비고";
                wk.Cells[1, 18].value = "아토 담당자";

                //Border Line Style
                rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, 18]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
                rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

                rg1 = wk.Range[wk.Cells[2, 1], wk.Cells[100, 18]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //속도개선 ON
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
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

        //엑셀업로드
        private void BtnRegistrationExcelUpload_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "거래처관리", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            System.Windows.Forms.OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = dialog.FileName;
                string fileExtension = Path.GetExtension(filePath);
                string header = "Yes";  //rbHeaderYes.Checked ? "Yes" : "No";
                string connectionString = string.Empty;
                string sheetName = string.Empty;

                // 확장자로 구분하여 커넥션 스트링을 가져옮
                switch (fileExtension)
                {
                    case ".xls":    //Excel 97-03
                        connectionString = string.Format(Excel03ConString, filePath, header);
                        break;
                    case ".xlsx":  //Excel 07
                        connectionString = string.Format(Excel07ConString, filePath, header);
                        break;
                }
                // 첫 번째 시트의 이름을 가져옮
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
                Console.WriteLine("sheetName = " + sheetName);

                // 첫 번째 시트의 데이타를 읽어서 datagridview 에 보이게 함.
                using (OleDbConnection con = new OleDbConnection(connectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand())  
                    {
                        using (OleDbDataAdapter oda = new OleDbDataAdapter())
                        {
                            DataTable dt = new DataTable();
                            cmd.CommandText = "SELECT * From [" + sheetName + "]";
                            cmd.Connection = con;
                            con.Open();
                            oda.SelectCommand = cmd;
                            oda.Fill(dt);
                            con.Close();

                            if(dt.Rows.Count > 0)
                            {
                                StringBuilder sql;
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                int id = commonRepository.GetNextId("t_company", "id");
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (!string.IsNullOrEmpty(dt.Rows[i]["상호"].ToString()))
                                    { 
                                        CompanyModel cm = new CompanyModel();
                                        cm.id = id;
                                        cm.division = "매입처";
                                        cm.group_name = dt.Rows[i]["그룹"].ToString().Replace("'", "");
                                        cm.registration_number = dt.Rows[i]["사업자번호"].ToString().Replace("'", "");
                                        cm.name = dt.Rows[i]["상호"].ToString().Replace("'", "");
                                        cm.origin = dt.Rows[i]["국가"].ToString().Replace("'", "");
                                        cm.address = dt.Rows[i]["주소"].ToString().Replace("'", "");
                                        cm.ceo = dt.Rows[i]["대표자"].ToString().Replace("'", "");
                                        cm.tel = dt.Rows[i]["tel"].ToString().Replace("'", "");
                                        cm.fax = dt.Rows[i]["FAX"].ToString().Replace("'", "");
                                        cm.phone = dt.Rows[i]["PHONE"].ToString().Replace("'", "");
                                        cm.company_manager = dt.Rows[i]["업체 담당자"].ToString().Replace("'", "");
                                        cm.company_manager_position = dt.Rows[i]["업체 담당자 직급"].ToString().Replace("'", "");
                                        cm.email = dt.Rows[i]["이메일"].ToString().Replace("'", "");
                                        cm.sns1 = dt.Rows[i]["Skype (sns1)"].ToString().Replace("'", "");
                                        cm.sns2 = dt.Rows[i]["Kakao (sns2)"].ToString().Replace("'", "");
                                        cm.sns3 = dt.Rows[i]["Wechat (sns3)"].ToString().Replace("'", "");
                                        cm.web = dt.Rows[i]["웹사이트"].ToString().Replace("'", "");
                                        cm.remark = dt.Rows[i]["비고"].ToString().Replace("'", "");
                                        cm.ato_manager = dt.Rows[i]["아토 담당자"].ToString().Replace("'", "");
                                        cm.createtime = DateTime.Now.ToString("yyyy-MM-dd");
                                        cm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                                        cm.edit_user = um.user_name;
                                        /*double ato_capital_rate;
                                        if (!double.TryParse(dt.Rows[i]["판매가능기준(%)"].ToString().Replace("'", ""), out ato_capital_rate))
                                            ato_capital_rate = 0;

                                        cm.ato_capital_rate = ato_capital_rate;
                                        cm.seaover_company_code = dt.Rows[i]["씨오버 코드"].ToString().Replace("'", "");*/

                                        //Insert
                                        sql = companyRepository.InsertCompany(cm);
                                        sqlList.Add(sql);
                                        id++;
                                    }
                                }

                                //Execute
                                int results = commonRepository
                                    
                                    .UpdateTran(sqlList);
                                if (results == -1)
                                {
                                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                    this.Activate();
                                }
                                else
                                {
                                    MessageBox.Show(this, "등록완료");
                                    this.Activate();
                                    Refresh();
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        
    }
}
