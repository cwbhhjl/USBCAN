using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace USBCAN
{
    class Flash
    {
        public delegate void Updata(int cmd, string msg = null, int procssValue = 0, string msg2 = null);
        public event Updata updata;

        private uint physicalID;
        private uint functionID;
        private uint receiveID;

        private uint sendID;

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
        private int blCacheLength;
        private byte blCacheBlockSequenceCounter = 0x01;
        /// <summary>
        /// 当前的S19Block中数据已经发送的块数
        /// </summary>
        private int blCacheBlockSequenceIndex = 0x00;
        private uint s19FileDataCRC32Value = 0xFFFFFFFF;
        private uint s19BlockIndex;
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

        private Regex reg = new Regex("_[pf]$", RegexOptions.IgnoreCase);

        private ulong dataNum = 0;
        private ulong dataCounter = 0;

        private static string driverName = "FlashDriver_S12GX_V1.0.s19";

        public static string DriverName
        {
            get
            {
                return Environment.CurrentDirectory + "\\" + driverName;
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

            securityAccess = canStringToByte(carSelected["SecurityAccess"].ToString());

            if (carSelected["SecurityAccessMask"] != null)
            {
                sec = new Security(securityAccess[2], Convert.ToUInt32(carSelected["SecurityAccessMask"].ToString(), 16));
            }
            else
            {
                sec = new Security(securityAccess[2]);
            }
            
            canCtl = CanControl.getCanControl();
            this.s19 = s19;
        }

        public void init()
        {
            flashFlag = true;
            sendFlag = true;
            flashThread = new Thread(new ThreadStart(mainStart));
            flashThread.IsBackground = true;
        }

        public void mainStart()
        {
            sendThread = new Thread(new ParameterizedThreadStart(sendStart));
            handleThread = new Thread(new ParameterizedThreadStart(handleStart));

            sendThread.IsBackground = true;
            handleThread.IsBackground = true;

            if (carSelected["FlashDriver"] != null && new Regex("true", RegexOptions.IgnoreCase).IsMatch(carSelected["FlashDriver"].ToString()))
            {
                s19.getS19File();
            }

            updata(-1);

            dataNum = (ulong)sequence.Keys.Count + s19.getS19DataNum();
            dataCounter = 0;

            sendThread.Start();
            handleThread.Start();

            sendThread.Join();
            handleThread.Join();

            processIndex = 0;
            CanControl.canClearBuffer();
            updata(0);
        }

        void sendStart(object obj)
        {
            sendResult = 0;
            CanControl.canClearBuffer();

            while (flashFlag)
            {
                lock (canCtl)
                {
                    if (!sendFlag)
                    {
                        try { Monitor.Wait(canCtl); }
                        catch { }
                    }

                    if (!flashFlag)
                    {
                        break;
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

                    if (!flashFlag)
                    {
                        break;
                    }

                    handleCan();

                    sendFlag = true;
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
            catch (NullReferenceException)
            {
                flashFlag = false;
                updata(2, "刷写完成");
                return;
            }

            if (reg.IsMatch(processStr))
            {
                sendID = (processStr.EndsWith("_f") || processStr.EndsWith("_F")) ? functionID : physicalID;
                processStr = reg.Split(processStr)[0];
            }
            else
            {
                sendID = physicalID;
            }

            mainSendData.Clear();
            mainSendData.AddRange(canStringToByte(flashProcess[processStr].ToString()));
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
                    int blCacheBlockDataIndex = blCacheBlockSequenceIndex * (blCacheLength - 2);
                    mainSendData.Add(blCacheBlockSequenceCounter);
                    for (int i = 0; i < blCacheLength - 2; i++)
                    {
                        int indexTmp = blCacheBlockDataIndex + i;
                        if (indexTmp >= s19Block.Data.Length)
                        {
                            break;
                        }
                        mainSendData.Add(s19Block.Data[indexTmp]);
                    }
                    blCacheBlockSequenceCounter = (byte)((blCacheBlockSequenceCounter + 1) & 0xFF);
                    blCacheBlockSequenceIndex++;
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

            sendResult = CanControl.sendFrame(sendID, receiveID, mainSendData.ToArray());

            if (mainSendData[0] != SI.TDSI)
            {
                dataCounter++;
            }
            else
            {
                dataCounter = dataCounter + (ulong)mainSendData.Count - 2;
            }

            updata(4, "", (int)(dataCounter * 100 / dataNum));
        }

        private void handleCan()
        {
            switch (CanControl.Rev[1])
            {
                case SI.NRSI:
                    switch (CanControl.Rev[3])
                    {
                        case NRC.RCRRP:
                            if (processStr != "DataTransfer")
                            {
                                updata(1, processStr + "...wait");
                            }
                            while (CanControl.canLastReceive(receiveID) == null || CanControl.Rev[1] == SI.NRSI)
                            {
                                Delay(10);
                            }
                            handleCan();
                            break;

                        case NRC.RTDNE:
                            updata(1, processStr + "...keep alive");
                            for (int c = 0; c < 4; c++)
                            {
                                keepAlive();
                                Delay(2800);
                            }
                            break;

                        default:
                            flashFlag = false;
                            updata(5, 
                                processStr + "...fail", 0, 
                                "刷写失败: 7F" + Convert.ToString(CanControl.Rev[2], 16) + Convert.ToString(CanControl.Rev[3], 16));
                            break;
                    }
                    break;

                case SI.RDSI + 0x40:
                    int lengthFormatIdentifier = CanControl.Rev[2] >> 4;
                    blCacheLength = 0;
                    for (int i = 0; i < lengthFormatIdentifier; i++)
                    {
                        blCacheLength += CanControl.Rev[3 + i] * (int)Math.Pow(0x100, lengthFormatIdentifier - i - 1);
                    }
                    updata(1, "sending file...");
                    processIndex++;
                    break;

                case SI.TDSI + 0x40:
                    if (blCacheBlockSequenceIndex * (blCacheLength - 2) >= s19Block.Data.Length)
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
                        updata(1, "sending file...finish");
                        processIndex++;
                    }
                    else
                    {
                        processIndex = processIndex - 2;
                    }
                    blCacheBlockSequenceIndex = 0;
                    blCacheBlockSequenceCounter = 0x01;
                    break;

                case SI.ERSI + 0x40:
                    if (CanControl.Rev[2] == 0x01)
                    {
                        Delay(550);
                        updata(1, processStr + "...ok");
                        processIndex++;
                    }
                    break;

                default:
                    if (ServiceIdentifier + 0x40 == CanControl.Rev[1])
                    {
                        updata(1, processStr + "...ok");
                        processIndex++;
                    }
                    else
                    {
                        updata(5, processStr + "...fail", 0, "刷写失败");
                        flashFlag = false;
                    }
                    break;
            }
        }

        internal void go()
        {
            if (flashThread == null || flashThread.ThreadState == (ThreadState.Background | ThreadState.Unstarted)
                || flashThread.ThreadState == ThreadState.Stopped)
            {
                init();
                flashThread.Start();
            }
        }

        public string readVersion()
        {
            if (carSelected == null)
            {
                return null;
            }

            byte[] tmp = canStringToByte(carSelected["SoftwareVersion"].ToString());
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
                CanControl.sendFrame(physicalID, receiveID, canStringToByte(flashProcess["PresentTester"].ToString()));
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

        internal static byte[] canStringToByte(string str)
        {
            string[] strTmp = str.Split(' ');
            List<byte> byteTmp = new List<byte>();
            foreach (string i in strTmp)
            {
                byteTmp.Add(Convert.ToByte(i, 16));
            }
            return byteTmp.ToArray();
        }
    }
}
