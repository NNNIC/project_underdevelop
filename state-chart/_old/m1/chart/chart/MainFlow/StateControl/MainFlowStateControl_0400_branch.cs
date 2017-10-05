using System;

public partial class MainFlowStateControl {
    
    void br_loadexcel(Action<int,bool> state)
    {
        if (m_cmd== COMMAND.LOAD)
        {
            SetNextState(state);
        }
    }
    
    void br_editchart(Action<int,bool> state)
    {
    }
}