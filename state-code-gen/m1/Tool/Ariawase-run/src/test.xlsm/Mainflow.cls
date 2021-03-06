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
Public m_ff As FuncFlow
Public m_rc As ReadChart
Public m_su As StringUtil
Public m_sv As SaveUtil

Public m_enumstr   As String
Public m_selectstr As String
Public m_funcstr   As String

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
    For i = 0 To m_rc.m_statecount
        If m_enumstr <> "" Then
            m_enumstr = m_enumstr & "," & vbLf
        End If
        m_enumstr = m_enumstr & m_rc.m_statelist(i)
    Next
End Sub

Sub select_create()
    Dim i As Integer

    m_selectstr = ""
    For i = 0 To m_rc.m_statecount
        Dim st As String

        st = m_rc.m_statelist(i)
        If st <> "" Then
            If m_selectstr <> "" Then
                m_selectstr = m_selectstr & vbLf
            End If
            m_selectstr = m_selectstr & "Case " & Chr(34) & st & Chr(34) & vbLf & String(4,32) & st & "(bFirst)"
        End If

    Next

End Sub

Sub funcs_create()
    Dim i  As Integer
    Dim st As String

    For i = 0 To m_rc.m_statecount
        st = m_rc.m_statelist(i)
        If st <> "" Then 
            m_ff.m_state = st
            call m_ff.RunState
            If m_funcstr <> "" Then
                m_funcstr = m_funcstr & vbLf
            End If
            m_funcstr = m_funcstr + m_ff.m_outsrc
        End If
    Next
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
            outsrc = outsrc & vbLf
        End If
        If InStr(l,"$contents1$") > 0 And m_enumstr <> "" Then
            outsrc = outsrc & m_su.Indent(m_enumstr, m_pf.m_contents1_col)
            outsrc = outsrc & vbLf
        ElseIf InStr(l,"$contents2$") > 0 And m_funcstr <> "" Then
            outsrc = outsrc & m_su.Indent(m_funcstr, m_pf.m_contents2_col)
            outsrc = outsrc & vbLf
        ElseIf InStr(l,"$contents3$") > 0 And m_selectstr <> "" Then
            outsrc = outsrc & m_su.Indent(m_selectstr, m_pf.m_contents3_col)
            outsrc = outsrc & vbLf
        Else
            outsrc = outsrc & l
        End If
    Next

    outsrc = m_su.LineEnd_CrLf(outsrc)

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