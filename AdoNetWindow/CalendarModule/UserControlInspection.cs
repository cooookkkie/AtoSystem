using AdoNetWindow.Inspection;
using AdoNetWindow.Model;
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
    public partial class UserControlInspection : UserControl
    {
        InspectionModel im;
        int days;
        calendar cd;
        UsersModel um;
        bool comfirm = false;
        public UserControlInspection(InspectionModel inpectionModel, int num_day, calendar cal, UsersModel userModel)
        {
            InitializeComponent();
            im = inpectionModel;
            days = num_day;
            cd = cal;
            um = userModel;
        }

        private void UserControlInspection_Load(object sender, EventArgs e)
        {
            SetLabel(im);
        }

        #region Method
        public void SetLabel(InspectionModel im)
        {
            lbId.Text = im.id.ToString();
            lbSubid.Text = im.sub_id.ToString();

            lb.Text = "(" + im.origin + ")" + im.product + " " + im.sizes;
            if (!string.IsNullOrEmpty(im.inspection_results))
            {
                comfirm = true;
            }
            //정확도에 따른 차별
            if (im.warehousing_date_score == 100)
                lb.Font = new Font("돋움", 9, FontStyle.Bold);
            else if (im.warehousing_date_score == 90)
                lb.ForeColor = Color.Gray;
            else 
                lb.ForeColor = Color.LightGray;
        }
        #endregion

        #region Paint
        private void lb_Paint(object sender, PaintEventArgs e)
        {
            //결제완료 선
            if (comfirm)
            {
                e.Graphics.DrawLine(Pens.Black, new Point(0, lb.Height / 2), new Point(lb.Width, lb.Height / 2));
            }
        }

        private void UserControlInspection_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.Blue, 3);

            Rectangle rec = new Rectangle(5, 6, 5, 5);
            g.DrawRectangle(p, rec);
        }
        #endregion

        private void lb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            InspectionInfo info = new InspectionInfo(cd, um, im.id, im.sub_id);
            info.ShowDialog();
        }

        private void UserControlInspection_DoubleClick(object sender, EventArgs e)
        {
            InspectionInfo info = new InspectionInfo(cd, um, im.id, im.sub_id);
            info.ShowDialog();
        }
    }
}
