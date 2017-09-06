using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class StateManager_wait {

    public Action<string> m_callfuncname;

    Action<bool> m_curFunc;
    Action<bool> m_nextFunc;
    Action<bool> m_tempFunc;

    float m_limitTime;

    public void Update()
    {
        if (Time.time < m_limitTime) return; 

        if (m_nextFunc!=null)
        {
            m_curFunc = m_nextFunc;
            m_nextFunc = null;

            if (m_callfuncname!=null) m_callfuncname(m_curFunc.Method.Name);

            m_curFunc(true);
        }
        else
        {
            if (m_curFunc!=null)
            {
                m_curFunc(false);
            }
        }
    }

    public void Goto(Action<bool> func, float time = -1)
    {
        m_nextFunc = func;
        if (time>=0)
        {
            SetWait(time);
        }
    }
    public bool CheckState(Action<bool> func)
    {
        return (m_curFunc == func);
    }
    public void SetWait(float time)
    {
        m_limitTime = Time.time + time;
    }

    public void SetNext(Action<bool> func)
    {
        m_tempFunc = func;
    }

    public void GoNext()
    {
        m_nextFunc = m_tempFunc;
        m_tempFunc = null;
    }

}
