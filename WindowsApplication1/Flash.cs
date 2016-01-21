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
            //VCI_ERR_INFO vei;UInt32 dd;vei.ArLost_ErrData = 0;vei.ErrCode = 0;vei.Passive_ErrData1 = 0;vei.Passive_ErrData2 = 0;vei.Passive_ErrData3 = 0;
            sendData(carSelected["PhysicalID"].ToString(), carSelected["SoftwareVersion"].ToString());
            //dd = Form1.VCI_ReadErrInfo(Form1.m_devtype,Form1.m_devind,Form1.m_canind,ref vei);
            
            return "1";
        }

        unsafe private void sendData(String canID,String strData)
        {
            if (Form1.m_bOpen == 0)
                return;
            
            VCI_CAN_OBJ[] sendobj = new VCI_CAN_OBJ[1];//sendobj.Init();
            for (int j = 0; j < sendobj.Length; j++)
            {
                sendobj[j].SendType = 2;
                sendobj[j].RemoteFlag = 0;
                sendobj[j].ExternFlag = 0;
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


            uint res = Form1.VCI_Transmit(Form1.m_devtype, Form1.m_devind, Form1.m_canind, ref sendobj[0], 1);
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
