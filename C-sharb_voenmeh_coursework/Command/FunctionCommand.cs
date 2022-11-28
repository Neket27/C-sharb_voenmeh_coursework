using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using app.History;
using app.models;
using app.models.Entity;
using Microsoft.VisualBasic.FileIO;

namespace C_sharb_voenmeh_coursework.Command;

public class FunctionCommand : ModelOutput
{
    #region Variebles

    private FileInfo SaveCopyFile;
    private DirectoryInfo SaveCopyDirectory;
    private bool FlagCut = false;
    private FilePC SaveCutFile;
    private DirectoryPC SaveCutDirectory;
    private bool FlagCopyFile;
    public IDirectoryHistory History { get; set; }

    #endregion


    #region Functions

    public void OpenDirectory()
    {
        DirectoriesAndFiles.Clear();

        if (Name == "Этот компьютер")
        {
            foreach (var logicalDrive in Directory.GetLogicalDrives()) // Считываем все диски пк в коллекцию
                DirectoriesAndFiles.Add(new DirectoryPC(logicalDrive));
        }
        else
        {
            FilePath = ((MainWindow) Application.Current.MainWindow).PathFile.Text;
            var DirectoryInfo = new DirectoryInfo(FilePath);

            foreach (var directory in DirectoryInfo.GetDirectories())
                DirectoriesAndFiles.Add(new DirectoryPC(directory));

            foreach (var FileInfo in DirectoryInfo.GetFiles())
                DirectoriesAndFiles.Add(new FilePC(FileInfo));
        }
    }


    public void Open(object parameter)
    {
        if (parameter is String)
            parameter = new DirectoryPC((String) parameter);

        if (parameter is DirectoryPC directoryPc)
        {
            FilePath = directoryPc.FullName;
            Name = directoryPc.Name;
            History.Add(FilePath, Name);

            OpenDirectory();
        }
        else if (parameter is FilePC filePc)
        {
            new Process()
            {
                StartInfo = new ProcessStartInfo(filePc.FullName)
                {
                    UseShellExecute = true
                }
            }.Start();

            Click(null);
        }
    }


    protected void Close(object parameter)
    {
        Name = "Этот компьютер";
        FilePath = "Этот компьютер";
        History.Clear();
        OpenDirectory();
    }


    protected void Click(object parameter)
    {
        if (parameter is FilePC filePc)
        {
            PathIcon = filePc.FullName;

            FileInfo fileInfo = new FileInfo(filePc.FullName);
            if (fileInfo.Extension == ".txt")
            {
                using (FileStream fstream = File.OpenRead(filePc.FullName))
                {
                    byte[] buffer = new byte[fstream.Length];
                    fstream.Read(buffer, 0, buffer.Length);
                    TextInPreview = Encoding.Default.GetString(buffer);
                }
            }
            else
                TextInPreview = "";
        }
        else
            PathIcon = "";
    }

    protected void Copy(object parameter)
    {
        if (parameter is DirectoryPC directoryPc)
        {
            FilePath = directoryPc.FullName;
            SaveCopyDirectory = new DirectoryInfo(FilePath);
        }

        if (parameter is FilePC filePc)
        {
            SaveCopyFile = new FileInfo(filePc.FullName);
            FlagCopyFile = true;
        }
    }

