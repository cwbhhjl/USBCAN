using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace USBCAN
{
    /// <summary>
    /// ZLGCAN系列接口卡信息的数据类型
    /// </summary>
    public struct VCI_BOARD_INFO
    {
        public ushort hw_Version;
        public ushort fw_Version;
        public ushort dr_Version;
        public ushort in_Version;
        public ushort irq_Num;
        public byte can_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] str_Serial_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] str_hw_Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;
    }

    /// <summary>
    /// 定义CAN信息帧的数据类型
    /// </summary>
    unsafe public struct VCI_CAN_OBJ
    {
        public uint ID;
        public uint TimeStamp;
        public byte TimeFlag;
        public byte SendType;
        public byte RemoteFlag;
        public byte ExternFlag;
        public byte DataLen;
        public fixed byte Data[8];
        public fixed byte Reserved[3];
    }

    /// <summary>
    /// 定义CAN控制器状态的数据类型
    /// </summary>
    public struct VCI_CAN_STATUS
    {
        public byte ErrInterrupt;
        public byte regMode;
        public byte regStatus;
        public byte regALCapture;
        public byte regECCapture;
        public byte regEWLimit;
        public byte regRECounter;
        public byte regTECounter;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
    }

    /// <summary>
    /// 定义错误信息的数据类型
    /// </summary>
    public struct VCI_ERR_INFO
    {
        public uint ErrCode;
        public byte Passive_ErrData1;
        public byte Passive_ErrData2;
        public byte Passive_ErrData3;
        public byte ArLost_ErrData;
    }

    /// <summary>
    /// 定义初始化CAN的数据类型
    /// </summary>
    public struct VCI_INIT_CONFIG
    {
        public uint AccCode;
        public uint AccMask;
        public uint Reserved;
        public byte Filter;
        public byte Timing0;
        public byte Timing1;
        public byte Mode;
    }

    public class USBCAN
    {
        private static class NativeMethods
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
            internal static extern uint VCI_ReadErrInfo(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_ERR_INFO pErrInfo);
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

        public class Can
        {
            private USBCAN usbcan;

            public uint canIndex { get; }
            public bool isInit { get; private set; }
            public uint ReceiveNum
            {
                get
                {
                    return NativeMethods.VCI_GetReceiveNum((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex);
                }
            }

            internal Can(USBCAN usbcan, uint canIndex)
            {
                this.canIndex = canIndex;
                this.usbcan = usbcan;
                isInit = false;
            }

            public bool InitCan(ref VCI_INIT_CONFIG initConfig)
            {
                bool result = (NativeMethods.VCI_InitCAN((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex, ref initConfig) == 1);
                if (result)
                {
                    isInit = true;
                }
                return result;
            }

            public bool ReadErrInfo(ref VCI_ERR_INFO errInfo)
            {
                return (NativeMethods.VCI_ReadErrInfo((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex, ref errInfo) == 1);
            }

            public bool ReadCANStatus(ref VCI_CAN_STATUS canStatus)
            {
                return (NativeMethods.VCI_ReadCANStatus((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex, ref canStatus) == 1);
            }

            public bool ClearBuffer()
            {
                return (NativeMethods.VCI_ClearBuffer((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex) == 1);
            }

            public bool StartCan()
            {
                return (NativeMethods.VCI_StartCAN((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex) == 1);
            }

            public bool ResetCan()
            {
                return (NativeMethods.VCI_ResetCAN((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex) == 1);
            }

            public bool Send(ref VCI_CAN_OBJ send)
            {
                return (NativeMethods.VCI_Transmit((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex, ref send, 1) == 1);
            }

            public uint Receive(out IntPtr pt, int waitTime)
            {
                pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * 50);
                return NativeMethods.VCI_Receive((uint)usbcan.deviceType, usbcan.deviceIndex, canIndex, pt, 50, 100);
            }
        }
        /// <summary>
        /// 错误码定义
        /// </summary>
        public static Dictionary<uint, string> ERR = new Dictionary<uint, string>()
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

        public HardwareType deviceType { get; }
        public uint deviceIndex { get; }
        public Dictionary<uint, Can> canList { get; }
        public bool isOpen { get; private set; }

        public VCI_BOARD_INFO info = new VCI_BOARD_INFO();

        public USBCAN(HardwareType deviceType, uint deviceIndex)
        {
            this.deviceType = deviceType;
            this.deviceIndex = deviceIndex;
            isOpen = false;
            canList = new Dictionary<uint, Can>();
        }

        public bool OpenDevice()
        {
            bool result = (NativeMethods.VCI_OpenDevice((uint)deviceType, deviceIndex, 0u) == 1);
            if (result)
            {
                isOpen = true;
            }
            return result;
        }

        public bool CloseDevice()
        {
            bool result = (NativeMethods.VCI_CloseDevice((uint)deviceType, deviceIndex) == 1);
            if (result)
            {
                isOpen = false;
            }
            return result;
        }
        
        public void AddCan(uint canIndex)
        {
            canList.Add(canIndex, new Can(this, canIndex));
        }

        public bool ReadBoardInfo()
        {
            uint result = NativeMethods.VCI_ReadBoardInfo((uint)deviceType, deviceIndex, ref info);
            return result == 1;
        }
    }

}
