using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class HierarchyUtility  {

    /// <summary>
    /// シーン全走査
    /// </summary>
    /// <param name="action"></param>
    public static void TraverseGameObject(Action<Transform> action) {TraverseGameObject(null,action);}
    public static void TraverseGameObject(Transform root, Action<Transform> action) //if 'root' is null, it will traverse from the top level.
    {
        var rootList = new List<Transform>();
        
        if (root==null)
        {
            foreach(GameObject o in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (o.transform.parent==null) rootList.Add(o.transform);
            }
        }
        else
        {
            rootList.Add(root);
        }

        Action<Transform> travchildren = null;
        travchildren = (t) => 
        {
            action(t);
            if (t.childCount==0) return;
            for(int i=0;i<t.childCount;i++)
            {
                travchildren(t.GetChild(i));
            }
        };

        rootList.ForEach(travchildren);
    }

    public static GameObject FindGameObject(Transform root, string name, bool bIgnoreCase=false)
    {
        Transform find = null;
        TraverseGameObject(root,(t)=>{
            if (find!=null) return;
            if (bIgnoreCase)
            {
                if (t.name.ToLower() == name.ToLower())
                {
                    find = t;
                }
            }
            else
            {
                if (t.name == name)
                {
                    find = t;
                }
            }
        });

        return find!=null ? find.gameObject : null;
    }

    public static GameObject FindGameObjectContains(Transform root, string partofname)
    {
        Transform find = null;
        TraverseGameObject(root,(t)=>{
            if (find==null && t.name.Contains(partofname))
            {
                find = t;
            }
        });

        return find!=null ? find.gameObject : null;
    }

    public static GameObject[] CollectGameObjectContains(Transform root, string partofname)
    {
        List<GameObject> list = new List<GameObject>();
        TraverseGameObject(root,(t)=>{
            if (t.name.Contains(partofname))
            {
                list.Add(t.gameObject);
            }
        });

        return list.ToArray();
    }

    public static void TraverseSetLayerMask(Transform root, LayerMask layer)
    {
        TraverseGameObject(root,(t)=>{t.gameObject.layer = layer; });
    }

    public static void TraverseSetRenderSortOder(Transform root, int order)
    {
        TraverseGameObject(root,(t)=>{ if (t.GetComponent<Renderer>()!=null) t.GetComponent<Renderer>().sortingOrder = order; });
    }

    public static void TraverseSetShader(Transform root, Shader shader)
    {
        if (shader==null) return;
        TraverseGameObject(root, (t)=>
            {  
                if (t.GetComponent<Renderer>()!=null) 
                {
                    foreach(var mat in t.GetComponent<Renderer>().materials)
                    {
                        mat.shader = shader;  
                    } 
                }
            });
    }

    public static void TraverseVisibility(Transform root, bool bVisible)
    {
        TraverseGameObject(root, (t) =>
        {
            Renderer r = t.gameObject.GetComponent<Renderer>();
            if (r != null)
                r.enabled = bVisible;
        });
    }

    public static void TraverseRenderer(Transform root, Action<Renderer> act) // NULLチェック済みのレンダラを引数にできる。
    {
       TraverseGameObject(root,(t) =>
           {
               if (t.GetComponent<Renderer>()!=null) act(t.GetComponent<Renderer>());
           });
    }
    public static void TraverseMaterial(Transform root, Action<Material> act) // 
    {
        TraverseRenderer(root,r=>{
            foreach(var m in r.materials)
            {
                act(m);
            }
        });
    }

    /// <summary>
    /// 名前に指定文字列を含む、距離が最も近いゲームオブジェクトを探す
    /// </summary>
    public static GameObject FindNearestGameObjectContains(Transform root, Vector3 pos, string partofname)
    {
        Transform find = null;
        TraverseGameObject(root,(t)=>{
            if (!t.name.Contains(partofname)) return;
            if (find==null)
            {
                find = t;
            }
            else
            {
                if (  (find.position - pos).sqrMagnitude > (t.position - pos).sqrMagnitude )
                {
                    find = t;
                }
            }
        });

        return find!=null ? find.gameObject : null;
    }

}
