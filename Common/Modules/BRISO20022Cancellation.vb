Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Xml.Serialization

Namespace BRISO20022Cancellation
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=False)>
    Partial Public Class Document
        Inherits EntityBase(Of Document)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private fIToFIPmtCxlReqField As FIToFIPaymentCancellationRequestV01

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property FIToFIPmtCxlReq As FIToFIPaymentCancellationRequestV01
            Get
                Return Me.fIToFIPmtCxlReqField
            End Get
            Set(ByVal value As FIToFIPaymentCancellationRequestV01)
                Me.fIToFIPmtCxlReqField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=True)>
    Partial Public Class FIToFIPaymentCancellationRequestV01
        Inherits EntityBase(Of FIToFIPaymentCancellationRequestV01)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private assgnmtField As CaseAssignment2
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private ctrlDataField As ControlData1
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private undrlygField As List(Of PaymentTransactionInformation31)

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Assgnmt As CaseAssignment2
            Get
                Return Me.assgnmtField
            End Get
            Set(ByVal value As CaseAssignment2)
                Me.assgnmtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property CtrlData As ControlData1
            Get
                Return Me.ctrlDataField
            End Get
            Set(ByVal value As ControlData1)
                Me.ctrlDataField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlArrayAttribute(Order:=2)>
        <System.Xml.Serialization.XmlArrayItemAttribute("TxInf", GetType(PaymentTransactionInformation31), IsNullable:=False)>
        Public Property Undrlyg As List(Of PaymentTransactionInformation31)
            Get
                Return Me.undrlygField
            End Get
            Set(ByVal value As List(Of PaymentTransactionInformation31))
                Me.undrlygField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Partial Public Class CaseAssignment2
        Inherits EntityBase(Of CaseAssignment2)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private assgnrField As Party7Choice
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private assgneField As Party7Choice
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private creDtTmField As System.DateTime

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
        Public Property Assgnr As Party7Choice
            Get
                Return Me.assgnrField
            End Get
            Set(ByVal value As Party7Choice)
                Me.assgnrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property Assgne As Party7Choice
            Get
                Return Me.assgneField
            End Get
            Set(ByVal value As Party7Choice)
                Me.assgneField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property CreDtTm As System.DateTime
            Get
                Return Me.creDtTmField
            End Get
            Set(ByVal value As System.DateTime)
                Me.creDtTmField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Partial Public Class Party7Choice
        Inherits EntityBase(Of Party7Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As FinancialInstitution4

        <System.Xml.Serialization.XmlElementAttribute("Agt", Order:=0)>
        Public Property Item As FinancialInstitution4
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.itemField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IncludeInSchema:=False)>
    Public Enum ItemChoiceType
        Cd
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IncludeInSchema:=False)>
    Public Enum ItemChoiceType1
        Cd
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Public Enum DocumentType3Code
        SCOR
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IncludeInSchema:=False)>
    Public Enum ItemChoiceType3
        Cd
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IncludeInSchema:=False)>
    Public Enum ItemChoiceType2
        Cd
        Prtry
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Public Enum ServiceLevel3Code
        SEPA
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Partial Public Class PaymentTypeInformation22
        Inherits EntityBase(Of PaymentTypeInformation22)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private svcLvlField As ServiceLevel9Choice
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private lclInstrmField As LocalInstrument2Choice
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
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Public Enum ClearingSystemIdentification
        ACH
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Public Enum SettlementMethod1Code
        CLRG
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Partial Public Class OriginalTransactionReference13
        Inherits EntityBase(Of OriginalTransactionReference13)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private sttlmInfField As SettlementInformation13
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private pmtTpInfField As PaymentTypeInformation22
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
        Public Property SttlmInf As SettlementInformation13
            Get
                Return Me.sttlmInfField
            End Get
            Set(ByVal value As SettlementInformation13)
                Me.sttlmInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property PmtTpInf As PaymentTypeInformation22
            Get
                Return Me.pmtTpInfField
            End Get
            Set(ByVal value As PaymentTypeInformation22)
                Me.pmtTpInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property RmtInf As RemittanceInformation5
            Get
                Return Me.rmtInfField
            End Get
            Set(ByVal value As RemittanceInformation5)
                Me.rmtInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property UltmtDbtr As PartyIdentification32
            Get
                Return Me.ultmtDbtrField
            End Get
            Set(ByVal value As PartyIdentification32)
                Me.ultmtDbtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)>
        Public Property Dbtr As PartyIdentification33
            Get
                Return Me.dbtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.dbtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)>
        Public Property DbtrAcct As CashAccount17
            Get
                Return Me.dbtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.dbtrAcctField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=6)>
        Public Property DbtrAgt As FinancialInstitution4
            Get
                Return Me.dbtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.dbtrAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)>
        Public Property CdtrAgt As FinancialInstitution4
            Get
                Return Me.cdtrAgtField
            End Get
            Set(ByVal value As FinancialInstitution4)
                Me.cdtrAgtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)>
        Public Property Cdtr As PartyIdentification33
            Get
                Return Me.cdtrField
            End Get
            Set(ByVal value As PartyIdentification33)
                Me.cdtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=9)>
        Public Property CdtrAcct As CashAccount17
            Get
                Return Me.cdtrAcctField
            End Get
            Set(ByVal value As CashAccount17)
                Me.cdtrAcctField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=10)>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=True)>
    Partial Public Class CancellationReason2Choice
        Inherits EntityBase(Of CancellationReason2Choice)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As Object

        <System.Xml.Serialization.XmlElementAttribute("Cd", GetType(String), Order:=0)>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Public Enum CancellationReason4Code
        DUPL
        CUST
        AGNT
        CURR
        UPAY
        CUTA
    End Enum

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Partial Public Class PartyIdentification35
        Inherits EntityBase(Of PartyIdentification35)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private itemField As Object
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private idField As Party6Choice

        <System.Xml.Serialization.XmlElementAttribute("Id", GetType(Party6Choice), Order:=0)>
        Public Property Item As Object
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As Object)
                Me.itemField = value
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=True)>
    Partial Public Class CancellationReasonInformation3
        Inherits EntityBase(Of CancellationReasonInformation3)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgtrField As PartyIdentification35
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private rsnField As CancellationReason2Choice

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property Orgtr As PartyIdentification35
            Get
                Return Me.orgtrField
            End Get
            Set(ByVal value As PartyIdentification35)
                Me.orgtrField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property Rsn As CancellationReason2Choice
            Get
                Return Me.rsnField
            End Get
            Set(ByVal value As CancellationReason2Choice)
                Me.rsnField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
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
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    Partial Public Class OriginalGroupInformation3
        Inherits EntityBase(Of OriginalGroupInformation3)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlMsgIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlMsgNmIdField As String

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
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=True)>
    Partial Public Class PaymentTransactionInformation31
        Inherits EntityBase(Of PaymentTransactionInformation31)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cxlIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlGrpInfField As OriginalGroupInformation3
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlInstrIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlEndToEndIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlTxIdField As String
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlIntrBkSttlmAmtField As ActiveCurrencyAndAmount
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlIntrBkSttlmDtField As System.DateTime
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private cxlRsnInfField As CancellationReasonInformation3
        <EditorBrowsable(EditorBrowsableState.Never)>
        Private orgnlTxRefField As OriginalTransactionReference13

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property CxlId As String
            Get
                Return Me.cxlIdField
            End Get
            Set(ByVal value As String)
                Me.cxlIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=1)>
        Public Property OrgnlGrpInf As OriginalGroupInformation3
            Get
                Return Me.orgnlGrpInfField
            End Get
            Set(ByVal value As OriginalGroupInformation3)
                Me.orgnlGrpInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=2)>
        Public Property OrgnlInstrId As String
            Get
                Return Me.orgnlInstrIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlInstrIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=3)>
        Public Property OrgnlEndToEndId As String
            Get
                Return Me.orgnlEndToEndIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlEndToEndIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=4)>
        Public Property OrgnlTxId As String
            Get
                Return Me.orgnlTxIdField
            End Get
            Set(ByVal value As String)
                Me.orgnlTxIdField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=5)>
        Public Property OrgnlIntrBkSttlmAmt As ActiveCurrencyAndAmount
            Get
                Return Me.orgnlIntrBkSttlmAmtField
            End Get
            Set(ByVal value As ActiveCurrencyAndAmount)
                Me.orgnlIntrBkSttlmAmtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(DataType:="date", Order:=6)>
        Public Property OrgnlIntrBkSttlmDt As System.DateTime
            Get
                Return Me.orgnlIntrBkSttlmDtField
            End Get
            Set(ByVal value As System.DateTime)
                Me.orgnlIntrBkSttlmDtField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=7)>
        Public Property CxlRsnInf As CancellationReasonInformation3
            Get
                Return Me.cxlRsnInfField
            End Get
            Set(ByVal value As CancellationReasonInformation3)
                Me.cxlRsnInfField = value
            End Set
        End Property

        <System.Xml.Serialization.XmlElementAttribute(Order:=8)>
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
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=True)>
    Partial Public Class ControlData1
        Inherits EntityBase(Of ControlData1)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private nbOfTxsField As String

        <System.Xml.Serialization.XmlElementAttribute(Order:=0)>
        Public Property NbOfTxs As String
            Get
                Return Me.nbOfTxsField
            End Get
            Set(ByVal value As String)
                Me.nbOfTxsField = value
            End Set
        End Property
    End Class

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")>
    <System.SerializableAttribute()>
    <System.Diagnostics.DebuggerStepThroughAttribute()>
    <System.ComponentModel.DesignerCategoryAttribute("code")>
    <System.Xml.Serialization.XmlTypeAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01")>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="urn:iso:std:iso:20022:tech:xsd:camt.056.001.01", IsNullable:=True)>
    Partial Public Class UnderlyingTransaction2
        Inherits EntityBase(Of UnderlyingTransaction2)

        <EditorBrowsable(EditorBrowsableState.Never)>
        Private txInfField As List(Of PaymentTransactionInformation31)

        <System.Xml.Serialization.XmlElementAttribute("TxInf", Order:=0)>
        Public Property TxInf As List(Of PaymentTransactionInformation31)
            Get
                Return Me.txInfField
            End Get
            Set(ByVal value As List(Of PaymentTransactionInformation31))
                Me.txInfField = value
            End Set
        End Property
    End Class
End Namespace

