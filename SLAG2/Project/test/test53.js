/*
 Test 53
 
 3Dテキスト作成

*/

function Text3d_Create(s)
{
    var go = new UnityEngine.GameObject();
    var tm = go.AddComponent(typeof("UnityEngine.TextMesh"));
    tm.alignment = UnityEngine.TextAlignment.Center;
    tm.anchor    = UnityEngine.TextAnchor.MiddleCenter;
    tm.characterSize = 0.2;
    tm.fontSize = 64;
    tm.text = s;
}

Text3d_Create("123");

