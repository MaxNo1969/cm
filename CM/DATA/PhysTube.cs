using Protocol;
using System;
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
        public readonly double len;

        /// <summary>
        /// Диаметр трубы нужен только для определения ширины развертки
        /// Задаём один раз в конструкторе, после этого не меняем
        /// </summary>
        private readonly double diameter;

        /// <summary>
        /// Скорость движения трубы
        /// </summary>
        private readonly double speed;
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
        private readonly double[,] data = null;

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
        public Tube tube;
        public int mcols { get { return tube.mcols; } }
        public int mrows { get { return tube.mrows; } }
        public int cols { get { return tube.cols; } }
        public int rows { get { return tube.rows; } }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">Труба</param>
        public PhysTube(Tube _tube)
        {
            tube = _tube;
            len = tube.len;
            diameter = tube.typeSize.diameter;

            endWritedX = 0;
            endWritedY = 0;
            startReadX = 0;
            startReadY = 0;

            try
            {
                data = new double[Width, Height];
                int zones = (int)len / DefaultValues.zoneSize;
                write(tube, 0, tube.sections, 0, zones);
            }
            catch (InsufficientMemoryException ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("Не удалось выделить {0,6N2} М.", Width * Height * sizeof(double));
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");
                }
                #endregion
                throw ex;
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
                if (tube.typeSize.sensors.hallSensors.elementWidth == 0) return DefaultValues.hallSensorWidth;
                return tube.typeSize.sensors.hallSensors.elementWidth;
            }
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
                return (int)(len / cellXSize);
            }
        }
        /// <summary>
        /// Ширина развертки трубы в логических единицах
        /// Берем по суммарному количеству строк во всех датчиках
        /// </summary>
        public int Height { get { return tube.mrows * tube.rows; } }
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
                return DefaultValues.zoneSize;
            }
        }
        /// <summary>
        /// Размер зоны в логических единицах
        /// </summary>
        public int logZoneSize
        {
            get
            {
                return zoneSize / (int)cellXSize;
            }
        }

        /// <summary>
        /// Запись данных в физическую модель
        /// </summary>
        /// <param name="_tube">Труба для чтения и записи</param>
        /// <param name="_start">Начало данных(номер среза) </param>
        /// <param name="_sz">Размер записываемых данных (в срезах)</param>
        /// <param name="_znStart">Первая зоня для записи</param>
        /// <param name="_znCnt">Количество зон</param>
        public bool write(Tube _tube, int _start, int _sz, int _znStart, int _znCnt)
        {
            if ((_znStart + _znCnt) * logZoneSize > logDataSize - logZoneSize)
            {
                #region Логирование 
                {
                    string msg = string.Format("Попытка записи за границу массива:_znStart={0}, _znCnt={1}, logZoneSize={2}, logDataSize={3}",
                        _znStart, _znCnt, logZoneSize, logDataSize);
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
                return false;
            }
            double[,,,][] tmpData = createNormalizedSensorsArray(_tube, _start, _sz, _znStart, _znCnt);
            int measPerCell = (int)Math.Round((double)_sz / logZoneSize / _znCnt);
            double[] accum = new double[_tube.mrows * _tube.rows];
            for (int zn = _znStart; zn < _znStart + _znCnt; zn++)
            {
                for (int i = 0; i < logZoneSize; i++)
                {
                    for (int mrow = 0; mrow < _tube.mrows; mrow++)
                    {
                        for (int row = 0; row < _tube.rows; row++)
                        {
                            int cnt = 0;
                            accum[mrow * _tube.rows + row] = 0;
                            for (int mcol = 0; mcol < _tube.mcols; mcol++)
                            {
                                for (int col = 0; col < _tube.cols; col++)
                                {
                                    for (int j = 0; j < measPerCell && (zn - _znStart) * logZoneSize * measPerCell + i * measPerCell + j < _sz; j++)
                                    {
                                        accum[mrow * _tube.rows + row] += Math.Abs(tmpData[mcol, mrow, col, row][(zn - _znStart) * logZoneSize * measPerCell + i * measPerCell + j]);
                                        cnt++;
                                    }
                                }
                            }
                            if (cnt > 1) accum[mrow * _tube.rows + row] /= cnt;
                            data[zn * logZoneSize + i, mrow * _tube.rows + row] = accum[mrow * _tube.rows + row];
                        }
                    }
                }
            }
            endWritedX = (_znStart + _znCnt) * logZoneSize;
            return true;
        }

        static double[,,,][] createNormalizedSensorsArray(Tube _tube, int _start, int _sz, int _znStart, int _znCnt)
        {
            double[,,,][] tmpData = new double[_tube.mcols, _tube.mrows, _tube.cols, _tube.rows][];
            for (int mcol = 0; mcol < _tube.mcols; mcol++)
            {
                for (int mrow = 0; mrow < _tube.mrows; mrow++)
                {
                    for (int row = 0; row < _tube.rows; row++)
                    {
                        for (int col = 0; col < _tube.cols; col++)
                        {
                            tmpData[mcol, mrow, col, row] = _tube.getNormSensorData(mcol, mrow, col, row, _start, _sz);
                        }
                    }
                }
            }
            return tmpData;
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
