using System.Collections;
using System.IO;
using System.Windows;
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
    private string SaveValue;

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