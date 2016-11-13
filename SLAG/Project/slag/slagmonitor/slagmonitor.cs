#define nounity
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace slag
{
    public class monitor
    {
        #region interactive mode
        public enum CMD
        {
            HELP,
            LOAD,
            RUN,
            STEP,
            BP,
            DEBUG,

            LIST,
            ST,
            P,

            WD,

            TRY,
            DOC,

            QUIT,
        }

        static string   m_file;
        static string[] m_alltext;
        static string   m_raw;
        static string   m_workdir = @"N:\Project\slag\slag\Test";

        static Action<string> m_write = null;
        static Action<string> m_writeLine = null;
        static Func<string>   m_readLine = null;

        static void conWrite(string s)          { if (m_write!=null) m_write(s);                 }
        static void conWriteLine(string s=null) { if (m_writeLine!=null) m_writeLine(s);         }
        static string conReadLine()             { return m_readLine!=null ? m_readLine() : null; }

        public static void interactive(Action<string> write=null,Action<string> writeLine=null, Func<string> readline = null)
        {
            m_write     = write;
            m_writeLine = writeLine;
            m_readLine  = readline;

            string NL = Environment.NewLine;
            string help =
                "== COMMAND =="                                 + NL +
                "HELP          : Show this help."               + NL +
                "LOAD source   : Load source file (*.js)."      + NL +
                "RUN           : Execute."                      + NL +
                "STEP [o]      : Step over"                     + NL +
                "STEP i        : Step in"                       + NL +
                "BP line-num   : Set a break point at the line" + NL +
                "BP clear      : Clear all break pointers"      + NL +
                "BP            : List all break pointers"       + NL +
                "DEBUG {on|off}: Set Debug On or Off    "       + NL +
                "DEGUG {0|1|2} : Set Debug Level        "       + NL +
                "DEGUG         : Show Debug Status      "       + NL +
                "LIST          : List Source"                   + NL +
                "ST            : Show the status buffer"        + NL +
                "P variable    : Print the variable"            + NL +
                "WD            : Set Working Directory"         + NL +
                "TRY cmd       : Try command        "           + NL +
                "! cmd         : Try command"                   + NL +
                "DOC           : Document of all functions"     + NL +
                "QUIT          : Quit"                          + NL +
                ""                                              + NL +
                "";

            conWriteLine();

            slagtool.sys.DEBUGMODE = true;
            slagtool.process.SetLogFunc(write,writeLine);

            while(true)
            {
                conWrite(">");
                string rl = null;
                while(string.IsNullOrEmpty(rl))
                { 
                    rl = conReadLine();
                }
                if (rl==Environment.NewLine) continue;

                rl = rl.Replace("!","TRY ");
                var s = rl.Trim().ToUpper();
                var l = s.Split(' ');
                if (Array.IndexOf(Enum.GetNames(typeof(CMD)),l[0]) >= 0)
                {
                    var cmd = (CMD)Enum.Parse(typeof(CMD),l[0]);
                    switch(cmd)
                    {
                        case CMD.HELP: conWriteLine(help); break;
                        case CMD.QUIT:             return;
                        case CMD.LOAD: _load(l);   break;
                        case CMD.RUN:  _run(l);    break;
                        case CMD.STEP: _step(l);   break;
                        case CMD.BP:   _bp(l);     break;
                        case CMD.DEBUG:_debug(l);  break;
                        case CMD.LIST: _list(l);   break;
                        case CMD.ST:   _status(l); break;
                        case CMD.P:    _p(l);      break;
                        case CMD.WD:   _wd(l);     break;
                        case CMD.TRY:  _try(rl);   break;
                        case CMD.DOC:  _doc(rl);   break;
                        default: conWriteLine("The command is uknown."); break;
                    }
                }
            }
        }
        private static void _load(string[] args)
        {
            CancelTask();



            if (args.Length <2)
            {
                conWriteLine("Set file name !");
                return;
            }
            var file = args[1];

            if (string.IsNullOrEmpty(Path.GetExtension(file)))
            {
                file  += ".JS";
            }

            if (!File.Exists(file))
            {
                file = Path.Combine(m_workdir,file);
                if (!File.Exists(file))
                { 
                    conWriteLine("File not found");
                    return;
                }
            }
            if (Path.GetExtension(file)==".TXT" || Path.GetExtension(file)==".JS")
            {
                m_file = file;
                m_alltext = File.ReadAllLines(m_file,Encoding.UTF8);
                m_raw     = File.ReadAllText(m_file,Encoding.UTF8);

                conWriteLine("Loaded.");

                conWriteLine("Start Compiling");
                try { 
                    slagtool.process.Run(m_raw,true);
                } catch (SystemException e)
                {
                    conWriteLine("-- EXCEPTION --");
                    conWriteLine(e.Message);
                    conWriteLine("---------------");
                }
                conWriteLine("Done.");

                return;
            }
            
            conWriteLine("No Action");  
        }
        private static void _run(string[] args)
        {
            __run(slagtool.YDEF_DEBUG.STEPMODE.None);
        }
        private static void __run(slagtool.YDEF_DEBUG.STEPMODE step)
        {
            if (m_raw!=null)
            {
                conWriteLine("\n");
                slagtool.YDEF_DEBUG.stepMode = step;
                if (m_running)
                {
                    slagtool.YDEF_DEBUG.bForceToStop = false;
                }
                else
                { 
                    conWriteLine("==Run==");
#if nounity
                    RunAsync();
#endif
                }
                while(!slagtool.YDEF_DEBUG.bForceToStop && m_running) { Thread.Sleep(100); }
                if (!m_running)
                {
                    conWriteLine("\n==Done==");
                }
                else if (slagtool.YDEF_DEBUG.bForceToStop)
                {
                    conWriteLine(string.Format("Stop at line:{0}." , (slagtool.YDEF_DEBUG.stoppedLine+1).ToString("0000")));
                    __list(slagtool.YDEF_DEBUG.stoppedLine,5);
                }
                else
                {
                    conWriteLine("No Action");
                }
                conWriteLine("\n");
            }
            else
            {
                conWriteLine("No Action");
            }
        }
        private static void _step(string[] args)
        {
            slagtool.YDEF_DEBUG.STEPMODE mode = slagtool.YDEF_DEBUG.STEPMODE.StepOver;
            if (args.Length==1 || args[1]=="O")
            { 
                mode = slagtool.YDEF_DEBUG.STEPMODE.StepOver;
            }
            else if (args[1]=="I")
            {
                mode = slagtool.YDEF_DEBUG.STEPMODE.StepIn;
            }
            __run(mode);
        }
        private static void _bp(string[] args)
        {
            if (args.Length==1)
            {
                if (slagtool.YDEF_DEBUG.breakpoints!=null && slagtool.YDEF_DEBUG.breakpoints.Count > 0)
                {
                    for(int i = 0; i<slagtool.YDEF_DEBUG.breakpoints.Count; i++)
                    {
                        try { 
                        var l = slagtool.YDEF_DEBUG.breakpoints[i];
                            conWriteLine(string.Format("{0}:{1}:{2}",(i+1).ToString("00"),(l+1).ToString("0000"),m_alltext[l].TrimEnd()));
                        } catch { }
                    }
                }
                else
                {
                    conWriteLine("no break points exist.");
                }
                return;
            }
            else
            {
                var s = args[1].Trim();
                if (s == "CLEAR")
                {
                    slagtool.YDEF_DEBUG.breakpoints = null;
                    conWriteLine("Clear all breakpoints");
                    return;
                }
                int x = 0;
                if (int.TryParse(s,out x))
                {
                    if (m_alltext==null)
                    {
                        conWriteLine("There is no script.");
                        return;
                    }
                    var l = x - 1;
                    if (l >= 0 && l < m_alltext.Length)
                    {
                        if (slagtool.YDEF_DEBUG.breakpoints == null) slagtool.YDEF_DEBUG.breakpoints = new List<int>();
                        slagtool.YDEF_DEBUG.breakpoints.Add(l);
                        conWriteLine("Set A Breakpoint at " + x );
                        return;
                    }
                }
                conWriteLine("No Action");
            }
        }
        private static void _debug(string[] args)
        {
            if (args.Length==1)
            {
                bool onoff = slagtool.sys.DEBUGMODE;
                int  level = slagtool.sys.DEBUGLEVEL;

                conWriteLine(string.Format("DEBUG: {0}" , (onoff ? "ON" : "OFF")));
                if (onoff)
                {
                    conWriteLine("DEBUG LEVEL > " + level);
                }
                return;
            }
            if (args.Length>1)
            {
                var s = args[1];
                switch(s)
                {
                    case "ON":   slagtool.sys.DEBUGMODE = true;   conWriteLine("Change debug status."); return;
                    case "OFF":  slagtool.sys.DEBUGMODE = false;  conWriteLine("Change debug status."); return;
                    case "0":    slagtool.sys.DEBUGMODE = false;  conWriteLine("Change debug status."); return;
                    case "1":    slagtool.sys.DEBUGLEVEL = 1;     conWriteLine("Change debug status."); return;
                    case "2":    slagtool.sys.DEBUGLEVEL = 2;     conWriteLine("Change debug status."); return;
                }
            }
            conWriteLine("No Action");
        }
        private static void _list(string[] args)
        {
            if (m_alltext==null || m_alltext.Length==0)
            {
                conWriteLine("There is no source.");
                return;
            }

            conWriteLine(string.Format("== {0} ==", m_file));

            if (args.Length==1)
            { 
                __list();
            }
            else
            {
                if (args.Length>1)
                {
                    int l;
                    if (int.TryParse(args[1],out l))
                    {
                        __list(l,5);
                    }
                }
            }
        }
        private static void __list(int? line=null, int n = 0)
        {
            int start = 0;
            int end   = m_alltext.Length - 1;

            Func<int,int,int,int> clamp = (v,min,max)=> {
                if (v > max) return max;
                if (v < min) return min;
                return v;
            };

            if (line != null)
            {
                var l = (int)line;
                start = clamp(l-n,0,m_alltext.Length-1);
                end   = clamp(l+n,0,m_alltext.Length-1);
            }
            
            for(int i = start;i<=end; i++)
            {
                string cp1 = " ";
                string cp2 = "";
                if (slagtool.YDEF_DEBUG.stoppedLine == i)
                { 
                    cp1= ">";
                    cp2= "<-------------------- Current Point";
                }
                string bp = (slagtool.YDEF_DEBUG.breakpoints!=null && slagtool.YDEF_DEBUG.breakpoints.Contains(i)) ? "BP" : "  ";

                var s = m_alltext[i].TrimEnd();
                conWriteLine(string.Format("{0}:{1}:{2}:{3}{4}",cp1,bp,(i+1).ToString("0000"), s,cp2));
            }
        }
        private static void _status(string[] args)
        {
            int depth = 0;
            Action<Hashtable> wk = null;
            wk = (h) => {
                if (h==null) return;
                int d = depth ++;
                conWriteLine(string.Format(">> depth :{0} <<", d));
                foreach(var k in h.Keys)
                {
                    var ks = k.ToString();
                    if (ks.StartsWith("!")) continue;
                    var o = h[k];
                    string val = __dump(o);
                    
                    conWriteLine(string.Format("{0}:={1}",ks,val));
                }
                if (h.ContainsKey(slagtool.runtime.StateBuffer.KEY_PARENT))
                {
                    var ph = (Hashtable)h[slagtool.runtime.StateBuffer.KEY_PARENT];
                    wk(ph);
                }
                //conWriteLine("^^^^ DEPTH:{0} ^^^^", d);
            };

            if (slagtool.YDEF_DEBUG.current_sb!=null)
            { 
                wk(slagtool.YDEF_DEBUG.current_sb.m_front_dic);
            }
            else
            {
                conWriteLine("No Action");
            }
        }
        private static void _p(string[] args)
        {
            if (args.Length > 1)
            {
                var name = args[1];
                object o = null;
                try { o = slagtool.YDEF_DEBUG.current_sb.get(name); } catch(SystemException e) { o = e.Message; }
                if (o==null)
                {
                    conWriteLine("-null-");
                }
                else
                {
                    conWriteLine( __dump(o));
                }
            }
            else
            {
                conWriteLine("No Action");
            }
        }
        private static string __dump(object a)
        {
            string s= null;
            if (a==null)
            {
                return "-null-";
            }
            var t = a.GetType();
            if (t==typeof(List<object>))
            {
                if (s!=null) s+=",";
                s += "(";
                string f=null;
                foreach(var i in (List<object>)a )
                {
                    if (f!=null) f+=",";
                    f += __dump(i);
                }
                s += f + ")";
            }
            else
            {
                s += a.ToString();
            }
            return s;
        }
        private static void _wd(string[] args)
        {
            if (args.Length>=2)
            {
                if (Directory.Exists(args[1]))
                {
                    m_workdir = args[1];
                }
            }
            conWriteLine("Working Direcory : " + m_workdir);
        }
        private static void _try(string s)
        {
            try { 
                var raw = s.Substring(3).Trim();
                slagtool.process.Run(raw);
            }
            catch(SystemException e)
            {
                conWriteLine(e.Message);
            }
        }
        private static void _doc(string s)
        {
            var doc = slagtool.runtime.builtin.builtin_func.Help();
            conWriteLine(doc);
        }
#endregion

#region 非同期実行  //http://qiita.com/haminiku/items/cc299c1ed94d7ba3f9ec
        public static bool m_running = false;
#if nounity
        private static Thread m_thread;
        public static void RunAsync()
        {
            conWriteLine("!! runasync");
            m_thread = new Thread(()=> {
                try { 
                    slagtool.process.Run_from_savefile();
                } 
                catch (SystemException e)
                {
                    conWriteLine("-- EXCEPTION --");
                    conWriteLine(e.Message);
                    conWriteLine("---------------");
                }
            });
            m_thread.Start();
            m_running = false;
        }
        

        //public static async void RunAsync()
        //{
        //    slagtool.YDEF_DEBUG.bForceToStop = false;
        //    m_running = true;

        //    await Task.Run( ()=> {
        //        try { 
        //            slagtool.process.Run_from_savefile();
        //        } 
        //        catch (SystemException e)
        //        {
        //            conWriteLine("-- EXCEPTION --");
        //            conWriteLine(e.Message);
        //            conWriteLine("---------------");
        //        }
        //    });

        //    m_running = false;
        //}
#endif
        public static void CancelTask()
        {
            if (m_running)
            { 
                slagtool.YDEF_DEBUG.bRequestAbort = true;
            }
            while(m_running) Thread.Sleep(100);

            slagtool.YDEF_DEBUG.bForceToStop = false;
            slagtool.YDEF_DEBUG.stoppedLine = -1;
        }
#endregion
    }
}
