using System;
using System.IO;
using BRRSACryptography;

namespace TestCrypto
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: TestCrypto.exe <encryptfile|decryptfile|signfile> <source> <dest>");
                return;
            }

            string command = args[0].ToLower();
            string source = args[1];
            string dest = args[2];

            try
            {
                if (command == "encryptfile")
                {
                    Console.WriteLine("Encrypting and Signing {0} to {1}...", source, dest);
                    CryptographyHelper.EncryptFile(source, dest);
                    Console.WriteLine("Success.");
                }
                else if (command == "decryptfile")
                {
                    Console.WriteLine("Decrypting and Verifying {0} to {1}...", source, dest);
                    CryptographyHelper.DecryptFile(source, dest);
                    Console.WriteLine("Success.");
                }
                else if (command == "signfile")
                {
                    Console.WriteLine("Signing {0} to {1}...", source, dest);
                    CryptographyHelper.SignFilePgp(source, dest);
                    Console.WriteLine("Success.");
                }
                else
                {
                    Console.WriteLine("Unknown command: " + command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Error: " + ex.InnerException.Message);
                }
                Console.WriteLine("Stack: " + ex.StackTrace);
                Environment.Exit(1);
            }
        }
    }
}
