using AdoNetWindow.Model;
using AppConfiguration;
using Libs.Tools;
using Repositories;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdoNetWindow.LoginForm
{
    public partial class Logins : ApplicationRoot
    {
        IUsersRepository usersRepository;
        LogManager log;
        LoginCookie cookie;
        //캡쳐방지
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern uint SetWindowDisplayAffinity(IntPtr windowHandle, uint affinity);
        private const uint WDA_NONE = 0;
        private const uint WDA_MONITOR = 1;
        public Logins()
        {
            InitializeComponent();
            usersRepository = new UsersRepository();

            CheckForUpdates();
        }

        private void CheckForUpdates()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    UpdateCheckInfo info = ad.CheckForDetailedUpdate();

                    if (info.UpdateAvailable)
                    {
                        ad.Update();
                        MessageBox.Show("업데이트가 설치되었습니다. 애플리케이션을 다시 시작하세요.", "업데이트", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();
                    }
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("업데이트 파일을 다운로드하는 중 오류가 발생했습니다.\n\n" + dde.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("애플리케이션 배포에 대한 정보가 손상되었습니다.\n\n" + ide.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("알 수 없는 오류가 발생했습니다.\n\n" + ex.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Logins_Load(object sender, EventArgs e)
        {
            //캡쳐방지 해제
            SetWindowDisplayAffinity(this.Handle, WDA_NONE);

            //Log
            log = new LogManager(null, "_Login");
            log.WriteLine("[Begin Processing]-----");
            //Auth Login
            cookie = new LoginCookie(null, "_Cookies");
            /*string[] auth_login = cookie.AuthAccount();
            if (auth_login != null)
            {
                if (auth_login[0] == "True")
                {
                    cbAutoLogin.Checked = true;
                    txtUsername.Text = auth_login[1];
                    txtPassword.Text = auth_login[2];
                    txtPassword.Focus();
                }
            }*/
        }
        #region Button
        //닫기
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "프로그램을 종료하시겠습니까?", "YesOrNo" , MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Log
                log = new LogManager(null, "_Login");
                log.WriteLine("[Close Processing]-----");

                Application.ExitThread();
                try
                {
                    Environment.Exit(0);
                }
                catch
                { 
                
                }
            }
        }
        //로그인
        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginFunc();
            //MessageBox.Show(this,Path.Combine(MyApplication.Root, "Log"));
        }

        //회원가입
        private void btnJoinUs_Click(object sender, EventArgs e)
        {
            AdoNetWindow.Login.JoinUs join = new AdoNetWindow.Login.JoinUs();
            join.Show();
        }
        #endregion

        #region Method
        private bool Validation()
        {
            bool isFlag = false;
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show(this, "Username을 입력해주세요.");
                this.Activate();
                return isFlag;
            }
            else if (!string.IsNullOrEmpty(txtUsername.Text) && (txtUsername.Text.Contains('/') || txtUsername.Text.Contains('"') || txtUsername.Text.Contains('?')))
            {
                MessageBox.Show(this, "Username엔 특수문자를 사용할 수 없습니다.");
                this.Activate();
                return isFlag;
            }
            else if (txtPassword.Text == "")
            {
                MessageBox.Show(this, "비밀번호를 입력해주세요.");
                this.Activate();
                return isFlag;
            }
            return true;
        }
        private void LoginFunc()
        {
            if (!Validation())
            {
                return;
            }
            UsersModel usersModel = usersRepository.GetByUser(txtUsername.Text, txtPassword.Text);
            if (usersModel == null)
            {
                //Log
                log = new LogManager(null, "_Login");
                log.WriteLine("Login Failed (Not found account)");

                MessageBox.Show(this, "일치하는 계정정보가 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                //Seaover 사번없을 경우 
                if (usersModel.seaover_id == null || string.IsNullOrEmpty(usersModel.seaover_id))
                    usersModel.seaover_id = "199993";

                //Log
                log = new LogManager(null, "_Login");
                log.WriteLine("Login Success (" + usersModel.user_id.ToString() + ")");

                //LoginCookie
                cookie = new LoginCookie(null, "_Cookies");
                cookie.Write(cbAutoLogin.Checked, txtUsername.Text, txtPassword.Text);

                //Current login date
                List<StringBuilder> sqlList = new List<StringBuilder>();
                int result = usersRepository.UpdateCurrentDate(usersModel.user_id);
                if (result == -1)
                {
                    MessageBox.Show(this,"로그인 중 에러가 발생하였습니다.");
                    this.Activate();
                    return;
                }
               
                //Run Main Program
                ConfigurationLgin.LoginUserId = usersModel.user_id;
                this.Hide();
                /*Main main = new Main();
                main.Show();
                Program.ac.MainForm = main;*/
                CalendarModule.calendar cal = new CalendarModule.calendar(usersModel.user_id.ToString());
                cal.Show();
                cal.WindowState = FormWindowState.Maximized;
                cal.VersionCheck();
                Program.ac.MainForm = cal;
                this.Close();
            }
        }
        #endregion

        #region Key Event

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginFunc();
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtUsername.Text != "" && txtPassword.Text != "")
                {
                    LoginFunc();
                }
                else
                {
                    txtPassword.Focus();
                }
                
            }
        }
        #endregion
    }
}
