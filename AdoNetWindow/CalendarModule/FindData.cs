using AdoNetWindow.Model;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.CalendarModule
{
    public partial class FindData : Form
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        CalendarModule.calendar cal;
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        public FindData(UsersModel uModel,CalendarModule.calendar calendar)
        {
            InitializeComponent();
            cal = calendar;
            um = uModel;
        }
        private void FindData_Load(object sender, EventArgs e)
        {
            txtAtono.Focus();
        }

        #region Method
        private void DataSearching()
        {
            if (string.IsNullOrEmpty(txtAtono.Text) && string.IsNullOrEmpty(txtBlno.Text) && string.IsNullOrEmpty(txtLcno.Text))
            {
                MessageBox.Show(this, "검색값을 입력하지 않았습니다.");
                this.Activate();
            }
            else
            {
                cal.Find_pending(txtAtono.Text, txtBlno.Text, txtLcno.Text);
            }
        }
        #endregion

        #region Key event
        private void FindData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.F:
                        DataSearching();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                DataSearching();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
        private void txtEnddate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //날짜 완성
                Control tbb = (Control)sender;
                tbb.Text = common.strDatetime(tbb.Text);
            }
        }
        #endregion

        #region Butoon
        private void btnFind_Click(object sender, EventArgs e)
        {
            DataSearching();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        private void txtAtono_TextChanged(object sender, EventArgs e)
        {
            cal.RefreshFindIdx();
        }
    }
}
