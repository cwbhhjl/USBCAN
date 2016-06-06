using System;
using System.IO;

namespace USBCAN
{
    public class CanLog
    {
        private StreamWriter log = null;
        private string logStr;

        public CanLog()
        {
            logStr = "";
            //log = new StreamWriter(Environment.CurrentDirectory + "Can" + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".log", true);
        }

        unsafe public void recordLog(VCI_CAN_OBJ obj)
        {
            logStr += "帧ID:0x" + System.Convert.ToString((int)obj.ID, 16);
            logStr += "  帧格式:";
            if (obj.RemoteFlag == 0)
                logStr += "数据帧 ";
            else
                logStr += "远程帧 ";
            if (obj.ExternFlag == 0)
                logStr += "标准帧 ";
            else
                logStr += "扩展帧 ";

            if (obj.RemoteFlag == 0)
            {
                logStr += "数据: ";
                byte len = (byte)(obj.DataLen % 9);

                for (byte j = 0; j < len; j++)
                {
                    logStr += " " + Convert.ToString(obj.Data[j], 16);
                }
            }
            logStr += Environment.NewLine;
        }

        public void makeLog()
        {
            if (logStr != "")
            {
                log = new StreamWriter(Environment.CurrentDirectory + "Can" + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".log", true);
                log.WriteLine(logStr);
                log.Close();
            }
        }
    }
}
