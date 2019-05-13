using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CM
{
    class ZoneWriterThread
    {
        Tube tube = null;
        readonly SignalListDef sl = Program.signals;
        Thread writeZoneThread = null;
        bool isRunning = false;
        /// <summary>
        /// Флаг окончания трубы
        /// </summary>
        public bool bEndOfTube = false;

        /// <summary>
        /// Делегат начало следующей зоны
        /// </summary>
        /// <param name="_tm">Труба</param>
        public delegate void OnNextZone(Tube _tube);

        public OnNextZone onNextZone = null;

        /// <summary>
        /// Блокировка
        /// </summary>
        readonly object block = new object();

        readonly int zoneTime;
        int lastWritedSection = 0;
        int zone = 0;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">Труба</param>
        public ZoneWriterThread(Tube _tube)
        {
            tube = _tube;
            isRunning = false;
            zoneTime = (int)(Program.settings.ZoneSize / Program.settings.TubeSpeed);
        }

        /// <summary>
        /// Запуск потока эмуляции движения трубы
        /// </summary>
        public void start()
        {
            if (isRunning) return;
            writeZoneThread = new Thread(writeZoneFunc)
            {
                Name = "ZoneWriterThread"
            };
            isRunning = true;
            bEndOfTube = false;
            //tube.ptube.endWritedX = 0;
            //tube.ptube.endWritedY = 0;
            tube.reset();
            lastWritedSection = 0;
            writeZoneThread.Start();
        }

        /// <summary>
        /// Остановка потока эмуляции движения трубы
        /// </summary>
        public void stop()
        {
            if (!isRunning) return;
            isRunning = false;
            writeZoneThread.Join();
        }
        private void writeZoneFunc()
        {
            #region Логирование 
            {
                string msg = string.Format("Запуск zoneTime={0}, logZoneSize={1}", zoneTime, tube.ptube.logZoneSize);
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            while (isRunning)
            {
                if (tube.ptube.endWritedX < tube.ptube.Width)
                {
                    int currentSections = tube.sections;
                    if (currentSections > Tube.GetsectionsPerZone() * (zone + 1)+ Program.settings.Current.DeadZoneStart - 1)
                    {
                        #region Логирование 
                        {
                            string msg = string.Format(@"{0:hh\:mm\:ss\.ff} Начало зоны {1} : {2,5:f2} м ({3} из {4}) {5}",
                                DateTime.Now, zone, tube.ptube.l2px(tube.ptube.endWritedX) / 1000.0, tube.ptube.endWritedX, tube.ptube.Width, lastWritedSection);
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Message");
                        }
                        #endregion
                        //int currentSections = tube.sections;
                        //tube.raw2phys(lastWritedSection, currentSections - lastWritedSection, zone, 1);
                        tube.raw2phys(lastWritedSection, Tube.GetsectionsPerZone(), zone, 1);
                        Tube.TubeRes zoneRes = tube.getZoneResult(zone, out double _maxVal);
                        tube.Zones.Add(new Zone(currentSections,zoneRes));
                        #region Логирование 
                        {
                            string msg = string.Format(@"{0:hh\:mm\:ss\.ff} Результат по зоне {1} : {2} {3}",
                                DateTime.Now, zone, zoneRes.ToString(), _maxVal);
                            string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.info);
                            Debug.WriteLine(logstr, "Message");
                        }
                        #endregion
                        Program.signals.ControlResult(zoneRes);
                        //lastWritedSection = currentSections;
                        lastWritedSection += Tube.GetsectionsPerZone();
                        zone++;
                        onNextZone?.Invoke(tube);
                    }
                    //Thread.Sleep(zoneTime);
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
                    isRunning = false;
                    bEndOfTube = true;
                    writeZoneThread = null;
                    break;
                }
            }
        }
    }
}
