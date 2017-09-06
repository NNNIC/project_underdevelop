using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RayUtil {

    public static bool IsHit(out RaycastHit hitinfo, Vector3 origin, Vector3 dir, float len = 0)
    {
        Ray ray = new Ray(origin,dir);
        if (len==0)
        {
            Debug.DrawRay(origin,dir,Color.red,5);
            if (Physics.Raycast(ray,out hitinfo))
            {
                return true;
            }
        }
        else
        {
            Debug.DrawLine(origin,origin + dir.normalized * len);
            if (Physics.Raycast(ray,out hitinfo,len))
            {
                return true;
            }
        }
        return false;
    }

    public static RaycastHit? GetHitObject(Camera cam=null)
    {
        if (cam==null) cam = Camera.main;
        Ray ray= cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            return hit;
        }
        return null;
    }
    public static RaycastHit? GetHitObject(LayerMask layer,  Camera cam=null)
    {
        if (cam==null) cam = Camera.main;
        Ray ray= cam.ScreenPointToRay(Input.mousePosition);

        //Debug.DrawRay(ray.origin,ray.direction*1000,Color.red,2000);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,2000, 1<<layer.value))
        {
            return hit;
        }
        return null;
    }
}

