using UnityEngine;
using System.Collections;
using System;

public static class EnumUtil
{
    public static T GetNextCycric<T>(T n)
    {
        var list = Enum.GetNames(typeof(T));
        int idx = Array.FindIndex(list,i=>i==n.ToString());
        idx = (idx + 1) % list.Length;
        return (T)Enum.Parse(typeof(T),list[idx]);
    }

}
