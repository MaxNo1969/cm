using Protocol;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace CM
{
    /// <summary>
    /// Обмен по протоколу MODBUS
    /// </summary>
    public class ModBus : IDisposable
    {
        SerialPort ser;
        AutoResetEvent answer = null;
        /// <summary>
        /// Настройки порта RS485
        /// </summary>
        ComPortSettings settings;
        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string err { get; private set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        public ModBus(ComPortSettings _comPort)
        {
            settings = _comPort;
            #region Логирование 
            {
                string sEmul = (Program.cmdLineArgs.ContainsKey("NOCOM")) ? "(EMUL)" : string.Empty;
                string msg = string.Format("{0}{1}", _comPort.ToString(), sEmul);
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            if (Program.cmdLineArgs.ContainsKey("NOCOM"))
            {

            }
            else
            {
                ser = new SerialPort(settings.PortName)
                {
                    BaudRate = settings.BaudRate,
                    DataBits = settings.DataBits,
                    Parity = settings.Parity,
                    StopBits = settings.StopBits,
                    ReadTimeout = settings.ReadIntervalTimeout,
                };
                //ser = new SerialPort("COM8")
                //{
                //    BaudRate = 9600,
                //    DataBits = 8,
                //    Parity = Parity.None,
                //    StopBits = StopBits.One,
                //    ReadTimeout = 100,
                //};
                answer = new AutoResetEvent(false);
                ser.DataReceived += new SerialDataReceivedEventHandler(ser_DataReceived);
                try
                {
                    ser.Open();
                }
                //Указанный порт на текущий экземпляр SerialPort уже открыт.
                catch (InvalidOperationException)
                {
                    //Всё хорошо
                }
                //Запрещен доступ к порту или порт уже занят другим процессом 
                catch (UnauthorizedAccessException e)
                {
                    //throw e;
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", e.Message );
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr, "Error");

                    }
                    #endregion
                }
                //Один или несколько свойств для данного экземпляра являются недопустимыми. Например Parity, DataBits, или Handshake свойства не являются допустимыми; BaudRate меньше или равно нулю; ReadTimeout или WriteTimeout свойство меньше нуля и не InfiniteTimeout.
                catch (ArgumentOutOfRangeException e)
                {
                    err = e.Message;
                    throw e;
                }
                //Имя порта не начинается с «COM» или тип файла порта не поддерживается.
                catch (ArgumentException e)
                {
                    err = e.Message;
                    throw e;
                }
                //Порт указан в недопустимом состоянии или не удалось задать состояние базового порта. 
                //Например, параметры, переданные из этого SerialPort бы недопустимый объект.
                catch (IOException e)
                {
                    err = e.Message;
                    throw e;
                }
            }
        }
        void ser_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                answer.Set();
            }
        }
        /// <summary>
        /// Очистка
        /// </summary>
        public void Dispose()
        {
            #region Логирование
            {
                string msg = "";
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr);
            }
            #endregion 
            if (ser != null)
                ser.Dispose();
            ser = null;
        }

        bool ReadInputRegister(byte _cmd, int _abonent, int _pos, out ushort _result)
        {
            // 0 - абонент
            // 1 - ошибки
            // 2 - длина / код ошибки
            // 3 - данные
            // 4 - данные
            // 5 - crc
            // 6 - crc
            byte[] query = new byte[] { Convert.ToByte(_abonent), _cmd, 0, Convert.ToByte(_pos), 0, 1, 0, 0 };
            Crc16.Add(query);
            _result = 0;
            if (Program.cmdLineArgs.ContainsKey("NOCOM"))
            {
                return true;
            }
            else
            {
                try
                {
                    #region Логирование 
                    {
                        string msg = string.Format("{0}", string.Format("SEND:{0}", print(query)));
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    ser.Write(query, 0, query.Length);
                    //Thread.Sleep(1000);
                    WaitHelper.Wait(1);
                    if (answer.WaitOne(settings.ReadIntervalTimeout))
                    {
                        // 0 - абонент
                        // 1 - ошибки
                        // 2 - длина / код ошибки
                        // 3 - данные
                        // 4 - данные
                        // 5 - crc
                        // 6 - crc
                        int bytesToRead = ser.BytesToRead;
                        byte[] packet = new byte[7];
                        int count = ser.Read(packet, 0, packet.Length);
                        #region Логирование 
                        {
                            string msg = string.Format(string.Format("RECV:{0}(count={1}, BytesToRead={2}", print(packet),count,bytesToRead));
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Message");
                        }
                        #endregion
                        if (count != 7)
                        {
                            err = "Не смогли прочитать";
                            #region Логирование 
                            {
                                string msg = string.Format("{0},{1}", err, count );
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.info);
                                Debug.WriteLine(logstr, "Message");
                            }
                            #endregion
                            return false;
                        }
                        if (packet[0] != _abonent)
                        {
                            err = "Не тот абонент";
                            #region Логирование 
                            {
                                string msg = string.Format("{0}", err);
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.info);
                                Debug.WriteLine(logstr, "Message");
                            }
                            #endregion
                            return false;
                        }
                        if ((packet[1] & 0x80) != 0)
                        {
                            err = "Ошибка в ответе: " + packet[2].ToString();
                            #region Логирование 
                            {
                                string msg = string.Format("{0}", err);
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.info);
                                Debug.WriteLine(logstr, "Message");
                            }
                            #endregion
                            return false;
                        }
                        if (packet[2] != 7)
                        {
                            err = "Не верная длина в ответе";
                            #region Логирование 
                            {
                                string msg = string.Format("{0}", err);
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.info);
                                Debug.WriteLine(logstr, "Message");
                            }
                            #endregion
                            return false;
                        }
                        if (Crc16.Check(packet))
                        {
                            err = "Не верная контрольная сумма";
                            #region Логирование 
                            {
                                string msg = string.Format("{0}", err);
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.info);
                                Debug.WriteLine(logstr, "Message");
                            }
                            #endregion
                            return false;
                        }
                        _result = BitConverter.ToUInt16(new byte[2] { packet[6], packet[5] }, 0);
                        return true;
                    }
                    else
                    {
                        err = "Таймут чтения";
                        #region Логирование 
                        {
                            string msg = string.Format("{0}", err);
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Message");
                        }
                        #endregion
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    #region Логирование
                    {
                        string msg = string.Format("Ошибка:{0}", ex.Message);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr);
                    }
                    #endregion
                    err = ex.Message;
                    return false;
                }
            }
        }

        public static string print(byte[] _data)
        {
            string ret = string.Empty;
            foreach (byte b in _data)
                ret = ret + b.ToString() + " ";
            return ret;
        }

        /// <summary>
        /// Установка регистра
        /// </summary>
        /// <param name="_abonent">Абонент</param>
        /// <param name="_pos">Номер регистра</param>
        /// <param name="_data">Записываемое значение</param>
        /// <returns>true - запись удалась</returns>
        public bool setSingleRegister(int _abonent, int _pos, ushort _data)
        {
            byte[] query = new byte[] { Convert.ToByte(_abonent), 6, 0, Convert.ToByte(_pos), Convert.ToByte((_data >> 8) & 0xff), Convert.ToByte(_data & 0xff), 0, 0 };
            Crc16.Add(query);
            if (Program.cmdLineArgs.ContainsKey("NOCOM"))
            {
                return true;
            }
            else
            {
                try
                {
                    #region Логирование 
                    {
                        string msg = string.Format("SEND:{0}", print(query));
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    ser.Write(query, 0, query.Length);
                    //Thread.Sleep(1000);
                    WaitHelper.Wait(1);
                    if (answer.WaitOne(settings.ReadIntervalTimeout))
                    {
                        // 0 - абонент
                        // 1 - ошибки
                        // 2 - код ошиибки/регистр
                        // 3 - регистр
                        // 4 - 255
                        // 5 - 255
                        // 6 - crc
                        // 7 - crc
                        byte[] packet = new byte[8];
                        int bytesToRead = ser.BytesToRead;
                        int count = ser.Read(packet, 0, packet.Length);
                        #region Логирование 
                        {
                            string msg = string.Format(string.Format("RECV:{0}(count={1}, BytesToRead={2}", print(packet), count, bytesToRead));
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Message");
                        }
                        #endregion
                        if (count != 8)
                        {
                            err = "Не смогли прочитать";
                            #region Логирование 
                            {
                                string msg = string.Format("{0}", err);
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.info);
                                Debug.WriteLine(logstr, "Message");
                            }
                            #endregion
                            return false;
                        }
                        if (packet[0] != _abonent)
                        {
                            err = "Не тот абонент";
                            #region Логирование 
                            {
                                string msg = string.Format("{0}", err);
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.info);
                                Debug.WriteLine(logstr, "Message");
                            }
                            #endregion
                            return false;
                        }
                        if ((packet[1] & 0x80) != 0)
                        {
                            err = "Ошибка в ответе: " + packet[2].ToString();
                            #region Логирование 
                            {
                                string msg = string.Format("{0}", err);
                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                Log.add(logstr, LogRecord.LogReason.info);
                                Debug.WriteLine(logstr, "Message");
                            }
                            #endregion
                            return false;
                        }
                        //if (Crc16.Check(packet))
                        //{
                        //    err = "Не верная контрольная сумма";
                        //    #region Логирование 
                        //    {
                        //        string msg = string.Format("{0}", err);
                        //        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        //        Log.add(logstr, LogRecord.LogReason.info);
                        //        Debug.WriteLine(logstr, "Message");
                        //    }
                        //    #endregion
                        //    return false;
                        //}
                        return true;
                    }
                    else
                    {
                        err = "Таймут чтения";
                        #region Логирование 
                        {
                            string msg = string.Format("{0}", err);
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Message");
                        }
                        #endregion
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    #region Логирование
                    {
                        string msg = string.Format("Ошибка:{0}", ex.Message);
                        string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr);
                    }
                    #endregion
                    err = ex.Message;
                    return false;
                }
            }
        }
        /// <summary>
        /// Читаем входной регистр (команда 4)
        /// </summary>
        /// <param name="_abonent"></param>
        /// <param name="_pos"></param>
        /// <param name="_result"></param>
        /// <returns></returns>
        public bool ReadInputRegisterE(int _abonent, int _pos, out ushort _result)
        {
            return ReadInputRegister(4, _abonent, _pos, out _result);
        }
        /// <summary>
        /// Читаем входной регистр (команда 3)
        /// </summary>
        public bool ReadHoldingRegisterE(int _abonent, int _pos, out ushort _result)
        {
            return ReadInputRegister(3, _abonent, _pos, out _result);
        }
    }
}
