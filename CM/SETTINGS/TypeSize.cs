using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CM
{
    /// <summary>
    /// Типоразмер для сериализации
    /// </summary>
    [DisplayName("Типоразмер"), Description("Настройка типоразмеров")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class TypeSize:ParBase
    {
        /// <summary>
        /// Имя типоразмера
        /// </summary>
        [DisplayName(" 1.Наименование"), Description("Наименование типоразмера в формате <Тип(\"НКТ\" или \"СБТ\">-<Диаметр>-<Дополнительный признак>")]
        public string Name { get; set; }

        //[DisplayName(" 3.Диаметр"), Description("Диаметр")]
        //public int diameter { get; set; }

        /// <summary>
        /// Признак типа трубы НКТ или СБТ
        /// </summary>
        [XmlIgnore]
        [DisplayName(" 2.Тип"), Description("Тип \"НКТ\" или \"СБТ\"")]
        public string type
        {
            get
            {
                if (parseName(Name, out string _t, out int _d, out string _dop))
                    return _t;
                else
                    return null;
            }
        }

        /// <summary>
        /// Диаметр
        /// </summary>
        [XmlIgnore]
        [DisplayName(" 3.Диаметр"), Description("Диаметр")]
        public int diameter
        {
            get
            {
                if (parseName(Name, out string _t, out int _d, out string _dop))
                    return _d;
                else
                    return -1;
            }
        }

        /// <summary>
        /// Дополнительные признаки из названия типоразмера (до первого разделителя)
        /// </summary>
        [XmlIgnore]
        [DisplayName(" 4.Дополнительно"), Description("Диаметр")]
        public string dop
        {
            get
            {
                if (parseName(Name, out string _t, out int _d, out string _dop))
                    return _dop;
                else
                    return null;
            }
        }

        /// <summary>
        /// Минимальный годный участок
        /// </summary>
        [DisplayName(" 4.Минимальный годный участок"), Description("Минимальный годный участок")]
        public int MinGoodLength { get; set; }
        
        /// <summary>
        /// Порог класса 1
        /// </summary>
        [DisplayName("Порог 1 класса, %"), Browsable(true)]
        public double Border1 { get; set; }
        
        /// <summary>
        /// Порог класса 2
        /// </summary>
        [DisplayName("Порог 2 класса, %"), Browsable(true)]
        public double Border2 { get; set; }
        
        /// <summary>
        /// Мервая зона в начале трубы
        /// </summary>
        [DisplayName("Мертвая зона в начале, мм"), Browsable(true)]
        public int DeadZoneStart { get; set; }
        
        /// <summary>
        /// Мервая зона в конце трубы
        /// </summary>
        [DisplayName("Мертвая зона в конце, мм"), Browsable(true)]
        public int DeadZoneFinish { get; set; }

        [DisplayName("Датчики"), Browsable(true)]
        public SensorSettings sensors { get; set; }

        [DisplayName("Блок питания"), Browsable(true)]
        public RectifierSettings rectifier { get; set; }

        /// <summary>
        /// Преобразование в строку для вывода в PropertyGrid
        /// </summary>
        /// <returns>строка для вывода в PropertyGrid</returns>
        public override string ToString()
        {
            return Name;
        }

        public TypeSize(string _name)
        {
            Name = _name;
            sensors = new SensorSettings();
            rectifier = new RectifierSettings();
        }

        public TypeSize()
        {
            Name = "Новый";
            sensors = new SensorSettings();
            rectifier = new RectifierSettings();
        }
        #region Разбор имени типоразмера
        /// <summary>
        /// Возможные типы труб
        /// </summary>
        private static readonly string[] _types = { "НКТ", "СБТ" };
        /// <summary>
        /// Возможные диаметры для НКТ
        /// </summary>
        private static readonly int[] _dNKT = { 73, 89, 102, 114 };
        /// <summary>
        /// Возможные диаметры для СБТ
        /// </summary>
        private static readonly int[] _dSBT = { 73, 89, 102, 114, 127 };
        private static readonly char[] _delimiters = { '-', ' ', 'х', 'x', '_' };
        /// <summary>
        /// Проверка имени типоразмера на соответствие для разбора
        /// разобранные части записываются в выходные переменные
        /// </summary>
        /// <param name="_str">Строка для разбора</param>
        /// <param name="_type">out: тип трубы НКТ или СБТ</param>
        /// <param name="_diameter">out: Диаметр трубы</param>
        /// <param name="_dop">out: оставшаяся часть в имени типоразмера</param>
        /// <returns>true - если удалось разобрать имя типоразмера</returns>
        public static bool parseName(string _str, out string _type, out int _diameter, out string _dop)
        {
            _type = string.Empty;
            _diameter = 0;
            _dop = string.Empty;
            if (_str == null || _str == string.Empty) return false;
            //Имя задаем в виде (НКТ|СБТ)<разделитель>(диаметр)<разделитель>(доп.признак) 
            string[] parsedTypeSize = _str.Split(_delimiters);
            if (parsedTypeSize.Length < 2) return false;
            if (parsedTypeSize[0] == "НКТ" || parsedTypeSize[0] == "СБТ")
            {
                _type = parsedTypeSize[0];
                int diam;
                try
                {
                    diam = Convert.ToInt32(parsedTypeSize[1]);
                }
                catch
                {
                    return false;
                }
                //Проверка допустимых параметров для типов труб
                if ((_type == "НКТ" && !Array.Exists<int>(_dNKT, el => el == diam))
                    || (_type == "СБТ" && !Array.Exists<int>(_dSBT, el => el == diam))) return false;
                _diameter = diam;
                if (parsedTypeSize.Length > 2) _dop = parsedTypeSize[2];
                return true;
            }
            return false;
        }
        #endregion
    };
}
