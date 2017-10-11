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
        MoveFlowStateControl m_moveFlowStateControl;

        void Move_init()
        {
            m_moveFlowStateControl = new MoveFlowStateControl();
            m_moveFlowStateControl.Init();
        }

        void Move_update()
        {
            m_moveFlowStateControl.Update();
        }
    }
}