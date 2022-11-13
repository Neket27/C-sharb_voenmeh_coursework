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

public class DragDrop : IDropTarget
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

        if (dropInfo.Data is FilePC)
        {
            FilePC filePc = (FilePC) dropInfo.Data;
            File.Copy(filePc.FullName, entityDirectoryAndFile.FullName + "\\" + filePc.Name, true);
            ((IList) dropInfo.DragInfo.SourceCollection).Remove(filePc);
            File.Delete(filePc.FullName);
            FunctionCommand.OpenDirectory();
        }
        else
        {
            DirectoryPC directoryPc = (DirectoryPC) dropInfo.Data;
            FileSystem.CopyDirectory(directoryPc.FullName, entityDirectoryAndFile.FullName + "\\" + directoryPc.Name, true);
            ((IList) dropInfo.DragInfo.SourceCollection).Remove(directoryPc);
            FileSystem.DeleteDirectory(directoryPc.FullName,DeleteDirectoryOption.DeleteAllContents);
            FunctionCommand.OpenDirectory();
            }
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