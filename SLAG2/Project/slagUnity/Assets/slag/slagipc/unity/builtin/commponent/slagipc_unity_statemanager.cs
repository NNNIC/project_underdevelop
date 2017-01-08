using UnityEngine;
using System.Collections;
using slagipc;
using slagtool;

public class slagipc_unity_statemanager : MonoBehaviour {

#if obs
    public class StateManager
    {
        string m_cur;
        string m_next;

        int    m_waitcnt;
        float  m_waittime;

        float  dbg_elapsedtime=0; //時間計測

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
                if (m_cur!=null) wk.Log("!" + m_cur + " elapsed " + dbg_elapsedtime +" sec ! (wo synctime)");
                dbg_elapsedtime = 0;
                m_cur  = m_next;
                m_next = null;
                bFirst = true;
            }
            if (cmd_sub.m_slag!=null &&  m_cur!=null) {
                var save = Time.realtimeSinceStartup;
                cmd_sub.m_slag.CallFunc(m_cur,new object[1] { bFirst });
                dbg_elapsedtime += Time.realtimeSinceStartup - save;
            }
        }
    }
#else
    public class StateManager
    {
        YVALUE m_cur;
        YVALUE m_next;

        int    m_waitcnt;
        float  m_waittime;

        float  dbg_elapsedtime=0; //時間計測

        public void Goto(YVALUE func)      { m_next     = func; }
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
                if (m_cur!=null) wk.Log("!" + m_cur + " elapsed " + dbg_elapsedtime +" sec ! (wo synctime)");
                dbg_elapsedtime = 0;
                m_cur  = m_next;
                m_next = null;
                bFirst = true;
            }
            if (cmd_sub.m_slag!=null &&  m_cur!=null) {
                var save = Time.realtimeSinceStartup;
                if (sys.USETRY)
                {
                    try {  
            	        cmd_sub.m_slag.CallFunc(m_cur,new object[1] { bFirst });
                    }
                    catch (System.Exception e)
                    {
                        slagtool.sys.logline("--- 例外発生 ---");
                        slagtool.sys.logline(e.Message);
                        slagtool.sys.logline("----------------");
                    }
                }
                else
                { 
                    cmd_sub.m_slag.CallFunc(m_cur,new object[1] { bFirst });
                }
                dbg_elapsedtime += Time.realtimeSinceStartup - save;
            }
        }
    }

#endif

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

    public void Goto(slagtool.YVALUE v)
    {
        m_sm.Goto(v);
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
