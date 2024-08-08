using AdoNetWindow.DashboardForSales;
using AdoNetWindow.Model;
using AdoNetWindow.PurchaseManager;
using AdoNetWindow.SEAOVER.Bookmark;
using AdoNetWindow.SEAOVER.PriceComparison;
using Repositories;
using Repositories.Group;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER
{
    public partial class BookmarkManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IGroupRepository groupRepository = new GroupRepository();
        SEAOVER.PriceComparison.PriceComparison pc = null;
        CostAccounting ca = null;
        DetailDashboard dd = null;
        DetailDashBoardForSales ddfs = null;
        public List<GroupModel> gm;
        UsersModel um;
        int registration_mode;
        string formType;
        public enum enumType
        {
            선택,
            등록,
            이동,
            복사
        }
        public BookmarkManager(UsersModel uModel, SEAOVER.PriceComparison.PriceComparison pComparison, List<GroupModel> gModel = null, int type = 0, string form_type = "업체별시세현황")
        {
            InitializeComponent();
            pc = pComparison;
            um = uModel;
            gm = gModel;
            registration_mode = type;
            formType = form_type;
            txtDivision.Text = formType;
            txtDivision.Enabled = false;
        }
        public BookmarkManager(UsersModel uModel, DetailDashBoardForSales detailDashboard, List<GroupModel> gModel = null, int type = 0, string form_type = "영업부대시보드")
        {
            InitializeComponent();
            ddfs = detailDashboard;
            um = uModel;
            gm = gModel;
            registration_mode = type;
            formType = form_type;
            txtDivision.Text = "업체별시세현황 " + formType;
            txtDivision.Enabled = false;
        }

        public BookmarkManager(UsersModel uModel, CostAccounting costAccounting, List<GroupModel> gModel = null, int type = 0, string form_type = "원가계산")
        {
            InitializeComponent();
            ca = costAccounting;
            um = uModel;
            gm = gModel;
            registration_mode = type;
            formType = form_type;
            txtDivision.Text = formType;
            txtDivision.Enabled = false;
        }
        public BookmarkManager(UsersModel uModel, DetailDashboard detailDashboard, List<GroupModel> gModel = null, int type = 0, string form_type = "무역부대시보드")
        {
            InitializeComponent();
            dd = detailDashboard;
            um = uModel;
            gm = gModel;
            registration_mode = type;
            formType = form_type;
            txtDivision.Text = "원가계산 " + formType;
            txtDivision.Enabled = false;
        }

        private void BookmarkManager_Load(object sender, EventArgs e)
        {
            dgvBookmark.Columns["id"].Visible = false;
            GetGroup();
            SetHeadeStyle();
            this.ActiveControl = txtFormName;
        }

        #region Method
        private void SelectGroup(DataGridViewRow row)
        {
            //선택
            if (registration_mode == 0)
            {
                if (pc != null)
                    pc.SetGroup(Convert.ToInt32(row.Cells["id"].Value.ToString()), row.Cells["form_name"].Value.ToString());
                else if (ca != null)
                    ca.SetGroup(Convert.ToInt32(row.Cells["id"].Value.ToString()), row.Cells["form_name"].Value.ToString());
                else if (dd != null)
                    dd.SetGroupData(Convert.ToInt32(row.Cells["id"].Value.ToString()));
                else if (ddfs != null)
                    ddfs.SetGroupData(Convert.ToInt32(row.Cells["id"].Value.ToString()));

                this.Dispose();
            }
            //등록
            else if (registration_mode == 1)
            {
                if(row.Cells["division"].Value != null && !string.IsNullOrEmpty(row.Cells["division"].Value.ToString()))
                    formType = row.Cells["division"].Value.ToString();

                if (pc != null)
                    InsertBookMark(row);
                else if (ca != null)
                    InsertBookMark(row);
                else if (dd!= null)
                    InsertBookMark(row);
                else if (ddfs != null)
                    InsertBookMark(row);
            }
            //복사
            else if (registration_mode == 2)
                CopyBookMark(row);
            //이동
            else if (registration_mode == 3)
                MoveBookMark(row);

            this.Dispose();
        }
        private void DeleteGroup(DataGridViewRow row)
        {
            if (MessageBox.Show(this, "[" + row.Cells["form_name"].Value.ToString() + "] 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(row.Cells["id"].Value.ToString());
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = groupRepository.DeleteGroup(id);
                sqlList.Add(sql);
                if (groupRepository.UpdateTrans(sqlList) == -1)
                    MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                else
                    dgvBookmark.Rows.Remove(row);
            }
        }
        public void UpdateGroup(DataGridViewRow row)
        {
            int id = Convert.ToInt32(row.Cells["id"].Value.ToString());
            BookmarkAddManager bam = new BookmarkAddManager(this, um, id, row.Cells["group_name"].Value.ToString(), row.Cells["form_name"].Value.ToString());
            bam.ShowDialog();
        }
        public void update(int id, string group, string name)
        {
            StringBuilder sql = groupRepository.UpdateGroup(id, group, name);
            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);
            if (groupRepository.UpdateTrans(sqlList) == -1)
                MessageBox.Show(this, "수정중 에러가 발생하였습니다.");
            else
            {
                for (int i = 0; i < dgvBookmark.Rows.Count; i++)
                {
                    if (dgvBookmark.Rows[i].Cells["id"].Value.ToString() == id.ToString())
                    {
                        dgvBookmark.Rows[i].Cells["group_name"].Value = group;
                        dgvBookmark.Rows[i].Cells["form_name"].Value = name;
                    }
                }
            }
        }
        private void SetHeadeStyle()
        {
            dgvBookmark.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgvBookmark.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBookmark.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgvBookmark.DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Regular);
        }
        public void InsertGroup(string group_name, string form_name)
        {
            int n = dgvBookmark.Rows.Add();
            dgvBookmark.Rows[n].Cells["id"].Value = commonRepository.GetNextId("t_group", "id");
            dgvBookmark.Rows[n].Cells["group_name"].Value = group_name;
            dgvBookmark.Rows[n].Cells["form_name"].Value = form_name;
            dgvBookmark.Rows[n].Cells["updatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd"); 
            dgvBookmark.Rows[n].Cells["createdatetime"].Value = DateTime.Now.ToString("yyyy-MM-dd");
            if (gm != null && gm.Count > 0)
                SelectGroup(dgvBookmark.Rows[n]);
            else
                MessageBox.Show(this, "선택한 품목이 없어 품목을 추가하지 않고 창을 닫을시 생성한 즐겨찾기는 자동 삭제됩니다!");
        }
        private void GetGroup()
        {
            dgvBookmark.Rows.Clear();
            DataTable dt = groupRepository.GetGroupList(txtDivision.Text, txtGroupName.Text, txtFormName.Text);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = dgvBookmark.Rows.Add();
                dgvBookmark.Rows[n].Cells["id"].Value = dt.Rows[i]["id"].ToString();
                dgvBookmark.Rows[n].Cells["division"].Value = dt.Rows[i]["group_division"].ToString();
                dgvBookmark.Rows[n].Cells["group_name"].Value = dt.Rows[i]["group_name"].ToString();
                dgvBookmark.Rows[n].Cells["form_name"].Value = dt.Rows[i]["form_name"].ToString();
                DateTime updatetime;
                if (dt.Rows[i]["updatetime"] == null || !DateTime.TryParse(dt.Rows[i]["updatetime"].ToString(), out updatetime))
                    dgvBookmark.Rows[n].Cells["updatetime"].Value = "";
                else
                    dgvBookmark.Rows[n].Cells["updatetime"].Value = updatetime.ToString("yyyy-MM-dd");

                DateTime createdatetime;
                if (dt.Rows[i]["createdatetime"] == null || !DateTime.TryParse(dt.Rows[i]["createdatetime"].ToString(), out createdatetime))
                    dgvBookmark.Rows[n].Cells["createdatetime"].Value = "";
                else
                    dgvBookmark.Rows[n].Cells["createdatetime"].Value = createdatetime.ToString("yyyy-MM-dd");
            }
        }
        private void InsertBookMark(DataGridViewRow row)
        {
            if (dgvBookmark.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(row.Cells["id"].Value.ToString());
                string group_name = row.Cells["group_name"].Value.ToString();
                string form_name = row.Cells["form_name"].Value.ToString();

                int sub_id = commonRepository.GetNextId("t_group", "sub_id", "id", id.ToString());

                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < gm.Count; i++)
                {
                    GroupModel model = new GroupModel();
                    model.id = id;
                    model.sub_id = sub_id;
                    model.group_division = formType;
                    model.group_name = group_name;
                    model.form_name = form_name;
                    model.form_type = 1;
                    model.form_remark = "";
                    model.category = gm[i].category;
                    model.product = gm[i].product;
                    model.product_code = gm[i].product_code;
                    model.origin = gm[i].origin;
                    model.origin_code = gm[i].origin_code;
                    model.sizes = gm[i].sizes;
                    model.sizes_code = gm[i].sizes_code;
                    model.unit = gm[i].unit;
                    model.cost_unit = gm[i].cost_unit;
                    model.month_around = gm[i].month_around;
                    double enable_days = gm[i].enable_days;
                    if (double.IsInfinity(enable_days))
                        enable_days = 9999999;
                    else if (double.IsNaN(enable_days))
                        enable_days = 0;
                    model.enable_days = enable_days;
                    model.price_unit = gm[i].price_unit;
                    model.unit_count = gm[i].unit_count;
                    model.seaover_unit = gm[i].seaover_unit;
                    model.division = gm[i].division;
                    model.row = 1;
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                    model.createdatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    model.offer_price = gm[i].offer_price;
                    model.offer_cost_price = gm[i].offer_cost_price;
                    model.offer_company = gm[i].offer_company;
                    model.offer_updatetime = gm[i].offer_updatetime;


                    StringBuilder sql;
                    if (model.group_division == "업체별시세현황")
                    {
                        sql = groupRepository.DeleteGroup(model);
                        sqlList.Add(sql);
                    }
                    

                    sql = groupRepository.InsertGroup(model);
                    sqlList.Add(sql);

                    sub_id++;
                }
                   
                int result = groupRepository.UpdateTrans(sqlList);
                if (result == -1)
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                else
                {
                    MessageBox.Show(this, "완료");
                    this.Dispose();
                }
                    
            }
        }
        private void InsertBookMark2(string group_name, string form_name)
        {
            if (!string.IsNullOrEmpty(form_name))
            {
                int id = commonRepository.GetNextId("t_group", "id");
                int sub_id = commonRepository.GetNextId("t_group", "sub_id", "id", id.ToString());

                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < gm.Count; i++)
                {
                    GroupModel model = new GroupModel();
                    model.id = id;
                    model.sub_id = sub_id;
                    model.group_division = formType;
                    model.group_name = group_name;
                    model.form_name = form_name;
                    model.form_type = 1;
                    model.form_remark = "";
                    model.category = gm[i].category;
                    model.product = gm[i].product;
                    model.origin = gm[i].origin;
                    model.sizes = gm[i].sizes;
                    model.unit = gm[i].unit;
                    model.cost_unit = gm[i].cost_unit;
                    model.month_around = gm[i].month_around;
                    double enable_days = gm[i].enable_days;
                    if (double.IsInfinity(enable_days))
                        enable_days = 9999999;
                    else if (double.IsNaN(enable_days))
                        enable_days = 0;
                    model.enable_days = enable_days;
                    model.price_unit = gm[i].price_unit;
                    model.unit_count = gm[i].unit_count;
                    model.seaover_unit = gm[i].seaover_unit;
                    model.division = gm[i].division;
                    model.row = 1;
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                    model.createdatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    model.offer_price = gm[i].offer_price;
                    model.offer_cost_price = gm[i].offer_cost_price;
                    model.offer_company = gm[i].offer_company;
                    model.offer_updatetime = gm[i].offer_updatetime;


                    StringBuilder sql;
                    if (model.group_division == "업체별시세현황")
                    {
                        sql = groupRepository.DeleteGroup(model);
                        sqlList.Add(sql);
                    }


                    sql = groupRepository.InsertGroup(model);
                    sqlList.Add(sql);

                    sub_id++;
                }

                int result = groupRepository.UpdateTrans(sqlList);
                if (result == -1)
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                else
                {
                    MessageBox.Show(this, "완료");
                    this.Dispose();
                }

            }
        }
        private void CopyBookMark(DataGridViewRow row)
        {
            if (dgvBookmark.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(row.Cells["id"].Value.ToString());
                string group_name = row.Cells["group_name"].Value.ToString();
                string form_name = row.Cells["form_name"].Value.ToString();

                int sub_id = commonRepository.GetNextId("t_group", "sub_id", "id", id.ToString());

                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < gm.Count; i++)
                {
                    GroupModel model = new GroupModel();
                    model.id = id;
                    model.sub_id = sub_id;
                    model.group_division = formType;
                    model.group_name = group_name;
                    model.form_name = form_name;
                    model.form_type = 1;
                    model.form_remark = "";
                    model.category = gm[i].category;
                    model.product = gm[i].product;
                    model.origin = gm[i].origin;
                    model.sizes = gm[i].sizes;
                    model.unit = gm[i].unit;
                    model.cost_unit = gm[i].cost_unit;
                    model.month_around = gm[i].month_around;
                    model.enable_days = gm[i].enable_days;
                    model.price_unit = gm[i].price_unit;
                    model.unit_count = gm[i].unit_count;
                    model.seaover_unit = gm[i].seaover_unit;
                    model.division = gm[i].division;
                    model.row = 1;
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    DateTime createdatetime;
                    if (row.Cells["createdatetime"].Value != null && DateTime.TryParse(row.Cells["createdatetime"].Value.ToString(), out createdatetime))
                        model.createdatetime = createdatetime.ToString("yyyy-MM-dd");

                    StringBuilder sql = groupRepository.DeleteGroup(model);
                    sqlList.Add(sql);

                    sql = groupRepository.InsertGroup(model);
                    sqlList.Add(sql);

                    sub_id++;
                }

                int result = groupRepository.UpdateTrans(sqlList);
                if (result == -1)
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                else
                {
                    MessageBox.Show(this, "완료");
                    this.Dispose();
                }

            }
        }

        private void MoveBookMark(DataGridViewRow row)
        {
            if (formType == "업체별시세현황" && dgvBookmark.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(row.Cells["id"].Value.ToString());
                string group_name = row.Cells["group_name"].Value.ToString();
                string form_name = row.Cells["form_name"].Value.ToString();

                int sub_id = commonRepository.GetNextId("t_group", "sub_id", "id", id.ToString());

                List<GroupModel> deleteList = new List<GroupModel>();
                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < gm.Count; i++)
                {
                    GroupModel DeleteModel = new GroupModel();
                    DeleteModel.id = gm[i].id;
                    DeleteModel.category = gm[i].category;
                    DeleteModel.product = gm[i].product;
                    DeleteModel.origin = gm[i].origin;
                    DeleteModel.sizes = gm[i].sizes;
                    DeleteModel.unit = gm[i].unit;
                    DeleteModel.price_unit = gm[i].price_unit;
                    DeleteModel.unit_count = gm[i].unit_count;
                    DeleteModel.seaover_unit = gm[i].seaover_unit;
                    DeleteModel.division = gm[i].division;
                    deleteList.Add(DeleteModel);

                    StringBuilder sql = groupRepository.DeleteGroup(DeleteModel);
                    sqlList.Add(sql);

                    GroupModel insertModel = new GroupModel();
                    insertModel.id = id;
                    insertModel.sub_id = sub_id;
                    insertModel.group_division = "업체별시세현황";
                    insertModel.group_name = group_name;
                    insertModel.form_name = form_name;
                    insertModel.form_type = 1;
                    insertModel.form_remark = "";
                    insertModel.category = gm[i].category;
                    insertModel.product = gm[i].product;
                    insertModel.origin = gm[i].origin;
                    insertModel.sizes = gm[i].sizes;
                    insertModel.unit = gm[i].unit;
                    insertModel.price_unit = gm[i].price_unit;
                    insertModel.unit_count = gm[i].unit_count;
                    insertModel.seaover_unit = gm[i].seaover_unit;
                    insertModel.division = gm[i].division;
                    insertModel.row = 1;
                    insertModel.edit_user = um.user_name;
                    insertModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                    insertModel.enable_days = gm[i].enable_days;
                    insertModel.month_around = gm[i].month_around;

                    DateTime createdatetime;
                    if (row.Cells["createdatetime"].Value != null && DateTime.TryParse(row.Cells["createdatetime"].Value.ToString(), out createdatetime))
                        insertModel.createdatetime = createdatetime.ToString("yyyy-MM-dd");

                    sql = groupRepository.InsertGroup(insertModel);
                    sqlList.Add(sql);

                    sub_id++;
                }

                int result = groupRepository.UpdateTrans(sqlList);
                if (result == -1)
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                else
                {
                    pc.MoveProductDelete(deleteList);
                    MessageBox.Show(this, "완료");
                    this.Dispose();
                }

            }
        }

        #endregion

        #region Button Click
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvBookmark.Rows.Count == 0)
            {
                MessageBox.Show(this, "내역을 선택해주세요.");
                return;
            }
            else if (dgvBookmark.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "내역을 선택해주세요.");
                return;
            }
            SelectGroup(dgvBookmark.SelectedRows[0]);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetGroup();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Bookmark.BookmarkAddManager bam = new Bookmark.BookmarkAddManager(this, um);
            bam.StartPosition = FormStartPosition.CenterParent;
            bam.ShowDialog();
        }
        public void AddBookMark(string group, string bookmark)
        {
            Bookmark.BookmarkAddManager bam = new Bookmark.BookmarkAddManager(this, um);
            bam.StartPosition = FormStartPosition.CenterParent;
            string group_name;
            string form_name;
            bool isAdd = bam.AddBookMark(group, bookmark, out group_name, out form_name);

            if (isAdd)
            {
                InsertBookMark2(group_name, form_name);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Datagridview event
        private void dgvBookmark_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                SelectGroup(dgvBookmark.Rows[e.RowIndex]);
            }
        }
        #endregion

        #region Key event
        private void txtDivision_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetGroup();
        }
        private void BookmarkManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetGroup();
                        break;
                    case Keys.A:
                        btnAdd.PerformClick();
                        break;
                    case Keys.N:
                        txtGroupName.Text = String.Empty;
                        txtFormName.Text = String.Empty;
                        txtGroupName.Focus();
                        break;
                    case Keys.M:
                        txtGroupName.Focus();
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
                    case Keys.Escape:
                        this.Dispose();
                        break;
                }
            }
        }

        private void txtGroupName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion

        #region 우클릭 메뉴
        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgvBookmark.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    if (dgvBookmark.SelectedRows.Count == 1)
                    {
                        dgvBookmark.ClearSelection();
                        dgvBookmark.Rows[e.RowIndex].Selected = true;
                    }
                    else
                        dgvBookmark.Rows[e.RowIndex].Selected = true;
                }
                else
                {
                    if (dgvBookmark.Columns[e.ColumnIndex].Name == "btnSelect")
                        SelectGroup(dgvBookmark.Rows[e.RowIndex]);
                }
            }
        }
        //우클릭 메뉴 Create
        private void dgvBookmark_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvBookmark.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();

                    if (dgvBookmark.SelectedRows.Count > 0)
                    {
                        double group_id;
                        m.Items.Add("선택");
                        m.Items.Add("수정");
                        m.Items.Add("삭제");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvBookmark, e.Location);
                    }
                }
            }
            catch
            {
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            if (dgvBookmark.SelectedRows.Count > 0)
            {
                row = dgvBookmark.SelectedRows[0];
            }

            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();

            switch (e.ClickedItem.Text)
            {
                case "선택":
                    SelectGroup(row);
                    break;
                case "수정":
                    UpdateGroup(row);
                    break;
                case "삭제":
                    DeleteGroup(row);
                    break;
            }
        }
        #endregion

        
    }
}
