using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace CM
{
    /// <summary>
    /// Параметры для настройки платы татирования АЦП
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class ComPortPars
    {
        class PortConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<string> L = new List<string>();
                for (int i = 1; i < 21; i++)
                    L.Add("COM" + i.ToString());
                return (new StandardValuesCollection(L));
            }
        }
        /// <summary>
        /// Имя порта
        /// </summary>
        [DisplayName("Порт"), DefaultValue("COM1"), Browsable(true)]
        [TypeConverter(typeof(PortConverter))]
        public string Port { get; set; }

        class BaudConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return (new StandardValuesCollection(new List<int>() { 9600, 19200 }));
            }
        }
        /// <summary>
        /// Скорость порта
        /// </summary>
        [DisplayName("Скорость"), DefaultValue(19200), Browsable(true)]
        [TypeConverter(typeof(BaudConverter))]
        public int BaudRate { get; set; }

        class ByteSizeConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) { return (new StandardValuesCollection(new List<int>() { 7, 8 })); }
        }
        /// <summary>
        /// Количество бит
        /// </summary>
        [DisplayName("Битов"), DefaultValue(8), Browsable(true)]
        [TypeConverter(typeof(ByteSizeConverter))]
        public int DataBits { get; set; }

        static string[] parities = { "No", "Odd", "Even", "Mark", "Space" };
        class ParityConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < parities.Length; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string v = value as string;
                for (int i = 0; i < parities.Length; i++)
                {
                    if (parities[i] == v)
                        return (i);
                }
                return (0);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return (parities[Convert.ToInt32(value)]); }
        }
        /// <summary>
        /// Четность
        /// </summary>
        [DisplayName("Четность"), TypeConverter(typeof(ParityConverter)), DefaultValue(0), Browsable(true)]
        public int Parity { get; set; }

        static string[] stopbits = { "1", "1.5", "2" };
        class StopBitsConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(
              ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < stopbits.Length; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string v = value as string;
                for (int i = 0; i < stopbits.Length; i++)
                {
                    if (stopbits[i] == v)
                        return (i);
                }
                return (0);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return (stopbits[Convert.ToInt32(value)]); }
        }
        /// <summary>
        /// Стоп Биты
        /// </summary>
        [DisplayName("Стоп Биты"), DefaultValue(0), Browsable(true)]
        [TypeConverter(typeof(StopBitsConverter))]
        public int StopBits { get; set; }

        /// <summary>
        /// Интервал чтения
        /// </summary>
        [DisplayName("Интервал чтения"), DefaultValue(500), Browsable(true)]
        public int ReadIntervalTimeout { get; set; }
        /// <summary>
        /// Интервал полный
        /// </summary>
        [DisplayName("Интервал полный"), DefaultValue(500), Browsable(true)]
        public int ReadTotalTimeoutConstant { get; set; }
        /// <summary>
        /// Интервал умноженный
        /// </summary>
        [DisplayName("Интервал умноженный"), DefaultValue(500), Browsable(true)]
        public int ReadTotalTimeoutMultiplier { get; set; }

        class TimeoutConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(
              ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < 16; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
        }
        /// <summary>
        /// Время ожидания, мс
        /// </summary>
        [DisplayName("Время ожидания, мс"), DefaultValue(1), TypeConverter(typeof(TimeoutConverter)), Browsable(true)]
        public int Timeout { get; set; }
        /// <summary>
        /// Строковое предствление для PropertyGrid
        /// </summary>
        /// <returns>Строковое предствление для PropertyGrid</returns>
        public override string ToString() { return (Port); }
    }
}
