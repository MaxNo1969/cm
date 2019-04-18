using Protocol;
using System.Diagnostics;

namespace CM
{
    /// <summary>
    /// Плата цифрового ввода/вывода (PCIE1730 или эмулятор)
    /// </summary>
    public abstract class PCIE_1730
    {
        /// <summary>
        /// Очищена?
        /// </summary>
        protected bool disposed = false;
        /// <summary>
        /// Очищена?
        /// </summary>
        /// <returns></returns>
        public bool Disposed()
        {
            return disposed;
        }
        /// <summary>
        /// Очистить
        /// </summary>
        public virtual void Dispose()
        {
            disposed = true;
        }
        /// <summary>
        /// Имя платы ввода/вывода для инициализации
        /// </summary>
        protected string name;
        /// <summary>
        /// Имя платы вводв/вывода
        /// </summary>
        /// <returns></returns>
        public string Name()
        {
            return (name);
        }
        /// <summary>
        /// Буфер для чтения входных сигналов
        /// </summary>
        protected byte[] values_in;
        /// <summary>
        /// Буфер для чтения и записи выходных сигналов
        /// </summary>
        protected byte[] values_out;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_name">Имя платы для инициализации</param>
        /// <param name="_portCount_in">Количество входящих сигналов</param>
        /// <param name="_portCount_out">Количество исходящих сигналов</param>
        public PCIE_1730(string _name, int _portCount_in, int _portCount_out)
        {
            //portCount_in = _portCount_in;
            //portCount_out = _portCount_out;
            name = _name;
            values_in = new byte[_portCount_in];
            values_out = new byte[_portCount_out];
            string s = string.Format("{0}: {1}: {2}({3},{4})",
                "PCIE_1730", "Конструктор",name,_portCount_in,_portCount_out);
            Log.add(s);
            Debug.WriteLine(s);
        }
        /// <summary>
        /// Читаем входные сигналы
        /// </summary>
        /// <returns></returns>
        public abstract byte[] Read();
        /// <summary>
        /// Читаем выходные сигналы
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ReadOut();
        /// <summary>
        /// Записываем выходные сигналы
        /// </summary>
        /// <param name="_values_out"></param>
        public abstract void Write(byte[] _values_out);

        /// <summary>
        /// Получить бит в позиции _position
        /// </summary>
        /// <param name="_values">Битовый массив</param>
        /// <param name="_position">позиция для чтения</param>
        /// <returns></returns>
        public static bool GetBit(byte[] _values, int _position)
        {
            int bt = _position / 8;
            int pos = _position - bt * 8;
            return ((_values[bt] & (1 << pos)) != 0);
        }
        /// <summary>
        /// Получить бит в позиции _position
        /// </summary>
        /// <param name="_position">позиция для чтения</param>
        /// <param name="_in">вход/выход</param>
        /// <returns></returns>
        public bool GetBit(int _position, bool _in = true)
        {
            int bt = _position / 8;
            int pos = _position - bt * 8;
            byte[] data = (_in) ? values_in : values_out;
            return ((data[bt] & (1 << pos)) != 0);
        }
        /// <summary>
        /// Записать бит в позиции _position
        /// </summary>
        /// <param name="_position">позиция для записи</param>
        /// <param name="_val">Значение</param>
        /// <param name="_in">вход/выход</param>
        /// <returns></returns>
        public void SetBit(int _position, bool _val, bool _in=true)
        {
            int bt = _position / 8;
            int pos = _position - bt * 8;
            byte[] data = (_in) ? values_in : values_out;
            if (_val)
                data[bt] |= (byte)(1 << pos);
            else
                data[bt] &= (byte)(~((1) << pos));
        }

    }
}
