using UnityEngine;
using System.Collections;
using System.Threading;
using slagipc;
using UnityEngine.SceneManagement;


public class slagipc_unity_main : MonoBehaviour {

    public static slagipc_unity_main V; //veridical pointer ... self pointer
    public static netcomm m_netcomm;

    bool m_bReqAbort;
    bool m_bEnd;

    void Awake()
    {
        V = this;
        
    }

	IEnumerator Start () {

        m_bReqAbort = false;
        m_bEnd      = false;

        UnityEngine.Debug.logger.logEnabled = false;

        netcomm.Log = (s)=>Debug.Log(s);
        FilePipe.Log= (s)=>Debug.Log(s);

        m_netcomm = new netcomm();
        m_netcomm.Start();

        slagtool.util.SetLogFunc(wk.SendWriteLine,wk.SendWrite);
        slagtool.util.SetDebugLevel(0);
        slagtool.util.SetBuitIn(typeof(slagipc_unity_builtinfunc));
        slagtool.util.SetCalcOp(slagipc_unity_builtincalc_op.Calc_op);

        slagipc.cmd.init();

        while(true)
        {
            if (m_bReqAbort) break;

            yield return null;
     
            if (m_bReqAbort) break;

            var cmd = m_netcomm.GetCmd();
            if (cmd==null)
            {
                continue;
            }
            slagipc.cmd.execute(cmd);
        }
        m_bEnd = true;
    }

    void Update()
    {
        if (!m_bReqAbort) wk.Update();
        if (!m_bReqAbort) slagipc.cmd_sub.UpdateExec();
    }

    void Reset()
    {
        StartCoroutine(_reset_co());
    }
    IEnumerator _reset_co()
    {

        Debug.Log("RESET!");

        if (m_netcomm!=null)
        {
            m_netcomm.Terminate();
            while(!m_netcomm.IsEnd()) yield return null;
            m_netcomm = null;
        }

        m_bReqAbort = true;

        while(!m_bEnd) yield return null;

        SceneManager.LoadScene("remotereset");
    }

}
