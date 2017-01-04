using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace USBCAN.Burn
{
    public class FlashControl
    {
        public delegate void ComboBoxItemsAddRange(string[] range);
        public ComboBoxItemsAddRange comboBoxItemsAddRange;

        private ConfigJson config;
        private ZLGCANJson zlgCan;
        DeviceControl device;
        Car car;
        public string CarSelected { get; set; }

        private Thread mainThread;

        public FlashControl()
        {
            mainThread = new Thread(new ThreadStart(MainProcess));
            CarSelected = "NULL";
        }

        public void MainStart()
        {
            mainThread.IsBackground = true;
            mainThread.Start();
        }

        void MainProcess()
        {
            config = FlashConfig.ParseConfig("json/config.json", out zlgCan);
            comboBoxItemsAddRange(config.cars);

            while(CarSelected == "NULL")
            {
                ;
            }
            ;
        }
    }
}
