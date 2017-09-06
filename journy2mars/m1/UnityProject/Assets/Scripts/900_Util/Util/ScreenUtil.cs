using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUtil  {

    public static Camera m_cam;

    public static Vector3 GetWorldPosition(Vector3 mouseposition)
    {
        Camera cam = m_cam!=null ? m_cam : Camera.main;
        return cam.ScreenToWorldPoint(mouseposition);
    }
}
