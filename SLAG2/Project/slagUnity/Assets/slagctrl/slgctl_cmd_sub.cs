using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace slgctl
{
    class cmd_sub
    {
        public static void Load(string file, bool bRunNow)
        {
            if (file==null)
            {
                wk.SendWriteLine("ERROR:File name is null!");
                return;
            }
            var ext = Path.GetExtension(file).ToUpper();
            if (ext!=".JS" && ext!=".BIN")
            {
                wk.SendWriteLine("ERROR:File name is not allowed");
                return;
            }
            if (!File.Exists(file))
            {
                wk.SendWriteLine("ERROR:File does not exist!");
            }

            string raw = null;
            try { 
                raw = File.ReadAllText(file,Encoding.UTF8);
            }
            catch(System.Exception e)
            {
                wk.SendWriteLine("-- EXCEPTION --");
                wk.SendWriteLine(e.Message);
                wk.SendWriteLine("---------------");
                return;
            }

            try
            {
                if (bRunNow)
                { 
                    slagtool.util.ExeSrc(raw);
                    wk.SendWriteLine(".. Done to read and run " + file);
                }
                else
                {
                    slagtool.util.LoadSrc(raw);
                    wk.SendWriteLine(".. Done to read " + file);
                }
            } catch (SystemException e)
            {
                wk.SendWriteLine("-- EXCEPTION --");
                wk.SendWriteLine(e.Message);
                wk.SendWriteLine("---------------");
                return;
            }
        }

        public static void LoadBin(string base64str)
        {
            try
            {
                slagtool.util.LoadBase64(base64str);
            } catch (SystemException e)
            {
                wk.SendWriteLine("-- EXCEPTION --");
                wk.SendWriteLine(e.Message);
                wk.SendWriteLine("---------------");
            }
        }

        public static void Run()
        {
            try
            {
                slagtool.util.Run();
            } catch (SystemException e)
            {
                wk.SendWriteLine("-- EXCEPTION --");
                wk.SendWriteLine(e.Message);
                wk.SendWriteLine("---------------");
            }
        }

        public static void Test()
        {
            wk.SendWriteLine(".. 1234.\n567."  );
        }
    }
}
