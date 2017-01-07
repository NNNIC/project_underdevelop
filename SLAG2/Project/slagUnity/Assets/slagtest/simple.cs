using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class simple : MonoBehaviour {

    public string m_folder = @"N:\Project\test";

    public string m_file   = "test01";


    void Start()
    {
        Exec();
    }

    [ContextMenu("Execute")]
    public void Exec()
    {
        var file = m_file;
        if (string.IsNullOrEmpty(Path.GetExtension(file)))
        {
            file += ".js";
        }
        var path = Path.Combine(m_folder,file);
        var src  = File.ReadAllText(path,Encoding.UTF8);
        
        slagtool.util.SetLogFunc(LogLine,Log);
        slagtool.util.SetDebugLevel(1);
        slagtool.util.SetBuitIn(typeof(slagipc_unity_builtinfunc));

        var slag = new slagtool.slag();
        slag.LoadSrc(src);

        DumpAllLog();

        slag.Run();

        DumpAllLog();
    }

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

        System.Diagnostics.Debug.WriteLine(m_log);

        m_alllog += m_log + System.Environment.NewLine;

        m_log = null;
    }
    public void DumpAllLog()
    {
        Debug.Log(m_alllog);
        m_alllog = null;
    }

}