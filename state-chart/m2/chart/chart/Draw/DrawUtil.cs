using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

public class DrawUtil
{
    const double ARROW_DEGREE = 45d;

    public static Color  m_pen_color = Color.Black;
    public static double m_pen_size  = 10;
    public static void   Pen_set(double size)  { m_pen_size =size; }
    public static void   Pen_set(Color col) { m_pen_color = col; }
    public static void   Pen_set(Color col, float size) { Pen_set(col); Pen_set(size);  }
    private static Pen   Pen_create()
    {
        return new Pen(m_pen_color,(int)m_pen_size);
    }
    private static Pen   Pen_create_arrow()
    {
        var pen = Pen_create();
        pen.EndCap = LineCap.ArrowAnchor;
        return pen;
    }

    private static Font __font;
    public static Font  m_font
    {
        get { if (__font == null) __font = new Font("メイリオ",14);　return __font; }
        set { if (__font!=null) __font.Dispose(); __font = value;                   }
    }

    public static void Rect(Graphics g, Rectangle rect)
    {
        using (var pen = Pen_create())
        {
            g.DrawRectangle(pen, rect);
        }
    }
    public static void Text(Graphics g, string text, Rectangle rect)
    {
        using (var pen = Pen_create())
        {
            g.DrawString(text, m_font, pen.Brush, rect);
        }
    }
    public static void Line(Graphics g, PointF a, PointF b)
    {
        using (var pen = Pen_create())
        {
            g.DrawLine(pen, a, b);
        }
    }
    public static void Arrow(Graphics g, PointF src, PointF dst)
    {
        using (var pen = Pen_create_arrow())
        {
            g.DrawLine(pen,src,dst);
        }
    }
    public static void Arrow(Graphics g, List<Point> list)
    {
        using (var pen_head = Pen_create_arrow())
        using (var pen = Pen_create())
        {
            for(var i = 0; i<list.Count; i++)
            {
                //if (i==0)
                //{
                //    g.DrawLine(pen_head, list[i], list[i+1]);
                //}
                if (i==list.Count-2)
                {
                    g.DrawLine(pen_head, list[i], list[i+1]);
                    break;
                }
                else
                {
                    g.DrawLine(pen_head, list[i], list[i+1]);
                }
            }
        }
    }
    public static Point Add_X(Point a, int x)
    {
        return new Point(a.X + x, a.Y);
    }
    public static Point Add_Y(Point a, int y)
    {
        return new Point(a.X , a.Y + y);
    }
    public static Point Add_Point(Point a, Point b)
    {
        return new Point(a.X + b.X, a.Y + b.Y);
    }
}

