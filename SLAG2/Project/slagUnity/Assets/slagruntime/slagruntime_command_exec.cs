using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace slagruntime
{
    class command_exec
    {
        public static void Load(string file)
        {
            if (file==null)
            {
                util.SendWriteLine("ERROR:File name is null!");
                return;
            }
            var ext = Path.GetExtension(file).ToUpper();
            if (ext!=".JS" && ext!=".BIN")
            {
                util.SendWriteLine("ERROR:File name is not allowed");
                return;
            }
            if (!File.Exists(file))
            {
                util.SendWriteLine("ERROR:File does not exist!");
            }

            var raw = File.ReadAllText(file,Encoding.UTF8);

            try
            {
                slagtool.process.Run(raw,true);
            } catch (SystemException e)
            {
                util.SendWriteLine("-- EXCEPTION --");
                util.SendWriteLine(e.Message);
                util.SendWriteLine("---------------");
                return;
            }
            util.SendWriteLine(".. Read.");
        }
    }
}
