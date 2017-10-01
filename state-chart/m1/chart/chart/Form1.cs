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
        MainFlowStateControl m_mfsc = new MainFlowStateControl();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ステート管理実行
            m_mfsc.Start();
            //
            pictureBox_output.Parent = panel1;
            pictureBox_temp.Parent = pictureBox_output;

            var canvas = new Bitmap(pictureBox_output.Width, pictureBox_output.Height);
            using (var g = Graphics.FromImage(canvas))
            {
                g.DrawRectangle(Pens.Black, 10, 20, 100, 80);
            }


            
            pictureBox_output.Image = canvas;
        }

        private void pictureBox_output_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Hello .NET Guide!", myFont, Brushes.Green, new Point(2, 2));
            }
            DrawUtil.Arrow(e.Graphics,new PointF(0,100),new PointF(100,100)  );
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_mfsc.Update();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_mfsc.Load(ofd.FileName);
            }
        }
    }
}
