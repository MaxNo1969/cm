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
    public class CsvHelperTests
    {
        [TestMethod()]
        public void writeReadCsvFileTest()
        {
            Assert.IsNull(CsvHelper.readCsvFile("NotFound.csv"));
            List<double> test = new List<double>
            { 0, -1.3, 35.22, -8.5, 22, double.NaN, double.NegativeInfinity, double.PositiveInfinity };
            string fName = "test.csv";
            CsvHelper.writeCsvFile(fName, test);
            List<double> res = CsvHelper.readCsvFile(fName);
            CollectionAssert.AreEquivalent(res, test);
        }
    }
}