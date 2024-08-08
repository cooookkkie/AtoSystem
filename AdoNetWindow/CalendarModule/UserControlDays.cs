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
using System.Windows.Threading;

namespace AdoNetWindow.CalendarModule
{
    public partial class UserControlDays : UserControl
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        calendar cd;
        int accentDate, nday, sYear, sMonth, findId;
        DateTime dt;
        int[] no1;
        string[] name1;

        UsersModel um;
        List<CustomsModel> cm1;
        List<MemoModel> mm1;
        List<CustomsModel> sm1;
        List<CustomsModel> nm1;
        List<MemoModel> smm1;
        List<ArriveModel> am1;
        List<InspectionModel> im1;
        DataTable gDt1;

        List<string> fDiv = null;
        List<DateTime> fDate = null;
        List<int> fId = null;
        int fIdx = -1;

        public UserControlDays()
        {
            InitializeComponent();
        }
        public UserControlDays(calendar cd2)
        {
            InitializeComponent();
            cd = cd2;
        }
        public void days(UsersModel umodel, calendar cd2, int numdays, int year, int month, int[] no, string[] name
            , List<CustomsModel> cm, List<MemoModel> mm, List<CustomsModel> sm, List<CustomsModel> nm, List<MemoModel> smm, DataTable alarmDt, List<ArriveModel> am, List<InspectionModel> im, DataTable gDt = null
            , List<string> findDiv = null, List<DateTime> findDate = null, List<int> findId = null, int findIdx = -1, bool overDays = false)
        {
            um = umodel;
            cd = cd2;
            cm1 = cm;
            mm1 = mm;
            sm1 = sm;
            nm1 = nm;
            smm1 = smm;
            am1 = am;
            im1 = im;
            gDt1 = gDt;

            no1 = no;
            name1 = name;
            sYear = year;
            sMonth = month;
            nday = numdays;
            DateTime dt = new DateTime(year, month, numdays);
            //주말일 경우 칸을 작게하기
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
            {
                this.Width = 119;
                this.pPending.Width = 117;
            }
            else
            {
                this.Width = 259;
                this.pPending.Width = 257;
            }

            //주말확인
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
            {
                lbdays.ForeColor = Color.Red;
            }
            //공휴일 확인
            for (int i = 0; i < no.Length; i++)
            {
                try
                {
                    DateTime dt2 = new DateTime(year, month, no[i]);

                    if (numdays == no[i])
                    {
                        lbdays.ForeColor = Color.Red;
                        lbname.ForeColor = Color.Red;
                        lbname.Text = name[i];
                    }
                }
                catch
                { }
            }
            //일자출력
            lbdays.Text = numdays + "";
            //같은 주에 담음달 내역이면 월을 추가
            if (overDays)
                lbdays.Text = month + "/" + lbdays.Text;

            //총 결제금액(Krw, Usd)
            double krw = 0;
            double usd = 0;
            //결제내역
            if (cm != null)
            {
                for (int i = 0; i < cm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(cm[i].payment_date);

                    if (cm[i].ato_no.Length > 0)
                    {
                        if (tempDt.Day == numdays && tempDt.Month == month
                            && cm[i].ato_no.Substring(0, 2) != "ad" && cm[i].ato_no.Substring(0, 2) != "AD"
                            && cm[i].ato_no.Substring(0, 2) != "dw" && cm[i].ato_no.Substring(0, 2) != "DW"
                            && cm[i].ato_no.Substring(0, 2) != "jd" && cm[i].ato_no.Substring(0, 2) != "JD"
                            && cm[i].ato_no.Substring(0, 2) != "dl" && cm[i].ato_no.Substring(0, 2) != "DL"
                            && cm[i].ato_no.Substring(0, 2) != "hs" && cm[i].ato_no.Substring(0, 2) != "HS"
                            && cm[i].ato_no.Substring(0, 2) != "od" && cm[i].ato_no.Substring(0, 2) != "OD")
                        {
                            if (cm[i].payment_date_status != "결제완료")
                            {
                                usd = usd + Convert.ToDouble(cm[i].total_amount);
                            }
                        }
                    }
                }
            }
            //기타결제 내역(무역)
            if (mm != null)
            {
                for (int i = 0; i < mm.Count(); i++)
                {
                    DateTime tempDt = new DateTime(mm[i].syear, mm[i].smonth, mm[i].sday);
                    if (tempDt.Day == nday && tempDt.Month == sMonth)
                    {
                        if (mm[i].pay_status != "결제완료")
                        {
                            if (mm[i].currency == "KRW")
                            {
                                krw = krw + Convert.ToDouble(mm[i].pay_amount);
                            }
                            else if (mm[i].currency == "USD")
                            {
                                usd = usd + Convert.ToDouble(mm[i].pay_amount);
                            }
                        }
                    }
                }
            }
            
            //오늘 +3영얼일
            DateTime threeOverDay;
            common.GetPlusDay(DateTime.Now, 3, out threeOverDay);
            //미확인 내역
            if (nm != null)
            {
                for (int i = 0; i < nm.Count(); i++)
                {
                    /*DateTime tempDt = new DateTime();
                    if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) > Convert.ToDateTime(nm[i].payment_date))
                    {
                        tempDt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        tempDt = tempDt.AddDays(3);
                    }
                    else
                    {
                        tempDt = Convert.ToDateTime(nm[i].payment_date);
                    }*/

                    DateTime tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(nm[i].payment_date);
                    if (tempDt.Day == numdays && tempDt.Month == month)
                    {
                        if (tempDt.Day == numdays
                            && nm[i].ato_no.Substring(0, 2) != "ad" && nm[i].ato_no.Substring(0, 2) != "AD"
                            && nm[i].ato_no.Substring(0, 2) != "dw" && nm[i].ato_no.Substring(0, 2) != "DW"
                            && nm[i].ato_no.Substring(0, 2) != "jd" && nm[i].ato_no.Substring(0, 2) != "JD"
                            && nm[i].ato_no.Substring(0, 2) != "dl" && nm[i].ato_no.Substring(0, 2) != "DL"
                            && nm[i].ato_no.Substring(0, 2) != "hs" && nm[i].ato_no.Substring(0, 2) != "HS"
                            && nm[i].ato_no.Substring(0, 2) != "od" && nm[i].ato_no.Substring(0, 2) != "OD")
                        {
                            usd = usd + Convert.ToDouble(nm[i].total_amount);
                        }
                    }
                }
            }

            //엮을 품목
            if (gDt != null)
            {
                for (int i = 0; i < gDt.Rows.Count; i++)
                {
                    DateTime tempDt = new DateTime();
                    if (DateTime.TryParse(gDt.Rows[i]["guarantee_date"].ToString(), out tempDt))
                    {
                        string atoNo = gDt.Rows[i]["ato_no"].ToString();
                        if (tempDt.Year == year && tempDt.Month == month && tempDt.Day == numdays && tempDt >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            if (atoNo.Substring(0, 2) != "ad" && atoNo.Substring(0, 2) != "AD"
                                && atoNo.Substring(0, 2) != "dw" && atoNo.Substring(0, 2) != "DW"
                                && atoNo.Substring(0, 2) != "jd" && atoNo.Substring(0, 2) != "JD"
                                && atoNo.Substring(0, 2) != "dl" && atoNo.Substring(0, 2) != "DL"
                                && atoNo.Substring(0, 2) != "hs" && atoNo.Substring(0, 2) != "HS"
                                && atoNo.Substring(0, 2) != "od" && atoNo.Substring(0, 2) != "OD")
                            {
                                krw = krw + (int)(Convert.ToDouble(gDt.Rows[i]["total_amount"].ToString()) * 0.7);
                            }
                        }
                    }
                }
            }
            //========================================================================================
            // TOTAL 결제금액 출력
            if (usd > 0)
            {
                lbPay.Text = "$" + usd.ToString("#,##0.00");
            }

            if (krw > 0)
            {
                if (lbPay.Text == "")
                {
                    lbPay.Text = @"\" + krw.ToString("#,##0");
                }
                else
                {
                    lbPay.Text = lbPay.Text + ", " + @"\" + krw.ToString("#,##0");
                }
            }

            //강조일자
            fDiv = findDiv;
            fDate = findDate;
            fId = findId;
            fIdx = findIdx;

            nday = numdays;
            
            //세부일정 출력
            pendingList(year, month, numdays, cm, mm, sm, nm, smm, alarmDt, am, im, gDt);
        }

