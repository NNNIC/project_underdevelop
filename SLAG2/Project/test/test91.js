//
// TEST 91
//
// ”š“–‚ÄƒQ[ƒ€
// Guess What number is.
//
// 3Œ…‚Ì”š‚ğ“–‚Ä‚Ä‚­‚¾‚³‚¢B


var MAXTRY = 20;
var NUM = 0;

function Question()
{
	NUM = NUM + 1;

	// Select each digits of a number is unieque.
	var number = 0;
	while(true)
	{
		number = RandomInt(100,999);
		var n3 =  number % 10;
		var n2 =  ToInt(number / 10) % 10;
		var n1 =  ToInt(number / 100);
		
		//Print("" + number + "\n");

		if (n1!=n2 && n1!=n3 && n2!=n3)
		{
			break;
		}
	}

	Print("\n\n\n");
	Print("-----------------------------\n");
	Print("!! Question " + NUM + " !! \n");
	Print("-----------------------------\n");
	Sleep(0.5);
	Print("There is a number between from 100 to 999.\n");
	Print("Guess the number!\n\n");
	Sleep(0.5);

	for(var tri=0;tri<=MAXTRY;tri=tri+1)
	{
		if (tri==MAXTRY)
		{
			Print("You faild. Game Over!!\n");
			return 1;
		}
		Print("### TRY " + (tri  + 1) + " ###\n");

		var n = 0;
		while(true)
		{
			n = ToNumber(ReadLine("Your guess is >"));
			if (n<100 || n>999) 
			{
				Print("Input is not correct.\n");
				continue;
			}
			else
			{
				break;
			}
		}

		//Print("===\n");
		//Print("n=" + n + "\n");
		//Print("number=" + number + "\n");
		//Print("===\n");

		if (n == number)
		{
			Sleep(0.2);
		    Print("\n");
			Print("******************************************************\n");
			Sleep(0.2);
			Print("... Congraturations! Yes, the number is " + n + "\n");
			Sleep(0.2);
			Print("******************************************************\n\n");
			Sleep(0.2);
			return 0;						
		}
		else if (n > number)
		{
			Print("... The number is less than " + n + "\n");
		}
		else if (n < number)
		{
			Print("... The number is grater than " + n + "\n");
		}
		else 
		{
			Print("!! unexpected \n");
		}

		var n3 =  n % 10;
		var n2 =  ToInt(n / 10) % 10;
		var n1 =  ToInt(n / 100);

		var nm3 = number % 10;
		var nm2 = ToInt(number / 10) % 10;
		var nm1 = ToInt(number / 100);

		var rightplace = 0;
		if (n3==nm3) {rightplace = rightplace +1;}
		if (n2==nm2) {rightplace = rightplace +1;}
		if (n1==nm1) {rightplace = rightplace +1;}

		var anyplace = 0;
		if (n1==nm2 || n1==nm3) {anyplace = anyplace + 1;}
		if (n2==nm1 || n2==nm3) {anyplace = anyplace + 1;}
		if (n3==nm2 || n3==nm1) {anyplace = anyplace + 1;}

		Print("The digits of the number exist the same place : " + rightplace + "\n" );
		Print("The digits of the number exist any place      : " + anyplace   + "\n" );
	}
	return 1;
}

//
//  Main
//

var NL="\n";

Print(
	 "##########################" + NL 
	+"#  GUESS WAHT NUMBER IS  #" + NL
	+"##########################" + NL
);

while(true)
{
	var t = Question();
	if (t==1) { break; }
}
