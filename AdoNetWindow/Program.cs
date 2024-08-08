using AdoNetWindow.LoginForm;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
/*using DevExpress.Maui;*/

namespace AdoNetWindow
{
    static class Program
    {
        

        public static ApplicationContext ac = new ApplicationContext();
        
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseDevExpress()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();*/


            // 프로그램이 관리자 권한으로 실행되지 않았다면, 다시 실행합니다.
            /*if (!IsAdministrator())
            {
                RestartAsAdministrator();
                return;
            }*/


            //중복 프로그램 실행 방지
            /*if (IsExistProcess(Process.GetCurrentProcess().ProcessName))
            {
                MessageBox.Show("이미 실행중입니다!");
                return;
            }
            else*/
            {
                //캘린더 폼을 메인으로 시작
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ac.MainForm = new Logins();
                Application.Run(ac);
            }
        }


        


        static bool IsExistProcess(string processName)
        {
            Process[] process = Process.GetProcesses();
            int cnt = 0;
            //프로세스명으로 확인해서 동일한 프로세스 개수가 2개이상인지 확인합니다. 
            //현재실행하는 프로세스도 포함되기때문에 1보다커야합니다.
            foreach (var p in process)
            {
                if (p.ProcessName == processName)
                    cnt++;
                if (cnt > 1)
                    return true;
            }
            return false;
        }

        // 프로그램이 관리자 권한으로 실행되었는지 확인하는 함수입니다.
        static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        // 프로그램을 관리자 권한으로 다시 실행하는 함수입니다.
        static void RestartAsAdministrator()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Assembly.GetEntryAssembly().CodeBase;
            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas";
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to start the program with administrator privileges.");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
