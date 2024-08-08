using AdoNetWindow.Model;
using AdoNetWindow.Product;
using Repositories;
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
    public partial class ConvertProductToSeaover : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        UsersModel um;
        public ConvertProductToSeaover(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }

        private void ConvertProductToSeaover_Load(object sender, EventArgs e)
        {
            SetColumnHeaderStyleSetting();
        }

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
            DataGridView dgv = dgvProduct;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgv.Columns["id"].Visible = false;
            dgv.Columns["sub_id"].Visible = false;
        }

        private int GetData()
        {
            int cnt = 0;
            CallProcedure();
            DataTable seaoverDt = seaoverRepository.GetOneColumn("품명, 원산지, 규격", "", "");
            DataTable productDt = customsRepository.GetNotSeaoverProduct();

            for (int i = 0; i < productDt.Rows.Count; i++)
            {
                /*if (productDt.Rows[i]["product"].ToString() == "흰다리새우(500G*10)")
                {
                    MessageBox.Show(this,"qq");
                }*/

                string whr = "품명 = '" + productDt.Rows[i]["product"].ToString() + "'"
                    + " AND 원산지 = '" + productDt.Rows[i]["origin"].ToString() + "'"
                    + " AND 규격 = '" + productDt.Rows[i]["sizes"].ToString() + "'";
                DataRow[] dr = seaoverDt.Select(whr);
                if (dr.Length == 0)
                {
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = dgvProduct.Rows[n];

                    row.Cells["id"].Value = productDt.Rows[i]["id"].ToString();
                    row.Cells["sub_id"].Value = productDt.Rows[i]["sub_id"].ToString();
                    row.Cells["ato_no"].Value = productDt.Rows[i]["ato_no"].ToString();
                    row.Cells["product"].Value = productDt.Rows[i]["product"].ToString();
                    row.Cells["origin"].Value = productDt.Rows[i]["origin"].ToString();
                    row.Cells["sizes"].Value = productDt.Rows[i]["sizes"].ToString();
                    row.Cells["box_weight"].Value = productDt.Rows[i]["box_weight"].ToString();
                    row.Cells["cost_unit"].Value = productDt.Rows[i]["cost_unit"].ToString();
                    cnt++;
                }
            }
            return cnt;
        }

        //업체별시세현황 스토어프로시져 호출
        private void CallProcedure()
        {
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            try
            {
                if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
                {
                    MessageBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "에러가 발생하였습니다. \n(msg) " + ex.Message);
                this.Activate();
            }
        }

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (dgvProduct.Columns[e.ColumnIndex].Name == "btnUpdate")
                    {
                        ProductManager ps = new ProductManager(um, this);
                        ps.ShowDialog();
                    }
                }
                else
                { 
                    dgvProduct.ClearSelection();
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        public void AddProduct2(List<string[]> list)
        {
            if (list != null && list.Count > 0 && dgvProduct.SelectedRows.Count > 0)
            {
                if (MessageBox.Show(this, "선택하신 정보로 변경하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataGridViewRow row = dgvProduct.SelectedRows[0];
                    //데이터 출력
                    row.Cells["product"].Value = list[0][0];
                    row.Cells["origin"].Value = list[0][1];
                    row.Cells["sizes"].Value = list[0][2];
                    row.Cells["box_weight"].Value = list[0][3];
                    row.Cells["cost_unit"].Value = list[0][4];

                    int results = seaoverRepository.UpdateProduct(row.Cells["id"].Value.ToString(), row.Cells["sub_id"].Value.ToString()
                        , list[0][0], list[0][1], list[0][2], list[0][3], list[0][4]);
                    if (results == -1)
                    {
                        MessageBox.Show(this, "수정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                }
            }
        }

        #region 우클릭 메뉴
        //우클릭 메뉴 Create
        private void dgvProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("상세내역");
                    m.Items.Add("품목변경");
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvProduct, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch
            {
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            if (dgvProduct.SelectedRows.Count > 0)
            {
                row = dgvProduct.SelectedRows[0];
            }

            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();

            switch (e.ClickedItem.Text)
            {
                case "상세내역":
                    if (row.Index >= 0)
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
                            int id = Convert.ToInt32(row.Cells["id"].Value.ToString());
                            UnPendingManager upm = new UnPendingManager(um, id);
                            upm.TopMost = true;
                            upm.Show();
                        }
                    }
                    break;
                case "품목변경":
                    ProductManager ps = new ProductManager(um, this);
                    ps.TopMost = true;
                    ps.ShowDialog();
                    break;
            }
        }
        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ConvertProductToSeaover_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                { 
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
