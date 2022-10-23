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

            if(value is DirectoryPC)
            {
              var resource =  Application.Current.TryFindResource("folder");

                if (resource is Image img)
                    return img.Source;
               
            }
            else if(value is FilePC)
            {
                var resource = Application.Current.TryFindResource("file");

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
