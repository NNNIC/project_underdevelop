using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System;

class winpipe
    {
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "create")]    /*public*/ static extern void create(IntPtr readpipename,IntPtr writepipename);
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "destroy")]   /*public*/ static extern void destroy();
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "read")]      /*public*/ static extern int read();
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "write")]     /*public*/ static extern void write(IntPtr msg);
        [DllImport("winpipe.dll",CallingConvention = CallingConvention.Cdecl,EntryPoint = "get_buf")]   /*public*/ static extern IntPtr get_buf();

        //tool
        public static IntPtr ConvertToPtr(string s)
        {
            return (IntPtr)Marshal.StringToHGlobalAnsi(s);
        }
        public static string ConvertToString(IntPtr p)
        {
            return (string)Marshal.PtrToStringAnsi(p);
        }
        public static void Create(string readpipename, string writepipename)
        {
            Console.WriteLine("READ :" + readpipename);
            Console.WriteLine("WRITE:" + writepipename);

            var ptr_readpipename = ConvertToPtr(readpipename);
            var ptr_writepipename= ConvertToPtr(writepipename);

            create(ptr_readpipename,ptr_writepipename);

            Marshal.FreeHGlobal(ptr_readpipename);
            Marshal.FreeHGlobal(ptr_writepipename);
        }
        public static void Destroy()
        {
            destroy();
        }
        public static string Read()
        {
            var size = read();
            if (size==0) return null;

            var ptr_buf = get_buf();

            return ConvertToString(ptr_buf);
        }
        public static void Write(string msg)
        {
            var ptr_msg = ConvertToPtr(msg);
            write(ptr_msg);
            Marshal.FreeHGlobal(ptr_msg);
        }
    }

public class test : MonoBehaviour {

    const string PIPENAME_PASS1_A =	"\\\\.\\pipe\\testpipe_1A";
    const string PIPENAME_PASS1_B =	"\\\\.\\pipe\\testpipe_1B";


	// Use this for initialization
    void Start () {
        winpipe.Create(	PIPENAME_PASS1_B,PIPENAME_PASS1_A );	

        //while(true)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //    while(true)
        //    {
        //        var s = winpipe.Read();
        //        if (s!=null)
        //        {
        //            m_output += s + Environment.NewLine;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}
	}
	
	// Update is called once per frame
	void Update () {

        while (true)
        {
            var s = winpipe.Read();
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
    string m_output = "※送り先のアプリを起動してください(start_sendto_pipe_exe.bat)\n";
    Vector2 m_scpos;
    private void OnGUI()
    {
        var h = 30;
        m_msg = GUI.TextField(new Rect(0,0,Screen.width-50,h),m_msg);
        if (GUI.Button(new Rect(Screen.width - 50,0,50,h),"Send"))
        {
            if (!string.IsNullOrEmpty(m_msg))
            {
                winpipe.Write(m_msg);
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
