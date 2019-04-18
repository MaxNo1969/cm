using System.Diagnostics;
using System.Threading;
using Protocol;

namespace CM
{
    /// <summary>
    /// Поток для эмуляции движения трубы 
    /// </summary>
    public class TubeMoveThread
    {
        PhysTube ptube = null;
        SignalListDef sl = Program.signals;
        Thread moveTubeThread = null;
        bool isMoving = false;
        /// <summary>
        /// Флаг окончания трубы
        /// </summary>
        public bool bEndOfTube = false;

        /// <summary>
        /// Делегат движения трубы
        /// </summary>
        /// <param name="_tm">Труба</param>
        public delegate void OnTubeMove(PhysTube _tm);
        /// <summary>
        /// Труба начала движение
        /// </summary>
        public event OnTubeMove tubeStart = null;
        /// <summary>
        /// Труба продвинулась
        /// </summary>
        public event OnTubeMove tubeMove = null;
        /// <summary>
        /// Труба закончила движение
        /// </summary>
        public event OnTubeMove tubeEnd = null;

        /// <summary>
        /// Через какое количество осчетов будем генерировать событие
        /// </summary>
        private const int eventFreq = 100;

        /// <summary>
        /// Блокировка
        /// </summary>
        readonly object block = new object();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">Труба</param>
        public TubeMoveThread(PhysTube _ptube)
        {
            ptube = _ptube;
            //moveTubeThread = new Thread(moveTubeThreadFunc);
            isMoving = false;
        }

        /// <summary>
        /// Запуск потока эмуляции движения трубы
        /// </summary>
        public void start()
        {
            if (isMoving) return;
            moveTubeThread = new Thread(moveTubeThreadFunc)
            {
                Name = "MoveTubeThread"
            };
            isMoving = true;
            bEndOfTube = false;
            ptube.startReadX = 0;
            moveTubeThread.Start();
        }

        /// <summary>
        /// Остановка потока эмуляции движения трубы
        /// </summary>
        public void stop()
        {
            if (!isMoving) return;
            isMoving = false;
            moveTubeThread.Join();
        }

        private void moveTubeThreadFunc()
        {
            #region Логирование 
            {
                string msg = string.Format("{0}", "Запуск" );
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            tubeStart?.Invoke(ptube);
            sl.set(sl.iSTRB,true);
            while (isMoving)
            {
                if (ptube.startReadX < ptube.Width)
                {
                    if (ptube.l2px(ptube.startReadX) % Program.settings.ZoneSize == 0)
                    {
                        sl.set(sl.iSTRB, true);
                    }
                    lock (block)
                    {
                        ptube.startReadX++;
                    }
                    Thread.Sleep(ptube.cellTime);
                    tubeMove?.Invoke(ptube);
                }
                else
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", "Труба вышла из установки");
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");

                    }
                    #endregion
                    tubeEnd?.Invoke(ptube);
                    isMoving = false;
                    bEndOfTube = true;
                    moveTubeThread = null;
                    break;
                }
            }
        }
    }
}
