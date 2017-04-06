using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    [ContextMenu("Create ONE WAY Arrow")]
    public void Create()
    {
        ArrowMaker.CreateArrow( Arrow.TYPE.ONEWAY);
    }

    [ContextMenu("Create CURVE R Arrow")]
    public void Create2()
    {
        ArrowMaker.CreateArrow( Arrow.TYPE.TURN_R);
    }

    [ContextMenu("Create CURVE L Arrow")]
    public void Create3()
    {
        ArrowMaker.CreateArrow( Arrow.TYPE.TURN_L);
    }

    [ContextMenu("Create U Tern R Arrow")]
    public void Create4()
    {
        ArrowMaker.CreateArrow( Arrow.TYPE.U_TURN_R);
    }

    [ContextMenu("Create U Tern L Arrow")]
    public void Create5()
    {
        ArrowMaker.CreateArrow( Arrow.TYPE.U_TURN_L);
    }

    [ContextMenu("Create S Tern R Arrow")]
    public void Create6()
    {
        ArrowMaker.CreateArrow( Arrow.TYPE.S_TURN_R);
    }

    [ContextMenu("Create S Tern L Arrow")]
    public void Create7()
    {
        ArrowMaker.CreateArrow( Arrow.TYPE.S_TURN_L);
    }
}
