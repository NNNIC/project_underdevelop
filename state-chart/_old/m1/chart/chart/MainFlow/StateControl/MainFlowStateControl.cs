using System;

public partial class MainFlowStateControl {
    
    public enum COMMAND
    {
        NONE,
        LOAD,
    }

    StateManaterWithPhase m_sm;
    COMMAND m_cmd = COMMAND.NONE;

    public void Start()
    {
        m_sm = new StateManaterWithPhase();
        m_sm.SetNext(S_NONE);
        m_sm.GoNext();
    }

    public void Update()
    {
        m_sm.Update();
    }

    void SetNextState(Action<int,bool> state=null)
    {
        m_sm.SetNext(state);
    }

    bool HasNextState()
    {
        return m_sm.HasNextState();
    }

    void GoNextState()
    {
        m_sm.GoNext();
    }
    
    string m_filename = string.Empty;
    public void Load(string file)
    {
        m_filename = file;
        m_cmd = COMMAND.LOAD;
    }
}