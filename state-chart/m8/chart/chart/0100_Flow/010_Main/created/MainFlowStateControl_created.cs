﻿//This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-chart\m8\chart\chart\0100_Flow\010_Main\doc\MainFlow.xlsx
public enum MainFlowState
{
    S_NONE
    ,S_START
    ,S_ERROR
    ,S_CHECK_ARGS
    ,S_WAITCOMMAND
    ,S_LOADEXCEL
    ,S_CREATECHART
    ,S_EDITCHART
    ,S_SAVELAYOUT

}
public partial class MainFlowStateControl {
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
        S_START
        開始
    */
    void S_START(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_CHECK_ARGS);
                /*
                    エラークリア
                */
                error_clear();
                /*
                */
                load_clear();
            }
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_ERROR
    */
    void S_ERROR(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_WAITCOMMAND);
                /*
                    エラー時表示
                    エラークリア
                */
                error_if_show();
                error_clear();
                /*
                */
                load_clear();
            }
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_CHECK_ARGS
        コマンド引数確認
    */
    void S_CHECK_ARGS(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_WAITCOMMAND);
                /*
                    コマンド引数にファイルが指定されているか確認
                */
                check_args();
            }
            /*
            */
            br_loadexcel(S_LOADEXCEL);;
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_WAITCOMMAND
        コマンド待ち
    */
    void S_WAITCOMMAND(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState();
                /*
                    コマンド入力確認
                */
                command_check();
            }
            /*
                ロードへ
                チャート編集へ
                レイアウトセーブへ
            */
            br_loadexcel(S_LOADEXCEL);
            br_editchart(S_EDITCHART);
            br_savelayout(S_SAVELAYOUT);
    ;
            if (HasNextState())
            {
                /*
                */
                command_clear();;
                GoNextState();
            }
        }
    }
    /*
        S_LOADEXCEL
        エクセルロード
    */
    void S_LOADEXCEL(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_CREATECHART);
                /*
                    エクセルファイルロード
                */
                load_excel();
            }
            /*
                エラー時遷移
            */
            br_error(S_ERROR);;
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_CREATECHART
        チャート作成
    */
    void S_CREATECHART(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_WAITCOMMAND);
                /*
                    チャート作成
                */
                chart_draw();
            }
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_EDITCHART
        チャート編集
    */
    void S_EDITCHART(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_WAITCOMMAND);
                /*
                    チャート編集開始
                */
                chart_edit_start();
            }
            /*
                チャート編集終了待ち
            */
            if (!chart_edit_isDone()) return;
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    /*
        S_SAVELAYOUT
        レイアウト保存
    */
    void S_SAVELAYOUT(int phase, bool bFirst)
    {
        if (phase == 0)
        {
            if (bFirst)
            {
                SetNextState(S_WAITCOMMAND);
                /*
                    レイアウト保存
                */
                save_layout();
            }
            /*
                エラー時遷移
            */
            br_error(S_ERROR);;
            if (HasNextState())
            {
                GoNextState();
            }
        }
    }
    
}
