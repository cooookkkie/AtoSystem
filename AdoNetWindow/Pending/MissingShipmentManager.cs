using AdoNetWindow.CalendarModule;
using AdoNetWindow.Common;
using AdoNetWindow.Model;
using Microsoft.Office.Interop.Excel;
using Repositories;
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
using DataTable = System.Data.DataTable;
using TextBox = System.Windows.Forms.TextBox;

namespace AdoNetWindow.Pending
{
    public partial class MissingShipmentManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        Libs.Tools.Common common = new Libs.Tools.Common();
        calendar cal;
        UsersModel um;
        public MissingShipmentManager(calendar cal, UsersModel um)
        {
            InitializeComponent();
            this.cal = cal;
            this.um = um;
        }
        private void MissingShipmentManager_Load(object sender, EventArgs e)
        {
            txtStandardDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        #region Method
        public void CalendarOpenAlarm(bool isMsg = false)
        {
            int cnt = GetData();
            if (cnt > 0)
            {
                this.Show();
                this.Owner = cal;
            }

            else if (isMsg)
            {
                MessageBox.Show(this, "임박 품목내역이 없습니다.");
                this.Activate();
            }
        }
        private int GetData()
        {
            DateTime standard_date;
            if (!DateTime.TryParse(txtStandardDate.Text, out standard_date))
                standard_date = DateTime.Now;

            dgvPending.Rows.Clear();
            DataTable pendingDt = customsRepository.GetMissingETDPendingList(standard_date, txtAtono.Text, txtManager.Text);
            if (pendingDt != null && pendingDt.Rows.Count > 0)
            { 
                for(int i = 0; i <  pendingDt.Rows.Count; i++) 
                {
                    int n = dgvPending.Rows.Add();
                    DataGridViewRow row = dgvPending.Rows[n];
                    row.Cells["custom_id"].Value = pendingDt.Rows[i]["id"].ToString();
                    row.Cells["ato_no"].Value = pendingDt.Rows[i]["ato_no"].ToString();
                    row.Cells["contract_year"].Value = pendingDt.Rows[i]["contract_year"].ToString();
                    row.Cells["contract_no"].Value = pendingDt.Rows[i]["contract_no"].ToString();
                    row.Cells["bl_no"].Value = pendingDt.Rows[i]["bl_no"].ToString();
                    row.Cells["shipment_date"].Value = pendingDt.Rows[i]["shipment_date"].ToString();
                    row.Cells["etd"].Value = pendingDt.Rows[i]["etd"].ToString();
                    row.Cells["eta"].Value = pendingDt.Rows[i]["eta"].ToString();
                    row.Cells["manager"].Value = pendingDt.Rows[i]["manager"].ToString();
                }
            }

            return pendingDt.Rows.Count;

        }
        #endregion

        #region Button
        private void btnStandardDate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtStandardDate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnShipmentDateUpdate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                foreach (DataGridViewRow row in dgvPending.Rows)
                    row.Cells["shipment_date"].Value = sDate;
            }
        }

        private void btnEtdUpdate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                foreach (DataGridViewRow row in dgvPending.Rows)
                    row.Cells["etd"].Value = sDate;
            }
        }

        private void btnEtaUpdate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                foreach (DataGridViewRow row in dgvPending.Rows)
                    row.Cells["eta"].Value = sDate;
            }
        }
        #endregion

        #region Key event
        private void MissingShipmentManager_KeyDown(object sender, KeyEventArgs e)
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
                    case Keys.M:
                        txtAtono.Focus();
                        break;
                    case Keys.N:
                        txtAtono.Text = string.Empty;
                        txtManager.Text = string.Empty;
                        txtAtono.Focus();
                        break;
                }
            }
        }

        private void txtStandardDate_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox con = (TextBox)sender;
            if (con.Name.Equals("txtStandardDate")) 
                con.Text = common.strDatetime(con.Text);
                

            if (e.KeyCode == Keys.Enter)
                GetData();
        }
        #endregion

        #region datagridview event
        private void dgvPending_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvPending.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
                if (dgvPending.Columns[e.ColumnIndex].Name.Equals("shipment_date")
                    || dgvPending.Columns[e.ColumnIndex].Name.Equals("etd")
                    || dgvPending.Columns[e.ColumnIndex].Name.Equals("eta"))
                {
                    if (dgvPending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        dgvPending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = common.strDatetime(dgvPending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                this.dgvPending.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
            }
        }
        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
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
            //유효성검사
            if (dgvPending.Rows.Count == 0)
            {
                messageBox.Show(this, "수정할 내역이 없습니다.");
                return;
            }
            foreach(DataGridViewRow row in dgvPending.Rows) 
            {
                DateTime dt;
                if (row.Cells["shipment_date"].Value != null && !string.IsNullOrEmpty(row.Cells["shipment_date"].Value.ToString())
                    && !DateTime.TryParse(row.Cells["shipment_date"].Value.ToString(), out dt))
                {
                    dgvPending.ClearSelection();
                    dgvPending.CurrentCell = row.Cells["shipment_date"];
                    row.Cells["shipment_date"].Selected = true;
                    messageBox.Show(this, "날짜 형식이 아닙니다.");
                    return;
                }

                if (row.Cells["etd"].Value != null && !string.IsNullOrEmpty(row.Cells["etd"].Value.ToString())
                    && !DateTime.TryParse(row.Cells["etd"].Value.ToString(), out dt))
                {
                    dgvPending.ClearSelection();
                    dgvPending.CurrentCell = row.Cells["etd"];
                    row.Cells["etd"].Selected = true;
                    messageBox.Show(this, "날짜 형식이 아닙니다.");
                    return;
                }

                if (row.Cells["eta"].Value != null && !string.IsNullOrEmpty(row.Cells["eta"].Value.ToString())
                    && !DateTime.TryParse(row.Cells["eta"].Value.ToString(), out dt))
                {
                    dgvPending.ClearSelection();
                    dgvPending.CurrentCell = row.Cells["eta"];
                    row.Cells["eta"].Selected = true;
                    messageBox.Show(this, "날짜 형식이 아닙니다.");
                    return;
                }
            }

            //데이터 수정
            List<StringBuilder> sqlList = new List<StringBuilder>();
            foreach (DataGridViewRow row in dgvPending.Rows)
            {
                int id = Convert.ToInt32(row.Cells["custom_id"].Value);
                string shipment_date = "";
                if (row.Cells["shipment_date"].Value != null && DateTime.TryParse(row.Cells["shipment_date"].Value.ToString(), out DateTime shipmentDt))
                    shipment_date = shipmentDt.ToString("yyyy-MM-dd");
                string etd = "";
                if (row.Cells["etd"].Value != null && DateTime.TryParse(row.Cells["etd"].Value.ToString(), out DateTime etdDt))
                    etd = etdDt.ToString("yyyy-MM-dd");
                string eta = "";
                if (row.Cells["eta"].Value != null && DateTime.TryParse(row.Cells["eta"].Value.ToString(), out DateTime etaDt))
                    eta = etaDt.ToString("yyyy-MM-dd");


                StringBuilder sql = customsRepository.UpdateShipmentSchdule(id, shipment_date, etd, eta);
                sqlList.Add(sql);
            }
            if(sqlList.Count > 0) 
            {
                int result = commonRepository.UpdateTran(sqlList);
                if (result == -1)
                    messageBox.Show(this, "수정중 에러가 발생하였습니다.");
                else
                {
                    messageBox.Show(this, "수정완료");
                    GetData();
                }
            }
        }
    }
}
