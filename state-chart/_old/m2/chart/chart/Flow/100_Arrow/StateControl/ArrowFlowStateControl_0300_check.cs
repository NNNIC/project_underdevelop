using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ArrowFlowStateControl 
{
    bool m_bStraight;
    void check_if_straight() {
        m_bStraight = false;
        if (!IsHit(m_buf_src,m_buf_dst))
        {
            PointUtil.CreateHVLine(m_src,m_dst,ref m_pointList);
            m_bStraight = true;
        }
    }
    bool m_bReachX;
    void check_up_if_reach_x() {
        _check_updown_if_reach_x();
    }
    void check_down_if_reach_x() {
        _check_updown_if_reach_x();
    }
    void _check_updown_if_reach_x()
    {
        m_bReachX = false;
        if (!IsHit(m_cur,m_cur_dst))
        {
            m_pointList.Clear();
            m_pointList.Add(m_src);
            m_pointList.Add(m_buf_src);
            m_pointList.Add(m_cur);
            m_pointList.Add(m_cur_dst);
            m_pointList.Add(m_buf_dst);
            m_pointList.Add(m_dst);
            m_bReachX = true;
        }
    }
}