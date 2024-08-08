using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.DuplicateCompany
{
    public partial class SheetNameManage : UserControl
    {
        Control con;
        public SheetNameManage(Control con, Point p)
        {
            InitializeComponent();
            this.con = con;
            this.Location = p;

            this.Width = con.Width;
            this.Height = con.Height;
            txtSheetName.Width = con.Width;
            txtSheetName.Height = con.Height;
        }

        private void txtSheetName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(txtSheetName.Text.Trim()))
                {
                    MessageBox.Show(this,"시트명을 입력해주세요!");
                }
                else
                {
                    con.Text = txtSheetName.Text.Trim();
                    this.Dispose();
                }

            }
            else if (e.KeyCode == Keys.Escape)
                this.Dispose();
        }
    }
}
