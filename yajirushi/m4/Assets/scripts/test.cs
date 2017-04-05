using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    [ContextMenu("Create ONE WAY Arrow")]
    public void Create()
    {
        Arrow.CreateOneWay();
    }

    [ContextMenu("Create RIGHT CURVE Arrow")]
    public void Create2()
    {
        Arrow.CreateRightCurve();
    }

    [ContextMenu("Create Left CURVE Arrow")]
    public void Create3()
    {
        Arrow.CreateLeftCurve();
    }

    [ContextMenu("Create U-Tern Arrow")]
    public void Create4()
    {
        Arrow.CreateUTern();
    }

}
