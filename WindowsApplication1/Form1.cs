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

        public Form1()
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
                        if (s19.readFile(openS19Dialog.FileName) == 1)
                        {
                            s19.lineToBlock();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

    }
}