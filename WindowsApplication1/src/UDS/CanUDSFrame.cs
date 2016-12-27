using System;
using System.Collections.Generic;
using System.Linq;

namespace USBCAN.UDS
{
    public class CanUDSFrame
    {
        public uint id { get; }
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
            if (multiFrames.Count == 0)
            {
                return null;
            }
            return multiFrames.Dequeue();
        }

        /// <summary>
        /// 初始化 CanUDSFrame 实例，将传入的原始数据字节数组转为符合 ISO 15363 的 CAN 帧字节数组队列
        /// </summary>
        public CanUDSFrame(uint id, byte[] data)
        {
            this.id = id;
            multiFrames = dataArrayToCanContent(data);
            allFrames = new Queue<byte[]>(multiFrames);
        }

        private byte[] makeSingleFrame(byte[] data)
        {
            byte[] singleFrame = Enumerable.Repeat((byte)0xFF, 8).ToArray();
            byte len = (byte)data.Length;
            singleFrame[0] = len;
            Array.Copy(data, 0, singleFrame, 1, len);
            return singleFrame;
        }

        private byte[] makeFirstFrame(byte[] data)
        {
            byte[] firstFrame = new byte[8];
            firstFrame[0] = (byte)(((N_PCI.FF.N_PCItype << 4) & 0xF0) | (data.Length >> 8) & 0x0F);
            firstFrame[1] = (byte)(data.Length & 0xFF);
            Array.Copy(data, 0, firstFrame, 2, 6);
            return firstFrame;
        }

        private byte[] makeConsecutiveFrame(byte[] data, int index)
        {
            return null;
        }

        private Queue<byte[]> dataArrayToCanContent(byte[] data)
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

                int remainder;
                int framesNum = calcFramesNum(len, out remainder);

                tmpQ.Enqueue(makeFirstFrame(data));

                int index = 6;
                byte SN = 0;
                for (int i = 1; i < framesNum; i++)
                {
                    byte[] tmp = new byte[8];
                    if ((++SN) > 0x0F)
                    {
                        SN = 0;
                    }
                    tmp[0] = (byte)(((N_PCI.CF.N_PCItype << 4) & 0xF0) | SN);
                    if (i < framesNum - 1 || remainder == 0)
                    {
                        for (int j = 1; j < tmp.Length; j++, index++)
                        {
                            tmp[j] = data[index];
                        }
                    }
                    else
                    {
                        tmp = Enumerable.Repeat((byte)0xFF, 8).ToArray();
                        tmp[0] = (byte)(((N_PCI.CF.N_PCItype << 4) & 0xF0) | SN);
                        Array.Copy(data, index, tmp, 1, remainder);
                    }
                    tmpQ.Enqueue(tmp);
                }
                return tmpQ;
            }
        }

        private int calcFramesNum(int length, out int remainder)
        {
            if (length <= 7)
            {
                remainder = length;
                return 1;
            }
            else
            {
                length -= 6;
                remainder = length % 7;
                int num = length / 7;
                return remainder > 0 ? num + 2 : num + 1;
            }
        }
    }
}
