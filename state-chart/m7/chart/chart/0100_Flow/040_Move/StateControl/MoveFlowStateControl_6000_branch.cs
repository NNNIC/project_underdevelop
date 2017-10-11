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
    void br_on_state(Action<int,bool> st) {
        if (st!=null) SetNextState(st);
    }
    void br_cancel(Action<int,bool> st) {
        if (st==null) SetNextState(st);
    }
    void br_create_fail(Action<int,bool> st) {
        if (m_select_fail) SetNextState(st);
    }
    void br_mouseup(Action<int,bool> st) {
        if (m_mouse_isUp) SetNextState(st);
    }
    void br_mouseup_cancel(Action<int,bool> st) {
        if (m_mouse_isLeave) SetNextState(st);
    }
}

