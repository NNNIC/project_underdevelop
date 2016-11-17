using UnityEngine;
using System.Collections;
using System.Threading;
using slagruntime;

public class slagruntime_main : MonoBehaviour {

    public static communicate m_comm;

	IEnumerator Start () {
        m_comm = new communicate();
        m_comm.Start();

        slagtool.process.SetLogFunc(util.SendWrite,util.SendWriteLine);

        while(true)
        {
            yield return null;
            var cmd = m_comm.GetCmd();
            if (cmd==null)
            {
                continue;
            }   
            command.execute(cmd);
        }
    }

    void Update()
    {
        util.Update();
    }
}
