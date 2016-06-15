﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace USBCAN
{
    class Flash
    {
        private uint physicalID;
        private uint functionID;
        private uint receiveID;

        private uint processIndex = 0;
        private string processStr = null;

        private bool flashFlag = false;
        private bool sendFlag = false;
        private bool afterKeepFlag = false;

        private byte ServiceIdentifier;
        private int Error;
        private List<byte> mainSendData;

        private byte[] securityAccess;
        private byte securityAccessType;

        private IDictionary carSelected = null;
        private IDictionary flashProcess = null;
        private IDictionary sequence = null;

        private Security sec = null;
        private CanControl canCtl = null;

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
            sequence = (IDictionary)ConfigurationManager.GetSection("FlashConfig/" + carSelected["FlashSequence"].ToString());

            physicalID = Convert.ToUInt32(carSelected["PhysicalID"].ToString(), 16);
            functionID = Convert.ToUInt32(carSelected["FunctionID"].ToString(), 16);
            receiveID = Convert.ToUInt32(carSelected["ReceiveID"].ToString(), 16);
            
            securityAccess = CanControl.canStringToByte(carSelected["SecurityAccess"].ToString());
            securityAccessType = securityAccess[0];

            sec = new Security(securityAccess[1]);
            canCtl = CanControl.getCanControl();
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
            string indexStrTmp = null;
            Error = 0;

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
                        catch(Exception ex)
                        {
                            
                        }
                    }

                    processIndex = processStr == "SecurityAccess" && afterKeepFlag ? (byte)(processIndex + 1) : processIndex;

                    if (processIndex == 6)
                    {
                        break;
                    }

                    indexStrTmp = processIndex.ToString();
                    processStr = sequence[indexStrTmp].ToString();
                    mainSendData = CanControl.canStringToByte(flashProcess[processStr].ToString()).ToList();
                    ServiceIdentifier = mainSendData[0];

                    switch (processStr)
                    {
                        case "SecurityAccess":
                            securityAccessType = afterKeepFlag ? (byte)(securityAccessType - 1) : securityAccessType;
                            if(securityAccessType != securityAccess[0])
                            {
                                byte[] seed = new byte[4];
                                for(int i = 0; i < 4; i++)
                                {
                                    seed[i] = CanControl.Rev[3 + i];
                                }
                                byte[] key = sec.seedToKey(seed);

                                mainSendData.Add(securityAccessType);
                                mainSendData.AddRange(key);                           
                            }
                            else
                            {
                                mainSendData.Add(securityAccessType);                               
                            }

                            Error = CanControl.sendFrame(physicalID, receiveID, mainSendData.ToArray());
                            securityAccessType++;
                            if(securityAccessType - securityAccess[0] == 2)
                            {
                                break;
                            }
                            processIndex = securityAccessType != securityAccess[0] ? processIndex - 1 : processIndex;
                            break;
                        default:
                            Error = CanControl.sendFrame(physicalID, receiveID, mainSendData.ToArray());
                            break;
                    }

                    afterKeepFlag = false;

                    if (Error < 0)
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

                    sendFlag = true;
                    Monitor.Pulse(canCtl);
                }
            }
            //CanControl.canLastReceive(receiveID);
        }

        private void handleCan()
        {
            if (ServiceIdentifier + 0x40 == CanControl.Rev[1])
            {
                processIndex++;
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
                            processIndex++;
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
