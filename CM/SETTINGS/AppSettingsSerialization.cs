using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CM
{
    public static class AppSettingsSerialization
    {
        private const string className = "AppSettingsSerialization";
        /// <summary>
        /// Загрузка параметров из файла
        /// </summary>
        /// <returns>Указатель на параметры</returns>
        public static AppSettings load(string _fName)
        {
            AppSettings settings;
            try
            {
                #region Логирование
                {
                    string logstr = string.Format("{0}: {1}", className, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr);
                }
                #endregion 

                // передаем в конструктор тип класса
                XmlSerializer formatter = new XmlSerializer(typeof(AppSettings));            // десериализация
                using (FileStream fs = new FileStream(_fName, FileMode.Open))
                {
                    settings = (AppSettings)formatter.Deserialize(fs);
                }
            }
            catch
            {
                //Обработка первого запуска - файл настроек ещё не записан
                #region Логирование
                {
                    string msg = "Первый запуск - файл настроек ещё не записан";
                    string logstr = string.Format("{0}: {1}: {2}", className, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                    log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr);
                }
                #endregion 
                settings = new AppSettings()
                {
                    changed = true,
                };
                save(settings, _fName);
            }
            return settings;
        }
        /// <summary>
        /// Запись параметров в файл
        /// </summary>
        /// <param name="_s">Параметры для записи</param>
        public static void save(AppSettings _settings,string _fName)
        {
            if (_settings.changed)
            {
                #region Логирование
                {
                    string logstr = string.Format("{0}: {1}", className, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    log.add(logstr, LogRecord.LogReason.info);
                    Debug.WriteLine(logstr);
                }
                #endregion 

                // передаем в конструктор тип класса
                try
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(AppSettings));            // десериализация
                    using (FileStream fs = new FileStream(_fName, FileMode.Create))
                    {
                        formatter.Serialize(fs, _settings);
                        _settings.changed = false;
                    }
                }
                catch (Exception ex)
                {
                    #region Логирование
                    {
                        string msg = ex.Message;
                        string logstr = string.Format("{0}: {1}: {2}", className, System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
                        log.add(logstr, LogRecord.LogReason.error);
                        Debug.WriteLine(logstr);
                    }
                    #endregion
                }
            }
            return;
        }
    }
}
