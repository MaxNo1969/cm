using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM
{
    public static class CmdLineHelper
    {
        public static Dictionary<string,string> getCmdLineParameters(string[] _args)
        {
            Dictionary<string, string> cmdStr = new Dictionary<string, string>();
            if ((_args == null) || _args.Length < 1)
            {
                cmdStr.Add("NONE", "true");
                return cmdStr;
            }
            foreach (string s in _args)
            {
                string[] ss = s.Split(new char[] { ':' });
                if (ss[0][0] != '/')
                {
                    throw new ArgumentException("Ошибка аргументов командной строки");
                }
                string paramName = ss[0].Substring(1);
                cmdStr[paramName] = (ss.Length > 1) ? ss[1] : "true";
            }
            return cmdStr;
        }
    }
}
