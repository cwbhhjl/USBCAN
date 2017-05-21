using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BtFlash.Device.ZLG
{
    /// <summary>
    /// ZLG设备类型
    /// </summary>
    public enum HardwareType : uint
    {
        VCI_PCI5121 = 1,
        VCI_PCI9810 = 2,
        VCI_USBCAN1 = 3,
        VCI_USBCAN2 = 4,
        VCI_PCI9820 = 5,
        VCI_CAN232 = 6,
        VCI_PCI5110 = 7,
        VCI_CANLITE = 8,
        VCI_ISA9620 = 9,
        VCI_ISA5420 = 10,
        VCI_PC104CAN = 11,
        VCI_CANETUDP = 12,
        VCI_DNP9810 = 13,
        VCI_PCI9840 = 14,
        VCI_PC104CAN2 = 15,
        VCI_PCI9820I = 16,
        VCI_CANETTCP = 17,
        VCI_PEC9920 = 18,
        VCI_PCI5010U = 19,
        VCI_USBCAN_E_U = 20,
        VCI_USBCAN_2E_U = 21,
        VCI_PCI5020U = 22,
        VCI_EG20T_CAN = 23,
        VCI_PCIE9221 = 24
    }

    /// <summary>
    /// USBCAN非托管库函数
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_OpenDevice(uint DeviceType, uint DeviceInd, uint Reserved);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_CloseDevice(uint DeviceType, uint DeviceInd);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_InitCAN(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_ReadBoardInfo(uint DeviceType, uint DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_ReadErrInfo(uint DeviceType, uint DeviceInd, int CANInd, ref VCI_ERR_INFO pErrInfo);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_ReadCANStatus(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_CAN_STATUS pCANStatus);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_GetReference(uint DeviceType, uint DeviceInd, uint CANInd, uint RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_SetReference(uint DeviceType, uint DeviceInd, uint CANInd, uint RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_GetReceiveNum(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_ClearBuffer(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_StartCAN(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_ResetCAN(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        internal static extern uint VCI_Transmit(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_CAN_OBJ pSend, uint Len);
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        internal static extern uint VCI_Receive(uint DeviceType, uint DeviceInd, uint CANInd, IntPtr pReceive, uint Len, int WaitTime);
    }

    public class ZlgCan
    {
        private ZlgDevice usbcan;
        private VCI_INIT_CONFIG initConfig;
        private VCI_ERR_INFO err;

        public uint Index { get; }

        public uint ReceiveNum
        {
            get { return NativeMethods.VCI_GetReceiveNum((uint)usbcan.DeviceType, usbcan.DeviceIndex, Index); }
        }

        public VCI_INIT_CONFIG Config { get { return initConfig; } }

        internal ZlgCan(ZlgDevice usbcan, uint index)
        {
            this.usbcan = usbcan;
            Index = index;
            initConfig = new VCI_INIT_CONFIG();
            err = new VCI_ERR_INFO();
        }

        public bool InitCan(byte timing0, byte timing1, uint accCode = 0, uint accMask = 0xFFFFFFFF, byte filter = 1, byte mode = 0)
        {
            initConfig.AccCode = accCode;
            initConfig.AccMask = accMask;
            initConfig.Timing0 = timing0;
            initConfig.Timing1 = timing1;
            initConfig.Filter = filter;
            initConfig.Mode = mode;
            bool result = (NativeMethods.VCI_InitCAN((uint)usbcan.DeviceType, usbcan.DeviceIndex, Index, ref initConfig) == 1);
            return result;
        }

        public bool ReadErrorInfo(out uint errCode)
        {
            bool result = (NativeMethods.VCI_ReadErrInfo((uint)usbcan.DeviceType, usbcan.DeviceIndex, (int)Index, ref err) == 1);
            errCode = err.ErrCode;
            return result;
        }

        private bool ReadStatus(ref VCI_CAN_STATUS canStatus)
        {
            return (NativeMethods.VCI_ReadCANStatus((uint)usbcan.DeviceType, usbcan.DeviceIndex, Index, ref canStatus) == 1);
        }

        public bool ClearBuffer()
        {
            return (NativeMethods.VCI_ClearBuffer((uint)usbcan.DeviceType, usbcan.DeviceIndex, Index) == 1);
        }

        public bool Start()
        {
            return (NativeMethods.VCI_StartCAN((uint)usbcan.DeviceType, usbcan.DeviceIndex, Index) == 1);
        }

        public bool Reset()
        {
            return (NativeMethods.VCI_ResetCAN((uint)usbcan.DeviceType, usbcan.DeviceIndex, Index) == 1);
        }

        public bool Send(ref VCI_CAN_OBJ send)
        {
            return (NativeMethods.VCI_Transmit((uint)usbcan.DeviceType, usbcan.DeviceIndex, Index, ref send, 1) == 1);
        }

        public IEnumerable<VCI_CAN_OBJ> Receive(int size = 50, int waitTime = 100)
        {
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * size);
            List<VCI_CAN_OBJ> list = new List<VCI_CAN_OBJ>();
            uint num = NativeMethods.VCI_Receive((uint)usbcan.DeviceType, usbcan.DeviceIndex, Index, pt, 50, waitTime);

            for (int i = 0; i < num; i++)
            {
                list.Add((VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((uint)pt + (num - 1) * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ)));
            }

            Marshal.FreeHGlobal(pt);
            return list;
        }
    }

    public class ZlgDevice
    {
        /// <summary>
        /// 错误码定义
        /// </summary>
        private static Dictionary<uint, string> ErrDic = new Dictionary<uint, string>()
        {
            {0x00000001, "CAN控制器内部FIFO溢出" },
            {0x00000002, "CAN控制器错误报警" },
            {0x00000004, "CAN控制器消极错误" },
            {0x00000008, "CAN控制器仲裁丢失" },
            {0x00000010, "CAN控制器总线错误" },
            {0x00000020, "CAN控制器总线关闭" },
            {0x00000100, "设备已经打开" },
            {0x00000200, "打开设备错误" },
            {0x00000400, "设备没有打开" },
            {0x00000800, "缓冲区溢出" },
            {0x00001000, "此设备不存在" },
            {0x00002000, "装载动态库失败" },
            {0x00004000, "执行命令失败" },
            {0x00008000, "内存不足" },
        };

        private VCI_ERR_INFO err = new VCI_ERR_INFO();

        private bool opened = false;

        public HardwareType DeviceType { get; }

        public uint DeviceIndex { get; }

        private Dictionary<uint, ZlgCan> CanList { get; }

        private bool HasOpen()
        {
            if (!opened)
            {
                return false;
            }
            else
            {
                VCI_BOARD_INFO info = new VCI_BOARD_INFO();
                if (ReadBoardInfo(ref info))
                {
                    return true;
                }
                else
                {
                    CloseDevice();
                    return false;
                }
            }
        }

        public string GetError(uint code)
        {
            return ErrDic[code];
        }

        public ZlgDevice(HardwareType deviceType, uint deviceIndex)
        {
            DeviceType = deviceType;
            DeviceIndex = deviceIndex;
            CanList = new Dictionary<uint, ZlgCan>();
        }

        public bool OpenDevice()
        {
            bool result = (NativeMethods.VCI_OpenDevice((uint)DeviceType, DeviceIndex, 0u) == 1);
            if (result)
            {
                opened = true;
            }
            return result;
        }

        public bool CloseDevice()
        {
            bool result = (NativeMethods.VCI_CloseDevice((uint)DeviceType, DeviceIndex) == 1);
            if (result)
            {
                opened = false;
            }
            return result;
        }

        public ZlgCan AddCan(uint index)
        {
            if (!CanList.ContainsKey(index))
            {
                CanList.Add(index, new ZlgCan(this, index));
            }

            return CanList[index];
        }

        public ZlgCan GetCan(uint index)
        {
            if (!CanList.ContainsKey(index))
            {
                return null;
            }
            return CanList[index];
        }

        public bool ReadBoardInfo(ref VCI_BOARD_INFO info)
        {
            uint result = NativeMethods.VCI_ReadBoardInfo((uint)DeviceType, DeviceIndex, ref info);
            return result == 1;
        }

        public bool ReadErrorInfo(out uint errCode)
        {
            bool result = (NativeMethods.VCI_ReadErrInfo((uint)DeviceType, DeviceIndex, -1, ref err) == 1);
            errCode = err.ErrCode;
            return result;
        }
    }
}
