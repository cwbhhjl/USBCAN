using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

namespace USBCAN
{
    public class CanControl
    {
        public const int VCI_PCI5121 = 1;
        public const int VCI_PCI9810 = 2;
        public const int VCI_USBCAN1 = 3;
        public const int VCI_USBCAN2 = 4;
        public const int VCI_USBCAN2A = 4;
        public const int VCI_PCI9820 = 5;
        public const int VCI_CAN232 = 6;
        public const int VCI_PCI5110 = 7;
        public const int VCI_CANLITE = 8;
        public const int VCI_ISA9620 = 9;
        public const int VCI_ISA5420 = 10;
        public const int VCI_PC104CAN = 11;
        public const int VCI_CANETUDP = 12;
        public const int VCI_CANETE = 12;
        public const int VCI_DNP9810 = 13;
        public const int VCI_PCI9840 = 14;
        public const int VCI_PC104CAN2 = 15;
        public const int VCI_PCI9820I = 16;
        public const int VCI_CANETTCP = 17;
        public const int VCI_PEC9920 = 18;
        public const int VCI_PCI5010U = 19;
        public const int VCI_USBCAN_E_U = 20;
        public const int VCI_USBCAN_2E_U = 21;
        public const int VCI_PCI5020U = 22;
        public const int VCI_EG20T_CAN = 23;
        public const int VCI_PCIE9221 = 24;

