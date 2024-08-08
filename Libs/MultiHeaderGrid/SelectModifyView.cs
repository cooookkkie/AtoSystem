using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Libs.MultiHeaderGrid
{
    public partial class SelectModifyView : Form
    {
        public delegate void AcceptSelectModify(object sender, AcceptSelectModifyEventArgs e);
        public event AcceptSelectModify SelectModifyAccepted = null;
        public SelectModifyView()
        {
            InitializeComponent();
            this.tbValue.LostFocus += tbValue_LostFocus;
            this.tbValue.KeyPress += tbValue_KeyPress;
        }
        void tbValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (SelectModifyAccepted != null)
                    SelectModifyAccepted(this, new AcceptSelectModifyEventArgs(this.tbValue.Text));

                this.tbValue.Text = string.Empty;
                this.Hide();
                e.Handled = true;
            }
        }

        void tbValue_LostFocus(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
    public class AcceptSelectModifyEventArgs : EventArgs
    {
        public string Value { get; set; }
        public AcceptSelectModifyEventArgs(string value)
        {
            this.Value = value;
        }
    }
}
