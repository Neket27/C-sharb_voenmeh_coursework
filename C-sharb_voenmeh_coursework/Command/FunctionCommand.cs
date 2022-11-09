using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using app.History;
using app.models;
using app.models.Entity;

namespace C_sharb_voenmeh_coursework.Command;

public class FunctionCommand :BaseViewModel
{
    public ObservableCollection<EntityDirectoryAndFile> DirectoriesAndFilesLeftPanel { get; set; } =
        new ObservableCollection<EntityDirectoryAndFile>();

    public ObservableCollection<EntityDirectoryAndFile> DirectoriesAndFiles { get; set; } =
        new ObservableCollection<EntityDirectoryAndFile>();
    
    public EntityDirectoryAndFile SelectedFileEntity { get; set; }
    public IDirectoryHistory History { get; set; }
    
    private FileInfo SaveCopyFile;
    private bool FlagCut = false;
    private FilePC SaveCutFile;
    public string PathIcon { get; set; }
    public string TextInPreview { get; set; }
    
    public string FilePath { get; set; } = "Этот компьютер";
    public string Name { get; set; } = "Этот компьютер";
    
    
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
                var DirectoryInfo = new DirectoryInfo(FilePath);

                foreach (var directory in DirectoryInfo.GetDirectories())
                    DirectoriesAndFiles.Add(new DirectoryPC(directory));

                foreach (var FileInfo in DirectoryInfo.GetFiles())
                    DirectoriesAndFiles.Add(new FilePC(FileInfo));
            }
        }


        protected void Open(object parameter)
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
            Name="Этот компьютер";
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
            if(parameter is DirectoryPC directoryPc)
            {
                FilePath = directoryPc.FullName;
                DirectoryInfo directoryInfo = new DirectoryInfo(FilePath);
            } 

            if (parameter is FilePC filePc)
            {
                SaveCopyFile = new FileInfo(filePc.FullName);
            }
        }

        protected void Paste(object? parameter)
        {
            if (parameter == null)
                parameter = new DirectoryPC(FilePath);

            if (parameter is DirectoryPC directoryPc)
            {
                FilePath = directoryPc.FullName; 
            
                if (FlagCut)
                {
                    string path1 = SaveCutFile.FullName;
                    string name1 = SaveCutFile.Name;
                    string path2 = directoryPc.FullName + "\\"+SaveCutFile.Name;
                    string name2 = directoryPc.Name;
                    File.Copy (path1, path2, true);
                    File.Delete(path1);
                    FlagCut = false;
                }
                else
                {
                    SaveCopyFile.CopyTo(FilePath + "\\" + SaveCopyFile.Name);
                }
                OpenDirectory();
            }
        }

        protected void Delete(object? parameter)
        {
            if (parameter is DirectoryPC directoryPc)
            {
                FilePath = directoryPc.FullName;
                DirectoryInfo directoryInfo = new DirectoryInfo(FilePath);
                directoryInfo.Delete();
                OpenDirectory();
            }

            if (parameter is FilePC filePc)
            {
                FileInfo fileInfo = new FileInfo(filePc.FullName);
                fileInfo.Delete();
                OpenDirectory();
            }
        }

        protected void Cut(object parameter)
        {
            if (parameter is FilePC filePc)
            {
                FlagCut = true;
                SaveCutFile = filePc;
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