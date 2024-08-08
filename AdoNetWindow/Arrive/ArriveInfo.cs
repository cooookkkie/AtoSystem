using AdoNetWindow.Model;
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

namespace AdoNetWindow.Arrive
{
    public partial class ArriveInfo : ApplicationRoot
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        IMemoArriveRepository memoArriveRepository = new MemoArriveRepository();
        CalendarModule.calendar cd;
        UsersModel um;
        ArriveModel am;
        public ArriveInfo(CalendarModule.calendar cal, UsersModel umodel, ArriveModel amodel = null)
        {
            InitializeComponent();
            cd = cal;
            um = umodel;
            am = amodel;
        }

        private void ArriveInfo_Load(object sender, EventArgs e)
        {
            GetArriveAsOne();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.MaximizeBox = false;
        }

        private void GetArriveAsOne()
        {
            List<ArriveModel> model = new List<ArriveModel>();

            //하나검색
            if (am != null)
            {
                lbOrigin.Text = am.origin.ToString();
                lbProduct.Text = am.product.ToString();
                lbSizes.Text = am.sizes.ToString();
                lbBoxweight.Text = am.box_weight.ToString();
                lbQty.Text = am.quantity_on_paper.ToString("#,##0");

                //Listbox 
                DateTime sttdate = new DateTime(Convert.ToDateTime(am.pending_date).Year, Convert.ToDateTime(am.pending_date).Month, 1, 0, 0, 0).AddDays(-7);
                DateTime enddate = new DateTime(Convert.ToDateTime(am.pending_date).Year, Convert.ToDateTime(am.pending_date).Month, 1, 0, 0, 0).AddMonths(1).AddDays(-1);
                int days = Convert.ToDateTime(am.pending_date).Day;
                int months = Convert.ToDateTime(am.pending_date).Month;

                List<ArriveModel> amodel = customsRepository.GetArriveDetails(sttdate, enddate, am.product, am.sizes, am.origin);
                if (amodel.Count > 0)
                {
                    //주말일경우 날짜 수정
                    for (int i = 0; i < amodel.Count(); i++)
                    {
                        DateTime tempDt = Convert.ToDateTime(amodel[i].pending_date);
                        int[] no;
                        string[] name;
                        getRedDay(tempDt.Year, tempDt.Month, out no, out name);

                    retry:
                        //주말일 경우 수정
                        if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                        {
                            tempDt = tempDt.AddDays(2);
                        }
                        else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                        {
                            tempDt = tempDt.AddDays(1);
                        }
                        amodel[i].pending_date = tempDt.ToString("yyyy-MM-dd");
                        getRedDay(tempDt.Year, tempDt.Month, out no, out name);

                        //공휴일일 경우
                        foreach (int n in no)
                        {
                            if (n == tempDt.Day)
                            {
                                tempDt = tempDt.AddDays(1);
                                goto retry;
                            }
                        }
                    }
                    //출력
                    for (int i = 0; i < amodel.Count; i++) 
                    {
                        if (Convert.ToDateTime(amodel[i].pending_date).Month == months && Convert.ToDateTime(amodel[i].pending_date).Day == days)
                        {
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = amodel[i].cc_status;
                            lvi.SubItems.Add(amodel[i].etd);
                            lvi.SubItems.Add(amodel[i].eta);
                            lvi.SubItems.Add(amodel[i].warehousing_date);
                            lvi.SubItems.Add(amodel[i].pending_date);
                            lvi.SubItems.Add(amodel[i].box_weight);
                            lvi.SubItems.Add(amodel[i].quantity_on_paper.ToString("#,##0"));
                            lvi.SubItems.Add(amodel[i].manager);
                            lvi.SubItems.Add(amodel[i].id.ToString());
                            lvi.SubItems.Add(amodel[i].sub_id.ToString());
                            if (amodel[i].eta_status == "미확정(etd기준)")
                            {
                                lvi.SubItems.Add("40%");
                            }
                            else if (amodel[i].eta_status == "미확정(eta기준)")
                            {
                                lvi.SubItems.Add("70%");
                            }
                            else if (amodel[i].eta_status == "미확정(창고입고일 기준)")
                            {
                                lvi.SubItems.Add("80%");
                            }
                            else if (amodel[i].eta_status == "확정")
                            {
                                lvi.SubItems.Add("90%");
                            }
                            else
                            {
                                lvi.SubItems.Add("기타");
                            }
                            scheduleList.Items.Add(lvi);

                            //메모출력
                            List<MemoArriveModel> mam = new List<MemoArriveModel>();
                            mam = memoArriveRepository.GetArriveModel(amodel[i].id, amodel[i].sub_id);
                            if (mam.Count > 0)
                            {
                                for (int j = 0; j < mam.Count; j++)
                                {
                                    ArriveMemo amc = new ArriveMemo(mam[j], this, um);
                                    this.arriveMemoPanel.Controls.Add(amc);
                                }
                            }
                        }   
                    }
                    
                    //디자인
                    foreach (ListViewItem item in  scheduleList.Items)
                    {
                        item.UseItemStyleForSubItems = false; //비활성화 해야 변경가능
                        if (item.SubItems[0].Text == "통관" || item.SubItems[0].Text == "확정")
                        {
                            item.SubItems[0].ForeColor = Color.Red;
                        }

                        item.SubItems[4].BackColor = Color.Salmon;
                        item.SubItems[4].ForeColor = Color.White;
                        item.SubItems[10].BackColor = Color.SandyBrown;
                    }
                }
            }
        }

