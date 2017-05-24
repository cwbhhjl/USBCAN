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

        private Queue<List<byte>> MultiFrames { get; }
        /// <summary>
        /// 获取原始数据的所有 CAN 帧队列
        /// </summary>
        public Queue<List<byte>> AllFrames { get; }

        /// <summary>
        /// 剩余的CAN帧数
        /// </summary>
        public int Count { get { return MultiFrames.Count; } }

        /// <summary>
        /// 获取下一个 CAN 帧
        /// </summary>
        /// <returns>CAN帧字节数组</returns>
        public List<byte> NextFrame()
        {
            return MultiFrames?.Dequeue();
        }

        /// <summary>
        /// 初始化 CanUDSFrame 实例，将传入的原始数据字节数组转为符合 ISO 15363 的 CAN 帧字节数组队列
        /// </summary>
        public UDSFrame(uint id, ICollection<byte> data)
        {
            ID = id;
            MultiFrames = ToCanContent(data);
            AllFrames = new Queue<List<byte>>(MultiFrames);
        }

        private List<byte> ToSingleFrame(ICollection<byte> data)
        {
            if (data.Count > 7)
            {
                throw new ArgumentOutOfRangeException();
            }

            List<byte> frame = data.ToList();
            frame.Insert(0, (byte)frame.Count);

            for (int i = frame.Count; i < 8; i++)
            {
                frame.Add(0xFF);
            }

            return frame;
        }

        private Queue<List<byte>> ToMultiFrames(ICollection<byte> data)
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
            frame.Insert(0, (byte)(count & 0xFF));
            frame.Insert(0, (byte)(((N_PCI.FF.N_PCItype << 4) & 0xF0) | (count >> 8) & 0x0F));
            Queue<List<byte>> frames = new Queue<List<byte>>();
            frames.Enqueue(frame);

            for (int i = 0, SN = 0; i * 7 + 6 < count; i++, SN++)
            {
                List<byte> cList = new List<byte>
                {
                    (byte)((SN + 1) & 0x0F | (N_PCI.CF.N_PCItype << 4))
                };
                cList.AddRange(data.Take(i * 7 + 13).Skip(i * 7 + 6));
                frames.Enqueue(cList);
            }
            var last = frames.Last();
            last.AddRange(Enumerable.Repeat<byte>(0xFF, 8 - last.Count));

            return frames;
        }

        private Queue<List<byte>> ToCanContent(ICollection<byte> data)
        {
            if (data.Count <= 7)
            {
                Queue<List<byte>> tmp = new Queue<List<byte>>();
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
