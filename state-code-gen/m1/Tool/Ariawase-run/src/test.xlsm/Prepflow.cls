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

Public m_tplsrc   As String
Public m_tplfunc  As String

Public m_filename As String
Public m_lang     As String
Public m_tplsrc2  As String

Public m_contents1_col As Integer 'tplsrc内の $contents1$のカラム位置 ・・・文字整形に利用
Public m_contents2_col As Integer 'tplsrc内の $contents2$のカラム位置 ・・・文字整形に利用
Public m_contents3_col As Integer 'tplsrc内の $contents3$のカラム位置 ・・・文字整形に利用

Public m_wb As Workbook
Public m_su As StringUtil

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
        
        Call PrepFlow_Select(m_curFunc, bFirst)
        
        If m_curFunc = "S_END" Then
            Exit For
        End If

    Next

End Sub


Sub SetNextState(func As String)
    m_nextFunc = func
End Sub


'サブルーチン


Sub tplsrc_get()
    Dim ws As Worksheet
    Dim value As String
    Set ws = m_wb.Worksheets("template-source")
    value = ws.Cells(1, 1).value
    m_tplsrc = value
End Sub

Sub tplfunc_get()
    Dim ws As Worksheet
    Dim value As String
    Set ws = m_wb.Worksheets("template-statefunc")
    value = ws.Cells(1, 1).value
    m_tplfunc = value
End Sub

Sub props_get()
    Dim mark_output As String
    Dim mark_lang   As String

    Dim mark_cts_1  As String
    Dim mark_cts_2  As String
    Dim mark_cts_3  As String

    Dim lines       As Variant
    Dim line_size   As Integer
    Dim i           As Integer

    mark_output = ":output="
    mark_lang   = ":lang="

    mark_cts_1  = "$contents1$"
    mark_cts_2  = "$contents2$"
    mark_cts_3  = "$contents3$"
    m_contents1_col = -1
    m_contents2_col = -1
    m_contents3_col = -1

    m_filename = ""
    m_lang = ""
    m_tplsrc2 = ""

    lines = Split(m_tplsrc, vbLf)
    line_size = UBound(lines, 1)
    
    For i = 0 To line_size
        Dim l As String
        
        l = lines(i)
        l = m_su.TrimEnd(l)
        Debug.Print (l)
                
        If InStr(l, mark_output) = 1 Then
            m_filename = Mid(l, Len(mark_output) + 1)
        ElseIf InStr(l, mark_lang) = 1 Then
            m_lang = Mid(l, Len(mark_lang) + 1)
        ElseIf InStr(l, "::") = 1 Then
            ' スキップ
        Else
            If m_tplsrc2 <> "" Then
                m_tplsrc2 = m_tplsrc2 & vbLf
            End If
            m_tplsrc2 = m_tplsrc2 & l

            ' $contents1$～$contents3$のカラム位置
            If InStr(l, mark_cts_1) > 0 Then
                m_contents1_col = InStr(l, mark_cts_1)
            ElseIf InStr(l, mark_cts_2) > 0 Then
                m_contents2_col = InStr(l, mark_cts_2)
            ElseIf InStr(l, mark_cts_3) > 0 Then
                m_contents3_col = InStr(l, mark_cts_3)
            End If

        End If
    Next
    
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