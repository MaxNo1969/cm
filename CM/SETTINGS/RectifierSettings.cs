using CM;
using System;
using System.ComponentModel;

namespace CM
{
    /// <summary>
    /// Тип стабилизации (по току или по напряжению)
    /// </summary>
    public enum EIU
    {
        /// <summary>
        /// Стабилизация по току
        /// </summary>
        [Description("По току")]
        ByI,
        /// <summary>
        /// Стабилизация по напряжению
        /// </summary>
        [Description("По напряжению")]
        ByU
    }

    /// <summary>
    /// Параметры для настройки блока питания
    /// </summary>
    [DisplayName("Блок питания"), Description("Настройки подключения для источника питания"), De]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class ModbusRectifierSettings:ParBase
    {
        /// <summary>
        /// Номер абонента modbus
        /// </summary>
        [Browsable(true)]
        [DisplayName("Абонент"), Description("Номер абонента modbus")]
        public int Abonent { get; set; }
        /// <summary>
        /// Порт для шины modbus
        /// </summary>
        [Browsable(true)]
        [DisplayName("Порт RS482"), Description("Настройка порта RS482 для шины modbus")]
        public ComPortSettings Port { get; set; }

        /// <summary>
        /// Период опроса, мс
        /// </summary>
        [DisplayName("Период опроса, мс"), /*DefaultValue(1000),*/ Browsable(true)]
        public int Period { get; set; }

        /// <summary>
        /// Строковое представлениедля отображения в PropertyGrid
        /// </summary>
        /// <returns>Строковое представлениедля отображения в PropertyGrid</returns>
        public override string ToString()
        {
            return string.Format("Аб.{0},Порт: {1},Пер: {2}c, ", Abonent.ToString(), Port.Port, Period.ToString());
        }
        public ModbusRectifierSettings()
        {
            Port = new ComPortSettings();
        }
    }

    /// <summary>
    /// Параметры для настройки блока питания
    /// </summary>
    [DisplayName("Блок питания"), Description("Настройки контроля для источника питания")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class RectifierSettings
    {
        /// <summary>
        /// Тип контроля (по току/по напряжению)
        /// </summary>
        [DisplayName("Тип контроля"), Browsable(true), /*DefaultValue(EIU.ByI)*/]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EIU TpIU { get; set; }

        /// <summary>
        /// Длительность работы
        /// </summary>
        [DisplayName("Длительность работы, с"), /*DefaultValue(10),*/ Browsable(true)]
        public int Timeout { get; set; }

        /// <summary>
        /// Требуемый ток, А
        /// </summary>
        [DisplayName("Требуемый ток, А"), Browsable(true)/*, DefaultValue(1.0)*/]
        public double NominalI { get; set; }

        /// <summary>
        /// Требуемое напряжение, В
        /// </summary>
        [DisplayName("Требуемое напряжение, В"), Browsable(true)/*, DefaultValue(220.0)*/]
        public double NominalU { get; set; }

        /// <summary>
        /// Максимальный ток, А
        /// </summary>
        [DisplayName("Максимальный ток, А"), Browsable(true)/*, DefaultValue(1.0)*/]
        public double MaxI { get; set; }

        /// <summary>
        /// Максимальное напряжение, В
        /// </summary>
        [DisplayName("Максимальное напряжение, В"), Browsable(true)/*, DefaultValue(260.0)*/]
        public double MaxU { get; set; }

        /// <summary>
        /// Сопротивления перегрева, Ом
        /// </summary>
        [DisplayName("Сопротивления перегрева, Ом"), Browsable(true)]
        public double MaxR { get; set; }

        /// <summary>
        /// Строковое представлениедля отображения в PropertyGrid
        /// </summary>
        /// <returns>Строковое представлениедля отображения в PropertyGrid</returns>
        public override string ToString()
        {
            string ret = string.Format("Раб: {0}c, ", Timeout.ToString());
            switch (TpIU)
            {
                case EIU.ByI:
                    ret += string.Format("{0}А, макс: {1}В", NominalI.ToString(), MaxU.ToString());
                    break;
                case EIU.ByU:
                    ret += string.Format("{0}В, макс: {1}А", NominalU.ToString(), MaxI.ToString());
                    break;
            }
            return ret;
        }
    }
}
