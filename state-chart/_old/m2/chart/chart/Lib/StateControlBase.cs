using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StateControlBase
{
    protected StateManagerWithPhase m_sm;

    protected void sc_start(Action<int,bool> state=null)
    {
        m_sm = new StateManagerWithPhase();
        m_sm.SetNext(state);
        m_sm.GoNext();
    }

    protected void sc_update()
    {
        m_sm.Update();
    }

    protected void SetNextState(Action<int,bool> state=null)
    {
        m_sm.SetNext(state);
    }

    protected bool HasNextState()
    {
        return m_sm.HasNextState();
    }

    protected void GoNextState()
    {
        m_sm.GoNext();
    }
    protected void NextPhase()
    {
        m_sm.NextPhase(true);
    }
}
