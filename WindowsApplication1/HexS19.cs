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
        string driverPath = Environment.CurrentDirectory + "\\FlashDriver_S12GX_V1.0.s19";
        string[] line;
        int lineNum = 0;
        ArrayList lineTmp = new ArrayList();

        public void readFile()
        {
            string tmp;
            using (FileStream fs = File.OpenRead(driverPath))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                while(!sr.EndOfStream)
                {
                    lineTmp.Add(sr.ReadLine());
                }
                sr.Close();
                line = (string[])lineTmp.ToArray(typeof(string));
            }
            for (int i = 0; i < lineTmp.Count; i++)
            {
                tmp = ((string)lineTmp[i]).Substring(0, 2);
                if (tmp.Equals("S0") || tmp.Equals("S7") || tmp.Equals("S8") || tmp.Equals("S9"))
                {
                    lineTmp.Remove(lineTmp[i]);
                    lineNum += 1;
                    i--;
                }
                if(tmp.Equals("S1"))
                {

                }
            }
            int index = 0;
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
        uint lineAddress;
        byte[] date;
    }
}
