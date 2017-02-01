using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class playslag : MonoBehaviour {

    List<string> files = null;
    slagtool.slag m_slag;

	void Start () {
        var fileinfos = new DirectoryInfo(Application.dataPath + "/Resources/bin").GetFiles("*.bytes");
        files = new List<string>();
        Array.ForEach(fileinfos,f=> { files.Add(f.FullName); Debug.Log(f.Name); });

        slagtool.util.SetDebugLevel(0);
        slagtool.util.SetBuitIn(typeof(slagremote_unity_builtinfunc));
        slagtool.util.SetCalcOp(slagremote_unity_builtincalc_op.Calc_op);

        m_slag = new slagtool.slag();

        slagremote_unity_root.SLAG = m_slag;
	}
	
	void Update () {
		
	}

    Vector2 m_pos;
    void OnGUI()
    {
        var gh = GUILayout.Height(50);

        GUILayout.BeginArea(new Rect(0,Screen.height / 2, Screen.width/2,Screen.height / 2));
        m_pos = GUILayout.BeginScrollView(m_pos);
        for(int i = 0; i<files.Count; i++)
        {
            var fn = Path.GetFileNameWithoutExtension(files[i]);
            if (GUILayout.Button(fn,gh))
            {
                var bytes = Resources.Load<TextAsset>("bin/" + fn).bytes;
                m_slag.LoadBin(bytes);
                m_slag.Run();
            }
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();


    }



}
