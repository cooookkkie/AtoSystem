using AdoNetWindow.CalendarModule.VacationManager;
using AdoNetWindow.DashboardForSales.MultiDashboard;
using AdoNetWindow.OverseaManufacturingBusiness;
using AdoNetWindow.PurchaseManager;
using AdoNetWindow.SEAOVER._2Line;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Common.PrintManager
{
    public partial class PrintManager : Form
    {
        PurchaseManager.CostAccounting ca = null;
        _2LineForm lf = null;
        HandlingProduct hp = null;
        VacationDashboard vd = null;
        MultiDashBoard md = null;
        CostAccountingGroup cag = null;

        public PrintManager(PrintDocument printDoc, int total_pages = 1)
        {
            InitializeComponent();
            prtPreControl.MouseWheel += MouseWheelEvent;
            //ca.InitVariable();
            prtDocument = printDoc;
            prtPreControl.Document = prtDocument;
            prtPreControl.UseAntiAlias = true;
            nudCurPage.Maximum = total_pages;
            nudTotalPage.Maximum = total_pages;
            nudTotalPage.Text = total_pages.ToString();
        }
        public PrintManager(PurchaseManager.CostAccountingGroup cag, PrintDocument printDoc, int total_pages = 1)
        {
            InitializeComponent();
            prtPreControl.MouseWheel += MouseWheelEvent;

            this.cag = cag;
            cag.InitVariable();
            prtDocument = printDoc;
            prtPreControl.Document = prtDocument;
            prtPreControl.UseAntiAlias = true;
            nudCurPage.Maximum = total_pages;
            nudTotalPage.Maximum = total_pages;
            nudTotalPage.Text = total_pages.ToString();
        }
        public PrintManager(PurchaseManager.CostAccounting costAccounting, PrintDocument printDoc, int total_pages = 1)
        {
            InitializeComponent();
            prtPreControl.MouseWheel += MouseWheelEvent;

            ca = costAccounting;
            ca.InitVariable();
            prtDocument = printDoc;
            prtPreControl.Document = prtDocument;
            prtPreControl.UseAntiAlias = true;
            nudCurPage.Maximum = total_pages;
            nudTotalPage.Maximum = total_pages;
            nudTotalPage.Text = total_pages.ToString();
        }
        public PrintManager(_2LineForm lineform, PrintDocument printDoc, int total_pages = 1)
        {
            InitializeComponent();
            prtPreControl.MouseWheel += MouseWheelEvent;

            lf = lineform;
            if(ca !=null)
                ca.InitVariable();
            else if(lf != null)
                lf.InitVariable();
            prtDocument = printDoc;
            prtPreControl.Document = prtDocument;
            prtPreControl.UseAntiAlias = true;
            nudCurPage.Maximum = total_pages;
            nudTotalPage.Maximum = total_pages;
            nudTotalPage.Text = total_pages.ToString();
        }

        public PrintManager(HandlingProduct hp, PrintDocument printDoc, int total_pages = 1)
        {
            InitializeComponent();
            prtPreControl.MouseWheel += MouseWheelEvent;

            this.hp = hp;
            hp.InitVariable();
            prtDocument = printDoc;
            prtPreControl.Document = prtDocument;
            prtPreControl.UseAntiAlias = true;
            nudCurPage.Maximum = total_pages;
            nudTotalPage.Maximum = total_pages;
            nudTotalPage.Text = total_pages.ToString();
        }
        public PrintManager(VacationDashboard vd, PrintDocument printDoc, int total_pages = 1)
        {
            InitializeComponent();
            prtPreControl.MouseWheel += MouseWheelEvent;

            this.vd = vd;
            vd.InitVariable();
            prtDocument = printDoc;
            prtPreControl.Document = prtDocument;
            prtPreControl.UseAntiAlias = true;
            nudCurPage.Maximum = total_pages;
            nudTotalPage.Maximum = total_pages;
            nudTotalPage.Text = total_pages.ToString();
        }
        public PrintManager(MultiDashBoard md, PrintDocument printDoc, int total_pages = 1)
        {
            InitializeComponent();
            prtPreControl.MouseWheel += MouseWheelEvent;

            this.md = md;
            md.InitVariable();
            prtDocument = printDoc;
            prtPreControl.Document = prtDocument;
            prtPreControl.UseAntiAlias = true;
            nudCurPage.Maximum = total_pages;
            nudTotalPage.Maximum = total_pages;
            nudTotalPage.Text = total_pages.ToString();
        }

        #region Method
        //Count pages
        public static int GetPageCount(PrintDocument pd)
        {
            int count = 0;
            PrintController original = pd.PrintController;
            pd.PrintController = new PreviewPrintController();
            pd.PrintPage += (sender, e) => count++;
            pd.Print();
            pd.PrintController = original;
            return count;
        }

        #endregion

        #region Button
        private void btnSelectPrinter_Click(object sender, EventArgs e)
        {
            prtDialog.Document = prtDocument;
            prtDialog.AllowSelection = true;
            prtDialog.AllowSomePages = true;

            if (prtDialog.ShowDialog() == DialogResult.OK)
            {
                if (lf != null)
                    lf.InitVariable();

                if (ca != null)
                    ca.InitVariable();

                if (hp != null)
                    hp.InitVariable();

                if (vd != null)
                    vd.InitVariable();

                if (md != null)
                    md.InitVariable();

                if (cag != null)
                    cag.InitVariable();

                try
                {
                    prtDocument.Print();
                }
                catch { }
            }
            Thread.Sleep(1000);
            this.Activate();
        }
        private void btnPrintSelet_Click(object sender, EventArgs e)
        {
            prtDialog.Document = prtDocument;
            prtDialog.AllowSelection = true;
            prtDialog.AllowSomePages = true;

            if (lf != null)
                lf.InitVariable();

            if (ca != null)
                ca.InitVariable();

            if (hp != null)
                hp.InitVariable();

            if (vd != null)
                vd.InitVariable();

            if (md != null)
                md.InitVariable();

            if (cag != null)
                cag.InitVariable();

            try
            {
                prtDocument.Print();
            }
            catch { }

            Thread.Sleep(1000);
            this.Activate();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnPre_Click(object sender, EventArgs e)
        {
            int current_page;
            if (!int.TryParse(nudCurPage.Text, out current_page))
            {
                current_page = 1;
            }
            int total_page;
            if (!int.TryParse(nudTotalPage.Text, out total_page))
            {
                total_page = 1;
            }

            if (current_page > 1)
            {
                current_page -= 1;
                nudCurPage.Value = current_page;
                prtPreControl.StartPage = current_page - 1;
                prtPreControl.Refresh();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int current_page;
            if (!int.TryParse(nudCurPage.Text, out current_page))
            {
                current_page = 1;
            }
            int total_page;
            if (!int.TryParse(nudTotalPage.Text, out total_page))
            {
                total_page = 1;
            }

            if (current_page < total_page)
            {
                current_page += 1;
                nudCurPage.Value = current_page;
                prtPreControl.StartPage = current_page - 1;
                prtPreControl.Refresh();
            }
        }
        private void btnPlusZoom_Click(object sender, EventArgs e)
        {
            int zoom = (int)((prtPreControl.Zoom + 0.1) * 100);
            if (zoom <= 1000)
            {
                prtPreControl.Zoom = prtPreControl.Zoom + 0.1;
                nudZoomSize.Value = zoom;
            }
        }

        private void btnMinusZoom_Click(object sender, EventArgs e)
        {
            int zoom = (int)((prtPreControl.Zoom - 0.1) * 100);
            if (zoom > 1)
            {
                prtPreControl.Zoom = prtPreControl.Zoom - 0.1;
                nudZoomSize.Value = zoom;
            }
        }
        #endregion

        #region Print event
        int i = 1;

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(String.Format("Hello.net Page # {0}", i), new Font("Arial", 35),
             Brushes.Black, e.MarginBounds.Left,
            e.MarginBounds.Top);
            if (i++ < 10)
                e.HasMorePages = true;
            else
            {
                e.HasMorePages = false;
                i = 1;
            }

        }
        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            MessageBox.Show(this, "Print Start");
            this.Activate();
        }

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            MessageBox.Show(this,"Print End");
            this.Activate();
        }
        #endregion

        #region Keys event
        private void PrintManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        btnPrintSelect.PerformClick();
                        break;
                    case Keys.O:
                        btnSelectPrinter.PerformClick();
                        break;
                }
            }
        }
        private void PrintManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)16)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    btnPrintSelect.PerformClick();
                }

            }
            else if (e.KeyChar == (char)15)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    btnSelectPrinter.PerformClick();
                }
            }
        }
        #endregion

        #region 기타 Event
        //휠 확대, 축소
        private void MouseWheelEvent(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            { 
                var prtpreviewControl = sender as PrintPreviewControl;

                if (e.Delta > 0)
                {
                    int zoom = (int)((prtpreviewControl.Zoom + 0.1) * 100);
                    if (zoom <= 1000)
                    {
                        prtpreviewControl.Zoom = prtpreviewControl.Zoom + 0.1;
                        nudZoomSize.Value = zoom;
                    }
                }
                else if (e.Delta < 0)
                {
                    int zoom = (int)((prtpreviewControl.Zoom - 0.1) * 100);
                    if (zoom > 1)
                    {
                        prtpreviewControl.Zoom = prtpreviewControl.Zoom - 0.1;
                        nudZoomSize.Value = zoom;
                    }
                }
            }
        }
        private void nudZoomSize_ValueChanged(object sender, EventArgs e)
        {
            prtPreControl.Zoom = (Convert.ToDouble(nudZoomSize.Value) / 100);
        }

        #endregion

        
    }
}
