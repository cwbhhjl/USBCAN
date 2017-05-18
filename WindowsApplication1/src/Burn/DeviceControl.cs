using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using USBCAN.Device;
using USBCAN.UDS;

namespace USBCAN.Burn
{
    public class DeviceControl
    {
        private ZLGCAN zlgDevice;
        private ZLGCAN.Can zlgChannel;

        private VCI_CAN_OBJ zlgObj;

        private byte[] timing;

        public bool IsConnect
        {
            get;
            private set;
        }

        public string ReadError()
        {
            uint errCode;
            if (zlgDevice.ReadErrInfo(out errCode))
            {
                return ZLGCAN.ERR[errCode];
            }
            else
            {
                return null;
            }
            
        }

        public bool Connect()
        {
            if (!IsConnect)
            {
                if (zlgDevice.OpenDevice() && zlgChannel.InitCan(timing[0],timing[1]) 
                    && zlgChannel.StartCan())
                {
                    IsConnect = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (!zlgDevice.ReadBoardInfo())
                {
                    IsConnect = false;
                    return Connect();
                }
                return true;
            }
        }

        public bool Close()
        {
            if(zlgDevice.CloseDevice())
            {
                IsConnect = false;
                return true;
            }
            return false;
        }

        public void DetectDeviceChange()
        {
            if (!IsConnect)
            {
                Connect();
            }
            else if (!zlgDevice.ReadBoardInfo())
            {
                if (!Connect())
                {
                    Close();
                    IsConnect = false;
                }
            } 
        }

        public DeviceControl(ZLGCANJson zlgcan)
        {
            zlgDevice = new ZLGCAN((ZLGCAN.HardwareType)zlgcan.deviceType, zlgcan.deviceIndex);
            zlgDevice.AddCan(zlgcan.channel);
            zlgChannel = zlgDevice.CanList[zlgcan.channel];
            timing = new byte[2] { Convert.ToByte(zlgcan.timing0, 16), Convert.ToByte(zlgcan.timing1, 16) };
            IsConnect = false;
            zlgObj = new VCI_CAN_OBJ() { SendType = 0, RemoteFlag = 0, ExternFlag = 0, DataLen = 8 };
        }

        public bool SendUDSFrame(CanUDSFrame frame)
        {
            uint id = frame.id;
            zlgObj.ID = id;
            byte[] data = frame.nextFrame();

            bool result = SendByteArray(data);

            if (!result || frame.framesCount == 0)
            {
                return result;
            }

            return SendConsecutiveFrames(frame);
        }

        private bool SendByteArray(byte[] data)
        {
            zlgChannel.ClearBuffer();
            ByteCopyToObj(data, ref zlgObj);
            return zlgChannel.Send(ref zlgObj);
        }

        public Queue<byte[]> ReceiveUDSFrame(uint id)
        {
            Queue<byte[]> rawData = GetResponse(id);
            Queue<byte[]> messageQueue = new Queue<byte[]>();

            if (rawData == null)
            {
                return null;
            }
            List<byte> dataList = new List<byte>();
            int N_PCItype;
            while (rawData.Count != 0)
            {
                byte[] data = rawData.Dequeue();
                N_PCItype = data[0] >> 4;
                if (N_PCItype == N_PCI.SF.N_PCItype)
                {
                    int SF_DL = data[0] & 0x0F;
                    byte[] message = new byte[SF_DL];
                    Array.Copy(data, 1, message, 0, SF_DL);
                    messageQueue.Enqueue(message);
                }
                else if (N_PCItype == N_PCI.FF.N_PCItype)
                {
                    int FF_DL = ((data[0] & 0x0F) << 8) | data[1];
                    byte[] allData = new byte[FF_DL];
                    Array.Copy(data, 2, allData, 0, 6);
                    SendByteArray(CanUDSFrame.FlowControl);
                    if(GetConsecutiveFrame(id, FF_DL, allData))
                    {
                        messageQueue.Enqueue(allData);
                        return messageQueue;
                    }
                    return null;
                }
            }
            return messageQueue.Count == 0 ? null : messageQueue;
        }

        private bool SendConsecutiveFrames(CanUDSFrame frame)
        {
            byte[] data; bool result;
            uint id = frame.id;
            data = GetFlowControl(id);

            if (data == null)
            {
                return false;
            }

            handleFlowControl:
            switch (data[0])
            {
                case (N_PCI.FC.N_PCItype << 4) | N_PCI.FC.FS.CTS:
                    int BS = (data[1] == 0) ? 600 : data[1];
                    byte STmin = data[2];
                    for (int i = 0; i < BS; i++)
                    {
                        ByteCopyToObj(frame.nextFrame(), ref zlgObj);
                        result = zlgChannel.Send(ref zlgObj);
                        if (!result)
                        {
                            return false;
                        }
                        if (frame.framesCount == 0)
                        {
                            return true;
                        }
                        Thread.Sleep(STmin);
                    }
                    goto case (N_PCI.FC.N_PCItype << 4) | N_PCI.FC.FS.WT;
                case (N_PCI.FC.N_PCItype << 4) | N_PCI.FC.FS.WT:
                    data = GetFlowControl(id);
                    if (data == null)
                    {
                        return false;
                    }
                    goto handleFlowControl;
                case (N_PCI.FC.N_PCItype << 4) | N_PCI.FC.FS.OVFLW:
                    return false;
                default:
                    return false;
            }
        }

        private Queue<byte[]> ReceivRawData(uint id)
        {
            uint num = zlgChannel.ReceiveNum;

            if (num == 0)
            {
                return null;
            }

            Queue<byte[]> receive = new Queue<byte[]>();

            IntPtr pt;
            num = zlgChannel.Receive(out pt);
            for (int i = 0; i < num; i++)
            {
                VCI_CAN_OBJ objTmp;
                objTmp = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((uint)pt + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                if (objTmp.ID != id)
                {
                    continue;
                }
                receive.Enqueue(ObjCopyToByte(ref objTmp));
            }
            Marshal.FreeHGlobal(pt);

            return (receive.Count == 0) ? null : receive;
        }

        private byte[] ReceiveOneData(uint id)
        {
            uint num = zlgChannel.ReceiveNum;

            if (num == 0)
            {
                return null;
            }

            IntPtr pt;
            num = zlgChannel.Receive(out pt);
            for (int i = (int)num; i > 0; i--)
            {
                VCI_CAN_OBJ objTmp;
                objTmp = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((uint)pt + (i - 1) * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                if (objTmp.ID != id)
                {
                    continue;
                }
                byte[] data = ObjCopyToByte(ref objTmp);
                Marshal.FreeHGlobal(pt);
                return data;
            }
            Marshal.FreeHGlobal(pt);
            return null;
        }

        private Queue<byte[]> GetResponse(uint id, uint waitTime = 80)
        {
            Queue<byte[]> data;
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < waitTime)
            {
                data = ReceivRawData(id);
                if (data != null)
                {
                    return data;
                }
            }
            return null;
        }

        private bool GetConsecutiveFrame(uint id, int FF_DL, byte[] all)
        {
            byte SN = 1;
            int index = 6;
            while (index != FF_DL)
            {
                Queue<byte[]> dataQueue = ReceivRawData(id);
                while (dataQueue != null && dataQueue.Count != 0)
                {
                    byte[] data = dataQueue.Dequeue();
                    if (data[0] != (N_PCI.CF.N_PCItype << 4 | SN))
                    {
                        return false;
                    }
                    int len = (FF_DL - index) >= 7 ? 7 : FF_DL - index;
                    Array.Copy(data, 1, all, index, len);
                    SN++;
                    SN &= 0x0F;
                    index += len;
                }
            }
            return true;
        }

        private byte[] GetFlowControl(uint id)
        {
            byte[] data;
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < 80)
            {
                data = ReceiveOneData(id);
                if (data != null)
                {
                    return data;
                }
            }
            return null;
        }

        unsafe private void ByteCopyToObj(byte[] data, ref VCI_CAN_OBJ obj)
        {
            fixed(byte *pData = obj.Data)
            {
                for (int i = 0; i < 8; i++)
                {
                    pData[i] = data[i];
                }
            }
        }

        unsafe private byte[] ObjCopyToByte(ref VCI_CAN_OBJ obj)
        {
            byte[] data = new byte[8];
            fixed(byte *pData = obj.Data)
            {
                for (int i = 0; i < 8; i++)
                {
                    data[i] = pData[i];
                }
            }
            return data;
        }
    }
}
