using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using USBCAN.Burn;
using System.IO;
using USBCAN.Device;

namespace USBCAN
{
    public partial class FormMain : Form
    {
        IDictionary carSelected = null;
        Flash flash = null;
        HexS19 s19 = new HexS19();
        FileBoxItem flashDriverDefaultPath = new FileBoxItem(Flash.DriverName);
        System.Collections.Generic.List<string> fileList = new System.Collections.Generic.List<string>();
        private string sha1 = null;

        //DeviceControl dc;
        ZLGCANJson zlgCan;
        ConfigJson config;
        Car carConfig;
        FlashControl flashControl;

        public FormMain()
        {
            InitializeComponent();
            //string json;
            //using (StreamReader sr = new StreamReader("json/car/N330.json"))
            //{
            //    json = sr.ReadToEnd();
            //}
            //Car c = JsonConvert.DeserializeObject<Car>(JObject.Parse(json).ToString());
            //JsonConvert.DeserializeObject(json);
            //JObject o = JObject.Parse(json);
            //JToken jt = o["process"];
            //string[] sa = JsonConvert.DeserializeObject<string[]>(jt.ToString());
            //using (StreamReader sr = new StreamReader("process.json"))
            //{
            //    json = sr.ReadToEnd();
            //}
            //o = JObject.Parse(json);
            //jt = o[sa[0]];
            //Type t = Type.GetType("JsonConvert");
            //Type d = Type.GetType(sa[0]);
            //MethodInfo mi = t.GetMethod("DeserializeObject").MakeGenericMethod(d);
            //UDSDiagnosticControl ms = JsonConvert.DeserializeObject<UDSDiagnosticControl>(jt.ToString());
            ////var ms = mi.Invoke(Activator.CreateInstance(t), new object[] { jt.ToString() });
            //byte a = ((UDSMessage)ms).ServiceId;
        }

