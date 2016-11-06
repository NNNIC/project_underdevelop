﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace langtool.runtime.builtin
{
    class builtin_func
    {
        public class item
        {
            public bool   bSysOrApp;
            public string name;
            
            public MethodInfo mi;

            public string Help()
            {
                var s =  mi.Invoke(null,new object[] {true,null,null});
                return s.ToString();
            }
            public object Exec(object[] ol, StateBuffer sb=null)
            {
                return mi.Invoke(null,new object[] {false,ol,sb });
            }
        }

        public static Hashtable m_hash;

        public static void Init()
        {
            m_hash = new Hashtable();

            foreach(var m in typeof(builtin_sysfunc).GetMethods())
            {
                var n = m.Name.ToUpper();
                if (!n.StartsWith("F_")) continue;
                n = n.Substring(2);
                m_hash[n] = new item() {bSysOrApp=true, name = m.Name.Substring(2), mi = m };                                
            }
            foreach(var m in typeof(builtin_appfunc).GetMethods())
            {
                var n = m.Name.ToUpper();
                if (!n.StartsWith("F_")) continue;
                n = n.Substring(2);
                m_hash[n] = new item() {bSysOrApp=false, name = m.Name.Substring(2), mi = m };                                
            }
        }
        public static bool IsFunc(string name)
        {
            var i = (item)m_hash[name.ToUpper()];
            return (i!=null);
        }
        public static object Run(string name, object[] ol,StateBuffer sb)
        {
            var i = (item)m_hash[name.ToUpper()];
            if (i ==null) return null;

            return i.Exec(ol,sb);
        }
        public static string Help()
        {
            if (m_hash==null) Init();

            string NL = Environment.NewLine;
            string s = "== System Functions ==" + NL;
            foreach(var k in m_hash.Keys)
            {
                var i = (item)m_hash[k];
                if (i.bSysOrApp)
                {
                    s+= helpFormat(i) + NL; 
                }
            }
            s+= "== User Functions ==" + NL;
            foreach(var k in m_hash.Keys)
            {
                var i = (item)m_hash[k];
                if (!i.bSysOrApp)
                {
                    s+= helpFormat(i) + NL;
                }
            }

            return s;
        }
        private static string helpFormat(item i)
        {
            string NL = Environment.NewLine;
            string s = string.Format("{0,-20}", i.name) + " : " ;
            var help = i.Help();
            if (string.IsNullOrEmpty(help))
            {
                return s;
            }
            var lines= help.Split('\x0a');
            s += lines[0];
            if (lines.Length==1) return s;
            for(int n = 1; n < lines.Length; n++)
            {
                var t = lines[n].Trim();
                if (string.IsNullOrEmpty(t)) continue;
                s += NL + new string(' ',23) + t;
            }
            return s;
        }
    }
}
