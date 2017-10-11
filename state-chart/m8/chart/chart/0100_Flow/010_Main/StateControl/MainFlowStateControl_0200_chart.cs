using System;

public partial class MainFlowStateControl {
    
    void chart_draw()
    {
        m_chartman.Create();
        m_chartman.CreateArrowLine();
        m_chartman.Draw();
    }
    
    void chart_edit_start()
    {
    }
    bool chart_edit_isDone()
    {
        return true;
    }
}