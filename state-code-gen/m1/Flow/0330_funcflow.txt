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
Public m_tpl2 As String    ' 修正テンプレート
Public m_outsrc As String  ' 出力

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
    
    Dim lines     As Variant
    Dim line_size As Variant
    Dim outsrc    As String
    Dim mark_op   As String
    Dim mark_cl   As String
    Dim bInMark   As Boolean
    Dim bExist    As Boolean
    Dim i         As Integer

    mark_op  = "<<<?"
    mark_cl  = ">>>"

    outsrc = ""
    lines     = Split(m_pf.m_tplfunc, vbLf)
    line_size = UBound(lines,1)

    bInMark = False
    bExist  = False
    For i = 0 To line_size
        Dim l    As String ' 一行
        Dim bApd As String ' 追加？
        bApd = False 
        l = lines(i)
        l = m_su.TrimEnd(l)
        If bInMark = False Then
            ' 　　　　　　　　　　　　　　　　マーク外の時
            Dim index_op As Integer
            index_op = InStr(l, mark_op)
            If index_op > 0 Then '            mark_op発見
                Dim name As String
                bInMark = True
                name = Mid(l,index_op + Len(mark_op))
                name = Trim(name)
                If m_rc.GetValue(m_state,name) <> "" Then
                    bExist = True
                Else
                    bExist = False
                End If
            Else
                bInMark = False
                bApd = True
            End If
        Else
            '                                  マーク内の時
            Dim index_cl As Integer
            index_cl = InStr(l, mark_cl)
            If index_cl > 0 Then '             mark_cl発見
                bInMark = False
            Else
                If bExist Then
                    bApd = True
                End If
            End If
        End If
        
        If bApd Then
            If outsrc <> "" Then
                outsrc = outsrc & vbLf
            End If
            outsrc = outsrc & l
        End If
    Next
    m_tpl2 = outsrc
End Sub

Sub value_insert

    Dim lines     As Variant
    Dim line_size As Integer
    Dim outsrc    As String
    Dim mark_op   As String
    Dim mark_cl   As String
    Dim i         As Integer
    Dim j         As Integer

    mark_op = "[["
    mark_cl = "]]"

    outsrc    = ""
    lines     = Split(m_tpl2, vbLf)
    line_size = UBound(lines,1) 

    For i = 0 To line_size
        Dim l        As String
        Dim index_op As Integer
        Dim index_cl As Integer
        
        l = lines(i)
        l = m_su.TrimEnd(l)

        For j = 0 To 100 ' 一行の中に複数ある場合に対応
　　　　　　Dim l2   As String
            Dim name As String
            Dim v    As String

            index_op = InStr(l,mark_op)
            index_cl = InStr(l,mark_cl)

            '確認
            If index_op = 0 Then 
                Exit For
            End If
            If index_op >= Index_cl Then
                Exit For
            End If
            
            name = Mid(l,index_op + Len(mark_op), index_cl - index_op - Len(mark_op) )
　　　　　　v    = m_rc.GetValue(m_state,name)
            l2   = m_su.Insert(l, mark_op & name & mark_cl, v)
            l2   = m_su.TrimEnd(l2)
            l    = l2
        Next

        If outsrc <> "" Then
            outsrc = outsrc & vbLf
        End If
        
        outsrc = outsrc & l
    Next
    m_outsrc = outsrc
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