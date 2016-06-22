﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;

namespace USBCAN
{
    class HexS19
    {
        private const byte S1AddressLen = 2;
        private const byte S2AddressLen = 3;
        private const byte S3AddressLen = 4;
        private const byte S5AddressLen = 2;

        private Queue<string> files = new Queue<string>();
        private Thread hexThread = null;
        private AutoResetEvent ctl = new AutoResetEvent(false);

        private S19Line[] s19Line = null;
        private List<string> strLineTmp = null;

        private List<S19Block[]> s19Files = new List<S19Block[]>();
        private Queue<S19Block[]> s19FilesGhost;

        internal int Count
        {
            get
            {
                return s19Files.Count;
            }
        }

        internal ThreadState HexThreadState
        {
            get
            {
                return hexThread.ThreadState;
            }
        }

        public HexS19()
        {
            hexThread = new Thread(hexStart);
            hexThread.IsBackground = true;
            hexThread.Start();
        }

        public void wakeUpHexThread()
        {
            if (hexThread.ThreadState != ThreadState.Background)
            {
                ctl.Set();
            }
        }

        private void hexStart()
        {
            while (true)
            {
                if (files.Count == 0 || files == null)
                {
                    ctl.WaitOne();
                }
                lock (s19Files)
                {
                    int r = readFile(files.Dequeue());
                    if (r == 1)
                    {
                        lineToBlock();
                    }
                }
            }

        }

        public void addFile(string[] files)
        {
            foreach (string file in files)
            {
                this.files.Enqueue(file);
            }
        }

        public void addFile(string file)
        {
            files.Enqueue(file);
        }

        internal void syncFilesWithUI(int cmd,int index, string[] files = null)
        {
            switch (cmd)
            {
                case -1:
                    s19Files.RemoveAt(index);
                    break;
                case 1:
                    addFile(files);
                    wakeUpHexThread();
                    break;
                case 2:
                    addFile(files);
                    wakeUpHexThread();

                    Thread.Sleep(1);
                    lock (s19Files)
                    {
                        if (s19Files.Count != 1)
                        {
                            S19Block[] tmp = s19Files[Count - 1];
                            s19Files.RemoveAt(Count - 1);
                            s19Files.Insert(0, tmp);
                        }
                    }
                    break;
            }
        }

        public S19Block[] getS19File()
        {
            if(s19FilesGhost == null || s19FilesGhost.Count == 0)
            {
                s19FilesGhost = new Queue<S19Block[]>(s19Files);
            }
            return s19FilesGhost.Dequeue();
        }

