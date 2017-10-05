using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class RectUtil
{
    public static Rectangle Add(Rectangle a, Rectangle b)
    {
        var min_x = MathUtil.Min(a.X,b.X,a.X+a.Width,b.X+b.Width);
        var max_x = MathUtil.Max(a.X,b.X,a.X+a.Width,b.X+b.Width);

        var min_y = MathUtil.Min(a.Y,b.Y,a.Y+a.Height,b.Y+b.Height);
        var max_y = MathUtil.Max(a.Y,b.Y,a.Y+a.Height,b.Y+b.Height);
       
        return new Rectangle((int)min_x,(int)min_y,   (int)(max_x - min_x), (int)(max_y - min_y));
    }
}
