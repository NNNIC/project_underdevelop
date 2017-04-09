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
#if obs
        var head = com.m_head;
        if (head!=null)
        {
            head.position = Handles.PositionHandle(head.position,Quaternion.identity);

            if (Handles.Button(head.position,Quaternion.identity,0.2f,0.6f,Handles.SphereCap)) {
                Debug.Log("touched!");
            }
        }
#endif
        Vector3? headpos = com.m_head!=null ? (Vector3?)com.m_head.position : null;
        Vector3? midpos  = com.m_mid!=null  ? (Vector3?)com.m_mid.position  : null;
        
        if (headpos!=null && midpos!=null)
        {
            Vector3 buttonpos = Vector3.zero;
            if (m_bAlt)
            {
                com.m_mid.position = Handles.PositionHandle(com.m_mid.position,Quaternion.identity);
                buttonpos = com.m_mid.position;
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
