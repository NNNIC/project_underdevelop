VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "StringUtil"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False








Option Explicit

public Function Indent(str As String, ind As Integer) As String ' 各行の先頭に指定されたインデント数を挿入する
    Dim lines       As Variant
    Dim line_size   As Integer

    Dim outsrc      As String
    Dim i           As Integer
    
    lines = Split(str, vbLf)
    line_size = UBound(lines, 1)
    
    outsrc = ""
    For i=0 To line_size -1
        Dim l As String
        l = lines(i)
        l = String(ind, 32) + l
        
        If outsrc <> "" Then
            outsrc = outsrc & vbCrLf
        End If
        outsrc = outsrc & l
    Next
    Indent = outsrc
End Function

public Function Insert(str As String, mark As String, instr As String) As String
    Dim mark_start  As Integer
    Dim mark_next   As Integr

    Dim lines       As Variant
    Dim line_size   As Integer
    Dim bSingle     As Boolean

    Dim outsrc      As String
    Dim i           As Integer

    Insert = ""

    mark_start = InStr(str, mark)
    If mark_start <= 0 Then
        MsgBox "Error Insert Mark:" & mark 
        End
    End If
    mark_next = mark_start + Len(mark)

    '挿入文確認  1行の場合は別処理
    lines = Split(instr, vbLf)
    line_size = UBound(lines,1)
    
    If line_size > 1 Then
       bSingle = False
    Else
       bSingle = True
    End If
    
    'シングル時は、リプレイスのみ
    If bSingle Then
        outsrc = Replace(str, mark, instr)
        Insert = outsrc
        End Function
    End If

    '複数行時は、１行名はmarkの直前まで その後、インデントさせて挿入、最後にmark後の元文字列を入れる
    outsrc = Mid(str,1,mark_start) & vbCrLf
    outsrc = Indent(instr, mark_next) & vbCrLf
    outsrc = outsrc + String(20,mark_next)
    outsrc = outsrc + Mid(str,mark_next)
    
    Insert = outsrc
End Function