        public void pendingList(int year, int month, int days, List<CustomsModel> cm, List<MemoModel> mm, List<CustomsModel> sm, List<CustomsModel> nm, List<MemoModel> smm, DataTable alarmDt, List<ArriveModel> am, List<InspectionModel> im, DataTable gDt)
        {
            //속도개선(끄기)
            this.pPending.SuspendLayout();
            DateTime dt = new DateTime(year, month, days);

            //pending(결제) 내용
            if (cm != null)
            {
                for (int i = 0; i < cm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(cm[i].payment_date);

                    //강제변환
                    if (cm[i].ato_no == "BF02")
                    {
                        tempDt = Convert.ToDateTime(cm[i].payment_date);
                    }
                    //결제내역
                    if (cm[i].ato_no.Length > 0)
                    { 
                        if(tempDt.Day == days && tempDt.Month == month
                        && cm[i].ato_no.Substring(0, 2) != "ad" && cm[i].ato_no.Substring(0, 2) != "AD"
                        && cm[i].ato_no.Substring(0, 2) != "dw" && cm[i].ato_no.Substring(0, 2) != "DW"
                        && cm[i].ato_no.Substring(0, 2) != "jd" && cm[i].ato_no.Substring(0, 2) != "JD"
                        && cm[i].ato_no.Substring(0, 2) != "dl" && cm[i].ato_no.Substring(0, 2) != "DL"
                        && cm[i].ato_no.Substring(0, 2) != "hs" && cm[i].ato_no.Substring(0, 2) != "HS"
                        && cm[i].ato_no.Substring(0, 2) != "od" && cm[i].ato_no.Substring(0, 2) != "OD") 
                        {
                            string str = "$" + cm[i].total_amount.ToString("#,##0.00") + "  " + cm[i].ato_no + "  " + cm[i].payment_bank;
                            //구분추가
                            if (cm[i].division != null && cm[i].division != "")
                            {
                                str += "[" + cm[i].division.Trim() + "]";
                            }

                            UserControlPending ucPending;
                            //강조(깜빡임)
                            if (fIdx >= 0)
                            {
                                if(fDiv[fIdx] == "custom" && fDate[fIdx] == tempDt)
                                    ucPending = new UserControlPending(cd.year, cd.month, cd, um, fDate[fIdx].Day, cm[i].payment_date, Convert.ToInt16(cm[i].payment_type));
                                else
                                    ucPending = new UserControlPending(cd.year, cd.month, cd, um, 0, cm[i].payment_date, Convert.ToInt16(cm[i].payment_type));
                            }
                            //일반
                            else
                            {
                                ucPending = new UserControlPending(cd.year, cd.month, cd, um, 0, cm[i].payment_date, Convert.ToInt16(cm[i].payment_type));
                            }

                            //Control 추가
                            ucPending.pending(str, cm[i].payment_date_status, cm[i].id, cm[i].usance_type, cm[i].product);
                            pPending.Controls.Add(ucPending);

                        }
                    }
                }
            }
            //memo 내용(기타결제, 무역부)
            if (mm != null)
            {
                for (int i = 0; i < mm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    tempDt = new DateTime(mm[i].syear, mm[i].smonth, mm[i].sday);
                    if (tempDt.Day == nday && tempDt.Month == month)
                    {

                        string str = mm[i].contents;
                        string temp = "";
                        if (mm[i].pay_amount > 0)
                        {
                            if (mm[i].currency == "USD")
                            {
                                temp = "$" + string.Format("{0:#,##0.00}", mm[i].pay_amount) + " " + mm[i].pay_bank;
                            }
                            else
                            {
                                temp = @"\" + string.Format("{0:#,##0}", mm[i].pay_amount) + " " + mm[i].pay_bank;
                            }
                        }
                        temp = temp.Trim();
                        if (temp != "")
                        {
                            str = "(" + temp + ")" + str;
                        }
                        UserControlDayMemo ucMemo;
                        //강조(깜빡임)
                        if (fIdx >= 0)
                        {
                            if (fDiv[fIdx] == "memo" && fDate[fIdx] == tempDt)
                                ucMemo = new UserControlDayMemo(mm[i], days, cd, um, true);
                            else
                                ucMemo = new UserControlDayMemo(mm[i], days, cd, um);
                        }
                        //일반
                        else
                            ucMemo = new UserControlDayMemo(mm[i], days, cd, um);

                        ucMemo.Memo(str);
                        pPending.Controls.Add(ucMemo);
                    }
                }
            }
            
            //Unknown(미확인 결제) 내용
            if (nm != null)
            {
                for (int i = 0; i < nm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(nm[i].payment_date);

                    if (tempDt.Day == days && tempDt.Month == month
                        && nm[i].ato_no.Substring(0, 2) != "ad" && nm[i].ato_no.Substring(0, 2) != "AD"
                        && nm[i].ato_no.Substring(0, 2) != "dw" && nm[i].ato_no.Substring(0, 2) != "DW"
                        && nm[i].ato_no.Substring(0, 2) != "jd" && nm[i].ato_no.Substring(0, 2) != "JD"
                        && nm[i].ato_no.Substring(0, 2) != "dl" && nm[i].ato_no.Substring(0, 2) != "DL"
                        && nm[i].ato_no.Substring(0, 2) != "hs" && nm[i].ato_no.Substring(0, 2) != "HS"
                        && nm[i].ato_no.Substring(0, 2) != "od" && nm[i].ato_no.Substring(0, 2) != "OD")
                    {
                        string str = "$" + nm[i].total_amount.ToString("#,##0.00") + "  " + nm[i].ato_no + "  " + nm[i].payment_bank + "(" + nm[i].accuracy + "%)";
                        UserControlPending ucPending = new UserControlPending(cd.year, cd.month, cd, um, 0, nm[i].payment_date);
                        //강조(깜빡임)
                        if (accentDate == days && nm[i].id == findId)
                        {
                            ucPending = new UserControlPending(cd.year, cd.month, cd, um, accentDate, nm[i].payment_date);
                        }
                        //일반
                        else
                        {
                            ucPending = new UserControlPending(cd.year, cd.month, cd, um, 0, nm[i].payment_date);
                        }
                        //정확도 수정
                        switch (nm[i].accuracy)
                        {
                            case "50":
                                nm[i].payment_date_status = "";
                                break;
                            case "60":
                                nm[i].payment_date_status = "";
                                break;
                            case "70":
                                nm[i].payment_date_status = "";
                                break;
                            case "80":
                                nm[i].payment_date_status = "미확정";
                                break;
                        }
                        //Control 추가
                        ucPending.pending(str, nm[i].payment_date_status, nm[i].id, nm[i].usance_type, nm[i].product, nm[i].accuracy);
                        pPending.Controls.Add(ucPending);
                    }
                }
            }

            //memo 내용(영업부)
            if (smm != null)
            {
                for (int i = 0; i < smm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    tempDt = new DateTime(smm[i].syear, smm[i].smonth, smm[i].sday);
                    if (tempDt.Day == nday && tempDt.Month == month)
                    {

                        string str = smm[i].contents;
                        string temp = str;

                        str = smm[i].contents;
                        temp = str;
                        UserControlDayMemo ucMemo = new UserControlDayMemo(smm[i], days, cd, um);

                        ucMemo.Memo(str);
                        pPending.Controls.Add(ucMemo);
                    }
                }
            }
            //영업알람(영업부)
            if (alarmDt != null && alarmDt.Rows.Count > 0)
            {
                for (int i = 0; i < alarmDt.Rows.Count; i++)
                {
                    if (DateTime.TryParse(alarmDt.Rows[i]["alarm_date"].ToString(), out DateTime tempDt))
                    {
                        if (tempDt.Day == nday && tempDt.Month == month)
                        {
                            /*UserControlDayMemo ucMemo = new UserControlDayMemo(mm[i].id, days , cd);*/
                            UserControlSalesAlarm ucAlarm = new UserControlSalesAlarm(alarmDt.Rows[i], days, cd, um);
                            pPending.Controls.Add(ucAlarm);
                        }
                    }
                }
            }

            //Shipment 내용
            if (sm != null)
            {
                for (int i = 0; i < sm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    if (DateTime.TryParse(sm[i].etd, out tempDt))
                    {
                        if (DateTime.Now.Day <= nday && tempDt.Day == days && tempDt.Month == month)
                        {
                            string str = sm[i].ato_no + "  " + sm[i].manager;
                            UserControlPending ucPending;

                            //강조(깜빡임)
                            if (accentDate == days && sm[i].id == findId)
                            {
                                ucPending = new UserControlPending(cd.year, cd.month, cd, um, accentDate, sm[i].payment_date);
                            }
                            //일반
                            else
                            {
                                ucPending = new UserControlPending(cd.year, cd.month, cd, um, 0, sm[i].payment_date);
                            }


                            ucPending.shipment(str, sm[i].id, sm[i].product);
                            pPending.Controls.Add(ucPending);
                        }
                    }
                }
            }

            //통관 예정
            if (am != null)
            {
                DateTime tempDt;
                //주말 및 공휴일 경우 일자수정
                if (lbdays.ForeColor == Color.Red)
                {
                    for (int i = 0; i < am.Count(); i++)
                    {
                        tempDt = Convert.ToDateTime(am[i].pending_date);

                        if (tempDt.Day == days && tempDt.Month == month)
                        {
                            tempDt = Convert.ToDateTime(am[i].pending_date.ToString()).AddDays(1);
                            am[i].pending_date = tempDt.ToString();
                        }
                    }
                }
                
                //출력
                for (int i = 0; i < am.Count(); i++)
                {
                    tempDt = Convert.ToDateTime(am[i].pending_date);

                    if (tempDt.Day == days && tempDt.Month == month)
                    {
                        UserControlArrive ucArrive = new UserControlArrive(am[i], days, cd, um);
                        /*ucPending.unknown(str, nm[i].payment_date_status, nm[i].id);*/
                        pPending.Controls.Add(ucArrive);
                    }
                }
            }

            //검품 예정
            if (im != null)
            {
                DateTime tempDt;

                for (int i = 0; i < im.Count(); i++)
                {
                    tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(im[i].warehousing_date);

                    if (tempDt.Day == days && tempDt.Month == month)
                    {
                        UserControlInspection ucInspection = new UserControlInspection(im[i], days, cd, um);
                        /*ucPending.unknown(str, nm[i].payment_date_status, nm[i].id);*/
                        pPending.Controls.Add(ucInspection);
                    }
                }
            }

            //엮을 예정
            if (gDt != null)
            {
                DateTime tempDt;

                for (int i = 0; i < gDt.Rows.Count; i++)
                {
                    if (DateTime.TryParse(gDt.Rows[i]["guarantee_date"].ToString(), out tempDt))
                    { 
                        if (tempDt.Year == year && tempDt.Month == month && tempDt.Day == days && tempDt >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            DataRow dr = gDt.Rows[i];
                            UserControlGuarantee ucg = new UserControlGuarantee(cd, um, dr);
                            pPending.Controls.Add(ucg);
                        }
                    }
                }
            }
            //배치순서 변경
            SetOrderByControls();
            pPending.Invalidate();

            //속도개선(켜기)
            this.pPending.ResumeLayout();
        }

        private void SetOrderByControls()
        {
            //미기입
            foreach (Control con in pPending.Controls)
            {
                switch (con.Name)
                {
                    case "UserControlPending":
                        break;
                    case "UserControlDayMemo":
                        break;
                    default:
                        pPending.Controls.SetChildIndex(con, 0);
                        break;
                }
            }
            //미기입
            foreach (Control con in pPending.Controls)
            {
                switch (con.Name)
                {
                    case "UserControlPending":

                        UserControlPending pending = (UserControlPending)con;
                        if (pending.payment_status == "미기입(50%)")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                }
            }
            //미기입
            foreach (Control con in pPending.Controls)
            {
                switch (con.Name)
                {
                    case "UserControlPending":

                        UserControlPending pending = (UserControlPending)con;
                        if (pending.payment_status == "미기입(60%)")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                }
            }
            //미기입
            foreach (Control con in pPending.Controls)
            {
                switch (con.Name)
                {
                    case "UserControlPending":

                        UserControlPending pending = (UserControlPending)con;
                        if (pending.payment_status == "미기입(70%)")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                }
            }
            //미확정
            foreach (Control con in pPending.Controls)
            {
                switch (con.Name)
                {
                    case "UserControlPending":

                        UserControlPending pending = (UserControlPending)con;
                        if (pending.payment_status == "미확정")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                    case "UserControlDayMemo":

                        UserControlDayMemo memo = (UserControlDayMemo)con;
                        if (memo.payment_status == "미확정")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                }
            }
            //확정
            foreach (Control con in pPending.Controls)
            {
                switch (con.Name)
                {
                    case "UserControlPending":

                        UserControlPending pending = (UserControlPending)con;
                        if (pending.payment_status == "확정" || pending.payment_status == "확정(LG)")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                    case "UserControlDayMemo":

                        UserControlDayMemo memo = (UserControlDayMemo)con;
                        if (memo.payment_status == "확정" || memo.payment_status == "확정(LG)")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                }
            }
            //확정(마감)
            foreach (Control con in pPending.Controls)
            {
                switch (con.Name)
                {
                    case "UserControlPending":

                        UserControlPending pending = (UserControlPending)con;
                        if (pending.payment_status == "확정(마감)")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                    case "UserControlDayMemo":

                        UserControlDayMemo memo = (UserControlDayMemo)con;
                        if (memo.payment_status == "확정(마감)")
                        {
                            pPending.Controls.SetChildIndex(con, 0);
                        }
                        break;
                }
            }
            

        }

        //확대
        private void lbdays_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ReadingDay rd = new ReadingDay(cd, um, nday, sYear, sMonth, no1, name1, cm1, mm1, sm1, nm1, smm1, am1, im1, gDt1);
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            rd.StartPosition = FormStartPosition.Manual;
            rd.Location = new Point(X, Y);
            rd.ShowDialog();
        }
        //기타결제 추가
        private void lbAdd_Click(object sender, EventArgs e)
        {
            if (lbdays.Text.Contains('/'))
            {
                string[] txt = lbdays.Text.Split('/');
                if (txt.Length > 0)
                {
                    int days = Convert.ToInt16(txt[1]);
                    DateTime dt = new DateTime(cd.year, cd.month, 1).AddMonths(1);

                    UsersMemo uModel = new UsersMemo(dt.Year, dt.Month, days, 0, cd, true, um);
                    uModel.StartPosition = FormStartPosition.CenterParent;
                    uModel.ShowDialog();

                    cd.displayMemo(cd.year, cd.month);
                }
            }
            else
            {
                int sday = int.Parse(lbdays.Text);
                UsersMemo uModel = new UsersMemo(cd.year, cd.month, sday, 0, cd, true, um);
                uModel.StartPosition = FormStartPosition.CenterParent;
                uModel.ShowDialog();

                cd.displayMemo(cd.year, cd.month);
            }
        }


        private void UserControlDays_Paint(object sender, PaintEventArgs e)
        {
            //강조테두리
            if (accentDate > 0 && accentDate == nday)
            {
                Graphics g = this.CreateGraphics();
                Pen p = new Pen(Color.Blue, 2);

                Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
                g.DrawRectangle(p, rec);

                lbdays.BackColor = Color.Blue;
                lbdays.ForeColor = Color.White;
            }
        }



    }
}
