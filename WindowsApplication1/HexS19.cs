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
        ArrayList lineTmp = new ArrayList();

        public void readFile()
        {
            using (FileStream fs = File.OpenRead(driverPath))
            {
                //byte[] b = new byte[1024];
                //UTF8Encoding temp = new UTF8Encoding(true);
                //while (fs.Read(b, 0, b.Length) > 0)
                //{
                //    filetmp += temp.GetString(b);
                //}
                StreamReader sr = new StreamReader(fs, UnicodeEncoding.Default);
                while(!sr.EndOfStream)
                {
                    lineTmp.Add(sr.ReadLine());
                    //filetmp+=sr.ReadLine();
                }
                line = (string[])lineTmp.ToArray(typeof(string));
                char[] loc = { '\r', '\n' };
                //lineTmp = filetmp.Split(loc);
            }
        }

        //S19Line[] stringToHex(string[] tmp)
        //{
        //    for(int i=0;i<tmp.Length;i++)
        //    {
        //        switch (tmp[i].Substring(0, 2))
        //        {
        //            case "S0": break;
        //            case "S1":tmp[i].Substring(2, 2);
        //        }


        //        for (int j=0;j<tmp[i].Length;j+=2)
        //        {
                    
        //        }
        //    }
        //    int index = 0;
        //    tmp[0].Substring(index, 2).Equals("S0")


        //        Convert.ToByte("0x"+ tmp[0].Substring(index, 2));
        //}

    }

    class S19Line
    {
        uint lineAddress;
        byte[] date;
    }
}
