using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class LineUtil
{

    #region //    https://qiita.com/ykob/items/ab7f30c43a0ed52d16f2 

    private static bool IsOnLine(float ax, float ay, float bx, float by, float cx, float cy)
    {
        var l1 = (bx - ax) * (bx - ax) + (by - ay) * (by - ay);
        var l2 = (cx - ax) * (cx - ax) + (cy - ay) * (cy - ay);
        var cd = (bx - ax) * (cx - ax) + (by - ay) * (cy - ay);
        return cd >= 0 && cd * cd == l1 * l2 && l1 >= l2;

    } 
    // a-b, c-d
    public static bool IsHit(Point a, Point b, Point c, Point d)
    {
        var ax = a.X;       var ay = a.Y;
        var bx = b.X;       var by = b.Y;
        var cx = c.X;       var cy = c.Y;
        var dx = d.X;       var dy = d.Y;
        
        var ta = (cx - dx) * (ay - cy) + (cy - dy) * (cx - ax);
        var tb = (cx - dx) * (by - cy) + (cy - dy) * (cx - bx);
        var tc = (ax - bx) * (cy - ay) + (ay - by) * (ax - cx);
        var td = (ax - bx) * (dy - ay) + (ay - by) * (ax - dx);

        // 端点を含まないで交差するか または 一方の線分上にもう一方の線分の端点がのっているか
        return tc * td < 0 && ta * tb < 0
                || IsOnLine(ax, ay, bx, by, cx, cy)
                || IsOnLine(ax, ay, bx, by, dx, dy)
                || IsOnLine(cx, cy, dx, dy, ax, ay)
                || IsOnLine(cx, cy, dx, dy, bx, by);       
    }


    public static bool IsHit(Point a, Point b, Rectangle rect)
    {
        /*
           o-p
           | |
           q-r
        */

        Point o = new Point(rect.X             , rect.Y              );
        Point p = new Point(rect.X + rect.Width, rect.Y              );
        Point q = new Point(rect.X             , rect.Y + rect.Height);
        Point r = new Point(rect.X + rect.Width, rect.Y + rect.Height);

        var isHit_top   = IsHit(a,b,o,p);
        var isHit_right = IsHit(a,b,p,r);
        var isHit_bot   = IsHit(a,b,r,q);
        var isHit_left  = IsHit(a,b,q,o);

        return isHit_top || isHit_right || isHit_bot || isHit_left;
    }
    #endregion

    public static bool IsOverlapped(Point a, Point b, Point c, Point d, bool? bHorizontal_or_Vertical=null)
    {
        var diff_ab = DrawUtil.Sub_Point(a,b);
        var diff_cd = DrawUtil.Sub_Point(c,d);

        var len_ab  = DrawUtil.Len_Point(a,b);
        var len_cd  = DrawUtil.Len_Point(c,d);

        if (bHorizontal_or_Vertical==null || (bool)bHorizontal_or_Vertical==true)
        {
            if (diff_ab.X == 0 && diff_cd.X == 0)
            {
                var min_y = MathUtil.Min(a.Y,b.Y,c.Y,d.Y);
                var max_y = MathUtil.Max(a.Y,b.Y,c.Y,d.Y);
            
                var len = max_y - min_y;
                return (len < len_ab + len_cd);             
            }
        }
        if (bHorizontal_or_Vertical==null || (bool)bHorizontal_or_Vertical == false)
        {
            if (diff_ab.Y == 0 && diff_cd.Y == 0)
            {
                var min_x = MathUtil.Min(a.X,b.X,c.X,d.X);
                var max_x = MathUtil.Max(a.X,b.X,c.X,d.X);
            
                var len = max_x - min_x;
                return (len < len_ab + len_cd);             
            }
        }
        return false;
    }

    public static bool IsOverlapped(Point a, Point b, List<Point> list, bool? bHorizontal_or_vertical=null)
    {
        if (list == null || list.Count < 2) return false;
        for(var i = 0; i < list.Count-1; i++)
        {
            var c = list[i];
            var d = list[i+1];
            if (IsOverlapped(a,b,c,d, bHorizontal_or_vertical))
            {
                return true;
            }
        }
        return false;
    }
}
