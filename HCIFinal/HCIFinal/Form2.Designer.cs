namespace HCIFinal
{
    partial class Form2
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ExitButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PinButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.folderName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ExitButton.FlatAppearance.BorderSize = 0;
            this.ExitButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ExitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Location = new System.Drawing.Point(406, 11);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(18, 18);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "\r\n";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.PinButton);
            this.panel1.Controls.Add(this.AddButton);
            this.panel1.Controls.Add(this.folderName);
            this.panel1.Controls.Add(this.ExitButton);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 40);
            this.panel1.TabIndex = 1;
            // 
            // PinButton
            // 
            this.PinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PinButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PinButton.FlatAppearance.BorderSize = 0;
            this.PinButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.PinButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.PinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PinButton.Location = new System.Drawing.Point(380, 12);
            this.PinButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PinButton.Name = "PinButton";
            this.PinButton.Size = new System.Drawing.Size(16, 16);
            this.PinButton.TabIndex = 4;
            this.PinButton.Text = "\r\n";
            this.PinButton.UseVisualStyleBackColor = true;
            this.PinButton.Click += new System.EventHandler(this.PinButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddButton.FlatAppearance.BorderSize = 0;
            this.AddButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.AddButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddButton.Location = new System.Drawing.Point(37, 13);
            this.AddButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(15, 15);
            this.AddButton.TabIndex = 3;
            this.AddButton.Text = "\r\n";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // folderName
            // 
            this.folderName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.folderName.AutoSize = true;
            this.folderName.Font = new System.Drawing.Font("等线", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.folderName.Location = new System.Drawing.Point(187, 13);
            this.folderName.Name = "folderName";
            this.folderName.Size = new System.Drawing.Size(45, 19);
            this.folderName.TabIndex = 2;
            this.folderName.Text = "主页";
            this.folderName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Location = new System.Drawing.Point(0, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(433, 483);
            this.panel2.TabIndex = 2;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel2_Paint);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(433, 529);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("等线", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label folderName;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button PinButton;
        private System.Windows.Forms.Panel panel2;
    }
}

