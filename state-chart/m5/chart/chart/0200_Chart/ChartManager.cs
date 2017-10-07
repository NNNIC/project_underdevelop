//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Detail=DrawStateBox.Detail;
//>>>

public partial class ChartManager
{
    const    float  LEN_BETWEEN_STATES = 60f;
    readonly PointF POINT_START        = new PointF(30,150);

    #region 内部アクセス
    PictureBox      m_mainPicture { get { return chart.Form1.V.pictureBox_main; } }
    List<StateData> m_stateData   { get { return StateInfo.m_stateData;         } }
    Bitmap          m_canvas      { get { return chart.Form1.V.m_canvas;        } }
    Graphics        m_g           { get { return chart.Form1.V.m_g;             } }
    #endregion

    public void Create()
    {
        var statelist = get_all_states();
        for(var i = 0; i<statelist.Count; i++)
        {
            var st  = m_stateData[i];
            st.m_layout = DrawStateBox.CreateLayout(m_g,st,Detail.Detailed);
        }
    }

    public void Draw()
    {
        if (m_stateData == null) return;

        m_g.Clear(Color.FromArgb(65,65,65));

        using (var pen = new Pen(Color.FromArgb(112, 112, 112), 1))
        {
            for (var y = 0; y < m_canvas.Height; y += 20)
            {
                m_g.DrawLine(pen, 0, y, m_canvas.Width, y);
            }
            for(var x=0; x<m_canvas.Width; x+= 20)
            {
                m_g.DrawLine(pen,x,0,x,m_canvas.Height);
            }
        }

        var point     = POINT_START;

        foreach(var st in m_stateData)
        {
            if (st.m_layout==null) continue;
            DrawStateBox.DrawLayout(m_g,point,st.m_layout);
            point=PointUtil.Add_X(point,st.m_layout.Frame.Width + LEN_BETWEEN_STATES);
        }
    }
}
