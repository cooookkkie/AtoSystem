using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.DuplicateCompany
{
    public partial class RemoveDuplicateCompanyManager : Form
    {
        DuplicateCompany dc = null;
        SalesManager sm = null;
        UsersModel um;
        DataGridView returnDgv = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public RemoveDuplicateCompanyManager(UsersModel um, DuplicateCompany dc)
        {
            InitializeComponent();
            this.um = um;
            this.dc = dc;
        }
        public RemoveDuplicateCompanyManager(UsersModel um, SalesManager sm)
        {
            InitializeComponent();
            this.um = um;
            this.sm = sm;
        }

        private void RemoveDuplicateCompanyManager_Load(object sender, EventArgs e)
        {
            dgvLimitSetting.Rows.Clear();
            int n = dgvLimitSetting.Rows.Add();
            dgvLimitSetting.Rows[n].Cells["division"].Value = "중복 거래처" ;
            dgvLimitSetting.Rows[n].Cells["division"].ToolTipText = "공용DATA ~ 거래중";
            dgvLimitSetting.Rows[n].Cells["cbLimitCount"].Value = true;
            dgvLimitSetting.Rows[n].Cells["limit_count"].Value = 3;
            dgvLimitSetting.Rows[n].Cells["limit_terms"].Value = 12;
            dgvLimitSetting.Rows[n].Height = 40;
            /*n = dgvLimitSetting.Rows.Add();
            dgvLimitSetting.Rows[n].Cells["division"].Value = "폐업";
            dgvLimitSetting.Rows[n].Cells["cbLimitCount"].Value = true;
            dgvLimitSetting.Rows[n].Cells["limit_count"].Value = 3;
            dgvLimitSetting.Rows[n].Cells["limit_terms"].Value = 12;*/

            n = dgvLimitSetting.Rows.Add();
            dgvLimitSetting.Rows[n].Cells["division"].Value = "취급X";
            dgvLimitSetting.Rows[n].Cells["cbLimitCount"].Value = true;
            dgvLimitSetting.Rows[n].Cells["limit_count"].Value = 1;
            dgvLimitSetting.Rows[n].Cells["limit_terms"].Value = 12;
            dgvLimitSetting.Rows[n].Height = 40;

            dgvLimitSetting.Rows[n].Cells["cbLimitCount"].ReadOnly = true;
            dgvLimitSetting.Rows[n].Cells["cbLimitCount"].Style.BackColor = Color.Gray;
            dgvLimitSetting.Rows[n].Cells["cbLimitTerms"].ReadOnly = true;
            dgvLimitSetting.Rows[n].Cells["cbLimitTerms"].Style.BackColor = Color.Gray;

            dgvLimitSetting.Rows[n].Cells["limit_count"].Style.BackColor = Color.Gray;
            dgvLimitSetting.Rows[n].Cells["limit_count"].ReadOnly = true;
            dgvLimitSetting.Rows[n].Cells["limit_terms"].Style.BackColor = Color.Gray;
            dgvLimitSetting.Rows[n].Cells["limit_terms"].ReadOnly = true;


            n = dgvLimitSetting.Rows.Add();
            dgvLimitSetting.Rows[n].Cells["division"].Value = "8개월 미만 영업 SEAOVER거래처";
            //dgvLimitSetting.Rows[n].Cells["cbLimitCount"].Value = true;
            dgvLimitSetting.Rows[n].Cells["limit_count"].Value = 1;
            dgvLimitSetting.Rows[n].Cells["limit_terms"].Value = 8;
            dgvLimitSetting.Rows[n].Cells["cbLimitTerms"].Value = true;
            dgvLimitSetting.Rows[n].Height = 40;

            dgvLimitSetting.Rows[n].Cells["cbLimitCount"].ReadOnly = true;
            dgvLimitSetting.Rows[n].Cells["cbLimitCount"].Style.BackColor = Color.Gray;
            dgvLimitSetting.Rows[n].Cells["cbLimitTerms"].ReadOnly = true;
            dgvLimitSetting.Rows[n].Cells["cbLimitTerms"].Style.BackColor = Color.Gray;

            dgvLimitSetting.Rows[n].Cells["limit_count"].Style.BackColor = Color.Gray;
            dgvLimitSetting.Rows[n].Cells["limit_count"].ReadOnly = true;
            dgvLimitSetting.Rows[n].Cells["limit_terms"].Style.BackColor = Color.Gray;
            dgvLimitSetting.Rows[n].Cells["limit_terms"].ReadOnly = true;

            /*if (dc != null)
            {
                *//*if (messageBox.Show(this, "설정 내역으로 중복 거래처를 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;*//*

                DataTable limitSettingDt = common.ConvertDgvToDataTable(dgvLimitSetting);
                dc.RemoveDuplicateCompany(limitSettingDt);
                this.Dispose();
                messageBox.Show(this,"완료");
            }
            else if (sm != null)
            {
                *//*if (messageBox.Show(this, "설정 내역으로 중복 거래처를 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;*//*

                DataTable limitSettingDt = common.ConvertDgvToDataTable(dgvLimitSetting);
                sm.RemoveDuplicateCompany(limitSettingDt);
                this.Dispose();
                messageBox.Show(this,"완료");
            }*/

        }

        public DataTable GetLimitSetting()
        {
            this.ShowDialog();

            return common.ConvertDgvToDataTable(returnDgv);
        }

        #region Button
        Libs.Tools.Common common = new Libs.Tools.Common();
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion  

        #region Key event
        private void RemoveDuplicateCompanyManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { }
            else 
            {
                switch (e.KeyCode)
                { 
                    case Keys.Enter:
                        returnDgv = dgvLimitSetting;
                        this.Close();
                        break;
                    case Keys.Escape:
                        returnDgv = dgvLimitSetting;
                        this.Close();
                        break;
                }
            }
        }
        #endregion

        private void RemoveDuplicateCompanyManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            returnDgv = dgvLimitSetting;
        }
    }
}
