using Microsoft.VisualStudio.TestTools.UnitTesting;
using USBCAN.Burn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.Burn.Tests
{
    [TestClass()]
    public class FlashConfigTests
    {
        [TestMethod()]
        public void FlashConfigTest()
        {
            //FlashConfig f = new FlashConfig(new string[2] { "json/config.json", "json/process.json" });
            //Car c = f.parseCar("N330");
            //Assert.Fail();
            ZLGCANJson zlgCan;
            ConfigJson c = FlashConfig.ParseConfig("json/config.json", out zlgCan);
            Car car = FlashConfig.ParseCar("N330", "json/process.json", c);
        }
    }
}