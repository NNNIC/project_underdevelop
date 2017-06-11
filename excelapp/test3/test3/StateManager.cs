using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StateManager 
{
    Action<bool> m_curstate;
    Action<bool> m_nextstate;
    
    //リクエスト
    public void Goto(Action<bool> func) 
    { 
        m_nextstate = func;  
    }
    
    //更新
    public void Update()
    {
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
