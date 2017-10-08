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

public partial class StateData
{
    public List<Point>   m_ArrowLine_toNext;
    public List<Point>[] m_ArrowLine_branches;
}
