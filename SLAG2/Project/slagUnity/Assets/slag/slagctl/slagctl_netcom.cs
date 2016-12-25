using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace slagctl
{
    public class netcomm
    {
        public static Action<string> Log = (s)=>{ System.Diagnostics.Debug.WriteLine(s);  };

        string m_myname  = "unity";

        string m_toname  = "mon";

        FilePipe       m_pipe;
        Queue<string>  m_sendlog;
        Thread         m_thread;
        
        object        m_mtx;
        string        m_cmd;

        bool          m_bReqAbort;
        bool          m_bEnd;

        public void Start()
        {
            //Log("netcomm:start");

            m_bReqAbort = false;
            m_bEnd      = false;

            m_mtx = new object();
            //Log("netcomm:start+1");

            m_pipe   = new FilePipe(m_myname);
            m_pipe.Start(wk.Log);
            //Log("netcomm:start+2");

            m_sendlog   = new Queue<string>();
            m_thread = new Thread(Work);
            m_thread.IsBackground = true;
            m_thread.Priority = ThreadPriority.AboveNormal;
            m_thread.Start();

            //Log("netcomm:start+3");
        }

        public void Terminate()
        {
            if (m_pipe!=null)
            {
                m_pipe.Terminate();
            }
        }

        public bool IsEnd()
        {
            if (m_pipe!=null)
            {
                if (m_pipe.IsEnd())
                {
                    m_bReqAbort = true;
                    m_pipe = null;
                }
                return false;
            }

            if (m_bEnd && m_thread!=null)
            {
                m_thread.Join();
                m_thread = null;
                return false;
            }

            return m_bEnd;
        }

        private void Work()
        {
            //Log("nectcomm:Work.START");

            try { 
                while(true)
                {
                    _update();

                    var cmd = m_pipe.Read();
                    if (cmd!=null)
                    { 
                        record(cmd);
                    }
                    else
                    { 
                        Thread.Sleep(33);
                    }
                    if (m_bReqAbort) break;
                }
            }
            catch (SystemException e) { Log(e.Message);   }

            //Log("nectcomm:Work.END");
            m_bEnd = true;
        }

        private void record(string cmd)
        {
            lock(m_mtx)
            { 
                m_cmd = cmd;
            }
        }

        #region ログ
        public void SendMsg(string s)
        {
            Log(s);
            lock(m_sendlog)
            {
                m_sendlog.Enqueue(s);
            }
        }
        private void _update()
        {
            string s = null;
            lock(m_sendlog)
            {
                while(m_sendlog.Count>0)
                {
                    if (m_bReqAbort) break;

                    if (s!=null) s+=Environment.NewLine;
                    s+=m_sendlog.Dequeue();
                }
            }
            if (!m_bReqAbort)
            { 
                if (s!=null) m_pipe.Write(s,m_toname);
            }
        }
        #endregion

        #region cmd
        public string GetCmd()
        {
            string s = null;
            lock(m_mtx)
            {
                s= m_cmd;
                m_cmd = null;
            }
            return s;
        }
        #endregion
    }
}
