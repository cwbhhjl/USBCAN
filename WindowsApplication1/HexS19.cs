using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApplication1
{
    class HexS19
    {
        public const byte S1AddressLen = 4;
        public const byte S2AddressLen = 6;
        public const byte S3AddressLen = 8;
        public const byte S5AddressLen = 4;
        int indexLine = 0;
        string driverPath = Environment.CurrentDirectory + "\\FlashDriver_S12GX_V1.0.s19";
        string[] line;
        int lineNum = 0;
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
                    //lineNum += 1;
                    i--;
                }
                if(tmp.Equals("S1"))
                {
                    s19LineTmp.Add(new S19Line());
                    S19Line tmpS19 = ((S19Line)s19LineTmp[i]);
                    tmpS19.num= Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2),16);
                    checkSum += tmpS19.num;
                    tmpS19.num = (byte)(tmpS19.num - S1AddressLen/2-1);
                    indexLine += 2;
                    tmpS19.lineAddress= Convert.ToUInt32(((string)strLineTmp[i]).Substring(indexLine, S1AddressLen), 16);
                    indexLine += S1AddressLen;
                    tmpS19.date = new byte[tmpS19.num];
                    for(int j=0;j<tmpS19.date.Length;j++)
                    {
                        tmpS19.date[j] = Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16);
                        checkSum += tmpS19.date[j];
                        indexLine += 2;
                    }
                    if((0xff-checkSum)==Convert.ToByte(((string)strLineTmp[i]).Substring(indexLine, 2), 16))
                    {
                        tmpS19.sumCheck = checkSum;
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

        //S19Line[] stringToHex(string[] line)
        //{
        //    for (int i = 0; i < line.Length; i++)
        //    {
        //        if(line[i].Substring(0,2).Equals("S1")|| line[i].Substring(0, 2).Equals("S2")|| line[i].Substring(0, 2).Equals("S3")|| line[i].Substring(0, 2).Equals("S5"))
        //        {
        //            lineNum += 1;
        //        }
        //    }
        //    int index = 0;
        //    Convert.ToByte("0x" + line[0].Substring(index, 2));
        //}

    }

    class S19Line
    {
        public uint lineAddress;
        public byte num;
        public byte[] date;
        public byte sumCheck;
    }
}
