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
    Bitmap   m_select_bitmap;
    Graphics m_gSel; 
    bool     m_select_fail;
    Point    m_saveCuirsorPos_inSelect;
    void select_create() {
        m_select_fail   = false;
        if (m_st!=null && m_st.m_layout!=null)
        {
            m_pbsl.Location = m_st.m_layout.offset_Frame.Location;
            m_pbsl.Size     = m_st.m_layout.offset_Frame.Size;
            m_select_bitmap = new Bitmap(m_pbsl.Width,m_pbsl.Height);
            m_pbsl.Image    = m_select_bitmap;
            m_gSel          = Graphics.FromImage(m_select_bitmap);
            DrawStateBox.DrawLayout_for_select(m_gSel,m_st.m_layout);
            m_pbsl.Show();

            m_saveCuirsorPos_inSelect = m_pbsl.PointToClient(Cursor.Position);
        }

    }
    void select_clear() {
        m_pbsl.Hide();

        if (m_gSel!=null) {
            m_gSel.Dispose();
            m_gSel = null;
        }
        if (m_select_bitmap!=null)
        {
            m_select_bitmap.Dispose();
            m_select_bitmap = null;
        }
    }
}

