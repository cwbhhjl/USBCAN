using System;
using System.IO;
using System.Text;

namespace USBCAN
{
    internal class CanLog
    {
        private static StreamWriter log = null;
        private static StringBuilder logStr = new StringBuilder();

        unsafe internal static void recordLog(VCI_CAN_OBJ obj)
        {
            if (CanControl.log)
            {
                logStr.Append("ID:0x" + Convert.ToString((int)obj.ID, 16));
                logStr.Append("    data: ");
                byte[] tmp = new byte[8]; 
                for (byte j = 0; j < 8; j++)
                {
                    tmp[j] = obj.Data[j];
                }
                logStr.Append(BitConverter.ToString(tmp));
                logStr.Append(Environment.NewLine);
            }
        }

        internal static void recordLog(uint id, byte[] data)
        {
            if (CanControl.log)
            {
                logStr.Append("ID:0x" + Convert.ToString(id, 16));
                logStr.Append("    data: ");
                logStr.Append(BitConverter.ToString(data));
                logStr.Append(Environment.NewLine);
            }
        }

        internal static void makeLog()
        {
            if (CanControl.log && logStr.Length != 0)
            {
                log = new StreamWriter(Environment.CurrentDirectory + "\\flash" + DateTime.Now.ToString("yyyyMMdd_HHmmssff") + ".log", true);
                log.WriteLine(logStr);
                log.Close();
            }
            logStr.Clear();
        }
    }
}
