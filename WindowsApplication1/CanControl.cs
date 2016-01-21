using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApplication1
{
    class CanControl
    {
        const int VCI_PCI5121 = 1;
        const int VCI_PCI9810 = 2;
        const int VCI_USBCAN1 = 3;
        const int VCI_USBCAN2 = 4;
        const int VCI_USBCAN2A = 4;
        const int VCI_PCI9820 = 5;
        const int VCI_CAN232 = 6;
        const int VCI_PCI5110 = 7;
        const int VCI_CANLITE = 8;
        const int VCI_ISA9620 = 9;
        const int VCI_ISA5420 = 10;
        const int VCI_PC104CAN = 11;
        const int VCI_CANETUDP = 12;
        const int VCI_CANETE = 12;
        const int VCI_DNP9810 = 13;
        const int VCI_PCI9840 = 14;
        const int VCI_PC104CAN2 = 15;
        const int VCI_PCI9820I = 16;
        const int VCI_CANETTCP = 17;
        const int VCI_PEC9920 = 18;
        const int VCI_PCI5010U = 19;
        const int VCI_USBCAN_E_U = 20;
        const int VCI_USBCAN_2E_U = 21;
        const int VCI_PCI5020U = 22;
        const int VCI_EG20T_CAN = 23;
        const int VCI_PCIE9221 = 24;

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadErrInfo(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_ERR_INFO pErrInfo);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadCANStatus(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_STATUS pCANStatus);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_GetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ClearBuffer(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ResetCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len);

        //[DllImport("controlcan.dll")]
        //static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pReceive, UInt32 Len, Int32 WaitTime);

        public static UInt32 m_devtype = 4;//USBCAN2

        public static UInt32 m_bOpen = 0;
        public static UInt32 m_devind = 0;
        public static UInt32 m_canind = 0;

        public static VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[50];

        public UInt32[] m_arrdevtype = new UInt32[20];

        unsafe public CanControl()
        {
            VCI_OpenDevice(m_devtype, m_devind, 0);
            
            VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
            config.AccCode = 0;config.AccMask = 4294967295;config.Filter = 1;config.Timing1 = 28;config.Timing0 = 0;
            config.Mode = 0;
            VCI_InitCAN(m_devtype, m_devind, m_canind, ref config);
            VCI_StartCAN(m_devtype, m_devind, m_canind);
            VCI_CAN_OBJ[] vco=new VCI_CAN_OBJ[1];
            vco[0].ID = 0x00000001;
            vco[0].SendType = 0;
            vco[0].RemoteFlag = 0;
            vco[0].ExternFlag = 0;
            vco[0].DataLen = 1;
            //vco[0].Data[0] = (byte)0x66;
            fixed (VCI_CAN_OBJ* sendobjs = &vco[0])
            {
                sendobjs[0].Data[0] = 0x66;
                sendobjs[0].Data[1] = 0x66;
                sendobjs[0].Data[2] = 0x66;
                sendobjs[0].Data[3] = 0x66;
                sendobjs[0].Data[4] = 0x66;
                sendobjs[0].Data[5] = 0x66;
                sendobjs[0].Data[6] = 0x66;
                sendobjs[0].Data[7] = 0x66;
            }
            VCI_ERR_INFO vei; UInt32 dd; vei.ArLost_ErrData = 0; vei.ErrCode = 0; vei.Passive_ErrData1 = 0; vei.Passive_ErrData2 = 0; vei.Passive_ErrData3 = 0;
            uint res = VCI_Transmit(m_devtype, m_devind, m_canind, ref vco[0], (uint)1);
            dd = VCI_ReadErrInfo(m_devtype, m_devind, m_canind, ref vei);
            VCI_ERR_INFO ve=vei;
        }
    }
}
