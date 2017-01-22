// カードパネル

var cardpc_$ht;

function cardpc_createPanel__createpostion($num, $parentgo)
{
    var $go = new GameObject("pos" + $num);
    $go.transform.parent = $parentgo.transform;
    $go.transform.localPosition = new Vector3(-187 + 93 * $num, 0, 0);
}
function cardpc_createPanel()
{
    cardpc_$ht = Hashtable();
    cardpc_$ht.go = new GameObject("cardpc");
    
    for(var $i = 0; $i<5; $i++) cardpc_createPanel__createpostion($i,cardpc_$ht.go);
}
function cardpc_deal($pos/*場所*/, $mark, $num) 
{
    var $ht = util_create_card($mark,$num);
    $ht.flip(false);
    
    var postr = cardpc_$ht.go.transform.find("pos" + $pos);
    $ht.go.transform.parent = postr;
    $ht.go.transform.localPosition = Vector3.zero;
    
    return $ht;
}

cardpc_createPanel();
var $ht0 = cardpc_deal(0,"heart",1);
var $ht1 = cardpc_deal(1,"heart",13);
var $ht2 = cardpc_deal(2,"heart",12);
var $ht3 = cardpc_deal(3,"heart",11);
var $ht4 = cardpc_deal(4,"heart",10);

$ht0.flip(true);
$ht1.flip(true);
$ht2.flip(true);
$ht3.flip(true);
$ht4.flip(true);
