using AdoNetWindow.Model;
using Repositories;
using Repositories.Common;
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
    public partial class AddGroup : Form
    {
        IGroupItemRepository groupItemRepository = new GroupItemRepository();
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um;
        GroupManager gm;
        public AddGroup(UsersModel uModel, GroupManager gManager)
        {
            InitializeComponent();
            um = uModel;
            gm = gManager;
            txtManager.Text = um.user_name;
            txtGroupName.Focus();
        }

        #region Method
        private void InsertGroup()
        {
            if (string.IsNullOrEmpty(cbDivision.Text.Trim()))
            {
                MessageBox.Show(this, "구분을 선택해주세요.");
                this.Activate();
                return;
            }
            if (string.IsNullOrEmpty(txtGroupName.Text.Trim()))
            {
                MessageBox.Show(this, "그룹명을 입력해주세요.");
                this.Activate();
                return;
            }

            GroupItemModel model = new GroupItemModel();
            model.id = commonRepository.GetNextId("t_group_item", "id");
            model.sub_id = 0;
            model.division = cbDivision.Text;
            model.manager = txtManager.Text;
            model.group_name = txtGroupName.Text;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
            model.edit_user = um.user_name;

            StringBuilder sql = groupItemRepository.InsertSql(model);
            List<StringBuilder> sqlList = new List<StringBuilder>();
            sqlList.Add(sql);

            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                this.Dispose();
                gm.GetGroup();
                gm.GetItem(model.id.ToString(), true);
            }
        }
        #endregion


        #region Button Click
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            InsertGroup();
        }
        #endregion

        #region Key event
        private void AddGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        InsertGroup();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        #endregion
    }
}
