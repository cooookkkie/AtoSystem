using AdoNetWindow.Model;
using Repositories;
using Repositories.SalesPartner;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement
{
    public partial class TodaySalesDashboard : Form
    {
        IUsersRepository usersRepository = new UsersRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ISalesPartnerRepository salesPartnerRepository = new SalesPartnerRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        string[] strHeaders = new string[1];
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public TodaySalesDashboard(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
            dgvSales.Scroll += (s, e) => dgvSales.Invalidate();
        }

        private void TodaySalesDashboard_Load(object sender, EventArgs e)
        {
            //부서
            DataTable departmentDt = commonRepository.SelectAsOne("t_department", "name", "", "");
            for (int i = 0; i < departmentDt.Rows.Count; i++)
            {
                cbDepartment.Items.Add(departmentDt.Rows[i]["name"].ToString());
            }
            //년도
            for(int i = 2016; i <= DateTime.Now.Year; i++)
                cbYear.Items.Add(i.ToString());
            cbYear.Text = DateTime.Now.Year.ToString();
            strHeaders[0] = "기준정보";
        }

        #region Method
        public void GetData()
        {
            dgvSales.Rows.Clear();
            if (dgvSales.Columns.Count > 6)
            {
                for (int i = dgvSales.Columns.Count - 1; i >= 6; i--)
                    dgvSales.Columns.Remove(dgvSales.Columns[i]);
            }

            //주차 출력
            int year;
            if (!int.TryParse(cbYear.Text, out year))
            {
                messageBox.Show(this,"기준녀도를 선택해주세요.");
                this.Activate();
                return;
            }

            DateTime sttdate = new DateTime(year, 1, 1);
            DateTime enddate = sttdate;
            int week = 0;
            int month = 1;
            sttdate = common.GetFirstDateOfWeek(year, week);
            enddate = sttdate.AddDays(4);
            //출력
            int n;
            DataGridViewRow row;
            while (sttdate.Year <= year)
            {
                //합계
                if (enddate.Year > year)
                    enddate = new DateTime(year, 12, 31);
                if (month < sttdate.Month)
                {
                    n = dgvSales.Rows.Add();
                    row = dgvSales.Rows[n];
                    row.Cells["sYear"].Value = "평균";
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    n = dgvSales.Rows.Add();
                    row = dgvSales.Rows[n];
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                    row.Cells["sYear"].Value = "합계";

                    month = sttdate.Month;
                }

                n = dgvSales.Rows.Add();
                row = dgvSales.Rows[n];
                row.Cells["sYear"].Value = year.ToString();
                row.Cells["sMonth"].Value = sttdate.Month.ToString();
                row.Cells["sWeek"].Value = (week + 1).ToString();

                row.Cells["sdate"].Value = sttdate.ToString("yyyy-MM-dd");
                row.Cells["edate"].Value = enddate.ToString("yyyy-MM-dd");

                int workDays;
                common.GetWorkDay(sttdate, enddate, out workDays);
                row.Cells["work_days"].Value = workDays.ToString("#,##0");

                week++;
                sttdate = common.GetFirstDateOfWeek(year, week);
                enddate = sttdate.AddDays(4);
            }
            if (dgvSales.Rows.Count > 0)
            {
                n = dgvSales.Rows.Add();
                row = dgvSales.Rows[n];
                row.Cells["sYear"].Value = "평균";
                row.DefaultCellStyle.BackColor = Color.LightGray;
                n = dgvSales.Rows.Add();
                row = dgvSales.Rows[n];
                row.DefaultCellStyle.BackColor = Color.LightBlue;
                row.Cells["sYear"].Value = "합계";
            }

            //영업부 출력
            DataTable userDt = usersRepository.GetUsers("", cbDepartment.Text, "", txtManager.Text, "승인", "");
            //strHeaders = new string[userDt.Rows.Count + 1];

            if (userDt.Rows.Count > 0)
            {
                //Header list
                List<string> userList = new List<string>();
                for (int i = 0; i < userDt.Rows.Count; i++)
                {
                    if (!userList.Contains(userDt.Rows[i]["user_name"].ToString()))
                    {
                        userList.Add(userDt.Rows[i]["user_name"].ToString());
                        dgvSales.Columns.Add(userDt.Rows[i]["user_name"].ToString() + "_AM", "오전");
                        dgvSales.Columns[userDt.Rows[i]["user_name"].ToString() + "_AM"].Width = 40;
                        dgvSales.Columns.Add(userDt.Rows[i]["user_name"].ToString() + "_PM", "오후");
                        dgvSales.Columns[userDt.Rows[i]["user_name"].ToString() + "_PM"].Width = 40;
                        dgvSales.Columns.Add(userDt.Rows[i]["user_name"].ToString() + "_TOTAL", "합계");
                        dgvSales.Columns[userDt.Rows[i]["user_name"].ToString() + "_TOTAL"].Width = 40;
                        dgvSales.Columns.Add(userDt.Rows[i]["user_name"].ToString() + "_POTENTIAL1", "잠재1");
                        dgvSales.Columns[userDt.Rows[i]["user_name"].ToString() + "_POTENTIAL1"].Width = 40;
                        dgvSales.Columns[userDt.Rows[i]["user_name"].ToString() + "_POTENTIAL1"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
                        dgvSales.Columns.Add(userDt.Rows[i]["user_name"].ToString() + "_POTENTIAL2", "잠재2");
                        dgvSales.Columns[userDt.Rows[i]["user_name"].ToString() + "_POTENTIAL2"].Width = 40;
                        dgvSales.Columns[userDt.Rows[i]["user_name"].ToString() + "_POTENTIAL2"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
                    }
                }
                strHeaders = new string[userList.Count + 2];
                strHeaders[0] = "기준정보";
                for (int i = 0; i < userList.Count; i++)
                    strHeaders[i + 1] = userList[i];
                //데이터 출력
                DataTable saleDt = salesPartnerRepository.GetSalesSumary(year, cbDepartment.Text, txtManager.Text);
                if (saleDt.Rows.Count > 0)
                {
                    for (int i = 0; i < saleDt.Rows.Count; i++)
                    {
                        DateTime sdate;
                        if (DateTime.TryParse(saleDt.Rows[i]["updatetime"].ToString(), out sdate))
                        {
                            for (int j = 0; j < dgvSales.Rows.Count; j++)
                            {
                                row = dgvSales.Rows[j];
                                if (row.Cells["sdate"].Value != null && DateTime.TryParse(row.Cells["sdate"].Value.ToString(), out sttdate)
                                    && row.Cells["edate"].Value != null && DateTime.TryParse(row.Cells["edate"].Value.ToString(), out enddate))
                                {
                                    if (sdate >= sttdate && sdate <= enddate)
                                    {
                                        int col_idx = GetColumnIndex(saleDt.Rows[i]["edit_user"].ToString() + "_AM");
                                        if (col_idx > 0)
                                        {
                                            int am, pm, pt1, pt2;
                                            if (row.Cells[col_idx].Value == null || !int.TryParse(row.Cells[col_idx].Value.ToString(), out am))
                                                am = 0;
                                            am += Convert.ToInt32(saleDt.Rows[i]["am_cnt"].ToString());
                                            row.Cells[col_idx].Value = am.ToString("#,##0");
                                            if (row.Cells[col_idx + 1].Value == null || !int.TryParse(row.Cells[col_idx + 1].Value.ToString(), out pm))
                                                pm = 0;
                                            pm += Convert.ToInt32(saleDt.Rows[i]["pm_cnt"].ToString());
                                            row.Cells[col_idx + 2].Value = (am + pm).ToString("#,##0");
                                            row.Cells[col_idx + 1].Value = pm.ToString("#,##0");
                                            if (row.Cells[col_idx + 3].Value == null || !int.TryParse(row.Cells[col_idx + 3].Value.ToString(), out pt1))
                                                pt1 = 0;
                                            pt1 += Convert.ToInt32(saleDt.Rows[i]["potential1_cnt"].ToString());
                                            row.Cells[col_idx + 3].Value = pt1.ToString("#,##0");
                                            if (row.Cells[col_idx + 4].Value == null || !int.TryParse(row.Cells[col_idx + 4].Value.ToString(), out pt2))
                                                pt2 = 0;
                                            pt2 += Convert.ToInt32(saleDt.Rows[i]["potential2_cnt"].ToString());
                                            row.Cells[col_idx + 4].Value = pt2.ToString("#,##0");

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //평균 합계
                    for (int j = 6; j < dgvSales.ColumnCount; j += 5)
                    {
                        int wk = 0, am = 0, pm = 0, pt1 = 0, pt2 = 0;
                        for (int i = 0; i < dgvSales.Rows.Count; i++)
                        {
                            if (dgvSales.Rows[i].Cells["sYear"].Value.ToString() == "평균")
                            {
                                dgvSales.Rows[i].Cells[j].Value = (am / wk).ToString("#,##0");
                                dgvSales.Rows[i].Cells[j + 1].Value = (pm / wk).ToString("#,##0");
                                dgvSales.Rows[i].Cells[j + 2].Value = ((am + pm) / wk).ToString("#,##0");
                                dgvSales.Rows[i].Cells[j + 3].Value = (pt1 / wk).ToString("#,##0");
                                dgvSales.Rows[i].Cells[j + 4].Value = (pt2 / wk).ToString("#,##0");
                            }
                            else if (dgvSales.Rows[i].Cells["sYear"].Value.ToString() == "합계")
                            {
                                dgvSales.Rows[i].Cells[j].Value = (am).ToString("#,##0");
                                dgvSales.Rows[i].Cells[j + 1].Value = (pm).ToString("#,##0");
                                dgvSales.Rows[i].Cells[j + 2].Value = (am + pm).ToString("#,##0");
                                dgvSales.Rows[i].Cells[j + 3].Value = (pt1).ToString("#,##0");
                                dgvSales.Rows[i].Cells[j + 4].Value = (pt2).ToString("#,##0");

                                wk = 0; am = 0; pm = 0; pt1 = 0; pt2 = 0;
                            }
                            else
                            {
                                wk++;
                                int tmp;
                                if (dgvSales.Rows[i].Cells[j].Value == null || !int.TryParse(dgvSales.Rows[i].Cells[j].Value.ToString(), out tmp))
                                    tmp = 0;
                                am += tmp;
                                if (dgvSales.Rows[i].Cells[j + 1].Value == null || !int.TryParse(dgvSales.Rows[i].Cells[j + 1].Value.ToString(), out tmp))
                                    tmp = 0;
                                pm += tmp;
                                if (dgvSales.Rows[i].Cells[j + 3].Value == null || !int.TryParse(dgvSales.Rows[i].Cells[j + 3].Value.ToString(), out tmp))
                                    tmp = 0;
                                pt1 += tmp;
                                if (dgvSales.Rows[i].Cells[j + 4].Value == null || !int.TryParse(dgvSales.Rows[i].Cells[j + 4].Value.ToString(), out tmp))
                                    tmp = 0;
                                pt2 += tmp;
                            }
                        }
                    }

                }
            }
            //오늘날짜 찾아가기
            if (dgvSales.Rows.Count > 0)
            {
                for (int i = 0; i < dgvSales.Rows.Count; i++)
                {
                    DateTime sdate, edate;
                    if (dgvSales.Rows[i].Cells["sdate"].Value != null && DateTime.TryParse(dgvSales.Rows[i].Cells["sdate"].Value.ToString(), out sdate) 
                        && dgvSales.Rows[i].Cells["edate"].Value != null && DateTime.TryParse(dgvSales.Rows[i].Cells["edate"].Value.ToString(), out edate))
                    {
                        DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        if (sdate <= currentDate && currentDate <= edate)
                        {
                            dgvSales.ClearSelection();
                            dgvSales.Rows[i].Selected = true;
                            int firstIndex = i - 10;
                            if (firstIndex < 0)
                                firstIndex = 0;
                            dgvSales.FirstDisplayedScrollingRowIndex = firstIndex;
                            break;
                        }
                    }
                }
            }
            //틀고정
            if (dgvSales.Columns.Count > 5)
            {
                dgvSales.Columns[0].Frozen = true;
                dgvSales.Columns[1].Frozen = true;
                dgvSales.Columns[2].Frozen = true;
                dgvSales.Columns[3].Frozen = true;
                dgvSales.Columns[4].Frozen = true;
                dgvSales.Columns[5].Frozen = true;
            }

        }

        private DateTime GetFirstDateOfWeek(int year, int week)
        { 
            DateTime firstDateOfYear = new DateTime(year, 1, 1);
            DateTime fisrtDateOfFirstWeek = firstDateOfYear.AddDays(7- (int)(firstDateOfYear.DayOfWeek) + 1);

            return fisrtDateOfFirstWeek.AddDays(7 * (week - 1));
        }


        private int GetColumnIndex(string col_name)
        {
            int idx = 0;

            for (int i = 0; i < dgvSales.ColumnCount; i++)
            {
                if (dgvSales.Columns[i].Name == col_name)
                {
                    idx = i;
                    break;
                }
            }
            return idx;
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region Datagridview 멀티헤더
        private void dgvSales_Paint(object sender, PaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            // 기준정보
            {
                Rectangle r1 = gv.GetCellDisplayRectangle(0, -1, false);
                int width1 = gv.GetCellDisplayRectangle(1, -1, false).Width;
                int width2 = gv.GetCellDisplayRectangle(2, -1, false).Width;
                int width3 = gv.GetCellDisplayRectangle(3, -1, false).Width;
                int width4 = gv.GetCellDisplayRectangle(4, -1, false).Width;
                int width5 = gv.GetCellDisplayRectangle(5, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 + width2 + width3 + width4 + width5 - 2;
                //r1.Width = r1.Width + width1 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(221, 235, 247);
                e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                e.Graphics.DrawString(strHeaders[0], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            if (strHeaders.Length > 1)
            {
                int col_inx = 5;
                for (int i = 1; i < strHeaders.Length; i++)
                {
                    Color col = Color.FromArgb(226, 239, 218);
                    col_inx += 1;
                    Rectangle r1 = gv.GetCellDisplayRectangle(col_inx, -1, false);
                    col_inx += 1;
                    int width1 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width2 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width3 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width4 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + width1 + width2 + width3 + width4 - 2;
                    r1.Height = (r1.Height / 2) - 2;
                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                    Color colBlue = Color.FromArgb(221, 235, 247);
                    e.Graphics.FillRectangle(new SolidBrush(col), r1);
                    e.Graphics.DrawString(strHeaders[i], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                }
            }
        }
        private void dgvSales_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
        }

        private void dgvSales_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);

            
        }

        private void dgvSales_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }
        #endregion

        #region Key event
        private void TodaySalesDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.N:
                        txtManager.Text = String.Empty;
                        txtManager.Focus();
                        break;
                    case Keys.M:
                        txtManager.Focus();
                        break;
                }
            }
        }
        private void txtManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearching.PerformClick();
        }
        #endregion
    }
}

