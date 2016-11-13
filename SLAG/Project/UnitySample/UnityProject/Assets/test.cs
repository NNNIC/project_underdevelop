using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

public class test : MonoBehaviour
{
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
