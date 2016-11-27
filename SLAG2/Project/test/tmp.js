//
// TMP
//

//var path = System.io.path;
//Print(path.getfilenamewithoutextension("abc.def"));
//Print(System.io.path.getfilenamewithoutextension("abc.def"));

//var go = new UnityEngine.GameObject("hoge");

PrintLn("Check PrintLn Works");
PrintLn("Check PrintLn Works");
PrintLn("Check PrintLn Works");
PrintLn("Check PrintLn Works");
PrintLn("Check PrintLn Works");

var x = 1;

var y = x == 1 ? "yes" : "no";

PrintLn("y=" + y);

var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
var speed = 50;
var cur = 0;
function Update()
{
    UnityEngine.Debug.Log(Time.time);

    cur += speed / 10;
    go.transform.localEulerAngles = new UnityEngine.Vector3(cur, 0, 0);

    var bUp = UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow);
    var bDwn = UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow);
    
    if (bUp)  { speed++; Println(speed); }
    if (bDwn) { speed--; Println(speed); }
}