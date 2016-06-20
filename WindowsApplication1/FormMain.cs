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

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CanControl.canConnect();
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

            if (s19.S19Block == null)
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

            if (!flash.flashThread.IsAlive)
            {
                flash.flashThread.Start();
            }
        }

        private void comboBox_Config_Click(object sender, EventArgs e)
        {
            flash = null;
            comboBox_Config.Items.Clear();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
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
            flash = new Flash(carSelected, s19);
            flash.init();
        }

        private void button_LoadS19_Click(object sender, EventArgs e)
        {
            openS19Dialog.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] files = openS19Dialog.FileNames;
            openS19Dialog.InitialDirectory = files[0].Substring(0, files[0].LastIndexOfAny("\\".ToCharArray()));
            s19.addFile(files);
            s19.wakeUpHexThread();
        }

        private void FileBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
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
            }
            else
            {
                (sender as ListBox).DoDragDrop((sender as ListBox).Items[index], DragDropEffects.Move);
                //DragDropEffects dde1 = DoDragDrop((sender as ListBox).Items[index], DragDropEffects.Move);
            }
        }
    }

    class FileBoxItem
    {
        private string fileName = null;
        private string filePath = null;

        public string FilePath
        {
            get
            {
                return filePath;
            }
        }

        override public string ToString()
        {
            return fileName;
        }

        internal FileBoxItem(string filePath)
        {
            this.filePath = filePath;
            fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
        }
    }
}