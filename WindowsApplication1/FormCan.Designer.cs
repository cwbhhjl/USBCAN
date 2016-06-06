namespace USBCAN
{
    partial class FormCan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Flash = new System.Windows.Forms.Button();
            this.button_File = new System.Windows.Forms.Button();
            this.textBox_SoftwareVersion = new System.Windows.Forms.TextBox();
            this.comboBox_Config = new System.Windows.Forms.ComboBox();
            this.listBox_Info = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button_Flash
            // 
            this.button_Flash.Enabled = false;
            this.button_Flash.Location = new System.Drawing.Point(12, 83);
            this.button_Flash.Name = "button_Flash";
            this.button_Flash.Size = new System.Drawing.Size(173, 39);
            this.button_Flash.TabIndex = 6;
            this.button_Flash.Text = "程序烧写";
            this.button_Flash.UseVisualStyleBackColor = true;
            // 
            // button_File
            // 
            this.button_File.AutoSize = true;
            this.button_File.Location = new System.Drawing.Point(12, 38);
            this.button_File.Name = "button_File";
            this.button_File.Size = new System.Drawing.Size(173, 39);
            this.button_File.TabIndex = 7;
            this.button_File.Text = "文件选择";
            this.button_File.UseVisualStyleBackColor = true;
            // 
            // textBox_SoftwareVersion
            // 
            this.textBox_SoftwareVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_SoftwareVersion.Location = new System.Drawing.Point(12, 128);
            this.textBox_SoftwareVersion.Name = "textBox_SoftwareVersion";
            this.textBox_SoftwareVersion.Size = new System.Drawing.Size(173, 14);
            this.textBox_SoftwareVersion.TabIndex = 9;
            this.textBox_SoftwareVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // comboBox_Config
            // 
            this.comboBox_Config.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Config.FormattingEnabled = true;
            this.comboBox_Config.Location = new System.Drawing.Point(12, 12);
            this.comboBox_Config.Name = "comboBox_Config";
            this.comboBox_Config.Size = new System.Drawing.Size(57, 20);
            this.comboBox_Config.TabIndex = 10;
            // 
            // listBox_Info
            // 
            this.listBox_Info.BackColor = System.Drawing.SystemColors.Window;
            this.listBox_Info.FormattingEnabled = true;
            this.listBox_Info.ItemHeight = 12;
            this.listBox_Info.Location = new System.Drawing.Point(12, 161);
            this.listBox_Info.Name = "listBox_Info";
            this.listBox_Info.Size = new System.Drawing.Size(173, 244);
            this.listBox_Info.TabIndex = 11;
            // 
            // FormCan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 428);
            this.Controls.Add(this.listBox_Info);
            this.Controls.Add(this.comboBox_Config);
            this.Controls.Add(this.textBox_SoftwareVersion);
            this.Controls.Add(this.button_File);
            this.Controls.Add(this.button_Flash);
            this.Name = "FormCan";
            this.Text = "FormCan";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Flash;
        private System.Windows.Forms.Button button_File;
        private System.Windows.Forms.TextBox textBox_SoftwareVersion;
        private System.Windows.Forms.ComboBox comboBox_Config;
        private System.Windows.Forms.ListBox listBox_Info;
    }
}