using System;

namespace USBCAN
{
    partial class FormMain
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
            System.Windows.Forms.Label label_car;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.button_LoadS19 = new System.Windows.Forms.Button();
            this.comboBox_Config = new System.Windows.Forms.ComboBox();
            this.openS19Dialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox_File = new System.Windows.Forms.GroupBox();
            this.FileBox = new System.Windows.Forms.ListBox();
            this.checkBox_Log = new System.Windows.Forms.CheckBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_CAN = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_Error = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_Flash = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar_Flash = new System.Windows.Forms.ToolStripProgressBar();
            this.listBox = new System.Windows.Forms.ListBox();
            this.label_Version = new System.Windows.Forms.Label();
            this.textBox_Version = new System.Windows.Forms.TextBox();
            this.button_Flash = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuI_Start = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Flash = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Version = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Reset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_FileReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_About = new System.Windows.Forms.ToolStripMenuItem();
            label_car = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox_File.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_car
            // 
            label_car.AutoSize = true;
            label_car.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            label_car.Location = new System.Drawing.Point(11, 6);
            label_car.Name = "label_car";
            label_car.Size = new System.Drawing.Size(65, 20);
            label_car.TabIndex = 13;
            label_car.Text = "车型选择";
            // 
            // button_LoadS19
            // 
            this.button_LoadS19.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_LoadS19.Location = new System.Drawing.Point(3, 3);
            this.button_LoadS19.Name = "button_LoadS19";
            this.button_LoadS19.Size = new System.Drawing.Size(207, 35);
            this.button_LoadS19.TabIndex = 8;
            this.button_LoadS19.Text = "加载烧写文件";
            this.button_LoadS19.UseVisualStyleBackColor = true;
            this.button_LoadS19.Click += new System.EventHandler(this.button_LoadS19_Click);
            // 
            // comboBox_Config
            // 
            this.comboBox_Config.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Config.FormattingEnabled = true;
            this.comboBox_Config.Location = new System.Drawing.Point(82, 6);
            this.comboBox_Config.Name = "comboBox_Config";
            this.comboBox_Config.Size = new System.Drawing.Size(81, 20);
            this.comboBox_Config.TabIndex = 9;
            this.comboBox_Config.SelectedIndexChanged += new System.EventHandler(this.comboBox_Config_SelectedIndexChanged);
            this.comboBox_Config.Click += new System.EventHandler(this.comboBox_Config_Click);
            // 
            // openS19Dialog
            // 
            this.openS19Dialog.Filter = "文本文件 (*.txt)|*.txt|S19 文件 (*.s19)|*.s19|所有文件 (*.*)|*.*";
            this.openS19Dialog.FilterIndex = 2;
            this.openS19Dialog.InitialDirectory = "D:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\Common7\\IDE";
            this.openS19Dialog.Multiselect = true;
            this.openS19Dialog.RestoreDirectory = true;
            this.openS19Dialog.Title = "请选择您要烧写的文件";
            this.openS19Dialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer.Location = new System.Drawing.Point(0, 28);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.groupBox_File);
            this.splitContainer.Panel1.Controls.Add(this.button_LoadS19);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.checkBox_Log);
            this.splitContainer.Panel2.Controls.Add(this.statusStrip);
            this.splitContainer.Panel2.Controls.Add(this.listBox);
            this.splitContainer.Panel2.Controls.Add(this.label_Version);
            this.splitContainer.Panel2.Controls.Add(label_car);
            this.splitContainer.Panel2.Controls.Add(this.textBox_Version);
            this.splitContainer.Panel2.Controls.Add(this.comboBox_Config);
            this.splitContainer.Panel2.Controls.Add(this.button_Flash);
            this.splitContainer.Size = new System.Drawing.Size(666, 416);
            this.splitContainer.SplitterDistance = 220;
            this.splitContainer.TabIndex = 7;
            // 
            // groupBox_File
            // 
            this.groupBox_File.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox_File.Controls.Add(this.FileBox);
            this.groupBox_File.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox_File.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_File.Location = new System.Drawing.Point(0, 47);
            this.groupBox_File.Name = "groupBox_File";
            this.groupBox_File.Size = new System.Drawing.Size(220, 369);
            this.groupBox_File.TabIndex = 9;
            this.groupBox_File.TabStop = false;
            this.groupBox_File.Text = "待烧写的文件";
            // 
            // FileBox
            // 
            this.FileBox.AllowDrop = true;
            this.FileBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileBox.FormattingEnabled = true;
            this.FileBox.HorizontalScrollbar = true;
            this.FileBox.ItemHeight = 17;
            this.FileBox.Location = new System.Drawing.Point(3, 19);
            this.FileBox.Name = "FileBox";
            this.FileBox.Size = new System.Drawing.Size(214, 347);
            this.FileBox.TabIndex = 0;
            this.FileBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileBox_DragDrop);
            this.FileBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileBox_DragEnter);
            this.FileBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FileBox_MouseDown);
            // 
            // checkBox_Log
            // 
            this.checkBox_Log.AutoSize = true;
            this.checkBox_Log.Location = new System.Drawing.Point(3, 47);
            this.checkBox_Log.Name = "checkBox_Log";
            this.checkBox_Log.Size = new System.Drawing.Size(42, 16);
            this.checkBox_Log.TabIndex = 17;
            this.checkBox_Log.Text = "log";
            this.checkBox_Log.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_CAN,
            this.toolStripStatusLabel_Error,
            this.toolStripStatusLabel_Flash,
            this.toolStripProgressBar_Flash});
            this.statusStrip.Location = new System.Drawing.Point(0, 390);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(442, 26);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 16;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_CAN
            // 
            this.toolStripStatusLabel_CAN.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel_CAN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel_CAN.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.toolStripStatusLabel_CAN.Name = "toolStripStatusLabel_CAN";
            this.toolStripStatusLabel_CAN.Size = new System.Drawing.Size(86, 21);
            this.toolStripStatusLabel_CAN.Text = "CAN：未连接";
            // 
            // toolStripStatusLabel_Error
            // 
            this.toolStripStatusLabel_Error.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel_Error.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel_Error.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.toolStripStatusLabel_Error.Name = "toolStripStatusLabel_Error";
            this.toolStripStatusLabel_Error.Size = new System.Drawing.Size(60, 21);
            this.toolStripStatusLabel_Error.Text = "错误：无";
            // 
            // toolStripStatusLabel_Flash
            // 
            this.toolStripStatusLabel_Flash.Margin = new System.Windows.Forms.Padding(20, 3, 0, 2);
            this.toolStripStatusLabel_Flash.Name = "toolStripStatusLabel_Flash";
            this.toolStripStatusLabel_Flash.Size = new System.Drawing.Size(56, 21);
            this.toolStripStatusLabel_Flash.Text = "刷写进度";
            // 
            // toolStripProgressBar_Flash
            // 
            this.toolStripProgressBar_Flash.Name = "toolStripProgressBar_Flash";
            this.toolStripProgressBar_Flash.Size = new System.Drawing.Size(190, 20);
            // 
            // listBox
            // 
            this.listBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 14;
            this.listBox.Location = new System.Drawing.Point(0, 66);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(436, 312);
            this.listBox.TabIndex = 15;
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Version.Location = new System.Drawing.Point(179, 6);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(51, 20);
            this.label_Version.TabIndex = 14;
            this.label_Version.Text = "版本号";
            // 
            // textBox_Version
            // 
            this.textBox_Version.Cursor = System.Windows.Forms.Cursors.Hand;
            this.textBox_Version.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBox_Version.Location = new System.Drawing.Point(236, 6);
            this.textBox_Version.Name = "textBox_Version";
            this.textBox_Version.ReadOnly = true;
            this.textBox_Version.ShortcutsEnabled = false;
            this.textBox_Version.Size = new System.Drawing.Size(74, 21);
            this.textBox_Version.TabIndex = 12;
            this.textBox_Version.TabStop = false;
            this.textBox_Version.Click += new System.EventHandler(this.textBox_Version_Click);
            // 
            // button_Flash
            // 
            this.button_Flash.BackColor = System.Drawing.Color.AliceBlue;
            this.button_Flash.FlatAppearance.BorderColor = System.Drawing.Color.LightCoral;
            this.button_Flash.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Flash.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Flash.Location = new System.Drawing.Point(353, 3);
            this.button_Flash.Name = "button_Flash";
            this.button_Flash.Size = new System.Drawing.Size(83, 35);
            this.button_Flash.TabIndex = 8;
            this.button_Flash.Text = "开始烧写";
            this.button_Flash.UseVisualStyleBackColor = true;
            this.button_Flash.Click += new System.EventHandler(this.button_Flash_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuStrip_Main);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 29);
            this.panel1.TabIndex = 8;
            // 
            // menuStrip_Main
            // 
            this.menuStrip_Main.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuI_Start,
            this.toolStripMenuItem_About});
            this.menuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Main.Name = "menuStrip_Main";
            this.menuStrip_Main.Size = new System.Drawing.Size(666, 25);
            this.menuStrip_Main.TabIndex = 0;
            this.menuStrip_Main.Text = "menuStrip1";
            // 
            // toolStripMenuI_Start
            // 
            this.toolStripMenuI_Start.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem,
            this.toolStripMenuItem_Flash,
            this.ToolStripMenuItem_Version,
            this.toolStripMenuItem_Reset,
            this.toolStripMenuItem_FileReset});
            this.toolStripMenuI_Start.Name = "toolStripMenuI_Start";
            this.toolStripMenuI_Start.Size = new System.Drawing.Size(44, 21);
            this.toolStripMenuI_Start.Text = "开始";
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem.Text = "加载文件";
            this.ToolStripMenuItem.Click += new System.EventHandler(this.button_LoadS19_Click);
            // 
            // toolStripMenuItem_Flash
            // 
            this.toolStripMenuItem_Flash.Name = "toolStripMenuItem_Flash";
            this.toolStripMenuItem_Flash.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_Flash.Text = "开始烧写";
            this.toolStripMenuItem_Flash.Click += new System.EventHandler(this.button_Flash_Click);
            // 
            // ToolStripMenuItem_Version
            // 
            this.ToolStripMenuItem_Version.Name = "ToolStripMenuItem_Version";
            this.ToolStripMenuItem_Version.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_Version.Text = "读取版本号";
            this.ToolStripMenuItem_Version.Click += new System.EventHandler(this.textBox_Version_Click);
            // 
            // toolStripMenuItem_Reset
            // 
            this.toolStripMenuItem_Reset.Name = "toolStripMenuItem_Reset";
            this.toolStripMenuItem_Reset.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_Reset.Text = "连接重置";
            this.toolStripMenuItem_Reset.Click += new System.EventHandler(this.toolStripMenuItem_Reset_Click);
            // 
            // toolStripMenuItem_FileReset
            // 
            this.toolStripMenuItem_FileReset.Name = "toolStripMenuItem_FileReset";
            this.toolStripMenuItem_FileReset.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_FileReset.Text = "文件重置";
            // 
            // toolStripMenuItem_About
            // 
            this.toolStripMenuItem_About.Name = "toolStripMenuItem_About";
            this.toolStripMenuItem_About.Size = new System.Drawing.Size(44, 21);
            this.toolStripMenuItem_About.Text = "关于";
            this.toolStripMenuItem_About.Click += new System.EventHandler(this.toolStripMenuItem_About_Click);
            // 
            // FormMain
            // 
            this.AcceptButton = this.button_Flash;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(666, 444);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip_Main;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "USBCAN2-FLASH";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBox_File.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip_Main.ResumeLayout(false);
            this.menuStrip_Main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_LoadS19;
        private System.Windows.Forms.Button button_Flash;
        private System.Windows.Forms.ComboBox comboBox_Config;
        private System.Windows.Forms.OpenFileDialog openS19Dialog;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBox_File;
        private System.Windows.Forms.Label label_Version;
        private System.Windows.Forms.TextBox textBox_Version;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuI_Start;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Flash;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Version;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_CAN;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Error;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Flash;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar_Flash;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Reset;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_FileReset;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_About;
        private System.Windows.Forms.CheckBox checkBox_Log;
        internal System.Windows.Forms.ListBox FileBox;
    }
}

