using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace slagruntime
{
    class command
    {
        public enum CMD
        {
            NONE,
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
            QUIT    //Quit and Close          --- 実行中OK
        }

        public static void execute(string cmdbuff)
        {
            string p1;
            CMD cmd = GetCmd(cmdbuff,out p1);
            switch(cmd)
            {
                case CMD.LOAD:    command_exec.Load(p1,false); break;
                case CMD.LOADRUN: command_exec.Load(p1,true); break;
                case CMD.LOADBIN: break;
                case CMD.RUN:     break;
                case CMD.STEP:    break;
                case CMD.BP:      break;
                case CMD.PRINT:   break;
                case CMD.STOP:    break;
                case CMD.RESUME:  break;
                case CMD.TEST:    command_exec.Test(); break;
                case CMD.QUIT:    break;
                default: util.SendWriteLine("ignore:" + cmdbuff); break;
            }
        }

        public static void execute_in_running(string cmdbuff)
        {
            string p1;
            CMD cmd = GetCmd(cmdbuff,out p1);
            switch(cmd)
            {
                case CMD.STOP: break;
                case CMD.QUIT: break;
                default: util.SendWriteLine("ignore:" + cmd.ToString()); break;
            }
        }

        // --- tool for this class
        private static CMD GetCmd(string cmdbuff,out string p1)
        {
            var token = cmdbuff.Split(' ');
            string p0 = token[0].ToUpper();
            p1        = token.Length>1 ? token[1] : null;

            if (!Enum.IsDefined(typeof(CMD),p0))
            { 
                util.SendWriteLine("Unknow command:" + cmdbuff);
                return CMD.NONE;
            }
            var cmd = (CMD)Enum.Parse(typeof(CMD),p0);
            return cmd;
        }
    }
}
