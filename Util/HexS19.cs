using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

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
            {"S0", 0 },{"S1", 2 },{"S2", 3 },{"S3", 4 },{"S7", 0 },{"S8", 0 },{"S9", 0 }
        };

        public IEnumerable<S19Block> ConvertS19(string path)
        {
            var lines = ConvertFileToLines(path).Select(l => ConvertDataStrToS19Line(l)).ToArray();

            List<S19Block> s19Blocks = new List<S19Block>();
            List<byte> s19BlockData = new List<byte>();

            int currentBlockLength = 0;
            byte[] currentBlockLengthByte = new byte[4];
            S19Line currentBlockFirstLine = lines[0];

            for (int lineNum = 0; lineNum < lines.Length; lineNum++)
            {
                s19BlockData.AddRange(lines[lineNum].Data);

                checked { currentBlockLength += lines[lineNum].Data.Length; }

                if (lineNum + 1 == lines.Length || lines[lineNum].lineAddressAll + lines[lineNum].Data.Length != lines[lineNum + 1].lineAddressAll)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        currentBlockLengthByte[3 - i] = (byte)((currentBlockLength & (0xff * (uint)Math.Pow(0x100, i))) >> 8 * i);
                    }

                    s19Blocks.Add(new S19Block(currentBlockFirstLine.lineAddress, currentBlockLengthByte, s19BlockData.ToArray()));

                    if (lineNum + 1 == lines.Length)
                    {
                        break; 
                    }
                    currentBlockFirstLine = lines[lineNum + 1];
                    s19BlockData.Clear();
                    currentBlockLength = 0;
                }
            }

            return s19Blocks;
        }

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

        private S19Line ConvertDataStrToS19Line(string line)
        {
            var list = Regex.Matches(line, @"\w\w").Cast<Match>().Select(m => m.Value).ToList();
            byte addressLength = AddressTypeLength[list[0]];

            if (addressLength == 0) return null;

            list.RemoveAt(0);
            var data = list.Select(c => Convert.ToByte(c, 16)).ToList();
            byte rawChecksum = data.Last();
            data.RemoveAt(data.Count - 1);

            if ((0xFF - (byte)data.Sum(d => d)) != rawChecksum) throw new BadChecksumException();

            list.RemoveAt(0);
            return new S19Line(data.GetRange(0, addressLength).ToArray(), data.GetRange(addressLength, data.Count).ToArray());
        }
    }

    class S19Line
    {
        public byte[] lineAddress = { 0, 0, 0, 0 };
        public uint lineAddressAll;
        public byte[] Data;

        public S19Line(byte[] address, byte[] data)
        {
            Array.Copy(address, 0, lineAddress, 4 - address.Length, address.Length);
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