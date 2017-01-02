function util_ChangeColor(go,color)
{
    PrintLn(color);
    var r = go.GetComponent(typeof(UnityEngine.Renderer));
    r.material.SetColor("_Color", color);
}