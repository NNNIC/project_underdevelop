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
    int m_diff  = DUNIT;

    int m_diffSP = 0;
    int m_diffPQ = 0;
    int m_diffTG = 0;

    void setdiff_SP() {
        m_diffSP = m_diff;
    }
    void setdiff_PQ() {
        m_diffPQ = m_diff;
    }
    void setdiff_TG() {
        m_diffTG = m_diff;
    }

    void setdiff_allclear() {
        m_diffSP = 0;
        m_diffPQ = 0;
        m_diffTG = 0;
    }

    void setdiff_PQ_chkQR() {
        if (m_bCheckQR)
        {
            setdiff_PQ();
        }
    }
    void setdiff_SP_chkSQ() {
        if (m_bCheckSQ)
        {
            setdiff_SP();
        }
    }
    void setdiff_TG_chkRG() {
        if (m_bCheckRG)
        {
            setdiff_TG();
        }
    }
}
