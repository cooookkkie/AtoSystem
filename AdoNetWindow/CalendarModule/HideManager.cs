using AdoNetWindow.Model;
using Repositories;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.CalendarModule
{
    public partial class HideManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IHideRepository hideRepository = new HideRepository();
        UsersModel um;
        public HideManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }

        private void HideManager_Load(object sender, EventArgs e)
        {
            dgvGuarantee.Columns["id"].Visible = false;
            GuaranteeStock();
        }

        #region Method
        private void GuaranteeStock()
        {
            dgvGuarantee.Rows.Clear();
            //제외 테이블
            DataTable hDt = hideRepository.GetHideTable(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), "", "", "", "", "", "담보가능재고");
            if (hDt.Rows.Count > 0)
            {
                for (int i = 0; i < hDt.Rows.Count; i++)
                {
                    int n = dgvGuarantee.Rows.Add();

                    dgvGuarantee.Rows[n].Cells["id"].Value = hDt.Rows[i]["id"].ToString();
                    dgvGuarantee.Rows[n].Cells["category"].Value = hDt.Rows[i]["category"].ToString();
                    dgvGuarantee.Rows[n].Cells["until_date"].Value = Convert.ToDateTime(hDt.Rows[i]["until_date"].ToString()).ToString("yyyy-MM-dd");
                    dgvGuarantee.Rows[n].Cells["edit_user"].Value = hDt.Rows[i]["edit_user"].ToString();
                    dgvGuarantee.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(hDt.Rows[i]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                }
            }
        }
        #endregion

        #region Key event
        private void HideManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        #endregion

        #region 우클릭 메뉴 이벤트
        private void multiHeaderGrid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    //Selection
                    dgvGuarantee.ClearSelection();
                    DataGridViewRow selectRow = this.dgvGuarantee.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;
                }
            }
            catch
            {
            }
        }

        private void multiHeaderGrid1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvGuarantee.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("수정");
                    m.Items.Add("삭제");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvGuarantee, e.Location);
                }
            }
            catch
            { }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (dgvGuarantee.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row = dgvGuarantee.SelectedRows[0];
                    if (row.Index < 0)
                    {
                        return;
                    }
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    StringBuilder sql = new StringBuilder();
                    HideModel hm = new HideModel();

                    DataGridViewRow dgvRow = dgvGuarantee.SelectedRows[0];

                    switch (e.ClickedItem.Text)
                    {
                        case "수정":
                            hm.id = Convert.ToInt32(dgvRow.Cells["id"].Value);
                            hm.division = "담보가능재고";
                            hm.category = dgvRow.Cells["category"].Value.ToString();
                            hm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                            hm.edit_user = um.user_name;

                            DateTime until_date;
                            if (dgvRow.Cells["until_date"].Value != null && DateTime.TryParse(dgvRow.Cells["until_date"].Value.ToString(), out until_date))
                            {
                                hm.until_date = until_date.ToString("yyyy-MM-dd");
                                dgvRow.Cells["until_date"].Value = until_date.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                MessageBox.Show(this, "제외일자를 확인해주세요.");
                                this.Activate();
                            }
                            sql = hideRepository.UpdateHide(hm);
                            sqlList.Add(sql);
                            if (hideRepository.UpdateTrans(sqlList) == -1)
                            {
                                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            else
                            {
                                GuaranteeStock();
                            }
                            break;
                        case "삭제":
                            hm.id = Convert.ToInt32(dgvRow.Cells["id"].Value);
                           
                            sql = hideRepository.DeleteHide(hm.id);
                            sqlList.Add(sql);
                            if (hideRepository.UpdateTrans(sqlList) == -1)
                            {
                                MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            else
                            {
                                GuaranteeStock();
                            }
                            break;
                    }
                }
                catch
                { }
            }
        }
        #endregion

        #region Click event
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        

        private void dgvGuarantee_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvGuarantee.Columns[e.ColumnIndex].Name == "until_date")
                {
                    Common.Calendar cd = new Common.Calendar();
                    string sDate = cd.GetDate(true);
                    if (sDate != null)
                    {
                        DateTime tmpDate = Convert.ToDateTime(sDate);
                        dgvGuarantee.Rows[e.RowIndex].Cells["until_date"].Value = tmpDate.ToString("yyyy-MM-dd");
                        dgvGuarantee.EndEdit();
                    }
                }
            }
        }
        #endregion
    }
}
