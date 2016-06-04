using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Collections;
using System.IO;


namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        IDictionary carSelected = null;
        Flash flash;
        HexS19 s19 = new HexS19();

        public static VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[50];

        public uint[] m_arrdevtype = new uint[20];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 11;
            comboBox_DevIndex.SelectedIndex = 0;
            comboBox_CANIndex.SelectedIndex = 0;
            textBox_AccCode.Text = "00000000";
            textBox_AccMask.Text = "FFFFFFFF";
            textBox_Time0.Text = "00";
            textBox_Time1.Text = "1C";
            comboBox_Filter.SelectedIndex = 1;
            comboBox_Mode.SelectedIndex = 0;
            comboBox_SendType.SelectedIndex = 2;
            comboBox_FrameFormat.SelectedIndex = 0;
            comboBox_FrameType.SelectedIndex = 0;
            textBox_ID.Text = "00000741";
            textBox_Data.Text = "00 01 02 03 04 05 06 07 ";

            //
            Int32 curindex = 0;
            comboBox_devtype.Items.Clear();

            curindex = comboBox_devtype.Items.Add("VCI_PCI5121");
            m_arrdevtype[curindex] = CanControl.VCI_PCI5121;
            //comboBox_devtype.Items[0] = "VCI_PCI5121";
            //m_arrdevtype[0]=  VCI_PCI5121 ;

            curindex = comboBox_devtype.Items.Add("VCI_PCI9810");
            m_arrdevtype[curindex] = CanControl.VCI_PCI9810;
            //comboBox_devtype.Items[1] = "VCI_PCI9810";
            //m_arrdevtype[1]=  VCI_PCI9810 ;

            curindex = comboBox_devtype.Items.Add("VCI_USBCAN1(I+)");
            m_arrdevtype[curindex] = CanControl.VCI_USBCAN1;
            //comboBox_devtype.Items[2] = "VCI_USBCAN1";
            //m_arrdevtype[2]=  VCI_USBCAN1 ;

            curindex = comboBox_devtype.Items.Add("VCI_USBCAN2(II+)");
            m_arrdevtype[curindex] = CanControl.VCI_USBCAN2;
            //comboBox_devtype.Items[3] = "VCI_USBCAN2";
            //m_arrdevtype[3]=  VCI_USBCAN2 ;

            curindex = comboBox_devtype.Items.Add("VCI_USBCAN2A");
            m_arrdevtype[curindex] = CanControl.VCI_USBCAN2A;
            //comboBox_devtype.Items[4] = "VCI_USBCAN2A";
            //m_arrdevtype[4]=  VCI_USBCAN2A ;

            curindex = comboBox_devtype.Items.Add("VCI_PCI9820");
            m_arrdevtype[curindex] = CanControl.VCI_PCI9820;
            //comboBox_devtype.Items[5] = "VCI_PCI9820";
            //m_arrdevtype[5]=  VCI_PCI9820 ;

            curindex = comboBox_devtype.Items.Add("VCI_PCI5110");
            m_arrdevtype[curindex] = CanControl.VCI_PCI5110;
            //comboBox_devtype.Items[6] = "VCI_PCI5110";
            //m_arrdevtype[6]=  VCI_PCI5110 ;

            curindex = comboBox_devtype.Items.Add("VCI_CANLITE");
            m_arrdevtype[curindex] = CanControl.VCI_CANLITE;

            curindex = comboBox_devtype.Items.Add("VCI_ISA9620");
            m_arrdevtype[curindex] = CanControl.VCI_ISA9620;
            //comboBox_devtype.Items[7] = "VCI_ISA9620";
            //m_arrdevtype[7]=  VCI_ISA9620 ;

            curindex = comboBox_devtype.Items.Add("VCI_ISA5420");
            m_arrdevtype[curindex] = CanControl.VCI_ISA5420;
            //comboBox_devtype.Items[8] = "VCI_ISA5420";
            //m_arrdevtype[8]=  VCI_ISA5420 ;

            curindex = comboBox_devtype.Items.Add("VCI_PC104CAN");
            m_arrdevtype[curindex] = CanControl.VCI_PC104CAN;
            //comboBox_devtype.Items[9] = "VCI_PC104CAN";
            //m_arrdevtype[9]=  VCI_PC104CAN ;

            curindex = comboBox_devtype.Items.Add("VCI_DNP9810");
            m_arrdevtype[curindex] = CanControl.VCI_DNP9810;
            //comboBox_devtype.Items[10] = "VCI_DNP9810";
            //m_arrdevtype[10]=  VCI_DNP9810 ;

            curindex = comboBox_devtype.Items.Add("VCI_PCI9840");
            m_arrdevtype[curindex] = CanControl.VCI_PCI9840;
            //comboBox_devtype.Items[11] = "VCI_PCI9840";
            //m_arrdevtype[11]=   VCI_PCI9840;

            curindex = comboBox_devtype.Items.Add("VCI_PC104CAN2");
            m_arrdevtype[curindex] = CanControl.VCI_PC104CAN2;
            //comboBox_devtype.Items[12] = "VCI_PC104CAN2";
            //m_arrdevtype[12]=  VCI_PC104CAN2 ;

            curindex = comboBox_devtype.Items.Add("VCI_PCI9820I");
            m_arrdevtype[curindex] = CanControl.VCI_PCI9820I;
            //comboBox_devtype.Items[13] = "VCI_PCI9820I";
            //m_arrdevtype[13]=  VCI_PCI9820I ;

            curindex = comboBox_devtype.Items.Add("VCI_PEC9920");
            m_arrdevtype[curindex] = CanControl.VCI_PEC9920;

            curindex = comboBox_devtype.Items.Add("VCI_PCIE9221");
            m_arrdevtype[curindex] = CanControl.VCI_PCIE9221;
            //comboBox_devtype.Items[14] = "VCI_PEC9920";
            //m_arrdevtype[14]= VCI_PEC9920  ;


            comboBox_devtype.SelectedIndex = 3;
            comboBox_devtype.MaxDropDownItems = comboBox_devtype.Items.Count;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CanControl.canClose();
            //CanControl.canLog.makeLog();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (CanControl.isOpen == 1)
            {
                CanControl.VCI_CloseDevice(CanControl.deviceType, CanControl.deviceIndex);
                CanControl.isOpen = 0;
            }
            else
            {
                CanControl.deviceType = m_arrdevtype[comboBox_devtype.SelectedIndex];
                CanControl.deviceIndex = (uint)comboBox_DevIndex.SelectedIndex;
                CanControl.canIndex = (uint)comboBox_CANIndex.SelectedIndex;
                if (CanControl.VCI_OpenDevice(CanControl.deviceType, CanControl.deviceIndex, 0) == 0)
                {
                    MessageBox.Show("打开设备失败,请检查设备类型和设备索引号是否正确", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                CanControl.isOpen = 1;
                VCI_INIT_CONFIG config = new VCI_INIT_CONFIG();
                config.AccCode = System.Convert.ToUInt32("0x" + textBox_AccCode.Text, 16);
                config.AccMask = System.Convert.ToUInt32("0x" + textBox_AccMask.Text, 16);
                config.Timing0 = System.Convert.ToByte("0x" + textBox_Time0.Text, 16);
                config.Timing1 = System.Convert.ToByte("0x" + textBox_Time1.Text, 16);
                config.Filter = (byte)comboBox_Filter.SelectedIndex;
                config.Mode = (byte)comboBox_Mode.SelectedIndex;
                CanControl.VCI_InitCAN(CanControl.deviceType, CanControl.deviceIndex, CanControl.canIndex, ref config);
            }
            buttonConnect.Text = CanControl.isOpen == 1 ? "断开" : "连接";
            //timer_rec.Enabled = CanControl.m_bOpen == 1 ? true : false;
        }

        unsafe private void timer_rec_Tick(object sender, EventArgs e)
        {
            uint res = new uint();
            res = CanControl.VCI_GetReceiveNum(CanControl.deviceType, CanControl.deviceIndex, CanControl.canIndex);
            if (res == 0)
                return;
            //res = VCI_Receive(m_devtype, m_devind, m_canind, ref m_recobj[0],50, 100);

            /////////////////////////////////////
            uint con_maxlen = 50;
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (int)con_maxlen);

            res = CanControl.VCI_Receive(CanControl.deviceType, CanControl.deviceIndex, CanControl.canIndex, pt, con_maxlen, 100);
            ////////////////////////////////////////////////////////

            String str = "";
            for (uint i = 0; i < res; i++)
            {
                VCI_CAN_OBJ obj = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((uint)pt + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));

                str = "接收到数据: ";
                str += "  帧ID:0x" + System.Convert.ToString((int)obj.ID, 16);
                str += "  帧格式:";
                if (obj.RemoteFlag == 0)
                    str += "数据帧 ";
                else
                    str += "远程帧 ";
                if (obj.ExternFlag == 0)
                    str += "标准帧 ";
                else
                    str += "扩展帧 ";

                //////////////////////////////////////////
                if (obj.RemoteFlag == 0)
                {
                    str += "数据: ";
                    byte len = (byte)(obj.DataLen % 9);
                    byte j = 0;
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[0], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[1], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[2], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[3], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[4], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[5], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[6], 16);
                    if (j++ < len)
                        str += " " + System.Convert.ToString(obj.Data[7], 16);

                }

                listBox_Info.Items.Add(str);
                listBox_Info.SelectedIndex = listBox_Info.Items.Count - 1;
            }
            Marshal.FreeHGlobal(pt);
        }

        private void button_StartCAN_Click(object sender, EventArgs e)
        {
            if (CanControl.isOpen == 0)
                return;
            CanControl.VCI_StartCAN(CanControl.deviceType, CanControl.deviceIndex, CanControl.canIndex);
        }

        private void button_StopCAN_Click(object sender, EventArgs e)
        {
            if (CanControl.isOpen == 0)
                return;
            CanControl.VCI_ResetCAN(CanControl.deviceType, CanControl.deviceIndex, CanControl.canIndex);
        }

        unsafe private void button_Send_Click(object sender, EventArgs e)
        {
            if (CanControl.isOpen == 0)
                return;
            int num = int.Parse(textBox1.Text);
            VCI_CAN_OBJ[] sendobj = new VCI_CAN_OBJ[num];//sendobj.Init();
            for (int j = 0; j < sendobj.Length; j++)
            {
                sendobj[j].SendType = (byte)comboBox_SendType.SelectedIndex;
                sendobj[j].RemoteFlag = (byte)comboBox_FrameFormat.SelectedIndex;
                sendobj[j].ExternFlag = (byte)comboBox_FrameType.SelectedIndex;
                sendobj[j].ID = System.Convert.ToUInt32("0x" + textBox_ID.Text, 16);
                int len = (textBox_Data.Text.Length + 1) / 3;
                sendobj[j].DataLen = System.Convert.ToByte(len);
                string strdata = textBox_Data.Text;
                int i = -1;
                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[0] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[1] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[2] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[3] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[4] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[5] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[6] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
                    }

                if (i++ < len - 1)
                    fixed (VCI_CAN_OBJ* sendobjs = &sendobj[0])
                    {
                        sendobjs[j].Data[7] = System.Convert.ToByte("0x" + strdata.Substring(i * 3, 2), 16);
                    }
            }

            uint res = CanControl.VCI_Transmit(CanControl.deviceType, CanControl.deviceIndex, CanControl.canIndex, ref sendobj[0], (uint)num);
            if (res == 0)
            {
                MessageBox.Show("发送失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void textBox_AccCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBox1.SelectedIndex;
            switch (i)
            {
                case 0:
                    {
                        textBox_Time0.Text = "bf";
                        textBox_Time1.Text = "ff";
                    }
                    break;
                case 1:
                    {
                        textBox_Time0.Text = "31";
                        textBox_Time1.Text = "1c";
                    }
                    break;
                case 2:
                    {
                        textBox_Time0.Text = "18";
                        textBox_Time1.Text = "1c";
                    }
                    break;
                case 3:
                    {
                        textBox_Time0.Text = "87";
                        textBox_Time1.Text = "ff";
                    }
                    break;
                case 4:
                    {
                        textBox_Time0.Text = "09";
                        textBox_Time1.Text = "1c";
                    }
                    break;
                case 5:
                    {
                        textBox_Time0.Text = "83";
                        textBox_Time1.Text = "ff";
                    }
                    break;
                case 6:
                    {
                        textBox_Time0.Text = "04";
                        textBox_Time1.Text = "1c";
                    }
                    break;
                case 7:
                    {
                        textBox_Time0.Text = "03";
                        textBox_Time1.Text = "1c";
                    }
                    break;
                case 8:
                    {
                        textBox_Time0.Text = "81";
                        textBox_Time1.Text = "fa";
                    }
                    break;
                case 9:
                    {
                        textBox_Time0.Text = "01";
                        textBox_Time1.Text = "1c";
                    }
                    break;
                case 10:
                    {
                        textBox_Time0.Text = "80";
                        textBox_Time1.Text = "fa";
                    }
                    break;
                case 11:
                    {
                        textBox_Time0.Text = "00";
                        textBox_Time1.Text = "1c";
                    }
                    break;
                case 12:
                    {
                        textBox_Time0.Text = "80";
                        textBox_Time1.Text = "b6";
                    }
                    break;
                case 13:
                    {
                        textBox_Time0.Text = "00";
                        textBox_Time1.Text = "16";
                    }
                    break;
                case 14:
                    {
                        textBox_Time0.Text = "00";
                        textBox_Time1.Text = "14";
                    }
                    break;

            }
        }

        private void button_Flash_Click(object sender, EventArgs e)
        {
            //textBox_Data.Text = carSelected["SoftwareVersion"].ToString();
            //button_Send_Click(sender, e);
            //return;
            flash.connect();
            if (s19.readFile(flash.DriverName) == 1)
            {
                s19.lineToBlock();
            }
            else
            {
                MessageBox.Show("S19文件校验和验证失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void comboBox_Config_Click(object sender, EventArgs e)
        {
            comboBox_Config.Items.Clear();
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            int confnum = config.GetSectionGroup("CarConfig").Sections.Count;
            //String[] carInfo=new String[confnum];
            for (int index = 0; index < confnum; index++)
            {
                //carInfo[index]= config.GetSectionGroup("CarConfig").Sections.GetKey(index);
                comboBox_Config.Items.Add(config.GetSectionGroup("CarConfig").Sections.GetKey(index));
            }
        }

        private void comboBox_Config_SelectedIndexChanged(object sender, EventArgs e)
        {
            string car = (string)comboBox_Config.SelectedItem;
            carSelected = (IDictionary)ConfigurationManager.GetSection("CarConfig/" + car);
            flash = new Flash(carSelected);
        }

        private void button_LoadS19_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openS19Dialog = new OpenFileDialog();

            openS19Dialog.InitialDirectory = System.Environment.CurrentDirectory;
            openS19Dialog.Filter = "文本文件 (*.txt)|*.txt|S19 文件 (*.s19)|*.s19";
            openS19Dialog.FilterIndex = 2;
            openS19Dialog.RestoreDirectory = true;

            if (openS19Dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openS19Dialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                            byte[] buf = new byte[2048];
                            UTF8Encoding temp = new UTF8Encoding(true);
                            myStream.Read(buf, 0, buf.Length);
                            string fileTemp = temp.GetString(buf);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}