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

public partial class ArrowFlowStateControl2 
{
    bool m_bStraight;
    void check_straight()
    {
        m_bStraight = false;

        if (IsHit_statebox(m_posS,m_posG))
        {
            m_bStraight = true;
        }
    }

    bool m_bCheckQR;
    void check_QR()
    {
        m_bCheckQR = false;

        if (IsHit_statebox(m_posQ,m_posR))
        {
            m_bCheckQR = true;
        }
        else if (IsHit_HorizontalLine(m_posQ,m_posR))
        {
            m_bCheckQR = true;
        }
    }

    bool m_bCheckSQ;
    void check_SQ()
    {
        m_bCheckSQ = false;

        if (IsHit_startLine(m_state_start,m_posS,m_posQ))
        {
            m_bCheckSQ = true;
        }
    }

    bool m_bCheckRG;
    void check_RG()
    {
        m_bCheckRG = false;

        if (IsHit_goalLine(m_state_goal,m_posR,m_posG))
        {
            m_bCheckRG = true;
        }
    }
}