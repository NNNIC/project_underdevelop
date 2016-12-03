//
// Test 51
//


var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
var speed = 50;
var cur = 0;
HookUpdate("Update");
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
