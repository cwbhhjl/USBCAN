using System;
using System.Collections;
using System.IO;
using System.Text;

namespace WindowsApplication1
{
    class HexS19
    {
        public const byte S1AddressLen = 2;
        public const byte S2AddressLen = 3;
        public const byte S3AddressLen = 4;
        public const byte S5AddressLen = 2;
        int indexLine = 0;
        string driverPath = Environment.CurrentDirectory + "\\FlashDriver_S12GX_V1.0.s19";
        string[] line;
        ArrayList strLineTmp = new ArrayList();
        ArrayList s19LineTmp = new ArrayList();

        public int readFile()
        {
            string tmp;
            using (FileStream fs = File.OpenRead(driverPath))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                while(!sr.EndOfStream)
                {
                    strLineTmp.Add(sr.ReadLine());
                }
                sr.Close();
                line = (string[])strLineTmp.ToArray(typeof(string));
            }
            for (int i = 0; i < strLineTmp.Count; i++)
            {
                byte checkSum = 0;
                tmp = ((string)strLineTmp[i]).Substring(indexLine, 2);
                indexLine += 2;
                if (tmp.Equals("S0") || tmp.Equals("S7") || tmp.Equals("S8") || tmp.Equals("S9"))
                {
                    strLineTmp.Remove(strLineTmp[i]);
                    i--;
                }
                if(tmp.Equals("S1"))
                {
                    s19LineTmp.Add(new S19Line());
                    S19Line tmpS19 = ((S19Line)s19LineTmp[i]);
                    tmpS19.num= Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
                    checkSum += tmpS19.num;
                    tmpS19.num = (byte)(tmpS19.num - S1AddressLen-1);
                    indexLine += 2;
                    tmpS19.lineAddress = new byte[S1AddressLen];
                    for(int a=0;a<S1AddressLen;a++)
                    {
                        tmpS19.lineAddress[a]= Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
                        indexLine += 2;
                        checkSum += tmpS19.lineAddress[a];
                    }
                    tmpS19.date = new byte[tmpS19.num];
                    for(int j=0;j<tmpS19.date.Length;j++)
                    {
                        tmpS19.date[j] = Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
                        checkSum += tmpS19.date[j];
                        indexLine += 2;
                    }
                    if((0xff-checkSum)==Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16))
                    {
                        tmpS19.sumCheck = (byte)(0xff-checkSum);
                    }
                    else
                    {
                        return 0;
                    }
                }
                indexLine = 0;
            }
            return 1;
        }

    }

    class S19Line
    {
        public byte[] lineAddress;
        public byte num;
        public byte[] date;
        public byte sumCheck;
    }
}
