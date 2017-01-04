/*
    test93 ストップウォッチ
     
     1. 1分計と2秒計
　　　
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

//var secframe = CreateCircle(3.5,50,true);
//util_ChangeColor(secframe,new UnityEngine.Color(164/255,164/255,164/255,1));


function S_INIT(bFirst)
{
    m_sm.Goto(S_CREATE_BIG_FRAME);
}

var m_big_frame;
var m_big_hand;

function S_CREATE_BIG_FRAME(bFirst)
{
    m_big_frame = Create_big_frame();
    m_big_hand  = Create_big_hand(m_big_frame);
    m_sm.Goto(S_CREATE_MINI_FRAME);
}

var m_mini_frame;
var m_mini_hand;

function S_CREATE_MINI_FRAME(bFirst)
{
    m_mini_frame = Create_mini_frame();
    m_mini_hand  = Create_mini_hand(m_mini_frame);
    
    m_sm.Goto(S_TIMERSTART);
}

var m_elapsed;
function S_TIMERSTART(bFirst)
{
    if (bFirst)
    {
        m_elapsed = 0;
        return;
    }
    m_elapsed += UnityEngine.Time.deltaTime;
    
    var angle_big = (360/4) * m_elapsed;
    
    m_big_hand.transform.localEulerAngles = UnityEngine.Vector3.back * angle_big;
    
    var angle_mini =  (360/12) * UnityEngine.Mathf.Floor(m_elapsed);
    m_mini_hand.transform.localEulerAngles = UnityEngine.Vector3.back * angle_mini;
}


var m_sm = StateManager();
m_sm.Goto(S_INIT);
