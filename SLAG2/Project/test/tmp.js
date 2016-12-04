//
// Tmp
//
var s_guess = 123;

var n3 = s_guess % 10;
var n2 = UnityEngine.Mathf.Floor(s_guess / 10) % 10;
var n1 = UnityEngine.Mathf.Floor(s_guess / 100);

PrintLn("s_guess.split=" + n1 +"," + n2 + "," + n3);