        public int readFile(string filePath)
        {
            int indexLine = 0;
            List<S19Line> s19LineTmp = new List<S19Line>();
            strLineTmp = new List<string>();
            string tmp;

            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    while (!sr.EndOfStream)
                    {
                        strLineTmp.Add(sr.ReadLine());
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
            

            for (int i = 0; i < strLineTmp.Count; i++)
            {
                byte checkSum = 0;
                tmp = strLineTmp[i].Substring(indexLine, 2);
                indexLine += 2;
                if (tmp.Equals("S0") || tmp.Equals("S7") || tmp.Equals("S8") || tmp.Equals("S9"))
                {
                    strLineTmp.Remove(strLineTmp[i]);
                    i--;
                }
                else if (tmp.Equals("S1") || tmp.Equals("S2") || tmp.Equals("S3"))
                {
                    byte tmpAddressLen = 0;
                    s19LineTmp.Add(new S19Line());
                    S19Line tmpS19 = s19LineTmp[i];
                    tmpS19.num = Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16);
                    checkSum += tmpS19.num;

                    switch (tmp)
                    {
                        case "S1": tmpAddressLen = S1AddressLen; break;
                        case "S2": tmpAddressLen = S2AddressLen; break;
                        case "S3": tmpAddressLen = S3AddressLen; break;
                    }

                    tmpS19.num = (byte)(tmpS19.num - tmpAddressLen - 1);
                    indexLine += 2;
                    tmpS19.lineAddress = new byte[4];
                    tmpS19.lineAddress.Initialize();
                    for (int a = 0; a < 4; a++)
                    {
                        int indexTmp = 4 - tmpAddressLen + a;
                        if (indexTmp < 4)
                        {
                            tmpS19.lineAddress[indexTmp] = Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16);
                            tmpS19.lineAddressAll += (uint)(tmpS19.lineAddress[indexTmp] * (Math.Pow(0x100, tmpAddressLen - a - 1)));
                            indexLine += 2;
                            checkSum += tmpS19.lineAddress[indexTmp];
                        }
                        else
                        {
                            break;
                        }
                    }
                    //for (int a = 0; a < tmpAddressLen; a++)
                    //{

                    //    tmpS19.lineAddress[a] = Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16);
                    //    tmpS19.lineAddressAll += (uint)(tmpS19.lineAddress[a] * (Math.Pow(0x100, tmpAddressLen - a - 1)));
                    //    indexLine += 2;
                    //    checkSum += tmpS19.lineAddress[a];
                    //}
                    tmpS19.date = new byte[tmpS19.num];
                    for (int j = 0; j < tmpS19.date.Length; j++)
                    {
                        tmpS19.date[j] = Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16);
                        checkSum += tmpS19.date[j];
                        indexLine += 2;
                    }
                    if ((0xff - checkSum) == Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16))
                    {
                        tmpS19.sumCheck = (byte)(0xff - checkSum);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return -1;
                }
                indexLine = 0;
            }
            s19Line = s19LineTmp.ToArray();
            return 1;
        }

        private void hex()
        {
            int indexLine = 0;
            List<S19Line> s19LineTmp = new List<S19Line>();
            strLineTmp = new List<string>();
            string tmp;

            for (int i = 0; i < strLineTmp.Count; i++)
            {
                byte checkSum = 0;
                tmp = strLineTmp[i].Substring(indexLine, 2);
                indexLine += 2;
                if (tmp.Equals("S0") || tmp.Equals("S7") || tmp.Equals("S8") || tmp.Equals("S9"))
                {
                    strLineTmp.Remove(strLineTmp[i]);
                    i--;
                }
                if (tmp.Equals("S1") || tmp.Equals("S2") || tmp.Equals("S3"))
                {
                    byte tmpAddressLen = 0;
                    s19LineTmp.Add(new S19Line());
                    S19Line tmpS19 = s19LineTmp[i];
                    tmpS19.num = Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16);
                    checkSum += tmpS19.num;

                    switch (tmp)
                    {
                        case "S1": tmpAddressLen = S1AddressLen; break;
                        case "S2": tmpAddressLen = S2AddressLen; break;
                        case "S3": tmpAddressLen = S3AddressLen; break;
                    }

                    tmpS19.num = (byte)(tmpS19.num - tmpAddressLen - 1);
                    indexLine += 2;
                    tmpS19.lineAddress = new byte[tmpAddressLen];
                    for (int a = 0; a < tmpAddressLen; a++)
                    {

                        tmpS19.lineAddress[a] = Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16);
                        tmpS19.lineAddressAll += (ushort)(tmpS19.lineAddress[a] * (Math.Pow(0x100, tmpAddressLen - a - 1)));
                        indexLine += 2;
                        checkSum += tmpS19.lineAddress[a];
                    }
                    tmpS19.date = new byte[tmpS19.num];
                    for (int j = 0; j < tmpS19.date.Length; j++)
                    {
                        tmpS19.date[j] = Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16);
                        checkSum += tmpS19.date[j];
                        indexLine += 2;
                    }
                    if ((0xff - checkSum) == Convert.ToByte(strLineTmp[i].Substring(indexLine, 2), 16))
                    {
                        tmpS19.sumCheck = (byte)(0xff - checkSum);
                    }
                    else
                    {
                        return;
                    }
                }
                indexLine = 0;
            }
            s19Line = s19LineTmp.ToArray();
        }

        public void lineToBlock()
        {
            List<S19Block> s19BlockTmp = new List<S19Block>();
            List<byte> s19BlockData = new List<byte>();

            int currentBlockIndex = 0;
            byte[] currentBlockAddress = s19Line[0].lineAddress;
            uint currentBlockLength = 0;
            byte[] currentBlockLengthByte = new byte[4];

            if (s19Line == null)
            {
                return;
            }

            for (int lineNum = 0; lineNum < s19Line.Length; lineNum++)
            {
                foreach (byte tmp in s19Line[lineNum].date)
                {
                    s19BlockData.Add(tmp);
                }

                currentBlockLength += s19Line[lineNum].num;

                if (lineNum + 1 >= s19Line.Length || s19Line[lineNum].lineAddressAll + s19Line[lineNum].num != s19Line[lineNum + 1].lineAddressAll)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        currentBlockLengthByte[3 - i] = (byte)((currentBlockLength & (0xff * (uint)Math.Pow(0x100, i))) >> 8 * i);
                    }

                    s19BlockTmp.Add(
                        new S19Block(
                            s19Line[currentBlockIndex].lineAddress,
                            currentBlockLengthByte,
                            s19BlockData.ToArray()
                            ));

                    if (lineNum + 1 >= s19Line.Length)
                    {
                        break;
                    }
                    //blockNum++;
                    currentBlockIndex = lineNum + 1;
                    s19BlockData = new List<byte>();
                    currentBlockLength = 0;
                    currentBlockAddress = s19Line[lineNum + 1].lineAddress;
                }
            }
            s19Files.Add(s19BlockTmp.ToArray());
            return;
        }

        internal ulong getS19DataNum()
        {
            ulong dataNum = 0;
            s19Files.ForEach(s => { Array.ForEach(s, ss => { dataNum += (ulong)ss.Data.Length; }); });
            return dataNum;
        }
    }

    class S19Line
    {
        public byte[] lineAddress;
        public uint lineAddressAll;
        public byte num;
        public byte[] date;
        public byte sumCheck;
    }

    class S19Block
    {
        private byte[] startAddress = new byte[4];
        private byte[] dataLength = new byte[4];
        private byte[] data;

        public byte[] StartAddress
        {
            get
            {
                return startAddress;
            }
        }

        public byte[] DataLength
        {
            get
            {
                return dataLength;
            }
        }

        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        public S19Block(byte[] startAddress, byte[] dataLength, byte[] data)
        {
            startAddress.CopyTo(this.startAddress, 0);
            dataLength.CopyTo(this.dataLength, 0);
            this.data = data;
        }

    }
}
