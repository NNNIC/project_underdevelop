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

public partial class MoveFlowStateControl : StateControlBase {
    
    #region アクセス
    ChartViewer         m_cv        { get { return ChartViewer.V;                  } }
    PictureBox          m_pbmain    { get { return m_cv.pictureBox_main;           } }
    PictureBox          m_pbhl      { get { return m_cv.pictureBox_highlite;       } }
    PictureBox          m_pbsl      { get { return m_cv.pictureBox_select;         } }
    List<StateData>     m_stateData { get { return StateInfo.m_stateData;          } }
    public ChartManager m_chartman  { get { return chart.ChartViewer.V.m_chartman; } }
    #endregion
    public void Init()
    {
        sc_start(S_WAIT_MOUSEDOWN);
    }

    public void Update()
    {
        sc_update();
    }
}
