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

namespace AdoNetWindow.LoginForm
{
    public partial class UsersManager : Form
    {
        IUsersRepository usersRepository = new UsersRepository();

        public UsersManager()
        {
            InitializeComponent();
        }

        private void GetUsers()
        {
            UsersList.Items.Clear();

            List<UsersModel> model = new List<UsersModel>();
            model = usersRepository.GetUsers();
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = model[i].user_id.ToString();
                    lvi.SubItems.Add(model[i].user_name.ToString());
                    lvi.SubItems.Add(model[i].tel.ToString());
                    lvi.SubItems.Add(model[i].user_status.ToString());
                    UsersList.Items.Add(lvi);
                }
            }
        }
        private void UsersManager_Load(object sender, EventArgs e)
        {
            txtId.Text = "";
            txtName.Text = "";
            txtTel.Text = "";
            cbStatus.Text = "";
            GetUsers();
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show(this,"사용자를 선택해주세요.");
                this.Activate();
            }
            else if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(this,"이름을 입력해주세요.");
                this.Activate();
            }
            else if (string.IsNullOrEmpty(txtTel.Text))
            {
                MessageBox.Show(this,"연락처를 입력해주세요.");
                this.Activate();
            }
            else
            {
                UsersModel model = new UsersModel();
                model.user_id = txtId.Text;
                model.user_name = txtName.Text;
                model.tel = txtTel.Text;
                model.user_status = cbStatus.Text;

                int result = usersRepository.UpdateUser(model);
                if (result == -1)
                {
                    MessageBox.Show(this,"수정중 에러가 발생 하였습니다.");
                    this.Activate();
                }
                else
                {
                    GetUsers();
                }
            }
        }
        private void Delete()
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show(this,"사용자를 선택해주세요.");
                this.Activate();
            }
            else
            {
                UsersModel model = new UsersModel();
                model.user_id = txtId.Text;
                model.user_name = txtName.Text;
                model.tel = txtTel.Text;
                model.user_status = "삭제";

                int result = usersRepository.UpdateUser(model);
                if (result == -1)
                {
                    MessageBox.Show(this,"삭제중 에러가 발생 하였습니다.");
                    this.Activate();
                }
                else
                {
                    GetUsers();
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void UsersManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        private void UsersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UsersList.SelectedItems.Count > 0)
            {
                txtId.Text = UsersList.Items[UsersList.SelectedItems[0].Index].SubItems[0].Text;
                txtName.Text = UsersList.Items[UsersList.SelectedItems[0].Index].SubItems[1].Text;
                txtTel.Text = UsersList.Items[UsersList.SelectedItems[0].Index].SubItems[2].Text;
                cbStatus.Text = UsersList.Items[UsersList.SelectedItems[0].Index].SubItems[3].Text;
            }
        }
    }
}
