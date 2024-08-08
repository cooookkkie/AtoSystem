using AdoNetWindow.Inspection;
using AdoNetWindow.Model;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Arrive
{
    public partial class InspectionList : Form
    {
        IinspectionRepository inspectionRepository = new InspectionRepository();
        CalendarModule.calendar cd;
        Libs.ftpCommon ftp = new Libs.ftpCommon();
        UsersModel um;
        
        Libs.Tools.Common common = new Libs.Tools.Common();

        public InspectionList(CalendarModule.calendar cal, UsersModel umodel)
        {
            InitializeComponent();
            cd = cal;
            um = umodel;
        }

        private void InspectionList_Load(object sender, EventArgs e)
        {
            txtInSttdate.Text = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            txtInEnddate.Text = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
            GetInspection();
        }


        #region Method
        //검품내역 가져오기
        private void GetInspection()
        {
            DateTime sttdate;
            if (!DateTime.TryParse(txtInSttdate.Text, out sttdate))
            {
                MessageBox.Show(this, "입고 검색기간을 확인해주세요.", " "
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button1
                    , MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            DateTime enddate;
            if (!DateTime.TryParse(txtInEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "입고 검색기간을 확인해주세요.", " "
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button1
                    , MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            dgvInspection.Rows.Clear();
            DataTable inspectionDt = inspectionRepository.GetInspectinoList(txtEtdSttdate.Text, txtEtdEnddate.Text, txtEtaSttdate.Text, txtEtaEnddate.Text, txtInSttdate.Text, txtInEnddate.Text
                , txtAtono.Text, txtBlno.Text, txtWarehouse.Text, cbCcStatus.Text, txtIncomeManager.Text, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtInspectionManager.Text, cbInspectionStatus.Text);


            for (int i = 0; i < inspectionDt.Rows.Count; i++)
            {
                int n = dgvInspection.Rows.Add();
                DataGridViewRow row = dgvInspection.Rows[n];


                row.Cells["id"].Value = inspectionDt.Rows[i]["id"].ToString();
                row.Cells["sub_id"].Value = inspectionDt.Rows[i]["sub_id"].ToString();
                row.Cells["inspection_cnt"].Value = inspectionDt.Rows[i]["inspection_cnt"].ToString();
                row.Cells["contract_year"].Value = inspectionDt.Rows[i]["contract_year"].ToString();
                row.Cells["ato_no"].Value = inspectionDt.Rows[i]["ato_no"].ToString();
                row.Cells["bl_no"].Value = inspectionDt.Rows[i]["bl_no"].ToString();
                row.Cells["warehouse"].Value = inspectionDt.Rows[i]["warehouse"].ToString();

                row.Cells["etd"].Value = inspectionDt.Rows[i]["etd"].ToString();
                row.Cells["eta"].Value = inspectionDt.Rows[i]["eta"].ToString();

                DateTime warehousing_date;
                if (DateTime.TryParse(inspectionDt.Rows[0]["warehousing_date"].ToString(), out warehousing_date))
                {
                    warehousing_date = SetInspection(warehousing_date);
                    row.Cells["warehousing_date"].Value = warehousing_date.ToString("yyyy-MM-dd");
                }


                //row.Cells["warehousing_date"].Value = inspectionDt.Rows[i]["warehousing_date"].ToString();
                row.Cells["warehouse"].Value = inspectionDt.Rows[i]["warehouse"].ToString();
                row.Cells["remark"].Value = inspectionDt.Rows[i]["remark"].ToString();

                row.Cells["cc_status"].Value = inspectionDt.Rows[i]["cc_status"].ToString();
                row.Cells["product"].Value = inspectionDt.Rows[i]["product"].ToString();
                row.Cells["origin"].Value = inspectionDt.Rows[i]["origin"].ToString();
                row.Cells["sizes"].Value = inspectionDt.Rows[i]["sizes"].ToString();
                row.Cells["unit"].Value = inspectionDt.Rows[i]["box_weight"].ToString();
                row.Cells["qty"].Value = inspectionDt.Rows[i]["qty"].ToString();
                row.Cells["income_manager"].Value = inspectionDt.Rows[i]["income_manager"].ToString();
                row.Cells["inspection_manager"].Value = inspectionDt.Rows[i]["inspection_manager"].ToString();
                //row.Cells["inspection_status"].Value = inspectionDt.Rows[i]["inspection_status"].ToString();
                row.Cells["inspection_date"].Value = inspectionDt.Rows[i]["inspection_date"].ToString();
            }


        }



        private DateTime SetInspection(DateTime warehousing_date)
        {
            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt;
            DateTime tempDt1 = new DateTime(warehousing_date.Year, warehousing_date.Month, 1);
            DateTime tempDt2 = new DateTime(warehousing_date.Year, warehousing_date.Month, 1).AddDays(-1);
            common.getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            common.getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
            //주말일경우 날짜 수정

            tempDt = warehousing_date;
        retry:
            //주말일 경우 수정
            if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                tempDt = tempDt.AddDays(2);
            else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                tempDt = tempDt.AddDays(1);

            //법정공휴일일 경우
            if (tempDt.Month == tempDt1.Month)
            {
                foreach (int n in no1)
                {
                    if (n == tempDt.Day)
                    {
                        tempDt = tempDt.AddDays(1);
                        goto retry;
                    }
                }
            }
            else
            {
                foreach (int n in no2)
                {
                    if (n == tempDt.Day)
                    {
                        tempDt = tempDt.AddDays(1);
                        goto retry;
                    }
                }
            }

            warehousing_date = tempDt;
            return warehousing_date;
        }
        

        private void SetHeader()
        {
            if (dgvInspection.RowCount > 0)
            {
                dgvInspection.Columns["id"].Visible = false;
                dgvInspection.Columns["sub_id"].Visible = false;
                dgvInspection.Columns["edit_date"].Visible = false;
                dgvInspection.Columns["edit_user"].Visible = false;
                dgvInspection.Columns["inspection_results"].Visible = false;

                dgvInspection.Columns["warehousing_date"].HeaderText = "창고입고예정일";
                dgvInspection.Columns["warehouse"].HeaderText = "창고";
                dgvInspection.Columns["origin"].HeaderText = "원산지";
                dgvInspection.Columns["product"].HeaderText = "품목명";
                dgvInspection.Columns["sizes"].HeaderText = "사이즈";
                dgvInspection.Columns["box_weight"].HeaderText = "박스중량(kg)";
                dgvInspection.Columns["quantity_on_paper"].HeaderText = "계약수량";
                dgvInspection.Columns["inspection_date"].HeaderText = "검품일";
                dgvInspection.Columns["inspection_results"].HeaderText = "검품내용";
                dgvInspection.Columns["inspection_manager"].HeaderText = "검품 담당자";
                foreach (DataGridViewRow dgr in dgvInspection.Rows)
                {
                    if (!string.IsNullOrEmpty(dgr.Cells["inspection_date"].Value.ToString()))
                    {
                        dgr.Cells["inspection_date"].Value = Convert.ToDateTime(dgr.Cells["inspection_date"].Value).ToString("yyyy-MM-dd");
                    }
                }
            }
            //헤더 디자인
            this.dgvInspection.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            this.dgvInspection.ColumnHeadersDefaultCellStyle.BackColor = Color.RosyBrown;
            this.dgvInspection.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
        }
        #endregion

        #region Button
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetInspection();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnEtdSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEtdSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEtdEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEtdEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEtaSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEtaSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEtaEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEtaEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnInSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtInSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnInEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtInEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            dgvInspection.Rows.Clear();
            DataTable inspectionDt = inspectionRepository.GetInspectinoList(txtEtdSttdate.Text, txtEtdEnddate.Text, txtEtaSttdate.Text, txtEtaEnddate.Text, txtInSttdate.Text, txtInEnddate.Text
                , txtAtono.Text, txtBlno.Text, txtWarehouse.Text, cbCcStatus.Text, txtIncomeManager.Text, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtInspectionManager.Text, cbInspectionStatus.Text);


            for (int i = 0; i < inspectionDt.Rows.Count; i++)
            {
                int n = dgvInspection.Rows.Add();
                DataGridViewRow row = dgvInspection.Rows[n];


                row.Cells["id"].Value = inspectionDt.Rows[i]["id"].ToString();
                row.Cells["sub_id"].Value = inspectionDt.Rows[i]["sub_id"].ToString();
                row.Cells["inspection_cnt"].Value = inspectionDt.Rows[i]["inspection_cnt"].ToString();
                row.Cells["ato_no"].Value = inspectionDt.Rows[i]["ato_no"].ToString();
                row.Cells["bl_no"].Value = inspectionDt.Rows[i]["bl_no"].ToString();
                row.Cells["warehouse"].Value = inspectionDt.Rows[i]["warehouse"].ToString();

                row.Cells["etd"].Value = inspectionDt.Rows[i]["etd"].ToString();
                row.Cells["eta"].Value = inspectionDt.Rows[i]["eta"].ToString();
                row.Cells["warehousing_date"].Value = inspectionDt.Rows[i]["warehousing_date"].ToString();

                row.Cells["cc_status"].Value = inspectionDt.Rows[i]["cc_status"].ToString();
                row.Cells["product"].Value = inspectionDt.Rows[i]["product"].ToString();
                row.Cells["origin"].Value = inspectionDt.Rows[i]["origin"].ToString();
                row.Cells["sizes"].Value = inspectionDt.Rows[i]["sizes"].ToString();
                row.Cells["unit"].Value = inspectionDt.Rows[i]["box_weight"].ToString();
                row.Cells["qty"].Value = inspectionDt.Rows[i]["qty"].ToString();
                row.Cells["income_manager"].Value = inspectionDt.Rows[i]["income_manager"].ToString();
                row.Cells["inspection_manager"].Value = inspectionDt.Rows[i]["inspection_manager"].ToString();
                row.Cells["inspection_status"].Value = inspectionDt.Rows[i]["inspection_status"].ToString();
                row.Cells["inspection_date"].Value = inspectionDt.Rows[i]["inspection_date"].ToString();
            }

            /*InspectionModel model = new InspectionModel();

            try
            {
                model.id = Convert.ToInt32(lbId.Text);
                model.sub_id = Convert.ToInt32(lbSubId.Text);
                model.warehousing_date = lbWarehousngDate.Text;
                model.warehouse = lbWarehouse.Text;
                model.origin = lbOrigin.Text;
                model.product = lbProdcut.Text;
                model.sizes = lbSizes.Text;
                model.box_weight = lbBoxWeight.Text;
                model.quantity_on_paper = Convert.ToDouble(lbQty.Text);
                model.inspection_date = dtpInspectionDate.Value.ToString("yyyy-MM-dd");
                model.inspection_results = txtInspectionResults.Text;
                model.inspection_manager = txtInspectionManager.Text;

                if (string.IsNullOrEmpty(model.inspection_manager))
                {
                    MessageBox.Show(this,"검품 담당자를 입력해주세요.");
                    return;
                }
                else if (string.IsNullOrEmpty(model.inspection_results))
                {
                    MessageBox.Show(this,"검품 내용을 입력해주세요.");
                    return;
                }

                if (MessageBox.Show(this,"검품내용을 저장합니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }

                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = new StringBuilder();
                //삭제
                sql = inspectionRepository.DeleteInspection(model);
                sqlList.Add(sql);
                //등록
                sql = inspectionRepository.InsertInspection(model);
                sqlList.Add(sql);
                //Execute
                int results = inspectionRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
                }
                else
                {
                    GetInspection();
                }
            }
            catch 
            {
                MessageBox.Show(this,"등록할 내역을 다시 선택해주세요.");
            }*/
        }
        #endregion

        #region Key event
        private void txtInSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            //날짜 완성
            Control tbb = (Control)sender;
            tbb.Text = common.strDatetime(tbb.Text);
        }
        private void InspectionList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetInspection();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtAtono.Text = String.Empty;
                        txtBlno.Text = String.Empty;
                        txtWarehouse.Text = String.Empty;
                        txtIncomeManager.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        private void txtAtono_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetInspection();
        }



        #endregion

        #region 우클릭 메뉴
        private void dgvInspection_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgvInspection.ClearSelection();
                    dgvInspection.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        private void dgvInspection_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvInspection.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("수정");
                    m.Items.Add("서류폴더");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvInspection, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch { }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (dgvInspection.SelectedRows.Count == 0)
                    return;
                DataGridViewRow dr = dgvInspection.SelectedRows[0];
                int eRowindedx = dgvInspection.SelectedRows[0].Index;

                /*PendingInfo p;*/
                switch (e.ClickedItem.Text)
                {
                    case "수정":
                        int id;
                        if (dr.Cells["id"].Value == null || !int.TryParse(dr.Cells["id"].Value.ToString(), out id))
                        {
                            MessageBox.Show(this, "정보를 찾을 수 없습니다.", " "
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Question
                                , MessageBoxDefaultButton.Button1
                                , MessageBoxOptions.DefaultDesktopOnly);
                            return;
                        }
                        int sub_id;
                        if (dr.Cells["sub_id"].Value == null || !int.TryParse(dr.Cells["sub_id"].Value.ToString(), out sub_id))
                        {
                            MessageBox.Show(this, "정보를 찾을 수 없습니다.", " "
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Question
                                , MessageBoxDefaultButton.Button1
                                , MessageBoxOptions.DefaultDesktopOnly);
                            return;
                        }
                        InspectionInfo inspectioninfo = new InspectionInfo(cd, um, id, sub_id);
                        inspectioninfo.Show();
                        break;
                    case "서류폴더":
                        string folder_path = dr.Cells["contract_year"].Value.ToString()
                            + "/" + ftp.ReplaceName(dr.Cells["ato_no"].Value.ToString())
                            + "/" + ftp.ReplaceName(dr.Cells["product"].Value.ToString())
                            + "/" + ftp.ReplaceName(dr.Cells["origin"].Value.ToString())
                            + "/" + ftp.ReplaceName(dr.Cells["sizes"].Value.ToString());
                        try
                        {
                            if (ftp.CheckDirectory(folder_path, true))
                            {
                                DirectoryInfo di = new DirectoryInfo("O:");
                                //DirectoryInfo.Exists로 폴더 존재유무 확인
                                if (!di.Exists)
                                {
                                    string userid, pwd;
                                    FTPmanager.FtpLoginManager ftpLogin = new FTPmanager.FtpLoginManager(um, out userid, out pwd);
                                    if (!(userid == null || pwd == null))
                                    {
                                        string errMsg;
                                        if (!ftp.StartDirectory(folder_path, out errMsg, userid, pwd))
                                            MessageBox.Show(this,errMsg, " "
                                                        , MessageBoxButtons.OK
                                                        , MessageBoxIcon.Question
                                                        , MessageBoxDefaultButton.Button1
                                                        , MessageBoxOptions.DefaultDesktopOnly);
                                        else
                                            System.Diagnostics.Process.Start("explorer.exe", @"O:\" + folder_path.Replace("/", @"\"));
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Process.Start(@"O:\" + folder_path.Replace("/", @"\"));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this,ex.Message, " "
                                    , MessageBoxButtons.OK
                                    , MessageBoxIcon.Question
                                    , MessageBoxDefaultButton.Button1
                                    , MessageBoxOptions.DefaultDesktopOnly);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message, " "
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button1
                    , MessageBoxOptions.DefaultDesktopOnly);
            }
        }
        #endregion
        
    }
}



