using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace slgctl
{
    public class cmd_sub
    {
        public static slagtool.slag m_slag;

        public static slagtool.slag Load(string file)
        {
            if (file==null)
            {
                wk.SendWriteLine("ERROR:File name is null!");
                return null;
            }
            var ext = Path.GetExtension(file).ToUpper();
            if (ext!=".JS" && ext!=".BIN")
            {
                wk.SendWriteLine("ERROR:File name is not allowed");
                return null;
            }
            if (!File.Exists(file))
            {
                wk.SendWriteLine("ERROR:File does not exist!");
            }

            m_slag = null;

            try
            {
                m_slag = new slagtool.slag();
                m_slag.LoadFile(file);
                wk.SendWriteLine("Loaded.");
            }
            catch(SystemException e)
            {
                wk.SendWriteLine("-- EXCEPTION --");
                wk.SendWriteLine(e.Message);
                wk.SendWriteLine("---------------");
                return null;
            }

            return m_slag;
        }

        public static void Run(slagtool.slag slag = null)
        {
            if (slag!=null) m_slag = slag;

            try
            {
                UpdateClear();
                m_slag.Run();
            }
            catch (SystemException e)
            {
                wk.SendWriteLine("-- EXCEPTION --");
                wk.SendWriteLine(e.Message);
                wk.SendWriteLine("---------------");
            }
        }

        public static void Test()
        {
            wk.SendWriteLine(".. 1234.\n567.");
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
            if (m_slag==null) return;
            if (m_updateFunc==null) return;
            try { 
                foreach(var f in m_updateFunc)
                {
                    m_slag.CallFunc(f);
                }
            }
            catch(SystemException e)
            {
                wk.SendWriteLine("-- EXCEPTION --");
                wk.SendWriteLine(e.Message);
                wk.SendWriteLine("---------------");
            }
        }
        
    }
}
