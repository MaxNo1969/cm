using System;
using System.Collections.Generic;

namespace Protocol
{
    /// <summary>
    /// Запись для логирования
    /// </summary>
    public class LogRecord
    {
        /// <summary>
        /// Момент записи
        /// </summary>
        public DateTime dt { get; private set; }
        /// <summary>
        /// Причина
        /// </summary>
        public enum LogReason
        {
            /// <summary>
            /// Отладка
            /// </summary>
            debug = 0,
            /// <summary>
            /// Информация
            /// </summary>
            info = 1,
            /// <summary>
            /// Предупреждение
            /// </summary>
            warning = 2,
            /// <summary>
            /// Ошибка
            /// </summary>
            error = 3
        };
        /// <summary>
        /// Причина логирования
        /// </summary>
        public LogReason reason { get; private set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string text { get; private set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_reason"></param>
        public LogRecord(string _text = "", LogReason _reason = LogReason.info)
        {
            dt = DateTime.Now;
            text = _text;
            reason = _reason;
        }
    }
    /// <summary>
    /// Класс для логирования
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Запись в лог
        /// </summary>
        public delegate void OnLogChanged();
        /// <summary>
        /// При записи в лог
        /// </summary>
        public static OnLogChanged onLogChanged = null;

        static Queue<LogRecord> p = new Queue<LogRecord>();
        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="s">сообщение</param>
        /// <param name="_reason">причина логирования</param>
        public static void add(string s, LogRecord.LogReason _reason = LogRecord.LogReason.info) 
        {
            //Проверим переполнение
            if (p.Count >= int.MaxValue)
                p.Dequeue();
            p.Enqueue(new LogRecord(s, _reason));
            onLogChanged?.Invoke();
        }
        /// <summary>
        /// Получить запись из очереди логироания
        /// </summary>
        /// <returns></returns>
        public static LogRecord get() 
        {
            if (p.Count > 0) return p.Dequeue();
            else return null;
        }
        /// <summary>
        /// Размер лога
        /// </summary>
        /// <returns></returns>
        public static int size() { return p.Count; }
    }
}
