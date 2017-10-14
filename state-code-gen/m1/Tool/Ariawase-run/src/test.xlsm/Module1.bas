Attribute VB_Name = "CodeGenerator"
Option Explicit

Dim m_wb As Workbook

Dim m_mf As MainFlow
Dim m_pf As PrepFlow
Dim m_rc As ReadChart
Dim m_su As StringUtil
Dim m_sv As SaveUtil
    

Sub Generate()

    Dim bTest As Boolean
 
    If Dir(ThisWorkbook.Path & "\test-flow.xlsx") <> "" Then
        Set m_wb = Workbooks.Open(ThisWorkbook.Path & "\test-flow.xlsx", True)
        bTest = True
    Else
        Set m_wb = ThisWorkbook
        bTest = False
    End If
    
    Set m_mf = New MainFlow
    Set m_pf = New PrepFlow
    Set m_rc = New ReadChart
    Set m_su = New StringUtil
    Set m_sv = New SaveUtil
    
    Set m_mf.m_pf = m_pf
    Set m_mf.m_rc = m_rc
    Set m_mf.m_su = m_su
    Set m_mf.m_sv = m_sv

    Set m_pf.m_wb = m_wb
    Set m_pf.m_su = m_su

    Set m_rc.m_wb = m_wb

    Call m_mf.RunState
    
    If bTest Then
        m_wb.Close
    End If
    
    'MsgBox "èIóπ" & vbCrLf & m_pf.m_tplsrc2 & vbCrLf & m_pf.m_contents1_col & vbCrLf & m_pf.m_contents2_col & vbCrLf & m_pf.m_contents3_c

    MsgBox m_mf.m_outsrc

End Sub

