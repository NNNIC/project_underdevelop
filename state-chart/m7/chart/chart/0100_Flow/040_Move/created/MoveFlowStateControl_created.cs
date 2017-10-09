//This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-chart\m7\chart\chart\0100_Flow\040_Move\doc\MoveFlow.xlsx
public enum MoveFlowState
{
    S_NONE
    ,S_WAIT_MOUSEDOWN
    ,S_CHECK_STATE
    ,S_CREATE_SELECT
    ,S_WAIT_MOUSEUP
    ,S_MOVE_DONE
    ,S_MOVE_CANCEL

}
public partial class MoveFlowStateControl {
    /*
        S_NONE
    */
    void S_NONE(int phase, bool bFirst)
    {
        if (phase == 0) {
            if (bFirst) {
                SetNextState();
            }
            NextPhase();
            return;
        }
        if (phase == 1) {
            if (bFirst) {
                /*
                */
                S_NONE
                if (HasNextState())
                {
                    GoNextState();
                }
            }
        }
    }
    /*
        S_WAIT_MOUSEDOWN
        マウスダウン待ち
    */
    void S_WAIT_MOUSEDOWN(int phase, bool bFirst)
    {
        if (phase == 0) {
            if (bFirst) {
                SetNextState(S_CHECK_STATE);
            }
                /*
                */
                if (!Mouse_isDown()) {
                    return;
                }
            NextPhase();
            return;
        }
        if (phase == 1) {
            if (bFirst) {
                /*
                    マウスダウン待ち
                */
                S_WAIT_MOUSEDOWN
                if (HasNextState())
                {
                    GoNextState();
                }
            }
        }
    }
    /*
        S_CHECK_STATE
        ステート確認
    */
    void S_CHECK_STATE(int phase, bool bFirst)
    {
        if (phase == 0) {
            if (bFirst) {
                SetNextState();
            }
            NextPhase();
            return;
        }
        if (phase == 1) {
            if (bFirst) {
                /*
                    ステート内か確認
                */
                check_on_state();
                /*
                    ステート確認
                */
                S_CHECK_STATE
                /*
                    ステート内時
                    キャンセル時
                */
                br_on_state(S_CREATE_SELECT);
                br_cancel(S_WAIT_MOUSEDOWN);
    ;
                if (HasNextState())
                {
                    GoNextState();
                }
            }
        }
    }
    /*
        S_CREATE_SELECT
        セレクト作成
    */
    void S_CREATE_SELECT(int phase, bool bFirst)
    {
        if (phase == 0) {
            if (bFirst) {
                SetNextState(S_WAIT_MOUSEUP);
            }
            NextPhase();
            return;
        }
        if (phase == 1) {
            if (bFirst) {
                /*
                    セレクト作成
                */
                select_create();
                /*
                    セレクト作成
                */
                S_CREATE_SELECT
                if (HasNextState())
                {
                    GoNextState();
                }
            }
        }
    }
    /*
        S_WAIT_MOUSEUP
        マウスアップ待ち
    */
    void S_WAIT_MOUSEUP(int phase, bool bFirst)
    {
        if (phase == 0) {
            if (bFirst) {
                SetNextState();
            }
                /*
                    キャンセルを考慮して他のイベントも含め
                */
                if (!Mouse_isAny()) {
                    return;
                }
            NextPhase();
            return;
        }
        if (phase == 1) {
            if (bFirst) {
                /*
                    マウスアップイベント確認
                    キャンセルイベント確認
                */
                check_mouseup();
                check_cancel();
                /*
                    マウスアップ待ち
                */
                S_WAIT_MOUSEUP
                /*
                */
                br_mouseup(S_MOVE_DONE);
                br_mouseup_cancel(S_MOVE_CANCEL);
    ;
                if (HasNextState())
                {
                    GoNextState();
                }
            }
        }
    }
    /*
        S_MOVE_DONE
        移動完了
    */
    void S_MOVE_DONE(int phase, bool bFirst)
    {
        if (phase == 0) {
            if (bFirst) {
                SetNextState(S_WAIT_MOUSEDOWN);
            }
            NextPhase();
            return;
        }
        if (phase == 1) {
            if (bFirst) {
                /*
                    セレクトクリア
                */
                select_clear();
                /*
                    移動完了
                */
                S_MOVE_DONE
                if (HasNextState())
                {
                    GoNextState();
                }
            }
        }
    }
    /*
        S_MOVE_CANCEL
        移動キャンセル
    */
    void S_MOVE_CANCEL(int phase, bool bFirst)
    {
        if (phase == 0) {
            if (bFirst) {
                SetNextState(S_WAIT_MOUSEDOWN);
            }
            NextPhase();
            return;
        }
        if (phase == 1) {
            if (bFirst) {
                /*
                */
                select_clear();
                /*
                    移動キャンセル
                */
                S_MOVE_CANCEL
                if (HasNextState())
                {
                    GoNextState();
                }
            }
        }
    }
    
}
