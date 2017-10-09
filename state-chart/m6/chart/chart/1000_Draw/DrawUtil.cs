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

public partial class DrawUtil
{
    static float m_text_margin_width = D.m_text_margin_width;
    static float m_text_margin_height= D.m_text_margin_height;

    static float ARROWHEAD_WIDTH  = D.ARROWHEAD_WIDTH;
    static float ARROWHEAD_HEIGHT = D.ARROWHEAD_HEIGHT;


    public static void DrawBox_LineAndFill(
        Graphics g,
        PointF   point,
        float    width,
        float    height,
        float    lineWidth,
        Color    lineColor,
        Color    fillColor
    )
    {
        using (var pen = new Pen(fillColor, 1))
        {
            g.FillRectangle(pen.Brush, point.X, point.Y, width, height);
        }

        using (var pen = new Pen(lineColor, lineWidth))
        {
            g.DrawRectangle(pen,point.X,point.Y,width,height);
        }
    }

    public static void DrawText(
        Graphics g,
        string   text,
        string   fontname,
        Color    color,
        float    size,

        Rectangle rect
    )
    {
        using (var pen = new Pen(color, size))
        using (var font = new Font(fontname,size))
        {
            g.DrawString(text, font, pen.Brush, rect);
        }
    }

    public static void DrawBoxText_LineAndFill(
        Graphics g,
        string text,
        string fontname,
        Color  fontcolor,
        float  fontsize,

        PointF point,
        float  width,
        float  height,
        float  lineWidth,
        Color  lineColor,
        Color  fillColor
    )
    {
        DrawBox_LineAndFill(g,point,width,height,lineWidth,lineColor,fillColor);

        var textpoint = PointUtil.Add_XY(point, m_text_margin_width , m_text_margin_height);
        var textwidth = width - m_text_margin_width * 2;
        var textheight= height - m_text_margin_height * 2;
        var rect = new Rectangle((int)textpoint.X, (int)textpoint.Y,(int)textwidth,(int)textheight);
        DrawText(g,text,fontname,fontcolor,fontsize,rect);
    }

    public static void DrawBoxText_LineAndFill(
        Graphics g,
        string text,
        string fontname,
        Color  fontcolor,
        float  fontsize,

        Rectangle rect,

        float  lineWidth,
        Color  lineColor,
        Color  fillColor
    )
    {
        DrawBoxText_LineAndFill(g,text,fontname,fontcolor,fontsize, rect.Location,rect.Width,rect.Height,lineWidth,lineColor,fillColor);
    }

    public static SizeF GetTextBoxSize(Graphics g, string text, string fontname, float fontsize,float boxWidth)
    {
        var textWidth = boxWidth - m_text_margin_width * 2;
        var textSize  = GetTextSize(g,text,fontname,fontsize,textWidth);
        return new SizeF(boxWidth,textSize.Height + m_text_margin_height * 2);
    }

    public static SizeF GetTextSize(
        Graphics g,
        string text,
        string fontname,
        float  fontsize,
        float  width
        )
    {
        SizeF sz = default(Size);
        using (var font = new Font(fontname, fontsize))
        using (var sf = new StringFormat(StringFormat.GenericTypographic))
        {
            sz = g.MeasureString(text, font,int.MaxValue,sf);
        }

        return sz;
    }

    public static void DrawCircle_LinaAndFill(
        Graphics  g,
        Rectangle rect,
        Color     linecolor,
        Color     fillcolor
    )
    {
        using (var pen = new Pen(fillcolor, 1))
        {
            g.FillEllipse(pen.Brush,rect);
        }
        using (var pen = new Pen(linecolor, 1))
        {
            g.DrawEllipse(pen,rect);
        }
    }

