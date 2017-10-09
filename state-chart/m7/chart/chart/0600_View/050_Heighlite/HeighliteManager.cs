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

    /*
        ストーリー

        1.カーソル位置確認
        2.あるステートのFrame内にカーソルがある
        3.フレームサイズの半透明pictureboxを生成
        4.ステートと重ねる

        5.カーソル位置確認
        6.別のステートのフレーム内にカーソルがある
        7.先の削除し、生成
        8.ステートと重ねる

        9.カーソル位置確認
        10.下に何もない
        11.削除

    */


public partial class HeighliteManager
{
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

    StateData   m_cur; //現在フォーカス中のステート


    public void Init()
    {
        m_bShow = false;
        m_cur   = null;

        m_pbhl.BackColor = Color.FromArgb(80,255,255,0);
        m_pbhl.Parent   =  m_pbmain;
    }

    public void Upadte()
    {
        var st = GetStateAtCursor();
        if (st!=m_cur)
        {
            m_cur =st;
            if (m_cur == null)
            {
                Hide();
            }
            else
            {
                Hide();
                CreateNewSelectState();
            } 

        }   
    }

    private void Hide()
    {
        m_bShow = false;
    }

    private void CreateNewSelectState()
    {
        var rect= m_cur.m_layout.offset_Frame;
        m_pbhl.Size = rect.Size;
        m_pbhl.Location = rect.Location;
        m_bShow = true;
    }
}
