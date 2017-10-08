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

    public void CreateArrowLine()
    {
        var statelist = get_all_states();
        for(var i = 0; i<statelist.Count; i++)
        {
            var st  = m_stateData[i];
            st.m_ArrowLine_toNext = null;
            st.m_ArrowLine_branches = null;

            if (st.m_layout!=null)
            {
                //Nextへ
                if (st.m_dist_nextstate!=null && st.m_dist_nextstate.m_layout!=null)
                {
                    var start = st.m_layout.offset_point_out;
                    var goal =  st.m_dist_nextstate.m_layout.offset_point_in;

                    st.m_ArrowLine_toNext = ArrowFlowUtil.Create(st,st.m_dist_nextstate,null,start,goal);
                }
                //branchesへ
                if (st.m_dist_branches!=null)
                {
                    st.m_ArrowLine_branches = new List<Point>[st.NumBranches];

                    for(var j = 0; j<st.m_dist_branches.Length; j++)
                    {
                        if (st.m_dist_branches[j].m_layout!=null)
                        {
                            var start = st.m_layout.offset_point_out_branches(j);
                            var goal  = st.m_dist_branches[j].m_layout.offset_point_in;

                            st.m_ArrowLine_branches[j] = ArrowFlowUtil.Create(st,st.m_dist_branches[j],j,start,goal);
                        }
                    }
                }
            }          
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

        var point = Point.Truncate( POINT_START);

        foreach(var st in m_stateData)
        {
            if (st.m_layout==null) continue;

            st.m_layout.offset = point;

            DrawStateBox.DrawLayout(m_g,st.m_layout);

            point=PointUtil.Add_X(point,st.m_layout.Frame.Width + (int)LEN_BETWEEN_STATES);
        }
    }
}