        [DllImport("controlcan.dll")]
        private static extern uint VCI_OpenDevice(uint DeviceType, uint DeviceInd, uint Reserved);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_CloseDevice(uint DeviceType, uint DeviceInd);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_InitCAN(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_ReadBoardInfo(uint DeviceType, uint DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_ReadErrInfo(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_ERR_INFO pErrInfo);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_ReadCANStatus(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_CAN_STATUS pCANStatus);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_GetReference(uint DeviceType, uint DeviceInd, uint CANInd, uint RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_SetReference(uint DeviceType, uint DeviceInd, uint CANInd, uint RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_GetReceiveNum(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_ClearBuffer(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_StartCAN(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_ResetCAN(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        private static extern uint VCI_Transmit(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_CAN_OBJ pSend, uint Len);
        //[DllImport("controlcan.dll")]
        //static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        private static extern uint VCI_Receive(uint DeviceType, uint DeviceInd, uint CANInd, IntPtr pReceive, uint Len, int WaitTime);

        public static uint deviceType = VCI_USBCAN2;
        private static bool isOpen = false;
        public static uint deviceIndex = 0;
        public static uint canIndex = 0;

        private static byte[] rev = new byte[8];

        public static VCI_CAN_OBJ obj = new VCI_CAN_OBJ();

        private static CanControl canCtl;

        public static VCI_ERR_INFO errorInfo = new VCI_ERR_INFO();

        public static uint res = 0;

        public static byte[] Rev
        {
            get
            {
                return rev;
            }
        }

        //public static CanLog canLog = new CanLog();

        private CanControl() { }

        public static CanControl getCanControl()
        {
            if (canCtl == null)
            {
                canCtl = new CanControl();
            }

            return canCtl;
        }

        unsafe public static bool canConnect()
        {
            if (!isOpen)
            {
                if (VCI_OpenDevice(deviceType, deviceIndex, 0) == 0)
                {
                    VCI_ReadErrInfo(deviceType, deviceIndex, canIndex, ref errorInfo);
                    return false;
                }

                isOpen = true;
                VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();

                config.AccCode = 0;
                config.AccMask = 0xFFFFFFFF;
                config.Timing0 = 0;
                config.Timing1 = 28;
                config.Filter = 1;
                config.Mode = 0;

                VCI_InitCAN(deviceType, deviceIndex, canIndex, ref config);
                VCI_StartCAN(deviceType, deviceIndex, canIndex);

                obj.SendType = 0;
                obj.RemoteFlag = 0;
                obj.ExternFlag = 0;
                obj.DataLen = 8;

                fixed (byte* pData = obj.Data)
                {
                    for (int n = 0; n < 8; n++)
                    {
                        pData[n] = 0xFF;
                    }
                }
            }

            return isOpen;
        }

        unsafe public static int sendFrame(string canID, string strData)
        {
            return sendFrame(Convert.ToUInt32(canID, 16), canStringToByte(strData));
        }

        unsafe public static int sendFrame(uint canID, byte[] data)
        {
            if (!isOpen)
            {
                return -1;
            }

            obj.ID = canID;

            fixed (byte* pData = obj.Data)
            {
                for (int n = 0; n < data.Length; n++)
                {
                    pData[n] = data[n];
                }
            }

            return (int)VCI_Transmit(deviceType, deviceIndex, canIndex, ref obj, 1);
        }

        unsafe public static int sendFrame(uint canID, uint receiveID, byte[] data)
        {
            if (!isOpen)
            {
                return -1;
            }

            int len = data.Length;
            obj.ID = canID;

            if (len <= 7)
            {
                fixed (byte* pData = obj.Data)
                {
                    pData[0] = (byte)len;
                    for (int n = 0; n < 7; n++)
                    {
                        pData[n + 1] = n < len ? data[n] : (byte)0xFF;
                    }
                }
            }
            else
            {
                if (len > 0xFFF)
                {
                    return -3;
                }

                fixed (byte* pData = obj.Data)
                {
                    pData[0] = (byte)(((N_PCI.FF.N_PCItype << 4) & 0xF0) | (len >> 8) & 0x0F);
                    pData[1] = (byte)(len & 0xFF);

                    for (int n = 0; n < 6; n++)
                    {
                        pData[n + 2] = data[n];
                    }
                }
            }

            if (VCI_Transmit(deviceType, deviceIndex, canIndex, ref obj, 1) != 1)
            {
                return -5;
            }

            if (!waitForResponse(receiveID))
            {
                return -2;
            }
            else
            {
                if (len <= 7)
                {
                    return 1;
                }
            }

            int index = 6;
            int dataCount = (int)Math.Ceiling((len - 6) / 7.0);
            byte BS, STmin;
            byte SN = 1;

        handleFlowControl:
            switch (rev[0])
            {
                case (N_PCI.FC.N_PCItype << 4) | N_PCI.FC.FS.CTS:

                    BS = rev[1];
                    STmin = rev[2];

                    for (byte j = 0; j < BS; j++)
                    {
                        fixed (byte* pData = obj.Data)
                        {
                            pData[0] = (byte)(0x20 | SN);
                            for (int n = 0; n < 7; n++)
                            {
                                pData[n + 1] = index < len ? data[index] : (byte)0xFF;
                                index++;
                            }
                        }
                        VCI_Transmit(deviceType, deviceIndex, canIndex, ref obj, 1);

                        if (index >= len)
                        {
                            break;
                        }

                        SN++;
                        SN = SN > 0x0F ? (byte)0 : SN;
                    }

                    Flash.Delay(STmin);
                    goto case (N_PCI.FC.N_PCItype << 4) | N_PCI.FC.FS.WT;

                case (N_PCI.FC.N_PCItype << 4) | N_PCI.FC.FS.WT:
                    if (!waitForResponse(receiveID))
                    {
                        return -2;
                    }
                    goto handleFlowControl;

                case (N_PCI.FC.N_PCItype << 4) | N_PCI.FC.FS.OVFLW:
                    return -4;

                default:
                    return 2;
            }
        }

        private static bool waitForResponse(uint receiveID)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < 75)
            {
                if (VCI_GetReceiveNum(deviceType, deviceIndex, canIndex) > 0)
                {
                    canLastReceive(receiveID);
                    return true;
                }
            }
            return false;
        }

        unsafe public static byte[] canLastReceive(uint canId)
        {
            res = VCI_GetReceiveNum(deviceType, deviceIndex, canIndex);

            if (res == 0)
            {
                return null;
            }

            VCI_CAN_OBJ objTmp;
            List<VCI_CAN_OBJ> canObj = new List<VCI_CAN_OBJ>();

            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * 50);
            res = VCI_Receive(deviceType, deviceIndex, canIndex, pt, 50, 100);

            for (int i = 0; i < res; i++)
            {
                objTmp = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((uint)pt + (res - 1) * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                if (objTmp.ID != canId)
                {
                    continue;
                }
                canObj.Add(objTmp);
            }

            //canLog.recordLog(obj);
            objTmp = canObj[canObj.Count - 1];

            for (int j = 0; j < 8; j++)
            {
                rev[j] = objTmp.Data[j];
            }

            res = 0;
            Marshal.FreeHGlobal(pt);
            return rev;
        }

        static void canReset()
        {
            if (!isOpen)
            {
                return;
            }
            VCI_ResetCAN(deviceType, deviceIndex, canIndex);
        }

        public static void canClose()
        {
            if (isOpen)
            {
                VCI_CloseDevice(deviceType, deviceIndex);
                isOpen = false;
            }
        }

        public static byte[] canStringToByte(string str)
        {
            string[] strTmp = str.Split(' ');
            List<byte> byteTmp = new List<byte>();
            foreach (string i in strTmp)
            {
                byteTmp.Add(Convert.ToByte(i, 16));
            }
            return byteTmp.ToArray();
        }
    }
}
