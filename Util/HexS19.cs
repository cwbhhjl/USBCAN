using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
using System.Runtime.Serialization;

namespace BtFlash.Util
{
    [Serializable]
    public sealed class BadChecksumException : ApplicationException
    {
        public BadChecksumException() : base() { }

        public BadChecksumException(string msg) : base(msg) { }

        public BadChecksumException(string msg, Exception inner) : base(msg, inner) { }

        private BadChecksumException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public class HexS19
    {
        private static readonly byte S1AddressLen = 2;
        private static readonly byte S2AddressLen = 3;
        private static readonly byte S3AddressLen = 4;
        private static readonly byte S5AddressLen = 2;

        private static readonly Dictionary<string, byte> AddressTypeLength = new Dictionary<string, byte>
        {
            {"S0", 0 },
            {"S1", 2 },
            {"S2", 3 },
            {"S3", 4 },
            {"S7", 0 },
            {"S8", 0 },
            {"S9", 0 }
        };

        private S19Line[] s19Line = null;
        private List<string> strLineTmp = null;

        private List<S19Block[]> s19Files = new List<S19Block[]>();
        private Queue<S19Block[]> s19FilesGhost;

        private IEnumerable<string> ConvertFileToLines(string path)
        {
            List<string> strLines = new List<string>();

            using (FileStream fs = File.OpenRead(path))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                while (!sr.EndOfStream)
                {
                    strLines.Add(sr.ReadLine());
                }
            }

            return strLines;
        }

        public T[] SplitArray<T>(T[] Source, int StartIndex, int EndIndex)
        {
            T[] result = new T[EndIndex - StartIndex];
            for (int i = 0; i < EndIndex - StartIndex; i++) result[i] = Source[i + StartIndex];
            return result;
        }

        private S19Line ConvertDataStrToS19Line(string line)
        {
            byte addressLength = AddressTypeLength[line.Substring(0, 2)];

            if (addressLength == 0)
            {
                return null;
            }

            byte[] data = new byte[(line.Length >> 1) - 3];

            int dataLength = data.Length;
            for (int i = 0; i < dataLength; i++)
            {
                data[i] = Convert.ToByte(line.Substring(i * 2 + 4, 2), 16);
            }

            byte checksum = Convert.ToByte(line.Substring(2, 2), 16);
            Array.ForEach(data, d => { checksum += d; });

            if ((0xff - checksum) != Convert.ToByte(line.Substring(line.Length - 2, 2)))
            {
                throw new BadChecksumException();
            }

            return new S19Line(SplitArray(data, 0, addressLength), SplitArray(data, addressLength, dataLength));
        }

        private IEnumerable<S19Line> ConvertLineStrsToS19Lines(IEnumerable<string> lines)
        {
            LinkedList<S19Line> s19LineTmp = new LinkedList<S19Line>();

            foreach (var line in lines)
            {
                byte addressLen = AddressTypeLength[line.Substring(0, 2)];

                if (addressLen == 0)
                {
                    continue;
                }
                else
                {
                    int indexLine = 2;
                    byte checkSum = 0;

                    byte lineNum = Convert.ToByte(line.Substring(indexLine, 2), 16);
                    checkSum += lineNum;

                    S19Line tmpS19 = new S19Line((byte)(lineNum - addressLen - 1));
                    indexLine += 2;

                    for (int n = 0; n < addressLen; n++)
                    {
                        int indexTmp = 4 + n - addressLen;
                        tmpS19.lineAddress[indexTmp] = Convert.ToByte(line.Substring(indexLine, 2), 16);
                        tmpS19.lineAddressAll += (uint)(tmpS19.lineAddress[indexTmp] * (Math.Pow(0x100, addressLen - n - 1)));
                        indexLine += 2;
                        checkSum += tmpS19.lineAddress[indexTmp];
                    }

                    for (int j = 0; j < tmpS19.Data.Length; j++)
                    {
                        tmpS19.Data[j] = Convert.ToByte(line.Substring(indexLine, 2), 16);
                        checkSum += tmpS19.Data[j];
                        indexLine += 2;
                    }

                    if ((0xff - checkSum) != Convert.ToByte(line.Substring(indexLine, 2), 16))
                    {
                        return null;
                    }
                    s19LineTmp.AddLast(tmpS19);
                }
            }
            return s19LineTmp;
        }

        public void lineToBlock()
        {
            List<S19Block> s19BlockTmp = new List<S19Block>();
            List<byte> s19BlockData = new List<byte>();

            int currentBlockIndex = 0;
            uint currentBlockLength = 0;
            byte[] currentBlockLengthByte = new byte[4];

            if (s19Line == null)
            {
                return;
            }

            for (int lineNum = 0; lineNum < s19Line.Length; lineNum++)
            {
                foreach (byte tmp in s19Line[lineNum].Data)
                {
                    s19BlockData.Add(tmp);
                }

                currentBlockLength += (uint)s19Line[lineNum].Data.Length;

                if (lineNum + 1 >= s19Line.Length || s19Line[lineNum].lineAddressAll + s19Line[lineNum].Data.Length != s19Line[lineNum + 1].lineAddressAll)
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
                    currentBlockIndex = lineNum + 1;
                    s19BlockData = new List<byte>();
                    currentBlockLength = 0;
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
        public byte[] lineAddress = { 0, 0, 0, 0 };
        public uint lineAddressAll;
        public byte[] Data;

        public S19Line(byte num)
        {
            Data = new byte[num];
        }

        public S19Line(byte[] address, byte[] data)
        {
            lineAddress = address;
            Data = data;
        }
    }

    public class S19Block
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
