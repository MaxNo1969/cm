using Protocol;
using System;
using System.Diagnostics;
using System.Threading;

namespace CM
{
    /// <summary>
    /// Состояние потока изменилось
    /// </summary>
    /// <param name="_state"></param>
    public delegate void StateChanged(System.Threading.ThreadState _state);
    /// <summary>
    /// Поток чтения данных с АЦП и записи в трубу
    /// </summary>
    public class ReadDataThread : IDisposable
    {
        public delegate void OnDataRead(double[] _data);
        /// <summary>
        /// Событие при чтении данных с АЦП
        /// </summary>
        public OnDataRead onDataRead = null;
        /// <summary>
        /// Изменилось состояние потока
        /// </summary>
        public StateChanged stateChanged = null;

        IWriteable<double> writer;
        
        private bool isRunning;

        private Thread thread;
        private double[] data;
        /// <summary>
        /// Блокировка
        /// </summary>
        private readonly object block = new object();
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_lcard">Класс модуля АЦП</param>
        /// <param name="_tube">Абстракция трубы для записи данных</param>
        public ReadDataThread(LCard _lcard, Tube _tube)
        {
            string s = GetType().Name + string.Format(": Конструктор");
            log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);
            //size = _size;
            isRunning = false;
            l502 = _lcard;
            tube = _tube;
            //Будем считывать за раз показания по матрице целиком
            //size = tube.settings.matrixSettings.numCols * tube.settings.matrixSettings.numRows;
        }
        /// <summary>
        /// Модуль АЦП (L502). Будем синхронно читать данные в потоке
        /// </summary>
        LCard l502;
        /// <summary>
        /// Труба для записи считанных данных 
        /// </summary>
        Tube tube;
        private void threadFunc(object _params)
        {
            while (isRunning)
            {
                //записываем если ещё есть место в буфере
                lock (block)
                {
                    data = l502.Read();
                    if (data != null)
                    {

                        if (tube.write(data) == -1)
                        {
                            #region Логирование
                            {
                                string msg = "Данные не помещаюся в буфер.";
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                log.add(logstr, LogRecord.LogReason.warning);
                                Debug.WriteLine(logstr);
                            }
                            #endregion 
                            break;
                        }
                        onDataRead?.Invoke(data);
                    }
                }
            }
            string _s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Вышли";
            log.add(_s, LogRecord.LogReason.info);
            stateChanged?.Invoke(thread.ThreadState);
        }
        /// <summary>
        /// Задержка после запуска платы тактирования перед запуском АЦП
        /// </summary>
        const int MTADCWaitTimeout = 200;
        /// <summary>
        /// Запуск потока сбора данных с АЦП
        /// </summary>
        public bool start()
        {
            string s;
            if (!isRunning)
            {
                //Запускаем сбор данных
                if (l502 is LCardVirtual) ((LCardVirtual)l502).index = 0;
                if (!l502.Start())
                {
                    s = string.Format("{0}: {1}: Ошибка: не удалось запустить L502", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    log.add(s, LogRecord.LogReason.error);
                    Debug.WriteLine(s);
                    return false;
                }
                Thread.Sleep(MTADCWaitTimeout);
                //Запускаем тактирование АЦП
                if (!MTADC.MTADC.start())
                {
                    s = string.Format("{0}: {1}: Ошибка: не удалось запустить плату тактирования АЦП", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    log.add(s, LogRecord.LogReason.error);
                    Debug.WriteLine(s);
                    return false;
                }
                isRunning = true;
                tube.reset(true);
                thread = new Thread(threadFunc)
                {
                    IsBackground = true,
                    Name = "ReadDataThread",
                };
                thread.Start();
                stateChanged?.Invoke(thread.ThreadState);
                s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                log.add(s, LogRecord.LogReason.info);
                Debug.WriteLine(s);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Остановка потока сбора данных с АЦП
        /// </summary>
        public void stop()
        {
            if (isRunning)
            {
                isRunning = false;
                thread.Join();
                MTADC.MTADC.stop();
                l502.Stop();
                stateChanged?.Invoke(thread.ThreadState);
                stateChanged = null;
                thread = null;
                string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                log.add(s, LogRecord.LogReason.info);
                Debug.WriteLine(s);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Очистка
        /// </summary>
        public void Dispose()
        {
            onDataRead = null;
            stop();
        }
    }
}
