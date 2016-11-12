using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

public class test : MonoBehaviour
{
    //Server m_server;
    //void Start()
    //{
    //    m_server = new Server();
    //    m_server.Run();
    //}

    //void Update()
    //{
    //    if (m_server==null) return;

    //    if (m_server.m_req.HasMsg())
    //    {
    //        var req = m_server.m_req.GetMsg();
    //        m_server.m_res.SetMsg("OK!" + Time.time);
    //    }
    //}

    //public void OnDestroy()
    //{
    //    m_server.Stop();
    //}

    //TcpServer m_server;
    //void Start()
    //{
    //    m_server = new TcpServer();
    //    m_server.Start();
    //}

    //void Update()
    //{
    //    m_server.Update();
    //}

    TcpPipe m_pipe;
    void Start()
    {
        m_pipe = new TcpPipe("127.0.0.1",2001);
        m_pipe.Start(s=>Debug.Log(s));
    }
    void Update()
    {
        m_pipe.Update();
        var readmsg = m_pipe.Read();
        if (readmsg!=null)
        {
            m_pipe.Write("127.0.0.1",2002,"REPLY:" + Time.time +"<" + readmsg);
        }

    }
}
