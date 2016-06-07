using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace USBCAN
{
    class Flash
    {
        private uint physicalID;
        private uint functionID;
        private uint receiveID;
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

        private Thread flashThread = null;
        private Thread sendThread = null;
        private Thread receiveThread = null;

        public Flash(IDictionary carSelected)
        {
            this.carSelected = carSelected;
            sec = new Security(carSelected);

            physicalID = Convert.ToUInt32(carSelected["PhysicalID"].ToString(), 16);
            functionID = Convert.ToUInt32(carSelected["FunctionID"].ToString(), 16);
            receiveID = Convert.ToUInt32(carSelected["ReceiveID"].ToString(), 16);
        }

        public void init()
        {
            flashThread = new Thread(new ThreadStart(start));
        }

        public void start()
        {
            sendThread = new Thread(new ThreadStart(sendCan));
            receiveThread = new Thread(new ThreadStart(receiveCan));

            sendThread.Join();
            receiveThread.Join();
        }

        void sendCan()
        {
            enterExSession();

            checkPreProg();

            setDtcOff();

            disableCommunication();

            enterProgSession();

            requestSeed();
        }

        void receiveCan()
        {
            CanControl.canLastReceive(receiveID);
        }

        public string readVersion()
        {
            if (carSelected == null)
            {
                return "";
            }
            //VCI_ERR_INFO vei;UInt32 dd;vei.ArLost_ErrData = 0;vei.ErrCode = 0;vei.Passive_ErrData1 = 0;vei.Passive_ErrData2 = 0;vei.Passive_ErrData3 = 0;
            CanControl.sendFrame(carSelected["PhysicalID"].ToString(), carSelected["SoftwareVersion"].ToString());
            //dd = Form1.VCI_ReadErrInfo(Form1.m_devtype,Form1.m_devind,Form1.m_canind,ref vei);

            return "1";
        }

        private void enterExSession()
        {
            if (carSelected == null)
            {
                return;
            }

            CanControl.sendFrame(carSelected["PhysicalID"].ToString(), carSelected["ExtendedSession"].ToString());
        }

        private void checkPreProg()
        {
            if (carSelected == null)
            {
                return;
            }

            CanControl.sendFrame(carSelected["PhysicalID"].ToString(), carSelected["PreProgrammingCheck"].ToString());
        }

        private void setDtcOff()
        {
            if (carSelected == null)
            {
                return;
            }

            CanControl.sendFrame(carSelected["PhysicalID"].ToString(), carSelected["DtcSetOFF"].ToString());
        }

        private void disableCommunication()
        {
            if (carSelected == null)
            {
                return;
            }

            CanControl.sendFrame(carSelected["PhysicalID"].ToString(), carSelected["CommunicationDisable"].ToString());
        }

        private void enterProgSession()
        {
            if (carSelected == null)
            {
                return;
            }

            CanControl.sendFrame(carSelected["PhysicalID"].ToString(), carSelected["ProgrammingSession"].ToString());
        }

        private void requestSeed()
        {
            if (carSelected == null)
            {
                return;
            }

            CanControl.sendFrame(carSelected["PhysicalID"].ToString(), carSelected["SeedRequest"].ToString());
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