        void ComboBoxItemsAddRange(string[] range)
        {
            Invoke(new MethodInvoker(delegate
            {
                comboBox_Config.Items.AddRange(range);
            }));
            
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            flashControl = new FlashControl();
            flashControl.comboBoxItemsAddRange += new FlashControl.ComboBoxItemsAddRange(ComboBoxItemsAddRange);
            flashControl.MainStart();

            s19.updata += new HexS19.Updata(updataFileBox);
            if (CanControl.canConnect())
            {
                toolStripStatusLabel_CAN.Text = "CAN : true";
            }
            else if (ZLGCAN.ERR.ContainsKey(CanControl.errorInfo.ErrCode))
            {
                MessageBox.Show(ZLGCAN.ERR[CanControl.errorInfo.ErrCode], "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            sha1 = BitConverter.ToString(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(File.OpenRead(Flash.DriverName)));
            if (System.IO.File.Exists(Flash.DriverName) && sha1.Equals(Flash.flashSha1))
            {
                s19.syncFilesWithUI(1, -1, new string[1] { Flash.DriverName });
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CanControl.canClose();
        }

        private void button_Flash_Click(object sender, EventArgs e)
        {
            if (!CanControl.canConnect())
            {
                MessageBox.Show("打开设备失败,请检查设备已连接并没有被其他程序占用", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if (ZLGCAN.ERR.ContainsKey(CanControl.errorInfo.ErrCode))
                {
                    MessageBox.Show(ZLGCAN.ERR[CanControl.errorInfo.ErrCode], "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return;
            }
            else
            {
                toolStripStatusLabel_CAN.Text = "CAN : true";
            }

            if (s19.Count == 0)
            {
                MessageBox.Show("请选择s19文件", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (flash == null)
            {
                MessageBox.Show("请选择车型", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            flash.go();
        }

        private void comboBox_Config_Click(object sender, EventArgs e)
        {
            flash = null;
            //comboBox_Config.Items.Clear();
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //int confnum = config.GetSectionGroup("CarConfig").Sections.Count;
            //for (int index = 0; index < confnum; index++)
            //{
            //    comboBox_Config.Items.Add(config.GetSectionGroup("CarConfig").Sections.GetKey(index));
            //}
        }

        private void comboBox_Config_SelectedIndexChanged(object sender, EventArgs e)
        {
            string car = (string)comboBox_Config.SelectedItem;
            lock (flashControl.CarSelected)
            {
                flashControl.CarSelected = car;
            }
            //try
            //{
            //    carConfig = FlashConfig.ParseCar(car, "json/process.json", config);
            //}
            //catch (FileNotFoundException)
            //{
            //    MessageBox.Show("无法找到配置文件，请检查", "错误",
            //                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            //无法找到车辆json配置文件时
            //catch (ArgumentException)
            //{
            //    MessageBox.Show("配置文件参数缺失，请检查", "错误",
            //                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            
            try
            {
                carSelected = (IDictionary)ConfigurationManager.GetSection("CarConfig/" + car);
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("配置文件错误，请检查", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            flash = new Flash(carSelected, s19);
            flash.updata += new Flash.Updata(updataUI);

            if (carSelected["FlashDriver"] != null)
            {
                string flashDriverConfig = carSelected["FlashDriver"].ToString().ToLower();
                if (FileBox.Items.Count == 0 || ((FileBoxItem)FileBox.Items[0]).FilePath != Flash.DriverName)
                {
                    switch (flashDriverConfig)
                    {
                        case "true":
                            if (System.IO.File.Exists(Flash.DriverName))
                            {
                                sha1 = BitConverter.ToString(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(System.IO.File.OpenRead(Flash.DriverName)));
                                if (!sha1.Equals(Flash.flashSha1))
                                {
                                    MessageBox.Show("默认FlashDriver文件可能被修改，请检查", "错误",
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                                s19.syncFilesWithUI(2, 0, new string[1] { Flash.DriverName });
                            }
                            else
                            {
                                MessageBox.Show("未在当前目录下发现FlashDriver，请手动添加", "错误",
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            break;
                        case "false":
                            break;
                        default:
                            if (!System.IO.File.Exists(flashDriverConfig))
                            {
                                MessageBox.Show("未发现指定文件，请确认路径是否正确", "错误",
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            }
                            if (FileBox.Items.Count == 0)
                            {
                                s19.syncFilesWithUI(2, 0, new string[1] { flashDriverConfig });
                            }
                            else
                            {
                                if (flashDriverConfig.Contains("\\"))
                                {
                                    if (((FileBoxItem)FileBox.Items[0]).FilePath.ToLower() != flashDriverConfig)
                                    {
                                        s19.syncFilesWithUI(2, 0, new string[1] { flashDriverConfig });
                                    }
                                }
                                else if (FileBox.Items[0].ToString().ToLower() != flashDriverConfig)
                                {
                                    s19.syncFilesWithUI(2, 0, new string[1] { flashDriverConfig });
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (flashDriverConfig)
                    {
                        case "false":
                            s19.syncFilesWithUI(-1, 0);
                            break;
                        case "true":
                            break;
                        default:
                            if (!System.IO.File.Exists(flashDriverConfig))
                            {
                                MessageBox.Show("未发现指定文件，请确认路径是否正确", "错误",
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            }
                            s19.syncFilesWithUI(-1, 0);
                            s19.syncFilesWithUI(2, 0, new string[1] { flashDriverConfig });
                            break;
                    }
                }
            }
            else
            {
                if ((FileBox.Items.Count == 0 || ((FileBoxItem)FileBox.Items[0]).FilePath != Flash.DriverName)
                    && System.IO.File.Exists(Flash.DriverName))
                {
                    sha1 = BitConverter.ToString(new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(System.IO.File.OpenRead(Flash.DriverName)));
                    if (!sha1.Equals(Flash.flashSha1))
                    {
                        MessageBox.Show("默认FlashDriver文件可能被修改，请检查", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    s19.syncFilesWithUI(2, 0, new string[1] { Flash.DriverName });
                }
            }

            textBox_Version_Click(textBox_Version, e);
        }

        private void button_LoadS19_Click(object sender, EventArgs e)
        {
            openS19Dialog.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] files = openS19Dialog.FileNames;
            if (!checkRepeatFiles(files))
            {
                return;
            }
            openS19Dialog.InitialDirectory = files[0].Substring(0, files[0].LastIndexOfAny("\\".ToCharArray()));
            s19.syncFilesWithUI(1, -1, files);
        }

        private void FileBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (!checkRepeatFiles(files))
                {
                    return;
                }
                s19.syncFilesWithUI(1, -1, files);
            }
        }

        private void FileBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void FileBox_MouseDown(object sender, MouseEventArgs e)
        {
            int index = ((ListBox)sender).IndexFromPoint(e.X, e.Y);

            if ((index & 0xFFFFFFFF) == 0xFFFFFFFF || e.Button == MouseButtons.Middle)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                s19.syncFilesWithUI(-1, index);
            }
            else
            {
                (sender as ListBox).DoDragDrop((sender as ListBox).Items[index], DragDropEffects.Move);
                //DragDropEffects dde1 = DoDragDrop((sender as ListBox).Items[index], DragDropEffects.Move);
            }
        }

        private void textBox_Version_Click(object sender, EventArgs e)
        {
            if (flash != null)
            {
                textBox_Version.Text = flash.readVersion();
            }
            else
            {
                textBox_Version.Text = "";
            }
        }

        private void updataUI(int cmd, string msg = null, int processValue = 0, string msg2 = null)
        {
            Invoke(new MethodInvoker(delegate
            {
                switch (cmd)
                {
                    case 1:
                        listBox.Items.Insert(listBox.Items.Count, msg);
                        listBox.SelectedIndex = listBox.Items.Count - 1;
                        listBox.Refresh();
                        break;
                    case 2:
                        MessageBox.Show(msg, "INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 3:
                        toolStripProgressBar_Flash.Increment(processValue);
                        break;
                    case 4:
                        toolStripProgressBar_Flash.Value = processValue > 100 ? 100 : processValue;
                        break;
                    case 5:
                        listBox.Items.Insert(listBox.Items.Count, msg);
                        listBox.SelectedIndex = listBox.Items.Count - 1;
                        listBox.Refresh();
                        MessageBox.Show(msg2, "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case -1:
                        listBox.Items.Clear();
                        toolStripProgressBar_Flash.Value = 0;
                        textBox_Version.Text = flash.readVersion();
                        textBox_Version.Enabled = false;
                        FileBox.Enabled = false;
                        button_LoadS19.Enabled = false;
                        comboBox_Config.Enabled = false;
                        toolStripMenuI_Start.Enabled = false;
                        checkBox_Log.Enabled = false;
                        break;
                    case 0:
                        textBox_Version.Enabled = true;
                        FileBox.Enabled = true;
                        button_LoadS19.Enabled = true;
                        comboBox_Config.Enabled = true;
                        toolStripMenuI_Start.Enabled = true;
                        checkBox_Log.Enabled = true;
                        checkBox_Log.Checked = false;
                        CanControl.log = false;
                        textBox_Version.Text = flash.readVersion();
                        break;
                    default:
                        break;
                }

            }));
        }

        private void updataFileBox(int cmd, string filePath, int index = 0)
        {
            Invoke(new MethodInvoker(delegate
            {
                switch (cmd)
                {
                    case 1:
                        FileBox.Items.Add(new FileBoxItem(filePath));
                        break;
                    case 2:
                        FileBox.Items.Insert(index, new FileBoxItem(filePath));
                        break;
                    case -1:
                        FileBox.Items.RemoveAt(index);
                        break;
                    case -2:
                        FileBox.Items.Clear();
                        break;
                }
            }));
        }

        private void toolStripMenuItem_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("联系：cwbhhjl@gmail.com\n\n" + "软件版本：" + Application.ProductVersion.ToString() +
                "\n\nIcons made by Freepik (http://www.freepik.com) , is licensed by CC 3.0 (http://creativecommons.org/licenses/by/3.0/)",
                "关于", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DEVICECHANGE = 0x219;
            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    if (CanControl.IsOpen)
                    {
                        if (CanControl.readBoardInfo() == 0)
                        {
                            toolStripStatusLabel_CAN.Text = CanControl.canConnect() ? "CAN : true" : "CAN : false";
                            CanControl.canClose();
                        }
                    }
                    else
                    {
                        //CanControl.canClose();
                        if (CanControl.canConnect())
                        {
                            toolStripStatusLabel_CAN.Text = "CAN : true";
                            textBox_Version_Click(textBox_Version, new EventArgs());
                        }
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void toolStripMenuItem_Reset_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel_CAN.Text = CanControl.canReInit() == 1 ? "CAN : true" : "CAN : false";
        }

        private void checkBox_Log_Click(object sender, EventArgs e)
        {
            CanControl.log = checkBox_Log.Checked ? true : false;
        }

        private bool checkRepeatFiles(string[] files)
        {
            fileList.Clear();
            foreach (var it in FileBox.Items)
            {
                fileList.Add(((FileBoxItem)it).FilePath);
            }
            foreach (var a in files)
            {
                if (fileList.Contains(a))
                {
                    MessageBox.Show("含有重复文件，请重新选择", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            return true;
        }

        private void toolStripMenuItem_FileReset_Click(object sender, EventArgs e)
        {
            s19.syncFilesWithUI(-2, 0);
        }
    }

    class FileBoxItem
    {
        private string fileName = null;

        public string FilePath { get; }

        override public string ToString()
        {
            return fileName;
        }

        internal FileBoxItem(string filePath)
        {
            FilePath = filePath;
            fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
        }
    }
}