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

namespace AdoNetWindow.CalendarModule
{
    public partial class ReadingDay : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        calendar cd;
        UsersModel um;
        int nday;
        int sMonth;
        int sYear;
        DateTime dt;

        public ReadingDay(calendar cd1, UsersModel umodel, int numdays, int year, int month, int[] no, string[] name1
            , List<CustomsModel> cm, List<MemoModel> mm, List<CustomsModel> sm, List<CustomsModel> nm, List<MemoModel> smm, List<ArriveModel> am, List<InspectionModel> im, DataTable gDt)
        {
            InitializeComponent();
            cd = cd1;
            nday = numdays;
            sMonth = month;
            sYear = year;
            um = umodel;
            DateTime dt = new DateTime(year, month, numdays);

            //주말확인
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
            {
                lbdays.ForeColor = Color.Red;
            }
            //공휴일 확인
            for (int i = 0; i < no.Length; i++)
            {

                DateTime dt2 = new DateTime(year, month, no[i]);

                if (numdays == no[i])
                {
                    lbdays.ForeColor = Color.Red;
                    lbname.ForeColor = Color.Red;
                    lbname.Text = name1[i];
                }
            }
            //일자출력
            lbdays.Text = numdays + "";
            //총 결제내역(krw, usd)
            int krw = 0;
            double usd = 0;
            //결제내역
            if (cm != null)
            {
                for (int i = 0; i < cm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(cm[i].payment_date);

                    if (tempDt.Day == numdays && tempDt.Month == month)
                    {
                        if (cm[i].payment_date_status != "결제완료"
                            && cm[i].ato_no.Substring(0, 2) != "ad" && cm[i].ato_no.Substring(0, 2) != "AD"
                            && cm[i].ato_no.Substring(0, 2) != "dw" && cm[i].ato_no.Substring(0, 2) != "DW"
                            && cm[i].ato_no.Substring(0, 2) != "jd" && cm[i].ato_no.Substring(0, 2) != "JD"
                            && cm[i].ato_no.Substring(0, 2) != "dl" && cm[i].ato_no.Substring(0, 2) != "DL"
                            && cm[i].ato_no.Substring(0, 2) != "hs" && cm[i].ato_no.Substring(0, 2) != "HS"
                            && cm[i].ato_no.Substring(0, 2) != "od" && cm[i].ato_no.Substring(0, 2) != "OD")
                        {
                            usd = usd + Convert.ToDouble(cm[i].total_amount);
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
                    if (tempDt.Day == numdays && tempDt.Month == month)
                    {
                        if (mm[i].pay_status != "결제완료")
                        {
                            if (mm[i].currency == "KRW")
                            {
                                krw = krw + Convert.ToInt32(mm[i].pay_amount);
                            }
                            else if (mm[i].currency == "USD")
                            {
                                usd = usd + Convert.ToDouble(mm[i].pay_amount);
                            }
                        }
                    }
                }
            }
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
                    if (tempDt.Day == numdays && tempDt.Month == month && tempDt.Day == numdays)
                    {
                        if (nm[i].payment_date_status != "결제완료"
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
            //세부일정 출력
            pendingList(year, month, numdays, cm, mm, sm, nm, smm, am, im, gDt);
        }

        #region Method
        public void pendingList(int year, int month, int days, List<CustomsModel> cm, List<MemoModel> mm, List<CustomsModel> sm, List<CustomsModel> nm, List<MemoModel> smm, List<ArriveModel> am, List<InspectionModel> im, DataTable gDt)
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
                    if (cm[i].ato_no == "BF02")
                    {
                        tempDt = Convert.ToDateTime(cm[i].payment_date);
                    }
                    if (cm[i].ato_no.Length > 0)
                    {
                        if (tempDt.Day == days && tempDt.Month == month
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

                            UserControlPending ucPending = new UserControlPending(cd.year, cd.month, cd, um, 0, cm[i].payment_date);

                            //Control 추가
                            ucPending.pending(str, cm[i].payment_date_status, cm[i].id, cm[i].usance_type, cm[i].product);
                            ucPending.Width = 500;
                            ucPending.Height = 20;
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
                    DateTime tempDt = new DateTime(mm[i].syear, mm[i].smonth, mm[i].sday);
                    if (tempDt.Day == nday && tempDt.Month == sMonth)
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
                        UserControlDayMemo ucMemo = new UserControlDayMemo(mm[i], days, cd, um);
                        ucMemo.Memo(str);
                        ucMemo.Width = 500;
                        ucMemo.Height = 20;
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
                        ucPending.Width = 500;
                        ucPending.Height = 20;
                        pPending.Controls.Add(ucPending);
                    }
                }
            }

            //memo 내용(영업부)
            if (smm != null)
            {
                for (int i = 0; i < smm.Count(); i++)
                {
                    DateTime tempDt = new DateTime(smm[i].syear, smm[i].smonth, smm[i].sday);
                    if (tempDt.Day == nday && tempDt.Month == sMonth)
                    {

                        string str = smm[i].contents;
                        string temp = str;

                        /*UserControlDayMemo ucMemo = new UserControlDayMemo(mm[i].id, days , cd);*/
                        UserControlDayMemo ucMemo = new UserControlDayMemo(smm[i], days, cd, um);
                        ucMemo.Memo(str);
                        ucMemo.Width = 500;
                        ucMemo.Height = 20;
                        pPending.Controls.Add(ucMemo);
                    }
                }
            }
            //Shipment 내용
            if (sm != null)
            {
                for (int i = 0; i < sm.Count(); i++)
                {
                    DateTime tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(sm[i].etd);

                    if (DateTime.Now.Day <= nday && tempDt.Day == days && tempDt.Month == month
                        && sm[i].ato_no.Substring(0, 2) != "ad" && sm[i].ato_no.Substring(0, 2) != "AD"
                        && sm[i].ato_no.Substring(0, 2) != "dw" && sm[i].ato_no.Substring(0, 2) != "DW"
                        && sm[i].ato_no.Substring(0, 2) != "jd" && sm[i].ato_no.Substring(0, 2) != "JD"
                        && sm[i].ato_no.Substring(0, 2) != "dl" && sm[i].ato_no.Substring(0, 2) != "DL"
                        && sm[i].ato_no.Substring(0, 2) != "hs" && sm[i].ato_no.Substring(0, 2) != "HS"
                        && sm[i].ato_no.Substring(0, 2) != "od" && sm[i].ato_no.Substring(0, 2) != "OD")
                    {
                        string str = sm[i].ato_no + "  " + sm[i].manager;
                        UserControlPending ucPending = new UserControlPending(cd.year, cd.month, cd, um, 0, sm[i].payment_date);
                        ucPending.shipment(str, sm[i].id, sm[i].product);
                        ucPending.Width = 500;
                        ucPending.Height = 20;
                        pPending.Controls.Add(ucPending);
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


                for (int i = 0; i < am.Count(); i++)
                {
                    tempDt = new DateTime();
                    tempDt = Convert.ToDateTime(am[i].pending_date);

                    if (tempDt.Day == days && tempDt.Month == month)
                    {
                        UserControlArrive ucArrive = new UserControlArrive(am[i], days, cd, um);
                        /*ucPending.unknown(str, nm[i].payment_date_status, nm[i].id);*/
                        ucArrive.Width = 500;
                        ucArrive.Height = 20;
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
                        ucInspection.Width = 500;
                        ucInspection.Height = 20;
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
                            ucg.Width = 500;
                            ucg.Height = 20;
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

        #endregion

        #region Click event
        private void lbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbAdd_Click(object sender, EventArgs e)
        {
            int sday = int.Parse(lbdays.Text);

            UsersMemo uModel = new UsersMemo(cd.year, cd.month, sday, 0, cd, true, um);
            uModel.StartPosition = FormStartPosition.CenterParent;
            uModel.Text = lbdays.Text + "일 계획";
            uModel.ShowDialog();
            cd.displayMemo(cd.year, cd.month);
            this.Dispose();
        }
        #endregion

        #region Key down event
        private void ReadingDay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == (Keys.Control | Keys.Shift))
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Bitmap bt = common.ScreenCaptureForm(new Point(this.Left, this.Top), this.Size);
                        Clipboard.SetImage(bt);
                        break;
                } 
            }
        }
        #endregion
    }
}
