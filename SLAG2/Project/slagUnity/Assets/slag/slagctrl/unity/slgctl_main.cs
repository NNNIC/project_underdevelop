using UnityEngine;
using System.Collections;
using System.Threading;
using slgctl;
using UnityEngine.SceneManagement;

public class slgctl_main : MonoBehaviour {

    public static netcomm m_netcomm;

    bool m_bReqAbort;
    bool m_bEnd;

	IEnumerator Start () {

        m_bReqAbort = false;
        m_bEnd      = false;

        m_netcomm = new netcomm();
        m_netcomm.Start();

        slagtool.util.SetLogFunc(wk.SendWrite,wk.SendWriteLine,1);
        slagtool.util.SetBuitIn(typeof(unity_builtinfunc));

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
            slgctl.cmd.execute(cmd);
        }
        m_bEnd = true;
    }

    void Update()
    {
        if (!m_bReqAbort) wk.Update();
        if (!m_bReqAbort) slgctl.cmd_sub.UpdateExec();
    }

    void Reset()
    {
        StartCoroutine(_reset_co());
    }
    IEnumerator _reset_co()
    {

        Debug.Log("Reset!!");

        if (m_netcomm!=null)
        {
            m_netcomm.Terminate();
            while(!m_netcomm.IsEnd()) yield return null;
            m_netcomm = null;
        }

        Debug.Log(".");
        m_bReqAbort = true;

        while(!m_bEnd) yield return null;
        Debug.Log("..");

        SceneManager.LoadScene("reset");
    }

}
