Imports System
Imports System.Diagnostics
Imports System.Xml.Serialization
Imports System.Collections
Imports System.Xml.Schema
Imports System.ComponentModel
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports BrClearing.Common.BRISO20022DD416

Namespace BRISO20022Response
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

        Public Overridable Function Serialize() As String
            Dim streamReader As System.IO.StreamReader = Nothing
            Dim memoryStream As System.IO.MemoryStream = Nothing

            Try
                memoryStream = New System.IO.MemoryStream()
                Serializer.Serialize(memoryStream, Me)
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin)
                streamReader = New System.IO.StreamReader(memoryStream)
                Return streamReader.ReadToEnd()
            Finally

                If (streamReader IsNot Nothing) Then
                    streamReader.Dispose()
                End If

                If (memoryStream IsNot Nothing) Then
                    memoryStream.Dispose()
                End If
            End Try
        End Function

        Public Shared Function Deserialize(ByVal xml As String, <Out> ByRef obj As T, <Out> ByRef exception As System.Exception) As Boolean
            exception = Nothing
            obj = Nothing

            Try
                obj = Deserialize(xml)
                Return True
            Catch ex As System.Exception
                exception = ex
                Return False
            End Try
        End Function

        Public Shared Function Deserialize(ByVal xml As String, <Out> ByRef obj As T) As Boolean
            Dim exception As System.Exception = Nothing
            Return Deserialize(xml, obj, exception)
        End Function

        Public Shared Function Deserialize(ByVal xml As String) As T
            Dim stringReader As System.IO.StringReader = Nothing

            Try
                stringReader = New System.IO.StringReader(xml)
                Return (CType((Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))), T))
            Finally

                If (stringReader IsNot Nothing) Then
                    stringReader.Dispose()
                End If
            End Try
        End Function

        Public Overridable Function SaveToFile(ByVal fileName As String, <Out> ByRef exception As System.Exception) As Boolean
            exception = Nothing

            Try
                SaveToFile(fileName)
                Return True
            Catch e As System.Exception
                exception = e
                Return False
            End Try
        End Function

        Public Overridable Sub SaveToFile(ByVal fileName As String)
            Dim streamWriter As System.IO.StreamWriter = Nothing

            Try
                Dim xmlString As String = Serialize()
                Dim xmlFile As System.IO.FileInfo = New System.IO.FileInfo(fileName)
                streamWriter = xmlFile.CreateText()
                streamWriter.WriteLine(xmlString)
                streamWriter.Close()
            Finally

                If (streamWriter IsNot Nothing) Then
                    streamWriter.Dispose()
                End If
            End Try
        End Sub

        Public Shared Function LoadFromFile(ByVal fileName As String, <Out> ByRef obj As T, <Out> ByRef exception As System.Exception) As Boolean
            exception = Nothing
            obj = Nothing

            Try
                obj = LoadFromFile(fileName)
                Return True
            Catch ex As System.Exception
                exception = ex
                Return False
            End Try
        End Function

        Public Shared Function LoadFromFile(ByVal fileName As String, <Out> ByRef obj As T) As Boolean
            Dim exception As System.Exception = Nothing
            Return LoadFromFile(fileName, obj, exception)
        End Function

        Public Shared Function LoadFromFile(ByVal fileName As String) As T
            Dim file As System.IO.FileStream = Nothing
            Dim sr As System.IO.StreamReader = Nothing

            Try
                file = New System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read)
                sr = New System.IO.StreamReader(file)
                Dim xmlString As String = sr.ReadToEnd()
                sr.Close()
                file.Close()
                Return Deserialize(xmlString)
            Finally

                If (file IsNot Nothing) Then
                    file.Dispose()
                End If

                If (sr IsNot Nothing) Then
                    sr.Dispose()
                End If
            End Try
        End Function

        Public Overridable Function Clone() As T
            Return (CType((Me.MemberwiseClone()), T))
        End Function
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=False)>
    Partial Public Class Document
        Inherits EntityBase(Of Document)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private fIToFIPmtStsRptField As FIToFIPaymentStatusReportV03

        Public Sub New()
            MyBase.New()
            Me.fIToFIPmtStsRptField = New FIToFIPaymentStatusReportV03
        End Sub
        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property FIToFIPmtStsRpt As FIToFIPaymentStatusReportV03
            Get
                Return Me.fIToFIPmtStsRptField
            End Get
            Set(ByVal value As FIToFIPaymentStatusReportV03)
                Me.fIToFIPmtStsRptField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class FIToFIPaymentStatusReportV03
        Inherits EntityBase(Of FIToFIPaymentStatusReportV03)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private grpHdrField As GroupHeader37
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlGrpInfAndStsField As OriginalGroupInformation20
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private txInfAndStsField As List(Of PaymentTransactionInformation26)

        Public Sub New()
            MyBase.New()
            Me.grpHdrField = New GroupHeader37
            Me.orgnlGrpInfAndStsField = New OriginalGroupInformation20
            Me.txInfAndStsField = New List(Of PaymentTransactionInformation26)
        End Sub
        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property GrpHdr As GroupHeader37
            Get
                Return Me.grpHdrField
            End Get
            Set(ByVal value As GroupHeader37)
                Me.grpHdrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property OrgnlGrpInfAndSts As OriginalGroupInformation20
            Get
                Return Me.orgnlGrpInfAndStsField
            End Get
            Set(ByVal value As OriginalGroupInformation20)
                Me.orgnlGrpInfAndStsField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("TxInfAndSts", Order:=2)>
        Public Property TxInfAndSts As List(Of PaymentTransactionInformation26)
            Get
                Return Me.txInfAndStsField
            End Get
            Set(ByVal value As List(Of PaymentTransactionInformation26))
                Me.txInfAndStsField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class GroupHeader37
        Inherits EntityBase(Of GroupHeader37)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private msgIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private creDtTmField As System.DateTime
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private instgAgtField As FinancialInstitution4
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private instdAgtField As FinancialInstitution4

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property MsgId As String
            Get
                Return Me.msgIdField
            End Get
            Set(ByVal value As String)
                Me.msgIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property CreDtTm As System.DateTime
            Get
                Return Me.creDtTmField
            End Get
            Set(ByVal value As System.DateTime)
                Me.creDtTmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property InstgAgt As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class FinancialInstitution4
        Inherits EntityBase(Of FinancialInstitution4)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private finInstnIdField As FinancialInstitutionIdentification7

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property FinInstnId As FinancialInstitutionIdentification7
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification7)
                Me.finInstnIdField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class FinancialInstitutionIdentification7
        Inherits EntityBase(Of FinancialInstitutionIdentification7)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private bICField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property BIC As String
            Get
                Return Me.bICField
            End Get
            Set(ByVal value As String)
                Me.bICField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class AccountIdentification4Choice
        Inherits EntityBase(Of AccountIdentification4Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)>
        Public Property Item As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class CashAccount17
        Inherits EntityBase(Of CashAccount17)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As AccountIdentification4Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Id As AccountIdentification4Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentification4Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PostalAddress7
        Inherits EntityBase(Of PostalAddress7)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ctryField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private adrLineField As List(Of String)

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Ctry As String
            Get
                Return Me.ctryField
            End Get
            Set(ByVal value As String)
                Me.ctryField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("AdrLine", Order:=1)>
        Public Property AdrLine As List(Of String)
            Get
                Return Me.adrLineField
            End Get
            Set(ByVal value As List(Of String))
                Me.adrLineField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PartyIdentification33
        Inherits EntityBase(Of PartyIdentification33)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private nmField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private pstlAdrField As PostalAddress7
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As Party6Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Nm As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property PstlAdr As PostalAddress7
            Get
                Return Me.pstlAdrField
            End Get
            Set(ByVal value As PostalAddress7)
                Me.pstlAdrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property Id As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class Party6Choice
        Inherits EntityBase(Of Party6Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("OrgId", GetType(OrganisationIdentification4), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("PrvtId", GetType(PersonIdentification5), Order:=0)>
        Public Property Item As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class OrganisationIdentification4
        Inherits EntityBase(Of OrganisationIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("BICOrBEI", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericOrganisationIdentification1), Order:=0)>
        Public Property Item As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class GenericOrganisationIdentification1
        Inherits EntityBase(Of GenericOrganisationIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private schmeNmField As OrganisationIdentificationSchemeName1Choice
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private issrField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Id As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property SchmeNm As OrganisationIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As OrganisationIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property Issr As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class OrganisationIdentificationSchemeName1Choice
        Inherits EntityBase(Of OrganisationIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemElementNameField As ItemChoiceType

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")>
        Public Property Item As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ItemElementName As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema:=False)>
    Public Enum ItemChoiceType
        Cd
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PersonIdentification5
        Inherits EntityBase(Of PersonIdentification5)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("DtAndPlcOfBirth", GetType(DateAndPlaceOfBirth), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("Othr", GetType(GenericPersonIdentification1), Order:=0)>
        Public Property Item As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class DateAndPlaceOfBirth
        Inherits EntityBase(Of DateAndPlaceOfBirth)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private birthDtField As System.DateTime
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private prvcOfBirthField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cityOfBirthField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ctryOfBirthField As String

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=0)>
        Public Property BirthDt As System.DateTime
            Get
                Return Me.birthDtField
            End Get
            Set(ByVal value As System.DateTime)
                Me.birthDtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property PrvcOfBirth As String
            Get
                Return Me.prvcOfBirthField
            End Get
            Set(ByVal value As String)
                Me.prvcOfBirthField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property CityOfBirth As String
            Get
                Return Me.cityOfBirthField
            End Get
            Set(ByVal value As String)
                Me.cityOfBirthField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property CtryOfBirth As String
            Get
                Return Me.ctryOfBirthField
            End Get
            Set(ByVal value As String)
                Me.ctryOfBirthField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class GenericPersonIdentification1
        Inherits EntityBase(Of GenericPersonIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private schmeNmField As PersonIdentificationSchemeName1Choice
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private issrField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Id As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property SchmeNm As PersonIdentificationSchemeName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As PersonIdentificationSchemeName1Choice)
                Me.schmeNmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property Issr As String
            Get
                Return Me.issrField
            End Get
            Set(ByVal value As String)
                Me.issrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PersonIdentificationSchemeName1Choice
        Inherits EntityBase(Of PersonIdentificationSchemeName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemElementNameField As ItemChoiceType1

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")>
        Public Property Item As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ItemElementName As ItemChoiceType1
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType1)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema:=False)>
    Public Enum ItemChoiceType1
        Cd
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PartyIdentification32
        Inherits EntityBase(Of PartyIdentification32)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private nmField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As Party6Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Nm As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property Id As Party6Choice
            Get
                Return Me.idField
            End Get
            Set(ByVal value As Party6Choice)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class CreditorReferenceType1Choice
        Inherits EntityBase(Of CreditorReferenceType1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As DocumentType3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)>
        Public Property Item As DocumentType3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As DocumentType3Code)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Public Enum DocumentType3Code
        SCOR
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class CreditorReferenceType2
        Inherits EntityBase(Of CreditorReferenceType2)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cdOrPrtryField As CreditorReferenceType1Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property CdOrPrtry As CreditorReferenceType1Choice
            Get
                Return Me.cdOrPrtryField
            End Get
            Set(ByVal value As CreditorReferenceType1Choice)
                Me.cdOrPrtryField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class CreditorReferenceInformation2
        Inherits EntityBase(Of CreditorReferenceInformation2)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private tpField As CreditorReferenceType2
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private refField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Tp As CreditorReferenceType2
            Get
                Return Me.tpField
            End Get
            Set(ByVal value As CreditorReferenceType2)
                Me.tpField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property Ref As String
            Get
                Return Me.refField
            End Get
            Set(ByVal value As String)
                Me.refField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class StructuredRemittanceInformation7
        Inherits EntityBase(Of StructuredRemittanceInformation7)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cdtrRefInfField As CreditorReferenceInformation2

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property CdtrRefInf As CreditorReferenceInformation2
            Get
                Return Me.cdtrRefInfField
            End Get
            Set(ByVal value As CreditorReferenceInformation2)
                Me.cdtrRefInfField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class RemittanceInformation5
        Inherits EntityBase(Of RemittanceInformation5)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Strd", GetType(StructuredRemittanceInformation7), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("Ustrd", GetType(String), Order:=0)>
        Public Property Item As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class ChequeDetails
        Inherits EntityBase(Of ChequeDetails)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private chkNmbrField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private accNoField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private microcodeField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private bankCodeField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private branchCodeField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private endorsementField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private truncDtTmField As System.DateTime
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private truncDtTmFieldSpecified As Boolean

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property ChkNmbr As String
            Get
                Return Me.chkNmbrField
            End Get
            Set(ByVal value As String)
                Me.chkNmbrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property AccNo As String
            Get
                Return Me.accNoField
            End Get
            Set(ByVal value As String)
                Me.accNoField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property Microcode As String
            Get
                Return Me.microcodeField
            End Get
            Set(ByVal value As String)
                Me.microcodeField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property BankCode As String
            Get
                Return Me.bankCodeField
            End Get
            Set(ByVal value As String)
                Me.bankCodeField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)>
        Public Property BranchCode As String
            Get
                Return Me.branchCodeField
            End Get
            Set(ByVal value As String)
                Me.branchCodeField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)>
        Public Property Endorsement As String
            Get
                Return Me.endorsementField
            End Get
            Set(ByVal value As String)
                Me.endorsementField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)>
        Public Property TruncDtTm As System.DateTime
            Get
                Return Me.truncDtTmField
            End Get
            Set(ByVal value As System.DateTime)
                Me.truncDtTmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TruncDtTmSpecified As Boolean
            Get
                Return Me.truncDtTmFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.truncDtTmFieldSpecified = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class RestrictedInstitutionSchemaName1Choice
        Inherits EntityBase(Of RestrictedInstitutionSchemaName1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private prtryField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Prtry As String
            Get
                Return Me.prtryField
            End Get
            Set(ByVal value As String)
                Me.prtryField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class RestrictedIdentification1
        Inherits EntityBase(Of RestrictedIdentification1)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private schmeNmField As RestrictedInstitutionSchemaName1Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Id As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property SchmeNm As RestrictedInstitutionSchemaName1Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As RestrictedInstitutionSchemaName1Choice)
                Me.schmeNmField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class FinancialInstitutionIdentification8
        Inherits EntityBase(Of FinancialInstitutionIdentification8)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private othrField As RestrictedIdentification1

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Othr As RestrictedIdentification1
            Get
                Return Me.othrField
            End Get
            Set(ByVal value As RestrictedIdentification1)
                Me.othrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class FinancialInstitution5
        Inherits EntityBase(Of FinancialInstitution5)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private finInstnIdField As FinancialInstitutionIdentification8

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property FinInstnId As FinancialInstitutionIdentification8
            Get
                Return Me.finInstnIdField
            End Get
            Set(ByVal value As FinancialInstitutionIdentification8)
                Me.finInstnIdField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class AccountIdentificationIBAN
        Inherits EntityBase(Of AccountIdentificationIBAN)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("IBAN", Order:=0)>
        Public Property Item As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class CashAccount16
        Inherits EntityBase(Of CashAccount16)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As AccountIdentificationIBAN

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Id As AccountIdentificationIBAN
            Get
                Return Me.idField
            End Get
            Set(ByVal value As AccountIdentificationIBAN)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class RestrictedPersonIdentificationSchemaName2Choice
        Inherits EntityBase(Of RestrictedPersonIdentificationSchemaName2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private prtryField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Prtry As String
            Get
                Return Me.prtryField
            End Get
            Set(ByVal value As String)
                Me.prtryField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class RestrictedIdentification2
        Inherits EntityBase(Of RestrictedIdentification2)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private schmeNmField As RestrictedPersonIdentificationSchemaName2Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Id As String
            Get
                Return Me.idField
            End Get
            Set(ByVal value As String)
                Me.idField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property SchmeNm As RestrictedPersonIdentificationSchemaName2Choice
            Get
                Return Me.schmeNmField
            End Get
            Set(ByVal value As RestrictedPersonIdentificationSchemaName2Choice)
                Me.schmeNmField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PersonIdentification4
        Inherits EntityBase(Of PersonIdentification4)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private othrField As RestrictedIdentification2

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Othr As RestrictedIdentification2
            Get
                Return Me.othrField
            End Get
            Set(ByVal value As RestrictedIdentification2)
                Me.othrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PartyPrivate1
        Inherits EntityBase(Of PartyPrivate1)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As PersonIdentification4

        <System.Xml.Serialization.XmlElementAttribute("PrvtId", Order:=0)>
        Public Property Item As PersonIdentification4
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As PersonIdentification4)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PartyIdentification35
        Inherits EntityBase(Of PartyIdentification35)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private nmField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As PartyPrivate1

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Nm As String
            Get
                Return Me.nmField
            End Get
            Set(ByVal value As String)
                Me.nmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property Id As PartyPrivate1
            Get
                Return Me.idField
            End Get
            Set(ByVal value As PartyPrivate1)
                Me.idField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class AmendmentInformationDetails6
        Inherits EntityBase(Of AmendmentInformationDetails6)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlMndtIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlCdtrSchmeIdField As PartyIdentification35
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlDbtrAcctField As CashAccount16
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlDbtrAgtField As FinancialInstitution5

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property OrgnlMndtId As String
            Get
                Return Me.orgnlMndtIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMndtIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property OrgnlCdtrSchmeId As PartyIdentification35
            Get
                Return Me.orgnlCdtrSchmeIdField
            End Get
            Set(ByVal value As PartyIdentification35)
                Me.orgnlCdtrSchmeIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property OrgnlDbtrAcct As CashAccount16
            Get
                Return Me.orgnlDbtrAcctField
            End Get
            Set(ByVal value As CashAccount16)
                Me.orgnlDbtrAcctField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property OrgnlDbtrAgt As FinancialInstitution5
            Get
                Return Me.orgnlDbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution5)
                Me.orgnlDbtrAgtField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class MandateRelatedInformation6
        Inherits EntityBase(Of MandateRelatedInformation6)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private mndtIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private dtOfSgntrField As System.DateTime
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private amdmntIndField As Boolean
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private amdmntIndFieldSpecified As Boolean
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private amdmntInfDtlsField As AmendmentInformationDetails6
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private elctrncSgntrField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property MndtId As String
            Get
                Return Me.mndtIdField
            End Get
            Set(ByVal value As String)
                Me.mndtIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=1)>
        Public Property DtOfSgntr As System.DateTime
            Get
                Return Me.dtOfSgntrField
            End Get
            Set(ByVal value As System.DateTime)
                Me.dtOfSgntrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property AmdmntInd As Boolean
            Get
                Return Me.amdmntIndField
            End Get
            Set(ByVal value As Boolean)
                Me.amdmntIndField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmdmntIndSpecified As Boolean
            Get
                Return Me.amdmntIndFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.amdmntIndFieldSpecified = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property AmdmntInfDtls As AmendmentInformationDetails6
            Get
                Return Me.amdmntInfDtlsField
            End Get
            Set(ByVal value As AmendmentInformationDetails6)
                Me.amdmntInfDtlsField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)>
        Public Property ElctrncSgntr As String
            Get
                Return Me.elctrncSgntrField
            End Get
            Set(ByVal value As String)
                Me.elctrncSgntrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class CategoryPurpose1Choice
        Inherits EntityBase(Of CategoryPurpose1Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemElementNameField As ItemChoiceType3

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")>
        Public Property Item As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ItemElementName As ItemChoiceType3
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType3)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema:=False)>
    Public Enum ItemChoiceType3
        Cd
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class LocalInstrument2Choice
        Inherits EntityBase(Of LocalInstrument2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemElementNameField As ItemChoiceType2

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("Prtry", GetType(String), Order:=0)>
        <System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")>
        Public Property Item As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ItemElementName As ItemChoiceType2
            Get
                Return Me.itemElementNameField
            End Get
            Set(ByVal value As ItemChoiceType2)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema:=False)>
    Public Enum ItemChoiceType2
        Cd
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class ServiceLevel9Choice
        Inherits EntityBase(Of ServiceLevel9Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As ServiceLevel3Code

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)>
        Public Property Item As ServiceLevel3Code
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ServiceLevel3Code)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Public Enum ServiceLevel3Code
        SEPA
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class PaymentTypeInformation22
        Inherits EntityBase(Of PaymentTypeInformation22)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private svcLvlField As ServiceLevel9Choice
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private lclInstrmField As LocalInstrument2Choice
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private seqTpField As SequenceType1Code
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private seqTpFieldSpecified As Boolean
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ctgyPurpField As CategoryPurpose1Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property SvcLvl As ServiceLevel9Choice
            Get
                Return Me.svcLvlField
            End Get
            Set(ByVal value As ServiceLevel9Choice)
                Me.svcLvlField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property LclInstrm As LocalInstrument2Choice
            Get
                Return Me.lclInstrmField
            End Get
            Set(ByVal value As LocalInstrument2Choice)
                Me.lclInstrmField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property SeqTp As SequenceType1Code
            Get
                Return Me.seqTpField
            End Get
            Set(ByVal value As SequenceType1Code)
                Me.seqTpField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SeqTpSpecified As Boolean
            Get
                Return Me.seqTpFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.seqTpFieldSpecified = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property CtgyPurp As CategoryPurpose1Choice
            Get
                Return Me.ctgyPurpField
            End Get
            Set(ByVal value As CategoryPurpose1Choice)
                Me.ctgyPurpField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Public Enum SequenceType1Code
        FRST
        RCUR
        FNAL
        OOFF
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class ClearingSystemIdentification3Choice
        Inherits EntityBase(Of ClearingSystemIdentification3Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As ClearingSystemIdentification

        <System.Xml.Serialization.XmlElementAttribute("Prtry", Order:=0)>
        Public Property Item As ClearingSystemIdentification
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As ClearingSystemIdentification)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Public Enum ClearingSystemIdentification
        ACH
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class SettlementInformation13
        Inherits EntityBase(Of SettlementInformation13)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private sttlmMtdField As SettlementMethod1Code
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private clrSysField As ClearingSystemIdentification3Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property SttlmMtd As SettlementMethod1Code
            Get
                Return Me.sttlmMtdField
            End Get
            Set(ByVal value As SettlementMethod1Code)
                Me.sttlmMtdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property ClrSys As ClearingSystemIdentification3Choice
            Get
                Return Me.clrSysField
            End Get
            Set(ByVal value As ClearingSystemIdentification3Choice)
                Me.clrSysField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Public Enum SettlementMethod1Code
        CLRG
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class OriginalTransactionReference13
        Inherits EntityBase(Of OriginalTransactionReference13)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private intrBkSttlmAmtField As ActiveCurrencyAndAmount
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private intrBkSttlmDtField As System.DateTime
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private intrBkSttlmDtFieldSpecified As Boolean
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private reqdColltnDtField As System.DateTime
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private reqdColltnDtFieldSpecified As Boolean
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cdtrSchmeIdField As PartyIdentification34
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private sttlmInfField As SettlementInformation13
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private pmtTpInfField As PaymentTypeInformation22
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private mndtRltdInfField As MandateRelatedInformation6
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private chequeTxField As ChequeDetails
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private rmtInfField As RemittanceInformation5
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ultmtDbtrField As PartyIdentification32
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private dbtrField As PartyIdentification33
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private dbtrAcctField As CashAccount17
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private dbtrAgtField As FinancialInstitution4
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cdtrAgtField As FinancialInstitution4
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cdtrField As PartyIdentification33
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cdtrAcctField As CashAccount17
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ultmtCdtrField As PartyIdentification32

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property IntrBkSttlmAmt As ActiveCurrencyAndAmount
            Get
                Return Me.intrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.intrBkSttlmAmtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute("IntrBkSttlmDt", DataType:="date", Order:=1)>
        Public Property IntrBkSttlmDtt As System.DateTime
            Get
                Return Me.intrBkSttlmDtField
            End Get
            Set(ByVal value As System.DateTime)
                Me.intrBkSttlmDtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property IntrBkSttlmDtSpecified As Boolean
            Get
                Return Me.intrBkSttlmDtFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.intrBkSttlmDtFieldSpecified = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=2)>
        Public Property ReqdColltnDt As System.DateTime
            Get
                Return Me.reqdColltnDtField
            End Get
            Set(ByVal value As System.DateTime)
                Me.reqdColltnDtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ReqdColltnDtSpecified As Boolean
            Get
                Return Me.reqdColltnDtFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.reqdColltnDtFieldSpecified = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property CdtrSchmeId As PartyIdentification34
            Get
                Return Me.cdtrSchmeIdField
            End Get
            Set(ByVal value As PartyIdentification34)
                Me.cdtrSchmeIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)>
        Public Property SttlmInf As SettlementInformation13
            Get
                Return Me.sttlmInfField
            End Get
            Set(ByVal value As SettlementInformation13)
                Me.sttlmInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)>
        Public Property PmtTpInf As PaymentTypeInformation22
            Get
                Return Me.pmtTpInfField
            End Get
            Set(ByVal value As PaymentTypeInformation22)
                Me.pmtTpInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)>
        Public Property MndtRltdInf As MandateRelatedInformation6
            Get
                Return Me.mndtRltdInfField
            End Get
            Set(ByVal value As MandateRelatedInformation6)
                Me.mndtRltdInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)>
        Public Property ChequeTx As ChequeDetails
            Get
                Return Me.chequeTxField
            End Get
            Set(ByVal value As ChequeDetails)
                Me.chequeTxField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)>
        Public Property RmtInf As RemittanceInformation5
            Get
                Return Me.rmtInfField
            End Get
            Set(ByVal value As RemittanceInformation5)
                Me.rmtInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=9)>
        Public Property UltmtDbtr As PartyIdentification32
            Get
                Return Me.ultmtDbtrField
            End Get
            Set(ByVal value As PartyIdentification32)
                Me.ultmtDbtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=10)>
        Public Property Dbtr As PartyIdentification33
            Get
                Return Me.dbtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.dbtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=11)>
        Public Property DbtrAcct As CashAccount17
            Get
                Return Me.dbtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.dbtrAcctField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=12)>
        Public Property DbtrAgt As FinancialInstitution4
            Get
                Return Me.dbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.dbtrAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=13)>
        Public Property CdtrAgt As FinancialInstitution4
            Get
                Return Me.cdtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.cdtrAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=14)>
        Public Property Cdtr As PartyIdentification33
            Get
                Return Me.cdtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.cdtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=15)>
        Public Property CdtrAcct As CashAccount17
            Get
                Return Me.cdtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.cdtrAcctField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=16)>
        Public Property UltmtCdtr As PartyIdentification32
            Get
                Return Me.ultmtCdtrField
            End Get
            Set(ByVal value As PartyIdentification32)
                Me.ultmtCdtrField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class ActiveCurrencyAndAmount
        Inherits EntityBase(Of ActiveCurrencyAndAmount)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ccyField As ActiveCurrencyCode
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private valueField As String

        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Ccy As ActiveCurrencyCode
            Get
                Return Me.ccyField
            End Get
            Set(ByVal value As ActiveCurrencyCode)
                Me.ccyField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value As String
            Get
                Return Me.valueField
            End Get
            Set(ByVal value As String)
                Me.valueField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Public Enum ActiveCurrencyCode
        ETB
        USD
        EUR
        GBP
        JPY
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class PartyIdentification34
        Inherits EntityBase(Of PartyIdentification34)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Id", GetType(Party6Choice), Order:=0)>
        <System.Xml.Serialization.XmlElementAttribute("Nm", GetType(String), Order:=0)>
        Public Property Item As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Partial Public Class ChargesInformation5
        Inherits EntityBase(Of ChargesInformation5)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private amtField As ActiveCurrencyAndAmount
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ptyField As FinancialInstitution4

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Amt As ActiveCurrencyAndAmount
            Get
                Return Me.amtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.amtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property Pty As FinancialInstitution4
            Get
                Return Me.ptyField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.ptyField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class PaymentTransactionInformation26
        Inherits EntityBase(Of PaymentTransactionInformation26)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private stsIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlInstrIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlEndToEndIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlTxIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private txStsField As TransactionIndividualStatus3Code
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private txStsFieldSpecified As Boolean
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private stsRsnInfField As StatusReasonInformation8
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private chrgsInfField As ChargesInformation5
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private instgAgtField As FinancialInstitution4
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private instdAgtField As FinancialInstitution4
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlTxRefField As OriginalTransactionReference13


        Public Sub New()
            MyBase.New()
            Me.stsRsnInfField = New StatusReasonInformation8
            Me.chrgsInfField = New ChargesInformation5
            Me.instgAgtField = New FinancialInstitution4
            Me.orgnlTxRefField = New OriginalTransactionReference13
            Me.instdAgtField = New FinancialInstitution4
        End Sub

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property StsId As String
            Get
                Return Me.stsIdField
            End Get
            Set(ByVal value As String)
                Me.stsIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property OrgnlInstrId As String
            Get
                Return Me.orgnlInstrIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlInstrIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property OrgnlEndToEndId As String
            Get
                Return Me.orgnlEndToEndIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlEndToEndIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property OrgnlTxId As String
            Get
                Return Me.orgnlTxIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlTxIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)>
        Public Property TxSts As TransactionIndividualStatus3Code
            Get
                Return Me.txStsField
            End Get
            Set(ByVal value As TransactionIndividualStatus3Code)
                Me.txStsField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TxStsSpecified As Boolean
            Get
                Return Me.txStsFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.txStsFieldSpecified = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)>
        Public Property StsRsnInf As StatusReasonInformation8
            Get
                Return Me.stsRsnInfField
            End Get
            Set(ByVal value As StatusReasonInformation8)
                Me.stsRsnInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)>
        Public Property ChrgsInf As ChargesInformation5
            Get
                Return Me.chrgsInfField
            End Get
            Set(ByVal value As ChargesInformation5)
                Me.chrgsInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)>
        Public Property InstgAgt As FinancialInstitution4
            Get
                Return Me.instgAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instgAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)>
        Public Property InstdAgt As FinancialInstitution4
            Get
                Return Me.instdAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.instdAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=9)>
        Public Property OrgnlTxRef As OriginalTransactionReference13
            Get
                Return Me.orgnlTxRefField
            End Get
            Set(ByVal value As OriginalTransactionReference13)
                Me.orgnlTxRefField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Public Enum TransactionIndividualStatus3Code
        RJCT
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class StatusReasonInformation8
        Inherits EntityBase(Of StatusReasonInformation8)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgtrField As PartyIdentification34
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private rsnField As StatusReason6Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Orgtr As PartyIdentification34
            Get
                Return Me.orgtrField
            End Get
            Set(ByVal value As PartyIdentification34)
                Me.orgtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property Rsn As StatusReason6Choice
            Get
                Return Me.rsnField
            End Get
            Set(ByVal value As StatusReason6Choice)
                Me.rsnField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class StatusReason6Choice
        Inherits EntityBase(Of StatusReason6Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As String

        <System.Xml.Serialization.XmlElementAttribute("Cd", Order:=0)>
        Public Property Item As String
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As String)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class OriginalGroupInformation20
        Inherits EntityBase(Of OriginalGroupInformation20)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlMsgIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlMsgNmIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private grpStsField As TransactionGroupStatus3Code
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private grpStsFieldSpecified As Boolean
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private stsRsnInfField As StatusReasonInformation8

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property OrgnlMsgId As String
            Get
                Return Me.orgnlMsgIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMsgIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property OrgnlMsgNmId As String
            Get
                Return Me.orgnlMsgNmIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlMsgNmIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property GrpSts As TransactionGroupStatus3Code
            Get
                Return Me.grpStsField
            End Get
            Set(ByVal value As TransactionGroupStatus3Code)
                Me.grpStsField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GrpStsSpecified As Boolean
            Get
                Return Me.grpStsFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.grpStsFieldSpecified = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property StsRsnInf As StatusReasonInformation8
            Get
                Return Me.stsRsnInfField
            End Get
            Set(ByVal value As StatusReasonInformation8)
                Me.stsRsnInfField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    Public Enum TransactionGroupStatus3Code
        ACCP
        ACSC
        PART
        RJCT
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class OrganisationIdentification5
        Inherits EntityBase(Of OrganisationIdentification5)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private bICOrBEIField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property BICOrBEI As String
            Get
                Return Me.bICOrBEIField
            End Get
            Set(ByVal value As String)
                Me.bICOrBEIField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable:=True)>
    Partial Public Class Party7Choice
        Inherits EntityBase(Of Party7Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As OrganisationIdentification5

        <System.Xml.Serialization.XmlElementAttribute("OrgId", Order:=0)>
        Public Property Item As OrganisationIdentification5
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As OrganisationIdentification5)
                Me.itemField = value
            End Set
        End Property
    End Class
End Namespace

