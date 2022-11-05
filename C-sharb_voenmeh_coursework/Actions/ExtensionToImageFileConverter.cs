using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace C_sharb_voenmeh_coursework.Actions
{
    internal static class Ico
    {
        private static Icons? Icons=Icons.Open("icons.json");
        public static FileInfo GetImagePath(string extension)
        {
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (Icons.MyIcons.ContainsKey(extension))
            {
                var path = Icons.MyIcons[extension];
                return new FileInfo(Path.Combine(applicationDirectory, "Resource/Icons", path));
            }

            return new FileInfo(Path.Combine(applicationDirectory, "Resource/Icons", "folder2.jpg"));
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

        // public static void Save(Icons  dictionaryIcons, string path)
        // {
        //     JsonSerializerOptions options = new JsonSerializerOptions()
        //     {
        //         WriteIndented = true,
        //     };
        //     try
        //     {
        //         string json = JsonSerializer.Serialize(dictionaryIcons, options);
        //         File.WriteAllText(path, json);
        //     }
        //     catch (Exception e)
        //     {
        //     }
        // }
    }
}