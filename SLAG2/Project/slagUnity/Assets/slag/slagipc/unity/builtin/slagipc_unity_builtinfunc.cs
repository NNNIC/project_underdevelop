using UnityEngine;
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

    public static object F_AddBehaviour(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "slagipc_unity_monobehaviourコンポネントを追加" + NL +
                   "フォーマット) var bhv = AddBehaviour([GameObject]);"+NL +
                   "　　　　　　　GameObject指定がない場合、slag実行メインのGameObjectに追加";
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

    #region ステートマシン
    public static object F_StateManager(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "ステート管理作成" +NL+ 
                   "フォーマット) var sm = StateManager([GameObject]);" + NL +
                   "slagipc_unity_statemanagerクラスに詳細あり";
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
            util._error("StateManager関数のパラメータが不正です");
        }

        sm.Init();

        return sm;
    }

    #endregion

    public static object F_SendMsg(bool bHelp, object[] ol, StateBuffer sb)
    {
        if (bHelp)
        {
            return "GameObjectにメッセージを送る。メッセージ受信先が存在すれば指定関数を実行。"+NL+
                   "フォーマット) SendMsg(GameObject,名前[,パラメータ・・・])" + NL +
                   "slagipc_unity_monoehaviourクラス内に詳細あり ";
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
