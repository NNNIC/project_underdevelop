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
    public partial class Form_Debug:Form
    {
        public static Form_Debug V;

        #region アクセス
        ChartViewer FormMain   { get { return ChartViewer.V;              } }
        PictureBox  PicBoxMain { get { return FormMain.pictureBox_main;   } }
        #endregion

        public Form_Debug()
        {
            V = this;
            InitializeComponent();
        }

        private void timer1_Tick(object sender,EventArgs e)
        {
            textBox_mouse.Text = Cursor.Position.ToString();
            textBox_form.Text  = FormMain.PointToClient(Cursor.Position).ToString();
            textBox_main.Text  = PicBoxMain.PointToClient(Cursor.Position).ToString();
        }

        private void Form_Debug_Load(object sender,EventArgs e)
        {

        }
    }
}
