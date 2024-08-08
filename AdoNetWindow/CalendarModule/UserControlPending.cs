using AdoNetWindow.Model;
using AdoNetWindow.Pending;
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
    public partial class UserControlPending : UserControl
    {
        ICustomsRepository customrepository = new CustomsRepository();

        Libs.Tools.Common common = new Libs.Tools.Common();
        calendar cd;
        UsersModel um;
        Graphics g;

        int syear, smonth;
        int mode, aDate;
        int paymentType;
        bool payBool;
        string products;
        string paymentDate;
        public string payment_status;
        private System.Windows.Threading.DispatcherTimer timer;
        private bool isBlinking = false;


        public UserControlPending(int year, int month, calendar cd2, UsersModel umodel, int accentDate = 0, string payment_date = "", int payment_type = 0)
        {
            InitializeComponent();
            cd = cd2;
            syear = year;
            smonth = month;
            paymentDate = payment_date;
            um = umodel;
            aDate = accentDate;
            paymentType = payment_type;
            lb.SendToBack();
        }

        private void UserControlPending_Load(object sender, EventArgs e)
        {
            if (aDate > 0)
            {
                timer_start();
            }
            this.MouseHover += UserControlGuarantee_MouseHover;
            lb.MouseHover += lb_MouseHover;
            lbId.MouseHover += lb_MouseHover;
            lbEnd.MouseHover += lb_MouseHover;
            btnLg.MouseHover += lb_MouseHover;


            /*ToolTip txtToolTip = new ToolTip();
            txtToolTip.ToolTipTitle = "Search guide";
            txtToolTip.IsBalloon = true;
            txtToolTip.ShowAlways = true;
            txtToolTip.UseFading = true;
            // obj.SetToolTip( 연결할control, "보이고 싶은 내용" )
            txtToolTip.SetToolTip(this, "yyyy 형식으로 넣으면 연도검색(2020), yyyymm 으로 넣으면 연월검색(202005)");*/

        }

        #region 결제완료 및 확정여부 구분(Paint event)
        private void lb_Paint(object sender, PaintEventArgs e)
        {
            //결제완료 선
            if (payBool)
            {
                e.Graphics.DrawLine(Pens.Black, new Point(0, lb.Height / 2), new Point(lb.Width, lb.Height / 2));
            }
        }

        private void UserControlPending_Paint(object sender, PaintEventArgs e)
        {
            g = this.CreateGraphics();
            Rectangle rect = new Rectangle(5, 6, 5, 5);

            if (mode == 1)
            {
                Pen p = new Pen(Color.Red, 3);
                g.DrawEllipse(p, rect);
            }
            else if (mode == 2)
            {
                Pen p = new Pen(Color.SandyBrown, 3);
                g.DrawEllipse(p, rect);
            }
            else if (mode == 3)
            {
                Pen p = new Pen(Color.Black, 3);
                g.DrawEllipse(p, rect);
            }
            else if (mode == 4)
            {
                /*Pen p = new Pen(Color.DarkRed, 3);
                g.DrawEllipse(p, rect);*/
                lbEnd.Visible = true;
            }
            else if (mode == 90)
            {
                Pen p = new Pen(Color.BlueViolet, 3);
                g.DrawEllipse(p, rect);
            }
            else if (mode == 80)
            {
                Pen p = new Pen(Color.BlueViolet, 3);
                g.DrawEllipse(p, rect);
            }
            else if (mode == 70)
            {
                Pen p = new Pen(Color.BlueViolet, 3);
                g.DrawEllipse(p, rect);
            }
        }
        #endregion

        #region 팬딩구분
        public void pending(string str, string status, int id, string income, string product, string accuaray = "")
        {
            //Status
            if (string.IsNullOrEmpty(status))
                status = "미기입";
            payment_status = status;
            if (payment_status == "미기입")
            {
                payment_status += "(" + accuaray + "%)";
            }
            //Products
            product = product.Replace("\t", "\r").Trim();
            products = product;
            //Title
            lbId.Text = id.ToString();
            lb.Text = str;
            lb.Font = new Font("중고딕", 9, FontStyle.Regular);
            //mode
            mode = 1;
            //Output
            if (status == "결제완료")
            {
                lb.ForeColor = Color.Gray;
                //lb.Font = new Font(lb.Font, FontStyle.Bold);
                payBool = true;
            }
            else if (status == "확정")
            {
                lb.ForeColor = Color.Red;
                lb.Font = new Font(lb.Font, FontStyle.Bold);
            }
            else if (status == "확정(마감)")
            {
                lb.ForeColor = Color.DarkRed;
                lb.Font = new Font(lb.Font, FontStyle.Bold);
                mode = 4;
            }
            else if (status == "확정(LG)")
            {
                lb.ForeColor = Color.Red;
                lb.Font = new Font(lb.Font, FontStyle.Bold);
                mode = 5;
            }
            else if (status == "미확정")
            {
                lb.ForeColor = Color.Purple;
            }
            else
            {
                lbId.Text = id.ToString();
                lb.Text = str;
                mode = 3;
            }
            /*else if (status == "확정(LG)")
            {
                lb.ForeColor = Color.DarkRed;
                lb.Font = new Font(lb.Font, FontStyle.Bold);
                lb.Text = "(LG)"+ lb.Text;
                mode = 4;
            }*/


            //수입구분
            if (income == "티티" || income == "TT" || income == "T/T")
            {
                btnLg.Visible = true;
                btnLg.BackgroundImage = Properties.Resources.TT_icon;
            }
            else if (income == "유산스" || income == "US" || income == "us" || income == "USANCE" || income == "usance" || income == "O")
            {
                btnLg.Visible = true;
                btnLg.BackgroundImage = Properties.Resources.USANCE_icon;
            }
            else if (income == "AT" || income == "at" || income == "엣사이트" || income == "atsight" || income == "at sight")
            {
                btnLg.Visible = true;
                btnLg.BackgroundImage = Properties.Resources.AT_icon;
            }
            else if (mode == 5)
            {
                btnLg.Visible = true;
                btnLg.BackgroundImage = Properties.Resources.lg_icon;
            }

        }

        public void pending(string str, int id, string product, int score)
        {
            //Status
            payment_status = "미통관분";
            //Products
            product = product.Replace(",", "\n,").Trim();
            products = product;
            //Title
            lbId.Text = id.ToString();
            lb.Text = str;
            //mode
            mode = score;
            if (mode == 90)
            {
                lb.ForeColor = Color.BlueViolet;
                lb.Font = new Font("중고딕", 8 , FontStyle.Bold);
            }
            else if (mode == 80)
            {
                lb.ForeColor = Color.BlueViolet;
                lb.Font = new Font("중고딕", 8, FontStyle.Regular);
            }
            else if (mode == 70)
            {
                lb.ForeColor = Color.Black;
                lb.Font = new Font("중고딕", 8, FontStyle.Regular);
            }
            else if (mode <= 60)
            {
                lb.ForeColor = Color.Black;
                lb.Font = new Font("중고딕", 8, FontStyle.Regular);
            }
        }

        public void shipment(string str, int id, string product)
        {
            product = product.Replace("\t", "\r").Trim();
            products = product;
            lbId.Text = id.ToString();
            lb.Text = str;
            lb.BackColor = Color.Beige;
            mode = 2;
        }
        #endregion

        #region Mouse hover
        private void UserControlGuarantee_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "";
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this, products);
        }

        private void lb_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "";
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(lb, products);
        }
        #endregion

        #region 내역수정 더블클릭
        private void lb_DoubleClick(object sender, EventArgs e)
        {
            int id = int.Parse(lbId.Text.ToString());
            /*if (paymentType == 0)
            {*/
                UnPendingManager view = new UnPendingManager(cd, um, "", id);
                view.StartPosition = FormStartPosition.CenterParent;
                view.ShowDialog();
            /*}
            else
            {
                Pending.AdvancePaymentManager view = new Pending.AdvancePaymentManager(cd, um, id, paymentType, paymentDate);
                view.StartPosition = FormStartPosition.CenterParent;
                view.ShowDialog();
            }*/
        }
        private void UserControlPending_DoubleClick(object sender, EventArgs e)
        {
            /*int id = int.Parse(lbId.Text.ToString());
            CalendarModule.Pending pending = new CalendarModule.Pending(id, syear, smonth, cd, um);

            pending.StartPosition = FormStartPosition.CenterParent;
            pending.ShowDialog();*/

            int id = int.Parse(lbId.Text.ToString());
            UnPendingManager upm = new UnPendingManager(cd, um, "", id);
            upm.StartPosition = FormStartPosition.CenterParent;
            upm.ShowDialog();
        }
        
        #endregion

        #region 깜빡거림 효과
        private void timer_start()
        {
            this.timer = new System.Windows.Threading.DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.timer.Tick += timer_Tick;
            this.timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (isBlinking)
            {
                this.lb.Visible = false;
            }
            else
            {
                this.lb.Visible = true;
            }
            isBlinking = !isBlinking;
        }
        #endregion

        #region 우클릭 메뉴, Method
        //우클릭 메뉴 Create
        private void btnLg_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DateTime dt;
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right && DateTime.TryParse(paymentDate, out dt) && (um.department.Contains("관리부") || um.department.Contains("전산") || um.department.Contains("경리") || um.department.Contains("무역")))
                {
                    this.BorderStyle = BorderStyle.Fixed3D;
                    ContextMenuStrip m = new ContextMenuStrip();
                    //공통
                    if (mode <= 6)
                    {
                        m.Items.Add("결제일 직접선택");
                        m.Items.Add("결제일 5일 뒤로");
                        m.Items.Add("결제일 10일 뒤로");
                        m.Items.Add("결제일 20일 뒤로");
                        m.Items.Add("결제일 30일 뒤로");
                        m.Items.Add("초기화(예상값)");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator1";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("수정");
                        m.Items.Add("결제완료");
                    }
                    else
                    {
                        m.Items.Add("통관처리");
                        m.Items.Add("통관일자 변경");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator1";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("수정");
                    }
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.Closed += new ToolStripDropDownClosedEventHandler(m_Closed);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(lb, e.Location);
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
        //우클릭 메뉴 Closiing event
        void m_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.BorderStyle = BorderStyle.None;
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "결제일 직접선택":
                        { 
                            Common.Calendar cal = new Common.Calendar();
                            string sDate = cal.GetDate(true);
                            if (sDate != null)
                            {
                                DelayPaymentDate(sDate);
                            }
                        }
                        break;
                    case "결제일 5일 뒤로":
                        DelayPaymentDate(5);
                        break;
                    case "결제일 10일 뒤로":
                        DelayPaymentDate(10);
                        break;
                    case "결제일 20일 뒤로":
                        DelayPaymentDate(20);
                        break;
                    case "결제일 30일 뒤로":
                        DelayPaymentDate(30);
                        break;
                    case "초기화(예상값)":
                        DelayPaymentDate(0);
                        break;
                    case "수정":
                        int id = int.Parse(lbId.Text.ToString());
                        UnPendingManager upm = new UnPendingManager(cd, um, "", id);
                        upm.StartPosition = FormStartPosition.CenterParent;
                        upm.ShowDialog();
                        break;
                    case "결제완료":
                        SetCompletePayment();
                        break;
                    case "통관처리":
                        ConvertToPending();
                        break;
                    case "통관일자 변경":
                        { 
                            Common.Calendar cal = new Common.Calendar();
                            string sDate = cal.GetDate(true);
                            if (sDate != null)
                            {
                                DelayPendingDate(sDate);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
            }
        }

        private void DelayPaymentDate(int delay_days)
        {
            int results;
            if (delay_days == 0)
            {
                results = customrepository.DelayPaymentDate(lbId.Text, null);
            }
            else
            {
                DateTime delay_date;
                common.GetPlusDay(Convert.ToDateTime(paymentDate), delay_days, out delay_date);

                results = customrepository.DelayPaymentDate(lbId.Text, delay_date.ToString("yyyy-MM-dd"));
            }
            //결과
            if (results == -1)
            {
                MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
            }
            else
            {
                cd.displayDays(cd.year, cd.month);
            }
        }
        private void DelayPaymentDate(string delay_date)
        {
            DateTime dt;
            if (!string.IsNullOrEmpty(delay_date) && DateTime.TryParse(delay_date, out dt))
            {
                int results = customrepository.DelayPaymentDate(lbId.Text, delay_date);
                //결과
                if (results == -1)
                {
                    MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                }
                else
                {
                    cd.displayDays(cd.year, cd.month);
                }
            }
        }
        private void ConvertToPending()
        {
            int results = customrepository.UpdateData(lbId.Text, "cc_status", "통관");
            //결과
            if (results == -1)
                MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
            else
                cd.displayWidget();
        }
        private void DelayPendingDate(string delay_date)
        {
            DateTime dt;
            if (!string.IsNullOrEmpty(delay_date) && DateTime.TryParse(delay_date, out dt))
            {
                int results = customrepository.DelayDate(lbId.Text, "pending_check", delay_date);
                //결과
                if (results == -1)
                    MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                else
                    cd.displayWidget();
            }
        }
        private void SetCompletePayment()
        {
            DateTime paymentDt;
            if (!DateTime.TryParse(paymentDate, out paymentDt))
            {
                MessageBox.Show(this,"결제일자가 입력되지 않았습니다.");
                return;
            }

            //Messagebox
            if (MessageBox.Show(this,"결제상태를 결제완료(" + paymentDate + ") 처리 하시겠습니까??", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //수정
                int results = customrepository.UpdatePayment(lbId.Text, "결제완료");
                if (results == -1)
                {
                    MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                }
                else
                {
                    cd.displayDays(cd.year, cd.month);
                }
            }
        }
        #endregion
    }
}
