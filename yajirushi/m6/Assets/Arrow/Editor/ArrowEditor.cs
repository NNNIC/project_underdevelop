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
    }

    bool m_bAlt;
    private void OnSceneGUI()
    {
        Tools.current = Tool.None;
        var com = (Arrow)target;

        if (com==null) return;
        Vector3? headpos = com.m_head!=null ? (Vector3?)com.m_head.position : null;
        Vector3? midpos  = com.m_mids!=null && com.m_mids.Length>0  ? (Vector3?)com.m_mids[0].position  : null;
        
        if (headpos!=null && midpos!=null)
        {
            Vector3 buttonpos = Vector3.zero;
            if (m_bAlt)
            {
                com.m_mids[0].position = Handles.PositionHandle(com.m_mids[0].position,Quaternion.identity);
                buttonpos = com.m_mids[0].position;
            }
            else
            {
                com.m_head.position = Handles.PositionHandle(com.m_head.position,Quaternion.identity);
                buttonpos = com.m_head.position;
            }
            if (Handles.Button(buttonpos, Quaternion.identity,0.2f,0.6f,Handles.SphereCap))
            {
                m_bAlt = !m_bAlt;
            }
        }
        else if (headpos!=null)
        {
            com.m_head.position = Handles.PositionHandle((Vector3)headpos,Quaternion.identity);
        }
    }
}
