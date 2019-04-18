using System.Collections.Generic;

namespace CM
{
    public interface IDataReader<T>
    {
        /// <summary>
        /// Начать сбор данных с АЦП
        /// </summary>
        /// <returns>true - сбор данных стартовал успешно</returns>
        bool Start();
        /// <summary>
        /// Остановить сбор данных с АЦП
        /// </summary>
        /// <returns>true - Сбор данных успешно остановлен</returns>
	    bool Stop();
        /// <summary>
        /// Синхронное чтение данных с АЦП
        /// </summary>
        /// <returns>Массив прочитанных данных с АЦП</returns>
	    T[] Read();
    }
}
