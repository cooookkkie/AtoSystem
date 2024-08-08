using AdoNetWindow.Model;
using Repositories;
using Repositories.Calender;
using Repositories.Config;
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
    public partial class FavoriteMenuSettingManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IFavoriteMenuRepository favoriteMenuRepository = new FavoriteMenuRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        CalendarModule.calendar cal;
        UsersModel um;
        public FavoriteMenuSettingManager(CalendarModule.calendar calendar, UsersModel uModel)
        {
            InitializeComponent();
            cal = calendar;
            um = uModel;
        }
        private void FavoriteMenuSettingManager_Load(object sender, EventArgs e)
        {
            GetData();
            SetAllMenu();
        }

        #region Method
        private void GetData()
        {
            List<FavoriteMenuModel> model = favoriteMenuRepository.GetFavoriteMenu(um.user_id);
            if (model.Count > 0)
            {
                for (int i= 0; i < model.Count; i++)    
                {
                    int n = dgvSettingMenu.Rows.Add();
                    dgvSettingMenu.Rows[n].Cells["category"].Value = model[i].category;
                    dgvSettingMenu.Rows[n].Cells["form_name"].Value = model[i].form_name;
                }
            }
        }
        private void MenuDelete(int rowindex)
        {
            dgvSettingMenu.Rows.Remove(dgvSettingMenu.Rows[rowindex]);
        }
        
        private void SetAllMenu()
        {
            DataGridView dgv = dgvAllMenu;





            //로그인 유저 권한설정======================================================
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
                MenuStrip msMenu = cal.GetMenu();
                foreach (ToolStripItem tsItem in msMenu.Items)
                {
                    if (tsItem is ToolStripMenuItem)
                    {
                        ToolStripMenuItem mainMenu = (ToolStripMenuItem)tsItem;
                        string group_name = mainMenu.Text;
                        foreach (ToolStripItem subItem in mainMenu.DropDownItems)
                        {
                            if (subItem is ToolStripMenuItem)
                            {
                                string form_name = subItem.Text;
                                bool is_flag = true;
                                DataRow[] ddr = authorityDt.Select($"group_name = '{group_name}' AND form_name = '{form_name}'");
                                if (ddr.Length > 0)
                                {
                                    if (!bool.TryParse(ddr[0]["is_visible"].ToString(), out is_flag))
                                        is_flag = false;
                                }

                                if (is_flag)
                                {
                                    foreach (DataGridViewColumn col in dgv.Columns)
                                    {
                                        if (col.HeaderText == group_name)
                                        {
                                            if (dgv.Rows.Count == 0)
                                            {
                                                int n = dgv.Rows.Add();
                                                dgv.Rows[n].Cells[col.Name].Value = form_name;
                                                break;
                                            }
                                            else
                                            {
                                                foreach (DataGridViewRow row in dgv.Rows)
                                                {
                                                    //한줄추가
                                                    if (row.Index == dgv.RowCount - 1 && (row.Cells[col.Name].Value != null && !string.IsNullOrEmpty(row.Cells[col.Name].Value.ToString())))
                                                    {
                                                        int n = dgv.Rows.Add();
                                                        dgv.Rows[n].Cells[col.Name].Value = form_name;
                                                        break;
                                                    }
                                                    //추가없이 데이터 추가
                                                    else if (row.Index < dgv.RowCount - 1)
                                                    {
                                                        if (row.Cells[col.Name].Value == null || string.IsNullOrEmpty(row.Cells[col.Name].Value.ToString()))
                                                        {
                                                            row.Cells[col.Name].Value = form_name;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }






            /*//로그인 유저 권한설정======================================================
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
                    if (bool.TryParse(ddr["is_visible"].ToString(), out bool is_visible) && is_visible)
                    {

                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            if (col.HeaderText == ddr["group_name"].ToString())
                            {
                                if (dgv.Rows.Count == 0)
                                {
                                    int n = dgv.Rows.Add();
                                    dgv.Rows[n].Cells[col.Name].Value = ddr["form_name"].ToString();
                                    break;
                                }
                                else
                                {

                                    foreach (DataGridViewRow row in dgv.Rows)
                                    {
                                        //한줄추가
                                        if (row.Index == dgv.RowCount - 1 && (row.Cells[col.Name].Value != null && !string.IsNullOrEmpty(row.Cells[col.Name].Value.ToString())))
                                        {
                                            int n = dgv.Rows.Add();
                                            dgv.Rows[n].Cells[col.Name].Value = ddr["form_name"].ToString();
                                            break;
                                        }
                                        //추가없이 데이터 추가
                                        else if (row.Index < dgv.RowCount - 1)
                                        {
                                            if (row.Cells[col.Name].Value == null || string.IsNullOrEmpty(row.Cells[col.Name].Value.ToString()))
                                            {
                                                row.Cells[col.Name].Value = ddr["form_name"].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
/*




            if (um.auth_level >= 90)
            {
                dgv.Rows.Add("국가별 배송기간", "수출입현황", "검품리스트", "품목 관리", "원금회수율 관리", "거래처별 매입단가 일괄등록", "입항 일정", "품목단가표");
                dgv.Rows.Add("나라별 공휴일", "HACCP 업체정보", "", "거래처 관리", "영업거래처 관리", "거래처별 매입단가 조회", "팬딩 등록", "취급품목서");
                dgv.Rows.Add("내정보 수정", "수출업소대장", "", "조업/계약시기", "영업전화 대시보드", "매입단가 그래프", "팬딩 조회", "취급품목서(초밥류)");
                dgv.Rows.Add("관리자 설정", "식품제조가공업정보", "", "", "", "원가계산", "팬딩 조회2", "업체별시세현황");
                dgv.Rows.Add("", "수입식품 허가정보", "", "", "", "원가 및 재고 대시보드", "", "업체별시세현황2");
                dgv.Rows.Add("", "", "", "", "", "해외제조업소 및 수입업체 수출입", "", "품명별매출한도");
                dgv.Rows.Add("", "", "", "", "", "", "", "품명별 매출관리 대시보드");
            }
            else if (um.auth_level >= 50)
            {
                dgv.Rows.Add("국가별 배송기간", "수출입현황", "검품리스트", "품목 관리", "원금회수율 관리", "거래처별 매입단가 일괄등록", "입항 일정", "품목단가표");
                dgv.Rows.Add("나라별 공휴일", "HACCP 업체정보", "", "거래처 관리", "영업거래처 관리", "거래처별 매입단가 조회", "팬딩 등록", "취급품목서");
                dgv.Rows.Add("내정보 수정", "수출업소대장", "", "조업/계약시기", "영업전화 대시보드", "매입단가 그래프", "팬딩 조회", "취급품목서(초밥류)");
                dgv.Rows.Add("", "식품제조가공업정보", "", "", "", "원가계산", "팬딩 조회2", "업체별시세현황");
                dgv.Rows.Add("", "수입식품 허가정보", "", "", "", "원가 및 재고 대시보드", "", "업체별시세현황2");
                dgv.Rows.Add("", "", "", "", "", "해외제조업소 및 수입업체 수출입", "", "품명별매출한도");
                dgv.Rows.Add("", "", "", "", "", "", "", "품명별 매출관리 대시보드");
            }
            else 
            {
                dgv.Rows.Add("국가별 배송기간", "수출입현황", "검품리스트", "조업/계약시기", "원금회수율 관리", "거래처별 매입단가 일괄등록", "입항 일정", "품목단가표");
                dgv.Rows.Add("나라별 공휴일", "HACCP 업체정보", "", "", "영업거래처 관리", "거래처별 매입단가 조회", "팬딩 등록", "취급품목서");
                dgv.Rows.Add("내정보 수정", "수출업소대장", "", "", "영업전화 대시보드", "매입단가 그래프", "팬딩 조회", "취급품목서(초밥류)");
                dgv.Rows.Add("", "식품제조가공업정보", "", "", "", "원가계산", "팬딩 조회2", "업체별시세현황");
                dgv.Rows.Add("", "수입식품 허가정보", "", "", "", "원가 및 재고 대시보드", "", "업체별시세현황2");
                dgv.Rows.Add("", "", "", "", "", "해외제조업소 및 수입업체 수출입", "", "품명별매출한도");
                dgv.Rows.Add("", "", "", "", "", "", "", "품명별 매출관리 대시보드");

                dgv.Columns["import"].Visible = false;
                dgv.Columns["pendding"].Visible = false;
            }*/
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(252, 228, 214);

            dgv = dgvSettingMenu;
            dgvSettingMenu.Init();
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

        }

        private void AddForm(DataGridViewCell cell)
        {
            string form_name = cell.Value.ToString();
            string category = dgvAllMenu.Columns[cell.ColumnIndex].HeaderText;
            //중복검사
            if (dgvSettingMenu.Rows.Count > 0 && !string.IsNullOrEmpty(form_name) && !string.IsNullOrEmpty(category))
            {
                for (int i = 0; i < dgvSettingMenu.Rows.Count; i++)
                {
                    if (dgvSettingMenu.Rows[i].Cells["category"].Value.ToString() == category
                    && dgvSettingMenu.Rows[i].Cells["form_name"].Value.ToString() == form_name)
                    {
                        MessageBox.Show(this, "이미 추가된 메뉴입니다.");
                        this.Activate();
                        return;
                    }
                }
            }
            int cnt = commonRepository.GetNextId("t_favorite_menu", "form_number", "user_id", "'" + um.user_id + "'");
            //Insert
            FavoriteMenuModel model = new FavoriteMenuModel();
            model.user_id = um.user_id;
            model.form_number = cnt;
            model.category = category;
            model.form_name = form_name;
            StringBuilder sql = favoriteMenuRepository.InsertSql(model);
            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);
            int result = favoriteMenuRepository.UpdateTrans(sqlList);
            if (result == -1)
            {
                MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                this.Activate();
                return;
            }
            //추가
            int n = dgvSettingMenu.Rows.Add();
            dgvSettingMenu.Rows[n].Cells["category"].Value = category;
            dgvSettingMenu.Rows[n].Cells["form_name"].Value = form_name;
            cal.GetFavoriteMenu();

        }
        #endregion

        #region Datagridview event
        private void dgvAllMenu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dgvAllMenu.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (!(cell.Value == null || cell.Value == ""))
                {
                    AddForm(cell);
                }
            }
        }
        private void dgvSettingMenu_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        // RowPointPaint 이벤트핸들러            
        // 행헤더 열영역에 행번호를 보여주기 위해 장방형으로 처리           
        Rectangle rect = new Rectangle(e.RowBounds.Location.X,
            e.RowBounds.Location.Y,
            dgvSettingMenu.RowHeadersWidth - 4,
            e.RowBounds.Height);
        // 위에서 생성된 장방형내에 행번호를 보여주고 폰트색상 및 배경을 설정           
        TextRenderer.DrawText(e.Graphics,
            (e.RowIndex + 1).ToString(),
            dgvSettingMenu.RowHeadersDefaultCellStyle.Font,
            rect,                                
            dgvSettingMenu.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }
        #endregion

        #region Key event
        private void FavoriteMenuSettingManager_KeyDown(object sender, KeyEventArgs e)
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
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Datagridview event
        private void dgvSettingMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSettingMenu.Columns[e.ColumnIndex].Name == "btnDelete")
                {
                    DataGridViewRow row = dgvSettingMenu.Rows[e.RowIndex];  
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    StringBuilder sql = favoriteMenuRepository.DeleteSql(um.user_id, row.Cells["category"].Value.ToString(), row.Cells["form_name"].Value.ToString());
                    sqlList.Add(sql);
                    //Execute
                    int result = favoriteMenuRepository.UpdateTrans(sqlList);
                    if (result == -1)
                    {
                        MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        MenuDelete(e.RowIndex);
                        cal.GetFavoriteMenu();
                    }
                }
            }
        }
        #endregion
    }
}
