//
// TEST 92 Poaker Game
//
// ポーカーゲーム
//
// H : Hearts
// D : Diamonds
// C : Clubs
// S : Spades
// J : Jokers

function _makeDeck() 
{
    var all_hearts   = ("HA","H2","H3","H4","H5","H6","H7","H8","H9","HX","HJ","HQ","HK");
    var all_diamonds = ("DA","D2","D3","D4","D5","D6","D7","D8","D9","DX","DJ","DQ","DK");
    var all_clubs    = ("CA","C2","C3","C4","C5","C6","C7","C8","C9","CX","CJ","CQ","CK");
    var all_spades   = ("SA","S2","S3","S4","S5","S6","S7","S8","S9","SX","SJ","SQ","SK");
    var all_jokers   = null;    //("JO");

    var deck = ListCombine(all_hearts,all_diamonds,all_clubs,all_clubs,all_jokers);
    
    return deck;
}

// ----

var STOCK;

function _select(num)
{
    var sel;
    for(var i = 0; i<num; i=i+1)
    {
        var s = STOCK[0];//ListAt(STOCK,0); //ListSelectRandom(STOCK); // STOCKより１枚選択
        ListRemove(STOCK,s);             // STOCKより対象を削除
        sel = ListCombine(sel,s);       // 結果を合成
    }
    return sel;
}

function _show_deck(name, deck, num, wait)
{
	Print(name + " : ");
	for(var i = 0; i<5; i=i+1)
	{
		Sleep(wait);
		if (i>=num)
		{
			Print("?? "); 
		}
		else
		{
			var card = ListAt(deck,i);
			Print(card + " ");
		}
	}
	Print("\n");
}
function _show_deck_for_select(deck)
{
    Print("---------------------- _show_deck_for_select\n");
	Dump(deck);
    var s="";
	for(var i = 1; i<=ListSize(deck); i=i+1)
	{
	    s = s+ " " + i + "=>" + ListAt(deck,i-1);
	}
	Print(s + "\n");
}

// analyze hand
function _sort1(deck)
{
    var n1 = Substring(ListAt(deck,0),1,1);
    var n2 = Substring(ListAt(deck,1),1,1);
    var n3 = Substring(ListAt(deck,2),1,1);
    var n4 = Substring(ListAt(deck,3),1,1);
    var n5 = Substring(ListAt(deck,4),1,1);

	var l = (n1,n2,n3,n4,n5);
	l = ListSort(l);
	return l;
}

function IsOnePair(l)
{
	var n1 = ListAt(l,0);
	var n2 = ListAt(l,1);
	var n3 = ListAt(l,2);
	var n4 = ListAt(l,3);
	var n5 = ListAt(l,4);
	
	if (n1==n2 && n2!=n3 && n3!=n4 && n4!=n5) {
	    return true;
	} //AABCD
	if (n1!=n2 && n2==n3 && n3!=n4 && n4!=n5) {
	    return true;
    } //ABBCD
	if (n1!=n2 && n2!=n3 && n3==n4 && n4!=n5) {
	    return true;
	} //ABCCD
	if (n1!=n2 && n2!=n3 && n3!=n4 && n4==n5) {
	    return true;
	} //ABCDD

	return false;
}
function IsTwoPair(l)
{
    var n1 = ListAt(l,0);
	var n2 = ListAt(l,1);
	var n3 = ListAt(l,2);
	var n4 = ListAt(l,3);
	var n5 = ListAt(l,4);
                                                         // 1 2 3 4 5
	if (n1==n2 && n3==n4 && n1!=n3 && n4!=n5) {return true;} // A,A,B,B,C
	if (n1==n2 && n4==n5 && n1!=n3 && n3!=n4) {return true;} // A,A,B,C,C
	if (n2==n3 && n4==n5 && n1!=n2 && n2!=n4) {return true;} // A,B,B,C,C
	return false;
}
function IsThreeCards(l)
{
    var n1 = ListAt(l,0);
	var n2 = ListAt(l,1);
	var n3 = ListAt(l,2);
	var n4 = ListAt(l,3);
	var n5 = ListAt(l,4);
                                                             // 1 2 3 4 5
	if (n1==n2 && n2==n3 && n3!=n4 && n4!=n5) {return true;} // A,A,A,B,C
	if (n1!=n2 && n2==n3 && n3==n4 && n4!=n5) {return true;} // A,B,B,B,C
	if (n1!=n2 && n2!=n3 && n3==n4 && n4==n5) {return true;} // A,B,C,C,C
	return false;
}
function Is4Cards(l)
{
    var n1 = ListAt(l,0);
	var n2 = ListAt(l,1);
	var n3 = ListAt(l,2);
	var n4 = ListAt(l,3);
	var n5 = ListAt(l,4);

	if (n1==n2 && n1==n3 && n1==n4 && n1!=n5) {return true;}
	if (n1!=n2 && n2==n3 && n2==n4 && n2==n5) {return true;}
}
function IsFullHouse(l)
{
    var n1 = ListAt(l,0);
	var n2 = ListAt(l,1);
	var n3 = ListAt(l,2);
	var n4 = ListAt(l,3);
	var n5 = ListAt(l,4);

    if (n1==n2 && n3==n4 && n4==n5 && n1!=n3) {return true;}    //AABBB
    if (n1==n2 && n1==n3 && n4==n5 && n1!=n4) {return true;}    //AAABB

	return false;
}

