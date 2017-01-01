using UnityEngine;
using System.Collections;

namespace slagipc
{
#if UNITY_5
    public class wk
    { 
        public static void SendWrite(string s)
        {
            slagipc.unity.wk_.SendWrite(s);
        }
        public static void SendWriteLine(string s=null)
        {
            slagipc.unity.wk_.SendWriteLine(s);
        }

        public static void Log(string s)
        {
            slagipc.unity.wk_.Log(s);
        }

        public static void Update()
        {
            slagipc.unity.wk_.Update();
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