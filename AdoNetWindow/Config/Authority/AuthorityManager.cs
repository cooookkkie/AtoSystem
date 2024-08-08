using AdoNetWindow.CalendarModule;
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

namespace AdoNetWindow.Config
{
    public partial class AuthorityManager : Form
    {
        UsersModel um;
        calendar cal;
        public AuthorityManager(UsersModel um, calendar cal)
        {
            InitializeComponent();
            this.um = um;
            this.cal = cal;
        }

        private void AuthorityManager_Load(object sender, EventArgs e)
        {
            GetData();
        }

        #region Method
        private void GetData()
        {
            //초기화
            if (dgvAuthority.ColumnCount > 3)
            {
                for (int i = dgvAuthority.ColumnCount - 1; i >= 3; i--)
                    dgvAuthority.Columns.Remove(dgvAuthority.Columns[i]);
            }

            MenuStrip ms = cal.GetMenu();

            foreach (ToolStripMenuItem item in ms.Items)
            {
                if (!item.Text.Contains("님 반갑습니다") && item.Text != "로그아웃")
                {
                    DataGridViewColumn col = new DataGridViewColumn();
                    col.Name = item.Name;
                    col.HeaderText = item.Text;
                    dgvAuthority.Columns.Add(col);
                }
            }
        }
        #endregion

        #region Button
        private void btnSelect_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        private void AuthorityManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode) 
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.A:
                        btnUpdate.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
    }
}
