//
// TMP
//

//var path = System.io.path;
//Print(path.getfilenamewithoutextension("abc.def"));
//Print(System.io.path.getfilenamewithoutextension("abc.def"));

//var go = new UnityEngine.GameObject("hoge");
var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);

function Update()
{
    UnityEngine.Debug.Log(UnityEngine.Time.time);
}