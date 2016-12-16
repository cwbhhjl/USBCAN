using System;
using System.Collections.Generic;

namespace USBCAN.UDS
{
    /// <summary>
    /// Negative response codes
    /// </summary>
    public struct NRC
    {
        /// <summary>
        /// serviceNotSupported
        /// </summary>
        public const byte SNS = 0x11;
        /// <summary>
        /// subFunctionNotSupported
        /// </summary>
        public const byte SFNS = 0x12;
        /// <summary>
        /// incorrectMessageLengthOrInvalidFormat
        /// </summary>
        public const byte IMLOIF = 0x13;
        /// <summary>
        /// responseTooLong
        /// </summary>
        public const byte RTL = 0x14;
        /// <summary>
        /// busyRepeatRequest
        /// </summary>
        public const byte BRR = 0x21;
        /// <summary>
        /// conditionsNotCorrect
        /// </summary>
        public const byte CNC = 0x22;
        /// <summary>
        /// requestSequenceError
        /// </summary>
        public const byte RSE = 0x24;
        /// <summary>
        /// invalidKey
        /// </summary>
        public const byte IK = 0x35;
        /// <summary>
        /// requiredTimeDelayNotExpired
        /// </summary>
        public const byte RTDNE = 0x37;
        /// <summary>
        /// generalProgrammingFailure
        /// </summary>
        public const byte GPF = 0x72;
        /// <summary>
        /// wrongBlockSequenceCounter
        /// </summary>
        public const byte WBSC = 0x73;
        /// <summary>
        /// requestCorrectlyReceived-ResponsePending
        /// </summary>
        public const byte RCRRP = 0x78;
    }

    /// <summary>
    /// Service identifier
    /// </summary>
    public struct SI
    {
        /// <summary>
        /// Negative response service identifier
        /// </summary>
        public const byte NRSI = 0x7F;
        /// <summary>
        /// ECU reset service identifier
        /// </summary>
        public const byte ERSI = 0x11;
        /// <summary>
        /// Read data by identifier service identifier
        /// </summary>
        public const byte RDBISI = 0x22;
        /// <summary>
        /// Security access service identifier
        /// </summary>
        public const byte SASI = 0x27;
        /// <summary>
        /// Communication control service identifier
        /// </summary>
        public const byte CCSI = 0x28;
        /// <summary>
        /// Request download service identifier
        /// </summary>
        public const byte RDSI = 0x34;
        /// <summary>
        /// Transfer data service identifier
        /// </summary>
        public const byte TDSI = 0x36;
        /// <summary>
        /// Request transfer exit service identifier
        /// </summary>
        public const byte RTESI = 0x37;
        /// <summary>
        /// Tester present request service identifier 
        /// </summary>
        public const byte TPSI = 0x3E;
    }

    public struct N_PCI
    {
        public struct SF
        {
            public const byte N_PCItype = 0;
        }

        public struct FF
        {
            public const byte N_PCItype = 1;
        }

        public struct CF
        {
            public const byte N_PCItype = 2;
        }

        public struct FC
        {
            public const byte N_PCItype = 3;

            public struct FS
            {
                public const byte CTS = 0x0;
                public const byte WT = 0x1;
                public const byte OVFLW = 0x02;
            }
        }
        
    }

    public class CanUDSFrame
    {
        //public int length { get; }
        private Queue<byte[]> multiFrames { get; }
        /// <summary>
        /// 获取原始数据的所有 CAN 帧队列
        /// </summary>
        public Queue<byte[]> allFrames { get; }

        /// <summary>
        /// 剩余的CAN帧数
        /// </summary>
        public int framesCount
        {
            get
            {
                return multiFrames.Count;
            }
        }

        /// <summary>
        /// 获取下一个 CAN 帧
        /// </summary>
        /// <returns>CAN帧字节数组</returns>
        public byte[] nextFrame()
        {
            if(multiFrames.Count == 0)
            {
                return null;
            }
            return multiFrames.Dequeue();
        }

        /// <summary>
        /// 初始化 CanUDSFrame 实例，将传入的原始数据字节数组转为符合 ISO 15363 的 CAN 帧字节数组队列
        /// </summary>
        public CanUDSFrame(byte[] data)
        {
            //length = data.Length;
            multiFrames = dataArrayToCanContent(data);
            allFrames = new Queue<byte[]>(multiFrames);
        }

        private byte[] makeSingleFrame(byte[] data)
        {
            byte[] tmp = new byte[8];
            byte len = (byte)data.Length;
            tmp[0] = len;
            for (int i = 1; i < 8; i++)
            {
                tmp[i] = (i - 1) < len ? data[i - 1] : (byte)0xFF;
            }
            return tmp;
        }

        private byte[] makeFirstFrame(byte[] data)
        {
            byte[] tmp = new byte[8];
            int len = data.Length;
            tmp[0] = (byte)(((N_PCI.FF.N_PCItype << 4) & 0xF0) | (len >> 8) & 0x0F);
            tmp[1] = (byte)(len & 0xFF);
            for (int n = 0; n < 6; n++)
            {
                tmp[n + 2] = data[n];
            }
            return tmp;
        }

        private byte[] makeConsecutiveFrame(byte[]data ,int index)
        {
            return null;
        }

        private Queue<byte[]>dataArrayToCanContent(byte[] data)
        {
            Queue<byte[]> tmpQ = new Queue<byte[]>();
            int len = data.Length;
            if (len <= 7)
            {
                tmpQ.Enqueue(makeSingleFrame(data));
                return tmpQ;
            }
            else
            {
                if (len > 0xFFF)
                {
                    Exception e = new Exception("too long");
                    throw e;
                }
                    
                int framesNum = calcFramesNum(len);
                //byte[][] tmp = new byte[framesNum][];

                tmpQ.Enqueue(makeFirstFrame(data));

                int index = 6;
                byte SN = 0;
                for (int i = 1; i < framesNum; i++)
                {
                    byte[] tmp = new byte[8];
                    SN++;
                    if (SN > 0x0F)
                    {
                        SN = 0;
                    }
                    tmp[0] = (byte)(((N_PCI.CF.N_PCItype << 4) & 0xF0) | SN);
                    for (int j = 1; j < tmp.Length; j++)
                    {
                        tmp[j] = index < len ? data[index] : (byte)0xFF;
                        index++;
                    }
                    
                    tmpQ.Enqueue(tmp);
                }
                return tmpQ;
            }
        }

        private int calcFramesNum(int length)
        {
            if (length <= 7)
            {
                return 1;
            }
            else
            {
                length -= 6;
                return (int)Math.Ceiling(((double)length) / 7) + 1;
            }
        }
    }
}
