using app.models;
using C_sharb_voenmeh_coursework.Actions;
using Explorer.Shared.ViewModels;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace C_sharb_voenmeh_coursework.Convertores
{
    internal class FileEntityToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = new Image();



            if (value is DirectoryPC directoryPC)
            {
                if(directoryPC.Name == "Этот компьютер")
                {
                    var resource = Application.Current.TryFindResource("pc");
                    if (resource is Image img)
                        return img.Source;
                }
                else if (directoryPC.Name == "Рабочий стол")
                {
                    var resource = Application.Current.TryFindResource("folder-desktop");
                    if (resource is Image img)
                        return img.Source;
                }
                else if (directoryPC.Name == "Видео")
                {
                    var resource = Application.Current.TryFindResource("multimedia");
                    if (resource is Image img)
                        return img.Source;
                }
                else if (directoryPC.Name == "Документы")
                {
                    var resource = Application.Current.TryFindResource("documents");
                    if (resource is Image img)
                        return img.Source;
                }
                else if (directoryPC.Name == "Изображения")
                {
                    var resource = Application.Current.TryFindResource("image-viewer");
                    if (resource is Image img)
                        return img.Source;

                } 
                else if (directoryPC.Name == "Музыка")
                {
                    var resource = Application.Current.TryFindResource("audio-player");
                    if (resource is Image img)
                        return img.Source;
                }
                else
                {
                    var resource = Application.Current.TryFindResource("folder");
                    if (resource is Image img)
                        return img.Source;
                }
            }
            else if(value is FilePC filePC)
            {
                var extension = new FileInfo(filePC.FullName).Extension;
                //var imagePath = Path.GetDirectoryName(filePC.FullName);
                var imagePath = ExtensionToImageFileConverter.GetImagePath(extension);
                var settings = new WpfDrawingSettings
                {
                    TextAsGeometry = false,
                    IncludeRuntime = true,
                };
                

                if (imagePath.Extension.ToLower() == ".svg")
                {
                    var converter = new FileSvgReader(settings);
                    var drawing = converter.Read(imagePath.FullName);
                    if(drawing != null)
                        return new DrawingImage(drawing);
                }
                else
                {
                    var bitmapSourse = new BitmapImage(new Uri(imagePath.FullName));
                    return bitmapSourse;
                }


                var resource = Application.Current.TryFindResource("text-editor");

                if (resource is Image img)
                    return img.Source;
            }

                     
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
