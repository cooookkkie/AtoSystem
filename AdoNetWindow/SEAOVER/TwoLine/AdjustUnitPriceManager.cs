using AdoNetWindow.Model;
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
    public partial class AdjustUnitPriceManager : Form
    {
        _2LineForm twoForm = null;
        UsersModel um = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public AdjustUnitPriceManager(UsersModel um, _2LineForm twoForm)
        {
            InitializeComponent();
            this.twoForm = twoForm;
            this.um = um;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if(twoForm != null) 
            {
                double rate;
                if (!double.TryParse(txtAdjustRate.Text, out rate))
                {
                    messageBox.Show(this, "조정율 값을 확인해주세요!");
                    return;
                }
                twoForm.SetAdjustUnitPrice(cbType.Text, rate);
                this.Dispose();
            }
        }
    }
}
