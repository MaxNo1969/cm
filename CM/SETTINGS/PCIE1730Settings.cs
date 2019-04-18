using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CM
{
    /// <summary>
    /// Настройки платы PCIE-1730
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class PCIE1730Settings:ParBase
    {
        /// <summary>
        /// Номер устройства
        /// </summary>
        [Category("1.Настройка модуля")]
        [DisplayName("2.Номер устройства"), Description("Номер устройства")]
        public int DevNum { get; set; }
        /// <summary>
        /// Количество входящих портов
        /// </summary>
        [Category("1.Настройка модуля")]
        [DisplayName("3.Количество входящих портов"), Description("Количество входящих портов")]
        public int PortcountIn { get; set; }
        /// <summary>
        /// Количество исходящих портов
        /// </summary>
        [Category("1.Настройка модуля")]
        [DisplayName("4.Количество исходящих портов"), Description("Количество исходящих портов")]
        public int PortcountOut { get; set; }
        /// <summary>
        /// Задержка в потоке чтения портов
        /// </summary>
        [Category("1.Настройка модуля")]
        [DisplayName("5.Задежка"), Description("Задержка в потоке чтения портов")]
        public int SignalListTimeout { get; set; }
        /// <summary>
        /// Управление подключенными сигналами
        /// </summary>
        [DisplayName("6.Сигналы"), Description("Управление подключенными сигналами"), Category("1.Настройка модуля")]
        [TypeConverter(typeof(CollectionTypeConverter))]
        public L_SignalSettings Signals {get; set;}

        /// <summary>
        /// Строковое преобразование (для PropertyGrid)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("PCIE-1730,BID#{0}", DevNum);
        }
    }
}
