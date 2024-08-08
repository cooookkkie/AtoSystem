using AdoNetWindow.Model;
using Repositories.Calender;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.CalendarModule.VacationManager
{
    public partial class VacationAdminManager : Form
    {
        IAnnualRepository annualRepository = new AnnualRepository();
        UsersModel um;
        public VacationAdminManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        #region Button
        private void btnRegister_Click(object sender, EventArgs e)
        {
            AddVacation av = new AddVacation(um);
            av.Owner = this;
            av.Show();
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            dgvAnnual.Rows.Clear();
            /*DataTable annualDt = annualRepository.GetAnnual(txtUserName.Text, txtDepartment.Text, cbDivision.Text, cbStatus.Text);
            for (int i = 0; i < annualDt.Rows.Count; i++)
            {
                int n = dgvAnnual.Rows.Add();
                DataGridViewRow row = dgvAnnual.Rows[n];
                row.Cells["annual_id"].Value = annualDt.Rows[i]["id"].ToString();
                row.Cells["annual_user_id"].Value = annualDt.Rows[i]["user_id"].ToString();
                row.Cells["annual_user_name"].Value = annualDt.Rows[i]["user_name"].ToString();
                row.Cells["annual_department"].Value = annualDt.Rows[i]["department"].ToString();
                row.Cells["annual_division"].Value = annualDt.Rows[i]["division"].ToString();

                DateTime sttdate;
                if(DateTime.TryParse(annualDt.Rows[i]["sttdate"].ToString(), out sttdate))
                    row.Cells["sttdate"].Value = sttdate.ToString("yyyy-MM-dd");
                DateTime enddate;
                if (DateTime.TryParse(annualDt.Rows[i]["enddate"].ToString(), out enddate))
                    row.Cells["enddate"].Value = enddate.ToString("yyyy-MM-dd");

                row.Cells["annual_use_days"].Value = annualDt.Rows[i]["used_days"].ToString();
                row.Cells["remark"].Value = annualDt.Rows[i]["remark"].ToString();
                row.Cells["status"].Value = annualDt.Rows[i]["status"].ToString();
            }*/
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void VacationAdminManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.A:
                        btnRegister.PerformClick();
                        break;

                    case Keys.M:
                        txtUserName.Focus();
                        break;
                    case Keys.N:
                        txtUserName.Text = String.Empty;
                        txtDepartment.Text = String.Empty;
                        txtUserName.Focus();
                        break;
                }
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearching.PerformClick();
        }
        #endregion

        private void dgvAnnual_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgvAnnual.SelectedRows.Count == 0)
                    dgvAnnual.Rows[e.RowIndex].Selected = true;
               }

        }
        #region 우클릭 메뉴
        private void dgvAnnual_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvAnnual.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;
                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("내용수정");
                    ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                    toolStripSeparator2.Name = "toolStripSeparator";
                    toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator2);
                    m.Items.Add("승인");
                    m.Items.Add("대기");
                    m.Items.Add("반려");
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.Show(dgvAnnual, e.Location);
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
                case "내용수정":

                    if (dgvAnnual.SelectedRows.Count > 0)
                    {
                        int annual_id;
                        if (dgvAnnual.SelectedRows[0].Cells["annual_id"].Value != null && int.TryParse(dgvAnnual.SelectedRows[0].Cells["annual_id"].Value.ToString(), out annual_id))
                        {
                            AddVacation av = new AddVacation(um, annual_id);
                            av.Owner = this;
                            av.Show();
                        }
                    }
                    break;
                case "승인":
                    
                    break;
                case "대기":

                    break;
                case "반려":

                    break;
                default:
                    break;
            }
        }


        private void UpdateStatus(string status)
        {
            //MSG
            if (MessageBox.Show(this, $"선택 내역을 {status} 처리겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;


            for (int i = 0; i < dgvAnnual.Rows.Count; i++)
            {
                if (dgvAnnual.Rows[i].Selected)
                { 
                    
                }
            }
        }
        #endregion
    }
}

