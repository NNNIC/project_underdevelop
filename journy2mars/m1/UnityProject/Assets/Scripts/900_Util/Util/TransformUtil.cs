using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransformUtil  {

    public static List<Transform> GetChildren(Transform t)
    {
        var list = new List<Transform>();
        for(var i = 0; i<t.childCount; i++)
        {
            list.Add(t.GetChild(i));
        }
        return list;
    }

    public static void DoChildren(Transform t, Action<Transform> func)
    {
        for(var i = 0; i<t.childCount; i++)
        {
            func(t.GetChild(i));
        }
    }


}
