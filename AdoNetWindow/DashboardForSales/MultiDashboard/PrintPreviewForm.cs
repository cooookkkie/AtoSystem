using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AdoNetWindow.DashboardForSales.MultiDashboard
{
    internal class PrintPreviewForm : Form
    {
        private List<Bitmap> bitmaps;

        public PrintPreviewForm(List<Bitmap> bitmaps)
        {
            this.bitmaps = bitmaps;
        }
    }
}