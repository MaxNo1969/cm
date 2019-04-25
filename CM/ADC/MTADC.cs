using Protocol;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace CM
{
    //Управление платой формирования тактирования АЦП
    public class MTADC : IDisposable
    {
        ComPortSettings settings;
        //static string portName = "COM7";
        SerialPort ser = null;
        AutoResetEvent answer = null;
        readonly byte[] buf;
        public MTADC(ComPortSettings _pars)
        {
            settings = _pars;
            #region Логирование 
            {
                string msg = string.Format("Конструктор: {0}", settings.PortName);
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            buf = new byte[2];
            buf[0] = (byte)'!';
            if (Program.cmdLineArgs.ContainsKey("NOCOM"))
            {

            }
            else
            {
                ser = new SerialPort(settings.PortName)
                {
                    BaudRate = settings.BaudRate,
                    Parity = settings.Parity,
                    DataBits = settings.DataBits,
                    StopBits = settings.StopBits,
                    ReadTimeout = settings.ReadIntervalTimeout,
                };
                //ser = new SerialPort("COM7")
                //{
                //    BaudRate = 19200,
                //    Parity = Parity.None,
                //    DataBits = 8,
                //    StopBits = StopBits.One,
                //    ReadTimeout = 500,
                //};
                answer = new AutoResetEvent(false);
                ser.DataReceived += new SerialDataReceivedEventHandler(ser_DataReceived);
                try
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", "Открываем порт" );
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    ser.Open();
                }
                //Указанный порт на текущий экземпляр SerialPort уже открыт.
                catch (InvalidOperationException)
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", "Указанный порт на текущий экземпляр SerialPort уже открыт.");
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    //Всё хорошо
                }
                //Запрещен доступ к порту или порт уже занят другим процессом 
                catch (UnauthorizedAccessException e)
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}({1})", "Запрещен доступ к порту или порт уже занят другим процессом ",e.Message);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    //throw e;
                }
                //Один или несколько свойств для данного экземпляра являются недопустимыми. Например Parity, DataBits, или Handshake свойства не являются допустимыми; BaudRate меньше или равно нулю; ReadTimeout или WriteTimeout свойство меньше нуля и не InfiniteTimeout.
                catch (ArgumentOutOfRangeException e)
                {
                    throw e;
                }
                //Имя порта не начинается с «COM» или тип файла порта не поддерживается.
                catch (ArgumentException e)
                {
                    throw e;
                }
                //Порт указан в недопустимом состоянии или не удалось задать состояние базового порта. 
                //Например, параметры, переданные из этого SerialPort бы недопустимый объект.
                catch (IOException e)
                {
                    throw e;
                }
            }

        }
        public bool started = false;
        void ser_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                answer.Set();
            }
        }
        public string cmd(int _bc)
        {
            buf[1] = (byte)((int)'0' + _bc);
            if (Program.cmdLineArgs.ContainsKey("NOCOM"))
            {
                switch (_bc)
                {
                    case 1:
                        if (started)
                            return "OK1";
                        else
                            return "OK0";
                    case 2:
                        started = false;
                        return "OK2";
                    case 3:
                        started = true;
                        return "OK3";
                    case 4:
                        started = false;
                        return "OK4";
                    case 5:
                        started = false;
                        return "OK5";
                    case 6:
                        started = false;
                        return "224";
                    default:
                        return "???";
                }
            }
            else
            {
                try
                {
                    ser.Write(buf, 0, 2);
                    if (answer.WaitOne(settings.ReadIntervalTimeout))
                    {
                        if (_bc == 3)
                            started = true;
                        else if (_bc == 1)
                        { }
                        else
                            started = false;
                        return ser.ReadLine().Trim();
                    }
                }
                catch (TimeoutException)
                { }
            }
            return "error";
        }

        public void Dispose()
        {
            Log.add("MTADC Dispose()");
            Debug.WriteLine("MTADC Dispose()");
            if (ser != null)
                ser.Close();
            //mtadc = null;
        }
        //public MTADC mtadc = null; 
        public bool start()
        {
            try
            {
                //Проверяем запущено ли тактирование и связь с платой
                string s = cmd(1);
                if(s=="error")
                {
                    throw new Exception("Нет ответа от платы тактирования...");
                }
                #region Логирование 
                {
                    string msg = string.Format("MTADC:start: ->(!1) <-({0})", s);
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
                //Если тактирование не запущено - запускаем
                if (s.Trim() == "OK0")
                {
                    //s = mtadc.cmd(5);
                    //Thread.Sleep(1000);
                    s = cmd(3);
                    #region Логирование 
                    {
                        string msg = string.Format("MTADC:start: ->(!3) <-({0})", s);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion Логирование 
                }
            }
            catch (Exception ex)
            {

                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message );
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");

                }
                #endregion
                return false;
            }
            return true;
        }
        public void stop()
        {
            {
                //Проверяем запущено ли тактирование и связь с платой
                string s = cmd(1);
                #region Логирование 
                {
                    string msg = string.Format("MTADC:start: ->(!1) <-({0})", s);
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion Логирование 
                //Если тактирование запущено - останавливаем
                if (s.Trim() == "OK1")
                {
                    s = cmd(4);
                    #region Логирование 
                    {
                        string msg = string.Format("MTADC:start: ->(!4) <-({0})", s);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion Логирование 
                }
            }
        }

        public override string ToString()
        {
            return string.Format("MTADC: Port={0}",settings.PortName);
        }
    }
} 