/*
    test93 ストップウォッチ
     
     1. 12秒系計と4秒計
　　　
　　 2. ボタン２つ。
     
[START] [RESET]
      __
    /  0 \
   /      \　　＜－－真ん中にもう一つの
   |    15|   
   \      /
    \ 30 /
      ~~
*/


function $_INIT($bFirst)
{
    $m_sm.Goto($_CREATE_BIG_FRAME);
}

var $m_big_frame;
var $m_big_hand;

function $_CREATE_BIG_FRAME($bFirst)
{
    $m_big_frame = Create_big_frame();
    $m_big_hand  = Create_big_hand($m_big_frame);
    $m_sm.Goto($_CREATE_MINI_FRAME);
}

var $m_mini_frame;
var $m_mini_hand;

function $_CREATE_MINI_FRAME($bFirst)
{
    $m_mini_frame = Create_mini_frame();
    $m_mini_hand  = Create_mini_hand($m_mini_frame);
    
    $m_mini_frame.transform.localPosition += Vector3.back * 0.01;
    
    $m_sm.Goto($_TIMERSTART);
}

var $m_elapsed;
function $_TIMERSTART($bFirst)
{
    if ($bFirst)
    {
        $m_elapsed = 0;
        return;
    }
    $m_elapsed += Time.deltaTime;
    
    var $angle_big = (360/4) * $m_elapsed;
    
    $m_big_hand.transform.localEulerAngles = Vector3.back * $angle_big;
    
    var $angle_mini =  (360/12) * Mathf.Floor($m_elapsed);
    $m_mini_hand.transform.localEulerAngles = Vector3.back * $angle_mini;
}


var $m_sm = StateManager();
$m_sm.Goto($_INIT);


//---

var $btn = CreateButton(1,1,Color.white);
$btn.transform.localPosition += Vector3.up * 4.2;

function $_UpdateCheckTouch($go)
{
    if (!Input.GetMouseButtonDown(0)) {return;}
    
    var $pos = Input.mousePosition;
    Dump($pos);
    var $hitgo = GetObjectAtScreenPoint($pos);
    
    if ($hitgo!=null)
    {
        Dump($hitgo);
    }
}

var $m_bhv = AddBehaviour();
$m_bhv.m_updateFunc = $_UpdateCheckTouch;
