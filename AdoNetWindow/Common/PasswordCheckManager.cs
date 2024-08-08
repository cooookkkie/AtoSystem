using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace AdoNetWindow.Common
{
    public partial class PasswordCheckManager : Form
    {
        Libs.MessageBox messageBox = new Libs.MessageBox();
        string password = "";
        bool isFlag = false;
        public PasswordCheckManager(string password)
        {
            InitializeComponent();
            this.password = password;
        }

        public bool isPasswordCheck() 
        {
            this.ShowDialog();

            return isFlag;
        }

        #region Button
        private void btnRegistrate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                messageBox.Show(this, "PW를 입력해주세요.");
                return;
            }
            else if (txtPassword.Text != password)
            {
                messageBox.Show(this, "PW가 일치하지 않습니다.");
                this.Dispose();
            }
            else
            {
                isFlag = true;
                this.Dispose();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void PasswordCheckManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { }
            else if (e.Modifiers == Keys.Control)
            { }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnRegistrate.PerformClick();
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
