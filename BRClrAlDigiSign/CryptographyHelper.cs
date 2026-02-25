using System;
using System.Security.Cryptography;
using System.Text;

namespace BRRSACryptography
{
   public static class CryptographyHelper
    {
       private static bool _optimalAsymmetricEncryptionPadding = false;

       //These keys are of 2048byte
       private readonly static string PublicKey = "MjA0OCE8UlNBS2V5VmFsdWU+PE1vZHVsdXM+MGFOK1E0SVg0SThqbVZlOVkxVmhvSkJzbkFEa2QwM0tSN0k4NUUxS3grNEgzUGZ2QXFyQWdNaXdoNVhhaTAya0o4a2MrOUQxYUNpMFE2SzBjS3BUYTkvamFQWU9XWkRkMUEvS0NoZFFyTEYwZmhUSUErUWM4ZUNDVGFSeXZEWGRUWEtuL2VKb0hINHBlWGFMTGpaQ3Nld2NYSnFGcVZoRU1lL3M4dVFZb0k4NmxCTlV0aXppZlF2OHUzcU1QOGtBN1NMbFJOOFAwdWxPdTk3UXpET1V2SUdhbjZOMTNOcGpEVWxYa21paWZ2Z0dSczkwM0NtM3RVMEt2dDVIaVMyejR3NTcrSHYrUWxqUnRqa1F4ZDJmVmdmSXRBcDcvY3hqS0xQMlQ5RGRiY1dETXZuVEhyYW5YREhFUEJ2bmdoWjN3K2w0Mlk5WmgrVVJVNXVhejc1TXJRPT08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU+";
       private readonly static string PrivateKey = "MjA0OCE8UlNBS2V5VmFsdWU+PE1vZHVsdXM+MGFOK1E0SVg0SThqbVZlOVkxVmhvSkJzbkFEa2QwM0tSN0k4NUUxS3grNEgzUGZ2QXFyQWdNaXdoNVhhaTAya0o4a2MrOUQxYUNpMFE2SzBjS3BUYTkvamFQWU9XWkRkMUEvS0NoZFFyTEYwZmhUSUErUWM4ZUNDVGFSeXZEWGRUWEtuL2VKb0hINHBlWGFMTGpaQ3Nld2NYSnFGcVZoRU1lL3M4dVFZb0k4NmxCTlV0aXppZlF2OHUzcU1QOGtBN1NMbFJOOFAwdWxPdTk3UXpET1V2SUdhbjZOMTNOcGpEVWxYa21paWZ2Z0dSczkwM0NtM3RVMEt2dDVIaVMyejR3NTcrSHYrUWxqUnRqa1F4ZDJmVmdmSXRBcDcvY3hqS0xQMlQ5RGRiY1dETXZuVEhyYW5YREhFUEJ2bmdoWjN3K2w0Mlk5WmgrVVJVNXVhejc1TXJRPT08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPjdRUjVqaUdhcVMrL0dQK0ZCVUJVZUsrSUsyV3BOK0NCbTFtTkR2ZFFLZ2hXcGJqNktBNzcrWEk4Zk1IaHRqMW1HUzh6b3BCdDZrWUMzRjFUMjcwNjVOMVhFU3RCMjlMWWFMUi9IYXJsSTBhRE82N0E0d0wzdktYQXo4SXlZVWVma28vNnVrd21xVk9BVndXZlRoUHFLTWFvdjJnVzNadC9mL0txSlpPY0tJOD08L1A+PFE+NG0yczNiWGN6NjVqaGpodkFPQlZPcU9XRVJVcTZpMTU2ako4YTI4LzM5dDN2QjhGUDZ0RDZNRFZwdUw5YlN4VkZQbDd4TENsVCtzS1FSTFY2QkJvS1ZHeGJyb2FhaTk5SUhZVDArdkxYN0pUOXdYYkFsMGloWDZ5NjJlVDZLbkg2TGdSWjhVaGc5REpmMUt5UEtrVnhMditYZ3lneDJrKzNISkJFMDhrZlFNPTwvUT48RFA+YmlyTVRWSTFUenFRT3BCYVJneVd0dG53RXl2WHNjODZydmk2NWgveU5QZFhQd1J4MGpqVXlpZGRFaWdVUDk3ZUhBcWlsNGRHYjhSSEJDS2xIZGdoRGN3TDhrRUhCYVJGVHJYRytBVTlLRjVZRlYrdTdGV3V6UkxHUWt0am0yL3RMZlNhZUFsNVZ1OG5Fa3ZjdGkvdmdUQ0ZzeUl3cnFUZWFKeGtsYjY4c09VPTwvRFA+PERRPm84MjU5KzZmM25LSE4wdE9LbkVmMXpzT3ppN2xDK0o0UncvYzZJL2p3cHR2cEYxQW04M1hZK05VN05WYmh3WkNTdGt0UzU2TENTUzlGMVI4QU5VVnRZYU8rTTZUVzJ3ZGRrVlZsTU1KTHFMQWtYemY4bmJVQ1lVZUJLUzY0TjhxYXRYMmFYNjMvemNrL2dJaVJUWUJOSURYeE13WDZBcmdXZlFoMWNrMFdXYz08L0RRPjxJbnZlcnNlUT5ENkR4bWE5bGtEZExtZllNYlVXOEpUcTA2RE9CNEZ3M3dtQjh6NFZveHRaRGVTdlZzcXpCZ1ozSUdBVUVnMVNrb3JYWDZoL0VBNnN0Mnh3cUtyMVRVYXJTNG9TTTM2SG9WZUZNaGlQT0FLQ21Cd0tFc0xWekgyNGJSYnZkSnpjbHFGZjAwdlh2cGR1cXZQRmJUbk5uWGYwcGpySjBNbjU5Vm5DdW1pUndEYU09PC9JbnZlcnNlUT48RD5EL1BMK09FZVpnVWVaVjNLMkpNM3Nac3IySmNzeTNOTThEVzg4blo3T1VtQXFhem9NQ0RFTTBpU1BrOEY2VkhJS3dyZnZMM0MzdUV5RG0rRUlZcWtmSEgyQVJld01kcVhOcnJZMmZtNk50bnVEbVlRYmxOZWJIMTBET3AxbktMUXEwTU5TNEYrQUtVTkRPUWJaVEhWMDJvdklsRFQyUFlSZ21kS2RaVXIxcFBNVWpHUDZ0cU5KZjJHRXlzYWE2WGJKdHd4NStYNi8rQkVUZktDOTRzQlZzb1VpUVNQMnN3OC85M3hncUxTMjgvY2RqWmtNcWY4ZHlMU01Rb0ZlRER0dVF3M2lWRUJtVVZtc05HOFQ3KzBCOWRab3ExTW9zVmFROU9MYkxLalZIUm5UOVkwZ2R3bFNybklYV3dac1Jtcy9aaEtFaHRqN2hMMDg3RFNVdEZlZFE9PTwvRD48L1JTQUtleVZhbHVlPg==";

