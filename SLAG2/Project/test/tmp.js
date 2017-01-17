/*

 TMP.JS

*/
"using UnityEngine";


var $go = GameObject.CreatePrimitive(PrimitiveType.Quad);
$go.GetComponent(typeof(Renderer)).material = new Material(Shader.Find("Unlit/Color"));
