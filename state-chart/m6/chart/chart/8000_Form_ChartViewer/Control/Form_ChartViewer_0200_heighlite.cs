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
    public partial class ChartViewer 
    {
        HeighliteManager m_heighliteManager;

        void Heighlite_init()
        {
            m_heighliteManager = new HeighliteManager();
            m_heighliteManager.Init();
        }

        void Heighlite_update()
        {
            m_heighliteManager.Upadte();
        }
    }
}