VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "FuncFlow"
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

Public m_state  As String
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
        
        Call FuncFlow_Select(m_curFunc, bFirst)
        
        If m_curFunc = "S_END" Then
            Exit For
        End If

    Next

End Sub


Sub SetNextState(func As String)
    m_nextFunc = func
End Sub


'サブルーチン

Sub tpl_expand()
End Sub

Sub value_insert
End Sub
' This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-code-gen\m1\Flow\0300_Func.xlsx
'
Sub FuncFlow_Select(func As String, bFirst As Boolean)
    Select Case func
        Case "S_START"
        	S_START(bFirst)
        Case "S_PRETPL"
        	S_PRETPL(bFirst)
        Case "S_INSERT"
        	S_INSERT(bFirst)
        Case "S_END"
        	S_END(bFirst)
        
    End Select
End Sub
'
' 開始
Sub S_START(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_PRETPL")
    End If
End Sub
' テンプレート展開
Sub S_PRETPL(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_INSERT")
            call tpl_expand
    End If
End Sub
' 値を挿入
Sub S_INSERT(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_END")
            call value_insert
    End If
End Sub
' 終了
Sub S_END(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_END")
    End If
End Sub
