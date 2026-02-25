Imports System.Collections.Generic

Public Enum MessageType
    Success = 0
    Info = 1
    Warning = 2
    [Error] = 3
End Enum

Public Class GenerationResult
    Public Success As Boolean = True
    Public Message As String = ""
    Public MessageType As MessageType = MessageType.Info
    Public Details As New List(Of String)()

    Public Sub AddError(ByVal msg As String)
        Me.MessageType = MessageType.Error
        Me.Success = False
        Me.Details.Add("ERROR: " & msg)
        If String.IsNullOrEmpty(Me.Message) Then Me.Message = msg
    End Sub

    Public Sub AddWarning(ByVal msg As String)
        If Me.MessageType <> MessageType.Error Then Me.MessageType = MessageType.Warning
        Me.Details.Add("WARNING: " & msg)
        If String.IsNullOrEmpty(Me.Message) Then Me.Message = msg
    End Sub

    Public Sub AddInfo(ByVal msg As String)
        Me.Details.Add("INFO: " & msg)
        If String.IsNullOrEmpty(Me.Message) Then Me.Message = msg
    End Sub
End Class
