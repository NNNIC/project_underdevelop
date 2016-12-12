using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace slgctl
{
    class cmd
    {
        public enum COMMAND
        {
            NONE,
            WD,     //Set Working Directory
            LOAD,   //Load FILENAME (.js or .txt)
            LOADRUN,//Load and run FILENAME (.js or .txt)
            LOADBIN,//Store BASE64 binary
            RUN,    //Run
            STEP,   //Step in or out
            BP,     //Set breakpoint          
            PRINT,  //Print variable
            STOP,   //Stop next line          --- 実行中OK
            RESUME, //Resume
            TEST,   //Test
            DEBUG,  //Debug [0|1|2]
            HELP, 
            QUIT,   //Quit and Close          --- 実行中OK
            RESET,  //Same as Quit
        }

        public static string m_workDir = @"N:\Project\test";

        public static void execute(string cmdbuff)
        {
            string p1;
            COMMAND cmd = GetCmd(cmdbuff,out p1);
            switch(cmd)
            {
                case COMMAND.WD:      if (!string.IsNullOrEmpty(p1)) Set_WorkingDirectoy(p1);          break;
                case COMMAND.LOAD:    if (!string.IsNullOrEmpty(p1)) cmd_sub.Load(Path.Combine(m_workDir,p1));  break;
                case COMMAND.RUN:     cmd_sub.Run();                    break;
                case COMMAND.STEP:    break;
                case COMMAND.BP:      break;
                case COMMAND.PRINT:   break;
                case COMMAND.STOP:    break;
                case COMMAND.RESUME:  break;
                case COMMAND.TEST:    cmd_sub.Test();           break;
                case COMMAND.DEBUG:   cmd_sub.Debug(p1);        break;

                case COMMAND.RESET:
                case COMMAND.QUIT:    cmd_sub.Reset();          break;

                case COMMAND.HELP:    cmd_sub.Help();           break;
                default: wk.SendWriteLine("ignore:" + cmdbuff); break;
            }
        }

        public static void execute_in_running(string cmdbuff)
        {
            string p1;
            COMMAND cmd = GetCmd(cmdbuff,out p1);
            switch(cmd)
            {
                case COMMAND.STOP: break;
                case COMMAND.QUIT: break;
                default: wk.SendWriteLine("ignore:" + cmd.ToString()); break;
            }
        }

        // --- tool for this class
        private static COMMAND GetCmd(string cmdbuff,out string p1)
        {
            var token = cmdbuff.Split(' ');
            string p0 = token[0].ToUpper();
            p1        = token.Length>1 ? token[1] : null;

            if (!Enum.IsDefined(typeof(COMMAND),p0))
            { 
                wk.SendWriteLine("Unknow command:" + cmdbuff);
                return COMMAND.NONE;
            }
            var cmd = (COMMAND)Enum.Parse(typeof(COMMAND),p0);
            return cmd;
        }
        private static void Set_WorkingDirectoy(string dir)
        {
            if (dir!=null)
            {
                m_workDir = dir;
            }
            wk.SendWriteLine("Current Working Directory : " + m_workDir);
        }
    }
}
