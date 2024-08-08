using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.OverseaManufacturingBusiness
{
    public partial class UploadDataCalendarUnit : UserControl
    {
        public UploadDataCalendarUnit(int year, int month, int day, int seaFood = 0, int processedFood = 0, int livestockProduct = 0, int agriculturalProduct = 0)
        {
            InitializeComponent();
            if (day == 0)
            {
                foreach (Control col in this.Controls)
                    col.Visible = false;

                this.BackColor = Color.LightGray;
            }
            else
            {
                if (day > 0)
                {
                    DateTime sdate = new DateTime(year, month, day);
                    if (sdate.DayOfWeek == DayOfWeek.Sunday)
                        lbDay.ForeColor = Color.Red;
                    else if (sdate.DayOfWeek == DayOfWeek.Saturday)
                        lbDay.ForeColor = Color.Blue;
                }

                lbDay.Text = day.ToString();
                lbSeafood.Text = seaFood.ToString("#,##0");
                lbProcessedFood.Text = processedFood.ToString("#,##0");
                lbLivestockproduct.Text = livestockProduct.ToString("#,##0");
                lbAgriculturalproduct.Text = agriculturalProduct.ToString("#,##0");
                lbTotalCount.Text = (seaFood + processedFood + livestockProduct + agriculturalProduct).ToString("#,##0");
            }
        }
    }
}
