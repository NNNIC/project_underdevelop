//This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-chart\m1\chart\chart\MainFlow\doc\MainFlow.xlsx
public enum MainFlowState
{
    S_NONE
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
            */
            br_loadexcel(S_LOADEXCEL);
            br_editchart(S_EDITCHART);
    ;
            if (HasNextState())
            {
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
            }
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
