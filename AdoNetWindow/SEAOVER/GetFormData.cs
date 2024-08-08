using AdoNetWindow.Model;
using AdoNetWindow.SEAOVER.OneLine;
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

namespace AdoNetWindow.SEAOVER
{
    public partial class GetFormData : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IBookmarkRepository bookmarkRepository = new BookmarkRepository();
        IUsersRepository usersRepository = new UsersRepository();
        UsersModel um;
        OneLineForm olf = null;
        public GetFormData(UsersModel uModel, OneLineForm olForm = null)
        {
            InitializeComponent();
            um = uModel;
            olf = olForm;

            rbSushi.Checked = true;

            //Image Column
            ((DataGridViewImageColumn)dgvFormList.Columns["is_favorite"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvFormList.Columns["is_favorite"].DefaultCellStyle.NullValue = null;

            this.ActiveControl = txtFormname;
        }
        private void GetFormData_Load(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        #region Method
        private void SelectForm(DataGridViewRow row)
        {
            int id;
            if (row.Cells["id"].Value == null || !int.TryParse(row.Cells["id"].Value.ToString(), out id))
            {
                MessageBox.Show(this, "품목서 정보를 찾을 수 없습니다.");
                this.Activate();
            }
            else if (olf != null)
            {
                olf.InputFormData(id);
                this.Dispose();
            }
        }
        #endregion

        #region Key event
        private void txtGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }
        private void GetFormData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.S:
                        btnSelect.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.M:
                        txtFormname.Focus();
                        break;
                    case Keys.N:
                        txtGroup.Text = String.Empty;
                        txtFormname.Text = String.Empty;
                        txtCreateuser.Text = String.Empty;
                        txtFormname.Focus();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void cbIsFavorite_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvFormList.Rows.Count > 0 && dgvFormList.SelectedRows.Count > 0)
                SelectForm(dgvFormList.SelectedRows[0]);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvFormList.Rows.Clear();
            int form_type = 0;
            if (rbNormal.Checked)
                form_type = 1;
            else if (rbSushi.Checked)
                form_type = 2;
            DataTable formDt = bookmarkRepository.GetFormList(form_type, txtGroup.Text, txtFormname.Text, txtCreateuser.Text, cbIsFavorite.Checked, um.user_id);
            for (int i = 0; i < formDt.Rows.Count; i++)
            {
                int n = dgvFormList.Rows.Add();
                DataGridViewRow row = dgvFormList.Rows[n];
                row.Cells["id"].Value = formDt.Rows[i]["id"].ToString();
                row.Cells["group_name"].Value = formDt.Rows[i]["group_name"].ToString();
                row.Cells["form_name"].Value = formDt.Rows[i]["form_name"].ToString();
                row.Cells["edit_user"].Value = formDt.Rows[i]["edit_user"].ToString();
                row.Cells["form_type"].Value = formDt.Rows[i]["form_type"].ToString();
                DateTime updatetime;
                if (DateTime.TryParse(formDt.Rows[i]["updatetime"].ToString(), out updatetime))
                    row.Cells["updatetime"].Value = updatetime.ToString("yyyy-MM-dd");
                row.Cells["create_user"].Value = formDt.Rows[i]["create_user"].ToString();
                //즐겨찾기
                bool is_favorite;
                if (!bool.TryParse(formDt.Rows[i]["is_favorite"].ToString(), out is_favorite))
                    is_favorite = false;
                if (is_favorite)
                {
                    row.DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                }
                //공용 즐겨찾기
                bool is_notification;
                if (!bool.TryParse(formDt.Rows[i]["is_notification"].ToString(), out is_notification))
                    is_notification = false;
                if (is_notification)
                    row.Cells["is_favorite"].Value = Properties.Resources.Star_icon;
                else
                    row.Cells["is_favorite"].Value = null;  

            }
        }
        #endregion

