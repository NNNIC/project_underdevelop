using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUtil {

    public static bool IsTouch()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            return true;
        }
#else
        if (Input.touchCount>0)
        {
            return true;
        }
#endif
        return false;
    }

    public static Vector3 GetTouchPosition()
    {
        return Input.mousePosition;
    }

    public static Vector3 GetTouchPositionInWorld(Camera cam = null)
    {
        if (cam==null) cam = Camera.main;

        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
