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
    public class TypeSizeTests
    {
        [TestMethod()]
        public void parseNameTest()
        {
            string tsName = "НКТ 73x10";
            string type;
            int diameter;
            string dop;
            TypeSize.parseName(tsName, out type, out diameter, out dop);
            Assert.AreEqual(type,"НКТ");
            Assert.AreEqual(diameter, 73);
            Assert.AreEqual(dop, "10");
            Assert.IsFalse(TypeSize.parseName("NKT 73x10", out type, out diameter, out dop));
            Assert.IsFalse(TypeSize.parseName("НКТ73x10", out type, out diameter, out dop));
            Assert.IsFalse(TypeSize.parseName("НКТ 70x10", out type, out diameter, out dop));
        }
    }
}