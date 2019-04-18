using Protocol;
using System;
using System.Diagnostics;
using System.Threading;

namespace CM
{
    /// <summary>
    /// Регистры для управления выпрямителем по шине ModBus
    /// </summary>
    public class RectifierRegister
    {
        /// <summary>
        /// Адрес регистра
        /// </summary>
        public int reg;
        /// <summary>
        /// Чтение/запись
        /// </summary>
        public bool rw;
        /// <summary>
        /// Наименование
        /// </summary>
        public string name;
        /// <summary>
        /// Описание
        /// </summary>
        public string hint;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_reg">Адрес регистра</param>
        /// <param name="_rw">Чтение/запись</param>
        /// <param name="_name">Наименование</param>
        /// <param name="_hint">Описание</param>
        public RectifierRegister(int _reg, bool _rw, string _name, string _hint)
        {
            reg = _reg;
            rw = _rw;
            name = _name;
            hint = _hint;
        }
        /// <summary>
        /// Рабочие регистры выпрямителя
        /// </summary>
        public static RectifierRegister[] registers =
        {
            new RectifierRegister( 1, false, "OutputVoltage", "Выходное напряжение ( дискретность 0.1В), только чтение(функция 4)" ),
            new RectifierRegister(2, false, "OutputCurrent", "Выходной ток ( дискретность 0.1А), только чтение (функция 4)"),
            new RectifierRegister(3, false, "CurrentProcessTimeSec", "Текущее время процесса секунды (дискретность 1с), только чтение (функция 4)"),
            new RectifierRegister(4, false, "CurrentProcessTimeMin", "Текущее время процесса минуты (дискретность 1м), только чтение (функция 4)"),
            new RectifierRegister(5, false, "CurrentProcessTimeHour", "Текущее время процесса часы (дискретность 1ч), только чтение (функция 4)"),
            new RectifierRegister(6, false, "ResidualProcessTimeSec", "Остаточное время процесса секунды (дискретность 1с), только чтение (функция 4)"),
            new RectifierRegister(7, false, "ResidualProcessTimeMin", "Остаточное время процесса минуты (дискретность 1м), только чтение (функция 4)"),
            new RectifierRegister(8, false, "ResidualProcessTimeHour", "Остаточное время процесса часы (дискретность 1ч), только чтение (функция 4)"),
            new RectifierRegister(9, false, "TestCycleCounter", "Счетчик испытательных циклов, только чтение (функция 4)"),
            new RectifierRegister(13, false, "OutputVoltageLessMinimum", "Число в регистре равно 0 при Uвых>Uмин (задается в установках). Число в регистре равно 1 при Uвых<Uмин (задается в установках). Tолько чтение (функция 4)."),
            new RectifierRegister(14, false, "OutputVoltageMoreMaximum", "Число в регистре равно 0 при Uвых<Uмакс (задается в установках). Число в регистре равно 1 при Uвых<Uмакс (задается в установках). Tолько чтение (функция 4)."),
            new RectifierRegister(50, true, "SettingCurrentSourceMode", "Уставочный ток для режима источника тока ( дискретность 0.1А), чтение(функция 3)"),
            new RectifierRegister(51, true, "SettingVoltageSourceMode", "Уставочное напряжение для режима источника напряжения (дискретность 0.1В), чтение (функция 3)"),
            new RectifierRegister(52, true, "MaximumVoltageCurrentSourceMode", "Максимальное напряженеие для режима источника тока (дискретность 0.1В), чтение (функция 3)"),
            new RectifierRegister(53, true, "MaximumCurrentVoltageSourceMode", "Максимальный ток для режима источника напряжения (дискретность 0.1А), чтение (функция 3)"),
            new RectifierRegister(54, true, "SettingOperatingTimePowerSourceSec", "Установочное время работы для источника тока, секунды (дискретность 1с), чтение (функция 3)"),
            new RectifierRegister(55, true, "SettingOperatingTimePowerSourceMin", "Установочное время работы для источника тока, минуты (дискретность 1м), чтение (функция 3)"),
            new RectifierRegister(56, true, "SettingOperatingTimePowerSourceHour", "Установочное время работы для источника тока, часы (дискретность 1ч), чтение (функция 3)"),
            new RectifierRegister(57, true, "SettingTimeVoltageSourceSec", "Установочное время работы для источника напряжения, секунды (дискретность 1с), чтение (функция 3)"),
            new RectifierRegister(58, true, "SettingTimeVoltageSourceMin", "Установочное время работы для источника напряжения, минуты (дискретность 1м), чтение (функция 3)"),
            new RectifierRegister(59, true, "SettingTimeVoltageSourceHour", "Установочное время работы для источника напряжения, часы (дискретность 1ч), чтение (функция 3)"),
            new RectifierRegister(60, true, "SwitchVoltageSourceMode", "Включение/включенность режима источника напряжения, чтение(функция 3)"),
            new RectifierRegister(61, true, "SwitchCurrentSourceMode", "Включение/включенность режима источника тока, чтение(функция 3)"),
            new RectifierRegister(62, true, "SwitchingRelayState", "Состояние-переключение реле реверса, 0 - прямое, 1 - обратное, чтение(функция 3)"),
            new RectifierRegister(63, true, "StateSwitchingAutoReverseFunction", "Состояние-переключение функции автореверса, 0 - выключено, 1 - включено, чтение(функция 3)"),
            new RectifierRegister(64, true, "AutoReverseWorkingTimeDirect", "Автореверс, время работы прямое, (дискретность 1с), чтение (функция 3)"),
            new RectifierRegister(65, true, "AutoReverseWorkingTimeReverse", "Автореверс, время работы обратное, (дискретность 1с), чтение (функция 3)"),
            new RectifierRegister(66, true, "AutoReverseSwitchPauseTime", "Автореверс, время паузы при переключении,(дискретность 1с), чтение (функция 3)"),
            new RectifierRegister(67, true, "AutoReverseDirectCurrentStabilization1", "Автореверс, ток стабилизации прямой, (дискретность 0.1А), чтение (функция 3)"),
            new RectifierRegister(68, true, "AutoReverseDirectCurrentStabilization2", "Автореверс, ток стабилизации прямой, (дискретность 0.1А), чтение (функция 3)"),
            new RectifierRegister(69, true, "AutoReverseStabilizationVoltageDirect", "Автореверс, напряжение стабилизации прямое, (дискретность 0.1В), чтение(функция 3)"),
            new RectifierRegister(70, true, "AutoReverseStabilizingReverseVoltage", "Автореверс, напряжение стабилизации обратное, (дискретность 0.1В), чтение (функция 3)"),
        };
    }

