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

namespace AdoNetWindow.SaleManagement
{
    public partial class TxtFindManger : Form
    {
        UsersModel um;
        DailyBusiness.DailyBusiness db = null;
        public TxtFindManger(UsersModel um, DailyBusiness.DailyBusiness db)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;
        }

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            if (db != null)
            {
                if (!db.SearchingTxt(txtFind.Text, txtExcept.Text))
                    MessageBox.Show("찾을 내역이 없습니다!");
                else
                    this.Opacity = 30;
            }
                
        }
        #endregion

        #region Key event
        private void TxtFindManger_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtFind.Focus();
                        break;
                    case Keys.N:
                        txtFind.Text = String.Empty;
                        txtExcept.Text = String.Empty;
                        txtFind.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnSearching.PerformClick();
                        break;
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                }
            }       
        }
        #endregion
    }
}
