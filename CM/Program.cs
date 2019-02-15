using FPS;
using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM
{
    static class Program
    {
        public static Dictionary<string, string> cmdLineArgs;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                FormPosSaver.deser();
                cmdLineArgs = CmdLineHelper.getCmdLineParameters(args);
                #region Логирование 
                {
                    string msg = string.Format("{0}", @"Program: Начало выполнения программы.");
                    string logstr = string.Format("{0}: {1}: {2}", @"Program", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
                Application.Run(new FRMain());
            }
            catch (Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message);
                    string logstr = string.Format("{0}: {1}: {2}", "Program", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");

                }
                #endregion
            }
            finally
            {
                FormPosSaver.ser();
            }
        }
    }
}
