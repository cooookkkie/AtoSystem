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
    public partial class RemoveDuplicateCompanyMessageBox : Form
    {
        bool isFlag = false;
        public RemoveDuplicateCompanyMessageBox()
        {
            InitializeComponent();
        }

        public bool RemoveDuplicateCompanyMessageBoxResult()
        {
            this.ShowDialog();

            return isFlag;
        }


        private void btnSelect_Click(object sender, EventArgs e)
        {
            isFlag = true;
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            isFlag = false;
            this.Dispose();
        }

        private void RemoveDuplicateCompanyMessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
            }
            else if (e.Modifiers == Keys.Control)
            { }
            else
            {
                switch (e.KeyCode)
                { 
                    case Keys.Escape:
                        btnSelect.PerformClick();
                        break;
                    case Keys.Enter:
                        btnSelect.PerformClick();
                        break;
                }
            }
        }
    }
}
