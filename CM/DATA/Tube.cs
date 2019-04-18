using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Protocol;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CM
{
    public class Tube : IDataWriter<double>
    {
        /// <summary>
        /// Событие, вызывается если данные поменялись
        /// </summary>
        public event DataChanged onDataChanged = null;

        //public List<double> raw { get; private set; }
        //public TypeSize typeSize;
        ///// <summary>
        ///// Список стробов для зонирования
        ///// </summary>
        //public List<Strobe> rtube.strobes;
        public RawTube rtube;
        public PhysTube ptube;

        public int rawDataSize { get { return rtube.rawDataSize; } }
        public int sectionSize { get { return rtube.ts.sensors.sectionSize; } }
        public int sections { get { return rawDataSize / sectionSize; } }
        public int mcols { get { return rtube.ts.sensors.sensors.dim.cols; } }
        public int mrows { get { return rtube.ts.sensors.sensors.dim.rows; } }
        public int cols { get { return rtube.ts.sensors.hallSensors.dim.cols; } }
        public int rows { get { return rtube.ts.sensors.hallSensors.dim.rows; } }

        private double[,,,] sensorsAvgValues;
        private double[,,,] sensorsMaxDeviationsValues;

        private void loadAvgAndDeviations()
        {
            string fName = string.Format("{0}.csv", rtube.ts.Name);
            string s;
            string[] values;
            sensorsAvgValues = new double[mcols, mrows, cols, rows];
            sensorsMaxDeviationsValues = new double[mcols, mrows, cols, rows];
            if (File.Exists(fName))
            {
                using (StreamReader sr = new StreamReader(fName))
                {
                    for (int mcol = 0; mcol < mcols; mcol++)
                        for (int mrow = 0; mrow < mrows; mrow++)
                            for (int col = 0; col < cols; col++)
                                for (int row = 0; row < rows; row++)
                                {
                                    s = sr.ReadLine();
                                    if(s!=null)
                                    {
                                        values = s.Split(new char[] { ';' });
                                        sensorsAvgValues[mcol, mrow, col, row] = Convert.ToDouble(values[0]);
                                        sensorsMaxDeviationsValues[mcol, mrow, col, row] = Convert.ToDouble(values[1]);
                                    }
                                }
                }
            }
        }


        public int len;

        public Tube(TypeSize _ts,int _len)
        {
            len = _len;
            rtube = new RawTube(_ts,_len);
            ptube = new PhysTube(_ts,_len);
            loadAvgAndDeviations();
            //onDataChanged?.Invoke(null);
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
                    val = rtube.data[ind + _i * sectionSize];
                }
                catch
                {
                    val = double.NaN;
                    //throw (new IndexOutOfRangeException());
                }
                return val;
            }
        }

        /// <summary>
        /// Индексатор для доступа к данным по трубе по логическим ячейкам
        /// </summary>
        /// <param name="_c">Строка матрицы датчиков</param>
        /// <param name="_r">Ряд в матрице датчиков</param>
        /// <returns>Значение</returns>
        public double this[int _c, int _r]
        {
            get
            {
                double val;
                try
                {
                    val = ptube[_c,_r];
                }
                catch
                {
                    val = double.NaN;
                    //throw (new IndexOutOfRangeException());
                }
                return val;
            }
        }

        #region Сериализация
        /// <summary>
        /// Сериализация в файл для сохранения данных по трубе 
        /// </summary>
        /// <param name="_fileName">Имя файла</param>
        /// <returns>true - если труба успешно сохранена</returns>
        public static bool save(Tube _tube,string _fileName)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", "Tube", System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(_fileName, FileMode.Create))
                {
                    formatter.Serialize(fs, _tube.rtube);
                    #region Логирование 
                    {
                        string msg = string.Format("Данные сохранены. Размер: {0}", fs.Length);
                        string logstr = string.Format("{0}: {1}: {2}", "Tube", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    return true;
                }
            }
            catch(Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message );
                    string logstr = string.Format("{0}: {1}: {2}", "Tube", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");

                }
                #endregion
                return false;
            }
        }
        /// <summary>
        /// Загрузка сохраненной трубы из файла
        /// </summary>
        /// <param name="_fileName">Имя файла</param>
        /// <returns>true - если труба успешно загружена</returns>
        public static bool load(ref Tube _tube,string _fileName)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", "Tube", System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(_fileName, FileMode.Open))
                {
                    _tube.rtube = (RawTube)formatter.Deserialize(fs);
                    _tube.fillSensorAvgAndDeviationValues();
                    //using (StreamWriter sw = new StreamWriter(string.Format("{0}.csv", _tube.rtube.ts.Name)))
                    //{
                    //    for(int mcol=0;mcol<_tube.mcols;mcol++)
                    //        for (int mrow=0;mrow<_tube.mrows;mrow++)
                    //            for(int col=0;col<_tube.cols;col++)
                    //                for(int row=0;row<_tube.rows;row++)
                    //                {
                    //                    sw.WriteLine(string.Format("{0};{1}", 
                    //                        _tube.sensorsAvgValues[mcol,mrow,col,row], _tube.sensorsMaxDeviationsValues[mcol, mrow, col, row]));
                    //                }
                    //}
                    _tube.ptube = new PhysTube(_tube.rtube.ts, _tube.rtube.len);
                    _tube.raw2phys(0, _tube.sections, 0, _tube.ptube.Width / _tube.ptube.logZoneSize);
                    _tube.onDataChanged?.Invoke(null);
                    return true;
                }
            }
            catch(Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message );
                    string logstr = string.Format("{0}: {1}: {2}", "Tube", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");

                }
                #endregion
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
            SensorSettings _sensor, double[] _data)
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
            if (rtube.data.Count == 0)
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
            if (rtube.data.Count == 0)
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
            if (rtube.data.Count == 0)
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
            if (rtube.data.Count == 0)
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
                val = Math.Abs(rtube.data[ind + _i * sectionSize] - sensorsAvgValues[_mc, _mr, _c, _r]) / sensorsMaxDeviationsValues[_mc, _mr, _c, _r];
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
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            sensorsAvgValues = new double[mcols, mrows, cols, rows];
            sensorsMaxDeviationsValues = new double[mcols, mrows, cols, rows];
            //for (int mcol = 0; mcol < mcols; mcol++)
            //{
            //    for (int mrow = 0; mrow < mrows; mrow++)
            //    {
            //        for (int col = 0; col < cols; col++)
            //        {
            //            for (int row = 0; row < rows; row++)
            //            {
            //                sensorsAvgValues[mcol, mrow, col, row] = getSensorAvg(mcol, mrow, col, row);
            //                sensorsMaxDeviationsValues[mcol, mrow, col, row] = getSensorMaxDeviation(mcol, mrow, col, row);
            //            }
            //        }
            //    }
            //}
            Parallel.For(0, mcols, mcol =>
                 {
                     Parallel.For(0, mrows, mrow =>
                       {
                           Parallel.For(0, cols, col =>
                             {
                                 Parallel.For(0, rows, row =>
                                   {
                                       sensorsAvgValues[mcol, mrow, col, row] = getSensorAvg(mcol, mrow, col, row);
                                       sensorsMaxDeviationsValues[mcol, mrow, col, row] = getSensorMaxDeviation(mcol, mrow, col, row);
                                   });
                             });
                       });
                 });
        }

        /// <summary>
        /// Обработка получения нового строба
        /// </summary>
        /// <returns>Номер среза окончания записанной зоны</returns>
        internal int addStrobe()
        {
            int zoneBound = 0;
            int zoneStart = 0;
            lock (rtube.strobes)
            {
                if (rtube.strobes.Count > 0)
                {
                    zoneBound = rawDataSize / sectionSize;
                    zoneStart = rtube.strobes.Last().bound;
                }
                else
                {
                    Strobe strobe = new Strobe(zoneBound);
                    rtube.strobes.Add(strobe);
                    #region Логирование 
                    {
                        string msg = string.Format(@"Время: {0:hh\:mm\:ss\.ff} {1}-{2} {3}", strobe.dt, zoneStart, zoneBound, zoneBound - zoneStart); string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                }
            }
            if (zoneBound - zoneStart > 0)
            {
                if (raw2phys(zoneStart, zoneBound - zoneStart, rtube.strobes.Count - 1, 1))
                {
                    onDataChanged?.Invoke(null);
                    Strobe strobe = new Strobe(zoneBound);
                    rtube.strobes.Add(strobe);
                    #region Логирование 
                    {
                        string msg = string.Format(@"Время: {0:hh\:mm\:ss\.ff} {1}-{2} {3}", strobe.dt, zoneStart, zoneBound, zoneBound - zoneStart); string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    return strobe.bound * sectionSize;
                }
            }
            return 0;
        }

        /// <summary>
        /// Запись данных в физическую модель
        /// </summary>
        /// <param name="_start">Начало данных(номер среза) </param>
        /// <param name="_sz">Размер записываемых данных (в срезах)</param>
        /// <param name="_znStart">Первая зоня для записи</param>
        /// <param name="_znCnt">Количество зон</param>
        public bool raw2phys(int _start, int _sz, int _znStart, int _znCnt)
        {
            if ((_znStart + _znCnt) * ptube.logZoneSize > ptube.Width - ptube.logZoneSize)
            {
                #region Логирование 
                {
                    string msg = string.Format("Попытка записи за границу массива:_znStart={0}, _znCnt={1}, logZoneSize={2}, logDataSize={3}",
                        _znStart, _znCnt, ptube.logZoneSize, ptube.logDataSize);
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
                ptube.expand(1);
                //return false;
            }
            double[,,,][] tmpData = createNormalizedSensorsArray(_start, _sz);
            int measPerCell = (int)Math.Round((double)_sz / ptube.logZoneSize / _znCnt);
            double[] accum = new double[mrows * rows];
            for (int zn = _znStart; zn < _znStart + _znCnt; zn++)
            {
                for (int i = 0; i < ptube.logZoneSize; i++)
                {
                    for (int mrow = 0; mrow < mrows; mrow++)
                    {
                        for (int row = 0; row < rows; row++)
                        {
                            int cnt = 0;
                            accum[mrow * rows + row] = 0;
                            for (int mcol = 0; mcol < mcols; mcol++)
                            {
                                for (int col = 0; col < cols; col++)
                                {
                                    for (int j = 0; j < measPerCell && (zn - _znStart) * ptube.logZoneSize * measPerCell + i * measPerCell + j < _sz; j++)
                                    {
                                        accum[mrow * rows + row] += Math.Abs(tmpData[mcol, mrow, col, row][(zn - _znStart) * ptube.logZoneSize * measPerCell + i * measPerCell + j]);
                                        cnt++;
                                    }
                                }
                            }
                            if (cnt > 1) accum[mrow * rows + row] /= cnt;
                            ptube[zn * ptube.logZoneSize + i, mrow * rows + row] = accum[mrow * rows + row];
                        }
                    }
                }
            }
            //Parallel.For(_znStart, _znStart + _znCnt, zn =>
            //{
            //    Parallel.For(0, ptube.logZoneSize, i =>
            //    {
            //        Parallel.For(0, mrows, mrow =>
            //          {
            //              Parallel.For(0, rows, row =>
            //                {
            //                    int cnt = 0;
            //                    accum[mrow * rows + row] = 0;
            //                    for (int mcol = 0; mcol < mcols; mcol++)
            //                    {
            //                        for (int col = 0; col < cols; col++)
            //                        {
            //                            for (int j = 0; j < measPerCell && (zn - _znStart) * ptube.logZoneSize * measPerCell + i * measPerCell + j < _sz; j++)
            //                            {
            //                                accum[mrow * rows + row] += Math.Abs(tmpData[mcol, mrow, col, row][(zn - _znStart) * ptube.logZoneSize * measPerCell + i * measPerCell + j]);
            //                                cnt++;
            //                            }
            //                        }
            //                    }
            //                    if (cnt > 1) accum[mrow * rows + row] /= cnt;
            //                    ptube[zn * ptube.logZoneSize + i, mrow * rows + row] = accum[mrow * rows + row];
            //                });
            //          });
            //    });
            //});
            ptube.endWritedX = (_znStart + _znCnt) * ptube.logZoneSize;
            return true;
        }



        private double[,,,][] createNormalizedSensorsArray(int _start, int _sz)
        {
            double[,,,][] tmpData = new double[mcols, mrows, cols, rows][];
            //for (int mcol = 0; mcol < mcols; mcol++)
            //{
            //    for (int mrow = 0; mrow < mrows; mrow++)
            //    {
            //        for (int row = 0; row < rows; row++)
            //        {
            //            for (int col = 0; col < cols; col++)
            //            {
            //                tmpData[mcol, mrow, col, row] = getNormSensorData(mcol, mrow, col, row, _start, _sz);
            //            }
            //        }
            //    }
            //}
            Parallel.For(0, mcols, mcol =>
            {
                Parallel.For(0, mrows, mrow =>
                {
                    Parallel.For(0, cols, col =>
                    {
                        Parallel.For(0, rows, row =>
                        {
                            tmpData[mcol, mrow, col, row] = getNormSensorData(mcol, mrow, col, row, _start, _sz);
                        });
                    });
                });
            });
            return tmpData;
        }

        /// <summary>
        /// Запись данных в физическую модель
        /// </summary>
        /// <param name="_start">Начало данных(номер среза) </param>
        /// <param name="_sz">Размер записываемых данных (в срезах)</param>
        /// <param name="_znStart">Первая зоня для записи</param>
        /// <param name="_znCnt">Количество зон</param>
        public bool phys2raw(int _start, int _sz, int _znStart, int _znCnt)
        {
            int measPerCell = (int)Math.Round((double)_sz / ptube.logZoneSize / _znCnt);
            for(int zn=_znStart;zn<_znStart+_znCnt;zn++)
                for(int x=0;x<ptube.logZoneSize;x++)
                    for(int mrow=0;mrow<mrows;mrow++)
                        for(int row=0;row<rows;row++)
                        {
                            for(int i=0;i<measPerCell;i++)
                            {
                                rtube[0, mrow, zn * ptube.logZoneSize * measPerCell + x * measPerCell + i, row, _start] =
                                    ptube[zn * ptube.logZoneSize + x, mrow * rows + row];
                            }
                        }
            return true;
        }

        #region IDataWriter implementation
        int startWriteZoneSection = 0;
        int zones = 0;
        public int Write(IEnumerable<double> _data)
        {
            int measesPerZone = (int)((double)Program.settings.ZoneSize * Program.settings.lCardSettings.FrequencyCollect / DefaultValues.Speed/1000);
            rtube.Write(_data);
            if (rawDataSize / measesPerZone > zones)
            {
                raw2phys(startWriteZoneSection, sections-startWriteZoneSection, zones, 1);
                startWriteZoneSection = sections;
                zones++;
            }
            onDataChanged?.Invoke(_data);
            return _data.Count();
        }

        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }
        #endregion IDataWriter implementation
        public void reset()
        {
            rtube.reset();
            ptube.reset();
        }
    }
}
