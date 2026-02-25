'Imports System.Collections
'Imports Org.BouncyCastle.Cms
'Imports Org.BouncyCastle.Crypto
'Imports Org.BouncyCastle.Security
'Imports Org.BouncyCastle.X509.Store
'Imports X509 = Org.BouncyCastle.X509
'Imports Org.BouncyCastle.Crypto.Generators
'Imports System.Security.Cryptography.X509Certificates

'Public Class Montran
'    Private cUser As String = ""
'    Private Cert As X509.X509Certificate = Nothing
'    Private Key As AsymmetricCipherKeyPair = Nothing

'    Public Sub New(ByVal CertUser As String)
'        cUser = CertUser
'        GetCertificate()
'    End Sub

'    Private Sub GetCertificate()
'        Dim st As X509Store = Nothing
'        Try
'            st = New X509Store(StoreName.My, StoreLocation.CurrentUser)
'            st.Open(OpenFlags.[ReadOnly])
'            Dim col As X509Certificate2Collection = st.Certificates.Find(X509FindType.FindBySubjectName, cUser, True)
'            If col.Count > 0 Then
'                Dim thisCert As X509Certificate2 = col(0)
'                Cert = DotNetUtilities.FromX509Certificate(thisCert)
'                Key = GenerateKeys(thisCert.PrivateKey.KeySize)
'            End If
'        Catch
'        Finally
'            st.Close()
'        End Try
'    End Sub

'    Public Function GenerateKeys(ByVal keySizeInBits As Integer) As AsymmetricCipherKeyPair
'        Dim r As New RsaKeyPairGenerator()
'        r.Init(New KeyGenerationParameters(New SecureRandom(), keySizeInBits))
'        Dim keys As AsymmetricCipherKeyPair = r.GenerateKeyPair()
'        Return keys
'    End Function

'    Public Function SignFile(ByVal DataBytes As Byte()) As Byte()
'        Dim certList As IList = New ArrayList()
'        Dim crlList As IList = New ArrayList()
'        Dim msg As CmsProcessable = New CmsProcessableByteArray(DataBytes)
'        certList.Add(Cert)
'        Dim x509Certs As IX509Store = X509StoreFactory.Create("Certificate/Collection", New X509CollectionStoreParameters(certList))
'        Dim gen As New CmsSignedDataGenerator()
'        gen.AddSigner(Key.[Private], Cert, CmsSignedDataGenerator.DigestSha1)
'        gen.AddCertificates(x509Certs)
'        Dim signedData As CmsSignedData = gen.Generate(msg, True)
'        Return signedData.GetEncoded()
'    End Function

'    Public Function ReadSigned(ByVal DataBytes As Byte()) As Byte()
'        Try
'            Dim signed As New CmsSignedData(DataBytes)
'            Dim st As IX509Store = signed.GetCertificates("Collection/BC")
'            Dim infoStore As SignerInformationStore = signed.GetSignerInfos()
'            If infoStore Is Nothing Then Return Nothing
'            Dim c As ICollection = infoStore.GetSigners()
'            Dim it As IEnumerator = c.GetEnumerator()
'            it.MoveNext()
'            Dim signer As SignerInformation = DirectCast(it.Current, SignerInformation)
'            Dim coll As ICollection = st.GetMatches(signer.SignerID)
'            it = coll.GetEnumerator()
'            it.MoveNext()
'            Dim cert As X509.X509Certificate = DirectCast(it.Current, X509.X509Certificate)
'            If cert Is Nothing Then Return Nothing
'            If signer.Verify(cert) Then
'                Return DirectCast(signed.SignedContent.GetContent(), Byte())
'            Else
'                Return Nothing
'            End If
'        Catch
'            Return DataBytes
'        End Try
'    End Function
'End Class