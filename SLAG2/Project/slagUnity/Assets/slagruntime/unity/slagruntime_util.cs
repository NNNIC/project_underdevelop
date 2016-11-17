using UnityEngine;
using System.Collections;
using System;

namespace slagruntime { 
    public class util
    {
        private static string m_tmp;
        public static void SendWrite(string s)
        {
            m_tmp += s;
        }
        public static void SendWriteLine(string s=null)
        {
            m_tmp += s;
            m_tmp = null;
            slagruntime_main.m_comm.SendMsg(s);
            Debug.Log(s);
        }

        static object m_logmtx = new object();
        static string m_logbuf;
        public static void Log(string s)
        {
            lock(m_logmtx)
            { 
                if (m_logbuf!=null) m_logbuf += System.Environment.NewLine;
                m_logbuf += s;
            }
        }

        public static void Update()
        {
            if (m_logbuf!=null) Debug.Log(m_logbuf);
            m_logbuf = null;
        }
    }

}
