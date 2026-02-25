Public Class BRClr
    Private Const cNo_Text As String = ""
    Public Function GenerateParameters(ByVal m_Array As Object) As String
        Dim StrSql As String = ""
        Dim IntI As Integer
        Dim intNoOFElementPassed As Integer
        Const cEQUALTO = "="
        Const cCOMMA = ","
        Const cCOLON = "'"
        Const cATE = "@"
        intNoOFElementPassed = UBound(m_Array, 1)
        If intNoOFElementPassed <= 0 Then
            Exit Function
        End If
        For IntI = 0 To intNoOFElementPassed Step 2
            If Trim(m_Array(IntI)) <> cNo_Text Then
                If VarType(m_Array(IntI + 1)) = vbCurrency Then
                    StrSql = StrSql & cATE & m_Array(IntI) & cEQUALTO & m_Array(IntI + 1) & cCOMMA
                ElseIf VarType(m_Array(IntI + 1)) = vbDouble Then
                    StrSql = StrSql & cATE & m_Array(IntI) & cEQUALTO & m_Array(IntI + 1) & cCOMMA
                ElseIf VarType(m_Array(IntI + 1)) = VariantType.Integer Then
                    StrSql = StrSql & cATE & m_Array(IntI) & cEQUALTO & m_Array(IntI + 1) & cCOMMA
                Else
                    StrSql = StrSql & cATE & m_Array(IntI) & cEQUALTO & cCOLON & m_Array(IntI + 1) & cCOLON & cCOMMA
                End If
            End If
        Next IntI
        GenerateParameters = Left(StrSql, Len(StrSql) - 1)
    End Function
    Public Function IsDivisbleByTwo(ByVal m_value As Long, Optional ByVal m_base1 As Boolean = True) As Boolean
        If m_base1 Then
            m_value = m_value + 1
        End If
        If ((m_value Mod 2) = 0) Then
            IsDivisbleByTwo = True
        Else
            IsDivisbleByTwo = False
        End If
    End Function
    Public Function GetModify(ByVal m_sp_Name As String, ByVal ParamArray m_ParamName_ParamValue() As Object) As String
        Dim StrSql As String = ""
        If Trim(m_sp_Name) = cNo_Text Then
            MsgBox("No stored procedure name specified")
            Exit Function
        End If
        GetModify = False
        If IsDivisbleByTwo(UBound(m_ParamName_ParamValue, 1)) Then
            StrSql = "exec " & m_sp_Name & " " & GenerateParameters(m_ParamName_ParamValue) & ""
        End If
        Return StrSql
    End Function
End Class
