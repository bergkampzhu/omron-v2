namespace omron
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txbRatio = new System.Windows.Forms.TextBox();
            this.txbLoad = new System.Windows.Forms.TextBox();
            this.txbSave = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(67, 84);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(76, 28);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "读取";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "利用率：";
            // 
            // txbRatio
            // 
            this.txbRatio.Location = new System.Drawing.Point(265, 23);
            this.txbRatio.Name = "txbRatio";
            this.txbRatio.ReadOnly = true;
            this.txbRatio.Size = new System.Drawing.Size(100, 21);
            this.txbRatio.TabIndex = 2;
            // 
            // txbLoad
            // 
            this.txbLoad.Location = new System.Drawing.Point(12, 57);
            this.txbLoad.Name = "txbLoad";
            this.txbLoad.ReadOnly = true;
            this.txbLoad.Size = new System.Drawing.Size(131, 21);
            this.txbLoad.TabIndex = 3;
            // 
            // txbSave
            // 
            this.txbSave.Location = new System.Drawing.Point(12, 118);
            this.txbSave.Name = "txbSave";
            this.txbSave.ReadOnly = true;
            this.txbSave.Size = new System.Drawing.Size(131, 21);
            this.txbSave.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(67, 145);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 28);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 266);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(179, 123);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(396, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(179, 123);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 610);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txbSave);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txbLoad);
            this.Controls.Add(this.txbRatio);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoad);
            this.Name = "Form1";
            this.Text = "欧姆龙杯";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbRatio;
        private System.Windows.Forms.TextBox txbLoad;
        private System.Windows.Forms.TextBox txbSave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

