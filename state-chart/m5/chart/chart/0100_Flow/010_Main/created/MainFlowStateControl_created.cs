//This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-chart\m2\chart\chart\Flow\010_Main\doc\MainFlow.xlsx
public enum MainFlowState
{
    S_NONE
    ,S_START
    ,S_ERROR
    ,S_WAITCOMMAND
    ,S_LOADEXCEL
    ,S_CREATECHART
    ,S_EDITCHART

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
                SetNextState(S_WAITCOMMAND);
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
            */
            br_loadexcel(S_LOADEXCEL);
            br_editchart(S_EDITCHART);
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
        エクセルファイルロード
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
                エラー時は、S_ERRORへ
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
    
}
