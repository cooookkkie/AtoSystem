using AdoNetWindow.Model;
using System;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class ConferenceController : Form
    {
        UsersModel um;
        PriceComparison pc = null;
        public ConferenceController(UsersModel uModel, PriceComparison pComparison)
        {
            InitializeComponent();
            um = uModel;
            pc = pComparison;
        }

        private void ConferenceController_Load(object sender, EventArgs e)
        {
            AdvanceInitialize();

            txtExcluedSttdate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            txtExcluedEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            rbEasy.Checked = true;
        }

        #region Method
        private void AdvanceInitialize()
        {
            dgvNormalPriceAdvance.Rows.Clear();
            string advance_type = cbNormalPriceAdvanceType.Text;

            int n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "10↑";
            
            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "9";

            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "8";

            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "7";

            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "6";

            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "5";

            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "4";

            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "3";

            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "2";

            n = dgvNormalPriceAdvance.Rows.Add();
            dgvNormalPriceAdvance.Rows[n].Cells["company_count"].Value = "1";
            dgvNormalPriceAdvance.Rows[n].Cells["remark"].Value = "회전율 3개월 " + advance_type;



            if (rbRank.Checked)
            {
                if (rbEasy.Checked)
                {
                    dgvNormalPriceAdvance.Rows[0].Cells["limit_rank"].Value = 4;
                    dgvNormalPriceAdvance.Rows[1].Cells["limit_rank"].Value = 4;
                    dgvNormalPriceAdvance.Rows[2].Cells["limit_rank"].Value = 4;
                    dgvNormalPriceAdvance.Rows[3].Cells["limit_rank"].Value = 3;
                    dgvNormalPriceAdvance.Rows[4].Cells["limit_rank"].Value = 3;
                    dgvNormalPriceAdvance.Rows[5].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[6].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[7].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[8].Cells["limit_rank"].Value = 1;
                }
                else if (rbMiddle.Checked)
                {
                    dgvNormalPriceAdvance.Rows[0].Cells["limit_rank"].Value = 3;
                    dgvNormalPriceAdvance.Rows[1].Cells["limit_rank"].Value = 3;
                    dgvNormalPriceAdvance.Rows[2].Cells["limit_rank"].Value = 3;
                    dgvNormalPriceAdvance.Rows[3].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[4].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[5].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[6].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[7].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[8].Cells["limit_rank"].Value = 1;
                }
                else if (rbHard.Checked)
                {
                    dgvNormalPriceAdvance.Rows[0].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[1].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[2].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[3].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[4].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[5].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[6].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[7].Cells["limit_rank"].Value = 2;
                    dgvNormalPriceAdvance.Rows[8].Cells["limit_rank"].Value = 1;
                }

                dgvNormalPriceAdvance.Rows[0].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[1].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[2].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[3].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[4].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[5].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[6].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[7].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[8].Cells["remark"].Value = "업체 수 중에 '등수' " + advance_type;
                dgvNormalPriceAdvance.Rows[9].Cells["remark"].Value = "회전율 3개월 " + advance_type;
            }
            else
            {
                if (rbEasy.Checked)
                {
                    dgvNormalPriceAdvance.Rows[0].Cells["limit_rate"].Value = "75%";
                    dgvNormalPriceAdvance.Rows[1].Cells["limit_rate"].Value = "75%";
                    dgvNormalPriceAdvance.Rows[2].Cells["limit_rate"].Value = "75%";
                    dgvNormalPriceAdvance.Rows[3].Cells["limit_rate"].Value = "75%";
                    dgvNormalPriceAdvance.Rows[4].Cells["limit_rate"].Value = "75%";
                    dgvNormalPriceAdvance.Rows[5].Cells["limit_rate"].Value = "75%";
                    dgvNormalPriceAdvance.Rows[6].Cells["limit_rate"].Value = "66%";
                    dgvNormalPriceAdvance.Rows[7].Cells["limit_rate"].Value = "50%";
                    dgvNormalPriceAdvance.Rows[8].Cells["limit_rate"].Value = "0%";

                    dgvNormalPriceAdvance.Rows[6].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                    dgvNormalPriceAdvance.Rows[7].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                    dgvNormalPriceAdvance.Rows[8].Cells["remark"].Value = "회전율 3개월 " + advance_type;
                }
                else if (rbMiddle.Checked)
                {
                    dgvNormalPriceAdvance.Rows[0].Cells["limit_rate"].Value = "50%";
                    dgvNormalPriceAdvance.Rows[1].Cells["limit_rate"].Value = "50%";
                    dgvNormalPriceAdvance.Rows[2].Cells["limit_rate"].Value = "50%";
                    dgvNormalPriceAdvance.Rows[3].Cells["limit_rate"].Value = "50%";
                    dgvNormalPriceAdvance.Rows[4].Cells["limit_rate"].Value = "50%";
                    dgvNormalPriceAdvance.Rows[5].Cells["limit_rate"].Value = "50%";
                    dgvNormalPriceAdvance.Rows[6].Cells["limit_rate"].Value = "33%";
                    dgvNormalPriceAdvance.Rows[7].Cells["limit_rate"].Value = "0%";
                    dgvNormalPriceAdvance.Rows[8].Cells["limit_rate"].Value = "0%";

                    dgvNormalPriceAdvance.Rows[6].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                    dgvNormalPriceAdvance.Rows[7].Cells["remark"].Value = "회전율 3개월 " + advance_type;
                    dgvNormalPriceAdvance.Rows[8].Cells["remark"].Value = "회전율 3개월 " + advance_type;
                }
                else if (rbHard.Checked)
                {
                    dgvNormalPriceAdvance.Rows[0].Cells["limit_rate"].Value = "25%";
                    dgvNormalPriceAdvance.Rows[1].Cells["limit_rate"].Value = "25%";
                    dgvNormalPriceAdvance.Rows[2].Cells["limit_rate"].Value = "25%";
                    dgvNormalPriceAdvance.Rows[3].Cells["limit_rate"].Value = "25%";
                    dgvNormalPriceAdvance.Rows[4].Cells["limit_rate"].Value = "25%";
                    dgvNormalPriceAdvance.Rows[5].Cells["limit_rate"].Value = "25%";
                    dgvNormalPriceAdvance.Rows[6].Cells["limit_rate"].Value = "0%";
                    dgvNormalPriceAdvance.Rows[7].Cells["limit_rate"].Value = "0%";
                    dgvNormalPriceAdvance.Rows[8].Cells["limit_rate"].Value = "0%";

                    dgvNormalPriceAdvance.Rows[6].Cells["remark"].Value = "회전율 3개월 " + advance_type;
                    dgvNormalPriceAdvance.Rows[7].Cells["remark"].Value = "회전율 3개월 " + advance_type;
                    dgvNormalPriceAdvance.Rows[8].Cells["remark"].Value = "회전율 3개월 " + advance_type;
                }

                dgvNormalPriceAdvance.Rows[0].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                dgvNormalPriceAdvance.Rows[1].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                dgvNormalPriceAdvance.Rows[2].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                dgvNormalPriceAdvance.Rows[3].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                dgvNormalPriceAdvance.Rows[4].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                dgvNormalPriceAdvance.Rows[5].Cells["remark"].Value = "업체 수 중에 '비율' " + advance_type;
                dgvNormalPriceAdvance.Rows[9].Cells["remark"].Value = "회전율 3개월 " + advance_type;
            }
        }
        private bool SetPriceComparsionForm()
        {
            bool isFlag = false;
            if (pc != null)
            {
                isFlag = true;
            }
            else
            {
                FormCollection fc = Application.OpenForms;
                bool isFormActive = false;
                foreach (Form frm in fc)
                {
                    //iterate through
                    if (frm.Name == "PriceComparison")
                    {
                        pc = (PriceComparison)frm;
                        isFlag = true;
                        break;
                    }
                }
            }
            return isFlag;
        }
        #endregion

        #region Key event
        private void txtRank_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(txtRank.Text, out int rank) && dgvNormalPriceAdvance.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvNormalPriceAdvance.Rows.Count; i++)
                        dgvNormalPriceAdvance.Rows[i].Cells["limit_rank"].Value = rank;
                }
            }
        }
        private void ConferenceController_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnReflection.PerformClick();
                        break;
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
        #endregion

        #region Button
        private void btnReflection_Click(object sender, EventArgs e)
        {
            bool isFlag = SetPriceComparsionForm();
            if (isFlag)
            {
                pc.SetConference(this);

                /*int stockType = 0;
                if (rbAllStock.Checked)
                    stockType = 0;
                else if (rbOutOfStock.Checked)
                    stockType = 1;
                else if (rbInStock.Checked)
                    stockType = 2;


                int impendingType = 0;
                if (cbImpending.Checked)
                {
                    if (cbImpendingType.Text == "소진일자")
                        impendingType = 1;
                    else if (cbImpendingType.Text == "추천계약일")
                        impendingType = 2;
                }

                int salesCount = 0;
                if (cbSalesCount.Checked)
                {
                    if (cbSalesCountType.Text == "증가한")
                        salesCount = 1;
                    else if (cbSalesCountType.Text == "감소한")
                        salesCount = 2;
                }

                double stt_margin;
                if (!double.TryParse(txtMinPriceSttMargin.Text, out stt_margin))
                    stt_margin = 0;
                double end_margin;
                if (!double.TryParse(txtMinPriceEndMargin.Text, out end_margin))
                    end_margin = 0;

                //고급검색
                pc.SetConference(stockType, cbRoundStock.Checked, (int)nudSttRound.Value, (int)nudEndRound.Value, impendingType
                    , cbEmptyExcept.Checked, cbMinPrice.Checked, cbMinPriceType.Text, cbNormalPrice.Checked, cbNormalPriceType.Text, cbShipmentCost.Checked, cbShipmentCostType.Text, cbOfferCost.Checked, cbOfferCostType.Text
                    , salesCount
                    , cbAllUpDown.Checked, cbUp1.Checked, cbUp2.Checked, cbUp3.Checked, cbUp4.Checked, cbDown1.Checked, cbDown2.Checked, cbDown3.Checked, cbDown4.Checked, cbDown5.Checked, cbDown6.Checked
                    , cbMinPriceMarginRate.Checked, stt_margin, end_margin);


                //일반시세 고급검색
                if (cbNormalPriceAdvance.Checked)
                {

                    int normalTerms = 0;
                    if (rbNormalTerms15Days.Checked)
                        normalTerms = 15;
                    if (rbNormalTerms1Months.Checked)
                        normalTerms = 1;
                    if (rbNormalTerms45Days.Checked)
                        normalTerms = 45;
                    if (rbNormalTerms2Months.Checked)
                        normalTerms = 2;
                    if (rbAllTerms.Checked)
                        normalTerms = 100;

                    DataTable normalDt = Libs.Tools.Common.DataGridView_To_Datatable(dgvNormalPriceAdvance);

                    DateTime eSttDt, eEndDt;

                    if (!DateTime.TryParse(txtExcluedSttdate.Text, out eSttDt) || !DateTime.TryParse(txtExcluedEnddate.Text, out eEndDt))
                    {
                        MessageBox.Show(this, "단가수정일 제외기간을 다시 확인해주세요.");
                        this.Activate();
                        return;
                    }
                    if (!cbExcludeSaleTerms.Checked)
                    {
                        eSttDt = new DateTime(2050, 1, 1);
                        eEndDt = new DateTime(2050, 1, 1);
                    }


                    string month_round_txt = "";
                    if(dgvNormalPriceAdvance.Rows[dgvNormalPriceAdvance.Rows.Count - 1].Cells["remark"].Value != null)
                        month_round_txt = Regex.Replace(dgvNormalPriceAdvance.Rows[dgvNormalPriceAdvance.Rows.Count - 1].Cells["remark"].Value.ToString().Trim(), @"[^0-9]", "").ToString();
                    int month_round;
                    if (!int.TryParse(month_round_txt, out month_round))
                        month_round = 3;


                    pc.SetNoralPriceAdvance(normalTerms, normalDt, eSttDt, eEndDt, month_round);
                }*/
            }
            else
            {
                MessageBox.Show(this, "[업체별시세현황]이 활성화 되어있지 않습니다.");
                this.Activate();
            }
        }
        

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        #region Level Radio
        private void rbEasy_CheckedChanged(object sender, EventArgs e)
        {
            AdvanceInitialize();
        }

        private void rbMiddle_CheckedChanged(object sender, EventArgs e)
        {
            AdvanceInitialize();
        }

        private void rbHard_CheckedChanged(object sender, EventArgs e)
        {
            AdvanceInitialize();
        }
        private void cbNormalPriceAdvanceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdvanceInitialize();
        }

        private void rbRank_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRate.Checked)
                AdvanceInitialize();
            else
                AdvanceInitialize();
        }
        #endregion

        private void btnExcluedSttdateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtExcluedSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnExcluedEnddateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtExcluedEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }
    }
}
