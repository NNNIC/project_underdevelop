using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpUtil  {

    public static string DumpList<T>(List<T> l)
    {
        var s = string.Empty;
        foreach(var i in l)
        {
            if (s!=string.Empty) s+=",";
            s+= i;
        }

        return s;
    }


}
