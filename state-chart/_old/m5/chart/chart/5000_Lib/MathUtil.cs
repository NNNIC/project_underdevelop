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
using LineType=DrawUtil.LineType;
using D=Define;
//>>>

public class MathUtil
{
    public static float Min(float a, float b, float c, float d)
    {
        var x = Math.Min(a,b);
        x     = Math.Min(x,c);
        x     = Math.Min(x,d);
        return x;
    }
    public static float Max(float a, float b, float c, float d)
    {
        var x = Math.Max(a,b);
        x     = Math.Max(x,c);
        x     = Math.Max(x,d);
        return x;
    }

}
