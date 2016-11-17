//
// TEST 07
//

function hoge(x,y,z) 
{
   return x+y+z;
}
function hehe() 
{
   ConsoleWrite("A");
}
function hege()
{
	Print(hoge(5,6,7));
	hehe();
	Print("\n");
}

var c = hoge(1,2,3);

ConsoleWriteLine("hoge()=" + c);

hehe();
hehe();
hehe();
ConsoleWriteLine();
hege();


