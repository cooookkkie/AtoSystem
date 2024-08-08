using AdoNetWindow.LoginForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void 로그아웃ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.Hide();
            Logins login = new Logins();
            login.Show();
            Program.ac.MainForm = login;
            this.Close();
            foreach (Form openForm in Application.OpenForms)
            {
                if(openForm.Name != "Login")
                 { 
                    openForm.Dispose();
                }
            }
            
        }

        private void 캘린더ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //CalendarModule.calendar MdiChildForm = new CalendarModule.calendar();
            //MdiChildForm.MdiParent = this;
            //MdiChildForm.Show();


            /*//폼 중복 열기 방지
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.Name == "Sales") // 열린 폼의 이름 검사
                {
                    if (openForm.WindowState == FormWindowState.Minimized)
                    {  // 폼을 최소화시켜 하단에 내려놓았는지 검사
                        openForm.WindowState = FormWindowState.Normal;

                        openForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);

                    }
                    openForm.Activate();
                    return;
                }
            }
            CalendarModule.calendar wb = new CalendarModule.calendar();  // 폼이 실행되지 않았으면 New Form 객체 생성
                                     // 자식폼으로 값을 넘겨줄 것이 있으면 이 부분에 코드를 추가
            wb.StartPosition = FormStartPosition.Manual;  // 원하는 위치를 직접 지정해서 띄우기 위해
            wb.Location = new Point(this.Location.X + this.Width, this.Location.Y); // 메인폼의 오른쪽에 위치토록
            wb.Show();*/
        }
    }
}
