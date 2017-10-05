using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace chart
{
    public partial class Form1 : Form
    {
        public static Form1 V;

        public MainFlowStateControl m_mfsc = new MainFlowStateControl();
        public ChartManager         m_chartman = new ChartManager();

        public Form1()
        {
            V = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ステート管理実行
            m_mfsc.Start();
            //
            pictureBox_main.Parent = panel1;
            //ictureBox_temp.Parent = pictureBox_main;

            //var canvas = new Bitmap(pictureBox_main.Width, pictureBox_main.Height);
            //using (var g = Graphics.FromImage(canvas))
            //{
            //    g.DrawRectangle(Pens.Black, 10, 20, 100, 80);
            //}

            //pictureBox_main.Image = canvas;
        }

        private void pictureBox_output_Paint(object sender, PaintEventArgs e)
        {
            //using (Font myFont = new Font("Arial", 14))
            //{
            //    e.Graphics.DrawString("Hello .NET Guide!", myFont, Brushes.Green, new Point(2, 2));
            //}
            //DrawUtil.Arrow(e.Graphics,new PointF(0,100),new PointF(100,100)  );

            m_chartman.Draw();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_mfsc.Update();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Users\gea01\Documents\project_underdevelop\state-chart\m2\chart\chart\Flow\010_Main\doc";
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_mfsc.Load(ofd.FileName);
            }
        }
    }
}
