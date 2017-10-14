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

Public m_pf As PrepFlow
Public m_rc As ReadChart
Public m_su As StringUtil
Public m_sv As SaveUtil

Public m_enumstr As String

Public m_outsrc As String 

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


'サブルーチン

Sub preprocess()
    Call m_pf.RunState
End Sub

Sub chartread_prepare()
    Call m_rc.Init
End Sub

Sub enum_create()
    Dim i As Integer

    m_enumstr = ""
    For i = 0 To m_rc.m_statecount - 1
        If m_enumstr <> "" Then
            m_enumstr = m_enumstr & "," & vbLf
        End If
        m_enumstr = m_enumstr & m_rc.m_statelist(i)
    Next
End Sub

Sub select_create()

End Sub

Sub funcs_create()

End Sub

Sub out_create()
    Dim lines       As Variant
    Dim line_size   As Integer
    Dim outsrc      As String
    Dim i           As Integer
    
    outsrc    = ""
    lines     = Split(m_pf.m_tplsrc2, vbLf)
    line_size = UBound(lines, 1)
    
    For i=0 To line_size
        Dim l As String
        l = lines(i)
        If outsrc <> "" Then
            outsrc = outsrc & vbCrLf
        End If
        If InStr(l,"$contents1$") > 0 Then
            outsrc = outsrc & m_su.Indent(m_enumstr, m_pf.m_contents1_col)
            outsrc = outsrc & vbCrLf
        Else
            outsrc = outsrc & l
        End If
    Next

    Call m_sv.SaveShiftJis(ThisWorkbook.Path & "\" & m_pf.m_filename, outsrc)

    m_outsrc = outsrc

End Sub

'-------------------------------------------------------------------------------
'-------------------------------------------------------------------------------
'-------------------------------------------------------------------------------
'-------------------------------------------------------------------------------

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
' 開始
Sub S_START(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_PREPARE")
    End If
End Sub
' 準備
Sub S_PREPARE(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_CREATE_ENUM")
            ' 事前処理
            call preprocess
            ' チャートリード準備
            call chartread_prepare
    End If
End Sub
' ENUM文生成
Sub S_CREATE_ENUM(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_CREATE_SELECT")
            ' enum文生成
            call enum_create
    End If
End Sub
' SELECT文生成 ※VBA時のみ
Sub S_CREATE_SELECT(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_CREATE_FUNCS")
            ' セレクト文生成
            call select_create
    End If
End Sub
' 関数生成
Sub S_CREATE_FUNCS(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_CREATE_OUTPUT")
            ' 関数作成
            call funcs_create
    End If
End Sub
' 出力生成
Sub S_CREATE_OUTPUT(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_END")
            ' 出力生成
            call out_create
    End If
End Sub
' 終了
Sub S_END(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_END")
    End If
End Sub
