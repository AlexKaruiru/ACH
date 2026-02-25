Namespace DatatbltoRecordset
    Module DatatbltoRecordset
        'Function DataTableToRecordSet(ByVal dt As System.Data.DataTable) As ADODB.Recordset
        '    Dim rs As New ADODB.Recordset
        '    Dim nCol, nRow, nTotalColumns As Integer
        '    Dim nDataType As ADODB.DataTypeEnum, nDataLength As Integer
        '    Dim col As System.Data.DataColumn
        '    Try
        '        nTotalColumns = dt.Columns.Count
        '        Dim strFields(nTotalColumns - 1) As String, objValues(nTotalColumns - 1) As Object
        '        For nCol = 0 To nTotalColumns - 1
        '            col = dt.Columns.Item(nCol)
        '            nDataLength = 0
        '            Select Case (col.DataType.FullName)
        '                Case "System.String"
        '                    nDataType = ADODB.DataTypeEnum.adVarWChar
        '                    nDataLength = col.MaxLength
        '                Case "System.Int32"
        '                    nDataType = ADODB.DataTypeEnum.adInteger
        '                Case "System.DateTime"
        '                    nDataType = ADODB.DataTypeEnum.adDate
        '                Case "System.Boolean"
        '                    nDataType = ADODB.DataTypeEnum.adBoolean
        '                Case "System.Single"
        '                    nDataType = ADODB.DataTypeEnum.adSingle
        '                Case "System.Double"
        '                    nDataType = ADODB.DataTypeEnum.adDouble
        '                Case Else
        '                    nDataType = ADODB.DataTypeEnum.adVariant
        '            End Select
        '            rs.Fields.Append(col.ColumnName, nDataType, nDataLength, ADODB.FieldAttributeEnum.adFldMayBeNull)
        '            strFields(nCol) = col.ColumnName
        '        Next nCol
        '        rs.Open()
        '        For nRow = 0 To dt.Rows.Count - 1
        '            rs.AddNew()
        '            For nCol = 0 To nTotalColumns - 1
        '                rs.Fields(nCol).Value = dt.Rows(nRow).Item(nCol)
        '            Next
        '            rs.Update()
        '        Next
        '        Return rs
        '    Catch ex As Exception
        '        MsgBox("Error: " & ex.ToString, MsgBoxStyle.Critical)
        '        Return Nothing
        '    End Try
        'End Function
    End Module
End Namespace

