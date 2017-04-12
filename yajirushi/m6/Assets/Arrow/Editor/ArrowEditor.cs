using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Arrow))]
public class ArrowEditor : Editor {


    public override void OnInspectorGUI()
    {
        var com = (Arrow)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("RESET"))
        {
            com.m_type = Arrow.TYPE.NONE;
            com.Update();
        }
        if (GUILayout.Button("Rotate 90"))
        {
            var angle = com.transform.localEulerAngles;
            var y = (angle.y + 90) % 360;
            com.transform.localEulerAngles = new Vector3(0,y,0);

        }
    }

    private void OnSceneGUI()
    {
     
        Tools.current = Tool.None;
        var com = (Arrow)target;

        if (com == null) return;
        Vector3? headpos = com.m_head != null ? (Vector3?)com.m_head.position : null;
        Vector3? midpos = com.m_mids != null && com.m_mids.Length > 0 ? (Vector3?)com.m_mids[0].position : null;

        if (headpos != null)
        {
            com.m_head.position = Handles.PositionHandle((Vector3)headpos, Quaternion.identity);

            if (midpos != null)
            {
                for (var i = 0; i < com.m_mids.Length; i++)
                {
                    com.m_mids[i].position = Handles.PositionHandle(com.m_mids[i].position, Quaternion.identity);
                }
            }
        }
    }
}
