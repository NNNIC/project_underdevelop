using System;
using System.Drawing;
using System.Windows.Forms;

public partial class MainFlowStateControl {
    void error_if_show()
    {
        if (!string.IsNullOrEmpty(m_error))
        {
            MessageBox.Show(m_error);
        }
    }
    void error_clear()
    {
        m_error = null;
    }	    
}