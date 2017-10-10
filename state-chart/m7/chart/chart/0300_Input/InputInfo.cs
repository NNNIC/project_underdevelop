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

public class InputInfo
{
    public static INPUTMOUSEEVANT m_inputMouseEvent
    {
        set {
            if (__inputMouseEvent != value)
            {
                __inputMouseEvent = value;
                DBG.LogEvent(value.ToString());
            }
        }
        get { 
            return __inputMouseEvent;
        }       
    }
    static INPUTMOUSEEVANT __inputMouseEvent = INPUTMOUSEEVANT.NONE;
}