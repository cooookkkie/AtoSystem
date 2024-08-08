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
    public partial class UpdateCompanyInfoManager : Form
    {
        UsersModel um;
        SalesManager sm;
        public UpdateCompanyInfoManager(UsersModel um, SalesManager sm)
        {
            InitializeComponent();
            this.um = um;   
            this.sm = sm;
        }

        private void UpdateCompanyInfoManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnRegister.PerformClick();
                        break;
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (sm != null)
            {
                int updateType = 0;
                if (rbUpdateInfo.Checked)
                    updateType = 1;
                else if (rbCommonData.Checked)
                    updateType = 2;
                else if (rbMyData.Checked)
                    updateType = 3;
                else if (rbPotential1.Checked)
                    updateType = 4;
                else if (rbPotential2.Checked)
                    updateType = 5;
                else if (rbTrading.Checked)
                    updateType = 6;
                else if (rbNoneHandled.Checked)
                    updateType = 7;

                if (updateType == 0)
                {
                    MessageBox.Show(this,"수정 타입을 선택해주세요.");
                    this.Activate();
                    return;
                }
                else
                {
                    sm.UpdateCompany(updateType);
                    this.Dispose();
                }
            }
                
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
