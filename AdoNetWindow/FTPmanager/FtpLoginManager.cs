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

namespace AdoNetWindow.FTPmanager
{
    public partial class FtpLoginManager : Form
    {
        UsersModel um;
        string userid = null;
        string pwd = null;
        public FtpLoginManager(UsersModel uModel, out string userid, out string pwd)
        {
            InitializeComponent();
            um = uModel;

            this.ShowDialog();
            userid = this.userid;
            pwd = this.pwd;
        }

        #region Key event
        private void FtpLoginManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnLogin.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                }
            }
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }
        #endregion

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserid.Text))
            {
                MessageBox.Show(this, "ID를 입력해주세요.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtPwd.Text))
            {
                MessageBox.Show(this, "PW를 입력해주세요.");
                this.Activate();
                return;
            }
            userid = txtUserid.Text;
            pwd = txtPwd.Text;
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.userid = null;
            this.pwd = null;
            this.Dispose();
        }
    }
}
