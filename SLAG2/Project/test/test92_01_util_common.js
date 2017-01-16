"using UnityEngine";
"using System";

function util_CreateTextObj($s)
{
    var $go = new GameObject($s);
    var $tm = $go.AddComponent(typeof(TextMesh));
    $tm.alignment = TextAlignment.Center;
    $tm.anchor    = TextAnchor.MiddleCenter;
    $tm.characterSize = 0.2;
    $tm.fontSize = 64;
    $tm.text = $s;
    
    return $go;
}

