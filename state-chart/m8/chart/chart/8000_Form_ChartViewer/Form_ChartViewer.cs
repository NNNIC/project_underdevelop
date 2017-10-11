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

            //LoadSave.TRY_LoadData_byArgs();
        }

        private void pictureBox_output_Paint(object sender, PaintEventArgs e)
        {
            //Refresh();
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
            LoadSave.DO_LoadData();
        }

        private void pictureBox_main_VisibleChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void saveLayoutToolStripMenuItem_Click(object sender,EventArgs e)
        {
            LoadSave.DO_SaveLayout();
        }
    }
}
