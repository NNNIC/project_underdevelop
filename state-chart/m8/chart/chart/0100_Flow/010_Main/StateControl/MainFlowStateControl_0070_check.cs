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

public partial class MainFlowStateControl {
    
    void check_args()
    {
        var args = Environment.GetCommandLineArgs();
        if (args!=null && args.Length>1)
        {
            Load(args[1]);
        }
    }
}