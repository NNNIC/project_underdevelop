//This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-chart\m6\chart\chart\0100_Flow\030_Heighlight\doc\HeighlightFlow.xlsx
public enum HeighlightFlowState
{
    S_NONE
    ,S_IDLE
    ,S_NOSTATE
    ,S_NEWSTATE
    ,S_END

}
public partial class HeighlightFlowStateControl {
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
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_IDLE
        アイドル
    */
    void S_IDLE(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState();
            }
            /*
                ポインタ直下のステートの変化確認
            */
            check_st_on_point();
            /*
                ステートばない時
                新ステート時
            */
            br_if_nostate(S_NOSTATE);
            br_if_newstate(S_NEWSTATE);
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_NOSTATE
        ステート無し
    */
    void S_NOSTATE(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_IDLE);
            }
            /*
                カレントステート無し
            */
            setcur_none();
            /*
            */
            heighlight_hide();
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_NEWSTATE
        ステートあり
    */
    void S_NEWSTATE(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_IDLE);
            }
            /*
                カレントステートを更新
            */
            setcur_new();
            /*
            */
            heighlight_show();
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
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    
}
