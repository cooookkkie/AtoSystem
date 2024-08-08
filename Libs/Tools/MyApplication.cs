using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Tools
{
    public class MyApplication
    {
        public static string Root
        {
            get 
            {
                //현재 어플리케이션 설치경로 리턴
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
