using System.Collections;
using System.IO;
using System.Windows;
using app;
using app.models;
using app.models.Entity;
using C_sharb_voenmeh_coursework.Command;
using GongSolutions.Wpf.DragDrop;
using Microsoft.VisualBasic.FileIO;

namespace C_sharb_voenmeh_coursework;

public class DragDrop : ModelOutput, IDropTarget
{
    public static DragDrop Instance = new DragDrop();
    private FunctionCommand FunctionCommand = new FunctionCommand();

    public void DragOver(IDropInfo dropInfo)
    {
        if (dropInfo.TargetItem is DirectoryPC)
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
        else
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;

        var dataObject = dropInfo.Data as IDataObject;
        dropInfo.Effects = DragDropEffects.Move;
    }


    public void Drop(IDropInfo dropInfo)
    {
      
        EntityDirectoryAndFile entityDirectoryAndFile = (EntityDirectoryAndFile) dropInfo.TargetItem;
        if (entityDirectoryAndFile==null)
        {
           string pathDirectory=((MainWindow) Application.Current.MainWindow).PathFile.Text;
            entityDirectoryAndFile = new DirectoryPC(pathDirectory);
        }

        if (dropInfo.Data is FilePC filePc)
        {
            if (filePc.FullName != entityDirectoryAndFile.FullName + "\\" + filePc.Name)
            {
                try
                {
                    File.Copy(filePc.FullName, entityDirectoryAndFile.FullName + "\\" + filePc.Name, true);
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(filePc);
                    // File.Delete(filePc.FullName);
                    FileSystem.DeleteFile(filePc.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
                }
                catch { }
           }
        }
        else
        {
            var dataObject = dropInfo.Data as DataObject;
           if (dataObject != null && dataObject.ContainsFileDropList())
            {
                var files = dataObject.GetFileDropList();
                foreach (var path in files)
                {
                    FileInfo fileInfo = new FileInfo(path);
                        if (fileInfo.Extension == "")
                        {
                            FileSystem.CopyDirectory(path, entityDirectoryAndFile.FullName + "\\" + fileInfo.Name,
                                true);
                            // FunctionCommand.OpenDirectory();
                            FunctionCommand.Open(new DirectoryInfo(entityDirectoryAndFile.FullName));
                        }
                        else
                        {
                            string path2 = entityDirectoryAndFile.FullName + "\\" + fileInfo.Name;
                            File.Copy(path, path2, true);
                            
                            // var a = new DirectoryInfo(entityDirectoryAndFile.FullName);
                            // var f = new DirectoryPC(a);
                            // FunctionCommand.Open(f);
                        }
                    }
                    //  ((IList) dropInfo.DragInfo.SourceCollection).Remove(path);
                }
            else
            {
                var dataObjectDir = dropInfo.Data as DirectoryPC;

                if (dataObjectDir.FullName != entityDirectoryAndFile.FullName + "\\" + dataObjectDir.Name)
                {
                    bool flagExistence = false;
                    var DirectoryInfo = new DirectoryInfo(entityDirectoryAndFile.FullName);
                    foreach (var dir in DirectoryInfo.GetDirectories())
                    {
                        if (dir.Name == dataObjectDir.Name)
                            flagExistence = true;
                    }

                    if (flagExistence == false)
                    {
                        try
                        {
                            FileSystem.CopyDirectory(dataObjectDir.FullName,
                            entityDirectoryAndFile.FullName + "\\" + dataObjectDir.Name, true);
                            File.SetAttributes(dataObjectDir.FullName, FileAttributes.Normal);
                            //FileSystem.DeleteDirectory(dataObjectDir.FullName, DeleteDirectoryOption.DeleteAllContents); //выдаёт ошибку по доступу
                            FileSystem.DeleteDirectory(dataObjectDir.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
                            ((IList)dropInfo.DragInfo.SourceCollection).Remove(dataObjectDir);
                            flagExistence = false;
                        }
                        catch { }
                    }

                }
            }
        }

        FunctionCommand.OpenDirectory();
    }
}


//
//
// public void DragOver(IDropInfo dropInfo)
// {
//     if (dropInfo.TargetCollection is ObservableCollection<EntityDirectoryAndFile> targetCollection)
//     {
//         if (dropInfo.Data is EntityDirectoryAndFile fileEntityViewModel)
//         {
//             if (dropInfo.TargetItem == null && !targetCollection.Contains(fileEntityViewModel))
//             {
//                 dropInfo.Effects = DragDropEffects.Move;
//                 dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
//                 dropInfo.EffectText ="Moveto";
//                 dropInfo.DestinationText = "в папку";
//             }
//
//             if ((dropInfo.TargetItem is EntityDirectoryAndFile directoryViewModel) && (directoryViewModel != fileEntityViewModel))
//             {
//                 dropInfo.Effects =  DragDropEffects.Move;
//                 dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
//                 dropInfo.EffectText ="Moveto2";
//                 dropInfo.DestinationText = "в папку2";
//
//             }
//         }
//
//         if (dropInfo.Data is ICollection<object> collectionFileEntityViewModel)
//         {
//                     
//         }
//     }
//         
// }
//
// public void Drop(IDropInfo dropInfo)
// {
//     FilePC dataObject = dropInfo.Data as FilePC;
//     if (dataObject != null )
//     {
//         var files = dataObject;
//         DirectoriesAndFiles.Add(files);
//         // Items.Add(files);
//         // foreach (var file in files)
//         // {
//         //     Items.Add(new FilePC(file));
//         //     DirectoriesAndFiles.Add(new FilePC(file));
//         //
//         // }
//         OpenDirectory();
//     }
// }