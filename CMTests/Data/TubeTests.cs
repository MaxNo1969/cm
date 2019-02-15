using Microsoft.VisualStudio.TestTools.UnitTesting;
using CM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.Tests
{
    [TestClass()]
    public class TubeTests
    {
        [TestMethod()]
        public void TubeTest()
        {
            TypeSize ts = new TypeSize("СБТ 73 01");
            Tube tube = new Tube(ts,DefaultValues.tubeLen);
            Assert.IsNotNull(tube);
        }

        [TestMethod()]
        public void writeReadDumpTest()
        {
            List<double> test = new List<double>
            { 0, -1.3, 35.22, -8.5, 22, double.NaN, double.NegativeInfinity, double.PositiveInfinity };
            string fName = "test.dump";
            DumpHelper.writeDumpFile(fName, test);
            TypeSize ts = new TypeSize("СБТ 73 01");
            Tube tube = new Tube(ts,DefaultValues.tubeLen);
            tube.readDump("test.dump");
            CollectionAssert.AreEquivalent(test, tube.raw);            
        }

        [TestMethod()]
        public void readCSVTest()
        {
            List<double> test = new List<double>
            { 0, -1.3, 35.22, -8.5, 22, double.NaN, double.NegativeInfinity, double.PositiveInfinity };
            string fName = "test.csv";
            CsvHelper.writeCsvFile(fName, test);
            TypeSize ts = new TypeSize("СБТ 73 01");
            Tube tube = new Tube(ts,DefaultValues.tubeLen);
            tube.readCSV("test.csv");
            CollectionAssert.AreEquivalent(test, tube.raw);
        }
    }
}