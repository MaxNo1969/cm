using Protocol;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CM
{
    public delegate void OnDataRead(IEnumerable<double> _data);
    /// <summary>
    /// Абстрактный класс для представления АЦП LCard502 (Наследуются LCardReal и LCardVirtual(эмлятор))
    /// </summary>
    public abstract class LCard: IDataReader<double>
    {

        //public MTADC mtdadc;
        /// <summary>
        /// Размер буфера для приема данных по умолчанию
        /// Речь шла о 1500 датчиках по умолчанию
        /// </summary>
        //public const int LCardBufferSize = 1024;
        /// <summary>
        /// Размер буфера для приема данных с АЦП. Будем брать как размер одной матрицы
        /// </summary>
        protected uint raw_size;
        /// <summary>
        /// Буфер под сырые данные с АЦП
        /// </summary>
	    protected uint[] rawi;
        /// <summary>
        /// Буфер под преобразованные данные с АЦП. С этим уже будем работать
        /// </summary>
	    protected double[] raw;
        /// <summary>
        /// Сбор данных стартовал
        /// </summary>
	    protected bool IsStarted;
        /// <summary>
        /// Сбор данных стартовал
        /// </summary>
        public bool started { get { return IsStarted; } }
        /// <summary>
        /// ТаймАут для сбора (с каким периодом будем скидывать данные из ацп в большой буфер,мс)
        /// </summary>
	    protected uint RECV_TOUT;
        /// <summary>
        /// Количество основных подключенных датчиков (скорее в сего для нашего случая - 1)
        /// </summary>
	    protected int numMainSensors = 1;
        /// <summary>
        /// Количество дополнительных подключенных датчиков  (скорее в сего для нашего случая - 0)
        /// </summary>
	    protected int numOtherSensors = 0;

        /// <summary>
        /// Загрузка основных настроек
        /// </summary>
        protected virtual void LoadMainSettings()
        {
            numMainSensors = settings.Channels.Count;
        }
        /// <summary>
        /// Заагрузка дополнительных настроек
        /// </summary>
        protected virtual void LoadOtherSettings()
        {
            numOtherSensors = settings.Others.Count;
        }

        /// <summary>
        /// Настройки LCard
        /// </summary>
        protected LCardSettings settings;

        public OnDataRead onDataRead = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_bufSize">Размер буфера считываемого за раз из буфера АЦП</param>
        public LCard(LCardSettings _params)
        {
            settings = _params;
            raw_size = (uint)settings.BufSize;
            rawi = new uint[raw_size];
            raw = new double[raw_size];
            RECV_TOUT = (uint)settings.RECV_TOUT;
            IsStarted = false;
            #region Логирование
            {
                string msg = string.Format("Размер буфера: {0}, Таймаут ожидания пакета: {1}", raw_size, RECV_TOUT);
                string logstr = string.Format("{0}: {1}: {2}", "LCard", "Конструктор", msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr);
            }
            #endregion
        }
        /// <summary>
        /// Начать сбор данных с АЦП
        /// </summary>
        /// <returns>true - сбор данных стартовал успешно</returns>
        public abstract bool Start();
        /// <summary>
        /// Остановить сбор данных с АЦП
        /// </summary>
        /// <returns>true - Сбор данных успешно остановлен</returns>
	    public abstract bool Stop();
        /// <summary>
        /// Синхронное чтение данных с АЦП
        /// </summary>
        /// <returns>Массив прочитанных данных с АЦП</returns>
	    public abstract double[] Read();
        /// <summary>
        /// Асинхронное чтение данных 
        /// </summary>
        /// <param name="_ch">Номер канала</param>
        /// <returns>Преобразованное значение очередного отсчета</returns>
	    public abstract double GetValue(int _ch);

        /// <summary>
        /// Для отладки. Чтение _cnt байт
        /// </summary>
        /// <param name="_cnt">Запрашиваемый размер данных</param>
        /// <returns>Массив данных с АЦП (из RawTube)</returns>
        public abstract double[] ReadPacket(int _cnt);
        /// <summary>
	    /// Загрузить настройки L502
	    /// </summary>
	    /// <param name="_parms">Настройки L502</param>
        public void LoadSettings(LCardSettings _parms)
        {
            settings = _parms;
        }
    }
}
