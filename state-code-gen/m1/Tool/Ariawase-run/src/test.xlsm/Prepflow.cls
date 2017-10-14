VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "PrepFlow"
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


'サブルーチン

public m_tplsrc   As String
public m_tplfunc  As String
public m_filename As String
public m_lang     As String

Sub tplsrc_get()
    Dim ws As Worksheet
    Dim value As String
    Set ws = m_wb.Worksheets("template-source")
    Set value = wsHoge.Cells(1, 1).value
    Set m_tplsrc = value
End Sub

Sub tplfunc_get()
    Dim ws As Worksheet
    Dim value As String
    Set ws = m_wb.Worksheets("template-statefunc")
    Set value = wsHoge.Cells(1, 1).value
    Set m_tplfunc = value
End Sub

Sub props_get()

End Sub
' This source is created by ExcelStateChartConverter.exe. Source : C:\Users\gea01\Documents\project_underdevelop\state-code-gen\m1\Flow\0200_Preprocess.xlsx
'
Sub PrepFlow_Select(func As String, bFirst As Boolean)
    Select Case func
        Case "S_START"
        	S_START(bFirst)
        Case "S_GETTPL"
        	S_GETTPL(bFirst)
        Case "S_GETPROP"
        	S_GETPROP(bFirst)
        Case "S_END"
        	S_END(bFirst)
        
    End Select
End Sub
'
' 開始
Sub S_START(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_GETTPL")
    End If
End Sub
' テンプレート取得
Sub S_GETTPL(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_GETPROP")
            call tplsrc_get
            call tplfunc_get
    End If
End Sub
' プロパティの取得
Sub S_GETPROP(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_END")
            call props_get
    End If
End Sub
' 終了
Sub S_END(bFirst As Boolean)
    If bFirst = True Then
            SetNextState("S_END")
    End If
End Sub
