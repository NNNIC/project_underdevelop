using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

public class test : MonoBehaviour
{
    Server m_server;
    void Start()
    {
        m_server = new Server();
        m_server.Run();
    }

    void Update()
    {
        if (m_server==null) return;

        if (m_server.m_req.HasMsg())
        {
            var req = m_server.m_req.GetMsg();
            m_server.m_res.SetMsg("OK!" + Time.time);
        }
    }

    public void OnDestroy()
    {
        m_server.Stop();
    }
}
