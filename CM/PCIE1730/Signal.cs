using System;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Сигнал платы цифрового ввода/вывода
    /// </summary>
    public class Signal
    {
        private readonly onSet OnSet;
        private readonly onWait OnWait;
        private readonly Object SignalsLock;

        /// <summary>
        /// Параметры сигнала
        /// </summary>
        public SignalSettings sgSet;
        /// <summary>
        /// Название сигнала
        /// </summary>
        public string Name { get { return sgSet.Name; }}
        /// <summary>
        /// Номер платы вводв/вывода
        /// </summary>
        //public int board { get { return sgSet.board; }}
        ///// <summary>
        ///// входной/выходной
        ///// </summary>
        public bool Input { get { return sgSet.Input; }}
        /// <summary>
        /// Цифровой?
        /// </summary>
        public bool Digital { get { return sgSet.Digital; }}
        /// <summary>
        /// Битовое смещение
        /// </summary>
        public int position { get { return sgSet.Position; }}
        /// <summary>
        /// Описание
        /// </summary>
        public string Hint { get { return sgSet.Hint; }}
        /// <summary>
        /// ?
        /// </summary>
        public string EOn { get { return sgSet.EOn; }}
        /// <summary>
        /// ?
        /// </summary>
        public string EOff { get { return sgSet.EOff; }}
        /// <summary>
        /// Время ождания
        /// </summary>
        public int Timeout { get { return sgSet.Timeout; }}
        /// <summary>
        /// Признак сброса сигнала
        /// </summary>
        public bool NoReset { get { return sgSet.NoReset; }}
        /// <summary>
        /// Вывод отладочной информации в лог о переключении
        /// </summary>
        public bool Verbal { get { return sgSet.Verbal; }}
        /// <summary>
        /// Значение сигнала
        /// </summary>
        public bool val;
        /// <summary>
        /// Предыдущее значение сигнала
        /// </summary>
        public bool val_prev;
        /// <summary>
        /// Время последнего переключения сигнала
        /// </summary>
        public DateTime last_changed;
        /// <summary>
        /// Фронт?
        /// </summary>
        public bool front;
        /// <summary>
        /// ?
        /// </summary>
        public bool IsAlarm;
        /// <summary>
        /// ?
        /// </summary>
        public bool AlarmVal;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_name">Название</param>
        /// <param name="_OnSet">Обработчик при выставлении</param>
        /// <param name="_OnWait">Обработчик при ожидании</param>
        /// <param name="_SignalsLock">Блокировка</param>
        public Signal(string _name, onSet _OnSet, onWait _OnWait, Object _SignalsLock)
        {
            sgSet = new SignalSettings
            {
                Name = _name
            };
            OnSet = _OnSet;
            OnWait = _OnWait;
            SignalsLock = _SignalsLock;
            val = false;
            val_prev = false;
            last_changed = DateTime.Now;
            front = false;
            IsAlarm = false;
            AlarmVal = false;
        }
        private bool WasConst0(bool _val, int _period)
        {
            if (last_changed <= DateTime.Now - new TimeSpan(0, 0, 0, 0, _period))
            {
                if (val == _val)
                    return (true);
            }
            return (false);
        }

        /// <summary>
        /// Изменялся ли сигнал за _period
        /// </summary>
        /// <param name="_val">Значение</param>
        /// <param name="_period">Период(мс)</param>
        /// <returns></returns>
        public bool WasConst(bool _val, int _period)
        {
            bool ret;
            DateTime last_changed1;
            lock (SignalsLock)
            {
                ret = val;
                last_changed1 = last_changed;
            }
            if (last_changed1 <= DateTime.Now - new TimeSpan(0, 0, 0, 0, _period))
            {
                if (ret == _val)
                    return (true);
            }
            return (false);
        }
        private void Set0(bool _val)
        {
            if (Input)
                MessageBox.Show("Signal::Set: попытка выставить входной сигнал: " + Name);
            if (val_prev != val)
            {
                last_changed = DateTime.Now;
                val_prev = val;
                val = _val;
            }
            OnSet();
        }

        /// <summary>
        /// Значение
        /// </summary>
        public bool Val
        {
            get
            {
                bool ret;
                lock (SignalsLock)
                {
                    ret = val;
                }
                return (ret);
            }
            set
            {
                if (Input)
                    MessageBox.Show("Signal::Set: попытка выставить входной сигнал: " + Name);
                lock (SignalsLock)
                {
                    //if (val_prev != value)
                    {
                        last_changed = DateTime.Now;
                        val_prev = val;
                        val = value;
                    }
                    OnSet();
                }
            }
        }
        /// <summary>
        /// Ожидать сигнал _tm мс 
        /// </summary>
        /// <param name="_val">Значение</param>
        /// <param name="_tm">Таймаут(мс)</param>
        /// <returns></returns>
        public string Wait(bool _val, int _tm)
        {
            if (!Input)
            {
                MessageBox.Show("Signal::Wait: попытка подождать выходной сигнал: " + Name);
                return null;
            }
            if (Val == _val)
                return ("Ok");
            return (OnWait(_val, this, new TimeSpan(0, 0, 0, 0, _tm)));
        }
        /// <summary>
        /// Фронт сигнала
        /// </summary>
        public bool Front
        {
            get
            {
                bool v;
                lock (SignalsLock)
                {
                    v = front;
                }
                return (v);
            }
        }
        /// <summary>
        /// Тревога?
        /// </summary>
        /// <param name="_on"></param>
        public void Alarm(bool _on)
        {
            lock (SignalsLock)
            {
                if (_on)
                {
                    IsAlarm = true;
                    AlarmVal = val;
                }
                else
                {
                    IsAlarm = false;
                    AlarmVal = false;
                }
            }
        }
    }
}
