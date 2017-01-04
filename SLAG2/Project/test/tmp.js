/*
 Tmp
*/

"using UnityEngine";

function util_ChangeColor(go,color)
{
    PrintLn(color);
    //var r = go.GetComponent(typeof(UnityEngine.Renderer));
    var r = go.GetComponent(typeof(Renderer));
    r.material.SetColor("_Color", color);
}