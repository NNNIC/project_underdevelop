using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using slagtool;
using slagtool.runtime;
using slagtool.runtime.builtin;

public class slagipc_unity_monoehaviour : MonoBehaviour {

    [System.NonSerialized]    public YVALUE m_startFunc;
    [System.NonSerialized]    public YVALUE m_updateFunc;
    [System.NonSerialized]    public YVALUE m_onDestroyFunc;
    [System.NonSerialized]    public YVALUE m_onMouseUpAsButtonFunc;
    [System.NonSerialized]    public Hashtable m_hashtable;


	void Start () {
        callfunc(m_startFunc);
	}
	
	void Update () {
        callfunc(m_updateFunc);
	}

    void OnDestroy()
    {
        callfunc(m_onDestroyFunc);
    }

    void OnMouseUpAsButton()
    {
        callfunc(m_onMouseUpAsButtonFunc);
        
    }

    #region Send Message
    /*
        UnityのSendMessage似た動作をする機能

        スクリプト

        function $_hoge()
        {
            PrintLn("called");
        }

        var $go = new GameObject();
        var $hv = AddBehaviour($go);       ---- 当コンポネント追加
        $hv.AddMsgFunc("xyz",$_hoge);      ---- $_hoge関数を "xyz"として登録
         :
         :
        SendMsg($go, "xyz");　　       --- GameObjectに対してSendMessageを送信。xyz名で定義された関数($_hoge)が呼び出される
    */
    public void AddMsgFunc(string name, YVALUE func)
    {
        if (m_hashtable==null) m_hashtable = new Hashtable();
        name = name.ToUpper();
        m_hashtable[name] = func;
    }
    public void SendMessageSocket(object o)
    {
        if (o==null || !(o is List<object>))
        {
            throw new System.Exception("unexpected");
        }

        var ol = (List<object>)o;
        string name = null;
        if (ol.Count > 0)
        {
            name = ol[0].ToString().ToUpper().Trim('\"');

            if (m_hashtable == null || !m_hashtable.ContainsKey(name))
            {
                slagtool.sys.logline("関数登録がありません : " + name);
                return;
            }
            var func = m_hashtable[name];
            if (func is YVALUE)
            {
                ol.RemoveAt(0); //先頭のnameを削除

                callfunc((YVALUE)func,ol);
                return;
            }
        }
        throw new System.Exception("unexpected");
    }
    #endregion

    //-- util for this class
    void callfunc(YVALUE func)
    {
        if (func!=null && slagipc.cmd_sub.m_slag!=null)
        { 
            if (slagtool.sys.USETRY)
            {
                try {  
            	    slagipc.cmd_sub.m_slag.CallFunc(func,new object[1] { gameObject });
                } catch (System.Exception e)
                {
                    slagtool.sys.logline("--- 例外発生 ---");
                    slagtool.sys.logline(e.Message);
                    slagtool.sys.logline("----------------");
                }
            }
            else
            {
            	slagipc.cmd_sub.m_slag.CallFunc(func,new object[1] { gameObject });
            }
        }
    }
    void callfunc(YVALUE func, object o)
    {
        if (o==null)
        {
            callfunc(func);
            return;
        }

        List<object> ol = null;
        if (o is List<object>)
        {
            ol = (List<object>)o;
        }
        else
        {
            ol = new List<object>();
            ol.Add(o);
        }
        
        ol.Insert(0,gameObject);

        var oary = ol.ToArray();

        if (func!=null && slagipc.cmd_sub.m_slag!=null)
        { 
            if (slagtool.sys.USETRY)
            {
                try {  
            	    slagipc.cmd_sub.m_slag.CallFunc(func,oary);
                } catch (System.Exception e)
                {
                    slagtool.sys.logline("--- 例外発生 ---");
                    slagtool.sys.logline(e.Message);
                    slagtool.sys.logline("----------------");
                }
            }
            else
            {
            	slagipc.cmd_sub.m_slag.CallFunc(func,oary);
            }
        }
    }

}
