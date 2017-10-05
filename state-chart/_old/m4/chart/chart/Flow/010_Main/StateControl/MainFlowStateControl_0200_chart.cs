using System;

public partial class MainFlowStateControl {
    
    void chart_draw()
    {
        m_chartman.Create();
    }
    
    void chart_edit_start()
    {
    }
    bool chart_edit_isDone()
    {
        return true;
    }
}