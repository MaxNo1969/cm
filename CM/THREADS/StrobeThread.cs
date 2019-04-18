using Protocol;
using System.Diagnostics;
using System.Threading;

namespace CM
{
    /// <summary>
    /// Класс для обработки стробов с установки по границам зон
    /// </summary>
    public class StrobeThread
    {
        Tube tube = null;
        SignalListDef sl = Program.signals;
        Thread strobeTh = null;
        bool isRunning = false;

        /// <summary>
        /// Делегат прихода строба 
        /// </summary>
        /// <param name="_ind">Номер строба</param>
        public delegate void OnStrobe(int _ind);
        /// <summary>
        /// Событие при приходе очередного строба
        /// </summary>
        public event OnStrobe strobeRise = null;
        /// <summary>
        /// Задерка цикла
        /// </summary>
        private const int strobeDelay = 1000;

        /// <summary>
        /// Блокировка
        /// </summary>
        private readonly object block = new object();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">Труба</param>
        public StrobeThread(Tube _tube)
        {
            tube = _tube;
            //moveTubeThread = new Thread(moveTubeThreadFunc);
            isRunning = false;
        }

        /// <summary>
        /// Запускаем прием стробов
        /// </summary>
        public void start()
        {
            if (isRunning) return;
            #region Логирование
            {
                string msg = "";
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr);
            }
            #endregion
            tube.rtube.strobes.Clear();
            strobeTh = new Thread(strobeThreadFunc)
            {
                Name = "StrobeThread",
            };
            isRunning = true;
            strobeTh.Start();
        }
        /// <summary>
        /// Останавливаем прием стробов
        /// </summary>
        public void stop()
        {
            if (!isRunning) return;
            #region Логирование
            {
                string msg = "";
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr);
            }
            #endregion 
            isRunning = false;
            strobeTh.Join();
        }


        private void strobeThreadFunc()
        {
            string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Вход";
            Log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);

            while (isRunning)
            {
                s = sl.iSTRB.Wait(true,strobeDelay);
                if (s == "Ok")
                {
                    sl.set(sl.iSTRB, false);
                    int ind = tube.addStrobe();
                    strobeRise?.Invoke(ind);
                }
                Thread.Sleep(10);
            }
        }
    }
}
