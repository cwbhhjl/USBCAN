using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace USBCAN
{
    public class CanControl
    {
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
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        private static extern uint VCI_Receive(uint DeviceType, uint DeviceInd, uint CANInd, IntPtr pReceive, uint Len, int WaitTime);

        public static uint deviceType = (uint)USBCAN.HardwareType.VCI_USBCAN2;
        private static bool isOpen = false;
        public static uint deviceIndex = 0;
        public static uint canIndex = 0;

        private static byte[] rev = new byte[8];

        public static VCI_CAN_OBJ obj = new VCI_CAN_OBJ();
        public static VCI_ERR_INFO errorInfo = new VCI_ERR_INFO();
        public static VCI_BOARD_INFO boardInfo = new VCI_BOARD_INFO();

        private static CanControl canCtl;

        public static uint res = 0;

        public static byte[] Rev
        {
            get
            {
                return rev;
            }
        }

        internal static bool IsOpen
        {
            get
            {
                return isOpen;
            }

            set
            {
                isOpen = value;
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
 
                VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();

                config.AccCode = 0;
                config.AccMask = 0xFFFFFFFF;
                config.Timing0 = 0;
                config.Timing1 = 28;
                config.Filter = 1;
                config.Mode = 0;

                if(VCI_InitCAN(deviceType, deviceIndex, canIndex, ref config) != 1
                    || VCI_StartCAN(deviceType, deviceIndex, canIndex) != 1)
                {
                    return false;
                }

                isOpen = true;
                //VCI_InitCAN(deviceType, deviceIndex, canIndex, ref config);
                //VCI_StartCAN(deviceType, deviceIndex, canIndex);

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
            else if(readBoardInfo() == 0)
            {
                isOpen = false;
                canConnect();
            }

            return isOpen;
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
                VCI_ReadErrInfo(deviceType, deviceIndex, canIndex, ref errorInfo);
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

                        try
                        {
                            VCI_Transmit(deviceType, deviceIndex, canIndex, ref obj, 1);
                        }
                        catch (Exception)
                        {
                            return -6;
                        }
                        
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
                if (canLastReceive(receiveID) != null)
                {
                    //canLastReceive(receiveID);
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

            if(canObj.Count == 0)
            {
                return null;
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

        static uint canReset()
        {
            if (!isOpen)
            {
                return 2;
            }
            return VCI_ResetCAN(deviceType, deviceIndex, canIndex);
        }

        public static void canClose()
        {
            if(VCI_CloseDevice(deviceType, deviceIndex) == 1)
            {
                isOpen = false;
            }
        }

        public static uint canReInit()
        {
            if(VCI_ResetCAN(deviceType, deviceIndex, canIndex) == 1)
            {
                return VCI_StartCAN(deviceType, deviceIndex, canIndex);
            }
            return 2;
        }

        public static uint canClearBuffer()
        {
            return VCI_ClearBuffer(deviceType, deviceIndex, canIndex);
        }

        public static uint readBoardInfo()
        {
            return VCI_ReadBoardInfo(deviceType, deviceIndex, ref boardInfo);
        }
    }
}