        #region Datagridview event
        private void dgvFormList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
                SelectForm(dgvFormList.Rows[e.RowIndex]);
        }
        #endregion

        #region 우클릭 메뉴
        private void dgvFormList_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvFormList.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    if (this.dgvFormList.Rows.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        if (this.dgvFormList.SelectedRows.Count == 1)
                            m.Items.Add("품목서 선택");
                        m.Items.Add("공용 즐겨찾기 추가/해제");
                        m.Items.Add("나만의 즐겨찾기 추가/해제");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.Show(dgvFormList, e.Location);
                        //Selection
                        /*PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;*/
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "품목서 선택":
                    {
                        SelectForm(dgvFormList.SelectedRows[0]);
                    }
                    break;
                case "품목서 삭제":
                    {
                        if (MessageBox.Show(this, "선택하신 정보가 삭제됩니다", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            List<StringBuilder> sqlList = new List<StringBuilder>();
                            for (int i = dgvFormList.Rows.Count - 1; i >= 0; i--)
                            {
                                DataGridViewRow row = dgvFormList.Rows[i];
                                if (row.Selected)
                                {
                                    int id;
                                    if (row.Cells["id"].Value == null || !int.TryParse(row.Cells["id"].Value.ToString(), out id))
                                    {
                                        MessageBox.Show(this, "품목서 정보를 찾을 수 없습니다.");
                                        this.Activate();
                                        return;
                                    }
                                    //데이터 삭제
                                    StringBuilder sql = seaoverRepository.DeleteFormData(id.ToString());
                                    sqlList.Add(sql);
                                    //행삭제
                                    dgvFormList.Rows.Remove(row);
                                }
                            }
                            //Execute
                            if (seaoverRepository.UpdateTrans(sqlList) == -1)
                            {
                                MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                        }
                    }
                    break;
                case "나만의 즐겨찾기 추가/해제":
                        UpdateBookmark(false);
                    break;
                case "공용 즐겨찾기 추가/해제":
                        UpdateBookmark(true);
                    break;
                default:
                    break;
            }
        }

        private void UpdateBookmark(bool is_notice = false)
        {
            List<StringBuilder> sqlList = new List<StringBuilder>();
            for (int i = 0; i < dgvFormList.RowCount; i++)
            {
                DataGridViewRow row = dgvFormList.Rows[i];
                if (row.Selected)
                {
                    //즐겨찾기 유무
                    bool is_favorite;
                    if (row.Cells["is_favorite"].Value == null || string.IsNullOrEmpty(row.Cells["is_favorite"].Value.ToString()))
                        is_favorite = false;
                    else
                        is_favorite = true;

                    //공용 즐겨찾기
                    bool is_notification;
                    if (row.Cells["is_notification"].Value == null || !bool.TryParse(row.Cells["is_notification"].Value.ToString(), out is_notification))
                        is_notification = false;

                    //데이터 유효성검사
                    UsersModel usermodel = usersRepository.GetUserInfo(row.Cells["create_user"].Value.ToString());
                    if (usermodel == null)
                    {
                        MessageBox.Show(this, "[" + row.Cells["id"].Value.ToString() + "]" + row.Cells["form_name"].Value.ToString() + " 품목서의 " + row.Cells["create_user"].Value.ToString() + "는(은) 유저정보를 찾을 수 없습니다.", " ");
                        this.Activate();
                        return;
                    }
                    else if (usermodel.auth_level < 95 && is_notification)
                    {
                        MessageBox.Show(this, "[" + row.Cells["id"].Value.ToString() + "]" + row.Cells["form_name"].Value.ToString() + " 품목서는 권한이 없는 즐겨찾기입니다.", " ");
                        this.Activate();
                        return;
                    }
                    else if (usermodel.auth_level > um.auth_level)
                    {
                        MessageBox.Show(this, "[" + row.Cells["id"].Value.ToString() + "]" + row.Cells["form_name"].Value.ToString() + " 품목서는 권한이 없는 즐겨찾기입니다.", " ");
                        this.Activate();
                        return;
                    }

                    //Insert
                    BookmarkModel model = new BookmarkModel();
                    model.user_id = um.user_id;
                    model.form_id = Convert.ToInt16(row.Cells["id"].Value.ToString());
                    model.form_type = Convert.ToInt16(row.Cells["form_type"].Value.ToString()); ;
                    model.is_notification = is_notice;

                    StringBuilder sql = bookmarkRepository.DeleteSql(model);
                    sqlList.Add(sql);

                    if (!is_favorite)
                    {
                        sql = bookmarkRepository.InsertSql(model);
                        sqlList.Add(sql);

                        row.Cells["is_favorite"].Value = Properties.Resources.Star_icon;
                    }
                    else
                        row.Cells["is_favorite"].Value = null;

                }
            }
            if (sqlList.Count > 0)
            {
                int results = bookmarkRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                btnSearch.PerformClick();
            }
        }

        #endregion

        private void rbSushi_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
    }
}
