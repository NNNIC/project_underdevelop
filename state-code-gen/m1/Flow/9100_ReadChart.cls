VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "ReadChart"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False








Option Explicit

Public m_staterow   As Integer 'ステートの行
Public m_statelist  As Variant 'ステートリスト
Public m_statecount As Integer 'ステート数

Public m_wb As Workbook

Dim m_ws     As WorkSheet
Dim m_maxrow As Integer
Dim m_maxcol As Integer

Public Sub Init()
    Set m_ws = m_wb.Worksheets("state-chart")

    With m_ws.UsedRange
        m_maxrow = .Rows.Count
        m_maxcol = .Columns.Count
    End With
    
    Call statelist_create

End Sub

Public Function GetValue(st As String, cat As String) 'ステートとカテゴリを指定して値を得る
    Dim row, col As Integer
    col = get_statecol(st)
    row = get_row(cat)
    
    GetValue = ""
    If col > 0 And col <= m_maxcol And row > 0 And row <= m_maxrow Then
        GetValue = m_ws.Cells(row, col).value
    End If
End Function

Sub statelist_create()
    Dim i        As Integer
    
    m_staterow  = get_row("state")
    m_statelist = Array()
    
    For i = 3 To m_maxcol
       Dim v   As String  '値
       Dim c   As String  '先頭文字
       Dim ac  As Integer '先頭文字アスキーコード
       Dim bOk As Boolean

       v = m_ws.Cells(m_staterow, i)
       If Len(v) <> 0 Then
           v = Trim(v)
           If Len(v) <> 0 Then
               bOk = False
               c = Mid(v, 1, 1)
               c = UCase(c)
               ac = Asc(c)
               If ac >= 65 And ac <= 90 Then ' A-Z
                  bOk = True
               ElseIf ac = 95 Then ' _
                  bOk = True
               End If
               
               If bOk Then
                   ReDim Preserve m_statelist(UBound(m_statelist) + 1)
                   m_statelist(UBound(m_statelist)) = v
               End If
            End If
        End If
    Next

    m_statecount = UBound(m_statelist)

End Sub


Function get_row(arg As String)
    Dim i As Integer
    Dim v As String
    
    get_row = -1
    For i = 1 To m_maxrow
        v = m_ws.Cells(i, 2).value
        v = Trim(v)
        If v = arg Then
            get_row = i
            Exit For
        End If
    Next
    
End Function

Function get_statecol(arg As String)
    Dim i As Integer
    Dim v As String
    
    get_statecol = -1
    For i = 3 To m_maxcol
        v = m_ws.Cells(m_staterow, i)
        If v = arg Then
            get_statecol = i
            Exit For
        End If
    Next
End Function
