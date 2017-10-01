using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Node = ChartManager.Node;

public partial class ArrowFlowStateControl
{
    bool IsHit(Point a, Point b)
    {
        foreach(var node in m_nodelist)
        {
            if (LineUtil.IsHit(a,b,node.rect))
            {
                return true;
            }
        }        
        return false;
    }
}