using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using Node = ChartManager.Node;


public partial class ArrowFlowStateControl : StateControlBase
{
    #region アクセス用
    List<Node> m_nodelist { get { return chart.Form1.V.m_chartman.m_nodeList; } }
    int        ARROW_GAP  { get { return ChartManager.ARROW_GAP;              } }
    int        ARROW_DIF  { get { return ChartManager.ARROW_DIF;              } } 
    #endregion

    List<Point> m_pointList = new List<Point>();
    
    Point       m_src;
    Point       m_dst;
    
    Point       m_buf_src; //緩衝付き位置
    Point       m_buf_dst; //

    bool        m_up_or_down;
   
    Point       m_cur;
    Point       m_cur_dst { get { return new Point(m_buf_dst.X, m_cur.Y );   } }

    static int  m_callcout = 0;

    public void Init(
        Point src,     Point dst,
        Point buf_src, Point buf_dst,
        bool up_or_down
    )
    {
        m_src = src;
        m_dst = dst;
        m_buf_src = buf_src;
        m_buf_dst = buf_dst;
        m_up_or_down = up_or_down;

        sc_start();
    }

    public void Calc()
    {
        SetNextState(S_START);
        GoNextState();

        for(var loop = 0; loop < 1000; loop ++)
        {
            if (m_sm.CheckState(S_END))
            {
                break;
            }
            sc_update();
        }
        if (!m_sm.CheckState(S_END))
        {
            MessageBox.Show("unexpected");
            m_pointList = new List<Point>();
            m_pointList.Add(m_src);
            m_pointList.Add(m_dst);
        }

        m_callcout++;
    }
    public List<Point> GetResult()
    {
        return m_pointList;
    }

}