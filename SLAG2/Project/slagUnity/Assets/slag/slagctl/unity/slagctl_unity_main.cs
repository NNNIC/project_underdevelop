using UnityEngine;
using System.Collections;
using System.Threading;
using slagctl;
using UnityEngine.SceneManagement;


public class slagctl_unity_main : MonoBehaviour {

    public static slagctl_unity_main V; //veridical pointer ... self pointer
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

        slagtool.util.SetLogFunc(wk.SendWrite,wk.SendWriteLine,0);
        slagtool.util.SetBuitIn(typeof(slagctl_unity_builtinfunc));
        slagtool.util.SetCalcOp(slagctl_unity_builtincalc_op.Calc_op);

        slagctl.cmd.init();

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
            slagctl.cmd.execute(cmd);
        }
        m_bEnd = true;
    }

    void Update()
    {
        if (!m_bReqAbort) wk.Update();
        if (!m_bReqAbort) slagctl.cmd_sub.UpdateExec();
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
