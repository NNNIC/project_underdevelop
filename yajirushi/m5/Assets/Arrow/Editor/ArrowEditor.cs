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
        
        if (GUILayout.Button("CHANGE"))
        {
            com.Update();
            
        }
    }

    private void OnSceneGUI()
    {
        Tools.current = Tool.None;
        var com = (Arrow)target;

        var head = com.m_head;
        if (head!=null)
        {
            head.position = Handles.PositionHandle(head.position,Quaternion.identity);
        }
    }
}
