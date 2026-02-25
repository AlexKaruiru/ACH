Imports System.IO
Imports System.Xml.Serialization
Imports System.ComponentModel

Namespace BRISO20022DD416 ' Direct Debits 416
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



    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IsNullable:=False)> _
    Partial Public Class Document
        Inherits EntityBase(Of Document)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtRtrField As PaymentReturnV06

        '''<summary>
        '''Document class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.PmtRtr = New PaymentReturnV06
        End Sub

        '''<remarks/>
        Public Property PmtRtr() As PaymentReturnV06
            Get
                Return Me.pmtRtrField
            End Get
            Set(value As PaymentReturnV06)
                Me.pmtRtrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class PaymentReturnV06
        Inherits EntityBase(Of PaymentReturnV06)
        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private grpHdrField As GroupHeader72

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlGrpInfField As OriginalGroupHeader2

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private txInfField As List(Of PaymentTransaction65)

        Private splmtryDataField() As SupplementaryData1


        Public Sub New()
            MyBase.New()
            Me.txInfField = New List(Of PaymentTransaction65)
            Me.grpHdrField = New GroupHeader72
            Me.orgnlGrpInfField = New OriginalGroupHeader2
        End Sub

        '''<remarks/>
        Public Property GrpHdr() As GroupHeader72
            Get
                Return Me.grpHdrField
            End Get
            Set(value As GroupHeader72)
                Me.grpHdrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlGrpInf() As OriginalGroupHeader2
            Get
                Return Me.orgnlGrpInfField
            End Get
            Set(value As OriginalGroupHeader2)
                Me.orgnlGrpInfField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TxInf")> _
        Public Property TxInf() As List(Of PaymentTransaction65)
            Get
                Return Me.txInfField
            End Get
            Set(value As List(Of PaymentTransaction65))
                Me.txInfField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class GroupHeader72

        Private msgIdField As String

        Private creDtTmField As Date

        Private authstnField() As Authorisation1Choice

        Private btchBookgField As Boolean

        Private btchBookgFieldSpecified As Boolean

        Private nbOfTxsField As String

        Private ctrlSumField As Decimal

        Private ctrlSumFieldSpecified As Boolean

        Private grpRtrField As Boolean

        Private grpRtrFieldSpecified As Boolean

        Private ttlRtrdIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        Private intrBkSttlmDtField As Date

        Private intrBkSttlmDtFieldSpecified As Boolean

        Private sttlmInfField As SettlementInstruction4

        Private instgAgtField As BranchAndFinancialInstitutionIdentification5

        Private instdAgtField As BranchAndFinancialInstitutionIdentification5

        '''<remarks/>
        Public Property MsgId() As String
            Get
                Return Me.msgIdField
            End Get
            Set(value As String)
                Me.msgIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CreDtTm() As Date
            Get
                Return Me.creDtTmField
            End Get
            Set(value As Date)
                Me.creDtTmField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Authstn")> _
        Public Property Authstn() As Authorisation1Choice()
            Get
                Return Me.authstnField
            End Get
            Set(value As Authorisation1Choice())
                Me.authstnField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BtchBookg() As Boolean
            Get
                Return Me.btchBookgField
            End Get
            Set(value As Boolean)
                Me.btchBookgField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property BtchBookgSpecified() As Boolean
            Get
                Return Me.btchBookgFieldSpecified
            End Get
            Set(value As Boolean)
                Me.btchBookgFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property NbOfTxs() As String
            Get
                Return Me.nbOfTxsField
            End Get
            Set(value As String)
                Me.nbOfTxsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CtrlSum() As Decimal
            Get
                Return Me.ctrlSumField
            End Get
            Set(value As Decimal)
                Me.ctrlSumField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property CtrlSumSpecified() As Boolean
            Get
                Return Me.ctrlSumFieldSpecified
            End Get
            Set(value As Boolean)
                Me.ctrlSumFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property GrpRtr() As Boolean
            Get
                Return Me.grpRtrField
            End Get
            Set(value As Boolean)
                Me.grpRtrField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property GrpRtrSpecified() As Boolean
            Get
                Return Me.grpRtrFieldSpecified
            End Get
            Set(value As Boolean)
                Me.grpRtrFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property TtlRtrdIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.ttlRtrdIntrBkSttlmAmtField
            End Get
            Set(value As ActiveCurrencyAndAmount)
                Me.ttlRtrdIntrBkSttlmAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("IntrBkSttlmDt", DataType:="date")> _
        Public Property IntrBkStlmDt() As Date
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(value As Date)
                Me.intrBkSttlmDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property IntrBkSttlmDtSpecified() As Boolean
            Get
                Return Me.intrBkSttlmDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.intrBkSttlmDtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SttlmInf() As SettlementInstruction4
            Get
                Return Me.sttlmInfField
            End Get
            Set(value As SettlementInstruction4)
                Me.sttlmInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InstgAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instgAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instgAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InstdAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instdAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instdAgtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class Authorisation1Choice

        Private itemField As Object

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(Authorisation1Code)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum Authorisation1Code

        '''<remarks/>
        AUTH

        '''<remarks/>
        FDET

        '''<remarks/>
        FSUM

        '''<remarks/>
        ILEV
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class SupplementaryData1

        Private plcAndNmField As String

        Private envlpField As System.Xml.XmlElement

        '''<remarks/>
        Public Property PlcAndNm() As String
            Get
                Return Me.plcAndNmField
            End Get
            Set(value As String)
                Me.plcAndNmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Envlp() As System.Xml.XmlElement
            Get
                Return Me.envlpField
            End Get
            Set(value As System.Xml.XmlElement)
                Me.envlpField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class GarnishmentType1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType15

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType15
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType15)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType15

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class GarnishmentType1

        Private cdOrPrtryField As GarnishmentType1Choice

        Private issrField As String

        '''<remarks/>
        Public Property CdOrPrtry() As GarnishmentType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(value As GarnishmentType1Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class Garnishment1

        Private tpField As GarnishmentType1

        Private grnsheeField As PartyIdentification43

        Private grnshmtAdmstrField As PartyIdentification43

        Private refNbField As String

        Private dtField As Date

        Private dtFieldSpecified As Boolean

        Private rmtdAmtField As ActiveOrHistoricCurrencyAndAmount

        Private fmlyMdclInsrncIndField As Boolean

        Private fmlyMdclInsrncIndFieldSpecified As Boolean

        Private mplyeeTermntnIndField As Boolean

        Private mplyeeTermntnIndFieldSpecified As Boolean

        '''<remarks/>
        Public Property Tp() As GarnishmentType1
            Get
                Return Me.tpField
            End Get
            Set(value As GarnishmentType1)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Grnshee() As PartyIdentification43
            Get
                Return Me.grnsheeField
            End Get
            Set(value As PartyIdentification43)
                Me.grnsheeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property GrnshmtAdmstr() As PartyIdentification43
            Get
                Return Me.grnshmtAdmstrField
            End Get
            Set(value As PartyIdentification43)
                Me.grnshmtAdmstrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RefNb() As String
            Get
                Return Me.refNbField
            End Get
            Set(value As String)
                Me.refNbField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property Dt() As Date
            Get
                Return Me.dtField
            End Get
            Set(value As Date)
                Me.dtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property DtSpecified() As Boolean
            Get
                Return Me.dtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property RmtdAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.rmtdAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.rmtdAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FmlyMdclInsrncInd() As Boolean
            Get
                Return Me.fmlyMdclInsrncIndField
            End Get
            Set(value As Boolean)
                Me.fmlyMdclInsrncIndField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property FmlyMdclInsrncIndSpecified() As Boolean
            Get
                Return Me.fmlyMdclInsrncIndFieldSpecified
            End Get
            Set(value As Boolean)
                Me.fmlyMdclInsrncIndFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property MplyeeTermntnInd() As Boolean
            Get
                Return Me.mplyeeTermntnIndField
            End Get
            Set(value As Boolean)
                Me.mplyeeTermntnIndField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property MplyeeTermntnIndSpecified() As Boolean
            Get
                Return Me.mplyeeTermntnIndFieldSpecified
            End Get
            Set(value As Boolean)
                Me.mplyeeTermntnIndFieldSpecified = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class PartyIdentification43

        Private nmField As String

        Private pstlAdrField As PostalAddress6

        Private idField As Party11Choice

        Private ctryOfResField As String

        Private ctctDtlsField As ContactDetails2

        '''<remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PstlAdr() As PostalAddress6
            Get
                Return Me.pstlAdrField
            End Get
            Set(value As PostalAddress6)
                Me.pstlAdrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Id() As Party11Choice
            Get
                Return Me.idField
            End Get
            Set(value As Party11Choice)
                Me.idField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CtryOfRes() As String
            Get
                Return Me.ctryOfResField
            End Get
            Set(value As String)
                Me.ctryOfResField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CtctDtls() As ContactDetails2
            Get
                Return Me.ctctDtlsField
            End Get
            Set(value As ContactDetails2)
                Me.ctctDtlsField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
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

        Private adrLineField() As String

        '''<remarks/>
        Public Property AdrTp() As AddressType2Code
            Get
                Return Me.adrTpField
            End Get
            Set(value As AddressType2Code)
                Me.adrTpField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property AdrTpSpecified() As Boolean
            Get
                Return Me.adrTpFieldSpecified
            End Get
            Set(value As Boolean)
                Me.adrTpFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Dept() As String
            Get
                Return Me.deptField
            End Get
            Set(value As String)
                Me.deptField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SubDept() As String
            Get
                Return Me.subDeptField
            End Get
            Set(value As String)
                Me.subDeptField = value
            End Set
        End Property

        '''<remarks/>
        Public Property StrtNm() As String
            Get
                Return Me.strtNmField
            End Get
            Set(value As String)
                Me.strtNmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BldgNb() As String
            Get
                Return Me.bldgNbField
            End Get
            Set(value As String)
                Me.bldgNbField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PstCd() As String
            Get
                Return Me.pstCdField
            End Get
            Set(value As String)
                Me.pstCdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TwnNm() As String
            Get
                Return Me.twnNmField
            End Get
            Set(value As String)
                Me.twnNmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CtrySubDvsn() As String
            Get
                Return Me.ctrySubDvsnField
            End Get
            Set(value As String)
                Me.ctrySubDvsnField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(value As String)
                Me.ctryField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum AddressType2Code

        '''<remarks/>
        ADDR

        '''<remarks/>
        PBOX

        '''<remarks/>
        HOME

        '''<remarks/>
        BIZZ

        '''<remarks/>
        MLTO

        '''<remarks/>
        DLVY
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class Party11Choice

        Private itemField As Object

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OrgId", GetType(OrganisationIdentification8)), _
         System.Xml.Serialization.XmlElementAttribute("PrvtId", GetType(PersonIdentification5))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class OrganisationIdentification8

        Private anyBICField As String

        Private othrField() As GenericOrganisationIdentification1

        '''<remarks/>
        Public Property AnyBIC() As String
            Get
                Return Me.anyBICField
            End Get
            Set(value As String)
                Me.anyBICField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class GenericOrganisationIdentification1

        Private idField As String

        Private schmeNmField As OrganisationIdentificationSchemeName1Choice

        Private issrField As String

        '''<remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SchmeNm() As OrganisationIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(value As OrganisationIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class OrganisationIdentificationSchemeName1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType5

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType5
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType5)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType5

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class PersonIdentification5

        Private dtAndPlcOfBirthField As DateAndPlaceOfBirth

        Private othrField() As GenericPersonIdentification1

        '''<remarks/>
        Public Property DtAndPlcOfBirth() As DateAndPlaceOfBirth
            Get
                Return Me.dtAndPlcOfBirthField
            End Get
            Set(value As DateAndPlaceOfBirth)
                Me.dtAndPlcOfBirthField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DateAndPlaceOfBirth

        Private birthDtField As Date

        Private prvcOfBirthField As String

        Private cityOfBirthField As String

        Private ctryOfBirthField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property BirthDt() As Date
            Get
                Return Me.birthDtField
            End Get
            Set(value As Date)
                Me.birthDtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PrvcOfBirth() As String
            Get
                Return Me.prvcOfBirthField
            End Get
            Set(value As String)
                Me.prvcOfBirthField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CityOfBirth() As String
            Get
                Return Me.cityOfBirthField
            End Get
            Set(value As String)
                Me.cityOfBirthField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CtryOfBirth() As String
            Get
                Return Me.ctryOfBirthField
            End Get
            Set(value As String)
                Me.ctryOfBirthField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class GenericPersonIdentification1

        Private idField As String

        Private schmeNmField As PersonIdentificationSchemeName1Choice

        Private issrField As String

        '''<remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SchmeNm() As PersonIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(value As PersonIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class PersonIdentificationSchemeName1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType6

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType6
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType6)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType6

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ContactDetails2

        Private nmPrfxField As NamePrefix1Code

        Private nmPrfxFieldSpecified As Boolean

        Private nmField As String

        Private phneNbField As String

        Private mobNbField As String

        Private faxNbField As String

        Private emailAdrField As String

        Private othrField As String

        '''<remarks/>
        Public Property NmPrfx() As NamePrefix1Code
            Get
                Return Me.nmPrfxField
            End Get
            Set(value As NamePrefix1Code)
                Me.nmPrfxField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property NmPrfxSpecified() As Boolean
            Get
                Return Me.nmPrfxFieldSpecified
            End Get
            Set(value As Boolean)
                Me.nmPrfxFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PhneNb() As String
            Get
                Return Me.phneNbField
            End Get
            Set(value As String)
                Me.phneNbField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MobNb() As String
            Get
                Return Me.mobNbField
            End Get
            Set(value As String)
                Me.mobNbField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FaxNb() As String
            Get
                Return Me.faxNbField
            End Get
            Set(value As String)
                Me.faxNbField = value
            End Set
        End Property

        '''<remarks/>
        Public Property EmailAdr() As String
            Get
                Return Me.emailAdrField
            End Get
            Set(value As String)
                Me.emailAdrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Othr() As String
            Get
                Return Me.othrField
            End Get
            Set(value As String)
                Me.othrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum NamePrefix1Code

        '''<remarks/>
        DOCT

        '''<remarks/>
        MIST

        '''<remarks/>
        MISS

        '''<remarks/>
        MADM
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ActiveOrHistoricCurrencyAndAmount

        Private ccyField As String

        Private valueField As Decimal

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Ccy() As String
            Get
                Return Me.ccyField
            End Get
            Set(value As String)
                Me.ccyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value() As Decimal
            Get
                Return Me.valueField
            End Get
            Set(value As Decimal)
                Me.valueField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxRecordDetails1

        Private prdField As TaxPeriod1

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        '''<remarks/>
        Public Property Prd() As TaxPeriod1
            Get
                Return Me.prdField
            End Get
            Set(value As TaxPeriod1)
                Me.prdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxPeriod1

        Private yrField As Date

        Private yrFieldSpecified As Boolean

        Private tpField As TaxRecordPeriod1Code

        Private tpFieldSpecified As Boolean

        Private frToDtField As DatePeriodDetails

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property Yr() As Date
            Get
                Return Me.yrField
            End Get
            Set(value As Date)
                Me.yrField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property YrSpecified() As Boolean
            Get
                Return Me.yrFieldSpecified
            End Get
            Set(value As Boolean)
                Me.yrFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Tp() As TaxRecordPeriod1Code
            Get
                Return Me.tpField
            End Get
            Set(value As TaxRecordPeriod1Code)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property TpSpecified() As Boolean
            Get
                Return Me.tpFieldSpecified
            End Get
            Set(value As Boolean)
                Me.tpFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property FrToDt() As DatePeriodDetails
            Get
                Return Me.frToDtField
            End Get
            Set(value As DatePeriodDetails)
                Me.frToDtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum TaxRecordPeriod1Code

        '''<remarks/>
        MM01

        '''<remarks/>
        MM02

        '''<remarks/>
        MM03

        '''<remarks/>
        MM04

        '''<remarks/>
        MM05

        '''<remarks/>
        MM06

        '''<remarks/>
        MM07

        '''<remarks/>
        MM08

        '''<remarks/>
        MM09

        '''<remarks/>
        MM10

        '''<remarks/>
        MM11

        '''<remarks/>
        MM12

        '''<remarks/>
        QTR1

        '''<remarks/>
        QTR2

        '''<remarks/>
        QTR3

        '''<remarks/>
        QTR4

        '''<remarks/>
        HLF1

        '''<remarks/>
        HLF2
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DatePeriodDetails

        Private frDtField As Date

        Private toDtField As Date

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property FrDt() As Date
            Get
                Return Me.frDtField
            End Get
            Set(value As Date)
                Me.frDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property ToDt() As Date
            Get
                Return Me.toDtField
            End Get
            Set(value As Date)
                Me.toDtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxAmount1

        Private rateField As Decimal

        Private rateFieldSpecified As Boolean

        Private taxblBaseAmtField As ActiveOrHistoricCurrencyAndAmount

        Private ttlAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dtlsField() As TaxRecordDetails1

        '''<remarks/>
        Public Property Rate() As Decimal
            Get
                Return Me.rateField
            End Get
            Set(value As Decimal)
                Me.rateField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property RateSpecified() As Boolean
            Get
                Return Me.rateFieldSpecified
            End Get
            Set(value As Boolean)
                Me.rateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxblBaseAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.taxblBaseAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.taxblBaseAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TtlAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.ttlAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.ttlAmtField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
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

        '''<remarks/>
        Public Property Tp() As String
            Get
                Return Me.tpField
            End Get
            Set(value As String)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Ctgy() As String
            Get
                Return Me.ctgyField
            End Get
            Set(value As String)
                Me.ctgyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CtgyDtls() As String
            Get
                Return Me.ctgyDtlsField
            End Get
            Set(value As String)
                Me.ctgyDtlsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DbtrSts() As String
            Get
                Return Me.dbtrStsField
            End Get
            Set(value As String)
                Me.dbtrStsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CertId() As String
            Get
                Return Me.certIdField
            End Get
            Set(value As String)
                Me.certIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FrmsCd() As String
            Get
                Return Me.frmsCdField
            End Get
            Set(value As String)
                Me.frmsCdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Prd() As TaxPeriod1
            Get
                Return Me.prdField
            End Get
            Set(value As TaxPeriod1)
                Me.prdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxAmt() As TaxAmount1
            Get
                Return Me.taxAmtField
            End Get
            Set(value As TaxAmount1)
                Me.taxAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddtlInf() As String
            Get
                Return Me.addtlInfField
            End Get
            Set(value As String)
                Me.addtlInfField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxAuthorisation1

        Private titlField As String

        Private nmField As String

        '''<remarks/>
        Public Property Titl() As String
            Get
                Return Me.titlField
            End Get
            Set(value As String)
                Me.titlField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxParty2

        Private taxIdField As String

        Private regnIdField As String

        Private taxTpField As String

        Private authstnField As TaxAuthorisation1

        '''<remarks/>
        Public Property TaxId() As String
            Get
                Return Me.taxIdField
            End Get
            Set(value As String)
                Me.taxIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RegnId() As String
            Get
                Return Me.regnIdField
            End Get
            Set(value As String)
                Me.regnIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxTp() As String
            Get
                Return Me.taxTpField
            End Get
            Set(value As String)
                Me.taxTpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Authstn() As TaxAuthorisation1
            Get
                Return Me.authstnField
            End Get
            Set(value As TaxAuthorisation1)
                Me.authstnField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxParty1

        Private taxIdField As String

        Private regnIdField As String

        Private taxTpField As String

        '''<remarks/>
        Public Property TaxId() As String
            Get
                Return Me.taxIdField
            End Get
            Set(value As String)
                Me.taxIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RegnId() As String
            Get
                Return Me.regnIdField
            End Get
            Set(value As String)
                Me.regnIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxTp() As String
            Get
                Return Me.taxTpField
            End Get
            Set(value As String)
                Me.taxTpField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxInformation4

        Private cdtrField As TaxParty1

        Private dbtrField As TaxParty2

        Private ultmtDbtrField As TaxParty2

        Private admstnZoneField As String

        Private refNbField As String

        Private mtdField As String

        Private ttlTaxblBaseAmtField As ActiveOrHistoricCurrencyAndAmount

        Private ttlTaxAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dtField As Date

        Private dtFieldSpecified As Boolean

        Private seqNbField As Decimal

        Private seqNbFieldSpecified As Boolean

        Private rcrdField() As TaxRecord1

        '''<remarks/>
        Public Property Cdtr() As TaxParty1
            Get
                Return Me.cdtrField
            End Get
            Set(value As TaxParty1)
                Me.cdtrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Dbtr() As TaxParty2
            Get
                Return Me.dbtrField
            End Get
            Set(value As TaxParty2)
                Me.dbtrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UltmtDbtr() As TaxParty2
            Get
                Return Me.ultmtDbtrField
            End Get
            Set(value As TaxParty2)
                Me.ultmtDbtrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AdmstnZone() As String
            Get
                Return Me.admstnZoneField
            End Get
            Set(value As String)
                Me.admstnZoneField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RefNb() As String
            Get
                Return Me.refNbField
            End Get
            Set(value As String)
                Me.refNbField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Mtd() As String
            Get
                Return Me.mtdField
            End Get
            Set(value As String)
                Me.mtdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TtlTaxblBaseAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.ttlTaxblBaseAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.ttlTaxblBaseAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TtlTaxAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.ttlTaxAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.ttlTaxAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property Dt() As Date
            Get
                Return Me.dtField
            End Get
            Set(value As Date)
                Me.dtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property DtSpecified() As Boolean
            Get
                Return Me.dtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SeqNb() As Decimal
            Get
                Return Me.seqNbField
            End Get
            Set(value As Decimal)
                Me.seqNbField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property SeqNbSpecified() As Boolean
            Get
                Return Me.seqNbFieldSpecified
            End Get
            Set(value As Boolean)
                Me.seqNbFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class CreditorReferenceType1Choice

        Private itemField As Object

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(DocumentType3Code)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum DocumentType3Code

        '''<remarks/>
        RADM

        '''<remarks/>
        RPIN

        '''<remarks/>
        FXDR

        '''<remarks/>
        DISP

        '''<remarks/>
        PUOR

        '''<remarks/>
        SCOR
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class CreditorReferenceType2

        Private cdOrPrtryField As CreditorReferenceType1Choice

        Private issrField As String

        '''<remarks/>
        Public Property CdOrPrtry() As CreditorReferenceType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(value As CreditorReferenceType1Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class CreditorReferenceInformation2

        Private tpField As CreditorReferenceType2

        Private refField As String

        '''<remarks/>
        Public Property Tp() As CreditorReferenceType2
            Get
                Return Me.tpField
            End Get
            Set(value As CreditorReferenceType2)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Ref() As String
            Get
                Return Me.refField
            End Get
            Set(value As String)
                Me.refField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class RemittanceAmount2

        Private duePyblAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dscntApldAmtField() As DiscountAmountAndType1

        Private cdtNoteAmtField As ActiveOrHistoricCurrencyAndAmount

        Private taxAmtField() As TaxAmountAndType1

        Private adjstmntAmtAndRsnField() As DocumentAdjustment1

        Private rmtdAmtField As ActiveOrHistoricCurrencyAndAmount

        '''<remarks/>
        Public Property DuePyblAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.duePyblAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.duePyblAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DscntApldAmt")> _
        Public Property DscntApldAmt() As DiscountAmountAndType1()
            Get
                Return Me.dscntApldAmtField
            End Get
            Set(value As DiscountAmountAndType1())
                Me.dscntApldAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CdtNoteAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.cdtNoteAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.cdtNoteAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TaxAmt")> _
        Public Property TaxAmt() As TaxAmountAndType1()
            Get
                Return Me.taxAmtField
            End Get
            Set(value As TaxAmountAndType1())
                Me.taxAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdjstmntAmtAndRsn")> _
        Public Property AdjstmntAmtAndRsn() As DocumentAdjustment1()
            Get
                Return Me.adjstmntAmtAndRsnField
            End Get
            Set(value As DocumentAdjustment1())
                Me.adjstmntAmtAndRsnField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RmtdAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.rmtdAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.rmtdAmtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DiscountAmountAndType1

        Private tpField As DiscountAmountType1Choice

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        '''<remarks/>
        Public Property Tp() As DiscountAmountType1Choice
            Get
                Return Me.tpField
            End Get
            Set(value As DiscountAmountType1Choice)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DiscountAmountType1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType13

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType13
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType13)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType13

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxAmountAndType1

        Private tpField As TaxAmountType1Choice

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        '''<remarks/>
        Public Property Tp() As TaxAmountType1Choice
            Get
                Return Me.tpField
            End Get
            Set(value As TaxAmountType1Choice)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class TaxAmountType1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType14

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType14
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType14)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType14

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DocumentAdjustment1

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        Private cdtDbtIndField As CreditDebitCode

        Private cdtDbtIndFieldSpecified As Boolean

        Private rsnField As String

        Private addtlInfField As String

        '''<remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CdtDbtInd() As CreditDebitCode
            Get
                Return Me.cdtDbtIndField
            End Get
            Set(value As CreditDebitCode)
                Me.cdtDbtIndField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property CdtDbtIndSpecified() As Boolean
            Get
                Return Me.cdtDbtIndFieldSpecified
            End Get
            Set(value As Boolean)
                Me.cdtDbtIndFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Rsn() As String
            Get
                Return Me.rsnField
            End Get
            Set(value As String)
                Me.rsnField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddtlInf() As String
            Get
                Return Me.addtlInfField
            End Get
            Set(value As String)
                Me.addtlInfField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum CreditDebitCode

        '''<remarks/>
        CRDT

        '''<remarks/>
        DBIT
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class RemittanceAmount3

        Private duePyblAmtField As ActiveOrHistoricCurrencyAndAmount

        Private dscntApldAmtField() As DiscountAmountAndType1

        Private cdtNoteAmtField As ActiveOrHistoricCurrencyAndAmount

        Private taxAmtField() As TaxAmountAndType1

        Private adjstmntAmtAndRsnField() As DocumentAdjustment1

        Private rmtdAmtField As ActiveOrHistoricCurrencyAndAmount

        '''<remarks/>
        Public Property DuePyblAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.duePyblAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.duePyblAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DscntApldAmt")> _
        Public Property DscntApldAmt() As DiscountAmountAndType1()
            Get
                Return Me.dscntApldAmtField
            End Get
            Set(value As DiscountAmountAndType1())
                Me.dscntApldAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CdtNoteAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.cdtNoteAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.cdtNoteAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TaxAmt")> _
        Public Property TaxAmt() As TaxAmountAndType1()
            Get
                Return Me.taxAmtField
            End Get
            Set(value As TaxAmountAndType1())
                Me.taxAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdjstmntAmtAndRsn")> _
        Public Property AdjstmntAmtAndRsn() As DocumentAdjustment1()
            Get
                Return Me.adjstmntAmtAndRsnField
            End Get
            Set(value As DocumentAdjustment1())
                Me.adjstmntAmtAndRsnField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RmtdAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.rmtdAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.rmtdAmtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DocumentLineType1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType12

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType12
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType12)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType12

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DocumentLineType1

        Private cdOrPrtryField As DocumentLineType1Choice

        Private issrField As String

        '''<remarks/>
        Public Property CdOrPrtry() As DocumentLineType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(value As DocumentLineType1Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DocumentLineIdentification1

        Private tpField As DocumentLineType1

        Private nbField As String

        Private rltdDtField As Date

        Private rltdDtFieldSpecified As Boolean

        '''<remarks/>
        Public Property Tp() As DocumentLineType1
            Get
                Return Me.tpField
            End Get
            Set(value As DocumentLineType1)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Nb() As String
            Get
                Return Me.nbField
            End Get
            Set(value As String)
                Me.nbField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property RltdDt() As Date
            Get
                Return Me.rltdDtField
            End Get
            Set(value As Date)
                Me.rltdDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property RltdDtSpecified() As Boolean
            Get
                Return Me.rltdDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.rltdDtFieldSpecified = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class DocumentLineInformation1

        Private idField() As DocumentLineIdentification1

        Private descField As String

        Private amtField As RemittanceAmount3

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Id")> _
        Public Property Id() As DocumentLineIdentification1()
            Get
                Return Me.idField
            End Get
            Set(value As DocumentLineIdentification1())
                Me.idField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Desc() As String
            Get
                Return Me.descField
            End Get
            Set(value As String)
                Me.descField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amt() As RemittanceAmount3
            Get
                Return Me.amtField
            End Get
            Set(value As RemittanceAmount3)
                Me.amtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ReferredDocumentType3Choice

        Private itemField As Object

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(DocumentType6Code)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum DocumentType6Code

        '''<remarks/>
        MSIN

        '''<remarks/>
        CNFA

        '''<remarks/>
        DNFA

        '''<remarks/>
        CINV

        '''<remarks/>
        CREN

        '''<remarks/>
        DEBN

        '''<remarks/>
        HIRI

        '''<remarks/>
        SBIN

        '''<remarks/>
        CMCN

        '''<remarks/>
        SOAC

        '''<remarks/>
        DISP

        '''<remarks/>
        BOLD

        '''<remarks/>
        VCHR

        '''<remarks/>
        AROI

        '''<remarks/>
        TSUT

        '''<remarks/>
        PUOR
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ReferredDocumentType4

        Private cdOrPrtryField As ReferredDocumentType3Choice

        Private issrField As String

        '''<remarks/>
        Public Property CdOrPrtry() As ReferredDocumentType3Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(value As ReferredDocumentType3Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ReferredDocumentInformation7

        Private tpField As ReferredDocumentType4

        Private nbField As String

        Private rltdDtField As Date

        Private rltdDtFieldSpecified As Boolean

        Private lineDtlsField() As DocumentLineInformation1

        '''<remarks/>
        Public Property Tp() As ReferredDocumentType4
            Get
                Return Me.tpField
            End Get
            Set(value As ReferredDocumentType4)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Nb() As String
            Get
                Return Me.nbField
            End Get
            Set(value As String)
                Me.nbField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property RltdDt() As Date
            Get
                Return Me.rltdDtField
            End Get
            Set(value As Date)
                Me.rltdDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property RltdDtSpecified() As Boolean
            Get
                Return Me.rltdDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.rltdDtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class StructuredRemittanceInformation13

        Private rfrdDocInfField() As ReferredDocumentInformation7

        Private rfrdDocAmtField As RemittanceAmount2

        Private cdtrRefInfField As CreditorReferenceInformation2

        Private invcrField As PartyIdentification43

        Private invceeField As PartyIdentification43

        Private taxRmtField As TaxInformation4

        Private grnshmtRmtField As Garnishment1

        Private addtlRmtInfField() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RfrdDocInf")> _
        Public Property RfrdDocInf() As ReferredDocumentInformation7()
            Get
                Return Me.rfrdDocInfField
            End Get
            Set(value As ReferredDocumentInformation7())
                Me.rfrdDocInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RfrdDocAmt() As RemittanceAmount2
            Get
                Return Me.rfrdDocAmtField
            End Get
            Set(value As RemittanceAmount2)
                Me.rfrdDocAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CdtrRefInf() As CreditorReferenceInformation2
            Get
                Return Me.cdtrRefInfField
            End Get
            Set(value As CreditorReferenceInformation2)
                Me.cdtrRefInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Invcr() As PartyIdentification43
            Get
                Return Me.invcrField
            End Get
            Set(value As PartyIdentification43)
                Me.invcrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Invcee() As PartyIdentification43
            Get
                Return Me.invceeField
            End Get
            Set(value As PartyIdentification43)
                Me.invceeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxRmt() As TaxInformation4
            Get
                Return Me.taxRmtField
            End Get
            Set(value As TaxInformation4)
                Me.taxRmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property GrnshmtRmt() As Garnishment1
            Get
                Return Me.grnshmtRmtField
            End Get
            Set(value As Garnishment1)
                Me.grnshmtRmtField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class RemittanceInformation11

        Private ustrdField() As String

        Private strdField() As StructuredRemittanceInformation13

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Ustrd")> _
        Public Property Ustrd() As String()
            Get
                Return Me.ustrdField
            End Get
            Set(value As String())
                Me.ustrdField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class MandateSetupReason1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType11

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType11
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType11)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType11

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class FrequencyPeriod1

        Private tpField As Frequency6Code

        Private cntPerPrdField As Decimal

        '''<remarks/>
        Public Property Tp() As Frequency6Code
            Get
                Return Me.tpField
            End Get
            Set(value As Frequency6Code)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CntPerPrd() As Decimal
            Get
                Return Me.cntPerPrdField
            End Get
            Set(value As Decimal)
                Me.cntPerPrdField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum Frequency6Code

        '''<remarks/>
        YEAR

        '''<remarks/>
        MNTH

        '''<remarks/>
        QURT

        '''<remarks/>
        MIAN

        '''<remarks/>
        WEEK

        '''<remarks/>
        DAIL

        '''<remarks/>
        ADHO

        '''<remarks/>
        INDA

        '''<remarks/>
        FRTN
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class Frequency21Choice

        Private itemField As Object

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Prd", GetType(FrequencyPeriod1)), _
         System.Xml.Serialization.XmlElementAttribute("Tp", GetType(Frequency6Code))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class AmendmentInformationDetails10

        Private orgnlMndtIdField As String

        Private orgnlCdtrSchmeIdField As PartyIdentification43

        Private orgnlCdtrAgtField As BranchAndFinancialInstitutionIdentification5

        Private orgnlCdtrAgtAcctField As CashAccount24

        Private orgnlDbtrField As PartyIdentification43

        Private orgnlDbtrAcctField As CashAccount24

        Private orgnlDbtrAgtField As BranchAndFinancialInstitutionIdentification5

        Private orgnlDbtrAgtAcctField As CashAccount24

        Private orgnlFnlColltnDtField As Date

        Private orgnlFnlColltnDtFieldSpecified As Boolean

        Private orgnlFrqcyField As Frequency21Choice

        Private orgnlRsnField As MandateSetupReason1Choice

        '''<remarks/>
        Public Property OrgnlMndtId() As String
            Get
                Return Me.orgnlMndtIdField
            End Get
            Set(value As String)
                Me.orgnlMndtIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlCdtrSchmeId() As PartyIdentification43
            Get
                Return Me.orgnlCdtrSchmeIdField
            End Get
            Set(value As PartyIdentification43)
                Me.orgnlCdtrSchmeIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlCdtrAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.orgnlCdtrAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.orgnlCdtrAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlCdtrAgtAcct() As CashAccount24
            Get
                Return Me.orgnlCdtrAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.orgnlCdtrAgtAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlDbtr() As PartyIdentification43
            Get
                Return Me.orgnlDbtrField
            End Get
            Set(value As PartyIdentification43)
                Me.orgnlDbtrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlDbtrAcct() As CashAccount24
            Get
                Return Me.orgnlDbtrAcctField
            End Get
            Set(value As CashAccount24)
                Me.orgnlDbtrAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlDbtrAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.orgnlDbtrAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.orgnlDbtrAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlDbtrAgtAcct() As CashAccount24
            Get
                Return Me.orgnlDbtrAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.orgnlDbtrAgtAcctField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property OrgnlFnlColltnDt() As Date
            Get
                Return Me.orgnlFnlColltnDtField
            End Get
            Set(value As Date)
                Me.orgnlFnlColltnDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property OrgnlFnlColltnDtSpecified() As Boolean
            Get
                Return Me.orgnlFnlColltnDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.orgnlFnlColltnDtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlFrqcy() As Frequency21Choice
            Get
                Return Me.orgnlFrqcyField
            End Get
            Set(value As Frequency21Choice)
                Me.orgnlFrqcyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlRsn() As MandateSetupReason1Choice
            Get
                Return Me.orgnlRsnField
            End Get
            Set(value As MandateSetupReason1Choice)
                Me.orgnlRsnField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class BranchAndFinancialInstitutionIdentification5

        Private finInstnIdField As FinancialInstitutionIdentification8

        Private brnchIdField As BranchData2

        '''<remarks/>
        Public Property FinInstnId() As FinancialInstitutionIdentification8
            Get
                Return Me.finInstnIdField
            End Get
            Set(value As FinancialInstitutionIdentification8)
                Me.finInstnIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BrnchId() As BranchData2
            Get
                Return Me.brnchIdField
            End Get
            Set(value As BranchData2)
                Me.brnchIdField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class FinancialInstitutionIdentification8

        Private bICFIField As String

        Private clrSysMmbIdField As ClearingSystemMemberIdentification2

        Private nmField As String

        Private pstlAdrField As PostalAddress6

        Private othrField As GenericFinancialIdentification1

        '''<remarks/>
        Public Property BICFI() As String
            Get
                Return Me.bICFIField
            End Get
            Set(value As String)
                Me.bICFIField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClrSysMmbId() As ClearingSystemMemberIdentification2
            Get
                Return Me.clrSysMmbIdField
            End Get
            Set(value As ClearingSystemMemberIdentification2)
                Me.clrSysMmbIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PstlAdr() As PostalAddress6
            Get
                Return Me.pstlAdrField
            End Get
            Set(value As PostalAddress6)
                Me.pstlAdrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Othr() As GenericFinancialIdentification1
            Get
                Return Me.othrField
            End Get
            Set(value As GenericFinancialIdentification1)
                Me.othrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ClearingSystemMemberIdentification2

        Private clrSysIdField As ClearingSystemIdentification2Choice

        Private mmbIdField As String

        '''<remarks/>
        Public Property ClrSysId() As ClearingSystemIdentification2Choice
            Get
                Return Me.clrSysIdField
            End Get
            Set(value As ClearingSystemIdentification2Choice)
                Me.clrSysIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MmbId() As String
            Get
                Return Me.mmbIdField
            End Get
            Set(value As String)
                Me.mmbIdField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ClearingSystemIdentification2Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType3

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType3
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType3)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType3

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class GenericFinancialIdentification1

        Private idField As String

        Private schmeNmField As FinancialIdentificationSchemeName1Choice

        Private issrField As String

        '''<remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SchmeNm() As FinancialIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(value As FinancialIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class FinancialIdentificationSchemeName1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType4

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType4
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType4)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType4

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class BranchData2

        Private idField As String

        Private nmField As String

        Private pstlAdrField As PostalAddress6

        '''<remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PstlAdr() As PostalAddress6
            Get
                Return Me.pstlAdrField
            End Get
            Set(value As PostalAddress6)
                Me.pstlAdrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class CashAccount24

        Private idField As AccountIdentification4Choice

        Private tpField As CashAccountType2Choice

        Private ccyField As String

        Private nmField As String

        '''<remarks/>
        Public Property Id() As AccountIdentification4Choice
            Get
                Return Me.idField
            End Get
            Set(value As AccountIdentification4Choice)
                Me.idField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Tp() As CashAccountType2Choice
            Get
                Return Me.tpField
            End Get
            Set(value As CashAccountType2Choice)
                Me.tpField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Ccy() As String
            Get
                Return Me.ccyField
            End Get
            Set(value As String)
                Me.ccyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(value As String)
                Me.nmField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class AccountIdentification4Choice

        Private itemField As Object

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("IBAN", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericAccountIdentification1))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class GenericAccountIdentification1

        Private idField As String

        Private schmeNmField As AccountSchemeName1Choice

        Private issrField As String

        '''<remarks/>
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(value As String)
                Me.idField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SchmeNm() As AccountSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(value As AccountSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class AccountSchemeName1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class CashAccountType2Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType1

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType1
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType1)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType1

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class MandateRelatedInformation10

        Private mndtIdField As String

        Private DtOfSgntField As Date

        Private dtOfSgntrFieldSpecified As Boolean

        Private amdmntIndField As Boolean

        Private amdmntIndFieldSpecified As Boolean

        Private amdmntInfDtlsField As AmendmentInformationDetails10

        Private elctrncSgntrField As String

        Private frstColltnDtField As Date

        Private frstColltnDtFieldSpecified As Boolean

        Private FnlColtDtField As Date

        Private fnlColltnDtFieldSpecified As Boolean

        Private frqcyField As Frequency21Choice

        Private rsnField As MandateSetupReason1Choice

        '''<remarks/>
        Public Property MndtId() As String
            Get
                Return Me.mndtIdField
            End Get
            Set(value As String)
                Me.mndtIdField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DtOfSgntr", DataType:="date")> _
        Public Property DtOfSgnt() As Date
            Get
                Return Me.DtOfSgntField
            End Get
            Set(value As Date)
                Me.DtOfSgntField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property DtOfSgntrSpecified() As Boolean
            Get
                Return Me.dtOfSgntrFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dtOfSgntrFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AmdmntInd() As Boolean
            Get
                Return Me.amdmntIndField
            End Get
            Set(value As Boolean)
                Me.amdmntIndField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property AmdmntIndSpecified() As Boolean
            Get
                Return Me.amdmntIndFieldSpecified
            End Get
            Set(value As Boolean)
                Me.amdmntIndFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AmdmntInfDtls() As AmendmentInformationDetails10
            Get
                Return Me.amdmntInfDtlsField
            End Get
            Set(value As AmendmentInformationDetails10)
                Me.amdmntInfDtlsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ElctrncSgntr() As String
            Get
                Return Me.elctrncSgntrField
            End Get
            Set(value As String)
                Me.elctrncSgntrField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property FrstColltnDt() As Date
            Get
                Return Me.frstColltnDtField
            End Get
            Set(value As Date)
                Me.frstColltnDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property FrstColltnDtSpecified() As Boolean
            Get
                Return Me.frstColltnDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.frstColltnDtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FnlColltnDt", DataType:="date")> _
        Public Property FnlColtDt() As Date
            Get
                Return Me.FnlColtDtField
            End Get
            Set(value As Date)
                Me.FnlColtDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property FnlColltnDtSpecified() As Boolean
            Get
                Return Me.fnlColltnDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.fnlColltnDtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Frqcy() As Frequency21Choice
            Get
                Return Me.frqcyField
            End Get
            Set(value As Frequency21Choice)
                Me.frqcyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Rsn() As MandateSetupReason1Choice
            Get
                Return Me.rsnField
            End Get
            Set(value As MandateSetupReason1Choice)
                Me.rsnField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class CategoryPurpose1Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType10

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType10
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType10)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType10

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class LocalInstrument2Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType9

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType9
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType9)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType9

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ServiceLevel8Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType8

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType8
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType8)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType8

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class PaymentTypeInformation25

        Private instrPrtyField As Priority2Code

        Private instrPrtyFieldSpecified As Boolean

        Private clrChanlField As ClearingChannel2Code

        Private clrChanlFieldSpecified As Boolean

        Private svcLvlField As ServiceLevel8Choice

        Private lclInstrmField As LocalInstrument2Choice

        Private seqTpField As SequenceType3Code

        Private seqTpFieldSpecified As Boolean

        Private ctgyPurpField As CategoryPurpose1Choice

        '''<remarks/>
        Public Property InstrPrty() As Priority2Code
            Get
                Return Me.instrPrtyField
            End Get
            Set(value As Priority2Code)
                Me.instrPrtyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property InstrPrtySpecified() As Boolean
            Get
                Return Me.instrPrtyFieldSpecified
            End Get
            Set(value As Boolean)
                Me.instrPrtyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClrChanl() As ClearingChannel2Code
            Get
                Return Me.clrChanlField
            End Get
            Set(value As ClearingChannel2Code)
                Me.clrChanlField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ClrChanlSpecified() As Boolean
            Get
                Return Me.clrChanlFieldSpecified
            End Get
            Set(value As Boolean)
                Me.clrChanlFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SvcLvl() As ServiceLevel8Choice
            Get
                Return Me.svcLvlField
            End Get
            Set(value As ServiceLevel8Choice)
                Me.svcLvlField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LclInstrm() As LocalInstrument2Choice
            Get
                Return Me.lclInstrmField
            End Get
            Set(value As LocalInstrument2Choice)
                Me.lclInstrmField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SeqTp() As SequenceType3Code
            Get
                Return Me.seqTpField
            End Get
            Set(value As SequenceType3Code)
                Me.seqTpField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property SeqTpSpecified() As Boolean
            Get
                Return Me.seqTpFieldSpecified
            End Get
            Set(value As Boolean)
                Me.seqTpFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CtgyPurp")> _
        Public Property CtgyPurpl() As CategoryPurpose1Choice
            Get
                Return Me.ctgyPurpField
            End Get
            Set(value As CategoryPurpose1Choice)
                Me.ctgyPurpField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum Priority2Code

        '''<remarks/>
        HIGH

        '''<remarks/>
        NORM
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum ClearingChannel2Code

        '''<remarks/>
        RTGS

        '''<remarks/>
        RTNS

        '''<remarks/>
        MPNS

        '''<remarks/>
        BOOK
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum SequenceType3Code

        '''<remarks/>
        FRST

        '''<remarks/>
        RCUR

        '''<remarks/>
        FNAL

        '''<remarks/>
        OOFF

        '''<remarks/>
        RPRE
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class EquivalentAmount2

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        Private ccyOfTrfField As String

        '''<remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CcyOfTrf() As String
            Get
                Return Me.ccyOfTrfField
            End Get
            Set(value As String)
                Me.ccyOfTrfField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class AmountType4Choice

        Private itemField As Object

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EqvtAmt", GetType(EquivalentAmount2)), _
         System.Xml.Serialization.XmlElementAttribute("InstdAmt", GetType(ActiveOrHistoricCurrencyAndAmount))> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IsNullable:=True)> _
    Partial Public Class OriginalTransactionReference22

        Private intrBkSttlmAmtField As ActiveOrHistoricCurrencyAndAmount

        Private amtField As AmountType4Choice

        Private intrBkSttlmDtField As Date

        Private intrBkSttlmDtFieldSpecified As Boolean

        Private reqdColltnDtField As Date

        Private reqdColltnDtFieldSpecified As Boolean

        Private reqdExctnDtField As Date

        Private reqdExctnDtFieldSpecified As Boolean

        Private cdtrSchmeIdField As PartyIdentification43

        Private sttlmInfField As SettlementInstruction4

        Private pmtTpInfField As PaymentTypeInformation25

        Private pmtMtdField As PaymentMethod4Code

        Private pmtMtdFieldSpecified As Boolean

        Private mndtRltdInfField As MandateRelatedInformation10

        Private rmtInfField As RemittanceInformation11

        Private ultmtDbtrField As PartyIdentification43

        Private dbtrField As PartyIdentification43

        Private dbtrAcctField As CashAccount24

        Private dbtrAgtField As BranchAndFinancialInstitutionIdentification5

        Private dbtrAgtAcctField As CashAccount24

        Private cdtrAgtField As BranchAndFinancialInstitutionIdentification5

        Private cdtrAgtAcctField As CashAccount24

        Private cdtrField As PartyIdentification43

        Private cdtrAcctField As CashAccount24

        Private ultmtCdtrField As PartyIdentification43
        '''<summary>
        '''Document class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.mndtRltdInfField = New MandateRelatedInformation10
        End Sub
        '''<remarks/>
        Public Property IntrBkSttlmAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.intrBkSttlmAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.intrBkSttlmAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amt() As AmountType4Choice
            Get
                Return Me.amtField
            End Get
            Set(value As AmountType4Choice)
                Me.amtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property IntrBkSttlmDt() As Date
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(value As Date)
                Me.intrBkSttlmDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property IntrBkSttlmDtSpecified() As Boolean
            Get
                Return Me.intrBkSttlmDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.intrBkSttlmDtFieldSpecified = value
            End Set
        End Property



        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ReqdColltnDt", DataType:="date")> _
        Public Property ReqdColnDt() As Date
            Get
                Return Me.reqdColltnDtField
            End Get
            Set(value As Date)
                Me.reqdColltnDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ReqdColltnDtSpecified() As Boolean
            Get
                Return Me.reqdColltnDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.reqdColltnDtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property ReqdExctnDt() As Date
            Get
                Return Me.reqdExctnDtField
            End Get
            Set(value As Date)
                Me.reqdExctnDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ReqdExctnDtSpecified() As Boolean
            Get
                Return Me.reqdExctnDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.reqdExctnDtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property CdtrSchmeId() As PartyIdentification43
            Get
                Return Me.cdtrSchmeIdField
            End Get
            Set(value As PartyIdentification43)
                Me.cdtrSchmeIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SttlmInf() As SettlementInstruction4
            Get
                Return Me.sttlmInfField
            End Get
            Set(value As SettlementInstruction4)
                Me.sttlmInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PmtTpInf() As PaymentTypeInformation25
            Get
                Return Me.pmtTpInfField
            End Get
            Set(value As PaymentTypeInformation25)
                Me.pmtTpInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PmtMtd() As PaymentMethod4Code
            Get
                Return Me.pmtMtdField
            End Get
            Set(value As PaymentMethod4Code)
                Me.pmtMtdField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property PmtMtdSpecified() As Boolean
            Get
                Return Me.pmtMtdFieldSpecified
            End Get
            Set(value As Boolean)
                Me.pmtMtdFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property MndtRltdInf() As MandateRelatedInformation10
            Get
                Return Me.mndtRltdInfField
            End Get
            Set(value As MandateRelatedInformation10)
                Me.mndtRltdInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RmtInf() As RemittanceInformation11
            Get
                Return Me.rmtInfField
            End Get
            Set(value As RemittanceInformation11)
                Me.rmtInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UltmtDbtr() As PartyIdentification43
            Get
                Return Me.ultmtDbtrField
            End Get
            Set(value As PartyIdentification43)
                Me.ultmtDbtrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Dbtr() As PartyIdentification43
            Get
                Return Me.dbtrField
            End Get
            Set(value As PartyIdentification43)
                Me.dbtrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DbtrAcct() As CashAccount24
            Get
                Return Me.dbtrAcctField
            End Get
            Set(value As CashAccount24)
                Me.dbtrAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DbtrAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.dbtrAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.dbtrAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DbtrAgtAcct() As CashAccount24
            Get
                Return Me.dbtrAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.dbtrAgtAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CdtrAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.cdtrAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.cdtrAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CdtrAgtAcct() As CashAccount24
            Get
                Return Me.cdtrAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.cdtrAgtAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Cdtr() As PartyIdentification43
            Get
                Return Me.cdtrField
            End Get
            Set(value As PartyIdentification43)
                Me.cdtrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CdtrAcct() As CashAccount24
            Get
                Return Me.cdtrAcctField
            End Get
            Set(value As CashAccount24)
                Me.cdtrAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UltmtCdtr() As PartyIdentification43
            Get
                Return Me.ultmtCdtrField
            End Get
            Set(value As PartyIdentification43)
                Me.ultmtCdtrField = value
            End Set
        End Property
        ' '''<remarks/>
        '<System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        'Public Property ReqdColltnDt() As Date
        '    Get
        '        Return Me.reqdColltnDtField
        '    End Get
        '    Set(value As Date)
        '        Me.reqdColltnDtField = value
        '    End Set
        'End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
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

        '''<remarks/>
        Public Property SttlmMtd() As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(value As SettlementMethod1Code)
                Me.sttlmMtdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SttlmAcct() As CashAccount24
            Get
                Return Me.sttlmAcctField
            End Get
            Set(value As CashAccount24)
                Me.sttlmAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClrSys() As ClearingSystemIdentification3Choice
            Get
                Return Me.clrSysField
            End Get
            Set(value As ClearingSystemIdentification3Choice)
                Me.clrSysField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InstgRmbrsmntAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instgRmbrsmntAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instgRmbrsmntAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InstgRmbrsmntAgtAcct() As CashAccount24
            Get
                Return Me.instgRmbrsmntAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.instgRmbrsmntAgtAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InstdRmbrsmntAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instdRmbrsmntAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instdRmbrsmntAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InstdRmbrsmntAgtAcct() As CashAccount24
            Get
                Return Me.instdRmbrsmntAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.instdRmbrsmntAgtAcctField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThrdRmbrsmntAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.thrdRmbrsmntAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.thrdRmbrsmntAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ThrdRmbrsmntAgtAcct() As CashAccount24
            Get
                Return Me.thrdRmbrsmntAgtAcctField
            End Get
            Set(value As CashAccount24)
                Me.thrdRmbrsmntAgtAcctField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum SettlementMethod1Code

        '''<remarks/>
        INDA

        '''<remarks/>
        INGA

        '''<remarks/>
        COVE

        '''<remarks/>
        CLRG
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ClearingSystemIdentification3Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType2

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType2
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType2)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType2

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum PaymentMethod4Code

        '''<remarks/>
        CHK

        '''<remarks/>
        TRF

        '''<remarks/>
        DD

        '''<remarks/>
        TRA
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class Charges2

        Private amtField As ActiveOrHistoricCurrencyAndAmount

        Private agtField As BranchAndFinancialInstitutionIdentification5

        '''<remarks/>
        Public Property Amt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Agt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.agtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.agtField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class OriginalGroupInformation3
        Inherits EntityBase(Of OriginalGroupInformation3)
        Private orgnlMsgIdField As String

        Private orgnlMsgNmIdField As String

        Private orgnlCreDtTmField As Date

        Private orgnlCreDtTmFieldSpecified As Boolean


        '''<remarks/>
        Public Property OrgnlMsgId() As String
            Get
                Return Me.orgnlMsgIdField
            End Get
            Set(value As String)
                Me.orgnlMsgIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlMsgNmId() As String
            Get
                Return Me.orgnlMsgNmIdField
            End Get
            Set(value As String)
                Me.orgnlMsgNmIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlCreDtTm() As Date
            Get
                Return Me.orgnlCreDtTmField
            End Get
            Set(value As Date)
                Me.orgnlCreDtTmField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property OrgnlCreDtTmSpecified() As Boolean
            Get
                Return Me.orgnlCreDtTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.orgnlCreDtTmFieldSpecified = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class PaymentTransaction65

        Private rtrIdField As String

        Private orgnlGrpInfField As OriginalGroupInformation3

        Private orgnlInstrIdField As String

        Private orgnlEndToEndIdField As String

        Private orgnlTxIdField As String

        Private orgnlClrSysRefField As String

        Private orgnlIntrBkSttlmAmtField As ActiveOrHistoricCurrencyAndAmount

        Private rtrdIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        Private intrBkSttlmDtField As Date

        Private intrBkSttlmDtFieldSpecified As Boolean

        Private sttlmPrtyField As Priority3Code

        Private sttlmPrtyFieldSpecified As Boolean

        Private rtrdInstdAmtField As ActiveOrHistoricCurrencyAndAmount

        Private xchgRateField As Decimal

        Private xchgRateFieldSpecified As Boolean

        Private compstnAmtField As ActiveOrHistoricCurrencyAndAmount

        Private chrgBrField As ChargeBearerType1Code

        Private chrgBrFieldSpecified As Boolean

        Private chrgsInfField() As Charges2

        Private instgAgtField As BranchAndFinancialInstitutionIdentification5

        Private instdAgtField As BranchAndFinancialInstitutionIdentification5

        Private rtrRsnInfField() As PaymentReturnReason1

        Private orgnlTxRefField As OriginalTransactionReference22

        Private splmtryDataField() As SupplementaryData1


        '''<summary>
        '''Document class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.OrgnlGrpInf = New OriginalGroupInformation3
            Me.OrgnlTxRef = New OriginalTransactionReference22
        End Sub

        '''<remarks/>
        Public Property RtrId() As String
            Get
                Return Me.rtrIdField
            End Get
            Set(value As String)
                Me.rtrIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlGrpInf() As OriginalGroupInformation3
            Get
                Return Me.orgnlGrpInfField
            End Get
            Set(value As OriginalGroupInformation3)
                Me.orgnlGrpInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlInstrId() As String
            Get
                Return Me.orgnlInstrIdField
            End Get
            Set(value As String)
                Me.orgnlInstrIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlEndToEndId() As String
            Get
                Return Me.orgnlEndToEndIdField
            End Get
            Set(value As String)
                Me.orgnlEndToEndIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlTxId() As String
            Get
                Return Me.orgnlTxIdField
            End Get
            Set(value As String)
                Me.orgnlTxIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlClrSysRef() As String
            Get
                Return Me.orgnlClrSysRefField
            End Get
            Set(value As String)
                Me.orgnlClrSysRefField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlIntrBkSttlmAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.orgnlIntrBkSttlmAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.orgnlIntrBkSttlmAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RtrdIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.rtrdIntrBkSttlmAmtField
            End Get
            Set(value As ActiveCurrencyAndAmount)
                Me.rtrdIntrBkSttlmAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property IntrBkSttlmDt() As Date
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(value As Date)
                Me.intrBkSttlmDtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property IntrBkSttlmDtSpecified() As Boolean
            Get
                Return Me.intrBkSttlmDtFieldSpecified
            End Get
            Set(value As Boolean)
                Me.intrBkSttlmDtFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SttlmPrty() As Priority3Code
            Get
                Return Me.sttlmPrtyField
            End Get
            Set(value As Priority3Code)
                Me.sttlmPrtyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property SttlmPrtySpecified() As Boolean
            Get
                Return Me.sttlmPrtyFieldSpecified
            End Get
            Set(value As Boolean)
                Me.sttlmPrtyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property RtrdInstdAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.rtrdInstdAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.rtrdInstdAmtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property XchgRate() As Decimal
            Get
                Return Me.xchgRateField
            End Get
            Set(value As Decimal)
                Me.xchgRateField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property XchgRateSpecified() As Boolean
            Get
                Return Me.xchgRateFieldSpecified
            End Get
            Set(value As Boolean)
                Me.xchgRateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property CompstnAmt() As ActiveOrHistoricCurrencyAndAmount
            Get
                Return Me.compstnAmtField
            End Get
            Set(value As ActiveOrHistoricCurrencyAndAmount)
                Me.compstnAmtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ChrgBr")> _
        Public Property ChrgBrM() As ChargeBearerType1Code
            Get
                Return Me.chrgBrField
            End Get
            Set(value As ChargeBearerType1Code)
                Me.chrgBrField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ChrgBrSpecified() As Boolean
            Get
                Return Me.chrgBrFieldSpecified
            End Get
            Set(value As Boolean)
                Me.chrgBrFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ChrgsInf")> _
        Public Property ChrgsInf() As Charges2()
            Get
                Return Me.chrgsInfField
            End Get
            Set(value As Charges2())
                Me.chrgsInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InstgAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instgAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instgAgtField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InstdAgt() As BranchAndFinancialInstitutionIdentification5
            Get
                Return Me.instdAgtField
            End Get
            Set(value As BranchAndFinancialInstitutionIdentification5)
                Me.instdAgtField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RtrRsnInf")> _
        Public Property RtrRsnInf() As PaymentReturnReason1()
            Get
                Return Me.rtrRsnInfField
            End Get
            Set(value As PaymentReturnReason1())
                Me.rtrRsnInfField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlTxRef() As OriginalTransactionReference22
            Get
                Return Me.orgnlTxRefField
            End Get
            Set(value As OriginalTransactionReference22)
                Me.orgnlTxRefField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ActiveCurrencyAndAmount

        Private ccyField As String

        Private valueField As Decimal

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Ccy() As String
            Get
                Return Me.ccyField
            End Get
            Set(value As String)
                Me.ccyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value() As Decimal
            Get
                Return Me.valueField
            End Get
            Set(value As Decimal)
                Me.valueField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum Priority3Code

        '''<remarks/>
        URGT

        '''<remarks/>
        HIGH

        '''<remarks/>
        NORM
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Public Enum ChargeBearerType1Code

        '''<remarks/>
        DEBT

        '''<remarks/>
        CRED

        '''<remarks/>
        SHAR

        '''<remarks/>
        SLEV
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class PaymentReturnReason1
        Inherits EntityBase(Of PaymentReturnReason1)
        Private orgtrField As PartyIdentification43

        Private rsnField As ReturnReason5Choice

        Private addtlInfField() As String

        '''<remarks/>
        Public Property Orgtr() As PartyIdentification43
            Get
                Return Me.orgtrField
            End Get
            Set(value As PartyIdentification43)
                Me.orgtrField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Rsn() As ReturnReason5Choice
            Get
                Return Me.rsnField
            End Get
            Set(value As ReturnReason5Choice)
                Me.rsnField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddtlInf")> _
        Public Property AddtlInf() As String()
            Get
                Return Me.addtlInfField
            End Get
            Set(value As String())
                Me.addtlInfField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class ReturnReason5Choice

        Private itemField As String

        Private itemElementNameField As ItemChoiceType7

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType7
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType7)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType7

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.06")> _
    Partial Public Class OriginalGroupHeader2

        Private orgnlMsgIdField As String

        Private orgnlMsgNmIdField As String

        Private orgnlCreDtTmField As Date

        Private orgnlCreDtTmFieldSpecified As Boolean

        Private rtrRsnInfField() As PaymentReturnReason1

        '''<remarks/>
        Public Property OrgnlMsgId() As String
            Get
                Return Me.orgnlMsgIdField
            End Get
            Set(value As String)
                Me.orgnlMsgIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlMsgNmId() As String
            Get
                Return Me.orgnlMsgNmIdField
            End Get
            Set(value As String)
                Me.orgnlMsgNmIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OrgnlCreDtTm() As Date
            Get
                Return Me.orgnlCreDtTmField
            End Get
            Set(value As Date)
                Me.orgnlCreDtTmField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property OrgnlCreDtTmSpecified() As Boolean
            Get
                Return Me.orgnlCreDtTmFieldSpecified
            End Get
            Set(value As Boolean)
                Me.orgnlCreDtTmFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RtrRsnInf")> _
        Public Property RtrRsnInf() As PaymentReturnReason1()
            Get
                Return Me.rtrRsnInfField
            End Get
            Set(value As PaymentReturnReason1())
                Me.rtrRsnInfField = value
            End Set
        End Property
    End Class
End Namespace
