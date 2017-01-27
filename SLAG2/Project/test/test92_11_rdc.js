// ラウンドコントローラ

var g_$bet;   //ベット数
var g_$coins; //所持コイン
var g_$stock; //札山
var g_$hands; //手札
var g_$hands_htlist;//手札カードハッシュテーブルリスト
var g_$gain;  //儲け

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
        msgpc_$ht.event_reset();
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
                
                msgpc_$ht.event_reset();
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
                
                msgpc_$ht.event_reset();
                
                $sm.WaitTime(0.2);
                $sm.Goto(rdc_$_Deal_1);
                return;
            }
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
         //var t = g_$stock[0];
         //PrintLn("---->"+t[0] + "," + t[1]  );
         
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
        //g_$stock = cm_create_stock();
        
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
        msgpc_$ht.event_reset();
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
            return;
        }
        if (msgpc_$ht.event_on)
        {
            if (msgpc_$ht.event_callPushed)
            {
                msgpc_$ht.event_reset();
                $sm.Goto(rdc_$_Call);
                return;
            }
            if (msgpc_$ht.event_changePushed)
            {
                msgpc_$ht.event_reset();
                $sm.Goto(rdc_$_Open_2);
                return;
            }
            
        }
    }
}
function rdc_$_Open_2($sm,$bFirst)
{
    if ($bFirst)
    {
        PrintLn("Open_2");
        msgpc_$ht.callbutton.go.SetActive(false);
        msgpc_$ht.changebutton.go.SetActive(false);
        for(var $i = 0; $i<5; $i++)
        {
            var $ht = g_$hands_htlist[$i];
            if ($ht.GetFlip()==false)
            {
                $ht.go.SetActive(false);
        
                //var t = g_$stock[0];
                //PrintLn("---->"+ t[0] + "," + t[1]  );
        
                var $cd = cm_get_onecard(g_$stock);
                
                var $ht2 = cardpc_deal($i, $cd[0], $cd[1]);
                g_$hands_htlist[$i] = $ht2;
                $ht = g_$hands_htlist[$i];
            }
        
            $ht.flip(true);
        }
        $sm.Goto(rdc_$_Call);
    }
}
function rdc_$_Call($sm,$bFirst)
{
    if ($bFirst)
    {
        PrintLn("Call");

        msgpc_$ht.callbutton.go.SetActive(false);
        msgpc_$ht.changebutton.go.SetActive(false);
        
        var $hands = [];
        for(var $i = 0; $i<5; $i++)
        {
            var $ht = g_$hands_htlist[$i];
            $ht.setSelect(false); 
            $hands.Add([$ht.mark,$ht.num]);
        }
        var $result = cm_get_result($hands);
        PrintLn($result);
        
        g_$gain = 0;
        switch($result)
        {
        case "ONEPAIR":       g_$gain = oddspc_$ht.onepair.gain;       oddspc_set_blink(HD_ONEPAIR);   break;
        case "TWOPAIR":       g_$gain = oddspc_$ht.twopair.gain;       oddspc_set_blink(HD_TWOPAIR);   break;
        case "THREECARDS":    g_$gain = oddspc_$ht.threecards.gain;    oddspc_set_blink(HD_THREECARDS);break;
        case "FOURCARDS":     g_$gain = oddspc_$ht.fourcards.gain;     oddspc_set_blink(HD_FOURCARDS); break;
        case "FULLHOUSE":     g_$gain = oddspc_$ht.fullhouse.gain;     oddspc_set_blink(HD_FULLHOUSE); break;
        case "STRAIGHT":      g_$gain = oddspc_$ht.straight.gain;      oddspc_set_blink(HD_STRAIGHT);  break;
        case "FLUSH":         g_$gain = oddspc_$ht.flush.gain;         oddspc_set_blink(HD_FLUSH);     break;
        case "STRAIGHTFLASH": g_$gain = oddspc_$ht.straightflush.gain; oddspc_set_blink(HD_SF);        break;
        }
        if (g_$gain != 0)
        {
            $sm.Goto(rdc_$_PAY);
        }
    }
}
function rdc_$_PAY($sm,$bFirst)
{
    if ($bFirst)
    {
    }
}


var rdc_$sm = StateManager();
rdc_$sm.Goto(rdc_$_Init);

