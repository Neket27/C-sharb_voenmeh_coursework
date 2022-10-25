using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace C_sharb_voenmeh_coursework.Actions
{
    internal static class ExtensionToImageFileConverter
    {

        private static IconsSettings _settings;

        static ExtensionToImageFileConverter()
        {
            var settings = new IconsSettings
            {
                Icons =new Dictionary<string, string>
                {
                    {".exe","system-windows.ico"}
                }

            };
            IconsSettings.Save(settings, "icons.json");


            _settings = IconsSettings.Open("icons.json");
        }


        public static FileInfo GetImagePath(string extension)
        {
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (_settings.Icons.ContainsKey(extension))
            {
                var path = _settings.Icons[extension];
                return new FileInfo(Path.Combine(applicationDirectory, "Resource/Icons", path));
            }


            return new FileInfo(Path.Combine(applicationDirectory,"Resource/Icons", "folder2.jpg"));
        }

    }

    public class IconsSettings
    {
        public Dictionary<string, string> Icons { get; set; } = new Dictionary<string, string>();
        public static IconsSettings Open(string path)
        {
            var json = File.ReadAllText(path);

            try
            {
                var settings  = JsonSerializer.Deserialize<IconsSettings>(json);
                return settings;
            }
            catch(Exception e)
            {

            }
            return new IconsSettings();
        }

        public static void Save(IconsSettings settings, string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            try
            {
                var json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(path, json);
            }
            catch(Exception e)
            {


            }

         
        }
       
    }
}
