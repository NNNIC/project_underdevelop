// ラウンドコントローラ

var g_$bet;   //ベット数
var g_$coins; //所持コイン
var g_$stock; //札山
var g_$hands; //手札
var g_$hands_htlist;//手札カードハッシュテーブルリスト

function rdc_$_Init($sm, $bFirst)
{
    if ($bFirst)
    {
        oddspc_createPanel();//オッズパネル作成
        cardpc_createPanel();//カードパネル作成
        msgpc_createPanel(); //メッセージパネル作成
        
        $sm.Goto(rdc_$_WaitStart);
    }
}

function rdc_$_WaitStart($sm, $bFirst)
{
    if ($bFirst)
    {
        g_$bet   = 0;
        g_$coins = 100;
        oddspc_set_credits(g_$bet);
        msgpc_$ht.betline.setmsg("BET: 0 C.");
        msgpc_$ht.totalline.setmsg("TOTAL: " + g_$coins + " C.");
        msgpc_$ht.addbutton.go.SetActive(true);
        msgpc_$ht.startbutton.go.SetActive(true);
        msgpc_$ht.reset();
    }
    else
    {
        if (msgpc_$ht.event_on)
        {
            if (msgpc_$ht.event_addPushed)
            {
                g_$bet++;
                g_$coins--;

                msgpc_$ht.betline.setmsg("BET: " + g_$bet + " C.");
                msgpc_$ht.totalline.setmsg("TOTAL: " + g_$coins + " C.");
                
                oddspc_set_credits(g_$bet);
            }
            else if (msgpc_$ht.event_startPushed)
            {
                if (g_$bet==0)
                {
                    g_$bet++;
                    g_$coins--;
                    
                    msgpc_$ht.betline.setmsg("BET: " + g_$bet + " C.");
                    msgpc_$ht.totalline.setmsg("TOTAL: " + g_$coins + " C.");
                    
                    oddspc_set_credits(g_$bet);
                }
                
                msgpc_$ht.addbutton.go.SetActive(false);
                msgpc_$ht.startbutton.go.SetActive(false);
                
                $sm.WaitTime(0.2);
                $sm.Goto(rdc_$_Deal_1);
            }
            msgpc_$ht.reset();
        }
    }
}
function rdc_$_Deal_1($sm, $bFirst)
{
     if ($bFirst)
     {
         PrintLn("DEAL_1");
         g_$stock = cm_create_stock();
         //cm_shuffle(g_$stock);
         g_$hands = cm_get_fivecards(g_$stock);
         
         g_$hands_htlist = [];
         for(var $i = 0; $i<5; $i++)
         {
             var $cd = g_$hands[$i];
             var $ht = cardpc_deal($i, $cd[0], $cd[1]);
             g_$hands_htlist.Add($ht);
         }
         $sm.WaitTime(0.5);
         $sm.Goto(rdc_$_Open_1);
     }
}
function rdc_$_Open_1($sm, $bFirst)
{
    if ($bFirst)
    {
        PrintLn("OPEN_1");
        g_$stock = cm_create_stock();
        
        for(var $i = 0;$i < 5; $i++)
        {
            var $ht = g_$hands_htlist[$i];
            $ht.flip(true);
        }
        $sm.WaitTime(0.5);
        $sm.Goto(rdc_$_Change);
    }
}

var rdc_$_Change_call_or_change;
var rdc_$_Change_flip_count;
function rdc_$_Change($sm, $bFirst)
{
    if ($bFirst)
    {
        PrintLn("CHANGE");
        rdc_$_Change_call_or_change = true;
        rdc_$_Change_flip_count = 0;
        for(var $i=0;$i<5;$i++)
        {
            var $ht = g_$hands_htlist[$i];
            $ht.setSelect(true);
            //msgpc_$ht.centerline.setmsg("CHANE OR CALL");
            //msgpc_$ht.centerline.go.SetActive(true);
            msgpc_$ht.callbutton.go.SetActive(true);
        }
    }
    else
    {
        if (cardpc_$ht.event_touched!=null)
        {
            var $go = cardpc_$ht.event_touched;
            cardpc_$ht.event_reset();
            var pos = $go.transform.parent.name;
            if (pos.length==4 && pos.StartsWith("pos"))
            {
                var numstr = pos.SubString(3,1);
                //PrintLn(numstr);
                var n = int.parse(numstr);
                PrintLn(n.toString());
                var $ht = g_$hands_htlist[n];
                if ($ht.getFlip()==true)
                {
                    $ht.flip(false);
                    if (rdc_$_Change_call_or_change)
                    {
                        rdc_$_Change_call_or_change = false;
                        msgpc_$ht.callbutton.go.SetActive(false);
                        msgpc_$ht.changebutton.go.SetActive(true);
                    }
                    rdc_$_Change_flip_count++;
                    
                    if (rdc_$_Change_flip_count==5)
                    {
                        $sm.Goto(rdc_$_Open_2);
                        return;
                    }
                }
            }
        }
    }
}
function rdc_$_Open_2($sm,$bFirst)
{
    if ($bFirst)
    {
        PrintLn("Open_2");
    }
}

var rdc_$sm = StateManager();
rdc_$sm.Goto(rdc_$_Init);

