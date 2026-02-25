
Imports System.IO
Imports System.Xml.Serialization
Imports System.ComponentModel


Namespace BRISO20022CT816 'Credit Transfer 816
#Region "Base entity class"
    Partial Public Class EntityBase(Of T)

        Private Shared sSerializer As System.Xml.Serialization.XmlSerializer

        Private Shared ReadOnly Property Serializer() As System.Xml.Serialization.XmlSerializer
            Get
                If (sSerializer Is Nothing) Then
                    sSerializer = New System.Xml.Serialization.XmlSerializer(GetType(T))
                End If
                Return sSerializer
            End Get
        End Property

#Region "Serialize/Deserialize"
        '''<summary>
        '''Serializes current EntityBase object into an XML document
        '''</summary>
        '''<returns>string XML value</returns>
        Public Overridable Function Serialize() As String
            Dim streamReader As System.IO.StreamReader = Nothing
            Dim memoryStream As System.IO.MemoryStream = Nothing
            Try
                memoryStream = New System.IO.MemoryStream
                Serializer.Serialize(memoryStream, Me)
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin)
                streamReader = New System.IO.StreamReader(memoryStream)
                Return streamReader.ReadToEnd
            Finally
                If (Not (streamReader) Is Nothing) Then
                    streamReader.Dispose()
                End If
                If (Not (memoryStream) Is Nothing) Then
                    memoryStream.Dispose()
                End If
            End Try
        End Function

        '''<summary>
        '''Deserializes workflow markup into an EntityBase object
        '''</summary>
        '''<param name="xml">string workflow markup to deserialize</param>
        '''<param name="obj">Output EntityBase object</param>
        '''<param name="exception">output Exception value if deserialize failed</param>
        '''<returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        Public Overloads Shared Function Deserialize(ByVal xml As String, ByRef obj As T, ByRef exception As System.Exception) As Boolean
            exception = Nothing
            obj = CType(Nothing, T)
            Try
                obj = Deserialize(xml)
                Return True
            Catch ex As System.Exception
                exception = ex
                Return False
            End Try
        End Function

        Public Overloads Shared Function Deserialize(ByVal xml As String, ByRef obj As T) As Boolean
            Dim exception As System.Exception = Nothing
            Return Deserialize(xml, obj, exception)
        End Function

        Public Overloads Shared Function Deserialize(ByVal xml As String) As T
            Dim stringReader As System.IO.StringReader = Nothing
            Try
                stringReader = New System.IO.StringReader(xml)
                Return CType(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader)), T)
            Finally
                If (Not (stringReader) Is Nothing) Then
                    stringReader.Dispose()
                End If
            End Try
        End Function

        '''<summary>
        '''Serializes current EntityBase object into file
        '''</summary>
        '''<param name="fileName">full path of outupt xml file</param>
        '''<param name="exception">output Exception value if failed</param>
        '''<returns>true if can serialize and save into file; otherwise, false</returns>
        Public Overridable Overloads Function SaveToFile(ByVal fileName As String, ByRef exception As System.Exception) As Boolean
            exception = Nothing
            Try
                SaveToFile(fileName)
                Return True
            Catch e As System.Exception
                exception = e
                Return False
            End Try
        End Function

        Public Overridable Overloads Sub SaveToFile(ByVal fileName As String)
            Dim streamWriter As System.IO.StreamWriter = Nothing
            Try
                Dim xmlString As String = Serialize()
                Dim xmlFile As System.IO.FileInfo = New System.IO.FileInfo(fileName)
                streamWriter = xmlFile.CreateText
                streamWriter.WriteLine(xmlString)
                streamWriter.Close()
            Finally
                If (Not (streamWriter) Is Nothing) Then
                    streamWriter.Dispose()
                End If
            End Try
        End Sub

        '''<summary>
        '''Deserializes xml markup from file into an EntityBase object
        '''</summary>
        '''<param name="fileName">string xml file to load and deserialize</param>
        '''<param name="obj">Output EntityBase object</param>
        '''<param name="exception">output Exception value if deserialize failed</param>
        '''<returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        Public Overloads Shared Function LoadFromFile(ByVal fileName As String, ByRef obj As T, ByRef exception As System.Exception) As Boolean
            exception = Nothing
            obj = CType(Nothing, T)
            Try
                obj = LoadFromFile(fileName)
                Return True
            Catch ex As System.Exception
                exception = ex
                Return False
            End Try
        End Function

        Public Overloads Shared Function LoadFromFile(ByVal fileName As String, ByRef obj As T) As Boolean
            Dim exception As System.Exception = Nothing
            Return LoadFromFile(fileName, obj, exception)
        End Function

        Public Overloads Shared Function LoadFromFile(ByVal fileName As String) As T
            Dim file As System.IO.FileStream = Nothing
            Dim sr As System.IO.StreamReader = Nothing
            Try
                file = New System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read)
                sr = New System.IO.StreamReader(file)
                Dim xmlString As String = sr.ReadToEnd
                sr.Close()
                file.Close()
                Return Deserialize(xmlString)
            Finally
                If (Not (file) Is Nothing) Then
                    file.Dispose()
                End If
                If (Not (sr) Is Nothing) Then
                    sr.Dispose()
                End If
            End Try
        End Function
#End Region
    End Class
