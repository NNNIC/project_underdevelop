using UnityEngine;
using System.Collections;

namespace slagctl
{
#if UNITY_5
    public class wk
    { 
        public static void SendWrite(string s)
        {
            slagctl.unity.wk_.SendWrite(s);
        }
        public static void SendWriteLine(string s=null)
        {
            slagctl.unity.wk_.SendWriteLine(s);
        }

        public static void Log(string s)
        {
            slagctl.unity.wk_.Log(s);
        }

        public static void Update()
        {
            slagctl.unity.wk_.Update();
        }
    }
#else
    public class wk
    { 
        public static void SendWrite(string s)
        {
        }
        public static void SendWriteLine(string s=null)
        {
        }

        public static void Log(string s)
        {
        }

        public static void Update()
        {
        }
    }

#endif
}