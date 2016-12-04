using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class guiDisplay : MonoBehaviour
{
    static guiDisplay V;

#region フレームワーク
    void Start()
    {
        V = this;
    }

    void OnGUI()
    {
        guishow();
    }
#endregion

    public class Item {
        public bool           bReadOrWrite;
        public string         text;
    };

    List<Item> m_list;

    Vector2 m_pos;

    string m_inputtext;
    string m_inputlabel;
    Action<string> m_inputcallback;

    void guishow()
    {
        if (m_list==null) return;
        GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
        m_pos = GUILayout.BeginScrollView(m_pos);

        foreach(var i in m_list)
        {
            if (i.bReadOrWrite)
            {
                GUILayout.Label("== [INPUT] ==");
                if (m_inputlabel!=null) GUILayout.Label(m_inputlabel);
                m_inputtext = GUILayout.TextField(m_inputtext);
                if (GUILayout.Button("OK"))
                {
                    if (m_inputcallback!=null) m_inputcallback(m_inputtext);
                    i.text = m_inputlabel + ":" + m_inputtext + Environment.NewLine;
                    i.bReadOrWrite = false;
                }
            }
            else
            {
                GUILayout.Label(i.text);
            }
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    #region write
    public static void WriteLine(string s) { V.writeLine(s); }
    public static void Write(string s)     { V.write(s);     }
    void writeLine(string s)
    {
        if (m_list == null) m_list = new List<Item>();
        m_list.Add(new Item() { bReadOrWrite=false, text =s });
    }
    void write(string s)
    {
        if (m_list == null) m_list = new List<Item>();
        if (m_list.Count==0)
        {
            m_list.Add(new Item() { bReadOrWrite=false, text =s });
        }
        else
        {
            var l = m_list[m_list.Count-1];
            l.text += s;
            m_list[m_list.Count-1] = l;
        }
    }
    #endregion

    #region input
    public static void GetInput(string label, string initialtext, Action<string> cb)
    {
        V.getinput(label,initialtext,cb);
    }
    void getinput(string label, string initialtext, Action<string> cb)
    {
        m_inputlabel = label;
        m_inputtext = initialtext!=null ? initialtext : "?";
        m_inputcallback = cb;
        if (m_list == null) m_list = new List<Item>();
        m_list.Add(new Item() { bReadOrWrite=true, text =null });
    }
    #endregion

}
#if obs
public class guiDisplay : MonoBehaviour
{

    static guiDisplay V;

    Action m_act;


#region フレームワーク
    void Start()
    {
        V = this;
        m_pos = new Vector2(0,1);
    }

    void OnGUI()
    {
        _showlog();

        if (m_act != null) m_act();
    }
#endregion

#region log
    List<string> m_log;
    Vector2 m_pos;
    public static void WriteLine(string s)
    {
        if (V.m_log == null) V.m_log = new List<string>();
        V.m_log.Add(s);
    }
    public static void Write(string s)
    {
        if (V.m_log == null) V.m_log = new List<string>();
        if (V.m_log.Count==0)
        {
            V.m_log.Add(s);
        }
        else
        {
            var l = V.m_log[V.m_log.Count-1];
            l += s;
            V.m_log[V.m_log.Count-1] = l;
        }
    }
    void _showlog()
    {
        GUILayout.Label("== [LOG] ==");
        GUILayout.BeginArea(new Rect(0, 14, Screen.width, Screen.height/2-14));
        m_pos = GUILayout.BeginScrollView(m_pos);
        if (m_log != null) m_log.ForEach(i => GUILayout.Label(i));
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
#endregion

#region 入力
    string m_inputlabel;
    string m_inputtext;
    Action<string> m_inputcallback;
    void _inputtxt()
    {
        GUILayout.BeginArea(new Rect(0,Screen.height / 2, Screen.width, Screen.height / 2));
        GUILayout.Label("== [INPUT] ==");
        if (m_inputlabel != null) GUILayout.Label(m_inputlabel);
        m_inputtext = GUILayout.TextField(m_inputtext);
        GUILayout.Space(1);
        if (GUILayout.Button("OK"))
        {
            m_inputcallback(m_inputtext);
        }
        GUILayout.EndArea();
    }

    public static void GetInput(string label, string initialtext, Action<string> cb)
    {
        V.m_inputlabel = label;
        V.m_inputtext = initialtext!=null ? initialtext : "?";

        V.m_act = V._inputtxt;
        V.m_inputcallback = (s) => { cb(s); V.m_act = null; };
    }

#endregion
}
#endif