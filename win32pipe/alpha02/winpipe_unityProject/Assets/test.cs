using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.IO;
using System;

public class test : MonoBehaviour {

    const string PIPENAME_PASS1_A =	"\\\\.\\pipe\\testpipe_1A";
    const string PIPENAME_PASS1_B =	"\\\\.\\pipe\\testpipe_1B";


	// Use this for initialization
    void Start () {
        winpipe_work.Init();
        winpipe_work.Create(PIPENAME_PASS1_B,PIPENAME_PASS1_A );	
	}
	
	// Update is called once per frame
	void Update () {

        while (true)
        {
            var s = winpipe_work.Read();
            if (s != null)
            {
                m_output += s + Environment.NewLine;
            }
            else
            {
                break;
            }
        }

    }

    string m_msg="";
    string m_output = "※Run the received pipe application (start_sendto_pipe_exe.bat)\n";
    Vector2 m_scpos;
    private void OnGUI()
    {
        var h = 30;
        m_msg = GUI.TextField(new Rect(0,0,Screen.width-50,h),m_msg);
        if (GUI.Button(new Rect(Screen.width - 50,0,50,h),"Send"))
        {
            if (!string.IsNullOrEmpty(m_msg))
            {
                winpipe_work.Write(m_msg);
            }
            m_msg = "";
        }

        GUILayout.BeginArea(new Rect(0,h,Screen.width,Screen.height-h));
        m_scpos =  GUILayout.BeginScrollView(m_scpos);
        GUILayout.Label(m_output);

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
