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
    Point m_posP;
    Point m_posQ;
    Point m_posR;
    Point m_posT;

    bool  m_bUD_PQ; //P->Q の方向 上か下か
    bool  m_bLR_QR; //Q->R の方向 右から左か

    void outline_create() {
        m_bUD_PQ = (m_posS.Y <= m_posG.Y );
        m_bLR_QR = (m_posS.X <= m_posG.Y );

        m_posP = m_posS;
        m_posQ = m_posP;

        m_posT = m_posG;
        m_posR = m_posT;
    }
}