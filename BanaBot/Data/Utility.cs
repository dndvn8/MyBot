using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Serialization;

namespace BanaBot.Data
{
    internal class Utility
    {
        public static void ClearDirectory(string directory)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(directory);
                foreach (FileInfo info2 in info.GetFiles())
                {
                    info2.Attributes = FileAttributes.Normal;
                    info2.Delete();
                }
                foreach (DirectoryInfo info3 in info.GetDirectories())
                {
                    info3.Attributes = FileAttributes.Normal;
                    ClearDirectory(info3.FullName);
                    info3.Delete();
                }
            }
            catch
            {
            }
        }

        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            Directory.CreateDirectory(targetPath);
            foreach (string str in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(str.Replace(sourcePath, targetPath));
            }
            foreach (string str2 in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(str2, str2.Replace(sourcePath, targetPath), true);
            }
        }

        public static void CreateFileFromResource(string path, string resource, bool overwrite = false)
        {
            if (overwrite || !File.Exists(path))
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
        }

        public static string GetMultiLanguageText(string key)
        {
            return Application.Current.FindResource(key).ToString();
        }

        public static string MakeValidFileName(string name)
        {
            string str = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string pattern = string.Format(@"([{0}]*\.+$)|([{0}]+)", str);
            return Regex.Replace(name, pattern, "_");
        }

        public static void MapClassToXmlFile(Type type, object obj, string path)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                serializer.Serialize((TextWriter)writer, obj);
            }
        }

        public static object MapXmlFileToClass(Type type, string path)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                return serializer.Deserialize(reader);
            }
        }

        public static string Md5Checksum(string filePath)
        {
            string str;
            try
            {
                using (MD5 md = MD5.Create())
                {
                    using (FileStream stream = File.OpenRead(filePath))
                    {
                        str = BitConverter.ToString(md.ComputeHash(stream)).Replace("-", "").ToLower();
                    }
                }
            }
            catch (Exception)
            {
                str = "-1";
            }
            return str;
        }

        public static string Md5Hash(string s)
        {
            StringBuilder builder = new StringBuilder();
            byte[] buffer = MD5.Create().ComputeHash(Encoding.Default.GetBytes(s));
            foreach (byte num in buffer)
            {
                builder.Append(num.ToString("x2"));
            }
            return builder.ToString();
        }

        public static bool OverwriteFile(string file, string path)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(path);
                if ((directoryName != null) && !Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                try
                {
                    File.Move(file, path);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                    throw;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ReadResourceString(string resource)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }

        public static bool RenameFileIfExists(string file, string path)
        {
            try
            {
                int num = 1;
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                string extension = Path.GetExtension(file);
                string str3 = path;
                string directoryName = Path.GetDirectoryName(path);
                if (directoryName != null)
                {
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    while (File.Exists(str3))
                    {
                        str3 = Path.Combine(directoryName, string.Format("{0} ({1})", fileNameWithoutExtension, num++) + extension);
                    }
                    File.Move(file, str3);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static int VersionToInt(Version version)
        {
            return ((((version.Major * 0x989680) + (version.Minor * 0x2710)) + (version.Build * 100)) + version.Revision);
        }

        public static string WildcardToRegex(string pattern)
        {
            return ("^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$");
        }
    }
}
