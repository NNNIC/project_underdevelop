using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesUtil  {

    public static GameObject LoadAndInstanceGameObjectPrefab(string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        if (prefab==null)
        {
            Debug.Log("File not found : ResourcesUtil.Load " + path );
            return null;
        }
        var go = (GameObject)GameObject.Instantiate(prefab);
        return go;
    }

    
}