    public static void DrawLine_obs(Graphics g,  Point a, Point b, Color color, int size,LineType type)
    {
        switch(type)
        {
            case LineType.STRAIGHT: DrawLine__straight_obs(g,a,b,color, size); break;
            default:                DrawLine__straight_obs(g,a,b,color, size); break;
        }
    }
    private static void DrawLine__straight_obs(Graphics g,  Point a, Point b, Color color, int size)
    {
        using (var pen = new Pen(color, size))
        {
            g.DrawLine(pen,a,b);
        }
    }
    public static void DrawLine(Graphics g, List<Point> plist, Color color, int size, LineType type)
    {
        if (plist == null) return;
        switch(type)
        {
            case LineType.STRAIGHT: DrawLine__straight(g,plist,color, size); break;
            case LineType.BEZIR:    DrawLine__bezir(g,plist,color,size);     break;
            default:                DrawLine__straight(g,plist,color, size); break;
        }
    }
    private static void DrawLine__straight(Graphics g,  List<Point> plist, Color color, int size)
    {
        if (plist.Count<=1) return;
        using (var pen = Pen_create_arrow(color, size))
        {
            g.DrawLines(pen,plist.ToArray());
        }
    }
    private static void DrawLine__bezir(Graphics g, List<Point> list, Color color, int size)
    {
        //ポイントリストをベジェ曲線用に分解する。
        //ベジェ曲線にするには、４点ずつの指定となる。
        //最初の４点は、開始点ー次点　次点ー後半分となる
        //その後は、前半点-次点　次点-後半点
        //最後の４点は、前半点-次点　次点ー最終点

        if (list.Count < 1) return;
        if (list.Count==2)
        {
            DrawLine__bezir(g,list[0],list[1],color,size);
            return;
        }

        var nlist= new List<Point>();

        for(var i = 0; ;i++)
        {
            Point b0,b1,b2,b3;
            if (i==0)
            {
                b0 = list[0];
                b1 = list[1];
                b2 = list[1];
                b3 = list.Count==3 ? list[2] : PointUtil.Center(list[1],list[2]);

                nlist.Add(b0);
                nlist.Add(b1);
                nlist.Add(b2);
                nlist.Add(b3);

                if (list.Count == 3) break;
            }
            else if (i==list.Count-3) //最後
            {
                b0 = PointUtil.Center(list[i],list[i+1]);
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
                b0 = PointUtil.Center(list[i],list[i+1]);
                b1 = list[i+1];
                b2 = list[i+1];
                b3 = PointUtil.Center(list[i+1],list[i+2]);
                nlist.Add(b0);
                nlist.Add(b1);
                nlist.Add(b2);
                nlist.Add(b3);
            }
        }

        using (var pen_head = Pen_create_arrow(color,size))
        using (var pen = Pen_create(color,size))
        {
            for(var i = 0; i<nlist.Count-4;i+=4)
            {
                g.DrawBezier(pen,nlist[i],nlist[i+1],nlist[i+2],nlist[i+3]);
            }
            var j = nlist.Count-4;
            g.DrawBezier(pen_head,nlist[j],nlist[j+1],nlist[j+2],nlist[j+3]);
        }
    }
    private static void DrawLine__bezir(Graphics g, Point a, Point b, Color color, int size)
    {
        if (a.X == b.X)
        {
            using(var pen_head = Pen_create_arrow(color,size))
            {
                g.DrawLine(pen_head,a,b);
            }
            return;
        }
        var c = PointUtil.Center(a,b);
        var p1 = a;
        var p2 = PointUtil.Mod_Y(c,a.Y);
        var p3 = PointUtil.Mod_Y(c,b.Y);
        var p4 = b;

        using(var pen_head = Pen_create_arrow(color,size))
        {
            g.DrawBezier(pen_head,p1,p2,p3,p4);
        }

    }

    private static Pen   Pen_create(Color color, int size)
    {
        return new Pen(color,size);
    }
    private static Pen   Pen_create_arrow(Color color, int size)
    {
        var pen = Pen_create(color, size);
        //pen.EndCap = LineCap.ArrowAnchor;
        pen.CustomEndCap = new AdjustableArrowCap(ARROWHEAD_WIDTH,ARROWHEAD_HEIGHT);
        return pen;
    }
}
