using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN.UDS
{
    public class UDSMessage
    {
        internal byte ServiceId;
        internal byte SubFunction;
        private byte idTmp;

        internal byte[] data = null;

        public string serviceId
        {
            get
            {
                return serviceId;
            }
            set
            {
                serviceId = value;
                idTmp = Convert.ToByte(serviceId, 16);
            }
        }

        public string subFunction
        {
            get
            {
                return subFunction;
            }
            set
            {
                subFunction = value;
                SubFunction = Convert.ToByte(subFunction, 16);
            }
        }

        public string content { get; set; }

        public byte[] getContent()
        {
            if (data == null)
            {
                data = messageToArray(content);
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