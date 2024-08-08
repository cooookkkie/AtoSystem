using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libs
{
    public partial class MessageBox : Form
    {
        bool isResult = false;
        public MessageBox()
        {
            InitializeComponent();
        }

        public bool Show(Form owner, string message, string title = "")
        {
            this.Owner = owner;
            this.TopMost = true;
            this.lbMessage.Text = message;
            if (message.Length > 30)
                this.Width = 500;
            else
                this.Width = 300;
            this.lbTitle.Text = title;

            btnOk.Text = "확인";
            this.pnCancel.Visible = false;
            this.ActiveControl = btnOk;
            btnOk.Focus();
            this.ShowDialog();

            return true;
        }

        public DialogResult Show(Form owner, string message, string title, MessageBoxButtons mbb = MessageBoxButtons.YesNo)
        {
            this.Owner = owner;
            this.TopMost = true;
            this.lbMessage.Text = message;
            if (message.Length > 30)
                this.Width = 500;
            else
                this.Width = 300;
            this.lbTitle.Text = title;
            this.pnCancel.Visible = true;

            this.ActiveControl = btnOk;
            btnOk.Focus();
            this.ShowDialog();

            if (isResult)
                return DialogResult.Yes;
            else
                return DialogResult.No;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            isResult = true;
            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            isResult = false;
            this.Close();
        }

        private void MessageBoxShow_Load(object sender, EventArgs e)
        {

        }

        private void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Y:
                        btnOk.PerformClick();
                        break;
                    case Keys.N:
                        btnCancle.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        btnCancle.PerformClick();
                        break;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                SelectNextControl(this.ActiveControl, false, true, true, true);
                return true;
            }
            else if (keyData == Keys.Right)
            {
                SelectNextControl(this.ActiveControl, true, true, true, true);
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
