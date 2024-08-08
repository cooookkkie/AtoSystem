using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Libs.Tools
{
    public enum LogType { Daily, Monthly }
    public class LogManager
    {
        private string _path;

        public LogManager(string path, LogType logType, string prefix, string postifx)
        {
            _path = path;
            _SetLogPath(logType, prefix, postifx);
        }
        public LogManager(string prefix, string postfix)
            : this(Path.Combine(MyApplication.Root, "Log"), LogType.Daily, prefix, postfix)
        {

        }

        //인자가 있는 생성자에 현재경로에 Log폴더를 포함한 경로로 다시 생성
        public LogManager()
            : this(Path.Combine(MyApplication.Root, "Log"), LogType.Daily, null, null)
        {
        }

        private void _SetLogPath(LogType logType, string prefix, string postfix)
        {
            string path = String.Empty;
            string name = String.Empty;

            switch (logType)
            {
                case LogType.Daily:
                    path = String.Format(@"{0}\{1}\", DateTime.Now.Year, DateTime.Now.ToString("MM"));
                    name = DateTime.Now.ToString("yyyyMMdd");
                    break;
                case LogType.Monthly:
                    path = String.Format(@"{0}\", DateTime.Now.Year);
                    name = DateTime.Now.ToString("yyyyMM");
                    break;
            }

            _path = Path.Combine(_path, path);
            //경로가 존재하지 않으면 생성(System.IO 를 using 했기때문에 생략)
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            //접두, 접미
            if (!String.IsNullOrEmpty(prefix))
                name = prefix + name;
            if (!String.IsNullOrEmpty(postfix))
                name = name + prefix;
            //확장자
            name += ".txt";

            //_path에 File 이름을 포함
            _path = Path.Combine(_path, name);
        }

        public void Write(string data)
        {
            try
            {
                //경로의 파일을 열어서 data 기록, 없으면 생성(true)
                using (StreamWriter writer = new StreamWriter(_path, true))
                {
                    writer.Write(data);
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void WriteLine(string data)
        {
            try
            {
                //경로의 파일을 열어서 data 기록, 없으면 생성(true), + 줄바꿈
                using (StreamWriter writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyyMMdd HH:mm:ss\t") + data);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
