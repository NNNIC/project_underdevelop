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

public partial class HeighlightFlowStateControl  {
   
    void setcur_new()
    {
        m_cur    = m_target;
        m_target = null;
    }

    void setcur_none()
    {
        m_cur    = null;
        m_target = null;
    }
}
