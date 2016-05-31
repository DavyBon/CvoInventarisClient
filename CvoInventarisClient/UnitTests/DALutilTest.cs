using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CvoInventarisClient.App_Code;

namespace CvoInventarisClient.UnitTests
{
    [TestClass]
    public class DALutilTest
    {
        [TestMethod]
        public void checkIntForDBNUllTestShouldReturnDBNullValueWhenPassNull()
        {
            object result = DALutil.checkIntForDBNUll(null);
            Assert.AreEqual(DBNull.Value, result);
        }

        [TestMethod]
        public void checkIntForDBNUllTestShouldReturnIntWhenPassInt()
        {
            object result = DALutil.checkIntForDBNUll(5);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void checkDecimalForDBNUllTestShouldReturnDBNullValueWhenPassNull()
        {
            object result = DALutil.checkDecimalForDBNUll(null);
            Assert.AreEqual(DBNull.Value, result);
        }

        [TestMethod]
        public void checkDecimalForDBNUllTestShouldReturnDecimalWhenPassDecimal()
        {
            object result = DALutil.checkDecimalForDBNUll(3.5M);
            Assert.AreEqual(3.5M, result);
        }
    }
}