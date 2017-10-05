using System;
using System.Collections.Generic;

public partial class MainFlowStateControl : StateControlBase {
    
    public enum COMMAND
    {
        NONE,
        LOAD,
    }


    //StateManagerWithPhase m_sm;
    COMMAND m_cmd = COMMAND.NONE;
    string  m_error = null;

    public ChartManager_obs m_chartman { get { return chart.Form1.V.m_chartman; } }
    public ExcelProgram m_excelpgm { get; private set;                        }

    public void Start()
    {
        //m_sm = new StateManagerWithPhase();
        //m_sm.SetNext(S_START);
        //m_sm.GoNext();
        sc_start(S_START);
    }

    public void Update()
    {
        m_sm.Update();
    }

    //void SetNextState(Action<int,bool> state=null)
    //{
    //    m_sm.SetNext(state);
    //}

    //bool HasNextState()
    //{
    //    return m_sm.HasNextState();
    //}

    //void GoNextState()
    //{
    //    m_sm.GoNext();
    //}
    
    string m_filename = string.Empty;
    public void Load(string file)
    {
        m_filename = file;
        m_cmd = COMMAND.LOAD;
    }
}