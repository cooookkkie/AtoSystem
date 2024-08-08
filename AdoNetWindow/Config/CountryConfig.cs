using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
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
    public partial class CountryConfig : Form
    {
        IConfigRepository configRepository = new ConfigRepository();
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um;
        public CountryConfig(UsersModel um)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.MaximizeBox = false;
            this.um = um;   
        }

        private void CountryConfig_Load(object sender, EventArgs e)
        {
            nupDelivery.Minimum = 0;
            nupDelivery.Maximum = 100;
            GetCountry();
        }


        private void GetCountry()
        {
            countryList.Items.Clear();

            List<CountryModel> model = new List<CountryModel>();
            model = configRepository.GetCountryConfig();
            if (model.Count > 0 )
            {
                for (int i = 0; i < model.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = model[i].country_name.ToString();
                    lvi.SubItems.Add(model[i].delivery_days.ToString());

                    countryList.Items.Add(lvi);
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "설정", "국가별 배송기간", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            string name = txtCountry.Text.ToString();
            int days = Convert.ToInt32(nupDelivery.Value);

            if (name == "")
            {
                MessageBox.Show(this, "국가명을 작성해주세요.");
                this.Activate();
                return;
            }
            else
            {
                int results = -1;
                List<CountryModel> model = new List<CountryModel>();
                model = configRepository.GetCountryConfig(name);
                if (model.Count > 0)
                {
                    results = configRepository.updateCountry(name, days);
                }
                else
                {
                    results = configRepository.InsertCountry(name, days);
                }

                if (results == -1)
                {
                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    GetCountry();
                }
            }
        }

        private void countryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (countryList.SelectedItems.Count != 0)
            {
                int selectRow = countryList.SelectedItems[0].Index;
                txtCountry.Text = countryList.Items[selectRow].SubItems[0].Text;
                nupDelivery.Value = Convert.ToInt32(countryList.Items[selectRow].SubItems[1].Text);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "설정", "국가별 배송기간", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            string name = txtCountry.Text.ToString();
            if (name == "")
            {
                MessageBox.Show(this, "삭제할 국가를 선택해주세요.");
                this.Activate();
                return;
            }
            else
            {
                int results = -1;
                results = configRepository.DeleteCountry(name);

                if (results == -1)
                {
                    MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    GetCountry();
                }

            }
        }
    }
}
