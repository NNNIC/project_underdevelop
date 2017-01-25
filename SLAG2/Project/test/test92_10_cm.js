// カードマネージャ

var HEART="heart";
var DAIA ="diamond";
var CLAB ="clab";
var SPAD ="spade";

function cm_create_stock_get_mark($i)
{
    switch($i)
    {
    case 0: return HEART;
    case 1: return DAIA;
    case 2: return CLAB;
    case 3: return SPAD;
    }
}
function cm_create_stock()
{
    var $stock = [];
    for(var $i = 0; $i<4; $i++)
    {
        var $mark = cm_create_stock_get_mark($i);
        for(var $j = 1; $j<=13; $j++)
        {
            var $cd = [$mark,$j];
            $stock.Add($cd);
        }
    }
    return $stock;
}
function cm_get_fivecards($stock)
{
    var $hand = [];
    
    for(var $i = 0; $i < 5; $i++)
    {
        $hand.Add($stock[0]);
        $stock.RemoveAt(0);
    }
    return $hand;
}
