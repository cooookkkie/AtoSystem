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
    public partial class UserControlMemo : UserControl
    {
        IMemoRepository memoRepository = new MemoRepository();
        calendar cd;
        UsersModel userModel;
        int syear, smonth;
        public UserControlMemo(MemoModel model, int year, int month, calendar cd2, int wdt, UsersModel um)
        {
            InitializeComponent();
            lbId.Text = model.id.ToString();
            lbManager.Text = model.manager;
            setControlSizes(model.contents.ToString());
            this.Width = wdt;
            txtMemo.Width = wdt;
            syear = year;
            smonth = month;
            cd = cd2;
            userModel = um;
        }

        private void setControlSizes(string contetns) 
        {
            int height = 0;
            txtMemo.Text = contetns;
            if (!string.IsNullOrEmpty(contetns))
            {
                string[] lines = contetns.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    height += 15;
                }
            }

            if (height <= 106)
                height = 106;

            this.Height = height + 23;
            txtMemo.Height = height;
        }

        private void lbEdit_Click(object sender, EventArgs e)
        {
            MemoModel mm = new MemoModel();
            mm= memoRepository.GetMemoAsOne(Convert.ToInt32(lbId.Text));

            UsersMemo um = new UsersMemo(syear, smonth, int.Parse(lbId.Text), cd, false, mm, userModel);

            um.StartPosition = FormStartPosition.CenterParent;
            um.Text = "월별 통합메모";
            um.ShowDialog();
        }

        //Delete
        private void label1_Click(object sender, EventArgs e)
        {
            MemoModel mm = memoRepository.GetMemoAsOne(Convert.ToInt32(lbId.Text));

            if (userModel.auth_level >= mm.user_auth)
            {

                if (MessageBox.Show(this,"메모를 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MemoModel model = new MemoModel();
                    int id = int.Parse(lbId.Text);

                    int exe = memoRepository.DeleteMemo(id);
                    if (id == 0 || exe == -1)
                    {
                        MessageBox.Show(this,"등록시 에러가 발생하였습니다.");
                    }
                    else
                    {
                        cd.displayMemo(syear, smonth);
                    }
                    this.Dispose();
                }
            }
            else
            {
                MessageBox.Show(this,"권한이 없습니다.");
            }
        }
    }
}
