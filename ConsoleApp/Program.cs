using CM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp
{
    class Program
    {
        static void dump2tube(string _dumpName, string _tubeName, TypeSize _typeSize)
        {
            Tube tube = new Tube(_typeSize, DefaultValues.TubeLen);
            Console.WriteLine("Set tube.typeSize.Sensors:{0}", tube.rtube.ts.sensors.ToString());
            Console.WriteLine(string.Format(@"Reading file: {0}...", _dumpName));
            DumpReader reader = new DumpReader(_dumpName);
            int size = tube.Write(reader.Read());
            Console.WriteLine(string.Format("{0} values readed from file \"{1}\" ({2}M)",
                size, _dumpName, size * sizeof(double) / 1024));
            if (!Tube.save(tube, _tubeName))
            {
                Console.WriteLine(string.Format("Tube.save error"));
                return;
            }

        }

        static void serializationTest(string _fileName, object _ob)
        {
            try
            {
                Type type = _ob.GetType();
                XmlSerializer formatter = new XmlSerializer(type);            // десериализация
                using (FileStream fs = new FileStream(_fileName, FileMode.Create))
                {
                    formatter.Serialize(fs, _ob);
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine(string.Format("{0}: {1}: {2}", _ob.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }

        }
        static void Main(string[] args)
        {
            string tubeName;
            string dumpName;
            TypeSize ts;

            tubeName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
               System.IO.Path.DirectorySeparatorChar + "tube1.bin";
            dumpName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
               System.IO.Path.DirectorySeparatorChar + "Dump.dbl";
            ts = new TypeSize("НКТ 73х01")
            {
                sensors = new SensorSettings(1, 4, 8, 16),
            };
            Tube tube = new Tube(ts,6000);
            Console.WriteLine("Set tube.typeSize.Sensors:{0}", tube.rtube.ts.sensors.ToString());
            Console.WriteLine(string.Format(@"Reading file: {0}...", dumpName));
            DumpReader reader = new DumpReader(dumpName);
            int size = tube.Write(reader.Read());
            Console.WriteLine(string.Format("{0} values readed from file \"{1}\" ({2}M)",
                size, dumpName, size * sizeof(double) / 1024));
            if (!Tube.save(tube, tubeName))
            {
                Console.WriteLine(string.Format("Tube.save error"));
                return;
            }

            if(!Tube.load(ref tube,tubeName))
            {
                Console.WriteLine(string.Format("Tube.load error"));
                return;
            }

            Console.WriteLine("All done. Press any key...");
            Console.ReadKey();
        }
    }
}
