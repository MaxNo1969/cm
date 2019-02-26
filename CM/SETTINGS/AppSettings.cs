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
        public int zoneSize { get; set; }

        [Browsable(true)]
        [DisplayName("Максимальное количество зон"), Description("Максимальное количество зон"), Category("1.Труба")]
        public int MaxZones { get; set; }

        [Browsable(false)]
        public TypeSize Current { get { return TypeSizes?.Current; } set { if(TypeSizes != null)TypeSizes.Current = value; } }

        [Browsable(true)]
        [DisplayName("Типоразмер"), Description("Настройка типоразмера"), Category("1.Труба")]
        public L_TypeSize TypeSizes { get; set; }

        [Browsable(true)]
        [DisplayName("Плата тактирования АЦП"), Description("Настройки платы тактирования АЦП"), Category("3.Оборудование")]
        public MTADCSettings mtadcSettings { get; set; }

        [Browsable(true)]
        [DisplayName("Выпрямитель"), Description("Настройки порта для выпрямителя/стабилизатора"), Category("3.Оборудование")]
        public ModbusRectifierSettings rectifierSettings { get; set; }

        public AppSettings()
        {
            //zoneSize = DefaultValues.ZoneSize;
            //MaxZones = DefaultValues.MaxZones;
            //TypeSizes = new L_TypeSize(this);
            //mtadcSettings = new MTADCSettings();
            //rectifierSettings = new ModbusRectifierSettings();
            Type t = GetType();
            PropertyInfo[] pii = t.GetProperties();
            foreach(PropertyInfo pi in pii)
            {
                AddNew(pi);
            }
        }
    }
}
