using FormsExtras;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
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
        /// <summary>
        /// Событие при чтении данных с АЦП
        /// </summary>
        public OnDataRead onDataRead = null;
        /// <summary>
        /// Изменилось состояние потока
        /// </summary>
        public StateChanged stateChanged = null;

        public IDataWriter<double> writer;
        public IDataReader<double> reader;
        
        private bool isRunning;

        private Thread thread;
        private IEnumerable<double> data;
        /// <summary>
        /// Блокировка
        /// </summary>
        private readonly object block = new object();
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_lcard">Класс модуля АЦП</param>
        /// <param name="_tube">Абстракция трубы для записи данных</param>
        public ReadDataThread(IDataReader<double> _reader, IDataWriter<double> _writer)
        {
            #region Логирование 
            {
                string msg = string.Format("reader={0},writer={1}", _reader.GetType().Name, _writer.GetType().Name);
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            reader = _reader;
            writer = _writer;
            //thread = new Thread(threadFunc)
            //{
            //    IsBackground = true,
            //    Name = "ReadDataThread",
            //};
            isRunning = false;
        }

        private void threadFunc(object _params)
        {
            while (isRunning)
            {
                //записываем если ещё есть место в буфере
                lock (block)
                {
                    data = reader.Read();
                    if (data != null)
                    {
                        if (writer.Write(data) == -1)
                        {
                            #region Логирование
                            {
                                string msg = string.Format("{0}:Ошибка записи данных.",writer.GetType().Name);
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.warning);
                                Debug.WriteLine(logstr,"Warning");
                            }
                            #endregion 
                        }
                        else
                            onDataRead?.Invoke(data);
                    }
                    else
                    {
                        #region Логирование
                        {
                            string msg = string.Format("{0}:Ошибка чтения данных.", reader.GetType().Name);
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.warning);
                            Debug.WriteLine(logstr, "Warning");
                        }
                        #endregion
                    }
                }
            }
            stateChanged?.Invoke(thread.ThreadState);
        }
        /// <summary>
        /// Запуск потока сбора данных
        /// </summary>
        public bool Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                if (reader.Start() && writer.Start())
                {
                    thread = new Thread(threadFunc)
                    {
                        IsBackground = true,
                        Name = "ReadDataThread",
                    };
                    thread.Start();
                    stateChanged?.Invoke(thread.ThreadState);
                    return true;
                }
                else
                {
                    isRunning = false;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Остановка потока сбора данных с АЦП
        /// </summary>
        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                thread.Join();
                writer.Stop();
                reader.Stop();
                stateChanged?.Invoke(thread.ThreadState);
                thread = null;
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
            stateChanged = null;
            Stop();
        }
    }
}
