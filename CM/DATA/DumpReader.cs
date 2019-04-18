using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM
{
    public class DumpReader : IDataReader<double>
    {
        readonly List<double> dump;
        public double[] Read()
        {
            return dump.ToArray();
        }

        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }

        public DumpReader(string _fileName)
        {
            dump = DumpHelper.readDumpFile(_fileName);
            if (dump == null)
                throw new Exception("Ошибка чтения дампа");
        }
    }
}
