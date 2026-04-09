using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;

using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
using System.Security.Cryptography.X509Certificates;

namespace BRRSACryptography
{
    public static class CryptographyHelper
    {
        private static bool _optimalAsymmetricEncryptionPadding = false;

        // Load keys and certificates dynamically from configuration
        private static string PublicKey => LoadKeyFromConfig("PublicKeyPath");
        private static string PrivateKey => LoadKeyFromConfig("PrivateKeyPath");
        private static string CertificatePath => ConfigurationManager.AppSettings["CertificatePath"];
        private static string CertificatePassword => ConfigurationManager.AppSettings["CertificatePassword"];
        private static bool UseCertificate => bool.TryParse(ConfigurationManager.AppSettings["UseCertificate"], out var useCert) && useCert;
        private static string SharedKey => ConfigurationManager.AppSettings["SharedKey"];

        static CryptographyHelper()
        {
            LogSigningConfiguration();
        }

        private static void LogSigningConfiguration()
        {
            bool signingEnabled = UseCertificate || (!string.IsNullOrEmpty(PublicKey) && !string.IsNullOrEmpty(PrivateKey));
            Console.WriteLine($"Signing Enabled: {signingEnabled}");

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

        private static string LoadKeyFromConfig(string keyPathConfig)
        {
            var path = ConfigurationManager.AppSettings[keyPathConfig];
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                throw new FileNotFoundException($"Key file not found: {path}");

            return File.ReadAllText(path);
        }

        private static byte[] Encrypt(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", nameof(data));
            int maxLength = GetMaxDataLength(keySize);
            if (data.Length > maxLength) throw new ArgumentException($"Maximum data length is {maxLength}", nameof(data));
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", nameof(keySize));
            if (string.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", nameof(publicKeyXml));

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicKeyXml);
                return provider.Encrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        private static byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", nameof(data));
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", nameof(keySize));
            if (string.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", nameof(publicAndPrivateKeyXml));

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
                throw new ArgumentException("Key string is null or empty.", nameof(keyString));

            // Example: Parse the key string to extract key size and XML
            // This is a placeholder implementation. Replace with your actual logic.
            keySize = 2048; // Default key size
            keyXml = keyString; // Assuming the keyString is the XML representation of the key
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
    }
}
