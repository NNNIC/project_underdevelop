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
   
    void heighlight_show() {
        m_bShow = false;
        if (m_cur==null)
        {
           return; 
        }
        
        if (m_cur.m_layout!=null)
        {
            var rect = m_cur.m_layout.offset_Frame;
            if (!rect.IsEmpty)
            {
                m_pbhl.Location = rect.Location;
                m_pbhl.Size     = rect.Size;
                m_bShow = true;
            }
        }
    }
    void heighlight_hide() {
        m_bShow = false;
    }
}
