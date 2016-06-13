using System;
using System.Collections;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;

namespace USBCAN
{
    class Flash
    {
        private uint physicalID;
        private uint functionID;
        private uint receiveID;

        private uint currentCan = 0;

        private bool flashFlag = false;
        private bool sendFlag = false;

        private IDictionary carSelected = null;
        private Security sec = null;
        private CanControl canCtl = CanControl.getCanControl();

        private string driverName = "\\FlashDriver_S12GX_V1.0.s19";

        public string DriverName
        {
            get
            {
                return Environment.CurrentDirectory + driverName;
            }

            set
            {
                driverName = "\\" + value;
            }
        }

        private Thread flashThread = null;
        private Thread sendThread = null;
        private Thread receiveThread = null;

        public Flash(IDictionary carSelected)
        {
            this.carSelected = carSelected;
            sec = new Security(carSelected);

            physicalID = Convert.ToUInt32(carSelected["PhysicalID"].ToString(), 16);
            functionID = Convert.ToUInt32(carSelected["FunctionID"].ToString(), 16);
            receiveID = Convert.ToUInt32(carSelected["ReceiveID"].ToString(), 16);
        }

        public void init()
        {
            flashFlag = true;
            sendFlag = true;
            flashThread = new Thread(new ThreadStart(mainStart));
        }

        public void mainStart()
        {
            sendThread = new Thread(new ParameterizedThreadStart(sendCan));
            receiveThread = new Thread(new ParameterizedThreadStart(receiveCan));
        }

        void sendCan(object obj)
        {
            IDictionary flashConfig = (IDictionary)ConfigurationManager.GetSection("FlashConfig/" + carSelected["FlashProcess"].ToString());

            while (flashFlag)
            {
                lock(canCtl)
                {
                    if(!sendFlag)
                    {
                        try
                        {
                            Monitor.Wait(canCtl);
                        }
                        catch
                        {

                        }
                    }

                    CanControl.sendFrame(physicalID, CanControl.canStringToByte(flashConfig[currentCan++.ToString()].ToString()));
                    sendFlag = false;
                    Monitor.Pulse(canCtl);
                }
            }
        }

        void receiveCan(object obj)
        {
            while (flashFlag)
            {
                lock (canCtl)
                {
                    if (sendFlag)
                    {
                        try
                        {
                            Monitor.Wait(canCtl);
                        }
                        catch
                        {

                        }
                    }

                    CanControl.canLastReceive(receiveID);
                    sendFlag = true;
                    Monitor.Pulse(canCtl);
                }
            }
            //CanControl.canLastReceive(receiveID);
        }

        public string readVersion()
        {
            if (carSelected == null)
            {
                return null;
            }

            CanControl.sendFrame(physicalID, receiveID, CanControl.canStringToByte(carSelected["SoftwareVersion"].ToString()));
            
            if(CanControl.send[1] + 0x40 == CanControl.Rev[1])
            {
                return Convert.ToString(CanControl.Rev[4], 16) + "." + Convert.ToString(CanControl.Rev[5], 16);
            }

            return "fail";
        }
        
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }

    }
}
