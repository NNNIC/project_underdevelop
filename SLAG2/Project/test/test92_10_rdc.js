// ���E���h�R���g���[��

var g_$bet;   //�x�b�g��
var g_$coins; //�����R�C��

function rdc_$_Init($sm, $bFirst)
{
    if ($bFirst)
    {
        oddspc_createPanel();//�I�b�Y�p�l���쐬
        cardpc_createPanel();//�J�[�h�p�l���쐬
        msgpc_createPanel(); //���b�Z�[�W�p�l���쐬
        
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
     }
}
var rdc_$sm = StateManager();
rdc_$sm.Goto(rdc_$_Init);

