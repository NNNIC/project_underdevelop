﻿//<<<include=using_text.txt
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
    StateData m_st;

    void check_on_state() {
        m_st = GetStateAtCursor();
    }
    //void check_mouseup() { }
    //void check_mousecancel() { }
    //void check_cancel() { }
}

