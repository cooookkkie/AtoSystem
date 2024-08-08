using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
using Repositories.ContractRecommendation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Product
{
    public partial class ContractRecommendationManager2 : Form
    {
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        IContractRecommendationRepository contractRecommendationRepository = new ContractRecommendationRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        UsersModel um;
        public ContractRecommendationManager2(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }
        public ContractRecommendationManager2(UsersModel uModel, string product, string origin)
        {
            InitializeComponent();
            um = uModel;
            txtProduct.Text = product;
            txtOrigin.Text = origin;

            GetData(true);
        }
        private void ContractRecommendationManager2_Load(object sender, EventArgs e)
        {
            SetHeaderStyleSetting();
        }

        #region Method
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void InsertData()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "조업/계약시기", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            dgvProduct.ClearSelection();
            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this, "등록할 데이터가 없습니다.");
                this.Activate();
                return;
            }
            if(um.auth_level < 50)
            {
                MessageBox.Show(this, "접근권한이 없습니다.");
                this.Activate();
                return;
            }
            //Msg
            if (MessageBox.Show(this,dgvProduct.Rows.Count / 2 + "개 내역을 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //Sql
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();
            for (int i = 0; i < dgvProduct.Rows.Count; i = i + 2)
            {
                //Delete
                string product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                string origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                sql = contractRecommendationRepository.DeleteRecommend(product, origin);
                sqlList.Add(sql);
                //Insert
                for (int j = 3; j < 15; j++)
                {
                    //계약시기
                    DataGridViewCell cell = dgvProduct.Rows[i].Cells[j];
                    ContractRecommendationModel contractModel = new ContractRecommendationModel();
                    contractModel.product = product;
                    contractModel.origin = origin;
                    contractModel.division = "계약시기";
                    contractModel.month = j - 2;

                    if (!Convert.ToBoolean(cell.Value))
                        contractModel.recommend_level = 0;
                    else if (cell.Style.BackColor == Color.Pink)
                        contractModel.recommend_level = 1;
                    else if (cell.Style.BackColor == Color.Red)
                        contractModel.recommend_level = 2;
                    else if (cell.Style.BackColor == Color.DarkRed)
                        contractModel.recommend_level = 3;

                    if (dgvProduct.Rows[i].Cells["remark"].Value == null)
                        dgvProduct.Rows[i].Cells["remark"].Value = "";
                    contractModel.remark = dgvProduct.Rows[i].Cells["remark"].Value.ToString();
                    contractModel.edit_user = um.user_name;
                    contractModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    sql = contractRecommendationRepository.InsertRecommend(contractModel);
                    sqlList.Add(sql);

                    //조업시기
                    cell = dgvProduct.Rows[i + 1].Cells[j];

                    ContractRecommendationModel operatingModel = new ContractRecommendationModel();
                    operatingModel.product = product;
                    operatingModel.origin = origin;
                    operatingModel.division = "조업시기";
                    operatingModel.month = j - 2;

                    if (!Convert.ToBoolean(cell.Value))
                        operatingModel.recommend_level = 0;
                    else if (cell.Style.BackColor == Color.LightGreen)
                        operatingModel.recommend_level = 1;
                    else if (cell.Style.BackColor == Color.Green)
                        operatingModel.recommend_level = 2;
                    else if (cell.Style.BackColor == Color.DarkGreen)
                        operatingModel.recommend_level = 3;
                    operatingModel.remark = dgvProduct.Rows[i + 1].Cells["remark"].Value.ToString();
                    operatingModel.edit_user = um.user_name;
                    operatingModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd");

                    sql = contractRecommendationRepository.InsertRecommend(operatingModel);
                    sqlList.Add(sql);
                }
            }
            //Execute
            if (sqlList.Count > 0)
            {
                int results = contractRecommendationRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                    GetData();
            }
        }

        private void SetHeaderStyleSetting()
        {
            //Color col = Color.FromArgb(221, 235, 247);
            Color col = Color.FromArgb(43, 94, 170);

            DataGridView dgv = dgvProduct;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = col;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
        private void CallProductProcedure()
        {
            //업체별시세현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                ////업체별시세현황 스토어프로시져 호출
                if (seaoverRepository.CallStoredProcedure(user_id, sDate, eDate) == 0)
                {
                    MessageBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
                this.Activate();
            }
        }
        private DataTable SetTable()
        {
            DataTable dt = new DataTable();

            DataColumn col01 = new DataColumn();
            col01.DataType = System.Type.GetType("System.String");
            col01.AllowDBNull = false;
            col01.ColumnName = "product";
            col01.Caption = "품명";
            col01.DefaultValue = "";
            dt.Columns.Add(col01);

            DataColumn col02 = new DataColumn();
            col02.DataType = System.Type.GetType("System.String");
            col02.AllowDBNull = true;
            col02.ColumnName = "origin";
            col02.Caption = "원산지";
            col02.DefaultValue = "";
            dt.Columns.Add(col02);

            return dt;
        }
        
        private void GetData(bool isExactly = false)
        {
            dgvProduct.Rows.Clear();
            //업체별시세현황 호출
            CallProductProcedure();
            //조업시기 검색
            string operating = txtOperating.Text;
            //계약시기 검색
            string contract = txtContract.Text;
            //씨오버 품목정보
            DataTable seaoverDt = seaoverRepository.GetProductTable3(txtProduct.Text, txtOrigin.Text, 1000000, isExactly);
            DataTable atoDt = productOtherCostRepository.GetProductByUser(txtProduct.Text, txtOrigin.Text, "");

            if (atoDt.Rows.Count > 0)
            {
                for (int i = 0; i < atoDt.Rows.Count; i++)
                {
                    DataRow dr = seaoverDt.NewRow();
                    dr["품명"] = atoDt.Rows[i]["product"].ToString();
                    dr["원산지"] = atoDt.Rows[i]["origin"].ToString();
                    seaoverDt.Rows.Add(dr);
                }
                seaoverDt.AcceptChanges();
                seaoverDt = seaoverDt.DefaultView.ToTable(true);
            }

            //무역담당자
            DataTable tmDt = purchasePriceRepository.GetTradeManaer(txtProduct.Text, txtOrigin.Text, txtManager.Text.Trim());
            //조업, 계약시기 정보
            DataTable recommendDt = contractRecommendationRepository.GetRecommendGroupConcat(txtProduct.Text, txtOrigin.Text, operating.Trim(), contract.Trim());

            if (seaoverDt.Rows.Count > 0)
            {
                Color col = Color.FromArgb(221, 235, 247);
                bool isColor = false;

                //등록된 품목
                for (int i = 0; i < seaoverDt.Rows.Count; i++)
                {
                    bool isOutPut = true;
                    //월별 데이터
                    string operating_term = "";
                    string contract_term = "";
                    string operating_remark = "";
                    string contract_remark = "";
                    DataRow[] dtRow = null;
                    if (recommendDt.Rows.Count == 0 && (!string.IsNullOrEmpty(operating) || !string.IsNullOrEmpty(contract)))
                        isOutPut = false;
                    else if (recommendDt.Rows.Count > 0)
                    {
                        string whr = "product = '" + seaoverDt.Rows[i]["품명"].ToString() + "'"
                                    + " AND origin = '" + seaoverDt.Rows[i]["원산지"].ToString() + "'";
                        dtRow = recommendDt.Select(whr);
                        if (dtRow.Length == 0 && (!string.IsNullOrEmpty(operating) || !string.IsNullOrEmpty(contract)))
                            isOutPut = false;
                        else if(dtRow.Length > 0)
                        {
                            operating_term = dtRow[0]["operating"].ToString().Replace(",", "");
                            contract_term = dtRow[0]["contract"].ToString().Replace(",", "");

                            operating_remark = dtRow[0]["operating_remark"].ToString();
                            contract_remark = dtRow[0]["contract_remark"].ToString();
                        }
                    }
                    //담당자 검색
                    string manager = "";
                    if (isOutPut)
                    {
                        string whr = "product = '" + seaoverDt.Rows[i]["품명"].ToString() + "'"
                                + " AND origin = '" + seaoverDt.Rows[i]["원산지"].ToString() + "'";
                        DataRow[] dr = tmDt.Select(whr);
                        if (dr.Length == 0 && !string.IsNullOrEmpty(txtManager.Text.Trim()))
                            isOutPut = false;
                        else if (dr.Length > 0)
                            manager = dr[0]["manager"].ToString();
                    }

                    //Data출력======================================================================================
                    if (isOutPut)
                    {
                        //계약시기
                        int n = dgvProduct.Rows.Add();
                        dgvProduct.Rows[n].Cells["product"].Value = seaoverDt.Rows[i]["품명"].ToString();
                        dgvProduct.Rows[n].Cells["origin"].Value = seaoverDt.Rows[i]["원산지"].ToString();
                        dgvProduct.Rows[n].Cells["division"].Value = "계약시기";
                        dgvProduct.Rows[n].Cells["remark"].Value = contract_remark;
                        dgvProduct.Rows[n].Cells["manager"].Value = manager;
                        if (!string.IsNullOrEmpty(contract_term))
                        {
                            string[] txt = contract_term.Split('^');
                            if (txt.Length > 0)
                            {
                                for (int j = 0; j < txt.Length; j++)
                                    SetMonth(dgvProduct.Rows[n], txt[j]);
                            }
                        }
                        dgvProduct.Rows[n].Cells["division"].Style.Font = new Font("중고딕", 9, FontStyle.Bold);
                        //조업시기
                        n = dgvProduct.Rows.Add();
                        dgvProduct.Rows[n].Cells["product"].Value = seaoverDt.Rows[i]["품명"].ToString();
                        dgvProduct.Rows[n].Cells["origin"].Value = seaoverDt.Rows[i]["원산지"].ToString();
                        dgvProduct.Rows[n].Cells["division"].Value = "조업시기";
                        dgvProduct.Rows[n].Cells["remark"].Value = operating_remark;
                        dgvProduct.Rows[n].Cells["manager"].Value = manager;
                        if (!string.IsNullOrEmpty(operating_term))
                        {
                            string[] txt = operating_term.Split('^');
                            if (txt.Length > 0)
                            {
                                for (int j = 0; j < txt.Length; j++)
                                {
                                    SetMonth(dgvProduct.Rows[n], txt[j]);
                                }
                            }
                        }
                        //색배치
                        if (isColor)
                        {
                            dgvProduct.Rows[n - 1].DefaultCellStyle.BackColor = col;
                            dgvProduct.Rows[n].DefaultCellStyle.BackColor = col;
                            isColor = !isColor;
                        }
                        else
                        {
                            dgvProduct.Rows[n - 1].DefaultCellStyle.BackColor = Color.Empty;
                            dgvProduct.Rows[n].DefaultCellStyle.BackColor = Color.Empty;
                            isColor = !isColor;
                        }
                    }
                }
                    
            }
        }

        private void SetMonth(DataGridViewRow row, string txt)
        {
            if (!string.IsNullOrEmpty(txt))
            { 
                string[] tmp = txt.Trim().Split('_');
                string month = tmp[0];
                string level = tmp[1];
                string col_name = "";
                if (Convert.ToInt16(level) > 0)
                {
                    switch (month)
                    {
                        case "1":
                            col_name = "jan";
                            break;
                        case "2":
                            col_name = "feb";
                            break;
                        case "3":
                            col_name = "mar";
                            break;
                        case "4":
                            col_name = "apr";
                            break;
                        case "5":
                            col_name = "may";
                            break;
                        case "6":
                            col_name = "jun";
                            break;
                        case "7":
                            col_name = "jul";
                            break;
                        case "8":
                            col_name = "aug";
                            break;
                        case "9":
                            col_name = "sep";
                            break;
                        case "10":
                            col_name = "oct";
                            break;
                        case "11":
                            col_name = "nov";
                            break;
                        case "12":
                            col_name = "dec";
                            break;

                    }


                    if (level == "1")
                    {
                        if (row.Index % 2 == 1)
                            row.Cells[col_name].Style.BackColor = Color.LightGreen;
                        else
                            row.Cells[col_name].Style.BackColor = Color.Pink;
                    }
                    else if (level == "2")
                    {
                        if (row.Index % 2 == 1)
                            row.Cells[col_name].Style.BackColor = Color.Green;
                        else
                            row.Cells[col_name].Style.BackColor = Color.Red;
                    }
                    else if (level == "3")
                    {
                        if (row.Index % 2 == 1)
                            row.Cells[col_name].Style.BackColor = Color.DarkGreen;
                        else
                            row.Cells[col_name].Style.BackColor = Color.DarkRed;
                    }
                    row.Cells[col_name].Value = true;
                }
            }
        }


        #endregion

        #region Button
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertData();
        }
        #endregion

        #region Key event
        private void ContractRecommendationManager2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        InsertData();
                        break;
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtManager.Text = String.Empty;
                        txtContract.Text = String.Empty;
                        txtOperating.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        #endregion

        #region 우클릭 메뉴 Method
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
                    if (dgvProduct.SelectedCells.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("선택(약함)");
                        m.Items.Add("선택(보통)");
                        m.Items.Add("선택(중요)");
                        m.Items.Add("해제");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvProduct, e.Location);
                    }
                }
            }
            catch
            {
            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "선택(약함)":
                    {
                        int row = dgvProduct.SelectedCells[0].RowIndex;
                        int col = 0;
                        foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                        {
                            if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 14)
                                col = cell.ColumnIndex;
                        }

                        if (col > 0)
                        {
                            bool isChecked = Convert.ToBoolean(dgvProduct.Rows[row].Cells[col].Value);
                            foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                            {
                                if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 14)
                                {
                                    cell.Value = true;
                                    if (cell.RowIndex % 2 == 1)
                                        cell.Style.BackColor = Color.LightGreen;
                                    else
                                        cell.Style.BackColor = Color.Pink;
                                }
                            }
                        }
                        dgvProduct.EndEdit();
                    }
                    break;
                case "선택(보통)":
                    {
                        int row = dgvProduct.SelectedCells[0].RowIndex;
                        int col = 0;
                        foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                        {
                            if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 14)
                                col = cell.ColumnIndex;
                        }

                        if (col > 0)
                        {
                            bool isChecked = Convert.ToBoolean(dgvProduct.Rows[row].Cells[col].Value);
                            foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                            {
                                if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 14)
                                {
                                    cell.Value = true;
                                    if (cell.RowIndex % 2 == 1)
                                        cell.Style.BackColor = Color.Green;
                                    else
                                        cell.Style.BackColor = Color.Red;
                                }
                            }
                        }
                        dgvProduct.EndEdit();
                    }
                    break;
                case "선택(중요)":
                    {
                        int row = dgvProduct.SelectedCells[0].RowIndex;
                        int col = 0;
                        foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                        {
                            if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 14)
                                col = cell.ColumnIndex;
                        }

                        if (col > 0)
                        {
                            bool isChecked = Convert.ToBoolean(dgvProduct.Rows[row].Cells[col].Value);
                            foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                            {
                                if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 14)
                                {
                                    cell.Value = true;
                                    if (cell.RowIndex % 2 == 1)
                                        cell.Style.BackColor = Color.DarkGreen;
                                    else
                                        cell.Style.BackColor = Color.DarkRed;
                                }
                            }
                        }
                        dgvProduct.EndEdit();
                    }
                    break;
                case "해제":
                    {
                        int row = dgvProduct.SelectedCells[0].RowIndex;
                        int col = 0;
                        foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                        {
                            if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 14)
                                col = cell.ColumnIndex;
                        }

                        if (col > 0)
                        {
                            bool isChecked = Convert.ToBoolean(dgvProduct.Rows[row].Cells[col].Value);
                            foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                            {
                                if (cell.ColumnIndex >= 3 && cell.ColumnIndex <= 14)
                                {
                                    cell.Value = false;
                                    cell.Style.BackColor = Color.White;
                                }
                            }
                        }
                        dgvProduct.EndEdit();
                    }
                    break;
            }
        }
        #endregion

        #region ToolTip
        private void txtContract_MouseHover(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //Tooltip
            ToolTip toolTip = new ToolTip();
            string tooltipTxt = "검색할 월을 띄어쓰기로 구분하여 입력해주세요.";
            toolTip.SetToolTip(tb, tooltipTxt);
        }
        #endregion

        #region Merge 효과
        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
                return;

            if (e.RowIndex % 2 == 0)
                e.AdvancedBorderStyle.Top = dgvProduct.AdvancedCellBorderStyle.Top;
            else
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
        }

        private void dgvProduct_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0)
                return;


            if (e.ColumnIndex <= 1 && e.RowIndex % 2 != 0)
            {
                e.Value = "";
                e.FormattingApplied = true;
            }
        }
        #endregion

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex >= 3 && e.ColumnIndex <= 14)
                {
                    dgvProduct.EndEdit();
                    DataGridViewCell cell = dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (e.RowIndex % 2 == 1)
                    {
                        if (cell.Style.BackColor == Color.White)
                        {
                            cell.Style.BackColor = Color.LightGreen;
                            cell.Value = true;
                        }
                        else if (cell.Style.BackColor == Color.LightGreen)
                        {
                            cell.Style.BackColor = Color.Green;
                            cell.Value = true;
                        }
                        else if (cell.Style.BackColor == Color.Green)
                        {
                            cell.Style.BackColor = Color.DarkGreen;
                            cell.Value = true;
                        }
                        else if (cell.Style.BackColor == Color.DarkGreen)
                        {
                            cell.Style.BackColor = Color.White;
                            cell.Value = false;
                        }
                    }
                    else
                    {
                        if (cell.Style.BackColor == Color.White)
                        {
                            cell.Style.BackColor = Color.Pink;
                            cell.Value = true;
                        }
                        else if (cell.Style.BackColor == Color.Pink)
                        {
                            cell.Style.BackColor = Color.Red;
                            cell.Value = true;
                        }
                        else if (cell.Style.BackColor == Color.Red)
                        {
                            cell.Style.BackColor = Color.DarkRed;
                            cell.Value = true;
                        }
                        else if (cell.Style.BackColor == Color.DarkRed)
                        {
                            cell.Style.BackColor = Color.White;
                            cell.Value = false;
                        }
                    }
                    dgvProduct.ClearSelection();
                }
            }
        }
    }
}
