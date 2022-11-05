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
using System.Xml.Linq;

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
                            Image resource = (Image)Application.Current.TryFindResource("pc");
                            if (resource is Image img)
                                return img.Source;
                            break;
                        }

                    case "Рабочий стол":
                        {
                            Image resource = (Image)Application.Current.TryFindResource("folder-desktop");
                            if (resource is Image img)
                                return img.Source;
                            break;
                        }

                    case "Видео":
                        {
                            Image resource = (Image)Application.Current.TryFindResource("multimedia");
                            if (resource is Image img)
                                return img.Source;
                            break;
                        }

                    case "Документы":
                        {
                            Image resource = (Image)Application.Current.TryFindResource("documents");
                            if (resource is Image img)
                                return img.Source;
                            break;
                        }

                    case "Изображения":
                        {
                            Image resource = (Image)Application.Current.TryFindResource("image-viewer");
                            if (resource is Image img)
                                return img.Source;
                            break;
                        }

                    case "Музыка":
                        {
                            Image resource = (Image)Application.Current.TryFindResource("audio-player");
                            if (resource is Image img)
                                return img.Source;
                            break;
                        }

                    default:
                        {
                            Image resource = (Image)Application.Current.TryFindResource("folder");
                            if (resource is Image img)
                                return img.Source;
                            break;
                        }
                }
            }
            else if(value is FilePC filePC)
            {
                string extension = new FileInfo(filePC.FullName).Extension;
                FileInfo imagePath = Ico.GetImagePath(extension);
                WpfDrawingSettings settings = new WpfDrawingSettings()
                {
                    TextAsGeometry = false,
                    IncludeRuntime = true,
                };
                //if (imagePath.Extension.ToLower() == ".doc")
                //{
                //    FileSvgReader converter = new FileSvgReader(settings);
                //    DrawingGroup drawing = converter.Read(imagePath.FullName);
                //    if (drawing != null)
                //        return new DrawingImage(drawing);
                //}
                //else
                //{
                 BitmapImage bitmapSourse = new BitmapImage(new Uri(imagePath.FullName));
                    return bitmapSourse;
                //}

                Image resource = (Image) Application.Current.TryFindResource("text-editor");

                if (resource is Image img)
                    return img.Source;
            }
                     
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        //private ImageSource? GetPathIconDirectoryPC(string name)
        //{
        //    switch (name)
        //    {
        //        case "Этот компьютер":
        //            {
        //                Image resource = (Image)Application.Current.TryFindResource("pc");
        //                if (resource is Image img)
        //                    return img.Source;
        //                break;
        //            }

        //        case "Рабочий стол":
        //            {
        //                Image resource = (Image)Application.Current.TryFindResource("folder-desktop");
        //                if (resource is Image img)
        //                    return img.Source;
        //                break;
        //            }

        //        case "Видео":
        //            {
        //                Image resource = (Image)Application.Current.TryFindResource("multimedia");
        //                if (resource is Image img)
        //                    return img.Source;
        //                break;
        //            }

        //        case "Документы":
        //            {
        //                Image resource = (Image)Application.Current.TryFindResource("documents");
        //                if (resource is Image img)
        //                    return img.Source;
        //                break;
        //            }

        //        case "Изображения":
        //            {
        //                Image resource = (Image)Application.Current.TryFindResource("image-viewer");
        //                if (resource is Image img)
        //                    return img.Source;
        //                break;
        //            }

        //        case "Музыка":
        //            {
        //                Image resource = (Image)Application.Current.TryFindResource("audio-player");
        //                if (resource is Image img)
        //                    return img.Source;
        //                break;
        //            }

        //        default:
        //            {
        //                Image resource = (Image)Application.Current.TryFindResource("folder");
        //                if (resource is Image img)
        //                    return img.Source;
        //                break;
        //            }
        //    }
        //    return null;
        //}
        }
    }
