//This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-chart\m2\chart\chart\Flow\100_Arrow\doc\ArrowFlow.xlsx
public enum ArrowFlowState
{
    S_NONE
    ,S_START
    ,S_UP
    ,S_DOWN
    ,S_DRAW
    ,S_END

}
public partial class ArrowFlowStateControl {
    /*
        S_NONE
    */
    void S_NONE(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState();
             }
             NextPhase();
        }
        if (phase == 1)
        {
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_START
        開始
    */
    void S_START(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState();
                /*
                */
                set_src();
                set_dst();
             }
             NextPhase();
        }
        if (phase == 1)
        {
            /*
            */
            check_if_straight();
            /*
                直線が可能なら描画へ
                障害物がある場合は、上下の要求に応じて
            */
            br_if_straight(S_DRAW);
            br_up_if_req(S_UP);
            br_down_if_req(S_DOWN);
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_UP
        上へ
    */
    void S_UP(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState();
                /*
                    仮の次のポイントクリア
                */
                set_temp_clear();
             }
             NextPhase();
        }
        if (phase == 1)
        {
            /*
                仮ポイント上へ
            */
            temp_up();
            /*
                行き先のＸ位置まで線を引いた場合、障害物がない時、記録
            */
            check_up_if_reach_x();
            /*
            */
            br_up_if_reach_x(S_DRAW);
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_DOWN
        下へ
    */
    void S_DOWN(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState();
                /*
                    仮の次のポイントクリア
                */
                set_temp_clear();
             }
             NextPhase();
        }
        if (phase == 1)
        {
            /*
                仮ポイント下へ
            */
            temp_down();
            /*
                行き先のＸ位置まで線を引いた場合、障害物がない時、記録
            */
            check_down_if_reach_x();
            /*
            */
            br_down_if_reach_x(S_DRAW);
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_DRAW
        描画
    */
    void S_DRAW(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_END);
             }
             NextPhase();
        }
        if (phase == 1)
        {
            /*
                矢印を引く
                ※線データを登録
            */
            draw_start();
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_END
        終了
    */
    void S_END(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState();
             }
             NextPhase();
        }
        if (phase == 1)
        {
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    
}
