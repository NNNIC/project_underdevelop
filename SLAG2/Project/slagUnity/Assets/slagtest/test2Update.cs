﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class test2Update : MonoBehaviour {

    public string m_folder;
    public string m_file;

    StateManager m_sm;

	void Start () {
	    m_sm = new StateManager();
        m_sm.Goto(S_WAITKEY);
	}
	
	void Update () {
	    m_sm.Update();
	}

    //---

    void S_WAITKEY(bool bFirst)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            m_sm.Goto(S_RUN);
        }
    }

    slagtool.runtime.process m_proc;

    void S_RUN(bool bFirst)
    {
        if (bFirst)
        {
            var file = m_file;
            if (string.IsNullOrEmpty(Path.GetExtension(file)))
            {
                file += ".js";
            }
            var src  = File.ReadAllText(Path.Combine(m_folder,file),Encoding.UTF8);
            slagtool.util.SetLogFunc(Log,LogLine,2);

            slagtool.util.LoadSrc(src);
            m_proc = slagtool.util.CreateProcess();
            DumpAllLog();

            m_proc.Run();
            DumpAllLog();

        }
        else
        {
            m_proc.CallFunc("Update");
        }
    }
    //---
        //---
    string m_alllog = null;
    string m_log;
    public void Log(string s)
    {
        m_log += s;
    }
    public void LogLine(string s)
    {
        m_log += s;
        //Debug.Log(m_log);
        System.Diagnostics.Debug.WriteLine(m_log);

        m_alllog += m_log + System.Environment.NewLine;

        m_log = null;
    }
    public void DumpAllLog()
    {
        Debug.Log(m_alllog);
        m_alllog = null;
    }

    //--
#if UNITY_EDITOR
    [ContextMenu("Edit source")]
    void OpenEdit()
    {
        var path = Path.Combine(m_folder,m_file);
        var editor= @"C:\Program Files\Hidemaru\Hidemaru.exe";
        System.Diagnostics.Process.Start(editor,path);
    }
#endif
}
