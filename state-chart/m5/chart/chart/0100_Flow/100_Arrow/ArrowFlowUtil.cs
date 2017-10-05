using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class ArrowFlowUtil
{
    public static List<Point> Create(
        Point src,    Point dst,
        Point buf_src,Point buf_dst,
        bool up_or_down)
    {
        var ctr = new ArrowFlowStateControl();
        ctr.Init(src,dst,buf_src,buf_dst,up_or_down);
        ctr.Calc();
        return ctr.GetResult();
    } 
}
