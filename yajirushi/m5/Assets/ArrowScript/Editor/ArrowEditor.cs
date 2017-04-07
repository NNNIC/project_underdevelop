using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Arrow))]
public class ArrowEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        Tools.current = Tool.None;
        var com = (Arrow)target;

        
    }
}
