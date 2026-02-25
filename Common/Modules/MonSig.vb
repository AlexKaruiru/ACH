Imports System.Collections
Imports System.Security.Cryptography.X509Certificates
Imports System.Windows.Forms
Imports Org.BouncyCastle.Cms
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Generators
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.X509.Store

Namespace ACH_Files
    Public Class MonSig
        Private _cUser As String = String.Empty
        Private _cert As Org.BouncyCastle.X509.X509Certificate
        Private _key As AsymmetricCipherKeyPair

        Public Sub New(certUser As String)
            _cUser = certUser
            '_cUser = "MWCBTZT0"
            GetCertificate()
        End Sub

        Private Sub GetCertificate()
            Dim st As X509Store = Nothing
            Try
                st = New X509Store(StoreName.My, StoreLocation.CurrentUser)
                st.Open(OpenFlags.[ReadOnly])
                Dim col As X509Certificate2Collection = st.Certificates.Find(X509FindType.FindBySubjectName, _cUser, False)
                If col.Count > 0 Then
                    MessageBox.Show("GetCertificate: Imeingia hapa")
                    Dim thisCert As X509Certificate2 = col(0)
                    _cert = DotNetUtilities.FromX509Certificate(thisCert)
                    _key = GenerateKeys(thisCert.PrivateKey.KeySize)
                End If
            Catch
            Finally
                st.Close()
            End Try
        End Sub

        Public Function GenerateKeys(keySizeInBits As Integer) As AsymmetricCipherKeyPair
            Dim r As New RsaKeyPairGenerator()
            r.Init(New KeyGenerationParameters(New SecureRandom(), keySizeInBits))
            Dim keys As AsymmetricCipherKeyPair = r.GenerateKeyPair()
            Return keys
        End Function

        Public Function SignFile(dataBytes As Byte()) As Byte()
            Dim certList As IList = New ArrayList()
            Dim crlList As IList = New ArrayList()
            Dim msg As CmsProcessable = New CmsProcessableByteArray(dataBytes)
            certList.Add(_cert)
            Dim x509Certs As IX509Store = X509StoreFactory.Create("Certificate/Collection", New X509CollectionStoreParameters(certList))
            Dim gen As New CmsSignedDataGenerator()
            gen.AddSigner(_key.[Private], _cert, CmsSignedDataGenerator.DigestSha1)
            gen.AddCertificates(x509Certs)
            Dim signedData As CmsSignedData = gen.Generate(msg, True)
            Return signedData.GetEncoded()
        End Function

        Public Function ReadSigned(dataBytes As Byte()) As Byte()
            Try
                Dim signed As New CmsSignedData(dataBytes)
                Dim st As IX509Store = signed.GetCertificates("Collection/BC")
                Dim infoStore As SignerInformationStore = signed.GetSignerInfos()
                If infoStore Is Nothing Then
                    Return Nothing
                End If
                Dim c As ICollection = infoStore.GetSigners()
                Dim it As IEnumerator = c.GetEnumerator()
                it.MoveNext()
                Dim signer As SignerInformation = DirectCast(it.Current, SignerInformation)
                Dim coll As ICollection = st.GetMatches(signer.SignerID)
                it = coll.GetEnumerator()
                it.MoveNext()
                Dim cert As Org.BouncyCastle.X509.X509Certificate = DirectCast(it.Current, Org.BouncyCastle.X509.X509Certificate)
                If cert Is Nothing Then
                    Return Nothing
                End If
                If signer.Verify(cert) Then
                    Return DirectCast(signed.SignedContent.GetContent(), Byte())
                End If
                Return dataBytes
            Catch
                Return dataBytes
            End Try
        End Function
    End Class
End Namespace

