using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.UDS
{
    public class UDSMessage
    {
        public byte ServiceId;
        public byte SubFunction;

        protected string strServiceId;
        protected string strSubFunction;
        protected string strContent;

        public byte[] data = null;

        public string serviceId
        {
            set
            {
                strServiceId = value;
                ServiceId = Convert.ToByte(strServiceId, 16);
            }
        }

        public string subFunction
        {
            set
            {
                strSubFunction = value;
                SubFunction = Convert.ToByte(strSubFunction, 16);
            }
        }

        public string content
        {
            set
            {
                strContent = value;
            }
        }

        public byte[] getContent()
        {
            if (data == null)
            {
                data = messageToArray(strContent);
            }
            return data;
        }

        protected static byte[] messageToArray(string str)
        {
            string[] strTmp = str.Split(' ');
            List<byte> byteTmp = new List<byte>();
            foreach (string i in strTmp)
            {
                byteTmp.Add(Convert.ToByte(i, 16));
            }
            return byteTmp.ToArray();
        }
    }

    public class UDSDiagnosticControl : UDSMessage
    {
        public static readonly new byte ServiceId = 0x10;
    }

    public class UDSEcuReset : UDSMessage
    {
        public static readonly new byte ServiceId = 0x11;
    }

    public class UDSSecurityAccess : UDSMessage
    {
        public static readonly new byte ServiceId = 0x27;
        byte[] SecurityAccessDataRecord = null;
        string securityAccessDataRecord
        {
            get
            {
                return securityAccessDataRecord;
            }
            set
            {
                securityAccessDataRecord = value;
                SecurityAccessDataRecord = messageToArray(securityAccessDataRecord);
            }
        }
    }

    public class UDSCommunicationControl : UDSMessage
    {
        public static readonly new byte ServiceId = 0x28;
    }
}