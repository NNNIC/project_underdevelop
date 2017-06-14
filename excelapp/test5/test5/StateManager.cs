using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StateManager 
{
    Action<bool> m_curstate;
    Action<bool> m_nextstate;
    
    int        m_limit;

    //リクエスト
    public void Goto(Action<bool> func, int milliseconds = 0) 
    { 
        m_nextstate = func;  
        Wait(milliseconds);
    }
    
    //ウエイト
    public void Wait(int millisenconds)
    {
        m_limit = DateTime.Now.Millisecond + millisenconds;
    }

    //更新
    public void Update()
    {
        if (m_limit > DateTime.Now.Millisecond) return;

        if (m_nextstate!=null)
        {
            m_curstate = m_nextstate;
            m_nextstate = null;
            m_curstate(true);
        }
        else
        {
            if (m_curstate!=null)
            {
                m_curstate(false);
            }
        }
    }
}
