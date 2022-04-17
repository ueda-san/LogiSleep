namespace LogiSleep
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxLED = new System.Windows.Forms.CheckBox();
            this.checkBoxScreen = new System.Windows.Forms.CheckBox();
            this.buttonTurnOff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxLED
            // 
            this.checkBoxLED.AutoSize = true;
            this.checkBoxLED.Checked = true;
            this.checkBoxLED.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLED.Location = new System.Drawing.Point(13, 13);
            this.checkBoxLED.Name = "checkBoxLED";
            this.checkBoxLED.Size = new System.Drawing.Size(351, 22);
            this.checkBoxLED.TabIndex = 0;
            this.checkBoxLED.Text = "Turns off keyboard LED when screen is off";
            this.checkBoxLED.UseVisualStyleBackColor = true;
            this.checkBoxLED.CheckedChanged += new System.EventHandler(this.checkBoxLED_CheckedChanged);
            // 
            // checkBoxScreen
            // 
            this.checkBoxScreen.AutoSize = true;
            this.checkBoxScreen.Location = new System.Drawing.Point(13, 57);
            this.checkBoxScreen.Name = "checkBoxScreen";
            this.checkBoxScreen.Size = new System.Drawing.Size(295, 22);
            this.checkBoxScreen.TabIndex = 1;
            this.checkBoxScreen.Text = "Prevent the screen from turning off";
            this.checkBoxScreen.UseVisualStyleBackColor = true;
            this.checkBoxScreen.CheckedChanged += new System.EventHandler(this.checkBoxScreen_CheckedChanged);
            // 
            // buttonTurnOff
            // 
            this.buttonTurnOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTurnOff.Location = new System.Drawing.Point(411, 97);
            this.buttonTurnOff.Name = "buttonTurnOff";
            this.buttonTurnOff.Size = new System.Drawing.Size(198, 34);
            this.buttonTurnOff.TabIndex = 2;
            this.buttonTurnOff.Text = "Turn off now";
            this.buttonTurnOff.UseVisualStyleBackColor = true;
            this.buttonTurnOff.Click += new System.EventHandler(this.buttonTurnOff_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 143);
            this.Controls.Add(this.buttonTurnOff);
            this.Controls.Add(this.checkBoxScreen);
            this.Controls.Add(this.checkBoxLED);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "LogiSleep";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxLED;
        private System.Windows.Forms.CheckBox checkBoxScreen;
        private System.Windows.Forms.Button buttonTurnOff;
    }
}

