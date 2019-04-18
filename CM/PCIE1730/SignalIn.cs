using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CM
{
    /// <summary>
    /// Входящий сигнал
    /// </summary>
    public class SignalIn
    {
        private Signal signal;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_signal">Сигнал</param>
        public SignalIn(Signal _signal)
        {
            signal = _signal;
        }
        /// <summary>
        /// Значение
        /// </summary>
        public bool Val
        {
            get => signal.Val;
            set => signal.val = value;
        }
        /// <summary>
        /// Изменялся ли сигнал за _period
        /// </summary>
        /// <param name="_value">Значение</param>
        /// <param name="_period">Период(мс)</param>
        /// <returns></returns>
        public bool WasConst(bool _value, int _period)
        {
            return (signal.WasConst(_value, _period));
        }
        /// <summary>
        /// Ожидание сигнала 
        /// </summary>
        /// <param name="_value">Значение</param>
        /// <param name="_tm">Время ожидания(мс)</param>
        /// <returns></returns>
        public string Wait(bool _value, int _tm)
        {
            return (signal.Wait(_value, _tm));
        }
        //        public bool Front { get { return (signal.Front); } }
        /// <summary>
        /// Название 
        /// </summary>
        public string Name { get { return (signal.Name); } }
        /// <summary>
        /// Битовая позиция
        /// </summary>
        public int Position { get { return (signal.position); } }
        /// <summary>
        /// Подсказка
        /// </summary>
        public string Hint { get { return (signal.Hint); } }
        /// <summary>
        /// Выставить флаг "Тревога"
        /// </summary>
        public void SetAlarm()
        {
            signal.Alarm(true);
        }
        /// <summary>
        /// Снять флаг "Тревога"
        /// </summary>
        public void UnSetAlarm()
        {
            signal.Alarm(false);
        }
    }
}