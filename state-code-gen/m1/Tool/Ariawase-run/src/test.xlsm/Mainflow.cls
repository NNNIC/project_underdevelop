VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "MainFlow"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False

Option Explicit

Dim m_curFunc As String
Dim m_nextFunc As String

Sub RunState()
    
    m_curFunc = ""
    m_nextFunc = "S_START"
    
    Dim i As Integer
    
    For i = 0 To 10000 Step 1
    
        Dim bFirst As Boolean
    
        If m_nextFunc <> "" Then
            m_curFunc = m_nextFunc
            bFirst = True
        Else
            bFirst = False
        End If
        
        Call MainFlow_Select(m_curFunc, bFirst)
        
        If m_curFunc = "S_END" Then
            Exit For
        End If

    Next

End Sub


Sub SetNextState(func As String)
    m_nextFunc = func
End Sub


'�X�^�u

Sub preprocess()

End Sub

Sub statelist_create()

End Sub

Sub enum_create()

End Sub

Sub select_create()

End Sub

Sub funcs_create()

End Sub

Sub out_create()

End Sub
' This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-code-gen\m1\Flow\0100_Main.xlsx
'
Sub MainFlow_Select(func As String, bFirst As Boolean)
    Select Case func
        Case "S_START"
        	S_START(bFirst)
        Case "S_PREPARE"
        	S_PREPARE(bFirst)
        Case "S_CREATE_ENUM"
        	S_CREATE_ENUM(bFirst)
        Case "S_CREATE_SELECT"
        	S_CREATE_SELECT(bFirst)
        Case "S_CREATE_FUNCS"
        	S_CREATE_FUNCS(bFirst)
        Case "S_CREATE_OUTPUT"
        	S_CREATE_OUTPUT(bFirst)
        Case "S_END"
        	S_END(bFirst)
        
    End Select
End Sub
'
' �J�n
Sub S_START(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_PREPARE")
    End If
End Sub
' ����
Sub S_PREPARE(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_CREATE_ENUM")
            ' �X�e�[�g���X�g�쐬
            call statelist_create
    End If
End Sub
' ENUM������
Sub S_CREATE_ENUM(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_CREATE_SELECT")
            ' enum������
            call enum_create
    End If
End Sub
' SELECT������ ��VBA���̂�
Sub S_CREATE_SELECT(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_CREATE_FUNCS")
            ' �Z���N�g������
            call select_create
    End If
End Sub
' �֐�����
Sub S_CREATE_FUNCS(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_CREATE_OUTPUT")
            ' �֐��쐬
            call funcs_create
    End If
End Sub
' �o�͐���
Sub S_CREATE_OUTPUT(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_END")
            ' �o�͐���
            call out_create
    End If
End Sub
' �I��
Sub S_END(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_END")
    End If
End Sub
