using System;
using System.IO;
using System.Reflection;

namespace Server.Util
{
    /// <summary>
    /// Util class meant for generally usefull static methods.
    /// </summary>
    public static class Toolkit
    {
        private static string _ApplicationPath = null;

        public static String ApplicationPath
        {
            get
            {
                if (_ApplicationPath == null)
                    _ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return _ApplicationPath;
            }
        }

        public static String ReadFile(String filepath)
        {
            if (String.IsNullOrWhiteSpace(filepath))
                return String.Empty;

            String fileContent = string.Empty;
            String fileUsed = filepath;

            if (filepath.StartsWith("~/"))
                filepath = filepath.Replace("~/", ApplicationPath);

            // Return the file or an empty string.
            if (File.Exists(fileUsed))
            {
                using (FileStream fs = new FileStream(fileUsed, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        fileContent = sr.ReadToEnd();
                    }
                }
            }

            return fileContent;
        }
    }
}