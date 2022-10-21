using app.History;
using app.models;
using app.models.Entity;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

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

        #endregion
       
        #region Commands
        public ICommand OpenCommand { get; set; }

        public ICommand CloseCommand { get; set; }
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
            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);
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
            var current = _history.Current;
            FilePath = current.DirectoryPath;
            Name = current.DirectoryPathName;
            OpenDirectory();
        }

        private void OnMoveBack(object obj)
        {
            _history.MoveBack();
            var current = _history.Current;
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

            if (parameter is DirectoryPC directoryViewModel)
            {
                FilePath = directoryViewModel.FullName;
                Name = directoryViewModel.Name;
                _history.Add(FilePath, Name);

                OpenDirectory();
            }
        }

        #endregion
        
    }
}