/*
    ÉeÉLÉXÉg Å{Å@îwåi
*/
function util_create_msgobj($msg)
{
    var $ht      = Hashtable();
    $ht.go       = new GameObject("msgobj");
    $ht.txtgo    = util_CreateTextObj($msg);
    $ht.textmesh = $ht.txtgo.GetComponent(typeof(TextMesh));
    $ht.bggo     = GameObject.CreatePrimitive(PrimitiveType.Quad);
    
    $ht.bggo.GetComponent(typeof(Renderer)).material = new Material(Shader.Find("Unlit/Color"));
    $ht.bggo_material = $ht.bggo.GetComponent(typeof(renderer)).material;
    
    return $ht;
    
}

var utilmsg_$ht = util_create_msgobj("test");
