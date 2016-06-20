using System;
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

        private byte[] securityAccess;

        private uint processIndex = 0;
        private string processStr = null;

        private bool flashFlag = false;
        private bool sendFlag = false;

        private byte ServiceIdentifier;
        private int sendResult;
        private List<byte> mainSendData = new List<byte>();

        /// <summary>
        /// 下位机一次能够接受的最大数据长度
        /// </summary>
        private int bootCacheLength;
        private byte bootCacheBlockSequenceCounter = 0x01;
        /// <summary>
        /// 当前的S19Block中数据已经发送的块数
        /// </summary>
        private int bootCacheBlockSequenceIndex = 0x00;
        private uint s19FileDataCRC32Value = 0xFFFFFFFF;
        private uint s19BlockIndex = 0;
        /// <summary>
        /// 当前正在发送的S19Block
        /// </summary>
        private S19Block s19Block = null;
        /// <summary>
        /// 当前发送的S19文件
        /// </summary>
        private S19Block[] s19File = null;

        private IDictionary carSelected = null;
        private IDictionary flashProcess = null;
        private IDictionary sequence = null;

        private Security sec = null;
        private CanControl canCtl = null;
        private HexS19 s19 = null;

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

        public Flash(IDictionary carSelected, HexS19 s19)
        {
            this.carSelected = carSelected;
            flashProcess = (IDictionary)ConfigurationManager.GetSection("FlashConfig/Process");
            sequence = (IDictionary)ConfigurationManager.GetSection("FlashConfig/" + carSelected["FlashSequence"].ToString());

            physicalID = Convert.ToUInt32(carSelected["PhysicalID"].ToString(), 16);
            functionID = Convert.ToUInt32(carSelected["FunctionID"].ToString(), 16);
            receiveID = Convert.ToUInt32(carSelected["ReceiveID"].ToString(), 16);

            securityAccess = CanControl.canStringToByte(carSelected["SecurityAccess"].ToString());

            sec = new Security(securityAccess[2]);
            canCtl = CanControl.getCanControl();
            this.s19 = s19;
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
            sendResult = 0;

            while (flashFlag)
            {
                lock (canCtl)
                {
                    if (!sendFlag)
                    {
                        try { Monitor.Wait(canCtl); }
                        catch { }
                    }

                    sendCan();

                    if (sendResult < 0)
                    {
                        flashFlag = false;
                    }

                    sendFlag = false;
                    Monitor.Pulse(canCtl);
                }
            }
        }

        void sendCan()
        {
            try
            {
                processStr = sequence[processIndex.ToString()].ToString();
            }
            catch(NullReferenceException)
            {
                flashFlag = false;
                return;
            }

            mainSendData.Clear();
            mainSendData.AddRange(CanControl.canStringToByte(flashProcess[processStr].ToString()));
            ServiceIdentifier = mainSendData[0];

            switch (processStr)
            {
                case "SeedRequest":
                    mainSendData.Add(securityAccess[0]);
                    break;

                case "KeySend":
                    byte[] seed = new byte[4];
                    Array.Copy(CanControl.Rev, 3, seed, 0, 4);
                    byte[] key = sec.seedToKey(seed);
                    mainSendData.Add(securityAccess[1]);
                    mainSendData.AddRange(key);
                    break;

                case "DownloadRequest":
                    if (s19File == null)
                    {
                        s19File = s19.getS19File();
                        s19FileDataCRC32Value = CRC32.GetCRC_Custom(s19File);
                    }
                    s19Block = s19File[s19BlockIndex];
                    mainSendData.AddRange(s19Block.StartAddress);
                    mainSendData.AddRange(s19Block.DataLength);
                    break;

                case "DataTransfer":
                    int bootCacheBlockDataIndex = bootCacheBlockSequenceIndex * (bootCacheLength - 2);
                    mainSendData.Add(bootCacheBlockSequenceCounter);
                    for (int i = 0; i < bootCacheLength - 2; i++)
                    {
                        int indexTmp = bootCacheBlockDataIndex + i;
                        if (indexTmp >= s19Block.Data.Length)
                        {
                            break;
                        }
                        mainSendData.Add(s19Block.Data[indexTmp]);
                    }
                    bootCacheBlockSequenceCounter = (byte)((bootCacheBlockSequenceCounter + 1) & 0xFF);
                    bootCacheBlockSequenceIndex++;
                    break;

                case "RoutineIdentifier":
                    byte[] crc32Byte = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        crc32Byte[i] = (byte)(s19FileDataCRC32Value >> (8 * (3 - i)));
                    }
                    mainSendData.AddRange(crc32Byte);
                    break;

                case "MemoryErase":
                    mainSendData.Add(0x44);
                    if (s19File == null)
                    {
                        s19File = s19.getS19File();
                        s19FileDataCRC32Value = CRC32.GetCRC_Custom(s19File);
                    }
                    mainSendData.AddRange(s19File[s19BlockIndex].StartAddress);
                    mainSendData.AddRange(s19File[s19BlockIndex].DataLength);
                    break;

                default:
                    break;
            }

            sendResult = CanControl.sendFrame(physicalID, receiveID, mainSendData.ToArray());
        }

        void handleStart(object obj)
        {
            while (flashFlag)
            {
                lock (canCtl)
                {
                    if (sendFlag)
                    {
                        try { Monitor.Wait(canCtl); }
                        catch { }
                    }

                    handleCan();

                    sendFlag = true;
                    Monitor.Pulse(canCtl);
                }
            }
        }

        private void handleCan()
        {
            switch (CanControl.Rev[1])
            {
                case SI.NRSI:
                    switch (CanControl.Rev[3])
                    {
                        case NRC.RCRRP:
                            while (CanControl.canLastReceive(receiveID) == null || CanControl.Rev[1] == SI.NRSI)
                            {
                                Delay(10);
                            }
                            handleCan();
                            break;

                        case NRC.RTDNE:
                            for (int c = 0; c < 4; c++)
                            {
                                keepAlive();
                                Delay(2800);
                            }
                            break;

                        default:
                            flashFlag = false;
                            break;
                    }
                    break;

                case SI.RDSI + 0x40:
                    int lengthFormatIdentifier = CanControl.Rev[2] >> 4;
                    bootCacheLength = 0;
                    for (int i = 0; i < lengthFormatIdentifier; i++)
                    {
                        bootCacheLength += CanControl.Rev[3 + i] * (int)Math.Pow(0x100, lengthFormatIdentifier - i - 1);
                    }
                    processIndex++;
                    break;

                case SI.TDSI + 0x40:
                    if (bootCacheBlockSequenceIndex * (bootCacheLength - 2) >= s19Block.Data.Length)
                    {
                        s19BlockIndex++;
                        processIndex++;
                    }
                    break;

                case SI.RTESI + 0x40:
                    if (s19BlockIndex >= s19File.Length)
                    {
                        s19File = null;
                        s19BlockIndex = 0;
                        processIndex++;
                    }
                    else
                    {
                        processIndex = processIndex - 2;
                    }
                    bootCacheBlockSequenceIndex = 0;
                    bootCacheBlockSequenceCounter = 0x01;
                    break;

                case SI.ERSI + 0x40:
                    if (CanControl.Rev[2] == 0x01)
                    {
                        Delay(550);
                        processIndex++;
                    }
                    break;

                default:
                    if (ServiceIdentifier + 0x40 == CanControl.Rev[1])
                    {
                        processIndex++;
                    }
                    else
                    {
                        flashFlag = false;
                    }
                    break;
            }
        }

        public string readVersion()
        {
            if (carSelected == null)
            {
                return null;
            }

            byte[] tmp = CanControl.canStringToByte(carSelected["SoftwareVersion"].ToString());
            CanControl.sendFrame(physicalID, receiveID, tmp);

            if (SI.RDBISI + 0x40 == CanControl.Rev[1])
            {
                return Convert.ToString(CanControl.Rev[4], 16) + "." + Convert.ToString(CanControl.Rev[5], 16);
            }

            return "fail";
        }

        private bool keepAlive()
        {
            if (carSelected != null)
            {
                CanControl.sendFrame(physicalID, receiveID, CanControl.canStringToByte(flashProcess["PresentTester"].ToString()));
                return SI.TPSI + 0x40 == CanControl.Rev[1];
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