    protected void Paste(object? parameter)
    {
        if (parameter == null)
            parameter = new DirectoryPC(FilePath);

        if (parameter is DirectoryPC directoryPc)
        {
            FilePath = directoryPc.FullName;
            
            string source = FilePath;
            string dest = FilePath;

            if (FlagCut)
            {
                if (FlagCopyFile)
                {
                    ///


                    ////

                    string path1 = SaveCutFile.FullName;
                    string name1 = SaveCutFile.Name;
                    string path2 = directoryPc.FullName + "\\" + SaveCutFile.Name;
                    string name2 = directoryPc.Name;
                    File.Copy(path1, path2, true);
                    File.Delete(path1);
                    FlagCut = false;
                }
                else
                {
                    FileSystem.CopyDirectory(SaveCutDirectory.FullName, FilePath + "\\" + SaveCutDirectory.Name);
                    FileSystem.DeleteDirectory(SaveCutDirectory.FullName, DeleteDirectoryOption.DeleteAllContents);
                    FlagCut = false;
                }
            }
            else
            {
                if (FlagCopyFile)
                {
                    if (SaveCopyFile.FullName == FilePath + "\\" + SaveCopyFile.Name)
                    {
                        ////count copy
                        source = FilePath;
                        dest = FilePath;
                        var destFilePath = dest + @"\" + SaveCopyFile.Name;

                        var postfix = 1;
                        while (File.Exists(destFilePath))
                        {
                            var fileNameNoExt = Path.GetFileNameWithoutExtension(destFilePath);
                            var fileExt = Path.GetExtension(destFilePath);

                            if (postfix == 1)
                                destFilePath = dest + @"\" + fileNameNoExt + postfix + fileExt;
                            else
                                destFilePath = dest + @"\" +
                                               fileNameNoExt.Remove(
                                                   fileNameNoExt.Length - postfix.ToString().Length) +
                                               postfix + fileExt;

                            postfix++;
                        }
                        ///////////////
                        File.Copy(source + @"\" + SaveCopyFile.Name, destFilePath);
                    }
                    else
                    {
                        SaveCopyFile.CopyTo(FilePath + "\\" + SaveCopyFile.Name);
                    }
                }
                else
                {
                    // FileSystem.CopyDirectory(SaveCopyDirectory.FullName, FilePath + "\\" + SaveCopyDirectory.Name);
                    
                    if (SaveCopyDirectory.FullName == FilePath+ "\\" + SaveCopyDirectory.Name )
                    {
                    ////count copy
                     source = FilePath;
                     dest = FilePath;
                    var destFilePath = dest + @"\" + SaveCopyDirectory.Name;
                    var postfix = 1;
                    while (Directory.Exists(destFilePath))
                    {
                        var fileNameNoExt = Path.GetFileNameWithoutExtension(destFilePath);
                        var fileExt = Path.GetExtension(destFilePath);

                        if (postfix == 1)
                            destFilePath = dest + @"\" + fileNameNoExt + postfix + fileExt;
                        else
                            destFilePath = dest + @"\" +
                                           fileNameNoExt.Remove(
                                               fileNameNoExt.Length - postfix.ToString().Length) +
                                           postfix + fileExt;

                        postfix++;
                    }

                    FileSystem.CopyDirectory(source + @"\" + SaveCopyDirectory.Name, destFilePath);
                    /////////
                }
                    else
                    { 
                        FileSystem.CopyDirectory(SaveCopyDirectory.FullName, FilePath + "\\" + SaveCopyDirectory.Name); 
                    }
                    }

                FlagCopyFile = false;
            }

            OpenDirectory();
        }
    }

    protected void Delete(object? parameter)
    {
        if (parameter is DirectoryPC directoryPc)
        {
            FileSystem.DeleteDirectory(directoryPc.FullName, DeleteDirectoryOption.DeleteAllContents);
            OpenDirectory();
        }

        if (parameter is FilePC filePc)
        {
            FileSystem.DeleteFile(filePc.FullName);
            OpenDirectory();
        }
    }

    protected void Cut(object parameter)
    {
        if (parameter is DirectoryPC directoryPc)
        {
            FlagCut = true;
            SaveCutDirectory = directoryPc;
        }

        if (parameter is FilePC filePc)
        {
            FlagCut = true;
            SaveCutFile = filePc;
            FlagCopyFile = true;
        }
    }


    protected void CallLeftPanel()
    {
        string UserName = Environment.UserName;
        EntityDirectoryAndFile DirDesktop = new DirectoryPC("C:\\Users\\" + UserName + "\\Desktop");
        EntityDirectoryAndFile DirVideo = new DirectoryPC("C:\\Users\\" + UserName + "\\Videos");
        EntityDirectoryAndFile DirDocumet = new DirectoryPC("C:\\Users\\" + UserName + "\\Documents");
        EntityDirectoryAndFile DirPictures = new DirectoryPC("C:\\Users\\" + UserName + "\\Pictures");
        EntityDirectoryAndFile DirMusic = new DirectoryPC("C:\\Users\\" + UserName + "\\Music");
        EntityDirectoryAndFile DirThisComputer = new DirectoryPC("Этот компьютер");

        DirThisComputer.Name = "Этот компьютер";
        DirDesktop.Name = "Рабочий стол";
        DirVideo.Name = "Видео";
        DirDocumet.Name = "Документы";
        DirPictures.Name = "Изображения";
        DirMusic.Name = "Музыка";
        DirectoriesAndFilesLeftPanel.Add(DirThisComputer);
        DirectoriesAndFilesLeftPanel.Add(DirDesktop);
        DirectoriesAndFilesLeftPanel.Add(DirVideo);
        DirectoriesAndFilesLeftPanel.Add(DirPictures);
        DirectoriesAndFilesLeftPanel.Add(DirMusic);
    }

    #endregion
}