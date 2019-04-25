using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CM
{
    /// <summary>
    /// Класс для хранения сырых данных по трубе.
    /// </summary>
    [Serializable]
    public class RawTube:IDataWriter<double>
    {
        /// <summary>
        /// Типоразмер
        /// </summary>
        public TypeSize ts;
        /// <summary>
        /// Сырые данные
        /// </summary>
        public List<double> data;

        public readonly int len;

        public RawTube(TypeSize _ts,int _len)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            ts = _ts;
            len = _len;
            data = new List<double>();
        }
        
        ///<summary>
        ///Вспомогательные параметры
        ///</summary>
        public int rawDataSize { get { return data.Count; } }
        public int sectionSize { get { return ts.sensors.sectionSize; } }
        public int sections { get { return data.Count / sectionSize; } }
        public int mcols { get { return ts.sensors.sensors.dim.cols; } }
        public int mrows { get { return ts.sensors.sensors.dim.rows; } }
        public int cols { get { return ts.sensors.hallSensors.dim.cols; } }
        public int rows { get { return ts.sensors.hallSensors.dim.rows; } }

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
                    val = data[ind + _i * sectionSize];
                }
                catch
                {
                    val = double.NaN;
                    //throw (new IndexOutOfRangeException());
                }
                return val;
            }
            set
            {
                int ind = _mc * mrows * cols * rows + _mr * cols * rows + _r * cols + _c;
                while (ind + _i * sectionSize < data.Count) data.Add(0);
                data[ind + _i * sectionSize]=value;
            }
        }
        #region Сериализация
        /// <summary>
        /// Сериализация в файл для сохранения данных по трубе 
        /// </summary>
        /// <param name="_fileName">Имя файла</param>
        /// <returns>true - если труба успешно сохранена</returns>
        public static bool save(RawTube _tube, string _fileName)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", "RawTube", System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(_fileName, FileMode.Create))
                {
                    formatter.Serialize(fs, _tube);
                    #region Логирование 
                    {
                        string msg = string.Format("Данные сохранены. Размер: {0}",fs.Length);
                        string logstr = string.Format("{0}: {1}: {2}", "RawTube", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        Log.add(logstr, LogRecord.LogReason.info);
                        Debug.WriteLine(logstr, "Message");
                    }
                    #endregion
                    return true;
                }
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
        /// <summary>
        /// Загрузка сохраненной трубы из файла
        /// </summary>
        /// <param name="_fileName">Имя файла</param>
        /// <returns>true - если труба успешно загружена</returns>
        public static bool load(RawTube _raw, string _fileName)
        {
            #region Логирование 
            {
                string logstr = string.Format("{0}: {1}", "RawTube", System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr, "Message");
            }
            #endregion
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(_fileName, FileMode.Open))
                {
                    _raw = (RawTube)formatter.Deserialize(fs);
                    if (_raw != null)
                    {
                        #region Логирование 
                        {
                            string msg = string.Format("Данные загружены. Размер: {0}", fs.Length);
                            string logstr = string.Format("{0}: {1}: {2}", "RawTube", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.error);
                            Debug.WriteLine(logstr, "Error");
                        }
                        #endregion
                        return true;
                    }
                    else
                    {
                        #region Логирование 
                        {
                            string msg = string.Format("Ошибка загрузки данных . Файл: {0}", _fileName);
                            string logstr = string.Format("{0}: {1}: {2}", "RawTube", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                            Log.add(logstr, LogRecord.LogReason.error);
                            Debug.WriteLine(logstr, "Error");
                        }
                        #endregion
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                #region Логирование 
                {
                    string msg = string.Format("{0}", ex.Message );
                    string logstr = string.Format("{0}: {1}: {2}", "RawTube", System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    Log.add(logstr, LogRecord.LogReason.error);
                    Debug.WriteLine(logstr, "Error");

                }
                #endregion
                return false;
            }
        }
        #endregion Сериализация

        #region IDataWriter implementation
        public int Write(IEnumerable<double> _data)
        {
            data.AddRange(_data);
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
            data.Clear();
        }
    }
}
