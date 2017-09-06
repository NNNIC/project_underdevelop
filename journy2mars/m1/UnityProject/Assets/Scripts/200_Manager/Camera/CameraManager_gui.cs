using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraManager {

    private void OnGUI()
    {
        if (GUILayout.Button("Move Next"))
        {
            MoveNext();
        }
    }


}
