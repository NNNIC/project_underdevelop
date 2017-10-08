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
    void br_straight(Action<int,bool> st)
    {
        if (m_bStraight)
        {
            SetNextState(st);
        }
    }
    void br_checkQR(Action<int,bool> st)
    {
        if (m_bCheckQR)
        {
            SetNextState(st);
        }
    }
    void br_checkSQ(Action<int,bool> st)
    {
        if (m_bCheckSQ)
        {
            SetNextState(st);
        }
    }
    void br_checkRG(Action<int,bool> st)
    {
        if (m_bCheckRG)
        {
            SetNextState(st);
        }
    }
}