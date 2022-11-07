using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using app.models;
using C_sharb_voenmeh_coursework.Actions;
using SharpVectors.Renderers.Wpf;
using Image = System.Windows.Controls.Image;

namespace C_sharb_voenmeh_coursework.Convertores
{
    internal class FileEntityToImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = new Image();

            if (value is DirectoryPC directoryPC)
            {
                switch (directoryPC.Name)
                {
                    case "Этот компьютер":
                    {
                        Image resource = (Image) Application.Current.TryFindResource("pc");
                        if (resource is Image img)
                            return img.Source;
                        break;
                    }

                    case "Рабочий стол":
                    {
                        Image resource = (Image) Application.Current.TryFindResource("folder-desktop");
                        if (resource is Image img)
                            return img.Source;
                        break;
                    }

                    case "Видео":
                    {
                        Image resource = (Image) Application.Current.TryFindResource("multimedia");
                        if (resource is Image img)
                            return img.Source;
                        break;
                    }

                    case "Документы":
                    {
                        Image resource = (Image) Application.Current.TryFindResource("documents");
                        if (resource is Image img)
                            return img.Source;
                        break;
                    }

                    case "Изображения":
                    {
                        Image resource = (Image) Application.Current.TryFindResource("image-viewer");
                        if (resource is Image img)
                            return img.Source;
                        break;
                    }

                    case "Музыка":
                    {
                        Image resource = (Image) Application.Current.TryFindResource("audio-player");
                        if (resource is Image img)
                            return img.Source;
                        break;
                    }

                    default:
                    {
                        Image resource = (Image) Application.Current.TryFindResource("folder");
                        if (resource is Image img)
                             return img.Source;
                        break;
                    }
                }
            }
            else if (value is FilePC filePC)
            {
                string extension = new FileInfo(filePC.FullName).Extension;
                FileInfo? imagePath = Ico.GetImagePath(extension);
                WpfDrawingSettings settings = new WpfDrawingSettings()
                {
                    TextAsGeometry = false,
                    IncludeRuntime = true,
                };
              
                if (imagePath != null)
                {
                    BitmapImage bitmapSourse = new BitmapImage(new Uri(imagePath.FullName));
                    return bitmapSourse;
                }
                else
                {
                    //Системные иконки windows
                    Icon ico = Icon.ExtractAssociatedIcon(filePC.FullName);
                    Bitmap bitmap = ico.ToBitmap(); 
                    return BitmapBitmapSourse(bitmap); 
                }
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private BitmapSource BitmapBitmapSourse(Bitmap bitmap)
        {                
            BitmapSource i = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return i;
        }
    }


    }
