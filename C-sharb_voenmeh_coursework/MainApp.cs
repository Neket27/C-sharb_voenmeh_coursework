using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using app.History;
using app.models;
using app.models.Entity;

namespace app
{
    public class MainApp : BaseViewModel
    {
        #region Variebles

        public ObservableCollection<EntityDirectoryAndFile> DirectoriesAndFilesLeftPanel { get; set; } =
            new ObservableCollection<EntityDirectoryAndFile>();

        public ObservableCollection<EntityDirectoryAndFile> DirectoriesAndFiles { get; set; } =
            new ObservableCollection<EntityDirectoryAndFile>();

        public string FilePath { get; set; } = "Этот компьютер";
        public EntityDirectoryAndFile SelectedFileEntity { get; set; }
        public string Name { get; set; } = "Этот компьютер";
        private readonly IDirectoryHistory _history;
        private FileInfo SaveCopyFile;
        private bool flag = false;
        private object ParamCut;
        public string PathIcon { get; set; }

        #endregion

        #region Commands

        public ICommand OpenCommand { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public ICommand CopyCommand { get; set; }
        public ICommand PasteCommand { get; set; }
        public ICommand CutCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public DelegateCommand MoveBackCommand { get; set; }
        public DelegateCommand MoveForwardCommand { get; set; }

        #endregion

        #region Constructors

        public MainApp()
        {
            //Левая панель
            CallLeftPanel();
            //Конец левой панели

            //Центральный вывод дисков, папок, файлов
            OpenDirectory();
            // Конец центрального вывода дисков, папок, файлов

            _history = new DirectoryHistory("Этот компьютер", "Этот компьютер");
            OpenCommand = new DelegateCommand(Open);
            ClickCommand = new Explorer.Shared.ViewModels.DelegateCommand(Click);
            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);

            CopyCommand = new DelegateCommand(Copy);
            PasteCommand = new DelegateCommand(Paste);
            DeleteCommand = new DelegateCommand(Delete);
            CutCommand = new DelegateCommand(Cut);

            Name = _history.Current.DirectoryPathName;
            FilePath = _history.Current.DirectoryPath;

            OpenDirectory();
            _history.HistoryChanged += History_HistoryChanged;
        }

        #endregion

        #region Functions

        private void CallLeftPanel()
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

        private void History_HistoryChanged(object? sender, EventArgs e)
        {
            MoveBackCommand?.RaiseCanExecuteChanged();
            MoveForwardCommand?.RaiseCanExecuteChanged();
        }

        private bool OnCanMoveForward(object obj) => _history.CanMoveForward;
        private bool OnCanMoveBack(object obj) => _history.CanMoveBack;
        private void OnMoveForward(object obj)
        {
            _history.MoveForward();
            DirectoryNode current = _history.Current;
            FilePath = current.DirectoryPath;
            Name = current.DirectoryPathName;
            OpenDirectory();
        }

        private void OnMoveBack(object obj)
        {
            _history.MoveBack();
            DirectoryNode current = _history.Current;
            FilePath = current.DirectoryPath;
            Name = current.DirectoryPathName;
            OpenDirectory();
        }

        private void OpenDirectory()
        {
            DirectoriesAndFiles.Clear();
            //FilePath = SelectedFileEntity.FullName;

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


        private void Open(object parameter)
        {
            if (parameter is String)
                parameter = new DirectoryPC((String) parameter);

            if (parameter is DirectoryPC directoryPc)
            {
                FilePath = directoryPc.FullName;
                Name = directoryPc.Name;
                _history.Add(FilePath, Name);

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
            }
        }

        private void Click(object parameter)
        {
            if (parameter is FilePC filePc)
            {
                PathIcon = filePc.FullName;
            }
            else
            {
                PathIcon = "";
            }
        }

        private void Copy(object parameter)
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

        private void Paste(object? parameter)
        {
            if (parameter == null)
                parameter = new DirectoryPC(FilePath);

            if (parameter is DirectoryPC directoryPc)
            {
                FilePath = directoryPc.FullName; 
                SaveCopyFile.CopyTo(FilePath + "\\" + SaveCopyFile.Name);
                OpenDirectory();
            }
        }

        private void Delete(object? parameter)
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

        private void Cut(object parameter)
        {
            Copy(parameter);
            Delete(parameter);
           
        }
        #endregion
    }
}