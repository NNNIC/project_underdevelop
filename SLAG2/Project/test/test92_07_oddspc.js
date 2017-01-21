/*

   オッズパネル
   
   +----------------------+
   | ONE PAIR         2   |
   | TWO PAIR         4   |
   | THREE CARDS      10  |
   | FULL HOUSE       50  |
   | FOUR CARDS       50  |
   | STRAIGHT         50  |
   | FLUSH            50  |
   | STRAIGHT FLUSH   100 |
   +----------------------+

*/
var HD_ONEPAIR   =  1;
var HD_TWOPAIR   =  2;
var HD_THREECARDS=  4;
var HD_FULLHOUSE =  8;
var HD_FOURCARDS = 16;
var HD_STRAIGHT  = 32;
var HD_FLUSH     = 64;
var HD_SF        =128;

var oddspc_$ht = null;

function oddspc_createPanel__create_lineobj($num, $hand, $odds, $parentgo)
{
    var $ht = Hashtable();
    $ht.go      = new GameObject($num.ToString() + $hand);
    $ht.go.transform.parent = $parentgo.transform;
    $ht.handobj = util_create_msgobj($hand);
    $ht.oddsobj = util_create_msgobj($odds.ToString());
    $ht.odds    = $odds;
    
    //
    var $y = 350 - 30 * $num;
    
    $ht.handobj.go.transform.parent = $ht.go.transform;
    $ht.handobj.go.transform.localPosition = new Vector3(-85,$y,0);
    $ht.handobj.go.transform.localScale = Vector3.one * 0.7;
    
    $ht.oddsobj.go.transform.parent = $ht.go.transform;
    $ht.oddsobj.go.transform.localPosition = new Vector3(135,$y,0);
    $ht.oddsobj.go.transform.localScale = Vector3.one * 0.7;
    
    return $ht;
}
function oddspc_createPanel__set_credits($lineobj,$credits)
{
    var $ht   = $lineobj;
    var $odds = $ht.odds;
    
    $ht.oddsobj.setmsg(($odds * $credits).ToString());
    $ht.oddsobj.setbgsize_reset(1.4);
}
function oddspc_createPanel__blink($lineobj,$onoff)
{
    var $ht   = $lineobj;
    $ht.handobj.blink($onoff);
    $ht.oddsobj.blink($onoff);
}

function oddspc_createPanel()
{
    oddspc_$ht = Hashtable();
    oddspc_$ht.go = new GameObject("oddspc");
    oddspc_$ht.onepair       = oddspc_createPanel__create_lineobj(0,"ONE PAIR",        2, oddspc_$ht.go);
    oddspc_$ht.twopair       = oddspc_createPanel__create_lineobj(1,"TWO PAIR",        4, oddspc_$ht.go);
    oddspc_$ht.threecards    = oddspc_createPanel__create_lineobj(2,"THREE CARDS",    10, oddspc_$ht.go);
    oddspc_$ht.fourcards     = oddspc_createPanel__create_lineobj(3,"FOUR CARDS",     50, oddspc_$ht.go);
    oddspc_$ht.fullhouse     = oddspc_createPanel__create_lineobj(4,"FULL HOUSE",     50, oddspc_$ht.go);
    oddspc_$ht.straight      = oddspc_createPanel__create_lineobj(5,"STRAIGHT",       50, oddspc_$ht.go);
    oddspc_$ht.flush         = oddspc_createPanel__create_lineobj(6,"FLUSH",          50, oddspc_$ht.go);
    oddspc_$ht.straightflush = oddspc_createPanel__create_lineobj(7,"STRAIGHT FLUSH", 200,oddspc_$ht.go);
}

function oddspc_set_credits($credits)
{
    oddspc_createPanel__set_credits(oddspc_$ht.onepair,      $credits);
    oddspc_createPanel__set_credits(oddspc_$ht.twopair,      $credits);
    oddspc_createPanel__set_credits(oddspc_$ht.threecards,   $credits);
    oddspc_createPanel__set_credits(oddspc_$ht.fourcards,    $credits);
    oddspc_createPanel__set_credits(oddspc_$ht.fullhouse,    $credits);
    oddspc_createPanel__set_credits(oddspc_$ht.straight,     $credits);
    oddspc_createPanel__set_credits(oddspc_$ht.flush,        $credits);
    oddspc_createPanel__set_credits(oddspc_$ht.straightflush,$credits);
}

function oddspc_set_blink($num)
{
    switch($num)
    {
    case 1:     oddspc_createPanel__blink(oddspc_$ht.onepair,   true);   break;
    case 2:     oddspc_createPanel__blink(oddspc_$ht.twopair,   true);   break;
    case 4:     oddspc_createPanel__blink(oddspc_$ht.threecards,true);   break;
    case 8:     oddspc_createPanel__blink(oddspc_$ht.fullhouse, true);   break;
    case 16:    oddspc_createPanel__blink(oddspc_$ht.fourcards, true);   break;
    case 32:    oddspc_createPanel__blink(oddspc_$ht.straight,  true);   break;
    case 64:    oddspc_createPanel__blink(oddspc_$ht.flush,     true);   break;
    case 128:   oddspc_createPanel__blink(oddspc_$ht.straightflush,true);   break;
    }
}

oddspc_createPanel();
oddspc_set_credits(2);
oddspc_set_blink(HD_FLUSH);
oddspc_set_blink(HD_FULLHOUSE);
