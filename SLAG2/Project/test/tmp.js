/*

 TMP.JS

*/
"using UnityEngine";


function $_HOGE($sm, $bFirst)
{
    var i = $sm.usrobj;
    PrintLn(i);
}

var sm = statemanager();
sm.usrobj = 1;
sm.goto($_HOGE);

