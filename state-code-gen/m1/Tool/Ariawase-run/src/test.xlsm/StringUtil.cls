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
    For i=0 To line_size
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

public Function Insert(str As String, mark As String, repstr As String) As String
    Dim mark_start  As Integer
    Dim mark_next   As Integer

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
    lines     = Split(repstr, vbLf)
    line_size = UBound(lines,1)
    
    If line_size >= 1 Then
       bSingle = False
    Else
       bSingle = True
    End If
    
    'シングル時は、リプレイスのみ
    If bSingle Then
        outsrc = Replace(str, mark, repstr)
        Insert = outsrc
    Else
        '複数行時は、１行名はmarkの直前まで その後、インデントさせて挿入、最後にmark後の元文字列を入れる
        outsrc = Mid(str,1,mark_start) & vbLf
        outsrc = Indent(repstr, mark_start) & vbLf
        outsrc = outsrc + String(mark_next,32)
        outsrc = outsrc + Mid(str,mark_next)
        
        Insert = outsrc
    End If
End Function

public Function TrimEnd(str As String) As String
    Dim leng  As Integer
    Dim str2 As String
    Dim i    As Integer
    Dim bDone As Boolean

    leng = Len(str)
    str2 = ""

    bDone = False
    For i = leng To 1 Step -1
        Dim c As String
        Dim a As Integer
        
        c = Mid(str,i,1)

        If bDone = False Then
            a = Asc(c)
            If a > 32 Or a < 0 Then
                str2 = c
                bDone = True
            End If
        Else
            str2 = c + str2
        End If
    Next

    TrimEnd = str2

End Function

public Function LineEnd_CrLf(str As String) As String '改行コードを CrLfに変更
    Dim lines     As Variant
    Dim line_size As Integer
    Dim outsrc    As String
    Dim i         As Integer

    outsrc = ""
    lines     = Split(str, vbLf)
    line_size = UBound(lines,1)

    For i = 0 To line_size
        Dim l As String
        l = lines(i)
        l = TrimEnd(l)
        If outsrc <> "" Then
            outsrc = outsrc & vbCrLf
        End If
        outsrc = outsrc & l
    Next
    
    LineEnd_CrLf = outsrc
End Function

public Function IsMutiLine(str As String) As Boolean '複数行確認
    Dim lines     As Variant
    Dim line_size As Integer
    
    lines     = Split(str, vbLf)
    line_size = UBound(lines,1)

    If line_size > 1 Then
        IsMultiLine = True
    Else
        IsMultiLine = False
    End If
End Function
