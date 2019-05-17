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
            Tube tube = new Tube(ts);
            Assert.IsNotNull(tube);
        }
    }
}