/*
    test93 ストップウォッチ
     
     1. 1分計と2秒計
　　　
　　 2. ボタン２つ。
     
[START] [RESET]
      __
    /  0 \
   /      \　　＜－－真ん中にもう一つの
   |    15|   
   \      /
    \ 30 /
      ~~

   時計は上向き。
   

  var minf = CreateMinFrame(); 分計のフレーム作成
  var secf = CreateSecFrame(); 秒系のフレーム作成
  var minh = CreateMinHand() ; 分計の針
  var sech = CreateSecHand() ; 秒計の針

  var label_01 = CreateTxtObj("1"); //ラベル1
  var label_02 = CreateTxtObj("2"); //ラベル2
  
  
  var notch = CreateNotch(); //ノッチ（刻み）
  
  notch.Add();


*/

//PrintLn("test93_01_libA");


function CreateNotch()
{
    var go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);
    go.transform.localScale = new UnityEngine.Vector3(1,1,1);
    return go;
}
function CreateTxtObj(s)
{
    var go = new UnityEngine.GameObject();
    var tm = go.AddComponent(typeof("UnityEngine.TextMesh"));
    tm.alignment = UnityEngine.TextAlignment.Center;
    tm.anchor    = UnityEngine.TextAnchor.MiddleCenter;
    tm.characterSize = 0.2;
    tm.fontSize = 64;
    tm.text = s;
}

// ※円作成
function _CC_Create_GameObject(d)
{
    var go = new UnityEngine.GameObject();
    var mr = go.AddComponent(typeof ("UnityEngine.MeshRenderer"));
    mr.material = new UnityEngine.Material(UnityEngine.Shader.Find("Unlit/Color"));

    var mf = go.AddComponent(typeof ("UnityEngine.MeshFilter"));

    var mesh = new UnityEngine.Mesh();

    mesh.vertices  = ToArray(UnityEngine.Vector3,        d.verts);
    mesh.uv        = ToArray(UnityEngine.Vector2,        d.uvs);
    mesh.normals   = ToArray(UnityEngine.Vector3,        d.nrms);
    mesh.triangles = ToArray(System.Int32,               d.trs);

    mf.mesh = mesh;

    return go;
}
function _CC_Add_Vertex(d,v0,v1,v2,uv0,uv1,uv2)
{
    var i0 = d.size++;
    var i1 = d.size++;
    var i2 = d.size++;

    d.verts.Add(v0);
    d.verts.Add(v1);
    d.verts.Add(v2);

    d.uvs.Add(uv0);
    d.uvs.Add(uv1);
    d.uvs.Add(uv2);

    var n = UnityEngine.Vector3.Cross(v0 - v1, v1 - v2);
    
    d.nrms.Add(n);
    d.nrms.Add(n);
    d.nrms.Add(n);

    d.trs.Add(i0);
    d.trs.Add(i1);
    d.trs.Add(i2);

    return d;
}

function _CC_ToVector2From3(v)
{
    return new UnityEngine.Vector2(v.x,v.y);
}
function CreateCircle(radius,num_of_div,rev)
{
/*
    rev : reverse   true or false

*/
    var d = HashTable();
    d.size   = 0;
    d.verts  = [];
    d.uvs    = [];
    d.nrms   = [];
    d.trs    = [];

    var angle_deg = 360 / num_of_div;
    var angle = angle_deg * UnityEngine.Mathf.Deg2Rad;


    for(var n = 0; n<num_of_div; n++)
    {
         var v0 = UnityEngine.Vector3.zero;

         var a1 = angle * n;
         var x1 = UnityEngine.Mathf.Cos(a1);
         var y1 = UnityEngine.Mathf.Sin(a1);
         var v1 = new UnityEngine.Vector3(x1,y1,0);


         var a2 = angle * (n+1);
         var x2 = UnityEngine.Mathf.Cos(a2);
         var y2 = UnityEngine.Mathf.Sin(a2);
         var v2 = new UnityEngine.Vector3(x2,y2,0);

         var uv0 = new UnityEngine.Vector2(0.5,0.5); //UVは2次元で正方向1x1の大きさ
         var uv1 = _CC_ToVector2From3(v1/2) + uv0;
         var uv2 = _CC_ToVector2From3(v2/2) + uv0;


	 if (radius != 1)
         {
             v1 *= radius;
             v2 *= radius;
         }


         if (rev)
         {
  	     d = _CC_Add_Vertex(d,v2,v1,v0,uv2,uv1,uv0);
         }
         else
         {
  	     d = _CC_Add_Vertex(d,v0,v1,v2,uv0,uv1,uv2);
         }
    }
    
    var go = _CC_Create_GameObject(d);
    go.name = "Circle_div_"+num_of_div;
    return go;
}
