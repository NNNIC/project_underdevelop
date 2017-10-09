namespace chart
{
    partial class Form_Debug
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
            if(disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            this.textBox_mouse = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_main = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_form = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_mouse
            // 
            this.textBox_mouse.Location = new System.Drawing.Point(53, 13);
            this.textBox_mouse.Name = "textBox_mouse";
            this.textBox_mouse.Size = new System.Drawing.Size(139, 19);
            this.textBox_mouse.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "mouse";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "main";
            // 
            // textBox_main
            // 
            this.textBox_main.Location = new System.Drawing.Point(53, 63);
            this.textBox_main.Name = "textBox_main";
            this.textBox_main.Size = new System.Drawing.Size(139, 19);
            this.textBox_main.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "form";
            // 
            // textBox_form
            // 
            this.textBox_form.Location = new System.Drawing.Point(53, 38);
            this.textBox_form.Name = "textBox_form";
            this.textBox_form.Size = new System.Drawing.Size(139, 19);
            this.textBox_form.TabIndex = 4;
            // 
            // Form_Debug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_form);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_main);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_mouse);
            this.Name = "Form_Debug";
            this.Text = "Form_Debug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_mouse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_main;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_form;
    }
}