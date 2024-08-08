using AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs
{
    public class ClassRoot
    {
        protected ConfigurationMgr1 dbInstance;
        protected ConfigurationMgr2 dbInstance2;
        protected ConfigurationMgr3 dbInstance3;
        protected ConfigurationWebFtpMgr ftpInstance;

        public ClassRoot()
        {
            dbInstance = ConfigurationMgr1.Instance();
            dbInstance2 = ConfigurationMgr2.Instance();
            dbInstance3 = ConfigurationMgr3.Instance();
            ftpInstance = ConfigurationWebFtpMgr.Instance();
        }

        public string AddSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            if (InputTxt == null)
                return String.Empty;

            string Result = InputTxt;
            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }
            Result = Result.Replace("\n", @"\n").Replace("\r", @"\r");

            return Result.Trim();
        }

        public static string StripSlashes(string InputTxt)
        {

            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }
            return Result;
        }

    }
}
