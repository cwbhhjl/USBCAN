using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;

namespace USBCAN
{
    public partial class FormMain : Form
    {
        IDictionary carSelected = null;
        Flash flash;
        HexS19 s19 = new HexS19();
        OpenFileDialog openS19Dialog = new OpenFileDialog();

        public FormMain()
        {
            InitializeComponent();

            openS19Dialog.InitialDirectory = Environment.CurrentDirectory;
            openS19Dialog.Title = "请选择您要烧写的文件";
            openS19Dialog.Filter = "文本文件 (*.txt)|*.txt|S19 文件 (*.s19)|*.s19|所有文件 (*.*)|*.*";
            openS19Dialog.FilterIndex = 2;
            openS19Dialog.AddExtension = true;
            openS19Dialog.RestoreDirectory = true;
            openS19Dialog.CheckFileExists = true;
            openS19Dialog.CheckPathExists = true;
            openS19Dialog.Multiselect = true;
            openS19Dialog.FileOk += new System.ComponentModel.CancelEventHandler(openFileDialog1_FileOk);
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
            }

            if (s19.S19Block == null)
            {
                MessageBox.Show("请选择s19文件", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void comboBox_Config_Click(object sender, EventArgs e)
        {
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
            flash = new Flash(carSelected);
            flash.init();
            flash.flashThread.Start();
        }

        private void button_LoadS19_Click(object sender, EventArgs e)
        {
            openS19Dialog.ShowDialog();
            /*
            if (openS19Dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openS19Dialog.OpenFile()) != null)
                    {
                        myStream.Close(); 
                        if (s19.readFile(openS19Dialog.FileName) == 1)
                        {                           
                            s19.lineToBlock();                           
                        }
                        else
                        {
                            MessageBox.Show("S19文件校验和验证失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            */
        }

        private void openFileDialog1_FileOk(object sender,System.ComponentModel.CancelEventArgs e)
        {
            string[] files = openS19Dialog.FileNames;
            openS19Dialog.InitialDirectory = files[0].Substring(0, files[0].LastIndexOfAny("\\".ToCharArray()));
            s19.addFile(files);
        }
    }
}