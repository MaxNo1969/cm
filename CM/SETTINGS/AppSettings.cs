using FormsExtras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CM
{
    /// <summary>
    /// Делегат на смену параметров
    /// </summary>
    public delegate void OnChangeSettings(object _param);
    /// <summary>
    /// Класс для хранения настроек параметров приложения. Хранить будем в XML
    /// </summary>
    [Serializable]
    public class AppSettings:ParBase
    {
        [XmlIgnore]
        public bool changed = false;

        [XmlIgnore]
        public OnChangeSettings onChangeSettings = null;

        /// <summary>
        /// Размер одной зоны
        /// </summary>
        [Browsable(true)]
        [DisplayName("Размер зоны"), Description("Размер зоны (мм)"), Category("1.Труба")]
        public int ZoneSize { get; set; }

        //[Browsable(true)]
        //[DisplayName("Максимальное количество зон"), Description("Максимальное количество зон"), Category("1.Труба")]
        //public int MaxZones { get; set; }

        [Browsable(true)]
        [DisplayName("Длина трубы"), Description("Длина трубы"), Category("1.Труба")]
        public int TubeLen { get; set; }


        [Browsable(true)]
        [DisplayName("Типоразмеры"), Description("Список допустимых тпоразмеров"), Category("1.Труба")]
        public L_TypeSize TypeSizes { get; set; }

        [Browsable(true)]
        [DisplayName("Типоразмер"), Description("Текущий типоразмер"), Category("1.Труба")]
        public TypeSize Current { get; set; }

        [Browsable(true)]
        [DisplayName("Выпрямитель"), Description("Настройки порта для выпрямителя/стабилизатора"), Category("3.Оборудование")]
        public ModbusRectifierSettings rectifierSettings { get; set; }

        [Browsable(true)]
        [DisplayName("L502"), Description("Настройки АЦП"), Category("3.Оборудование")]
        public LCardSettings lCardSettings { get; set; }

        [Browsable(true)]
        [DisplayName("Плата тактирования АЦП"), Description("Настройки платы тактирования АЦП"), Category("3.Оборудование")]
        public MTADCSettings mtadcSettings { get; set; }

        [Browsable(true)]
        [DisplayName("PCIE-1730"), Description("Настройки платы ввода/вывода (Advantech PCIE-1730)"), Category("3.Оборудование")]
        public PCIE1730Settings pCIE1730Settings { get; set; }

        public AppSettings():base()
        {
            //zoneSize = DefaultValues.ZoneSize;
            //MaxZones = DefaultValues.MaxZones;
            //TypeSizes = new L_TypeSize(this);
            //mtadcSettings = new MTADCSettings();
            //rectifierSettings = new ModbusRectifierSettings();
        }
    }
}
