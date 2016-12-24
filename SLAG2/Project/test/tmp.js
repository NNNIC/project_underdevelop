/*
 Tmp
*/

function ToVector2From3(v)
{
    return new UnityEngine.Vector2(v.x,v.y);
}


var v  = new UnityEngine.Vector3(1,2,3);
var v2 = v / 2;
var u = ToVector2From3(v2);
Dump(u.x);

