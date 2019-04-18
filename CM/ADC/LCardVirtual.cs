using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CM
{
    class LCardVirtual : LCard
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="_size">Размер пакета считывемого за раз из буфера АЦП</param>
        public LCardVirtual(LCardSettings _pars) : base(_pars)
        {
            #region Логирование 
            {
                string msg = "";
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");

            }
            #endregion
        }

        /// <summary>
        /// Модель трубы для эмуляции
        /// </summary>
        public Tube srcTube = null;

        /// <summary>
        /// Текущий индех для чтения в трубе
        /// </summary>
        public int index = 0;

        public override double GetValue(int _ch)
        {
            return 0.0;
        }

        public override double[] Read()
        {
            if (!IsStarted)
            {
                #region Логирование 
                {
                    string msg = "Не запущена";
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");
                }
                #endregion
                return null;
            }
            if (srcTube == null)
            {
                #region Логирование 
                {
                    string msg = "Не задана труба для эмуляции";
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");
                }
                #endregion
                return null;
            }
            double[] data = new double[raw_size];
            lock (srcTube)
            {
                if (index + raw_size < srcTube.rawDataSize)
                    Array.Copy(srcTube.rtube.data.ToArray(), index, data, 0, raw_size);
                onDataRead?.Invoke(data);
                index += (int)raw_size;
            }
            Thread.Sleep((int)(raw_size * 1000.0 / settings.FrequencyCollect));
            return data;
        }

        public override bool Start()
        {
            if (IsStarted) return IsStarted;
            Program.rectifier.Start();
            mtdadc.start();
            #region Логирование 
            {
                string msg = string.Format("{0}", "");
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            if (srcTube == null)
            {
                #region Логирование 
                {
                    string msg = @"Не задана модель для эмуляции";
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");
                }
                #endregion
                return false;
            }
            LoadMainSettings();
            index = 0;
            IsStarted = true;
            return IsStarted;
        }
        public override bool Stop()
        {
            if (!IsStarted) return true;
            Program.rectifier.Stop();
            mtdadc.stop();
            #region Логирование 
            {
                string msg = string.Format("{0}", "");
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");

            }
            #endregion
            IsStarted = false;
            return true;
        }
        public override double[] ReadPacket(int _cnt)
        {
            Random r = new Random();
            double[] ret = new double[_cnt];
            for (int i = 0; i < _cnt; i++)
            {
                ret[i] = srcTube.rtube.data[index + i];
            }
            return ret;
        }
    }
}
