using UnityEngine;
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
	    if (m_sm==null) m_sm.Update();
	}

    //---

    void S_WAITKEY(bool bFirst)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            m_sm.Goto(S_RUN);
        }
    }

    //slagtool.runtime.process m_proc;
    slagtool.slag m_slag;

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
            slagtool.util.SetLogFunc(LogLine,Log);
            slagtool.util.SetDebugLevel(2);
            slagtool.util.SetBuitIn(typeof(slagunity_builtinfunc));

            m_slag = new slagtool.slag(this);
            m_slag.LoadSrc(src);

            //m_proc = slagtool.util.CreateProcess();
            DumpAllLog();

            m_slag.Run();

            DumpAllLog();

        }
        else
        {
            m_slag.CallFunc("Update");
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
        if (s!=null)     m_log += s;
        if (m_log!=null) m_alllog += m_log;
        m_alllog += System.Environment.NewLine;
        m_log = null;
    }
    public void DumpAllLog()
    {
        if (!string.IsNullOrEmpty(m_alllog)) Debug.Log(m_alllog); 
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
