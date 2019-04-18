using Protocol;
using System.Diagnostics;

namespace CM
{
    /// <summary>
    /// Эмулятор платы ввода/вывода 
    /// </summary>
    public class PCIE_1730_virtual : PCIE_1730
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_portCount_in"></param>
        /// <param name="_portCount_out"></param>
        public PCIE_1730_virtual(string _name, int _portCount_in, int _portCount_out):base(_name,_portCount_in,_portCount_out)
        {
            string s = string.Format("{0}: {1}: {2}({3},{4})",
                "PCIE_1730_virtual", "Конструктор", name, _portCount_in, _portCount_out);
            Log.add(s);
            Debug.WriteLine(s);
        }
        /// <summary>
        /// Читаем входные сигналы
        /// </summary>
        /// <returns></returns>
        public override byte[] Read()
        {
            if (disposed)
                return (null);
            return (values_in);
        }
        /// <summary>
        /// Читаем выходные сигналы
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadOut()
        {
            if (disposed)
                return (null);
            return (values_out);
        }
        /// <summary>
        /// Записываем выходные сигналы
        /// </summary>
        /// <param name="_values_out"></param>
        public override void Write(byte[] _values_out)
        {
            if (disposed)
                return;
        }
    }
}
