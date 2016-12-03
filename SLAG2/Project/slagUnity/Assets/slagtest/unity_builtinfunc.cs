﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if NUMBERISFLOAT
using number = System.Single;
#else
using number = System.Double;
#endif

using slagtool.runtime;
using slagtool.runtime.builtin;


public class unity_builtinfunc {
    static string NL = Environment.NewLine;

    public static object F_Println(bool bHelp,object[] ol,StateBuffer sb)
    {
        if (bHelp)
        {
            return "Print a string with break line." + NL + "ex)PrintLn(\"hoge!\");";
        }
        kit.check_num_of_args(ol,1);
        var o = kit.get_ol_at(ol,0);
        var s = kit.convert_escape(o);

        Debug.Log(s);
        guiDisplay.WriteLine(s);

        return null;
    }
    public static object F_Print(bool bHelp,object[] ol,StateBuffer sb)
    {
        if (bHelp)
        {
            return "Print a string." + NL + "ex)Print(\"hoge!\");";
        }
        kit.check_num_of_args(ol,1);
        var o = kit.get_ol_at(ol,0);
        var s = kit.convert_escape(o);

        Debug.Log(s);
        guiDisplay.Write(s);

        return null;
    }
    public static object F_Dump(bool bHelp,object[] ol,StateBuffer sb)
    {
        if (bHelp)
        {
            return "Dump a variable." + NL +"ex)Dump(x);";
        }

        if (ol==null) return "-null-";

        Func<object,string> tostr = null;
        Func<List<object>,string> join = (l)=> {
            string t= null;
            foreach(var e in l)
            {
                if (t!=null) t+=",";
                t+= tostr(e);
            }
            return t;
        };

        tostr = (a) => {
            if (a==null) return "-null-";
            if (a.GetType()==typeof(List<object>))
            {
                var l = (List<object>)a;
                return "(" + join(l) + ")";
            }
            return a.ToString();
        };

        string s = null;
        foreach(var o in ol)
        {
            if (s!=null) s+=",";
            s += tostr(o);
        }

        UnityEngine.Debug.Log(s);
        guiDisplay.WriteLine(s);

        return s;
    }
    static string m_readtext;
    public static object F_ReadLineStart(bool bHelp, object[] ol, StateBuffer sb)
    {
        m_readtext = null;
        var label = kit.get_string_at(ol,0);
        guiDisplay.GetInput(label,"",(s)=>m_readtext=s);
        return null;
    }
    public static object F_ReadLineDone(bool bHelp, object[] ol, StateBuffer sb)
    {
        return m_readtext;
    }

    public static object F_HookUpdate(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "Hook up a function with Update on monobehaviour." + NL +"ex) HookUpdate(\"function\"";
        }
        var f = kit.get_string_at(ol,0);

        slgctl.cmd_sub.UpdateAddFunc(f);

        return null;
    }

    #region ステートマシン
    public static object F_StateInit(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "Initialize StateMachine and set the initial state." + NL + "format) StateInit(function name)";
        }
        var f = kit.get_string_at(ol,0);

        slgctl.cmd_sub.StateInit(f);

        return null;
    }
    public static object F_StateGoto(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "Set New State";
        }
        var f = kit.get_string_at(ol,0);

        slgctl.cmd_sub.StateGoto(f);

        return null;
    }
    public static object F_StateWait(bool bHelp, object[] ol, StateBuffer sb)
    {
        var f = kit.get_number_at(ol,0);
        var c = f * 60.0f;
        slgctl.cmd_sub.StateWaitCnt((int)c);
        return null;
    }
    #endregion
}
