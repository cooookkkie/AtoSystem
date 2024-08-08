using AdoNetWindow.SEAOVER._2Line;
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
    public partial class TextFinder : Form
    {
        _2LineForm twoLineForm = null;
        OneLine.OneLineForm oneLineForm = null;
        public TextFinder(_2LineForm twoLineForm)
        {
            InitializeComponent();
            this.twoLineForm = twoLineForm;
        }
        public TextFinder(OneLine.OneLineForm oneLineForm)
        {
            InitializeComponent();
            this.oneLineForm = oneLineForm;
        }
        #region Button
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if(twoLineForm != null)
                twoLineForm.SearchingTxt(txtFindAll.Text, txtFindExcept.Text);
            else if (oneLineForm != null)
                oneLineForm.SearchingTxt(txtFindAll.Text, txtFindExcept.Text);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void TextFinder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.M:
                        txtFindAll.Focus();
                        break;
                    case Keys.N:
                        txtFindAll.Text= string.Empty;
                        txtFindExcept.Text = string.Empty;
                        txtFindAll.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnSearch.PerformClick();
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
