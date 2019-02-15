using System;
using System.ComponentModel;

namespace CM
{
    [DisplayName("Размер"), Description("Размер")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class Dim
    {
        [DisplayName("Столбцов"), Description("Столбцов"), DefaultValue(1)]
        public int cols { get; set; }
        [DisplayName("Строк"), Description("Строк"), DefaultValue(1)]
        public int rows { get; set; }
        public int size { get { return cols * rows; } }
        public override string ToString()
        {
            return string.Format("{0}X{1}", cols, rows);
        }
        public Dim(int _cols=1,int _rows=1)
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
        public Block(int _cols=1,int _rows=1,string _order="1,2,3,4")
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
        public HallSensors(int _cols=1,int _rows=1)
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
    public class SensorPars
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

        public int sectionSize { get { return sensors.size * hallSensors.size; } }
        public int mcols { get { return sensors.dim.cols; } }
        public int mrows { get { return sensors.dim.rows; } }
        public int cols { get { return hallSensors.dim.cols; } }
        public int rows { get { return hallSensors.dim.rows; } }

        public SensorPars(int _mcols = 1, int _mrows = 1, int _cols = 1, int _rows = 1)
        {
            sensors = new Block(_mcols, _mrows);
            hallSensors = new HallSensors(_cols, _rows);
        }
    }
}