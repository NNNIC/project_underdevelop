using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ArrowFlowStateControl
{
    void br_if_straight(Action<int,bool> state)
    {
        if (m_bStraight)
        {
            SetNextState(state);
        }
    }	
    void br_up_if_req(Action<int,bool> state)
    {
        if (m_up_or_down)
        {
            SetNextState(state);
        }
    }
    void br_down_if_req(Action<int,bool> state)
    {
        if (!m_up_or_down)
        {
            SetNextState(state);
        }
    }
    void br_up_if_reach_x(Action<int, bool> state)
    {
        if (m_bReachX)
        {
            SetNextState(state);
        }
    }
    void br_down_if_reach_x(Action<int, bool> state)
    {
        if (m_bReachX)
        {
            SetNextState(state);
        }
    }
}