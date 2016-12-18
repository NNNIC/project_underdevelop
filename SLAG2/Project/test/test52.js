/*
    TEST 52
*/
function CreateRectangleGameObject(width,height)
{
    var go = new UnityEngine.GameObject();
    var mr = go.AddComponent(typeof("UnityEngine.MeshRenderer"));
    //mr.material = new UnityEngine.Material(UnityEngine.Shader.Find("Standard"));
    var shader = UnityEngine.Resources.Load("Shaders/SampleAlphaTex");
    var tex    = UnityEngine.Resources.Load("2d/at");
    mr.material = new UnityEngine.Material(shader);
    mr.material.SetTexture("_MainTex",tex);

    var mf = go.AddComponent(typeof("UnityEngine.MeshFilter"));
    mf.mesh = CreateRectangleMesh(width,height);

    return go;
}

function CreateRectangleMesh(width,height)
{
    var verts   = new UnityEngine.Vector3[4];
    var normals = new UnityEngine.Vector3[4];
    var uv      = new UnityEngine.Vector2[4];
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

    tri[0] = CastInt(0);
    tri[1] = CastInt(2);
    tri[2] = CastInt(3);

    tri[3] = CastInt(0);
    tri[4] = CastInt(3);
    tri[5] = CastInt(1);

    var mesh  = new UnityEngine.Mesh();
    mesh.vertices = verts;
    mesh.triangles = tri;
    mesh.uv = uv;
    mesh.normals = normals;

    return mesh;
}

var go= CreateRectangleGameObject(1,1);
go.name = "test52";
