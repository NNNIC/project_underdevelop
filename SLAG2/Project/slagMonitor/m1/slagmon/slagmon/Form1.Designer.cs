namespace slagmon
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
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.components = new System.ComponentModel.Container();
            this.textBox1_log = new System.Windows.Forms.TextBox();
            this.textBox2_src = new System.Windows.Forms.TextBox();
            this.textBox3_input = new System.Windows.Forms.TextBox();
            this.label1_log = new System.Windows.Forms.Label();
            this.label2_source = new System.Windows.Forms.Label();
            this.label3_input = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1clear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1_log
            // 
            this.textBox1_log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1_log.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1_log.Location = new System.Drawing.Point(2, 24);
            this.textBox1_log.Multiline = true;
            this.textBox1_log.Name = "textBox1_log";
            this.textBox1_log.ReadOnly = true;
            this.textBox1_log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1_log.Size = new System.Drawing.Size(398, 328);
            this.textBox1_log.TabIndex = 0;
            this.textBox1_log.WordWrap = false;
            // 
            // textBox2_src
            // 
            this.textBox2_src.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2_src.Location = new System.Drawing.Point(406, 24);
            this.textBox2_src.Multiline = true;
            this.textBox2_src.Name = "textBox2_src";
            this.textBox2_src.ReadOnly = true;
            this.textBox2_src.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2_src.Size = new System.Drawing.Size(379, 328);
            this.textBox2_src.TabIndex = 1;
            this.textBox2_src.WordWrap = false;
            // 
            // textBox3_input
            // 
            this.textBox3_input.AllowDrop = true;
            this.textBox3_input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3_input.Location = new System.Drawing.Point(2, 370);
            this.textBox3_input.Multiline = true;
            this.textBox3_input.Name = "textBox3_input";
            this.textBox3_input.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox3_input.Size = new System.Drawing.Size(783, 96);
            this.textBox3_input.TabIndex = 2;
            this.textBox3_input.WordWrap = false;
            this.textBox3_input.TextChanged += new System.EventHandler(this.textBox3_input_TextChanged);
            this.textBox3_input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox3_input_KeyDown);
            this.textBox3_input.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox3_input_KeyPress);
            // 
            // label1_log
            // 
            this.label1_log.AutoSize = true;
            this.label1_log.Location = new System.Drawing.Point(0, 9);
            this.label1_log.Name = "label1_log";
            this.label1_log.Size = new System.Drawing.Size(20, 12);
            this.label1_log.TabIndex = 3;
            this.label1_log.Text = "log";
            // 
            // label2_source
            // 
            this.label2_source.AutoSize = true;
            this.label2_source.Location = new System.Drawing.Point(404, 9);
            this.label2_source.Name = "label2_source";
            this.label2_source.Size = new System.Drawing.Size(39, 12);
            this.label2_source.TabIndex = 4;
            this.label2_source.Text = "source";
            // 
            // label3_input
            // 
            this.label3_input.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3_input.AutoSize = true;
            this.label3_input.Location = new System.Drawing.Point(0, 355);
            this.label3_input.Name = "label3_input";
            this.label3_input.Size = new System.Drawing.Size(30, 12);
            this.label3_input.TabIndex = 5;
            this.label3_input.Text = "input";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1clear
            // 
            this.button1clear.Location = new System.Drawing.Point(26, 6);
            this.button1clear.Name = "button1clear";
            this.button1clear.Size = new System.Drawing.Size(50, 18);
            this.button1clear.TabIndex = 6;
            this.button1clear.Text = "clear";
            this.button1clear.UseVisualStyleBackColor = true;
            this.button1clear.Click += new System.EventHandler(this.button1clear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 469);
            this.Controls.Add(this.button1clear);
            this.Controls.Add(this.label3_input);
            this.Controls.Add(this.label2_source);
            this.Controls.Add(this.label1_log);
            this.Controls.Add(this.textBox3_input);
            this.Controls.Add(this.textBox2_src);
            this.Controls.Add(this.textBox1_log);
            this.Name = "Form1";
            this.Text = "slag monitor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1_log;
        private System.Windows.Forms.TextBox textBox2_src;
        private System.Windows.Forms.TextBox textBox3_input;
        private System.Windows.Forms.Label label1_log;
        private System.Windows.Forms.Label label2_source;
        private System.Windows.Forms.Label label3_input;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1clear;
    }
}

