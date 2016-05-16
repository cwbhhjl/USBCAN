using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

//1.ZLGCAN系列接口卡信息的数据类型。
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

/////////////////////////////////////////////////////
//2.定义CAN信息帧的数据类型。
unsafe public struct VCI_CAN_OBJ  //使用不安全代码
{
    public uint ID;
    public uint TimeStamp;
    public byte TimeFlag;
    public byte SendType;
    public byte RemoteFlag;//是否是远程帧
    public byte ExternFlag;//是否是扩展帧
    public byte DataLen;

    public fixed byte Data[8];

    public fixed byte Reserved[3];
}

////2.定义CAN信息帧的数据类型。
//public struct VCI_CAN_OBJ 
//{
//    public UInt32 ID;
//    public UInt32 TimeStamp;
//    public byte TimeFlag;
//    public byte SendType;
//    public byte RemoteFlag;//是否是远程帧
//    public byte ExternFlag;//是否是扩展帧
//    public byte DataLen;
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
//    public byte[] Data;
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
//    public byte[] Reserved;

//    public void Init()
//    {
//        Data = new byte[8];
//        Reserved = new byte[3];
//    }
//}

//3.定义CAN控制器状态的数据类型。
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

//4.定义错误信息的数据类型。
public struct VCI_ERR_INFO
{
    public uint ErrCode;
    public byte Passive_ErrData1;
    public byte Passive_ErrData2;
    public byte Passive_ErrData3;
    public byte ArLost_ErrData;
}

//5.定义初始化CAN的数据类型
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

