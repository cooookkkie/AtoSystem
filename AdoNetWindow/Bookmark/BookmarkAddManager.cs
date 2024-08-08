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

namespace AdoNetWindow.SEAOVER.Bookmark
{
    public partial class BookmarkAddManager : Form
    {
        SEAOVER.BookmarkManager bm;
        UsersModel um;
        bool isNew;
        int update_id;
        string gname, fname;
        bool isAdd = false;
        public BookmarkAddManager(SEAOVER.BookmarkManager bManager, UsersModel uModel)
        {
            InitializeComponent();
            bm = bManager;
            um = uModel;
            isNew = true;
        }
        public BookmarkAddManager(SEAOVER.BookmarkManager bManager, UsersModel uModel, int id, string group, string name)
        {
            InitializeComponent();
            bm = bManager;
            um = uModel;
            update_id = id;
            isNew = false;
            btnAdd.Text = "수정(A)";
            txtGroupName.Text = group;
            txtFormName.Text = name;
        }

        public bool AddBookMark(string group_name, string form_name, out string group, out string form)
        {
            txtGroupName.Text = group_name;
            txtFormName.Text = form_name + "(1)";
            this.ShowDialog();

            group = gname;
            form = fname;

            return isAdd;
        }

        #region Key event
        private void BookmarkAddManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnAdd.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.M:
                        txtGroupName.Focus();
                        break;
                    case Keys.N:
                        txtGroupName.Text = String.Empty;
                        txtFormName.Text = String.Empty;
                        txtGroupName.Focus();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        this.Dispose();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFormName.Text.Trim()))
            {
                MessageBox.Show(this, "즐겨찾기를 입력해주세요.");
                return;
            }

            if (isNew)
            {
                isAdd = true;
                gname = txtGroupName.Text;
                fname = txtFormName.Text;
                bm.InsertGroup(txtGroupName.Text, txtFormName.Text);
            }
            else
            {
                bm.update(update_id, txtGroupName.Text, txtFormName.Text);
            }
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion
    }
}
