using System;
using System.IO;
using System.Text;

namespace USBCAN
{
    internal class CanLog
    {
        private static StreamWriter log = null;
        private static StringBuilder logStr = new StringBuilder();

        private static bool logFlag = false;

        unsafe internal static void recordLog(VCI_CAN_OBJ obj)
        {
            if (CanControl.log)
            {
                logStr.Append("帧ID:0x" + Convert.ToString((int)obj.ID, 16));

                if (obj.RemoteFlag == 0)
                {
                    logStr.Append("    数据: ");
                    byte len = (byte)(obj.DataLen % 9);

                    for (byte j = 0; j < len; j++)
                    {
                        logStr.Append(" " + Convert.ToString(obj.Data[j], 16));
                    }
                }
                logStr.Append(Environment.NewLine);
            }
        }

        internal static void recordLog(uint id, byte[] data)
        {
            if (CanControl.log)
            {
                logStr.Append("帧ID:0x" + Convert.ToString(id, 16));
                logStr.Append("    数据: ");
                logStr.Append(BitConverter.ToString(data));
                logStr.Append(Environment.NewLine);
            }
        }

        internal static void makeLog()
        {
            if (CanControl.log && logStr.Length != 0)
            {
                log = new StreamWriter(Environment.CurrentDirectory + "Can" + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".log", true);
                log.WriteLine(logStr);
                log.Close();
            }
            logStr.Clear();
            logFlag = false;
        }
    }
}
