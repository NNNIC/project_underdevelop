using UnityEngine;
using System.Collections;
using slagctl;

public class slagctl_unity_statemanager : MonoBehaviour {

    public class StateManager
    {
        string m_cur;
        string m_next;

        int    m_waitcnt;
        float  m_waittime;

        public void Goto(string func)      { m_next     = func; }
        public void WaitCount(int c)       { m_waitcnt  = c;    }
        public void WaitTime(float time)   { m_waittime = time; }

        public void Update(float deltaTime)
        {
            if (m_waitcnt>0)
            {
                m_waitcnt--;
                return;
            }
            if (m_waittime>0)
            {
                m_waittime -= deltaTime;
                return;
            }

            bool bFirst = false;
            if (m_next!=null)
            {
                m_cur  = m_next;
                m_next = null;
                bFirst = true;
            }
            if (cmd_sub.m_slag!=null &&  m_cur!=null) { 
                cmd_sub.m_slag.CallFunc(m_cur,new object[1] { bFirst });
            }
        }
    }

    StateManager m_sm;

	public void Init () {
        if (m_sm==null)
        {   
            m_sm = new StateManager();
        }
	}

    void Start()
    {
        Init();
    }
	
	void Update () {
	   m_sm.Update(Time.deltaTime);
	}

    public void Goto(string func)
    {
        m_sm.Goto(func);
    }

    public void WaitCount(int cnt)
    {
        m_sm.WaitCount(cnt);
    }

    public void WaitTime(float time)
    {
        m_sm.WaitTime(time);
    }
}
