using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    public partial class SheetNameManager : Form
    {
        Control con;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public SheetNameManager(Control con, Point p)
        {
            InitializeComponent();
            this.con = con;
            this.Location = p;
            txtSheetName.Text = con.Text;
        }

        private void SheetNameManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(txtSheetName.Text.Trim()))
                {
                    messageBox.Show(this,"시트명을 입력해주세요!");
                }
                else
                {
                    con.Text = txtSheetName.Text.Replace("$", "").Trim();
                    this.Dispose();
                }

            }
            else if (e.KeyCode == Keys.Escape)
                this.Dispose();
        }
    }
}
