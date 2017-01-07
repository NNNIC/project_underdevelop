﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using number = System.Double;
using slagtool.runtime;
using slagtool.runtime.builtin;


public class slagipc_unity_builtinfunc {
    static string NL = Environment.NewLine;

    public static object F_Println(bool bHelp,object[] ol,StateBuffer sb)
    {
        if (bHelp)
        {
            return "Print a string with break line." + NL + "ex)PrintLn(\"hoge!\");";
        }
        
        var s = "";
        if (ol!=null&&ol.Length>0)
        { 
            kit.check_num_of_args(ol,1);
            var o = kit.get_ol_at(ol,0);
            s = kit.convert_escape(o);
        }
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
        Func<List<object>,string> join_list = (l)=> {
            string t= null;
            foreach(var e in l)
            {
                if (t!=null) t+=",";
                t+= tostr(e);
            }
            return t;
        };
        Func<Array,string> join_array = (l)=> {
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
                return "(" + join_list(l) + ")";
            }
            if (a.GetType().IsArray)
            {
                var l = (Array)a;
                return "(" + join_array(l) +")";
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
        if (bHelp) return "";

        m_readtext = null;
        var label = kit.get_string_at(ol,0);
        guiDisplay.GetInput(label,"",(s)=>m_readtext=s);
        return null;
    }
    public static object F_ReadLineDone(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp) return "";

        return m_readtext;
    }

    //public static object F_HookUpdate(bool bHelp, object[] ol, StateBuffer sb)
    //{
    //    if (bHelp)
    //    {
    //        return "Hook up a function with Update on monobehaviour." + NL +"ex) HookUpdate(\"function\"";
    //    }
    //    var f = kit.get_string_at(ol,0);

    //    slagipc.cmd_sub.UpdateAddFunc(f);

    //    return null;
    //}

    public static object F_AddBehaviour(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "Add \"slagipc_unity_monobehaviour\" to the game object and return component." + NL +"ex) var bhv = AddBehaviour(go)";
        }
        GameObject go = null;
        if (ol.Length==0)
        {
            go =  slagipc_unity_main.V.gameObject;
        }
        else if (ol[0] is GameObject)
        {
            go = (GameObject)ol[0];
        }
        return go.AddComponent<slagipc_unity_monoehaviour>();
    }

    //public static object F_SetStartCall(bool bHelp, object[] ol, StateBuffer sb)
    //{
    //    if (bHelp)
    //    {
    //        return "Set Start Call to the specified GameObject." + NL + "ex) var a=new GameObject(); SetStartCall(a,\"StartCall\");  ";
    //    }

    //    kit.check_num_of_args(ol,2);
    //    var go = (GameObject)ol[0];
    //    var sm = go.GetComponent<slagipc_unity_monoehaviour>();
    //    if (sm==null) sm = go.AddComponent<slagipc_unity_monoehaviour>();
    //    sm.m_startFunc = kit.get_string_at(ol,1);
        
    //    return null;
    //}
    //public static object F_SetUpdateCall(bool bHelp, object[] ol, StateBuffer sb)
    //{
    //    if (bHelp)
    //    {
    //        return "Set Update Call to the specified GameObject." + NL + "ex) var a=new GameObject(); SetUpdateCall(a,\"UpdateCall\");  ";
    //    }

    //    kit.check_num_of_args(ol,2);
    //    var go = (GameObject)ol[0];
    //    var sm = go.GetComponent<slagipc_unity_monoehaviour>();
    //    if (sm==null) sm = go.AddComponent<slagipc_unity_monoehaviour>();
    //    sm.m_updateFunc = kit.get_string_at(ol,1);
        
    //    return null;
    //}
    //public static object F_SetOnDestroyCall(bool bHelp, object[] ol, StateBuffer sb)
    //{
    //    if (bHelp)
    //    {
    //        return "Set OnDestroy Call to the specified GameObject." + NL + "ex) var a=new GameObject(); SetOnDestroyCall(a,\"OnDestroyCall\");  ";
    //    }

    //    kit.check_num_of_args(ol,2);
    //    var go = (GameObject)ol[0];
    //    var sm = go.GetComponent<slagipc_unity_monoehaviour>();
    //    if (sm==null) sm = go.AddComponent<slagipc_unity_monoehaviour>();
    //    sm.m_onDestroyFunc = kit.get_string_at(ol,1);
        
    //    return null;
    //}
    #region ステートマシン
#if obs
    public static object F_StateInit(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "Initialize StateMachine and set the initial state." + NL + "format) StateInit(function name)";
        }
        var f = kit.get_string_at(ol,0);

        slagctl.cmd_sub.StateInit(f);

        return null;
    }
    public static object F_StateGoto(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "Set New State";
        }
        var f = kit.get_string_at(ol,0);

        slagctl.cmd_sub.StateGoto(f);

        return null;
    }
    public static object F_StateWait(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp) return "";

        var f = kit.get_number_at(ol,0);
        var c = f * 60.0f;
        slagctl.cmd_sub.StateWaitCnt((int)c);
        return null;
    }
#else
    public static object F_StateManager(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "Create new State Manager. ex) var sm = new StateManager(); or var sm=new StateManager(gameObject);" +NL+ 
                   "If you set a Game Object as a parameter, the state manager will be belonged to the Game Object" ;
        }

        slagipc_unity_statemanager sm = null;
        if (ol.Length == 0)
        {
            sm = slagipc_unity_main.V.gameObject.AddComponent<slagipc_unity_statemanager>();
        }
        else if (ol[0] is GameObject)
        {
            sm = ((GameObject)ol[0]).AddComponent<slagipc_unity_statemanager>();
        }
        else
        {
            util._error("StateManager has unknown parameter.");
        }

        sm.Init();

        return sm;
    }

#endif
    #endregion

    public static object F_SendMsg(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "GameObjectにメッセージを送る。メッセージ受信先が存在すれば指定関数を実行。"+NL+
                   "フォーマット) SendMsg(GameObject,名前[,パラメータ・・・])";
        }
        GameObject go   = null;
        if (ol.Length>0)
        {
            go   = (GameObject)ol[0];
        }
        List<object> plist=null;
        if (ol.Length>1)
        {
            plist = new List<object>();
            for(int i = 1; i<ol.Length; i++)
            {
                plist.Add(ol[i]);
            }
        }
        if (plist==null)
        {
            util._error("SendMsgの引数が正しくありません");
        }
        go.SendMessage("SendMessageSocket",plist);

        return null;
    }

    public static object F_GetObjectAtScreenPoint(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "スクリーンポジションの位置にあるオブジェクトを返す"+NL+
                   "フォーマット) GetObjectAtScreenPoint(Vector3[,Camera])" +NL+
                   "例)　var go = GetObjectAtScreenPoint(new Vector3(100,100,0));";
        }

        Vector3 pos=Vector3.zero;
        Camera cam = null;
        if (ol.Length>=1 && ol[0] is Vector3)
        {
            pos = (Vector3)ol[0];
            cam = Camera.main;
        }
        if (ol.Length>=2 && ol[1] is Camera)
        {
            cam = (Camera)ol[1];
        }
        if (cam==null)
        {
            util._error("GetObjectAtScreenPoint:引数が正しくありません");
        }
        
        var ray = cam.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            if (hit.collider!=null)
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }

}
