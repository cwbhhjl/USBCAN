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
        private string processStr = null;

        private bool flashFlag = false;
        private bool sendFlag = false;
        private bool afterKeepFlag = false;

        private byte securityAccessType;

        private IDictionary carSelected = null;
        private IDictionary flashProcess = null;
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

        public Thread flashThread = null;
        private Thread sendThread = null;
        private Thread handleThread = null;

        public Flash(IDictionary carSelected)
        {
            this.carSelected = carSelected;
            flashProcess = (IDictionary)ConfigurationManager.GetSection("FlashConfig/Process");           
            physicalID = Convert.ToUInt32(carSelected["PhysicalID"].ToString(), 16);
            functionID = Convert.ToUInt32(carSelected["FunctionID"].ToString(), 16);
            receiveID = Convert.ToUInt32(carSelected["ReceiveID"].ToString(), 16);
            securityAccessType = Convert.ToByte(carSelected["SecurityAccessType"].ToString(), 16);
            sec = new Security(securityAccessType);
        }

        public void init()
        {
            flashFlag = true;
            sendFlag = true;
            flashThread = new Thread(new ThreadStart(mainStart));
        }

        public void mainStart()
        {
            sendThread = new Thread(new ParameterizedThreadStart(sendStart));
            handleThread = new Thread(new ParameterizedThreadStart(handleStart));
            sendThread.Start();
            handleThread.Start();
            sendThread.Join();
            handleThread.Join();
        }

        void sendStart(object obj)
        {
            IDictionary sequence = (IDictionary)ConfigurationManager.GetSection("FlashConfig/" + carSelected["FlashSequence"].ToString());
            string indexStrTmp = null;
            int sendError = 0;

            while (flashFlag)
            {
                lock (canCtl)
                {
                    if (!sendFlag)
                    {
                        try
                        {
                            Monitor.Wait(canCtl);
                        }
                        catch
                        {

                        }
                    }

                    currentCan = processStr == "SecurityAccess" && afterKeepFlag ? (byte)(currentCan + 1) : currentCan;

                    if (currentCan == 6)
                    {
                        break;
                    }

                    indexStrTmp = currentCan.ToString();
                    processStr = sequence[indexStrTmp].ToString();

                    switch (processStr)
                    {
                        case "SecurityAccess":
                            securityAccessType = afterKeepFlag ? (byte)(securityAccessType - 1) : securityAccessType;
                            if(securityAccessType != Convert.ToByte(carSelected["SecurityAccessType"].ToString(), 16))
                            {
                                byte[] key;
                                byte[] seed = new byte[4];
                                for(int i = 0; i < 4; i++)
                                {
                                    seed[i] = CanControl.Rev[3 + i];
                                }
                                key = sec.seedToKey(seed);
                                sendError = CanControl.sendFrame(physicalID, receiveID, CanControl.canStringToByte(flashProcess[processStr].ToString() + " " + securityAccessType.ToString() + " " + Convert.ToString(key[0], 16) + " " + Convert.ToString(key[1], 16) + " " + Convert.ToString(key[2], 16) + " " + Convert.ToString(key[3], 16)));
                            }
                            else
                            {
                                sendError = CanControl.sendFrame(physicalID, receiveID, CanControl.canStringToByte(flashProcess[processStr].ToString() + " " + securityAccessType.ToString()));
                            }                                                   
                            securityAccessType++;
                            if(securityAccessType - Convert.ToByte(carSelected["SecurityAccessType"].ToString(), 16) == 2)
                            {
                                break;
                            }
                            currentCan = securityAccessType != Convert.ToByte(carSelected["SecurityAccessType"].ToString(), 16) ? currentCan - 1 : currentCan;
                            break;
                        default:
                            sendError = CanControl.sendFrame(physicalID, receiveID, CanControl.canStringToByte(flashProcess[processStr].ToString()));
                            break;
                    }

                    afterKeepFlag = false;

                    if (sendError < 0)
                    {
                        break;
                    }

                    sendFlag = false;
                    Monitor.Pulse(canCtl);
                }
            }
        }

        void handleStart(object obj)
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

                    handleCan();
                    //if (CanControl.send[0] + 0x40 == CanControl.Rev[1])
                    //{
                    //    currentCan++;
                    //}
                    //else
                    //{
                    //    switch (CanControl.Rev[1])
                    //    {

                    //    }
                    //}

                    sendFlag = true;
                    Monitor.Pulse(canCtl);
                }
            }
            //CanControl.canLastReceive(receiveID);
        }

        private void handleCan()
        {
            if (CanControl.send[0] + 0x40 == CanControl.Rev[1])
            {
                currentCan++;
            }
            else if (CanControl.Rev[1] == SI.NRSI)
            {
                switch (CanControl.Rev[3])
                {
                    case NRC.RCRRP:
                        while (CanControl.canLastReceive(receiveID) == null || CanControl.Rev[1] == SI.NRSI)
                        {
                            Delay(10);
                        }
                        if(CanControl.Rev[1] == CanControl.send[0] + 0x40)
                        {
                            currentCan++;
                            break;
                        }
                        flashFlag = false;
                        break;
                    case NRC.RTDNE:
                        for(int c = 0; c < 4; c++)
                        {
                            keepAlive();
                            Delay(3500);
                        }
                        afterKeepFlag = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public string readVersion()
        {
            if (carSelected == null)
            {
                return null;
            }

            CanControl.sendFrame(physicalID, receiveID, CanControl.canStringToByte(carSelected["SoftwareVersion"].ToString()));

            if (CanControl.send[0] + 0x40 == CanControl.Rev[1])
            {
                return Convert.ToString(CanControl.Rev[4], 16) + "." + Convert.ToString(CanControl.Rev[5], 16);
            }

            return "fail";
        }

        public bool keepAlive()
        {
            if (carSelected != null)
            {
                CanControl.sendFrame(physicalID, receiveID, CanControl.canStringToByte(flashProcess["PresentTester"].ToString()));
                return CanControl.send[0] + 0x40 == CanControl.Rev[1];
            }
            return false;
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