        public void GetArriveMemo()
        {
            this.arriveMemoPanel.Controls.Clear();
            List<ArriveModel> model = new List<ArriveModel>();

            //하나검색
            if (am != null)
            {
                //Listbox 
                DateTime sttdate = new DateTime(Convert.ToDateTime(am.pending_date).Year, Convert.ToDateTime(am.pending_date).Month, 1, 0, 0, 0).AddDays(-7);
                DateTime enddate = new DateTime(Convert.ToDateTime(am.pending_date).Year, Convert.ToDateTime(am.pending_date).Month, 1, 0, 0, 0).AddMonths(1).AddDays(-1);
                int days = Convert.ToDateTime(am.pending_date).Day;
                int months = Convert.ToDateTime(am.pending_date).Month;

                List<ArriveModel> amodel = customsRepository.GetArriveDetails(sttdate, enddate, am.product, am.sizes, am.origin);
                if (amodel.Count > 0)
                {
                    //주말일경우 날짜 수정
                    for (int i = 0; i < amodel.Count(); i++)
                    {
                        DateTime tempDt = Convert.ToDateTime(amodel[i].pending_date);
                        int[] no;
                        string[] name;
                        getRedDay(tempDt.Year, tempDt.Month, out no, out name);

                    retry:
                        //주말일 경우 수정
                        if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                        {
                            tempDt = tempDt.AddDays(2);
                        }
                        else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                        {
                            tempDt = tempDt.AddDays(1);
                        }
                        amodel[i].pending_date = tempDt.ToString("yyyy-MM-dd");
                        getRedDay(tempDt.Year, tempDt.Month, out no, out name);

                        //공휴일일 경우
                        foreach (int n in no)
                        {
                            if (n == tempDt.Day)
                            {
                                tempDt = tempDt.AddDays(1);
                                goto retry;
                            }
                        }
                    }
                    //출력
                    for (int i = 0; i < amodel.Count; i++)
                    {
                        if (Convert.ToDateTime(amodel[i].pending_date).Month == months && Convert.ToDateTime(amodel[i].pending_date).Day == days)
                        {
                            //메모출력
                            List<MemoArriveModel> mam = new List<MemoArriveModel>();
                            mam = memoArriveRepository.GetArriveModel(amodel[i].id, amodel[i].sub_id);
                            if (mam.Count > 0)
                            {
                                for (int j = 0; j < mam.Count; j++)
                                {
                                    ArriveMemo amc = new ArriveMemo(mam[j], this, um);
                                    this.arriveMemoPanel.Controls.Add(amc);
                                }
                            }

                        }
                    }
                }
            }

        }


