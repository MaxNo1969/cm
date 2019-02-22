using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CM
{
    [DisplayName("Размеры"), Description("Размеры")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class Dim
    {
        [DisplayName("Столбцов"), Description("Столбцов"), /*DefaultValue(1)*/]
        public int cols { get; set; }
        [DisplayName("Строк"), Description("Строк"), /*DefaultValue(1)*/]
        public int rows { get; set; }
        [XmlIgnore]
        public int size { get { return cols * rows; } }
        public override string ToString()
        {
            return string.Format("{0}X{1}", cols, rows);
        }
        public Dim()
        {
            cols = 1;
            rows = 1;
        }
        public Dim(int _cols,int _rows)
        {
            cols = _cols;
            rows = _rows;
        }
    }

    /// <summary>
    /// Настройки для целых датчиков
    /// </summary>
    [DisplayName("Датчики"), Description("Датчики в сборе")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class Block
    {
        /// <summary>
        /// Количество целых датчиков
        /// </summary> 
        [DisplayName("Размер"), Description("Размер")]
        public Dim dim { get; set; }
        /// <summary>
        /// Порядок чтения датчиков
        /// </summary> 
        [DisplayName("Порядок чтения датчиков"), Description("Порядок чтения датчиков"), Category("Установка")]
        [DefaultValue("1,2,3,4")]
        public string sensorOrder { get; set; }
        /// <summary>
        /// Преобразование в строку для вывода в PropertyGrid
        /// </summary>
        /// <returns>строка для вывода в PropertyGrid</returns>
        public override string ToString()
        {
            return dim.ToString();
        }
        public Block()
        {
            dim = new Dim();
        }
        public Block(int _cols,int _rows,string _order="1,2,3,4")
        {
            dim = new Dim(_cols, _rows);
            sensorOrder = _order;
        }
        public int size { get { return dim.size; } }
    }
    /// <summary>
    /// Один датчик
    /// </summary>
    [DisplayName("Датчики Холла"), Description("Датчики Холла в одном блоке")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class HallSensors
    {
        /// <summary>
        /// Количество целых датчиков
        /// </summary> 
        [DisplayName("Размер"), Description("Размер")]
        public Dim dim { get; set; }

        #region Линейные размеры
        /// <summary>
        /// Размер элемента по горизонали (мм)
        /// </summary>
        [DisplayName("Горизонтальный размер"), Description("Размер элемента по горизонали (мм)")]
        [DefaultValue(5)]
        public int elementWidth { get; set; }
        /// <summary>
        /// Размер элемента по вертикали  (мм)
        /// </summary>
        [DisplayName("Вертикальный размер"), Description("Размер элемента по вертикали (мм)")]
        [DefaultValue(8)]
        public int elementHeight { get; set; }
        /// <summary>
        /// Горизонтальный зазор между элементами (мм)
        /// </summary>
        [DisplayName("Горизонтальный зазор"), Description("Горизонтальный зазор между элементами (мм)")]
        [DefaultValue(2)]
        public int xGap { get; set; }
        /// <summary>
        /// Вертикальный зазор между элементами
        /// </summary>
        [DisplayName("Вертикльный зазор"), Description("Вертикальный зазор между элементами (мм)")]
        [DefaultValue(2)]
        public int yGap { get; set; }
        #endregion
        /// <summary>
        /// Преобразование в строку для вывода в PropertyGrid
        /// </summary>
        /// <returns>строка для вывода в PropertyGrid</returns>
        public override string ToString()
        {
            return dim.ToString();
        }
        public HallSensors()
        {
            dim = new Dim();
            elementWidth = DefaultValues.hallSensorWidth;
            elementHeight = DefaultValues.hallSensorHeight;
            xGap = DefaultValues.xGap;
            yGap = DefaultValues.yGap;
        }
        public HallSensors(int _cols,int _rows)
        {
            dim = new Dim(_cols, _rows);
            elementWidth = DefaultValues.hallSensorWidth;
            elementHeight = DefaultValues.hallSensorHeight;
            xGap = DefaultValues.xGap;
            yGap = DefaultValues.yGap;
        }
        public int size { get { return dim.size; } }
    }


    /// <summary>
    /// Параметры датчиков
    /// </summary>
    [DisplayName("Настройки датчиков"), Description("Настройка датчиков")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class SensorSettings:ParBase
    {
        /// <summary>
        /// Матрица больших датчиков
        /// </summary>
        [DisplayName("Датчики"), Description("Настройка параметров целых датчиков")]
        public Block sensors { get; set; }
        /// <summary>
        /// Матрица датчиков Холла
        /// </summary>
        [DisplayName("Датчики Холла"), Description("Настройка параметров датчиков Холла в одном блоке")]
        public HallSensors hallSensors { get; set; }
        /// <summary>
        /// Строковое представление
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}X{1}", sensors.ToString(), hallSensors.ToString());
        }

        [XmlIgnore]
        public int sectionSize { get { return sensors.size * hallSensors.size; } }
        [XmlIgnore]
        public int mcols { get { return sensors.dim.cols; } }
        [XmlIgnore]
        public int mrows { get { return sensors.dim.rows; } }
        [XmlIgnore]
        public int cols { get { return hallSensors.dim.cols; } }
        [XmlIgnore]
        public int rows { get { return hallSensors.dim.rows; } }

        public SensorSettings()
        {
            sensors = new Block(1, 1);
            hallSensors = new HallSensors(1, 1);
        }

        public SensorSettings(int _mcols = 1, int _mrows = 1, int _cols = 1, int _rows = 1)
        {
            sensors = new Block(_mcols, _mrows);
            hallSensors = new HallSensors(_cols, _rows);
        }
    }
}