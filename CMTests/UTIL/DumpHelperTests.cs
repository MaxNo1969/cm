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
    public class DumpHelperTests
    {
        [TestMethod()]
        public void writeReadDumpFileTest()
        {
            Assert.IsNull(DumpHelper.readDumpFile("NotFound.dbl"));
            List<double> test = new List<double>
            { 0, -1.3, 35.22, -8.5, 22, double.NaN, double.NegativeInfinity, double.PositiveInfinity };
            string fName = "test.dump";
            DumpHelper.writeDumpFile(fName, test);
            List<double> res = DumpHelper.readDumpFile(fName);
            CollectionAssert.AreEquivalent(res,test);
        }
    }
}