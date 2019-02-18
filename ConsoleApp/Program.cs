using CM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program started...");
            string fileName = "Dump.dbl";
            string tubeName = "tube1.bin";
            string dumpFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
               System.IO.Path.DirectorySeparatorChar + fileName;
            TypeSize ts = new TypeSize("НКТ 73х01")
            {
                sensors = new SensorPars(1, 4, 8, 16),
            };
            Tube tube = new Tube(ts, DefaultValues.tubeLen);
            Console.WriteLine("Set tube.typeSize.Sensors:{0}", tube.typeSize.sensors.ToString());
            Console.WriteLine(string.Format(@"Reading file: {0}...", dumpFileName));
            DumpReader reader = new DumpReader(dumpFileName); 
            int size = tube.Write(reader.Read());
            Console.WriteLine(string.Format("{0} values readed from file \"{1}\" ({2}M)",
                size, dumpFileName, size * sizeof(double) / 1024));
            if (!Tube.save(tube, tubeName))
            {
                Console.WriteLine(string.Format("Tube.save error"));
                return;
            }


            fileName = "Dump2.dbl";
            tubeName = "tube2.bin";
            dumpFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
               System.IO.Path.DirectorySeparatorChar + fileName;
            Console.WriteLine(string.Format(@"Reading file: {0}...", dumpFileName));
            ts = new TypeSize("СБТ 127х9,2")
            {
                sensors = new SensorPars(1, 4, 8, 32),
            };
            tube = new Tube(ts, DefaultValues.tubeLen);
            Console.WriteLine("Set tube.typeSize.Sensors:{0}", tube.typeSize.sensors.ToString());
            reader = new DumpReader(dumpFileName);
            size = tube.Write(reader.Read());
            Console.WriteLine(string.Format("{0} values readed from file \"{1}\" ({2}M)",
                size, dumpFileName, size * sizeof(double) / 1024));
            if (!Tube.save(tube, tubeName))
            {
                Console.WriteLine(string.Format("Tube.save error"));
                return;
            }
            Console.WriteLine("All done. Press any key...");
            Console.ReadKey();
        }
    }
}
