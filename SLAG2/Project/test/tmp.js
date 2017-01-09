/*
 Tmp
*/

"using UnityEngine";

// #############
// #ボタン作成 #
function _CB_CreateRectangleMesh($width,$height)
{
    var $verts   = new Vector3[4];
    var $normals = new Vector3[4];
    var $uv      = new Vector2[4];
    var $tri     = new int[6];
    
    var $hw = $width  / 2;
    var $hh = $height / 2;

    $verts[0] = new Vector3(-$hw, -$hh , 0);
    $verts[1] = new Vector3(+$hw, -$hh , 0);
    $verts[2] = new Vector3(-$hw, +$hh , 0);
    $verts[3] = new Vector3(+$hw, +$hh , 0);

    for (var $i = 0; $i < $normals.Length; $i++) {
        $normals[$i] = Vector3.up;
    }

    $uv[0] = new Vector2(0, 0);
    $uv[1] = new Vector2(1, 0);
    $uv[2] = new Vector2(0, 1);
    $uv[3] = new Vector2(1, 1);

    $tri[0] = 0;
    $tri[1] = 2;
    $tri[2] = 3;

    $tri[3] = 0;
    $tri[4] = 3;
    $tri[5] = 1;

    var $mesh  = new Mesh();
    $mesh.vertices  = $verts;
    $mesh.triangles = $tri;
    $mesh.uv        = $uv;
    $mesh.normals   = $normals;

    return $mesh;
}

function _CB_CreateBox($width,$height)
{
    var $go = new GameObject();
    var $mr = $go.AddComponent(typeof(MeshRenderer));
    $mr.material = new Material(Shader.Find("Unlit/Color"));
    $mr.material.SetColor("_Color",Color.white);
    
    var $mf = $go.AddComponent(typeof(MeshFilter));
    $mf.mesh = _CB_CreateRectangleMesh($width,$height);
    
    var $bc = $go.AddComponent(typeof(BoxCollider));

    return $go;
}

function _CB_SetMsg($ht,$msg)
{
}
function _CB_SetColor($ht,$col)
{
}
function _CB_PushDefaultFunc($ht)
{
    PrintLn("ボタン:"+$ht.go.name + "がPushされました");
    PrintLn("※$ht.pushfuncで差し替えて用");
}
function CreateButton($butman, $id, $width, $height, $white)
{
    $ht         = Hashtable();
    $ht.go      = _CB_CreateBox($width,$height);
    $ht.setmsg  = _CB_SetMsg;
    $ht.setcol  = _CB_SetColor;
    $ht.pushfunc= _CB_PushDefaultFunc;
}

// #ボタン作成 #
// #############

function $_UpdateCheckClick($go)
{
    if (!Input.GetMouseButtonDown(0)) return;
    
    var $pos = Input.mousePosition;
    Dump($pos);
    var $hitgo = GetObjectAtScreenPoint($pos);
    
    if ($hitgo!=null)
    {
        Dump($hitgo);
    }
}



var $but     = CreateButton(1,1,Color.yellow);
var $but_bhv = AddBehaviour($but);
$but_bhv.AddMsgFunc( 

var $bhv = AddBehaviour();
$bhv.m_updateFunc = $_UpdateCheckClick;



