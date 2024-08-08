using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement.SalesManagerModule;
using Repositories;

namespace AdoNetWindow.CalendarModule
{
    public partial class UserControlDayMemo : UserControl
    {
        
        calendar cd;
        MemoModel model = new MemoModel();
        UsersModel um = new UsersModel();

        IMemoRepository memoRepository = new MemoRepository();

        public string payment_status;
        int syear, smonth, sday, sid;
        bool payBool = false;
        int mode;

        private System.Windows.Threading.DispatcherTimer accenttimer;
        private bool isBlinking = false;

        bool isAcnt = false;

        public UserControlDayMemo(int id, int days, calendar cd2, bool isAccent = false)
        {
            InitializeComponent();
            cd = cd2;
            syear = cd.year;
            smonth = cd.month;
            sday = days;
            sid = id;
            isAcnt = isAccent;
        }
        public UserControlDayMemo(MemoModel mm, int days, calendar cd2, UsersModel uModel, bool isAccent = false)
        {
            InitializeComponent();
            um = uModel;
            model = mm;
            cd = cd2;
            syear = cd.year;
            smonth = cd.month;
            sday = days;
            sid = mm.id;

            string[] bkRGB = mm.backColor.Split(' ');
            string[] ftRGB = mm.fontColor.Split(' ');
            mode = 1;

            if (bkRGB.Length > 2)
            {
                lb.BackColor = Color.FromArgb(Convert.ToInt32(bkRGB[0]), Convert.ToInt32(bkRGB[1]), Convert.ToInt32(bkRGB[2]));
            }
            if (bkRGB.Length > 2)
            {
                lb.ForeColor = Color.FromArgb(Convert.ToInt32(ftRGB[0]), Convert.ToInt32(ftRGB[1]), Convert.ToInt32(ftRGB[2]));
            }
            if (!string.IsNullOrEmpty(mm.font_size))
            {
                lb.Font = new Font(mm.font, Convert.ToInt32(mm.font_size));
            }
            if (mm.font_bold == "1")
            {
                lb.Font = new Font(lb.Font, FontStyle.Bold);
            }

            if (mm.font_italic == "1")
            {
                lb.Font = new Font(lb.Font, FontStyle.Italic);
            }

            if (mm.pay_status == "결제완료")
            {
                lb.ForeColor = Color.Gray;
                //lb.Font = new Font(lb.Font, FontStyle.Bold);
                payBool = true;
            }
            else if (mm.pay_status == "확정")
            {
                lb.ForeColor = Color.Red;
                lb.Font = new Font(lb.Font, FontStyle.Bold);
            }
            else if (mm.pay_status == "확정(마감)")
            {
                lb.ForeColor = Color.DarkRed;
                lb.Font = new Font(lb.Font, FontStyle.Bold);
                mode = 2;
            }
            payment_status = mm.pay_status;
            //강조
            isAcnt = isAccent;
        }


        private void UserControlDayMemo_Paint(object sender, PaintEventArgs e)
        {
            if (mode == 1)
            {
                Graphics g = this.CreateGraphics();
                Rectangle rect = new Rectangle(5, 6, 5, 5);
                Pen p;
                if (model.user_auth >= 50)
                {
                    p = new Pen(Color.Green, 3);
                }
                else
                {
                    p = new Pen(Color.Gray, 3);
                }

                g.DrawEllipse(p, rect);
            }
            else
            {
                lbEnd.Visible = true;
            }
            //강조
            if (isAcnt)
            {
                timer_start();
            }
        }

        private void lb_Paint(object sender, PaintEventArgs e)
        {
            //결제완료 선
            if (payBool)
            {
                e.Graphics.DrawLine(Pens.Black, new Point(0, lb.Height / 2), new Point(lb.Width, lb.Height / 2));
            }
        }

        private void UserControlDayMemo_DoubleClick(object sender, EventArgs e)
        {
            if (model.division == "보험만료일")
            {
                CompanyInsuranceManager cim = new CompanyInsuranceManager(um, cd, model.contents.Replace("보험만료일", "").Replace(" ", ""));
                cim.Show();
                cim.GetDatatable();
            }
            else
            {
                UsersMemo uMemo = new UsersMemo(syear, smonth, int.Parse(lbId.Text), cd, false, model, um);
                uMemo.StartPosition = FormStartPosition.CenterParent;
                uMemo.ShowDialog();
            }
        }

        

        private void lb_DoubleClick(object sender, EventArgs e)
        {
            if (model.division == "보험만료일")
            {
                CompanyInsuranceManager cim = new CompanyInsuranceManager(um, cd, model.contents.Replace("보험만료일", "").Replace(" ", ""));
                cim.Show();
                cim.GetDatatable();
            }
            else
            {
                UsersMemo uMomo = new UsersMemo(syear, smonth, int.Parse(lbId.Text), cd, false, model, um);
                uMomo.StartPosition = FormStartPosition.CenterParent;
                uMomo.ShowDialog();
            }
        }
        public void Memo(string str)
        {
            //내용 출력
            lbId.Text = sid.ToString();
            lb.Font = new Font("중고딕", 9, FontStyle.Regular);
            lb.Text = str;
        }

        #region 깜빡거림 효과
        private void timer_start()
        {
            this.accenttimer = new System.Windows.Threading.DispatcherTimer();
            this.accenttimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            this.accenttimer.Tick += timer_Tick;
            this.accenttimer.Start();
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
        private void lb_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DateTime dt = new DateTime(syear, smonth, sday);
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right && um.auth_level >= 50)
                {
                    this.BorderStyle = BorderStyle.Fixed3D;
                    ContextMenuStrip m = new ContextMenuStrip();
                    //공통
                    m.Items.Add("수정");
                    if (model.division != "보험만료일")
                        m.Items.Add("결제완료");
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
                    case "수정":
                        if (model.division == "보험만료일")
                        {
                            CompanyInsuranceManager cim = new CompanyInsuranceManager(um, cd, model.contents.Replace("보험만료일", "").Replace(" ", ""));
                            cim.Show();
                            cim.GetDatatable();
                        }
                        else
                        {
                            UsersMemo uMomo = new UsersMemo(syear, smonth, int.Parse(lbId.Text), cd, false, model, um);
                            uMomo.StartPosition = FormStartPosition.CenterParent;
                            uMomo.ShowDialog();
                        }
                        break;
                    case "결제완료":
                        SetCompletePayment();
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

        private void SetCompletePayment()
        {
            try
            {
                DateTime paymentDt = new DateTime(syear, smonth, sday);

                //Messagebox
                if (MessageBox.Show(this,"결제상태를 결제완료(" + paymentDt.ToString("yyyy-MM-dd") + ") 처리 하시겠습니까??", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //수정
                    int results = memoRepository.PaymentMemo(Convert.ToInt32(lbId.Text));
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
            catch
            {
                MessageBox.Show(this,"결제일자가 입력되지 않았습니다.");
            }
        }
        #endregion
    }
}
