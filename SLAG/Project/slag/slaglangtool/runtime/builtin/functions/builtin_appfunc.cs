using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace slagtool.runtime.builtin
{
    public class builtin_appfunc
    {
        //static string NL = kit.NL;

        ////ハンドルを取得する。
        //[DllImport( "kernel32.dll" )]
        //static extern IntPtr GetStdHandle( int inputHandle );

        ////現在のコンソールモードを取得する。
        //[DllImport( "kernel32.dll" )]
        //static extern bool GetConsoleMode(
        //    IntPtr consoleHandle,
        //    ref int mode );

        ////コンソールモードを設定する。
        //[DllImport( "kernel32.dll" )]
        //static extern bool SetConsoleMode(
        //    IntPtr consoleHandle,
        //    int mode );

        //public static object F_SetVT(bool bHelp, object[] ol,StateBuffer sb) //set vt100 mode
        //{
        //    //https://msdn.microsoft.com/en-us/library/ms686033(v=vs.85).aspx

        //    if (bHelp)
        //    {
        //        return "Set console to vt100 mode.";
        //    }

        //    const int STD_INPUT_HANDLE  = -10;
        //    const int STD_OUTPUT_HANDLE = -11;

        //    var ho = GetStdHandle(STD_OUTPUT_HANDLE);
        //    var hi = GetStdHandle(STD_INPUT_HANDLE);
        //    int save_o_mode=0;
        //    int save_i_mode=0;
        //    GetConsoleMode(ho,ref save_o_mode);
        //    GetConsoleMode(hi,ref save_i_mode);

        //    Console.WriteLine("save output mode = {0}",save_o_mode.ToString("X") );
        //    Console.WriteLine("save input mode  = {0}",save_i_mode.ToString("X") );

        //    int new_o_mode = save_o_mode | 4;
        //    int new_i_mode = save_i_mode | 4;

        //    SetConsoleMode(ho, new_o_mode);
        //    SetConsoleMode(hi, new_i_mode);

        //    GetConsoleMode(ho,ref save_o_mode);
        //    GetConsoleMode(hi,ref save_i_mode);

        //    Console.WriteLine("current output mode = {0}",save_o_mode.ToString("X") );
        //    Console.WriteLine("current input mode  = {0}",save_i_mode.ToString("X") );

        //    return null;
        //}

        //public static object F_PrintVT(bool bHelp, object[] ol,StateBuffer sb)
        //{
        //    //https://msdn.microsoft.com/en-us/library/mt638032(v=vs.85).aspx

        //    if (bHelp)
        //    {
        //        return "Print string. 'ESC' will be converted to escape character.";
        //    }

        //    var s = kit.get_ol_at(ol,0);

        //    var ns = s.ToString().Replace("ESC","\x1b");

        //    Console.WriteLine(ns);
        //    return null;
        //}

        //public static object F_Cursor(bool bHelp, object[] ol, StateBuffer sb)
        //{
        //    if (bHelp)
        //    {
        //        return "Set CURSOR at (X,Y) postion on VT100 display." + NL +"Format:Cursor(x,y)";
        //    }

        //    var x = (int)kit.get_double_at(ol,0);
        //    var y = (int)kit.get_double_at(ol,1);

        //    var t = string.Format("\x1b[{0};{1}H",y.ToString(),x.ToString());

        //    Console.WriteLine(t);

        //    return null;
        //}

        //public static object F_Put(bool bHelp, object[] ol, StateBuffer sb)
        //{
        //    if (bHelp)
        //    {
        //        return "Pug a string at (X,Y) position on VT100 display." + NL +"Format:Put(x,y,string)";
        //    }
        //    var x = (int)kit.get_double_at(ol,0);
        //    var y = (int)kit.get_double_at(ol,1);
        //    var s = kit.get_ol_at(ol,2).ToString();

        //    var t = string.Format("\x1b[{0};{1}H{2}",y.ToString(),x.ToString(),s);

        //    Console.WriteLine(t);

        //    return null;
        //}
    }
}
