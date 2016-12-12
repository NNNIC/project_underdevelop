using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace slgctl
{
    public class netcomm
    {
        string m_myname  = "unity";

        string m_toname  = "mon";

        FilePipe       m_pipe;
        Queue<string> m_log;
        Thread        m_thread;
        
        object        m_mtx;
        string        m_cmd;

        bool          m_bReqAbort;
        bool          m_bEnd;

        public void Start()
        {
            m_bReqAbort = false;
            m_bEnd      = false;

            m_mtx = new object();

            m_pipe   = new FilePipe(m_myname);
            m_pipe.Start(wk.Log);

            m_log    = new Queue<string>();
            m_thread = new Thread(Work);
            m_thread.Start();
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
                if (m_pipe.IsEnd() && m_log.Count==0)
                {
                    m_bReqAbort = true;
                    m_pipe = null;
                }
                return false;
            }

            if (m_bEnd && m_thread!=null)
            {
                m_thread.Abort();
                m_thread.Join();
                m_thread = null;
                return false;
            }

            return m_bEnd;
        }

        private void Work()
        {
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
            lock(m_log)
            {
                m_log.Enqueue(s);
            }
        }
        private void _update()
        {
            string s = null;
            lock(m_log)
            {
                while(m_log.Count>0)
                {
                    if (m_bReqAbort) break;

                    if (s!=null) s+=Environment.NewLine;
                    s+=m_log.Dequeue();
                }
            }
            if (!m_bReqAbort)
            { 
                if (s!=null) m_pipe.Write(s,m_toname);
                m_pipe.Update();
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
