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

public partial class MoveFlowStateControl  {

    void selectpos_update()
    {
        var pos = m_pbmain.PointToClient(Cursor.Position);
        m_pbsl.Location = PointUtil.Sub_Point(pos,m_saveCuirsorPos_inSelect);
    }


}

