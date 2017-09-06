using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateManager {

    Action<bool> m_curFunc;
    Action<bool> m_nextFunc;

    public void Update()
    {
        if (m_nextFunc!=null)
        {
            m_curFunc = m_nextFunc;
            m_nextFunc = null;

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

    public void Goto(Action<bool> func)
    {
        m_nextFunc = func;
    }

    public bool CheckState(Action<bool> func)
    {
        return (m_curFunc == func);
    }
}

public class StateManager_extend {
    Action<bool> m_curFunc;
    Action<bool> m_nextFunc;

    bool m_bNoWait;

    public void Update()
    {
        for(var loop = 0; loop<100; loop++)
        {
            m_bNoWait = false;
            _update();
            if (m_bNoWait)
            {
                continue;
            }
            else
            {
                break;
            }
        }
    }


    void _update()
    {
        if (m_nextFunc!=null)
        {
            m_curFunc = m_nextFunc;
            m_nextFunc = null;

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

    public void Goto(Action<bool> func, bool bNoWait=false)
    {
        m_nextFunc = func;
        m_bNoWait = bNoWait;
    }

    public bool CheckState(Action<bool> func)
    {
        return (m_curFunc == func);
    }
}


