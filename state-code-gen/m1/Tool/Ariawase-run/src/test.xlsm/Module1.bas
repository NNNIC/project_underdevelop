Attribute VB_Name = "Module1"
Option Explicit

Dim m_wb As Workbook

Dim m_mf As MainFlow
Dim m_pf As PrepFlow
    

Sub Test()

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
    
    Set m_mf.m_pf = m_pf
    Set m_pf.m_wb = m_wb

    Call m_mf.RunState
    
    If bTest Then
        m_wb.Close
    End If
    
    MsgBox "èIóπ" & vbCrLf & m_pf.m_filename & vbCrLf & m_pf.m_lang

End Sub

