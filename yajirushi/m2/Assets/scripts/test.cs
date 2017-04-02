using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    [ContextMenu("Create Universal Arrow")]
    public void Create()
    {
        Arrow.CreateTest1();
    }

    [ContextMenu("Create OneWay Arrow")]
    public void Create2()
    {
        Arrow.CreateTest2();
    }


}
