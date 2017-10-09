using System;
using System.Drawing;
using System.Windows.Forms;

public partial class MainFlowStateControl {
    

    void load_clear()
    {
        m_filename = null;
    }

    void load_excel()
    {
        m_excelpgm = new ExcelProgram();
        try {
            m_excelpgm.Load(m_filename);
        } catch (SystemException e)
        {
            m_error = e.Message;
        }
    }
}