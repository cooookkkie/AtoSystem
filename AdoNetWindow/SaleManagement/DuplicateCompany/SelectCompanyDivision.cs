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
    public partial class SelectCompanyDivision : Form
    {
        string division = null;
        string[] duplicates = new string[4];
        int potentials = 0;
        UsersModel um;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public SelectCompanyDivision(UsersModel um)
        {
            InitializeComponent();
            this.um = um;

            if (um.department.Contains("관리") || um.department.Contains("전산"))
                rbCommon.Enabled = true;
            else
                rbCommon.Enabled = false;
        }

        public string SelectDivision(out string[] duplicates, out int potentials)
        { 
            this.ShowDialog();

            duplicates = this.duplicates;
            potentials = this.potentials;

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
            {
                messageBox.Show(this,"구분을 선택해주세요.");
                this.Activate();
            }
            else if (rbNotSales.Checked && !cbNotHandling.Checked && !cbNotSendFax.Checked && !cbOutOfBusiness.Checked)
            {
                messageBox.Show(this,"영업금지 거래처로 등록시 아래 영업금지 세항목 중 하나 이상은 필수로 선택해주셔야합니다.");
                this.Activate();
            }
            else
            {
                if (rbCommon.Checked)
                {
                    if (messageBox.Show(this, "[공용DATA]는 모든 사용자에게 분배될 데이터로 관리자 외에는 확인할 수 없는 탭으로 등록됩니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                }


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
                    if (rbCommon.Checked)
                        division = rbCommon.Text;
                    else if (rbRandom.Checked)
                        division = rbRandom.Text;
                    else if (rbPotential1.Checked)
                        division = rbPotential1.Text;
                    else if (rbPotential2.Checked)
                        division = rbPotential2.Text;
                }

                duplicates = new string[5];
                if (cbDuplicateCommonData.Checked)
                    duplicates[0] = "TRUE";
                if (cbDuplicateMyData.Checked)
                    duplicates[1] = "TRUE";
                if (cbDuplicatePotential1.Checked)
                    duplicates[2] = "TRUE";
                if (cbDuplicatePotential2.Checked)
                    duplicates[3] = "TRUE";
                if (cbDuplicateTrading.Checked)
                    duplicates[4] = "TRUE";

                if (!int.TryParse(cbPotentialCountLimit.Text, out potentials))
                {
                    messageBox.Show(this,"잠재 중복제한 수를 다시 설정해주세요,");
                    this.Activate();
                    return;
                }

                this.Dispose();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            duplicates = new string[4];
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
