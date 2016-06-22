using System;
using System.IO;
using System.Text;

namespace USBCAN
{
    public class CanLog
    {
        private StreamWriter log = null;
        private StringBuilder logStr;

        public CanLog()
        {
            logStr = new StringBuilder();
            //log = new StreamWriter(Environment.CurrentDirectory + "Can" + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".log", true);
        }

        unsafe public void recordLog(VCI_CAN_OBJ obj)
        {
            logStr.Append("帧ID:0x" + Convert.ToString((int)obj.ID, 16));
            logStr.Append("  帧格式:");
            if (obj.RemoteFlag == 0)
                logStr.Append("数据帧 ");
            else
                logStr.Append("远程帧 ");
            if (obj.ExternFlag == 0)
                logStr.Append("标准帧 ");
            else
                logStr.Append("扩展帧 ");

            if (obj.RemoteFlag == 0)
            {
                logStr.Append("数据: ");
                byte len = (byte)(obj.DataLen % 9);

                for (byte j = 0; j < len; j++)
                {
                    logStr.Append(" " + Convert.ToString(obj.Data[j], 16));
                }
            }
            logStr.Append(Environment.NewLine);
        }

        public void makeLog()
        {
            if (logStr.Length == 0)
            {
                log = new StreamWriter(Environment.CurrentDirectory + "Can" + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".log", true);
                log.WriteLine(logStr);
                log.Close();
            }
        }
    }
}
