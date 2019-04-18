using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CM
{
    /// <summary>
    /// Параметры сигнала с платы цифрового ввода/вывода
    /// </summary>
    [TypeConverter(typeof(RExpandableObjectConverter))]
    public class SignalSettings : ParBase
    {
        /// <summary>
        /// Имя сигнала
        /// </summary>
        [DisplayName("Наименование"), NoCopy, Browsable(true), De]
        public string Name { get; set; }

        /// <summary>
        /// Входящий/Исходящий
        /// </summary>
        [DisplayName("Входящий"), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool Input { get; set; }

        /// <summary>
        /// Цифровой
        /// </summary>
        [DisplayName("Цифровой"), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool Digital { get; set; }

        /// <summary>
        /// Номер канала
        /// </summary>
        [DisplayName("Номер канала"), NoCopy, Browsable(true), De]
        public int Position { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Описание"), Browsable(true), De]
        public string Hint { get; set; }

        /// <summary>
        /// Ошибка On
        /// </summary>
        [DisplayName("Ошибка On"), Browsable(true), De]
        public string EOn { get; set; }

        /// <summary>
        /// Ошибка Off
        /// </summary>
        [DisplayName("Ошибка Off"), Browsable(true), De]
        public string EOff { get; set; }

        /// <summary>
        /// Задержка, мс
        /// </summary>
        [DisplayName("Задержка, мс"), Browsable(true), De]
        public int Timeout { get; set; }

        /// <summary>
        /// Не снимать
        /// </summary>
        [DisplayName("Не снимать"), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool NoReset { get; set; }

        /// <summary>
        /// Вербальный
        /// </summary>
        [DisplayName("Вербальный"), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool Verbal { get; set; }

        /// <summary>
        /// X
        /// </summary>
        [DisplayName("X"), Browsable(true), De]
        public int X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        [DisplayName("Y"), Browsable(true), De]
        public int Y { get; set; }

        /// <summary>
        /// История
        /// </summary>
        [DisplayName("История"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool FIFOEnable { get; set; }

        /// <summary>
        /// Строковое представление сигнала
        /// </summary>
        /// <returns>Короткая строка с характеристиками сигнала</returns>
        public override string ToString()
        {
            string ret = Position.ToString();
            ret += " ";
            ret += Input ? "Вх" : "Вых";
            ret += Digital ? " Ц" : "";
            ret += " ";
            ret += Name;
            return (ret);
        }
        /// <summary>
        /// Преобразование в список строк
        /// </summary>
        /// <returns></returns>
        public List<string> ToCS()
        {
            List<string> L = new List<string>
            {
                "[DName(\"" + Name + "\")]",
                "public Signal " + (Input ? "i" : "o") + ";"
            };
            return (L);
        }
    }
}