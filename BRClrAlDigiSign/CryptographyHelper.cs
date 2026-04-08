using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
            using (var rsa = cert.GetRSAPublicKey())
            {
                var data = Encoding.UTF8.GetBytes(plainText);
                var encrypted = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encrypted);
            }
        }

        private static string DecryptWithCertificate(string encryptedText)
        {
            if (!File.Exists(CertificatePath))
                throw new FileNotFoundException("Certificate file not found.", CertificatePath);

            var cert = new X509Certificate2(CertificatePath, CertificatePassword);
            using (var rsa = cert.GetRSAPrivateKey())
            {
                var data = Convert.FromBase64String(encryptedText);
                var decrypted = rsa.Decrypt(data, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decrypted);
            }
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
    }
}
