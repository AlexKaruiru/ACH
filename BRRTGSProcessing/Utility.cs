using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace BRRTGSProcessing
{
    internal class Utility
    {
        public static void WriteFile(string path, string content)
        {
            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (var sw = new StreamWriter(fs))
                    sw.Write(content);
            }
        }

        public static byte[] SignFiles(byte[] data, string cert)
        {
            var sign = Convert.ToBoolean(ConfigurationManager.AppSettings["Sign"]);
            if (sign)
            {
                var signature = new TokenSignature(cert);
                return signature.SignFile(data);
            }
            else
            {
                return data;
            }
        }

        public static byte[] RemoveSign(byte[] data, string cert)
        {
            var signature = new TokenSignature(cert);
            return signature.ReadSigned(data);
        }

        public static void ZipFile(string sFile, IEnumerable<string> di)
        {
            var z = new ZipFile(sFile);
            z.AddFiles(di, "");
            z.Save();
        }

        public static FileInfo[] UnZipFile(string temp, string file, out string sDir)
        {
            try
            {
                sDir = Path.Combine(temp, Path.GetFileNameWithoutExtension(file));
                if (!Directory.Exists(sDir))
                    Directory.CreateDirectory(sDir);
                var zArchive = new ZipFile(file);
                zArchive.ExtractAll(sDir, ExtractExistingFileAction.OverwriteSilently);
                var di = new DirectoryInfo(sDir);
                var fi = di.GetFiles();
                zArchive.Dispose();
                return fi;
            }
            catch (ZipException)
            {
                sDir = "";
                return new FileInfo[0];
            }
        }
    }
}
