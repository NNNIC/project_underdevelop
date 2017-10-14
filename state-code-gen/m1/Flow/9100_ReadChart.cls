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

Public m_statelist  As Variant 'ステートリスト

Public m_wb As Workbook

Sub statelist_create()
    Dim ws       As Worksheet
    Dim maxrow   As Integer
    Dim maxcol   As Integer
    Dim i,j,k,v  As Integer
    Dim list()   As String

    Set ws = m_wb.Worksheets("state-chart")
    With es.UsedRange
        maxrow = .Rows.Count
        maxcol = .Column.Count
    End With
    
    m_statelist 
    For i=4 To maxcol
       
End Sub

Function _get_row(arg As String)
    
    
End Function
