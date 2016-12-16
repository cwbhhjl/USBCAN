using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.UDS
{
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
            if (multiFrames.Count == 0)
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
