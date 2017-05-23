using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BtFlash.UDS
{
    public class UDSFrame
    {
        public static byte[] FlowControl = new byte[8] { 0x30, 0x00, 0x01, 0x55, 0x55, 0x55, 0x55, 0x55 };

        public uint ID { get; }

        private Queue<IList<byte>> MultiFrames { get; }
        /// <summary>
        /// 获取原始数据的所有 CAN 帧队列
        /// </summary>
        public Queue<IList<byte>> AllFrames { get; }

        /// <summary>
        /// 剩余的CAN帧数
        /// </summary>
        public int Count
        {
            get
            {
                return MultiFrames.Count;
            }
        }

        /// <summary>
        /// 获取下一个 CAN 帧
        /// </summary>
        /// <returns>CAN帧字节数组</returns>
        public IList<byte> NextFrame()
        {
            return MultiFrames?.Dequeue();
        }

        /// <summary>
        /// 初始化 CanUDSFrame 实例，将传入的原始数据字节数组转为符合 ISO 15363 的 CAN 帧字节数组队列
        /// </summary>
        public UDSFrame(uint id, IList<byte> data)
        {
            ID = id;
            MultiFrames = ToCanContent(data);
            AllFrames = new Queue<IList<byte>>(MultiFrames);
        }

        private List<byte> ToSingleFrame(IList<byte> data)
        {
            if (data.Count > 7)
            {
                throw new ArgumentOutOfRangeException();
            }

            List<byte> frame = data.ToList();
            frame.Insert(0, (byte)data.Count);

            for (int i = data.Count; i < 8; i++)
            {
                data.Add(0xFF);
            }

            return frame;
        }

        private Queue<IList<byte>> ToMultiFrames(IList<byte> data)
        {
            int count = data.Count;
            if (count < 8)
            {
                throw new ArgumentException();
            }
            else if (count > 0xFFF)
            {
                throw new ArgumentOutOfRangeException();
            }

            List<byte> frame = data.Take(6).ToList();
            frame.Insert(0, (byte)(data.Count & 0xFF));
            frame.Insert(0, (byte)(((N_PCI.FF.N_PCItype << 4) & 0xF0) | (data.Count >> 8) & 0x0F));

            Queue<IList<byte>> frames = new Queue<IList<byte>>();
            frames.Enqueue(frame);
            //data. http://bbs.csdn.net/topics/390836815
            int remainder;
            int framesNum = CalcFramesNum(count, out remainder);
            int index = 6;
            byte SN = 0;
            for (int i = 1; i < framesNum; i++)
            {
                byte[] tmpp = new byte[8];
                if ((++SN) > 0x0F)
                {
                    SN = 0;
                }
                tmpp[0] = (byte)(((N_PCI.CF.N_PCItype << 4) & 0xF0) | SN);
                if (i < framesNum - 1 || remainder == 0)
                {
                    for (int j = 1; j < 8; j++, index++)
                    {
                        tmpp[j] = data[index];
                    }
                }
                else
                {
                    tmpp = Enumerable.Repeat((byte)0xFF, 8).ToArray();
                    tmpp[0] = (byte)(((N_PCI.CF.N_PCItype << 4) & 0xF0) | SN);
                    Array.Copy(data.ToArray(), index, tmpp, 1, remainder);
                }
                frames.Enqueue(tmpp);
            }

            return frames;
        }

        private byte[] makeConsecutiveFrame(byte[] data, int index)
        {
            return null;
        }

        private Queue<IList<byte>> ToCanContent(IList<byte> data)
        {
            if (data.Count <= 7)
            {
                Queue<IList<byte>> tmp = new Queue<IList<byte>>();
                tmp.Enqueue(ToSingleFrame(data));
                return tmp;
            }

            return ToMultiFrames(data);
        }

        private int CalcFramesNum(int length, out int remainder)
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
