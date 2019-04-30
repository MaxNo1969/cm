using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormsExtras;
using Protocol;

namespace CM
{
    static class Program
    {
        public const string className = "Program";
        public static Dictionary<string, string> cmdLineArgs;
        public static AppSettings settings = null;
        public static Tube tube;
        public static int tubeCount;
        public static LCard lCard;
        public static MTADC mtdadc;
        public static int mtdadcFreq;
        public static SignalListDef signals;
        public static Rectifier rectifier;
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
                    string msg = @"Начало выполнения программы.";
                    string logstr = string.Format("{0}: {1}: {2}", className, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
                settings = AppSettingsSerialization.load(DefaultValues.defaultAppSettingsFileName);
                rectifier = new Rectifier(new ModBus(Program.settings.rectifierSettings.Port));
                if(cmdLineArgs.ContainsKey("NOLCARD"))
                    lCard = new LCardVirtual(settings.lCardSettings);
                else
                    lCard = new LCardReal(settings.lCardSettings);
                //Инициализация PCIE1730 и списка обрабатываемых сигналов
                signals = new SignalListDef();
                //Включаем питание датчиков
                signals.oPOWER.Val = true;
                WaitHelper.Wait(1000);
                //Инициализируем П-217
                mtdadc = new MTADC(Program.settings.mtadcSettings.port);
                //Получаем частоту внешней синхронизации
                int cmd6res = Convert.ToInt32(mtdadc.cmd(6));
                if (Program.settings.mtadcSettings.Freq == 0) Program.settings.mtadcSettings.Freq = 20000000;
                double T = 1 / (double)Program.settings.mtadcSettings.Freq;
                double t = T * (cmd6res + 1);
                mtdadcFreq = (int)(1 / t);
                //mtdadcFreq = 44440;
                #region Логирование 
                {
                    string msg = string.Format("mtdadcFreq = {0}", mtdadcFreq );
                    string logstr = string.Format("{0}: {1}: {2}", className, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
                if (settings.Current.Name != "Новый")
                    tube = new Tube(settings.Current, settings.TubeLen);
                else
                    tube = new Tube(settings.TypeSizes[0], settings.TubeLen);
                tubeCount = 0;
                Application.Run(new FRMain());
            }
            catch (Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message);
                    string logstr = string.Format("{0}: {1}: {2}", "Program", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");
                    Debug.Write(ex.StackTrace);

                }
                #endregion
            }
            finally
            {
                //Выключаем питание датчиков
                signals.oPOWER.Val = false;
                signals.Dispose();
                FormPosSaver.ser();
                AppSettingsSerialization.save(settings, DefaultValues.defaultAppSettingsFileName);
            }
        }
    }
}