namespace WindowsApplication1
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
        public static extern uint VCI_OpenDevice(uint DeviceType, uint DeviceInd, uint Reserved);
        [DllImport("controlcan.dll")]
        public static extern uint VCI_CloseDevice(uint DeviceType, uint DeviceInd);
        [DllImport("controlcan.dll")]
        public static extern uint VCI_InitCAN(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        static extern uint VCI_ReadBoardInfo(uint DeviceType, uint DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        public static extern uint VCI_ReadErrInfo(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_ERR_INFO pErrInfo);
        [DllImport("controlcan.dll")]
        static extern uint VCI_ReadCANStatus(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_CAN_STATUS pCANStatus);
        [DllImport("controlcan.dll")]
        static extern uint VCI_GetReference(uint DeviceType, uint DeviceInd, uint CANInd, uint RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        static extern uint VCI_SetReference(uint DeviceType, uint DeviceInd, uint CANInd, uint RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        public static extern uint VCI_GetReceiveNum(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        static extern uint VCI_ClearBuffer(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        public static extern uint VCI_StartCAN(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        public static extern uint VCI_ResetCAN(uint DeviceType, uint DeviceInd, uint CANInd);
        [DllImport("controlcan.dll")]
        public static extern uint VCI_Transmit(uint DeviceType, uint DeviceInd, uint CANInd, ref VCI_CAN_OBJ pSend, uint Len);
        //[DllImport("controlcan.dll")]
        //static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        public static extern uint VCI_Receive(uint DeviceType, uint DeviceInd, uint CANInd, IntPtr pReceive, uint Len, int WaitTime);

        public static uint m_devtype = 4;//USBCAN2
        public static uint m_bOpen = 0;
        public static uint m_devind = 0;
        public static uint m_canind = 0;

        //public static VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[50];
        static byte[] rev = new byte[8];

        public uint[] m_arrdevtype = new uint[20];

        public VCI_ERR_INFO m_errorInfo;

        private IDictionary carSelected = null;

        System.Threading.Timer recTimer = null;

        //public static CanLog canLog = new CanLog();

        unsafe public CanControl()
        {
            recTimer = new System.Threading.Timer(new TimerCallback(recTimer_Tick), null, Timeout.Infinite, Timeout.Infinite);

            //VCI_OpenDevice(m_devtype, m_devind, 0);

            //VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
            //config.AccCode = 0; config.AccMask = 4294967295; config.Filter = 1; config.Timing1 = 28; config.Timing0 = 0;
            //config.Mode = 0;
            //VCI_InitCAN(m_devtype, m_devind, m_canind, ref config);
            //VCI_StartCAN(m_devtype, m_devind, m_canind);
            //VCI_CAN_OBJ[] vco = new VCI_CAN_OBJ[1];
            //vco[0].ID = 0x00000001;
            //vco[0].SendType = 0;
            //vco[0].RemoteFlag = 0;
            //vco[0].ExternFlag = 0;
            //vco[0].DataLen = 1;
            //vco[0].Data[0] = (byte)0x66;
            //fixed (VCI_CAN_OBJ* sendobjs = &vco[0])
            //{
            //    sendobjs[0].Data[0] = 0x66;
            //    sendobjs[0].Data[1] = 0x66;
            //    sendobjs[0].Data[2] = 0x66;
            //    sendobjs[0].Data[3] = 0x66;
            //    sendobjs[0].Data[4] = 0x66;
            //    sendobjs[0].Data[5] = 0x66;
            //    sendobjs[0].Data[6] = 0x66;
            //    sendobjs[0].Data[7] = 0x66;
            //}
            //VCI_ERR_INFO vei; uint dd; vei.ArLost_ErrData = 0; vei.ErrCode = 0; vei.Passive_ErrData1 = 0; vei.Passive_ErrData2 = 0; vei.Passive_ErrData3 = 0;
            //uint res = VCI_Transmit(m_devtype, m_devind, m_canind, ref vco[0], 1);
            //dd = VCI_ReadErrInfo(m_devtype, m_devind, m_canind, ref vei);
            //VCI_ERR_INFO ve = vei;
        }

        public static void canConnect()
        {
            if (m_bOpen == 0)
            {
                if (VCI_OpenDevice(m_devtype, m_devind, 0) == 0)
                {
                    MessageBox.Show("打开设备失败,请检查设备类型和设备索引号是否正确", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                m_bOpen = 1;
                VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                config.AccCode = 0;
                config.AccMask = 0xFFFFFFFF;
                config.Timing0 = 0;
                config.Timing1 = 28;
                config.Filter = 1;
                config.Mode = 0;
                VCI_InitCAN(m_devtype, m_devind, m_canind, ref config);
                VCI_StartCAN(m_devtype, m_devind, m_canind);

            }
            //recTimer.Change(100, Timeout.Infinite);
        }

        public static uint canSend(ref VCI_CAN_OBJ pSend)
        {
            return VCI_Transmit(m_devtype, m_devind, m_canind, ref pSend, 1);
        }

        unsafe private void sendSingleFrame(string canID, string strData)
        {
            if (m_bOpen == 0)
            {
                return;
            }

            VCI_CAN_OBJ[] sendobj = new VCI_CAN_OBJ[1];//sendobj.Init();

            sendobj[0].SendType = 0;//正常发送：0；自发自收：2
            sendobj[0].RemoteFlag = 0;
            sendobj[0].ExternFlag = 0;
            sendobj[0].ID = Convert.ToUInt32(canID, 16);
            int len = (strData.Length + 1) / 3;
            sendobj[0].DataLen = System.Convert.ToByte(len);

            for (int n = -1; n < len - 1; n++)
            {
                fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                {
                    sendobjs[0].Data[n + 1] = Convert.ToByte("0x" + strData.Substring((n + 1) * 3, 2), 16);
                }
            }

            canSend(ref sendobj[0]);
            Flash.Delay(30);
        }

        unsafe private void sendSingleFrame(uint canID, byte[] date)
        {
            if (m_bOpen == 0)
            {
                return;
            }

            VCI_CAN_OBJ sendobj = new VCI_CAN_OBJ();

            sendobj.SendType = 0;
            sendobj.RemoteFlag = 0;
            sendobj.ExternFlag = 0;
            sendobj.ID = canID;
            int len = date.Length;
            sendobj.DataLen = Convert.ToByte(len);

            for (int n = 0; n < len; n++)
            {
                sendobj.Data[n] = date[n];
            }

            canSend(ref sendobj);
            Flash.Delay(30);
        }

        unsafe private void sendFrames(uint canID,byte[] data)
        {
            if(data.Length<=8)
            {
                sendSingleFrame(canID, data);
                return;
            }
            if (m_bOpen == 0)
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
            canSend(ref sendobj);
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
                    canSend(ref sendobj);
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

        public void setCar(IDictionary carSelected) {
            this.carSelected = carSelected;
            recTimer.Change(10, Timeout.Infinite);
        }

        unsafe public void recTimer_Tick(object state)
        {
            //StreamWriter log = new StreamWriter(Environment.CurrentDirectory + "Can.log", true);
            uint res = new uint();
            res = VCI_GetReceiveNum(m_devtype, m_devind, m_canind);
            if (res == 0)
            {
                return;
            }
            uint con_maxlen = 50;
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (int)con_maxlen);

            res = VCI_Receive(m_devtype, m_devind, m_canind, pt, con_maxlen, 100);

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

        static void canReset()
        {
            if (m_bOpen == 0)
            {
                return;
            }
            VCI_ResetCAN(m_devtype, m_devind, m_canind);
        }

        public static void canClose()
        {
            if (m_bOpen == 1)
            {
                VCI_CloseDevice(m_devtype, m_devind);
                m_bOpen = 0;
                //recTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
    }
}
