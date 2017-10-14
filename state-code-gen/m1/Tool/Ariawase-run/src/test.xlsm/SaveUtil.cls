VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "SaveUtil"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False







Option Explicit

Public Sub SaveShiftJis(filepath As String, str As String)
    Open filepath For Output As #1
        Print #1, str
    Close #1
End Sub

Public Sub SaveUtf8(filepath As String, str As String)

Dim output As ADODB.Stream

    Set output = New ADODB.Stream

    With output

        .Type = adTypeText

        .Charset = "UTF-8"

        .LineSeparator = adLF

        .Open

    End With

    

    output.WriteText str, adWriteLine

    output.SaveToFile filepath, adSaveCreateOverWrite

    output.Close

End Sub
