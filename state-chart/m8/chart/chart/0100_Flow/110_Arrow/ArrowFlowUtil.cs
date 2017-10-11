//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChartViewer = chart.ChartViewer;
using Detail=DrawStateBox.Detail;
using LineType=DrawUtil.LineType;
using D=Define;
//>>>

public class ArrowFlowUtil
{
    public static List<Point> Create(StateData start_st, StateData goal_st, int? branch_index, Point start, Point goal)
    {
        var ctr = new ArrowFlowStateControl2();
        ctr.Begin(start_st,goal_st, branch_index, start, goal);
        ctr.Calc();
        return ctr.GetResult();
    }
}
