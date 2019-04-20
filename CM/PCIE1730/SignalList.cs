using Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CM
{
    /// <summary>
    /// Делегат при установке сигнала
    /// </summary>
    public delegate void onSet();
    /// <summary>
    /// Делегат при ожидании сигнала
    /// </summary>
    /// <param name="_value">Значение</param>
    /// <param name="_signal">Сигнал</param>
    /// <param name="_timeout">Таймаут</param>
    /// <returns></returns>
    public delegate string onWait(bool _value, Signal _signal, TimeSpan _timeout);
    /// <summary>
    /// Список сигналов 
    /// </summary>
    public class SignalList : IDisposable
    {
        private bool disposed = false;
        /// <summary>
        /// Произведена ли очистка
        /// </summary>
        /// <returns></returns>
        public bool Disposed()
        {
            return (disposed);
        }
        /// <summary>
        /// Очистка занятых ресурсов
        /// </summary>
        public void Dispose()
        {
            string s = GetType().Name + ": " + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Отключаемся от PCIE1730";
            Log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);
            terminate = true;
            /*
            while (!th.IsAlive)
            {
                Thread.Sleep(1);
            }
            */
            if(th.IsAlive)th.Join(100);
            a1730.Dispose();
            disposed = true;
        }

        private readonly ThreadStart ts;
        private Thread th;
        private volatile bool terminate = false;

        /// <summary>
        /// Плата цифрового ввода/вывода (PCIE-1730)
        /// </summary>
        public PCIE_1730 a1730;

        private List<Signal> M = new List<Signal>();
        private List<Latch> L = new List<Latch>();
        private readonly object SignalsLock;
        private TimeSpan timeout;
        /// <summary>
        /// Список входных сигналов
        /// </summary>
        protected List<SignalIn> MIn;
        /// <summary>
        /// Список выходных сигналов
        /// </summary>
        protected List<SignalOut> MOut;


        /// <summary>
        /// Установить сигнал
        /// </summary>
        /// <param name="_sg">Сигнал</param>
        /// <param name="_val">Значение</param>
        public void set(SignalIn _sg, bool _val)
        {
            a1730.SetBit(_sg.Position, _val,true);
        }

        /// <summary>
        /// Прочитать сигнал
        /// </summary>
        /// <param name="_sg">Сигнал</param>
        /// <returns>Значение</returns>
        public bool get(SignalIn _sg)
        {
            return a1730.GetBit(_sg.Position,true);
        }

        /// <summary>
        /// Установить сигнал
        /// </summary>
        /// <param name="_sg">Сигнал</param>
        /// <param name="_val">Значение</param>
        public void set(SignalOut _sg, bool _val)
        {
            a1730.SetBit(_sg.Position, _val, false);
        }

        /// <summary>
        /// Прочитать сигнал
        /// </summary>
        /// <param name="_sg">Сигнал</param>
        /// <returns>Значение</returns>
        public bool get(SignalOut _sg)
        {
            return a1730.GetBit(_sg.Position, false);
        }
        /// <summary>
        /// Записать текущие значения сигналов в плату PCIE1730
        /// </summary>
        protected void WriteSignals()
        {
            byte[] values_out = a1730.ReadOut();
            for (int i = 0; i < M.Count(); i++)
            {
                Signal p = M[i];
                if (p.Input)
                    continue;
                if (p.Verbal)
                {
                    bool v = a1730.GetBit(p.position,false);
                    if (p.val != v)
                    {
                        Log.add(p.Name + " выставляем в " + (p.val ? "true" : "false"));
                    }
                }
                a1730.SetBit(p.position, p.val,false);
            }
            a1730.Write(values_out);
        }
        /// <summary>
        /// Ожидание сигнала
        /// </summary>
        /// <param name="_val">Значение</param>
        /// <param name="_signal">Сигнал</param>
        /// <param name="_timeout">Таймаут ожидания(мс)</param>
        /// <returns></returns>
        private string OnWait(bool _val, Signal _signal, TimeSpan _timeout)
        {
            Latch lp;
            lock (SignalsLock)
            {
                lp = new Latch(_val, _signal);
                L.Add(lp);
            }
            bool Signaled = lp.ev.WaitOne(_timeout);
            lp.ev.Reset();
            string ret;
            lock (SignalsLock)
            {
                if (Signaled)
                    ret = lp.reason;
                else
                    ret = "Не дождались";
                L.Remove(lp);
            }
            return (ret);
        }
        /// <summary>
        /// Конструктор (Напрямую нигде не вызывается)
        /// </summary>
        protected SignalList()
        {
            SignalsLock = new object();
            MIn = new List<SignalIn>();
            MOut = new List<SignalOut>();
            PCIE1730Settings settings = Program.settings.pCIE1730Settings;

            try
            {
                if (Program.cmdLineArgs.ContainsKey("NOA1730"))
                    a1730 = new PCIE_1730_virtual(settings.ToString(), settings.PortcountIn, settings.PortcountOut);
                else
                    a1730 = new PCIE_1730_real(settings.ToString(), settings.PortcountIn, settings.PortcountOut);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            string s = "SignalList: Constructor";
            Log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);
            timeout = new TimeSpan(0,0,0,0,settings.SignalListTimeout);
            s = string.Format("SignalListTimeout={0}", timeout);
            Log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);
            //Читаем сигналы из AppPars
            int cnt = settings.Signals.Count;
            s = string.Format("Будем читать {0} сигналов.", cnt);
            Log.add(s, LogRecord.LogReason.info);
            Debug.WriteLine(s);
            for (int i = 0; i < cnt; i++)
            {
                Signal sg = new Signal(settings.Signals[i].Name, WriteSignals, OnWait, SignalsLock)
                {
                    sgSet = settings.Signals[i]
                };
                M.Add(sg);
                s = string.Format("{0} {1}-{2}(Digital={3},EOn={4},EOff={5},Timeout={6},No_reset={7},Verbal={8})",
                    sg.position,sg.Name,sg.Hint,sg.Digital,sg.EOn,sg.EOn,sg.Timeout,sg.NoReset,sg.Verbal);
                Log.add(s, LogRecord.LogReason.info);
                Debug.WriteLine(s);
            }

            ts = new ThreadStart(Run);
            th = new Thread(ts)
            {
                Name = "SignalThread"
            };
        }
        /// <summary>
        /// Запуск потока обработки сигналов
        /// </summary>
        protected void Start()
        {
            th.Start();
        }
        private void ReadSignals()
        {
            if (disposed)
                return;
            byte[] vv = a1730.Read();
            DateTime dt = DateTime.Now;
            for (int i = 0; i < M.Count(); i++)
            {
                Signal p = M[i];
                if (!p.Input)
                    continue;
                bool v = a1730.GetBit(p.position,true);
                lock (SignalsLock)
                {
                    p.front = p.val != v;
                    if (p.front)
                    {
                        p.val_prev = p.val;
                        p.val = v;
                        p.last_changed = dt;

                        if (p.Verbal)
                            Log.add(p.Name + " стал " + (v ? "true" : "false"));
                    }
                }
            }
        }
        private void LatchSignals()
        {
            lock (SignalsLock)
            {
                for (int i = 0; i < L.Count(); i++)
                {
                    Latch lp = L[i];
                    if (!lp.terminated)
                    {
                        if (lp.signal.val == lp.val)
                        {
                            lp.terminated = true;
                            lp.reason = "Ok";
                        }
                        if (lp.terminated)
                            lp.ev.Set();
                    }
                }
            }
        }
        /// <summary>
        /// Виртуальная функция реакции
        /// </summary>
        protected virtual void Reaction()
        {
        }
        /// <summary>
        /// Виртуальная функция сигнал "Тревога"
        /// </summary>
        protected virtual void CheckAlarm()
        {
        }
        private void CheckAlarmL()
        {
            lock (SignalsLock)
            {
                for (int i = 0; i < M.Count(); i++)
                {
                    Signal p = M[i];
                    if (!p.Input)
                        continue;
                    if (!p.IsAlarm)
                        continue;
                    if (p.AlarmVal != p.val)
                    {
                        string msg;
                        if (p.val)
                        {
                            if (p.EOn.Length != 0)
                                msg = p.EOn;
                            else
                                msg = "Авария: " + p.Name + " true";
                        }
                        else
                        {
                            if (p.EOff.Length != 0)
                                msg = p.EOff;
                            else
                                msg = "Авария: " + p.Name + " false";
                        }
                        LatchTerminate0(msg);
                    }
                }
            }
        }
        private void Run()
        {
            while (!terminate)
            {
                ReadSignals();
                LatchSignals();
                CheckAlarmL();
                lock (SignalsLock)
                {
                    Reaction();
                    CheckAlarm();
                }
            }
        }
        /// <summary>
        /// LatchTerminate0
        /// </summary>
        /// <param name="_msg"></param>
        protected void LatchTerminate0(string _msg)
        {
            for (int i = 0; i < L.Count(); i++)
            {
                Latch lp = L[i];
                if (!lp.terminated)
                {
                    lp.terminated = true;
                    lp.reason = _msg;
                    lp.ev.Set();
                }
            }
        }
        private void LatchTerminate(string _msg)
        {
            lock (SignalsLock)
            {
                for (int i = 0; i < L.Count(); i++)
                {
                    Latch lp = L[i];
                    if (!lp.terminated)
                    {
                        lp.terminated = true;
                        lp.reason = _msg;
                        lp.ev.Set();
                    }
                }
            }
        }
        /// <summary>
        /// Найти сигнал по имени и направлению
        /// </summary>
        /// <param name="_name">Имя</param>
        /// <param name="_input">входящий/исходящий</param>
        /// <returns>Синал</returns>
        protected Signal Find(string _name, bool _input)
        {
            for (int i = 0; i < M.Count(); i++)
            {
                Signal p = M[i];
                if (p.Name == _name)
                {
                    if (p.Input != _input)
                        MessageBox.Show("SignalList: Find: Сигнал " + _name + " не " + (_input ? "входящий" : "исходящий"));
                    return (p);
                }
            }
            //MessageBox.Show("SignalList: Find: Сигнал " + _name + " не найден");
            throw new KeyNotFoundException(string.Format("SignalList: Find: Сигнал {0} не найден", _name));
            //return (null);
        }
        /// <summary>
        /// Количество входных сигналов
        /// </summary>
        public int CountIn { get { return (MIn.Count); } }
        /// <summary>
        /// Количество выходных сигналов
        /// </summary>
        public int CountOut { get { return (MOut.Count); } }
        /// <summary>
        /// Получить выходной сигнал по индексу
        /// </summary>
        /// <param name="_index">Индекс</param>
        /// <returns>Сигнал</returns>
        public SignalIn GetSignalIn(int _index)
        {
            return (MIn[_index]);
        }
        /// <summary>
        /// Получить выходной сигнал по индексу
        /// </summary>
        /// <param name="_index">Индекс</param>
        /// <returns>Сигнал</returns>
        public SignalOut GetSignalOut(int _index)
        {
            return (MOut[_index]);
        }
    }
}
