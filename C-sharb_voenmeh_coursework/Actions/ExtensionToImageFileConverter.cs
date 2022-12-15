using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace C_sharb_voenmeh_coursework.Actions
{
    internal static class Ico
    {
        private static Icons? Icons = Icons.Open("icons.json");

        public static string GetImagePath(string extension)
        {
            string applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (Icons.MyIcons.ContainsKey(extension))
            {
                string name = Icons.MyIcons[extension];
                return new FileInfo(Path.Combine(applicationDirectory, "Resource/Icons", name)).FullName;
            }

            return null;
        }
    }

    public class Icons
    {
        public Dictionary<string, string> MyIcons { get; set; } =
            new Dictionary<string, string>(); // название словаря должно соответствовать названию словаря json

        public static Icons? Open(string path)
        {
            string json = File.ReadAllText(path);

            try
            {
                Icons? dictionaryIcons = JsonSerializer.Deserialize<Icons>(json);
                return dictionaryIcons;
            }
            catch (Exception e) { }

            return new Icons();
        }
    }
}