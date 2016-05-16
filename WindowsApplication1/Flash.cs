using System;
using System.Collections;
using System.Windows.Forms;

namespace WindowsApplication1
{
    class Flash
    {
        private IDictionary carSelected = null;
        private Security sec = null;

        private string driverName = "\\FlashDriver_S12GX_V1.0.s19";

        public string DriverName
        {
            get
            {
                return Environment.CurrentDirectory + driverName;
            }

            set
            {
                driverName = "\\" + value;
            }
        }

        public Flash(IDictionary carSelected)
        {
            this.carSelected = carSelected;
            sec = new Security(carSelected);
        }

        public int start()
        {
            CanControl.canConnect();

            CanControl test = new CanControl();
            test.setCar(carSelected);

            enterExSession();

            checkPreProg();

            setDtcOff();

            disableCommunication();

            enterProgSession();

            requestSeed();

            return 1;
        }

        public string readVersion()
        {
            if (carSelected == null)
            {
                return "";
            }
            //VCI_ERR_INFO vei;UInt32 dd;vei.ArLost_ErrData = 0;vei.ErrCode = 0;vei.Passive_ErrData1 = 0;vei.Passive_ErrData2 = 0;vei.Passive_ErrData3 = 0;
            sendSingleFrame(carSelected["PhysicalID"].ToString(), carSelected["SoftwareVersion"].ToString());
            //dd = Form1.VCI_ReadErrInfo(Form1.m_devtype,Form1.m_devind,Form1.m_canind,ref vei);

            return "1";
        }

        private void enterExSession()
        {
            if (carSelected == null)
            {
                return;
            }

            sendSingleFrame(carSelected["PhysicalID"].ToString(), carSelected["ExtendedSession"].ToString());
        }

        private void checkPreProg()
        {
            if (carSelected == null)
            {
                return;
            }

            sendSingleFrame(carSelected["PhysicalID"].ToString(), carSelected["PreProgrammingCheck"].ToString());
        }

        private void setDtcOff()
        {
            if (carSelected == null)
            {
                return;
            }

            sendSingleFrame(carSelected["PhysicalID"].ToString(), carSelected["DtcSetOFF"].ToString());
        }

        private void disableCommunication()
        {
            if (carSelected == null)
            {
                return;
            }

            sendSingleFrame(carSelected["PhysicalID"].ToString(), carSelected["CommunicationDisable"].ToString());
        }

        private void enterProgSession()
        {
            if (carSelected == null)
            {
                return;
            }

            sendSingleFrame(carSelected["PhysicalID"].ToString(), carSelected["ProgrammingSession"].ToString());
        }

        private void requestSeed()
        {
            if (carSelected == null)
            {
                return;
            }

            sendSingleFrame(carSelected["PhysicalID"].ToString(), carSelected["SeedRequest"].ToString());
        }


        unsafe private void sendSingleFrame(String canID, String strData)
        {
            if (CanControl.m_bOpen == 0)
            {
                return;
            }

            VCI_CAN_OBJ[] sendobj = new VCI_CAN_OBJ[1];//sendobj.Init();

            sendobj[0].SendType = 0;//正常发送：0；自发自收：2
            sendobj[0].RemoteFlag = 0;
            sendobj[0].ExternFlag = 0;
            sendobj[0].ID = System.Convert.ToUInt32(canID, 16);
            int len = (strData.Length + 1) / 3;
            sendobj[0].DataLen = System.Convert.ToByte(len);

            for (int n = -1; n < len - 1; n++)
            {
                fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                {
                    sendobjs[0].Data[n + 1] = System.Convert.ToByte("0x" + strData.Substring((n + 1) * 3, 2), 16);
                }
            }

            CanControl.canSend(ref sendobj[0]);
            Delay(30);
        }

        void sendFrames()
        {

        }

        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }

    }
}
