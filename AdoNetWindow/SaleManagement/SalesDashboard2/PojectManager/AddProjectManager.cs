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

namespace AdoNetWindow.SaleManagement.SalesDashboard2.PojectManager
{   
    public partial class AddProjectManager : Form
    {
        IUsersRepository usersRepository = new UsersRepository();
        UsersModel um;
        public AddProjectManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }

        private void AddProjectManager_Load(object sender, EventArgs e)
        {
            DataTable departmentDt = usersRepository.GetOneData("department", "", "", "", "");
            DataTable teamDt = usersRepository.GetOneData("department", "", "", "", "");
            DataTable gardeDt = usersRepository.GetOneData("department", "", "", "", "");
        }

        #region Key event
        private void AddProjectManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnUpdate.PerformClick();
                        break;

                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        btnGetProduct.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnGetProduct_Click(object sender, EventArgs e)
        {

        }

        private void btnCalendarSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
                txtSttdate.Text = sdate;
        }

        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
                txtEnddate.Text = sdate;
        }
        #endregion

        
    }
}
