using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using slag;

public class test2 : MonoBehaviour {

    TcpPipe m_pipe;
    Queue<string> m_cmd;
   

	void Start () {
        m_pipe = new TcpPipe("127.0.0.1",2001);
        m_pipe.Start(s=>Debug.Log(s));
        m_cmd = new Queue<string>();
        StartCoroutine(Interactive());
	}
	
	void Update () {
        m_pipe.Update();
        var readmsg = m_pipe.Read();
        if (readmsg!=null)
        {
            //                      0123456789
            if (readmsg.StartsWith("<slag>cmd:"))
            {
                var s = readmsg.Substring(10);
                m_cmd.Enqueue(s);
            }
        }
	}

    IEnumerator Interactive()
    {
        string log = null;
        Action<string> write = (s)=> {
            log += s;
        };
        Action<string> writeline = (s)=> {
            log = "<slag>" + log + s;
            pipe_write(log);
        };
        Func<string> readcmd = ()=> {
            if (m_cmd!=null && m_cmd.Count>0)
            {
                return m_cmd.Dequeue();
            }
            return null;
        };
        
        monitor.interactive(write,writeline,readcmd);
        yield return null;
    }

    //---
    void pipe_write(string s)
    {
        m_pipe.Write("127.0.0.1",2002,s);
    }

}
