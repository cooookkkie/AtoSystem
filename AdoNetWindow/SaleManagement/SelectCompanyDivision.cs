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
    public partial class SelectCompanyDivision : Form
    {
        string division = null;
        public SelectCompanyDivision()
        {
            InitializeComponent();
        }

        public string SelectDivision()
        { 
            this.ShowDialog();

            return division;
        }
        #region Key event
        private void SelectCompanyDivision_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            { 
                case Keys.Enter:
                    btnAddCompany.PerformClick();
                    break;
                case Keys.Escape:
                    btnExit.PerformClick();
                    break;
            }
        }
        #endregion

        #region Button
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            if (!rbCommon.Checked && !rbRandom.Checked && !rbPotential1.Checked && !rbPotential2.Checked && !rbNotSales.Checked)
                MessageBox.Show("구분을 선택해주세요.");
            else if (rbNotSales.Checked && !cbNotHandling.Checked && !cbNotSendFax.Checked && !cbOutOfBusiness.Checked)
                MessageBox.Show("영업금지 거래처로 등록시 아래 영업금지 세항목 중 하나 이상은 필수로 선택해주셔야합니다.");
            else
            {
                if (rbNotSales.Checked)
                {
                    division = "";
                    if (cbNotHandling.Checked)
                        division += " " + cbNotHandling.Text;
                    if (cbNotSendFax.Checked)
                        division += " " + cbNotSendFax.Text;
                    if (cbOutOfBusiness.Checked)
                        division += " " + cbOutOfBusiness.Text;

                    division = division.Trim();
                }
                else
                { 
                    if(rbCommon.Checked)
                        division = rbCommon.Text;
                    else if (rbRandom.Checked)
                        division = rbRandom.Text;
                    else if (rbPotential1.Checked)
                        division = rbPotential1.Text;
                    else if (rbPotential2.Checked)
                        division = rbPotential2.Text;
                }
                    
                this.Dispose();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            division = null;
            this.Dispose();
        }
        private void rbNotSales_CheckedChanged(object sender, EventArgs e)
        {
            cbNotHandling.Enabled = rbNotSales.Checked;
            cbNotSendFax.Enabled = rbNotSales.Checked;
            cbOutOfBusiness.Enabled = rbNotSales.Checked;
        }
        #endregion


    }
}
