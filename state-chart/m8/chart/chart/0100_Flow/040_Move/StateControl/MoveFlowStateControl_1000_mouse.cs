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

    bool m_mouse_isUp;
    bool m_mouse_isLeave;

    bool Mouse_isDown() {
        if (InputInfo.m_inputMouseEvent == INPUTMOUSEEVANT.DOWN)
        {
            return true;
        }
        return false;
    }
    bool Mouse_isAny() {
        m_mouse_isUp  = false;
        m_mouse_isLeave = false;

        var mouseevent = InputInfo.m_inputMouseEvent;
        if (mouseevent == INPUTMOUSEEVANT.UP)
        {
            m_mouse_isUp = true;
            return true;
        }
        if (mouseevent == INPUTMOUSEEVANT.LEAVE)
        {
            m_mouse_isLeave = true;
            return true;
        }
        return false;
    }
}

