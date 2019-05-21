using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CM
{
    /// <summary>
    /// Физические характеристики трубы для привязи по размерам к логической матрице
    /// Поддерживаем поперечные зоны для укрупнения масштаба заключений
    /// </summary>
    [Serializable]
    public class PhysTube
    {
        public const double undefined = double.PositiveInfinity;
        /// <summary>
        /// Индексатор для доступа к данным по трубе по датчикам (по логическим единицам)
        /// </summary>
        /// <param name="_c">Столбец участка</param>
        /// <param name="_r">Строка усастка</param>
        /// <returns>Значение(???)</returns>
        public double this[int _c, int _r]
        {
            get
            {
                return data[_c, _r];
            }
            set
            {
                data[_c, _r] = value;
            }
        }

        /// <summary>
        /// Длинна трубы в мм
        /// Задаём один раз в конструкторе, после этого не меняем
        /// </summary>
        public double len;

        /// <summary>
        /// Диаметр трубы нужен только для определения ширины развертки
        /// Задаём один раз в конструкторе, после этого не меняем
        /// </summary>
        private readonly double diameter;

        /// <summary>
        /// Скорость движения трубы
        /// </summary>
        public double speed
        {
            get
            { return (Program.settings.TubeSpeed > 0) ? Program.settings.TubeSpeed : DefaultValues.Speed; }
            set
            { Program.settings.TubeSpeed = value; }
        }

        /// <summary>
        /// Время прохождения одной ячейки
        /// </summary>
        public int cellTime { get { return (int)(cellXSize / speed); } }

        /// <summary>
        /// Необходимый размер буфера для метрического отображения
        /// </summary>
        public int logDataSize
        {
            get
            {
                return Width * Height;
            }
        }
        /// <summary>
        /// Физическое отображение трубы
        /// </summary>
        public double[,] data = null;
        /// <summary>
        /// Указатель на текущее положение для чтения данных
        /// </summary>
        public int startReadX;
        /// <summary>
        /// Указатель на текущее положение для чтения данных
        /// </summary>
        public int startReadY;

        /// <summary>
        /// Указатель на текущее положение конца записанных данных
        /// </summary>
        public int endWritedX;
        /// <summary>
        /// Указатель на текущее положение конца записанных данных
        /// </summary>
        public int endWritedY;

        /// <summary>
        /// Труба
        /// </summary>
        public TypeSize ts;
        public int mcols { get { return ts.sensors.mcols; } }
        public int mrows { get { return ts.sensors.mrows; } }
        public int cols { get { return ts.sensors.cols; } }
        public int rows { get { return ts.sensors.rows; } }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_ts">Типоразмер</param>
        /// <param name="_len">Длина трубы</param>
        public PhysTube(TypeSize _ts)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            //if (Program.settings.TubeSpeed != 0)
            //    speed = Program.settings.TubeSpeed;
            //else
            //    speed = DefaultValues.Speed;
            ts = _ts;
            diameter = ts.diameter;

            endWritedX = 0;
            endWritedY = 0;
            startReadX = 0;
            startReadY = 0;

            try
            {
                data = new double[Width, Height];
                //if (Program.settings.ZoneSize == 0) Program.settings.ZoneSize = DefaultValues.ZoneSize;
                //int zones = (int)len / Program.settings.ZoneSize;
                reset();
                #region Логирование 
                {
                    string msg = string.Format("Width={0}, Height={1},cellXSize={2},cellYSize={3}, cellTime={4}",
                        Width, Height, cellXSize, cellYSize, cellTime);
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
            }
            catch (InsufficientMemoryException ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("Не удалось выделить {0,6N2} М.", Width * Height * sizeof(double));
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");
                }
                #endregion
                throw ex;
            }
        }

        public bool expand(int numZones)
        {
            try
            {
                double[,] newData = new double[Width + numZones * logZoneSize, Height];
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                        newData[x, y] = data[x, y];
                data = newData;
                len += zoneSize * numZones;
                return true;
            }
            catch(Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message );
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");

                }
                #endregion
                return false;
            }
        }

        /// <summary>
        /// Ширина трубы в мм
        /// Явно не задаём берем по диаметру
        /// </summary>
        public double height
        {
            get
            {
                return diameter * Math.PI;
            }
        }

        /// <summary>
        /// привяжемся к размеру одного датчика 
        /// с квадратом не проканает
        /// </summary>
        /// <summary>
        /// Размер в мм одного участка по горизонтали
        /// </summary>
        public double cellXSize
        {
            get
            {
                if (ts.sensors.hallSensors.elementWidth == 0) return DefaultValues.hallSensorWidth;
                return ts.sensors.hallSensors.elementWidth + ts.sensors.hallSensors.xGap;
            }
            //get
            //{
            //    return Program.settings.ZoneSize / Tube.GetsectionsPerZone();            //}
        }

        /// <summary>
        /// Размер в мм одного участка по вертикали
        /// </summary>
        public double cellYSize
        {
            get
            {
                return height / Height;
            }
        }

        /// <summary>
        /// Длинна трубы в логических единицах
        /// </summary>
        public int Width
        {
            get
            {
                return (int)Math.Ceiling(Program.settings.TubeLen / cellXSize)+p2lx(Program.settings.sensorsDistance);
                //return Tube.GetsectionsPerZone() * Program.settings.TubeLen / Program.settings.ZoneSize;
            }
        }
        /// <summary>
        /// Ширина развертки трубы в логических единицах
        /// Берем по суммарному количеству строк во всех датчиках
        /// </summary>
        public int Height { get { return ts.sensors.mrows * ts.sensors.rows; } }
        //public int logZoneSize { get { return zoneLen / xSize; } }

        /// <summary>
        /// Перевод из логических единиц в метры
        /// </summary>
        /// <param name="_x">Логическая координата</param>
        /// <returns>Расстояние от начала трубы</returns>
        public int l2px(int _x)
        {
            return (int)(_x * cellXSize);
        }
        /// <summary>
        /// Перевод из логических единиц в метры
        /// </summary>
        /// <param name="_y">Логическая координата</param>
        /// <returns>Расстояние от начала трубы</returns>
        public int l2py(int _y)
        {
            return (int)(_y * cellYSize);
        }
        /// <summary>
        /// Перевод из метров в логические единицы
        /// </summary>
        /// <param name="_x">Расстояние от начала трубы</param>
        /// <returns>Логическая координата</returns>
        public int p2lx(int _x)
        {
            return (int)(_x / cellXSize);
        }
        /// <summary>
        /// Перевод из метров в логические единицы
        /// </summary>
        /// <param name="_y">Расстояние от начала трубы</param>
        /// <returns>Логическая координата</returns>
        public int p2ly(int _y)
        {
            return (int)(_y / cellYSize);
        }

        /// <summary>
        /// Размер зоны в мм
        /// </summary>
        public int zoneSize
        {
            get
            {
                return DefaultValues.ZoneSize;
            }
        }
        /// <summary>
        /// Размер зоны в логических единицах
        /// </summary>
        public int logZoneSize
        {
            get
            {
                return Tube.GetsectionsPerZone();
            }
        }

        /// <summary>
        /// Сброс физической модели
        /// </summary>
        /// <param name="_val">Величина для заполнения</param>
        public void reset(double _val = undefined)
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    data[i, j] = _val;
            endWritedX = 0;
            endWritedY = 0;
            startReadX = 0;
            startReadY = 0;
        }
    }
}
