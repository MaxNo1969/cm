using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Protocol;
using System.Diagnostics;
using System.Threading.Tasks;
using CM;

namespace CM
{
    public class Tube : IDataWriter<double>
    {
        /// <summary>
        /// Событие, вызывается если данные поменялись
        /// </summary>
        public DataChanged onDataChanged = null;

        public RawTube rtube;
        public PhysTube ptube;

        ///// <summary>
        ///// Список стробов для зонирования
        ///// </summary>
        public List<Zone> Zones;

        public int rawDataSize { get { return rtube.rawDataSize; } }
        public static int sectionSize { get { return Program.settings.Current.sensors.sectionSize; } }
        public int sections { get { return rawDataSize / sectionSize; } }
        public static int mcols { get { return Program.settings.Current.sensors.sensors.dim.cols; } }
        public static int mrows { get { return Program.settings.Current.sensors.sensors.dim.rows; } }
        public static int cols { get { return Program.settings.Current.sensors.hallSensors.dim.cols; } }
        public static int rows { get { return Program.settings.Current.sensors.hallSensors.dim.rows; } }

        public static int DeadSectionsEnd => deadSectionsEnd;

        public static int GetsectionsPerZone()
        {
            return (int)((double)Program.settings.ZoneSize * Program.mtdadcFreq / 
                (Program.settings.TubeSpeed * 1000) / Program.settings.Current.sensors.sectionSize);
        }
        public double[,,,] sensorsAvgValues;

        static readonly int deadSectionsStart = Program.settings.Current.DeadZoneStart * GetsectionsPerZone() / Program.settings.ZoneSize;
        static readonly int deadSectionsEnd = Program.settings.Current.DeadZoneFinish * GetsectionsPerZone() / Program.settings.ZoneSize;

