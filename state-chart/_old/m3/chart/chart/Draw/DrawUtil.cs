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
    public static void ArrowCurve(Graphics g, List<Point> list)
    {
        using (var pen_head = Pen_create_arrow())
        {
            g.DrawCurve(pen_head,list.ToArray());
        }
    }
    public static void ArrowBezir_a(Graphics g, List<Point> list)
    {
        //ポイントリストをベジェ曲線用に分解する。
        //ベジェ曲線にするには、４点ずつの指定となる。
        //最初の４点は、開始点ー次点　次点ー後半分となる
        //その後は、前半点-次点　次点-後半点
        //最後の４点は、前半点-次点　次点ー最終点

        if (list.Count < 3) return; 

        var nlist= new List<Point>();
        nlist.Add(list[0]);

        for(var i = 0; ;i++)
        {
            Point b0,b1,b2,b3;
            if (i==0)
            {
                b0 = list[0];
                b1 = list[1];
                b2 = list[1];
                b3 = list.Count==3 ? list[2] : Center(list[1],list[2]);
                
                nlist.Add(b0);
                nlist.Add(b1);
                //nlist.Add(b2);
                nlist.Add(b3);

                if (list.Count == 3) break;
            }
            else if (i==list.Count-3) //最後
            {
                b0 = Center(list[i],list[i+1]);
                b1 = list[i+1];
                b2 = list[i+1];
                b3 = list[i+2];
                nlist.Add(b0);
                nlist.Add(b1);
                //nlist.Add(b2);
                nlist.Add(b3);
                break;
            }
            else
            {
                b0 = Center(list[i],list[i+1]);
                b1 = list[i+1];
                b2 = list[i+1];
                b3 = Center(list[i+1],list[i+2]);
                nlist.Add(b0);
                nlist.Add(b1);
                //nlist.Add(b2);
                nlist.Add(b3);
            }
        }

        using (var pen_head = Pen_create_arrow())
        {
            g.DrawBeziers(pen_head,nlist.ToArray());
        }

    }
    public static void ArrowBezir(Graphics g, List<Point> list)
    {
        //ポイントリストをベジェ曲線用に分解する。
        //ベジェ曲線にするには、４点ずつの指定となる。
        //最初の４点は、開始点ー次点　次点ー後半分となる
        //その後は、前半点-次点　次点-後半点
        //最後の４点は、前半点-次点　次点ー最終点

        if (list.Count < 3) return; 

        var nlist= new List<Point>();

        for(var i = 0; ;i++)
        {
            Point b0,b1,b2,b3;
            if (i==0)
            {
                b0 = list[0];
                b1 = list[1];
                b2 = list[1];
                b3 = list.Count==3 ? list[2] : Center(list[1],list[2]);
                
                nlist.Add(b0);
                nlist.Add(b1);
                nlist.Add(b2);
                nlist.Add(b3);

                if (list.Count == 3) break;
            }
            else if (i==list.Count-3) //最後
            {
                b0 = Center(list[i],list[i+1]);
                b1 = list[i+1];
                b2 = list[i+1];
                b3 = list[i+2];
                nlist.Add(b0);
                nlist.Add(b1);
                nlist.Add(b2);
                nlist.Add(b3);
                break;
            }
            else
            {
                b0 = Center(list[i],list[i+1]);
                b1 = list[i+1];
                b2 = list[i+1];
                b3 = Center(list[i+1],list[i+2]);
                nlist.Add(b0);
                nlist.Add(b1);
                nlist.Add(b2);
                nlist.Add(b3);
            }
        }

        using (var pen_head = Pen_create_arrow())
        using (var pen = Pen_create())
        {
            for(var i = 0; i<nlist.Count-4;i+=4)
            {
                g.DrawBezier(pen,nlist[i],nlist[i+1],nlist[i+2],nlist[i+3]);
            }
            var j = nlist.Count-4;
            g.DrawBezier(pen_head,nlist[j],nlist[j+1],nlist[j+2],nlist[j+3]);
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
    public static Point Sub_Point(Point a, Point b)
    {
        return new Point(a.X - b.X, a.Y - b.Y);
    }
    public static float Len_Point(Point a, Point b)
    {
        var d = Sub_Point(a,b);
        return  (float)Math.Sqrt((double)(d.X * d.X + d.Y * d.Y));
    }
    public static Point Abs(Point a)
    {
        return new Point(Math.Abs(a.X), Math.Abs(a.Y));
    }
    public static Point Center(Point a, Point b)
    {
        return new Point( (a.X + b.X) / 2, (a.Y + b.Y) /2 );
    }
}

