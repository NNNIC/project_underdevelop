/*
 Test 51
 
 キューブ生成・削除・回転制御
 
 ↑キーと↓キーで回転制御
 Delキーでキューブ削除

*/


var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
SetUpdateCall(go,"Update");
SetOnDestroyCall(go,"OnDestroy");

var speed = 50;
var cur = 0;

function Update(obj)
{
    UnityEngine.Debug.Log(UnityEngine.Time.time);
    cur += speed / 10;
    obj.transform.localEulerAngles = new UnityEngine.Vector3(cur, 0, 0);

    
    var bUp  = UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow);
    var bDwn = UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow);
    var bDel = UnityEngine.Input.GetKey(UnityEngine.KeyCode.Delete);

    if (bUp)  { speed++; Println(speed); }
    
    if (bDwn) { speed--; Println(speed); }

    if (bDel) { UnityEngine.Object.Destroy(obj); PrintLn("Destroy!");}
}

function OnDestroy(obj)
{
    PrintLn("OnDestroy called!");
}