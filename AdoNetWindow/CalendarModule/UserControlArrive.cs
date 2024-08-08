using AdoNetWindow.DashboardForSales;
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
    public partial class UserControlArrive : UserControl
    {
        ICustomsRepository customrepository = new CustomsRepository();  
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        ArriveModel am;
        UsersModel um;
        calendar cd;
        int days;
        string cc_status;

        public UserControlArrive(ArriveModel arriveModel, int num_day, calendar cal, UsersModel userModel)
        {
            InitializeComponent();
            am = arriveModel;
            um = userModel;
            cd = cal;
            days = num_day;
        }

        private void UserControlArrive_Load(object sender, EventArgs e)
        {
            SetLabel(am);
        }

        public void SetLabel(ArriveModel am)
        {
            lbId.Text = am.id.ToString();
            lbSubid.Text = am.sub_id.ToString();
            cc_status = am.cc_status;

            lb.Text = "(" + am.origin + ")" + am.product + "_" + am.sizes + "_" + am.box_weight + " [" + am.quantity_on_paper.ToString("#,##0") + "]";
            lb.Font = new Font("중고딕", 8, FontStyle.Regular);
           
            if (am.cc_status == "확정" || am.cc_status == "통관")
            {
                lb.ForeColor = Color.LightGray;
                lb.Font = new Font(lb.Font, FontStyle.Regular);
            }
            else
            {
                if (am.score == 90)
                {
                    lb.ForeColor = Color.BlueViolet;
                    lb.Font = new Font(lb.Font, FontStyle.Bold);
                }
                else if (am.score == 80)
                {
                    lb.ForeColor = Color.BlueViolet;
                    lb.Font = new Font(lb.Font, FontStyle.Regular);
                }
                else if (am.score == 70)
                {
                    lb.ForeColor = Color.Black;
                    lb.Font = new Font(lb.Font, FontStyle.Regular);
                }
                else if (am.score <= 60)
                {
                    lb.ForeColor = Color.Black;
                    lb.Font = new Font(lb.Font, FontStyle.Regular);
                }
            }
        }

        private void lb_Paint(object sender, PaintEventArgs e)
        {
            //결제완료 선
            if (am.cc_status == "통관" || am.cc_status == "확정")
            {
                e.Graphics.DrawLine(Pens.LightGray, new Point(0, lb.Height / 2), new Point(lb.Width, lb.Height / 2));
            }
        }

        private void UserControlArrive_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            if (am.cc_status == "통관" || am.cc_status == "확정")
            {
                DrawStar(g, Brushes.LightGray, 6, 7, 5);;
            }
            else
            {
                Rectangle rect = new Rectangle(5, 6, 5, 5);
                Pen p = new Pen(Color.BlueViolet, 3);
                g.DrawEllipse(p, rect);
            }
            
        }

        #region 별 그리기 - DrawStar(graphics, brush, radius, centerX, centerY) 
        /// <summary> 
        /// 별 그리기
        /// </summary> 
        /// <param name="graphics">그래픽스</param> 
        /// <param name="brush">브러시</param> \
        /// <param name="radius">반경</param> 
        /// <param name="centerX">중심점 X</param> 
        /// <param name="centerY">중심점 Y</param> 
        public void DrawStar(Graphics graphics, Brush brush, float radius, float centerX, float centerY) 
        { 
            float sin36 = (float)Math.Sin(36.0 * Math.PI / 180.0); 
            float sin72 = (float)Math.Sin(72.0 * Math.PI / 180.0);
            float cos36 = (float)Math.Cos(36.0 * Math.PI / 180.0); 
            float cos72 = (float)Math.Cos(72.0 * Math.PI / 180.0);
            float radius2 = radius * cos72 / cos36; 
            PointF[] pointArray = new PointF[10]; 
            pointArray[0] = new PointF(centerX , centerY - radius );
            pointArray[1] = new PointF(centerX + radius2 * sin36, centerY - radius2 * cos36);
            pointArray[2] = new PointF(centerX + radius * sin72, centerY - radius * cos72);
            pointArray[3] = new PointF(centerX + radius2 * sin72, centerY + radius2 * cos72); 
            pointArray[4] = new PointF(centerX + radius * sin36, centerY + radius * cos36); 
            pointArray[5] = new PointF(centerX , centerY + radius2 ); 
            pointArray[6] = new PointF(centerX - radius * sin36, centerY + radius * cos36); 
            pointArray[7] = new PointF(centerX - radius2 * sin72, centerY + radius2 * cos72);
            pointArray[8] = new PointF(centerX - radius * sin72, centerY - radius * cos72);
            pointArray[9] = new PointF(centerX - radius2 * sin36, centerY - radius2 * cos36); 
            graphics.FillPolygon(brush, pointArray);
        }
        #endregion

        #region 내역수정
        private void lb_DoubleClick(object sender, EventArgs e)
        {
            /*int id = int.Parse(lbId.Text.ToString());
            int sub_id = int.Parse(lbSubid.Text.ToString());
            Arrive.ArriveInfo arrive = new Arrive.ArriveInfo(cd, um, am);

            arrive.StartPosition = FormStartPosition.CenterParent;
            arrive.ShowDialog();*/

            int id = int.Parse(lbId.Text.ToString());
            UnPendingManager upm = new UnPendingManager(cd, um, "", id);
            upm.StartPosition = FormStartPosition.CenterParent;
            upm.ShowDialog();

        }
        private void UserControlArrive_DoubleClick(object sender, EventArgs e)
        {
            /*int id = int.Parse(lbId.Text.ToString());
            int sub_id = int.Parse(lbSubid.Text.ToString());
            Arrive.ArriveInfo arrive = new Arrive.ArriveInfo(cd, um, am);

            arrive.StartPosition = FormStartPosition.CenterParent;
            arrive.ShowDialog();*/

            int id = int.Parse(lbId.Text.ToString());
            UnPendingManager upm = new UnPendingManager(cd, um, "", id);
            upm.StartPosition = FormStartPosition.CenterParent;
            upm.ShowDialog();
        }
        #endregion

        public string getCcStatus()
        {
            return cc_status;
        }

        #region 우클릭 메뉴, Method
        //우클릭 메뉴 Create
        private void lb_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DateTime dt;
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right && um.auth_level >= 50)
                {
                    this.BorderStyle = BorderStyle.Fixed3D;
                    ContextMenuStrip m = new ContextMenuStrip();
                    //공통
                    m.Items.Add("영업 대시보드");
                    ToolStripSeparator toolStripSeparator0 = new ToolStripSeparator();
                    toolStripSeparator0.Name = "toolStripSeparator1";
                    toolStripSeparator0.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator0);
                    m.Items.Add("통관처리");
                    m.Items.Add("통관일자 변경");
                    ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                    toolStripSeparator1.Name = "toolStripSeparator1";
                    toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator1);
                    m.Items.Add("수정");
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
                        int id = int.Parse(lbId.Text.ToString());
                        UnPendingManager upm = new UnPendingManager(cd, um, "", id);
                        upm.StartPosition = FormStartPosition.CenterParent;
                        upm.ShowDialog();
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
                    case "영업 대시보드":
                        {
                            string[] product = new string[30];

                            product[0] = am.product;
                            product[1] = am.origin;
                            product[2] = am.sizes;
                            product[3] = am.box_weight;
                            product[22] = am.box_weight;
                            DataTable otherDt = productOtherCostRepository.GetProductInfoAsOneExactly(am.product, am.origin, am.sizes, am.box_weight);
                            if (otherDt != null)
                            {
                                product[4] = otherDt.Rows[0]["cost_unit"].ToString();
                                product[6] = otherDt.Rows[0]["custom"].ToString();
                                product[7] = otherDt.Rows[0]["tax"].ToString();
                                product[8] = otherDt.Rows[0]["incidental_expense"].ToString();
                                product[9] = otherDt.Rows[0]["purchase_margin"].ToString();
                                product[10] = otherDt.Rows[0]["production_days"].ToString();

                                bool weight_calculate;
                                if (!bool.TryParse(otherDt.Rows[0]["weight_calculate"].ToString(), out weight_calculate))
                                    weight_calculate = true;
                                product[19] = weight_calculate.ToString();

                                product[27] = otherDt.Rows[0]["delivery_days"].ToString();
                            }

                            List<string[]> list=  new List<string[]>();
                            list.Add(product);

                             DetailDashBoardForSales ddbf = new DetailDashBoardForSales(um, list);
                            ddbf.Show();


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
        private void ConvertToPending()
        {
            int results = customrepository.UpdateData(lbId.Text, "cc_status", "통관");
            //결과
            if (results == -1)
                MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
            else
                cd.displayDays(cd.year, cd.month);
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
                    cd.displayDays(cd.year, cd.month);
            }
        }
        #endregion
    }
}
