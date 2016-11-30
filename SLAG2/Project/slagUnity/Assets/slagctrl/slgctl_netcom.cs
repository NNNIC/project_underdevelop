﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace slgctl
{
    public class netcomm
    {
        string m_self_ip   = "unity";

        string m_to_ip   = "mon";

        FilePipe       m_pipe;
        Queue<string> m_log;
        Thread        m_thread;
        
        object        m_mtx;
        string        m_cmd;

        static netcomm V;

        public void Start()
        {
            V = this;
            m_mtx = new object();

            m_pipe   = new FilePipe(m_self_ip);
            m_pipe.Start(wk.Log);

            m_log    = new Queue<string>();
            m_thread = new Thread(Work);
            m_thread.Start();
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
            }
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
                    if (s!=null) s+=Environment.NewLine;
                    s+=m_log.Dequeue();
                }
            }
            if (s!=null) m_pipe.Write(s,m_to_ip);
            m_pipe.Update();
        }
        #endregion

        #region cmd
        public string GetCmd()
        {
            string s = null;
            lock(V.m_mtx)
            {
                s= V.m_cmd;
                V.m_cmd = null;
            }
            return s;
        }
        #endregion

    }
}
