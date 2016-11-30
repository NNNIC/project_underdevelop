using UnityEngine;
using System.Collections;
using System.Threading;
using slgctl;

public class slgctl_main : MonoBehaviour {

    public static netcomm m_netcomm;

	IEnumerator Start () {
        m_netcomm = new netcomm();
        m_netcomm.Start();

        slagtool.util.SetLogFunc(wk.SendWrite,wk.SendWriteLine);
        slagtool.util.SetBuitIn(typeof(unity_builtinfunc));

        while(true)
        {
            yield return null;
            var cmd = m_netcomm.GetCmd();
            if (cmd==null)
            {
                continue;
            }
            slgctl.cmd.execute(cmd);
        }
    }

    void Update()
    {
        wk.Update();
        slgctl.cmd_sub.UpdateExec();
    }
}
