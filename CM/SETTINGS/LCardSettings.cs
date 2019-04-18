using Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using x502api;

namespace CM
{
    /// <summary>
    ///  Параметры L502
    /// </summary>
    [DisplayName("LCard502"),Browsable(true),Description("Настройка платы АЦП")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class LCardSettings:ParBase
    {
        /// <summary>
        /// Номер устройства. У нас сейчас одна плата поэтому всегда будет 0
        /// Но на всякий случай читать будем из настроек
        /// </summary>
        [DisplayName("1.Номер устройства"), Description("Номер устройства")]
        public int DevNum { get; set; }
        /// <summary>
        /// Номер устройства. У нас сейчас одна плата поэтому всегда будет 0
        /// Но на всякий случай читать будем из настроек
        /// </summary>
        [DisplayName("2.Размер буфера"), Description("Количество отсчетов считываемых за один раз")]
        public int BufSize { get; set; }
        /// <summary>
        /// <list type="Режим синхронизации">
        ///  0 Внутренний сигнал
        ///  1 От внешнего мастера по разъему синхронизации
        ///  2 По фронту сигнала DI_SYN1
        ///  3 По фронту сигнала DI_SYN2
        ///  6 По спаду сигнала DI_SYN1
        ///  7 По спаду сигнала DI_SYN2
        ///  </list>
        /// </summary>
        [DisplayName("3.Режим Синхронизации"), Description("Настройка режима синхронизации")]
        public X502.Sync SyncMode { get; set; }
        /// <summary>
        /// Источник запуска синхронного ввода/вывода см. режимы syncMode
        /// </summary>
        [DisplayName("4.Источник запуска"), Description("Источник запуска синхронного ввода/вывода")]
        public X502.Sync SyncStartMode { get; set; }
        /// <summary>
        /// Частота сбора (Гц.) 
        /// </summary>
        [DisplayName("5.Частота сбора (Гц.)"), Description("Частота сбора данных")]
        public double FrequencyCollect { get; set; }
        /// <summary>
        /// Частота на канал (Гц.) 
        /// </summary>
        [DisplayName("6.Частота на канал (Гц.)"), Description("Частота на канал")]
        public double FrequencyPerChannel { get; set; }
        /// <summary>
        /// ТаймАут для сбора (с каким периодом будем скидывать данные из ацп в большой буфер,мс)
        /// </summary>
        [DisplayName("7.ТаймАут для сбора"), Description("ТаймАут для сбора (мс)"), Category("1.Настройка модуля")]
        public int RECV_TOUT {get; set;}

        /// <summary>
        /// Возвращает кол-во используемых каналов
        /// </summary>
        /// <returns>кол-во используемых каналов</returns>
	    int getCountChannels() 
	    {
		    return Channels.Count + Others.Count;
	    }

        /// <summary>
        /// Индивидуальные настройки для каждого канала. Основные каналы 
        /// </summary>
        [DisplayName("1.Основные каналы"), Description("Индивидуальные настройки для каждого канала")]
        [TypeConverter(typeof(CollectionTypeConverter))]
        public L_LCardChannels Channels { get; set; }
        /// <summary>
        /// Индивидуальные настройки для каждого канала. Дополнительные каналы 
        /// </summary>
        [DisplayName("2.Дополнительные Каналы"), Description("Индивидуальные настройки для каждого канала")]
        [TypeConverter(typeof(CollectionTypeConverter))]
        public L_LCardChannels Others { get; set; }

        /// <summary>
        /// Количество основных каналов
        /// </summary>
        [DisplayName("Основные количество"), Browsable(false), Description("Количество основных каналов")]
        public int NumChannels { get { return Channels.Count; } }
        /// <summary>
        /// Количество дополнительных каналов
        /// </summary>
        [DisplayName("Дополнительные количество"), Browsable(false), Description("Количество дополнительных каналов")]
        public int NumOthers { get { return Others.Count; } }
        /// <summary>
        /// Частота по умолчанию
        /// </summary>
        private const int defaultFrequency = 88888;
        private const X502.Sync defaultSyncMode = X502.Sync.DI_SYN2_RISE;
        private const int defaultRecvOut = 100;
        /// <summary>
        /// Конструктор 
        /// пока берем настройки из UranPars
        /// </summary>
	    public LCardSettings()
	    {
            #region Логирование
            {
                string msg = "Конструктор";
                string logstr = string.Format("{0}: {1}: {2}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name,msg);
                Log.add(logstr, LogRecord.LogReason.info);
                Debug.WriteLine(logstr);
            }
            #endregion Логирование
        }
        /// <summary>
        /// Преобразование в строку для вывода в PropertyGrid
        /// </summary>
        /// <returns>строка для вывода в PropertyGrid</returns>
        public override string ToString()
        {
            return "LCard502";
        }
    }
}
