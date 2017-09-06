using UnityEngine;
using System.Collections;

public static class VectorUtil {

    public static Vector3 Zero_X(Vector3 v) { return new Vector3(0  , v.y, v.z); }
    public static Vector3 Zero_Y(Vector3 v) { return new Vector3(v.x,   0, v.z); }
    public static Vector3 Zero_Z(Vector3 v) { return new Vector3(v.x, v.y,   0); }

    public static Vector3 Zero_XY(Vector3 v) { return new Vector3(0, 0,   v.z); }


    public static Vector3 Mod_X(Vector3 v, float x) { return new Vector3(  x, v.y, v.z); }
    public static Vector3 Mod_Y(Vector3 v, float y) { return new Vector3(v.x,   y, v.z); }
    public static Vector3 Mod_Z(Vector3 v, float z) { return new Vector3(v.x, v.y,   z); }

    public static Vector3 Minus_X(Vector3 v) { return new Vector3(-v.x, v.y,  v.z); }
    public static Vector3 Minus_Y(Vector3 v) { return new Vector3(v.x, -v.y,  v.z); }
    public static Vector3 Minus_Z(Vector3 v) { return new Vector3(v.x,  v.y, -v.z); } 

    public static Vector3 Minus_XY(Vector3 v) { return new Vector3(-v.x, -v.y, v.z); }

    public static Vector3 Abs(Vector3 v) { return new Vector3(Mathf.Abs(v.x),Mathf.Abs(v.y),Mathf.Abs(v.z)); }

    public static float equal_delta = 0.5f;
    public static bool    Equal(Vector3 a, Vector3 b)
    {
        var diff = a - b;
        return (diff.magnitude < equal_delta);
    }
}
