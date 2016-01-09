using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;

namespace WindowsApplication1
{
    class Flash
    {
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo);
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_ReadErrInfo(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_ERR_INFO pErrInfo);
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
        static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len);

        //[DllImport("controlcan.dll")]
        //static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pReceive, UInt32 Len, Int32 WaitTime);
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pReceive, UInt32 Len, Int32 WaitTime);

        static UInt32 m_devtype = 4;//USBCAN2

        UInt32 m_bOpen = 0;
        UInt32 m_devind = 0;
        UInt32 m_canind = 0;

        VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[50];

        UInt32[] m_arrdevtype = new UInt32[20];

        private IDictionary carSelected = null;

        public int start()
        {
            return 1;
        }

        public String readVersion()
        {
            if (carSelected == null)
            {
                return "";
            }
            sendData(carSelected["PhysicalID"].ToString(), carSelected["SoftwareVersion"].ToString()); 
            return "1";
        }

        unsafe private void sendData(String canID,String strData)
        {
            if (m_bOpen == 0)
                return;
            
            VCI_CAN_OBJ[] sendobj = new VCI_CAN_OBJ[1];//sendobj.Init();
            for (int j = 0; j < sendobj.Length; j++)
            {
                sendobj[j].SendType = (byte)0;
                sendobj[j].RemoteFlag = (byte)0;
                sendobj[j].ExternFlag = (byte)0;
                sendobj[j].ID = System.Convert.ToUInt32("0x" + canID, 16);
                int len = (strData.Length + 1) / 3;
                sendobj[j].DataLen = System.Convert.ToByte(len);
                int i = -1;
                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[0] = System.Convert.ToByte("0x" + strData.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[1] = System.Convert.ToByte("0x" + strData.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[2] = System.Convert.ToByte("0x" + strData.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[3] = System.Convert.ToByte("0x" + strData.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[4] = System.Convert.ToByte("0x" + strData.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[5] = System.Convert.ToByte("0x" + strData.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[6] = System.Convert.ToByte("0x" + strData.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[7] = System.Convert.ToByte("0x" + strData.Substring(i * 3, 2), 16);
                    }
            }



            uint res = VCI_Transmit(m_devtype, m_devind, m_canind, ref sendobj[0], 0);
            if (res == 0)
            {
                MessageBox.Show("发送失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void setCar(IDictionary carSelected)
        {
            this.carSelected = carSelected; 
        } 
    }
}
