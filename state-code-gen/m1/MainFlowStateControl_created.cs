public enum MainFlowState
{
     S_NONE,
     S_START,
     S_ERROR,
     S_CHECK_ARGS,
     S_WAITCOMMAND,
     S_LOADEXCEL,
     S_CREATECHART,
     S_EDITCHART,
     S_SAVELAYOUT

}
/*
Sub MainFlow_Select(func As String, bFirst As Boolean)
    Select Case func
          Case "S_NONE"
              S_NONE(bFirst)
          Case "S_START"
              S_START(bFirst)
          Case "S_ERROR"
              S_ERROR(bFirst)
          Case "S_CHECK_ARGS"
              S_CHECK_ARGS(bFirst)
          Case "S_WAITCOMMAND"
              S_WAITCOMMAND(bFirst)
          Case "S_LOADEXCEL"
              S_LOADEXCEL(bFirst)
          Case "S_CREATECHART"
              S_CREATECHART(bFirst)
          Case "S_EDITCHART"
              S_EDITCHART(bFirst)
          Case "S_SAVELAYOUT"
              S_SAVELAYOUT(bFirst)

    End Select
End Sub
*/
public partial class MainFlowStateControl {
    $contents2$
}

