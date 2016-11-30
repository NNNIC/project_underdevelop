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
            }
            catch(SystemException e)
            {
                wk.SendWriteLine("-- EXCEPTION --");
                wk.SendWriteLine(e.Message);
                wk.SendWriteLine("---------------");
                return null;
            }

            return m_slag;

            //string raw = null;
            //try { 
            //    raw = File.ReadAllText(file,Encoding.UTF8);
            //}
            //catch(System.Exception e)
            //{
            //    wk.SendWriteLine("-- EXCEPTION --");
            //    wk.SendWriteLine(e.Message);
            //    wk.SendWriteLine("---------------");
            //    return;
            //}

            //try
            //{
            //    //if (bRunNow)
            //    //{ 
            //    //    slagtool.util.ExeSrc(raw);
            //    //    wk.SendWriteLine(".. Done to read and run " + file);
            //    //}
            //    slagtool.util.LoadSrc(raw);
            //    wk.SendWriteLine(".. Done to read " + file);
            //} catch (SystemException e)
            //{
            //    wk.SendWriteLine("-- EXCEPTION --");
            //    wk.SendWriteLine(e.Message);
            //    wk.SendWriteLine("---------------");
            //    return;
            //}
        }

        public static void Run(slagtool.slag slag = null)
        {
            if (slag!=null) m_slag = slag;

            try
            {
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
    }
}
