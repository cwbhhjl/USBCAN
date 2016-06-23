using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;

namespace USBCAN
{
    public partial class FormMain : Form
    {
        IDictionary carSelected = null;
        Flash flash = null;
        HexS19 s19 = new HexS19();
        FileBoxItem flashDriverDefaultPath = new FileBoxItem(Flash.DriverName);
        System.Collections.Generic.List<string> fileList = new System.Collections.Generic.List<string>();

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (CanControl.canConnect())
            {
                toolStripStatusLabel_CAN.Text = "CAN : true";
            }
            
            FileBox.Items.Add(flashDriverDefaultPath);
            s19.syncFilesWithUI(1, -1, new string[1] { Flash.DriverName });
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CanControl.canClose();
            //CanControl.canLog.makeLog();
        }

        private void button_Flash_Click(object sender, EventArgs e)
        {
            if (!CanControl.canConnect())
            {
                MessageBox.Show("打开设备失败,请检查设备类型和设备索引号是否正确", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
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
            comboBox_Config.Items.Clear();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            int confnum = config.GetSectionGroup("CarConfig").Sections.Count;
            for (int index = 0; index < confnum; index++)
            {
                comboBox_Config.Items.Add(config.GetSectionGroup("CarConfig").Sections.GetKey(index));
            }
        }

        private void comboBox_Config_SelectedIndexChanged(object sender, EventArgs e)
        {
            string car = (string)comboBox_Config.SelectedItem;
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
                            FileBox.Items.Insert(0, flashDriverDefaultPath);
                            s19.syncFilesWithUI(2, 0, new string[1] { Flash.DriverName });
                            break;
                        case "false":
                            break;
                        default:
                            if (FileBox.Items.Count == 0 || (FileBox.Items[0].ToString() != flashDriverConfig && ((FileBoxItem)FileBox.Items[0]).FilePath != Flash.DriverName))
                            {
                                FileBox.Items.Insert(0, flashDriverConfig);
                                s19.syncFilesWithUI(2, 0, new string[1] { flashDriverConfig });
                            }
                            else if (((FileBoxItem)FileBox.Items[0]).FilePath == Flash.DriverName)
                            {
                                FileBox.Items.RemoveAt(0);
                                s19.syncFilesWithUI(-1, 0);
                                FileBox.Items.Add(new FileBoxItem(flashDriverConfig));
                                s19.syncFilesWithUI(1, -1, new string[1] { flashDriverConfig });
                            }
                            break;
                    }
                }
                else if (flashDriverConfig == "false")
                {
                    FileBox.Items.RemoveAt(0);
                    s19.syncFilesWithUI(-1, 0);
                }
            }
            else
            {
                if (FileBox.Items.Count == 0 || ((FileBoxItem)FileBox.Items[0]).FilePath != Flash.DriverName)
                {
                    FileBox.Items.Insert(0, flashDriverDefaultPath);
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
            fileList.Clear();
            foreach (var it in FileBox.Items)
            {
                fileList.Add(((FileBoxItem)it).FilePath);
            }
            string[] files = openS19Dialog.FileNames;
            foreach (var a in files)
            {
                if (fileList.Contains(a))
                {
                    MessageBox.Show("含有重复文件，请重新选择", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            openS19Dialog.InitialDirectory = files[0].Substring(0, files[0].LastIndexOfAny("\\".ToCharArray()));
            s19.syncFilesWithUI(1, -1, files);
            foreach (string s in files)
            {
                if (s.EndsWith(".s19") || s.EndsWith(".S19"))
                {
                    FileBox.Items.Add(new FileBoxItem(s));
                }
            }
        }

        private void FileBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                s19.syncFilesWithUI(1, -1, files);
                foreach (string s in files)
                {
                    if (s.EndsWith(".s19") || s.EndsWith(".S19"))
                    {
                        (sender as ListBox).Items.Add(new FileBoxItem(s));
                    }
                }
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
                (sender as ListBox).Items.RemoveAt(index);
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
                        break;
                    case 0:
                        textBox_Version.Enabled = true;
                        FileBox.Enabled = true;
                        button_LoadS19.Enabled = true;
                        comboBox_Config.Enabled = true;
                        toolStripMenuI_Start.Enabled = true;
                        textBox_Version.Text = flash.readVersion();
                        break;
                    default:
                        break;
                }

            }));
        }

        private void toolStripMenuItem_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("联系：cwbhhjl@gmail.com\n\n" + "软件版本：" + Application.ProductVersion.ToString() +
                "\n\nIcons made by Freepik(http://www.freepik.com), is licensed by CC 3.0(http://creativecommons.org/licenses/by/3.0/)",
                "关于", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DEVICECHANGE = 0x219;
            //const int WM_DEVICEARRVIAL = 0x8000;//如果m.Msg的值为0x8000那么表示有U盘插入
            //const int WM_DEVICEMOVECOMPLETE = 0x8004;
            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    //if(waitUSB == 0)
                    //{
                    //    toolStripStatusLabel_CAN.Text = CanControl.canConnect() ? "CAN : true" : "CAN : false";
                    //}
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
                        }
                    }
                    break;
            }
            base.WndProc(ref m);
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