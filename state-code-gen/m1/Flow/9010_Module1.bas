Attribute VB_Name = "Module1"
Option Explicit

Dim m_wb As Workbook

Dim m_mf As MainFlow
Dim m_pf As PrepFlow
    

Sub Test()

    Dim bTest As Boolean
 
    If Dir("C:\Users\gea01\Documents\project_underdevelop\state-code-gen\m1\test-flow.xlsx") <> "" Then
        Set m_wb = Workbooks.Open("C:\Users\gea01\Documents\project_underdevelop\state-code-gen\m1\test-flow.xlsx", True)
        bTest = True
    Else
        Set m_wb = ThisWorkbook
        bTest = False
    End If
    
    Set  m_mf = New MainFlow
    Set  m_pf = New PrepFlow


    Call m_mf.RunState
    
    If bTest Then
        m_wb.Close
    End If
    
    MsgBox "èIóπ"

End Sub
