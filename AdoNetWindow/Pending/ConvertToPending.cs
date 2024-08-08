using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
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

namespace AdoNetWindow.Pending
{
    public partial class ConvertToPending : Form
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        IStockRepository stockRepository = new StockRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        UnconfirmedPending pList;
        bool isOpen = false;
        public ConvertToPending(UsersModel uModel, UnconfirmedPending pendingList = null)
        {
            InitializeComponent();
            um = uModel;
            pList = pendingList;
        }

        private void ConvertToPending_Load(object sender, EventArgs e)
        {
            SetColumnHeaderStyleSetting();
        }

        #region Method
        public void CalendarOpenAlarm(bool isCounting = true)
        {
            SetColumnHeaderStyleSetting();
            int cnt = GetData();
            if (isCounting && cnt > 0)
                this.Show();
            else if (!isCounting)
                this.Show();
        }

        private void SetColumnHeaderStyleSetting()
        {
            dgvUnpending.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUnpending.ColumnHeadersHeight = 36;

            dgvUnpending.Columns["chk"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            dgvUnpending.Columns["ato_no"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            dgvUnpending.Columns["cc_status"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);
            dgvUnpending.Columns["pending_date"].HeaderCell.Style.BackColor = Color.FromArgb(221, 235, 247);

            dgvUnpending.Columns["seaover_indate"].HeaderCell.Style.BackColor = Color.FromArgb(226, 239, 218);
            dgvUnpending.Columns["seaover_cc_status"].HeaderCell.Style.BackColor = Color.FromArgb(226, 239, 218);

            dgvUnpending.Columns["update_status"].Visible = false;
            dgvUnpending.Columns["id"].Visible = false;
        }

        private int GetData()
        {
            //Call procedure 
            CallStockProcedure();
            //Get Data
            DataTable pendingDt = customsRepository.GetProblemPending();
            DataTable atoNoDt = stockRepository.GetPendingStock();
            int cnt = 0;
            if (pendingDt.Rows.Count > 0 && atoNoDt.Rows.Count > 0)
            {
                for (int i = 0; i < pendingDt.Rows.Count; i++)
                {
                    string indate = "";
                    string cc_status = "";
                    string[] tmp = pendingDt.Rows[i]["ato_no"].ToString().Split(' ');
                    string contract_year = pendingDt.Rows[i]["contract_year"].ToString();
                    string whr = $"AtoNo = '{tmp[0].Trim()}' AND 매입처 LIKE '%{contract_year.Substring(2, 2) + "-"}%'";
                    DataRow[] dr = atoNoDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        indate = Convert.ToDateTime(dr[0]["입고일자"].ToString()).ToString("yyyy-MM-dd");
                        cc_status = dr[0]["통관"].ToString().Trim();
                    }

                    int n = dgvUnpending.Rows.Add();
                    dgvUnpending.Rows[n].Cells["id"].Value = pendingDt.Rows[i]["id"].ToString();
                    dgvUnpending.Rows[n].Cells["ato_no"].Value = tmp[0].Trim();
                    dgvUnpending.Rows[n].Cells["cc_status"].Value = pendingDt.Rows[i]["cc_status"].ToString();
                    dgvUnpending.Rows[n].Cells["pending_date"].Value = pendingDt.Rows[i]["pending_date"].ToString();
                    dgvUnpending.Rows[n].Cells["seaover_indate"].Value = indate;
                    dgvUnpending.Rows[n].Cells["seaover_cc_status"].Value = cc_status;

                    if (pendingDt.Rows[i]["pending_date"].ToString() != "" && Convert.ToDateTime(pendingDt.Rows[i]["pending_date"].ToString()) < DateTime.Now.AddMonths(-3)
                        && pendingDt.Rows[i]["cc_status"].ToString() != cc_status)
                    {
                        dgvUnpending.Rows[n].Cells["update_status"].Value = -1;
                        dgvUnpending.Rows[n].Cells["chk"].Value = true;
                        cnt++;
                    }
                    else if (cc_status == "통관")
                    {
                        dgvUnpending.Rows[n].Cells["update_status"].Value = -1;
                        dgvUnpending.Rows[n].Cells["chk"].Value = true;
                        cnt++;
                    }
                    else
                        dgvUnpending.Rows[n].Cells["update_status"].Value = 0;
                }
                //정렬
                dgvUnpending.Sort(dgvUnpending.Columns["update_status"], ListSortDirection.Ascending);
            }
            return cnt;
        }

        private void CallStockProcedure(bool isDash = false)
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (stockRepository.CallStoredProcedureSTOCK(user_id, eDate, isDash) == 0)
                {
                    messageBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch (Exception e)
            {
                messageBox.Show(this,e.Message);
                this.Activate();
                return;
            }
        }
        #endregion

        #region Datagridview 멀티헤더
        private void dgvUnpending_Paint(object sender, PaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            string[] strHeaders = { "ATO", "SEAOVER"};
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            // Ato
            {
                Rectangle r1 = gv.GetCellDisplayRectangle(0, -1, false);
                int width1 = gv.GetCellDisplayRectangle(1, -1, false).Width;
                int width2 = gv.GetCellDisplayRectangle(2, -1, false).Width;
                int width3 = gv.GetCellDisplayRectangle(3, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 + width2 + width3 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(221, 235, 247);
                e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                e.Graphics.DrawString(strHeaders[0], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            // SEAOVER
            {
                Color col = Color.FromArgb(226, 239, 218);
                Rectangle r1 = gv.GetCellDisplayRectangle(4, -1, false);
                int width1 = gv.GetCellDisplayRectangle(5, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(221, 235, 247);
                e.Graphics.FillRectangle(new SolidBrush(col), r1);
                e.Graphics.DrawString(strHeaders[1], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
        }
        private void dgvUnpending_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r = e.CellBounds;
                r.Y += e.CellBounds.Height / 2;
                r.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r, true);
                e.PaintContent(r);
                e.Handled = true;
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvUnpending.Columns[e.ColumnIndex].Name == "chk")
                {
                    bool isChecked = Convert.ToBoolean(dgvUnpending.Rows[e.RowIndex].Cells["chk"].Value);
                    if (isChecked)
                        dgvUnpending.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.Red;
                    else
                        dgvUnpending.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.LightGray;
                }
                else if (dgvUnpending.Columns[e.ColumnIndex].Name == "cc_status")
                {
                    DateTime pending_date;
                    if (dgvUnpending.Rows[e.RowIndex].Cells["pending_date"].Value.ToString() != null
                        && DateTime.TryParse(dgvUnpending.Rows[e.RowIndex].Cells["pending_date"].Value.ToString(), out pending_date))
                    {
                        if (pending_date < DateTime.Now.AddMonths(-3)
                            && dgvUnpending.Rows[e.RowIndex].Cells["cc_status"].Value.ToString() != dgvUnpending.Rows[e.RowIndex].Cells["seaover_cc_status"].Value.ToString())
                            dgvUnpending.Rows[e.RowIndex].Cells["cc_status"].Style.ForeColor = Color.Red;
                        else if (dgvUnpending.Rows[e.RowIndex].Cells["seaover_cc_status"].Value.ToString() == "통관")
                            dgvUnpending.Rows[e.RowIndex].Cells["cc_status"].Style.ForeColor = Color.Red;
                    }
                    else if (dgvUnpending.Rows[e.RowIndex].Cells["seaover_cc_status"].Value.ToString() == "통관")
                        dgvUnpending.Rows[e.RowIndex].Cells["cc_status"].Style.ForeColor = Color.Red;
                }
            }
        }

        private void dgvUnpending_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void dgvUnpending_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }


        #endregion

        #region Button
        private void btnSelect_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            //유효성 검사
            if (dgvUnpending.Rows.Count == 0)
            {
                messageBox.Show(this, "수정할 내역이 없습니다.");
                this.Activate();
                return;
            }
            int cnt = 0;
            for (int i = 0; i < dgvUnpending.Rows.Count; i++)
            {
                bool isChecked = Convert.ToBoolean(dgvUnpending.Rows[i].Cells["chk"].Value);
                if (isChecked)
                    cnt++;
            }
            if(cnt == 0)
            {
                messageBox.Show(this, "수정할 내역을 선택해주세요.");
                this.Activate();
                return;
            }
            //MSG
            if (messageBox.Show(this, "선택하신 내역을 통관처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = new StringBuilder();

                for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvUnpending.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        int id = Convert.ToInt32(dgvUnpending.Rows[i].Cells["id"].Value.ToString());
                        sql = customsRepository.UpdateStatus( id
                                                            , "통관"
                                                            , dgvUnpending.Rows[i].Cells["seaover_indate"].Value.ToString());

                        sqlList.Add(sql);
                    }
                }
                //Execute
                if (sqlList.Count > 0)
                {
                    if (customsRepository.UpdateCustomTran(sqlList) == -1)
                    {
                        MessageBox.Show(this, "수정중 에러가 발생했습니다.");
                        this.Activate();
                    }
                    else if(pList != null)
                    {
                        this.Dispose();
                        pList.GetCustomInfo();
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        private void ConvertToPending_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
    }
}
