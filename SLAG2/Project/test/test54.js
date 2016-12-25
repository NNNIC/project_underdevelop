/*
 Test 54
 
 サークル作成

     1 2
     0

*/

var typeV3 = typeof ("UnityEngine.Vector3");
var typeV2 = typeof ("UnityEngine.Vector2");

function Create_GameObject(d)
{
    var go = new UnityEngine.GameObject();
    var mr = go.AddComponent(typeof ("UnityEngine.MeshRenderer"));
    mr.material = new UnityEngine.Material(UnityEngine.Shader.Find("Standard"));

    var mf = go.AddComponent(typeof ("UnityEngine.MeshFilter"));

    var mesh = new UnityEngine.Mesh();

    mesh.vertices = ToArray(typeV3, d.verts);

    Dump(mesh.vertices);

    mesh.uv        = ToArray(typeV2,        d.uvs);

    Dump(mesh.uv);

    mesh.normals   = ToArray(typeV3,        d.nrms);

    Dump(d.nrms);
    Dump(mesh.normals);

    mesh.triangles = ToArray("System.Int32",d.trs);

    Dump(mesh.triangles);

    mf.mesh = mesh;

    return go;
}


function Add_Vertex(d,v0,v1,v2,uv0,uv1,uv2)
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

function ToVector2From3(v)
{
    return new UnityEngine.Vector2(v.x,v.y);
}

function Circle_Create_test(radius,num_of_div)
{
     var step_angle = 360 / num_of_div;
     var d = HashTable();
     d.size   = 0;
     d.verts  = [];
     d.uvs    = [];
     d.nrms   = [];
     d.trs    = [];

     var v0  = new UnityEngine.Vector3(0, 0, 0);
     var v1  = new UnityEngine.Vector3(0, 1, 0);
     var v2  = new UnityEngine.Vector3(2, 0, 0);

     var uv0 = new UnityEngine.Vector2(0,   0  );
     var uv1 = new UnityEngine.Vector2(0,   0.1);
     var uv2 = new UnityEngine.Vector2(0.2, 0  );

     d = Add_Vertex(d, v0, v1, v2, uv0, uv1, uv2);

     var go = Create_GameObject(d);
     go.name = "Circle";
}

function Circle_Create(radius,num_of_div,rev)
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


    PrintLn("angle=" + angle_deg);

    for(var n = 0; n<num_of_div; n++)
    {
         var v0 = UnityEngine.Vector3.zero;

         var a1 = angle * n;
         var x1 = UnityEngine.Mathf.Cos(a1);
         var y1 = UnityEngine.Mathf.Sin(a1);
         var v1 = new UnityEngine.Vector3(x1,y1,0);

         PrintLn("n=" + n +"," + v1);

         var a2 = angle * (n+1);
         var x2 = UnityEngine.Mathf.Cos(a2);
         var y2 = UnityEngine.Mathf.Sin(a2);
         var v2 = new UnityEngine.Vector3(x2,y2,0);

         var uv0 = new UnityEngine.Vector2(0.5,0.5); //UVは2次元で正方向1x1の大きさ
         var uv1 = ToVector2From3(v1/2) + uv0;
         var uv2 = ToVector2From3(v2/2) + uv0;

         PrintLn("a1:a2=" + a1 + ":" + a2);

         if (rev)
         {
  	     d = Add_Vertex(d,v2,v1,v0,uv2,uv1,uv0);
         }
         else
         {
  	     d = Add_Vertex(d,v0,v1,v2,uv0,uv1,uv2);
         }
    }
    
    var go = Create_GameObject(d);
    go.name = "Circle_div_"+num_of_div;
    return go;
}

var g1 = Circle_Create(10, 4, true);
g1.name = "#1:" + go.name;
g1.transform.position = UnityEngine.Vector3.up * 3;


