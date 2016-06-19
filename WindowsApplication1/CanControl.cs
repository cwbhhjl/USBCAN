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

public struct CHGDESIPANDPORT
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public byte[] szpwd;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] szdesip;
    public int desport;

    public void Init()
    {
        szpwd = new byte[10];
        szdesip = new byte[20];
    }
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

        public static uint deviceType = 4;//USBCAN2
        private static bool isOpen = false;
        public static uint deviceIndex = 0;
        public static uint canIndex = 0;

        //public static VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[50];
        private static byte[] rev = new byte[8];
        public static byte[] send = new byte[8];
        public static VCI_CAN_OBJ obj = new VCI_CAN_OBJ();
        public static VCI_CAN_OBJ[] objs;
        //public static List<VCI_CAN_OBJ> objs = new List<VCI_CAN_OBJ>();

        private static CanControl canCtl;

        public uint[] m_arrdevtype = new uint[20];
        public VCI_ERR_INFO m_errorInfo;

        public static uint res = 0;

        public static System.Threading.Timer recTimer = null;


        public static bool IsOpen { get; }

        public static byte[] Rev { get; }

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
                    return isOpen;
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

            //recTimer = new System.Threading.Timer(new TimerCallback(recTimer_Tick), null, Timeout.Infinite, Timeout.Infinite);
            //recTimer.Change(20, Timeout.Infinite);
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

            data.CopyTo(send, 0);
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
            byte N_PCI = 0x00;
            byte FS, BS, STmin, SN;

            obj.ID = canID;

            if (len <= 7)
            {
                data.CopyTo(send, 0);
                for (int i = data.Length; i < send.Length; i++)
                {
                    send[i] = 0xFF;
                }

                fixed (byte* pData = obj.Data)
                {
                    pData[0] = (byte)len;
                    for (int n = 0; n < 7; n++)
                    {
                        pData[n + 1] = n < len ? data[n] : (byte)0xFF;
                    }
                }

                int ss = (int)VCI_Transmit(deviceType, deviceIndex, canIndex, ref obj, 1);

                int start = Environment.TickCount;
                while (Math.Abs(Environment.TickCount - start) < 75)
                {
                    if (VCI_GetReceiveNum(deviceType, deviceIndex, canIndex) > 0)
                    {
                        canLastReceive(receiveID);
                        return ss;
                    }
                }
                return -2;
            }
            else
            {
                N_PCI = 0x01;
                fixed (byte* pData = obj.Data)
                {
                    pData[0] = (byte)(((N_PCI << 4) & 0xF0) | ((len / 0x100) & 0x0F));
                    pData[1] = (byte)(len & 0xFF);

                    for (int n = 0; n < 6; n++)
                    {
                        pData[n + 2] = data[n];
                    }
                }

                VCI_Transmit(deviceType, deviceIndex, canIndex, ref obj, 1);

                int start = Environment.TickCount;
                while (Math.Abs(Environment.TickCount - start) < 75)
                {
                    if (VCI_GetReceiveNum(deviceType, deviceIndex, canIndex) > 0)
                    {
                        canLastReceive(receiveID);
                        break;
                    }
                }

                if (rev[0] == 0x30)
                {
                    BS = rev[1];
                    STmin = rev[2];
                    SN = 1;
                    int index = 6;

                    int BlockNums = (int)Math.Ceiling((len + 1) / 6.0) - 1;
                    int dataCount = (int)Math.Ceiling((len - 6) / 7.0);

                    for (int i = 0; i < dataCount;)
                    {
                        for (byte j = 0; j < BS; j++)
                        {
                            fixed (byte* pData = obj.Data)
                            {
                                pData[0] = (byte)(0x20 | SN);
                                for (int n = 0; n < 7; n++)
                                {
                                    pData[n + 1] = n < len - 6 - 7 * i ? data[index] : (byte)0xFF;
                                    index++;
                                }
                            }
                            VCI_Transmit(deviceType, deviceIndex, canIndex, ref obj, 1);
                            i++;
                            if (index >= len)
                            {
                                break;
                            }
                            SN++;
                            SN = SN > 0x0F ? (byte)0 : SN;
                        }
                        Flash.Delay(STmin);

                        start = Environment.TickCount;
                        while (Math.Abs(Environment.TickCount - start) < 75)
                        {
                            if (VCI_GetReceiveNum(deviceType, deviceIndex, canIndex) > 0)
                            {
                                canLastReceive(receiveID);
                                break;
                            }
                        }

                        if (i == BlockNums - 1)
                        {
                            break;
                        }
                        if (rev[0] == 30)
                        {
                            BS = rev[1];
                            STmin = rev[2];
                            continue;
                        }
                    }
                }

                return 0;
            }

        }

        unsafe public void sendFrames(uint canID, byte[] data)
        {
            if (data.Length <= 8)
            {
                sendFrame(canID, data);
                return;
            }
            if (!isOpen)
            {
                return;
            }
            VCI_CAN_OBJ sendobj = new VCI_CAN_OBJ();//sendobj.Init();

            sendobj.SendType = 0;//正常发送：0；自发自收：2
            sendobj.RemoteFlag = 0;
            sendobj.ExternFlag = 0;
            //sendobj[0].ID = Convert.ToUInt32(canID, 16);
            sendobj.ID = canID;
            int len = 8;
            sendobj.DataLen = Convert.ToByte(len);

            sendobj.Data[0] = (byte)(0x10 | ((data.Length / 0x100) & 0xf));
            sendobj.Data[1] = (byte)((data.Length % 0x100) & 0xff);

            int index = 0;
            byte BS, ST, SN = 0;
            byte BSNumber = 0;

            for (; index < 6; index++)
            {
                sendobj.Data[index + 2] = data[index];
            }
            VCI_Transmit(deviceType, deviceIndex, canIndex, ref sendobj, 1);
            if (rev[0] == 0x30)
            {
                BS = rev[1];
                ST = rev[2];

                for (; index < data.Length; index += 7)
                {
                    Flash.Delay(ST);
                    SN += 1;
                    if (SN == 0x10)
                    {
                        SN = 0;
                    }
                    sendobj.Data[0] = (byte)(SN | 0x20);
                    for (int i = 1; i < 8; i++, index++)
                    {
                        sendobj.Data[i] = data[index];
                    }
                    VCI_Transmit(deviceType, deviceIndex, canIndex, ref sendobj, 1);
                    BSNumber += 1;
                    if (BSNumber == BS)
                    {
                        BSNumber = 0;
                    }
                    if (rev[0] == 0x30)
                    {
                        BS = rev[1];
                        ST = rev[2];
                    }
                    else
                    {
                        return;
                    }
                }

            }
        }

        /*
        unsafe public static void recTimer_Tick(object state)
        {
            //StreamWriter log = new StreamWriter(Environment.CurrentDirectory + "Can.log", true);
            res = VCI_GetReceiveNum(deviceType, deviceIndex, canIndex);
            if (res == 0)
            {
                return;
            }
            uint con_maxlen = 50;
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (int)con_maxlen);

            res = VCI_Receive(deviceType, deviceIndex, canIndex, pt, con_maxlen, 100);

            for (uint i = 0; i < res; i++)
            {
                VCI_CAN_OBJ obj = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((uint)pt + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                //canLog.recordLog(obj);
                if(obj.ID!= Convert.ToUInt32(carSelected["ReceiveID"].ToString(), 16))
                {
                    //return;
                }
                else
                {
                    for (int j = 0; j < 8; j++)
                    {
                        rev[j] = obj.Data[j];
                    }
                }
            }
            Marshal.FreeHGlobal(pt);
        }
        */

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
            objTmp = (VCI_CAN_OBJ)canObj[canObj.Count - 1];

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
                //recTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public static byte[] canStringToByte(string str)
        {
            string[] strTmp = str.Split(' ');
            List<byte> byteTmp = new List<byte>();
            Array.ForEach(strTmp, s => byteTmp.Add(Convert.ToByte(s, 16)));
            return byteTmp.ToArray();
        }
    }
}
