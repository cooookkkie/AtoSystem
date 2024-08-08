using AdoNetWindow.Model;
using Repositories;
using Repositories.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Config
{
    public partial class GroupManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IGroupItemRepository groupItemRepository = new GroupItemRepository();
        UsersModel um;
        bool isSelect;
        PurchaseManager.PurchaseUnitPriceInfo pupi = null;
        public GroupManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            dgvGroup.Columns["btnSelectGroup"].Visible = false;
        }
        public GroupManager(UsersModel uModel, PurchaseManager.PurchaseUnitPriceInfo pi, bool isSelectMode = false)
        {
            InitializeComponent();
            um = uModel;
            pupi = pi;
            isSelect = isSelectMode;
            btnSelect.Visible = true;
            
            this.dgvGroup.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGroup_CellMouseDoubleClick);
        }


        private void GroupManager_Load(object sender, EventArgs e)
        {
            dgvGroup.Columns["id"].Visible = false;
            GetGroup();
        }

        #region Method
        public void GetGroup()
        {
            dgvGroup.Rows.Clear();
            //그룹 데이터
            DataTable gDt = groupItemRepository.GetGroup("", "", txtDivision.Text, txtManager.Text);
            if (gDt.Rows.Count > 0)
            {
                for (int i = 0; i < gDt.Rows.Count; i++)
                {
                    int n = dgvGroup.Rows.Add();
                    dgvGroup.Rows[n].Cells["id"].Value = gDt.Rows[i]["id"].ToString();
                    dgvGroup.Rows[n].Cells["division"].Value = gDt.Rows[i]["division"].ToString();
                    dgvGroup.Rows[n].Cells["group_name"].Value = gDt.Rows[i]["group_name"].ToString();
                    dgvGroup.Rows[n].Cells["manager"].Value = gDt.Rows[i]["manager"].ToString();
                }
            }
        }
        public void GetItem(string id, bool isNew = false)
        {
            dgvItem.Rows.Clear();
            DataTable gDt = groupItemRepository.GetItem(id, "", "", "");
            if (gDt.Rows.Count > 0)
            {
                for (int i = 0; i < gDt.Rows.Count; i++)
                {
                    if (gDt.Rows[i]["sub_id"].ToString() != "0")
                    { 
                        int n = dgvItem.Rows.Add();
                        dgvItem.Rows[n].Cells["item_code"].Value = gDt.Rows[i]["item_code"].ToString();
                    }

                    lbId.Text = gDt.Rows[i]["id"].ToString();
                    txtSelectedDivision.Text = gDt.Rows[i]["division"].ToString();
                    txtSelectedGroupName.Text = gDt.Rows[i]["group_name"].ToString();
                    txtSelectedUpdatetime.Text = Convert.ToDateTime(gDt.Rows[i]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                    txtSelectedManager.Text = gDt.Rows[i]["manager"].ToString();
                    txtSelectedEditUser.Text = gDt.Rows[i]["edit_user"].ToString();
                }
            }
            //ITEM 선택
            if(isNew)
                btnGetItem.PerformClick();
        }

        public void AddItem(string item_code)
        {
            int id;
            if (!int.TryParse(lbId.Text, out id))
            {
                MessageBox.Show(this, "그룹을 다시 선택해주세요.");
                this.Activate();
                return;
            }
            int n = dgvItem.Rows.Add();
            dgvItem.Rows[n].Cells["item_code"].Value = item_code;
        }
        #endregion

        #region Key event
        private void GroupManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetGroup();
                        break;
                    case Keys.A:
                        btnAddItem.PerformClick();
                        break;
                    case Keys.N:
                        txtDivision.Text = String.Empty;
                        txtGroupName.Text = String.Empty;
                        txtManager.Text = String.Empty;
                        txtDivision.Focus();
                        break;
                    case Keys.M:
                        txtDivision.Focus();
                        break;
                    case Keys.W:
                        btnAddGroup.PerformClick();
                        break;
                    case Keys.S:
                        if (isSelect)
                        {
                            btnSelect.PerformClick();
                        }
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;

                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        btnGetItem.PerformClick();
                        break;
                }
            }
        }
        private void txtDivision_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetGroup();
            }
        }
        #endregion

        #region Button Click
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            Config.AddGroup ad = new AddGroup(um, this);
            ad.ShowDialog();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(lbId.Text, out id))
            {
                MessageBox.Show(this, "그룹을 선택해주세요.");
                this.Activate();
                return;
            }
            if (dgvItem.Rows.Count == 0)
            {
                MessageBox.Show(this, "등록할 ITEM을 추가해주세요.");
                this.Activate();
                return;
            }
            //MSG
            if (MessageBox.Show(this,txtSelectedGroupName.Text + "의 ITEM을 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //INSERT
            List<StringBuilder> sqlList = new List<StringBuilder>();
            GroupItemModel model = new GroupItemModel();
            model.id = id;
            //Delete
            StringBuilder sql = groupItemRepository.DeleteSql(model);
            sqlList.Add(sql);
            //Insert
            int cnt = 1;
            for (int i = 0; i < dgvItem.Rows.Count; i++)
            {
                model = new GroupItemModel();
                model.id = id;
                model.sub_id = cnt;
                model.division = txtSelectedDivision.Text;
                model.group_name = txtSelectedGroupName.Text;
                model.item_code = dgvItem.Rows[i].Cells["item_code"].Value.ToString();
                model.manager = txtSelectedManager.Text;
                model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                model.edit_user = um.user_name;

                sql = groupItemRepository.InsertSql(model);
                sqlList.Add(sql);

                cnt++;
            }

            
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                MessageBox.Show(this, "등록완료");
                this.Activate();
            }
        }

        private void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            if (dgvGroup.SelectedRows.Count > 0)
            {
                try
                {
                    //MSG
                    if (MessageBox.Show(this,dgvGroup.SelectedRows[0].Cells["group_name"].Value.ToString() + "의 ITEM을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }

                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    GroupItemModel model = new GroupItemModel();
                    model.id = Convert.ToInt32(dgvGroup.SelectedRows[0].Cells["id"].Value.ToString());

                    StringBuilder sql = groupItemRepository.DeleteSql(model);
                    sqlList.Add(sql);

                    if (commonRepository.UpdateTran(sqlList) == -1)
                    {
                        MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        GetGroup();
                    }
                }
                catch
                {
                    MessageBox.Show(this, "그룹을 다시 선택해주세요.");
                    this.Activate();
                }
            }
        }
        private void btnGetItem_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(lbId.Text, out id))
            {
                MessageBox.Show(this, "그룹을 선택해주세요.");
                this.Activate();
                return;
            }

            if (txtSelectedDivision.Text == "품목")
            {
                Product.ProductManager pm = new Product.ProductManager(um, this, true);
                pm.Text = "ITEM을 선택해주세요.";
                pm.ShowDialog();
            }
            else if (txtSelectedDivision.Text == "거래처")
            {
                Company.CompanyManager cm = new Company.CompanyManager(um, this, true);
                cm.Text = "ITEM을 선택해주세요.";
                cm.ShowDialog();
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvGroup.Rows.Count == 0)
            {
                MessageBox.Show(this, "그룹을 선택해주세요.");
                this.Activate();
                return;
            }
            else if (dgvGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "그룹을 선택해주세요.");
                this.Activate();
                return;
            }
            //MSG
            if (MessageBox.Show(this, "선택한 그룹정보를 추가하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //ADD
            if (dgvGroup.Rows.Count > 0)
            {
                for (int i = 0; i < dgvGroup.Rows.Count; i++)
                {
                    bool isChecked = dgvGroup.Rows[i].Selected;
                    if (isChecked)
                    {
                        string id = dgvGroup.Rows[i].Cells["id"].Value.ToString();
                        string division = dgvGroup.Rows[i].Cells["division"].Value.ToString();
                        DataTable gDt = groupItemRepository.GetItem(id, "", "", "");
                        if (gDt.Rows.Count > 0)
                        {
                            if (division == "품목")
                            {
                                List<string[]> productInfoList = new List<string[]>();
                                for (int j = 0; j < gDt.Rows.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(gDt.Rows[j]["item_code"].ToString()))
                                    {
                                        string[] item = gDt.Rows[j]["item_code"].ToString().Split('^');
                                        if (item.Length > 0)
                                        {
                                            string[] productInfo = new string[6];

                                            productInfo[0] = item[0];
                                            productInfo[1] = item[1];
                                            productInfo[2] = item[2];
                                            productInfo[3] = item[3];
                                            if (item[4] == "")
                                                item[4] = "0";
                                            productInfo[4] = item[4];
                                            productInfo[5] = item[5];

                                            productInfoList.Add(productInfo);
                                        }
                                    }
                                }
                                pupi.AddProduct(productInfoList);
                            }
                            else if (division == "거래처")
                            {
                                Dictionary<int, string> selectCompanyList = new Dictionary<int, string>();
                                for (int j = 0; j < gDt.Rows.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(gDt.Rows[j]["item_code"].ToString()))
                                    {
                                        string[] item = gDt.Rows[j]["item_code"].ToString().Split('^');
                                        if (item.Length > 0)
                                        {
                                            int company_id = Convert.ToInt32(item[0]);
                                            string company = item[1];
                                            selectCompanyList.Add(company_id, company);
                                        }
                                    }
                                }
                                pupi.AddCompany(selectCompanyList);
                            }
                        }
                    }   
                }
            }
        }
        #endregion

        #region Datagridview event
        private void dgvGroup_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string id = dgvGroup.Rows[e.RowIndex].Cells["id"].Value.ToString();
                GetItem(id);
            }
        }
        private void dgvGroup_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvGroup.Rows.Count == 0)
                {
                    MessageBox.Show(this, "그룹을 선택해주세요.");
                    this.Activate();
                    return;
                }
                else if (dgvGroup.SelectedRows.Count == 0)
                {
                    MessageBox.Show(this, "그룹을 선택해주세요.");
                    this.Activate();
                    return;
                }
                //MSG
                if (MessageBox.Show(this, "선택한 그룹정보를 추가하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                //ADD
                if (dgvGroup.Rows.Count > 0)
                {   
                    DataGridViewRow row = dgvGroup.Rows[e.RowIndex];
                    string id = row.Cells["id"].Value.ToString();
                    string division = row.Cells["division"].Value.ToString();
                    DataTable gDt = groupItemRepository.GetItem(id, "", "", "");
                    if (gDt.Rows.Count > 0)
                    {
                        if (division == "품목")
                        {
                            List<string[]> productInfoList = new List<string[]>();
                            for (int j = 0; j < gDt.Rows.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(gDt.Rows[j]["item_code"].ToString()))
                                {
                                    string[] item = gDt.Rows[j]["item_code"].ToString().Split('^');
                                    if (item.Length > 0)
                                    {
                                        string[] productInfo = new string[6];

                                        productInfo[0] = item[0];
                                        productInfo[1] = item[1];
                                        productInfo[2] = item[2];
                                        productInfo[3] = item[3];
                                        if (item[4] == "")
                                            item[4] = "0";
                                        productInfo[4] = item[4];
                                        productInfo[5] = item[5];

                                        productInfoList.Add(productInfo);
                                    }
                                }
                            }
                            pupi.AddProduct(productInfoList);
                        }
                        else if (division == "거래처")
                        {
                            Dictionary<int, string> selectCompanyList = new Dictionary<int, string>();
                            if (gDt.Rows.Count > 0)
                            {
                                for (int j = 0; j < gDt.Rows.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(gDt.Rows[j]["item_code"].ToString()))
                                    {
                                        string[] item = gDt.Rows[j]["item_code"].ToString().Split('^');
                                        if (item.Length > 0)
                                        {
                                            int company_id = Convert.ToInt32(item[0]);
                                            string company = item[1];
                                            selectCompanyList.Add(company_id, company);
                                        }
                                    }
                                }
                                pupi.AddCompany(selectCompanyList);
                            }
                        }
                    }
                }
            }
        }
        private void dgvGroup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvGroup.Columns[e.ColumnIndex].Name == "btnSelectGroup")
                {
                    //MSG
                    if (MessageBox.Show(this, "선택한 그룹정보를 추가하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                    //ADD
                    if (dgvGroup.Rows.Count > 0)
                    {
                        DataGridViewRow row = dgvGroup.Rows[e.RowIndex];
                        string id = row.Cells["id"].Value.ToString();
                        string division = row.Cells["division"].Value.ToString();
                        DataTable gDt = groupItemRepository.GetItem(id, "", "", "");
                        if (gDt.Rows.Count > 0)
                        {
                            if (division == "품목")
                            {
                                List<string[]> productInfoList = new List<string[]>();
                                for (int j = 0; j < gDt.Rows.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(gDt.Rows[j]["item_code"].ToString()))
                                    {
                                        string[] item = gDt.Rows[j]["item_code"].ToString().Split('^');
                                        if (item.Length > 0)
                                        {
                                            string[] productInfo = new string[6];

                                            productInfo[0] = item[0];
                                            productInfo[1] = item[1];
                                            productInfo[2] = item[2];
                                            productInfo[3] = item[3];
                                            if (item[4] == "")
                                                item[4] = "0";
                                            productInfo[4] = item[4];
                                            productInfo[5] = item[5];

                                            productInfoList.Add(productInfo);
                                        }
                                    }
                                }
                                pupi.AddProduct(productInfoList);
                            }
                            else if (division == "거래처")
                            {
                                Dictionary<int, string> selectCompanyList = new Dictionary<int, string>();
                                if (gDt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < gDt.Rows.Count; j++)
                                    {
                                        if (!string.IsNullOrEmpty(gDt.Rows[j]["item_code"].ToString()))
                                        {
                                            string[] item = gDt.Rows[j]["item_code"].ToString().Split('^');
                                            if (item.Length > 0)
                                            {
                                                int company_id = Convert.ToInt32(item[0]);
                                                string company = item[1];
                                                selectCompanyList.Add(company_id, company);
                                            }
                                        }
                                    }
                                    pupi.AddCompany(selectCompanyList);
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
