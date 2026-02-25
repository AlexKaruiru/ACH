Imports System
Imports System.Diagnostics
Imports System.Xml.Serialization
Imports System.Collections
Imports System.Xml.Schema
Imports System.ComponentModel
Imports System.IO
Imports System.Text
Imports System.Collections.Generic

Namespace ISO.Debits

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

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=False)> _
    Partial Public Class Document
        Inherits EntityBase(Of Document)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private fIToFICstmrDrctDbtField As FIToFICustomerDirectDebitV02

        '''<summary>
        '''Document class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.fIToFICstmrDrctDbtField = New FIToFICustomerDirectDebitV02
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FIToFICstmrDrctDbt() As FIToFICustomerDirectDebitV02
            Get
                Return Me.fIToFICstmrDrctDbtField
            End Get
            Set(ByVal value As FIToFICustomerDirectDebitV02)
                Me.fIToFICstmrDrctDbtField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=True)> _
    Partial Public Class FIToFICustomerDirectDebitV02
        Inherits EntityBase(Of FIToFICustomerDirectDebitV02)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private grpHdrField As GroupHeader34

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private drctDbtTxInfField As List(Of DirectDebitTransactionInformation10)

        '''<summary>
        '''FIToFICustomerDirectDebitV02 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.drctDbtTxInfField = New List(Of DirectDebitTransactionInformation10)
            Me.grpHdrField = New GroupHeader34
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property GrpHdr() As GroupHeader34
            Get
                Return Me.grpHdrField
            End Get
            Set(ByVal value As GroupHeader34)
                Me.grpHdrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("DrctDbtTxInf", Order:=1)> _
        Public Property DrctDbtTxInf() As List(Of DirectDebitTransactionInformation10)
            Get
                Return Me.drctDbtTxInfField
            End Get
            Set(ByVal value As List(Of DirectDebitTransactionInformation10))
                Me.drctDbtTxInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=True)> _
    Partial Public Class GroupHeader34
        Inherits EntityBase(Of GroupHeader34)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private msgIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private creDtTmField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nbOfTxsField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ttlIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmInfField As SettlementInformation14

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4

        '''<summary>
        '''GroupHeader34 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.instdAgtField = New FinancialInstitution4
            Me.instgAgtField = New FinancialInstitution4
            Me.sttlmInfField = New SettlementInformation14
            Me.ttlIntrBkSttlmAmtField = New ActiveCurrencyAndAmount
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property MsgId() As String
            Get
                Return Me.msgIdField
            End Get
            Set(ByVal value As String)
                Me.msgIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property CreDtTm() As Date
            Get
                Return Me.creDtTmField
            End Get
            Set(ByVal value As Date)
                Me.creDtTmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property NbOfTxs() As String
            Get
                Return Me.nbOfTxsField
            End Get
            Set(ByVal value As String)
                Me.nbOfTxsField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property TtlIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.ttlIntrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.ttlIntrBkSttlmAmtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=4)> _
        Public Property IntrBkSttlmDt() As Date
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(ByVal value As Date)
                Me.intrBkSttlmDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property SttlmInf() As SettlementInformation14
            Get
                Return Me.sttlmInfField
            End Get
            Set(ByVal value As SettlementInformation14)
                Me.sttlmInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property InstdAgt() As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class ActiveCurrencyAndAmount
        Inherits EntityBase(Of ActiveCurrencyAndAmount)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ccyField As ActiveCurrencyCode

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private valueField As String

        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Ccy() As ActiveCurrencyCode
            Get
                Return Me.ccyField
            End Get
            Set(ByVal value As ActiveCurrencyCode)
                Me.ccyField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set(ByVal value As String)
                Me.valueField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Public Enum ActiveCurrencyCode


        '''<remarks/>
        ETB = 0

        '''<remarks/>
        USD = 1

        '''<remarks/>
        EUR = 3

        '''<remarks/>
        GBP = 2

        '''<remarks/>
        JPY = 4

        '''<remarks/>
        KES = 5
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class CreditorReferenceType1Choice
        Inherits EntityBase(Of CreditorReferenceType1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As DocumentType3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As DocumentType3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As DocumentType3Code)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Public Enum DocumentType3Code

        '''<remarks/>
        SCOR
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class CreditorReferenceType2
        Inherits EntityBase(Of CreditorReferenceType2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdOrPrtryField As CreditorReferenceType1Choice

        '''<summary>
        '''CreditorReferenceType2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdOrPrtryField = New CreditorReferenceType1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdOrPrtry() As CreditorReferenceType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(ByVal value As CreditorReferenceType1Choice)
                Me.cdOrPrtryField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class CreditorReferenceInformation2
        Inherits EntityBase(Of CreditorReferenceInformation2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private tpField As CreditorReferenceType2

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private refField As String

        '''<summary>
        '''CreditorReferenceInformation2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.tpField = New CreditorReferenceType2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Tp() As CreditorReferenceType2
            Get
                Return Me.tpField
            End Get
            Set(ByVal value As CreditorReferenceType2)
                Me.tpField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Ref() As String
            Get
                Return Me.refField
            End Get
            Set(ByVal value As String)
                Me.refField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class StructuredRemittanceInformation7
        Inherits EntityBase(Of StructuredRemittanceInformation7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrRefInfField As CreditorReferenceInformation2

        '''<summary>
        '''StructuredRemittanceInformation7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdtrRefInfField = New CreditorReferenceInformation2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdtrRefInf() As CreditorReferenceInformation2
            Get
                Return Me.cdtrRefInfField
            End Get
            Set(ByVal value As CreditorReferenceInformation2)
                Me.cdtrRefInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class RemittanceInformation5
        Inherits EntityBase(Of RemittanceInformation5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Strd", GetType(StructuredRemittanceInformation7), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Ustrd", GetType(String), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class Purpose2Choice
        Inherits EntityBase(Of Purpose2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PartyIdentification32
        Inherits EntityBase(Of PartyIdentification32)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification32 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class Party6Choice
        Inherits EntityBase(Of Party6Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("OrgId", GetType(OrganisationIdentification4), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("PrvtId", GetType(PersonIdentification5), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class OrganisationIdentification4
        Inherits EntityBase(Of OrganisationIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("BICOrBEI", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericOrganisationIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class GenericOrganisationIdentification1
        Inherits EntityBase(Of GenericOrganisationIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As OrganisationIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericOrganisationIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New OrganisationIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As OrganisationIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As OrganisationIdentificationSchemeName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class OrganisationIdentificationSchemeName1Choice
        Inherits EntityBase(Of OrganisationIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType1

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType1
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType1)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType1

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PersonIdentification5
        Inherits EntityBase(Of PersonIdentification5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("DtAndPlcOfBirth", GetType(DateAndPlaceOfBirth), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericPersonIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class DateAndPlaceOfBirth
        Inherits EntityBase(Of DateAndPlaceOfBirth)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private birthDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prvcOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cityOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryOfBirthField As String

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=0)> _
        Public Property BirthDt() As Date
            Get
                Return Me.birthDtField
            End Get
            Set(ByVal value As Date)
                Me.birthDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property PrvcOfBirth() As String
            Get
                Return Me.prvcOfBirthField
            End Get
            Set(ByVal value As String)
                Me.prvcOfBirthField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property CityOfBirth() As String
            Get
                Return Me.cityOfBirthField
            End Get
            Set(ByVal value As String)
                Me.cityOfBirthField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property CtryOfBirth() As String
            Get
                Return Me.ctryOfBirthField
            End Get
            Set(ByVal value As String)
                Me.ctryOfBirthField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class GenericPersonIdentification1
        Inherits EntityBase(Of GenericPersonIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As PersonIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericPersonIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New PersonIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As PersonIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As PersonIdentificationSchemeName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PersonIdentificationSchemeName1Choice
        Inherits EntityBase(Of PersonIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType2

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType2
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType2)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType2

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class AccountIdentification4Choice
        Inherits EntityBase(Of AccountIdentification4Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class CashAccount17
        Inherits EntityBase(Of CashAccount17)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As AccountIdentification4Choice

        '''<summary>
        '''CashAccount17 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New AccountIdentification4Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As AccountIdentification4Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentification4Choice)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PostalAddress7
        Inherits EntityBase(Of PostalAddress7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private adrLineField As List(Of String)

        '''<summary>
        '''PostalAddress7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.adrLineField = New List(Of String)
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(ByVal value As String)
                Me.ctryField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("AdrLine", Order:=1)> _
        Public Property AdrLine() As List(Of String)
            Get
                Return Me.adrLineField
            End Get
            Set(ByVal value As List(Of String))
                Me.adrLineField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PartyIdentification33
        Inherits EntityBase(Of PartyIdentification33)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private pstlAdrField As PostalAddress7

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification33 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
            ' Me.pstlAdrField = New PostalAddress7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        'Public Property PstlAdr() As PostalAddress7
        '    Get
        '        Return Me.pstlAdrField
        '    End Get
        '    Set(ByVal value As PostalAddress7)
        '        Me.pstlAdrField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=True)> _
    Partial Public Class PartyIdentification36
        Inherits EntityBase(Of PartyIdentification36)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As PartyPrivate1

        '''<summary>
        '''PartyIdentification36 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New PartyPrivate1
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As PartyPrivate1
            Get
                Return Me.idField
            End Get
            Set(ByVal value As PartyPrivate1)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PartyPrivate1
        Inherits EntityBase(Of PartyPrivate1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As PersonIdentification4

        '''<summary>
        '''PartyPrivate1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.itemField = New PersonIdentification4
        End Sub

        <System.Xml.Serialization.XmlElementAttribute("PrvtId", Order:=0)> _
        Public Property Item() As PersonIdentification4
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As PersonIdentification4)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PersonIdentification4
        Inherits EntityBase(Of PersonIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private othrField As RestrictedIdentification2

        '''<summary>
        '''PersonIdentification4 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.othrField = New RestrictedIdentification2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Othr() As RestrictedIdentification2
            Get
                Return Me.othrField
            End Get
            Set(ByVal value As RestrictedIdentification2)
                Me.othrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class RestrictedIdentification2
        Inherits EntityBase(Of RestrictedIdentification2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As RestrictedPersonIdentificationSchemaName2Choice

        '''<summary>
        '''RestrictedIdentification2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New RestrictedPersonIdentificationSchemaName2Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As RestrictedPersonIdentificationSchemaName2Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As RestrictedPersonIdentificationSchemaName2Choice)
                Me.schmeNmField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class RestrictedPersonIdentificationSchemaName2Choice
        Inherits EntityBase(Of RestrictedPersonIdentificationSchemaName2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prtryField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Prtry() As String
            Get
                Return Me.prtryField
            End Get
            Set(ByVal value As String)
                Me.prtryField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class RestrictedInstitutionSchemaName1Choice
        Inherits EntityBase(Of RestrictedInstitutionSchemaName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prtryField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Prtry() As String
            Get
                Return Me.prtryField
            End Get
            Set(ByVal value As String)
                Me.prtryField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class RestrictedIdentification1
        Inherits EntityBase(Of RestrictedIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As RestrictedInstitutionSchemaName1Choice

        '''<summary>
        '''RestrictedIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New RestrictedInstitutionSchemaName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As RestrictedInstitutionSchemaName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As RestrictedInstitutionSchemaName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class FinancialInstitutionIdentification8
        Inherits EntityBase(Of FinancialInstitutionIdentification8)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private othrField As RestrictedIdentification1

        '''<summary>
        '''FinancialInstitutionIdentification8 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.othrField = New RestrictedIdentification1
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Othr() As RestrictedIdentification1
            Get
                Return Me.othrField
            End Get
            Set(ByVal value As RestrictedIdentification1)
                Me.othrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class FinancialInstitution5
        Inherits EntityBase(Of FinancialInstitution5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private finInstnIdField As FinancialInstitutionIdentification8

        '''<summary>
        '''FinancialInstitution5 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.finInstnIdField = New FinancialInstitutionIdentification8
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FinInstnId() As FinancialInstitutionIdentification8
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification8)
                Me.finInstnIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class AccountIdentificationIBAN
        Inherits EntityBase(Of AccountIdentificationIBAN)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class CashAccount16
        Inherits EntityBase(Of CashAccount16)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As AccountIdentificationIBAN

        '''<summary>
        '''CashAccount16 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New AccountIdentificationIBAN
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As AccountIdentificationIBAN
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentificationIBAN)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PartyIdentification35
        Inherits EntityBase(Of PartyIdentification35)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As PartyPrivate1

        '''<summary>
        '''PartyIdentification35 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New PartyPrivate1
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As PartyPrivate1
            Get
                Return Me.idField
            End Get
            Set(ByVal value As PartyPrivate1)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class AmendmentInformationDetails6
        Inherits EntityBase(Of AmendmentInformationDetails6)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlMndtIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlCdtrSchmeIdField As PartyIdentification35

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlDbtrAcctField As CashAccount16

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlDbtrAgtField As FinancialInstitution5

        '''<summary>
        '''AmendmentInformationDetails6 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.orgnlDbtrAgtField = New FinancialInstitution5
            Me.orgnlDbtrAcctField = New CashAccount16
            Me.orgnlCdtrSchmeIdField = New PartyIdentification35
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property OrgnlMndtId() As String
            Get
                Return Me.orgnlMndtIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMndtIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property OrgnlCdtrSchmeId() As PartyIdentification35
            Get
                Return Me.orgnlCdtrSchmeIdField
            End Get
            Set(ByVal value As PartyIdentification35)
                Me.orgnlCdtrSchmeIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property OrgnlDbtrAcct() As CashAccount16
            Get
                Return Me.orgnlDbtrAcctField
            End Get
            Set(ByVal value As CashAccount16)
                Me.orgnlDbtrAcctField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property OrgnlDbtrAgt() As FinancialInstitution5
            Get
                Return Me.orgnlDbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution5)
                Me.orgnlDbtrAgtField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class MandateRelatedInformation6
        Inherits EntityBase(Of MandateRelatedInformation6)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private mndtIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dtOfSgntrField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private amdmntIndField As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private amdmntIndFieldSpecified As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private amdmntInfDtlsField As AmendmentInformationDetails6

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private elctrncSgntrField As String

        '''<summary>
        '''MandateRelatedInformation6 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.amdmntInfDtlsField = New AmendmentInformationDetails6
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property MndtId() As String
            Get
                Return Me.mndtIdField
            End Get
            Set(ByVal value As String)
                Me.mndtIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=1)> _
        Public Property DtOfSgntr() As Date
            Get
                Return Me.dtOfSgntrField
            End Get
            Set(ByVal value As Date)
                Me.dtOfSgntrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property AmdmntInd() As Boolean
            Get
                Return Me.amdmntIndField
            End Get
            Set(ByVal value As Boolean)
                Me.amdmntIndField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property AmdmntIndSpecified() As Boolean
            Get
                Return Me.amdmntIndFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.amdmntIndFieldSpecified = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property AmdmntInfDtls() As AmendmentInformationDetails6
            Get
                Return Me.amdmntInfDtlsField
            End Get
            Set(ByVal value As AmendmentInformationDetails6)
                Me.amdmntInfDtlsField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property ElctrncSgntr() As String
            Get
                Return Me.elctrncSgntrField
            End Get
            Set(ByVal value As String)
                Me.elctrncSgntrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=True)> _
    Partial Public Class DirectDebitTransaction6
        Inherits EntityBase(Of DirectDebitTransaction6)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private mndtRltdInfField As MandateRelatedInformation6

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrSchmeIdField As PartyIdentification36

        '''<summary>
        '''DirectDebitTransaction6 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdtrSchmeIdField = New PartyIdentification36
            Me.mndtRltdInfField = New MandateRelatedInformation6
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property MndtRltdInf() As MandateRelatedInformation6
            Get
                Return Me.mndtRltdInfField
            End Get
            Set(ByVal value As MandateRelatedInformation6)
                Me.mndtRltdInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property CdtrSchmeId() As PartyIdentification36
            Get
                Return Me.cdtrSchmeIdField
            End Get
            Set(ByVal value As PartyIdentification36)
                Me.cdtrSchmeIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class CategoryPurpose1Choice
        Inherits EntityBase(Of CategoryPurpose1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=True)> _
    Partial Public Class LocalInstrument3Choice
        Inherits EntityBase(Of LocalInstrument3Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class ServiceLevel9Choice
        Inherits EntityBase(Of ServiceLevel9Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ServiceLevel3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As ServiceLevel3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ServiceLevel3Code)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Public Enum ServiceLevel3Code

        '''<remarks/>
        SEPA
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=True)> _
    Partial Public Class PaymentTypeInformation22
        Inherits EntityBase(Of PaymentTypeInformation22)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private svcLvlField As ServiceLevel9Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private lclInstrmField As LocalInstrument3Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private seqTpField As SequenceType1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctgyPurpField As CategoryPurpose1Choice

        '''<summary>
        '''PaymentTypeInformation22 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            ' Me.ctgyPurpField = New CategoryPurpose1Choice
            Me.lclInstrmField = New LocalInstrument3Choice
            Me.svcLvlField = New ServiceLevel9Choice
            Me.seqTpField = New SequenceType1Code
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SvcLvl() As ServiceLevel9Choice
            Get
                Return Me.svcLvlField
            End Get
            Set(ByVal value As ServiceLevel9Choice)
                Me.svcLvlField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property LclInstrm() As LocalInstrument3Choice
            Get
                Return Me.lclInstrmField
            End Get
            Set(ByVal value As LocalInstrument3Choice)
                Me.lclInstrmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property SeqTp() As SequenceType1Code
            Get
                Return Me.seqTpField
            End Get
            Set(ByVal value As SequenceType1Code)
                Me.seqTpField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        'Public Property CtgyPurp() As CategoryPurpose1Choice
        '    Get
        '        Return Me.ctgyPurpField
        '    End Get
        '    Set(ByVal value As CategoryPurpose1Choice)
        '        Me.ctgyPurpField = Value
        '    End Set
        'End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Public Enum SequenceType1Code

        '''<remarks/>
        FRST

        '''<remarks/>
        RCUR

        '''<remarks/>
        FNAL

        '''<remarks/>
        OOFF
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class PaymentIdentification3
        Inherits EntityBase(Of PaymentIdentification3)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instrIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private endToEndIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private txIdField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property InstrId() As String
            Get
                Return Me.instrIdField
            End Get
            Set(ByVal value As String)
                Me.instrIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property EndToEndId() As String
            Get
                Return Me.endToEndIdField
            End Get
            Set(ByVal value As String)
                Me.endToEndIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property TxId() As String
            Get
                Return Me.txIdField
            End Get
            Set(ByVal value As String)
                Me.txIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=True)> _
    Partial Public Class DirectDebitTransactionInformation10
        Inherits EntityBase(Of DirectDebitTransactionInformation10)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtIdField As PaymentIdentification3

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtTpInfField As PaymentTypeInformation22

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private chrgBrField As ChargeBearerType1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private reqdColltnDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private drctDbtTxField As DirectDebitTransaction6

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAgtField As FinancialInstitution4

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private ultmtCdtrField As PartyIdentification32

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private IntrBkSttlmDt As PartyIdentification32

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private purpField As Purpose2Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private rmtInfField As RemittanceInformation5

        '''<summary>
        '''DirectDebitTransactionInformation10 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.rmtInfField = New RemittanceInformation5
            Me.purpField = New Purpose2Choice
            'Me.IntrBkSttlmDtField = New PartyIdentification32
            Me.dbtrAgtField = New FinancialInstitution4
            Me.dbtrAcctField = New CashAccount17
            Me.dbtrField = New PartyIdentification33
            Me.instdAgtField = New FinancialInstitution4
            Me.instgAgtField = New FinancialInstitution4
            '  Me.ultmtCdtrField = New PartyIdentification32
            Me.cdtrAgtField = New FinancialInstitution4
            Me.cdtrAcctField = New CashAccount17
            Me.cdtrField = New PartyIdentification33
            Me.drctDbtTxField = New DirectDebitTransaction6
            Me.intrBkSttlmAmtField = New ActiveCurrencyAndAmount
            Me.pmtTpInfField = New PaymentTypeInformation22
            Me.pmtIdField = New PaymentIdentification3
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property PmtId() As PaymentIdentification3
            Get
                Return Me.pmtIdField
            End Get
            Set(ByVal value As PaymentIdentification3)
                Me.pmtIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property PmtTpInf() As PaymentTypeInformation22
            Get
                Return Me.pmtTpInfField
            End Get
            Set(ByVal value As PaymentTypeInformation22)
                Me.pmtTpInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property IntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.intrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.intrBkSttlmAmtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property ChrgBr() As ChargeBearerType1Code
            Get
                Return Me.chrgBrField
            End Get
            Set(ByVal value As ChargeBearerType1Code)
                Me.chrgBrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=4)> _
        Public Property ReqdColltnDt() As Date
            Get
                Return Me.reqdColltnDtField
            End Get
            Set(ByVal value As Date)
                Me.reqdColltnDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property DrctDbtTx() As DirectDebitTransaction6
            Get
                Return Me.drctDbtTxField
            End Get
            Set(ByVal value As DirectDebitTransaction6)
                Me.drctDbtTxField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property Cdtr() As PartyIdentification33
            Get
                Return Me.cdtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.cdtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property CdtrAcct() As CashAccount17
            Get
                Return Me.cdtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.cdtrAcctField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)> _
        Public Property CdtrAgt() As FinancialInstitution4
            Get
                Return Me.cdtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.cdtrAgtField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=9)> _
        'Public Property UltmtCdtr() As PartyIdentification32
        '    Get
        '        Return Me.ultmtCdtrField
        '    End Get
        '    Set(ByVal value As PartyIdentification32)
        '        Me.ultmtCdtrField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=10)> _
        Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=11)> _
        Public Property InstdAgt() As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=12)> _
        Public Property Dbtr() As PartyIdentification33
            Get
                Return Me.dbtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.dbtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=13)> _
        Public Property DbtrAcct() As CashAccount17
            Get
                Return Me.dbtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.dbtrAcctField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=14)> _
        Public Property DbtrAgt() As FinancialInstitution4
            Get
                Return Me.dbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.dbtrAgtField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=15)> _
        'Public Property UltmtDbtr() As PartyIdentification32
        '    Get
        '        Return Me.ultmtDbtrField
        '    End Get
        '    Set(ByVal value As PartyIdentification32)
        '        Me.ultmtDbtrField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=15)> _
        Public Property Purp() As Purpose2Choice
            Get
                Return Me.purpField
            End Get
            Set(ByVal value As Purpose2Choice)
                Me.purpField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=16)> _
        Public Property RmtInf() As RemittanceInformation5
            Get
                Return Me.rmtInfField
            End Get
            Set(ByVal value As RemittanceInformation5)
                Me.rmtInfField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Public Enum ChargeBearerType1Code

        '''<remarks/>
        SLEV
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class FinancialInstitution4
        Inherits EntityBase(Of FinancialInstitution4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private finInstnIdField As FinancialInstitutionIdentification7

        '''<summary>
        '''FinancialInstitution4 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.finInstnIdField = New FinancialInstitutionIdentification7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FinInstnId() As FinancialInstitutionIdentification7
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification7)
                Me.finInstnIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class FinancialInstitutionIdentification7
        Inherits EntityBase(Of FinancialInstitutionIdentification7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private bICField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property BIC() As String
            Get
                Return Me.bICField
            End Get
            Set(ByVal value As String)
                Me.bICField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Partial Public Class ClearingSystemIdentification3Choice
        Inherits EntityBase(Of ClearingSystemIdentification3Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ClearingSystemIdentification

        <System.Xml.Serialization.XmlElementAttribute("Prtry", Order:=0)> _
        Public Property Item() As ClearingSystemIdentification
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ClearingSystemIdentification)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Public Enum ClearingSystemIdentification

        '''<remarks/>
        ACH
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02", IsNullable:=True)> _
    Partial Public Class SettlementInformation14
        Inherits EntityBase(Of SettlementInformation14)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmMtdField As SettlementMethod1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private clrSysField As ClearingSystemIdentification3Choice

        '''<summary>
        '''SettlementInformation14 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.clrSysField = New ClearingSystemIdentification3Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SttlmMtd() As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(ByVal value As SettlementMethod1Code)
                Me.sttlmMtdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property ClrSys() As ClearingSystemIdentification3Choice
            Get
                Return Me.clrSysField
            End Get
            Set(ByVal value As ClearingSystemIdentification3Choice)
                Me.clrSysField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.003.001.02")> _
    Public Enum SettlementMethod1Code

        '''<remarks/>
        CLRG
    End Enum
End Namespace

Namespace ISO.Credits

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

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IsNullable:=False)> _
    Partial Public Class Document
        Inherits EntityBase(Of Document)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private fIToFICstmrCdtTrfField As FIToFICustomerCreditTransferV02

        '''<summary>
        '''Document class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.fIToFICstmrCdtTrfField = New FIToFICustomerCreditTransferV02
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FIToFICstmrCdtTrf() As FIToFICustomerCreditTransferV02
            Get
                Return Me.fIToFICstmrCdtTrfField
            End Get
            Set(ByVal value As FIToFICustomerCreditTransferV02)
                Me.fIToFICstmrCdtTrfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IsNullable:=True)> _
    Partial Public Class FIToFICustomerCreditTransferV02
        Inherits EntityBase(Of FIToFICustomerCreditTransferV02)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private grpHdrField As GroupHeader33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtTrfTxInfField As List(Of CreditTransferTransactionInformation11)

        '''<summary>
        '''FIToFICustomerCreditTransferV02 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdtTrfTxInfField = New List(Of CreditTransferTransactionInformation11)
            Me.grpHdrField = New GroupHeader33
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property GrpHdr() As GroupHeader33
            Get
                Return Me.grpHdrField
            End Get
            Set(ByVal value As GroupHeader33)
                Me.grpHdrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("CdtTrfTxInf", Order:=1)> _
        Public Property CdtTrfTxInf() As List(Of CreditTransferTransactionInformation11)
            Get
                Return Me.cdtTrfTxInfField
            End Get
            Set(ByVal value As List(Of CreditTransferTransactionInformation11))
                Me.cdtTrfTxInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IsNullable:=True)> _
    Partial Public Class GroupHeader33
        Inherits EntityBase(Of GroupHeader33)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private msgIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private creDtTmField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nbOfTxsField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ttlIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmInfField As SettlementInformation13

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4

        '''<summary>
        '''GroupHeader33 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.instdAgtField = New FinancialInstitution4
            Me.instgAgtField = New FinancialInstitution4
            Me.sttlmInfField = New SettlementInformation13
            Me.ttlIntrBkSttlmAmtField = New ActiveCurrencyAndAmount
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property MsgId() As String
            Get
                Return Me.msgIdField
            End Get
            Set(ByVal value As String)
                Me.msgIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property CreDtTm() As Date
            Get
                Return Me.creDtTmField
            End Get
            Set(ByVal value As Date)
                Me.creDtTmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property NbOfTxs() As String
            Get
                Return Me.nbOfTxsField
            End Get
            Set(ByVal value As String)
                Me.nbOfTxsField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property TtlIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.ttlIntrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.ttlIntrBkSttlmAmtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=4)> _
        Public Property IntrBkSttlmDt() As Date
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(ByVal value As Date)
                Me.intrBkSttlmDtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property SttlmInf() As SettlementInformation13
            Get
                Return Me.sttlmInfField
            End Get
            Set(ByVal value As SettlementInformation13)
                Me.sttlmInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property InstdAgt() As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class ActiveCurrencyAndAmount
        Inherits EntityBase(Of ActiveCurrencyAndAmount)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ccyField As ActiveCurrencyCode

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private valueField As String

        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Ccy() As ActiveCurrencyCode
            Get
                Return Me.ccyField
            End Get
            Set(ByVal value As ActiveCurrencyCode)
                Me.ccyField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set(ByVal value As String)
                Me.valueField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Public Enum ActiveCurrencyCode

        '''<remarks/>
        TZS = 0

        '''<remarks/>
        USD = 1

        '''<remarks/>
        EUR = 3

        '''<remarks/>
        GBP = 2

        '''<remarks/>
        JPY = 4

        '''<remarks/>
        KES = 5
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class CreditorReferenceType1Choice
        Inherits EntityBase(Of CreditorReferenceType1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As DocumentType3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As DocumentType3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As DocumentType3Code)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Public Enum DocumentType3Code

        '''<remarks/>
        SCOR
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class CreditorReferenceType2
        Inherits EntityBase(Of CreditorReferenceType2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdOrPrtryField As CreditorReferenceType1Choice

        '''<summary>
        '''CreditorReferenceType2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdOrPrtryField = New CreditorReferenceType1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdOrPrtry() As CreditorReferenceType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(ByVal value As CreditorReferenceType1Choice)
                Me.cdOrPrtryField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class CreditorReferenceInformation2
        Inherits EntityBase(Of CreditorReferenceInformation2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private tpField As CreditorReferenceType2

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private refField As String

        '''<summary>
        '''CreditorReferenceInformation2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.tpField = New CreditorReferenceType2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Tp() As CreditorReferenceType2
            Get
                Return Me.tpField
            End Get
            Set(ByVal value As CreditorReferenceType2)
                Me.tpField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Ref() As String
            Get
                Return Me.refField
            End Get
            Set(ByVal value As String)
                Me.refField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class StructuredRemittanceInformation7
        Inherits EntityBase(Of StructuredRemittanceInformation7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrRefInfField As CreditorReferenceInformation2

        '''<summary>
        '''StructuredRemittanceInformation7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdtrRefInfField = New CreditorReferenceInformation2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdtrRefInf() As CreditorReferenceInformation2
            Get
                Return Me.cdtrRefInfField
            End Get
            Set(ByVal value As CreditorReferenceInformation2)
                Me.cdtrRefInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class RemittanceInformation5
        Inherits EntityBase(Of RemittanceInformation5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Strd", GetType(StructuredRemittanceInformation7), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Ustrd", GetType(String), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class Purpose2Choice
        Inherits EntityBase(Of Purpose2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class AccountIdentification4Choice
        Inherits EntityBase(Of AccountIdentification4Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class CashAccount17
        Inherits EntityBase(Of CashAccount17)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As AccountIdentification4Choice

        '''<summary>
        '''CashAccount17 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New AccountIdentification4Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As AccountIdentification4Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentification4Choice)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class PostalAddress7
        Inherits EntityBase(Of PostalAddress7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private adrLineField As List(Of String)

        '''<summary>
        '''PostalAddress7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.adrLineField = New List(Of String)
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(ByVal value As String)
                Me.ctryField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("AdrLine", Order:=1)> _
        Public Property AdrLine() As List(Of String)
            Get
                Return Me.adrLineField
            End Get
            Set(ByVal value As List(Of String))
                Me.adrLineField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class PartyIdentification33
        Inherits EntityBase(Of PartyIdentification33)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private pstlAdrField As PostalAddress7

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification33 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
            'Me.pstlAdrField = New PostalAddress7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        'Public Property PstlAdr() As PostalAddress7
        '    Get
        '        Return Me.pstlAdrField
        '    End Get
        '    Set(ByVal value As PostalAddress7)
        '        Me.pstlAdrField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class Party6Choice
        Inherits EntityBase(Of Party6Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("OrgId", GetType(OrganisationIdentification4), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("PrvtId", GetType(PersonIdentification5), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class OrganisationIdentification4
        Inherits EntityBase(Of OrganisationIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("BICOrBEI", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericOrganisationIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class GenericOrganisationIdentification1
        Inherits EntityBase(Of GenericOrganisationIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As OrganisationIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericOrganisationIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New OrganisationIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As OrganisationIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As OrganisationIdentificationSchemeName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class OrganisationIdentificationSchemeName1Choice
        Inherits EntityBase(Of OrganisationIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType2

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType2
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType2)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType2

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class PersonIdentification5
        Inherits EntityBase(Of PersonIdentification5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("DtAndPlcOfBirth", GetType(DateAndPlaceOfBirth), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericPersonIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class DateAndPlaceOfBirth
        Inherits EntityBase(Of DateAndPlaceOfBirth)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private birthDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prvcOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cityOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryOfBirthField As String

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=0)> _
        Public Property BirthDt() As Date
            Get
                Return Me.birthDtField
            End Get
            Set(ByVal value As Date)
                Me.birthDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property PrvcOfBirth() As String
            Get
                Return Me.prvcOfBirthField
            End Get
            Set(ByVal value As String)
                Me.prvcOfBirthField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property CityOfBirth() As String
            Get
                Return Me.cityOfBirthField
            End Get
            Set(ByVal value As String)
                Me.cityOfBirthField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property CtryOfBirth() As String
            Get
                Return Me.ctryOfBirthField
            End Get
            Set(ByVal value As String)
                Me.ctryOfBirthField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class GenericPersonIdentification1
        Inherits EntityBase(Of GenericPersonIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As PersonIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericPersonIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New PersonIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As PersonIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As PersonIdentificationSchemeName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class PersonIdentificationSchemeName1Choice
        Inherits EntityBase(Of PersonIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType3

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType3
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType3)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType3

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class PartyIdentification32
        Inherits EntityBase(Of PartyIdentification32)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification32 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class CategoryPurpose1Choice
        Inherits EntityBase(Of CategoryPurpose1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType1

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType1
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType1)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType1

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class LocalInstrument2Choice
        Inherits EntityBase(Of LocalInstrument2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class ServiceLevel9Choice
        Inherits EntityBase(Of ServiceLevel9Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ServiceLevel3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As ServiceLevel3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ServiceLevel3Code)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Public Enum ServiceLevel3Code

        '''<remarks/>
        SEPA
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IsNullable:=True)> _
    Partial Public Class PaymentTypeInformation21
        Inherits EntityBase(Of PaymentTypeInformation21)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private svcLvlField As ServiceLevel9Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private lclInstrmField As LocalInstrument2Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctgyPurpField As CategoryPurpose1Choice

        '''<summary>
        '''PaymentTypeInformation21 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.ctgyPurpField = New CategoryPurpose1Choice
            Me.lclInstrmField = New LocalInstrument2Choice
            Me.svcLvlField = New ServiceLevel9Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SvcLvl() As ServiceLevel9Choice
            Get
                Return Me.svcLvlField
            End Get
            Set(ByVal value As ServiceLevel9Choice)
                Me.svcLvlField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property LclInstrm() As LocalInstrument2Choice
            Get
                Return Me.lclInstrmField
            End Get
            Set(ByVal value As LocalInstrument2Choice)
                Me.lclInstrmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property CtgyPurp() As CategoryPurpose1Choice
            Get
                Return Me.ctgyPurpField
            End Get
            Set(ByVal value As CategoryPurpose1Choice)
                Me.ctgyPurpField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class PaymentIdentification3
        Inherits EntityBase(Of PaymentIdentification3)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instrIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private endToEndIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private txIdField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property InstrId() As String
            Get
                Return Me.instrIdField
            End Get
            Set(ByVal value As String)
                Me.instrIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property EndToEndId() As String
            Get
                Return Me.endToEndIdField
            End Get
            Set(ByVal value As String)
                Me.endToEndIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property TxId() As String
            Get
                Return Me.txIdField
            End Get
            Set(ByVal value As String)
                Me.txIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02", IsNullable:=True)> _
    Partial Public Class CreditTransferTransactionInformation11
        Inherits EntityBase(Of CreditTransferTransactionInformation11)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtIdField As PaymentIdentification3

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtTpInfField As PaymentTypeInformation21

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private chrgBrField As ChargeBearerType1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private ultmtDbtrField As PartyIdentification32

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ultmtCdtrField As PartyIdentification32

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private purpField As Purpose2Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private rmtInfField As RemittanceInformation5

        '''<summary>
        '''CreditTransferTransactionInformation11 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.rmtInfField = New RemittanceInformation5
            Me.purpField = New Purpose2Choice
            Me.ultmtCdtrField = New PartyIdentification32
            Me.cdtrAcctField = New CashAccount17
            Me.cdtrField = New PartyIdentification33
            Me.cdtrAgtField = New FinancialInstitution4
            Me.dbtrAgtField = New FinancialInstitution4
            Me.dbtrAcctField = New CashAccount17
            Me.dbtrField = New PartyIdentification33
            ' Me.ultmtDbtrField = New PartyIdentification32
            Me.instdAgtField = New FinancialInstitution4
            Me.instgAgtField = New FinancialInstitution4
            Me.intrBkSttlmAmtField = New ActiveCurrencyAndAmount
            Me.pmtTpInfField = New PaymentTypeInformation21
            Me.pmtIdField = New PaymentIdentification3
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property PmtId() As PaymentIdentification3
            Get
                Return Me.pmtIdField
            End Get
            Set(ByVal value As PaymentIdentification3)
                Me.pmtIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property PmtTpInf() As PaymentTypeInformation21
            Get
                Return Me.pmtTpInfField
            End Get
            Set(ByVal value As PaymentTypeInformation21)
                Me.pmtTpInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property IntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.intrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.intrBkSttlmAmtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property ChrgBr() As ChargeBearerType1Code
            Get
                Return Me.chrgBrField
            End Get
            Set(ByVal value As ChargeBearerType1Code)
                Me.chrgBrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property InstdAgt() As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        'Public Property UltmtDbtr() As PartyIdentification32
        '    Get
        '        Return Me.ultmtDbtrField
        '    End Get
        '    Set(ByVal value As PartyIdentification32)
        '        Me.ultmtDbtrField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property Dbtr() As PartyIdentification33
            Get
                Return Me.dbtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.dbtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)> _
        Public Property DbtrAcct() As CashAccount17
            Get
                Return Me.dbtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.dbtrAcctField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=9)> _
        Public Property DbtrAgt() As FinancialInstitution4
            Get
                Return Me.dbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.dbtrAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=10)> _
        Public Property CdtrAgt() As FinancialInstitution4
            Get
                Return Me.cdtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.cdtrAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=11)> _
        Public Property Cdtr() As PartyIdentification33
            Get
                Return Me.cdtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.cdtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=12)> _
        Public Property CdtrAcct() As CashAccount17
            Get
                Return Me.cdtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.cdtrAcctField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=13)> _
        Public Property UltmtCdtr() As PartyIdentification32
            Get
                Return Me.ultmtCdtrField
            End Get
            Set(ByVal value As PartyIdentification32)
                Me.ultmtCdtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=14)> _
        Public Property Purp() As Purpose2Choice
            Get
                Return Me.purpField
            End Get
            Set(ByVal value As Purpose2Choice)
                Me.purpField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=15)> _
        Public Property RmtInf() As RemittanceInformation5
            Get
                Return Me.rmtInfField
            End Get
            Set(ByVal value As RemittanceInformation5)
                Me.rmtInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Public Enum ChargeBearerType1Code

        '''<remarks/>
        SLEV
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class FinancialInstitution4
        Inherits EntityBase(Of FinancialInstitution4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private finInstnIdField As FinancialInstitutionIdentification7

        '''<summary>
        '''FinancialInstitution4 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.finInstnIdField = New FinancialInstitutionIdentification7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FinInstnId() As FinancialInstitutionIdentification7
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification7)
                Me.finInstnIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class FinancialInstitutionIdentification7
        Inherits EntityBase(Of FinancialInstitutionIdentification7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private bICField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property BIC() As String
            Get
                Return Me.bICField
            End Get
            Set(ByVal value As String)
                Me.bICField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class ClearingSystemIdentification3Choice
        Inherits EntityBase(Of ClearingSystemIdentification3Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ClearingSystemIdentification

        <System.Xml.Serialization.XmlElementAttribute("Prtry", Order:=0)> _
        Public Property Item() As ClearingSystemIdentification
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ClearingSystemIdentification)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Public Enum ClearingSystemIdentification

        '''<remarks/>
        ACH
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Partial Public Class SettlementInformation13
        Inherits EntityBase(Of SettlementInformation13)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmMtdField As SettlementMethod1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private clrSysField As ClearingSystemIdentification3Choice

        '''<summary>
        '''SettlementInformation13 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.clrSysField = New ClearingSystemIdentification3Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SttlmMtd() As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(ByVal value As SettlementMethod1Code)
                Me.sttlmMtdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property ClrSys() As ClearingSystemIdentification3Choice
            Get
                Return Me.clrSysField
            End Get
            Set(ByVal value As ClearingSystemIdentification3Choice)
                Me.clrSysField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02")> _
    Public Enum SettlementMethod1Code

        '''<remarks/>
        CLRG
    End Enum
End Namespace

Namespace ISO.Cheques

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

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IsNullable:=True)> _
    Partial Public Class Document
        Inherits EntityBase(Of Document)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private blkChqField As CoreBlkChkType

        '''<summary>
        '''Document class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.blkChqField = New CoreBlkChkType
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property BlkChq() As CoreBlkChkType
            Get
                Return Me.blkChqField
            End Get
            Set(ByVal value As CoreBlkChkType)
                Me.blkChqField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IsNullable:=True)> _
    Partial Public Class CoreBlkChkType
        Inherits EntityBase(Of CoreBlkChkType)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private grpHdrField As GroupHeader34

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private chqField As List(Of ChequeType)

        '''<summary>
        '''CoreBlkChkType class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.chqField = New List(Of ChequeType)
            Me.grpHdrField = New GroupHeader34
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property GrpHdr() As GroupHeader34
            Get
                Return Me.grpHdrField
            End Get
            Set(ByVal value As GroupHeader34)
                Me.grpHdrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("Chq", Order:=1)> _
        Public Property Chq() As List(Of ChequeType)
            Get
                Return Me.chqField
            End Get
            Set(ByVal value As List(Of ChequeType))
                Me.chqField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IsNullable:=True)> _
    Partial Public Class GroupHeader34
        Inherits EntityBase(Of GroupHeader34)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private msgIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private creDtTmField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nbOfTxsField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ttlIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmInfField As SettlementInformation14

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4

        '''<summary>
        '''GroupHeader34 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.instdAgtField = New FinancialInstitution4
            Me.instgAgtField = New FinancialInstitution4
            Me.sttlmInfField = New SettlementInformation14
            Me.ttlIntrBkSttlmAmtField = New ActiveCurrencyAndAmount
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property MsgId() As String
            Get
                Return Me.msgIdField
            End Get
            Set(ByVal value As String)
                Me.msgIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property CreDtTm() As Date
            Get
                Return Me.creDtTmField
            End Get
            Set(ByVal value As Date)
                Me.creDtTmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property NbOfTxs() As String
            Get
                Return Me.nbOfTxsField
            End Get
            Set(ByVal value As String)
                Me.nbOfTxsField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property TtlIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.ttlIntrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.ttlIntrBkSttlmAmtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=4)> _
        Public Property IntrBkSttlmDt() As Date
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(ByVal value As Date)
                Me.intrBkSttlmDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property SttlmInf() As SettlementInformation14
            Get
                Return Me.sttlmInfField
            End Get
            Set(ByVal value As SettlementInformation14)
                Me.sttlmInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property InstdAgt() As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class ActiveCurrencyAndAmount
        Inherits EntityBase(Of ActiveCurrencyAndAmount)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ccyField As ActiveCurrencyCode

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private valueField As String

        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Ccy() As ActiveCurrencyCode
            Get
                Return Me.ccyField
            End Get
            Set(ByVal value As ActiveCurrencyCode)
                Me.ccyField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set(ByVal value As String)
                Me.valueField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Public Enum ActiveCurrencyCode

        '''<remarks/>
        TZS = 0

        '''<remarks/>
        USD = 1

        '''<remarks/>
        EUR = 3

        '''<remarks/>
        GBP = 2

        '''<remarks/>
        JPY = 4

        '''<remarks/>
        KES = 5

        '''<remarks/>
        UGX = 6

        '''<remarks/>
        ETB = 7
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class CreditorReferenceType1Choice
        Inherits EntityBase(Of CreditorReferenceType1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As DocumentType3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As DocumentType3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As DocumentType3Code)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Public Enum DocumentType3Code

        '''<remarks/>
        SCOR
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class CreditorReferenceType2
        Inherits EntityBase(Of CreditorReferenceType2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdOrPrtryField As CreditorReferenceType1Choice

        '''<summary>
        '''CreditorReferenceType2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdOrPrtryField = New CreditorReferenceType1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdOrPrtry() As CreditorReferenceType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(ByVal value As CreditorReferenceType1Choice)
                Me.cdOrPrtryField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class CreditorReferenceInformation2
        Inherits EntityBase(Of CreditorReferenceInformation2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private tpField As CreditorReferenceType2

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private refField As String

        '''<summary>
        '''CreditorReferenceInformation2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.tpField = New CreditorReferenceType2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Tp() As CreditorReferenceType2
            Get
                Return Me.tpField
            End Get
            Set(ByVal value As CreditorReferenceType2)
                Me.tpField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Ref() As String
            Get
                Return Me.refField
            End Get
            Set(ByVal value As String)
                Me.refField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class StructuredRemittanceInformation7
        Inherits EntityBase(Of StructuredRemittanceInformation7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrRefInfField As CreditorReferenceInformation2

        '''<summary>
        '''StructuredRemittanceInformation7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdtrRefInfField = New CreditorReferenceInformation2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdtrRefInf() As CreditorReferenceInformation2
            Get
                Return Me.cdtrRefInfField
            End Get
            Set(ByVal value As CreditorReferenceInformation2)
                Me.cdtrRefInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class RemittanceInformation5
        Inherits EntityBase(Of RemittanceInformation5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Strd", GetType(StructuredRemittanceInformation7), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Ustrd", GetType(String), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class Purpose2Choice
        Inherits EntityBase(Of Purpose2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class PartyIdentification32
        Inherits EntityBase(Of PartyIdentification32)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification32 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class Party6Choice
        Inherits EntityBase(Of Party6Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("OrgId", GetType(OrganisationIdentification4), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("PrvtId", GetType(PersonIdentification5), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class OrganisationIdentification4
        Inherits EntityBase(Of OrganisationIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("BICOrBEI", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericOrganisationIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class GenericOrganisationIdentification1
        Inherits EntityBase(Of GenericOrganisationIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As OrganisationIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericOrganisationIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New OrganisationIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As OrganisationIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As OrganisationIdentificationSchemeName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class OrganisationIdentificationSchemeName1Choice
        Inherits EntityBase(Of OrganisationIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType1

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType1
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType1)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType1

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class PersonIdentification5
        Inherits EntityBase(Of PersonIdentification5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("DtAndPlcOfBirth", GetType(DateAndPlaceOfBirth), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericPersonIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class DateAndPlaceOfBirth
        Inherits EntityBase(Of DateAndPlaceOfBirth)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private birthDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prvcOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cityOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryOfBirthField As String

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=0)> _
        Public Property BirthDt() As Date
            Get
                Return Me.birthDtField
            End Get
            Set(ByVal value As Date)
                Me.birthDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property PrvcOfBirth() As String
            Get
                Return Me.prvcOfBirthField
            End Get
            Set(ByVal value As String)
                Me.prvcOfBirthField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property CityOfBirth() As String
            Get
                Return Me.cityOfBirthField
            End Get
            Set(ByVal value As String)
                Me.cityOfBirthField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property CtryOfBirth() As String
            Get
                Return Me.ctryOfBirthField
            End Get
            Set(ByVal value As String)
                Me.ctryOfBirthField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class GenericPersonIdentification1
        Inherits EntityBase(Of GenericPersonIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As PersonIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericPersonIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New PersonIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As PersonIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As PersonIdentificationSchemeName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class PersonIdentificationSchemeName1Choice
        Inherits EntityBase(Of PersonIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType2

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType2
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType2)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType2

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class AccountIdentification4Choice
        Inherits EntityBase(Of AccountIdentification4Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class CashAccount17
        Inherits EntityBase(Of CashAccount17)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As AccountIdentification4Choice

        '''<summary>
        '''CashAccount17 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New AccountIdentification4Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As AccountIdentification4Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentification4Choice)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class PostalAddress7
        Inherits EntityBase(Of PostalAddress7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private adrLineField As List(Of String)

        '''<summary>
        '''PostalAddress7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.adrLineField = New List(Of String)
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(ByVal value As String)
                Me.ctryField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("AdrLine", Order:=1)> _
        Public Property AdrLine() As List(Of String)
            Get
                Return Me.adrLineField
            End Get
            Set(ByVal value As List(Of String))
                Me.adrLineField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class PartyIdentification33
        Inherits EntityBase(Of PartyIdentification33)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private pstlAdrField As PostalAddress7

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification33 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
            '  Me.pstlAdrField = New PostalAddress7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        'Public Property PstlAdr() As PostalAddress7
        '    Get
        '        Return Me.pstlAdrField
        '    End Get
        '    Set(ByVal value As PostalAddress7)
        '        Me.pstlAdrField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class ChequeDetails
        Inherits EntityBase(Of ChequeDetails)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private chkNmbrField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private accNoField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private microcodeField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private bankCodeField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private branchCodeField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private endorsementField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private truncDtTmField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private truncDtTmFieldSpecified As Boolean

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property ChkNmbr() As String
            Get
                Return Me.chkNmbrField
            End Get
            Set(ByVal value As String)
                Me.chkNmbrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property AccNo() As String
            Get
                Return Me.accNoField
            End Get
            Set(ByVal value As String)
                Me.accNoField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Microcode() As String
            Get
                Return Me.microcodeField
            End Get
            Set(ByVal value As String)
                Me.microcodeField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property BankCode() As String
            Get
                Return Me.bankCodeField
            End Get
            Set(ByVal value As String)
                Me.bankCodeField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property BranchCode() As String
            Get
                Return Me.branchCodeField
            End Get
            Set(ByVal value As String)
                Me.branchCodeField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property Endorsement() As String
            Get
                Return Me.endorsementField
            End Get
            Set(ByVal value As String)
                Me.endorsementField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property TruncDtTm() As Date
            Get
                Return Me.truncDtTmField
            End Get
            Set(ByVal value As Date)
                Me.truncDtTmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property TruncDtTmSpecified() As Boolean
            Get
                Return Me.truncDtTmFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.truncDtTmFieldSpecified = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class CategoryPurpose1Choice
        Inherits EntityBase(Of CategoryPurpose1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IsNullable:=True)> _
    Partial Public Class LocalInstrument3Choice
        Inherits EntityBase(Of LocalInstrument3Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(ChequeTranCode), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Public Enum ChequeTranCode

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("1")> _
        Item1

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("2")> _
        Item2

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3")> _
        Item3

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("4")> _
        Item4
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class ServiceLevel9Choice
        Inherits EntityBase(Of ServiceLevel9Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ServiceLevel3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)>
        Public Property Item() As ServiceLevel3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ServiceLevel3Code)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Public Enum ServiceLevel3Code

        '''<remarks/>
        ACH

        '''<remarks/>
        CH

        '''<remarks/>
        SEPA

    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IsNullable:=True)> _
    Partial Public Class PaymentTypeInformation22
        Inherits EntityBase(Of PaymentTypeInformation22)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private svcLvlField As ServiceLevel9Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private lclInstrmField As LocalInstrument3Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctgyPurpField As CategoryPurpose1Choice

        '''<summary>
        '''PaymentTypeInformation22 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.ctgyPurpField = New CategoryPurpose1Choice
            ' Me.lclInstrmField = New LocalInstrument3Choice
            Me.svcLvlField = New ServiceLevel9Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SvcLvl() As ServiceLevel9Choice
            Get
                Return Me.svcLvlField
            End Get
            Set(ByVal value As ServiceLevel9Choice)
                Me.svcLvlField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property LclInstrm() As LocalInstrument3Choice
            Get
                Return Me.lclInstrmField
            End Get
            Set(ByVal value As LocalInstrument3Choice)
                Me.lclInstrmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property CtgyPurp() As CategoryPurpose1Choice
            Get
                Return Me.ctgyPurpField
            End Get
            Set(ByVal value As CategoryPurpose1Choice)
                Me.ctgyPurpField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class PaymentIdentification3
        Inherits EntityBase(Of PaymentIdentification3)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private instrIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private endToEndIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private txIdField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property InstrId() As String
            Get
                Return Me.instrIdField
            End Get
            Set(ByVal value As String)
                Me.instrIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property EndToEndId() As String
            Get
                Return Me.endToEndIdField
            End Get
            Set(ByVal value As String)
                Me.endToEndIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property TxId() As String
            Get
                Return Me.txIdField
            End Get
            Set(ByVal value As String)
                Me.txIdField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IsNullable:=True)> _
    Partial Public Class ChequeType
        Inherits EntityBase(Of ChequeType)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtIdField As PaymentIdentification3

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtTpInfField As PaymentTypeInformation22

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private chrgBrField As ChargeBearerType1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private reqdColltnDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private reqdColltnDtFieldSpecified As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private chequeTxField As ChequeDetails

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ultmtCdtrField As PartyIdentification32

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ultmtDbtrField As PartyIdentification32

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private purpField As Purpose2Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private rmtInfField As RemittanceInformation5

        '''<summary>
        '''ChequeType class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.rmtInfField = New RemittanceInformation5
            Me.purpField = New Purpose2Choice
            Me.ultmtDbtrField = New PartyIdentification32
            Me.dbtrAgtField = New FinancialInstitution4
            Me.dbtrAcctField = New CashAccount17
            Me.dbtrField = New PartyIdentification33
            Me.instdAgtField = New FinancialInstitution4
            Me.instgAgtField = New FinancialInstitution4
            Me.ultmtCdtrField = New PartyIdentification32
            Me.cdtrAgtField = New FinancialInstitution4
            Me.cdtrAcctField = New CashAccount17
            Me.cdtrField = New PartyIdentification33
            Me.chequeTxField = New ChequeDetails
            Me.intrBkSttlmAmtField = New ActiveCurrencyAndAmount
            Me.pmtTpInfField = New PaymentTypeInformation22
            Me.pmtIdField = New PaymentIdentification3
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property PmtId() As PaymentIdentification3
            Get
                Return Me.pmtIdField
            End Get
            Set(ByVal value As PaymentIdentification3)
                Me.pmtIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property PmtTpInf() As PaymentTypeInformation22
            Get
                Return Me.pmtTpInfField
            End Get
            Set(ByVal value As PaymentTypeInformation22)
                Me.pmtTpInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property IntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.intrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.intrBkSttlmAmtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property ChrgBr() As ChargeBearerType1Code
            Get
                Return Me.chrgBrField
            End Get
            Set(ByVal value As ChargeBearerType1Code)
                Me.chrgBrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=4)> _
        Public Property ReqdColltnDt() As Date
            Get
                Return Me.reqdColltnDtField
            End Get
            Set(ByVal value As Date)
                Me.reqdColltnDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ReqdColltnDtSpecified() As Boolean
            Get
                Return Me.reqdColltnDtFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.reqdColltnDtFieldSpecified = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property ChequeTx() As ChequeDetails
            Get
                Return Me.chequeTxField
            End Get
            Set(ByVal value As ChequeDetails)
                Me.chequeTxField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property Cdtr() As PartyIdentification33
            Get
                Return Me.cdtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.cdtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property CdtrAcct() As CashAccount17
            Get
                Return Me.cdtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.cdtrAcctField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)> _
        Public Property CdtrAgt() As FinancialInstitution4
            Get
                Return Me.cdtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.cdtrAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=9)> _
        Public Property UltmtCdtr() As PartyIdentification32
            Get
                Return Me.ultmtCdtrField
            End Get
            Set(ByVal value As PartyIdentification32)
                Me.ultmtCdtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=10)> _
        Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=11)> _
        Public Property InstdAgt() As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=12)> _
        Public Property Dbtr() As PartyIdentification33
            Get
                Return Me.dbtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.dbtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=13)> _
        Public Property DbtrAcct() As CashAccount17
            Get
                Return Me.dbtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.dbtrAcctField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=14)> _
        Public Property DbtrAgt() As FinancialInstitution4
            Get
                Return Me.dbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.dbtrAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=15)> _
        Public Property UltmtDbtr() As PartyIdentification32
            Get
                Return Me.ultmtDbtrField
            End Get
            Set(ByVal value As PartyIdentification32)
                Me.ultmtDbtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=16)> _
        Public Property Purp() As Purpose2Choice
            Get
                Return Me.purpField
            End Get
            Set(ByVal value As Purpose2Choice)
                Me.purpField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=17)> _
        Public Property RmtInf() As RemittanceInformation5
            Get
                Return Me.rmtInfField
            End Get
            Set(ByVal value As RemittanceInformation5)
                Me.rmtInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Public Enum ChargeBearerType1Code

        '''<remarks/>
        SLEV
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class FinancialInstitution4
        Inherits EntityBase(Of FinancialInstitution4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private finInstnIdField As FinancialInstitutionIdentification7

        '''<summary>
        '''FinancialInstitution4 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.finInstnIdField = New FinancialInstitutionIdentification7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FinInstnId() As FinancialInstitutionIdentification7
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification7)
                Me.finInstnIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class FinancialInstitutionIdentification7
        Inherits EntityBase(Of FinancialInstitutionIdentification7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private bICField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property BIC() As String
            Get
                Return Me.bICField
            End Get
            Set(ByVal value As String)
                Me.bICField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class ClearingSystemIdentification3Choice
        Inherits EntityBase(Of ClearingSystemIdentification3Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ClearingSystemIdentification

        <System.Xml.Serialization.XmlElementAttribute("Prtry", Order:=0)> _
        Public Property Item() As ClearingSystemIdentification
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ClearingSystemIdentification)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Public Enum ClearingSystemIdentification

        '''<remarks/>
        ACH
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IsNullable:=True)> _
    Partial Public Class SettlementInformation14
        Inherits EntityBase(Of SettlementInformation14)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmMtdField As SettlementMethod1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private clrSysField As ClearingSystemIdentification3Choice

        '''<summary>
        '''SettlementInformation14 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.clrSysField = New ClearingSystemIdentification3Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SttlmMtd() As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(ByVal value As SettlementMethod1Code)
                Me.sttlmMtdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property ClrSys() As ClearingSystemIdentification3Choice
            Get
                Return Me.clrSysField
            End Get
            Set(ByVal value As ClearingSystemIdentification3Choice)
                Me.clrSysField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Public Enum SettlementMethod1Code

        '''<remarks/>
        CLRG
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02", IsNullable:=True)> _
    Partial Public Class PartyIdentification36
        Inherits EntityBase(Of PartyIdentification36)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As PartyPrivate1

        '''<summary>
        '''PartyIdentification36 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New PartyPrivate1
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As PartyPrivate1
            Get
                Return Me.idField
            End Get
            Set(ByVal value As PartyPrivate1)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class PartyPrivate1
        Inherits EntityBase(Of PartyPrivate1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As PersonIdentification4

        '''<summary>
        '''PartyPrivate1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.itemField = New PersonIdentification4
        End Sub

        <System.Xml.Serialization.XmlElementAttribute("PrvtId", Order:=0)> _
        Public Property Item() As PersonIdentification4
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As PersonIdentification4)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class PersonIdentification4
        Inherits EntityBase(Of PersonIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private othrField As RestrictedIdentification2

        '''<summary>
        '''PersonIdentification4 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.othrField = New RestrictedIdentification2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Othr() As RestrictedIdentification2
            Get
                Return Me.othrField
            End Get
            Set(ByVal value As RestrictedIdentification2)
                Me.othrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class RestrictedIdentification2
        Inherits EntityBase(Of RestrictedIdentification2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As RestrictedPersonIdentificationSchemaName2Choice

        '''<summary>
        '''RestrictedIdentification2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New RestrictedPersonIdentificationSchemaName2Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As RestrictedPersonIdentificationSchemaName2Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As RestrictedPersonIdentificationSchemaName2Choice)
                Me.schmeNmField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.005.001.02")> _
    Partial Public Class RestrictedPersonIdentificationSchemaName2Choice
        Inherits EntityBase(Of RestrictedPersonIdentificationSchemaName2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prtryField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Prtry() As String
            Get
                Return Me.prtryField
            End Get
            Set(ByVal value As String)
                Me.prtryField = Value
            End Set
        End Property
    End Class
End Namespace

Namespace ISO.Responses

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

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=False)> _
    Partial Public Class Document
        Inherits EntityBase(Of Document)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private fIToFIPmtStsRptField As FIToFIPaymentStatusReportV03

        '''<summary>
        '''Document class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.fIToFIPmtStsRptField = New FIToFIPaymentStatusReportV03
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FIToFIPmtStsRpt() As FIToFIPaymentStatusReportV03
            Get
                Return Me.fIToFIPmtStsRptField
            End Get
            Set(ByVal value As FIToFIPaymentStatusReportV03)
                Me.fIToFIPmtStsRptField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class FIToFIPaymentStatusReportV03
        Inherits EntityBase(Of FIToFIPaymentStatusReportV03)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private grpHdrField As GroupHeader37

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlGrpInfAndStsField As OriginalGroupInformation20

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private txInfAndStsField As List(Of PaymentTransactionInformation26)

        '''<summary>
        '''FIToFIPaymentStatusReportV03 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.txInfAndStsField = New List(Of PaymentTransactionInformation26)
            Me.orgnlGrpInfAndStsField = New OriginalGroupInformation20
            Me.grpHdrField = New GroupHeader37
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property GrpHdr() As GroupHeader37
            Get
                Return Me.grpHdrField
            End Get
            Set(ByVal value As GroupHeader37)
                Me.grpHdrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property OrgnlGrpInfAndSts() As OriginalGroupInformation20
            Get
                Return Me.orgnlGrpInfAndStsField
            End Get
            Set(ByVal value As OriginalGroupInformation20)
                Me.orgnlGrpInfAndStsField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("TxInfAndSts", Order:=2)> _
        Public Property TxInfAndSts() As List(Of PaymentTransactionInformation26)
            Get
                Return Me.txInfAndStsField
            End Get
            Set(ByVal value As List(Of PaymentTransactionInformation26))
                Me.txInfAndStsField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class GroupHeader37
        Inherits EntityBase(Of GroupHeader37)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private msgIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private creDtTmField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4

        '''<summary>
        '''GroupHeader37 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.instdAgtField = New FinancialInstitution4
            Me.instgAgtField = New FinancialInstitution4
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property MsgId() As String
            Get
                Return Me.msgIdField
            End Get
            Set(ByVal value As String)
                Me.msgIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property CreDtTm() As Date
            Get
                Return Me.creDtTmField
            End Get
            Set(ByVal value As Date)
                Me.creDtTmField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        'Public Property InstgAgt() As FinancialInstitution4
        '    Get
        '        Return Me.instgAgtField
        '    End Get
        '    Set(ByVal value As FinancialInstitution4)
        '        Me.instgAgtField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property InstdAgt() As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = Value
            End Set
        End Property
        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
     Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = Value
            End Set
        End Property

    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class FinancialInstitution4
        Inherits EntityBase(Of FinancialInstitution4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private finInstnIdField As FinancialInstitutionIdentification7

        '''<summary>
        '''FinancialInstitution4 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.finInstnIdField = New FinancialInstitutionIdentification7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FinInstnId() As FinancialInstitutionIdentification7
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification7)
                Me.finInstnIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class FinancialInstitutionIdentification7
        Inherits EntityBase(Of FinancialInstitutionIdentification7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private bICField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property BIC() As String
            Get
                Return Me.bICField
            End Get
            Set(ByVal value As String)
                Me.bICField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class AccountIdentification4Choice
        Inherits EntityBase(Of AccountIdentification4Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class CashAccount17
        Inherits EntityBase(Of CashAccount17)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As AccountIdentification4Choice

        '''<summary>
        '''CashAccount17 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New AccountIdentification4Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As AccountIdentification4Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentification4Choice)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PostalAddress7
        Inherits EntityBase(Of PostalAddress7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private adrLineField As List(Of String)

        '''<summary>
        '''PostalAddress7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.adrLineField = New List(Of String)
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(ByVal value As String)
                Me.ctryField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("AdrLine", Order:=1)> _
        Public Property AdrLine() As List(Of String)
            Get
                Return Me.adrLineField
            End Get
            Set(ByVal value As List(Of String))
                Me.adrLineField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PartyIdentification33
        Inherits EntityBase(Of PartyIdentification33)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private pstlAdrField As PostalAddress7

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification33 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
            ' Me.pstlAdrField = New PostalAddress7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        'Public Property PstlAdr() As PostalAddress7
        '    Get
        '        Return Me.pstlAdrField
        '    End Get
        '    Set(ByVal value As PostalAddress7)
        '        Me.pstlAdrField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class Party6Choice
        Inherits EntityBase(Of Party6Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("OrgId", GetType(OrganisationIdentification4), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("PrvtId", GetType(PersonIdentification5), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class OrganisationIdentification4
        Inherits EntityBase(Of OrganisationIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("BICOrBEI", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericOrganisationIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class GenericOrganisationIdentification1
        Inherits EntityBase(Of GenericOrganisationIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As OrganisationIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericOrganisationIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New OrganisationIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As OrganisationIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As OrganisationIdentificationSchemeName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class OrganisationIdentificationSchemeName1Choice
        Inherits EntityBase(Of OrganisationIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PersonIdentification5
        Inherits EntityBase(Of PersonIdentification5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("DtAndPlcOfBirth", GetType(DateAndPlaceOfBirth), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericPersonIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class DateAndPlaceOfBirth
        Inherits EntityBase(Of DateAndPlaceOfBirth)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private birthDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prvcOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cityOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryOfBirthField As String

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=0)> _
        Public Property BirthDt() As Date
            Get
                Return Me.birthDtField
            End Get
            Set(ByVal value As Date)
                Me.birthDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property PrvcOfBirth() As String
            Get
                Return Me.prvcOfBirthField
            End Get
            Set(ByVal value As String)
                Me.prvcOfBirthField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property CityOfBirth() As String
            Get
                Return Me.cityOfBirthField
            End Get
            Set(ByVal value As String)
                Me.cityOfBirthField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property CtryOfBirth() As String
            Get
                Return Me.ctryOfBirthField
            End Get
            Set(ByVal value As String)
                Me.ctryOfBirthField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class GenericPersonIdentification1
        Inherits EntityBase(Of GenericPersonIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As PersonIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericPersonIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New PersonIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As PersonIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As PersonIdentificationSchemeName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PersonIdentificationSchemeName1Choice
        Inherits EntityBase(Of PersonIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType1

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType1
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType1)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType1

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PartyIdentification32
        Inherits EntityBase(Of PartyIdentification32)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification32 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class CreditorReferenceType1Choice
        Inherits EntityBase(Of CreditorReferenceType1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As DocumentType3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As DocumentType3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As DocumentType3Code)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Public Enum DocumentType3Code

        '''<remarks/>
        SCOR
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class CreditorReferenceType2
        Inherits EntityBase(Of CreditorReferenceType2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdOrPrtryField As CreditorReferenceType1Choice

        '''<summary>
        '''CreditorReferenceType2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdOrPrtryField = New CreditorReferenceType1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdOrPrtry() As CreditorReferenceType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(ByVal value As CreditorReferenceType1Choice)
                Me.cdOrPrtryField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class CreditorReferenceInformation2
        Inherits EntityBase(Of CreditorReferenceInformation2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private tpField As CreditorReferenceType2

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private refField As String

        '''<summary>
        '''CreditorReferenceInformation2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.tpField = New CreditorReferenceType2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Tp() As CreditorReferenceType2
            Get
                Return Me.tpField
            End Get
            Set(ByVal value As CreditorReferenceType2)
                Me.tpField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Ref() As String
            Get
                Return Me.refField
            End Get
            Set(ByVal value As String)
                Me.refField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class StructuredRemittanceInformation7
        Inherits EntityBase(Of StructuredRemittanceInformation7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrRefInfField As CreditorReferenceInformation2

        '''<summary>
        '''StructuredRemittanceInformation7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdtrRefInfField = New CreditorReferenceInformation2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdtrRefInf() As CreditorReferenceInformation2
            Get
                Return Me.cdtrRefInfField
            End Get
            Set(ByVal value As CreditorReferenceInformation2)
                Me.cdtrRefInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class RemittanceInformation5
        Inherits EntityBase(Of RemittanceInformation5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Strd", GetType(StructuredRemittanceInformation7), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Ustrd", GetType(String), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class ChequeDetails
        Inherits EntityBase(Of ChequeDetails)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private chkNmbrField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private accNoField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private microcodeField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private bankCodeField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private branchCodeField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private endorsementField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private truncDtTmField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private truncDtTmFieldSpecified As Boolean

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property ChkNmbr() As String
            Get
                Return Me.chkNmbrField
            End Get
            Set(ByVal value As String)
                Me.chkNmbrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property AccNo() As String
            Get
                Return Me.accNoField
            End Get
            Set(ByVal value As String)
                Me.accNoField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Microcode() As String
            Get
                Return Me.microcodeField
            End Get
            Set(ByVal value As String)
                Me.microcodeField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property BankCode() As String
            Get
                Return Me.bankCodeField
            End Get
            Set(ByVal value As String)
                Me.bankCodeField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property BranchCode() As String
            Get
                Return Me.branchCodeField
            End Get
            Set(ByVal value As String)
                Me.branchCodeField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property Endorsement() As String
            Get
                Return Me.endorsementField
            End Get
            Set(ByVal value As String)
                Me.endorsementField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property TruncDtTm() As Date
            Get
                Return Me.truncDtTmField
            End Get
            Set(ByVal value As Date)
                Me.truncDtTmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property TruncDtTmSpecified() As Boolean
            Get
                Return Me.truncDtTmFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.truncDtTmFieldSpecified = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class RestrictedInstitutionSchemaName1Choice
        Inherits EntityBase(Of RestrictedInstitutionSchemaName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prtryField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Prtry() As String
            Get
                Return Me.prtryField
            End Get
            Set(ByVal value As String)
                Me.prtryField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class RestrictedIdentification1
        Inherits EntityBase(Of RestrictedIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As RestrictedInstitutionSchemaName1Choice

        '''<summary>
        '''RestrictedIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New RestrictedInstitutionSchemaName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As RestrictedInstitutionSchemaName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As RestrictedInstitutionSchemaName1Choice)
                Me.schmeNmField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class FinancialInstitutionIdentification8
        Inherits EntityBase(Of FinancialInstitutionIdentification8)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private othrField As RestrictedIdentification1

        '''<summary>
        '''FinancialInstitutionIdentification8 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.othrField = New RestrictedIdentification1
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Othr() As RestrictedIdentification1
            Get
                Return Me.othrField
            End Get
            Set(ByVal value As RestrictedIdentification1)
                Me.othrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class FinancialInstitution5
        Inherits EntityBase(Of FinancialInstitution5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private finInstnIdField As FinancialInstitutionIdentification8

        '''<summary>
        '''FinancialInstitution5 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.finInstnIdField = New FinancialInstitutionIdentification8
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FinInstnId() As FinancialInstitutionIdentification8
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification8)
                Me.finInstnIdField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class AccountIdentificationIBAN
        Inherits EntityBase(Of AccountIdentificationIBAN)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class CashAccount16
        Inherits EntityBase(Of CashAccount16)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As AccountIdentificationIBAN

        '''<summary>
        '''CashAccount16 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New AccountIdentificationIBAN
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As AccountIdentificationIBAN
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentificationIBAN)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class RestrictedPersonIdentificationSchemaName2Choice
        Inherits EntityBase(Of RestrictedPersonIdentificationSchemaName2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prtryField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Prtry() As String
            Get
                Return Me.prtryField
            End Get
            Set(ByVal value As String)
                Me.prtryField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class RestrictedIdentification2
        Inherits EntityBase(Of RestrictedIdentification2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As RestrictedPersonIdentificationSchemaName2Choice

        '''<summary>
        '''RestrictedIdentification2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New RestrictedPersonIdentificationSchemaName2Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As RestrictedPersonIdentificationSchemaName2Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As RestrictedPersonIdentificationSchemaName2Choice)
                Me.schmeNmField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PersonIdentification4
        Inherits EntityBase(Of PersonIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private othrField As RestrictedIdentification2

        '''<summary>
        '''PersonIdentification4 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.othrField = New RestrictedIdentification2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Othr() As RestrictedIdentification2
            Get
                Return Me.othrField
            End Get
            Set(ByVal value As RestrictedIdentification2)
                Me.othrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PartyPrivate1
        Inherits EntityBase(Of PartyPrivate1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As PersonIdentification4

        '''<summary>
        '''PartyPrivate1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.itemField = New PersonIdentification4
        End Sub

        <System.Xml.Serialization.XmlElementAttribute("PrvtId", Order:=0)> _
        Public Property Item() As PersonIdentification4
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As PersonIdentification4)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PartyIdentification35
        Inherits EntityBase(Of PartyIdentification35)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As PartyPrivate1

        '''<summary>
        '''PartyIdentification35 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New PartyPrivate1
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As PartyPrivate1
            Get
                Return Me.idField
            End Get
            Set(ByVal value As PartyPrivate1)
                Me.idField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class AmendmentInformationDetails6
        Inherits EntityBase(Of AmendmentInformationDetails6)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlMndtIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlCdtrSchmeIdField As PartyIdentification35

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlDbtrAcctField As CashAccount16

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlDbtrAgtField As FinancialInstitution5

        '''<summary>
        '''AmendmentInformationDetails6 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.orgnlDbtrAgtField = New FinancialInstitution5
            Me.orgnlDbtrAcctField = New CashAccount16
            Me.orgnlCdtrSchmeIdField = New PartyIdentification35
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property OrgnlMndtId() As String
            Get
                Return Me.orgnlMndtIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMndtIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property OrgnlCdtrSchmeId() As PartyIdentification35
            Get
                Return Me.orgnlCdtrSchmeIdField
            End Get
            Set(ByVal value As PartyIdentification35)
                Me.orgnlCdtrSchmeIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property OrgnlDbtrAcct() As CashAccount16
            Get
                Return Me.orgnlDbtrAcctField
            End Get
            Set(ByVal value As CashAccount16)
                Me.orgnlDbtrAcctField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property OrgnlDbtrAgt() As FinancialInstitution5
            Get
                Return Me.orgnlDbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution5)
                Me.orgnlDbtrAgtField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class MandateRelatedInformation6
        Inherits EntityBase(Of MandateRelatedInformation6)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private mndtIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dtOfSgntrField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private amdmntIndField As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private amdmntIndFieldSpecified As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private amdmntInfDtlsField As AmendmentInformationDetails6

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private elctrncSgntrField As String

        '''<summary>
        '''MandateRelatedInformation6 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.amdmntInfDtlsField = New AmendmentInformationDetails6
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property MndtId() As String
            Get
                Return Me.mndtIdField
            End Get
            Set(ByVal value As String)
                Me.mndtIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=1)> _
        Public Property DtOfSgntr() As Date
            Get
                Return Me.dtOfSgntrField
            End Get
            Set(ByVal value As Date)
                Me.dtOfSgntrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property AmdmntInd() As Boolean
            Get
                Return Me.amdmntIndField
            End Get
            Set(ByVal value As Boolean)
                Me.amdmntIndField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property AmdmntIndSpecified() As Boolean
            Get
                Return Me.amdmntIndFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.amdmntIndFieldSpecified = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property AmdmntInfDtls() As AmendmentInformationDetails6
            Get
                Return Me.amdmntInfDtlsField
            End Get
            Set(ByVal value As AmendmentInformationDetails6)
                Me.amdmntInfDtlsField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property ElctrncSgntr() As String
            Get
                Return Me.elctrncSgntrField
            End Get
            Set(ByVal value As String)
                Me.elctrncSgntrField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class CategoryPurpose1Choice
        Inherits EntityBase(Of CategoryPurpose1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType3

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType3
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType3)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType3

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class LocalInstrument2Choice
        Inherits EntityBase(Of LocalInstrument2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType2

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType2
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType2)
                Me.itemElementNameField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType2

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class ServiceLevel9Choice
        Inherits EntityBase(Of ServiceLevel9Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ServiceLevel3Code

        <System.Xml.Serialization.XmlElementAttribute("Prtry", Order:=0)> _
        Public Property Item() As ServiceLevel3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ServiceLevel3Code)
                Me.itemField = value
            End Set
        End Property
     
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Public Enum ServiceLevel3Code

        '''<remarks/>
        ACH
        '''<remarks/>
        SEPA

    End Enum
  
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class PaymentTypeInformation22
        Inherits EntityBase(Of PaymentTypeInformation22)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private svcLvlField As ServiceLevel9Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private lclInstrmField As LocalInstrument2Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private seqTpField As SequenceType1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private seqTpFieldSpecified As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctgyPurpField As CategoryPurpose1Choice

        '''<summary>
        '''PaymentTypeInformation22 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.ctgyPurpField = New CategoryPurpose1Choice
            Me.lclInstrmField = New LocalInstrument2Choice
            Me.svcLvlField = New ServiceLevel9Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SvcLvl() As ServiceLevel9Choice
            Get
                Return Me.svcLvlField
            End Get
            Set(ByVal value As ServiceLevel9Choice)
                Me.svcLvlField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property LclInstrm() As LocalInstrument2Choice
            Get
                Return Me.lclInstrmField
            End Get
            Set(ByVal value As LocalInstrument2Choice)
                Me.lclInstrmField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property SeqTp() As SequenceType1Code
            Get
                Return Me.seqTpField
            End Get
            Set(ByVal value As SequenceType1Code)
                Me.seqTpField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property SeqTpSpecified() As Boolean
            Get
                Return Me.seqTpFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.seqTpFieldSpecified = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property CtgyPurp() As CategoryPurpose1Choice
            Get
                Return Me.ctgyPurpField
            End Get
            Set(ByVal value As CategoryPurpose1Choice)
                Me.ctgyPurpField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Public Enum SequenceType1Code

        '''<remarks/>
        FRST

        '''<remarks/>
        RCUR

        '''<remarks/>
        FNAL

        '''<remarks/>
        OOFF
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class ClearingSystemIdentification3Choice
        Inherits EntityBase(Of ClearingSystemIdentification3Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ClearingSystemIdentification

        <System.Xml.Serialization.XmlElementAttribute("Prtry", Order:=0)> _
        Public Property Item() As ClearingSystemIdentification
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ClearingSystemIdentification)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Public Enum ClearingSystemIdentification

        '''<remarks/>
        ACH
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class SettlementInformation13
        Inherits EntityBase(Of SettlementInformation13)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmMtdField As SettlementMethod1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private clrSysField As ClearingSystemIdentification3Choice

        '''<summary>
        '''SettlementInformation13 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.clrSysField = New ClearingSystemIdentification3Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SttlmMtd() As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(ByVal value As SettlementMethod1Code)
                Me.sttlmMtdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property ClrSys() As ClearingSystemIdentification3Choice
            Get
                Return Me.clrSysField
            End Get
            Set(ByVal value As ClearingSystemIdentification3Choice)
                Me.clrSysField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Public Enum SettlementMethod1Code

        '''<remarks/>
        CLRG
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class OriginalTransactionReference13
        Inherits EntityBase(Of OriginalTransactionReference13)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmDtFieldSpecified As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private reqdColltnDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private reqdColltnDtFieldSpecified As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrSchmeIdField As PartyIdentification34

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmInfField As SettlementInformation13

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtTpInfField As PaymentTypeInformation22

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private chequeTxField As ChequeDetails

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private rmtInfField As RemittanceInformation5

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ultmtDbtrField As PartyIdentification32

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ultmtCdtrField As PartyIdentification32

        '-------Added By Herbert to cater for manadate info-----------------
        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private mndtRltdInfField As MandateRelatedInformation6

        '''<summary>
        '''OriginalTransactionReference13 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.ultmtCdtrField = New PartyIdentification32
            Me.cdtrAcctField = New CashAccount17
            Me.cdtrField = New PartyIdentification33
            Me.cdtrAgtField = New FinancialInstitution4
            Me.dbtrAgtField = New FinancialInstitution4
            Me.dbtrAcctField = New CashAccount17
            Me.dbtrField = New PartyIdentification33
            Me.ultmtDbtrField = New PartyIdentification32
            Me.rmtInfField = New RemittanceInformation5
            Me.chequeTxField = New ChequeDetails
            Me.pmtTpInfField = New PaymentTypeInformation22
            Me.sttlmInfField = New SettlementInformation13
            Me.cdtrSchmeIdField = New PartyIdentification34
            Me.intrBkSttlmAmtField = New ActiveCurrencyAndAmount
            '-------Added By Herbert to cater for manadate info-----------------
            Me.mndtRltdInfField = New MandateRelatedInformation6
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property IntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.intrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.intrBkSttlmAmtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=1)> _
        Public Property IntrBkSttlmDt() As Date
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(ByVal value As Date)
                Me.intrBkSttlmDtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property IntrBkSttlmDtSpecified() As Boolean
            Get
                Return Me.intrBkSttlmDtFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.intrBkSttlmDtFieldSpecified = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=2)> _
        Public Property ReqdColltnDt() As Date
            Get
                Return Me.reqdColltnDtField
            End Get
            Set(ByVal value As Date)
                Me.reqdColltnDtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ReqdColltnDtSpecified() As Boolean
            Get
                Return Me.reqdColltnDtFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.reqdColltnDtFieldSpecified = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property CdtrSchmeId() As PartyIdentification34
            Get
                Return Me.cdtrSchmeIdField
            End Get
            Set(ByVal value As PartyIdentification34)
                Me.cdtrSchmeIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property SttlmInf() As SettlementInformation13
            Get
                Return Me.sttlmInfField
            End Get
            Set(ByVal value As SettlementInformation13)
                Me.sttlmInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property PmtTpInf() As PaymentTypeInformation22
            Get
                Return Me.pmtTpInfField
            End Get
            Set(ByVal value As PaymentTypeInformation22)
                Me.pmtTpInfField = Value
            End Set
        End Property

        '-------Added By Herbert to cater for manadate info-----------------
        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property MndtRltdInf() As MandateRelatedInformation6
            Get
                Return Me.mndtRltdInfField
            End Get
            Set(ByVal value As MandateRelatedInformation6)
                Me.mndtRltdInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property ChequeTx() As ChequeDetails
            Get
                Return Me.chequeTxField
            End Get
            Set(ByVal value As ChequeDetails)
                Me.chequeTxField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=9)> _
        Public Property RmtInf() As RemittanceInformation5
            Get
                Return Me.rmtInfField
            End Get
            Set(ByVal value As RemittanceInformation5)
                Me.rmtInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=10)> _
        Public Property UltmtDbtr() As PartyIdentification32
            Get
                Return Me.ultmtDbtrField
            End Get
            Set(ByVal value As PartyIdentification32)
                Me.ultmtDbtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=11)> _
        Public Property Dbtr() As PartyIdentification33
            Get
                Return Me.dbtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.dbtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=12)> _
        Public Property DbtrAcct() As CashAccount17
            Get
                Return Me.dbtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.dbtrAcctField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=13)> _
        Public Property DbtrAgt() As FinancialInstitution4
            Get
                Return Me.dbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.dbtrAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=14)> _
        Public Property CdtrAgt() As FinancialInstitution4
            Get
                Return Me.cdtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.cdtrAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=15)> _
        Public Property Cdtr() As PartyIdentification33
            Get
                Return Me.cdtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.cdtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=16)> _
        Public Property CdtrAcct() As CashAccount17
            Get
                Return Me.cdtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.cdtrAcctField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=17)> _
        Public Property UltmtCdtr() As PartyIdentification32
            Get
                Return Me.ultmtCdtrField
            End Get
            Set(ByVal value As PartyIdentification32)
                Me.ultmtCdtrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class ActiveCurrencyAndAmount
        Inherits EntityBase(Of ActiveCurrencyAndAmount)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ccyField As ActiveCurrencyCode

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private valueField As String

        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Ccy() As ActiveCurrencyCode
            Get
                Return Me.ccyField
            End Get
            Set(ByVal value As ActiveCurrencyCode)
                Me.ccyField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set(ByVal value As String)
                Me.valueField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Public Enum ActiveCurrencyCode

        '''<remarks/>
        TZS = 0

        '''<remarks/>
        USD = 1

        '''<remarks/>
        EUR = 3

        '''<remarks/>
        GBP = 2

        '''<remarks/>
        JPY = 4
        '''<remarks/>
        KES = 5
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class PartyIdentification34
        Inherits EntityBase(Of PartyIdentification34)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Id", GetType(Party6Choice), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Nm", GetType(String), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Partial Public Class ChargesInformation5
        Inherits EntityBase(Of ChargesInformation5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private amtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ptyField As FinancialInstitution4

        '''<summary>
        '''ChargesInformation5 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.ptyField = New FinancialInstitution4
            Me.amtField = New ActiveCurrencyAndAmount
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Amt() As ActiveCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.amtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Pty() As FinancialInstitution4
            Get
                Return Me.ptyField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.ptyField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class PaymentTransactionInformation26
        Inherits EntityBase(Of PaymentTransactionInformation26)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private stsIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlInstrIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlEndToEndIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlTxIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private txStsField As TransactionIndividualStatus3Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private txStsFieldSpecified As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private stsRsnInfField As StatusReasonInformation8
        ''test
        <EditorBrowsable(EditorBrowsableState.Never)> _
     Private addInfField As StatusReasonInformation8

        <EditorBrowsable(EditorBrowsableState.Never)> _
       Private chrgsInfField As ChargesInformation5

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private AddtlInf As ChargesInformation5

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlTxRefField As OriginalTransactionReference13

        '''<summary>
        '''PaymentTransactionInformation26 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.orgnlTxRefField = New OriginalTransactionReference13
            Me.instdAgtField = New FinancialInstitution4
            Me.instgAgtField = New FinancialInstitution4
            Me.chrgsInfField = New ChargesInformation5
            Me.stsRsnInfField = New StatusReasonInformation8

        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property StsId() As String
            Get
                Return Me.stsIdField
            End Get
            Set(ByVal value As String)
                Me.stsIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property OrgnlInstrId() As String
            Get
                Return Me.orgnlInstrIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlInstrIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property OrgnlEndToEndId() As String
            Get
                Return Me.orgnlEndToEndIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlEndToEndIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property OrgnlTxId() As String
            Get
                Return Me.orgnlTxIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlTxIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property TxSts() As TransactionIndividualStatus3Code
            Get
                Return Me.txStsField
            End Get
            Set(ByVal value As TransactionIndividualStatus3Code)
                Me.txStsField = Value
            End Set
        End Property


        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property TxStsSpecified() As Boolean
            Get
                Return Me.txStsFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.txStsFieldSpecified = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property StsRsnInf() As StatusReasonInformation8
            Get
                Return Me.stsRsnInfField
            End Get
            Set(ByVal value As StatusReasonInformation8)
                Me.stsRsnInfField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property ChrgsInf() As ChargesInformation5
            Get
                Return Me.chrgsInfField
            End Get
            Set(ByVal value As ChargesInformation5)
                Me.chrgsInfField = Value
            End Set
        End Property



        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)> _
        Public Property InstdAgt() As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=9)> _
        Public Property OrgnlTxRef() As OriginalTransactionReference13
            Get
                Return Me.orgnlTxRefField
            End Get
            Set(ByVal value As OriginalTransactionReference13)
                Me.orgnlTxRefField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Public Enum TransactionIndividualStatus3Code

        '''<remarks/>
        ACCP

        '''<remarks/>
        CLRD

        '''<remarks/>
        ACSC

        '''<remarks/>
        PART

        '''<remarks/>
        RJCT


    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class StatusReasonInformation8
        Inherits EntityBase(Of StatusReasonInformation8)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgtrField As PartyIdentification34

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private rsnField As StatusReason6Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
     Private addInf As StatusReason6Choice

        '''<summary>
        '''StatusReasonInformation8 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.rsnField = New StatusReason6Choice
            Me.orgtrField = New PartyIdentification34
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Orgtr() As PartyIdentification34
            Get
                Return Me.orgtrField
            End Get
            Set(ByVal value As PartyIdentification34)
                Me.orgtrField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Rsn() As StatusReason6Choice
            Get
                Return Me.rsnField
            End Get
            Set(ByVal value As StatusReason6Choice)
                Me.rsnField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class StatusReason6Choice
        Inherits EntityBase(Of StatusReason6Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = Value
            End Set
        End Property
        ''john
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class OriginalGroupInformation20
        Inherits EntityBase(Of OriginalGroupInformation20)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlMsgIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlMsgNmIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private grpStsField As TransactionGroupStatus3Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private grpStsFieldSpecified As Boolean

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private stsRsnInfField As StatusReasonInformation8

        '''<summary>
        '''OriginalGroupInformation20 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.stsRsnInfField = New StatusReasonInformation8
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property OrgnlMsgId() As String
            Get
                Return Me.orgnlMsgIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMsgIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property OrgnlMsgNmId() As String
            Get
                Return Me.orgnlMsgNmIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMsgNmIdField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property GrpSts() As TransactionGroupStatus3Code
            Get
                Return Me.grpStsField
            End Get
            Set(ByVal value As TransactionGroupStatus3Code)
                Me.grpStsField = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property GrpStsSpecified() As Boolean
            Get
                Return Me.grpStsFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.grpStsFieldSpecified = Value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property StsRsnInf() As StatusReasonInformation8
            Get
                Return Me.stsRsnInfField
            End Get
            Set(ByVal value As StatusReasonInformation8)
                Me.stsRsnInfField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")> _
    Public Enum TransactionGroupStatus3Code

        '''<remarks/>
        ACCP

        '''<remarks/>
        CLRD

        '''<remarks/>
        ACSC

        '''<remarks/>
        PART

        '''<remarks/>
        RJCT
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class OrganisationIdentification5
        Inherits EntityBase(Of OrganisationIdentification5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private bICOrBEIField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property BICOrBEI() As String
            Get
                Return Me.bICOrBEIField
            End Get
            Set(ByVal value As String)
                Me.bICOrBEIField = Value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)> _
    Partial Public Class Party7Choice
        Inherits EntityBase(Of Party7Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As OrganisationIdentification5

        '''<summary>
        '''Party7Choice class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.itemField = New OrganisationIdentification5
        End Sub

        <System.Xml.Serialization.XmlElementAttribute("OrgId", Order:=0)> _
        Public Property Item() As OrganisationIdentification5
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As OrganisationIdentification5)
                Me.itemField = Value
            End Set
        End Property
    End Class
