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

public class InputCallBacks
{
    private static Control m_panel   { get { return chart.ChartViewer.V.panel1;              } }
    private static Control m_pbMain  { get { return chart.ChartViewer.V.pictureBox_main;     } }
    private static Control m_pbBg    { get { return chart.ChartViewer.V.pictureBox_highlite; } }
    private static Control m_pbHl    { get { return chart.ChartViewer.V.pictureBox_highlite; } }
    private static Control m_pbSl    { get { return chart.ChartViewer.V.pictureBox_select;   } }

    public static void SetCallbacks()
    {
        SetCallbacks_sub(m_panel);
        SetCallbacks_sub(m_pbMain);
        SetCallbacks_sub(m_pbBg);
        SetCallbacks_sub(m_pbHl);
        SetCallbacks_sub(m_pbSl);
    }
    private static void SetCallbacks_sub(Control cnt)
    {
        cnt.MouseDown += MouseDown;
        cnt.MouseUp   += MouseUp;
        cnt.MouseLeave += MouseLevae;
    }


    public static void MouseDown (object sender, MouseEventArgs arg)  {  InputInfo.m_inputMouseEvent = INPUTMOUSEEVANT.DOWN;    }
    public static void MouseUp   (object sender, MouseEventArgs arg)  {  InputInfo.m_inputMouseEvent = INPUTMOUSEEVANT.UP;      }
    public static void MouseLevae(object sender, EventArgs arg)       {
        var pos = m_panel.PointToClient(Cursor.Position);
        var rect = new Rectangle(m_panel.Location, m_panel.Size);
        if (!rect.Contains(pos))
        {
            InputInfo.m_inputMouseEvent = INPUTMOUSEEVANT.LEAVE;
        }
    }

        
}
