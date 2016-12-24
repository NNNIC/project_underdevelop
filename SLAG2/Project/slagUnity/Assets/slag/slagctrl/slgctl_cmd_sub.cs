using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace slgctl
{
    public class cmd_sub
    {
        public static slagtool.slag m_slag;

        public static slagtool.slag Load(string path, string file)
        {
            string fullpath = null;
            try
            {
                fullpath = Path.Combine(path,file);
            }
            catch
            {
                wk.SendWriteLine("ERROR:Unexpcted path name");
                return null;
            }

            if (fullpath==null)
            {
                wk.SendWriteLine("ERROR:File name is null!");
                return null;
            }
            var ext = Path.GetExtension(fullpath).ToUpper();
            if (ext!=".JS" && ext!=".BIN")
            {
                wk.SendWriteLine("ERROR:File name is not allowed");
                return null;
            }
            if (!File.Exists(fullpath))
            {
                wk.SendWriteLine("ERROR:File does not exist!");
            }

            m_slag = null;

            if (slagtool.sys.USETRY)
            {
                try
                {
                    m_slag = new slagtool.slag();
                    m_slag.LoadFile(fullpath);
                }
                catch(SystemException e)
                {
                    wk.SendWriteLine("-- EXCEPTION --");
                    wk.SendWriteLine(e.Message);
                    wk.SendWriteLine("---------------");
                    return null;
                }
            }
            else
            {
                m_slag = new slagtool.slag();
                m_slag.LoadFile(fullpath);
            }
            wk.SendWriteLine("Loaded.");

//#if USETRY
//            try
//#endif
//            {
//                m_slag = new slagtool.slag();
//                m_slag.LoadFile(file);
//                wk.SendWriteLine("Loaded.");
//            }
//#if USETRY
//            catch(SystemException e)
//            {
//                wk.SendWriteLine("-- EXCEPTION --");
//                wk.SendWriteLine(e.Message);
//                wk.SendWriteLine("---------------");
//                return null;
//            }
//#endif
//#else
//            m_slag = new slagtool.slag();
//                m_slag.LoadFile(file);
//                wk.SendWriteLine("Loaded.");
//#endif

            return m_slag;
        }

        public static void Run(slagtool.slag slag = null)
        {
            if (slag!=null) m_slag = slag;

            UpdateClear();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            if (slagtool.sys.USETRY)
            {
                try { 
                    m_slag.Run();
                } 
                catch(SystemException e)
                {
                    wk.SendWriteLine("-- EXCEPTION --");
                    wk.SendWriteLine(e.Message);
                    wk.SendWriteLine("Stop at Line:" + slagtool.YDEF_DEBUG.current_v.get_dbg_line(true).ToString() );
                    wk.SendWriteLine("---------------");
                }
            }
            else
            {
                m_slag.Run();
            }
            sw.Stop();
            wk.SendWriteLine("! The program exection time : " + ((float)sw.ElapsedMilliseconds / 1000f).ToString("F3") + "sec !");

//#if USETRY
//            try
//#endif
//            {
//                UpdateClear();
//                var sw = new System.Diagnostics.Stopwatch();
//                sw.Start();
//                m_slag.Run();
//                sw.Stop();
//                wk.SendWriteLine("! The program used " + ((float)sw.ElapsedMilliseconds / 1000f).ToString("F3") + "sec !");
//            }
//#if USETRY
//            catch (SystemException e)
//            {
//                wk.SendWriteLine("-- EXCEPTION --");
//                wk.SendWriteLine(e.Message);
//                wk.SendWriteLine("---------------");
//            }
//#endif
        }

        public static void Reset()
        {
            m_updateFunc = null;
            UnityEngine.GameObject.Find("slgctl_main").SendMessage("Reset");
            //UnityEngine.SceneManagement.SceneManager.LoadScene("reset");// Application.loadedLevelName;
        }

        public static void Test()
        {
            wk.SendWriteLine(".. 1234.\n567.");
        }

        public static void Debug(string p)
        {
            int x = -1;
            if (!string.IsNullOrEmpty(p) && int.TryParse(p,out x) && x>=0 && x<=2)
            {
                wk.SendWriteLine("Set Debug Mode : " + x);
                slagtool.util.SetDebugMode(x);
            }
            else
            {
                wk.SendWriteLine("Current Debug Mode : " + slagtool.util.GetDebugMode());
            }
        }

        public static void Help()
        {
            var s = slagtool.runtime.builtin.builtin_func.Help();
            wk.SendWriteLine(s);
        }

        //-- Update用
        private static List<string> m_updateFunc;
        public static void UpdateClear()
        {
            m_updateFunc = null;
        }
        public static void UpdateAddFunc(string func)
        {
            if (m_updateFunc==null) m_updateFunc = new List<string>();
            m_updateFunc.Add(func);
        }
        public static void UpdateExec()
        {
            if (slagtool.sys.USETRY)
            {
                try
                {
                    _updateExec();
                }
                catch (SystemException e)
                {
                    wk.SendWriteLine("-- EXCEPTION --");
                    wk.SendWriteLine(e.Message);
                    wk.SendWriteLine("Stop at Line:" + slagtool.YDEF_DEBUG.current_v.get_dbg_line(true).ToString() );
                    wk.SendWriteLine("---------------");
                    m_sm = null;
                    m_updateFunc = null;
                }
            }
            else
            {
                _updateExec();
            }

            //try { 
            //    if (m_sm!=null) m_sm.Update();

            //    if (m_slag==null) return;
            //    if (m_updateFunc==null) return;

            //    var s = new System.Diagnostics.Stopwatch();
            //    s.Start();
            //    foreach (var f in m_updateFunc)
            //    {
            //        m_slag.CallFunc(f);
            //    }
            //    s.Stop();
            //}
            //catch(SystemException e)
            //{
            //    wk.SendWriteLine("-- EXCEPTION --");
            //    wk.SendWriteLine(e.Message);
            //    wk.SendWriteLine("---------------");
            //    m_sm = null;
            //    m_updateFunc = null;
            //}
        }
        private static void _updateExec()
        {
            if (m_sm!=null) m_sm.Update();

            if (m_slag==null) return;
            if (m_updateFunc==null) return;

            var s = new System.Diagnostics.Stopwatch();
            s.Start();
            foreach (var f in m_updateFunc)
            {
                m_slag.CallFunc(f);
            }
            s.Stop();
        }

        //-- StateMachine用
        public class StateMachine
        {
            string m_cur;
            string m_next;

            int    m_waitcnt;

            public void Goto(string func) { m_next     = func;  }
            public void Wait(int c)       { m_waitcnt  = c;}

            public void Update()
            {
                if (m_waitcnt>0)
                {
                    m_waitcnt--;
                    return;
                }
                bool bFirst = false;
                if (m_next!=null)
                {
                    m_cur  = m_next;
                    m_next = null;
                    bFirst = true;
                }
                if (m_slag!=null &&  m_cur!=null) m_slag.CallFunc(m_cur,new object[1] { bFirst });
            }
        }

        public static StateMachine m_sm;
        public static void StateInit(string func)
        {
            m_sm = new StateMachine();
            m_sm.Goto(func);
        }
        public static void StateGoto(string func)
        {
            m_sm.Goto(func);
        }
        public static void StateWaitCnt(int c)
        {
            m_sm.Wait(c);
        }
    }
}
