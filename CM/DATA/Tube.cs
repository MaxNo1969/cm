using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace CM
{
    /// <summary>
    /// Делегат, если данные поменялись
    /// </summary>
    public delegate void DataChanged(IEnumerable<double> _data);
    public interface IWriteable<T>
    {
        int write(IEnumerable<T> _data);
        event DataChanged onDataChanged;
    }

    [Serializable]
    public class Tube : IWriteable<double>
    {
        /// <summary>
        /// Событие, вызывается если данные поменялись
        /// </summary>
        public event DataChanged onDataChanged = null;

        public List<double> raw { get; private set; }
        public int rawDataSize { get { return raw.Count; } } 
        public TypeSize typeSize;

        public int sectionSize { get { return typeSize.sensors.sectionSize; } }
        public int sections { get { return raw.Count / sectionSize; } }
        public int mcols { get { return typeSize.sensors.sensors.dim.cols; } }
        public int mrows { get { return typeSize.sensors.sensors.dim.rows; } }
        public int cols { get { return typeSize.sensors.hallSensors.dim.cols; } }
        public int rows { get { return typeSize.sensors.hallSensors.dim.rows; } }

        [NonSerialized]
        private double[,,,] sensorsAvgValues = null;
        [NonSerialized]
        private double[,,,] sensorsMaxDeviationsValues = null;

        public int len;

        public Tube(TypeSize _ts,int _len)
        {
            len = _len;
            raw = new List<double>();
            typeSize = _ts;
        }

        /// <summary>
        /// Индексатор для доступа к данным по трубе по датчикам
        /// </summary>
        /// <param name="_mc">Столбец  датчиков</param>
        /// <param name="_mr">Ряд датчиков</param>
        /// <param name="_c">Строка матрицы датчиков</param>
        /// <param name="_r">Ряд в матрице датчиков</param>
        /// <param name="_i">Номер измерения</param>
        /// <returns>Значение</returns>
        public double this[int _mc, int _mr, int _c, int _r, int _i]
        {
            get
            {
                int ind = _mc * mrows * cols * rows + _mr * cols * rows + _r * cols + _c;
                double val;
                try
                {
                    val = raw[ind + _i * sectionSize];
                }
                catch
                {
                    val = double.NaN;
                    //throw (new IndexOutOfRangeException());
                }
                return val;
            }
        }

        #region Работа с дампом
        public int readDump(string _fileName)
        {
            List<double> data = DumpHelper.readDumpFile(_fileName);
            raw.Clear();
            write(data);
            return raw.Count;
        }
        public bool writeDump(string _fileName)
        {
            if (DumpHelper.writeDumpFile(_fileName, raw)) return true;
            return false;
        }
        #endregion Работа с дампом

        #region Работа с CSV
        public int readCSV(string _fileName)
        {
            List<double> data = CsvHelper.readCsvFile(_fileName);
            raw.Clear();
            write(data);
            return raw.Count;
        }
        public bool writeCSV(string _fileName)
        {
            if (CsvHelper.writeCsvFile(_fileName, raw)) return true;
            return false;
        }
        #endregion Работа с CSV

        #region Сериализация
        /// <summary>
        /// Сериализация в файл для сохранения данных по трубе 
        /// </summary>
        /// <param name="_fileName">Имя файла</param>
        /// <returns>true - если труба успешно сохранена</returns>
        public static bool save(Tube _tube,string _fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(_fileName, FileMode.Create))
                {
                    formatter.Serialize(fs, _tube);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Загрузка сохраненной трубы из файла
        /// </summary>
        /// <param name="_fileName">Имя файла</param>
        /// <returns>true - если труба успешно загружена</returns>
        public static bool load(out Tube _tube,string _fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(_fileName, FileMode.Open))
                {
                    _tube = (Tube)formatter.Deserialize(fs);
                    if (_tube != null)
                    {
                        _tube.fillSensorAvgAndDeviationValues();
                        _tube.onDataChanged?.Invoke(null);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                _tube = null;
                return false;
            }
        }
        #endregion Сериализация

        /// <summary>
        /// Получение данных по датчику
        /// </summary>
        /// <param name="_mc">Колонка матрицы</param>
        /// <param name="_mr">Строка матрица</param>
        /// <param name="_c">Колонка датчика</param>
        /// <param name="_r">Строка датчика</param>
        /// <param name="_sensor">Параметры датчиков для разбора массива</param>
        /// <param name="_data">Данные для разбора</param>
        /// <returns>Массив данных по одному датчику</returns>
        public static double[] getSensorData(int _mc, int _mr, int _c, int _r, 
            SensorPars _sensor, double[] _data)
        {
            int dim = _data.Length / _sensor.sectionSize;
            double[] ret = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                int ind = _mc * _sensor.mrows * _sensor.cols * _sensor.rows +
                    _mr * _sensor.cols * _sensor.rows + _r * _sensor.cols + _c;
                ret[i] = _data[i * _sensor.sectionSize + ind];
            }
            return ret;
        }

        /// <summary>
        /// Получить данные по датчику
        /// </summary>
        /// <param name="_mc">Колонка матрицы</param>
        /// <param name="_mr">Строка матрицы</param>
        /// <param name="_c">Колонка датчика</param>
        /// <param name="_r">Строка датчика</param>
        /// <param name="_start">Начальный срез</param>
        /// <param name="_count">Количество срезов</param>
        /// <returns>Массив с данными по датчику</returns>
        public double[] getSensorData(int _mc, int _mr, int _c, int _r, int _start, int _count)
        {
            if (_start < 0 || _start > sections - 1 || _count < 0 || _start + _count > sections - 1)
            {
                return null;
            }
            if (raw.Count == 0)
            {
                return null;
            }
            double[] ret = new double[_count];
            for (int i = 0; i < _count; i++)
            {
                ret[i] = this[_mc, _mr, _c, _r, _start + i];
            }
            return ret;
        }
        /// <summary>
        /// Получить данные по датчику
        /// </summary>
        /// <param name="_mc">Колонка матрицы</param>
        /// <param name="_mr">Строка матрицы</param>
        /// <param name="_c">Колонка датчика</param>
        /// <param name="_r">Строка датчика</param>
        /// <returns>Массив с данными по датчику</returns>
        public double[] getSensorData(int _mc, int _mr, int _c, int _r)
        {
            if (raw.Count == 0)
            {
                return null;
            }
            double[] ret = new double[sections];
            for (int i = 0; i < sections; i++)
            {
                ret[i] = this[_mc, _mr, _c, _r, i];
            }
            return ret;
        }

        /// <summary>
        /// Получить нормализованные данные по датчику
        /// </summary>
        /// <param name="_mc">Колонка матрицы</param>
        /// <param name="_mr">Строка матрицы</param>
        /// <param name="_c">Колонка датчика</param>
        /// <param name="_r">Строка датчика</param>
        /// <param name="_start">Начальный срез</param>
        /// <param name="_count">Количество срезов</param>
        /// <returns>Массив с данными по датчику</returns>
        public double[] getNormSensorData(int _mc, int _mr, int _c, int _r, int _start, int _count)
        {
            if (_start < 0 || _start > sections - 1 || _count < 0 || _start + _count > sections)
            {
                return null;
            }
            if (raw.Count == 0)
            {
                return null;
            }
            double[] ret = new double[_count];
            for (int i = 0; i < _count; i++)
            {
                ret[i] = getNorm(_mc, _mr, _c, _r, _start + i);
            }
            return ret;
        }
        /// <summary>
        /// Получить данные по датчику
        /// </summary>
        /// <param name="_mc">Колонка матрицы</param>
        /// <param name="_mr">Строка матрицы</param>
        /// <param name="_c">Колонка датчика</param>
        /// <param name="_r">Строка датчика</param>
        /// <returns>Массив с данными по датчику</returns>
        public double[] getNormSensorData(int _mc, int _mr, int _c, int _r)
        {
            if (raw.Count == 0)
            {
                return null;
            }
            double[] ret = new double[sections];
            for (int i = 0; i < sections; i++)
            {
                ret[i] = getNorm(_mc, _mr, _c, _r, i);
            }
            return ret;
        }



        /// <summary>
        /// Получить нормализованное значение по датчику
        /// </summary>
        /// <param name="_mc">Столбец  датчиков</param>
        /// <param name="_mr">Ряд датчиков</param>
        /// <param name="_c">Строка матрицы датчиков</param>
        /// <param name="_r">Ряд в матрице датчиков</param>
        /// <returns>Значение</returns>
        public double getNorm(int _mc, int _mr, int _c, int _r, int _i)
        {
            int ind = _mc * mrows * cols * rows + _mr * cols * rows + _r * cols + _c;
            double val;
            try
            {
                val = Math.Abs(raw[ind + _i * sectionSize] - sensorsAvgValues[_mc, _mr, _c, _r]) / sensorsMaxDeviationsValues[_mc, _mr, _c, _r];
            }
            catch
            {
                val = double.NaN;
                //throw (new IndexOutOfRangeException());
            }
            return val;
        }

        private double getSensorAvg(int _mc, int _mr, int _c, int _r)
        {
            return getSensorData(_mc, _mr, _c, _r).Average();
        }

        private double getSensorMaxDeviation(int _mc, int _mr, int _c, int _r)
        {
            double avg = getSensorAvg(_mc, _mr, _c, _r);
            double[] sensorData = getSensorData(_mc, _mr, _c, _r);
            double[] deviations = new double[sensorData.Length];
            for (int i = 0; i < sensorData.Length; i++) deviations[i] = Math.Abs(sensorData[i] - avg);
            return deviations.Max();
        }

        internal void fillSensorAvgAndDeviationValues()
        {
            sensorsAvgValues = new double[mcols, mrows, cols, rows];
            sensorsMaxDeviationsValues = new double[mcols, mrows, cols, rows];
            for (int mcol = 0; mcol < mcols; mcol++)
            {
                for (int mrow = 0; mrow < mrows; mrow++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        for (int row = 0; row < rows; row++)
                        {
                            sensorsAvgValues[mcol, mrow, col, row] = getSensorAvg(mcol, mrow, col, row);
                            sensorsMaxDeviationsValues[mcol, mrow, col, row] = getSensorMaxDeviation(mcol, mrow, col, row);
                        }
                    }
                }
            }
        }

        public int write(IEnumerable<double> _data)
        {
            raw.AddRange(_data);
            fillSensorAvgAndDeviationValues();
            onDataChanged?.Invoke(_data);
            return _data.Count();
        }
    }
}
