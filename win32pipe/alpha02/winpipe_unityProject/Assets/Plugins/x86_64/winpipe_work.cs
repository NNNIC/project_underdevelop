using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;

public class winpipe_work  {

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
        public static void Init() // ref http://www.mono-project.com/docs/advanced/pinvoke/
        {
            var dllrelpath   = @"Plugins\x86_64";
            var dllfullpath  = Path.Combine(Application.dataPath,dllrelpath);
            var pathvalue    = dllfullpath + ";" + Environment.GetEnvironmentVariable("path");

            Environment.SetEnvironmentVariable("path", pathvalue);
        }
}
