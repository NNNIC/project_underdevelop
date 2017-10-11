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

    void br_savelayout(Action<int,bool> state)
    {
        if (m_cmd == COMMAND.SAVELAYOUT)
        {
            SetNextState(state);
        }
    }

    void br_error(Action<int,bool> state)
    {
        if (!string.IsNullOrEmpty(m_error))
        {
            SetNextState(state);
        }
    }
}