/*
 Tmp
*/

"using UnityEngine";

function $_hoge($go,$a)
{
    PrintLn("called! - " + $a + " at " + $go.name);
}

function $_Update($go)
{
   if (Input.GetKeyDown(KeyCode.A))
   {
       PrintLn("A Key Down");
       SendMsg($go,"touch","touched!!");
   }
}

var $go = new GameObject("HOGE");
var $bh = AddBehaviour($go);
$bh.AddMsgFunc("touch",$_hoge);
$bh.m_updateFunc = $_Update;


