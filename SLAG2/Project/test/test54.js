/*
 Test 54
 
 サークル作成

     1 2
     0

*/


function Add_Vertex(d,v0,v1,v2,uv0,uv1,uv2)
{
    var i0 = d.size++
    var i1 = d.size++;
    var i2 = d.size++;

    d.verts.Add(v0);
    d.verts.Add(v1);
    d.verts.Add(v2);

    d.uvs.Add(uv0);
    d.uvs.Add(uv1);
    d.uvs.Add(uv2);

    d.trs.Add(i0);
    d.trs.Add(i1);
    d.trs.Add(i2);

    return d;
}

function Circle_Create(radius,num_of_div)
{
     var step_angle = 360 / num_of_div;
     var d = HashTable();
     d.size   = 0;
     d.verts  = [];
     d.uvs    = [];
     d.trs    = [];

     var v0  = new UnityEngine.Vector3(0, 0, 0);
     var v1  = new UnityEngine.Vector3(0, 1, 0);
     var v2  = new UnityEngine.Vector3(0, 2, 0);

     var uv0 = new UnityEngine.Vector2(0, 0, 0);
     var uv1 = new UnityEngine.Vector2(0, 0.1, 0);
     var uv2 = new UnityEngine.Vector2(0, 0.2, 0);

     d = Add_Vertex(d, v0, v1, v2, uv0, uv1, uv2);
}

Circle_Create(10,6);
