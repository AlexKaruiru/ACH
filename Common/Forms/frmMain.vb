
Imports BrClearing.Common.Modscan

Public Class frmMain

    Private Sub btnOutScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutScan.Click
        cScan = 0
        formCaller()
    End Sub

    Private Sub btnInScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInScan.Click
        cScan = 1
        formCaller()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        cScan = 2
        formCaller()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Windows.Forms.Application.Exit()
    End Sub
    Public Sub Startup()

        Select Case cDBType.ToUpper
            Case "SQL"
                cDBType = 1
            Case "ORACLE"
                cDBType = 0
        End Select
        Select Case cDBType
            Case 0 'Oracle
                BRDbType = systemDbTypes.dbTypeOracle
            Case 1 'SQL
                BRDbType = systemDbTypes.dbTypeSql
            Case 2 'Access
                BRDbType = systemDbTypes.dbTypeAccess
            Case 3 'MySQL
                BRDbType = systemDbTypes.dbTypeMySql
        End Select

    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Startup()
    End Sub

    Private Sub formCaller()
        Me.Visible = False
        If cScan = ENUM_Module_Called.Outward_scan Then 'Outward Scan Module 0
            '' '' '' ''Dim frm As New frmBROutwardClearing
            '' '' '' ''frm.Show()
        ElseIf cScan = ENUM_Module_Called.Search_Module Then ' Search Module 2
            Dim frm As New frmBRChequesSearchClearing
            frm.Show()
        ElseIf cScan = ENUM_Module_Called.Inward_scan Then 'Inwards Screen Module 1
            '' '' '' ''Dim frm As New frmBRInwardClearing
            '' '' '' ''frm.Show()
        ElseIf cScan = ENUM_Module_Called.Display_Signature Then 'Inwards Screen Module 2
            Dim frm As New frmBRImageNSignatureView
            frm.Show()
        End If
    End Sub
End Class