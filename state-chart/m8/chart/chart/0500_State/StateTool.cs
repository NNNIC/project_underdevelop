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

public class StateTool
{
    #region アクセス
    static ChartViewer m_cv            { get { return ChartViewer.V;            } }
    static PictureBox  m_pbmain        { get { return m_cv.pictureBox_main;     } }
    static PictureBox  m_pbhl          { get { return m_cv.pictureBox_highlite; } }
    static List<StateData> m_stateData { get { return StateInfo.m_stateData;    } }
    #endregion

    public static StateData GetStateAtCursor()
    {
        if (m_stateData==null) return null;

        var pos = m_pbmain.PointToClient(Cursor.Position);
        foreach(var st in m_stateData)
        {
            if (st==null || st.m_layout==null) continue;
            var rect = st.m_layout.offset_Frame;
            if (rect.IsEmpty) continue;

            if (rect.Contains(pos))
            {
                return st;
            }
        }
        return null;
    }  
}