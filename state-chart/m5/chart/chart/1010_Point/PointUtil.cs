//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
//>>>

public class PointUtil
{
    public static Point Add_X(Point a, int x)
    {
        return new Point(a.X + x, a.Y);
    }
    public static PointF Add_X(PointF a, float x)
    {
        return new PointF(a.X + x, a.Y);
    }

    public static Point Add_Y(Point a, int y)
    {
        return new Point(a.X , a.Y + y);
    }
    public static PointF Add_Y(PointF a, float y)
    {
        return new PointF(a.X , a.Y + y);
    }
    public static PointF Add_XY(PointF a, float x, float y)
    {
        return new PointF(a.X+x , a.Y + y);
    }


    public static Point Add_Point(Point a, Point b)
    {
        return new Point(a.X + b.X, a.Y + b.Y);
    }
    public static PointF Add_Point(PointF a, PointF b)
    {
        return new PointF(a.X + b.X, a.Y + b.Y);
    }

    public static Point Sub_Point(Point a, Point b)
    {
        return new Point(a.X - b.X, a.Y - b.Y);
    }
    public static PointF Sub_Point(PointF a, PointF b)
    {
        return new PointF(a.X - b.X, a.Y - b.Y);
    }

    public static float Len_Point(Point a, Point b)
    {
        var d = Sub_Point(a,b);
        return  (float)Math.Sqrt((double)(d.X * d.X + d.Y * d.Y));
    }
    public static float Len_Point(PointF a, PointF b)
    {
        var d = Sub_Point(a,b);
        return  (float)Math.Sqrt((double)(d.X * d.X + d.Y * d.Y));
    }
    public static Point Abs(Point a)
    {
        return new Point(Math.Abs(a.X), Math.Abs(a.Y));
    }
    public static PointF Abs(PointF a)
    {
        return new PointF(Math.Abs(a.X), Math.Abs(a.Y));
    }
    public static Point Center(Point a, Point b)
    {
        return new Point( (a.X + b.X) / 2, (a.Y + b.Y) /2 );
    }
    public static PointF Center(PointF a, PointF b)
    {
        return new PointF( (a.X + b.X) / 2, (a.Y + b.Y) /2 );
    }
}
