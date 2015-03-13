using System;
using System.IO;

namespace BanaBot.Data
{
    public static class Directories
    {
        public static readonly string ConfigFilePath = Path.Combine(CurrentDirectory, "config.xml");
        public static readonly string CurrentDirectory = (AppDomain.CurrentDomain.BaseDirectory + @"\");
        public static readonly string LoaderFilePath = Path.Combine(CurrentDirectory, "BanaBot.exe");
    }
}
