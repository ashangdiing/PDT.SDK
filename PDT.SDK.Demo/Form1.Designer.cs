namespace PDT.SDK.Demo
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.buttonCall = new System.Windows.Forms.Button();
            this.textBoxSMS = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(536, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonQuery
            // 
            this.buttonQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuery.Location = new System.Drawing.Point(536, 41);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(75, 23);
            this.buttonQuery.TabIndex = 1;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(424, 409);
            this.textBox1.TabIndex = 2;
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNumber.Location = new System.Drawing.Point(450, 70);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(161, 21);
            this.textBoxNumber.TabIndex = 3;
            this.textBoxNumber.TextChanged += new System.EventHandler(this.textBoxNumber_TextChanged);
            // 
            // buttonCall
            // 
            this.buttonCall.Location = new System.Drawing.Point(536, 98);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(75, 23);
            this.buttonCall.TabIndex = 4;
            this.buttonCall.Text = "呼叫";
            this.buttonCall.UseVisualStyleBackColor = true;
            this.buttonCall.Click += new System.EventHandler(this.buttonCall_Click);
            // 
            // textBoxSMS
            // 
            this.textBoxSMS.Location = new System.Drawing.Point(450, 137);
            this.textBoxSMS.Name = "textBoxSMS";
            this.textBoxSMS.Size = new System.Drawing.Size(161, 21);
            this.textBoxSMS.TabIndex = 5;
            this.textBoxSMS.Text = "这是一条短信！";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(536, 164);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "发短信";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 433);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxSMS);
            this.Controls.Add(this.buttonCall);
            this.Controls.Add(this.textBoxNumber);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "PDT.SDK模拟客户端";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.Button buttonCall;
        private System.Windows.Forms.TextBox textBoxSMS;
        private System.Windows.Forms.Button button2;
    }
}

