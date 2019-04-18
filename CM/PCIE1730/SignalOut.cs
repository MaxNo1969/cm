namespace CM
{
    /// <summary>
    /// Выходной сигнал
    /// </summary>
    public class SignalOut
    {
        private Signal signal;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_signal">Сигнал</param>
        public SignalOut(Signal _signal)
        {
            signal=_signal;
        }
        /// <summary>
        /// Значение
        /// </summary>
        public bool Val
        {
            get
            {
                return (signal.Val);
            }
            set
            {
                signal.Val = value;
            }
        }
        /// <summary>
        /// Наименование
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
    }
}