        private void loadAvg()
        {
            string fName = string.Format("{0}.csv", rtube.ts.Name);
            string s;
            sensorsAvgValues = new double[mcols, mrows, cols, rows];
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
                                        sensorsAvgValues[mcol, mrow, col, row] = Convert.ToDouble(s);
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
            //loadAvg();
            Zones = new List<Zone>();
            //onDataChanged?.Invoke(null);
            #region Логирование 
            {
                string msg = string.Format("rawDataSize={0}, sectionSize={1}, sections={2}, mcols={3}, mrows={4}, cols={5}, rows={6}, sectionsPerZone={7}",
                    rawDataSize, sectionSize, sections, mcols, mrows, cols, rows, GetsectionsPerZone());
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion            
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
                int ind = (_mc * mrows * cols * rows) + _mr * cols * rows + _r * cols + _c;
                return rtube.data[ind + _i * sectionSize]; ;
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
                return ptube[_c, _r]; 
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
                }
                //_tube.fillSensorAvgValues(deadSectionsStart - 1, _tube.sections - deadSectionsStart);
                _tube.ptube = new PhysTube(_tube.rtube.ts, _tube.rtube.len);
                int zone = 0;
                for (int i = 0; i < _tube.sections; i += Tube.GetsectionsPerZone())
                {
                    _tube.raw2phys(i, Tube.GetsectionsPerZone(), zone++, 1);
                    _tube.onDataChanged?.Invoke(null);
                }
                //_tube.raw2phys(0, _tube.sections, 0, _tube.ptube.Width / _tube.ptube.logZoneSize);
                _tube.onDataChanged?.Invoke(null);
                return true;

            }
            catch (Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message);
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
            if (_start < 0 || _start > sections - 1 || _count < 0 || _start + _count > sections)
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
            int dim = sections;
            double[] ret = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                ret[i] = this[_mc, _mr, _c, _r, i];
            }
            return ret;
        }

        public double getSensorAvg(int _mc, int _mr, int _c, int _r)
        {
            return getSensorData(_mc, _mr, _c, _r).Average();
        }

        public double getSensorAvg(int _mc, int _mr, int _c, int _r, int _start, int _count)
        {
            return getSensorData(_mc, _mr, _c, _r,_start,_count).Average();
        }

        internal void fillSensorAvgValues()
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            sensorsAvgValues = new double[mcols, mrows, cols, rows];
            //for (int mcol = 0; mcol < mcols; mcol++)
            //{
            //    for (int mrow = 0; mrow < mrows; mrow++)
            //    {
            //        for (int col = 0; col < cols; col++)
            //        {
            //            for (int row = 0; row < rows; row++)
            //            {
            //                sensorsAvgValues[mcol, mrow, col, row] = getSensorAvg(mcol, mrow, col, row);
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
                                   });
                             });
                       });
                 });
        }


        internal void fillSensorAvgValues(int _start,int _count)
        {
            #region Логирование 
            {
                string msg = string.Format("start={0}, count={1}", _start, _count);
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            sensorsAvgValues = new double[mcols, mrows, cols, rows];
            //for (int mcol = 0; mcol < mcols; mcol++)
            //{
            //    for (int mrow = 0; mrow < mrows; mrow++)
            //    {
            //        for (int col = 0; col < cols; col++)
            //        {
            //            for (int row = 0; row < rows; row++)
            //            {
            //                if (_start + _count > sections - deadSectionsEnd) _count = sections - deadSectionsEnd - _start;
            //                sensorsAvgValues[mcol, mrow, col, row] = getSensorAvg(mcol, mrow, col, row, _start, _count);
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
                            if (_start + _count > sections - deadSectionsEnd) _count = sections - deadSectionsEnd - _start;
                            sensorsAvgValues[mcol, mrow, col, row] = getSensorData(mcol, mrow, col, row, _start, _count).Average();
                        });
                    });
                });
            });
        }

        static string printAccum(double[] _accum)
        {
            string ret = string.Empty;
            for (int i = 0; i < _accum.Length; i++)
                ret += string.Format("{0}; ", _accum[i]);
            return ret;
        }

        public bool raw2phys(int _start, int _sz, int _znStart, int _znCnt)
        {
            if ((_znStart + _znCnt) * ptube.logZoneSize > ptube.Width)
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
                return false;
            }
            int[] sensorOrder = Program.settings.Current.sensors.sensors.getSensorOrder();
            int[] reversedSensors = Program.settings.Current.sensors.sensors.getReverseSensors();
            int currentSections = rtube.sections;
            //if (currentSections > (_znStart+1)*GetsectionsPerZone()+deadSectionsStart-1)
            if (currentSections > (_znStart + _znCnt) * GetsectionsPerZone() + deadSectionsStart)
            {
                fillSensorAvgValues(deadSectionsStart, currentSections - deadSectionsStart);
                int distLogSize = (int)(Program.settings.sensorsDistance / ptube.cellXSize);
                for (int zn = _znStart; zn < _znStart + _znCnt; zn++)
                {
                    for (int i = 0; i < ptube.logZoneSize; i++)
                    {
                        for (int mrow = 0; mrow < mrows; mrow++)
                        {
                            int xPos = (sensorOrder[mrow] < 2) ? 0 : distLogSize;
                            for (int row = 0; row < rows; row++)
                            {
                                //int orderedRow = (reversedSensors.Contains(mrow)) ? rows - row - 1 : row;
                                int orderedRow;
                                if (reversedSensors.Contains(sensorOrder[mrow]))
                                    orderedRow = rows - row - 1;
                                else
                                    orderedRow = row;
                                for (int mcol = 0; mcol < mcols; mcol++)
                                {
                                    for (int col = 0; col < cols; col++)
                                    {
                                        try
                                        {
                                            if (zn * ptube.logZoneSize + i < currentSections && xPos + zn * ptube.logZoneSize + i < ptube.Width)
                                            {
                                                if (_start + (zn - _znStart) * GetsectionsPerZone() + i < deadSectionsStart ||
                                                    _start + (zn - _znStart) * GetsectionsPerZone() + i > currentSections - deadSectionsEnd)
                                                {
                                                    ptube[xPos + zn * ptube.logZoneSize + i, mrow * rows + orderedRow] = 0;
                                                }
                                                else
                                                {
                                                    ptube[xPos + zn * ptube.logZoneSize + i, mrow * rows + orderedRow] = Math.Abs(this[mcol, sensorOrder[mrow], col, row,
                                                        _start + (zn - _znStart) * GetsectionsPerZone() + i] - sensorsAvgValues[mcol, sensorOrder[mrow], col, row]);
                                                }
                                            }
                                            else if (xPos + zn * ptube.logZoneSize + i < ptube.Width)
                                                ptube[xPos + zn * ptube.logZoneSize + i, mrow * rows + orderedRow] = 0;
                                        }
                                        catch (Exception ex)
                                        {
                                            #region Логирование 
                                            {
                                                string msg = string.Format("ptube[{0},{1}]=rtube[{2},{3},{4},{5},{6}] ({7}) Width={8},currentSections={9}",
                                                    xPos + zn * ptube.logZoneSize + i, mrow * rows + row,
                                                    mcol, mrow, col, row, (zn - _znStart) * GetsectionsPerZone() + i,
                                                    ex.Message, ptube.Width, currentSections);
                                                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                                                Log.add(logstr, LogRecord.LogReason.error);
                                                Debug.WriteLine(logstr, "Error");

                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                #region Логирование 
                {
                    string msg = string.Format("Мало данных: currentSections = {0}, Надо {1}", currentSections, (_znStart + 1) * GetsectionsPerZone() + deadSectionsStart - 1);
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
            }
            ptube.endWritedX = (_znStart + _znCnt) * ptube.logZoneSize;
            onDataChanged?.Invoke(null);
            return true;
        }

        public enum TubeRes { Good, Bad, Class2, Unknown }

        public TubeRes getZoneResult(int _zone,out double _maxVal)
        {
            TubeRes ret = TubeRes.Unknown;
            double maxVal=0;
            if(_zone* ptube.logZoneSize >= ptube.Width)
            {
                _maxVal = PhysTube.undefined;
                return ret;
            }
            for (int x = 0; x < ptube.logZoneSize && (_zone * ptube.logZoneSize + x < ptube.Width); x++)
            {
                for (int y = 0; y < ptube.Height; y++)
                {
                    double val = Math.Abs(ptube[_zone * ptube.logZoneSize + x, y]);
                    if (val > maxVal) maxVal = val;
                }
            }
            _maxVal = maxVal;
            //if (maxVal > ptube.ts.Border1) return TubeRes.Bad;
            //else if (maxVal > ptube.ts.Border2) return TubeRes.Class2;
            //else if (maxVal > 0) return TubeRes.Good;
            TypeSize ts = Program.settings.Current;
            if (maxVal > ts.Border1) return TubeRes.Bad;
            else if (maxVal > ts.Border2) return TubeRes.Class2;
            else if (maxVal > 0) return TubeRes.Good;
            return ret;
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

        public double[] getSectionsData(int _start,int _count)
        {
            double[] ret = new double[_count * rtube.sectionSize];
            for (int i = 0; i < _count * rtube.sectionSize; i++)
                ret[i] = rtube.data[_start * rtube.sectionSize + i];
            return ret;
        }

        #region IDataWriter implementation
        int startWriteZoneSection = 0;
        int zones = 0;
        public int Write(IEnumerable<double> _data)
        {
            //int measesPerZone = (int)((double)Program.settings.ZoneSize * Program.settings.lCardSettings.FrequencyCollect / DefaultValues.Speed/1000);
            int measesPerZone = (int)((double)Program.settings.ZoneSize * Program.mtdadcFreq / (ptube.speed*1000));
            rtube.Write(_data);
            if (rawDataSize / measesPerZone > zones)
            {
                #region Логирование 
                {
                    string msg = string.Format("Достигнута граница зоны {0} - {1}({2})", zones, sections, sections-startWriteZoneSection );
                    string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr, "Message");
                }
                #endregion
                raw2phys(startWriteZoneSection, sections-startWriteZoneSection, zones, 1);
                //Здесь надо проанализировать зону и выдать сигналы для результата
                startWriteZoneSection = sections;
                zones++;
            }
            onDataChanged?.Invoke(_data);
            return _data.Count();
        }

        public bool Start()
        {
            reset();
            return true;
        }

        public bool Stop()
        {
            return true;
        }
        #endregion IDataWriter implementation
        public void reset(double _val = PhysTube.undefined)
        {
            rtube.reset();
            ptube.reset(_val);
            Zones.Clear();
        }
    }
}