            #region 달력공휴일 계산
            //음력 공휴일계산=========================================================
            private void getRedDay(int year, int month, out int[] no, out string[] name1)
        {
            List<int> no2 = new List<int>();
            List<string> name2 = new List<string>();

            //특수 공휴일
            if (year == 2022)
            {
                switch (month)
                {
                    case 3:
                        no2.Add(9);
                        name2.Add("대통령 선거");
                        break;
                    default:
                        no = null;
                        name1 = null;
                        break;
                }
            }
            //법정공휴일
            switch (month)
            {
                case 1:
                    no2.Add(1);
                    name2.Add("신정");
                    break;
                case 3:
                    no2.Add(1);
                    name2.Add("삼일절");
                    break;
                case 5:
                    no2.Add(5);
                    name2.Add("어린이날");
                    break;
                case 6:
                    no2.Add(6);
                    name2.Add("현충일");
                    break;
                case 8:
                    no2.Add(15);
                    name2.Add("광복절");
                    break;
                case 9:
                    break;
                case 10:
                    no2.Add(3);
                    name2.Add("개천절");
                    no2.Add(9);
                    name2.Add("한글날");
                    break;
                case 12:
                    no2.Add(25);
                    name2.Add("크리스마스");
                    break;
                default:
                    no = null;
                    name1 = null;
                    break;
            }
            DateTime dt = new DateTime(year - 1, 12, 30);
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 1, 1);
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 1, 2);//설날
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 4, 8);//석가탄신일
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//석가탄신일
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("석가탄신일");
            }

            dt = new DateTime(year, 8, 14);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            dt = new DateTime(year, 8, 15);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            dt = new DateTime(year, 8, 16);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            no = no2.ToArray();
            name1 = name2.ToArray();
        }

        private DateTime convertKoreanMonth(int n음력년, int n음력월, int n음력일)//음력을 양력 변환
        {
            System.Globalization.KoreanLunisolarCalendar 음력 =
            new System.Globalization.KoreanLunisolarCalendar();

            bool b달 = 음력.IsLeapMonth(n음력년, n음력월);
            int n윤월;

            if (음력.GetMonthsInYear(n음력년) > 12)
            {
                n윤월 = 음력.GetLeapMonth(n음력년);
                if (b달)
                    n음력월++;
                if (n음력월 > n윤월)
                    n음력월++;
            }
            try
            {
                음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
            }
            catch
            {
                return 음력.ToDateTime(n음력년, n음력월, 음력.GetDaysInMonth(n음력년, n음력월), 0, 0, 0, 0);//음력은 마지막 날짜가 매달 다르기 때문에 예외 뜨면 그날 맨 마지막 날로 지정
            }

            return 음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
        }
        //음력 공휴일계산=========================================================
        #endregion



        private void btnInserMemo_Click(object sender, EventArgs e)
        {
            Insert();
        }

        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Insert();
            }
        }
        private void Insert()
        {
            MemoArriveModel model = new MemoArriveModel();
            int rowIndex = 0;
            if (scheduleList.SelectedItems.Count > 0)
            {
                rowIndex = scheduleList.SelectedItems[0].Index;
            }

            model.id = Convert.ToInt32(scheduleList.Items[rowIndex].SubItems[8].Text);
            model.sub_id = Convert.ToInt32(scheduleList.Items[rowIndex].SubItems[9].Text);
            model.content = txtContent.Text;
            model.edit_name = um.user_name.ToString();
            model.edit_date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            int results = memoArriveRepository.InsertMemo(model);
            if (results == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
            }
            else
            {
                txtContent.Text = "";
                GetArriveMemo();
            }
        }
    }
}
