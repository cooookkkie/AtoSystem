using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
using Repositories.Pending;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Pending
{
    public partial class ArrivalSchedule : Form
    {
        UsersModel um;
        ICommonRepository commonRepository = new CommonRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        IArrivalRepository arrivalRepository = new ArrivalRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public ArrivalSchedule(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }
        private void ArrivalSchedule_Load(object sender, EventArgs e)
        {
            nudSttYear.Value = DateTime.Now.AddYears(-1).Year;
            nudEndYear.Value = DateTime.Now.Year;

            //디자인 모드에서는 동작하지 않도록 한다.
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                //더블버퍼
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
            }
            SetDgvUnpendigStyleSetting();
            nudEndYear.Text = DateTime.Now.ToString("yyyy");
            GetData();
        }

        #region Method
        private void SetDgvUnpendigStyleSetting()
        {
            DataGridView dgv = dgvArrival;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            //Disable Sorting for DataGridView
            /*foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }*/
            //Style         
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            Color headerbackColor = Color.FromArgb(153, 204, 0);
            Color rowbackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["forwarder"].HeaderCell.Style.BackColor = headerbackColor;
            dgv.Columns["forwarder"].DefaultCellStyle.BackColor = rowbackColor;

            dgv.Columns["bl_status"].HeaderCell.Style.BackColor = headerbackColor;
            dgv.Columns["bl_status"].DefaultCellStyle.BackColor = rowbackColor;

            dgv.Columns["remark"].HeaderCell.Style.BackColor = headerbackColor;
            dgv.Columns["remark"].DefaultCellStyle.BackColor = rowbackColor;

            dgv.Columns["quarantine_type"].HeaderCell.Style.BackColor = headerbackColor;
            dgv.Columns["quarantine_type"].DefaultCellStyle.BackColor = rowbackColor;

            dgv.Columns["result_estimated_date"].HeaderCell.Style.BackColor = headerbackColor;
            dgv.Columns["result_estimated_date"].DefaultCellStyle.BackColor = rowbackColor;
        }
        private void GetData()
        {
            this.dgvArrival.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrival_CellValueChanged);
            dgvArrival.Rows.Clear();

            int type;
            if (rbNotyetDocument.Checked)
                type = 1;
            else if (rbReceiptDocument.Checked)
                type = 2;
            else 
                type = 3;

            DataTable arrivalDt = customsRepository.GetArrivalSchedule(nudSttYear.Value.ToString(), nudEndYear.Value.ToString(), txtDivision.Text, txtBlNo.Text, txtShippingCompany.Text
                                                                        , txtBlStatus.Text, txtAtono.Text, txtCustomOfficer.Text, txtWarehouse.Text, txtAgency.Text, type);
            if (arrivalDt.Rows.Count > 0)
            {
                for (int i = 0; i < arrivalDt.Rows.Count; i++)
                {
                    int n = dgvArrival.Rows.Add();
                    dgvArrival.Rows[n].Cells["id"].Value = arrivalDt.Rows[i]["id"].ToString();
                    dgvArrival.Rows[n].Cells["division"].Value = arrivalDt.Rows[i]["division"].ToString();
                    dgvArrival.Rows[n].Cells["bl_no"].Value = arrivalDt.Rows[i]["bl_no"].ToString();
                    dgvArrival.Rows[n].Cells["forwarder"].Value = arrivalDt.Rows[i]["forwarder"].ToString();
                    dgvArrival.Rows[n].Cells["eta"].Value = arrivalDt.Rows[i]["eta"].ToString();
                    dgvArrival.Rows[n].Cells["bl_status"].Value = arrivalDt.Rows[i]["bl_status"].ToString();
                    dgvArrival.Rows[n].Cells["products"].Value = arrivalDt.Rows[i]["product"].ToString();
                    dgvArrival.Rows[n].Cells["product_cnt"].Value = arrivalDt.Rows[i]["cnt"].ToString();
                    dgvArrival.Rows[n].Cells["products"].ToolTipText = arrivalDt.Rows[i]["product_group"].ToString().Replace(",", "\n");
                    dgvArrival.Rows[n].Cells["ato_no"].Value = arrivalDt.Rows[i]["ato_no"].ToString();
                    dgvArrival.Rows[n].Cells["custom_officer"].Value = arrivalDt.Rows[i]["broker"].ToString();
                    dgvArrival.Rows[n].Cells["warehouse"].Value = arrivalDt.Rows[i]["warehouse"].ToString();
                    dgvArrival.Rows[n].Cells["warehousing_date"].Value = arrivalDt.Rows[i]["warehousing_date"].ToString();
                    dgvArrival.Rows[n].Cells["agency"].Value = arrivalDt.Rows[i]["agency"].ToString();
                    dgvArrival.Rows[n].Cells["remark"].Value = arrivalDt.Rows[i]["remark"].ToString();
                    dgvArrival.Rows[n].Cells["sanitary_certificate"].Value = arrivalDt.Rows[i]["sanitary_certificate"].ToString();
                    dgvArrival.Rows[n].Cells["quarantine_type"].Value = arrivalDt.Rows[i]["quarantine_type"].ToString();
                    dgvArrival.Rows[n].Cells["result_estimated_date"].Value = arrivalDt.Rows[i]["result_estimated_date"].ToString();
                    //dgvArrival.Rows[n].Cells["is_quarantine"].Value = Convert.ToBoolean(arrivalDt.Rows[i]["is_quarantine"].ToString());
                }
                int rowNumber = 1;
                foreach (DataGridViewRow row in dgvArrival.Rows)
                {
                    if (row.IsNewRow) continue;
                    row.HeaderCell.Value = rowNumber;
                    rowNumber = rowNumber + 1;
                }
                dgvArrival.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            }
            this.dgvArrival.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrival_CellValueChanged);
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        private void Update()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "입항 일정", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            //유효성검사
            if (dgvArrival.Rows.Count == 0)
            {
                messageBox.Show(this, "수정할 내역이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                int cnt = 0;
                for (int i = 0; i < dgvArrival.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvArrival.Rows[i].Cells[0].Value);
                    if (isChecked)
                    {
                        cnt++;
                    }
                }
                if (cnt == 0)
                {
                    messageBox.Show(this, "수정할 내역이 없습니다.");
                    this.Activate();
                    return;
                }
            }
            //수정하기
            if (messageBox.Show(this, "수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();
                this.dgvArrival.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrival_CellValueChanged);
                for (int i = 0; i < dgvArrival.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvArrival.Rows[i].Cells[0].Value);
                    if (isChecked)
                    {
                        foreach (DataGridViewCell cell in dgvArrival.Rows[i].Cells)
                        {
                            if (cell.Value == null)
                                cell.Value = string.Empty;
                        }

                        //입항일정 데이터추가
                        ArrivalModel model = new ArrivalModel();
                        model.id = dgvArrival.Rows[i].Cells["id"].Value.ToString();
                        model.forwarder = dgvArrival.Rows[i].Cells["forwarder"].Value.ToString();
                        model.bl_status = dgvArrival.Rows[i].Cells["bl_status"].Value.ToString();
                        model.remark = dgvArrival.Rows[i].Cells["remark"].Value.ToString();
                        model.edit_date = DateTime.Now.ToString("yyyy-MM-dd");
                        model.edit_user = um.user_name;
                        model.quarantine_type = dgvArrival.Rows[i].Cells["quarantine_type"].Value.ToString();
                        model.result_estimated_date = dgvArrival.Rows[i].Cells["result_estimated_date"].Value.ToString();

                        if (rbNotyetDocument.Checked)
                            model.receipt_document = "";
                        else 
                            model.receipt_document = "O";

                        StringBuilder sql = new StringBuilder();
                        sql = arrivalRepository.DeleteSql(model.id);
                        sqlList.Add(sql);

                        sql = arrivalRepository.InsertSql(model);
                        sqlList.Add(sql);


                        if (!string.IsNullOrEmpty(dgvArrival.Rows[i].Cells["warehousing_date"].Value.ToString())
                            && !DateTime.TryParse(dgvArrival.Rows[i].Cells["warehousing_date"].Value.ToString(), out DateTime warehousing_date))
                        {
                            dgvArrival.ClearSelection();
                            dgvArrival.Rows[i].Cells["warehousing_date"].Selected = true;
                            messageBox.Show(this, "창고반입일의 값이 날짜 형식이 아닙니다.");
                        }

                        //Pending 수정
                        sql = customsRepository.UpdateSql2(model.id
                            , dgvArrival.Rows[i].Cells["division"].Value.ToString()
                            , dgvArrival.Rows[i].Cells["bl_no"].Value.ToString()
                            , dgvArrival.Rows[i].Cells["eta"].Value.ToString()
                            , dgvArrival.Rows[i].Cells["custom_officer"].Value.ToString()
                            , dgvArrival.Rows[i].Cells["warehouse"].Value.ToString()
                            , dgvArrival.Rows[i].Cells["warehousing_date"].Value.ToString()
                            , dgvArrival.Rows[i].Cells["agency"].Value.ToString()
                            , dgvArrival.Rows[i].Cells["sanitary_certificate"].Value.ToString());
                        sqlList.Add(sql);
                    }
                }
                this.dgvArrival.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrival_CellValueChanged);
                //Execute
                if (sqlList.Count > 0)
                {
                    int results = arrivalRepository.UpdateTran(sqlList);
                    if (results == -1)
                    {
                        messageBox.Show(this, "수정 중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        GetData();
                        messageBox.Show(this, "수정완료");
                        this.Activate();
                    }
                }
            }
        }
        #endregion

        #region Key event
        private void ArrivalSchedule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.A:
                        Update();
                        break;
                    case Keys.M:
                        txtDivision.Focus();
                        break;
                    case Keys.N:
                        txtDivision.Text = String.Empty;
                        txtShippingCompany.Text = String.Empty;
                        txtBlNo.Text = String.Empty;
                        txtBlStatus.Text = String.Empty;
                        txtAtono.Text = String.Empty;
                        txtCustomOfficer.Text = String.Empty;
                        txtWarehouse.Text = String.Empty;
                        txtAgency.Text = String.Empty;
                        txtDivision.Focus();
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
                    case Keys.F1:
                        rbNotyetDocument.Checked = true;
                        break;
                    case Keys.F2:
                        rbReceiptDocument.Checked = true;
                        break;
                    case Keys.F3:
                        rbAfterQuarantine.Checked = true;
                        break;
                }
            }
        }
        private void nudSttYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }
        #endregion

        #region Datagridview event
        private void dgvArrival_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgvArrival.Rows.Count > 0)
                {
                    dgvArrival.ClearSelection();
                    dgvArrival.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        private void dgvArrival_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    bool isChecked = Convert.ToBoolean(dgvArrival.Rows[e.RowIndex].Cells[0].Value);
                    if (isChecked)
                    {
                        dgvArrival.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.Red;
                    }
                    else
                    {
                        dgvArrival.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.LightGray;
                    }
                }
                if (dgvArrival.Columns[e.ColumnIndex].Name == "bl_status" 
                    && (dgvArrival.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && (dgvArrival.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "선입고X" || dgvArrival.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "선입고x)")))
                {
                    dgvArrival.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                }
            }
        }

        private void dgvArrival_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex > 0)
                {
                    this.dgvArrival.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrival_CellValueChanged);
                    dgvArrival.Rows[e.RowIndex].Cells[0].Value = true;
                    if ((dgvArrival.Columns[e.ColumnIndex].Name == "eta"
                        || dgvArrival.Columns[e.ColumnIndex].Name == "warehousing_date"
                        || dgvArrival.Columns[e.ColumnIndex].Name == "result_estimated_date")
                        && dgvArrival.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        string txt = dgvArrival.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        dgvArrival.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = common.strDatetime(txt);
                    }
                    this.dgvArrival.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrival_CellValueChanged);
                }
            }
        }
        #endregion

        #region Button
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        #endregion

        #region 우클릭 메뉴
        private void dgvArrival_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvArrival.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();

                    if (rbNotyetDocument.Checked)
                    {
                        m.Items.Add("입항반입후 (F2) 이동");
                        m.Items.Add("창고입고후 (F3) 이동");
                    }
                    else if (rbReceiptDocument.Checked)
                    {
                        m.Items.Add("입항반입전 (F1) 이동");
                        m.Items.Add("창고입고후 (F3) 이동");
                    }
                    else if (rbAfterQuarantine.Checked)
                    {
                        m.Items.Add("입항반입전 (F1) 이동");
                        m.Items.Add("입항반입후 (F2) 이동");
                        m.Items.Add("검역완료");
                    }

                    m.Items.Add("팬딩열기");
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvArrival, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch (Exception ex)
            {

            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (dgvArrival.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvArrival.SelectedRows[0];
                    if (dr.Index < 0)
                    {
                        return;
                    }

                    int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                    /*PendingInfo p;*/

                    switch (e.ClickedItem.Text)
                    {
                        case "입항반입전 (F1) 이동":
                            if (dgvArrival.SelectedRows.Count > 0)
                            {
                                //Messagebox
                                if (messageBox.Show(this, "입항반입전 (F1) 이동하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;

                                string id = dgvArrival.Rows[dgvArrival.SelectedRows[0].Index].Cells["id"].Value.ToString();
                                StringBuilder sql = arrivalRepository.DeleteSql(id);
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                sqlList.Add(sql);

                                ArrivalModel model = new ArrivalModel();
                                model.id = dgvArrival.SelectedRows[0].Cells["id"].Value.ToString();
                                model.forwarder = dgvArrival.SelectedRows[0].Cells["forwarder"].Value.ToString();
                                model.bl_status = dgvArrival.SelectedRows[0].Cells["bl_status"].Value.ToString();
                                model.remark = dgvArrival.SelectedRows[0].Cells["remark"].Value.ToString();
                                model.edit_date = DateTime.Now.ToString("yyyy-MM-dd");
                                model.edit_user = um.user_name;
                                model.receipt_document = "";
                                sql = arrivalRepository.InsertSql(model);
                                sqlList.Add(sql);

                                int result = arrivalRepository.UpdateTran(sqlList);
                                if (result == -1)
                                {
                                    messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                    this.Activate();
                                }
                                else
                                {
                                    dgvArrival.Rows.Remove(dgvArrival.Rows[dgvArrival.SelectedRows[0].Index]);
                                }
                            }
                            else
                            {
                                messageBox.Show(this, "내역을 선택해주세요.");
                                this.Activate();
                            }

                            break;
                        case "입항반입후 (F2) 이동":
                            if (dgvArrival.SelectedRows.Count > 0)
                            {
                                //Messagebox
                                if (messageBox.Show(this, "입항반입후 (F2) 이동하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                {
                                    return;
                                }

                                string id = dgvArrival.Rows[dgvArrival.SelectedRows[0].Index].Cells["id"].Value.ToString();
                                StringBuilder sql = arrivalRepository.DeleteSql(id);
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                sqlList.Add(sql);

                                ArrivalModel model = new ArrivalModel();
                                model.id = id;
                                model.forwarder = dgvArrival.SelectedRows[0].Cells["forwarder"].Value.ToString();
                                model.bl_status = dgvArrival.SelectedRows[0].Cells["bl_status"].Value.ToString();
                                model.remark = dgvArrival.SelectedRows[0].Cells["remark"].Value.ToString();
                                model.edit_date = DateTime.Now.ToString("yyyy-MM-dd");
                                model.edit_user = um.user_name;
                                model.receipt_document = "O";
                                sql = arrivalRepository.InsertSql(model);
                                sqlList.Add(sql);

                                sql = commonRepository.UpdateData("t_customs", "warehousing_date = '', is_quarantine = FALSE", $"id = {id}");
                                sqlList.Add(sql);

                                int result = arrivalRepository.UpdateTran(sqlList);
                                if (result == -1)
                                {
                                    messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                    this.Activate();
                                }
                                else
                                    dgvArrival.Rows.Remove(dgvArrival.Rows[dgvArrival.SelectedRows[0].Index]);
                            }
                            else
                            {
                                messageBox.Show(this, "내역을 선택해주세요.");
                                this.Activate();
                            }

                            break;
                        
                        case "창고입고후 (F3) 이동":
                            if (dgvArrival.SelectedRows.Count > 0)
                            {
                                //Messagebox
                                if (messageBox.Show(this, "창고입고후 (F3) 이동하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    return;

                                string id = dgvArrival.Rows[dgvArrival.SelectedRows[0].Index].Cells["id"].Value.ToString();
                                StringBuilder sql = commonRepository.UpdateData("t_customs", $"warehousing_date = '{DateTime.Now.ToString("yyyy-MM-dd")}', is_quarantine = FALSE", $"id = {id}");
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                sqlList.Add(sql);   

                                sql = commonRepository.UpdateData("t_arrival", $"receipt_document = 'O', edit_user = '{um.user_name}', edit_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'", $"id = {id}");
                                sqlList.Add(sql);

                                if (commonRepository.UpdateTran(sqlList) == -1)
                                {
                                    messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                    this.Activate();
                                }
                                else
                                {
                                    dgvArrival.Rows.Remove(dgvArrival.Rows[dgvArrival.SelectedRows[0].Index]);
                                    messageBox.Show(this, "완료");
                                    this.Activate();
                                }
                            }
                            else
                            {
                                messageBox.Show(this, "내역을 선택해주세요.");
                                this.Activate();
                            }

                            break;
                        case "검역완료":
                            if (dgvArrival.SelectedRows.Count > 0)
                            {
                                //Messagebox
                                if (messageBox.Show(this, "검역완료 처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                {
                                    return;
                                }

                                string id = dgvArrival.Rows[dgvArrival.SelectedRows[0].Index].Cells["id"].Value.ToString();
                                StringBuilder sql = commonRepository.UpdateData("t_customs", $"warehousing_date = '{DateTime.Now.ToString("yyyy-MM-dd")}', is_quarantine = TRUE", $"id = {id}");
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                sqlList.Add(sql);
                                if (commonRepository.UpdateTran(sqlList) == -1)
                                {
                                    messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                    this.Activate();
                                }
                                else
                                {
                                    dgvArrival.Rows.Remove(dgvArrival.Rows[dgvArrival.SelectedRows[0].Index]);
                                    messageBox.Show(this, "완료");
                                    this.Activate();
                                }
                            }
                            else
                            {
                                messageBox.Show(this, "내역을 선택해주세요.");
                                this.Activate();
                            }

                            break;

                        case "팬딩열기":
                            if (dgvArrival.SelectedRows.Count > 0)
                            {

                                FormCollection fc = Application.OpenForms;
                                bool isFormActive = false;
                                foreach (Form frm in fc)
                                {
                                    //iterate through
                                    if (frm.Name == "UnPendingManager")
                                    {
                                        frm.Activate();
                                        isFormActive = true;
                                    }
                                }

                                if (!isFormActive)
                                {
                                    int id = Convert.ToInt32(dgvArrival.Rows[dgvArrival.SelectedRows[0].Index].Cells["id"].Value.ToString());
                                    string cc_status = "미통관";
                                    UnPendingManager upm = new UnPendingManager(null, um, cc_status, id);
                                    upm.Show();

                                }
                                else
                                {
                                    messageBox.Show(this, "내역을 선택해주세요.");
                                    this.Activate();
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    messageBox.Show(this,ex.Message);
                    this.Activate();
                }
            }
            
        }
        #endregion

        private void rbNotyetDocument_CheckedChanged(object sender, EventArgs e)
        {
            if(rbNotyetDocument.Checked)
                GetData();
        }
        private void rbReceiptDocument_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReceiptDocument.Checked)
                GetData();
        }
        private void rbAfterQuarantine_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAfterQuarantine.Checked)
                GetData();
        }

        private void btnInOut_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "입항 일정", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (rbReceiptDocument.Checked)
            {
                int cnt = 0;
                if (dgvArrival.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvArrival.Rows.Count; i++)
                    {
                        bool isCheck = Convert.ToBoolean(dgvArrival.Rows[i].Cells["chk"].Value.ToString());
                        if (isCheck)
                        {
                            cnt++;
                        }
                    }
                }
                if (cnt == 0)
                {
                    messageBox.Show(this, "선택한 내역이 없습니다.");
                    this.Activate();
                    return;
                }

                //Messagebox
                if (messageBox.Show(this,cnt + "개 내역을 입고처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                StringBuilder sql;
                List<StringBuilder> sqlList = new List<StringBuilder>();


                string id = dgvArrival.Rows[dgvArrival.SelectedRows[0].Index].Cells["id"].Value.ToString();
                for (int i = 0; i < dgvArrival.Rows.Count; i++)
                {
                    bool isCheck = Convert.ToBoolean(dgvArrival.Rows[i].Cells["chk"].Value.ToString());
                    if (isCheck)
                    {
                        sql = customsRepository.UpdateDataString(id, "warehousing_date", DateTime.Now.ToString("yyyy-MM-dd"));
                        sqlList.Add(sql);
                    }
                    int result = customsRepository.UpdateCustomTran(sqlList);
                    if (result == -1)
                    {
                        messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        GetData();
                    }
                }
            }
        }

        
    }
}
