using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public partial class ArrowFlowStateControl
{
	void temp_up()
    {
        m_cur = DrawUtil_obs.Add_Y(m_cur,-(ARROW_GAP + m_callcout * ARROW_DIF) );
    }
    void temp_down()
    {
        m_cur = DrawUtil_obs.Add_Y(m_cur, ARROW_GAP + m_callcout * ARROW_DIF);
    }
}