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

namespace AdoNetWindow.Arrive
{
    public partial class ArriveMemo : UserControl
    {
        IMemoArriveRepository memoArriveRepository = new MemoArriveRepository();
        MemoArriveModel mam;
        UsersModel um;
        ArriveInfo ai;
        public ArriveMemo(MemoArriveModel mamodel, ArriveInfo arriveinfo, UsersModel umodel)
        {
            InitializeComponent();
            mam = mamodel;
            um = umodel;
            ai = arriveinfo;
            
        }

        private void ArriveMemo_Load(object sender, EventArgs e)
        {
            SetMemo();
        }

        private void SetMemo()
        {
            if (mam != null)
            {

                string[] contents = mam.content.ToString().Split('\n');
                if (contents.Length > 3)
                {
                    this.lbContent.Height = Convert.ToInt32(13.3 * contents.Length);
                    this.Height = Convert.ToInt32(13.3 * contents.Length) + 7;
                }

                this.lbContent.Text = mam.content.ToString();
                this.lbManager.Text = mam.edit_name.ToString();
                this.lbEditDate.Text = mam.edit_date.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbManager.Text == um.user_name)
            {
                if (MessageBox.Show(this, "메모를 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (mam != null)
                    {
                        int results = memoArriveRepository.DeleteMemo(mam.id, mam.sub_id, mam.content, mam.edit_name);
                        if (results == -1)
                        {
                            MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                        }
                        else
                        {
                            ai.GetArriveMemo();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "본인 메모만 삭제할 수 있습니다.");
            }
        }
    }
}
