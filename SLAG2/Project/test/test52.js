/*
    TEST 52
*/
function CreateRectangleGameObject(width,height)
{
    var go =  new UnityEngine.GameObject();
    go.AddComponet("MeshRender");
    var mf = go.AddComponent("MeshFilter");
    mf.mesh = CreateRectangleMesh(width,height);
}

function CreateRectangleMesh(width,height)
{
    var verts   = new UnityEngine.Vector3[4];
    var normals = new UnityEngine.Vector3[4];
    var uv      = new UnityEngine.Vector2[2];
    var tri     = new System.Int32[6];

    var hw = width  / 2;
    var hh = height / 2;

    verts[0] = new UnityEngine.Vector3(-hw, -hh, 0);
    verts[1] = new UnityEngine.Vector3(+hw, -hh, 0);
    verts[2] = new UnityEngine.Vector3(-hw, +hh, 0);
    verts[3] = new UnityEngine.Vector3(+hw, +hh, 0);

    for (var i = 0; i < normals.Length; i++) {
        normals[i] = UnityEngine.Vector3.up;
    }

    uv[0] = new UnityEngine.Vector2(0, 0);
    uv[1] = new UnityEngine.Vector2(1, 0);
    uv[2] = new UnityEngine.Vector2(0, 1);
    uv[3] = new UnityEngine.Vector2(1, 1);

    tri[0] = 0;
    tri[1] = 2;
    tri[2] = 3;

    tri[3] = 0;
    tri[4] = 3;
    tri[5] = 1;

    var mesh  = new UnityEngine.Mesh();
    mesh.vertices = verts;
    mesh.triangles = tri;
    mesh.uv = uv;
    mesh.normals = normals;

    return mesh;
}

var go= CreateRectangleGameObject(1,1);
go.name = "test52";
