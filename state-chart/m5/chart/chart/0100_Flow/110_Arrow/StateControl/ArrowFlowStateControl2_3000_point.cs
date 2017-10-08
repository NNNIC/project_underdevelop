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
using LineType=DrawUtil.LineType;
//>>>

public partial class ArrowFlowStateControl2
{
    void point_straight()
    {
        m_result = new List<Point>();
        m_result.Add(m_posS);
        m_result.Add(m_posG);
    }
    void point_PQRT()
    {
        var diff_sp = m_posP.X - m_posS.X;
        var diff_pq = m_posQ.Y - m_posP.Y;
        var diff_tg = m_posT.X - m_posG.X;
        var diff_rt = m_posR.Y - m_posT.Y;

        m_posP = PointUtil.Add_X(m_posS,  Math.Abs(diff_sp) + m_diffSP );
        m_posT = PointUtil.Add_X(m_posG,-(Math.Abs(diff_tg) + m_diffTG));

        m_posQ = PointUtil.Add_Y(m_posP,  diff_pq + ((m_bUD_PQ ? -1 : 1) * m_diffPQ));
        m_posR = new Point(m_posT.X,m_posQ.Y);
    }
    void point_createroute()
    {
        m_result = new List<Point>();
        m_result.Add(m_posS);
        m_result.Add(m_posP);
        m_result.Add(m_posQ);
        m_result.Add(m_posR);
        m_result.Add(m_posT);
        m_result.Add(m_posG);
    }
}
