using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class CameraManager {
    
    Transform m_cur_ref_tr;

    [ContextMenu("MoveNext")]
    public void MoveNext(int num=15)
    {
        if (m_cur_ref_tr == null)
        {
            m_cur_ref_tr = m_depot.transform.GetChild(0);
        }
        int index = 0;
        for(;index < m_depot.transform.childCount; index++)
        {
            var tr = m_depot.transform.GetChild(index);
            if (tr == m_cur_ref_tr)
            {
                break;
            }
        }

        var newindex = (index + 1) % m_depot.transform.childCount;
        m_cur_ref_tr = m_depot.transform.GetChild(newindex);

        Enumrator_set(MoveSlerp(m_cur_ref_tr.GetComponent<Camera>(),num));
    }

    IEnumerator MoveSlerp(Camera goal_cam, int frames)
    {
        var start_pos = m_mainCam.transform.position;
        var goal_pos  = goal_cam.transform.position;

        var start_rot = m_mainCam.transform.rotation;
        var goal_rot  = goal_cam.transform.rotation;

        var start_sz  = m_mainCam.orthographicSize;
        var goal_sz   = goal_cam.orthographicSize;

        var start_col = m_mainCam.backgroundColor;
        var goal_col  = goal_cam.backgroundColor;

        for(var i = 0; i<=frames ; i++)
        {
            var t = (float)i * (1.0f/frames);
            var pos = Vector3.Slerp(start_pos,goal_pos,t);
            var rot = Quaternion.Slerp(start_rot,goal_rot,t);
            var sz  = Mathf.Lerp(start_sz,goal_sz,t);
            var col = Color.Lerp(start_col, goal_col, t);

            m_mainCam.transform.position = pos;
            m_mainCam.transform.rotation = rot;
            m_mainCam.orthographicSize = sz;
            m_mainCam.backgroundColor  = col;

            yield return null;
        }
    }

}
