using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
    private object EntetyPcDoRename;
    private bool CreateDir;
    private bool CreateFileText;
    public IDirectoryHistory History { get; set; }

    #endregion

    #region Functions

    public void OpenDirectory()
    {
        DirectoriesAndFiles.Clear();
        try
        {
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
        catch { }
    }

    public void Open(object parameter)
    {
        if (parameter is String)
            parameter = new DirectoryPC((string) parameter);

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
            else if (fileInfo.Extension == ".rtf")
            {
                RichTextBox rtb = new RichTextBox();
                string rtf = File.ReadAllText(filePc.FullName);
                using (MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(rtf)))
                    rtb.Selection.Load(stream, DataFormats.Rtf);

                string text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
                // string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                TextInPreview = text;
            }
            else
                TextInPreview = "";
        }
        else
            PathIcon = "";
    }


    protected void Copy(object parameter)
    {
        FlagCut = false;
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
        try
        {
            if (parameter is FilePC filePc)
            {
                string path = Path.GetDirectoryName(filePc.FullName);
                parameter = new DirectoryPC(path);
            }

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
                        string path1 = SaveCutFile.FullName;
                        string name1 = SaveCutFile.Name;
                        string path2 = directoryPc.FullName + "\\" + SaveCutFile.Name;
                        string name2 = directoryPc.Name;
                        File.Copy(path1, path2, true);
                        File.Delete(path1);
                        FlagCut = false;
                        FlagCopyFile = false;
                    }
                    else
                    {
                        FileSystem.CopyDirectory(SaveCutDirectory.FullName, FilePath + "\\" + SaveCutDirectory.Name);
                        OpenDirectory();
                        // FileSystem.DeleteDirectory(SaveCutDirectory.FullName, DeleteDirectoryOption.DeleteAllContents);
                        FileSystem.DeleteDirectory(SaveCutDirectory.FullName, UIOption.AllDialogs,
                            RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
                        FlagCut = false;
                    }
                }
                else
                {
                    if (FlagCopyFile)
                    {
                        if (SaveCopyFile.FullName == FilePath + "\\" + SaveCopyFile.Name)
                        {
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
                                    destFilePath = dest + @"\" + fileNameNoExt.Remove(fileNameNoExt.Length - postfix.ToString().Length) + postfix + fileExt;

                                postfix++;
                            }
                            File.Copy(source + @"\" + SaveCopyFile.Name, destFilePath);
                        }
                        else
                            SaveCopyFile.CopyTo(FilePath + "\\" + SaveCopyFile.Name);
                        
                    }
                    else
                    {
                        try
                        {
                            if (SaveCopyDirectory.FullName == FilePath + "\\" + SaveCopyDirectory.Name || SaveCopyDirectory.FullName == FilePath)
                            {
                                if (SaveCopyDirectory.FullName == FilePath)
                                    FilePath = Path.GetDirectoryName(FilePath);
                                
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
                                        destFilePath = dest + @"\" + fileNameNoExt.Remove(fileNameNoExt.Length - postfix.ToString().Length) + postfix + fileExt;

                                    postfix++;
                                }
                                FileSystem.CopyDirectory(source + @"\" + SaveCopyDirectory.Name, destFilePath);
                                }
                            else
                            {
                                FileSystem.CopyDirectory(SaveCopyDirectory.FullName, FilePath + "\\" + SaveCopyDirectory.Name);
                            }
                        }catch { }
                    }
                    FlagCopyFile = false;
                }
                OpenDirectory();
            }
        }catch { }
    }

    protected void Delete(object? parameter)
    {
        if (parameter is DirectoryPC directoryPc)
        {
            // FileSystem.DeleteDirectory(directoryPc.FullName, DeleteDirectoryOption.DeleteAllContents);
            try
            {
                FileSystem.DeleteDirectory(directoryPc.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin,
                    UICancelOption.ThrowException);
            }catch { }
        }

        if (parameter is FilePC filePc)
        {
            try
            {
                // FileSystem.DeleteFile(filePc.FullName);
                FileSystem.DeleteFile(filePc.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
            }catch { }
        }
        OpenDirectory();
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

    protected void Rename(object parameter)
    {
        EntetyPcDoRename = parameter;
        IsPanelVisible = true;

        if (EntetyPcDoRename is DirectoryPC directoryPc)
            Name = directoryPc.Name;
        if (EntetyPcDoRename is FilePC filePc)
            Name = filePc.Name;
    }


    protected void RenameClose(object parameter)
    {
        try
        {
            string nameFile = ((MainWindow) Application.Current.MainWindow).TextBoxRename.Text;
            if (CreateDir == true && EntetyPcDoRename is DirectoryPC dir)
            {
                Directory.CreateDirectory(dir.FullName + "\\" + nameFile);
                CreateDir = false;
            }
            else if (CreateFileText == true && EntetyPcDoRename is DirectoryPC dir2)
            {
                if (nameFile != "")
                    File.Create(dir2.FullName + "\\" + nameFile + ".txt");

                CreateFileText = false;
            }
            else
            {
                if (EntetyPcDoRename is DirectoryPC directoryPc)
                {
                    if (nameFile != directoryPc.Name)
                    {
                        string curDir = Path.GetDirectoryName(directoryPc.FullName);
                        Directory.Move(directoryPc.FullName, Path.Combine(curDir, nameFile));
                        OpenDirectory();
                    }
                }

                if (EntetyPcDoRename is FilePC filePc)
                {
                    string curDir = Path.GetDirectoryName(filePc.FullName);
                    try
                    {
                        File.Move(filePc.FullName, Path.Combine(curDir, nameFile));
                    }catch { }
                    OpenDirectory();
                }
            }
        }
        catch
        {
            RenameClose(null);
        }

        IsPanelVisible = false;
        OpenDirectory();
    }


    protected void CreateDirectory(object parameter)
    {
        IsPanelVisible = true;
        CreateDir = true;
        
        if (parameter is FilePC filePc)
        {
            string path = Path.GetDirectoryName(filePc.FullName);
            parameter = new DirectoryPC(path);
        }

        if (parameter == null)
            parameter = new DirectoryPC(FilePath);

        EntetyPcDoRename = parameter;
        ((MainWindow) Application.Current.MainWindow).TextBoxRename.Text = "";
    }

    protected void CreateTextFile(object parameter)
    {
        IsPanelVisible = true;
        CreateFileText = true;

        if (parameter is FilePC filePc)
        {
            string path = Path.GetDirectoryName(filePc.FullName);
            parameter = new DirectoryPC(path);
        }

        if (parameter == null)
            parameter = new DirectoryPC(FilePath);

        EntetyPcDoRename = parameter;
        ((MainWindow) Application.Current.MainWindow).TextBoxRename.Text = "";
    }


    public void CallLeftPanel()
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