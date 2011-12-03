namespace MySynthe
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
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button_makewave = new System.Windows.Forms.Button();
            this.button_showwavedata = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "WAVファイルから読み込み";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(164, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "生成したWAVを保存して再生";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_makewave
            // 
            this.button_makewave.Location = new System.Drawing.Point(13, 43);
            this.button_makewave.Name = "button_makewave";
            this.button_makewave.Size = new System.Drawing.Size(163, 23);
            this.button_makewave.TabIndex = 2;
            this.button_makewave.Text = "音声波形データの作成";
            this.button_makewave.UseVisualStyleBackColor = true;
            this.button_makewave.Click += new System.EventHandler(this.button_makewave_Click);
            // 
            // button_showwavedata
            // 
            this.button_showwavedata.Location = new System.Drawing.Point(13, 102);
            this.button_showwavedata.Name = "button_showwavedata";
            this.button_showwavedata.Size = new System.Drawing.Size(163, 23);
            this.button_showwavedata.TabIndex = 3;
            this.button_showwavedata.Text = "波形の表示";
            this.button_showwavedata.UseVisualStyleBackColor = true;
            this.button_showwavedata.Click += new System.EventHandler(this.button_showwavedata_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(182, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(90, 19);
            this.textBox1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button_showwavedata);
            this.Controls.Add(this.button_makewave);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_makewave;
        private System.Windows.Forms.Button button_showwavedata;
        private System.Windows.Forms.TextBox textBox1;
    }
}