#End Region

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IsNullable:=False)> _
    Partial Public Class Document
        Inherits EntityBase(Of Document)
        Private fIToFICstmrCdtTrfField As FIToFICustomerCreditTransferV06

        Public Sub New()
            MyBase.New()
            Me.FIToFICstmrCdtTrf = New FIToFICustomerCreditTransferV06
        End Sub
        ''' <remarks/>
        Public Property FIToFICstmrCdtTrf() As FIToFICustomerCreditTransferV06
            Get
                Return Me.fIToFICstmrCdtTrfField
            End Get
            Set(ByVal value As FIToFICustomerCreditTransferV06)
                Me.fIToFICstmrCdtTrfField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class FIToFICustomerCreditTransferV06

        Private grpHdrField As GroupHeader70

        Private cdtTrfTxInfField As List(Of CreditTransferTransaction25)

        Private splmtryDataField As SupplementaryData1()

        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdtTrfTxInfField = New List(Of CreditTransferTransaction25)
            Me.grpHdrField = New GroupHeader70
        End Sub
        ''' <remarks/>
        Public Property GrpHdr() As GroupHeader70
            Get
                Return Me.grpHdrField
            End Get
            Set(value As GroupHeader70)
                Me.grpHdrField = value
            End Set
        End Property
       
        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CdtTrfTxInf")> _
        Public Property CdtTrfTxInf() As List(Of CreditTransferTransaction25)
            Get
                Return Me.cdtTrfTxInfField
            End Get
            Set(ByVal value As List(Of CreditTransferTransaction25))
                Me.cdtTrfTxInfField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SplmtryData")> _
        Public Property SplmtryData() As SupplementaryData1()
            Get
                Return Me.splmtryDataField
            End Get
            Set(value As SupplementaryData1())
                Me.splmtryDataField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class GroupHeader70

        Private msgIdField As String

        Private creDtTmField As System.DateTime

        Private btchBookgField As Boolean

        Private btchBookgFieldSpecified As Boolean

        Private nbOfTxsField As String

        Private ctrlSumField As Decimal

        Private ctrlSumFieldSpecified As Boolean

        Private ttlIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        Private intrBkSttlmDtField As System.DateTime

        Private intrBkSttlmDtFieldSpecified As Boolean

        Private sttlmInfField As SettlementInstruction4

        Private pmtTpInfField As PaymentTypeInformation21

        Private instgAgtField As BranchAndFinancialInstitutionIdentification5

        Private instdAgtField As BranchAndFinancialInstitutionIdentification5

        ''' <remarks/>
        Public Property MsgId() As String
            Get
                Return Me.msgIdField
            End Get
            Set(value As String)
                Me.msgIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CreDtTm() As System.DateTime
            Get
                Return Me.creDtTmField
            End Get
            Set(value As System.DateTime)
                Me.creDtTmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property BtchBookg() As Boolean
            Get
                Return Me.btchBookgField
            End Get
            Set(value As Boolean)
                Me.btchBookgField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property BtchBookgSpecified() As Boolean
            Get
                Return Me.btchBookgFieldSpecified
            End Get
            Set(value As Boolean)
                Me.btchBookgFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property NbOfTxs() As String
            Get
                Return Me.nbOfTxsField
            End Get
            Set(value As String)
                Me.nbOfTxsField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CtrlSum() As Decimal
            Get
                Return Me.ctrlSumField
            End Get
            Set(value As Decimal)
                Me.ctrlSumField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property CtrlSumSpecified() As Boolean
            Get
                Return Me.ctrlSumFieldSpecified
            End Get
            Set(value As Boolean)
                Me.ctrlSumFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TtlIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.ttlIntrBkSttlmAmtField
            End Get
            Set(value As ActiveCurrencyAndAmount)
                Me.ttlIntrBkSttlmAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("IntrBkSttlmDt", DataType:="date")> _
        Public Property InterTestDt() As System.DateTime
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(value As System.DateTime)
                Me.intrBkSttlmDtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property IntrBkSttlmDtSpecified() As Boolean
            Get
                Return Me.intrBkSttlmDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.intrBkSttlmDtFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SttlmInf() As SettlementInstruction4
            Get
                Return Me.sttlmInfField
            End Get
            Set(value As SettlementInstruction4)
                Me.sttlmInfField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PmtTpInf() As PaymentTypeInformation21
            Get
                Return Me.pmtTpInfField
            End Get
            Set(value As PaymentTypeInformation21)
                Me.pmtTpInfField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstgAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instgAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instgAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstdAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instdAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instdAgtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ActiveCurrencyAndAmount

        Private ccyField As String

        Private valueField As Decimal

        ''' <remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute> _
        Public Property Ccy() As String
            Get
                Return Me.ccyField
            End Get
            Set(value As String)
                Me.ccyField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlTextAttribute> _
        Public Property Value() As Decimal
            Get
                Return Me.valueField
            End Get
            Set(value As Decimal)
                Me.valueField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class SupplementaryData1

        Private plcAndNmField As String

        Private envlpField As System.Xml.XmlElement

        ''' <remarks/>
        Public Property PlcAndNm() As String
            Get
                Return Me.plcAndNmField
            End Get
            Set(value As String)
                Me.plcAndNmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Envlp() As System.Xml.XmlElement
            Get
                Return Me.envlpField
            End Get
            Set(value As System.Xml.XmlElement)
                Me.envlpField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class GarnishmentType1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType14

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType14
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType14)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType14

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class GarnishmentType1

        Private cdOrPrtryField As GarnishmentType1Choice

        Private issrField As String

        ''' <remarks/>
        Public Property CdOrPrtry() As GarnishmentType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(value As GarnishmentType1Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class Garnishment1

        Private tpField As GarnishmentType1

        Private grnsheeField As PartyIdentification43

        Private grnshmtAdmstrField As PartyIdentification43

        Private refNbField As String

        Private dtField As System.DateTime

        Private dtFieldSpecified As Boolean

        Private rmtdAmtField As ActiveOrHistoricCurrencyAndAmount

        Private fmlyMdclInsrncIndField As Boolean

        Private fmlyMdclInsrncIndFieldSpecified As Boolean

        Private mplyeeTermntnIndField As Boolean

        Private mplyeeTermntnIndFieldSpecified As Boolean

        ''' <remarks/>
        Public Property Tp() As GarnishmentType1
            Get
                Return Me.tpField
            End Get
            Set(value As GarnishmentType1)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Grnshee() As PartyIdentification43
            Get
                Return Me.grnsheeField
            End Get
            Set(value As PartyIdentification43)
                Me.grnsheeField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property GrnshmtAdmstr() As PartyIdentification43
            Get
                Return Me.grnshmtAdmstrField
            End Get
            Set(value As PartyIdentification43)
                Me.grnshmtAdmstrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RefNb() As String
            Get
                Return Me.refNbField
            End Get
            Set(value As String)
                Me.refNbField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property Dt() As System.DateTime
            Get
                Return Me.dtField
            End Get
            Set(value As System.DateTime)
                Me.dtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property DtSpecified() As Boolean
            Get
                Return Me.dtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dtFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RmtdAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.rmtdAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.rmtdAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property FmlyMdclInsrncInd() As Boolean
            Get
                Return Me.fmlyMdclInsrncIndField
            End Get
            Set(value As Boolean)
                Me.fmlyMdclInsrncIndField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property FmlyMdclInsrncIndSpecified() As Boolean
            Get
                Return Me.fmlyMdclInsrncIndFieldSpecified
            End Get
            Set(value As Boolean)
                Me.fmlyMdclInsrncIndFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property MplyeeTermntnInd() As Boolean
            Get
                Return Me.mplyeeTermntnIndField
            End Get
            Set(value As Boolean)
                Me.mplyeeTermntnIndField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property MplyeeTermntnIndSpecified() As Boolean
            Get
                Return Me.mplyeeTermntnIndFieldSpecified
            End Get
            Set(value As Boolean)
                Me.mplyeeTermntnIndFieldSpecified = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class PartyIdentification43

        Private nmField As String

        Private pstlAdrField As PostalAddress6

        Private idField As Party11Choice

        Private ctryOfResField As String

        Private ctctDtlsField As ContactDetails2

        ''' <remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PstlAdr() As PostalAddress6
            Get
                Return Me.pstlAdrField
            End Get
            Set(value As PostalAddress6)
                Me.pstlAdrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Id() As Party11Choice
            Get
                Return Me.idField
            End Get
            Set(value As Party11Choice)
                Me.idField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CtryOfRes() As String
            Get
                Return Me.ctryOfResField
            End Get
            Set(value As String)
                Me.ctryOfResField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CtctDtls() As ContactDetails2
            Get
                Return Me.ctctDtlsField
            End Get
            Set(value As ContactDetails2)
                Me.ctctDtlsField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class PostalAddress6

        Private adrTpField As AddressType2Code

        Private adrTpFieldSpecified As Boolean

        Private deptField As String

        Private subDeptField As String

        Private strtNmField As String

        Private bldgNbField As String

        Private pstCdField As String

        Private twnNmField As String

        Private ctrySubDvsnField As String

        Private ctryField As String

        Private adrLineField As String()

        ''' <remarks/>
        Public Property AdrTp() As AddressType2Code
            Get
                Return Me.adrTpField
            End Get
            Set(value As AddressType2Code)
                Me.adrTpField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property AdrTpSpecified() As Boolean
            Get
                Return Me.adrTpFieldSpecified
            End Get
            Set(value As Boolean)
                Me.adrTpFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Dept() As String
            Get
                Return Me.deptField
            End Get
            Set(value As String)
                Me.deptField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SubDept() As String
            Get
                Return Me.subDeptField
            End Get
            Set(value As String)
                Me.subDeptField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property StrtNm() As String
            Get
                Return Me.strtNmField
            End Get
            Set(value As String)
                Me.strtNmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property BldgNb() As String
            Get
                Return Me.bldgNbField
            End Get
            Set(value As String)
                Me.bldgNbField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PstCd() As String
            Get
                Return Me.pstCdField
            End Get
            Set(value As String)
                Me.pstCdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TwnNm() As String
            Get
                Return Me.twnNmField
            End Get
            Set(value As String)
                Me.twnNmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CtrySubDvsn() As String
            Get
                Return Me.ctrySubDvsnField
            End Get
            Set(value As String)
                Me.ctrySubDvsnField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(value As String)
                Me.ctryField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdrLine")> _
        Public Property AdrLine() As String()
            Get
                Return Me.adrLineField
            End Get
            Set(value As String())
                Me.adrLineField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum AddressType2Code

        ''' <remarks/>
        ADDR

        ''' <remarks/>
        PBOX

        ''' <remarks/>
        HOME

        ''' <remarks/>
        BIZZ

        ''' <remarks/>
        MLTO

        ''' <remarks/>
        DLVY
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class Party11Choice

        Private itemField As Object

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OrgId", GetType(OrganisationIdentification8))> _
        <System.Xml.Serialization.XmlElementAttribute("PrvtId", GetType(PersonIdentification5))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class OrganisationIdentification8

        Private anyBICField As String

        Private othrField As GenericOrganisationIdentification1()

        ''' <remarks/>
        Public Property AnyBIC() As String
            Get
                Return Me.anyBICField
            End Get
            Set(value As String)
                Me.anyBICField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Othr")> _
        Public Property Othr() As GenericOrganisationIdentification1()
            Get
                Return Me.othrField
            End Get
            Set(value As GenericOrganisationIdentification1())
                Me.othrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class GenericOrganisationIdentification1

        Private idField As String

        Private schmeNmField As OrganisationIdentificationSchemeName1Choice

        Private issrField As String

        ''' <remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SchmeNm() As OrganisationIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(value As OrganisationIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class OrganisationIdentificationSchemeName1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType8

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType8
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType8)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType8

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class PersonIdentification5

        Private dtAndPlcOfBirthField As DateAndPlaceOfBirth

        Private othrField As GenericPersonIdentification1()

        ''' <remarks/>
        Public Property DtAndPlcOfBirth() As DateAndPlaceOfBirth
            Get
                Return Me.dtAndPlcOfBirthField
            End Get
            Set(value As DateAndPlaceOfBirth)
                Me.dtAndPlcOfBirthField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Othr")> _
        Public Property Othr() As GenericPersonIdentification1()
            Get
                Return Me.othrField
            End Get
            Set(value As GenericPersonIdentification1())
                Me.othrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DateAndPlaceOfBirth

        Private birthDtField As System.DateTime

        Private prvcOfBirthField As String

        Private cityOfBirthField As String

        Private ctryOfBirthField As String

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property BirthDt() As System.DateTime
            Get
                Return Me.birthDtField
            End Get
            Set(value As System.DateTime)
                Me.birthDtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PrvcOfBirth() As String
            Get
                Return Me.prvcOfBirthField
            End Get
            Set(value As String)
                Me.prvcOfBirthField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CityOfBirth() As String
            Get
                Return Me.cityOfBirthField
            End Get
            Set(value As String)
                Me.cityOfBirthField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CtryOfBirth() As String
            Get
                Return Me.ctryOfBirthField
            End Get
            Set(value As String)
                Me.ctryOfBirthField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class GenericPersonIdentification1

        Private idField As String

        Private schmeNmField As PersonIdentificationSchemeName1Choice

        Private issrField As String

        ''' <remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SchmeNm() As PersonIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(value As PersonIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class PersonIdentificationSchemeName1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType9

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType9
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType9)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType9

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ContactDetails2

        Private nmPrfxField As NamePrefix1Code

        Private nmPrfxFieldSpecified As Boolean

        Private nmField As String

        Private phneNbField As String

        Private mobNbField As String

        Private faxNbField As String

        Private emailAdrField As String

        Private othrField As String

        ''' <remarks/>
        Public Property NmPrfx() As NamePrefix1Code
            Get
                Return Me.nmPrfxField
            End Get
            Set(value As NamePrefix1Code)
                Me.nmPrfxField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property NmPrfxSpecified() As Boolean
            Get
                Return Me.nmPrfxFieldSpecified
            End Get
            Set(value As Boolean)
                Me.nmPrfxFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PhneNb() As String
            Get
                Return Me.phneNbField
            End Get
            Set(value As String)
                Me.phneNbField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property MobNb() As String
            Get
                Return Me.mobNbField
            End Get
            Set(value As String)
                Me.mobNbField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property FaxNb() As String
            Get
                Return Me.faxNbField
            End Get
            Set(value As String)
                Me.faxNbField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property EmailAdr() As String
            Get
                Return Me.emailAdrField
            End Get
            Set(value As String)
                Me.emailAdrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Othr() As String
            Get
                Return Me.othrField
            End Get
            Set(value As String)
                Me.othrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum NamePrefix1Code

        ''' <remarks/>
        DOCT

        ''' <remarks/>
        MIST

        ''' <remarks/>
        MISS

        ''' <remarks/>
        MADM
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ActiveOrHistoricCurrencyAndAmount

        Private ccyField As String

        Private valueField As Decimal

        ''' <remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute> _
        Public Property Ccy() As String
            Get
                Return Me.ccyField
            End Get
            Set(value As String)
                Me.ccyField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlTextAttribute> _
        Public Property Value() As Decimal
            Get
                Return Me.valueField
            End Get
            Set(value As Decimal)
                Me.valueField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxInformation4

        Private cdtrField As TaxParty1

        Private dbtrField As TaxParty2

        Private ultmtDbtrField As TaxParty2

        Private admstnZoneField As String

        Private refNbField As String

        Private mtdField As String

        Private ttlTaxblBaseAmtField As ActiveOrHistoricCurrencyAndAmount

        Private ttlTaxAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dtField As System.DateTime

        Private dtFieldSpecified As Boolean

        Private seqNbField As Decimal

        Private seqNbFieldSpecified As Boolean

        Private rcrdField As TaxRecord1()

        ''' <remarks/>
        Public Property Cdtr() As TaxParty1
            Get
                Return Me.cdtrField
            End Get
            Set(value As TaxParty1)
                Me.cdtrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Dbtr() As TaxParty2
            Get
                Return Me.dbtrField
            End Get
            Set(value As TaxParty2)
                Me.dbtrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property UltmtDbtr() As TaxParty2
            Get
                Return Me.ultmtDbtrField
            End Get
            Set(value As TaxParty2)
                Me.ultmtDbtrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property AdmstnZone() As String
            Get
                Return Me.admstnZoneField
            End Get
            Set(value As String)
                Me.admstnZoneField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RefNb() As String
            Get
                Return Me.refNbField
            End Get
            Set(value As String)
                Me.refNbField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Mtd() As String
            Get
                Return Me.mtdField
            End Get
            Set(value As String)
                Me.mtdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TtlTaxblBaseAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.ttlTaxblBaseAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.ttlTaxblBaseAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TtlTaxAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.ttlTaxAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.ttlTaxAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property Dt() As System.DateTime
            Get
                Return Me.dtField
            End Get
            Set(value As System.DateTime)
                Me.dtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property DtSpecified() As Boolean
            Get
                Return Me.dtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dtFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SeqNb() As Decimal
            Get
                Return Me.seqNbField
            End Get
            Set(value As Decimal)
                Me.seqNbField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property SeqNbSpecified() As Boolean
            Get
                Return Me.seqNbFieldSpecified
            End Get
            Set(value As Boolean)
                Me.seqNbFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Rcrd")> _
        Public Property Rcrd() As TaxRecord1()
            Get
                Return Me.rcrdField
            End Get
            Set(value As TaxRecord1())
                Me.rcrdField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxParty1

        Private taxIdField As String

        Private regnIdField As String

        Private taxTpField As String

        ''' <remarks/>
        Public Property TaxId() As String
            Get
                Return Me.taxIdField
            End Get
            Set(value As String)
                Me.taxIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RegnId() As String
            Get
                Return Me.regnIdField
            End Get
            Set(value As String)
                Me.regnIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TaxTp() As String
            Get
                Return Me.taxTpField
            End Get
            Set(value As String)
                Me.taxTpField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxParty2

        Private taxIdField As String

        Private regnIdField As String

        Private taxTpField As String

        Private authstnField As TaxAuthorisation1

        ''' <remarks/>
        Public Property TaxId() As String
            Get
                Return Me.taxIdField
            End Get
            Set(value As String)
                Me.taxIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RegnId() As String
            Get
                Return Me.regnIdField
            End Get
            Set(value As String)
                Me.regnIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TaxTp() As String
            Get
                Return Me.taxTpField
            End Get
            Set(value As String)
                Me.taxTpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Authstn() As TaxAuthorisation1
            Get
                Return Me.authstnField
            End Get
            Set(value As TaxAuthorisation1)
                Me.authstnField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxAuthorisation1

        Private titlField As String

        Private nmField As String

        ''' <remarks/>
        Public Property Titl() As String
            Get
                Return Me.titlField
            End Get
            Set(value As String)
                Me.titlField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxRecord1

        Private tpField As String

        Private ctgyField As String

        Private ctgyDtlsField As String

        Private dbtrStsField As String

        Private certIdField As String

        Private frmsCdField As String

        Private prdField As TaxPeriod1

        Private taxAmtField As TaxAmount1

        Private addtlInfField As String

        ''' <remarks/>
        Public Property Tp() As String
            Get
                Return Me.tpField
            End Get
            Set(value As String)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Ctgy() As String
            Get
                Return Me.ctgyField
            End Get
            Set(value As String)
                Me.ctgyField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CtgyDtls() As String
            Get
                Return Me.ctgyDtlsField
            End Get
            Set(value As String)
                Me.ctgyDtlsField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property DbtrSts() As String
            Get
                Return Me.dbtrStsField
            End Get
            Set(value As String)
                Me.dbtrStsField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CertId() As String
            Get
                Return Me.certIdField
            End Get
            Set(value As String)
                Me.certIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property FrmsCd() As String
            Get
                Return Me.frmsCdField
            End Get
            Set(value As String)
                Me.frmsCdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Prd() As TaxPeriod1
            Get
                Return Me.prdField
            End Get
            Set(value As TaxPeriod1)
                Me.prdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TaxAmt() As TaxAmount1
            Get
                Return Me.taxAmtField
            End Get
            Set(value As TaxAmount1)
                Me.taxAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property AddtlInf() As String
            Get
                Return Me.addtlInfField
            End Get
            Set(value As String)
                Me.addtlInfField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxPeriod1

        Private yrField As System.DateTime

        Private yrFieldSpecified As Boolean

        Private tpField As TaxRecordPeriod1Code

        Private tpFieldSpecified As Boolean

        Private frToDtField As DatePeriodDetails

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property Yr() As System.DateTime
            Get
                Return Me.yrField
            End Get
            Set(value As System.DateTime)
                Me.yrField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property YrSpecified() As Boolean
            Get
                Return Me.yrFieldSpecified
            End Get
            Set(value As Boolean)
                Me.yrFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Tp() As TaxRecordPeriod1Code
            Get
                Return Me.tpField
            End Get
            Set(value As TaxRecordPeriod1Code)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property TpSpecified() As Boolean
            Get
                Return Me.tpFieldSpecified
            End Get
            Set(value As Boolean)
                Me.tpFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property FrToDt() As DatePeriodDetails
            Get
                Return Me.frToDtField
            End Get
            Set(value As DatePeriodDetails)
                Me.frToDtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum TaxRecordPeriod1Code

        ''' <remarks/>
        MM01

        ''' <remarks/>
        MM02

        ''' <remarks/>
        MM03

        ''' <remarks/>
        MM04

        ''' <remarks/>
        MM05

        ''' <remarks/>
        MM06

        ''' <remarks/>
        MM07

        ''' <remarks/>
        MM08

        ''' <remarks/>
        MM09

        ''' <remarks/>
        MM10

        ''' <remarks/>
        MM11

        ''' <remarks/>
        MM12

        ''' <remarks/>
        QTR1

        ''' <remarks/>
        QTR2

        ''' <remarks/>
        QTR3

        ''' <remarks/>
        QTR4

        ''' <remarks/>
        HLF1

        ''' <remarks/>
        HLF2
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DatePeriodDetails

        Private frDtField As System.DateTime

        Private toDtField As System.DateTime

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property FrDt() As System.DateTime
            Get
                Return Me.frDtField
            End Get
            Set(value As System.DateTime)
                Me.frDtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property ToDt() As System.DateTime
            Get
                Return Me.toDtField
            End Get
            Set(value As System.DateTime)
                Me.toDtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxAmount1

        Private rateField As Decimal

        Private rateFieldSpecified As Boolean

        Private taxblBaseAmtField As ActiveOrHistoricCurrencyAndAmount

        Private ttlAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dtlsField As TaxRecordDetails1()

        ''' <remarks/>
        Public Property Rate() As Decimal
            Get
                Return Me.rateField
            End Get
            Set(value As Decimal)
                Me.rateField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property RateSpecified() As Boolean
            Get
                Return Me.rateFieldSpecified
            End Get
            Set(value As Boolean)
                Me.rateFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TaxblBaseAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.taxblBaseAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.taxblBaseAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TtlAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.ttlAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.ttlAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Dtls")> _
        Public Property Dtls() As TaxRecordDetails1()
            Get
                Return Me.dtlsField
            End Get
            Set(value As TaxRecordDetails1())
                Me.dtlsField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxRecordDetails1

        Private prdField As TaxPeriod1

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        ''' <remarks/>
        Public Property Prd() As TaxPeriod1
            Get
                Return Me.prdField
            End Get
            Set(value As TaxPeriod1)
                Me.prdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class CreditorReferenceType1Choice

        Private itemField As Object

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(DocumentType3Code))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum DocumentType3Code

        ''' <remarks/>
        RADM

        ''' <remarks/>
        RPIN

        ''' <remarks/>
        FXDR

        ''' <remarks/>
        DISP

        ''' <remarks/>
        PUOR

        ''' <remarks/>
        SCOR
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class CreditorReferenceType2

        Private cdOrPrtryField As CreditorReferenceType1Choice

        Private issrField As String

        ''' <remarks/>
        Public Property CdOrPrtry() As CreditorReferenceType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(value As CreditorReferenceType1Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class CreditorReferenceInformation2

        Private tpField As CreditorReferenceType2

        Private refField As String

        ''' <remarks/>
        Public Property Tp() As CreditorReferenceType2
            Get
                Return Me.tpField
            End Get
            Set(value As CreditorReferenceType2)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Ref() As String
            Get
                Return Me.refField
            End Get
            Set(value As String)
                Me.refField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class RemittanceAmount2

        Private duePyblAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dscntApldAmtField As DiscountAmountAndType1()

        Private cdtNoteAmtField As ActiveOrHistoricCurrencyAndAmount

        Private taxAmtField As TaxAmountAndType1()

        Private adjstmntAmtAndRsnField As DocumentAdjustment1()

        Private rmtdAmtField As ActiveOrHistoricCurrencyAndAmount

        ''' <remarks/>
        Public Property DuePyblAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.duePyblAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.duePyblAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DscntApldAmt")> _
        Public Property DscntApldAmt() As DiscountAmountAndType1()
            Get
                Return Me.dscntApldAmtField
            End Get
            Set(value As DiscountAmountAndType1())
                Me.dscntApldAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CdtNoteAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.cdtNoteAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.cdtNoteAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TaxAmt")> _
        Public Property TaxAmt() As TaxAmountAndType1()
            Get
                Return Me.taxAmtField
            End Get
            Set(value As TaxAmountAndType1())
                Me.taxAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdjstmntAmtAndRsn")> _
        Public Property AdjstmntAmtAndRsn() As DocumentAdjustment1()
            Get
                Return Me.adjstmntAmtAndRsnField
            End Get
            Set(value As DocumentAdjustment1())
                Me.adjstmntAmtAndRsnField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RmtdAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.rmtdAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.rmtdAmtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DiscountAmountAndType1

        Private tpField As DiscountAmountType1Choice

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        ''' <remarks/>
        Public Property Tp() As DiscountAmountType1Choice
            Get
                Return Me.tpField
            End Get
            Set(value As DiscountAmountType1Choice)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DiscountAmountType1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType12

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType12
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType12)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType12

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxAmountAndType1

        Private tpField As TaxAmountType1Choice

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        ''' <remarks/>
        Public Property Tp() As TaxAmountType1Choice
            Get
                Return Me.tpField
            End Get
            Set(value As TaxAmountType1Choice)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxAmountType1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType13

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType13
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType13)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType13

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DocumentAdjustment1

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        Private cdtDbtIndField As CreditDebitCode

        Private cdtDbtIndFieldSpecified As Boolean

        Private rsnField As String

        Private addtlInfField As String

        ''' <remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CdtDbtInd() As CreditDebitCode
            Get
                Return Me.cdtDbtIndField
            End Get
            Set(value As CreditDebitCode)
                Me.cdtDbtIndField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property CdtDbtIndSpecified() As Boolean
            Get
                Return Me.cdtDbtIndFieldSpecified
            End Get
            Set(value As Boolean)
                Me.cdtDbtIndFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Rsn() As String
            Get
                Return Me.rsnField
            End Get
            Set(value As String)
                Me.rsnField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property AddtlInf() As String
            Get
                Return Me.addtlInfField
            End Get
            Set(value As String)
                Me.addtlInfField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum CreditDebitCode

        ''' <remarks/>
        CRDT

        ''' <remarks/>
        DBIT
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class RemittanceAmount3

        Private duePyblAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dscntApldAmtField As DiscountAmountAndType1()

        Private cdtNoteAmtField As ActiveOrHistoricCurrencyAndAmount

        Private taxAmtField As TaxAmountAndType1()

        Private adjstmntAmtAndRsnField As DocumentAdjustment1()

        Private rmtdAmtField As ActiveOrHistoricCurrencyAndAmount

        ''' <remarks/>
        Public Property DuePyblAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.duePyblAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.duePyblAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DscntApldAmt")> _
        Public Property DscntApldAmt() As DiscountAmountAndType1()
            Get
                Return Me.dscntApldAmtField
            End Get
            Set(value As DiscountAmountAndType1())
                Me.dscntApldAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CdtNoteAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.cdtNoteAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.cdtNoteAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TaxAmt")> _
        Public Property TaxAmt() As TaxAmountAndType1()
            Get
                Return Me.taxAmtField
            End Get
            Set(value As TaxAmountAndType1())
                Me.taxAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdjstmntAmtAndRsn")> _
        Public Property AdjstmntAmtAndRsn() As DocumentAdjustment1()
            Get
                Return Me.adjstmntAmtAndRsnField
            End Get
            Set(value As DocumentAdjustment1())
                Me.adjstmntAmtAndRsnField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RmtdAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.rmtdAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.rmtdAmtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DocumentLineType1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType11

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType11
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType11)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType11

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DocumentLineType1

        Private cdOrPrtryField As DocumentLineType1Choice

        Private issrField As String

        ''' <remarks/>
        Public Property CdOrPrtry() As DocumentLineType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(value As DocumentLineType1Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DocumentLineIdentification1

        Private tpField As DocumentLineType1

        Private nbField As String

        Private rltdDtField As System.DateTime

        Private rltdDtFieldSpecified As Boolean

        ''' <remarks/>
        Public Property Tp() As DocumentLineType1
            Get
                Return Me.tpField
            End Get
            Set(value As DocumentLineType1)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Nb() As String
            Get
                Return Me.nbField
            End Get
            Set(value As String)
                Me.nbField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property RltdDt() As System.DateTime
            Get
                Return Me.rltdDtField
            End Get
            Set(value As System.DateTime)
                Me.rltdDtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property RltdDtSpecified() As Boolean
            Get
                Return Me.rltdDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.rltdDtFieldSpecified = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class DocumentLineInformation1

        Private idField As DocumentLineIdentification1()

        Private descField As String

        Private amtField As RemittanceAmount3

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Id")> _
        Public Property Id() As DocumentLineIdentification1()
            Get
                Return Me.idField
            End Get
            Set(value As DocumentLineIdentification1())
                Me.idField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Desc() As String
            Get
                Return Me.descField
            End Get
            Set(value As String)
                Me.descField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Amt() As RemittanceAmount3
            Get
                Return Me.amtField
            End Get
            Set(value As RemittanceAmount3)
                Me.amtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ReferredDocumentType3Choice

        Private itemField As Object

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(DocumentType6Code))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum DocumentType6Code

        ''' <remarks/>
        MSIN

        ''' <remarks/>
        CNFA

        ''' <remarks/>
        DNFA

        ''' <remarks/>
        CINV

        ''' <remarks/>
        CREN

        ''' <remarks/>
        DEBN

        ''' <remarks/>
        HIRI

        ''' <remarks/>
        SBIN

        ''' <remarks/>
        CMCN

        ''' <remarks/>
        SOAC

        ''' <remarks/>
        DISP

        ''' <remarks/>
        BOLD

        ''' <remarks/>
        VCHR

        ''' <remarks/>
        AROI

        ''' <remarks/>
        TSUT

        ''' <remarks/>
        PUOR
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ReferredDocumentType4

        Private cdOrPrtryField As ReferredDocumentType3Choice

        Private issrField As String

        ''' <remarks/>
        Public Property CdOrPrtry() As ReferredDocumentType3Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(value As ReferredDocumentType3Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ReferredDocumentInformation7

        Private tpField As ReferredDocumentType4

        Private nbField As String

        Private rltdDtField As System.DateTime

        Private rltdDtFieldSpecified As Boolean

        Private lineDtlsField As DocumentLineInformation1()

        ''' <remarks/>
        Public Property Tp() As ReferredDocumentType4
            Get
                Return Me.tpField
            End Get
            Set(value As ReferredDocumentType4)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Nb() As String
            Get
                Return Me.nbField
            End Get
            Set(value As String)
                Me.nbField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property RltdDt() As System.DateTime
            Get
                Return Me.rltdDtField
            End Get
            Set(value As System.DateTime)
                Me.rltdDtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property RltdDtSpecified() As Boolean
            Get
                Return Me.rltdDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.rltdDtFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LineDtls")> _
        Public Property LineDtls() As DocumentLineInformation1()
            Get
                Return Me.lineDtlsField
            End Get
            Set(value As DocumentLineInformation1())
                Me.lineDtlsField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class StructuredRemittanceInformation13

        Private rfrdDocInfField As ReferredDocumentInformation7()

        Private rfrdDocAmtField As RemittanceAmount2

        Private cdtrRefInfField As CreditorReferenceInformation2

        Private invcrField As PartyIdentification43

        Private invceeField As PartyIdentification43

        Private taxRmtField As TaxInformation4

        Private grnshmtRmtField As Garnishment1

        Private addtlRmtInfField As String()

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RfrdDocInf")> _
        Public Property RfrdDocInf() As ReferredDocumentInformation7()
            Get
                Return Me.rfrdDocInfField
            End Get
            Set(value As ReferredDocumentInformation7())
                Me.rfrdDocInfField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RfrdDocAmt() As RemittanceAmount2
            Get
                Return Me.rfrdDocAmtField
            End Get
            Set(value As RemittanceAmount2)
                Me.rfrdDocAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CdtrRefInf() As CreditorReferenceInformation2
            Get
                Return Me.cdtrRefInfField
            End Get
            Set(value As CreditorReferenceInformation2)
                Me.cdtrRefInfField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Invcr() As PartyIdentification43
            Get
                Return Me.invcrField
            End Get
            Set(value As PartyIdentification43)
                Me.invcrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Invcee() As PartyIdentification43
            Get
                Return Me.invceeField
            End Get
            Set(value As PartyIdentification43)
                Me.invceeField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TaxRmt() As TaxInformation4
            Get
                Return Me.taxRmtField
            End Get
            Set(value As TaxInformation4)
                Me.taxRmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property GrnshmtRmt() As Garnishment1
            Get
                Return Me.grnshmtRmtField
            End Get
            Set(value As Garnishment1)
                Me.grnshmtRmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddtlRmtInf")> _
        Public Property AddtlRmtInf() As String()
            Get
                Return Me.addtlRmtInfField
            End Get
            Set(value As String())
                Me.addtlRmtInfField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class RemittanceInformation11

        Private ustrdField As String()

        Private strdField As StructuredRemittanceInformation13()

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Ustrd")> _
        Public Property Ustrd() As String()
            Get
                Return Me.ustrdField
            End Get
            Set(value As String())
                Me.ustrdField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Strd")> _
        Public Property Strd() As StructuredRemittanceInformation13()
            Get
                Return Me.strdField
            End Get
            Set(value As StructuredRemittanceInformation13())
                Me.strdField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class NameAndAddress10

        Private nmField As String

        Private adrField As PostalAddress6

        ''' <remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Adr() As PostalAddress6
            Get
                Return Me.adrField
            End Get
            Set(value As PostalAddress6)
                Me.adrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class RemittanceLocationDetails1

        Private mtdField As RemittanceLocationMethod2Code

        Private elctrncAdrField As String

        Private pstlAdrField As NameAndAddress10

        ''' <remarks/>
        Public Property Mtd() As RemittanceLocationMethod2Code
            Get
                Return Me.mtdField
            End Get
            Set(value As RemittanceLocationMethod2Code)
                Me.mtdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property ElctrncAdr() As String
            Get
                Return Me.elctrncAdrField
            End Get
            Set(value As String)
                Me.elctrncAdrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PstlAdr() As NameAndAddress10
            Get
                Return Me.pstlAdrField
            End Get
            Set(value As NameAndAddress10)
                Me.pstlAdrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum RemittanceLocationMethod2Code

        ''' <remarks/>
        FAXI

        ''' <remarks/>
        EDIC

        ''' <remarks/>
        URID

        ''' <remarks/>
        EMAL

        ''' <remarks/>
        POST

        ''' <remarks/>
        SMSM
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class RemittanceLocation4

        Private rmtIdField As String

        Private rmtLctnDtlsField As RemittanceLocationDetails1()

        ''' <remarks/>
        Public Property RmtId() As String
            Get
                Return Me.rmtIdField
            End Get
            Set(value As String)
                Me.rmtIdField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RmtLctnDtls")> _
        Public Property RmtLctnDtls() As RemittanceLocationDetails1()
            Get
                Return Me.rmtLctnDtlsField
            End Get
            Set(value As RemittanceLocationDetails1())
                Me.rmtLctnDtlsField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class TaxInformation3

        Private cdtrField As TaxParty1

        Private dbtrField As TaxParty2

        Private admstnZnField As String

        Private refNbField As String

        Private mtdField As String

        Private ttlTaxblBaseAmtField As ActiveOrHistoricCurrencyAndAmount

        Private ttlTaxAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dtField As System.DateTime

        Private dtFieldSpecified As Boolean

        Private seqNbField As Decimal

        Private seqNbFieldSpecified As Boolean

        Private rcrdField As TaxRecord1()

        ''' <remarks/>
        Public Property Cdtr() As TaxParty1
            Get
                Return Me.cdtrField
            End Get
            Set(value As TaxParty1)
                Me.cdtrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Dbtr() As TaxParty2
            Get
                Return Me.dbtrField
            End Get
            Set(value As TaxParty2)
                Me.dbtrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property AdmstnZn() As String
            Get
                Return Me.admstnZnField
            End Get
            Set(value As String)
                Me.admstnZnField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RefNb() As String
            Get
                Return Me.refNbField
            End Get
            Set(value As String)
                Me.refNbField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Mtd() As String
            Get
                Return Me.mtdField
            End Get
            Set(value As String)
                Me.mtdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TtlTaxblBaseAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.ttlTaxblBaseAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.ttlTaxblBaseAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TtlTaxAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.ttlTaxAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.ttlTaxAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property Dt() As System.DateTime
            Get
                Return Me.dtField
            End Get
            Set(value As System.DateTime)
                Me.dtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property DtSpecified() As Boolean
            Get
                Return Me.dtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dtFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SeqNb() As Decimal
            Get
                Return Me.seqNbField
            End Get
            Set(value As Decimal)
                Me.seqNbField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property SeqNbSpecified() As Boolean
            Get
                Return Me.seqNbFieldSpecified
            End Get
            Set(value As Boolean)
                Me.seqNbFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Rcrd")> _
        Public Property Rcrd() As TaxRecord1()
            Get
                Return Me.rcrdField
            End Get
            Set(value As TaxRecord1())
                Me.rcrdField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class StructuredRegulatoryReporting3

        Private tpField As String

        Private dtField As System.DateTime

        Private dtFieldSpecified As Boolean

        Private ctryField As String

        Private cdField As String

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        Private infField As String()

        ''' <remarks/>
        Public Property Tp() As String
            Get
                Return Me.tpField
            End Get
            Set(value As String)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property Dt() As System.DateTime
            Get
                Return Me.dtField
            End Get
            Set(value As System.DateTime)
                Me.dtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property DtSpecified() As Boolean
            Get
                Return Me.dtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dtFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(value As String)
                Me.ctryField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Cd() As String
            Get
                Return Me.cdField
            End Get
            Set(value As String)
                Me.cdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Inf")> _
        Public Property Inf() As String()
            Get
                Return Me.infField
            End Get
            Set(value As String())
                Me.infField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class RegulatoryAuthority2

        Private nmField As String

        Private ctryField As String

        ''' <remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(value As String)
                Me.ctryField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class RegulatoryReporting3

        Private dbtCdtRptgIndField As RegulatoryReportingType1Code

        Private dbtCdtRptgIndFieldSpecified As Boolean

        Private authrtyField As RegulatoryAuthority2

        Private dtlsField As StructuredRegulatoryReporting3()

        ''' <remarks/>
        Public Property DbtCdtRptgInd() As RegulatoryReportingType1Code
            Get
                Return Me.dbtCdtRptgIndField
            End Get
            Set(value As RegulatoryReportingType1Code)
                Me.dbtCdtRptgIndField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property DbtCdtRptgIndSpecified() As Boolean
            Get
                Return Me.dbtCdtRptgIndFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dbtCdtRptgIndFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Authrty() As RegulatoryAuthority2
            Get
                Return Me.authrtyField
            End Get
            Set(value As RegulatoryAuthority2)
                Me.authrtyField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Dtls")> _
        Public Property Dtls() As StructuredRegulatoryReporting3()
            Get
                Return Me.dtlsField
            End Get
            Set(value As StructuredRegulatoryReporting3())
                Me.dtlsField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum RegulatoryReportingType1Code

        ''' <remarks/>
        CRED

        ''' <remarks/>
        DEBT

        ''' <remarks/>
        BOTH
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class Purpose2Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType10

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType10
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType10)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType10

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class InstructionForNextAgent1

        Private cdField As Instruction4Code

        Private cdFieldSpecified As Boolean

        Private instrInfField As String

        ''' <remarks/>
        Public Property Cd() As Instruction4Code
            Get
                Return Me.cdField
            End Get
            Set(value As Instruction4Code)
                Me.cdField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property CdSpecified() As Boolean
            Get
                Return Me.cdFieldSpecified
            End Get
            Set(value As Boolean)
                Me.cdFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstrInf() As String
            Get
                Return Me.instrInfField
            End Get
            Set(value As String)
                Me.instrInfField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum Instruction4Code

        ''' <remarks/>
        PHOA

        ''' <remarks/>
        TELA
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class InstructionForCreditorAgent1

        Private cdField As Instruction3Code

        Private cdFieldSpecified As Boolean

        Private instrInfField As String

        ''' <remarks/>
        Public Property Cd() As Instruction3Code
            Get
                Return Me.cdField
            End Get
            Set(value As Instruction3Code)
                Me.cdField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property CdSpecified() As Boolean
            Get
                Return Me.cdFieldSpecified
            End Get
            Set(value As Boolean)
                Me.cdFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstrInf() As String
            Get
                Return Me.instrInfField
            End Get
            Set(value As String)
                Me.instrInfField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum Instruction3Code

        ''' <remarks/>
        CHQB

        ''' <remarks/>
        HOLD

        ''' <remarks/>
        PHOB

        ''' <remarks/>
        TELB
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class Charges2

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        Private agtField As BranchAndFinancialInstitutionIdentification5

        ''' <remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Agt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.agtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.agtField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class BranchAndFinancialInstitutionIdentification5

        Private finInstnIdField As FinancialInstitutionIdentification8

        Private brnchIdField As BranchData2

        ''' <remarks/>
        Public Property FinInstnId() As FinancialInstitutionIdentification8
            Get
                Return Me.finInstnIdField
            End Get
            Set(value As FinancialInstitutionIdentification8)
                Me.finInstnIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property BrnchId() As BranchData2
            Get
                Return Me.brnchIdField
            End Get
            Set(value As BranchData2)
                Me.brnchIdField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class FinancialInstitutionIdentification8

        Private bICFIField As String

        Private clrSysMmbIdField As ClearingSystemMemberIdentification2

        Private nmField As String

        Private pstlAdrField As PostalAddress6

        Private othrField As GenericFinancialIdentification1

        ''' <remarks/>
        Public Property BICFI() As String
            Get
                Return Me.bICFIField
            End Get
            Set(value As String)
                Me.bICFIField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property ClrSysMmbId() As ClearingSystemMemberIdentification2
            Get
                Return Me.clrSysMmbIdField
            End Get
            Set(value As ClearingSystemMemberIdentification2)
                Me.clrSysMmbIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PstlAdr() As PostalAddress6
            Get
                Return Me.pstlAdrField
            End Get
            Set(value As PostalAddress6)
                Me.pstlAdrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Othr() As GenericFinancialIdentification1
            Get
                Return Me.othrField
            End Get
            Set(value As GenericFinancialIdentification1)
                Me.othrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ClearingSystemMemberIdentification2

        Private clrSysIdField As ClearingSystemIdentification2Choice

        Private mmbIdField As String

        ''' <remarks/>
        Public Property ClrSysId() As ClearingSystemIdentification2Choice
            Get
                Return Me.clrSysIdField
            End Get
            Set(value As ClearingSystemIdentification2Choice)
                Me.clrSysIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property MmbId() As String
            Get
                Return Me.mmbIdField
            End Get
            Set(value As String)
                Me.mmbIdField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ClearingSystemIdentification2Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType3

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType3
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType3)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType3

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class GenericFinancialIdentification1

        Private idField As String

        Private schmeNmField As FinancialIdentificationSchemeName1Choice

        Private issrField As String

        ''' <remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SchmeNm() As FinancialIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(value As FinancialIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class FinancialIdentificationSchemeName1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType4

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType4
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType4)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType4

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class BranchData2

        Private idField As String

        Private nmField As String

        Private pstlAdrField As PostalAddress6

        ''' <remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PstlAdr() As PostalAddress6
            Get
                Return Me.pstlAdrField
            End Get
            Set(value As PostalAddress6)
                Me.pstlAdrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class SettlementTimeRequest2

        Private cLSTmField As System.DateTime

        Private cLSTmFieldSpecified As Boolean

        Private tillTmField As System.DateTime

        Private tillTmFieldSpecified As Boolean

        Private frTmField As System.DateTime

        Private frTmFieldSpecified As Boolean

        Private rjctTmField As System.DateTime

        Private rjctTmFieldSpecified As Boolean

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="time")> _
        Public Property CLSTm() As System.DateTime
            Get
                Return Me.cLSTmField
            End Get
            Set(value As System.DateTime)
                Me.cLSTmField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property CLSTmSpecified() As Boolean
            Get
                Return Me.cLSTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.cLSTmFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="time")> _
        Public Property TillTm() As System.DateTime
            Get
                Return Me.tillTmField
            End Get
            Set(value As System.DateTime)
                Me.tillTmField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property TillTmSpecified() As Boolean
            Get
                Return Me.tillTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.tillTmFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="time")> _
        Public Property FrTm() As System.DateTime
            Get
                Return Me.frTmField
            End Get
            Set(value As System.DateTime)
                Me.frTmField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property FrTmSpecified() As Boolean
            Get
                Return Me.frTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.frTmFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="time")> _
        Public Property RjctTm() As System.DateTime
            Get
                Return Me.rjctTmField
            End Get
            Set(value As System.DateTime)
                Me.rjctTmField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property RjctTmSpecified() As Boolean
            Get
                Return Me.rjctTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.rjctTmFieldSpecified = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class SettlementDateTimeIndication1

        Private dbtDtTmField As System.DateTime

        Private dbtDtTmFieldSpecified As Boolean

        Private cdtDtTmField As System.DateTime

        Private cdtDtTmFieldSpecified As Boolean

        ''' <remarks/>
        Public Property DbtDtTm() As System.DateTime
            Get
                Return Me.dbtDtTmField
            End Get
            Set(value As System.DateTime)
                Me.dbtDtTmField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property DbtDtTmSpecified() As Boolean
            Get
                Return Me.dbtDtTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dbtDtTmFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CdtDtTm() As System.DateTime
            Get
                Return Me.cdtDtTmField
            End Get
            Set(value As System.DateTime)
                Me.cdtDtTmField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property CdtDtTmSpecified() As Boolean
            Get
                Return Me.cdtDtTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.cdtDtTmFieldSpecified = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class PaymentIdentification3

        Private instrIdField As String

        Private endToEndIdField As String

        Private txIdField As String

        Private clrSysRefField As String

        ''' <remarks/>
        Public Property InstrId() As String
            Get
                Return Me.instrIdField
            End Get
            Set(value As String)
                Me.instrIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property EndToEndId() As String
            Get
                Return Me.endToEndIdField
            End Get
            Set(value As String)
                Me.endToEndIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property TxId() As String
            Get
                Return Me.txIdField
            End Get
            Set(value As String)
                Me.txIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property ClrSysRef() As String
            Get
                Return Me.clrSysRefField
            End Get
            Set(value As String)
                Me.clrSysRefField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class CreditTransferTransaction25
        Inherits EntityBase(Of CreditTransferTransaction25)

        Private pmtIdField As PaymentIdentification3

        Private pmtTpInfField As PaymentTypeInformation21

        Private intrBkSttlmAmtField As ActiveCurrencyAndAmount

        Private intrBkSttlmDtField As System.DateTime

        Private intrBkSttlmDtFieldSpecified As Boolean

        Private sttlmPrtyField As Priority3Code

        Private sttlmPrtyFieldSpecified As Boolean

        Private sttlmTmIndctnField As SettlementDateTimeIndication1

        Private sttlmTmReqField As SettlementTimeRequest2

        Private accptncDtTmField As System.DateTime

        Private accptncDtTmFieldSpecified As Boolean

        Private poolgAdjstmntDtField As System.DateTime

        Private poolgAdjstmntDtFieldSpecified As Boolean

        Private instdAmtField As ActiveOrHistoricCurrencyAndAmount

        Private xchgRateField As Decimal

        Private xchgRateFieldSpecified As Boolean

        Private chrgBrField As ChargeBearerType1Code

        Private chrgsInfField As Charges2()

        Private prvsInstgAgtField As BranchAndFinancialInstitutionIdentification5

        Private prvsInstgAgtAcctField As CashAccount24

        Private instgAgtField As BranchAndFinancialInstitutionIdentification5

        Private instdAgtField As BranchAndFinancialInstitutionIdentification5

        Private intrmyAgt1Field As BranchAndFinancialInstitutionIdentification5

        Private intrmyAgt1AcctField As CashAccount24

        Private intrmyAgt2Field As BranchAndFinancialInstitutionIdentification5

        Private intrmyAgt2AcctField As CashAccount24

        Private intrmyAgt3Field As BranchAndFinancialInstitutionIdentification5

        Private intrmyAgt3AcctField As CashAccount24

        Private ultmtDbtrField As PartyIdentification43

        Private initgPtyField As PartyIdentification43

        Private dbtrField As PartyIdentification43

        Private dbtrAcctField As CashAccount24

        Private dbtrAgtField As BranchAndFinancialInstitutionIdentification5

        Private dbtrAgtAcctField As CashAccount24

        Private cdtrAgtField As BranchAndFinancialInstitutionIdentification5

        Private cdtrAgtAcctField As CashAccount24

        Private cdtrField As PartyIdentification43

        Private cdtrAcctField As CashAccount24

        Private ultmtCdtrField As PartyIdentification43

        Private instrForCdtrAgtField As InstructionForCreditorAgent1()

        Private instrForNxtAgtField As InstructionForNextAgent1()

        Private purpField As Purpose2Choice

        Private rgltryRptgField As RegulatoryReporting3()

        Private taxField As TaxInformation3

        Private rltdRmtInfField As RemittanceLocation4()

        Private rmtInfField As RemittanceInformation11

        Private splmtryDataField As SupplementaryData1()

        '''<summary>
        ''' class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.PmtId = New PaymentIdentification3
        End Sub

        ''' <remarks/>
        Public Property PmtId() As PaymentIdentification3
            Get
                Return Me.pmtIdField
            End Get
            Set(value As PaymentIdentification3)
                Me.pmtIdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PmtTpInf() As PaymentTypeInformation21
            Get
                Return Me.pmtTpInfField
            End Get
            Set(value As PaymentTypeInformation21)
                Me.pmtTpInfField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property IntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.intrBkSttlmAmtField
            End Get
            Set(value As ActiveCurrencyAndAmount)
                Me.intrBkSttlmAmtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property IntrBkSttlmDt() As System.DateTime
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(value As System.DateTime)
                Me.intrBkSttlmDtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property IntrBkSttlmDtSpecified() As Boolean
            Get
                Return Me.intrBkSttlmDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.intrBkSttlmDtFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SttlmPrty() As Priority3Code
            Get
                Return Me.sttlmPrtyField
            End Get
            Set(value As Priority3Code)
                Me.sttlmPrtyField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property SttlmPrtySpecified() As Boolean
            Get
                Return Me.sttlmPrtyFieldSpecified
            End Get
            Set(value As Boolean)
                Me.sttlmPrtyFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SttlmTmIndctn() As SettlementDateTimeIndication1
            Get
                Return Me.sttlmTmIndctnField
            End Get
            Set(value As SettlementDateTimeIndication1)
                Me.sttlmTmIndctnField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SttlmTmReq() As SettlementTimeRequest2
            Get
                Return Me.sttlmTmReqField
            End Get
            Set(value As SettlementTimeRequest2)
                Me.sttlmTmReqField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property AccptncDtTm() As System.DateTime
            Get
                Return Me.accptncDtTmField
            End Get
            Set(value As System.DateTime)
                Me.accptncDtTmField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property AccptncDtTmSpecified() As Boolean
            Get
                Return Me.accptncDtTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.accptncDtTmFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property PoolgAdjstmntDt() As System.DateTime
            Get
                Return Me.poolgAdjstmntDtField
            End Get
            Set(value As System.DateTime)
                Me.poolgAdjstmntDtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property PoolgAdjstmntDtSpecified() As Boolean
            Get
                Return Me.poolgAdjstmntDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.poolgAdjstmntDtFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstdAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.instdAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.instdAmtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property XchgRate() As Decimal
            Get
                Return Me.xchgRateField
            End Get
            Set(value As Decimal)
                Me.xchgRateField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property XchgRateSpecified() As Boolean
            Get
                Return Me.xchgRateFieldSpecified
            End Get
            Set(value As Boolean)
                Me.xchgRateFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property ChrgBr() As ChargeBearerType1Code
            Get
                Return Me.chrgBrField
            End Get
            Set(value As ChargeBearerType1Code)
                Me.chrgBrField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ChrgsInf")> _
        Public Property ChrgsInf() As Charges2()
            Get
                Return Me.chrgsInfField
            End Get
            Set(value As Charges2())
                Me.chrgsInfField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PrvsInstgAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.prvsInstgAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.prvsInstgAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property PrvsInstgAgtAcct() As CashAccount24
            Get
                Return Me.prvsInstgAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.prvsInstgAgtAcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstgAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instgAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instgAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstdAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instdAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instdAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property IntrmyAgt1() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.intrmyAgt1Field
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.intrmyAgt1Field = value
            End Set
        End Property

        ''' <remarks/>
        Public Property IntrmyAgt1Acct() As CashAccount24
            Get
                Return Me.intrmyAgt1AcctField
            End Get
            Set(value As CashAccount24)
                Me.intrmyAgt1AcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property IntrmyAgt2() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.intrmyAgt2Field
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.intrmyAgt2Field = value
            End Set
        End Property

        ''' <remarks/>
        Public Property IntrmyAgt2Acct() As CashAccount24
            Get
                Return Me.intrmyAgt2AcctField
            End Get
            Set(value As CashAccount24)
                Me.intrmyAgt2AcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property IntrmyAgt3() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.intrmyAgt3Field
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.intrmyAgt3Field = value
            End Set
        End Property

        ''' <remarks/>
        Public Property IntrmyAgt3Acct() As CashAccount24
            Get
                Return Me.intrmyAgt3AcctField
            End Get
            Set(value As CashAccount24)
                Me.intrmyAgt3AcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property UltmtDbtr() As PartyIdentification43
            Get
                Return Me.ultmtDbtrField
            End Get
            Set(value As PartyIdentification43)
                Me.ultmtDbtrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InitgPty() As PartyIdentification43
            Get
                Return Me.initgPtyField
            End Get
            Set(value As PartyIdentification43)
                Me.initgPtyField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Dbtr() As PartyIdentification43
            Get
                Return Me.dbtrField
            End Get
            Set(value As PartyIdentification43)
                Me.dbtrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property DbtrAcct() As CashAccount24
            Get
                Return Me.dbtrAcctField
            End Get
            Set(value As CashAccount24)
                Me.dbtrAcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property DbtrAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.dbtrAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.dbtrAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property DbtrAgtAcct() As CashAccount24
            Get
                Return Me.dbtrAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.dbtrAgtAcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CdtrAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.cdtrAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.cdtrAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CdtrAgtAcct() As CashAccount24
            Get
                Return Me.cdtrAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.cdtrAgtAcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Cdtr() As PartyIdentification43
            Get
                Return Me.cdtrField
            End Get
            Set(value As PartyIdentification43)
                Me.cdtrField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CdtrAcct() As CashAccount24
            Get
                Return Me.cdtrAcctField
            End Get
            Set(value As CashAccount24)
                Me.cdtrAcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property UltmtCdtr() As PartyIdentification43
            Get
                Return Me.ultmtCdtrField
            End Get
            Set(value As PartyIdentification43)
                Me.ultmtCdtrField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InstrForCdtrAgt")> _
        Public Property InstrForCdtrAgt() As InstructionForCreditorAgent1()
            Get
                Return Me.instrForCdtrAgtField
            End Get
            Set(value As InstructionForCreditorAgent1())
                Me.instrForCdtrAgtField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InstrForNxtAgt")> _
        Public Property InstrForNxtAgt() As InstructionForNextAgent1()
            Get
                Return Me.instrForNxtAgtField
            End Get
            Set(value As InstructionForNextAgent1())
                Me.instrForNxtAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Purp() As Purpose2Choice
            Get
                Return Me.purpField
            End Get
            Set(value As Purpose2Choice)
                Me.purpField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RgltryRptg")> _
        Public Property RgltryRptg() As RegulatoryReporting3()
            Get
                Return Me.rgltryRptgField
            End Get
            Set(value As RegulatoryReporting3())
                Me.rgltryRptgField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Tax() As TaxInformation3
            Get
                Return Me.taxField
            End Get
            Set(value As TaxInformation3)
                Me.taxField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RltdRmtInf")> _
        Public Property RltdRmtInf() As RemittanceLocation4()
            Get
                Return Me.rltdRmtInfField
            End Get
            Set(value As RemittanceLocation4())
                Me.rltdRmtInfField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property RmtInf() As RemittanceInformation11
            Get
                Return Me.rmtInfField
            End Get
            Set(value As RemittanceInformation11)
                Me.rmtInfField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SplmtryData")> _
        Public Property SplmtryData() As SupplementaryData1()
            Get
                Return Me.splmtryDataField
            End Get
            Set(value As SupplementaryData1())
                Me.splmtryDataField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class PaymentTypeInformation21

        Private instrPrtyField As Priority2Code

        Private instrPrtyFieldSpecified As Boolean

        Private clrChanlField As ClearingChannel2Code

        Private clrChanlFieldSpecified As Boolean

        Private svcLvlField As ServiceLevel8Choice

        Private lclInstrmField As LocalInstrument2Choice

        Private ctgyPurpField As CategoryPurpose1Choice

        ''' <remarks/>
        Public Property InstrPrty() As Priority2Code
            Get
                Return Me.instrPrtyField
            End Get
            Set(value As Priority2Code)
                Me.instrPrtyField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property InstrPrtySpecified() As Boolean
            Get
                Return Me.instrPrtyFieldSpecified
            End Get
            Set(value As Boolean)
                Me.instrPrtyFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property ClrChanl() As ClearingChannel2Code
            Get
                Return Me.clrChanlField
            End Get
            Set(value As ClearingChannel2Code)
                Me.clrChanlField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ClrChanlSpecified() As Boolean
            Get
                Return Me.clrChanlFieldSpecified
            End Get
            Set(value As Boolean)
                Me.clrChanlFieldSpecified = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SvcLvl() As ServiceLevel8Choice
            Get
                Return Me.svcLvlField
            End Get
            Set(value As ServiceLevel8Choice)
                Me.svcLvlField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property LclInstrm() As LocalInstrument2Choice
            Get
                Return Me.lclInstrmField
            End Get
            Set(value As LocalInstrument2Choice)
                Me.lclInstrmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property CtgyPurp() As CategoryPurpose1Choice
            Get
                Return Me.ctgyPurpField
            End Get
            Set(value As CategoryPurpose1Choice)
                Me.ctgyPurpField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum Priority2Code

        ''' <remarks/>
        HIGH

        ''' <remarks/>
        NORM
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum ClearingChannel2Code

        ''' <remarks/>
        RTGS

        ''' <remarks/>
        RTNS

        ''' <remarks/>
        MPNS

        ''' <remarks/>
        BOOK
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ServiceLevel8Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType5

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType5
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType5)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType5

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class LocalInstrument2Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType6

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType6
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType6)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType6

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class CategoryPurpose1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType7

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType7
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType7)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType7

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum Priority3Code

        ''' <remarks/>
        URGT

        ''' <remarks/>
        HIGH

        ''' <remarks/>
        NORM
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum ChargeBearerType1Code

        ''' <remarks/>
        DEBT

        ''' <remarks/>
        CRED

        ''' <remarks/>
        SHAR

        ''' <remarks/>
        SLEV
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class CashAccount24

        Private idField As AccountIdentification4Choice

        Private tpField As CashAccountType2Choice

        Private ccyField As String

        Private nmField As String

        ''' <remarks/>
        Public Property Id() As AccountIdentification4Choice
            Get
                Return Me.idField
            End Get
            Set(value As AccountIdentification4Choice)
                Me.idField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Tp() As CashAccountType2Choice
            Get
                Return Me.tpField
            End Get
            Set(value As CashAccountType2Choice)
                Me.tpField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Ccy() As String
            Get
                Return Me.ccyField
            End Get
            Set(value As String)
                Me.ccyField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class AccountIdentification4Choice
        Inherits EntityBase(Of AccountIdentification4Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("IBAN", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericAccountIdentification1))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class GenericAccountIdentification1

        Private idField As String

        Private schmeNmField As AccountSchemeName1Choice

        Private issrField As String

        ''' <remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SchmeNm() As AccountSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(value As AccountSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class AccountSchemeName1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class CashAccountType2Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType1

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType1
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType1)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType1

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class ClearingSystemIdentification3Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType2

        ''' <remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String))> _
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        ''' <remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute> _
        Public Property ItemElementName() As ItemChoiceType2
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType2)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType2

        ''' <remarks/>
        Cd

        ''' <remarks/>
        Prtry
    End Enum

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Diagnostics.DebuggerStepThroughAttribute> _
    <System.ComponentModel.DesignerCategoryAttribute("code")> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Partial Public Class SettlementInstruction4

        Private sttlmMtdField As SettlementMethod1Code

        Private sttlmAcctField As CashAccount24

        Private clrSysField As ClearingSystemIdentification3Choice

        Private instgRmbrsmntAgtField As BranchAndFinancialInstitutionIdentification5

        Private instgRmbrsmntAgtAcctField As CashAccount24

        Private instdRmbrsmntAgtField As BranchAndFinancialInstitutionIdentification5

        Private instdRmbrsmntAgtAcctField As CashAccount24

        Private thrdRmbrsmntAgtField As BranchAndFinancialInstitutionIdentification5

        Private thrdRmbrsmntAgtAcctField As CashAccount24

        ''' <remarks/>
        Public Property SttlmMtd() As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(value As SettlementMethod1Code)
                Me.sttlmMtdField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property SttlmAcct() As CashAccount24
            Get
                Return Me.sttlmAcctField
            End Get
            Set(value As CashAccount24)
                Me.sttlmAcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property ClrSys() As ClearingSystemIdentification3Choice
            Get
                Return Me.clrSysField
            End Get
            Set(value As ClearingSystemIdentification3Choice)
                Me.clrSysField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstgRmbrsmntAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instgRmbrsmntAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instgRmbrsmntAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstgRmbrsmntAgtAcct() As CashAccount24
            Get
                Return Me.instgRmbrsmntAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.instgRmbrsmntAgtAcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstdRmbrsmntAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instdRmbrsmntAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instdRmbrsmntAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property InstdRmbrsmntAgtAcct() As CashAccount24
            Get
                Return Me.instdRmbrsmntAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.instdRmbrsmntAgtAcctField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property ThrdRmbrsmntAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.thrdRmbrsmntAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.thrdRmbrsmntAgtField = value
            End Set
        End Property

        ''' <remarks/>
        Public Property ThrdRmbrsmntAgtAcct() As CashAccount24
            Get
                Return Me.thrdRmbrsmntAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.thrdRmbrsmntAgtAcctField = value
            End Set
        End Property
    End Class

    ''' <remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")> _
    <System.SerializableAttribute> _
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.06")> _
    Public Enum SettlementMethod1Code

        ''' <remarks/>
        INDA

        ''' <remarks/>
        INGA

        ''' <remarks/>
        COVE

        ''' <remarks/>
        CLRG
    End Enum
End Namespace