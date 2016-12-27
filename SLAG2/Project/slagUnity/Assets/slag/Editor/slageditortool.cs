using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Diagnostics;
using System.IO;

public class slageditortool : MonoBehaviour {

    [MenuItem("slag/monitior")]
    static void CallMonitor()
    {
        var path = Path.Combine(Application.dataPath,@"..\..\slagMonitor\m2\slagmon\slagmon\bin\Debug\slagmon.exe");
        UnityEngine.Debug.Log("path="+path);
        Process.Start(path);
    }

}
