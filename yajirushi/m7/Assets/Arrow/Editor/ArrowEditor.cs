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

        if (com.m_hands != null)
        {
            for(var i = 0; i<com.m_hands.Length; i++)
            {
                com.m_hands[i].position = Handles.PositionHandle(com.m_hands[i].position, Quaternion.identity);
            }
        }
    }
}
