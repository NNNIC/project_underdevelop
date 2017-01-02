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

//PrintLn("test93_01_libB");

var secframe = CreateCircle(3.5,50,true);
util_ChangeColor(secframe,new UnityEngine.Color(64/255,64/255,64/255,1));
