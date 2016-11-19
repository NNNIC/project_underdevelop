using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class test1 : MonoBehaviour {

    public string m_folder = @"N:\Project\test";

    public string m_file   = "test01";

    [ContextMenu("Execute")]
    public void Exec()
    {
        var path = Path.Combine(m_folder,m_file + ".js");
        var src  = File.ReadAllText(path,Encoding.UTF8);
        
        slagtool.util.SetLogFunc(Log,LogLine);
        slagtool.util.ExeSrc(src);
    }

    //---
    string m_log;
    public void Log(string s)
    {
        m_log += s;
    }
    public void LogLine(string s)
    {
        m_log += s;
        Debug.Log(m_log);
        m_log = null;
    }
}
