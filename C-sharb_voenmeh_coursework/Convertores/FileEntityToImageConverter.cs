using app.models;
using Explorer.Shared.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

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
            else if(value is FilePC)
            {
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
