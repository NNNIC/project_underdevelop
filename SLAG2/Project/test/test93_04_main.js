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

  var minf = CreateMinFrame(); 分計のフレーム作成
  var secf = CreateSecFrame(); 秒系のフレーム作成
  var minh = CreateMinHand() ; 分計の針
  var sech = CreateSecHand() ; 秒計の針
*/

//var secframe = CreateCircle(3.5,50,true);
//util_ChangeColor(secframe,new UnityEngine.Color(164/255,164/255,164/255,1));

var m_big_frame;
var m_big_hand;

function S_INIT(bFirst)
{
    m_big_frame = Create_big_frame();
    m_big_hand  = Create_big_hand(m_big_frame);
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
    
    var angle = m_elapsed *(360/12);
    
    m_big_hand.transform.localEulerAngles = UnityEngine.Vector3.back * angle;
}

var m_sm = StateManager();
m_sm.Goto(S_INIT);

