using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
