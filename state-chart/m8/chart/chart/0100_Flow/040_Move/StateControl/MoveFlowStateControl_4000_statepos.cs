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

public partial class MoveFlowStateControl  {
    void statepos_update() {
        if (m_st!=null)
        {
            m_st.m_layout.offset = m_pbsl.Location;
            m_chartman.Update();
        }

    }
}

