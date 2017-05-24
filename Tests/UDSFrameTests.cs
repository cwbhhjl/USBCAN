using Microsoft.VisualStudio.TestTools.UnitTesting;
using BtFlash.UDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtFlash.UDS.Tests
{
    [TestClass()]
    public class UDSFrameTests
    {
        [TestMethod()]
        public void NextFrameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UDSFrameTest()
        {
            UDSFrame u = new UDSFrame(0x741, 
                Enumerable.Repeat<byte>(0x11, 120).ToList()
                );
            Assert.Fail();
        }
    }
}