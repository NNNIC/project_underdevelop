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
        const int WIDTH = 4000;
        const int HEIGHT= 1000;

        public static ChartViewer V;

        public MainFlowStateControl m_mfsc = new MainFlowStateControl();
        public ChartManager         m_chartman = new ChartManager();
        public Bitmap               m_maincanvas;
        public Bitmap               m_bgcanvas;

        public Graphics             m_gMain;
        public Graphics             m_gBg;

        public Form_Debug m_dbgForm;

        public ChartViewer()
        {
            V = this;
            InitializeComponent();
        }

        //bool m_pictureBox_select_show;

        private void Form_ChartViewer_Load(object sender, EventArgs e)
        {
            //デバッグフォーム
            m_dbgForm = new Form_Debug();
            m_dbgForm.Show();

            //ステート管理実行
            m_mfsc.Start();

            //BG用
            m_bgcanvas                = new Bitmap(WIDTH, HEIGHT);
            pictureBox_BG.Parent      = panel1;
            pictureBox_BG.Image       = m_bgcanvas;
            pictureBox_BG.Size        = new Size(WIDTH,HEIGHT);
            pictureBox_BG.Location    = new Point(0,0);
            m_gBg                     = Graphics.FromImage(m_bgcanvas);

            //メイン
            m_maincanvas              = new Bitmap(pictureBox_main.Width, pictureBox_main.Height);
            pictureBox_main.Parent    = pictureBox_BG;
            pictureBox_main.Image     = m_maincanvas;
            pictureBox_main.BackColor = Color.Transparent;
            m_gMain                   = Graphics.FromImage(m_maincanvas);

            //セレクト
            pictureBox_select.Parent = pictureBox_main;
            pictureBox_select.Hide();

            //入力コールバック設定
            InputCallBacks.SetCallbacks();

            //Control
            /*0200*/ Heighlight_init();
            /*0300*/ Move_init();

        }

        private void pictureBox_output_Paint(object sender, PaintEventArgs e)
        {

            //m_chartman.Draw();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_mfsc.Update();
            Refresh();

            //Control
            /*0200*/ Heighlight_update();
            /*0300*/ Move_update();
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
            //if (m_pictureBox_select_show)
            //{
            //    m_pictureBox_select_show = false;
            //    pictureBox_select.Hide();
            //}
            //else
            //{
            //    m_pictureBox_select_show = true;
            //    pictureBox_select.Show();
            //}
        }

        private void ChartViewer_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite("cv Mouse Down\n");
        }

        private void ChartViewer_DragDrop(object sender,DragEventArgs e)
        {
            DBG.LogWrite("DragDrop\n");
        }

        private void ChartViewer_DragEnter(object sender,DragEventArgs e)
        {
            DBG.LogWrite("DragEnter\n");
        }

        private void ChartViewer_DragLeave(object sender,EventArgs e)
        {
            DBG.LogWrite("DragLeave\n");
        }

        private void ChartViewer_DragOver(object sender,DragEventArgs e)
        {
            DBG.LogWrite("DragOver\n");
        }

        private void pictureBox_main_DragDrop(object sender,DragEventArgs e)
        {

        }

        private void pictureBox_main_DragEnter(object sender,DragEventArgs e)
        {

        }

        private void pictureBox_main_DragLeave(object sender,EventArgs e)
        {

        }

        private void pictureBox_main_DragOver(object sender,DragEventArgs e)
        {

        }

        private void pictureBox_main_GiveFeedback(object sender,GiveFeedbackEventArgs e)
        {

        }

        private void pictureBox_main_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite("main Mouse Down\n");
        }

        private void pictureBox_select_Click(object sender,EventArgs e)
        {
            DBG.LogWrite("select Mouse Click\n");

        }

        private void pictureBox_select_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite("select Mouse Down\n");
        }

        private void pictureBox_highlite_Click(object sender,EventArgs e)
        {

        }

        private void pictureBox_highlite_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite("heighlight Mouse Down\n");
        }

        private void pictureBox_collider_MouseDown(object sender,MouseEventArgs e)
        {
            DBG.LogWrite(" colider Mouse Down\n");
        }
    }
}
