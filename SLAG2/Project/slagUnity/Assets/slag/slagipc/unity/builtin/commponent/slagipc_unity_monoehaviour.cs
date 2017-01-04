using UnityEngine;
using System.Collections;

public class slagipc_unity_monoehaviour : MonoBehaviour {

    public string m_startFunc;
    public string m_updateFunc;
    public string m_onDestroyFunc;
    public string m_onMouseUpAsButtonFunc;

	// Use this for initialization
	void Start () {
        callfunc(m_startFunc);
	}
	
	// Update is called once per frame
	void Update () {
        callfunc(m_updateFunc);
	}

    void OnDestroy()
    {
        callfunc(m_onDestroyFunc);
    }

    void OnMouseUpAsButton()
    {
        callfunc(m_onMouseUpAsButtonFunc);
    }
    //-- util for this class
    void callfunc(string funcname)
    {
        if (slagipc.cmd_sub.m_slag!=null && !string.IsNullOrEmpty(funcname))
        { 
    	    slagipc.cmd_sub.m_slag.CallFunc(funcname,new object[1] { gameObject });
        }
    }

}
