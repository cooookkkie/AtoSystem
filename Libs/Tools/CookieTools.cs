using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Tools
{
    public class CookieTools
    {
        private string _path;

        public CookieTools(string path, string prefix, string postifx)
        {
            _path = path;
            _SetCookiePath(prefix, postifx);
        }
        public CookieTools(string prefix, string postfix)
        /*: this(Path.Combine(MyApplication.Root, "Cookies"),  prefix, postfix)*/
        : this(Path.Combine(@"C:\", "Cookies"), prefix, postfix)
        {
        }

        //인자가 있는 생성자에 현재경로에 Log폴더를 포함한 경로로 다시 생성
        public CookieTools()
            /*: this(Path.Combine(MyApplication.Root, "Cookies"),null, null)*/
            : this(Path.Combine(@"C:\", "Cookies"), null, null)
        {
        }

        private void _SetCookiePath(string prefix, string postfix)
        {
            string path = String.Empty;
            string name = String.Empty;

            _path = Path.Combine(_path, path);
            //경로가 존재하지 않으면 생성(System.IO 를 using 했기때문에 생략)
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            //접두, 접미
            if (!String.IsNullOrEmpty(prefix))
                name = prefix + name;
            if (!String.IsNullOrEmpty(postfix))
                name = name + postfix;

            //_path에 File 이름을 포함
            _path = Path.Combine(_path, name);
        }
        public void FileDelete()
        {
            DirectoryInfo di = new DirectoryInfo(_path);
            if (di.Exists)
            {
                FileInfo[] files = di.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
                Console.WriteLine("Files deleted successfully");
            }
        }
        public string GetSavePath(string postfix)
        { 
            //_path에 File 이름을 포함
            return Path.Combine(_path, postfix);
        }
    }
}
