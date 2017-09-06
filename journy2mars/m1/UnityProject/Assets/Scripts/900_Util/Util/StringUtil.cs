using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtil{

    /// <summary>
    /// Extract between start_mark and end_mark
    /// if start_mark is null, extract beggining to end_mark.
    /// if end_mark can not be exist in the string, return null.
    /// </summary>
    public static string Extract(string s, string start_mark, string end_mark)
    {
        if (string.IsNullOrEmpty(s)) return null;
        if (string.IsNullOrEmpty(end_mark)) return null;

        if (start_mark==null)
        {
            var index = s.IndexOf(end_mark);
            return s.Substring(0,index);
        }

        var si = s.IndexOf(start_mark);
        if (si < 0) return null;

        var ei = s.IndexOf(end_mark,si+start_mark.Length);
        if (ei < 0) return null;

        var si2 = si + start_mark.Length;
        return s.Substring(si2, ei - si2 );
    }



}
