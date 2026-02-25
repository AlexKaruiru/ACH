using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace BRRTGSProcessing.Response
{
    #region Base entity class
    public partial class EntityBase<T>
    {

        private static System.Xml.Serialization.XmlSerializer serializer;

        private static System.Xml.Serialization.XmlSerializer Serializer
        {
            get
            {
                if ((serializer == null))
                {
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                }
                return serializer;
            }
        }

        #region Serialize/Deserialize
        /// <summary>
        /// Serializes current EntityBase object into an XML document
        /// </summary>
        /// <returns>string XML value</returns>
        public virtual string Serialize()
        {
            System.IO.StreamReader streamReader = null;
            System.IO.MemoryStream memoryStream = null;
            try
            {
                memoryStream = new System.IO.MemoryStream();
                Serializer.Serialize(memoryStream, this);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                streamReader = new System.IO.StreamReader(memoryStream);
                return streamReader.ReadToEnd();
            }
            finally
            {
                if ((streamReader != null))
                {
                    streamReader.Dispose();
                }
                if ((memoryStream != null))
                {
                    memoryStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Deserializes workflow markup into an EntityBase object
        /// </summary>
        /// <param name="xml">string workflow markup to deserialize</param>
        /// <param name="obj">Output EntityBase object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool Deserialize(string xml, out T obj, out System.Exception exception)
        {
            exception = null;
            obj = default(T);
            try
            {
                obj = Deserialize(xml);
                return true;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public static bool Deserialize(string xml, out T obj)
        {
            System.Exception exception = null;
            return Deserialize(xml, out obj, out exception);
        }

        public static T Deserialize(string xml)
        {
            System.IO.StringReader stringReader = null;
            try
            {
                stringReader = new System.IO.StringReader(xml);
                return ((T)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
            }
            finally
            {
                if ((stringReader != null))
                {
                    stringReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Serializes current EntityBase object into file
        /// </summary>
        /// <param name="fileName">full path of outupt xml file</param>
        /// <param name="exception">output Exception value if failed</param>
        /// <returns>true if can serialize and save into file; otherwise, false</returns>
        public virtual bool SaveToFile(string fileName, out System.Exception exception)
        {
            exception = null;
            try
            {
                SaveToFile(fileName);
                return true;
            }
            catch (System.Exception e)
            {
                exception = e;
                return false;
            }
        }

        public virtual void SaveToFile(string fileName)
        {
            System.IO.StreamWriter streamWriter = null;
            try
            {
                string xmlString = Serialize();
                System.IO.FileInfo xmlFile = new System.IO.FileInfo(fileName);
                streamWriter = xmlFile.CreateText();
                streamWriter.WriteLine(xmlString);
                streamWriter.Close();
            }
            finally
            {
                if ((streamWriter != null))
                {
                    streamWriter.Dispose();
                }
            }
        }

        /// <summary>
        /// Deserializes xml markup from file into an EntityBase object
        /// </summary>
        /// <param name="fileName">string xml file to load and deserialize</param>
        /// <param name="obj">Output EntityBase object</param>
        /// <param name="exception">output Exception value if deserialize failed</param>
        /// <returns>true if this XmlSerializer can deserialize the object; otherwise, false</returns>
        public static bool LoadFromFile(string fileName, out T obj, out System.Exception exception)
        {
            exception = null;
            obj = default(T);
            try
            {
                obj = LoadFromFile(fileName);
                return true;
            }
            catch (System.Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public static bool LoadFromFile(string fileName, out T obj)
        {
            System.Exception exception = null;
            return LoadFromFile(fileName, out obj, out exception);
        }

        public static T LoadFromFile(string fileName)
        {
            System.IO.FileStream file = null;
            System.IO.StreamReader sr = null;
            try
            {
                file = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read);
                sr = new System.IO.StreamReader(file);
                string xmlString = sr.ReadToEnd();
                sr.Close();
                file.Close();
                return Deserialize(xmlString);
            }
            finally
            {
                if ((file != null))
                {
                    file.Dispose();
                }
                if ((sr != null))
                {
                    sr.Dispose();
                }
            }
        }
        #endregion

        #region Clone method
        /// <summary>
        /// Create a clone of this T object
        /// </summary>
        public virtual T Clone()
        {
            return ((T)(this.MemberwiseClone()));
        }
        #endregion
    }
    #endregion

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = false)]
    public partial class Document : EntityBase<Document>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FIToFIPaymentStatusReportV03 fIToFIPmtStsRptField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public FIToFIPaymentStatusReportV03 FIToFIPmtStsRpt
        {
            get
            {
                return this.fIToFIPmtStsRptField;
            }
            set
            {
                this.fIToFIPmtStsRptField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class FIToFIPaymentStatusReportV03 : EntityBase<FIToFIPaymentStatusReportV03>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private GroupHeader37 grpHdrField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private OriginalGroupInformation20 orgnlGrpInfAndStsField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<PaymentTransactionInformation26> txInfAndStsField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public GroupHeader37 GrpHdr
        {
            get
            {
                return this.grpHdrField;
            }
            set
            {
                this.grpHdrField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public OriginalGroupInformation20 OrgnlGrpInfAndSts
        {
            get
            {
                return this.orgnlGrpInfAndStsField;
            }
            set
            {
                this.orgnlGrpInfAndStsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("TxInfAndSts", Order = 2)]
        public List<PaymentTransactionInformation26> TxInfAndSts
        {
            get
            {
                return this.txInfAndStsField;
            }
            set
            {
                this.txInfAndStsField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class GroupHeader37 : EntityBase<GroupHeader37>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string msgIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private System.DateTime creDtTmField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitution4 instgAgtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitution4 instdAgtField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string MsgId
        {
            get
            {
                return this.msgIdField;
            }
            set
            {
                this.msgIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime CreDtTm
        {
            get
            {
                return this.creDtTmField;
            }
            set
            {
                this.creDtTmField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public FinancialInstitution4 InstgAgt
        {
            get
            {
                return this.instgAgtField;
            }
            set
            {
                this.instgAgtField = value;
            }
        }
        //Ajiwa
        //[System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        //public FinancialInstitution4 InstdAgt
        //{
        //    get
        //    {
        //        return this.instdAgtField;
        //    }
        //    set
        //    {
        //        this.instdAgtField = value;
        //    }
        //}
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class FinancialInstitution4 : EntityBase<FinancialInstitution4>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitutionIdentification7 finInstnIdField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public FinancialInstitutionIdentification7 FinInstnId
        {
            get
            {
                return this.finInstnIdField;
            }
            set
            {
                this.finInstnIdField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class FinancialInstitutionIdentification7 : EntityBase<FinancialInstitutionIdentification7>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string bICField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string BIC
        {
            get
            {
                return this.bICField;
            }
            set
            {
                this.bICField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class AccountIdentification4Choice : EntityBase<AccountIdentification4Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string itemField;

        [System.Xml.Serialization.XmlElementAttribute("IBAN", Order = 0)]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class CashAccount17 : EntityBase<CashAccount17>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private AccountIdentification4Choice idField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AccountIdentification4Choice Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PostalAddress7 : EntityBase<PostalAddress7>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string ctryField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<string> adrLineField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Ctry
        {
            get
            {
                return this.ctryField;
            }
            set
            {
                this.ctryField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AdrLine", Order = 1)]
        public List<string> AdrLine
        {
            get
            {
                return this.adrLineField;
            }
            set
            {
                this.adrLineField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PartyIdentification33 : EntityBase<PartyIdentification33>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string nmField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PostalAddress7 pstlAdrField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private Party6Choice idField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public PostalAddress7 PstlAdr
        {
            get
            {
                return this.pstlAdrField;
            }
            set
            {
                this.pstlAdrField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public Party6Choice Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class Party6Choice : EntityBase<Party6Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private object itemField;

        [System.Xml.Serialization.XmlElementAttribute("OrgId", typeof(OrganisationIdentification4), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("PrvtId", typeof(PersonIdentification5), Order = 0)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class OrganisationIdentification4 : EntityBase<OrganisationIdentification4>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private object itemField;

        [System.Xml.Serialization.XmlElementAttribute("BICOrBEI", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Othr", typeof(GenericOrganisationIdentification1), Order = 0)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class GenericOrganisationIdentification1 : EntityBase<GenericOrganisationIdentification1>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string idField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private OrganisationIdentificationSchemeName1Choice schmeNmField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string issrField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public OrganisationIdentificationSchemeName1Choice SchmeNm
        {
            get
            {
                return this.schmeNmField;
            }
            set
            {
                this.schmeNmField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Issr
        {
            get
            {
                return this.issrField;
            }
            set
            {
                this.issrField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class OrganisationIdentificationSchemeName1Choice : EntityBase<OrganisationIdentificationSchemeName1Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string itemField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ItemChoiceType itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Prtry", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        Cd,

        /// <remarks/>
        Prtry,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PersonIdentification5 : EntityBase<PersonIdentification5>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private object itemField;

        [System.Xml.Serialization.XmlElementAttribute("DtAndPlcOfBirth", typeof(DateAndPlaceOfBirth), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Othr", typeof(GenericPersonIdentification1), Order = 0)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class DateAndPlaceOfBirth : EntityBase<DateAndPlaceOfBirth>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private System.DateTime birthDtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string prvcOfBirthField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string cityOfBirthField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string ctryOfBirthField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 0)]
        public System.DateTime BirthDt
        {
            get
            {
                return this.birthDtField;
            }
            set
            {
                this.birthDtField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string PrvcOfBirth
        {
            get
            {
                return this.prvcOfBirthField;
            }
            set
            {
                this.prvcOfBirthField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string CityOfBirth
        {
            get
            {
                return this.cityOfBirthField;
            }
            set
            {
                this.cityOfBirthField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string CtryOfBirth
        {
            get
            {
                return this.ctryOfBirthField;
            }
            set
            {
                this.ctryOfBirthField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class GenericPersonIdentification1 : EntityBase<GenericPersonIdentification1>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string idField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PersonIdentificationSchemeName1Choice schmeNmField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string issrField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public PersonIdentificationSchemeName1Choice SchmeNm
        {
            get
            {
                return this.schmeNmField;
            }
            set
            {
                this.schmeNmField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Issr
        {
            get
            {
                return this.issrField;
            }
            set
            {
                this.issrField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PersonIdentificationSchemeName1Choice : EntityBase<PersonIdentificationSchemeName1Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string itemField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ItemChoiceType1 itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Prtry", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType1 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema = false)]
    public enum ItemChoiceType1
    {

        /// <remarks/>
        Cd,

        /// <remarks/>
        Prtry,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PartyIdentification32 : EntityBase<PartyIdentification32>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string nmField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private Party6Choice idField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public Party6Choice Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class CreditorReferenceType1Choice : EntityBase<CreditorReferenceType1Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private DocumentType3Code itemField;

        [System.Xml.Serialization.XmlElementAttribute("Cd", Order = 0)]
        public DocumentType3Code Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public enum DocumentType3Code
    {

        /// <remarks/>
        SCOR,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class CreditorReferenceType2 : EntityBase<CreditorReferenceType2>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private CreditorReferenceType1Choice cdOrPrtryField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CreditorReferenceType1Choice CdOrPrtry
        {
            get
            {
                return this.cdOrPrtryField;
            }
            set
            {
                this.cdOrPrtryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class CreditorReferenceInformation2 : EntityBase<CreditorReferenceInformation2>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private CreditorReferenceType2 tpField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string refField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CreditorReferenceType2 Tp
        {
            get
            {
                return this.tpField;
            }
            set
            {
                this.tpField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class StructuredRemittanceInformation7 : EntityBase<StructuredRemittanceInformation7>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private CreditorReferenceInformation2 cdtrRefInfField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CreditorReferenceInformation2 CdtrRefInf
        {
            get
            {
                return this.cdtrRefInfField;
            }
            set
            {
                this.cdtrRefInfField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class RemittanceInformation5 : EntityBase<RemittanceInformation5>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private object itemField;

        [System.Xml.Serialization.XmlElementAttribute("Strd", typeof(StructuredRemittanceInformation7), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Ustrd", typeof(string), Order = 0)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class ChequeDetails : EntityBase<ChequeDetails>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string chkNmbrField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string accNoField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string microcodeField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string bankCodeField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string branchCodeField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string endorsementField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private System.DateTime truncDtTmField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool truncDtTmFieldSpecified;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ChkNmbr
        {
            get
            {
                return this.chkNmbrField;
            }
            set
            {
                this.chkNmbrField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AccNo
        {
            get
            {
                return this.accNoField;
            }
            set
            {
                this.accNoField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Microcode
        {
            get
            {
                return this.microcodeField;
            }
            set
            {
                this.microcodeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string BankCode
        {
            get
            {
                return this.bankCodeField;
            }
            set
            {
                this.bankCodeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string BranchCode
        {
            get
            {
                return this.branchCodeField;
            }
            set
            {
                this.branchCodeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Endorsement
        {
            get
            {
                return this.endorsementField;
            }
            set
            {
                this.endorsementField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public System.DateTime TruncDtTm
        {
            get
            {
                return this.truncDtTmField;
            }
            set
            {
                this.truncDtTmField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TruncDtTmSpecified
        {
            get
            {
                return this.truncDtTmFieldSpecified;
            }
            set
            {
                this.truncDtTmFieldSpecified = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class RestrictedInstitutionSchemaName1Choice : EntityBase<RestrictedInstitutionSchemaName1Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string prtryField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Prtry
        {
            get
            {
                return this.prtryField;
            }
            set
            {
                this.prtryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class RestrictedIdentification1 : EntityBase<RestrictedIdentification1>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string idField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private RestrictedInstitutionSchemaName1Choice schmeNmField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public RestrictedInstitutionSchemaName1Choice SchmeNm
        {
            get
            {
                return this.schmeNmField;
            }
            set
            {
                this.schmeNmField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class FinancialInstitutionIdentification8 : EntityBase<FinancialInstitutionIdentification8>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private RestrictedIdentification1 othrField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public RestrictedIdentification1 Othr
        {
            get
            {
                return this.othrField;
            }
            set
            {
                this.othrField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class FinancialInstitution5 : EntityBase<FinancialInstitution5>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitutionIdentification8 finInstnIdField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public FinancialInstitutionIdentification8 FinInstnId
        {
            get
            {
                return this.finInstnIdField;
            }
            set
            {
                this.finInstnIdField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class AccountIdentificationIBAN : EntityBase<AccountIdentificationIBAN>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string itemField;

        [System.Xml.Serialization.XmlElementAttribute("IBAN", Order = 0)]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class CashAccount16 : EntityBase<CashAccount16>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private AccountIdentificationIBAN idField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AccountIdentificationIBAN Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class RestrictedPersonIdentificationSchemaName2Choice : EntityBase<RestrictedPersonIdentificationSchemaName2Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string prtryField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Prtry
        {
            get
            {
                return this.prtryField;
            }
            set
            {
                this.prtryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class RestrictedIdentification2 : EntityBase<RestrictedIdentification2>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string idField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private RestrictedPersonIdentificationSchemaName2Choice schmeNmField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public RestrictedPersonIdentificationSchemaName2Choice SchmeNm
        {
            get
            {
                return this.schmeNmField;
            }
            set
            {
                this.schmeNmField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PersonIdentification4 : EntityBase<PersonIdentification4>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private RestrictedIdentification2 othrField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public RestrictedIdentification2 Othr
        {
            get
            {
                return this.othrField;
            }
            set
            {
                this.othrField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PartyPrivate1 : EntityBase<PartyPrivate1>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PersonIdentification4 itemField;

        [System.Xml.Serialization.XmlElementAttribute("PrvtId", Order = 0)]
        public PersonIdentification4 Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PartyIdentification35 : EntityBase<PartyIdentification35>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string nmField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PartyPrivate1 idField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public PartyPrivate1 Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class AmendmentInformationDetails6 : EntityBase<AmendmentInformationDetails6>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string orgnlMndtIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PartyIdentification35 orgnlCdtrSchmeIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private CashAccount16 orgnlDbtrAcctField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitution5 orgnlDbtrAgtField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string OrgnlMndtId
        {
            get
            {
                return this.orgnlMndtIdField;
            }
            set
            {
                this.orgnlMndtIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public PartyIdentification35 OrgnlCdtrSchmeId
        {
            get
            {
                return this.orgnlCdtrSchmeIdField;
            }
            set
            {
                this.orgnlCdtrSchmeIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public CashAccount16 OrgnlDbtrAcct
        {
            get
            {
                return this.orgnlDbtrAcctField;
            }
            set
            {
                this.orgnlDbtrAcctField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public FinancialInstitution5 OrgnlDbtrAgt
        {
            get
            {
                return this.orgnlDbtrAgtField;
            }
            set
            {
                this.orgnlDbtrAgtField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class MandateRelatedInformation6 : EntityBase<MandateRelatedInformation6>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string mndtIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private System.DateTime dtOfSgntrField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool amdmntIndField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool amdmntIndFieldSpecified;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private AmendmentInformationDetails6 amdmntInfDtlsField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string elctrncSgntrField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string MndtId
        {
            get
            {
                return this.mndtIdField;
            }
            set
            {
                this.mndtIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 1)]
        public System.DateTime DtOfSgntr
        {
            get
            {
                return this.dtOfSgntrField;
            }
            set
            {
                this.dtOfSgntrField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public bool AmdmntInd
        {
            get
            {
                return this.amdmntIndField;
            }
            set
            {
                this.amdmntIndField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AmdmntIndSpecified
        {
            get
            {
                return this.amdmntIndFieldSpecified;
            }
            set
            {
                this.amdmntIndFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public AmendmentInformationDetails6 AmdmntInfDtls
        {
            get
            {
                return this.amdmntInfDtlsField;
            }
            set
            {
                this.amdmntInfDtlsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ElctrncSgntr
        {
            get
            {
                return this.elctrncSgntrField;
            }
            set
            {
                this.elctrncSgntrField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class CategoryPurpose1Choice : EntityBase<CategoryPurpose1Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string itemField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ItemChoiceType3 itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Prtry", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType3 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema = false)]
    public enum ItemChoiceType3
    {

        /// <remarks/>
        Cd,

        /// <remarks/>
        Prtry,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class LocalInstrument2Choice : EntityBase<LocalInstrument2Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string itemField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ItemChoiceType2 itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Prtry", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType2 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IncludeInSchema = false)]
    public enum ItemChoiceType2
    {

        /// <remarks/>
        Cd,

        /// <remarks/>
        Prtry,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class ServiceLevel9Choice : EntityBase<ServiceLevel9Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ServiceLevel3Code itemField;

        [System.Xml.Serialization.XmlElementAttribute("Cd", Order = 0)]
        public ServiceLevel3Code Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public enum ServiceLevel3Code
    {

        /// <remarks/>
        SEPA,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class PaymentTypeInformation22 : EntityBase<PaymentTypeInformation22>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ServiceLevel9Choice svcLvlField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private LocalInstrument2Choice lclInstrmField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private SequenceType1Code seqTpField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool seqTpFieldSpecified;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private CategoryPurpose1Choice ctgyPurpField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ServiceLevel9Choice SvcLvl
        {
            get
            {
                return this.svcLvlField;
            }
            set
            {
                this.svcLvlField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public LocalInstrument2Choice LclInstrm
        {
            get
            {
                return this.lclInstrmField;
            }
            set
            {
                this.lclInstrmField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public SequenceType1Code SeqTp
        {
            get
            {
                return this.seqTpField;
            }
            set
            {
                this.seqTpField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SeqTpSpecified
        {
            get
            {
                return this.seqTpFieldSpecified;
            }
            set
            {
                this.seqTpFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public CategoryPurpose1Choice CtgyPurp
        {
            get
            {
                return this.ctgyPurpField;
            }
            set
            {
                this.ctgyPurpField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public enum SequenceType1Code
    {

        /// <remarks/>
        FRST,

        /// <remarks/>
        RCUR,

        /// <remarks/>
        FNAL,

        /// <remarks/>
        OOFF,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class ClearingSystemIdentification3Choice : EntityBase<ClearingSystemIdentification3Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ClearingSystemIdentification itemField;

        [System.Xml.Serialization.XmlElementAttribute("Prtry", Order = 0)]
        public ClearingSystemIdentification Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public enum ClearingSystemIdentification
    {

        /// <remarks/>
        ACH,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class SettlementInformation13 : EntityBase<SettlementInformation13>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private SettlementMethod1Code sttlmMtdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ClearingSystemIdentification3Choice clrSysField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public SettlementMethod1Code SttlmMtd
        {
            get
            {
                return this.sttlmMtdField;
            }
            set
            {
                this.sttlmMtdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ClearingSystemIdentification3Choice ClrSys
        {
            get
            {
                return this.clrSysField;
            }
            set
            {
                this.clrSysField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public enum SettlementMethod1Code
    {

        /// <remarks/>
        CLRG,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class OriginalTransactionReference13 : EntityBase<OriginalTransactionReference13>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ActiveCurrencyAndAmount intrBkSttlmAmtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private System.DateTime intrBkSttlmDtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool intrBkSttlmDtFieldSpecified;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private System.DateTime reqdColltnDtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool reqdColltnDtFieldSpecified;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PartyIdentification34 cdtrSchmeIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private SettlementInformation13 sttlmInfField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PaymentTypeInformation22 pmtTpInfField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private MandateRelatedInformation6 mndtRltdInfField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ChequeDetails chequeTxField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private RemittanceInformation5 rmtInfField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PartyIdentification32 ultmtDbtrField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PartyIdentification33 dbtrField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private CashAccount17 dbtrAcctField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitution4 dbtrAgtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitution4 cdtrAgtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PartyIdentification33 cdtrField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private CashAccount17 cdtrAcctField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PartyIdentification32 ultmtCdtrField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ActiveCurrencyAndAmount IntrBkSttlmAmt
        {
            get
            {
                return this.intrBkSttlmAmtField;
            }
            set
            {
                this.intrBkSttlmAmtField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 1)]
        public System.DateTime IntrBkSttlmDt
        {
            get
            {
                return this.intrBkSttlmDtField;
            }
            set
            {
                this.intrBkSttlmDtField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IntrBkSttlmDtSpecified
        {
            get
            {
                return this.intrBkSttlmDtFieldSpecified;
            }
            set
            {
                this.intrBkSttlmDtFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 2)]
        public System.DateTime ReqdColltnDt
        {
            get
            {
                return this.reqdColltnDtField;
            }
            set
            {
                this.reqdColltnDtField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReqdColltnDtSpecified
        {
            get
            {
                return this.reqdColltnDtFieldSpecified;
            }
            set
            {
                this.reqdColltnDtFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public PartyIdentification34 CdtrSchmeId
        {
            get
            {
                return this.cdtrSchmeIdField;
            }
            set
            {
                this.cdtrSchmeIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public SettlementInformation13 SttlmInf
        {
            get
            {
                return this.sttlmInfField;
            }
            set
            {
                this.sttlmInfField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public PaymentTypeInformation22 PmtTpInf
        {
            get
            {
                return this.pmtTpInfField;
            }
            set
            {
                this.pmtTpInfField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public MandateRelatedInformation6 MndtRltdInf
        {
            get
            {
                return this.mndtRltdInfField;
            }
            set
            {
                this.mndtRltdInfField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public ChequeDetails ChequeTx
        {
            get
            {
                return this.chequeTxField;
            }
            set
            {
                this.chequeTxField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public RemittanceInformation5 RmtInf
        {
            get
            {
                return this.rmtInfField;
            }
            set
            {
                this.rmtInfField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public PartyIdentification32 UltmtDbtr
        {
            get
            {
                return this.ultmtDbtrField;
            }
            set
            {
                this.ultmtDbtrField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public PartyIdentification33 Dbtr
        {
            get
            {
                return this.dbtrField;
            }
            set
            {
                this.dbtrField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public CashAccount17 DbtrAcct
        {
            get
            {
                return this.dbtrAcctField;
            }
            set
            {
                this.dbtrAcctField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public FinancialInstitution4 DbtrAgt
        {
            get
            {
                return this.dbtrAgtField;
            }
            set
            {
                this.dbtrAgtField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public FinancialInstitution4 CdtrAgt
        {
            get
            {
                return this.cdtrAgtField;
            }
            set
            {
                this.cdtrAgtField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public PartyIdentification33 Cdtr
        {
            get
            {
                return this.cdtrField;
            }
            set
            {
                this.cdtrField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public CashAccount17 CdtrAcct
        {
            get
            {
                return this.cdtrAcctField;
            }
            set
            {
                this.cdtrAcctField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public PartyIdentification32 UltmtCdtr
        {
            get
            {
                return this.ultmtCdtrField;
            }
            set
            {
                this.ultmtCdtrField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class ActiveCurrencyAndAmount : EntityBase<ActiveCurrencyAndAmount>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ActiveCurrencyCode ccyField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string valueField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActiveCurrencyCode Ccy
        {
            get
            {
                return this.ccyField;
            }
            set
            {
                this.ccyField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public enum ActiveCurrencyCode
    {

        /// <remarks/>
        ETB,

        /// <remarks/>
        USD,

        /// <remarks/>
        EUR,

        /// <remarks/>
        GBP,

        /// <remarks/>
        JPY,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class PartyIdentification34 : EntityBase<PartyIdentification34>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private object itemField;

        [System.Xml.Serialization.XmlElementAttribute("Id", typeof(Party6Choice), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Nm", typeof(string), Order = 0)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public partial class ChargesInformation5 : EntityBase<ChargesInformation5>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ActiveCurrencyAndAmount amtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitution4 ptyField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ActiveCurrencyAndAmount Amt
        {
            get
            {
                return this.amtField;
            }
            set
            {
                this.amtField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public FinancialInstitution4 Pty
        {
            get
            {
                return this.ptyField;
            }
            set
            {
                this.ptyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class PaymentTransactionInformation26 : EntityBase<PaymentTransactionInformation26>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string stsIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string orgnlInstrIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string orgnlEndToEndIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string orgnlTxIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private TransactionIndividualStatus3Code txStsField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool txStsFieldSpecified;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private StatusReasonInformation8 stsRsnInfField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private ChargesInformation5 chrgsInfField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitution4 instgAgtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private FinancialInstitution4 instdAgtField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private OriginalTransactionReference13 orgnlTxRefField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string StsId
        {
            get
            {
                return this.stsIdField;
            }
            set
            {
                this.stsIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string OrgnlInstrId
        {
            get
            {
                return this.orgnlInstrIdField;
            }
            set
            {
                this.orgnlInstrIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string OrgnlEndToEndId
        {
            get
            {
                return this.orgnlEndToEndIdField;
            }
            set
            {
                this.orgnlEndToEndIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string OrgnlTxId
        {
            get
            {
                return this.orgnlTxIdField;
            }
            set
            {
                this.orgnlTxIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public TransactionIndividualStatus3Code TxSts
        {
            get
            {
                return this.txStsField;
            }
            set
            {
                this.txStsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TxStsSpecified
        {
            get
            {
                return this.txStsFieldSpecified;
            }
            set
            {
                this.txStsFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public StatusReasonInformation8 StsRsnInf
        {
            get
            {
                return this.stsRsnInfField;
            }
            set
            {
                this.stsRsnInfField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public ChargesInformation5 ChrgsInf
        {
            get
            {
                return this.chrgsInfField;
            }
            set
            {
                this.chrgsInfField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public FinancialInstitution4 InstgAgt
        {
            get
            {
                return this.instgAgtField;
            }
            set
            {
                this.instgAgtField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public FinancialInstitution4 InstdAgt
        {
            get
            {
                return this.instdAgtField;
            }
            set
            {
                this.instdAgtField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public OriginalTransactionReference13 OrgnlTxRef
        {
            get
            {
                return this.orgnlTxRefField;
            }
            set
            {
                this.orgnlTxRefField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public enum TransactionIndividualStatus3Code
    {

        /// <remarks/>
        RJCT,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class StatusReasonInformation8 : EntityBase<StatusReasonInformation8>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private PartyIdentification34 orgtrField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private StatusReason6Choice rsnField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public PartyIdentification34 Orgtr
        {
            get
            {
                return this.orgtrField;
            }
            set
            {
                this.orgtrField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public StatusReason6Choice Rsn
        {
            get
            {
                return this.rsnField;
            }
            set
            {
                this.rsnField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class StatusReason6Choice : EntityBase<StatusReason6Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string itemField;

        [System.Xml.Serialization.XmlElementAttribute("Cd", Order = 0)]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class OriginalGroupInformation20 : EntityBase<OriginalGroupInformation20>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string orgnlMsgIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string orgnlMsgNmIdField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private TransactionGroupStatus3Code grpStsField;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool grpStsFieldSpecified;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private StatusReasonInformation8 stsRsnInfField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string OrgnlMsgId
        {
            get
            {
                return this.orgnlMsgIdField;
            }
            set
            {
                this.orgnlMsgIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string OrgnlMsgNmId
        {
            get
            {
                return this.orgnlMsgNmIdField;
            }
            set
            {
                this.orgnlMsgNmIdField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public TransactionGroupStatus3Code GrpSts
        {
            get
            {
                return this.grpStsField;
            }
            set
            {
                this.grpStsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GrpStsSpecified
        {
            get
            {
                return this.grpStsFieldSpecified;
            }
            set
            {
                this.grpStsFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public StatusReasonInformation8 StsRsnInf
        {
            get
            {
                return this.stsRsnInfField;
            }
            set
            {
                this.stsRsnInfField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    public enum TransactionGroupStatus3Code
    {

        /// <remarks/>
        ACCP,

        /// <remarks/>
        ACSC,

        /// <remarks/>
        PART,

        /// <remarks/>
        RJCT,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class OrganisationIdentification5 : EntityBase<OrganisationIdentification5>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string bICOrBEIField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string BICOrBEI
        {
            get
            {
                return this.bICOrBEIField;
            }
            set
            {
                this.bICOrBEIField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pacs.002.001.03", IsNullable = true)]
    public partial class Party7Choice : EntityBase<Party7Choice>
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        private OrganisationIdentification5 itemField;

        [System.Xml.Serialization.XmlElementAttribute("OrgId", Order = 0)]
        public OrganisationIdentification5 Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }
}
