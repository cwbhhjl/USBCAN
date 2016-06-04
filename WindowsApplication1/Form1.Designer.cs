namespace WindowsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox_Info = new System.Windows.Forms.ListBox();
            this.button_LoadS19 = new System.Windows.Forms.Button();
            this.button_Flash = new System.Windows.Forms.Button();
            this.comboBox_Config = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // listBox_Info
            // 
            this.listBox_Info.FormattingEnabled = true;
            this.listBox_Info.ItemHeight = 12;
            this.listBox_Info.Location = new System.Drawing.Point(12, 28);
            this.listBox_Info.Name = "listBox_Info";
            this.listBox_Info.Size = new System.Drawing.Size(624, 172);
            this.listBox_Info.TabIndex = 0;
            // 
            // button_LoadS19
            // 
            this.button_LoadS19.Location = new System.Drawing.Point(435, 46);
            this.button_LoadS19.Name = "button_LoadS19";
            this.button_LoadS19.Size = new System.Drawing.Size(75, 23);
            this.button_LoadS19.TabIndex = 8;
            this.button_LoadS19.Text = "加载S19";
            this.button_LoadS19.UseVisualStyleBackColor = true;
            this.button_LoadS19.Click += new System.EventHandler(this.button_LoadS19_Click);
            // 
            // button_Flash
            // 
            this.button_Flash.Location = new System.Drawing.Point(583, 47);
            this.button_Flash.Name = "button_Flash";
            this.button_Flash.Size = new System.Drawing.Size(53, 23);
            this.button_Flash.TabIndex = 8;
            this.button_Flash.Text = "烧写";
            this.button_Flash.UseVisualStyleBackColor = true;
            this.button_Flash.Click += new System.EventHandler(this.button_Flash_Click);
            // 
            // comboBox_Config
            // 
            this.comboBox_Config.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Config.FormattingEnabled = true;
            this.comboBox_Config.Location = new System.Drawing.Point(516, 49);
            this.comboBox_Config.Name = "comboBox_Config";
            this.comboBox_Config.Size = new System.Drawing.Size(61, 20);
            this.comboBox_Config.TabIndex = 9;
            this.comboBox_Config.SelectedIndexChanged += new System.EventHandler(this.comboBox_Config_SelectedIndexChanged);
            this.comboBox_Config.Click += new System.EventHandler(this.comboBox_Config_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_LoadS19);
            this.groupBox3.Controls.Add(this.comboBox_Config);
            this.groupBox3.Controls.Add(this.button_Flash);
            this.groupBox3.Location = new System.Drawing.Point(13, 29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(642, 394);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "发送数据帧";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(666, 461);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "USBCAN2-FLASH";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox_Info;
        private System.Windows.Forms.Button button_LoadS19;
        private System.Windows.Forms.Button button_Flash;
        private System.Windows.Forms.ComboBox comboBox_Config;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}

