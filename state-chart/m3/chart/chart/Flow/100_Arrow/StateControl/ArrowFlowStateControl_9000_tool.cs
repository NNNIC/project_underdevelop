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
            if (LineUtil.IsHit(a,b,node.bounding))
            {
                return true;
            }

            if (LineUtil.IsOverlapped(a, b, node.arrow_next, true))
            {
                return true;
            }

            if (node.arrow_branch_list != null)
            {
                for (var i = 0; i < node.arrow_branch_list.Length; i++)
                {
                    if (LineUtil.IsOverlapped(a, b, node.arrow_branch_list[i], true))
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }
}