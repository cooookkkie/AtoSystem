using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class calendarUnit : UserControl
    {
        bool accentDay;
        public calendarUnit(bool isHoliday, DayOfWeek dow, int days, double shipment, double stock, double sales_count, bool shortDay)
        {
            InitializeComponent();

            lbDay.Text = days.ToString();
            lbShipment.Text = shipment.ToString("#,##0,00");
            if (shipment > 0)
            {
                lbShipment.ForeColor = Color.Blue; 
            }
            lbStock.Text = stock.ToString("#,##0.00");
            //소진일자
            accentDay = shortDay;
            //공휴일
            if (isHoliday)
            {
                lbDay.ForeColor = Color.Red;
            }
            //토요일 경우 파란색
            if (dow == DayOfWeek.Saturday)
            {
                lbDay.ForeColor = Color.Blue;
            }
            //재고소진
            if (stock <= 0)
            {
                lbStock.ForeColor = Color.Red;
            }
        }

        private void calendarUnit_Paint(object sender, PaintEventArgs e)
        {
            //강조테두리
            if (accentDay)
            {
                Graphics g = this.CreateGraphics();
                Pen p = new Pen(Color.Red, 2);

                Rectangle rec = new Rectangle(0, 0, this.Width - 2, this.Height - 2);
                g.DrawRectangle(p, rec);
            }
        }
    }
}