       public static string Encrypt(string plainText)
       {
           int keySize = 0;
           string publicKeyXml = "";

           GetKeyFromEncryptionString(PublicKey, out keySize, out publicKeyXml);

           var encrypted = Encrypt(Encoding.UTF8.GetBytes(plainText), keySize, publicKeyXml);
           
           return Convert.ToBase64String(encrypted);
       }

       private static byte[] Encrypt(byte[] data, int keySize, string publicKeyXml)
       {
           if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
           int maxLength = GetMaxDataLength(keySize);
           if (data.Length > maxLength) throw new ArgumentException(String.Format("Maximum data length is {0}", maxLength), "data");
           if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
           if (String.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", "publicKeyXml");

           using (var provider = new RSACryptoServiceProvider(keySize))
           {
               provider.FromXmlString(publicKeyXml);
               return provider.Encrypt(data, _optimalAsymmetricEncryptionPadding);
           }
       }

       public static string Decrypt(string encryptedText)
       {
           int keySize = 0;
           string publicAndPrivateKeyXml = "";

           GetKeyFromEncryptionString(PrivateKey, out keySize, out publicAndPrivateKeyXml);

           var decrypted = Decrypt(Convert.FromBase64String(encryptedText), keySize, publicAndPrivateKeyXml);
       
           return Encoding.UTF8.GetString(decrypted);
       }

       private static byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
       {
           if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
           if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
           if (String.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");

           using (var provider = new RSACryptoServiceProvider(keySize))
           {
               provider.FromXmlString(publicAndPrivateKeyXml);
               return provider.Decrypt(data, _optimalAsymmetricEncryptionPadding);
           }
       }

       private static int GetMaxDataLength(int keySize)
       {
           if (_optimalAsymmetricEncryptionPadding)
           {
               return ((keySize - 384) / 8) + 7;
           }
           return ((keySize - 384) / 8) + 37;
       }

       private static bool IsKeySizeValid(int keySize)
       {
           return keySize >= 384 && keySize <= 16384 && keySize % 8 == 0;
       }

       private static void GetKeyFromEncryptionString(string rawkey, out int keySize, out string xmlKey)
       {
           keySize = 0;
           xmlKey = "";

           if (rawkey != null && rawkey.Length > 0)
           {
               byte[] keyBytes = Convert.FromBase64String(rawkey);
               var stringKey = Encoding.UTF8.GetString(keyBytes);

               if (stringKey.Contains("!"))
               {
                   var splittedValues = stringKey.Split(new char[] { '!' }, 2);

                   try
                   {
                       keySize = int.Parse(splittedValues[0]);
                       xmlKey = splittedValues[1];
                   }
                   catch (Exception e) { }
               }
           }
       }
    }
}
