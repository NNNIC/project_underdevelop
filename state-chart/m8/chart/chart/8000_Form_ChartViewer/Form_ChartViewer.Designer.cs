namespace chart
{
    partial class ChartViewer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox_select = new System.Windows.Forms.PictureBox();
            this.pictureBox_highlite = new System.Windows.Forms.PictureBox();
            this.pictureBox_main = new System.Windows.Forms.PictureBox();
            this.pictureBox_BG = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_highlite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_BG)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox_select);
            this.panel1.Controls.Add(this.pictureBox_highlite);
            this.panel1.Controls.Add(this.pictureBox_main);
            this.panel1.Controls.Add(this.pictureBox_BG);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 737);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox_select
            // 
            this.pictureBox_select.Location = new System.Drawing.Point(540, 295);
            this.pictureBox_select.Name = "pictureBox_select";
            this.pictureBox_select.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_select.TabIndex = 1;
            this.pictureBox_select.TabStop = false;
            this.pictureBox_select.Click += new System.EventHandler(this.pictureBox_select_Click);
            this.pictureBox_select.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_select_MouseDown);
            // 
            // pictureBox_highlite
            // 
            this.pictureBox_highlite.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox_highlite.Location = new System.Drawing.Point(540, 230);
            this.pictureBox_highlite.Name = "pictureBox_highlite";
            this.pictureBox_highlite.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_highlite.TabIndex = 2;
            this.pictureBox_highlite.TabStop = false;
            this.pictureBox_highlite.Click += new System.EventHandler(this.pictureBox_highlite_Click);
            this.pictureBox_highlite.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_highlite_MouseDown);
            // 
            // pictureBox_main
            // 
            this.pictureBox_main.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_main.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_main.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox_main.Name = "pictureBox_main";
            this.pictureBox_main.Size = new System.Drawing.Size(4000, 800);
            this.pictureBox_main.TabIndex = 0;
            this.pictureBox_main.TabStop = false;
            this.pictureBox_main.VisibleChanged += new System.EventHandler(this.pictureBox_main_VisibleChanged);
            this.pictureBox_main.Click += new System.EventHandler(this.pictureBox_main_Click);
            this.pictureBox_main.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox_main_DragDrop);
            this.pictureBox_main.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox_main_DragEnter);
            this.pictureBox_main.DragOver += new System.Windows.Forms.DragEventHandler(this.pictureBox_main_DragOver);
            this.pictureBox_main.DragLeave += new System.EventHandler(this.pictureBox_main_DragLeave);
            this.pictureBox_main.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.pictureBox_main_GiveFeedback);
            this.pictureBox_main.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_output_Paint);
            this.pictureBox_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_main_MouseDown);
            // 
            // pictureBox_BG
            // 
            this.pictureBox_BG.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox_BG.Location = new System.Drawing.Point(540, 174);
            this.pictureBox_BG.Name = "pictureBox_BG";
            this.pictureBox_BG.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_BG.TabIndex = 4;
            this.pictureBox_BG.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 33;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip2.TabIndex = 3;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveLayoutToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // ChartViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip2);
            this.Name = "ChartViewer";
            this.Text = "Chart Viewer";
            this.Load += new System.EventHandler(this.Form_ChartViewer_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ChartViewer_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ChartViewer_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.ChartViewer_DragOver);
            this.DragLeave += new System.EventHandler(this.ChartViewer_DragLeave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChartViewer_MouseDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_highlite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_BG)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        public System.Windows.Forms.PictureBox pictureBox_main;
        public System.Windows.Forms.PictureBox pictureBox_highlite;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.PictureBox pictureBox_select;
        public System.Windows.Forms.PictureBox pictureBox_BG;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
    }
}

