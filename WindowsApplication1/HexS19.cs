using System;
using System.Collections;
using System.IO;
using System.Text;

namespace WindowsApplication1
{
    class HexS19
    {
        private const byte S1AddressLen = 2;
        private const byte S2AddressLen = 3;
        private const byte S3AddressLen = 4;
        private const byte S5AddressLen = 2;

        private S19Line[] s19Line = null;

        ArrayList strLineTmp = null;

        public void readFile(FileStream fs)
        {
            using (fs)
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);

                while (!sr.EndOfStream)
                {
                    strLineTmp.Add(sr.ReadLine());
                } 
            }
        }

        public int readFile(string filePath)
        {
            int indexLine = 0;
            ArrayList s19LineTmp = new ArrayList();
            strLineTmp = new ArrayList();
            string tmp;

            using (FileStream fs = File.OpenRead(filePath))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                while (!sr.EndOfStream)
                {
                    strLineTmp.Add(sr.ReadLine());
                }
                //sr.Close();
                //line = (string[])strLineTmp.ToArray(typeof(string));
            }
            //FileStream fs = null;
            //try
            //{
            //    fs = File.OpenRead(driverPath);
            //    using (StreamReader sr = new StreamReader(fs, Encoding.Default))
            //    {
            //        while (!sr.EndOfStream)
            //        {
            //            strLineTmp.Add(sr.ReadLine());
            //        }
            //    }
            //    line = (string[])strLineTmp.ToArray(typeof(string));
            //}
            //finally
            //{
            //    if(fs!=null)
            //    {
            //        fs.Dispose();
            //    }
            //}
            for (int i = 0; i < strLineTmp.Count; i++)
            {
                byte checkSum = 0;
                tmp = ((string)strLineTmp[i]).Substring(indexLine, 2);
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
                    S19Line tmpS19 = ((S19Line)s19LineTmp[i]);
                    tmpS19.num = Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
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

                        tmpS19.lineAddress[a] = Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
                        tmpS19.lineAddressAll += (ushort)(tmpS19.lineAddress[a] * (Math.Pow(0x100, tmpAddressLen - a - 1)));
                        indexLine += 2;
                        checkSum += tmpS19.lineAddress[a];
                    }
                    tmpS19.date = new byte[tmpS19.num];
                    for (int j = 0; j < tmpS19.date.Length; j++)
                    {
                        tmpS19.date[j] = Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
                        checkSum += tmpS19.date[j];
                        indexLine += 2;
                    }
                    if ((0xff - checkSum) == Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16))
                    {
                        tmpS19.sumCheck = (byte)(0xff - checkSum);
                    }
                    else
                    {
                        return 0;
                    }
                }
                indexLine = 0;
            }
            s19Line = (S19Line[])s19LineTmp.ToArray(typeof(S19Line));
            return 1;
        }

        void hex()
        {
            int indexLine = 0;
            ArrayList s19LineTmp = new ArrayList();
            strLineTmp = new ArrayList();
            string tmp;

            for (int i = 0; i < strLineTmp.Count; i++)
            {
                byte checkSum = 0;
                tmp = ((string)strLineTmp[i]).Substring(indexLine, 2);
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
                    S19Line tmpS19 = ((S19Line)s19LineTmp[i]);
                    tmpS19.num = Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
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

                        tmpS19.lineAddress[a] = Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
                        tmpS19.lineAddressAll += (ushort)(tmpS19.lineAddress[a] * (Math.Pow(0x100, tmpAddressLen - a - 1)));
                        indexLine += 2;
                        checkSum += tmpS19.lineAddress[a];
                    }
                    tmpS19.date = new byte[tmpS19.num];
                    for (int j = 0; j < tmpS19.date.Length; j++)
                    {
                        tmpS19.date[j] = Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
                        checkSum += tmpS19.date[j];
                        indexLine += 2;
                    }
                    if ((0xff - checkSum) == Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16))
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
            s19Line = (S19Line[])s19LineTmp.ToArray(typeof(S19Line));
        }

        public S19Block[] lineToBlock()
        {
            ArrayList s19BlockTmp = new ArrayList();
            ArrayList s19BlockData = new ArrayList();

            int currentBlockIndex = 0;
            byte[] currentBlockAddress = s19Line[0].lineAddress;
            uint currentBlockLength = 0;
            byte[] currentBlockLengthByte = new byte[4];

            if (s19Line == null)
            {
                return null;
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

                    //currentBlockLengthByte[3] = (byte)(currentBlockLength & 0xff);
                    //currentBlockLengthByte[2] = (byte)((currentBlockLength & 0xff00)>>8);
                    //currentBlockLengthByte[1] = (byte)((currentBlockLength & 0xff0000) >> 16);
                    //currentBlockLengthByte[0] = (byte)((currentBlockLength & 0xff000000) >> 24);

                    s19BlockTmp.Add(
                        new S19Block(
                            s19Line[currentBlockIndex].lineAddress, 
                            currentBlockLengthByte, 
                            (byte[])s19BlockData.ToArray(typeof(byte))
                            ));
                    if (lineNum + 1 >= s19Line.Length)
                    {
                        break;
                    }
                    //blockNum++;
                    currentBlockIndex = lineNum + 1;
                    s19BlockData = new ArrayList();
                    currentBlockLength = 0;
                    currentBlockAddress = s19Line[lineNum + 1].lineAddress;
                }
            }

            return (S19Block[])s19BlockTmp.ToArray(typeof(S19Block));
        }
    }

    class S19Line
    {
        public byte[] lineAddress;
        public ushort lineAddressAll;
        public byte num;
        public byte[] date;
        public byte sumCheck;
    }

    class S19Block
    {
        private byte[] startAddress;
        private byte[] dataLength;
        private byte[] data;
        public S19Block(byte[] startAddress, byte[] dataLength, byte[] data)
        {
            this.startAddress = startAddress;
            this.dataLength = dataLength;
            this.data = data;
        }
    }
}
