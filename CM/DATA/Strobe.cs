using System;

namespace CM
{
    /// <summary>
    /// Данные по стробу 
    /// </summary>
    [Serializable]
    public class Zone
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
        /// Результат по зоне
        /// </summary>
        public Tube.TubeRes res;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_bound">Номер сечения строба</param>
        public Zone(int _bound, Tube.TubeRes _res)
        {
            dt = DateTime.Now;
            bound = _bound;
            res = _res;
        }
    }
}