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
    //UnityEngine.Debug.Log(UnityEngine.Vector3.one);
    //go.transform.localEulerAngles = UnityEngine.Vector3.one;//new Vector3(Time.time / 100, 0, 0);
    go.transform.localEulerAngles = new UnityEngine.Vector3(UnityEngine.Time.time * 10, 0, 0);
}