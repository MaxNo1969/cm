using Protocol;
using System;
using System.Diagnostics;
using System.Threading;
using x502api;

namespace CM
{
    class LCardReal : LCard, IDisposable
    {
        // ! Хендл платы
        X502 hnd; /* Описатель модуля с которым работаем (null, если связи нет) */

        protected override void LoadMainSettings()
        {
            base.LoadMainSettings();
            hnd.LChannelCount = (uint)numMainSensors;
            for (int i = 0; i < numMainSensors; i++)
            {
                hnd.SetLChannel((uint)i, (uint)settings.Channels[i].logicalChannel,
                    (X502.LchMode)settings.Channels[i].collectedMode,
                    (X502.AdcRange)settings.Channels[i].range, 0);
            }
            // Настраиваем источник частоты синхронизации
            hnd.SyncMode = (X502.Sync)settings.SyncMode;
            // Настраиваем  источник запуска сбора
            hnd.SyncStartMode = (X502.Sync)settings.SyncStartMode;
            #region Логирование 
            {
                string msg = string.Format("hnd.SyncMode={0} hnd.SyncStartMode={1}", settings.SyncMode, settings.SyncStartMode);
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            double f_acq = settings.FrequencyPerChannel * numMainSensors;
            double f_lch = settings.FrequencyPerChannel;
            #region Логирование 
            {
                string msg = string.Format("f_acq={0} f_lch={1}", f_acq, f_lch);
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            // настраиваем частоту сбора с АЦП
            if (hnd.SetAdcFreq(ref f_acq, ref f_lch)!= lpcieapi.lpcie.Errs.OK)
            {
                string s = "LCardReal: " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + "Ошибка установки частоты АЦП";
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
            }
            //settings.frequencyCollect = f_acq;
            //settings.frequencyPerChannel = f_lch;
            // Записываем настройки в модуль
            if(hnd.Configure(0) != lpcieapi.lpcie.Errs.OK)
            {
                string s = "LCardReal: " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + "Ошибка записи настроек в модуль";
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
            }
        }

        public LCardReal(LCardSettings _pars) : base(_pars)
        {
            #region Логирование
            {
                string logstr = string.Format("{0}: {1}", "LCardReal", "Конструктор");
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr);
            }
            #endregion
            //1.При работе с модулями по интерфейсам PCI-Express или USB получить
            //список серийных номеров с помощью функций L502_GetSerialList() и
            //E502_UsbGetSerialList(), соответственно, или получить список записей о модулях
            //с помощью L502_GetDevRecordsList() и E502_UsbGetDevRecordsList(). При
            //работе по Ethernet можно использовать функции обнаружения устройств в
            //локальной сети, описанные в главе Обнаружение модулей в локальной сети , или,
            //если известен IP-адрес устройства, перейти сразу к пункту 2.         
            int res = L502.GetDevRecordsList(out X502.DevRec[] devrecs, 0);
            //Не одного модуля не найдено
            if (res == 0)
            {
                string s = "LCardReal: " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + "Не найдено подключеных модулей";
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
                throw new Exception(s);
            }
            //Ошибка при получении списка подключеных модулей
            if (res < 0)
            {
                string s = "LCardReal: Конструктор: Ошибка создания описателя модуля " + devrecs[0].DevName;
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
            }
            //2. Если в системе присутствует нужный модуль, создать описатель модуля с помощью X502_Create().
            //по идее у нас только одна плата, поэтому запись по ней будет в devrecs[0] 
            hnd = X502.Create(devrecs[settings.DevNum].DevName);
            if (hnd == null)
            {
                string s = "LCardReal: Конструктор: Ошибка при получении списка подключеных модулей";
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
            }
            //3. Установить соединение с модулем. При использовании записей о устройстве от-
            //крытие всегда выполняется с помощью X502_OpenByDevRecord(). При исполь-
            //зовании серийного номера используются L502_Open() или E502_OpenUsb() для
            //L502 и E502, подключенного по USB, соответственно. Чтобы установить связь с
            //модулем по Ethernet с использованием IP-адреса, необходимо использовать функ-
            //цию E502_OpenByIpAddr().
            if (hnd.Open(devrecs[settings.DevNum]) != lpcieapi.lpcie.Errs.OK)
            {
                string s = "LCardReal: Конструктор: Ошибка открытия модуля " + devrecs[0].DevName;
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
            }
        }
        public override bool Start()
        {
            //Если карта уже работает, то ничего не делаем 
            if (IsStarted) return IsStarted;
            IsStarted = false;
            //Загружаем параметры для основного АЦП 
            LoadMainSettings();
            if (hnd.StreamsEnable(X502.Streams.ADC) != lpcieapi.lpcie.Errs.OK)
            {
                string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + "не смогли разрешить потоки";
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
                return IsStarted;
            }
            if (hnd.StreamsStart() != lpcieapi.lpcie.Errs.OK)
            {
                string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + "не смогли стартовать потоки";
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
                return IsStarted;
            }
            mtdadc.start();
            IsStarted = true;
            return IsStarted;
        }
        public override bool Stop()
        {
            //Если карта не работает, то ничего не делаем 
            if (!IsStarted) return true;
            mtdadc.stop();
            if (hnd.StreamsStop() != lpcieapi.lpcie.Errs.OK)
            {
                string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + "не смогли остановить потоки";
                Log.add(s, LogRecord.LogReason.error);
                Debug.WriteLine(s);
                return false;
            }
            IsStarted = false;
            return true;
        }
        uint firstLch;

        public override double[] Read()
        {
            //uint count = hnd.RecvReadyCount;
            //#region Логирование 
            //{
            //    string msg = string.Format("{0}", count );
            //    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
            //    Log.add(logstr, LogRecord.LogReason.info);
            //    Debug.WriteLine(logstr, "Message");
            //}
            //#endregion
            ////if (count == 0) return null;
            //if (count < raw_size)
            //{
            //    string s = string.Format("{0}: {1}: В буфере нет необходимого объёма данных: Ожидем {2}, Сейчас {3}",
            //        GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, raw_size, count);
            //    Log.add(s, LogRecord.LogReason.info);
            //    Debug.WriteLine(s);
            //    return (null);
            //}
            //SetRawSize((uint)_size);
            int rcv_size = hnd.Recv(rawi, raw_size, RECV_TOUT);
            /* значение меньше нуля означает ошибку... */
            /*  получаем номер лог. канала, соответствующий первому
	            отсчету АЦП, так как до этого могли обработать
	            некратное количество кадров */
            firstLch = hnd.NextExpectedLchNum;
            //if (rcv_size != raw_size)
            //{
            //    string s = string.Format("{0}: {1} : Размер полученный: {2} не равен размеру запрошенному: {3}",
            //        GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, rcv_size, raw_size);
            //    Log.add(s, LogRecord.LogReason.info);
            //    Debug.WriteLine(s);
            //    return (null);
            //}
            // переводим АЦП в Вольты
            uint rcv_size1 = (uint)rcv_size;
            if (hnd.ProcessAdcData(rawi, raw, ref rcv_size1, X502.ProcFlags.VOLT) != lpcieapi.lpcie.Errs.OK)
            {
                string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + "Ошибка преобразования данных  в вольты";
                Log.add(s, LogRecord.LogReason.info);
                Debug.WriteLine(s);
                return null;
            }
            if (rcv_size != rcv_size1)
            {
                string s = string.Format("{0}: {1} : Размер преобразования полученный: {2} не равен размеру запрошенному: {3}",
                    GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, rcv_size1, rcv_size);
                Log.add(s, LogRecord.LogReason.info);
                Debug.WriteLine(s);
                return (null);
            }
            onDataRead?.Invoke(raw);
            return raw;
        }
        /// <summary>
        /// Как-бы асинхронный прием данных. В нашем случае бесполезен
        /// </summary>
        /// <param name="_ch">Номер логического канала</param>
        /// <returns>Значение</returns>
        public override double GetValue(int _ch)
        {
            double[] buf = new double[settings.Others.Count];
            //Тайм оут для ожидания приема одного кадра
            uint tout = 1000;
            hnd.AsyncGetAdcFrame(X502.ProcFlags.VOLT, tout, buf);
            return buf[_ch];
        }
        protected override void LoadOtherSettings()
        {
            base.LoadOtherSettings();
            for (int i = 0; i < numOtherSensors; i++)
            {
                hnd.SetLChannel((uint)i, (uint)settings.Others[i].logicalChannel,
                    (X502.LchMode)settings.Others[i].collectedMode,
                    (X502.AdcRange)settings.Others[i].range, 0);
            }
            // Настраиваем источник частоты синхронизации
            hnd.SyncMode = (X502.Sync)settings.SyncMode;
            // Настраиваем  источник запуска сбора
            hnd.SyncStartMode = (X502.Sync)settings.SyncStartMode;

            double f_acq = settings.FrequencyPerChannel * numOtherSensors;
            double f_lch = settings.FrequencyPerChannel;
            // настраиваем частоту сбора с АЦП
            hnd.SetAdcFreq(ref f_acq, ref f_lch);
            //settings.frequencyCollect = f_acq;
            //settings.frequencyPerChannel = f_lch;
            // Записываем настройки в модуль
            hnd.Configure(0);
        }

        public void Dispose()
        {
            Debug.WriteLine(GetType().Name + System.Reflection.MethodBase.GetCurrentMethod().Name);
            if (hnd != null) hnd.Close();
            hnd = null;
        }

        //Для отладки
        public override double[] ReadPacket(int _cnt)
        {
            uint[] data = new uint[_cnt];
            int rcv_size = hnd.Recv(data, (uint)data.Length, RECV_TOUT);
            // переводим АЦП в Вольты
            double[] ret = new double[_cnt];
            uint cnt1 = (uint)_cnt;
            hnd.ProcessAdcData(data, ret, ref cnt1, X502.ProcFlags.VOLT);
            return (ret);
        }
        public void clear()
        {
            uint count = hnd.RecvReadyCount;
            uint[] buf = new uint[count];
            hnd.Recv(buf, count, 100);
        }
    }
}
