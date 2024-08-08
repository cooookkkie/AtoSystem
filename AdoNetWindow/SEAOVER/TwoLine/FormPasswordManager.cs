using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.TwoLine
{
    public partial class FormPasswordManager : Form
    {
        string password = "";
        string input_password = "";
        public FormPasswordManager(string password)
        {
            InitializeComponent();
            this.password = password;
        }

        public bool CheckPassword()
        {
            this.ShowDialog();

            if(password == input_password)
                return true;
            else
                return false;
        }
        public string SettingPassword()
        {
            this.ShowDialog();

            return input_password;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            input_password = string.Empty;
            this.Dispose();
        }

        private void btnRegistrate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                MessageBox.Show(this, "비밀번호를 입력해주세요.");
                this.Activate();
                return;
            }
            input_password = txtPassword.Text;
            this.Dispose();
        }

        private void FormPasswordManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnRegistrate.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                btnExit.PerformClick();
        }
    }
}
