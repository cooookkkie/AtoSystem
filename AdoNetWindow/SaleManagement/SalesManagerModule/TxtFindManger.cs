using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement.SalesManagerModule;
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
    public partial class TxtFindManger : Form
    {
        UsersModel um;
        DailyBusiness.DailyBusiness db = null;
        SalesManager sm = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public TxtFindManger(UsersModel um, DailyBusiness.DailyBusiness db)
        {
            InitializeComponent();
            this.um = um;
            this.db = db;
        }
        public TxtFindManger(UsersModel um, SalesManager sm)
        {
            InitializeComponent();
            this.um = um;
            this.sm = sm;
        }
        private void TxtFindManger_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtCompany;
        }

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            //단어검색
            if (tabFindManager.SelectedTab.Name == "tabWord")
            {

                if (db != null)
                {
                    if (!db.SearchingTxt(txtFind.Text, txtExcept.Text))
                    {
                        messageBox.Show(this,"찾을 내역이 없습니다!");
                        this.Activate();
                    }
                    else
                        this.Opacity = 30;
                }
                else if (sm != null)
                {
                    if (!sm.SearchingTxt(txtFind.Text, txtExcept.Text))
                    {
                        messageBox.Show(this,"찾을 내역이 없습니다!");
                        this.Activate();
                    }
                    else
                        this.Opacity = 30;
                }
            }
            //거래처검색
            else
            {
                bool isAtoManagerEnbaled = false;
                if (tabFindManager.SelectedTab.Name == "tabCompany")
                {
                    if (string.IsNullOrEmpty(txtCompany.Text.Trim()) && string.IsNullOrEmpty(txtTel.Text.Trim()) && string.IsNullOrEmpty(txtRnum.Text.Trim()))
                    {
                        messageBox.Show(this, "검색항목을 입력해주세요!");
                        this.Activate();
                        return;
                    }
                    else if (!string.IsNullOrEmpty(txtTel.Text.Trim()) && string.IsNullOrEmpty(txtCompany.Text.Trim()) && string.IsNullOrEmpty(txtRnum.Text.Trim())
                        && txtTel.Text.Trim().Length < 4)
                    {
                        messageBox.Show(this, "전화번호 검색시 4자리 이상 입력하셔야합니다!");
                        this.Activate();
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtTel.Text.Trim()) && !string.IsNullOrEmpty(txtCompany.Text.Trim()) && string.IsNullOrEmpty(txtRnum.Text.Trim())
                        && txtCompany.Text.Trim().Length < 2)
                    {
                        messageBox.Show(this, "거래처명 검색시 2자리 이상 입력하셔야합니다!");
                        this.Activate();
                        return;
                    }
                    isAtoManagerEnbaled = true;
                }
                else if (tabFindManager.SelectedTab.Name == "tabEtcInfo")
                {
                    if (string.IsNullOrEmpty(txtDistribution.Text.Trim()) && string.IsNullOrEmpty(txtHandlingItem.Text.Trim()))
                    {
                        messageBox.Show(this, "검색항목을 입력해주세요!");
                        this.Activate();
                        return;
                    }
                }

                //검색출력
                FindCompanyList fcl = new FindCompanyList(um, sm, txtCompany.Text, txtTel.Text, txtRnum.Text, txtHandlingItem.Text, txtDistribution.Text, isAtoManagerEnbaled);
                fcl.Owner = sm;
                fcl.Show();
                this.Dispose();
            }

        }
        #endregion

        #region Key event
        private void TxtFindManger_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtFind.Focus();
                        break;
                    case Keys.N:
                        txtFind.Text = String.Empty;
                        txtExcept.Text = String.Empty;
                        txtFind.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        btnSearching.PerformClick();
                        break;
                    case Keys.Escape:
                        btnExit.PerformClick();
                        break;
                    case Keys.F1:
                        tabFindManager.SelectedIndex = 0;
                        break;
                    case Keys.F2:
                        tabFindManager.SelectedIndex = 1;
                        break;
                    case Keys.F3:
                        tabFindManager.SelectedIndex = 2;
                        break;
                }
            }       
        }

        #endregion

        private void tabFindManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabFindManager.SelectedTab.Name == "tabCompany")
                txtCompany.Focus();
            else if (tabFindManager.SelectedTab.Name == "tabWord")
                txtFind.Focus();
            else if (tabFindManager.SelectedTab.Name == "tabEtcInfo")
                txtHandlingItem.Focus();
        }
    }
}

