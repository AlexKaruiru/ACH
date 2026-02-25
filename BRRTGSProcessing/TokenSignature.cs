using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BRRTGSProcessing
{
    public class TokenSignature
    {
        private string _cUser = string.Empty;
        private Org.BouncyCastle.X509.X509Certificate _cert;
        private AsymmetricCipherKeyPair _key;

        public TokenSignature(string certUser)
        {
            _cUser = certUser;
            GetCertificate();
        }

        private void GetCertificate()
        {
            X509Store st = null;
            try
            {
                st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                st.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = st.Certificates.Find(X509FindType.FindBySubjectName, _cUser, true);
                if (col.Count > 0)
                {
                    X509Certificate2 thisCert = col[0];
                    _cert = DotNetUtilities.FromX509Certificate(thisCert);
                    _key = GenerateKeys(thisCert.PrivateKey.KeySize);
                }
            }
            catch { }
            finally
            {
                st.Close();
            }
        }

        public AsymmetricCipherKeyPair GenerateKeys(int keySizeInBits)
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), keySizeInBits));
            AsymmetricCipherKeyPair keys = r.GenerateKeyPair();
            return keys;
        }

        public byte[] SignFile(byte[] dataBytes)
        {
            IList certList = new ArrayList();
            IList crlList = new ArrayList();
            CmsProcessable msg = new CmsProcessableByteArray(dataBytes);
            certList.Add(_cert);
            IX509Store x509Certs = X509StoreFactory.Create("Certificate/Collection", new X509CollectionStoreParameters(certList));
            CmsSignedDataGenerator gen = new CmsSignedDataGenerator();
            gen.AddSigner(_key.Private, _cert, CmsSignedDataGenerator.DigestSha256);
            gen.AddCertificates(x509Certs);
            CmsSignedData signedData = gen.Generate(msg, true);
            return signedData.GetEncoded();
        }

        public byte[] ReadSigned(byte[] dataBytes)
        {
            try
            {
                CmsSignedData signed = new CmsSignedData(dataBytes);
                IX509Store st = signed.GetCertificates("Collection/BC");
                SignerInformationStore infoStore = signed.GetSignerInfos();
                if (infoStore == null)
                    return null;
                ICollection c = infoStore.GetSigners();
                IEnumerator it = c.GetEnumerator();
                it.MoveNext();
                SignerInformation signer = (SignerInformation)it.Current;
                ICollection coll = st.GetMatches(signer.SignerID);
                it = coll.GetEnumerator();
                it.MoveNext();
                Org.BouncyCastle.X509.X509Certificate cert = (Org.BouncyCastle.X509.X509Certificate)it.Current;
                if (cert == null)
                    return null;
                if (signer.Verify(cert))
                    return (byte[])signed.SignedContent.GetContent();
                return dataBytes;
            }
            catch
            {
                return dataBytes;
            }
        }
    }
}
