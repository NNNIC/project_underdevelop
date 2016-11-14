using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace slagruntime
{
    public class controller
    {
        string m_self_ip   = "127.0.0.1";
        int    m_self_port = 2001;

        string m_to_ip   = "127.0.0.1";
        int    m_to_port = 2002;

        TcpPipe       m_pipe;
        Queue<string> m_log;
        Thread        m_thread;
        


        public void Start()
        {
            m_pipe   = new TcpPipe(m_self_ip,m_self_port);
            m_log    = new Queue<string>();
            m_thread = new Thread(Work);
            m_thread.Start();
        }

        
        private void Work()
        {
            while(true)
            {
                Update();

                var cmd = m_pipe.Read();
                process(cmd);
                Thread.Sleep(33);
            }
        }

        private void process(string cmd)
        {

        }

        #region ログ
        public void Log(string s)
        {
            lock(m_log)
            {
                m_log.Enqueue(s);
            }
        }
        public void Update()
        {
            string s = null;
            lock(m_log)
            {
                while(m_log.Count>0)
                {
                    if (s!=null) s+=Environment.NewLine;
                    s+=m_log.Dequeue();
                }
            }
            m_pipe.Write(m_to_ip,m_to_port,s);
        }

        #endregion
    }
}
