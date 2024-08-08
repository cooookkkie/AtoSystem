using AdoNetWindow.Model;
using Repositories;
using Repositories.Calender;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.CalendarModule.VacationManager
{
    public partial class VacationDashboard : Form
    {
        IDepartmentRepository departmentRepository = new DepartmentRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IUsersRepository usersRepository = new UsersRepository();
        IAnnualRepository annualRepository = new AnnualRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um;
        public VacationDashboard(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
            txtUsername.Text = um.user_name;

            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                //유저별 권한가져오기
                DataRow[] dr = authorityDt.Select("id = 1");
                if (dr.Length > 0)
                    authorityDt = dr.CopyToDataTable();
                //없으면 부서별 권한가져오기
                else
                {
                    dr = authorityDt.Select("id = 2");
                    authorityDt = dr.CopyToDataTable();
                }
                authorityDt.AcceptChanges();

                //사용가능한 메뉴 설정
                foreach (DataRow ddr in authorityDt.Rows)
                {
                    if ("설정" == ddr["group_name"].ToString() && "연차관리" == ddr["form_name"].ToString())
                    {
                        if (bool.TryParse(ddr["is_admin"].ToString(), out bool is_admin) && !is_admin)
                        {
                            txtUsername.Enabled = false;
                            isSave = false;
                        }
                        break;
                    }
                }
            }
        }
        private void VacationDashboard_Load(object sender, EventArgs e)
        {
            for (int i = 2016; i <= DateTime.Now.Year + 2; i++)
                cbYear.Items.Add(i.ToString());

            cbYear.Text = DateTime.Now.Year.ToString();

            dgvVacation.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 112, 192);
            dgvVacation.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvVacation.Columns["accrued_annual"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvVacation.Columns["accrued_annual"].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);

            dgvVacation.Columns["total_annual"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvVacation.Columns["total_annual"].DefaultCellStyle.Font = new Font("굴림", 9 , FontStyle.Bold);

            dgvVacation.Columns["leave_annual"].DefaultCellStyle.ForeColor = Color.Red;
            dgvVacation.Columns["leave_annual"].DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);

            //부서
            DataTable departmentDt = departmentRepository.GetDepartment("", "");
            cbDepartment.Items.Clear();
            cbDepartment.Items.Add("전체");
            for (int i = 0; i < departmentDt.Rows.Count; i++)
                cbDepartment.Items.Add(departmentDt.Rows[i]["name"].ToString());

            btnSearch.PerformClick();
        }

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
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 + width2 - 2;
                //r1.Width = r1.Width + width1 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(0, 112, 192);
                e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                e.Graphics.DrawString("기본정보", gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            
            //1월 ~ 12월
            {
                int col_idx = 4;
                for (int i = 1; i <= 12; i++)
                {
                    Rectangle r1 = gv.GetCellDisplayRectangle(col_idx, -1, false);
                    col_idx++;
                    int width1 = gv.GetCellDisplayRectangle(col_idx, -1, false).Width;
                    col_idx++;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + width1 - 2;
                    //r1.Width = r1.Width + width1 - 2;
                    r1.Height = (r1.Height / 2) - 2;
                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                    Color colBlue = Color.FromArgb(0, 112, 192);
                    e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                    e.Graphics.DrawString(i + "월", gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(Color.DarkOrange), r1, format);
                }
            }
            //총합, 잔여
            {
                Rectangle r1 = gv.GetCellDisplayRectangle(28, -1, false);
                int width1 = gv.GetCellDisplayRectangle(29, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 - 2;
                //r1.Width = r1.Width + width1 - 2;
                r1.Height = (r1.Height / 2) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(0, 112, 192);
                e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                e.Graphics.DrawString("사용현황", gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
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

            /*//merge
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
            {
                e.AdvancedBorderStyle.Top = dgvVacation.AdvancedCellBorderStyle.Top;
                return;
            }
            else if (e.RowIndex >= 0 && dgvVacation.Rows[e.RowIndex].Cells[0].Value != dgvVacation.Rows[e.RowIndex - 1].Cells[0].Value)
            {
                e.AdvancedBorderStyle.Top = dgvVacation.AdvancedCellBorderStyle.Top;
                return;
            }*/


        }
        // 그리드 위쪽 선을 그릴지?
        // 현재 칸이 null이거나 위쪽 셀과 같으면 그리지 않는다.
        private bool IsDrawTopBorder(int column, int row)
        {
            // 첫번째 row는 무조건 그린다.
            if (row == 0)
                return true;

            // 두번째 column 까지만 체크(나머진 모두 그린다)
            if (column >= 2)
                return true;

            // 사용자 추가 허용인 경우 마지막줄은 추가용 빈킨다.
            if (dgvVacation.AllowUserToAddRows)
            {
                if (row == dgvVacation.Rows.Count - 1)
                    return true;
            }

            // 현재 column만 비교하지 않고 이전 column까지 비교해서
            // 모두 같으면 안그린다.

            // 상위 한가지라도 다르면 그린다.
            for (int col = 0; col <= column; ++col)
            {
                // 현재셀
                DataGridViewCell cell1 = dgvVacation[col, row];

                // 위쪽셀
                DataGridViewCell cell2 = dgvVacation[col, row - 1];
                if (cell1.Value == null || cell2.Value == null)
                {
                    return false;
                }

                string str1 = cell1.Value.ToString();
                string str2 = cell2.Value.ToString();

                // 현재셀이 null ("") 이면 이전셀과 같은 것으로 판단
                if (str1 == null || str1 == "")
                    continue;

                // 이전셀과 비교해서 다르면 찍어야함
                if (str1 != str2)
                    return true;

            }

            return false;

        }
        private void dgvVacation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 4)
                return;
            if (!IsDrawTopBorder(e.ColumnIndex, e.RowIndex))
            {
                e.Value = "";
                e.FormattingApplied = true;
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
        private void VacationDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtUsername.Focus();
                        break;
                    case Keys.N:
                        txtUsername.Text = String.Empty;
                        txtUsername.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        btnPreview.PerformClick();
                        break;
                    case Keys.E:
                        //btnExcel.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }
        bool isSave = true;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            isSave = false;
            int year;
            if (!int.TryParse(cbYear.Text, out year))
            {
                MessageBox.Show(this,"년도를 선택해주세요!");
                return;
            }

            //초기화
            this.dgvVacation.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVacation_CellValueChanged);
            dgvVacation.Rows.Clear();   
            DataTable userDt = usersRepository.GetUsers(year, cbWorkplace.Text, cbDepartment.Text, txtUsername.Text, cbSortType.Text, "", cbRetire.Checked);
            if (userDt.Rows.Count > 0)
            {
                DateTime sttdate = new DateTime(year, 1, 1);
                DateTime enddate = new DateTime(year, 12, 31);
                DataTable annualDt = annualRepository.GetAnnual("", txtUsername.Text, sttdate, enddate);

                
                for (int i = 0; i < userDt.Rows.Count; i++)
                {
                    bool isOutput = true;


                    DateTime user_out_date;
                    //퇴사자일 경우
                    if (userDt.Rows[i]["user_status"].ToString() == "퇴사")
                    {
                        if (DateTime.TryParse(userDt.Rows[i]["user_out_date"].ToString(), out user_out_date))
                        { 
                            if(user_out_date.Year < year)
                                isOutput = false;
                        }
                        else
                            isOutput = false;
                    }

                    if (isOutput)
                    {
                        Color col;
                        if (i % 2 == 0)
                            col = Color.FromArgb(252, 228, 214);
                        else
                            col = Color.FromArgb(226, 239, 218);


                        int n = dgvVacation.Rows.Add();
                        int rowIndex = n;
                        dgvVacation.Rows[n].Cells["user_id"].Value = userDt.Rows[i]["user_id"].ToString();
                        dgvVacation.Rows[n].Cells["user_name"].Value = userDt.Rows[i]["user_name"].ToString();

                        DateTime user_in_date;
                        if (DateTime.TryParse(userDt.Rows[i]["user_in_date"].ToString(), out user_in_date))
                            dgvVacation.Rows[n].Cells["in_date"].Value = user_in_date.ToString("yyyy-MM-dd");

                        double accrued_annual;
                        if (!double.TryParse(userDt.Rows[i]["vacation"].ToString(), out accrued_annual))
                            accrued_annual = 0;
                        //등록값이 없으면 계산
                        if (accrued_annual == 0)
                        {
                            TimeSpan ts = new DateTime(year - 1, 12, 31) - user_in_date;
                            int workDays = ts.Days + 1;
                            if (user_in_date.Year == year && workDays / 365 < 1)
                            {
                                while (DateTime.Now >= user_in_date)
                                {
                                    accrued_annual++;
                                    user_in_date = user_in_date.AddMonths(1);
                                }
                            }
                            else if (user_in_date.Year < year && workDays / 365 < 1)
                                accrued_annual = 15;
                            else
                                accrued_annual = Math.Round((double)(int)((double)workDays / 365) / 2, 0, MidpointRounding.AwayFromZero) + 14;
                        }

                        dgvVacation.Rows[n].Cells["accrued_annual"].Value = accrued_annual;
                        dgvVacation.Rows[n].Cells["accrued_annual"].Style.BackColor = Color.LightGray;
                        dgvVacation.Rows[n].DefaultCellStyle.BackColor = col;

                        n = dgvVacation.Rows.Add();
                        dgvVacation.Rows[n].Cells["user_id"].Value = userDt.Rows[i]["user_id"].ToString();
                        dgvVacation.Rows[n].Cells["user_name"].Value = userDt.Rows[i]["user_name"].ToString();
                        dgvVacation.Rows[n].DefaultCellStyle.BackColor = col;

                        if (DateTime.TryParse(userDt.Rows[i]["user_out_date"].ToString(), out user_out_date) && user_out_date.Year >= 2000)
                            dgvVacation.Rows[n].Cells["in_date"].Value = user_out_date.ToString("yyyy-MM-dd");

                        n = dgvVacation.Rows.Add();
                        dgvVacation.Rows[n].Cells["user_id"].Value = userDt.Rows[i]["user_id"].ToString();
                        dgvVacation.Rows[n].Cells["user_name"].Value = userDt.Rows[i]["user_name"].ToString();
                        dgvVacation.Rows[n].DefaultCellStyle.BackColor = col;

                        n = dgvVacation.Rows.Add();
                        dgvVacation.Rows[n].Cells["user_id"].Value = userDt.Rows[i]["user_id"].ToString();
                        dgvVacation.Rows[n].Cells["user_name"].Value = userDt.Rows[i]["user_name"].ToString();
                        dgvVacation.Rows[n].DefaultCellStyle.BackColor = col;

                        n = dgvVacation.Rows.Add();
                        dgvVacation.Rows[n].Cells["user_id"].Value = userDt.Rows[i]["user_id"].ToString();
                        dgvVacation.Rows[n].Cells["user_name"].Value = userDt.Rows[i]["user_name"].ToString();
                        dgvVacation.Rows[n].DefaultCellStyle.BackColor = col;

                        n = dgvVacation.Rows.Add();
                        dgvVacation.Rows[n].Cells["user_id"].Value = userDt.Rows[i]["user_id"].ToString();
                        dgvVacation.Rows[n].Cells["user_name"].Value = userDt.Rows[i]["user_name"].ToString();
                        dgvVacation.Rows[n].Cells["accrued_annual"].Value = "합계";
                        dgvVacation.Rows[n].DefaultCellStyle.BackColor = Color.LightGray;
                        dgvVacation.Rows[n].DefaultCellStyle.Font = new Font("굴림", 10, FontStyle.Bold);
                        dgvVacation.Rows[n].Cells["total_annual"].Style.BackColor = Color.LightGray;
                        dgvVacation.Rows[n].Cells["leave_annual"].Style.BackColor = Color.LightGray;


                        //사용연차 
                        if (annualDt.Rows.Count > 0)
                        {
                            DataRow[] annualDr = annualDt.Select($"user_id = '{userDt.Rows[i]["user_id"].ToString()}'");
                            if (annualDr.Length > 0)
                            {
                                for (int j = 0; j < annualDr.Length; j++)
                                {
                                    DateTime usedDate;
                                    double usedDays;
                                    if (DateTime.TryParse(annualDr[j]["sttdate"].ToString(), out usedDate) && double.TryParse(annualDr[j]["used_days"].ToString(), out usedDays))
                                    {
                                        int colIndex = ((usedDate.Month - 1) * 2) + 4;
                                        for (int k = rowIndex; k <= rowIndex + 4; k++)
                                        {
                                            if (dgvVacation.Rows[k].Cells[colIndex].Value == null || string.IsNullOrEmpty(dgvVacation.Rows[k].Cells[colIndex].Value.ToString()))
                                            {
                                                dgvVacation.Rows[k].Cells[colIndex].Value = usedDate.ToString("MM-dd");
                                                dgvVacation.Rows[k].Cells[colIndex + 1].Value = Math.Truncate(usedDays * 10) / 10;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //사용연차 계산
                        calculateVacation(n);
                    }
                }
            }
            this.dgvVacation.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVacation_CellValueChanged);

            if ((um.department.Contains("경리") || um.department.Contains("전산") || um.department.Contains("관리부")))
                isSave = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region 데이터 저장
        private void AccruedAnnual(int rowindex)
        {
            if (!isSave)
                return;

            int year;
            if (!int.TryParse(cbYear.Text, out year))
            {
                MessageBox.Show(this,"년도를 선택해주세요.");
                return;
            }

            string user_id = dgvVacation.Rows[rowindex].Cells["user_id"].Value.ToString();
            if (string.IsNullOrEmpty(user_id))
            {
                MessageBox.Show(this,"사용자 정보를 찾을수 없습니다.");
                return;
            }

            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = annualRepository.DeleteAccruedAnnual(user_id, year);
            sqlList.Add(sql);

            AcruedAnnualModel model = new AcruedAnnualModel();
            model.user_id = user_id;
            model.year = year;

            double vacation;
            if (dgvVacation.Rows[rowindex].Cells["accrued_annual"].Value == null || !double.TryParse(dgvVacation.Rows[rowindex].Cells["accrued_annual"].Value.ToString(), out vacation))
                vacation = 0;
            model.vacation = vacation;
            model.edit_user = um.user_name;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            sql = annualRepository.InsertAccruedAnnual(model);
            sqlList.Add(sql);

            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                    MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
            }
        }


        private void dgvVacation_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvVacation.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVacation_CellValueChanged);
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                {
                    AccruedAnnual(e.RowIndex);
                    calculateVacation(e.RowIndex);
                }
                else if (e.ColumnIndex >= 4 && e.ColumnIndex <= 27 && dgvVacation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    string cellValue = dgvVacation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    //날짜
                    if (e.ColumnIndex % 2 == 0)
                    {
                        if (cellValue.Length <= 2)
                        {
                            int d;
                            if(int.TryParse(cellValue, out d) && d > 0)
                            {
                                int year;
                                if(!int.TryParse(cbYear.Text, out year))
                                    year = DateTime.Now.Year;
                                int month = (int)Math.Ceiling((((double)e.ColumnIndex - 2) / 2));

                                DateTime used_date;
                                if (!DateTime.TryParse(year + "-" + month + "-" + d, out used_date))
                                {
                                    MessageBox.Show(this,"날짜 형식이 아닙니다.");
                                    return;
                                }
                                else
                                    dgvVacation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = used_date.ToString("MM-dd");
                            }
                        }
                        else 
                        {
                            if(DateTime.TryParse(cellValue, out DateTime sttDt))
                                dgvVacation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = sttDt.ToString("MM-dd");
                        }
                    }
                    //사용일수
                    else 
                        calculateVacation(e.RowIndex);

                    //데이터 저장
                    InsertVacation(e.RowIndex, e.ColumnIndex);

                }
            }
            this.dgvVacation.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVacation_CellValueChanged);
        }

        private void calculateVacation(int rowIndex)
        {
            //this.dgvVacation.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVacation_CellValueChanged);
            if (rowIndex >= 0)
            {
                rowIndex = (rowIndex / 6) * 6;

                //발생연차
                double accrued_annual;
                if (dgvVacation.Rows[rowIndex].Cells["accrued_annual"].Value == null || !double.TryParse(dgvVacation.Rows[rowIndex].Cells["accrued_annual"].Value.ToString(), out accrued_annual))
                    accrued_annual = 0;
                //사용연차
                double total_used_annual = 0;
                for (int i = 5; i <= 27; i = i + 2)
                {
                    double month_used_annual = 0;
                    for (int j = rowIndex; j <= rowIndex + 4; j++)
                    {
                        double used_annual;
                        if (dgvVacation.Rows[j].Cells[i].Value == null || !double.TryParse(dgvVacation.Rows[j].Cells[i].Value.ToString(), out used_annual))
                            used_annual = 0;

                        month_used_annual += used_annual;
                    }
                    total_used_annual += month_used_annual;
                    dgvVacation.Rows[rowIndex + 5].Cells[i].Value = month_used_annual;
                }

                //총사용, 잔여
                dgvVacation.Rows[rowIndex + 5].Cells["total_annual"].Value = total_used_annual;
                dgvVacation.Rows[rowIndex + 5].Cells["leave_annual"].Value = accrued_annual - total_used_annual;
            }
            //this.dgvVacation.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVacation_CellValueChanged);
        }

        private void InsertVacation(int rowIndex, int colIndex)
        {
            if (rowIndex >= 0 && isSave)
            {
                int year;
                if (!int.TryParse(cbYear.Text, out year))
                    return;

                //그달 연차내역 전체삭제
                //날짜 컬럼으로 옮기기
                if (colIndex % 2 == 1)
                    colIndex--;
                int month = (int)Math.Ceiling((((double)colIndex - 2) / 2));
                DateTime annualMonth = new DateTime(year, month, 1);
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = annualRepository.DeleteAnnual(dgvVacation.Rows[rowIndex].Cells["user_id"].Value.ToString(), annualMonth);
                sqlList.Add(sql);
                rowIndex = (rowIndex / 6) * 6;

                


                //다시 재등록
                int newid = commonRepository.GetNextId("t_used_vacation", "id"); ;
                for (int i = rowIndex; i <= rowIndex + 4; i++)
                {
                    DateTime sttdate;
                    double used_days;
                    if (dgvVacation.Rows[i].Cells[colIndex].Value != null && DateTime.TryParse(dgvVacation.Rows[i].Cells[colIndex].Value.ToString(), out sttdate)
                        && dgvVacation.Rows[i].Cells[colIndex + 1].Value != null && double.TryParse(dgvVacation.Rows[i].Cells[colIndex + 1].Value.ToString(), out used_days))
                    {
                        VacationModel model = new VacationModel();
                        model.id = newid;
                        newid++;
                        model.user_id = dgvVacation.Rows[rowIndex].Cells["user_id"].Value.ToString();
                        model.user_name = dgvVacation.Rows[rowIndex].Cells["user_name"].Value.ToString();
                        sttdate = new DateTime(year, month, sttdate.Day);
                        model.sttdate = sttdate.ToString("yyyy-MM-dd");
                        model.used_days = used_days;
                        model.edit_user = um.user_name;
                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        sql = annualRepository.InsertAnnual(model);
                        sqlList.Add(sql);
                    }
                }

                if (commonRepository.UpdateTran(sqlList) == -1)
                    MessageBox.Show(this,"등록시 에러가 발생하였습니다.");
            }
        }
        #endregion

        #region Print 
        private void btnPreview_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "설정", "연차관리", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvVacation.Rows.Count == 0)
            {
                MessageBox.Show(this, "출력할 내역이 없습니다.");
                this.Activate();
                return;
            }

            cnt = 0;
            pageNo = 1;
            this.printDocument1 = new PrintDocument();
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.DefaultPageSettings.Landscape = true;

            int pages = common.GetPageCount(printDocument1);

            Common.PrintManager.PrintManager pm = new Common.PrintManager.PrintManager(this, printDocument1, pages);
            pm.ShowDialog();
        }

        public void InitVariable()
        {
            cnt = 0;
            pageNo = 1;
        }

        //양식만들기
        int cnt = 0;
        int pageNo = 1;
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridView dgv = this.dgvVacation;

            int dialogWidth = printDocument1.DefaultPageSettings.PaperSize.Height - 40;    //페이지 전체넓이 printPreivew.Width  (가로모드라 반대로)
            int dialogHeight = printDocument1.DefaultPageSettings.PaperSize.Width;    //페이지 전체넓이 printPreivew.Height  (가로모드라 반대로)

            StringFormat sf = new StringFormat();  //컬럼안에 있는 값들 가운데로 정렬하기 위해서.
            sf.Alignment = StringAlignment.Center;
            int width, width1;             //width는 시작점 위치, width1은 datagrid 1개의 컬럼 가로길이
            int startWidth = 10;                   //시작 x좌표
            int startHeight = 120;                 //시작 y좌표
            int avgHeight = dgv.Rows[0].Height - 5;    //컬럼 하나의 높이
            int i, j;                              //반복문용 변수
            int temp = 0;                          //row 개수 세어줄것, cnt의 역활

            //Title, Footer
            e.Graphics.DrawString("연차대장", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, dialogWidth / 2 - 50, 40);
            e.Graphics.DrawString("인쇄일자 :  " + DateTime.Now.ToString("yyyy-MM-dd"), new Font("Arial", 11), Brushes.Black, dialogWidth - 200, 60);
            e.Graphics.DrawString("페이지번호 : " + pageNo, new Font("Arial", 11), Brushes.Black, dialogWidth - 200, 80);

            //column 전체 넓이
            int column_width = 0;
            int column_count = 0;
            for (i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv.Columns[i].Visible == true)
                {
                    column_width += dgv.Columns[i].Width;
                    column_count += 1;
                }
            }
            int adjust_font_size = (int)(column_count / 10);
            //columnCount는 일정
            double width_rate;            //컬럼 넓이비율
            double pre_width_rate;        //이전 컬럼 넓이비율
            int pre_idx = 0;
            int tmp = 0;
            for (i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv.Columns[i].Visible == true)
                {
                    if (tmp == 0)
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = 0;
                        width1 = (int)(((double)dialogWidth / 100) * width_rate);
                    }
                    else if (tmp > 0 && i <= dgv.ColumnCount - 2)
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;

                        //width = dgv.Columns[i - 1].Width + tempWidth;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                        width1 = (int)(((double)dialogWidth / 100) * width_rate);
                    }
                    else
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;
                        //width = dgv.Columns[i - 1].Width + tempWidth;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                        width1 = (int)(((double)dialogWidth / 100) * width_rate);
                    }

                    RectangleF drawRect = new RectangleF((float)(startWidth + width), (float)startHeight, (float)width1, avgHeight + 5);
                    e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width), (float)startHeight - 4, (float)width1, avgHeight + 5);
                    // e.Graphics.FillRectangle(Brushes.LightGray, (float)(startWidth + width), (float)startHeight, (float)width, avgHeight);
                    string headerTxt;
                    if (dgv.Columns[i].HeaderText.Contains("입사일"))
                        headerTxt = "입사/퇴사";
                    else if (dgv.Columns[i].HeaderText.Contains("날짜"))
                        headerTxt = ((i - 4) / 2 + 1) + "월";
                    else
                        headerTxt = dgv.Columns[i].HeaderText;

                    e.Graphics.DrawString(headerTxt, new Font("Arial", 8 - adjust_font_size, FontStyle.Bold), Brushes.Black, drawRect, sf);

                    startWidth += width;
                    tmp += 1;
                    pre_idx = i;
                }
            }

            startHeight += avgHeight + 1;
            for (i = cnt; i < dgv.RowCount; i++)
            {
                tmp = 0;
                pre_idx = 0;
                startWidth = 10;  //다시 초기화
                for (j = 0; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Columns[j].Visible == true && dgv.Rows[i].Cells[j].Visible == true)
                    {
                        if (tmp == 0)
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            //width = 0;
                            //width1 = dgv.Columns[j].Width + tempWidth;
                            width = 0;
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);

                        }
                        else if (tmp > 0 && j <= dgv.ColumnCount - 2)
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;

                            //width = dgv.Columns[j - 1].Width + tempWidth;
                            //width1 = dgv.Columns[j].Width + tempWidth;
                            width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);
                        }
                        else
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;
                            //width = dgv.Columns[i - 1].Width + tempWidth;
                            //width1 = dgv.Columns[i].Width + tempWidth;
                            width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);
                        }
                        RectangleF drawRect = new RectangleF((float)(startWidth + width), (float)startHeight + 2, (float)width1, avgHeight);
                        e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width), (float)startHeight, (float)width1, avgHeight);

                        if(dgv.Columns[j].HeaderText.Contains("입사일") && DateTime.TryParse(dgv.Rows[i].Cells[j].FormattedValue.ToString(), out DateTime dt))
                            e.Graphics.DrawString(dt.ToString("yy/MM/dd"), new Font("Arial", 8 - adjust_font_size, FontStyle.Bold), Brushes.Black, drawRect, sf);
                        else if((i + 1) % 6 == 0 && !string.IsNullOrEmpty(dgv.Rows[i].Cells[j].FormattedValue.ToString()))
                            e.Graphics.DrawString(dgv.Rows[i].Cells[j].FormattedValue.ToString(), new Font("Arial", 10 - adjust_font_size, FontStyle.Bold), Brushes.Red, drawRect, sf);
                        else
                            e.Graphics.DrawString(dgv.Rows[i].Cells[j].FormattedValue.ToString(), new Font("Arial", 8 - adjust_font_size, FontStyle.Bold), Brushes.Black, drawRect, sf);

                        startWidth += width;
                        tmp += 1;
                        pre_idx = j;
                    }
                }

                startHeight += avgHeight;
                temp++;
                cnt++;

                //한페이지당 36줄
                if (temp % 36 == 0)
                {
                    e.HasMorePages = true;
                    pageNo++;
                    return;
                }
            }
        }


        #endregion

       
    }
}
