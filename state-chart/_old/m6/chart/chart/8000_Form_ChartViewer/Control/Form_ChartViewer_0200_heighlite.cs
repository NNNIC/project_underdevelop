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
#if obs
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
#else
        HeighlightFlowStateControl m_heighlightControl;

        void Heighlight_init()
        {
            m_heighlightControl = new HeighlightFlowStateControl();
            m_heighlightControl.Init();
        }
        void Heighlight_update()
        {
            m_heighlightControl.Update();
        }

#endif
    }
}