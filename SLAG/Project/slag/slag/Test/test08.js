//
// TEST 08
//

var s1 = "hoge";
var s2 = "vv";
//var s3 = s1 + s2;

//consolewriteline(s1);
//consolewriteline(s2);
//consolewriteline(s3);
consolewriteline( s1 + s2 + "GG" );

//var x = 5 + 10 * 10;

//consolewriteline("5 + 10 * 10=" + x);


var s = ReadLine("Input>");

Print("#input=" + s + "\n\n");


for(var i=0; i<10; i=i+1)
{
	var n = RandomInt(8,9);
	Print("" + i + "=");
	Print(n);
	Print("\n");
}

Print("## END ##");

