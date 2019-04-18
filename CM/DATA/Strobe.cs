using System;

namespace CM
{
    /// <summary>
    /// Данные по стробу 
    /// </summary>
    [Serializable]
    public class Strobe
    {
        /// <summary>
        /// Время строба
        /// </summary>
        public DateTime dt;
        /// <summary>
        /// Номер сечения строба
        /// </summary>
        public int bound;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_bound">Номер сечения строба</param>
        public Strobe(int _bound)
        {
            dt = DateTime.Now;
            bound = _bound;
        }
    }
}