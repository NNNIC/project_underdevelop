Attribute VB_Name = "CodeGenerator_Updater"
Option Explicit


' �Q�l http://pineplanter.moo.jp/non-it-salaryman/2016/04/19/excel-vba-external-file/
Sub CodeGenerator_Update()
    Dim buf As String
    Dim filename(10) As String ' MAX�l�͓K��
    Dim fileext(10)  As String
    Dim i            As Integer
    Dim filepath     As String
    Dim cmp          As Object
    Dim name         As String
    
    filepath = "C:\Users\gea01\Documents\project_underdevelop\state-code-gen\m2\Tool\Ariawase-run\src\state_code_gen.xlsm"
    
    ' �A�b�v�f�[�g�t�@�C����ǂ�
    
    i = 0
    Open filepath & "\updatelist.txt" For Input As #1
        Do Until EOF(1)
            Line Input #1, buf
            If buf <> "" Then
                Dim tl As Variant
                tl = Split(buf, " ")
                filename(i) = tl(1)
                fileext(i) = tl(0)
                
                'MsgBox filename(i) & "." & fileext(i)
                i = i + 1
            End If
        Loop
    Close #1

    '�R���|�[�l���g�폜
    For i = 0 To UBound(filename,1)
        For Each cmp In ThisWorkbook.VBProject.VBComponents
            name = filename(i)
            name = Trim(name)
            If name <> "" Then
                If name = cmp.Name Then
                    ThisWorkbook.VBProject.VBComponents.Remove cmp
                    Exit For
                End If
             End If
         Next
    Next

    '�R���|�[�l���g�ǉ�
    For i = 0 To UBound(filename,1)
        name = filename(i)
        name = Trim(name)
        If name <> "" Then
            Dim fn As String
            fn = filepath & "\" & name & "." & fileext(i)
            If Dir(fn) = "" Then
                MsgBox (fn & �h �͑��݂��܂���B�X�L�b�v���܂��B�h)
            Else
                ThisWorkbook.VBProject.VBComponents.Import fn
            End If
        End If
    Next
    
    MsgBox("�X�V����")

End Sub