End Namespace

Namespace ISO.Cancellations

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
    'System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01"), _
    'System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=False)> _
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IsNullable:=False)> _
    Partial Public Class Document
        Inherits EntityBase(Of Document)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private fIToFIPmtCxlReqField As FIToFIPaymentCancellationRequestV01

        '''<summary>
        '''Document class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.fIToFIPmtCxlReqField = New FIToFIPaymentCancellationRequestV01
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property PmtRtr() As FIToFIPaymentCancellationRequestV01
            Get
                Return Me.fIToFIPmtCxlReqField
            End Get
            Set(ByVal value As FIToFIPaymentCancellationRequestV01)
                Me.fIToFIPmtCxlReqField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IsNullable:=True)> _
    Partial Public Class FIToFIPaymentCancellationRequestV01
        Inherits EntityBase(Of FIToFIPaymentCancellationRequestV01)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private assgnmtField As CaseAssignment2

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private ctrlDataField As ControlData1

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private TxInfField As List(Of PaymentTransactionInformation31)

        '''<summary>
        '''FIToFIPaymentCancellationRequestV01 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.TxInf = New List(Of PaymentTransactionInformation31)
            'Me.ctrlDataField = New ControlData1
            Me.assgnmtField = New CaseAssignment2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property GrpHdr() As CaseAssignment2
            Get
                Return Me.assgnmtField
            End Get
            Set(ByVal value As CaseAssignment2)
                Me.assgnmtField = value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        'Public Property CtrlData() As ControlData1
        '    Get
        '        Return Me.ctrlDataField
        '    End Get
        '    Set(ByVal value As ControlData1)
        '        Me.ctrlDataField = Value
        '    End Set
        'End Property
        '***
        ' <System.Xml.Serialization.XmlElementAttribute("CdtTrfTxInf", Order:=1)> _
        'Public Property CdtTrfTxInf() As List(Of CreditTransferTransactionInformation11)
        '     Get
        '         Return Me.cdtTrfTxInfField
        '     End Get
        '     Set(ByVal value As List(Of CreditTransferTransactionInformation11))
        '         Me.cdtTrfTxInfField = Value
        '     End Set
        ' End Property

        ' System.Xml.Serialization.XmlArrayItemAttribute("TxInf", GetType(PaymentTransactionInformation31), IsNullable:=False)> _
        '***
        '<System.Xml.Serialization.XmlArrayAttribute(Order:=1), _
        <System.Xml.Serialization.XmlElementAttribute("TxInf", Order:=1)> _
        Public Property TxInf() As List(Of PaymentTransactionInformation31)
            Get
                Return Me.TxInfField
            End Get
            Set(ByVal value As List(Of PaymentTransactionInformation31))
                Me.TxInfField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class CaseAssignment2
        Inherits EntityBase(Of CaseAssignment2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private assgnrField As Party7Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private assgneField As Party7Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private creDtTmField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nbOfTxsField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ttlIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private intrBkSttlmDtField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmInfField As SettlementInformation13


        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instgAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private instdAgtField As FinancialInstitution4
        '''<summary>
        '''CaseAssignment2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.assgneField = New Party7Choice
            ' Me.assgnrField = New Party7Choice
            ' Me.TtlIntrBkSttlmAmt = New FinancialInstitution4
            'Me.instgAgtField = New FinancialInstitution4
            'Me.sttlmInfField = New SettlementInformation13
            'Me.ttlIntrBkSttlmAmtField = New ActiveCurrencyAndAmount
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property MsgId() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property CreDtTm() As Date
            Get
                Return Me.creDtTmField
            End Get
            Set(ByVal value As Date)
                Me.creDtTmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property NbOfTxs() As String
            Get
                Return Me.nbOfTxsField 'Me.assgneField
            End Get
            Set(ByVal value As String)
                Me.nbOfTxsField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
     Public Property TtlRtrdIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.ttlIntrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.ttlIntrBkSttlmAmtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property IntrBkSttlmDt() As String
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(ByVal value As String)
                Me.intrBkSttlmDtField = value
            End Set
        End Property
        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property SttlmInf() As SettlementInformation13
            Get
                Return Me.sttlmInfField
            End Get
            Set(ByVal value As SettlementInformation13)
                Me.sttlmInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property InstgAgt() As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = value
            End Set
        End Property

    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class Party7Choice
        Inherits EntityBase(Of Party7Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As FinancialInstitution4

        '''<summary>
        '''Party7Choice class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.itemField = New FinancialInstitution4
        End Sub

        <System.Xml.Serialization.XmlElementAttribute("Agt", Order:=0)> _
        Public Property Item() As FinancialInstitution4
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class FinancialInstitution4
        Inherits EntityBase(Of FinancialInstitution4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private finInstnIdField As FinancialInstitutionIdentification7

        '''<summary>
        '''FinancialInstitution4 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.finInstnIdField = New FinancialInstitutionIdentification7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property FinInstnId() As FinancialInstitutionIdentification7
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification7)
                Me.finInstnIdField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class FinancialInstitutionIdentification7
        Inherits EntityBase(Of FinancialInstitutionIdentification7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private bICField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property BIC() As String
            Get
                Return Me.bICField
            End Get
            Set(ByVal value As String)
                Me.bICField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class AccountIdentification4Choice
        Inherits EntityBase(Of AccountIdentification4Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class CashAccount17
        Inherits EntityBase(Of CashAccount17)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As AccountIdentification4Choice

        '''<summary>
        '''CashAccount17 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New AccountIdentification4Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As AccountIdentification4Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentification4Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class PostalAddress7
        Inherits EntityBase(Of PostalAddress7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private adrLineField As List(Of String)

        '''<summary>
        '''PostalAddress7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.adrLineField = New List(Of String)
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Ctry() As String
            Get
                Return Me.ctryField
            End Get
            Set(ByVal value As String)
                Me.ctryField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("AdrLine", Order:=1)> _
        Public Property AdrLine() As List(Of String)
            Get
                Return Me.adrLineField
            End Get
            Set(ByVal value As List(Of String))
                Me.adrLineField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class PartyIdentification33
        Inherits EntityBase(Of PartyIdentification33)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        '<EditorBrowsable(EditorBrowsableState.Never)> _
        'Private pstlAdrField As PostalAddress7

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification33 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
            'Me.pstlAdrField = New PostalAddress7
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        'Public Property PstlAdr() As PostalAddress7
        '    Get
        '        Return Me.pstlAdrField
        '    End Get
        '    Set(ByVal value As PostalAddress7)
        '        Me.pstlAdrField = Value
        '    End Set
        'End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class Party6Choice
        Inherits EntityBase(Of Party6Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("OrgId", GetType(OrganisationIdentification4), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("PrvtId", GetType(PersonIdentification5), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class OrganisationIdentification4
        Inherits EntityBase(Of OrganisationIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("BICOrBEI", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericOrganisationIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class GenericOrganisationIdentification1
        Inherits EntityBase(Of GenericOrganisationIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As OrganisationIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericOrganisationIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New OrganisationIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As OrganisationIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As OrganisationIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class OrganisationIdentificationSchemeName1Choice
        Inherits EntityBase(Of OrganisationIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class PersonIdentification5
        Inherits EntityBase(Of PersonIdentification5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("DtAndPlcOfBirth", GetType(DateAndPlaceOfBirth), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericPersonIdentification1), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class DateAndPlaceOfBirth
        Inherits EntityBase(Of DateAndPlaceOfBirth)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private birthDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private prvcOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cityOfBirthField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ctryOfBirthField As String

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=0)> _
        Public Property BirthDt() As Date
            Get
                Return Me.birthDtField
            End Get
            Set(ByVal value As Date)
                Me.birthDtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property PrvcOfBirth() As String
            Get
                Return Me.prvcOfBirthField
            End Get
            Set(ByVal value As String)
                Me.prvcOfBirthField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property CityOfBirth() As String
            Get
                Return Me.cityOfBirthField
            End Get
            Set(ByVal value As String)
                Me.cityOfBirthField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property CtryOfBirth() As String
            Get
                Return Me.ctryOfBirthField
            End Get
            Set(ByVal value As String)
                Me.ctryOfBirthField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.0004.001.02")> _
    Partial Public Class GenericPersonIdentification1
        Inherits EntityBase(Of GenericPersonIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private schmeNmField As PersonIdentificationSchemeName1Choice

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private issrField As String

        '''<summary>
        '''GenericPersonIdentification1 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.schmeNmField = New PersonIdentificationSchemeName1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Id() As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SchmeNm() As PersonIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As PersonIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property Issr() As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class PersonIdentificationSchemeName1Choice
        Inherits EntityBase(Of PersonIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType1

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType1
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType1)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType1

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class PartyIdentification32
        Inherits EntityBase(Of PartyIdentification32)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private nmField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private idField As Party6Choice

        '''<summary>
        '''PartyIdentification32 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.idField = New Party6Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Nm() As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Id() As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class CreditorReferenceType1Choice
        Inherits EntityBase(Of CreditorReferenceType1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As DocumentType3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As DocumentType3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As DocumentType3Code)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Public Enum DocumentType3Code

        '''<remarks/>
        SCOR
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class CreditorReferenceType2
        Inherits EntityBase(Of CreditorReferenceType2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdOrPrtryField As CreditorReferenceType1Choice

        '''<summary>
        '''CreditorReferenceType2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdOrPrtryField = New CreditorReferenceType1Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdOrPrtry() As CreditorReferenceType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(ByVal value As CreditorReferenceType1Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class CreditorReferenceInformation2
        Inherits EntityBase(Of CreditorReferenceInformation2)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private tpField As CreditorReferenceType2

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private refField As String

        '''<summary>
        '''CreditorReferenceInformation2 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.tpField = New CreditorReferenceType2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Tp() As CreditorReferenceType2
            Get
                Return Me.tpField
            End Get
            Set(ByVal value As CreditorReferenceType2)
                Me.tpField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Ref() As String
            Get
                Return Me.refField
            End Get
            Set(ByVal value As String)
                Me.refField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class StructuredRemittanceInformation7
        Inherits EntityBase(Of StructuredRemittanceInformation7)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrRefInfField As CreditorReferenceInformation2

        '''<summary>
        '''StructuredRemittanceInformation7 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.cdtrRefInfField = New CreditorReferenceInformation2
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property CdtrRefInf() As CreditorReferenceInformation2
            Get
                Return Me.cdtrRefInfField
            End Get
            Set(ByVal value As CreditorReferenceInformation2)
                Me.cdtrRefInfField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class RemittanceInformation5
        Inherits EntityBase(Of RemittanceInformation5)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Strd", GetType(StructuredRemittanceInformation7), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Ustrd", GetType(String), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class CategoryPurpose1Choice
        Inherits EntityBase(Of CategoryPurpose1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType3

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType3
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType3)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType3

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class LocalInstrument2Choice
        Inherits EntityBase(Of LocalInstrument2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemElementNameField As ItemChoiceType2

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1), _
         System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType2
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType2)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType2

        '''<remarks/>
        Cd

        '''<remarks/>
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class ServiceLevel9Choice
        Inherits EntityBase(Of ServiceLevel9Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ServiceLevel3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)> _
        Public Property Item() As ServiceLevel3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ServiceLevel3Code)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Public Enum ServiceLevel3Code

        '''<remarks/>
        SEPA
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class PaymentTypeInformation22
        Inherits EntityBase(Of PaymentTypeInformation22)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private svcLvlField As ServiceLevel9Choice

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private lclInstrmField As LocalInstrument2Choice

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ctgyPurpField As CategoryPurpose1Choice

        '''<summary>
        '''PaymentTypeInformation22 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.ctgyPurpField = New CategoryPurpose1Choice
            Me.lclInstrmField = New LocalInstrument2Choice
            Me.svcLvlField = New ServiceLevel9Choice
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SvcLvl() As ServiceLevel9Choice
            Get
                Return Me.svcLvlField
            End Get
            Set(ByVal value As ServiceLevel9Choice)
                Me.svcLvlField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property LclInstrm() As LocalInstrument2Choice
            Get
                Return Me.lclInstrmField
            End Get
            Set(ByVal value As LocalInstrument2Choice)
                Me.lclInstrmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property CtgyPurp() As CategoryPurpose1Choice
            Get
                Return Me.ctgyPurpField
            End Get
            Set(ByVal value As CategoryPurpose1Choice)
                Me.ctgyPurpField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class ClearingSystemIdentification3Choice
        Inherits EntityBase(Of ClearingSystemIdentification3Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As ClearingSystemIdentification

        <System.Xml.Serialization.XmlElementAttribute("Prtry", Order:=0)> _
        Public Property Item() As ClearingSystemIdentification
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ClearingSystemIdentification)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Public Enum ClearingSystemIdentification

        '''<remarks/>
        ACH
    End Enum
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class SettlementInformation13
        Inherits EntityBase(Of SettlementInformation13)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmMtdField As SettlementMethod1Code

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private clrSysField As ClearingSystemIdentification3Choice

        '''<summary>
        '''SettlementInformation13 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.clrSysField = New ClearingSystemIdentification3Choice  'jj
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SttlmMtd() As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(ByVal value As SettlementMethod1Code)
                Me.sttlmMtdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property ClrSys() As ClearingSystemIdentification3Choice
            Get
                Return Me.clrSysField
            End Get
            Set(ByVal value As ClearingSystemIdentification3Choice)
                Me.clrSysField = value
            End Set
        End Property
    End Class
    'johnson
    Partial Public Class SettlementInformation131
        Inherits EntityBase(Of SettlementInformation131)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmMtdField As SettlementMethod1Code1

        '''<summary>
        '''SettlementInformation13 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            'Me.clrSysField = New ClearingSystemIdentification3Choice  'jj
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property SttlmMtd() As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(ByVal value As SettlementMethod1Code)
                Me.sttlmMtdField = value
            End Set
        End Property
    End Class
    '****************
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Public Enum SettlementMethod1Code

        '''<remarks/>
        CLRG
    End Enum
    Public Enum SettlementMethod1Code1

        '''<remarks/>
        CLRG
    End Enum
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class OriginalTransactionReference13
        Inherits EntityBase(Of OriginalTransactionReference13)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private sttlmInfField As SettlementInformation131

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private pmtTpInfField As PaymentTypeInformation22

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private rmtInfField As RemittanceInformation5

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private IntrBkSttlmDtField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private dbtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAgtField As FinancialInstitution4

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrField As PartyIdentification33

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cdtrAcctField As CashAccount17

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ultmtCdtrField As PartyIdentification32

        '''<summary>
        '''OriginalTransactionReference13 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            ' Me.ultmtCdtrField = New PartyIdentification32
            Me.cdtrAcctField = New CashAccount17
            Me.cdtrField = New PartyIdentification33
            Me.cdtrAgtField = New FinancialInstitution4
            Me.dbtrAgtField = New FinancialInstitution4
            Me.dbtrAcctField = New CashAccount17
            Me.dbtrField = New PartyIdentification33
            'Me.IntrBkSttlmDtField = New PartyIdentification32
            Me.rmtInfField = New RemittanceInformation5
            Me.pmtTpInfField = New PaymentTypeInformation22
            Me.sttlmInfField = New SettlementInformation131
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
      Public Property IntrBkSttlmDt() As String
            Get
                Return Me.IntrBkSttlmDtField
            End Get
            Set(ByVal value As String)
                Me.IntrBkSttlmDtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property SttlmInf() As SettlementInformation131
            Get
                Return Me.sttlmInfField
            End Get
            Set(ByVal value As SettlementInformation131)
                Me.sttlmInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property PmtTpInf() As PaymentTypeInformation22
            Get
                Return Me.pmtTpInfField
            End Get
            Set(ByVal value As PaymentTypeInformation22)
                Me.pmtTpInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property RmtInf() As RemittanceInformation5
            Get
                Return Me.rmtInfField
            End Get
            Set(ByVal value As RemittanceInformation5)
                Me.rmtInfField = value
            End Set
        End Property



        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property Dbtr() As PartyIdentification33
            Get
                Return Me.dbtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.dbtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property DbtrAcct() As CashAccount17
            Get
                Return Me.dbtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.dbtrAcctField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property DbtrAgt() As FinancialInstitution4
            Get
                Return Me.dbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.dbtrAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property CdtrAgt() As FinancialInstitution4
            Get
                Return Me.cdtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.cdtrAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)> _
        Public Property Cdtr() As PartyIdentification33
            Get
                Return Me.cdtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.cdtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=9)> _
        Public Property CdtrAcct() As CashAccount17
            Get
                Return Me.cdtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.cdtrAcctField = value
            End Set
        End Property

        '<System.Xml.Serialization.XmlElementAttribute(Order:=10)> _
        'Public Property UltmtCdtr() As PartyIdentification32
        '    Get
        '        Return Me.ultmtCdtrField
        '    End Get
        '    Set(ByVal value As PartyIdentification32)
        '        Me.ultmtCdtrField = Value
        '    End Set
        'End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IsNullable:=True)> _
    Partial Public Class CancellationReason2Choice
        Inherits EntityBase(Of CancellationReason2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        '<System.Xml.Serialization.XmlElementAttribute("Cd", GetType(CancellationReason4Code), Order:=0), _
        ' System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0)> _

        '<System.Xml.Serialization.XmlElementAttribute("Cd", GetType(CancellationReason4Code), Order:=0), _
        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0)> _
       Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Public Enum CancellationReason4Code

        '''<remarks/>
        DUPL

        '''<remarks/>
        CUST

        '''<remarks/>
        AGNT

        '''<remarks/>
        CURR

        '''<remarks/>
        UPAY

        '''<remarks/>
        CUTA
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class PartyIdentification34
        Inherits EntityBase(Of PartyIdentification34)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Id", GetType(Party6Choice), Order:=0), _
         System.Xml.Serialization.XmlElementAttribute("Nm", GetType(String), Order:=0)> _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IsNullable:=True)> _
    Partial Public Class CancellationReasonInformation3
        Inherits EntityBase(Of CancellationReasonInformation3)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgtrField As PartyIdentification34

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private rsnField As CancellationReason2Choice

        '''<summary>
        '''CancellationReasonInformation3 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.rsnField = New CancellationReason2Choice
            Me.orgtrField = New PartyIdentification34
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property Orgtr() As PartyIdentification34
            Get
                Return Me.orgtrField
            End Get
            Set(ByVal value As PartyIdentification34)
                Me.orgtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property Rsn() As CancellationReason2Choice
            Get
                Return Me.rsnField
            End Get
            Set(ByVal value As CancellationReason2Choice)
                Me.rsnField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class ActiveCurrencyAndAmount
        Inherits EntityBase(Of ActiveCurrencyAndAmount)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private ccyField As ActiveCurrencyCode

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private valueField As String

        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Ccy() As ActiveCurrencyCode
            Get
                Return Me.ccyField
            End Get
            Set(ByVal value As ActiveCurrencyCode)
                Me.ccyField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set(ByVal value As String)
                Me.valueField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Public Enum ActiveCurrencyCode

        '''<remarks/>
        TZS = 0

        '''<remarks/>
        USD = 1

        '''<remarks/>
        EUR = 3

        '''<remarks/>
        GBP = 2

        '''<remarks/>
        JPY = 4
        '''<remarks/>
        KES = 5
        '''<remarks/>
        ETB = 6
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02")> _
    Partial Public Class OriginalGroupInformation3
        Inherits EntityBase(Of OriginalGroupInformation3)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlMsgIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlMsgNmIdField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property OrgnlMsgId() As String
            Get
                Return Me.orgnlMsgIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMsgIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property OrgnlMsgNmId() As String
            Get
                Return Me.orgnlMsgNmIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMsgNmIdField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02"), _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.004.001.02", IsNullable:=True)> _
    Partial Public Class PaymentTransactionInformation31
        Inherits EntityBase(Of PaymentTransactionInformation31)

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cxlIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlGrpInfField As OriginalGroupInformation3

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlInstrIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlEndToEndIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlTxIdField As String

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
       Private RtrdIntrBkSttlmAmtField As ActiveCurrencyAndAmount

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlIntrBkSttlmDtField As Date

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private cxlRsnInfField As CancellationReasonInformation3

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private orgnlTxRefField As OriginalTransactionReference13

        '''<summary>
        '''PaymentTransactionInformation31 class constructor
        '''</summary>
        Public Sub New()
            MyBase.New()
            Me.orgnlTxRefField = New OriginalTransactionReference13
            Me.cxlRsnInfField = New CancellationReasonInformation3
            Me.orgnlIntrBkSttlmAmtField = New ActiveCurrencyAndAmount
            Me.RtrdIntrBkSttlmAmtField = New ActiveCurrencyAndAmount
            Me.orgnlGrpInfField = New OriginalGroupInformation3
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
        Public Property RtrId() As String
            Get
                Return Me.cxlIdField
            End Get
            Set(ByVal value As String)
                Me.cxlIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)> _
        Public Property OrgnlGrpInf() As OriginalGroupInformation3
            Get
                Return Me.orgnlGrpInfField
            End Get
            Set(ByVal value As OriginalGroupInformation3)
                Me.orgnlGrpInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)> _
        Public Property OrgnlInstrId() As String
            Get
                Return Me.orgnlInstrIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlInstrIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)> _
        Public Property OrgnlEndToEndId() As String
            Get
                Return Me.orgnlEndToEndIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlEndToEndIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)> _
        Public Property OrgnlTxId() As String
            Get
                Return Me.orgnlTxIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlTxIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)> _
        Public Property OrgnlIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.orgnlIntrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.orgnlIntrBkSttlmAmtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)> _
        Public Property RtrdIntrBkSttlmAmt() As ActiveCurrencyAndAmount
            Get
                Return Me.RtrdIntrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.RtrdIntrBkSttlmAmtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)> _
        Public Property RtrRsnInf() As CancellationReasonInformation3
            Get
                Return Me.cxlRsnInfField
            End Get
            Set(ByVal value As CancellationReasonInformation3)
                Me.cxlRsnInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)> _
        Public Property OrgnlTxRef() As OriginalTransactionReference13
            Get
                Return Me.orgnlTxRefField
            End Get
            Set(ByVal value As OriginalTransactionReference13)
                Me.orgnlTxRefField = value
            End Set
        End Property
    End Class

    '<System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
    ' System.SerializableAttribute(), _
    ' System.Diagnostics.DebuggerStepThroughAttribute(), _
    ' System.ComponentModel.DesignerCategoryAttribute("code"), _
    ' System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01"), _
    ' System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=True)> _
    'Partial Public Class ControlData1
    '    Inherits EntityBase(Of ControlData1)

    '    <EditorBrowsable(EditorBrowsableState.Never)> _
    '    Private nbOfTxsField As String

    '    <System.Xml.Serialization.XmlElementAttribute(Order:=0)> _
    '    Public Property NbOfTxs() As String
    '        Get
    '            Return Me.nbOfTxsField
    '        End Get
    '        Set(ByVal value As String)
    '            Me.nbOfTxsField = Value
    '        End Set
    '    End Property
    'End Class

    '<System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5420"), _
    ' System.SerializableAttribute(), _
    ' System.Diagnostics.DebuggerStepThroughAttribute(), _
    ' System.ComponentModel.DesignerCategoryAttribute("code"), _
    ' System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01"), _
    ' System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=True)> _
    'Partial Public Class UnderlyingTransaction2
    '    Inherits EntityBase(Of UnderlyingTransaction2)

    '    <EditorBrowsable(EditorBrowsableState.Never)> _
    '    Private txInfField As List(Of PaymentTransactionInformation31)

    '    '''<summary>
    '    '''UnderlyingTransaction2 class constructor
    '    '''</summary>
    '    'Public Sub New()
    '    '    MyBase.New()
    '    '    Me.txInfField = New List(Of PaymentTransactionInformation31)
    '    'End Sub

    '    '<System.Xml.Serialization.XmlElementAttribute("TxInf", Order:=0)> _
    '    'Public Property TxInf() As List(Of PaymentTransactionInformation31)
    '    '    Get
    '    '        Return Me.txInfField
    '    '    End Get
    '    '    Set(ByVal value As List(Of PaymentTransactionInformation31))
    '    '        Me.txInfField = Value
    '    '    End Set
    '    'End Property
    'End Class
End Namespace
