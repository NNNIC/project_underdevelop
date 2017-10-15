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
     /*
         S_NONE
         ���w��
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
         �J�n
     */
     void S_START(int phase, bool bFirst)
     {
         if (phase == 0)
         {
             if (bFirst)
             {
                 SetNextState(S_CHECK_ARGS);
                 /*
                     �G���[�N���A
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
                      �G���[���\��
                      �G���[�N���A
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
         �R�}���h�����m�F
     */
     void S_CHECK_ARGS(int phase, bool bFirst)
     {
         if (phase == 0)
         {
             if (bFirst)
             {
                 SetNextState(S_WAITCOMMAND);
                 /*
                     �R�}���h�����Ƀt�@�C�����w�肳��Ă��邩�m�F
                 */
                 check_args();
             }
             /*

             */
             br_loadexcel(S_LOADEXCEL);
             if (HasNextState())
             {
                 GoNextState();
             }
         }
     }
     /*
         S_WAITCOMMAND
         �R�}���h�҂�
     */
     void S_WAITCOMMAND(int phase, bool bFirst)
     {
         if (phase == 0)
         {
             if (bFirst)
             {
                 SetNextState();
                 /*
                     �R�}���h���͊m�F
                 */
                 command_check();
             }
             /*
                  ���[�h��
                  �`���[�g�ҏW��
                  ���C�A�E�g�Z�[�u��
             */
              br_loadexcel(S_LOADEXCEL);
              br_editchart(S_EDITCHART);
              br_savelayout(S_SAVELAYOUT);
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
         �G�N�Z�����[�h
     */
     void S_LOADEXCEL(int phase, bool bFirst)
     {
         if (phase == 0)
         {
             if (bFirst)
             {
                 SetNextState(S_CREATECHART);
                 /*
                     �G�N�Z���t�@�C�����[�h
                 */
                 load_excel();
             }
             /*
                 �G���[���J��
             */
             br_error(S_ERROR);
             if (HasNextState())
             {
                 GoNextState();
             }
         }
     }
     /*
         S_CREATECHART
         �`���[�g�쐬
     */
     void S_CREATECHART(int phase, bool bFirst)
     {
         if (phase == 0)
         {
             if (bFirst)
             {
                 SetNextState(S_WAITCOMMAND);
                 /*
                     �`���[�g�쐬
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
         �`���[�g�ҏW
     */
     void S_EDITCHART(int phase, bool bFirst)
     {
         if (phase == 0)
         {
             if (bFirst)
             {
                 SetNextState(S_WAITCOMMAND);
                 /*
                     �`���[�g�ҏW�J�n
                 */
                 chart_edit_start();
             }
             /*
                 �`���[�g�ҏW�I���҂�
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
         ���C�A�E�g�ۑ�
     */
     void S_SAVELAYOUT(int phase, bool bFirst)
     {
         if (phase == 0)
         {
             if (bFirst)
             {
                 SetNextState(S_WAITCOMMAND);
                 /*
                     ���C�A�E�g�ۑ�
                 */
                 save_layout();
             }
             /*
                 �G���[���J��
             */
             br_error(S_ERROR);
             if (HasNextState())
             {
                 GoNextState();
             }
         }
     }

}

