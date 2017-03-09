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
        public delegate void Update(FormMain.UpdateUI cmd, string msg = null, int procssValue = 0, string msg2 = null);
        public event Update update;

        public string car = "";

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

        private bool s300Flag = false;

        private static string driverName = "FlashDriver_S12GX_V1.0.s19";
        internal static string flashSha1 = "23-F7-A2-AA-F5-AC-72-21-71-8D-58-62-0E-FE-B9-1E-A2-43-46-C7";

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

            update(FormMain.UpdateUI.InitUI);

            dataNum = (ulong)sequence.Keys.Count + s19.getS19DataNum();
            dataCounter = 0;

            sendThread.Start();
            handleThread.Start();

            sendThread.Join();
            handleThread.Join();

            processIndex = 0;
            CanControl.canClearBuffer();
            CanLog.makeLog();
            update(FormMain.UpdateUI.FinishFlash);
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
                        update(FormMain.UpdateUI.ErrorMessageShow, processStr + "...fail", 0, canCtl.sendError[sendResult]);
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
                s300Flag = false;
                update(FormMain.UpdateUI.MessageShow, "刷写完成");
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
                    mainSendData.Add(securityAccess[1]);
                    mainSendData.AddRange(seedToKey());
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

                case "WriteDataByIdentifier-RepairShopCode":
                    mainSendData.AddRange(System.Text.Encoding.ASCII.GetBytes("sanhua atc"));
                    break;

                case "WriteDataByIdentifier-ProgrammingDate":
                    mainSendData.AddRange(programmingDate());
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

            update(FormMain.UpdateUI.ProgreeBarSet, null, (int)(dataCounter * 100 / dataNum));
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
                                update(FormMain.UpdateUI.UpdateListBox, processStr + "...wait");
                            }
                            while (CanControl.canLastReceive(receiveID) == null)
                            {
                                Delay(10);
                            }
                            handleCan();
                            break;

                        case NRC.RTDNE:
                            update(FormMain.UpdateUI.UpdateListBox, processStr + "...keep alive");
                            for (int c = 0; c < 4; c++)
                            {
                                keepAlive();
                                Delay(40);
                            }
                            break;

                        default:
                            flashFlag = false;
                            update(FormMain.UpdateUI.ErrorMessageShow, processStr + "...fail", 0, "刷写失败: " + BitConverter.ToString(CanControl.Rev));
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
                    update(FormMain.UpdateUI.UpdateListBox, "sending file...");
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
                        update(FormMain.UpdateUI.UpdateListBox, "sending file...finish");
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
                        update(FormMain.UpdateUI.UpdateListBox, processStr + "...ok");
                        processIndex++;
                    }
                    break;

                case SI.DSCSI + 0x40:
                    if (car == "S300" && !s300Flag && CanControl.Rev[2] == 0x02)
                    {
                        byte[] tmp = { 0x10, 0x02 };
                        int i = 10;
                        while (i > 0)
                        {
                            CanControl.sendFrame(physicalID, receiveID, tmp);
                            Delay(30);
                            i--;
                        }
                        s300Flag = true;
                        handleCan();
                    }
                    else
                    {
                        goto default;
                    }
                    break;

                default:
                    if (ServiceIdentifier + 0x40 == CanControl.Rev[1])
                    {
                        update(FormMain.UpdateUI.UpdateListBox, processStr + "...ok");
                        processIndex++;
                    }
                    else
                    {
                        update(FormMain.UpdateUI.ErrorMessageShow, processStr + "...fail", 0, "刷写失败");
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

            if (car == "S300" && CanControl.revFirst[2] == SI.RDBISI + 0x40)
            {
                byte[] verData = new byte[6];
                Array.Copy(CanControl.revFirst, 6, verData, 0, 2);
                Array.Copy(CanControl.Rev, 1, verData, 2, 4);
                string ver = ((char)CanControl.revFirst[5]).ToString();
                Array.ForEach(verData, v => { ver += v.ToString("X2"); });
                return ver;
            }
            else if (SI.RDBISI + 0x40 == CanControl.Rev[1])
            {
                string ver = CanControl.Rev[6] == 0x55 ?
                        Convert.ToString(CanControl.Rev[4], 16) + "." + Convert.ToString(CanControl.Rev[5], 16) :
                        Convert.ToString(CanControl.Rev[4], 16) + "." + Convert.ToString(CanControl.Rev[5], 16)
                        + "." + Convert.ToString(CanControl.Rev[6] >> 4, 16) + Convert.ToString(CanControl.Rev[6] & 0x0F, 16);
                return ver;
            }

            return "fail";
        }

        private string s300Did(byte[] data, int receiveNum)
        {
            CanControl.sendFrame(physicalID, receiveID, data);
            if (receiveNum <= 4)
            {
                if (CanControl.Rev[1] == SI.RDBISI + 0x40)
                {
                    if (CanControl.Rev[3] == 0x99)
                    {
                        byte[] verData = new byte[4];
                        Array.Copy(CanControl.Rev, 4, verData, 0, 4);
                        string ver = "";
                        Array.ForEach(verData, v => { ver += v.ToString("X2"); });
                        return ver;
                    }
                }
            }
            else
            {
                if (CanControl.revFirst[2] == SI.RDBISI + 0x40)
                {
                    if (receiveNum <= 10)
                    {
                        if (CanControl.revFirst[4] == 0x83 || CanControl.revFirst[4] == 0x84 || CanControl.revFirst[4] == 0x8E
                            || CanControl.revFirst[4] == 0x91 || CanControl.revFirst[4] == 0xA0 || CanControl.revFirst[4] == 0xA1)
                        {
                            byte[] verData = new byte[6];
                            Array.Copy(CanControl.revFirst, 6, verData, 0, 2);
                            Array.Copy(CanControl.Rev, 1, verData, 2, 4);
                            string ver = ((char)CanControl.revFirst[5]).ToString();
                            Array.ForEach(verData, v => { ver += v.ToString("X2"); });
                            return ver;
                        }
                        else if (CanControl.revFirst[4] == 0x8A)
                        {
                            byte[] verData = new byte[5];
                            Array.Copy(CanControl.revFirst, 5, verData, 0, 3);
                            Array.Copy(CanControl.Rev, 1, verData, 3, 2);
                            string ver = "";
                            Array.ForEach(verData, v => { ver += ((char)v); });
                            return ver;
                        }
                    }
                }
            }
            
            return "null";
        }

        public void readDID()
        {
            if (car != "S300")
            {
                return;
            }

            string v;
            v = s300Did(new byte[3] { 0x22, 0xF1, 0x83 }, 7);
            update(FormMain.UpdateUI.UpdateListBox, "ECU Bootloader Software Number: " + v);

            v = s300Did(new byte[3] { 0x22, 0xF1, 0x8E }, 7);
            update(FormMain.UpdateUI.UpdateListBox, "ECU Assembly Number: " + v);

            v = s300Did(new byte[3] { 0x22, 0xF1, 0x91 }, 7);
            update(FormMain.UpdateUI.UpdateListBox, "ECU Hardware Number: " + v);

            v = s300Did(new byte[3] { 0x22, 0xF1, 0xA0 }, 7);
            update(FormMain.UpdateUI.UpdateListBox, "Vihicle Network Number: " + v);

            v = s300Did(new byte[3] { 0x22, 0xF1, 0xA1 }, 7);
            update(FormMain.UpdateUI.UpdateListBox, "ECU Calibration Software 1 Number: " + v);

            v = s300Did(new byte[3] { 0x22, 0xF1, 0x8A }, 7);
            update(FormMain.UpdateUI.UpdateListBox, "System Supplier Identifer: " + v);

            v = s300Did(new byte[3] { 0x22, 0xF1, 0x99 }, 4);
            update(FormMain.UpdateUI.UpdateListBox, "Programming Date: " + v);
        }

        private IEnumerable<byte> programmingDate()
        {
            int[] dateInt = new int[4];
            DateTime now = DateTime.Now;
            dateInt[0] = now.Year / 100;
            dateInt[1] = now.Year % 100;
            dateInt[2] = now.Month;
            dateInt[3] = now.Day;
            List<byte> byteTmp = new List<byte>();
            Array.ForEach(dateInt, t => { byteTmp.Add(Convert.ToByte(t.ToString(), 16)); });
            return byteTmp;
        }

        private IEnumerable<byte> seedToKey()
        {
            byte[] seed = new byte[4];
            Array.Copy(CanControl.Rev, 3, seed, 0, 4);
            return sec.seedToKey(seed);
        }

        private bool keepAliveForS300()
        {
            return false;
        }

        private bool keepAlive()
        {
            //if(car == "S300")
            //{
            //    CanControl.sendFrame(physicalID, receiveID, canStringToByte(flashProcess["ProgrammingSession"].ToString()));
            //    return true;
            //}
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
