//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChartViewer = chart.ChartViewer;
using Detail=DrawStateBox.Detail;
using LineType=DrawUtil.LineType;
using D=Define;
//>>>

namespace chart
{
    public partial class ChartViewer : Form
    {
        public static ChartViewer V;

        public MainFlowStateControl m_mfsc = new MainFlowStateControl();
        public ChartManager         m_chartman = new ChartManager();
        public Bitmap               m_canvas;
        public Graphics             m_g;

        public Form_Debug m_dbgForm;

        public ChartViewer()
        {
            V = this;
            InitializeComponent();
        }

        bool m_pictureBox_select_show;

        private void Form_ChartViewer_Load(object sender, EventArgs e)
        {
            //デバッグフォーム
            m_dbgForm = new Form_Debug();
            m_dbgForm.Show();

            //ステート管理実行
            m_mfsc.Start();
            //
            pictureBox_main.Parent = panel1;
            //ictureBox_temp.Parent = pictureBox_main;

            m_canvas = new Bitmap(pictureBox_main.Width, pictureBox_main.Height);
            pictureBox_main.Image = m_canvas;
            m_g = Graphics.FromImage(m_canvas);

            pictureBox_select.Parent = pictureBox_main;
            pictureBox_select.Hide();
            m_pictureBox_select_show = false;

            //using (var g = Graphics.FromImage(canvas))
            //{
            //    g.DrawRectangle(Pens.Black, 10, 20, 100, 80);
            //}
            
            //Control
            /*0200*/ Heighlight_init();

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
            Refresh();

            if (m_pictureBox_select_show)
            {
                var pos = pictureBox_main.PointToClient(Cursor.Position);
                pictureBox_select.Location = pos;
            }

            //Control
            /*0200*/ Heighlight_update();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Users\gea01\Documents\project_underdevelop\state-chart\m7\chart\chart\0100_Flow\010_Main\doc";
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_mfsc.Load(ofd.FileName);
            }
        }

        private void pictureBox_main_VisibleChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void pictureBox_main_Click(object sender,EventArgs e)
        {
            if (m_pictureBox_select_show)
            {
                m_pictureBox_select_show = false;
                pictureBox_select.Hide();
            }
            else
            {
                m_pictureBox_select_show = true;
                pictureBox_select.Show();
            }
        }
    }
}
