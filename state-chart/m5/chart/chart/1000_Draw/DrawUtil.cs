//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Detail=DrawStateBox.Detail;
//>>>

public class DrawUtil
{
    public static float m_text_margin_width = 5;
    public static float m_text_margin_height= 5;

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
}