function tonum(n)
{
    if (n=="A") {return 1;}
    if (n=="X") {return 10;}
    if (n=="J") {return 11;}
    if (n=="Q") {return 12;}
    if (n=="K") {return 13;}

    var i = ToNumber(n);

	return i;
}
function IsStraight(deck)
{
    var n1 = Substring(ListAt(deck,0),1,1);   n1 = tonum(n1);
    var n2 = Substring(ListAt(deck,1),1,1);   n2 = tonum(n2);
    var n3 = Substring(ListAt(deck,2),1,1);   n3 = tonum(n3);
    var n4 = Substring(ListAt(deck,3),1,1);   n4 = tonum(n4);
    var n5 = Substring(ListAt(deck,4),1,1);   n5 = tonum(n5);
    
	var l = (n1,n2,n3,n4,n5);
	l = ListSort(l);

	n1 = ListAt(l,0);
	n2 = ListAt(l,1);
	n3 = ListAt(l,2);
	n4 = ListAt(l,3);
	n5 = ListAt(l,4);

	return (n5-n4==1 && n4-n3==1 && n3-n2==1 && n2-n1==1);
}
function IsFlush(deck)
{
    var n1 = Substring(ListAt(deck,0),0,1);
    var n2 = Substring(ListAt(deck,1),0,1);
    var n3 = Substring(ListAt(deck,2),0,1);
    var n4 = Substring(ListAt(deck,3),0,1);
    var n5 = Substring(ListAt(deck,4),0,1);
    
	return (n1==n2 && n1==n3 && n1==n4 && n4==n5);
}
function Game()
{
   STOCK = _makeDeck();
   Print("===STOCK===\n"); Dump(STOCK);Print("===END OF STOCK===\n\n");

   Print("#### START ####\n");
   Sleep(0.5);
   Print("!! Shuffle !!\n");
   
   STOCK = ListShuffle(STOCK);
   Print("===STOCK===\n"); Dump(STOCK);Print("===END OF STOCK===\n\n");
   
   Print("==DEAL==\n");
   
   var player1_deck;
   var player2_deck;
   var player3_deck;
   var player4_deck;

   for(var i = 0; i<5; i=i+1)
   {
       player1_deck = ListCombine(player1_deck,_select(1));
       player2_deck = ListCombine(player2_deck,_select(1));
       player3_deck = ListCombine(player3_deck,_select(1));
       player4_deck = ListCombine(player4_deck,_select(1));
   }

   _show_deck("DECK 1",player1_deck,2,0.2);
   _show_deck("DECK 2",player2_deck,2,0.2);
   _show_deck("DECK 3",player3_deck,2,0.2);
   _show_deck("DECK 4",player4_deck,2,0.2);

   var deck_num;
   while(true) {
       deck_num = ToNumber(ReadLine("Select your deck. Input from 1 to 4. > "));
	   if (deck_num >= 1 && deck_num <=4) { break; }
	   Print("Please input again!\n");
   }
   
   var player_deck;

   switch(deck_num)
   {
   case 1 : player_deck = player1_deck; break;
   case 2 : player_deck = player2_deck; break;
   case 3 : player_deck = player3_deck; break;
   case 4 : player_deck = player4_deck; break;
   }

   Sleep(1);
   Print("Open your deck\n");
   Sleep(1);

   _show_deck("YOUR DECK",player_deck,5,1);

   Sleep(1);
   Print("Select to play\n");
   Sleep(1);

   Print("1:Change 1 card. \n" );
   Print("2:Change 2 cards.\n");
   Print("3:Change 3 cards.\n");
   Print("4:Change 4 cards.\n");
   Print("5:Change all cards\n");
   Print("N:No Change\n");

   var ans;
   while(true)
   {
		Sleep(0.7);
        ans = ReadLine("Input '1','2','3','4','5' or 'N' >");
		if (ans=="N") { break; }
		var n = ToNumber(ans);
		if (n >= 1 && n <=5) { break; }
		Print("Please input again!\n");
   }

   if (ans!="N")
   {
       var n = ToNumber(ans);
	   if (n<5)
	   {
		   for(var i = 0; i<n ; i=i+1)
		   {
			   _show_deck_for_select(player_deck);
			   var cnum;
			   while(true){
		           cnum = ToNumber(ReadLine("Select a card at " + (i+1) + " of " + n + " >" ));
				   if (cnum >= 1 && cnum <= ListSize(player_deck))
				   {
				       break;
				   }
			   }
			   var card = ListAt(player_deck,(cnum-1));
			   ListRemove(player_deck,card);
		   }
		   _show_deck("CURRENT YOUR DECK:",player_deck,5-n, 0.2);
	   }
	   else 
	   {
	       player_deck = null;
		   Print("CURRENNT YOUR DECK: nothing\n");
	   }

	   var newsel = _select(n);

	   player_deck = ListCombine(player_deck, newsel);

	   _show_deck("FINAL YOUR DECK :",player_deck,5, 1);

   }

   Print("Result:\n");

   var sorted_x_1 =_sort1(player_deck);

	var bOnePair   = IsOnePair(sorted_x_1);
	var bTwoPair   = IsTwoPair(sorted_x_1);
	var b4Cards    = Is4Cards(sorted_x_1);
	var bFullHouse = IsFullHouse(sorted_x_1);
	var bStraight  = IsStraight(player_deck);
	var bFlush     = IsFlush(player_deck);

	if ( 
	  bOnePair   ||
	  bTwoPair   ||
	  b4Cards    ||
	  bFullHouse ||
	  bStraight  ||
	  bFlush   
	)
	{
		if (bOnePair)  { Print("One Pair ");   } 
		if (bTwoPair)  { Print("Two Pair ");   }
		if (b4Cards)   { Print("Four Cards "); }
		if (bFullHouse){ Print("Full House "); }
		if (bStraight) { Print("Straight ");   }
		if (bFlush)    { Print("Flush ");      }
	}
	else 
	{
		Print("No hands!");
	}
}


for(var i = 1; i<=10; i=i+1)
{
    Print("\n\n##### PLAY " + i + "####\n\n");
    Game();
}