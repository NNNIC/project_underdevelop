//This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-chart\m7\chart\chart\0100_Flow\040_Move\doc\MoveFlow.xlsx
public enum MoveFlowState
{
    S_NONE
    ,S_WAIT_MOUSEDOWN
    ,S_CREATE_SELECT
    ,S_WAIT_MOUSEUP
    ,S_MOVE_DONE
    ,S_MOVE_CANCEL

}
public partial class MoveFlowStateControl {
    /*
        S_NONE
        無指定
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
                SetNextState();
            }
            /*
                マウスダウン待ち
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
                    ステート内か確認
                */
                check_on_state();
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
                */
                br_create_fail(S_WAIT_MOUSEDOWN);;
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
                セレクト位置更新
            */
            selectpos_update();
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
                    マウスアップ時
                    キャンセル時
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
                    ステート位置更新
                */
                statepos_update();
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
                    セレクトクリア
                */
                select_clear();
                if (HasNextState())
                {
                    GoNextState();
                }
            }
        }
    }
    
}
