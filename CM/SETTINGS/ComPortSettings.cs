using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;

namespace CM
{
    /// <summary>
    /// Параметры для настройки платы татирования АЦП
    /// </summary>
    [DisplayName("Настройки COM порта"), Description("Настройки COM порта")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class ComPortSettings:ParBase
    {
        /// <summary>
        /// Имя порта
        /// </summary>
        [DisplayName("Порт"), /*DefaultValue("COM1"),*/ Browsable(true)]
        public string PortName { get; set; }

        class BaudConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return (new StandardValuesCollection(new List<int>() { 2400, 9600, 19200 }));
            }
        }
        /// <summary>
        /// Скорость порта
        /// </summary>
        [DisplayName("Скорость"), /*DefaultValue(9600),*/ Browsable(true)]
        [TypeConverter(typeof(BaudConverter))]
        public int BaudRate { get; set; }

        /// <summary>
        /// Количество бит
        /// </summary>
        [DisplayName("Битов"), /*DefaultValue(8),*/ Browsable(true)]
        public int DataBits { get; set; }

        /// <summary>
        /// Четность
        /// </summary>
        [DisplayName("Четность"), /*DefaultValue(Parity.None),*/ Browsable(true)]
        public Parity Parity { get; set; }

        /// <summary>
        /// Стоп Биты
        /// </summary>
        [DisplayName("Стоп Биты"), /*DefaultValue(StopBits.One),*/ Browsable(true)]
        public StopBits StopBits { get; set; }

        /// <summary>
        /// Интервал чтения
        /// </summary>
        [DisplayName("Интервал чтения"),/*DefaultValue(500),*/ Browsable(true)]
        public int ReadIntervalTimeout { get; set; }

        /// <summary>
        /// Строковое предствление для PropertyGrid
        /// </summary>
        /// <returns>Строковое предствление для PropertyGrid</returns>
        public override string ToString()
        {
            return string.Format("{0}({1},{2} bits,{3},{4})", PortName, BaudRate, DataBits, Parity, StopBits);
        }
    }
}
