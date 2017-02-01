using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Text;

public class slageditortool : MonoBehaviour {

    [MenuItem("slag/monitior")]
    static void CallMonitor()
    {
        var path = Path.Combine(Application.dataPath,@"..\..\slagMonitor\m2\slagmon\slagmon\bin\Debug\slagmon.exe");
        UnityEngine.Debug.Log("path="+path);
        Process.Start(path);
    }

    [MenuItem("slag/compile test files")]
    static void CompileTestFiles()
    {
        string   wd  = "N:/Project/test";
        string[] list = new string[] {
            "test01.js",
            "test02.js",
            "test03.js",
            "test04.js",
            "test05.js",

            "test06.js",
            "test07.js",
            "test08.js",
            "test09.js",
            "test10.js",

            "test11.js",
            "test12.js",
            "test13.js",
            "test14.js",

            "test51.js",
            "test52.js",
            "test53.js",
            "test54.js",
            "test55.js",

            "test55.js",

            "test91.js",
            "test92.inc",
            "test93.inc"
        };   

        string savefolder = Application.dataPath + "/slag/slagunity/Resources/bin";

        slagtool.slag slag = new slagtool.slag();
        foreach(var f in list)
        {
            if (Path.GetExtension(f)==".inc")
            { 
                var filelist = convert_inc(Path.Combine(wd,f));
                slag.LoadJSFiles(filelist);
            }
            else
            {
                slag.LoadFile(Path.Combine(wd,f));
            }

            slag.SaveBin(Path.Combine(savefolder,Path.GetFileNameWithoutExtension(f) + ".bytes"));

            UnityEngine.Debug.Log("Compiled .. "+ Path.GetFileNameWithoutExtension(f));
        }
    }
    static string[] convert_inc(string f)
    {
        List<string> filelist = new List<string>();

        string[] readlist = null;
        try { 
            readlist = File.ReadAllLines(f,Encoding.UTF8);
        } catch { return null; }

        if (readlist==null || readlist.Length==0) return null;

        foreach(var l in readlist)
        {
            var nl = l.Trim();
            if (string.IsNullOrEmpty(nl) || nl.StartsWith("//") ) continue;
            filelist.Add(Path.Combine(Path.GetDirectoryName(f),nl));
        }
        return filelist.ToArray();
    }
}
