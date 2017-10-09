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

public partial class HeighlightFlowStateControl :StateControlBase {
    #region アクセス
    ChartViewer m_cv            { get { return ChartViewer.V;            } }
    PictureBox  m_pbmain        { get { return m_cv.pictureBox_main;     } }
    PictureBox  m_pbhl          { get { return m_cv.pictureBox_highlite; } }
    List<StateData> m_stateData { get { return StateInfo.m_stateData;    } }
    #endregion

    public bool m_bShow {
        get { return m_bShow; }
        set {
            if (value==true) {
                m_pbhl.Show();
            }
            else {
                m_pbhl.Hide();
            }
            __bShow = value;
        }
    }
    bool __bShow;

    StateData   m_cur;   //現在フォーカス中のステート
    StateData   m_target;//チェックされたステート

    public void Init()
    {
        m_bShow  = false;
        m_cur    = null;
        m_target = null;

        m_pbhl.BackColor = Color.FromArgb(80,255,255,0);
        m_pbhl.Parent   =  m_pbmain;

        sc_start(S_IDLE);
    }

    public void Update()
    {
        while(true)
        {
            sc_update();
            if (m_sm.CheckState(S_IDLE))
            {
                break;
            }
        }
    }

}
