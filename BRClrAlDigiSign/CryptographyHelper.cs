using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
// Org.BouncyCastle.Asn1.Cms removed — was ambiguous with Asn1.Pkcs for ContentInfo (now fully-qualified)
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace BRRSACryptography
{
    public static class CryptographyHelper
    {
        private static bool _optimalAsymmetricEncryptionPadding = false;

        // Load keys and certificates dynamically from configuration
        private static string PublicKey { get { return LoadKeyFromConfig("PublicKeyPath"); } }
        private static string PrivateKey { get { return LoadKeyFromConfig("PrivateKeyPath"); } }
        private static string CertificatePath { get { return ConfigurationManager.AppSettings["CertificatePath"]; } }
        private static string CertificatePassword { get { return ConfigurationManager.AppSettings["CertificatePassword"]; } }
        private static bool UseCertificate { get { bool useCert; return bool.TryParse(ConfigurationManager.AppSettings["UseCertificate"], out useCert) && useCert; } }
        private static bool UsePgp { get { return !UseCertificate; } }
        private static bool Sign { get { bool sign; return bool.TryParse(ConfigurationManager.AppSettings["Sign"], out sign) && sign; } }
        public static bool EnableEncryption
        {
            get
            {
                bool encEnabled;
                return bool.TryParse(ConfigurationManager.AppSettings["EnableEncryption"], out encEnabled) && encEnabled;
            }
        }
        private static string SharedKey { get { return ConfigurationManager.AppSettings["SharedKey"]; } }

        static CryptographyHelper()
        {
            LogSigningConfiguration();
        }

        private static void LogSigningConfiguration()
        {
            bool signingEnabled = UseCertificate || (!string.IsNullOrEmpty(PublicKey) && !string.IsNullOrEmpty(PrivateKey));
            Console.WriteLine(string.Format("Signing Enabled: {0}", signingEnabled));

            if (signingEnabled)
            {
                Console.WriteLine("Signing Method: " + (UseCertificate ? "Certificates" : "Keys"));
                Console.WriteLine(UseCertificate
                    ? (File.Exists(CertificatePath) ? "Certificate found." : "Certificate not found.")
                    : (!string.IsNullOrEmpty(PublicKey) && !string.IsNullOrEmpty(PrivateKey) ? "Keys found." : "Keys not found."));
            }
            else
            {
                Console.WriteLine("Signing is disabled.");
            }
        }

        public static string Encrypt(string plainText)
        {
            if (UseCertificate && !string.IsNullOrEmpty(CertificatePath))
            {
                return EncryptWithCertificate(plainText);
            }

            int keySize = 0;
            string publicKeyXml = "";

            GetKeyFromEncryptionString(PublicKey, out keySize, out publicKeyXml);

            var encrypted = Encrypt(Encoding.UTF8.GetBytes(plainText), keySize, publicKeyXml);

            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string encryptedText)
        {
            if (UseCertificate && !string.IsNullOrEmpty(CertificatePath))
            {
                return DecryptWithCertificate(encryptedText);
            }

            int keySize = 0;
            string publicAndPrivateKeyXml = "";

            GetKeyFromEncryptionString(PrivateKey, out keySize, out publicAndPrivateKeyXml);

            var decrypted = Decrypt(Convert.FromBase64String(encryptedText), keySize, publicAndPrivateKeyXml);

            return Encoding.UTF8.GetString(decrypted);
        }

        public static byte[] ComputeHash(byte[] data)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(data);
            }
        }

        private static string EncryptWithCertificate(string plainText)
        {
            if (!File.Exists(CertificatePath))
                throw new FileNotFoundException("Certificate file not found.", CertificatePath);

            var cert = new X509Certificate2(CertificatePath, CertificatePassword);
            var rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;
            var data = Encoding.UTF8.GetBytes(plainText);
            var encrypted = rsa.Encrypt(data, false);
            return Convert.ToBase64String(encrypted);
        }

        private static string DecryptWithCertificate(string encryptedText)
        {
            if (!File.Exists(CertificatePath))
                throw new FileNotFoundException("Certificate file not found.", CertificatePath);

            var cert = new X509Certificate2(CertificatePath, CertificatePassword);
            var rsa = (RSACryptoServiceProvider)cert.PrivateKey;
            var data = Convert.FromBase64String(encryptedText);
            var decrypted = rsa.Decrypt(data, false);
            return Encoding.UTF8.GetString(decrypted);
        }

        public static void EncryptFile(string sourcePath, string destPath)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Source file not found.", sourcePath);

            // 1. If both are false, it's Scenario 1 (No Security)
            if (!Sign && !EnableEncryption)
            {
                if (sourcePath != destPath)
                    File.Copy(sourcePath, destPath, true);
                return;
            }

            // 2. OpenPGP (GPG) Branch
            if (UsePgp)
            {
                if (Sign && !EnableEncryption)
                {
                    SignFilePgp(sourcePath, destPath);
                }
                else
                {
                    EncryptFilePgp(sourcePath, destPath);
                }
                return;
            }

            // 3. Certificate (CMS) Branch
            if (UseCertificate && !string.IsNullOrEmpty(CertificatePath))
            {
                byte[] fileData = File.ReadAllBytes(sourcePath);
                var cert = new X509Certificate2(CertificatePath, CertificatePassword);

                if (Sign && !EnableEncryption)
                {
                    // Case 2: Sign-Only (CMS SignedData)
                    System.Security.Cryptography.Pkcs.ContentInfo content = new System.Security.Cryptography.Pkcs.ContentInfo(fileData);
                    SignedCms signedCms = new SignedCms(content);
                    CmsSigner signer = new CmsSigner(cert);
                    signedCms.ComputeSignature(signer);
                    File.WriteAllBytes(destPath, signedCms.Encode());
                }
                else if (!Sign && EnableEncryption)
                {
                    // Case 3: Encrypt-Only (CMS EnvelopedData)
                    System.Security.Cryptography.Pkcs.ContentInfo content = new System.Security.Cryptography.Pkcs.ContentInfo(fileData);
                    EnvelopedCms envelopedCms = new EnvelopedCms(content);
                    CmsRecipient recipient = new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, cert);
                    envelopedCms.Encrypt(recipient);
                    File.WriteAllBytes(destPath, envelopedCms.Encode());
                }
                else if (Sign && EnableEncryption)
                {
                    // Case 4: Sign-then-Encrypt (CMS Full Secure)
                    System.Security.Cryptography.Pkcs.ContentInfo content = new System.Security.Cryptography.Pkcs.ContentInfo(fileData);
                    SignedCms signedCms = new SignedCms(content);
                    CmsSigner signer = new CmsSigner(cert);
                    signedCms.ComputeSignature(signer);
                    byte[] signedData = signedCms.Encode();

                    System.Security.Cryptography.Pkcs.ContentInfo encContent = new System.Security.Cryptography.Pkcs.ContentInfo(signedData);
                    EnvelopedCms envelopedCms = new EnvelopedCms(encContent);
                    CmsRecipient recipient = new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, cert);
                    envelopedCms.Encrypt(recipient);
                    File.WriteAllBytes(destPath, envelopedCms.Encode());
                }
                return;
            }

            // 4. Default Fallback (Hybrid RSA+AES)
            if (EnableEncryption)
            {
                int keySize;
                string publicKeyXml;
                GetKeyFromEncryptionString(PublicKey, out keySize, out publicKeyXml);
                byte[] fileData = File.ReadAllBytes(sourcePath);
                using (var rsa = new RSACryptoServiceProvider(keySize))
                {
                    rsa.FromXmlString(publicKeyXml);
                    using (var aes = Aes.Create())
                    {
                        aes.KeySize = 256;
                        aes.GenerateKey();
                        aes.GenerateIV();

                        byte[] encryptedKey = rsa.Encrypt(aes.Key, false);
                        using (var ms = new MemoryStream())
                        {
                            using (var bw = new BinaryWriter(ms))
                            {
                                bw.Write(encryptedKey.Length);
                                bw.Write(encryptedKey);
                                bw.Write(aes.IV.Length);
                                bw.Write(aes.IV);

                                using (var encryptor = aes.CreateEncryptor())
                                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                                {
                                    cs.Write(fileData, 0, fileData.Length);
                                }
                            }
                            File.WriteAllBytes(destPath, ms.ToArray());
                        }
                    }
                }
            }
        }

        public static void DecryptFile(string sourcePath, string destPath)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Source file not found.", sourcePath);

            byte[] encryptedData = File.ReadAllBytes(sourcePath);

            if (UsePgp)
            {
                DecryptFilePgp(sourcePath, destPath);
                return;
            }

            try
            {
                // Try CMS Decryption first (Certificate)
                if (UseCertificate && !string.IsNullOrEmpty(CertificatePath))
                {
                    var cert = new X509Certificate2(CertificatePath, CertificatePassword);
                    System.Security.Cryptography.Pkcs.EnvelopedCms envelopedCms = new System.Security.Cryptography.Pkcs.EnvelopedCms();
                    envelopedCms.Decode(encryptedData);
                    envelopedCms.Decrypt(new X509Certificate2Collection(cert));
                    File.WriteAllBytes(destPath, envelopedCms.ContentInfo.Content);
                    return;
                }
            }
            catch
            {
                // If CMS fail, fall back to Manual Hybrid
            }

            // Manual Hybrid Decryption (RSA Keys + AES)
            int keySize;
            string privateKeyXml;
            GetKeyFromEncryptionString(PrivateKey, out keySize, out privateKeyXml);

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.FromXmlString(privateKeyXml);
                using (var msInput = new MemoryStream(encryptedData))
                using (var br = new BinaryReader(msInput))
                {
                    int keyLen = br.ReadInt32();
                    byte[] encryptedKey = br.ReadBytes(keyLen);
                    int ivLen = br.ReadInt32();
                    byte[] iv = br.ReadBytes(ivLen);

                    byte[] aesKey = rsa.Decrypt(encryptedKey, false);

                    using (var aes = Aes.Create())
                    {
                        aes.Key = aesKey;
                        aes.IV = iv;

                        using (var decryptor = aes.CreateDecryptor())
                        using (var msOutput = new MemoryStream())
                        {
                            using (var cs = new CryptoStream(msInput, decryptor, CryptoStreamMode.Read))
                            {
                                cs.CopyTo(msOutput);
                            }
                            File.WriteAllBytes(destPath, msOutput.ToArray());
                        }
                    }
                }
            }
        }

        private static void EncryptFilePgp(string sourcePath, string destPath)
        {
            int keySize;
            string publicKeyXml;
            GetKeyFromEncryptionString(PublicKey, out keySize, out publicKeyXml);

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.FromXmlString(publicKeyXml);
                var rsaParams = rsa.ExportParameters(false);
                var bcPubKey = DotNetUtilities.GetRsaPublicKey(rsaParams);
                var pgpPubKey = new PgpPublicKey(PublicKeyAlgorithmTag.RsaGeneral, bcPubKey, DateTime.UtcNow);

                PgpPrivateKey pgpPrivKey = null;
                if (Sign)
                {
                    try
                    {
                        int privKeySize;
                        string privateKeyXml;
                        GetKeyFromEncryptionString(PrivateKey, out privKeySize, out privateKeyXml);
                        using (var rsaPriv = new RSACryptoServiceProvider(privKeySize))
                        {
                            rsaPriv.FromXmlString(privateKeyXml);
                            var bcPrivKey = DotNetUtilities.GetRsaKeyPair(rsaPriv.ExportParameters(true)).Private;
                            pgpPrivKey = new PgpPrivateKey(bcPrivKey, pgpPubKey.KeyId);
                        }
                    }
                    catch { /* Signing is optional or might fail due to missing key */ }
                }

                using (Stream outputStream = File.Create(destPath))
                {
                    PgpEncryptedDataGenerator encGen = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Aes256, new SecureRandom());
                    encGen.AddMethod(pgpPubKey);

                    using (Stream encryptedOut = encGen.Open(outputStream, new byte[1 << 16]))
                    {
                        PgpCompressedDataGenerator comGen = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
                        using (Stream compressedOut = comGen.Open(encryptedOut))
                        {
                            PgpSignatureGenerator sigGen = null;
                            if (pgpPrivKey != null)
                            {
                                sigGen = new PgpSignatureGenerator(pgpPubKey.Algorithm, HashAlgorithmTag.Sha256);
                                sigGen.InitSign(PgpSignature.BinaryDocument, pgpPrivKey);
                                PgpSignatureSubpacketGenerator spGen = new PgpSignatureSubpacketGenerator();
                                sigGen.SetHashedSubpackets(spGen.Generate());
                                sigGen.GenerateOnePassVersion(false).Encode(compressedOut);
                            }

                            PgpLiteralDataGenerator lGen = new PgpLiteralDataGenerator();
                            FileInfo fileInfo = new FileInfo(sourcePath);
                            using (Stream literalOut = lGen.Open(compressedOut, PgpLiteralData.Binary, fileInfo.Name, fileInfo.Length, DateTime.UtcNow))
                            using (FileStream fs = File.OpenRead(sourcePath))
                            {
                                byte[] buf = new byte[1 << 16];
                                int len;
                                while ((len = fs.Read(buf, 0, buf.Length)) > 0)
                                {
                                    literalOut.Write(buf, 0, len);
                                    if (sigGen != null) sigGen.Update(buf, 0, len);
                                }
                            }

                            if (sigGen != null)
                            {
                                sigGen.Generate().Encode(compressedOut);
                            }
                        }
                    }
                }
            }
        }

        public static void SignFilePgp(string sourcePath, string destPath)
        {
            int keySize;
            string privateKeyXml;
            GetKeyFromEncryptionString(PrivateKey, out keySize, out privateKeyXml);

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.FromXmlString(privateKeyXml);
                var rsaParams = rsa.ExportParameters(true);
                var keyPair = DotNetUtilities.GetRsaKeyPair(rsaParams);
                var pgpPub = new PgpPublicKey(PublicKeyAlgorithmTag.RsaGeneral, keyPair.Public, DateTime.UtcNow);
                var pgpPriv = new PgpPrivateKey(keyPair.Private, pgpPub.KeyId);

                using (Stream outputStream = File.Create(destPath))
                {
                    PgpSignatureGenerator sigGen = new PgpSignatureGenerator(pgpPub.Algorithm, HashAlgorithmTag.Sha256);
                    sigGen.InitSign(PgpSignature.BinaryDocument, pgpPriv);
                    PgpSignatureSubpacketGenerator spGen = new PgpSignatureSubpacketGenerator();
                    sigGen.SetHashedSubpackets(spGen.Generate());
                    sigGen.GenerateOnePassVersion(false).Encode(outputStream);

                    PgpLiteralDataGenerator lGen = new PgpLiteralDataGenerator();
                    FileInfo fileInfo = new FileInfo(sourcePath);
                    using (Stream literalOut = lGen.Open(outputStream, PgpLiteralData.Binary, fileInfo.Name, fileInfo.Length, DateTime.UtcNow))
                    using (FileStream fs = File.OpenRead(sourcePath))
                    {
                        byte[] buf = new byte[1 << 16];
                        int len;
                        while ((len = fs.Read(buf, 0, buf.Length)) > 0)
                        {
                            literalOut.Write(buf, 0, len);
                            sigGen.Update(buf, 0, len);
                        }
                    }
                    sigGen.Generate().Encode(outputStream);
                }
            }
        }

        private static void DecryptFilePgp(string sourcePath, string destPath)
        {
            int keySize;
            string privateKeyXml;
            GetKeyFromEncryptionString(PrivateKey, out keySize, out privateKeyXml);

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.FromXmlString(privateKeyXml);
                var rsaParams = rsa.ExportParameters(true);
                var bcPrivKey = DotNetUtilities.GetRsaKeyPair(rsaParams).Private;

                using (Stream inputStream = File.OpenRead(sourcePath))
                {
                    PgpObjectFactory pgpFact = new PgpObjectFactory(PgpUtilities.GetDecoderStream(inputStream));
                    PgpObject pgpObj = pgpFact.NextPgpObject();

                    if (pgpObj is PgpEncryptedDataList)
                    {
                        PgpEncryptedDataList encList = (PgpEncryptedDataList)pgpObj;
                        PgpPublicKeyEncryptedData encData = null;
                        foreach (PgpPublicKeyEncryptedData pked in encList.GetEncryptedDataObjects())
                        {
                            encData = pked;
                            break;
                        }

                        if (encData != null)
                        {
                            using (Stream clearStream = encData.GetDataStream(new PgpPrivateKey(bcPrivKey, encData.KeyId)))
                            {
                                ProcessPgpMessage(clearStream, destPath);
                            }
                        }
                    }
                    else
                    {
                        // Not encrypted, could be Sign-Only. Rewind and process as plain PGP message.
                        inputStream.Position = 0;
                        ProcessPgpMessage(PgpUtilities.GetDecoderStream(inputStream), destPath);
                    }
                }
            }
        }

        private static void ProcessPgpMessage(Stream clearStream, string destPath)
        {
            PgpObjectFactory plainFact = new PgpObjectFactory(clearStream);
            PgpObject message = plainFact.NextPgpObject();

            PgpOnePassSignatureList onePassList = null;
            PgpSignatureList sigList = null;

            if (message is PgpCompressedData)
            {
                PgpCompressedData cData = (PgpCompressedData)message;
                plainFact = new PgpObjectFactory(cData.GetDataStream());
                message = plainFact.NextPgpObject();
            }

            if (message is PgpOnePassSignatureList)
            {
                onePassList = (PgpOnePassSignatureList)message;
                message = plainFact.NextPgpObject();
            }

            if (message is PgpLiteralData)
            {
                PgpLiteralData ld = (PgpLiteralData)message;
                PgpOnePassSignature ops = null;

                if (onePassList != null)
                {
                    try
                    {
                        int pubKeySize;
                        string publicKeyXml;
                        GetKeyFromEncryptionString(PublicKey, out pubKeySize, out publicKeyXml);
                        using (var rsaPub = new RSACryptoServiceProvider(pubKeySize))
                        {
                            rsaPub.FromXmlString(publicKeyXml);
                            var bcPubKey = DotNetUtilities.GetRsaPublicKey(rsaPub.ExportParameters(false));
                            var senderPubKey = new PgpPublicKey(PublicKeyAlgorithmTag.RsaGeneral, bcPubKey, DateTime.UtcNow);

                            ops = onePassList[0];
                            ops.InitVerify(senderPubKey);
                        }
                    }
                    catch { /* Verification key missing */ }
                }

                using (Stream ldStream = ld.GetInputStream())
                using (FileStream os = File.Create(destPath))
                {
                    byte[] buf = new byte[1 << 16];
                    int len;
                    while ((len = ldStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        os.Write(buf, 0, len);
                        if (ops != null) ops.Update(buf, 0, len);
                    }
                }

                if (ops != null)
                {
                    PgpObject next = plainFact.NextPgpObject();
                    if (next is PgpSignatureList)
                    {
                        sigList = (PgpSignatureList)next;
                        if (!ops.Verify(sigList[0]))
                        {
                            throw new Exception("PGP Signature verification failed!");
                        }
                    }
                }
            }
        }

        private static string LoadKeyFromConfig(string keyPathConfig)
        {
            var path = ConfigurationManager.AppSettings[keyPathConfig];
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                throw new FileNotFoundException(string.Format("Key file not found: {0}", path));

            return File.ReadAllText(path);
        }

        private static byte[] Encrypt(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            int maxLength = GetMaxDataLength(keySize);
            if (data.Length > maxLength) throw new ArgumentException(string.Format("Maximum data length is {0}", maxLength), "data");
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (string.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", "publicKeyXml");

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicKeyXml);
                return provider.Encrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        private static byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (string.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicAndPrivateKeyXml);
                return provider.Decrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        private static int GetMaxDataLength(int keySize)
        {
            return (keySize - 384) / 8 + 7;
        }

        private static bool IsKeySizeValid(int keySize)
        {
            return keySize >= 2048 && keySize % 1024 == 0;
        }

        public static string EncryptWithSharedKey(string plainText)
        {
            if (string.IsNullOrEmpty(SharedKey))
                throw new InvalidOperationException("Shared key is not configured.");

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(SharedKey);
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string DecryptWithSharedKey(string encryptedText)
        {
            if (string.IsNullOrEmpty(SharedKey))
                throw new InvalidOperationException("Shared key is not configured.");

            var data = Convert.FromBase64String(encryptedText);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(SharedKey);

                var iv = new byte[aes.BlockSize / 8];
                Array.Copy(data, iv, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(data, iv.Length, data.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static void GetKeyFromEncryptionString(string keyString, out int keySize, out string keyXml)
        {
            if (string.IsNullOrEmpty(keyString))
                throw new ArgumentException("Key string is null or empty.", "keyString");

            keySize = 2048; // Default

            // Detect PEM format
            if (keyString.Contains("-----BEGIN"))
            {
                try
                {
                    using (var reader = new StringReader(keyString))
                    {
                        var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader, new PasswordFinder(CertificatePassword));
                        object pemObject = pemReader.ReadObject();

                        AsymmetricKeyParameter keyParam = null;

                        if (pemObject is PemObject)
                        {
                            PemObject pem = (PemObject)pemObject;
                            if (pem.Type.Contains("ENCRYPTED PRIVATE KEY"))
                            {
                                EncryptedPrivateKeyInfo encInfo = EncryptedPrivateKeyInfo.GetInstance(pem.Content);
                                keyParam = PrivateKeyFactory.DecryptKey(new PasswordFinder(CertificatePassword).GetPassword(), encInfo);
                            }
                            else if (pem.Type.Contains("PRIVATE KEY"))
                            {
                                keyParam = PrivateKeyFactory.CreateKey(pem.Content);
                            }
                        }
                        else if (pemObject is AsymmetricKeyParameter)
                        {
                            keyParam = (AsymmetricKeyParameter)pemObject;
                        }
                        else if (pemObject is AsymmetricCipherKeyPair)
                        {
                            keyParam = ((AsymmetricCipherKeyPair)pemObject).Private;
                        }

                        if (keyParam != null)
                        {
                            RsaKeyParameters rsaKey = (RsaKeyParameters)keyParam;
                            RSAParameters rsaParams = new RSAParameters();
                            rsaParams.Modulus = rsaKey.Modulus.ToByteArrayUnsigned();

                            if (rsaKey.IsPrivate)
                            {
                                RsaPrivateCrtKeyParameters rck = (RsaPrivateCrtKeyParameters)rsaKey;
                                rsaParams.Exponent = rck.PublicExponent.ToByteArrayUnsigned();
                                rsaParams.P = rck.P.ToByteArrayUnsigned();
                                rsaParams.Q = rck.Q.ToByteArrayUnsigned();
                                rsaParams.DP = rck.DP.ToByteArrayUnsigned();
                                rsaParams.DQ = rck.DQ.ToByteArrayUnsigned();
                                rsaParams.InverseQ = rck.QInv.ToByteArrayUnsigned();
                                rsaParams.D = rck.Exponent.ToByteArrayUnsigned();
                            }
                            else
                            {
                                rsaParams.Exponent = rsaKey.Exponent.ToByteArrayUnsigned();
                            }

                            var cspParams = new CspParameters { ProviderType = 24 };
                            using (var rsa = new RSACryptoServiceProvider(cspParams))
                            {
                                rsa.ImportParameters(rsaParams);
                                keyXml = rsa.ToXmlString(rsaKey.IsPrivate);
                                keySize = rsa.KeySize;
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException("Failed to parse PEM key: " + ex.Message, ex);
                }
            }

            try
            {
                // Try to detect if it's the Base64(Size!Xml) format
                byte[] decodedBytes = Convert.FromBase64String(keyString);
                string decoded = Encoding.UTF8.GetString(decodedBytes);
                
                if (decoded.Contains("!"))
                {
                    string[] parts = decoded.Split('!');
                    if (parts.Length >= 2 && int.TryParse(parts[0], out keySize))
                    {
                        keyXml = string.Join("!", parts, 1, parts.Length - 1);
                        return;
                    }
                }
            }
            catch
            {
                // Fallback: If not Base64 or doesn't match 'Size!Xml', assume it's raw XML
            }

            keyXml = keyString;
        }

        public static byte[] SignData(byte[] data, Org.BouncyCastle.X509.X509Certificate certificate, AsymmetricKeyParameter privateKey)
        {
            var content = new CmsProcessableByteArray(data);
            var generator = new CmsSignedDataGenerator();

            generator.AddSigner(privateKey, certificate, CmsSignedGenerator.DigestSha256);
            var storeParams = new X509CollectionStoreParameters(new[] { certificate });
            var certStore = X509StoreFactory.Create("Certificate/Collection", storeParams);
            generator.AddCertificates(certStore);

            var signedData = generator.Generate(content, true);
            return signedData.GetEncoded();
        }

        public static bool VerifySignature(byte[] signedData, Org.BouncyCastle.X509.X509Certificate certificate)
        {
            var cmsSignedData = new CmsSignedData(signedData);
            var signers = cmsSignedData.GetSignerInfos().GetSigners();

            foreach (SignerInformation signer in signers)
            {
                var certs = cmsSignedData.GetCertificates("Collection");
                var certCollection = certs.GetMatches(signer.SignerID);

                foreach (Org.BouncyCastle.X509.X509Certificate cert in certCollection)
                {
                    if (signer.Verify(cert.GetPublicKey()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private class PasswordFinder : IPasswordFinder
        {
            private readonly string _password;

            public PasswordFinder(string password)
            {
                _password = password;
            }

            public char[] GetPassword()
            {
                return _password != null ? _password.ToCharArray() : new char[0];
            }
        }

        private static byte[] Normalize(byte[] data, int expectedLength)
        {
            if (data == null) return null;
            if (data.Length == expectedLength) return data;

            if (data.Length == expectedLength + 1 && data[0] == 0)
            {
                byte[] result = new byte[expectedLength];
                Array.Copy(data, 1, result, 0, expectedLength);
                return result;
            }

            if (data.Length < expectedLength)
            {
                byte[] result = new byte[expectedLength];
                Array.Copy(data, 0, result, expectedLength - data.Length, data.Length);
                return result;
            }

            return data;
        }

    }
}
