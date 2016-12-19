using UnityEngine;
using System.Collections;

public class slag_monoehaviour : MonoBehaviour {

    public string m_startfunc;
    public string m_updatefunc;
    public string m_ondestroyfunc;

	// Use this for initialization
	void Start () {
        if (slgctl.cmd_sub.m_slag!=null && !string.IsNullOrEmpty(m_startfunc))
        { 
    	    slgctl.cmd_sub.m_slag.CallFunc(m_startfunc,new object[1] { gameObject });
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (slgctl.cmd_sub.m_slag!=null && !string.IsNullOrEmpty(m_updatefunc))
        { 
    	    slgctl.cmd_sub.m_slag.CallFunc(m_updatefunc, new object[1] { gameObject });
        }
	}

    void OnDestroy()
    {
        if (slgctl.cmd_sub.m_slag!=null && !string.IsNullOrEmpty(m_ondestroyfunc))
        { 
    	    slgctl.cmd_sub.m_slag.CallFunc(m_ondestroyfunc,new object[1] { gameObject });
        }
    }
}
