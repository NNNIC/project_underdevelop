using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class PointUtil
{
    public static void CreateHVLine(Point src, Point dst, ref List<Point> list)
    {
        list.Clear();

        if (src.X == dst.X || src.Y == dst.Y)
        {
            list.Add(src);
            list.Add(dst);
            return;
        }

        var mid_x = (int)((float)(src.X + dst.X) * 0.5f);
        var p1 = new Point( mid_x, src.Y );
        var p2 = new Point( mid_x, dst.Y );

        list.Add(src);
        list.Add(p1);
        list.Add(p2);
        list.Add(dst);

        return;
    }
}