    /// <summary>
    /// Класс для управления выпрямителем
    /// </summary>
    public class Rectifier : IDisposable
    {
        readonly ModBus modbus;
        readonly RectifierSettings settings;
        readonly int abonent;
        /// <summary>
        /// Блок питания запущен в работу
        /// </summary>
        public bool started = false;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_modbus">ModBus для управления выпрямителем</param>
        public Rectifier(ModBus _modbus)
        {
            modbus = _modbus;
            if (Program.settings.Current.rectifier != null)
                settings = Program.settings.Current.rectifier;
            else
                settings = new RectifierSettings();
            abonent = Program.settings.rectifierSettings.Abonent;
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}: Порт:{2}, Абонент:{3}",
                    GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                    settings.ToString(), abonent);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
        }

        private double getDouble(int _pos)
        {
            if (modbus.ReadInputRegisterE(abonent, _pos, out ushort res))
            {
                return (double)res * 0.1;
            }
            else
                throw new Exception(modbus.err);
        }

        private int getInt(int _pos)
        {
            if (modbus.ReadInputRegisterE(abonent, _pos, out ushort res))
            {
                return res;
            }
            else
                throw new Exception(modbus.err);
        }

        private bool getBool(int _pos)
        {
            if (modbus.ReadInputRegisterE(abonent, _pos, out ushort res))
            {
                return res == 0;
            }
            else
                throw new Exception(modbus.err);
        }
        /// <summary>
        /// Выходное напряжение ( дискретность 0.1В), только чтение(функция 4)
        /// </summary>
        /// <returns>Выходное напряжение ( дискретность 0.1В), только чтение(функция 4)</returns>
        public double getVoltage()
        {
            return getDouble(1);
        }
        /// <summary>
        /// Выходной ток ( дискретность 0.1А), только чтение (функция 4)
        /// </summary>
        /// <returns>Выходной ток ( дискретность 0.1А), только чтение (функция 4)</returns>
        public double getAmperage()
        {
            return getDouble(2);
        }
        /// <summary>
        /// Текущее время процесса секунды (дискретность 1с), только чтение (функция 4)
        /// </summary>
        /// <returns>Текущее время процесса секунды (дискретность 1с), только чтение (функция 4)</returns>
        public int getCurrentProcessSeconds()
        {
            return getInt(3);
        }
        /// <summary>
        /// Текущее время процесса минуты (дискретность 1с), только чтение (функция 4)
        /// </summary>
        /// <returns>Текущее время процесса минуты (дискретность 1с), только чтение (функция 4)</returns>
        public int getCurrentProcessMinutes()
        {
            return getInt(4);
        }
        /// <summary>
        /// Текущее время процесса часы (дискретность 1ч), только чтение (функция 4)
        /// </summary>
        /// <returns>Текущее время процесса часы (дискретность 1ч), только чтение (функция 4)</returns>
        public int getCurrentProcessHours()
        {
            return getInt(5);
        }
        /// <summary>
        /// Остаточное время процесса секунды (дискретность 1с), только чтение (функция 4)
        /// </summary>
        /// <returns>Остаточное время процесса секунды (дискретность 1с), только чтение (функция 4)</returns>
        public int getResidualProcessSeconds()
        {
            return getInt(6);
        }
        /// <summary>
        /// Остаточное время процесса минуты (дискретность 1м), только чтение (функция 4)
        /// </summary>
        /// <returns>Остаточное время процесса минуты (дискретность 1м), только чтение (функция 4)</returns>
        public int getResidualProcessMinutes()
        {
            return getInt(7);
        }
        /// <summary>
        /// Остаточное время процесса  часы (дискретность 1ч), только чтение (функция 4)
        /// </summary>
        /// <returns>Остаточное время процесса часы (дискретность 1ч), только чтение (функция 4)</returns>
        public int getResidualProcessHours()
        {
            return getInt(8);
        }
        /// <summary>
        /// Число в регистре равно 0 при Uвых&gt;Uмин (задается в установках). Число в регистре равно 1 при Uвых &lt; Uмин (задается в установках). Tолько чтение (функция 4).
        /// </summary>
        /// <returns>Число в регистре равно 0 при Uвых&gt;Uмин (задается в установках). Число в регистре равно 1 при Uвых &lt; Uмин (задается в установках). Tолько чтение (функция 4).</returns>
        public bool bOutputVoltageLessMinimum()
        {
            return getBool(13);
        }
        /// <summary>
        /// Число в регистре равно 0 при Uвых&lt;Uмакс (задается в установках). Число в регистре равно 1 при Uвых&lt;Uмакс (задается в установках). Tолько чтение (функция 4).
        /// </summary>
        /// <returns>Число в регистре равно 0 при Uвых&lt;Uмакс (задается в установках). Число в регистре равно 1 при Uвых&lt;Uмакс (задается в установках). Tолько чтение (функция 4).</returns>
        public bool bOutputVoltageMoreMaximum()
        {
            return getBool(14);
        }
        private void setDouble(int _pos, double _val)
        {
            if (!modbus.setSingleRegister(abonent, _pos, (ushort)(_val / 0.1)))
                throw new Exception(modbus.err);
        }

        private void setInt(int _pos, int _val)
        {
            if (!modbus.setSingleRegister(abonent, _pos, (ushort)_val))
                throw new Exception(modbus.err);
        }

        private void setInt1(int _pos, int _val)
        {
            modbus.ReadInputRegisterE(abonent, _pos, out ushort res);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (res != _val)
            {
                modbus.setSingleRegister(abonent, _pos, (ushort)_val);
                modbus.ReadInputRegisterE(abonent, _pos, out res);
                //Задержка
                Thread.Sleep(100);
                if (sw.ElapsedMilliseconds > 1000) break;
            }
        }

            /// <summary>
            /// Установка рабочей силы тока
            /// </summary>
            public void setWorkAmperage()
        {
            if (settings.TpIU == EIU.ByI)
                setDouble(50, settings.NominalI);
            else
                throw new Exception("Попытка задать ток при контроле напряжения");
        }
        /// <summary>
        /// Установка рабочего напрячения
        /// </summary>
        public void setWorkVoltage()
        {
            if (settings.TpIU == EIU.ByU)
                setDouble(51, settings.NominalI);
            else
                throw new Exception("Попытка задать напряжение при контроле тока");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        /// <summary>
        /// IDisposable Support
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (modbus != null) modbus.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Rectifier() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }
        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// IDisposable Support
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
        /// <summary>
        /// Запустить выпрямитель
        /// </summary>
        /// <returns>запустиля/не запустился</returns>
        public bool start()
        {
            if (modbus == null) return false;
            switch (settings.TpIU)
            {
                case EIU.ByI:
                    //new RectifierRegister(50, true, "SettingCurrentSourceMode", "Уставочный ток для режима источника тока ( дискретность 0.1А), чтение(функция 3)"),
                    setWorkAmperage();
                    //new RectifierRegister(52, true, "MaximumVoltageCurrentSourceMode", "Максимальное напряженеие для режима источника тока (дискретность 0.1В), чтение (функция 3)"),
                    setDouble(51, settings.MaxU);
                    //new RectifierRegister(54, true, "SettingOperatingTimePowerSourceSec", "Установочное время работы для источника тока, секунды (дискретность 1с), чтение (функция 3)"),
                    setInt(54, settings.Timeout % 60);
                    //new RectifierRegister(55, true, "SettingOperatingTimePowerSourceMin", "Установочное время работы для источника тока, минуты (дискретность 1м), чтение (функция 3)"),
                    setInt(55, (settings.Timeout % 3600) / 60);
                    //new RectifierRegister(56, true, "SettingOperatingTimePowerSourceHour", "Установочное время работы для источника тока, часы (дискретность 1ч), чтение (функция 3)"),
                    setInt(56, settings.Timeout / 3600);
                    //new RectifierRegister(61, true, "SwitchCurrentSourceMode", "Включение/включенность режима источника тока, чтение(функция 3)"),
                    setInt(61, 1);
                    break;
                case EIU.ByU:
                    //new RectifierRegister(51, true, "SettingVoltageSourceMode", "Уставочное напряжение для режима источника напряжения (дискретность 0.1В), чтение (функция 3)"),
                    setWorkVoltage();
                    //new RectifierRegister(53, true, "MaximumCurrentVoltageSourceMode", "Максимальный ток для режима источника напряжения (дискретность 0.1А), чтение (функция 3)"),
                    setDouble(53, settings.MaxI);
                    //new RectifierRegister(57, true, "SettingTimeVoltageSourceSec", "Установочное время работы для источника напряжения, секунды (дискретность 1с), чтение (функция 3)"),
                    setInt(57, settings.Timeout % 60);
                    //new RectifierRegister(58, true, "SettingTimeVoltageSourceMin", "Установочное время работы для источника напряжения, минуты (дискретность 1м), чтение (функция 3)"),
                    setInt(58, (settings.Timeout % 3600) / 60);
                    //new RectifierRegister(59, true, "SettingTimeVoltageSourceHour", "Установочное время работы для источника напряжения, часы (дискретность 1ч), чтение (функция 3)"),
                    setInt(59, settings.Timeout / 3600);
                    //new RectifierRegister(60, true, "SwitchVoltageSourceMode", "Включение/включенность режима источника напряжения, чтение(функция 3)"),
                    setInt(60, 1);
                    break;
                default:
                    started = false;
                    return false;
            }
            started = true;
            return true;
        }
        public void Start()
        {
            if (modbus == null) return;
            #region Логирование 
            {
                string msg = string.Format("{0}", "Включаем блок питания" );
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            switch (settings.TpIU)
            {
                case EIU.ByI:
                    setInt1(61, 1);
                    break;
                case EIU.ByU:
                    setInt1(60, 1);
                    break;
                default:
                    break;
            }
        }
        public void Stop()
        {
            if (modbus == null) return;
            #region Логирование 
            {
                string msg = string.Format("{0}", "Выключаем блок питания");
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            switch (settings.TpIU)
            {
                case EIU.ByI:
                    setInt1(61, 0);
                    break;
                case EIU.ByU:
                    setInt1(60, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
