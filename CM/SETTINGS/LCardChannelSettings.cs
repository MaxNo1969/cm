using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using x502api;

namespace CM
{
    /// <summary>
    /// Класс для хранения снастроек логических каналов.
    /// списки channels и others в LCardSettings
    /// </summary>
    [DisplayName("Логические каналы"), Description("Настройка логических каналов"), Category("Кат для LCardChannelSettings")]
    [Serializable]
    public class LCardChannelSettings:ParBase
    {
        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public LCardChannelSettings()
        {
            range = X502.AdcRange.RANGE_10;
            logicalChannel = 0;
            collectedMode = X502.LchMode.DIFF;
        }
        /// <summary>
        /// Конструктор с передачей параметров канала
        /// </summary>
        /// <param name="_logicalChannel"></param>
        /// <param name="_range"></param>
        /// <param name="_collectedMode"></param>
        public LCardChannelSettings(uint _logicalChannel = 0, X502.AdcRange _range = X502.AdcRange.RANGE_10,
            X502.LchMode _collectedMode = X502.LchMode.COMM)
        {
		    range = _range;
		    logicalChannel = _logicalChannel;
		    collectedMode = _collectedMode;
	    }
        /// <summary>
        /// Входной диапазон
        ///     0 Диапазон +/-10V
        ///     1 Диапазон +/-5V
        ///     2 Диапазон +/-2V
        ///     3 Диапазон +/-1V
        ///     4 Диапазон +/-0.5V
        ///     5 Диапазон +/-0.2V
        /// </summary>
        static string[] ranges = { "±10", "±5", "±2", "±1", "±0.5", "±0.2" };
        class RangeConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < ranges.Length; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string v = value as string;
                for (int i = 0; i < ranges.Length; i++)
                {
                    if (ranges[i] == v)
                        return (i);
                }
                return (0);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return (ranges[Convert.ToInt32(value)]); }
        }
        //int lrange;
        /// <summary>
        /// Входной диапазон напряжений
        /// </summary>
        [DisplayName("3.Входной диапазон"), Description("Входной диапазон напряжений")]
        //[TypeConverter(typeof(RangeConverter))]
        public X502.AdcRange range { get;  set; }
        /// <summary>
        /// Входной диапазон - числовое значение
        /// </summary>
        [Browsable(false)]
        public double doubleRange
        {
            get
            {
                double ret = 0;
                switch(range)
                {
                    case X502.AdcRange.RANGE_02:
                        ret=0.2;
                        break;
                    case X502.AdcRange.RANGE_05:
                        ret = 0.5;
                        break;
                    case X502.AdcRange.RANGE_1:
                        ret = 1.0;
                        break;
                    case X502.AdcRange.RANGE_2:
                        ret = 2.0;
                        break;
                    case X502.AdcRange.RANGE_5:
                        ret = 5.0;
                        break;
                    case (int)X502.AdcRange.RANGE_10:
                        ret = 10.0;
                        break;
                }
                return ret;
            }
        }
        /// <summary>
        /// Номер логического канала 0-31 
        /// </summary>
        [DisplayName("1.Номер логического канала"), Description("Номер логического канала 0-31")]
        public uint logicalChannel { get; set; }

        static string[] modes = { "С общей землей", "Дифференциальный", "Измерение нуля" };
        class ModeConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < modes.Length; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string v = value as string;
                for (int i = 0; i < modes.Length; i++)
                {
                    if (modes[i] == v)
                        return (i);
                }
                return (0);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return (modes[Convert.ToInt32(value)]); }
        }
        /// <summary>
        /// Режим сбора
        ///     0 Измерение напряжения относительно общей земли
        ///     1 Дифференциальное измерение напряжения
        ///     2 Измерение собственного нуля
        /// </summary>
        [DisplayName("2.Режим сбора"), Description("Режим сбора")]
        //[TypeConverter(typeof(ModeConverter))]
        public L502.LchMode collectedMode { get; set; }

        public override string ToString()
        {
            return string.Format("Канал{0}", logicalChannel);
        }
    }
}
