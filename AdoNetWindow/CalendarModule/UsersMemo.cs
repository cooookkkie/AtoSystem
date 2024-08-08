using AdoNetWindow.Model;
using AppConfiguration;
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
    public partial class UsersMemo : Form
    {
        IMemoRepository memoRepository = new MemoRepository();
        IUsersRepository usersRepository = new UsersRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        int sYear, sMonth, sday;
        bool dayMemo;
        calendar cd;
        MemoModel memoModel;
        UsersModel userModel;

        #region Main Class
        public UsersMemo(int year, int month, int id, calendar cd2, bool dm = false, MemoModel mm = null, UsersModel um = null)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cd = cd2;
            //기타결제 정보
            MemoModel model = memoRepository.GetMemoAsOne(id);
            if (model == null)
            {
                MessageBox.Show(this,"결제정보를 찾을 수 없습니다.");
                return;
            }
            lbid.Text = id.ToString();
            sYear = model.syear;
            sMonth = model.smonth;
            sday = model.sday;
            txtPaymentDate.Text = new DateTime(sYear, sMonth, sday).ToString("yyyy-MM-dd");
            txtManager.Text = model.manager;
            lbUpdatetime.Text = "수정일자 : " + Convert.ToDateTime(model.updatetime.ToString()).ToString("yyyy-MM-dd");
            txtContents.Text = model.contents;
            cbBank.Text = model.pay_bank;
            cbCurrency.Text = model.currency;
            cbPayStatus.Text = model.pay_status;
            txtPayAmount.Text = model.pay_amount.ToString();
            dayMemo = dm;
            if (mm != null)
            {
                string[] bkRGB = mm.backColor.Split(' ');
                string[] ftRGB = mm.fontColor.Split(' ');

                if (bkRGB.Length > 2)
                {
                    btnColor.BackColor = Color.FromArgb(Convert.ToInt32(bkRGB[0]), Convert.ToInt32(bkRGB[1]), Convert.ToInt32(bkRGB[2]));
                }
                if (bkRGB.Length > 2)
                {
                    btnFontColor.BackColor = Color.FromArgb(Convert.ToInt32(ftRGB[0]), Convert.ToInt32(ftRGB[1]), Convert.ToInt32(ftRGB[2]));
                }
                if (!string.IsNullOrEmpty(mm.font))
                {
                    btnFont.Text = mm.font.ToString();
                }
                if (!string.IsNullOrEmpty(mm.font_size))
                {
                    nudSize.Value = Convert.ToInt32(mm.font_size);
                }

                if (mm.font_bold != "1" && mm.font_italic != "1")
                {
                    cbItalic.Text = "보통";
                }
                else if (mm.font_bold != "1" && mm.font_italic == "1")
                {
                    cbItalic.Text = "기울게";
                }
                else if (mm.font_bold == "1" && mm.font_italic != "1")
                {
                    cbItalic.Text = "굵게";
                }
                else if (mm.font_bold == "1" && mm.font_italic == "1")
                {
                    cbItalic.Text = "굵고 기울게";
                }
            }
            memoModel = mm;
            userModel = um;
            if (!string.IsNullOrEmpty(txtPayAmount.Text))
            {
                txtPayAmount.Text = GetN2(txtPayAmount.Text);
            }
            
        }

        public UsersMemo(int year, int month, int days, int id, calendar cd2, bool dm = false, UsersModel um = null)
        {
            InitializeComponent();
            cd = cd2;
            sYear = year;
            sMonth = month;
            sday = days;
            txtPaymentDate.Text = new DateTime(sYear, sMonth, sday).ToString("yyyy-MM-dd");

            dayMemo = dm;
            MemoModel model = memoRepository.GetMemoAsOne(id);
            if (model != null)
            {
                lbid.Text = id.ToString();
                txtManager.Text = model.manager;
                lbUpdatetime.Text = "수정일자 : " + model.updatetime;
                txtContents.Text = model.contents;
            }
            else
            {
                lbid.Text = "0";
                txtManager.Text = um.user_name;
                lbUpdatetime.Text = "수정일자 : " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                cbPayStatus.Text = "미확정";
            }
            memoModel = model;
            userModel = um;
        }

        private void UsersMemo_Load(object sender, EventArgs e)
        {
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.AutoSize = false;
            this.KeyPreview = true;
        }
        #endregion

        #region Key Event
        private void txtPaymentDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tbb = (TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
            }
        }
        private void txtPaymentDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(46) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))
                e.Handled = true;
        }
        private void UsersMemo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        InsertMemo();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        private void txtPayAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Back))
            {
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Delete))
            {
            }
            else if (e.KeyChar == Convert.ToChar(46))
            {
            }
            else if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtPayAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtPayAmount.Text = GetN2(txtPayAmount.Text);
                    txtPayAmount.SelectionStart = txtPayAmount.TextLength; //** 캐럿을 맨 뒤로 보낸다...
                    txtPayAmount.SelectionLength = 0;
                }
                catch (Exception ex)
                { }
            }
        }
        #endregion

        #region Button

        private void btnColor_Click(object sender, EventArgs e)
        {
            Color color = new Color();
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                color = cd.Color;
                this.btnColor.BackColor = color;
            }

        }

        private void btnFontColor_Click(object sender, EventArgs e)
        {
            Color color = new Color();
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                color = cd.Color;
                this.btnFontColor.BackColor = color;
            }            
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.btnFont.Text = fd.Font.Name.ToString();
                nudSize.Text = fd.Font.Size.ToString();

                if (fd.Font.Italic == false && fd.Font.Bold == false)
                {
                    cbItalic.Text = "보통";
                }
                else if (fd.Font.Italic == true && fd.Font.Bold == false)
                {
                    cbItalic.Text = "기울게";
                }
                else if (fd.Font.Italic == false && fd.Font.Bold == true)
                {
                    cbItalic.Text = "굵게";
                }
                else if (fd.Font.Italic == true && fd.Font.Bold == true)
                {
                    cbItalic.Text = "굵고 기울게";
                }
            }
        }        

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteMemo();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertMemo();
        }

        private void btnPaymentDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtPaymentDate.Text = tmpDate.ToString("yyyy-MM-dd");

                sYear = tmpDate.Year;
                sMonth = tmpDate.Month;
                sday = tmpDate.Day;
            }
        }

        
        #endregion

        #region Method
        private string GetN2(string A)
        {
            double B = Convert.ToDouble(A);
            string result;
            if (B == (int)B)
                result = B.ToString("#,##0");
            else
                result = B.ToString("N2");

            return result;
        }
        private void InsertMemo()
        {
            //로그인 유저정보
            UsersModel um = new UsersModel();
            um = usersRepository.GetUserInfo(ConfigurationLgin.LoginUserId);
            if (um == null)
            {
                MessageBox.Show(this,"유저정보를 찾을 수 없습니다.");
                return;
            }
            //유효성검사
            DateTime pay_date;
            double payment_amount;
            int id;
            if (string.IsNullOrEmpty(lbid.Text))
            {
                id = memoRepository.GetNextId();
                lbid.Text = id.ToString();
            }
            else
            {
                id = Convert.ToInt32(lbid.Text);
            }

            if (!(um.department.Contains("관리부") || um.department.Contains("전산") || um.department.Contains("경리") || um.department.Contains("무역")))
            {
                MessageBox.Show(this,"권한이 없습니다.");
                return;
            }
            else if (!DateTime.TryParse(txtPaymentDate.Text, out pay_date))
            {
                MessageBox.Show(this,"결제일자를 다시 선택해주세요.");
                return;
            }
            else if (!double.TryParse(txtPayAmount.Text, out payment_amount))
            {
                MessageBox.Show(this,"결제금액을 다시 입력해주세요.");
                return;
            }
            else if (string.IsNullOrEmpty(cbCurrency.Text))
            {
                MessageBox.Show(this,"결제금액 화폐단위를 선택해주세요.");
                return;
            }
            else if (cbCurrency.Text == "USD" && Convert.ToDouble(txtPayAmount.Text) >= 1000000)
            {
                if (MessageBox.Show(this,"결제금액이 1,000,000 달러를 이상 입력되었습니다. 계속 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            MemoModel model = new MemoModel();

            model.id = id;
            model.currency = this.cbCurrency.Text;
            model.pay_bank = this.cbBank.Text;
            model.pay_amount = payment_amount;
            model.pay_status = cbPayStatus.Text.ToString();
            model.contents = this.txtContents.Text;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            model.manager = txtManager.Text;
            model.user_auth = um.auth_level;
            model.syear = pay_date.Year ;
            model.smonth = pay_date.Month;
            model.sday = pay_date.Day;
            model.backColor = btnColor.BackColor.R.ToString() + " " + btnColor.BackColor.G.ToString() + " " + btnColor.BackColor.B.ToString();
            model.fontColor = btnFontColor.BackColor.R.ToString() + " " + btnFontColor.BackColor.G.ToString() + " " + btnFontColor.BackColor.B.ToString();
            model.font = btnFont.Text.ToString();
            model.font_size = nudSize.Value.ToString();

            if (cbItalic.Text == "보통")
            {
                model.font_bold = "false";
                model.font_italic = "false";
            }
            else if (cbItalic.Text == "기울게")
            {
                model.font_bold = "false";
                model.font_italic = "true";
            }
            else if (cbItalic.Text == "굵게")
            {
                model.font_bold = "true";
                model.font_italic = "false";
            }
            else if (cbItalic.Text == "굵고 기울게")
            {
                model.font_bold = "true";
                model.font_italic = "true";
            }
            int exe;
            //등록
            if (model.id == 0)
            {
                //달메모
                if (dayMemo == false)
                {
                    model.sday = 0;
                    exe = memoRepository.AddMemo(model);
                }
                //일메모
                else
                {
                    model.sday = pay_date.Day;
                    exe = memoRepository.AddMemo(model);
                }

                if (exe != -1)
                {
                    this.Close();
                    cd.displayMemo(sYear, sMonth);
                    cd.displayDays(sYear, sMonth);
                }
                else
                {
                    MessageBox.Show(this,"등록시 에러가 발생하였습니다.");
                }
            }
            //수정
            else
            {
                //달메모
                if (dayMemo == false)
                {
                    exe = memoRepository.UpdateMemo(model);
                }
                //일메모
                else
                {
                    model.sday = pay_date.Day;
                    exe = memoRepository.UpdateMemo(model);
                }

                if (exe != -1)
                {
                    this.Close();
                    cd.displayMemo(sYear, sMonth);
                    cd.displayDays(sYear, sMonth);
                }
                else
                {
                    MessageBox.Show(this,"수정시 에러가 발생하였습니다.");
                }
            }
        }

        private void DeleteMemo()
        {
            if (userModel != null && memoModel != null)
            {
                if (userModel.auth_level < 50)
                {
                    MessageBox.Show(this,"권한이 없습니다.");
                    return;
                }
            }

            if (MessageBox.Show(this,"메모를 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MemoModel model = new MemoModel();
                int id = int.Parse(lbid.Text);

                int exe = memoRepository.DeleteMemo(id);
                if (id == 0 || exe == -1)
                {
                    MessageBox.Show(this,"등록시 에러가 발생하였습니다.");
                }
                else
                {
                    this.Close();
                    cd.displayMemo(sYear, sMonth);
                    cd.displayDays(sYear, sMonth);
                }
            }
        }
        #endregion
    }
}
