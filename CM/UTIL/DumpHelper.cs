using System;
using System.Collections.Generic;
using System.IO;

namespace CM
{
    public static class DumpHelper
    {
        public static List<double> readDumpFile(string _fName)
        {
            List<double> data = new List<double>();
            byte[] bytes;
            try
            {
                using (FileStream stream = new FileStream(_fName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        while ((bytes = reader.ReadBytes(8)).Length == 8)
                        {
                            Array.Reverse(bytes);
                            double val = BitConverter.ToDouble(bytes, 0);
                            data.Add(val);
                        }
                        reader.Close();
                    }
                    stream.Close();
                }
                return data;
            }
            catch
            {
                return null;
            }
        }
        public static bool writeDumpFile(string _fName, List<double> _dump)
        {
            byte[] bytes;
            try
            {
                using (FileStream stream = new FileStream(_fName, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        foreach (double val in _dump)
                        {
                            bytes = BitConverter.GetBytes(val);
                            Array.Reverse(bytes);
                            writer.Write(bytes);
                        }
                        writer.Close();
                    }
                    stream.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
