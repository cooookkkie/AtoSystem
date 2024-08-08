using AdoNetWindow.Model;
using AdoNetWindow.SEAOVER.TwoLine;
using Repositories;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ubiety.Dns.Core;

namespace AdoNetWindow.Config.AuthorityManager
{
    public partial class AuthorityManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IUsersRepository usersRepository = new UsersRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();   
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um;
        public AuthorityManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }


        private void AuthorityManager_Load(object sender, EventArgs e)
        {
            cbAuthorityTemplate.Items.Clear();
            //부서
            DataTable resultDt = usersRepository.GetTragetData(1, "", "");
            if (resultDt.Rows.Count > 0)
            {
                for (int i = 0; i < resultDt.Rows.Count; i++)
                {
                    cbAuthorityTemplate.Items.Add(resultDt.Rows[i]["department"].ToString());
                }
            }
            //유저
            resultDt = usersRepository.GetTragetData(2, "", "");
            if (resultDt.Rows.Count > 0)
            {
                for (int i = 0; i < resultDt.Rows.Count; i++)
                {
                    cbAuthorityTemplate.Items.Add(resultDt.Rows[i]["department"].ToString() + "_" + resultDt.Rows[i]["user_name"].ToString());
                }
            }
        }

        #region Method
        private void GetTargetData()
        {
            dgvAuthorityTarget.Rows.Clear();

            int data_type;
            if(rbDepartment.Checked)
                data_type = 1;
            else 
                data_type = 2;
            DataTable resultDt = usersRepository.GetTragetData(data_type, txtDepartment.Text, txtUserName.Text);

            if (resultDt.Rows.Count > 0)
            {
                for (int i = 0; i < resultDt.Rows.Count; i++)
                {
                    int n = dgvAuthorityTarget.Rows.Add();
                    dgvAuthorityTarget.Rows[n].Cells["department"].Value = resultDt.Rows[i]["department"].ToString();
                    dgvAuthorityTarget.Rows[n].Cells["user_id"].Value = resultDt.Rows[i]["user_id"].ToString();
                    dgvAuthorityTarget.Rows[n].Cells["user_name"].Value = resultDt.Rows[i]["user_name"].ToString();
                    dgvAuthorityTarget.Rows[n].Cells["is_registration"].Value = Convert.ToBoolean(resultDt.Rows[i]["is_registration"].ToString());
                }
            }
        }

        private void GetAuthority()
        {
            dgvAuthority.Rows.Clear();

            //설정============================================
            string group_name = "설정";
            int n = dgvAuthority.Rows.Add();
            DataGridViewRow row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "국가별 배송기간";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "연차관리";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "관리자설정";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "FAX";

            //기준정보============================================
            group_name = "기준정보";
            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "품목관리";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "거래처관리";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "조업/계약시기";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "수입예정관리";

            //영업거래처 관리============================================
            group_name = "영업거래처 관리";
            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "원금회수율 관리";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "거래처 관리";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "영업일보";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "영업전화 대시보드";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "매출내역 대시보드";

            //수입관리============================================
            group_name = "수입관리";
            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "거래처별 매입단가 일괄등록";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "거래처별 매입단가 조회";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "매입단가 그래프";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "원가계산";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "원가 및 재고 대시보드";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "해외제조업소 및 수입업체 수출입";

            //팬딩관리============================================
            group_name = "팬딩관리";
            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "입항 일정";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "팬딩 등록";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "팬딩 수정";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "팬딩 조회";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "팬딩 조회2";

            //씨오버============================================
            group_name = "씨오버";
            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "품목단가표";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "취급품목서";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "취급품목서(초밥류)";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "업체별시세관리";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "품명별 매출한도";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "품명별 매출관리 대시보드";

            n = dgvAuthority.Rows.Add();
            row = dgvAuthority.Rows[n];
            row.Cells["group_name"].Value = group_name;
            row.Cells["form_name"].Value = "다중 대시보드";

            foreach (DataGridViewRow dr in dgvAuthority.Rows)
            {
                dr.Cells[2].Value = false;
                dr.Cells[3].Value = false;
                dr.Cells[4].Value = false;
                dr.Cells[5].Value = false;
                dr.Cells[6].Value = false;
                dr.Cells[7].Value = false;
                dr.Cells[8].Value = false;
            }

            //권한설정 내역 가져오기
            string user_id;
            if (rbDepartment.Checked)
                user_id = lbDepartment.Text;
            else
                user_id = lbUserid.Text;
            DataTable authorityDt = authorityRepository.GetAuthority(user_id);
            if (authorityDt.Rows.Count > 0)
            {
                foreach (DataRow authorityDr in authorityDt.Rows)
                {
                    bool isVisible;
                    if (!bool.TryParse(authorityDr["is_visible"].ToString(), out isVisible))
                        isVisible = false;

                    bool isAdd;
                    if (!bool.TryParse(authorityDr["is_add"].ToString(), out isAdd))
                        isAdd = false;


                    bool isUpdate;
                    if (!bool.TryParse(authorityDr["is_update"].ToString(), out isUpdate))
                        isUpdate = false;

                    bool isDelete;
                    if (!bool.TryParse(authorityDr["is_delete"].ToString(), out isDelete))
                        isDelete = false;

                    bool isExcel;
                    if (!bool.TryParse(authorityDr["is_excel"].ToString(), out isExcel))
                        isExcel = false;

                    bool isPrint;
                    if (!bool.TryParse(authorityDr["is_print"].ToString(), out isPrint))
                        isPrint = false;

                    bool isAdmin;
                    if (!bool.TryParse(authorityDr["is_admin"].ToString(), out isAdmin))
                        isAdmin = false;


                    foreach (DataGridViewRow dr in dgvAuthority.Rows)
                    {
                        if (dr.Cells["group_name"].Value != null && dr.Cells["group_name"].Value.ToString() == authorityDr["group_name"].ToString()
                            && dr.Cells["form_name"].Value != null && dr.Cells["form_name"].Value.ToString() == authorityDr["form_name"].ToString())
                        {
                            dr.Cells["isVisible"].Value = isVisible;
                            dr.Cells["isAdd"].Value = isAdd;
                            dr.Cells["isUpdate"].Value = isUpdate;
                            dr.Cells["isDelete"].Value = isDelete;
                            dr.Cells["isExcel"].Value = isExcel;
                            dr.Cells["isPrint"].Value = isPrint;
                            dr.Cells["isAdmin"].Value = isAdmin;
                            break;
                        }
                    }
                }
            }
            else if(rbUsers.Checked && authorityDt.Rows.Count == 0)
            {
                messageBox.Show(this, "유저별 권한 설정이 저장되지 않았습니다. 부서의 권한 설정을 불러오겠습니다.");
                authorityDt = authorityRepository.GetAuthority(lbDepartment.Text);
                if (authorityDt.Rows.Count > 0)
                {
                    foreach (DataRow authorityDr in authorityDt.Rows)
                    {
                        bool isVisible;
                        if (!bool.TryParse(authorityDr["is_visible"].ToString(), out isVisible))
                            isVisible = false;

                        bool isAdd;
                        if (!bool.TryParse(authorityDr["is_add"].ToString(), out isAdd))
                            isAdd = false;


                        bool isUpdate;
                        if (!bool.TryParse(authorityDr["is_update"].ToString(), out isUpdate))
                            isUpdate = false;

                        bool isDelete;
                        if (!bool.TryParse(authorityDr["is_delete"].ToString(), out isDelete))
                            isDelete = false;

                        bool isExcel;
                        if (!bool.TryParse(authorityDr["is_excel"].ToString(), out isExcel))
                            isExcel = false;

                        bool isPrint;
                        if (!bool.TryParse(authorityDr["is_print"].ToString(), out isPrint))
                            isPrint = false;

                        bool isAdmin;
                        if (!bool.TryParse(authorityDr["is_admin"].ToString(), out isAdmin))
                            isAdmin = false;


                        foreach (DataGridViewRow dr in dgvAuthority.Rows)
                        {
                            if (dr.Cells["group_name"].Value != null && dr.Cells["group_name"].Value.ToString() == authorityDr["group_name"].ToString()
                                && dr.Cells["form_name"].Value != null && dr.Cells["form_name"].Value.ToString() == authorityDr["form_name"].ToString())
                            {
                                dr.Cells["isVisible"].Value = isVisible;
                                dr.Cells["isAdd"].Value = isAdd;
                                dr.Cells["isUpdate"].Value = isUpdate;
                                dr.Cells["isDelete"].Value = isDelete;
                                dr.Cells["isExcel"].Value = isExcel;
                                dr.Cells["isPrint"].Value = isPrint;
                                dr.Cells["isAdmin"].Value = isAdmin;
                                break;
                            }
                        }
                    }
                }
            }
            else
                messageBox.Show(this, "저장된 설정내역이 없습니다.");
        }
        #endregion

        #region Button
        private void btnAuthorityRefresh_Click(object sender, EventArgs e)
        {
            if (messageBox.Show(this, "개별로 등록된 권한 설정내역을 초기화 하시겠습니까? \n * 초기화된 권한은 부서별 권한으로 대체됩니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            StringBuilder sql = authorityRepository.DeleteUsersAuthority("", rbUsers.Checked);
            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);

            if (commonRepository.UpdateTran(sqlList) == -1)
                messageBox.Show(this, "수정중 에러가 발생하였습니다.");
            else
            {
                messageBox.Show(this, "완료");
                GetTargetData();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetTargetData();
        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            if (rbDepartment.Checked && (string.IsNullOrEmpty(lbDepartment.Text) || lbDepartment.Text == "NULL"))
            {
                messageBox.Show(this, "부서를 먼저 선택해주세요!");
                return;
            }
            else if (rbUsers.Checked && (string.IsNullOrEmpty(lbUserid.Text) || lbUserid.Text == "NULL"))
            {
                messageBox.Show(this, "사용자를 먼저 선택해주세요!");
                return;
            }

            string messageTxt;
            if (rbDepartment.Checked)
                messageTxt = lbDepartment.Text + " 권한 설정내역을 저장하시겠습니까?";
            else
                messageTxt = lbDepartment.Text + " " + lbUsername.Text + "님의 권한 설정내역을 저장하시겠습니까?";

            if (messageBox.Show(this, messageTxt, "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            //기존내역 삭제
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();
            if(rbDepartment.Checked)
                sql = authorityRepository.DeleteAuthority(lbDepartment.Text);
            else
                sql = authorityRepository.DeleteAuthority(lbUserid.Text);

            sqlList.Add(sql);   
            //신규내역 등록
            AuthorityModel model = new AuthorityModel();
            model.department = lbDepartment.Text;
            if (rbDepartment.Checked)
                model.user_id = lbDepartment.Text;
            else
                model.user_id = lbUserid.Text;
            model.user_name = lbUsername.Text;
            model.edit_user = um.user_name;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (DataGridViewRow row in dgvAuthority.Rows)
            {
                if (row.Cells["form_name"].Value == null)
                    row.Cells["form_name"].Value = string.Empty;
                if (row.Cells["group_name"].Value == null)
                    row.Cells["group_name"].Value = string.Empty;

                model.form_name = row.Cells["form_name"].Value.ToString();
                model.group_name = row.Cells["group_name"].Value.ToString();

                bool isVisible;
                if (row.Cells["isVisible"].Value == null || !bool.TryParse(row.Cells["isVisible"].Value.ToString(), out isVisible))
                    isVisible = false;

                bool isAdd;
                if (row.Cells["isAdd"].Value == null || !bool.TryParse(row.Cells["isAdd"].Value.ToString(), out isAdd))
                    isAdd = false;

                bool isUpdate;
                if (row.Cells["isUpdate"].Value == null || !bool.TryParse(row.Cells["isUpdate"].Value.ToString(), out isUpdate))
                    isUpdate = false;

                bool isDelete;
                if (row.Cells["isDelete"].Value == null || !bool.TryParse(row.Cells["isDelete"].Value.ToString(), out isDelete))
                    isDelete = false;

                bool isExcel;
                if (row.Cells["isExcel"].Value == null || !bool.TryParse(row.Cells["isExcel"].Value.ToString(), out isExcel))
                    isExcel = false;

                bool isPrint;
                if (row.Cells["isPrint"].Value == null || !bool.TryParse(row.Cells["isPrint"].Value.ToString(), out isPrint))
                    isPrint = false;

                bool isAdmin;
                if (row.Cells["isAdmin"].Value == null || !bool.TryParse(row.Cells["isAdmin"].Value.ToString(), out isAdmin))
                    isAdmin = false;

                model.is_visible = isVisible;
                model.is_add = isAdd;
                model.is_update = isUpdate;
                model.is_delete = isDelete;
                model.is_excel = isExcel;
                model.is_print = isPrint;
                model.is_admin = isAdmin;
                if (rbUsers.Checked)
                    model.is_individual = true;
                //Sql txt
                sql = authorityRepository.InsertAuthority(model);
                sqlList.Add(sql);
            }
            //Commit
            if ((commonRepository.UpdateTran(sqlList)) == -1)
                messageBox.Show(this, "등록중 에러가 발생하였습니다.");
            else
                messageBox.Show(this, "등록완료");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void rbDepartment_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDepartment.Checked)
                GetTargetData();
            else
                GetTargetData();
        }
        #endregion

        #region Key event
        private void txtDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {

            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnSearch.PerformClick();
                        break;
                }
            }
        }
        private void AuthorityManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode) 
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.A:
                        btnRegistration.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;

                    case Keys.M:
                        txtDepartment.Focus();
                        break;
                    case Keys.N:
                        txtDepartment.Text = string.Empty;
                        txtUserName.Text = string.Empty;
                        txtDepartment.Focus();
                        break;
                }
            }
        }

        #endregion

        #region Datagridview event
        private void dgvAuthorityTarget_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvAuthorityTarget.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAuthorityTarget_CellValueChanged);
                foreach (DataGridViewRow row in dgvAuthorityTarget.Rows)
                    row.Cells["chk"].Value = false;
                this.dgvAuthorityTarget.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAuthorityTarget_CellValueChanged);
                dgvAuthorityTarget.Rows[e.RowIndex].Cells["chk"].Value = true;
            }
        }
        private void dgvAuthorityTarget_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvAuthorityTarget.Columns[e.ColumnIndex].Name == "chk")
                {
                    bool isChecked;
                    if (dgvAuthorityTarget.Rows[e.RowIndex].Cells["chk"].Value == null || !bool.TryParse(dgvAuthorityTarget.Rows[e.RowIndex].Cells["chk"].Value.ToString(), out isChecked))
                        isChecked = false;

                    if (isChecked)
                    {
                        dgvAuthorityTarget.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.Red;
                        dgvAuthorityTarget.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Beige;
                    }
                    else
                    {
                        dgvAuthorityTarget.Rows[e.RowIndex].Cells["chk"].Style.BackColor = Color.White;
                        dgvAuthorityTarget.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void dgvAuthorityTarget_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvAuthorityTarget.Columns[e.ColumnIndex].Name == "chk")
                {
                    bool isChecked;
                    if (dgvAuthorityTarget.Rows[e.RowIndex].Cells["chk"].Value == null || !bool.TryParse(dgvAuthorityTarget.Rows[e.RowIndex].Cells["chk"].Value.ToString(), out isChecked))
                        isChecked = false;

                    if(isChecked) 

                    {
                        if (rbDepartment.Checked)
                        {
                            txtUpdateTarget.Text = dgvAuthorityTarget.Rows[e.RowIndex].Cells["department"].Value.ToString();
                            lbUserid.Text = "NULL";
                            lbUsername.Text = "NULL";
                            lbDepartment.Text = dgvAuthorityTarget.Rows[e.RowIndex].Cells["department"].Value.ToString();
                        }
                        else
                        { 
                            txtUpdateTarget.Text = dgvAuthorityTarget.Rows[e.RowIndex].Cells["department"].Value.ToString() + "_" + dgvAuthorityTarget.Rows[e.RowIndex].Cells["user_name"].Value.ToString();
                            lbUserid.Text = dgvAuthorityTarget.Rows[e.RowIndex].Cells["user_id"].Value.ToString();
                            lbUsername.Text = dgvAuthorityTarget.Rows[e.RowIndex].Cells["user_name"].Value.ToString();
                            lbDepartment.Text = dgvAuthorityTarget.Rows[e.RowIndex].Cells["department"].Value.ToString();
                        }

                        GetAuthority();
                    }
                }
            }
        }
        private void dgvAuthority_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvAuthority.Columns[e.ColumnIndex].Name.Substring(0, 2) == "is")
                {
                    bool isChecked;
                    if (dgvAuthority.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !bool.TryParse(dgvAuthority.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out isChecked))
                        isChecked = false;

                    if (isChecked)
                        dgvAuthority.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightBlue;
                    else
                        dgvAuthority.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                }
            }
        }

        private void dgvAuthority_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvAuthority.Columns[e.ColumnIndex].Name.Substring(0, 2) == "is")
                {
                    bool isChecked;
                    if (dgvAuthority.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !bool.TryParse(dgvAuthority.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out isChecked))
                        isChecked = false;

                    dgvAuthority.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !isChecked;
                    dgvAuthority.EndEdit();
                }
            }
        }
        #endregion

        #region 우클릭 메뉴
        private void dgvAuthority_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    //dgvAuthority.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    if (dgvAuthority.SelectedRows.Count == 1)
                    {
                        dgvAuthority.ClearSelection();
                        dgvAuthority.Rows[e.RowIndex].Selected = true;
                    }
                    else
                    {
                        dgvAuthority.Rows[e.RowIndex].Selected = true;
                    }
                }
                else
                {
                    dgvAuthority.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    dgvAuthority.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                }
            }
        }
        private void dgvAuthority_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvAuthority.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    if (this.dgvAuthority.Rows.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("선택/해제");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.Show(dgvAuthority, e.Location);
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
                case "선택/해제":
                    if (dgvAuthority.SelectedCells.Count > 0)
                    {
                        bool isChecked = false;
                        foreach (DataGridViewCell cell in dgvAuthority.SelectedCells)
                        {
                            if (cell.OwningColumn.Name.Substring(0, 2) == "is")
                            {
                                if (cell.Value == null || !bool.TryParse(cell.Value.ToString(), out isChecked))
                                    isChecked = false;
                                break;
                            }
                        }

                        foreach (DataGridViewCell cell in dgvAuthority.SelectedCells)
                        {
                            if (cell.OwningColumn.Name.Substring(0, 2) == "is")
                                cell.Value = !isChecked;
                        }

                        dgvAuthority.EndEdit();
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void cbAuthorityTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(cbAuthorityTemplate.Text))
            {
                string[] template = cbAuthorityTemplate.Text.Split('_');
                DataTable authorityDt = new DataTable();
                if (template.Length == 2)
                    authorityDt = authorityRepository.GetAuthorityInfo(template[0], template[1]);
                else
                    authorityDt = authorityRepository.GetAuthorityInfo(template[0], "");


                if (authorityDt.Rows.Count > 0)
                {
                    foreach (DataRow authorityDr in authorityDt.Rows)
                    {
                        bool isVisible;
                        if (!bool.TryParse(authorityDr["is_visible"].ToString(), out isVisible))
                            isVisible = false;

                        bool isAdd;
                        if (!bool.TryParse(authorityDr["is_add"].ToString(), out isAdd))
                            isAdd = false;


                        bool isUpdate;
                        if (!bool.TryParse(authorityDr["is_update"].ToString(), out isUpdate))
                            isUpdate = false;

                        bool isDelete;
                        if (!bool.TryParse(authorityDr["is_delete"].ToString(), out isDelete))
                            isDelete = false;

                        bool isExcel;
                        if (!bool.TryParse(authorityDr["is_excel"].ToString(), out isExcel))
                            isExcel = false;

                        bool isPrint;
                        if (!bool.TryParse(authorityDr["is_print"].ToString(), out isPrint))
                            isPrint = false;

                        bool isAdmin;
                        if (!bool.TryParse(authorityDr["is_admin"].ToString(), out isAdmin))
                            isAdmin = false;


                        foreach (DataGridViewRow dr in dgvAuthority.Rows)
                        {
                            if (dr.Cells["group_name"].Value != null && dr.Cells["group_name"].Value.ToString() == authorityDr["group_name"].ToString()
                                && dr.Cells["form_name"].Value != null && dr.Cells["form_name"].Value.ToString() == authorityDr["form_name"].ToString())
                            {
                                dr.Cells["isVisible"].Value = isVisible;
                                dr.Cells["isAdd"].Value = isAdd;
                                dr.Cells["isUpdate"].Value = isUpdate;
                                dr.Cells["isDelete"].Value = isDelete;
                                dr.Cells["isExcel"].Value = isExcel;
                                dr.Cells["isPrint"].Value = isPrint;
                                dr.Cells["isAdmin"].Value = isAdmin;
                                break;
                            }
                        }
                    }
                }
                else
                    messageBox.Show(this, "저장된 내역이 없습니다!");

            }
        }

        #region 우클릭 메뉴2
        private void dgvAuthorityTarget_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvAuthorityTarget.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    if (this.dgvAuthorityTarget.Rows.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("권한설정 보기");
                        m.Items.Add("권한설정 초기화");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked2);
                        //Create 
                        m.Show(dgvAuthorityTarget, e.Location);
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
        void m_ItemClicked2(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvAuthorityTarget.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgvAuthorityTarget.SelectedRows[0];
                switch (e.ClickedItem.Text)
                {
                    case "권한설정 보기":
                        this.dgvAuthorityTarget.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAuthorityTarget_CellValueChanged);
                        foreach (DataGridViewRow row in dgvAuthorityTarget.Rows)
                            row.Cells["chk"].Value = false;
                        this.dgvAuthorityTarget.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAuthorityTarget_CellValueChanged);
                        dgvAuthorityTarget.Rows[dr.Index].Cells["chk"].Value = true;
                        break;
                    case "권한설정 초기화":

                        string target;
                        bool is_individual;
                        if (rbDepartment.Checked)
                        {
                            target = dr.Cells["department"].Value.ToString();
                            is_individual = false;
                        }
                        else
                        {
                            target = dr.Cells["user_id"].Value.ToString();
                            is_individual = true;
                        }

                        if (messageBox.Show(this, "[" + target + "]의 권한 설정내역을 초기화 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;

                        StringBuilder sql = authorityRepository.DeleteUsersAuthority(target, is_individual);
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        sqlList.Add(sql);

                        if (commonRepository.UpdateTran(sqlList) == -1)
                            messageBox.Show(this, "수정중 에러가 발생하였습니다.");
                        else
                        {
                            dr.Cells["is_registration"].Value = false;
                            messageBox.Show(this, "완료");
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void dgvAuthorityTarget_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgvAuthorityTarget.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    if (dgvAuthorityTarget.SelectedRows.Count == 1)
                    {
                        dgvAuthorityTarget.ClearSelection();
                        dgvAuthorityTarget.Rows[e.RowIndex].Selected = true;
                    }
                    else
                    {
                        dgvAuthorityTarget.Rows[e.RowIndex].Selected = true;
                    }
                }
                else
                {
                    dgvAuthorityTarget.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    dgvAuthorityTarget.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                }
            }
        }

        #endregion

        
    }
}
