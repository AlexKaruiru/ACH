Imports BrClearing.Common
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Module StartMod
    Sub Main()
        BRCTSScannerStartPoint()
    End Sub
    Public Sub BRCTSScannerStartPoint()
        Modscan.ReadTxtFile()
        ShowScan()
    End Sub
    Public Function ShowScan()
        Dim frmX As Int32 = 0
        Try
            Modscan.cDBType = 1
            Select Case Modscan.cDBType
                Case 0 'Oracle
                    Modscan.BRDbType = Modscan.systemDbTypes.dbTypeOracle
                Case 1 'SQL
                    Modscan.BRDbType = Modscan.systemDbTypes.dbTypeSql
                Case 2 'Access
                    Modscan.BRDbType = Modscan.systemDbTypes.dbTypeAccess
                Case 3 'MySQL
                    Modscan.BRDbType = Modscan.systemDbTypes.dbTypeMySql
            End Select
            If Modscan.cScan = Modscan.ENUM_Module_Called.Outward_scan Then 'Outward Scan Module
                If EnsureSingleInstance() Then
                    MessageBox.Show("Already opened await to scan")
                Else
                    Dim frm As New frmBROutwardClearingCTS
                    'frm.TopMost = True
                    frm.ShowDialog()
                End If


            ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.Search_Module Then ' Search Module
                Dim frm As New frmBRChequesSearchClearing
                frm.ShowDialog()
            ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.Inward_scan Then 'Inwards Screen Module
                ' '' '' '' ''Dim frm As New frmBRInwardClearing
                ' '' '' '' ''frm.ShowDialog()
            ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.Display_Signature Then 'Inwards Screen Module
                Dim frm As New frmBRImageNSignatureView
                frm.ShowDialog()
            ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.Generate_OutFile Then
                Dim frm As New FrmBROutFile
                frm.ShowDialog()
                'ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.Read_IncomingFiles Then
                '    Dim frm As New frmIncoming
                '    frm.ShowDialog()
            ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.Unpay Then ' Search Module
                Dim frm As New frmBRChequesSearchClearing
                frm.ShowDialog()
            ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.Represent_Cheque Then ' Search Module
                Dim frm As New frmBRChequesSearchClearing
                frm.ShowDialog()
            ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.Sign_The_File Then ' Search Module
                Dim frm As New frmEncryptEFT
                frm.ShowDialog()
            ElseIf Modscan.cScan = Modscan.ENUM_Module_Called.View_Mandate_Images Then ' Search Module
                Dim frm As New frmViewDDMandateImage
                frm.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return frmX
    End Function

    Private Function EnsureSingleInstance() As Boolean
        Try
            If UBound(Diagnostics.Process.GetProcessesByName(Diagnostics.Process.GetCurrentProcess.ProcessName)) > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
       
    End Function

End Module
