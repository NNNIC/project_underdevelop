using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Arrow))]
public class ArrowEditor : Editor {

    public static Vector3? m_save_head;

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

        if (com.SizeHandle()>0)
        {
            if (GUILayout.Button("Save 'head' position"))
            {
                m_save_head = com.GetHandle(0).position;
            }
        }
        
        if (m_save_head!=null)
        {
            if (GUILayout.Button("Restore 'head' Position"))
            {
                if (com.SizeHandle()>0)
                {
                    com.GetHandle(0).position = (Vector3)m_save_head;
                }
            }
        }

    }

    private void OnSceneGUI()
    {
     
        Tools.current = Tool.None;
        var com = (Arrow)target;

        if (com == null) return;

        if (com.SizeHandle() != 0)
        {
            for(var i = 0; i<com.SizeHandle() ; i++)
            {
                var hand = com.GetHandle(i);
                var save = hand.position;
                var p = Handles.PositionHandle(hand.position, Quaternion.identity);
                hand.position = new Vector3(p.x,save.y,p.z);
            }
            com.transform.position = Handles.PositionHandle(com.transform.position, Quaternion.identity);
        }
    }
}
